using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windowmancer.Extensions
{
  public static class ObservableCollectionExtensions
  {
    public static T Find<T>(this ObservableCollection<T> collection, Func<T, bool> check)
    {
      foreach (var t in collection)
      {
        if (check(t))
        {
          return t;
        }
      }
      return default(T);
    }

    public static void Remove<T>(this ObservableCollection<T> collection, Func<T, bool> check)
    {
      var item = default(T);
      var found = false;
      foreach (var t in collection)
      {
        if (check(t))
        {
          item = t;
          found = true;
          break;
        }
      }
      if (found)
      {
        collection.Remove(item);
      }
    }
  }
}
