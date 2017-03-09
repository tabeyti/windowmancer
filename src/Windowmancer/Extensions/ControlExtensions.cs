using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windowmancer.Extensions
{
  public static class ControlExtensions
  {
    /// <summary>
    /// Extension method that allows for automatic anonymous method invocation.
    /// </summary>
    public static void InvokeControl(this Control c, MethodInvoker mi)
    {
      try
      {
        c.Invoke(mi);
      }
      catch (Exception e)
      {
        MessageBox.Show(e.Message);
      }
    }
  }
}
