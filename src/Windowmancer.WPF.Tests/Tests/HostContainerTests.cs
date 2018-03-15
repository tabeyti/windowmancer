using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Services.Base;
using Windowmancer.WPF.Tests.Models;
using Windowmancer.WPF.Tests.Practices;
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

    [Theory]
    [Trait("Priority", "1")]
    [InlineData("hc_mass.json")]
    public async Task HostContainerTests_CreateClose(string userDataFile)
    {
      TestHelper.OverwriteUserData(userDataFile);

      var proc = TestHelper.CreateWindowmancer();

      await Task.Delay(10000).ConfigureAwait(false);

      CreateTestWindows(10, 1);

      await Task.Delay(20000).ConfigureAwait(false);
      proc.Kill();
    }

    #region Helper Methods

    private List<TestProcessWrapper> CreateTestWindows(int num, int startingIndex)
    {
      var list = new List<TestProcessWrapper>();
      for (int i = startingIndex; i < num + startingIndex; ++i)
      {
        list.Add(TestHelper.CreateTestWindowProcess($"TestWindow{i:00}"));
      }
      return list;
    }

    private bool IsWindowActive(TestProcessWrapper proc)
    {
      var allProcceses = System.Diagnostics.Process.GetProcesses();
      return allProcceses.Any(p => p.MainWindowTitle == proc.MainWindowTitle);
    }

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

    public class TestDispatcher : IDispatcher
    {
      public void Invoke(Action callback)
      {
        callback();
      }

      #endregion

    }
  }
}
