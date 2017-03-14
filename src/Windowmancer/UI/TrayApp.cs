using System;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using Windowmancer.Configuration;
using Windowmancer.Properties;
using Windowmancer.Services;
using System.Collections.Generic;
using Windowmancer.Models;
using System.Reflection;

namespace Windowmancer.UI
{
  public class TrayApp : ApplicationContext, IDisposable
  {
    private readonly IUnityContainer _serviceResolver;
    private NotifyIcon _trayIcon;
    private KeyHookManager _keyHookManager;
    private readonly ProfileManager _profileManager;
    private readonly WindowManager _windowManager;
    private ContextMenu _trayContextMenu;
    private readonly UserData _userData;
    private Editor _editor;

    public TrayApp(IUnityContainer serviceResolver)
    {
      _serviceResolver = serviceResolver;
      _userData = serviceResolver.Resolve<UserData>();
      _profileManager = serviceResolver.Resolve<ProfileManager>();
      _windowManager = serviceResolver.Resolve<WindowManager>();      
      _keyHookManager = serviceResolver.Resolve<KeyHookManager>();
      Initialize();
    }

    public new void Dispose()
    {
      _windowManager?.Dispose();
      _profileManager?.Dispose();
    }

    private void Initialize()
    {
      _keyHookManager.OnKeyCombinationSuccess += OnKeyCombinationSuccess;
      _trayContextMenu = BuildContextMenu();
      _trayIcon = new NotifyIcon
      {
        Icon = Resources.app_icon,
        ContextMenu = _trayContextMenu,
        Visible = true
      };
      _trayIcon.DoubleClick += (s, e) =>
      {
        OpenEditor();
      };
    }

    private ContextMenu BuildContextMenu()
    {
      var menuItems = GetProfileMenuItems();
      menuItems.Add(new MenuItem("-"));
      menuItems.Add(new MenuItem("Open", (s, e) => OpenEditor()));
      menuItems.Add(new MenuItem("-"));
      menuItems.Add(new MenuItem("Settings", TrayContextMenu_OnProfileSettings));
      menuItems.Add(new MenuItem("-"));
      menuItems.Add(new MenuItem("Exit", (s, e) => ExitApplication()));
      return new ContextMenu(menuItems.ToArray());
    }

    private List<MenuItem> GetProfileMenuItems()
    {
      var list = new List<MenuItem>();
      foreach (var p in _profileManager.Profiles)
      {
        var m = new MenuItem(p.Name, TrayContextMenu_OnProfileSelect);
        if (p.Id == _profileManager.ActiveProfile.Id)
        {
          m.Checked = true;
        }
        m.Tag = p;
        list.Add(m);
      }
      return list;
    }

    private void OpenEditor()
    {
      _editor = new Editor(_profileManager, _windowManager, _keyHookManager);
      // Update our context menu profile selection on profile change
      // in the editor.
      _editor.ProfileListBox.SelectedIndexChanged += (s, ev) =>
      {
        var profile = (Profile)_editor.ProfileListBox.SelectedItem;
        UncheckCheckedMenuItem();
        foreach (MenuItem m in _trayContextMenu.MenuItems)
        {
          if (m.Tag == profile)
          {
            m.Checked = true;
            break;
          }
        }
      };
      _editor.ShowDialog();
      _editor = null;
    }

    private void OnKeyCombinationSuccess()
    {
      _windowManager.RefreshProfile();
    }

    private void ExitApplication()
    {
      _trayIcon.Visible = false;
      Application.Exit();
    }

    private void UncheckCheckedMenuItem()
    {
      foreach (MenuItem m in _trayContextMenu.MenuItems)
      {
        if (m.Checked)
        {
          m.Checked = false;
          break;
        }
      }
    }

    #region Events

    public void TrayContextMenu_OnProfileSelect(object sender, EventArgs e)
    {
      // Uncheck previous item.
      UncheckCheckedMenuItem();
      var mi = ((MenuItem)sender);
      mi.Checked = true;
      _profileManager.UpdateActiveProfile(((Profile)mi.Tag).Id);
      _editor?.UpdateActiveProfile(((Profile)mi.Tag));
    }

    public void TrayContextMenu_OnProfileSettings(object sender, EventArgs e)
    {
      var settings = new SettingsDialog(_keyHookManager);
      settings.Show();
    }

    #endregion Events
  }
}
