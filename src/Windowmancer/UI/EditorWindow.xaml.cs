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
using System.Windows.Media;
using MenuItem = System.Windows.Controls.MenuItem;
using Windowmancer.Core.Services.Base;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class EditorWindow : IToastHost
  {
    public ProfileManager ProfileManager { get; }
    public ProcessMonitor ProcMonitor { get; }

    private readonly Dictionary<string, WindowHostContainer> _windowHostContainers = 
      new Dictionary<string, WindowHostContainer>();

    private readonly MonitorWindowManager _monitorWindowManager;
    private readonly KeyHookManager _keyHookManager;

    public ObservableCollection<FrameworkElement> ActiveWindowsContextMenuItems { get; set; }

    public EditorViewModel EditorViewModel { get; set; }
    
    public EditorWindow()
    {
      this.ProfileManager = App.ServiceResolver.Resolve<ProfileManager>();
      this.ProcMonitor = App.ServiceResolver.Resolve<ProcessMonitor>();
      _monitorWindowManager = App.ServiceResolver.Resolve<MonitorWindowManager>();
      _keyHookManager = App.ServiceResolver.Resolve<KeyHookManager>();

      this.EditorViewModel = new EditorViewModel();

      // Create context menu items for Active Windows datagrid.
      var addItem = new MenuItem { Header = "Add" };
      addItem.Click += ActiveWindowsDataGrid_MenuItemClick;
      var highlightItem = new MenuItem { Header = "Highlight" };
      highlightItem.Click += ActiveWindowsDataGrid_HighlightClick;
      var containerizeItem = new MenuItem { Header = "Add to new Container" };
      containerizeItem.Click += ActiveWindowsDataGrid_ContainerizeClick;
      this.ActiveWindowsContextMenuItems = new ObservableCollection<FrameworkElement>
      {
        addItem,
        new Separator(),
        highlightItem,
        new Separator(),
        containerizeItem
      };

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
        throw new Exception("HandleProfileConfigEdit - No flyout available at index 1");
      }
      this.InfoMessageLabel.Text = message;
      flyout.IsOpen = true;
    }

    private void HandleProfileConfigEdit(Profile profile = null)
    {
      var flyout = (Flyout)this.FindName("LeftFlyout");
      if (null == flyout)
      {
        throw new Exception("Could not locate flyout LeftFlyout.");
      }
      
      ProfileConfig content;
      if (null == profile)
      {
        flyout.Header = "Add Profile";
        content = new ProfileConfig((p) => 
        {
          ProfileManager.AddNewProfile(p);
          this.ProfileListBox.SelectedItem = p;
          this.EditorViewModel.Update();
          ShowItemMessageToast(p.Name, "Profile added.");
        });
      }
      else
      {
        flyout.Header = "Edit Profile";
        content = new ProfileConfig(profile, (p) =>
        {
          profile.Update(p);
          ShowItemMessageToast(p.Name, "Profile updated.");
        });
      }
      content.OnClose = () => { flyout.IsOpen = false; };
      flyout.Content = content;
      flyout.IsOpen = true;
    }

    private void HandleContainerConfigEdit(WindowContainer container = null)
    {
      var flyout = (Flyout)this.FindName("LeftFlyout");
      if (null == flyout)
      {
        throw new Exception("HandleContainerConfigEdit - No flyout available.");
      }

      ContainerConfig content;
      if (null == container)
      {
        flyout.Header = "Add Container";
        content = new ContainerConfig((c) =>
        {
          if (!this.ProfileManager.AddToActiveProfile(c))
          {
            ShowErrorItemToast(c.Name, "Container already exists.");
            return;
          }
          ShowItemMessageToast(c.Name, "Container added.");
        });
      }
      else
      {
        flyout.Header = "Edit Container";
        content = new ContainerConfig(container, (c) =>
        {
          ShowItemMessageToast(c.Name, "Container updated.");
        });
      }
      content.OnClose = () => { flyout.IsOpen = false; };
      flyout.Content = content;
      flyout.IsOpen = true;
    }

    private void HandleWindowConfigEdit(WindowInfo item = null)
    {
      var flyout = this.Flyouts.Items[0] as Flyout;
      if (flyout == null) return;

      var windowConfig = null == item ? 
        new WindowConfig(w =>
        {
          ProfileManager.AddToActiveProfile(w);
          ShowItemMessageToast(w.Name, "window configuration added.");
        }) : 
        new WindowConfig(item, (w) =>
        {
          item.Update(w);
          ShowItemMessageToast(w.Name, "window configuration updated");
        });
      windowConfig.OnClose += () => { flyout.IsOpen = false; };

      flyout.Content = windowConfig;
      flyout.IsOpen = true;
    }

    private void HandleWindowConfigEdit(Process item)
    {
      var flyout = this.Flyouts.Items[0] as Flyout;
      if (flyout == null)
      {
        return;
      }
      var windowConfig = new WindowConfig(item, c =>
      {
        ProfileManager.AddToActiveProfile(c);
        ShowItemMessageToast(c.Name, "added to window configuration list.");
      });
      windowConfig.OnClose += () => { flyout.IsOpen = false; };

      flyout.Content = windowConfig;
      flyout.IsOpen = true;
    }

    private void HandleSettingsDialog()
    {
      var flyout = this.Flyouts.Items[1] as Flyout;
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

    private void WindowConfigDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (this.MonitorWindowConfigDataGrid.SelectedItem == null) return;
      var item = (WindowInfo)MonitorWindowConfigDataGrid.SelectedItem;
      HandleWindowConfigEdit(item);
    }

    private void ActiveWindowsGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (this.ActiveWindowsDataGrid.SelectedItem == null) return;
      var item = ((MonitoredProcess)this.ActiveWindowsDataGrid.SelectedItem).GetProcess();
      HandleWindowConfigEdit(item);
    }

    private void MonitorWindowConfigDataGrid_MenuItemClick(object sender, RoutedEventArgs e)
    {
      WindowInfo item = null;
      switch ((string) ((MenuItem) sender).Header)
      {
        case "Add":
          HandleWindowConfigEdit();
          break;
        case "Edit":
          item = (WindowInfo)MonitorWindowConfigDataGrid.SelectedItem;
          HandleWindowConfigEdit(item);
          break;
        case "Delete":
          item = (WindowInfo)MonitorWindowConfigDataGrid.SelectedItem;
          ProfileManager.RemoveFromActiveProfile(item);
          ShowItemMessageToast(item.Name, "window configuration deleted.");
          break;
      }
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

    private void RunProfileButton_OnClick(object sender, RoutedEventArgs e)
    {
      _monitorWindowManager.RefreshProfile();
    }

    private void ProfileListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var item = (Profile)this.ProfileListBox.SelectedItem;
      HandleProfileConfigEdit(item);
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
    
    private static WindowHostContainer _windowHostContainer = null;
    private void ActiveWindowsDataGrid_ContainerizeClick(object sender, RoutedEventArgs e)
    {
      if (this.ActiveWindowsDataGrid.SelectedItem == null) return;
      var process = ((MonitoredProcess)this.ActiveWindowsDataGrid.SelectedItem).GetProcess();
      
      if (_windowHostContainer == null)
      {
        //_windowHostContainer = new WindowHostContainer("ham", 2, 2);
        //_windowHostContainer.Show();
      }
      //_windowHostContainer.DockProc(process);
    }
    
    // TODO: Debug
    private void EditorWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
      //if (_windowHostContainer == null)
      //{
      //  _windowHostContainer = new WindowHostContainer(new WindowContainer("Bagel Container", 2, 2));
      //  _windowHostContainer.Show();
      //}
      //_windowHostContainer.DockProc(Process.Start("notepad.exe"));
      //_windowHostContainer.DockProc(Process.Start("notepad.exe"));
      //_windowHostContainer.DockProc(Process.Start("mspaint.exe"));
    }

    private void WindowConfigDataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    { 
      if (((PropertyDescriptor)e.PropertyDescriptor).IsBrowsable == false)
        e.Cancel = true;
    }

    private void WindowContainers_MenuItemClick(object sender, RoutedEventArgs e)
    {
      var menuItem = (MenuItem)sender;
      switch (menuItem.Header as string)
      {
        case "Add":
          HandleContainerConfigEdit();
          break;
        case "Edit":
          var item = (WindowContainer)this.WindowContainersListBox.SelectedItem;
          HandleContainerConfigEdit(item);
          break;
        case "Delete":
          //ShowItemMessageToast(this.ProfileManager.ActiveProfile.Name, "profile deleted.");
          break;
      }
    }

    private void ContainerWindowConfig_MenuItemClick(object sender, RoutedEventArgs e)
    {
      WindowInfo item = null;
      switch ((string)((MenuItem)sender).Header)
      {
        case "Add":
          //HandleWindowConfigEdit();
          break;
        case "Edit":
          item = (WindowInfo)ContainerWindowConfigDataGrid.SelectedItem;
          //HandleWindowConfigEdit(item);
          break;
        case "Delete":
          item = (WindowInfo)ContainerWindowConfigDataGrid.SelectedItem;
          //ProfileManager.RemoveFromActiveProfile(item);
          //ShowItemMessageToast(item.Name, "window configuration deleted.");
          break;
      }
    }
  }
}
