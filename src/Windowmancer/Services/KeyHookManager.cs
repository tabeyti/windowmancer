using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windowmancer.Services
{
  public class KeyHookManager
  {
    private IKeyboardEvents _globalHook;
    private KeyComboConfig _keyComboConfig;
    private event Action OnKeyCombinationSuccess;

    public KeyHookManager(Action onKeyCombinationSuccess)
    {
      OnKeyCombinationSuccess = onKeyCombinationSuccess;
      Initialize();
    }

    public void Initialize()
    {
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
    private readonly List<KeyConfig> _keyCombination;

    public KeyComboConfig(IEnumerable<Keys> keys)
    {
      _keyCombination = new List<KeyConfig>();
      keys.ToList().ForEach(k => _keyCombination.Add(
        new KeyConfig { Key = k, IsDown = false }));
    }

    /// <summary>
    /// Updates the state of the key combination. If the combination is
    /// satisfied via all keys being pressed down, return true.
    /// </summary>
    public bool Update(Keys key, bool isDown)
    {
      var keyConfig = _keyCombination.Find(k => k.Key == key);
      if (null == keyConfig)
      {
        return false;
      }

      keyConfig.IsDown = isDown;
      return _keyCombination.TrueForAll(k => k.IsDown);
    }
  }

  public class KeyConfig
  {
    public Keys Key { get; set; }
    public bool IsDown { get; set; }
  }
}
