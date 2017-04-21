using System;
using System.Windows.Controls;
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
    }

    private void PreInitialize()
    {
      if (null == this.Profile)
      {
        this.Profile = new Profile();
      }
    }
  }
}
