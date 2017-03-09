using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windowmancer.Models
{
  public class ExceptionBox : Exception
  {
    public ExceptionBox(Exception e) : base(e.Message, e)
    {
      MessageBox.Show($"{e.Message}\n{e.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }


    public ExceptionBox(string message) : base(message)
    {
      MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }
}
