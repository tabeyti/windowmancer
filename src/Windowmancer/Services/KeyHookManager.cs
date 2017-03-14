using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windowmancer.Models;

namespace Windowmancer.Services
{
  public class KeyHookManager
  {
    public event Action OnKeyCombinationSuccess;

    public KeyComboConfig KeyComboConfig
    {
      get { return _userData.KeyComboConfig; }
      set { _userData.KeyComboConfig = value; }
    }

    private IKeyboardEvents _globalHook;
    private readonly UserData _userData;

    public KeyHookManager(UserData userData)
    {
      _userData = userData;
      Initialize();
    }

    public void Initialize()
    {
      _globalHook = Hook.GlobalEvents();
      _globalHook.KeyUp += (s, e) =>
      {
        var keyCode = GetActualKeyCode(e.KeyCode);
        _userData.KeyComboConfig.Update(keyCode, false);
      };
      _globalHook.KeyDown += (s, e) =>
      {
        var keyCode = GetActualKeyCode(e.KeyCode);
        if (_userData.KeyComboConfig.Update(keyCode, true))
        {
          OnKeyCombinationSuccess?.Invoke();
        }
      };
      _globalHook.KeyUp += (s, e) =>
      {
        _userData.KeyComboConfig.Update(e.KeyCode, false);
      };
    }

    /// <summary>
    /// We don't handle shift/ctrl/atl position keys (left and right).
    /// If we get one, assign it to it's generic key type.
    /// </summary>
    public Keys GetActualKeyCode(Keys keyCode)
    {
      switch (keyCode)
      {
        case Keys.LShiftKey:
        case Keys.RShiftKey:
          return Keys.Shift;
        case Keys.LControlKey:
        case Keys.RControlKey:
          return Keys.Control;
        case Keys.LMenu:
        case Keys.RMenu:
          return Keys.Alt;
        default:
          return keyCode;
      }
    }
  }

  public class KeyComboConfig
  {
    public List<KeyInfo> KeyCombination;

    public KeyComboConfig()
    {
    }

    public KeyComboConfig(IEnumerable<Keys> keys)
    {
      this.KeyCombination = new List<KeyInfo>();
      keys.ToList().ForEach(k => this.KeyCombination.Add(
        new KeyInfo { Key = k, IsDown = false }));
    }

    /// <summary>
    /// Updates the state of the key combination. If the combination is
    /// satisfied via all keys being pressed down, return true.
    /// </summary>
    public bool Update(Keys key, bool isDown)
    {
      var keyConfig = this.KeyCombination.Find(k => k.Key == key);
      if (null == keyConfig)
      {
        return false;
      }
      keyConfig.IsDown = isDown;
      if (this.KeyCombination.TrueForAll(k => k.IsDown))
      {
        this.KeyCombination.ForEach(k => k.IsDown = false);
        return true;
      }
      return false;
    }
  }
}
