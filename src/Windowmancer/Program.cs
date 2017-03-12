using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windowmancer.Pratices;
using Windowmancer.Services;
using Windowmancer.UI;

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
      //var stuff = new KeyComboConfig(new[] { Keys.LControlKey, Keys.LShiftKey, Keys.K });
      //var str = JsonConvert.SerializeObject(stuff);

      //var thing = JsonConvert.DeserializeObject<Keys[]>(str);

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new UI.TrayApp(ServiceResolver));
    }
  }
}
