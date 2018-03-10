using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Windowmancer.Core.Models
{
  public class DockableWindow : ICloneable
  {
    public Process Process { get; set; }
    public IntPtr ParentHandle { get; set; }

    public EventHandler OnProcessExited { get; set; }

    public int Row
    {
      get => (int) this.WindowConfig.HostContainerLayoutInfo.Row;
      set => this.WindowConfig.HostContainerLayoutInfo.Row = (uint) value;
    }
    
    public int Column 
    {
      get => (int) this.WindowConfig.HostContainerLayoutInfo.Column;
      set => this.WindowConfig.HostContainerLayoutInfo.Column = (uint) value;
    }

    public WindowConfig WindowConfig { get; set; }
    
    public DockableWindow() { }
    
    public DockableWindow(Process process, WindowConfig windowConfig)
    {
      this.Process = process;
      process.EnableRaisingEvents = true;
      this.WindowConfig = windowConfig;
    }

    public DockableWindow(WindowConfig windowConfig)
    {
      this.WindowConfig = windowConfig;
    }

    public object Clone()
    {
      return new DockableWindow
      {
        Process = this.Process,
        ParentHandle = this.ParentHandle,
        OnProcessExited = this.OnProcessExited,
        WindowConfig = (WindowConfig)this.WindowConfig.Clone()
      };
    }
  }
}
