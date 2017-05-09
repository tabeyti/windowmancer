using System;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using MahApps.Metro.Controls.Dialogs;
using Windowmancer.Core.Models;
using Windowmancer.Core.Services;
using Windowmancer.UI.Base;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class EditorWindow
  {
    public ProfileManager ProfileManager { get; }
    public ProcMonitor ProcMonitor { get; }

    private readonly WindowManager _windowManager;
    private readonly KeyHookManager _keyHookManager;

    public EditorViewModel EditorViewModel { get; set; }
    
    public EditorWindow()
    {
      this.ProfileManager = App.ServiceResolver.Resolve<ProfileManager>();
      this.ProcMonitor = App.ServiceResolver.Resolve<ProcMonitor>();

      _windowManager = App.ServiceResolver.Resolve<WindowManager>();
      _keyHookManager = App.ServiceResolver.Resolve<KeyHookManager>();
      

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
    }

    private void ShowItemMessageToast(string itemName, string message = null)
    {
      var flyout = this.Flyouts.Items[2] as Flyout;
      if (null == flyout)
      {
        throw new Exception("HandleProfileConfigEdit - No flyout available at index 1");
      }
      this.DeleteToastItem.Text = itemName;
      this.DeleteToastMessage.Text = message ?? this.DeleteToastItem.Text;
      flyout.IsOpen = true;
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
          ProfileManager.AddNewProfile(p);
          this.ProfileListBox.SelectedItem = p;
          this.EditorViewModel.Update();
          ShowItemMessageToast(p.Name, "profile added.");
        });
      }
      else
      {
        flyout.Header = "Edit Profile";
        content = new ProfileConfig(profile, (p) =>
        {
          profile.Update(p);
          ShowItemMessageToast(p.Name, "profile updated.");
        } );
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
      WindowInfo item = null;
      switch ((string) ((MenuItem) sender).Header)
      {
        case "Add":
          HandleWindowConfigEdit();
          break;
        case "Edit":
          item = (WindowInfo)WindowConfigDataGrid.SelectedItem;
          HandleWindowConfigEdit(item);
          break;
        case "Delete":
          item = (WindowInfo)WindowConfigDataGrid.SelectedItem;
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
      _windowManager.RefreshProfile();
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
  }
}
