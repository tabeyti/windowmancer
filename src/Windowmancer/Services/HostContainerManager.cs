﻿using System.Windows;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Services;
using Windowmancer.UI;
using Windowmancer.Core.Services.Base;

namespace Windowmancer.Services
{
  public class HostContainerManager : HostContainerManagerBase
  {
    public HostContainerManager(
      ProfileManager profileManager) : base(profileManager)
    {
    }

    protected override IHostContainerWindow CreateNewHostContainerWindow(HostContainerConfig config, bool enableEditor = false)
    {
      HostContainer hc = null;
      Helper.Dispatcher.Invoke(() =>
      {
        hc = new HostContainer(config, enableEditor);
        hc.Closed += (sender, args) =>
        {
          this.HostContainerWindows.Remove(hc);
        };
        this.HostContainerWindows.Add(hc);
      });
      return hc;
    }
  }
}
