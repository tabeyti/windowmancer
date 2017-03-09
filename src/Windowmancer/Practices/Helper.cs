using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windowmancer.Practices
{
  public class Helper
  {
    public static Icon GetSmallIcon(Icon icon)
    {
      var iconSize = SystemInformation.SmallIconSize;
      var bitmap = new Bitmap(iconSize.Width, iconSize.Height);

      using (var g = Graphics.FromImage(bitmap))
      {
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.DrawImage(icon.ToBitmap(), new Rectangle(Point.Empty, iconSize));
      }

      return Icon.FromHandle(bitmap.GetHicon());
    }
  }
}
