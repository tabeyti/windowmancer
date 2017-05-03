using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windowmancer.Extensions
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
