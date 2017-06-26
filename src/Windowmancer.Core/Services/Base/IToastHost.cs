using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windowmancer.Core.Services.Base
{
  public interface IToastHost
  {
    void ShowMessageToast(string message);
    void ShowItemMessageToast(string itemName, string message);
  }
}
