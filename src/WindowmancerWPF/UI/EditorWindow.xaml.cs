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
      this.Icon = Helper.ImageSourceForIcon(Properties.Resources.AppIcon);

      // Apply data sources.
      this.ProfileListBox.ItemsSource = _profileManager.Profiles;
      this.ProfileListBox.SelectedItem = _profileManager.ActiveProfile;
      this.WindowConfigDataGrid.ItemsSource = _profileManager.ActiveProfile.Windows;
      this.ActiveWindowsDataGrid.ItemsSource = _procMonitor.ActiveWindowProcs;

      // Start process monitor.
      _procMonitor.Start();
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
      flyout.Content = w;
      flyout.IsOpen = !flyout.IsOpen;

      //var d = new CustomDialog();
      //var settings = new MetroDialogSettings
      //{
      //  AffirmativeButtonText = "Okay",
      //  AnimateShow = true,
      //  NegativeButtonText = "Cancel",
      //  FirstAuxiliaryButtonText = "Okay"
      //};
      //d.Content = new WindowConfig(item);

      //this.ShowMetroDialogAsync(d, settings);

      //var dialog = new WindowConfigDialog(item) { Owner = this };
      //dialog.ShowDialog();
    }

    private void HandleProcessWindowConfig()
    {
      if (this.ActiveWindowsDataGrid.SelectedItem == null) return;
      var item = (MonitoredProcess)this.ActiveWindowsDataGrid.SelectedItem;

      var flyout = this.Flyouts.Items[0] as Flyout;
      if (flyout == null)
      {
        return;
      }
      var w = new WindowConfig(item.GetProcess());
      flyout.Content = w;
      flyout.IsOpen = !flyout.IsOpen;

      //var d = new CustomDialog(this);
      //var settings = new MetroDialogSettings
      //{
      //  AffirmativeButtonText = "Okay",
      //  AnimateShow = true,
      //  NegativeButtonText = "Cancel",
      //  FirstAuxiliaryButtonText = "Okay",
      //  ColorScheme = MetroDialogColorScheme.Inverted,
      //  CustomResourceDictionary = WindowmancerWPF.App.Current.Resources

      //};
      //d.Content = new WindowConfig(item.GetProcess());
      //this.ShowMetroDialogAsync(d, settings);

      //var dialog = new WindowConfigDialog(item.GetProcess()) { Owner = this };
      //var result = dialog.ShowDialog();
      //if (result == null)
      //{
      //  return;
      //}
    }

    private void AboutBox_Click(object sender, RoutedEventArgs e)
    {
      var about = new About
      {
        ApplicationLogo = Helper.ImageSourceForBitmap(Properties.Resources.AppLogo)
      };
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
  }
}
