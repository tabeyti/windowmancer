﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WindowmancerWPF.Practices
{
  public class Helper
  {
    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

    /// <summary>
    /// Retrieves the provided process' window rectangel.
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public static Win32.RECT GetProcessWindowRec(Process process)
    {
      var info = new Win32.WINDOWINFO();
      var rec = new Win32.RECT();

      Win32.GetWindowInfo(process.MainWindowHandle, ref info);
      rec = info.rcWindow;

      if (!Win32.IsIconic(process.MainWindowHandle))
      {
        return rec;
      }
      rec = Win32.GetPlacement(process.MainWindowHandle);
      return rec;
    }

    public static ImageSource ImageSourceForBitmap(Bitmap bmp)
    {
      var handle = bmp.GetHbitmap();
      try
      {
        return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
      }
      finally { DeleteObject(handle); }
    }

    public static ImageSource ImageSourceForIcon(Icon icon)
    {
      return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
    }

    public static Icon GetProcessIcon(Process process)
    {
      System.Drawing.Icon ico = null;
      try
      {
        ico = System.Drawing.Icon.ExtractAssociatedIcon(process.MainModule.FileName);
        ico = GetSmallIcon(ico);
      }
      catch
      {
        // ignore.
      }
      return ico;
    }

    public static ImageSource GetProcessIconImageSource(Process process)
    {
      ImageSource ico = null;
      try
      {
        var original = System.Drawing.Icon.ExtractAssociatedIcon(process.MainModule.FileName);
        ico = GetSmallIconAsBitmap(original);
      }
      catch
      {
        // ignore.
      }
      return ico;
    }

    public static Icon GetSmallIcon(Icon icon)
    {
      var iconSize = SystemInformation.SmallIconSize;
      var bitmap = new Bitmap(iconSize.Width, iconSize.Height);

      using (var g = Graphics.FromImage(bitmap))
      {
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.DrawImage(icon.ToBitmap(), new System.Drawing.Rectangle(System.Drawing.Point.Empty, iconSize));
      }

      return Icon.FromHandle(bitmap.GetHicon());
    }

    public static ImageSource GetSmallIconAsBitmap(Icon icon)
    {
      var iconSize = SystemInformation.SmallIconSize;
      var bitmap = new Bitmap(iconSize.Width, iconSize.Height);

      using (var g = Graphics.FromImage(bitmap))
      {
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.DrawImage(icon.ToBitmap(), new System.Drawing.Rectangle(System.Drawing.Point.Empty, iconSize));
      }

      var hBitmap = bitmap.GetHbitmap();
      ImageSource retval;

      try
      {
        retval = Imaging.CreateBitmapSourceFromHBitmap(
          hBitmap,
          IntPtr.Zero, 
          new Int32Rect(0, 0, bitmap.Width, bitmap.Height),
          BitmapSizeOptions.FromEmptyOptions());
        return retval;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      finally
      {
        DeleteObject(hBitmap);
      }

      return null;
    }

    public static int GetGreatestCommonDivisor(int a, int b)
    {
      while (true)
      {
        if (b == 0) return a;
        var a1 = a;
        a = b;
        b = a1 % b;
      }
    }
  }
}