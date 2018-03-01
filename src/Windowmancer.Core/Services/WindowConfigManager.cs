using System;
using System.Diagnostics;
using Windowmancer.Core.Extensions;
using Windowmancer.Core.Models;
using Windowmancer.Core.Services.Base;

namespace Windowmancer.Core.Services
{
  public class WindowConfigManager
  {
    private ProfileManager ProfileManager { get; set; }

    private Profile ActiveProfile => this.ProfileManager.ActiveProfile;
    private HostContainerManagerBase HostContainerManager { get; set; }
    private MonitorWindowManager MonitorWindowManager { get; set; }

    public WindowConfigManager(
      ProfileManager profileManager,
      MonitorWindowManager monitorWindowManager,
      HostContainerManagerBase hostContainerManager)
    {
      this.ProfileManager = profileManager;
      this.MonitorWindowManager = monitorWindowManager;
      this.HostContainerManager = hostContainerManager;
    }

    /// <summary>
    /// Applies an existing window config object to the provided process 
    /// window. If no window config exists for the passed process window,
    /// nothing is done.
    /// </summary>
    /// <param name="process"></param>
    /// <param name="newProcess"></param>
    public void ApplyWindowConfig(Process process, bool newProcess = false)
    {
      var windowConfig = this.ActiveProfile?.Windows.Find(p => p.IsMatch(process));
      if (windowConfig == null) return;
      if (newProcess && !windowConfig.ApplyOnProcessStart) return;

      switch (windowConfig.LayoutType)
      {
        case WindowConfigLayoutType.Monitor:
          this.MonitorWindowManager.ApplyWindowConfig(windowConfig, process);
          break;
        case WindowConfigLayoutType.HostContainer:
          this.HostContainerManager.ApplyWindowConfig(windowConfig, process);
          break;
        default:
          return;
      }
    }
  }
}
