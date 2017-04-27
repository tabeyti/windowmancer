using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Windowmancer.Extensions;
using Windowmancer.Models;
using Windowmancer.Practices;

namespace Windowmancer.Services
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

    public void ApplyWindowInfo(Process process)
    {
      var windowInfo = this.ActiveProfile.Windows.Find(p => p.IsMatch(process));
      if (windowInfo == null)
        return;
      ApplyWindowInfo(windowInfo, process);
    }

    public void ApplyWindowInfo(WindowInfo windowInfo, Process process)
    {
      var handle = process.MainWindowHandle;
      if (handle == IntPtr.Zero)
      {
        return;
      }

      var x = windowInfo.LocationInfo.PositionInfo.X;
      var y = windowInfo.LocationInfo.PositionInfo.Y;

      windowInfo.BringToFront.RunIfTrue(() =>
      {
        Win32.ShowWindow(handle, Win32.ShowWindowCommands.Maximize);
        Win32.SetForegroundWindow(handle);
      });
      Win32.MoveWindow(handle, x, y, windowInfo.SizeInfo.Width, windowInfo.SizeInfo.Height, true);
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
