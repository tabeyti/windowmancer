using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WindowmancerWPF.Models;

namespace WindowmancerWPF.UI
{
  /// <summary>
  /// Interaction logic for ProfileConfig.xaml
  /// </summary>
  public partial class ProfileConfig : UserControl
  {
    public Action OnClose { get; set; }

    public Action<Profile> OnSave { get; set; }

    public Profile Profile { get; set; }

    public ProfileConfig(Action<Profile> onSave)
    {
      this.OnSave = onSave;
      PreInitialize();
      InitializeComponent();
    }

    public ProfileConfig(Profile profile, Action<Profile> onSave)
    {
      this.OnSave = onSave;
      this.Profile = profile;
      PreInitialize();
      InitializeComponent();
      Initialize();
    }

    private void PreInitialize()
    {
      if (null == this.Profile)
      {
        this.Profile = new Profile
        {
          Id = Guid.NewGuid().ToString(),
          Name = "",
          Windows = new ObservableCollection<WindowInfo>()
        };
      }
    }
    private void Initialize()
    {
      this.ProfileNameTextBox.Text = this.Profile.Name;
    }

    private void Close()
    {
      var window = Window.GetWindow(this);
      if (window == null)
      {
        throw new Exception("ProfileConfig - Could locate active window to unbind the KeyDown listener.");
      }
      window.KeyDown -= ProfileConfig_HandleKeyPress;
      OnClose?.Invoke();
    }

    private void ProfileConfig_OnLoaded(object sender, RoutedEventArgs e)
    {
      var window = Window.GetWindow(this);
      if (window == null)
      {
        throw new Exception("ProfileConfig - Could locate active window to bind the KeyDown listener.");
      }
      window.KeyDown += ProfileConfig_HandleKeyPress;
    }
    
    private void ProfileConfig_HandleKeyPress(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key != Key.Escape)
      {
        return;
      }
      Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void OkayButton_Click(object sender, RoutedEventArgs e)
    {
      ForceDataValidation();
      OnSave?.Invoke(this.Profile);
      Close();
    }

    /// <summary>
    /// Forces an update to the data binding on a text box which has focus.
    /// </summary>
    private static void ForceDataValidation()
    {
      var textBox = Keyboard.FocusedElement as TextBox;
      if (textBox != null)
      {
        BindingExpression be = textBox.GetBindingExpression(TextBox.TextProperty);
        if (be != null && !textBox.IsReadOnly && textBox.IsEnabled)
        {
          be.UpdateSource();
        }
      }
    }
  }
}
