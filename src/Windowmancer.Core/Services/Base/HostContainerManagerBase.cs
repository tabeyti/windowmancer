using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windowmancer.Core.Extensions;
using Windowmancer.Core.Models;

namespace Windowmancer.Core.Services.Base
{
  public abstract class HostContainerManagerBase
  {
    public ObservableCollection<IHostContainerWindow> HostContainerWindows { get; set; }

    public ObservableCollection<HostContainerConfig> HostContainerConfigs => _profileManager.ActiveProfile.HostContainers;

    private readonly UserData _userData;
    private readonly ProfileManager _profileManager;

    protected HostContainerManagerBase(
      UserData userData,
      ProfileManager profileManager)
    {
      _userData = userData;
      _profileManager = profileManager;
      Initialize();
    }

    /// <summary>
    /// Applies the passed window config and process to the proper
    /// host container.
    /// </summary>
    /// <param name="windowConfig"></param>
    /// <param name="proces"></param>
    public void ApplyWindowConfig(WindowConfig windowConfig, Process proces)
    {
      var config = this.HostContainerConfigs.Find(hc =>
        hc.Name == windowConfig.HostContainerLayoutInfo.HostContainerId);

      if (null == config) return;

      ActivateHostContainer(config, false);
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
        CreateNewHostContainerWindow(config, enableEditor);
      hcw.ActivateWindow();
      hcw.Show();
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
        CreateNewHostContainerWindow(config);
      hcw.Show();
      hcw.ActivateWindow();
      hcw.DockNewProc(process);
      this.HostContainerWindows.Add(hcw);
    }

    public IHostContainerWindow GetHostContainerWindow(HostContainerConfig config)
    {
      return this.HostContainerWindows.Find(c => c.HostContainerConfig.Name == config.Name);
    }

    public bool IsHostContainerActive(HostContainerConfig config)
    {
      return this.HostContainerWindows.Any(c => c.HostContainerConfig.Name == config.Name);
    }

    private void Initialize()
    {
      this.HostContainerWindows = new ObservableCollection<IHostContainerWindow>();
    }
    
    protected abstract IHostContainerWindow CreateNewHostContainerWindow(
      HostContainerConfig config, 
      bool enableEditor = false);

    public void RemoveHostContainer(IHostContainerWindow hostContainer)
    {
      this.HostContainerWindows.Remove(hostContainer);
    }
  }
}
