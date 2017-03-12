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
  public class Windowmancer : ApplicationContext
  {
    private readonly IUnityContainer _serviceResolver;
    private NotifyIcon _trayIcon;
    private KeyHookManager _keyHookManager;
    private readonly ProfileManager _profileManager;
    private readonly WindowManager _windowManager;
    private ContextMenu _trayContextMenu;
    private MainForm _editor;

    public Windowmancer(IUnityContainer serviceResolver)
    {
      _serviceResolver = serviceResolver;
      _windowManager = new WindowManager();
      _profileManager = new ProfileManager(_serviceResolver.Resolve<ProfileManagerConfig>(), _windowManager);
      Initialize();
    }

    public new void Dispose()
    {
      _windowManager?.Dispose();
      _profileManager?.Dispose();
    }

    private void Initialize()
    {
      _keyHookManager = new KeyHookManager(OnKeyCombinationSuccess);

      // TODO: DEBUG CODE
      _keyHookManager.LoadKeyHookConfig(new KeyComboConfig(new[] { Keys.LControlKey, Keys.LShiftKey, Keys.K }));

      var menuItems = GetProfileMenuItems();
      menuItems.Add(new MenuItem("-"));
      menuItems.Add(new MenuItem("Open", (s,e) => OpenEditor()));
      menuItems.Add(new MenuItem("-"));
      menuItems.Add(new MenuItem("Profile Settings", TrayContextMenu_OnProfileSettings));
      menuItems.Add(new MenuItem("-"));
      menuItems.Add(new MenuItem("Exit", (s,e) => ExitApplication()));
      _trayContextMenu = new ContextMenu(menuItems.ToArray());

      _trayIcon = new NotifyIcon
      {
        Icon = Resources.app_icon,
        ContextMenu = _trayContextMenu,
        Visible = true
      };
      _trayIcon.MouseUp += (s, e) =>
      {
        if (e.Button == MouseButtons.Left)
        {
          MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
          mi.Invoke(_trayIcon, null);
        }
      };

      // TODO: DEBUG CODE

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
      _editor = new MainForm(_serviceResolver, _profileManager, _windowManager);
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
      if (null != _editor)
      {
        _editor.UpdateActiveProfile(((Profile)mi.Tag));
      }
    }

    public void TrayContextMenu_OnProfileSettings(object sender, EventArgs e)
    {
      var settings = new SettingsDialog();
      settings.Show();
    }

    #endregion Events
  }
}
