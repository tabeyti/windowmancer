using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Windowmancer.Core.Extensions;
using Windowmancer.Core.Services.Base;
using Color = System.Windows.Media.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Windows.Point;

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

    public static ImageSource GetProcessIconImageSource(Process process)
    {
      ImageSource ico = null;
      try
      {
        ico = GetSmallIconAsBitmap(process.GetIcon());
      }
      catch
      {
        // ignore.
      }
      return ico;
    }


    /// <summary>
    /// Returns a blank image source, filled with a default solid color,
    /// with the passed text transposed on the image.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static ImageSource GetBlankScreenShot(string text)
    {
      var source = GetBlankScreenShot();

      var imageRect = new Rect(0, 0, 440, 240);
      var centerPoint = new Point(imageRect.Width/2, imageRect.Height/2);

      var formattedText = new FormattedText(
        text, 
        CultureInfo.InvariantCulture, 
        System.Windows.FlowDirection.LeftToRight,
        new Typeface("Consolas"), 
        24, 
        System.Windows.Media.Brushes.Black);

      var visual = new DrawingVisual();
      using (var dc = visual.RenderOpen())
      {
        dc.DrawImage(source, imageRect);
        dc.DrawText(formattedText, new System.Windows.Point(centerPoint.X - formattedText.WidthIncludingTrailingWhitespace / 2, centerPoint.Y - formattedText.Height));
      }
      return new DrawingImage(visual.Drawing);
    }

    /// <summary>
    /// Returns a blank image source, filled with a default solid color.
    /// </summary>
    /// <returns></returns>
    public static ImageSource GetBlankScreenShot()
    {
      return GetBlankScreenShot(Colors.Coral);
    }

    /// <summary>
    /// Returns a blank image source, filled with a provided solid color.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static ImageSource GetBlankScreenShot(Color color)
    {
      return BitmapSource.Create(
        2,
        2,
        96,
        96,
        PixelFormats.Indexed1,
        new BitmapPalette(new List<Color> { color }),
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

      try
      {
        ImageSource retval = Imaging.CreateBitmapSourceFromHBitmap(
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

      if (null == process) return;

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
