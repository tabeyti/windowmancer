using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowmancerWPF.Models;

namespace WindowmancerWPF.UI
{
  /// <summary>
  /// Interaction logic for ProfileConfig.xaml
  /// </summary>
  public partial class ProfileConfig : UserControl
  {
    public event EventHandler OnClose;

    public Profile Profile { get; set; }

    public ProfileConfig()
    {
      PreInitialize();
      InitializeComponent();
    }

    public ProfileConfig(Profile profile)
    {
      this.Profile = profile;
      PreInitialize();
      InitializeComponent();
      Initialize();
    }

    private void PreInitialize()
    {
      if (null == this.Profile)
      {
        this.Profile = new Profile();
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
      OnClose?.Invoke(this, new EventArgs());
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
      Close();
    }
  }
}
