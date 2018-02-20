using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windowmancer.Core.Extensions;
using Windowmancer.Core.Models;
using Windowmancer.Core.Services;
using Windowmancer.UI;
using Microsoft.Practices.Unity;
using System;

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

      ActivateHostContainer(config, true);
      return GetHostContainerWindow(config);
    }

    /// <summary>
    /// Activates (creates) the window for the provided host container config.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="enableEditor"></param>
    public void ActivateHostContainer(HostContainerConfig config, bool enableEditor)
    {
      var hcw = IsHostContainerActive(config) ? 
        GetHostContainerWindow(config) : 
        new HostContainer(config, enableEditor);
      hcw.Show();
      hcw.Activate();
      this.HostContainerWindows.Add(hcw);
    }

    /// <summary>
    /// Activates (creates) the window for the provided host container config,
    /// adding the passed process to the host container.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="process"></param>
    public void ActivateHostContainer(HostContainerConfig config, Process process)
    {
      var hcw = IsHostContainerActive(config) ? 
        GetHostContainerWindow(config) : 
        new HostContainer(config);
      hcw.Show();
      hcw.Activate();
      hcw.DockProc(process);
      this.HostContainerWindows.Add(hcw);
    }

    public void ActivateHostContainer(HostContainerConfig config, Process process, WindowConfig windowConfig)
    {
      var hcw = IsHostContainerActive(config) ?
        GetHostContainerWindow(config) :
        new HostContainer(config);
      hcw.Show();
      hcw.Activate();
      hcw.DockProc(process, windowConfig);
      this.HostContainerWindows.Add(hcw);
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

    internal void RemoveHostContainer(HostContainer hostContainer)
    {
      this.HostContainerWindows.Remove(hostContainer);
    }
  }
}
