﻿using System;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using Windowmancer.Configuration;
using Windowmancer.Properties;
using Windowmancer.Services;

namespace Windowmancer.UI
{
  public class Windowmancer : ApplicationContext
  {
    private readonly IUnityContainer _serviceResolver;
    private NotifyIcon _trayIcon;
    private KeyHookManager _keyHookManager;
    private readonly ProfileManager _profileManager;
    private readonly WindowManager _windowManager;

    public Windowmancer(IUnityContainer serviceResolver)
    {
      _serviceResolver = serviceResolver;
      _profileManager = new ProfileManager(_serviceResolver.Resolve<ProfileManagerConfig>());
      _windowManager = new WindowManager();
      Initialize();
    }

    public new void Dispose()
    {
      _windowManager?.Dispose();
      _profileManager?.Dispose();
    }

    private void Initialize()
    {
      _windowManager.LoadProfile(_profileManager.ActiveProfile);
      _keyHookManager = new KeyHookManager(OnKeyCombinationSuccess);

      // TODO: DEBUG CODE
      _keyHookManager.LoadKeyHookConfig(new KeyComboConfig(new[] { Keys.LControlKey, Keys.LShiftKey, Keys.K }));

      _trayIcon = new NotifyIcon
      {
        Icon = Resources.app_icon,
        ContextMenu = new ContextMenu(new[]
        {
          new MenuItem("Open", Open),
          new MenuItem("Exit", Exit)
        }),
        Visible = true
      };
    }

    private void OnKeyCombinationSuccess()
    {
      _windowManager.RefreshProfile();
    }

    private void Exit(object sender, EventArgs e)
    {
      _trayIcon.Visible = false;
      Application.Exit();
    }

    private void Open(object sender, EventArgs e)
    {
      var editor = new MainForm(_serviceResolver, _profileManager, _windowManager);
      editor.Show();
    }
  }
}
