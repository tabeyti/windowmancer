/* TODO:
 * Show a box in ActiveWindowsDataGrid that has a list of processes
 * that the regexes of each window config is catching.
 */

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Windowmancer.Core.Extensions;
using Windowmancer.Core.Models;

namespace Windowmancer.Core.Services.Base
{
  public abstract class HostContainerManagerBase
  {
    private ObservableCollection<IHostContainerWindow> HostContainerWindows { get; set; }
    public ObservableCollection<HostContainerConfig> HostContainerConfigs => _profileManager.ActiveProfile.HostContainers;

    private readonly ProfileManager _profileManager;

    private object _lock = new object();

    protected HostContainerManagerBase(ProfileManager profileManager)
    {
      _profileManager = profileManager;
      Initialize();
    }

    /// <summary>
    /// Applies the passed window config and process to the proper
    /// host container.
    /// </summary>
    /// <param name="windowConfig"></param>
    /// <param name="process"></param>
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
      hcw.Dock(process, windowConfig);
    }

    /// <summary>
    /// Checks if the <see cref="HostContainerConfig"/> associated
    /// host container window exists.
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public bool HostContainerWindowExists(HostContainerConfig config)
    {
      lock (_lock)
      {
        return this.HostContainerWindows.Any(hcw => hcw.HostContainerConfig.Name == config.Name);
      }
    }

    public IHostContainerWindow GetOrCreateHostContainerWindow(HostContainerConfig config)
    {
      return HostContainerWindowExists(config)
        ? GetHostContainerWindow(config)
        : CreateNewHostContainerWindow(config);
    }
    
    /// <summary>
    /// Attempts to retrieve the host container for the associated
    /// <see cref="HostContainerConfig"/>.
    /// </summary>
    /// <param name="config"></param>
    /// <returns>The associated host container window. If none, then returns null.</returns>
    public IHostContainerWindow GetHostContainerWindow(HostContainerConfig config)
    {
      lock (_lock)
      {
        return this.HostContainerWindows.Find(c => c.HostContainerConfig.Name == config.Name);
      }
    }

    /// <summary>
    /// Removes the provided window config from any active host container window.
    /// </summary>
    /// <param name="windowConfig"></param>
    public void RemoveFromHostContainerWindow(WindowConfig windowConfig)
    {
      var container = this.HostContainerWindows.Find(hcw => 
        WindowConfigManager.IsHostContainerMatch(windowConfig, hcw.HostContainerConfig));
      container?.UnDock(windowConfig);
    }

    /// <summary>
    /// Validates there is an existing host container window instance.
    /// </summary>
    /// <param name="config"></param>
    /// <returns>True if active. False if not.</returns>
    public bool IsHostContainerWindowActive(HostContainerConfig config)
    {
      lock (_lock)
      {
        return this.HostContainerWindows.Any(c => c.HostContainerConfig.Name == config.Name);
      }
    }

    /// <summary>
    /// Removes the provided host container window from the active list.
    /// </summary>
    /// <param name="hostContainer"></param>
    public void RemoveHostContainerWindow(IHostContainerWindow hostContainer)
    {
      lock (_lock)
      {
        this.HostContainerWindows.Remove(hostContainer);
      }
    }

    /// <summary>
    /// Adds the passed host container window instance to the list of
    /// active host container windows.
    /// </summary>
    /// <param name="hostContainer"></param>
    public void AddHostContainerWindow(IHostContainerWindow hostContainer)
    {
      lock (_lock)
      {
        this.HostContainerWindows.Add(hostContainer);
      }
    }

    private void Initialize()
    {
      lock (_lock)
      {
        this.HostContainerWindows = new ObservableCollection<IHostContainerWindow>();
      }

      // Ensure that when the user switches to a new profile,
      // that all active host containers are destroyed.
      _profileManager.OnActiveProfileChanging += (sender, args) =>
      {
        lock (_lock)
        {
          // TODO: HACKITY HACK! There is a asynchronous issue of closing
          // TODO: windows after a profile is changed from the system tray
          // TODO: (and maybe other places). Copying to a temp list prior
          // TODO: to iterating and closing seems to fix it. Don't hate me.
          var copy = new List<IHostContainerWindow>();
          this.HostContainerWindows.ForEach(h => copy.Add(h));
          copy.ForEach(hwc => hwc.Close());
        }
      };
    }
    
    /// <summary>
    /// Creates a new host container window object using the passed
    /// config.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="enableEditor"></param>
    /// <returns></returns>
    protected abstract IHostContainerWindow CreateNewHostContainerWindow(
      HostContainerConfig config, 
      bool enableEditor = false);
  }
}
