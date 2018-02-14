using System;
using System.Diagnostics;
using System.Linq;
using Windowmancer.Core.Extensions;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;

namespace Windowmancer.Core.Services
{
  public class MonitorWindowManager : IDisposable
  {
    public Profile ActiveProfile { get; set; }

    #region Constructors

    public MonitorWindowManager()
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
    public void ApplyWindowConfig(Process process, bool newProcess = false)
    {
      var windowInfo = ActiveProfile?.Windows.Find(p => p.IsMatch(process));
      if (windowInfo == null) return;
      if (!newProcess || windowInfo.ApplyOnProcessStart)
      {
        ApplyWindowConfig(windowInfo, process);
      }
    }

    /// <summary>
    /// Applies the given layout info, held in the provided
    /// window info object, to the targeted process window.
    /// </summary>
    /// <param name="windowInfo"></param>
    /// <param name="process"></param>
    public void ApplyWindowConfig(WindowConfig windowInfo, Process process)
    {
      var handle = process.MainWindowHandle;
      if (handle == IntPtr.Zero)
      {
        return;
      }
      
      // Set layout values.
      var x = (int)windowInfo.MonitorLayoutInfo.PositionInfo.X;
      var y = (int)windowInfo.MonitorLayoutInfo.PositionInfo.Y;
      var width = windowInfo.MonitorLayoutInfo.SizeInfo.Width;
      var height = windowInfo.MonitorLayoutInfo.SizeInfo.Height;
      ShowWindowNormal(process);
      Win32.MoveWindow(handle, x, y, width, height, true);

      // Set styling values.
      SetWindowOpacityPercentage(process, windowInfo.StylingInfo.WindowOpacityPercentage);
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
        ApplyWindowConfig(windowInfo, p);
      }
    }

    #region Static Methods

    /// <summary>
    /// Attempts to retrieve the running process for the given
    /// window info.
    /// </summary>
    /// <param name="windowInfo"></param>
    /// <returns></returns>
    public static Process GetProcess(WindowConfig windowInfo)
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
    public static void ApplyWindowLayout(MonitorLayoutInfo layoutInfo, Process process)
    {
      if (null == layoutInfo || null == process)
      {
        return;
      }

      var handle = process.MainWindowHandle;
      if (handle == IntPtr.Zero)
      {
        return;
      }

      // Set layout values.
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
    /// <param name="process"></param>
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

    /// <summary>
    /// Retrieves the process window's rectangle as currently is.
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public static Win32.Win32_Rect GetWindowRectCurrent(Process process)
    {
      var rec = new Win32.Win32_Rect();
      Win32.GetWindowRect(process.MainWindowHandle, ref rec);
      return rec;
    }

    /// <summary>
    /// Retrieves the process window's rectangle when not minimized/maximized.
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public static Win32.Win32_Rect GetWindowRectNormal(Process process)
    {
      return Win32.GetPlacement(process.MainWindowHandle);
    }

    /// <summary>
    /// Set's the opacity percentage of the provided process window.
    /// A value of a 100 means the window is completely visible where a value
    /// of 0 means the window is completely transparent.
    /// </summary>
    /// <param name="process"></param>
    /// <param name="opacityPercentage"></param>
    public static void SetWindowOpacityPercentage(Process process, uint opacityPercentage)
    {
      if (opacityPercentage > 100)
      {
        throw new Exception($"MonitorWindowManager.SetWindowOpacityPercentage - Opacity percentage cannot be above 100. Value given: {opacityPercentage}");
      }
      
      var handle = process.MainWindowHandle;

      // Only set the layered window attribute if it hasn't already been set for this
      // window process handle.
      uint crKey = 0;
      byte bAlpha = 0;
      uint dwFlags = 0;
      Win32.GetLayeredWindowAttributes(process.MainWindowHandle, out crKey, out bAlpha, out dwFlags);

      if (dwFlags == 0)
      {
        Win32.SetWindowLong(handle, Win32.GWL_EXSTYLE, Win32.GetWindowLong(handle, Win32.GWL_EXSTYLE) ^ Win32.WS_EX_LAYERED);
      }

      opacityPercentage = opacityPercentage > 100 ? 100 : opacityPercentage;
      bAlpha = (byte)Math.Round(255 * ((double)opacityPercentage / 100));
      Win32.SetLayeredWindowAttributes(handle, 0, bAlpha, Win32.LWA_ALPHA);
    }

    /// <summary>
    /// Gets the targeted process window's opacity percentage. 
    /// A value of a 100 means the window is completely visible where a value
    /// of 0 means the window is completely transparent.
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public static uint GetWindowOpacityPercentage(Process process)
    {
      uint crKey = 0;
      byte bAlpha = 0;
      uint dwFlags = 0;
      Win32.GetLayeredWindowAttributes(process.MainWindowHandle, out crKey, out bAlpha, out dwFlags);
      return bAlpha == 0 ? 100 : (uint)Math.Round(100 * ((double)bAlpha / 255));
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
