using System;
using System.Diagnostics;
using Windowmancer.Core.Models;

namespace Windowmancer.Core.Services.Base
{
  public interface IHostContainerWindow
  {
    /// <summary>
    /// Shows the window. Implementation must be on
    /// or dispatched to the STA thread.
    /// </summary>
    void Show();

    /// <summary>
    /// Brings the window to the fore. Implementation must be on
    /// or dispatched to the STA thread.
    /// </summary>
    void ActivateWindow();

    /// <summary>
    /// Provides the user with the next available row/column index.
    /// </summary>
    /// <returns></returns>
    Tuple<uint, uint> NextDockProcRowColumn();

    /// <summary>
    /// Adds the process and associated window config to the container's
    /// next row/column, updating the window config's layout information
    /// accordingly.
    /// Implementation must be on or dispatched to the STA thread.
    /// </summary>
    void DockNewProc(Process process, WindowConfig windowConfig);

    /// <summary>
    /// Opens the host configuration editor of this container.
    /// </summary>
    void OpenEditor();

    /// <summary>
    /// Adds the process and associated window config to the container. 
    /// Implementation must be on or dispatched to the STA thread.
    /// </summary>
    /// <param name="process"></param>
    /// <param name="windowConfig"></param>
    void DockProc(Process process, WindowConfig windowConfig);

    HostContainerConfig HostContainerConfig { get; set; }
  }
}
