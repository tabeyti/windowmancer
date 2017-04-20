using System.Windows;
using Microsoft.Practices.Unity;
using WindowmancerWPF.Practices;
using WindowmancerWPF.UI;

namespace WindowmancerWPF
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App
  {
    public static IUnityContainer ServiceResolver = WMServiceResolver.Instance;

    private void App_OnExit(object sender, ExitEventArgs e)
    {
      ServiceResolver.Dispose();
    }

    private void App_OnStartup(object sender, StartupEventArgs e)
    {
      var editor = new EditorWindow();
      editor.Show();
    }
  }
}
