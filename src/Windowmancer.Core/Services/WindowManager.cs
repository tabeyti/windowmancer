using System;
using System.Diagnostics;
using System.Linq;
using Windowmancer.Core.Extensions;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;

namespace Windowmancer.Core.Services
{
  public class WindowManager : IDisposable
  {
    public Profile ActiveProfile { get; set; }

    #region Constructors

    public WindowManager()
    {
    }

    #endregion Constructors

    public void Dispose()
    {
    }

    public void ApplyWindowInfo(Process process, bool newProcess = false)
    {
      var windowInfo = ActiveProfile?.Windows.Find(p => p.IsMatch(process));
      if (windowInfo == null) return;
      if (!newProcess || windowInfo.ApplyOnProcessStart)
      {
        ApplyWindowInfo(windowInfo, process);
      }
    }

    public void ApplyWindowInfo(WindowInfo windowInfo, Process process)
    {
      var handle = process.MainWindowHandle;
      if (handle == IntPtr.Zero)
      {
        return;
      }
      
      var x = (int)windowInfo.LayoutInfo.PositionInfo.X;
      var y = (int)windowInfo.LayoutInfo.PositionInfo.Y;
      var width = windowInfo.LayoutInfo.SizeInfo.Width;
      var height = windowInfo.LayoutInfo.SizeInfo.Height;

      Win32.ShowWindow(handle, Win32.ShowWindowCommands.Restore);
      Win32.SetForegroundWindow(handle);
      Win32.MoveWindow(handle, x, y, width, height, true);
    }

    public static Process GetProcess(WindowInfo windowInfo)
    {
      if (null == windowInfo)
      {
        return null;
      }

      var allProcceses = System.Diagnostics.Process.GetProcesses();
      return allProcceses.ToList().Find(windowInfo.IsMatch);
    }

    public static void ApplyWindowLayout(WindowLayoutInfo layoutInfo, Process process)
    {
      var handle = process.MainWindowHandle;
      if (handle == IntPtr.Zero)
      {
        return;
      }

      var x = layoutInfo.PositionInfo.X;
      var y = layoutInfo.PositionInfo.Y;
      var width = layoutInfo.SizeInfo.Width;
      var height = layoutInfo.SizeInfo.Height;

      Win32.ShowWindow(handle, Win32.ShowWindowCommands.Maximize);
      Win32.SetForegroundWindow(handle);
      Win32.MoveWindow(handle, x, y, width, height, true);
    }

    public void RefreshProfile()
    {
      var allProcceses = System.Diagnostics.Process.GetProcesses();
      foreach (var p in allProcceses)
      {
        if (p.MainWindowTitle == string.Empty)
        {
          continue;
        }
        var windowInfo = this.ActiveProfile.Windows.Find(pr => pr.IsMatch(p));
        if (null == windowInfo) continue;
        ApplyWindowInfo(windowInfo, p);
      }
    }
  }
}
