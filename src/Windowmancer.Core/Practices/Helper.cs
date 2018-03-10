using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Windowmancer.Core.Services.Base;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Windowmancer.Core.Practices
{
  public class Helper
  {
    private static IDispatcher _dispatcher;
    public static IDispatcher Dispatcher
    {
      get
      {
        if (null == _dispatcher)
        {
          throw new Exception("Helper - Must set a IDispatcher before use.");
        }
        return _dispatcher;
      }
      set => _dispatcher = value;
    }

    private static IUnityContainer _serviceResolver;

    public static IUnityContainer ServiceResolver
    {
      get
      {
        if (null == _serviceResolver)
        {
          throw new Exception("Helper - Must set a ServiceResolver before use.");
        }
        return _serviceResolver;
      }
      set => _serviceResolver = value;
    }

    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

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

    public static ImageSource GetBlankScreenShot()
    {
      return BitmapImage.Create(
        2,
        2,
        96,
        96,
        PixelFormats.Indexed1,
        new BitmapPalette(new List<System.Windows.Media.Color> { Colors.Transparent }),
        new byte[] { 0, 0, 0, 0 },
        1);
    }

    /// <summary>
    /// Takes a screen shot of the process window and
    /// returns it converted to an image source.
    /// </summary>
    /// <param name="proc"></param>
    /// <returns></returns>
    public static ImageSource ScreenShotProcessWindow(Process proc)
    {
      if (null == proc)
      {
        throw new Exception($"{nameof(ScreenShotProcessWindow)} - Process null.");
      }

      var rec = new Win32.Win32_Rect();
      Win32.GetWindowRect(proc.MainWindowHandle, ref rec);

      var bmp = new Bitmap(rec.Width, rec.Height, PixelFormat.Format32bppArgb);
      var gfxBmp = Graphics.FromImage(bmp);
      var hdcBitmap = gfxBmp.GetHdc();

      Win32.PrintWindow(proc.MainWindowHandle, hdcBitmap, 0);

      gfxBmp.ReleaseHdc(hdcBitmap);
      gfxBmp.Dispose();

      return ImageSourceForBitmap(bmp);
    }

    public static Icon GetSmallIcon(Icon icon)
    {
      if (null == icon)
      {
        throw new Exception($"{nameof(GetSmallIcon)} - Icon null.");
      }

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
      if (null == icon)
      {
        throw new Exception($"{nameof(GetSmallIconAsBitmap)} - Icon null.");
      }

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

    public static dynamic GetConfig(string configFileName)
    {
      var assembly = Assembly.GetCallingAssembly();
      var resourceName = $"Windowmancer.{configFileName}";
      var content = string.Empty;
      
      using (var stream = assembly.GetManifestResourceStream(resourceName))
      using (var reader = new StreamReader(stream))
      {
        content = reader.ReadToEnd();
      }
      return JsonConvert.DeserializeObject(content);
    }

    public static void DispatcherInvoke(Action callback)
    {
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
