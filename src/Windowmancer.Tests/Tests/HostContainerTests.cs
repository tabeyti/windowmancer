using Windowmancer.Tests.Tests.Base;
using Xunit;
using Xunit.Abstractions;

namespace Windowmancer.Tests.Tests
{
  /// <summary>
  /// Validates the API of the MonitorWindowManager service.
  /// </summary>
  public class HostContainerTests : TestClassBase
  {
    public HostContainerTests(ITestOutputHelper xunitTestOutputHelper) : base(xunitTestOutputHelper)
    {
    }

    [Fact]
    [Trait("Priority", "1")]
    public async void HostContainerTests_Smoke()
    {
      


      //// Create process window.
      //var proc = AddResource(TestHelper.CreateWindowProcess());
      //await Task.Delay(1000).ConfigureAwait(false);

      //// Retrieve the current opacity of the window and verify it's at max.
      //var origOpacity = MonitorWindowManager.GetWindowOpacityPercentage(proc.Process);
      //Assert.Equal((uint)100, origOpacity);

      //// Set the desired opacity and validate.
      //MonitorWindowManager.SetWindowOpacityPercentage(proc.Process, opacity);
      //var alter = MonitorWindowManager.GetWindowOpacityPercentage(proc.Process);
      //Assert.Equal((uint)opacity, alter);
    }

    #region Helper Methods

    #endregion Helper Methods
  }
}
