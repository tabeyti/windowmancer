using System;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using Windowmancer.Configuration;
using Windowmancer.Properties;
using Windowmancer.Services;
using System.Collections.Generic;
using System.Drawing;
using Windowmancer.Models;
using System.Reflection;

namespace Windowmancer.UI
{
  public class TrayApp : ApplicationContext, IDisposable
  {
    private readonly IUnityContainer _serviceResolver;
    private readonly KeyHookManager _keyHookManager;
    private readonly ProfileManager _profileManager;
    private readonly WindowManager _windowManager;
    private readonly ProcMonitor _procMonitor;
    private readonly UserData _userData;
    private NotifyIcon _trayIcon;
    private ContextMenuStrip _trayContextMenu;
    private EditorForm _editor;

    public TrayApp(IUnityContainer serviceResolver)
    {
      _serviceResolver = serviceResolver;
      _userData = serviceResolver.Resolve<UserData>();
      _profileManager = serviceResolver.Resolve<ProfileManager>();
      _windowManager = serviceResolver.Resolve<WindowManager>();      
      _keyHookManager = serviceResolver.Resolve<KeyHookManager>();
      _procMonitor = serviceResolver.Resolve<ProcMonitor>();
      Initialize();
    }

    public new void Dispose()
    {
      _serviceResolver.Dispose();
    }

    private void Initialize()
    {
      _keyHookManager.OnKeyCombinationSuccess += OnKeyCombinationSuccess;
      _procMonitor.Start();

      _trayContextMenu = BuildContextMenu();
      _trayIcon = new NotifyIcon
      {
        Icon = Resources.AppIcon,
        ContextMenuStrip = _trayContextMenu,
        Visible = true
      };
      _trayIcon.DoubleClick += (s, e) => OpenEditor();
      _trayIcon.MouseDown += (s, e) =>
      {
        _trayIcon.ContextMenuStrip = BuildContextMenu();
      };

      // Hook into active profile updates to display proper
      // tooltip text for the tray icon.
      _trayIcon.Text = TrayIconTooltipText();
      _profileManager.OnActiveProfileUpdate += (s, e) =>
      {
        _trayIcon.Text = TrayIconTooltipText();
      };
    }

    private string TrayIconTooltipText()
    {
      return $"Windowmancer\nActive Profile - {_profileManager.ActiveProfile.Name}";
    }

    private ContextMenuStrip BuildContextMenu()
    {
      var menuItems = GetProfileMenuItems();
      menuItems.Add(new ToolStripSeparator());
      var openMenuItem = new ToolStripMenuItem("Open", null, (s, e) => OpenEditor());
      openMenuItem.Font = new Font(openMenuItem.Font, FontStyle.Bold);
      menuItems.Add(openMenuItem);
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
      foreach (var p in _profileManager.Profiles)
      {
        var m = new ToolStripMenuItem(p.Name, null, TrayContextMenu_OnProfileSelect) { Tag = p };
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

      _editor = new EditorForm(_profileManager, _windowManager, _keyHookManager, _procMonitor);
      _editor.ShowDialog();
      _editor = null;
    }

    private void OnKeyCombinationSuccess()
    {
      _windowManager.RefreshProfile();
    }

    private void UncheckCheckedMenuItem()
    {
      foreach (ToolStripItem m in _trayContextMenu.Items)
      {
        if (m.GetType() != typeof(ToolStripMenuItem))
        {
          continue;
        }

        var item = m as ToolStripMenuItem;
        if (item.Checked)
        {
          item.Checked = false;
          break;
        }
      }
    }

    #region Events

    private void ExitApplication()
    {
      _trayIcon.Visible = false;
      this.Dispose();
      Application.Exit();
    }

    private void TrayContextMenu_OnProfileSelect(object sender, EventArgs e)
    {
      // Uncheck previous item.
      var mi = ((ToolStripMenuItem)sender);
      UncheckCheckedMenuItem();
      mi.Checked = true;
      _profileManager.UpdateActiveProfile(((Profile)mi.Tag).Id);
      _editor?.UpdateActiveProfile(((Profile)mi.Tag));
      //_trayIcon.Text = TrayIconTooltipText();
    }

    private void TrayContextMenu_OnProfileSettings(object sender, EventArgs e)
    {
      var settings = new SettingsDialog(_keyHookManager);
      settings.Show();
    }

    #endregion Events
  }
}
