using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Windowmancer.Extensions;
using Windowmancer.Models;

namespace Windowmancer.Services
{
  public class WindowManager : IDisposable
  {
    #region DLL Imports

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);

    #endregion DLL Imports

    public Profile CurrentProfile { get; set; }

    #region Constructors

    public WindowManager()
    {
    }

    #endregion Constructors

    public void Dispose()
    {
    }

    public void LoadProfile(Profile profile)
    {
      this.CurrentProfile = profile;
    }

    public void ApplyWindowInfo(Process process)
    {
      var windowInfo = this.CurrentProfile.Windows.Find(p => p.IsMatch(process));
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
      var y = windowInfo.LocationInfo.PositionInfo.X;

      MoveWindow(handle, x, y, windowInfo.SizeInfo.Width, windowInfo.SizeInfo.Height, true);
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
        var windowInfo = this.CurrentProfile.Windows.Find(pr => pr.IsMatch(p));
        if (null == windowInfo) continue;
        ApplyWindowInfo(windowInfo, p);
      }
    }
  }
}
