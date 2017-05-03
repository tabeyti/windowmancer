using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using NHotkey.Wpf;
using Windowmancer.Models;

namespace Windowmancer.Services
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
      SetNewHotKeyConfig(_userData.HotKeyConfig);
    }
    
    public void UpdateHotKeyConfig(HotKeyConfig config)
    {
      _userData.HotKeyConfig = config ?? throw new Exception($"{this} - Invalid hot-key config provided for update.");
      SetNewHotKeyConfig(config);
      _userData.Save();
    }

    private void SetNewHotKeyConfig(HotKeyConfig config)
    {
      this.HotKeyConfig = config;
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

    public HotKeyConfig()
    {
      this.ModifierKeys = new List<ModifierKeys>();
      this.PrimaryKey = Key.None;
    }

    public HotKeyConfig(IEnumerable<ModifierKeys> modifierKeys, Key primaryKey)
    {
      this.ModifierKeys = modifierKeys.ToList();
      this.PrimaryKey = primaryKey;
    }
  }
}
