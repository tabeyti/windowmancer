using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windowmancer.Core.Models
{
  public class DockedWindow : ICloneable
  {
    public Process Process { get; set; }
    public IntPtr ParentHandle { get; set; }

    public int Row { get; set; }
    public int Column { get; set; }

    public DockedWindow() { }
    public DockedWindow(Process process)
    {
      this.Process = process;
    }

    public object Clone()
    {
      return new DockedWindow
      {
        Process = this.Process,
        ParentHandle = this.ParentHandle,
        Row = this.Row,
        Column = this.Column
      };
    }
  }
}
