using System.Collections.Generic;

namespace Windowmancer.Core.Extensions
{
  public static class CollectionExtensions
  {
    public static void RemoveAll<T>(this IList<T> collection)
    {
      var count = collection.Count;
      for (var i = 0; i < count; ++i)
      {
        collection.RemoveAt(i);
      }
    }
  }
}
