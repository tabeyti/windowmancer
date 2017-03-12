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
    private IKeyboardEvents _globalHook;
    private KeyComboConfig _keyComboConfig;
    private readonly UserData _userData;

    public KeyHookManager(UserData userData)
    {
      _userData = userData;
      Initialize();
    }

    public void Initialize()
    {
      _keyComboConfig = _userData.GlobalHotKeyCombo;

      _globalHook = Hook.GlobalEvents();
      _globalHook.KeyDown += (s, e) =>
      {
        if (_keyComboConfig.Update(e.KeyCode, true))
        {
          OnKeyCombinationSuccess?.Invoke();
        }
      };
      _globalHook.KeyUp += (s, e) =>
      {
        _keyComboConfig.Update(e.KeyCode, false);
      };
    }

    public void LoadKeyHookConfig(KeyComboConfig config)
    {
      _keyComboConfig = config;
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
      return this.KeyCombination.TrueForAll(k => k.IsDown);
    }
  }
}
