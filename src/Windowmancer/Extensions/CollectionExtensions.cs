using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Windowmancer.Extensions
{
  public static class CollectionExtensions
  {
    public static void RemoveAll(this UIElementCollection collection)
    {
      var count = collection.Count;
      for (var i = 0; i < count; ++i)
      {
        collection.RemoveAt(i);
      }
    }
  }
}
