using System;
using System.Diagnostics;

namespace Windowmancer.Core.Models
{
  public class DockableWindow : ICloneable
  {
    public Process Process { get; set; }
    public IntPtr ParentHandle { get; set; }

    public EventHandler OnProcessExited { get; set; }

    public int Row { get; set; }
    public int Column { get; set; }

    public DockableWindow() { }
    public DockableWindow(Process process)
    {
      this.Process = process;
      process.EnableRaisingEvents = true;
    }

    public object Clone()
    {
      return new DockableWindow
      {
        Process = this.Process,
        ParentHandle = this.ParentHandle,
        Row = this.Row,
        Column = this.Column,
        OnProcessExited = this.OnProcessExited
      };
    }
  }
}
