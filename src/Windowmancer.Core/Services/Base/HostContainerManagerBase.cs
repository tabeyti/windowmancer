/*
 * Show a box in ActiveWindowsDataGrid that has a list of processes
 * that the regexes of each window config is catching.
 */

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
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
    public void ApplyWindowConfig(WindowConfig windowConfig, Process process)
    {
      var config = this.HostContainerConfigs.Find(hc =>
        hc.Name == windowConfig.HostContainerLayoutInfo.HostContainerId);

      if (null == config) return;

      ActivateHostContainer(config, windowConfig, process);
    }
    
    /// <summary>
    /// Activates (creates) the window for the provided host container config.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="enableEditor"></param>
    public void ActivateHostContainer(HostContainerConfig config, bool enableEditor)
    {
      var hcw = GetOrCreateHostContainerWindow(config);
      hcw.ActivateWindow();
      hcw.Show();

      if (enableEditor)
      {
        hcw.OpenEditor();
      }
    }

    /// <summary>
    /// Activates (creates) the window for the provided host container config,
    /// adding the passed process and window config to the host container.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="windowConfig"></param>
    /// <param name="process"></param>
    public void ActivateHostContainer(HostContainerConfig config, WindowConfig windowConfig, Process process)
    {
      var hcw = GetOrCreateHostContainerWindow(config);
      hcw.Show();
      hcw.ActivateWindow();
      hcw.DockProc(process, windowConfig);
    }

    /// <summary>
    /// Checks if the <see cref="HostContainerConfig"/> associated
    /// host container window exists.
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public bool HostContainerWindowExists(HostContainerConfig config)
    {
      return this.HostContainerWindows.Any(hcw => hcw.HostContainerConfig.Name == config.Name);
    }

    public IHostContainerWindow GetOrCreateHostContainerWindow(HostContainerConfig config)
    {
      return HostContainerWindowExists(config)
        ? GetHostContainerWindow(config)
        : CreateNewHostContainerWindow(config);
    }
    
    /// <summary>
    /// Attempts to retrieve the <see cref="HostContainerConfig"/> associated
    /// host container window.
    /// </summary>
    /// <param name="config"></param>
    /// <returns>The associated host container window. If none, then returns null.</returns>
    public IHostContainerWindow GetHostContainerWindow(HostContainerConfig config)
    {
      return this.HostContainerWindows.Find(c => c.HostContainerConfig.Name == config.Name);
    }

    /// <summary>
    /// Validates there is an existing host container window instance.
    /// </summary>
    /// <param name="config"></param>
    /// <returns>True if active. False if not.</returns>
    public bool IsHostContainerWindowActive(HostContainerConfig config)
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
