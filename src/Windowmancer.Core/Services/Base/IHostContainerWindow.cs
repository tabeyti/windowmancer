using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windowmancer.Core.Models;

namespace Windowmancer.Core.Services.Base
{
  public interface IHostContainerWindow
  {
    void Show();
    void ActivateWindow();
    void DockNewProc(Process process);
    HostContainerConfig HostContainerConfig { get; set; }
  }
}
