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
    private ContextMenuStrip _trayContextMenu;
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
        ContextMenuStrip = _trayContextMenu,
        Visible = true
      };
      _trayIcon.DoubleClick += (s, e) => OpenEditor();
      _trayIcon.MouseDown += (s, e) =>
      {
        _trayIcon.ContextMenuStrip = BuildContextMenu();
      };
    }

    private ContextMenuStrip BuildContextMenu()
    {
      var menuItems = GetProfileMenuItems();
      menuItems.Add(new ToolStripSeparator());
      menuItems.Add(new ToolStripMenuItem("Open", null, (s, e) => OpenEditor()));
      menuItems.Add(new ToolStripMenuItem("Settings", null, TrayContextMenu_OnProfileSettings));
      menuItems.Add(new ToolStripSeparator());
      menuItems.Add(new ToolStripMenuItem("Exit", null,  (s, e) => ExitApplication()));
      var cms = new ContextMenuStrip();
      cms.Items.AddRange(menuItems.ToArray());
      return cms;
    }

    private List<ToolStripItem> GetProfileMenuItems()
    {
      var list = new List<ToolStripItem>();
      var headerMenuItem = new ToolStripLabel("Profiles");
      headerMenuItem.Font = new System.Drawing.Font(headerMenuItem.Font, System.Drawing.FontStyle.Bold);
      list.Add(headerMenuItem);
      list.Add(new ToolStripSeparator());
      foreach (var p in _profileManager.Profiles)
      {
        var m = new ToolStripMenuItem(p.Name, null, TrayContextMenu_OnProfileSelect);
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
      if (_editor != null)
      {
        return;
      }

      _editor = new Editor(_profileManager, _windowManager, _keyHookManager);
      // Update our context menu profile selection on profile change
      // in the editor.
      _editor.ProfileListBox.SelectedIndexChanged += (s, ev) =>
      {
        var profile = (Profile)_editor.ProfileListBox.SelectedItem;
        UncheckCheckedMenuItem();
        foreach (ToolStripMenuItem m in _trayContextMenu.Items)
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
      foreach (MenuItem m in _trayContextMenu.Items)
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
