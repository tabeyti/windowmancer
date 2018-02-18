using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windowmancer.Core.Extensions;
using Windowmancer.Core.Models;
using Windowmancer.Core.Services;
using Windowmancer.UI;
using Microsoft.Practices.Unity;

namespace Windowmancer.Services
{
  public class HostContainerManager
  {
    public ObservableCollection<HostContainer> HostContainerWindows { get; set; }

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

    public HostContainer CreateNewHostContainer()
    {
      // Create new host container config.
      var config = new HostContainerConfig(DefaultName());

      RunHostContainer(config, true);
      return GetHostContainerWindow(config);
    }

    /// <summary>
    /// Activates (creates) the window for the provided host container config.
    /// </summary>
    /// <param name="config"></param>
    public void RunHostContainer(HostContainerConfig config, bool enableEditor)
    {
      var hostContainerWindow = new HostContainer(config, enableEditor);
      hostContainerWindow.Show();
      this.HostContainerWindows.Add(hostContainerWindow);
    }

    /// <summary>
    /// Activates (creates) the window for the provided host container config,
    /// adding the passed process to the host container.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="process"></param>
    public void RunHostContainer(HostContainerConfig config, Process process)
    {
      var hostContainerWindow = new HostContainer(config);
      hostContainerWindow.Show();
      hostContainerWindow.DockProc(process);
      this.HostContainerWindows.Add(hostContainerWindow);
    }

    public HostContainer GetHostContainerWindow(HostContainerConfig config)
    {
      return this.HostContainerWindows.Find(c => c.HostContainerConfig.Name == config.Name);
    }

    public bool IsHostContainerActive(HostContainerConfig config)
    {
      return this.HostContainerWindows.Any(c => c.HostContainerConfig.Name == config.Name);
    }

    private void Initialize()
    {
      this.HostContainerWindows = new ObservableCollection<HostContainer>();
    }

    private string DefaultName()
    {
      var configs = this.HostContainerConfigs;
      const string defaultContainerName = "My Container";
      var i = 0;
      var name = $"{defaultContainerName} 0";

      while (configs.Any(c => c.Name == name))
      {
        name = $"{defaultContainerName} {i++}";
      }

      return name;
    }
  }
}
