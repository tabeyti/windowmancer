using System;
using System.Diagnostics;
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

      // Get absolute position based on monitor target.
      //var screen = Screen.AllScreens.ToList().Find(s => s.DeviceName == windowInfo.LocationInfo.DisplayName);
      //var x = screen.WorkingArea.Left + windowInfo.LocationInfo.PositionInfo.X;
      //var y = screen.WorkingArea.Top + windowInfo.LocationInfo.PositionInfo.Y;

      var x = windowInfo.LocationInfo.PositionInfo.X;
      var y = windowInfo.LocationInfo.PositionInfo.Y;

      windowInfo.BringToFront.RunIfTrue(() =>
      {
        Win32.ShowWindow(handle, Win32.ShowWindowCommands.Maximize);
        Win32.SetForegroundWindow(handle);
      });
      Win32.MoveWindow(handle, x, y, windowInfo.SizeInfo.Width, windowInfo.SizeInfo.Height, true);      
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
