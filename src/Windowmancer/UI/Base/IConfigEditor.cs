using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windowmancer.UI.Base
{
  public interface IConfigEditor<T>
  {
    Action OnClose { get; set; }

    Action<T> OnSave { get; set; }
  }
}
