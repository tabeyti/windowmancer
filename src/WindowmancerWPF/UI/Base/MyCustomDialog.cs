using System;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace WindowmancerWPF.UI.Base
{
  public partial class MyCustomDialog : CustomDialog
  {
    public MyCustomDialog()
    {
      Initialize();
      InitializeComponent();
    }

    private void Initialize()
    {
      this.Resources.MergedDictionaries.Add(new ResourceDictionary
      {
        Source = new Uri("pack://application:,,,/UI/Themes/MyBaseDark.xaml")
      });
    }
  }
}
