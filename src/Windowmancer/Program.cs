using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windowmancer.Pratices;

namespace Windowmancer
{
  static class Program
  {
    private static IUnityContainer ServiceResolver = WMServiceResolver.Instance;

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm(ServiceResolver));
    }
  }
}
