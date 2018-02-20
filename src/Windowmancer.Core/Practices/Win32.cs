using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Windowmancer.Core.Practices
{
  public static class Win32
  {
    [StructLayout(LayoutKind.Sequential)]
    public struct Win32_Rect
    {
      public int Left;
      public int Top;
      public int Right;
      public int Bottom;

      public int X => this.Left;
      public int Y => this.Top;

      public int Width
      {
        get => this.Right - this.Left;
        set => this.Right = value + this.Left;
      }

      public int Height
      {
        get => this.Bottom - this.Top;
        set => this.Bottom = value + this.Top;
      }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Win32_WindowConfig
    {
      public uint cbSize;
      public Win32.Win32_Rect rcWindow;
      public Win32.Win32_Rect rcClient;
      public uint dwStyle;
      public uint dwExStyle;
      public uint dwWindowStatus;
      public uint cxWindowBorders;
      public uint cyWindowBorders;
      public ushort atomWindowType;
      public ushort wCreatorVersion;

      public Win32_WindowConfig(bool? filler)
       : this()   // Allows automatic initialization of "cbSize" with "new Win32_WindowConfig(null/true/false)".
      {
        cbSize = (uint)(Marshal.SizeOf(typeof(Win32_WindowConfig)));
      }
    }

    /// <summary>
    /// See MSDN Win32_WindowPlacement Structure http://msdn.microsoft.com/en-us/library/ms632611(v=VS.85).aspx
    /// </summary>
    public struct Win32_WindowPlacement
    {
      public int length;
      public int flags;
      public int showCmd;
      public System.Drawing.Point ptMinPosition;
      public System.Drawing.Point ptMaxPosition;
      public Win32_Rect rcNormalPosition;
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetWindowRect(IntPtr hWnd, ref Win32.Win32_Rect win32Rect);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr FindWindow(string strClassName, string strWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowInfo(IntPtr hwnd, ref Win32_WindowConfig pwi);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsIconic(IntPtr hWnd);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("user32.dll")]
    public static extern bool GetWindowPlacement(IntPtr hWnd, ref Win32_WindowPlacement lpwndpl);

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool repaint);

    [DllImport("user32.DLL")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

    [DllImport("user32.dll")]
    public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetLayeredWindowAttributes(IntPtr hwnd, out uint crKey, out byte bAlpha, out uint dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
    
    [DllImport("user32.dll")]
    public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc,
      WinEventDelegate lpfnWinEventProc, uint idProcess,
      uint idThread, uint dwFlags);

    [DllImport("user32.dll")]
    public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern int GetMenuItemCount(IntPtr hMenu);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern bool GetMenuItemInfo(IntPtr hMenu, int uItem, bool fByPosition, [In, Out] MENUITEMINFO lpmii);

    [DllImport("user32.dll")]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
      IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    [DllImport("user32.dll")]
    public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class MENUITEMINFO
    {
      public int cbSize = Marshal.SizeOf(typeof(MENUITEMINFO));
      public int fMask;
      public int fType;
      public int fState;
      public int wID;
      public IntPtr hSubMenu;
      public IntPtr hbmpChecked;
      public IntPtr hbmpUnchecked;
      public IntPtr dwItemData;
      public IntPtr dwTypeData;
      public int cch;
    }

    public const int GWL_EXSTYLE = -20;
    public const int WS_EX_LAYERED = 0x80000;
    public const int LWA_ALPHA = 0x2;
    public const int LWA_COLORKEY = 0x1;
    
    /// <summary>
    /// Gets the window placement of the specified window in Normal state.
    /// </summary>
    /// <param name="handle">The handle.</param>
    /// <returns></returns>
    public static Win32.Win32_Rect GetPlacement(IntPtr handle)
    {
      var placement = new Win32_WindowPlacement();
      placement.length = System.Runtime.InteropServices.Marshal.SizeOf(placement);
      GetWindowPlacement(handle, ref placement);
      return placement.rcNormalPosition;
    }

    /// <summary>Enumeration of the different ways of showing a window using
    /// ShowWindowNormal</summary>
    public enum ShowWindowCommands : uint
    {
      /// <summary>Hides the window and activates another window.</summary>
      /// <remarks>See SW_HIDE</remarks>
      Hide = 0,
      /// <summary>Activates and displays a window. If the window is minimized
      /// or maximized, the system restores it to its original size and
      /// position. An application should specify this flag when displaying
      /// the window for the first time.</summary>
      /// <remarks>See SW_SHOWNORMAL</remarks>
      ShowNormal = 1,
      /// <summary>Activates the window and displays it as a minimized window.</summary>
      /// <remarks>See SW_SHOWMINIMIZED</remarks>
      ShowMinimized = 2,
      /// <summary>Activates the window and displays it as a maximized window.</summary>
      /// <remarks>See SW_SHOWMAXIMIZED</remarks>
      ShowMaximized = 3,
      /// <summary>Maximizes the specified window.</summary>
      /// <remarks>See SW_MAXIMIZE</remarks>
      Maximize = 3,
      /// <summary>Displays a window in its most recent size and position.
      /// This value is similar to "ShowNormal", except the window is not
      /// actived.</summary>
      /// <remarks>See SW_SHOWNOACTIVATE</remarks>
      ShowNormalNoActivate = 4,
      /// <summary>Activates the window and displays it in its current size
      /// and position.</summary>
      /// <remarks>See SW_SHOW</remarks>
      Show = 5,
      /// <summary>Minimizes the specified window and activates the next
      /// top-level window in the Z order.</summary>
      /// <remarks>See SW_MINIMIZE</remarks>
      Minimize = 6,
      /// <summary>Displays the window as a minimized window. This value is
      /// similar to "ShowMinimized", except the window is not activated.</summary>
      /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
      ShowMinNoActivate = 7,
      /// <summary>Displays the window in its current size and position. This
      /// value is similar to "Show", except the window is not activated.</summary>
      /// <remarks>See SW_SHOWNA</remarks>
      ShowNoActivate = 8,
      /// <summary>Activates and displays the window. If the window is
      /// minimized or maximized, the system restores it to its original size
      /// and position. An application should specify this flag when restoring
      /// a minimized window.</summary>
      /// <remarks>See SW_RESTORE</remarks>
      Restore = 9,
      /// <summary>Sets the show state based on the SW_ value specified in the
      /// STARTUPINFO structure passed to the CreateProcess function by the
      /// program that started the application.</summary>
      /// <remarks>See SW_SHOWDEFAULT</remarks>
      ShowDefault = 10,
      /// <summary>Windows 2000/XP: Minimizes a window, even if the thread
      /// that owns the window is hung. This flag should only be used when
      /// minimizing windows from a different thread.</summary>
      /// <remarks>See SW_FORCEMINIMIZE</remarks>
      ForceMinimized = 11
    }

    #region Helper Methods

    /// <summary>
    /// Retrieves the provided process' window rectangel.
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public static Win32.Win32_Rect GetProcessWindowRec(Process process)
    {
      var info = new Win32.Win32_WindowConfig();
      var rec = new Win32.Win32_Rect();

      Win32.GetWindowInfo(process.MainWindowHandle, ref info);
      rec = info.rcWindow;

      if (!Win32.IsIconic(process.MainWindowHandle))
      {
        return rec;
      }
      rec = Win32.GetPlacement(process.MainWindowHandle);
      return rec;
    }

    public static void WaitForProcessWindow(Process process)
    {
      var hWnd = FindWindow(null, process.MainWindowTitle);
      while (hWnd.ToInt32() == 0)
      {
        System.Threading.Thread.Sleep(500);
        hWnd = FindWindow(null, process.MainWindowTitle);
      }
    }

    #endregion Helper Methods
  }
}
