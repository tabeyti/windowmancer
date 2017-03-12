using System;
using System.Drawing;
using System.Runtime.InteropServices;
using static Windowmancer.Practices.Win32;

namespace Windowmancer.Practices
{
  public static class Win32
  {
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
      public int Left;
      public int Top;
      public int Right;
      public int Bottom;

      public int Width
      {
        get { return this.Right - this.Left; }
        set { this.Right = value + this.Left; }
      }

      public int Height
      {
        get { return this.Bottom - this.Top; }
        set { this.Bottom = value + this.Top; }
      }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWINFO
    {
      public uint cbSize;
      public Win32.RECT rcWindow;
      public Win32.RECT rcClient;
      public uint dwStyle;
      public uint dwExStyle;
      public uint dwWindowStatus;
      public uint cxWindowBorders;
      public uint cyWindowBorders;
      public ushort atomWindowType;
      public ushort wCreatorVersion;

      public WINDOWINFO(bool? filler)
       : this()   // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
      {
        cbSize = (uint)(Marshal.SizeOf(typeof(WINDOWINFO)));
      }
    }

    /// <summary>
    /// See MSDN WINDOWPLACEMENT Structure http://msdn.microsoft.com/en-us/library/ms632611(v=VS.85).aspx
    /// </summary>
    public struct WINDOWPLACEMENT
    {
      public int length;
      public int flags;
      public int showCmd;
      public Point ptMinPosition;
      public Point ptMaxPosition;
      public RECT rcNormalPosition;
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetWindowRect(IntPtr hWnd, ref Win32.RECT Rect);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr FindWindow(string strClassName, string strWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsIconic(IntPtr hWnd);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("user32.dll")]
    public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);

    [DllImport("user32.DLL")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("User32.dll")]

    public static extern IntPtr GetWindowDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

    /// <summary>
    /// Gets the window placement of the specified window in Normal state.
    /// </summary>
    /// <param name="handle">The handle.</param>
    /// <returns></returns>
    public static Win32.RECT GetPlacement(IntPtr handle)
    {
      var placement = new WINDOWPLACEMENT();
      placement.length = System.Runtime.InteropServices.Marshal.SizeOf(placement);
      GetWindowPlacement(handle, ref placement);
      return placement.rcNormalPosition;
    }

    /// <summary>Enumeration of the different ways of showing a window using
    /// ShowWindow</summary>
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
  }
}
