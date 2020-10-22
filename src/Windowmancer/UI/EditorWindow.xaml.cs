using System;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using MahApps.Metro.Controls.Dialogs;
using Windowmancer.Core.Configuration;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Services;
using Windowmancer.UI.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using MenuItem = System.Windows.Controls.MenuItem;
using Windowmancer.Core.Services.Base;
using Windowmancer.Services;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class EditorWindow : IToastHost
  {
    public ProfileManager ProfileManager { get; }
    public ProcessMonitor ProcMonitor { get; }

    private readonly MonitorWindowManager _monitorWindowManager;
    private readonly HostContainerManager _hostContainerManager;
    private readonly KeyHookManager _keyHookManager;

    public ObservableCollection<FrameworkElement> ActiveWindowsContextMenuItems { get; set; }

    public EditorViewModel EditorViewModel { get; set; }
    
    public EditorWindow()
    {
      this.ProfileManager = App.ServiceResolver.Resolve<ProfileManager>();
      this.ProcMonitor = App.ServiceResolver.Resolve<ProcessMonitor>();
      _monitorWindowManager = App.ServiceResolver.Resolve<MonitorWindowManager>();
      _keyHookManager = App.ServiceResolver.Resolve<KeyHookManager>();
      _hostContainerManager = App.ServiceResolver.Resolve<HostContainerManager>();

      this.EditorViewModel = new EditorViewModel();

      InitializeComponent();
      Initialize();
    }

    private void Initialize()
    {
      // Apply data sources.
      this.ProfileListBox.ItemsSource = ProfileManager.Profiles;
      this.ProfileListBox.SelectedItem = ProfileManager.ActiveProfile;

      // Start process monitor.
      this.ProcMonitor.Start();

      //this.HostContainerWindowConfigDataGrid.Columns
    }

    private void BuildActiveWindowsContextMenu()
    {
      if (this.ActiveWindowsContextMenuItems == null)
      {
        this.ActiveWindowsContextMenuItems = new ObservableCollection<FrameworkElement>();
      }
      this.ActiveWindowsContextMenuItems.Clear();

      // Create context menu items for Active Windows datagrid.
      var addItem = new MenuItem { Header = "Add to Monitor Layout" };
      addItem.Click += ActiveWindowsDataGrid_MenuItemClick;

      var highlightItem = new MenuItem { Header = "Highlight" };
      highlightItem.Click += ActiveWindowsDataGrid_HighlightClick;

      // Add a submenu for each container.
      var containerizeItem = new MenuItem { Header = "Add to Container" };
      containerizeItem.Click += ActiveWindowsDataGrid_ContainerizeClick;
      foreach (var c in this.ProfileManager.ActiveProfile.HostContainers)
      {
        containerizeItem.Items.Add(new MenuItem { Header = c.Name, Tag = c });
      }
      containerizeItem.Items.Add(new Separator());
      containerizeItem.Items.Add(new MenuItem { FontWeight = FontWeights.Bold,   Header = "New", Tag = null });

      var quickLayoutItem = new MenuItem {Header = "Quick Layout"};
      quickLayoutItem.Click += ActiveWindowsDataGrid_OnQuickLayouEdit;

      // If we are doing a batch, only provide the add to container menu
      if (this.ActiveWindowsDataGrid?.SelectedItems?.Count > 1)
      {
        this.ActiveWindowsContextMenuItems.Add(containerizeItem);
      }
      else
      {
        this.ActiveWindowsContextMenuItems.Add(addItem);
        this.ActiveWindowsContextMenuItems.Add(containerizeItem);
        this.ActiveWindowsContextMenuItems.Add(new Separator());
        this.ActiveWindowsContextMenuItems.Add(highlightItem);
        this.ActiveWindowsContextMenuItems.Add(new Separator());
        this.ActiveWindowsContextMenuItems.Add(quickLayoutItem);
      }      
    }

    #region Toast Methods

    public void ShowItemMessageToast(string itemName, string message = null)
    {
      if (itemName == null && message == null) return;

      var flyout = (Flyout)this.FindName("BottomFlyout");
      if (null == flyout)
      {
        throw new Exception("No flyout available for BottomFlyout.");
      }
      this.InfoItemLabel.Text = itemName;
      this.InfoMessageLabel.Text = message ?? this.InfoItemLabel.Text;
      flyout.IsOpen = true;
    }

    public void ShowErrorItemToast(string itemName, string message = null)
    {
      if (itemName == null && message == null) return;

      var flyout = (Flyout)this.FindName("ErrorFlyout");
      if (null == flyout)
      {
        throw new Exception("No flyout available for BottomFlyout.");
      }
      this.ErrorItemLabel.Text = itemName;
      this.ErrorMessageLabel.Text = message ?? this.InfoItemLabel.Text;
      flyout.IsOpen = true;
    }

    public void ShowMessageToast(string message)
    {
      if (message == null) return;

      var flyout = (Flyout)this.FindName("BottomFlyout");
      if (null == flyout)
      {
        throw new Exception("HandleProfileEdit - No flyout available at index 1");
      }
      this.InfoMessageLabel.Text = message;
      flyout.IsOpen = true;
    }

    #endregion

#region Handle Config Edit Methods

    private void HandleProfileEdit(Profile profile = null)
    {
      var flyout = (Flyout)this.FindName("LeftFlyout");
      if (null == flyout)
      {
        throw new Exception("Could not locate flyout LeftFlyout.");
      }
      
      ProfileEditor content;
      if (null == profile)
      {
        flyout.Header = "Add Profile";
        content = new ProfileEditor((p) => 
        {
          try
          {
            ProfileManager.AddNewProfile(p);
          }
          catch (Exception e)
          {
            MessageBox.Show(e.Message, "Oops");
            return;
          }
          this.ProfileListBox.SelectedItem = p;
          this.EditorViewModel.Update();
          ShowItemMessageToast(p.Name, "Profile added.");
        });
      }
      else
      {
        flyout.Header = "Edit Profile";
        content = new ProfileEditor(profile, (p) =>
        {
          profile.Update(p);
          ShowItemMessageToast(p.Name, "Profile updated.");
        });
      }
      content.OnClose = () => { flyout.IsOpen = false; };
      flyout.Content = content;
      flyout.IsOpen = true;
    }

    private void HandleHostContainerConfigEdit(HostContainerConfig config = null)
    {
      // If this is an edit, then check if the container is active.
      if (null != config)
      {
        // Activate container with editor opened.
        _hostContainerManager.ActivateHostContainer(config, true);
        return;
      }

      config = new HostContainerConfig(_hostContainerManager.GetDefaultHostContainerName());
      this.ProfileManager.AddToActiveProfile(config);
      _hostContainerManager.ActivateHostContainer(config, true);
    }

    private void HandleWindowConfigEdit()
    {
      var flyout = (Flyout)this.FindName("RightFlyout");
      if (flyout == null) return;

      var config = new WindowConfig();
      var wce = new WindowConfigEditor(config)
      {
        OnSave = w =>
        {
          ProfileManager.AddToActiveProfile(w);
          ShowItemMessageToast(w.Name, "window configuration added.");
        }
      };
      
      wce.OnClose += () => { flyout.IsOpen = false; };
      
      flyout.Content = wce;
      flyout.IsOpen = true;
    }

    private void HandleWindowConfigEdit(WindowConfig item)
    {
      var flyout = (Flyout)this.FindName("RightFlyout");
      if (flyout == null || item == null) return;

      flyout.TitleVisibility = Visibility.Collapsed;
      var wce = new WindowConfigEditor(item)
      {
        OnSave = (w) =>
        {
          item.Update(w);
          ShowItemMessageToast(w.Name, "window configuration updated");
        }
      };
      wce.OnClose += () => { flyout.IsOpen = false; };

      flyout.Content = wce;
      flyout.IsOpen = true;
    }

    private void HandleWindowConfigEdit(Process item)
    {
      var flyout = (Flyout)this.FindName("RightFlyout");
      if (flyout == null) return;

      var config = WindowConfig.FromProcess(item, Core.Models.WindowConfigLayoutType.Monitor);
      var wce = new WindowConfigEditor(config)
      {
        OnSave = c =>
        {
          ProfileManager.AddToActiveProfile(c);
          ShowItemMessageToast(c.Name, "added to window configuration list.");
        }
      };
      ;
      wce.OnClose += () => { flyout.IsOpen = false; };

      flyout.Content = wce;
      flyout.IsOpen = true;
    }

    private void HandleSettingsDialog()
    {
      var flyout = (Flyout)this.FindName("LeftFlyout");
      if (flyout == null) return;
      
      flyout.Header = "Preferences";
      var w = new PreferencesDialog(new PreferencesModel(_keyHookManager.HotKeyConfig), p =>
      {
        var prefs = (PreferencesModel) p;
        _keyHookManager.UpdateHotKeyConfig(prefs.HotKeyConfig);
        ShowItemMessageToast("Preferences", "updated.");
      });
      w.OnClose += () => { flyout.IsOpen = false; };
      flyout.Content = w;
      flyout.IsOpen = true;
    }

    private void HandleQuickLayoutEdit(Process process)
    {
      var flyout = (Flyout)this.FindName("RightFlyout");
      if (flyout == null) return;

      flyout.Header = "Monitor Layout Editor";
      var layoutEditor = new MonitorLayoutEditor(process, true)
      {
        OnSave = l => { MonitorWindowManager.ApplyLayout(l, process); },
        OnClose = () => { flyout.IsOpen = false; }
      };

      flyout.Content = layoutEditor;
      flyout.IsOpen = true;
    }

#endregion

    private void MonitorWindowConfigDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (this.MonitorWindowConfigDataGrid.SelectedItem == null) return;
      var item = (WindowConfig)MonitorWindowConfigDataGrid.SelectedItem;
      HandleWindowConfigEdit(item);
    }

    private void MonitorWindowConfigDataGrid_MenuItemClick(object sender, RoutedEventArgs e)
    {
      WindowConfig item = null;
      switch ((string) ((MenuItem) sender).Header)
      {
        case "Add":
          HandleWindowConfigEdit();
          break;
        case "Edit":
          item = (WindowConfig)MonitorWindowConfigDataGrid.SelectedItem;
          HandleWindowConfigEdit(item);
          break;
        case "Delete":
          item = (WindowConfig)MonitorWindowConfigDataGrid.SelectedItem;
          ProfileManager.RemoveFromActiveProfile(item);
          ShowItemMessageToast(item.Name, "window configuration deleted.");
          break;
      }
    }

    private void WindowConfigDataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
      if (((PropertyDescriptor)e.PropertyDescriptor).IsBrowsable == false)
        e.Cancel = true;
    }

    private void ProfileListContextMenu_MenuItemClick(object sender, RoutedEventArgs e)
    {
      var menuItem = (MenuItem) sender;
      switch (menuItem.Header as string)
      {
        case "Add":
          HandleProfileEdit();
          break;
        case "Edit":
          var item = (Profile)this.ProfileListBox.SelectedItem;
          HandleProfileEdit(item);
          break;
        case "Delete":
          ShowItemMessageToast(this.ProfileManager.ActiveProfile.Name, "profile deleted.");
          this.ProfileManager.DeleteActiveProfile();
          break;
      }
    }
    
    private void ProfileListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // TODO: Hack to update the window title on profile selection. Need to data bind dis
      this.EditorViewModel.Update();
    }

    private void ProfileListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var item = (Profile)this.ProfileListBox.SelectedItem;
      HandleProfileEdit(item);
    }

    private void RunProfileButton_OnClick(object sender, RoutedEventArgs e)
    {
      _monitorWindowManager.RunProfile();
      _hostContainerManager.RunProfile();
    }

    private void Preferences_Click(object sender, RoutedEventArgs e)
    {
      HandleSettingsDialog();
    }

    private void Documentation_Click(object sender, RoutedEventArgs e)
    {
      var userConfig = Helper.ServiceResolver.Resolve<UserConfig>();

      var startInfo = new ProcessStartInfo
      {
        FileName = userConfig.DocumentationLink
      };

      Process.Start(startInfo);
    }

    private void ActiveWindowsDataGrid_HighlightClick(object sender, RoutedEventArgs e)
    {
      if (this.ActiveWindowsDataGrid.SelectedItem == null) return;
      var process = ((MonitoredProcess)this.ActiveWindowsDataGrid.SelectedItem).GetProcess();

      // Restore window (if minimized) and bring to fore-ground
      MonitorWindowManager.ShowWindowNormal(process);
      var procRec = MonitorWindowManager.GetWindowRectCurrent(process);

      // Highlight the window temporarily.
      var windowHighlight = new WindowHighlight();
      Helper.Dispatcher.Invoke(() =>
      {
        windowHighlight.UpdateLayout(procRec.Left, procRec.Top, procRec.Width, procRec.Height, 4);
        windowHighlight.Show();
      });
    }

    private void ActiveWindowsDataGrid_MenuItemClick(object sender, RoutedEventArgs e)
    {
      if (this.ActiveWindowsDataGrid.SelectedItem == null) return;
      var item = ((MonitoredProcess)this.ActiveWindowsDataGrid.SelectedItem).GetProcess();
      HandleWindowConfigEdit(item);
    }

    private void ActiveWindowsDataGrid_ContainerizeClick(object sender, RoutedEventArgs e)
    {
      if (this.ActiveWindowsDataGrid.SelectedItem == null) { return; }

      var hostContainerConfig = (HostContainerConfig)((MenuItem)e.Source).Tag;

      // If this is a new container, make a config for it and adjust row/col to accomodate
      // all process windows
      if (hostContainerConfig == null)
      {
        hostContainerConfig = new HostContainerConfig(_hostContainerManager.GetDefaultHostContainerName());
        int rowsCols = 1;
        for (rowsCols = 1; rowsCols * rowsCols < this.ActiveWindowsDataGrid.SelectedItems.Count; rowsCols++) {}
        hostContainerConfig.Rows = rowsCols;
        hostContainerConfig.Columns = rowsCols;
        this.ProfileManager.AddToActiveProfile(hostContainerConfig);
      }

      foreach (var item in this.ActiveWindowsDataGrid.SelectedItems)
      {
        var process = ((MonitoredProcess)item).GetProcess();        

        // TODO:
        // First check if there would already be a window config of the same name.
        // If there would be a name clash, then append a unique index to the name.
        //var windowConfig = WindowConfig.FromProcess(process, Core.Models.WindowConfigLayoutType.HostContainer);
        //windowConfig.HostContainerLayoutInfo = new HostContainerLayoutInfo(hostContainerConfig.Name);
        //if (this.ProfileManager.IsInActiveProfile(windowConfig))
        //{
        //  windowConfig.Name = this.ProfileManager.DefaultWindowConfigName(windowConfig.Name);
        //}

        // Dock the process window to the host container.
        var windowConfig = _hostContainerManager.ActivateHostContainer(hostContainerConfig, process);
        if (windowConfig == null) { return; }

        // With the unique name, skip over window config editor,
        // and add it to the active profile.
        this.ProfileManager.AddToActiveProfile(windowConfig);
      }      
    }

    private void ActiveWindowsDataGrid_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
      BuildActiveWindowsContextMenu();
    }

    private void ActiveWindowsDataGrid_OnQuickLayouEdit(object sender, RoutedEventArgs e)
    {
      if (this.ActiveWindowsDataGrid.SelectedItem == null) return;
      var item = ((MonitoredProcess)this.ActiveWindowsDataGrid.SelectedItem).GetProcess();
      HandleQuickLayoutEdit(item);
    }

    private void ActiveWindowsGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (this.ActiveWindowsDataGrid.SelectedItem == null) return;
      var item = ((MonitoredProcess)this.ActiveWindowsDataGrid.SelectedItem).GetProcess();
      HandleWindowConfigEdit(item);
    }

    private void EditorWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
    }

    private void HostContainerWindowConfig_MenuItemClick(object sender, RoutedEventArgs e)
    {
      var item = (WindowConfig)HostContainerWindowConfigDataGrid.SelectedItem;
      switch ((string)((MenuItem)sender).Header)
      {
        case "Edit":          
          HandleWindowConfigEdit(item);
          break;
        case "Delete":
          //var item = (WindowConfig)HostContainerWindowConfigDataGrid.SelectedItem;
          _hostContainerManager.RemoveFromHostContainerWindow(item);
          ProfileManager.RemoveFromActiveProfile(item);
          ShowItemMessageToast(item.Name, "window configuration deleted.");
          break;
      }
    }

    private void MonitorWindowConfigList_OnFilter(object sender, FilterEventArgs e)
    {
      var item = (WindowConfig) e.Item;
      e.Accepted = item.MonitorLayoutInfo != null;
    }

    private void ContainerWindowConfigCollectionView_OnFilter(object sender, FilterEventArgs e)
    {
      var item = (WindowConfig)e.Item;

      e.Accepted = false;

      if (item.MonitorLayoutInfo != null) { return; }
      if (item.HostContainerLayoutInfo == null) { return; }
      var hostContainerConfig = this.HostContainersListBox.SelectedItem as HostContainerConfig;

      e.Accepted = item.HostContainerLayoutInfo.HostContainerId == hostContainerConfig?.Name;
    }

    private void AboutBox_Click(object sender, RoutedEventArgs e)
    {
      var dialog = new MyCustomDialog();
      var about = new AboutDialog(() =>
      {
        this.HideMetroDialogAsync(dialog);
      });

      var settings = new MetroDialogSettings
      {
        AffirmativeButtonText = "Okay",
        AnimateShow = true,
        FirstAuxiliaryButtonText = "Okay",
        ColorScheme = MetroDialogColorScheme.Theme
      };
      dialog.Content = about;
      this.ShowMetroDialogAsync(dialog, settings);
    }

    private void HostContainersListBox_ItemDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var item = (HostContainerConfig)this.HostContainersListBox.SelectedItem;
      _hostContainerManager.ActivateHostContainer(item, false);
    }

    private void HostContainersListBox_MenuItemClick(object sender, RoutedEventArgs e)
    {
      var menuItem = (MenuItem)sender;
      switch (menuItem.Header as string)
      {
        case "Add":
          HandleHostContainerConfigEdit();
          break;
        case "Edit":
          var item = (HostContainerConfig)this.HostContainersListBox.SelectedItem;
          HandleHostContainerConfigEdit(item);
          break;
        case "Delete":
          item = (HostContainerConfig)HostContainersListBox.SelectedItem;
          ProfileManager.RemoveFromActiveProfile(item);
          ShowItemMessageToast(item.Name, "container deleted.");
          break;
      }
    }

    private void HostContainersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (null == this.HostContainerWindowConfigDataGrid) { return; }
      CollectionViewSource.GetDefaultView(this.HostContainerWindowConfigDataGrid.ItemsSource).Refresh();
    }

    private void HostContainerWindowConfigDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (this.HostContainerWindowConfigDataGrid.SelectedItem == null) return;
      var item = (WindowConfig)this.HostContainerWindowConfigDataGrid.SelectedItem;
      HandleWindowConfigEdit(item);
    }
  }
}
