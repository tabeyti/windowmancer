using System;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using System.Windows;
using WindowmancerWPF.Models;
using WindowmancerWPF.Practices;
using WindowmancerWPF.Services;
using System.Windows.Input;
using System.Windows.Media;
using Gat.Controls;
using MahApps.Metro.Controls;
using System.Windows.Controls;

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
      var flyout = this.Flyouts.Items[1] as Flyout;
      if (null == flyout)
      {
        throw new Exception("HandleProfileConfigEdit - No flyout available at index 1");
      }
      
      ProfileConfig content;
      if (null == profile)
      {
        flyout.Header = "Add Profile";
        content = new ProfileConfig((p) => 
        {
          _profileManager.AddNewProfile(p);
          this.ProfileListBox.SelectedItem = p;
        });
      }
      else
      {
        flyout.Header = "Edit Profile";
        content = new ProfileConfig(profile);
      }
      content.OnClose = () => { flyout.IsOpen = false; };

      flyout.Content = content;
      flyout.IsOpen = true;
    }
    
    private void HandleWindowConfigEdit(WindowInfo item)
    {
      var flyout = this.Flyouts.Items[0] as Flyout;
      if (flyout == null)
      {
        return;
      }
      var w = new WindowConfig(item, c => { item.Update(c); });
      w.OnClose += () => { flyout.IsOpen = false; };

      flyout.Content = w;
      flyout.IsOpen = true;
    }

    private void HandleWindowConfigEdit(Process item)
    {
      var flyout = this.Flyouts.Items[0] as Flyout;
      if (flyout == null)
      {
        return;
      }
      var w = new WindowConfig(item, c =>
      {
        _profileManager.AddToActiveProfile(c);
      });
      w.OnClose += () => { flyout.IsOpen = false; };

      flyout.Content = w;
      flyout.IsOpen = true;
    }

    private void AboutBox_Click(object sender, RoutedEventArgs e)
    {
      var about = new About
      {
        ApplicationLogo = Helper.ImageSourceForBitmap(Properties.Resources.AppLogo),
        Window = {Background = Brushes.Black},
      };
      about.Show();
    }

    private void WindowConfigDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (this.WindowConfigDataGrid.SelectedItem == null) return;
      var item = (WindowInfo)WindowConfigDataGrid.SelectedItem;
      HandleWindowConfigEdit(item);
    }

    private void ActiveWindowsGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (this.ActiveWindowsDataGrid.SelectedItem == null) return;
      var item = ((MonitoredProcess)this.ActiveWindowsDataGrid.SelectedItem).GetProcess();
      HandleWindowConfigEdit(item);
    }

    private void WindowConfigDataGrid_MenuItemClick(object sender, RoutedEventArgs e)
    {
      if (this.WindowConfigDataGrid.SelectedItem == null) return;
      var item = (WindowInfo)WindowConfigDataGrid.SelectedItem;
      HandleWindowConfigEdit(item);
    }

    private void ActiveWindowsDataGrid_MenuItemClick(object sender, RoutedEventArgs e)
    {
      if (this.ActiveWindowsDataGrid.SelectedItem == null) return;
      var item = ((MonitoredProcess)this.ActiveWindowsDataGrid.SelectedItem).GetProcess();
      HandleWindowConfigEdit(item);
    }
    
    private void ProfileListContextMenu_MenuItemClick(object sender, RoutedEventArgs e)
    {
      var menuItem = (MenuItem) sender;
      switch (menuItem.Header as string)
      {
        case "Add":
          HandleProfileConfigEdit();
          break;
        case "Edit":
          var item = (Profile)this.ProfileListBox.SelectedItem;
          HandleProfileConfigEdit(item);
          break;
        case "Delete":
          break;
      }
    }

    private void RefreshProfile(object sender, RoutedEventArgs e)
    {

    }

    private void ProfileListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var profile = (Profile)(this.ProfileListBox.SelectedItem);
      _profileManager.ActiveProfile = profile;
      this.WindowConfigDataGrid.ItemsSource = _profileManager.ActiveProfile.Windows;
    }
  }
}
