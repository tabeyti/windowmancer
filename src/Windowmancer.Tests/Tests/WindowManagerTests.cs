using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Services;
using Windowmancer.Tests.Models;
using Windowmancer.Tests.Practices;
using Windowmancer.Tests.Tests.Base;
using Xunit;
using Xunit.Abstractions;

namespace Windowmancer.Tests.Tests
{
  /// <summary>
  /// Validates the API of the WindowManager service.
  /// </summary>
  public class WindowManagerTests : TestClassBase
  {
    public WindowManagerTests(ITestOutputHelper xunitTestOutputHelper) : base(xunitTestOutputHelper)
    {
    }

    [Theory]
    [Trait("Priority", "1")]
    [InlineData(50)]
    [InlineData(1)]
    [InlineData(99)]
    public async void WindowManagerTests_SetWindowOpacity(uint opacity)
    {
      // Create process window.
      var proc = AddResource(TestHelper.CreateWindowProcess());
      await Task.Delay(1000).ConfigureAwait(false);

      // Retrieve the current opacity of the window and verify it's at max.
      var origOpacity = WindowManager.GetWindowOpacityPercentage(proc.Process);
      Assert.Equal((uint)100, origOpacity);

      // Set the desired opacity and validate.
      WindowManager.SetWindowOpacityPercentage(proc.Process, opacity);
      var alter = WindowManager.GetWindowOpacityPercentage(proc.Process);
      Assert.Equal((uint)opacity, alter);
    }

    [Theory]
    [Trait("Priority", "1")]
    [InlineData(ProcessWindowState.Minimized)]
    [InlineData(ProcessWindowState.Maximized)]
    [InlineData(ProcessWindowState.Normal)]
    public async void WindowManagerTests_SetWindowState_ProcessWindowState(ProcessWindowState setState)
    {
      // Create process window.
      var proc = AddResource(TestHelper.CreateWindowProcess());
      await Task.Delay(1000).ConfigureAwait(false);

      // Verify it's in a normal state.
      var state = WindowManager.GetWindowState(proc.Process);
      Assert.Equal(ProcessWindowState.Normal, state);

      // Alter window placement and verify it's correct.
      WindowManager.SetWindowState(proc.Process, setState);
      await Task.Delay(1000).ConfigureAwait(false);
      state = WindowManager.GetWindowState(proc.Process);
      Assert.Equal(setState, state);

      // Show it in a normal position and verify.
      WindowManager.ShowWindowNormal(proc.Process);
      await Task.Delay(1000).ConfigureAwait(false);
      state = WindowManager.GetWindowState(proc.Process);
      Assert.Equal(ProcessWindowState.Normal, state);
    }

    [Fact]
    [Trait("Priority", "1")]
    public async void WindowManagerTests_SetWindowState_ProcessWindowRec()
    {
      // Create process window.
      var proc = AddResource(TestHelper.CreateWindowProcess());
      await Task.Delay(1000).ConfigureAwait(false);

      var originalRec = WindowManager.GetWindowRectCurrent(proc.Process);

      // Maximize window and verify the resulting rec.
      WindowManager.SetWindowState(proc.Process, ProcessWindowState.Maximized);
      await Task.Delay(1000).ConfigureAwait(false);
      var newRec = WindowManager.GetWindowRectCurrent(proc.Process);
      Assert.False(TestHelper.RecsMatch(originalRec, newRec));

      // Restore window size and verify it matches original rec.
      WindowManager.SetWindowState(proc.Process, ProcessWindowState.Normal);
      await Task.Delay(1000).ConfigureAwait(false);
      newRec = WindowManager.GetWindowRectCurrent(proc.Process);
      Assert.True(TestHelper.RecsMatch(originalRec, newRec));
    }

