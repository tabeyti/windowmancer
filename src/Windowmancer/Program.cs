using Microsoft.Practices.Unity;
using System;
using System.Windows.Forms;
using Windowmancer.Practices;

namespace Windowmancer
{
  static class Program
  {
    private static readonly IUnityContainer ServiceResolver = WMServiceResolver.Instance;

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new UI.TrayApp(ServiceResolver));
    }
  }
}
