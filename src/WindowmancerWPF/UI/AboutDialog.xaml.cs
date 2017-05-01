using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WindowmancerWPF.UI
{
  /// <summary>
  /// Interaction logic for AboutDialog.xaml
  /// </summary>
  public partial class AboutDialog : UserControl
  {
    public Action OnClose { get; set; }

    public Version Version => Assembly.GetExecutingAssembly().GetName().Version;

    public string ApplicationName => Assembly.GetExecutingAssembly().GetName().Name;

    public string Description => ((AssemblyDescriptionAttribute) Assembly.GetExecutingAssembly()
      .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;

    public string Copyright => ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly()
      .GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;

    public string Company => ((AssemblyCompanyAttribute)Assembly.GetExecutingAssembly()
      .GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)[0]).Company;


    public AboutDialog(Action onClose)
    {
      this.OnClose = onClose;
      InitializeComponent();
    }

    private void Close()
    {
      var window = Window.GetWindow(this);
      if (window == null)
      {
        throw new Exception("AboutDialog - Could locate active window to unbind the KeyDown listener.");
      }
      window.KeyDown -= AboutDialog_OnKeyPress;
      this.OnClose?.Invoke();
    }

    private void AboutDialog_OnLoaded(object sender, RoutedEventArgs e)
    {
      var window = Window.GetWindow(this);
      if (window == null)
      {
        throw new Exception("AboutDialog - Could locate active window to bind the KeyDown listener.");
      }
      window.KeyDown += AboutDialog_OnKeyPress;
    }

    private void AboutDialog_OnKeyPress(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key != Key.Escape && e.Key != Key.Return)
      {
        return;
      }
      Close();
    }
  }
}
