using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using NHotkey;
using NHotkey.Wpf;
using WindowmancerWPF.Models;

namespace WindowmancerWPF.Services
{
  public class KeyHookManager
  {
    public event Action OnKeyCombinationSuccess;

    public HotKeyConfig HotKeyConfig { get; private set; }

    private readonly UserData _userData;

    public KeyHookManager(UserData userData)
    {
      _userData = userData;
      Initialize();
    }

    public void Initialize()
    {
      this.HotKeyConfig = new HotKeyConfig(new[] { ModifierKeys.Control, ModifierKeys.Shift }.ToList(), Key.L);
      SetNewHotKeyConfig(HotKeyConfig);
    }
    
    public void UpdateKeyComboConfig(HotKeyConfig config)
    {
      //_userData.KeyComboConfig = config ?? throw new Exception($"{this} - Invalid config provided for update.");

      SetNewHotKeyConfig(config);

      //_userData.Save();
    }

    private void SetNewHotKeyConfig(HotKeyConfig config)
    {
      var modifierKeys = ModifierKeys.None;
      config.ModifierKeys.ForEach(m => modifierKeys |= m);

      HotkeyManager.Current.AddOrReplace("RescanProfile", config.PrimaryKey, modifierKeys, (s, e) =>
      {
        OnKeyCombinationSuccess?.Invoke();
      });
    }
  }

  public class HotKeyConfig
  {
    public List<ModifierKeys> ModifierKeys { get; set; }
    public Key PrimaryKey { get; set; }

    public HotKeyConfig(IEnumerable<ModifierKeys> modifierKeys, Key primaryKey)
    {
      this.ModifierKeys = modifierKeys.ToList();
      this.PrimaryKey = primaryKey;
    }
  }

  public class KeyComboConfig
  {
    public List<KeyInfo> KeyCombination { get; set; }

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
