using System.Diagnostics;
using Microsoft.Practices.Unity;
using System.Windows;
using WindowmancerWPF.Models;
using WindowmancerWPF.Practices;
using WindowmancerWPF.Services;
using System.Windows.Input;
using System.Windows.Media;
using Gat.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System;

namespace WindowmancerWPF.UI
{ 
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class EditorWindow
  {
    private readonly ProfileManager _profileManager;
    private readonly WindowManager _windowManager;
    private readonly KeyHookManager _keyHookManager;
    private readonly ProcMonitor _procMonitor;
    
    public EditorWindow()
    {
      _profileManager = App.ServiceResolver.Resolve<ProfileManager>();
      _windowManager = App.ServiceResolver.Resolve<WindowManager>();
      _keyHookManager = App.ServiceResolver.Resolve<KeyHookManager>();
      _procMonitor = App.ServiceResolver.Resolve<ProcMonitor>();

      InitializeComponent();
      Initialize();
    }

    private void Initialize()
    {
      // Apply data sources.
      this.ProfileListBox.ItemsSource = _profileManager.Profiles;
      this.ProfileListBox.SelectedItem = _profileManager.ActiveProfile;
      this.WindowConfigDataGrid.ItemsSource = _profileManager.ActiveProfile.Windows;
      this.ActiveWindowsDataGrid.ItemsSource = _procMonitor.ActiveWindowProcs;

      // Start process monitor.
      _procMonitor.Start();
    }

    private void HandleProfileConfigEdit(Profile profile = null)
    {
      ProfileConfig content = default(ProfileConfig);
      if (null != profile)
      {
        content = new ProfileConfig(profile);
      }
      else
      {
        content = new ProfileConfig();
      }

      var flyout = this.Flyouts.Items[1] as Flyout;
      if (flyout == null)
      {
        return;
      }

      content.OnClose += (s, e) =>
      {
        flyout.IsOpen = false;
        // TODO: Handle window config updates here
      };
      flyout.Content = content;
      flyout.CloseButtonVisibility = Visibility.Visible;
      flyout.IsOpen = true;
    }
    
    private void HandleWindowConfigEdit()
    {
      if (this.WindowConfigDataGrid.SelectedItem == null) return;
      var item = (WindowInfo)WindowConfigDataGrid.SelectedItem;

      var flyout = this.Flyouts.Items[0] as Flyout;
      if (flyout == null)
      {
        return;
      }
      var w = new WindowConfig(item);
      w.OnClose += (s, e) =>
      {
        flyout.IsOpen = false;
        // TODO: Handle window config updates here
      };

      flyout.Content = w;
      flyout.IsOpen = true;
    }

    private void HandleProcessWindowConfig()
    {
      if (this.ActiveWindowsDataGrid.SelectedItem == null) return;
      var item = ((MonitoredProcess)this.ActiveWindowsDataGrid.SelectedItem).GetProcess();

      var flyout = this.Flyouts.Items[0] as Flyout;
      if (flyout == null)
      {
        return;
      }
      var w = new WindowConfig(item);
      w.OnClose += (s, e) =>
      {
        flyout.IsOpen = false;
        // TODO: Handle window config updates here
      };

      flyout.Content = w;
      flyout.IsOpen = true;
    }

    private void AboutBox_Click(object sender, RoutedEventArgs e)
    {
      var about = new About
      {
        ApplicationLogo = Helper.ImageSourceForBitmap(Properties.Resources.AppLogo),
      };
      about.Window.Background = Brushes.Black;
      about.Show();
    }

    private void WindowConfigDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      HandleWindowConfigEdit();
    }

    private void ActiveWindowsGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      HandleProcessWindowConfig();
    }

    private void WindowConfigDataGrid_MenuItemClick(object sender, RoutedEventArgs e)
    {
      HandleWindowConfigEdit();
    }

    private void ActiveWindowsDataGrid_MenuItemClick(object sender, RoutedEventArgs e)
    {
      HandleProcessWindowConfig();
    }

    private void ShowModal(object sender, RoutedEventArgs e)
    {
      var flyout = this.Flyouts.Items[0] as Flyout;
      if (flyout == null)
      {
        return;
      }
      var w = new WindowConfig();
      flyout.Content = w;
      flyout.IsOpen = !flyout.IsOpen;
    }

    private void ProfileListContextMenu_MenuItemClick(object sender, RoutedEventArgs e)
    {
      HandleProfileConfigEdit();
    }
  }
}
