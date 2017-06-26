using System;
using System.Windows.Input;
using NHotkey.Wpf;
using Windowmancer.Core.Models;

namespace Windowmancer.Core.Services
{
  public class KeyHookManager
  {
    public event Action OnKeyCombinationSuccess;

    public HotKeyConfigModel HotKeyConfig { get; private set; }

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

    public void UpdateHotKeyConfig(HotKeyConfigModel config)
    {
      _userData.HotKeyConfig = config ?? throw new Exception($"{this} - Invalid hot-key config provided for update.");
      SetNewHotKeyConfig(config);
      _userData.Save();
    }

    private void SetNewHotKeyConfig(HotKeyConfigModel config)
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
}
