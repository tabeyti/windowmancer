using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windowmancer.Core.Models;
using Windowmancer.UI;

namespace Windowmancer.Services
{
  public class HostContainerManager
  {
    public Dictionary<string, HostContainer> HostContainers { get; set; } 

    private readonly UserData _userData;

    public HostContainerManager(UserData userData)
    {
      _userData = userData;
      Initialize();
    }

    private void Initialize()
    {
      this.HostContainers = new Dictionary<string, HostContainer>();
    }
  }
}
