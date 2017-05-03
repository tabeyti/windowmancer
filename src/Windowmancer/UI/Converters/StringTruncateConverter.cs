using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Windowmancer.UI.Converters
{
  public class StringTruncateConverter : IValueConverter
  {
    private static int _maxChars = 130;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value.GetType() != typeof(string))
      {
        return "null";
      }

      var text = (string) value;
      var diff = text.Length - _maxChars;
      if (diff <= 0) return value;

      var subStrLen = (_maxChars / 2);

      var first = text.Substring(0, subStrLen - 3);
      var second = text.Substring(text.Length - subStrLen);
      return $"{first}...{second}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return null;
    }
  }
}