    [Fact]
    [Trait("Priority", "2")]
    public void WindowManagerTests_ApplyWindowLayout_Position()
    {
      var proc = AddResource(TestHelper.CreateWindowProcess());
      
      // Create a WindowInfo object from our create process.
      var windowInfo = WindowInfo.FromProcess(proc.Process);

      var originalLayoutInfo = windowInfo.MonitorLayoutInfo;
      var modifiedLayoutInfo = (MonitorLayoutInfo)originalLayoutInfo.Clone();

      // Modify position values.
      TestHelper.ModifyLayoutInfoPosition(modifiedLayoutInfo);

      // Apply modified layout via window manager.
      WindowManager.ApplyWindowLayout(modifiedLayoutInfo, proc.Process);

      // Get the process' current position and verify it matches our modified one.
      var newRec = WindowManager.GetWindowRectCurrent(proc.Process);
      Assert.Equal(modifiedLayoutInfo.PositionInfo.X, newRec.Left);
      Assert.Equal(modifiedLayoutInfo.PositionInfo.Y, newRec.Top);
    }

    [Fact]
    [Trait("Priority", "2")]
    public void WindowManagerTests_ApplyWindowLayout_Size()
    {
      var proc = AddResource(TestHelper.CreateWindowProcess());

      // Create a WindowInfo object from our process.
      var windowInfo = WindowInfo.FromProcess(proc.Process);

      var originalLayoutInfo = windowInfo.MonitorLayoutInfo;
      var modifiedLayoutInfo = (MonitorLayoutInfo)originalLayoutInfo.Clone();

      // Modify the size values.
      TestHelper.ModifyLayoutInfoSize(modifiedLayoutInfo);

      // Apply modified layout via window manager.
      WindowManager.ApplyWindowLayout(modifiedLayoutInfo, proc.Process);

      // Get the process' current position and verify it matches our modified one.
      var newRec = WindowManager.GetWindowRectCurrent(proc.Process);
      Assert.Equal(modifiedLayoutInfo.SizeInfo.Width, newRec.Width);
      Assert.Equal(modifiedLayoutInfo.SizeInfo.Height, newRec.Height);
    }

    [Trait("Priority", "2")]
    public void WindowManagerTests_ApplyWindowLayout_NoValueChange()
    {
      var proc = AddResource(TestHelper.CreateWindowProcess());

      // Create WindowInfo object from our create process.
      var windowInfo = WindowInfo.FromProcess(proc.Process);

      // Apply current layout, no change, via window manager.
      WindowManager.ApplyWindowLayout(windowInfo.MonitorLayoutInfo, proc.Process);

      // Get the process' current position and verify it is the same.
      var newRec = WindowManager.GetWindowRectCurrent(proc.Process);
      Assert.Equal(windowInfo.MonitorLayoutInfo.PositionInfo.X, newRec.Left);
      Assert.Equal(windowInfo.MonitorLayoutInfo.PositionInfo.Y, newRec.Top);
    }

    [Fact]
    [Trait("Priority", "3")]
    public void WindowManagerTests_RefreshProfile_LayoutInfo()
    {
      var windowManager = ServiceResolver.Resolve<WindowManager>();

      // Create three process windows.
      var procList = new List<TestProcessWrapper>
      {
        AddResource(TestHelper.CreateWindowProcess()),
        AddResource(TestHelper.CreateWindowProcess()),
        AddResource(TestHelper.CreateWindowProcess())
      };
      var procDict = new Dictionary<TestProcessWrapper, WindowInfo>();

      // Create a WindowInfo objects from our processes.
      var windowInfoList = new List<WindowInfo>();
      procList.ForEach(p =>
      {
        var w = WindowInfo.FromProcess(p.Process);
        windowInfoList.Add(w);
        procDict.Add(p, w);
      });

      // Create a profile object to hold our window info and assign
      // to the window manager.
      var profile = TestHelper.CreateNewProfile(windowInfoList);
      windowManager.ActiveProfile = profile;

      // Modify the size/positioning of each window info object.
      windowInfoList.ForEach(w =>
      {
        TestHelper.ModifyLayoutInfoSize(w.MonitorLayoutInfo);
        TestHelper.ModifyLayoutInfoPosition(w.MonitorLayoutInfo);
      });
      
      windowManager.RefreshProfile();

      // Retrieve new process information for each window info and validate.
      foreach (var kv in procDict)
      {
        var proc = kv.Key;
        var windowInfo = kv.Value;

        var newRec = WindowManager.GetWindowRectCurrent(proc.Process);
        Assert.Equal(windowInfo.MonitorLayoutInfo.SizeInfo.Width, newRec.Width);
        Assert.Equal(windowInfo.MonitorLayoutInfo.SizeInfo.Height, newRec.Height);
        Assert.Equal(windowInfo.MonitorLayoutInfo.PositionInfo.X, newRec.Left);
        Assert.Equal(windowInfo.MonitorLayoutInfo.PositionInfo.Y, newRec.Top);
      }
    }

