/**
 * TODO: Process check (dissallow multiple instances of WM to run)
 * TODO: 
 * 
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Services;
using Windowmancer.Services;
using Windowmancer.UI;
using Application = System.Windows.Application;
using Control = System.Windows.Controls.Control;
using Point = System.Drawing.Point;

namespace Windowmancer
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App
  {
    public static IUnityContainer ServiceResolver = Practices.WmServiceResolver.Instance;

    // Static colors which should match theme colors in App.xaml
    // TODO: Grab colors from App.xaml instead of hard-code them
    public static Color TrayAppBgColor = Color.FromArgb(255, 30, 30, 30);
    public static Color TrayAppForegroundColor = Color.White;
    public static Color SelectedItemColor = Color.DeepSkyBlue;

    public ObservableCollection<Control> ContextMenuItemCollection { get; set; }

    private KeyHookManager _keyHookManager;
    private ProfileManager _profileManager;
    private MonitorWindowManager _monitorWindowManager;
    private HostContainerManager _hostContainerManager;
    private ProcessMonitor _procMonitor;

    private NotifyIcon _trayIcon;
    private ContextMenuStrip _trayContextMenu;
    private EditorWindow _editor;
    
    private void App_OnExit(object sender, ExitEventArgs e)
    {
      ServiceResolver.Dispose();
    }

    private void App_OnStartup(object sender, StartupEventArgs e)
    {
      Start();
    }

    public void Start()
    {
      // Set our application's dispatcher within Helper.
      Helper.Dispatcher = new WmDispatcher(this.Dispatcher);

      _profileManager = ServiceResolver.Resolve<ProfileManager>();
      _monitorWindowManager = ServiceResolver.Resolve<MonitorWindowManager>();
      _hostContainerManager = ServiceResolver.Resolve<HostContainerManager>();
      _keyHookManager = ServiceResolver.Resolve<KeyHookManager>();
      _procMonitor = ServiceResolver.Resolve<ProcessMonitor>();

      Initialize();

      // Open editor on first run.
      OpenEditor();
    }

    private void Initialize()
    {
      _keyHookManager.OnKeyCombinationSuccess += OnKeyCombinationSuccess;
      _procMonitor.Start();

      var iconStream = Application.GetResourceStream(
        new Uri("pack://application:,,,/Windowmancer;component/AppIcon.ico")).Stream;

      // Build system tray app.
      _trayContextMenu = BuildContextMenu();
      _trayIcon = new NotifyIcon
      {
        Icon = new System.Drawing.Icon(iconStream),
        ContextMenuStrip = _trayContextMenu,
        Visible = true
      };
      _trayIcon.DoubleClick += (s, e) => OpenEditor();
      _trayIcon.MouseDown += (s, e) => { _trayIcon.ContextMenuStrip = BuildContextMenu(); };

      // Hook into active profile updates to display proper
      // tooltip text for the tray icon.
      _trayIcon.Text = TrayIconTooltipText();
      _profileManager.OnActiveProfileUpdated += (s, e) => { _trayIcon.Text = TrayIconTooltipText(); };
    }

    private string TrayIconTooltipText()
    {
      return $"Windowmancer\nActive Container - {_profileManager.ActiveProfile.Name}";
    }
    
    private ContextMenuStrip BuildContextMenu()
    {
      var iconStream = Application.GetResourceStream(
        new Uri("pack://application:,,,/Windowmancer;component/AppIcon.ico")).Stream;
      var wmIcon = new System.Drawing.Bitmap(iconStream);

      var menuItems = new List<ToolStripItem>();

      // Create fresh button as first item.
      var rescanMenuItem = new ToolStripButton("Rescan Profile");
      rescanMenuItem.MouseEnter += TrayContextMenuItem_MouseEnter;
      rescanMenuItem.MouseLeave += TrayContextMenuItem_MouseLeave;
      rescanMenuItem.Dock = DockStyle.Fill;
      rescanMenuItem.Font = new Font(rescanMenuItem.Font, System.Drawing.FontStyle.Bold);
      rescanMenuItem.Click += (s, e) => 
      { 
        _monitorWindowManager.RunProfile();
        _hostContainerManager.RunProfile();
      };
      rescanMenuItem.Image = wmIcon;
      menuItems.Add(rescanMenuItem);
      menuItems.Add(new ToolStripSeparator());

      // Grab all selectable profiles.
      menuItems.AddRange(GetProfileMenuItems());
      menuItems.Add(new ToolStripSeparator());

      // Application control menu items.
      var openMenuItem = CreateMenuItem("Open", (s, e) => OpenEditor());
      openMenuItem.TextAlign = ContentAlignment.MiddleLeft;
      openMenuItem.Font = new Font(openMenuItem.Font, System.Drawing.FontStyle.Bold);
      menuItems.Add(openMenuItem);
      menuItems.Add(new ToolStripSeparator());
      menuItems.Add(CreateMenuItem("Exit", (s, e) => ExitApplication()));
      
      var cms = new ContextMenuStrip
      {
        BackColor = TrayAppBgColor,
        ForeColor = TrayAppForegroundColor,
        ShowCheckMargin = false,
        ShowImageMargin = false,
        Margin = new Padding(0),
        
      };
      cms.Items.AddRange(menuItems.ToArray());
      return cms;
    }

    /// <summary>
    /// Our method for creating menu items which automatically adds styling for
    /// mouse enter/leave events.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="onClick"></param>
    /// <returns></returns>
    private ToolStripMenuItem CreateMenuItem(string name, EventHandler onClick)
    {
      var menuItem = new ToolStripMenuItem(name, null, onClick);
      menuItem.MouseEnter += TrayContextMenuItem_MouseEnter;
      menuItem.MouseLeave += TrayContextMenuItem_MouseLeave;
      return menuItem;
    }
    
    private IEnumerable<ToolStripItem> GetProfileMenuItems()
    {
      var list = new List<ToolStripItem>();
      foreach (var p in _profileManager.Profiles)
      {
        var m = CreateMenuItem(p.Name, TrayContextMenu_OnProfileSelect);
        m.Tag = p;
        if (p.Id == _profileManager.ActiveProfile.Id)
        {
          SelectMenuItem(m);
        }
        m.Tag = p;
        list.Add(m);
      }
      return list;
    }

    private void OpenEditor()
    {
      if (_editor != null) return;

      _editor = new EditorWindow();
      _editor.ShowDialog();
      _editor = null;
    }

    private void OnKeyCombinationSuccess()
    {
      _monitorWindowManager.RunProfile();
    }

    private void UncheckCheckedMenuItem()
    {
      foreach (ToolStripItem m in _trayContextMenu.Items)
      {
        if (m.GetType() != typeof(ToolStripMenuItem)) continue;

        var item = m as ToolStripMenuItem;
        if (!item.Checked) continue;
        DeselectMenuItem(item);
        break;
      }
    }

    #region Events

    private void ExitApplication()
    {
      _trayIcon.Visible = false;
      Application.Current.Shutdown();
    }

    private void TrayContextMenu_OnProfileSelect(object sender, EventArgs e)
    {
      // Uncheck previous item.
      var mi = ((ToolStripMenuItem)sender);
      UncheckCheckedMenuItem();
      SelectMenuItem(mi);
      _profileManager.UpdateActiveProfile(((Profile)mi.Tag));
    }

    private void TrayContextMenuItem_MouseEnter(object sender, EventArgs e)
    {
      var item = sender as ToolStripItem;
      item.ForeColor = Color.FromArgb(item.ForeColor.ToArgb() ^ 0xffffff);
    }

    private void TrayContextMenuItem_MouseLeave(object sender, EventArgs e)
    {
      var item = sender as ToolStripItem;
      item.ForeColor = Color.FromArgb(item.ForeColor.ToArgb() ^ 0xffffff);
    }

    private void SelectMenuItem(ToolStripMenuItem mi)
    {
      mi.Checked = true;
      mi.ForeColor = SelectedItemColor;
    }

    private void DeselectMenuItem(ToolStripMenuItem mi)
    {
      mi.Checked = false;
      mi.ForeColor = Color.White;
    }

    #endregion Events

    public class MenuItemRenderer : ToolStripProfessionalRenderer
    {
      protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
      {
        var borderRectangle = new Rectangle(Point.Empty, e.Item.Size);
        var size = e.Item.Size;
        size.Width--;
        size.Height--;
        var bgRectangle = new Rectangle(Point.Empty, size);

        using (var borderBrush = new SolidBrush(App.TrayAppForegroundColor))
        using (var bgBrush = new SolidBrush(App.TrayAppBgColor))
        {
          e.Graphics.FillRectangle(borderBrush, borderRectangle);
          e.Graphics.FillRectangle(bgBrush, bgRectangle);
        }
      }
    }

    private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
      System.Windows.MessageBox.Show(e.Exception.Message, "Error");
    }
  }
}

