using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windowmancer.Core.Models;
using Windowmancer.UI;

namespace Windowmancer.Extensions
{
  public static class DisplayContainerExtensions
  {
    public static DisplayContainer FromWindowHost(WindowHostContainer window)
    {
      return new DisplayContainer(
        window.Title, 0, 0, 
        (int) window.ActualWidth, 
        (int) window.ActualHeight, 
        window.Rows, 
        window.Columns);
    }
  }
}