    [Fact]
    [Trait("Priority", "3")]
    public async void WindowManagerTests_RefreshProfile_StylingInfo()
    {
      var windowManager = ServiceResolver.Resolve<WindowManager>();

      // Create three process windows.
      var procList = new List<TestProcessWrapper>
      {
        AddResource(TestHelper.CreateWindowProcess()),
        AddResource(TestHelper.CreateWindowProcess()),
        AddResource(TestHelper.CreateWindowProcess())
      };

      var procDict = new Dictionary<TestProcessWrapper, WindowInfo>();

      // Create a WindowInfo objects from our processes.
      var windowInfoList = new List<WindowInfo>();
      procList.ForEach(p =>
      {
        var w = WindowInfo.FromProcess(p.Process);
        windowInfoList.Add(w);
        procDict.Add(p, w);
      });

      // Create a profile object to hold our window info and assign
      // to the window manager.
      var profile = TestHelper.CreateNewProfile(windowInfoList);
      windowManager.ActiveProfile = profile;

      // Modify window styling values for each and refresh the profile.
      windowInfoList.ForEach(w => TestHelper.ModifyStylingInfoOpacity(w.StylingInfo));
      windowManager.RefreshProfile();

      await Task.Delay(500).ConfigureAwait(false);

      // Retrieve new process information for each window info and validate.
      foreach (var kv in procDict)
      {
        var proc = kv.Key;
        var windowInfo = kv.Value;
        var currentOpacity = WindowManager.GetWindowOpacityPercentage(proc.Process);
        Assert.Equal(windowInfo.StylingInfo.WindowOpacityPercentage, currentOpacity);
      }
    }

    [Fact]
    [Trait("Priority", "3")]
    public async void WindowManagerTests_ApplyWindowInfo_ApplyOnProcessStart()
    {
      var windowManager = ServiceResolver.Resolve<WindowManager>();

      // Create profile with window info object.
      var profile = TestHelper.CreateNewProfile();
      var windowInfo = profile.Windows.First();
      windowInfo.ApplyOnProcessStart = false;
      windowManager.ActiveProfile = profile;

      // Create associated process.
      var proc = AddResource(TestHelper.CreateWindowProcess(windowInfo.Name));
      await Task.Delay(1000).ConfigureAwait(false);
      var oldRec = WindowManager.GetWindowRectCurrent(proc.Process);

      // Apply window info via manager.
      windowManager.ApplyWindowInfo(proc.Process, true);
      Task.Delay(1000).Wait();

      // Verify process values are the same.
      var newRec = WindowManager.GetWindowRectCurrent(proc.Process);
      Assert.Equal(oldRec.Left, newRec.Left);
      Assert.Equal(oldRec.Top, newRec.Top);
      Assert.Equal(oldRec.Width, newRec.Width);
      Assert.Equal(oldRec.Height, newRec.Height);

      // Set flag to alter window process on startup and apply via manager.
      windowInfo.ApplyOnProcessStart = true;
      windowManager.ApplyWindowInfo(proc.Process, true);

      // Verify process values changed.
      newRec = WindowManager.GetWindowRectCurrent(proc.Process);
      Assert.Equal(windowInfo.MonitorLayoutInfo.PositionInfo.X, newRec.Left);
      Assert.Equal(windowInfo.MonitorLayoutInfo.PositionInfo.Y, newRec.Top);
      Assert.Equal(windowInfo.MonitorLayoutInfo.SizeInfo.Width, newRec.Width);
      Assert.Equal(windowInfo.MonitorLayoutInfo.SizeInfo.Height, newRec.Height);
    }

    #region Helper Methods

    #endregion Helper Methods
  }
}
