using Microsoft.Practices.Unity;
using System.Windows;
using WindowmancerWPF.Models;
using WindowmancerWPF.Practices;
using WindowmancerWPF.Services;
using System.Windows.Input;
using Gat.Controls;
using MahApps.Metro.Controls;

namespace WindowmancerWPF.UI
{ 
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class EditorWindow : MetroWindow
  {
    // TODO: Temp
    private static IUnityContainer ServiceResolver { get; set; }
    private readonly ProfileManager _profileManager;
    private readonly WindowManager _windowManager;
    private readonly KeyHookManager _keyHookManager;
    private readonly ProcMonitor _procMonitor;
    
    public EditorWindow()
    {
      ServiceResolver = WMServiceResolver.Instance;

      // TODO: Temp
      _profileManager = ServiceResolver.Resolve<ProfileManager>();
      _windowManager = ServiceResolver.Resolve<WindowManager>();
      _keyHookManager = ServiceResolver.Resolve<KeyHookManager>();
      _procMonitor = ServiceResolver.Resolve<ProcMonitor>();

      InitializeComponent();
      Initialize();
    }

    private void Initialize()
    {
      this.ProfileListBox.ItemsSource = _profileManager.Profiles;
      this.ProfileListBox.SelectedItem = _profileManager.ActiveProfile;
      this.WindowConfigDataGrid.ItemsSource = _profileManager.ActiveProfile.Windows;
      this.ActiveWindowsGrid.ItemsSource = _procMonitor.ActiveWindowProcs;
      _procMonitor.Start();
    }

    #region Control Events

    #endregion Control Events

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
      if (this.WindowConfigDataGrid.SelectedItem == null) return;
      var item = (WindowInfo)WindowConfigDataGrid.SelectedItem;

      var dialog = new WindowConfigDialog(item) {Owner = this};
      dialog.ShowDialog();
    }

    private void ActiveWindowsGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (this.ActiveWindowsGrid.SelectedItem == null) return;
      var item = (MonitoredProcess)this.ActiveWindowsGrid.SelectedItem;

      var dialog = new WindowConfigDialog(item.GetProcess()) { Owner = this };
      dialog.ShowDialog();
    }
  }
}
