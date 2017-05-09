using System;

namespace Windowmancer.Core.Extensions
{
  public static class BoolExtensions
  {
    public static void RunIfTrue(this bool flag, Action action)
    {
      if (flag)
      {
        action();
      }
    }
  }
}
