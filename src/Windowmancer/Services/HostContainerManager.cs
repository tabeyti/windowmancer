using Windowmancer.Core.Models;
using Windowmancer.Core.Services;
using Windowmancer.UI;
using Windowmancer.Core.Services.Base;

namespace Windowmancer.Services
{
  public class HostContainerManager : HostContainerManagerBase
  {
    public HostContainerManager(
      UserData userData,
      ProfileManager profileManager) : base(userData, profileManager)
    {
    }

    protected override IHostContainerWindow CreateNewHostContainerWindow(HostContainerConfig config, bool enableEditor = false)
    {
      return new HostContainer(config, enableEditor);
    }
  }
}
