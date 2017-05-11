using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windowmancer.Core.Services.Base
{
  public interface IDispatcher
  {
    void Invoke(Action callback);
  }
}
