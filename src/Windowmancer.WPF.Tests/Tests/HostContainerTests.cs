﻿using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using Windowmancer.UI;
using Windowmancer.WPF.Tests.Tests.Base;
using Xunit;
using Xunit.Abstractions;

namespace Windowmancer.WPF.Tests.Tests
{
  /// <summary>
  /// Validates the API of the MonitorWindowManager service.
  /// </summary>
  public class HostContainerTests : TestClassBase
  {
    public HostContainerTests(ITestOutputHelper xunitTestOutputHelper) : base(xunitTestOutputHelper)
    {
      Helper.Dispatcher = new TestDispatcher();
    }

    [StaFact]
    [Trait("Priority", "1")]
    public void HostContainerTests_CreateClose()
    {
      var hcc = CreateHostContainerConfig();
      
      var window = new HostContainer(hcc);
      window.Show();

      //// Create process window.
      //var proc = AddResource(TestHelper.CreateWindowProcess());
      //await Task.Delay(1000).ConfigureAwait(false);

      //// Retrieve the current opacity of the window and verify it's at max.
      //var origOpacity = MonitorWindowManager.GetWindowOpacityPercentage(proc.Process);
      //Assert.Equal((uint) 100, origOpacity);

      //// Set the desired opacity and validate.
      //MonitorWindowManager.SetWindowOpacityPercentage(proc.Process, opacity);
      //var alter = MonitorWindowManager.GetWindowOpacityPercentage(proc.Process);
      //Assert.Equal((uint) opacity, alter);
    }

#region Helper Methods

    private readonly uint _containerCounter = 1;
    private HostContainerConfig CreateHostContainerConfig()
    {
      return new HostContainerConfig
      {
        Name = $"HostContainer{_containerCounter}",
        Rows = 3,
        Columns = 2,
        Id = "",
        IsActive = false,
      };
    }
#endregion

  }
}
