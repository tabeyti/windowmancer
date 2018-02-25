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
using System.Collections.Generic;
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

      BuildActiveWindowsContextMenu();

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
    }

#region Toast Methods

    public void ShowItemMessageToast(string itemName, string message = null)
    {
      if (itemName == null && message == null)
      {
        return;
      }

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
      if (itemName == null && message == null)
      {
        return;
      }

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
      if (message == null)
      {
        return;
      }

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
        // If it's not active, activate it with the config editor already open.
        if (!_hostContainerManager.IsHostContainerActive(config))
        {
          _hostContainerManager.ActivateHostContainer(config, true);
        }
        else
        {
          // If it's already active, bring the window to the fore,
          // and manually trigger the config editor to open.
          var hcw = _hostContainerManager.GetHostContainerWindow(config);
          hcw.Activate();
          hcw.HandleHostConfigEdit();
        }

        return;
      }

      // If this is a new container, use the manager to create a new
      // container, and add it to the active profile.
      var container = _hostContainerManager.CreateNewHostContainer();
      this.ProfileManager.AddToActiveProfile(container.HostContainerConfig);
    }

    private void HandleWindowConfigEdit()
    {
      var flyout = (Flyout)this.FindName("RightFlyout");
      if (flyout == null) return;

      var config = new WindowConfig();

      var wce = new WindowConfigEditor(config, w =>
      {
        //ProfileManager.AddToActiveProfile(w);
        //ShowItemMessageToast(w.Name, "window configuration added.");
      });
      wce.OnClose += () => { flyout.IsOpen = false; };

      flyout.Content = wce;
      flyout.IsOpen = true;
    }

    private void HandleWindowConfigEdit(WindowConfig item)
    {
      var flyout = (Flyout)this.FindName("RightFlyout");
      if (flyout == null || item == null) return;

      var wce = new WindowConfigEditor(item, (w) =>
        {
          //item.Update(w);
          //ShowItemMessageToast(w.Name, "window configuration updated");
        });
      wce.OnClose += () => { flyout.IsOpen = false; };

      flyout.Content = wce;
      flyout.IsOpen = true;
    }

    private void HandleWindowConfigEdit(Process item)
    {
      var flyout = (Flyout)this.FindName("RightFlyout");
      if (flyout == null)
      {
        return;
      }

      var config = WindowConfig.FromProcess(item);
      var wce = new WindowConfigEditor(config, c =>
      {
        ProfileManager.AddToActiveProfile(c);
        ShowItemMessageToast(c.Name, "added to window configuration list.");
      });
      wce.OnClose += () => { flyout.IsOpen = false; };

      flyout.Content = wce;
      flyout.IsOpen = true;
    }

    private void HandleSettingsDialog()
    {
      var flyout = (Flyout)this.FindName("LeftFlyout");
      if (flyout == null)
      {
        return;
      }
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
      var layoutEditor = new MonitorLayoutEditor(process)
      {
        OnSave = l => { MonitorWindowManager.ApplyLayout(l, process); },
        OnClose = () => { flyout.IsOpen = false; }
      };

      flyout.Content = layoutEditor;
      flyout.IsOpen = true;
    }

#endregion

    private void BuildActiveWindowsContextMenu()
    {
      // Create context menu items for Active Windows datagrid.
      var addItem = new MenuItem { Header = "Add Window Configuration" };
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
      
      // Set the source.
      this.ActiveWindowsContextMenuItems = new ObservableCollection<FrameworkElement>
      {
        addItem,
        containerizeItem,
        new Separator(),
        highlightItem,
      };
    }

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
      _monitorWindowManager.RefreshProfile();
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
      if (this.ActiveWindowsDataGrid.SelectedItem == null) return;

      var process = ((MonitoredProcess)this.ActiveWindowsDataGrid.SelectedItem).GetProcess();
      var hostContainerConfig = (HostContainerConfig)((MenuItem)e.Source).Tag; 

      // Add the process to the host container. The host container will
      // manage the window config.
      _hostContainerManager.ActivateHostContainer(hostContainerConfig, process);


      /////////////////////////////////////////////////////////////////////////



      //// TODO:
      //// Check to see if the host container targeted has enough room to hold the 
      //// process window.
      

      //// First check if there would already be a window config of the same name.
      //// If there would be a name clash, then append a unique index to the name.
      //var windowConfig = WindowConfig.FromProcess(process, false);
      //windowConfig.HostContainerLayoutInfo = new HostContainerLayoutInfo();
      //if (this.ProfileManager.IsInActiveProfile(windowConfig))
      //{
      //  windowConfig.Name = this.ProfileManager.DefaultWindowConfigName(windowConfig.Name);
      //}

      //// Dock the process window the host container.
      //_hostContainerManager.ActivateHostContainer(hostContainerConfig, process);

      //// With the unique name, skip over window config editor, 
      //// and add it to the active profile.
      //this.ProfileManager.AddToActiveProfile(windowConfig);

    }

    private void ActiveWindowsDataGrid_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
      BuildActiveWindowsContextMenu();
    }

    private void ActiveWindowsGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (this.ActiveWindowsDataGrid.SelectedItem == null) return;
      var item = ((MonitoredProcess)this.ActiveWindowsDataGrid.SelectedItem).GetProcess();
      HandleWindowConfigEdit(item);
    }

    private void EditorWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
      // TODO: Debug
      Process.Start("notepad.exe");
    }

    private void MonitorWindowConfigDataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    { 
      if (((PropertyDescriptor)e.PropertyDescriptor).IsBrowsable == false)
        e.Cancel = true;
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
          ShowItemMessageToast(this.ProfileManager.ActiveProfile.Name, "container deleted.");
          break;
      }
    }

    private void HostContainerWindowConfig_MenuItemClick(object sender, RoutedEventArgs e)
    {
      //WindowConfig item = null;
      //switch ((string)((MenuItem)sender).Header)
      //{
      //  case "Add":
      //    HandleWindowConfigEdit();
      //    break;
      //  case "Edit":
      //    item = (WindowConfig)HostContainerWindowConfigDataGrid.SelectedItem;
      //    HandleWindowConfigEdit(item);
      //    break;
      //  case "Delete":
      //    item = (WindowConfig)HostContainerWindowConfigDataGrid.SelectedItem;
      //    ProfileManager.RemoveFromActiveProfile(item);
      //    ShowItemMessageToast(item.Name, "window configuration deleted.");
      //    break;
      //}
    }

    private void MonitorWindowConfigList_OnFilter(object sender, FilterEventArgs e)
    {
      var item = (WindowConfig) e.Item;
      e.Accepted = item.MonitorLayoutInfo != null;
    }

    private void ContainerWindowConfigCollectionView_OnFilter(object sender, FilterEventArgs e)
    {
      var item = (WindowConfig)e.Item;
      e.Accepted = item.MonitorLayoutInfo == null;
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
  }
}
