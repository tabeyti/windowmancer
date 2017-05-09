using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Windowmancer.Core.Models;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for PreferencesDialog.xaml
  /// </summary>
  public partial class PreferencesDialog : UserControl
  {
    public Action OnClose { get; set; }
    public Action<object> OnSave { get; set; }

    public PreferencesModel Preferences { get; set; }

    public PreferencesDialog(PreferencesModel preferences)
    {
      this.Preferences = (PreferencesModel)preferences.Clone();
      InitializeComponent();
      Initialize();
    }

    public PreferencesDialog(PreferencesModel preferences, Action<object> onSave)
    {
      this.Preferences = (PreferencesModel)preferences.Clone();
      this.OnSave = onSave;
      InitializeComponent();
      Initialize();
    }

    private void Initialize()
    {
      this.HotKeyInputBox.SetHotKeyConfig(this.Preferences.HotKeyConfig);
    }

    private void Close(bool save)
    {
      var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
      if (window == null)
      {
        throw new Exception("PreferencesDialog - Could locate active window to unbind the KeyDown listener.");
      }
      window.KeyDown -= SettingsDialog_KeyDown;

      if (save)
      {
        this.Preferences.HotKeyConfig = this.HotKeyInputBox.GetHotKeyConfig();
        this.OnSave?.Invoke(this.Preferences);
      }
      this.OnClose?.Invoke();
    }

    private void PreferencesDialog_OnLoaded(object sender, RoutedEventArgs e)
    {
      var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
      if (window == null)
      {
        throw new Exception("AboutDialog - Could locate active window to bind the KeyDown listener.");
      }
      window.KeyDown += SettingsDialog_KeyDown; ;
    }

    private void SettingsDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Escape && e.Key != Key.Return)
      {
        return;
      }
      Close(e.Key == Key.Return);
    }
  }
}
