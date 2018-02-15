using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windowmancer.Core.Models;
using Windowmancer.Core.Services;
using Windowmancer.UI;

namespace Windowmancer.Services
{
  public class HostContainerManager
  {
    public Dictionary<string, HostContainer> HostContainerWindows { get; set; }

    public ObservableCollection<HostContainerConfig> HostContainerConfigs => _profileManager.ActiveProfile.HostContainers;

    private readonly UserData _userData;
    private readonly ProfileManager _profileManager;

    public HostContainerManager(
      UserData userData,
      ProfileManager profileManager)
    {
      _userData = userData;
      _profileManager = profileManager;
      Initialize();
    }

    /// <summary>
    /// Activates (creates) the window for the provided host container config.
    /// </summary>
    /// <param name="config"></param>
    public void ActivateHostContainer(HostContainerConfig config)
    {
      var hostContainerWindow = new HostContainer(config);
      hostContainerWindow.Show();
    }

    /// <summary>
    /// Activates (creates) the window for the provided host container config,
    /// adding the passed process to the host container.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="process"></param>
    public void ActivateHostContainer(HostContainerConfig config, Process process)
    {
      var hostContainerWindow = new HostContainer(config);
      hostContainerWindow.Show();
      hostContainerWindow.DockProc(process);
    }

    private void Initialize()
    {
      this.HostContainerWindows = new Dictionary<string, HostContainer>();
    }
  }
}
