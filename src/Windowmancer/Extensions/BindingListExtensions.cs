using System;
using System.ComponentModel;

namespace Windowmancer.Extensions
{
  public static class BindingListExtensions
  {
    public static int FindIndex<T>(this BindingList<T> list, Func<T, bool> check)
    {
      for (var i = 0; i < list.Count; i++)
      {
        if (check(list[i]))
        {
          return i;
        }
      }
      return -1;
    }

    public static T Find<T>(this BindingList<T> list, Func<T, bool> check)
    {
      foreach (var t in list)
      {
        if (check(t))
        {
          return t;
        }
      }
      return default(T);
    }

  }
}
