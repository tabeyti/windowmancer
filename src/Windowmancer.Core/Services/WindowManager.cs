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

    /// <summary>
    /// Applies an existing window info object to the provided process 
    /// window. If no window info exists for the passed process window,
    /// nothing is done.
    /// </summary>
    /// <param name="process"></param>
    /// <param name="newProcess"></param>
    public void ApplyWindowInfo(Process process, bool newProcess = false)
    {
      var windowInfo = ActiveProfile?.Windows.Find(p => p.IsMatch(process));
      if (windowInfo == null) return;
      if (!newProcess || windowInfo.ApplyOnProcessStart)
      {
        ApplyWindowInfo(windowInfo, process);
      }
    }

    /// <summary>
    /// Applies the given layout info, held in the provided
    /// window info object, to the targeted process window.
    /// </summary>
    /// <param name="windowInfo"></param>
    /// <param name="process"></param>
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

      ShowWindowNormal(process);
      Win32.MoveWindow(handle, x, y, width, height, true);
    }


    /// <summary>
    /// Re-scans our active profile, applying the window config to applicable
    /// process windows found.
    /// </summary>
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

    #region Static Methods

    /// <summary>
    /// Attempts to retrieve the running process for the given
    /// window info.
    /// </summary>
    /// <param name="windowInfo"></param>
    /// <returns></returns>
    public static Process GetProcess(WindowInfo windowInfo)
    {
      if (null == windowInfo)
      {
        return null;
      }

      var allProcceses = System.Diagnostics.Process.GetProcesses();
      return allProcceses.ToList().Find(windowInfo.IsMatch);
    }

    /// <summary>
    /// Applies the given layout info to the targeted process window.
    /// </summary>
    /// <param name="layoutInfo"></param>
    /// <param name="process"></param>
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

      ShowWindowNormal(process);
      Win32.MoveWindow(handle, x, y, width, height, true);
    }
    
    /// <summary>
    /// Shows a process window in it's normal position (non-maximized/minimized)
    /// and brought to foreground.
    /// </summary>
    /// <param name="handle"></param>
    public static void ShowWindowNormal(Process process)
    {
      SetWindowState(process, ProcessWindowState.Normal);
      Win32.SetForegroundWindow(process.MainWindowHandle);
    }

    /// <summary>
    /// Gets the provided process window's state.
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public static ProcessWindowState GetWindowState(Process process)
    {
      var handle = process.MainWindowHandle;
      var placement = new Win32.Win32_WindowPlacement();
      placement.length = System.Runtime.InteropServices.Marshal.SizeOf(placement);
      Win32.GetWindowPlacement(handle, ref placement);
      return ProcWinStateFromWin32(placement.showCmd);
    }

    /// <summary>
    /// Sets the given window state on the provided process window.
    /// </summary>
    /// <param name="process"></param>
    /// <param name="state"></param>
    public static void SetWindowState(Process process, ProcessWindowState state)
    {
      var handle = process.MainWindowHandle;
      var win32State = (Win32.ShowWindowCommands) state;
      Win32.ShowWindow(handle, win32State);
    }

    public static Win32.Win32_Rect GetCurrentRect(Process process)
    {
      var rec = new Win32.Win32_Rect();
      Win32.GetWindowRect(process.MainWindowHandle, ref rec);
      return rec;
    }

    public static Win32.Win32_Rect GetNormalRect(Process process)
    {
      return Win32.GetPlacement(process.MainWindowHandle);
    }

    private static ProcessWindowState ProcWinStateFromWin32(int win32WindowState)
    {
      var showCmd = (Win32.ShowWindowCommands)win32WindowState;
      switch (showCmd)
      {
        case Win32.ShowWindowCommands.Hide:
        case Win32.ShowWindowCommands.Minimize:
        case Win32.ShowWindowCommands.ShowMinimized:
          return ProcessWindowState.Minimized;
        case Win32.ShowWindowCommands.ShowNormal:
          return ProcessWindowState.Normal;
        case Win32.ShowWindowCommands.ShowMaximized:
          return ProcessWindowState.Maximized;
        default:
          return ProcessWindowState.Normal;
      }
    }

    #endregion
  }

  /// <summary>
  /// Our window states. Simplified version of 
  /// <see cref="Win32.ShowWindowCommands"/>.
  /// </summary>
  public enum ProcessWindowState
  {
    Normal = 1,
    Maximized = 3,
    Minimized = 6,
  }
}
