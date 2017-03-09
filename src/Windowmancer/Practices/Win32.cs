using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Windowmancer.Practices
{
  public class Win32
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
    private struct WINDOWPLACEMENT
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
    static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

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
  }
}
