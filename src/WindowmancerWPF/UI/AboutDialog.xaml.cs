using System;
using System.Reflection;
using System.Windows.Controls;

namespace WindowmancerWPF.UI
{
  /// <summary>
  /// Interaction logic for AboutDialog.xaml
  /// </summary>
  public partial class AboutDialog : UserControl
  {
    public Version Version => Assembly.GetExecutingAssembly().GetName().Version;

    public string ApplicationName => Assembly.GetExecutingAssembly().GetName().Name;

    public string Description => ((AssemblyDescriptionAttribute) Assembly.GetExecutingAssembly()
      .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;

    public string Copyright => ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly()
      .GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;

    public string Company => ((AssemblyCompanyAttribute)Assembly.GetExecutingAssembly()
      .GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)[0]).Company;


    public AboutDialog()
    {

      //var app = Assembly.GetExecutingAssembly();
      //var title = (AssemblyTitleAttribute)app.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0];
      //var product = (AssemblyProductAttribute)app.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0];
      //var copyright = (AssemblyCopyrightAttribute)app.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0];
      //var company = (AssemblyCompanyAttribute)app.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)[0];
      //var description = (AssemblyDescriptionAttribute)app.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0];

      InitializeComponent();
    }
  }
}
