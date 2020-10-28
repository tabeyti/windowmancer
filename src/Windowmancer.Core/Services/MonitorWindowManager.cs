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
    public Profile ActiveProfile => _profileManager.ActiveProfile;

    private readonly ProfileManager _profileManager;

#region Constructors

    public MonitorWindowManager(ProfileManager profileManager)
    {
      _profileManager = profileManager;
    }

#endregion Constructors

    public void Dispose()
    {
    }

    /// <summary>
    /// Applies an existing window config object to the provided process 
    /// window. If no window config exists for the passed process window,
    /// nothing is done.
    /// </summary>
    /// <param name="process"></param>
    /// <param name="newProcess"></param>
    public void ApplyWindowConfig(Process process, bool newProcess = false)
    {
      var windowConfig = ActiveProfile?.Windows.Find(p => p.IsMatch(process));
      if (windowConfig == null) return;
      if (!newProcess || windowConfig.ApplyOnProcessStart)
      {
        ApplyWindowConfig(windowConfig, process);
      }
    }

    /// <summary>
    /// Applies the given layout info, held in the provided
    /// window config object, to the targeted process window.
    /// </summary>
    /// <param name="windowConfig"></param>
    /// <param name="process"></param>
    public void ApplyWindowConfig(WindowConfig windowConfig, Process process)
    {
      var handle = process.MainWindowHandle;
      if (handle == IntPtr.Zero || windowConfig.LayoutType == WindowConfigLayoutType.HostContainer)
      {
        return;
      }
      
      // Set layout values.
      var x = (int)windowConfig.MonitorLayoutInfo.PositionInfo.X;
      var y = (int)windowConfig.MonitorLayoutInfo.PositionInfo.Y;
      var width = windowConfig.MonitorLayoutInfo.SizeInfo.Width;
      var height = windowConfig.MonitorLayoutInfo.SizeInfo.Height;
      ShowWindowNormal(process);
      Win32.MoveWindow(handle, x, y, width, height, true);

      // Set styling values.
      Helper.SetWindowOpacityPercentage(process, windowConfig.StylingInfo.WindowOpacityPercentage);
    }


    /// <summary>
    /// Re-scans our active profile, applying the window config to applicable
    /// process windows found.
    /// </summary>
    public void RunProfile(Process[] processes = null)
    {
      processes = processes ?? System.Diagnostics.Process.GetProcesses();
      foreach (var p in processes)
      {
        if (p.MainWindowTitle == string.Empty) { continue; }
        var windowConfig = this.ActiveProfile.Windows.Find(pr => pr.IsMatch(p));
        if (null == windowConfig || WindowConfigLayoutType.Monitor != windowConfig.LayoutType) continue;
        ApplyWindowConfig(windowConfig, p);
      }
    }

    #region Static Methods

    /// <summary>
    /// Applies the given layout info to the targeted process window.
    /// </summary>
    /// <param name="layoutInfo"></param>
    /// <param name="process"></param>
    public static void ApplyLayout(MonitorLayoutInfo layoutInfo, Process process)
    {
      if (null == layoutInfo || null == process) { return; }
      var handle = process.MainWindowHandle;
      if (handle == IntPtr.Zero) { return; }

      // Set layout values.
      var x = layoutInfo.PositionInfo.X;
      var y = layoutInfo.PositionInfo.Y;
      var width = layoutInfo.SizeInfo.Width;
      var height = layoutInfo.SizeInfo.Height;
      ShowWindowNormal(process);
      Win32.MoveWindow(handle, x, y, width, height, true);
    }

    /// <summary>
    /// Retrieves the layout object for the provided process window.
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public static MonitorLayoutInfo GetLayout(Process process)
    {
      var rect = Helper.GetWindowRectCurrent(process);
      return new MonitorLayoutInfo(
        rect.X,
        rect.Y,
        rect.Width,
        rect.Height);
    }

    public static void ApplyToGridLayout(
      Process process,
      string displayName,
      int totalRows,
      int totalCols,
      int row, int col)
    {
      var screen = Helper.GetScreen(displayName);

      var screenWidth = screen.Bounds.Width;
      var screenHeight = screen.Bounds.Height;

      var x = (screenWidth / totalCols) * col + screen.Bounds.X;
      var y = (screenHeight / totalRows) * row + screen.Bounds.Y;

      var width = (screenWidth / totalCols);
      var height = (screenHeight / totalRows);

      var layoutInfo = new MonitorLayoutInfo(x, y, width, height);
      ApplyLayout(layoutInfo, process);
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
