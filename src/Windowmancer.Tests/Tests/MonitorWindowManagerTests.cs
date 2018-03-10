﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Windowmancer.Core.Models;
using Windowmancer.Core.Services;
using Windowmancer.Tests.Models;
using Windowmancer.Tests.Practices;
using Windowmancer.Tests.Tests.Base;
using Xunit;
using Xunit.Abstractions;

namespace Windowmancer.Tests.Tests
{
  /// <summary>
  /// Validates the API of the MonitorWindowManager service.
  /// </summary>
  public class MonitorWindowManagerTests : TestClassBase
  {
    public MonitorWindowManagerTests(ITestOutputHelper xunitTestOutputHelper) : base(xunitTestOutputHelper)
    {
    }

    [Theory]
    [Trait("Priority", "1")]
    [InlineData(50)]
    [InlineData(1)]
    [InlineData(99)]
    public async void MonitorWindowManagerTests_SetWindowOpacity(uint opacity)
    {
      // Create process window.
      var proc = AddResource(TestHelper.CreateWindowProcess());
      await Task.Delay(1000).ConfigureAwait(false);

      // Retrieve the current opacity of the window and verify it's at max.
      var origOpacity = MonitorWindowManager.GetWindowOpacityPercentage(proc.Process);
      Assert.Equal((uint)100, origOpacity);

      // Set the desired opacity and validate.
      MonitorWindowManager.SetWindowOpacityPercentage(proc.Process, opacity);
      var alter = MonitorWindowManager.GetWindowOpacityPercentage(proc.Process);
      Assert.Equal((uint)opacity, alter);
    }

    [Theory]
    [Trait("Priority", "1")]
    [InlineData(ProcessWindowState.Minimized)]
    [InlineData(ProcessWindowState.Maximized)]
    [InlineData(ProcessWindowState.Normal)]
    public async void MonitorWindowManagerTests_SetWindowState_ProcessWindowState(ProcessWindowState setState)
    {
      // Create process window.
      var proc = AddResource(TestHelper.CreateWindowProcess());
      await Task.Delay(1000).ConfigureAwait(false);

      // Verify it's in a normal state.
      var state = MonitorWindowManager.GetWindowState(proc.Process);
      Assert.Equal(ProcessWindowState.Normal, state);

      // Alter window placement and verify it's correct.
      MonitorWindowManager.SetWindowState(proc.Process, setState);
      await Task.Delay(1000).ConfigureAwait(false);
      state = MonitorWindowManager.GetWindowState(proc.Process);
      Assert.Equal(setState, state);

      // Show it in a normal position and verify.
      MonitorWindowManager.ShowWindowNormal(proc.Process);
      await Task.Delay(1000).ConfigureAwait(false);
      state = MonitorWindowManager.GetWindowState(proc.Process);
      Assert.Equal(ProcessWindowState.Normal, state);
    }

    [Fact]
    [Trait("Priority", "1")]
    public async void MonitorWindowManagerTests_SetWindowState_ProcessWindowRec()
    {
      // Create process window.
      var proc = AddResource(TestHelper.CreateWindowProcess());
      await Task.Delay(1000).ConfigureAwait(false);

      var originalRec = MonitorWindowManager.GetWindowRectCurrent(proc.Process);

      // Maximize window and verify the resulting rec.
      MonitorWindowManager.SetWindowState(proc.Process, ProcessWindowState.Maximized);
      await Task.Delay(1000).ConfigureAwait(false);
      var newRec = MonitorWindowManager.GetWindowRectCurrent(proc.Process);
      Assert.False(TestHelper.RecsMatch(originalRec, newRec));

      // Restore window size and verify it matches original rec.
      MonitorWindowManager.SetWindowState(proc.Process, ProcessWindowState.Normal);
      await Task.Delay(1000).ConfigureAwait(false);
      newRec = MonitorWindowManager.GetWindowRectCurrent(proc.Process);
      Assert.True(TestHelper.RecsMatch(originalRec, newRec));
    }

    [Fact]
    [Trait("Priority", "2")]
    public void MonitorWindowManagerTests_ApplyWindowLayout_Position()
    {
      var proc = AddResource(TestHelper.CreateWindowProcess());
      
      // Create a WindowConfig object from our create process.
      var windowConfig = WindowConfig.FromProcess(proc.Process, WindowConfigLayoutType.Monitor);

      var originalLayoutInfo = windowConfig.MonitorLayoutInfo;
      var modifiedLayoutInfo = (MonitorLayoutInfo)originalLayoutInfo.Clone();

      // Modify position values.
      TestHelper.ModifyLayoutInfoPosition(modifiedLayoutInfo);

      // Apply modified layout via window manager.
      MonitorWindowManager.ApplyLayout(modifiedLayoutInfo, proc.Process);

      // Get the process' current position and verify it matches our modified one.
      var newRec = MonitorWindowManager.GetWindowRectCurrent(proc.Process);
      Assert.Equal(modifiedLayoutInfo.PositionInfo.X, newRec.Left);
      Assert.Equal(modifiedLayoutInfo.PositionInfo.Y, newRec.Top);
    }

    [Fact]
    [Trait("Priority", "2")]
    public void MonitorWindowManagerTests_ApplyWindowLayout_Size()
    {
      var proc = AddResource(TestHelper.CreateWindowProcess());

      // Create a WindowConfig object from our process.
      var windowConfig = WindowConfig.FromProcess(proc.Process, WindowConfigLayoutType.Monitor);

      var originalLayoutInfo = windowConfig.MonitorLayoutInfo;
      var modifiedLayoutInfo = (MonitorLayoutInfo)originalLayoutInfo.Clone();

      // Modify the size values.
      TestHelper.ModifyLayoutInfoSize(modifiedLayoutInfo);

      // Apply modified layout via window manager.
      MonitorWindowManager.ApplyLayout(modifiedLayoutInfo, proc.Process);

      // Get the process' current position and verify it matches our modified one.
      var newRec = MonitorWindowManager.GetWindowRectCurrent(proc.Process);
      Assert.Equal(modifiedLayoutInfo.SizeInfo.Width, newRec.Width);
      Assert.Equal(modifiedLayoutInfo.SizeInfo.Height, newRec.Height);
    }

    [Trait("Priority", "2")]
    public void MonitorWindowManagerTests_ApplyWindowLayout_NoValueChange()
    {
      var proc = AddResource(TestHelper.CreateWindowProcess());

      // Create WindowConfig object from our create process.
      var windowConfig = WindowConfig.FromProcess(proc.Process, WindowConfigLayoutType.Monitor);

      // Apply current layout, no change, via window manager.
      MonitorWindowManager.ApplyLayout(windowConfig.MonitorLayoutInfo, proc.Process);

      // Get the process' current position and verify it is the same.
      var newRec = MonitorWindowManager.GetWindowRectCurrent(proc.Process);
      Assert.Equal(windowConfig.MonitorLayoutInfo.PositionInfo.X, newRec.Left);
      Assert.Equal(windowConfig.MonitorLayoutInfo.PositionInfo.Y, newRec.Top);
    }

    [Fact]
    [Trait("Priority", "3")]
    public void MonitorWindowManagerTests_RefreshProfile_LayoutInfo()
    {
      var windowManager = ServiceResolver.Resolve<MonitorWindowManager>();

      // Create three process windows.
      var procList = new List<TestProcessWrapper>
      {
        AddResource(TestHelper.CreateWindowProcess()),
        AddResource(TestHelper.CreateWindowProcess()),
        AddResource(TestHelper.CreateWindowProcess())
      };
      var procDict = new Dictionary<TestProcessWrapper, WindowConfig>();

      // Create a WindowConfig objects from our processes.
      var windowConfigList = new List<WindowConfig>();
      procList.ForEach(p =>
      {
        var w = WindowConfig.FromProcess(p.Process, WindowConfigLayoutType.Monitor);
        windowConfigList.Add(w);
        procDict.Add(p, w);
      });

      // Create a profile object to hold our window info and assign
      // to the window manager.
      var profile = TestHelper.CreateNewProfile(windowConfigList);
      windowManager.ActiveProfile = profile;

      // Modify the size/positioning of each window info object.
      windowConfigList.ForEach(w =>
      {
        TestHelper.ModifyLayoutInfoSize(w.MonitorLayoutInfo);
        TestHelper.ModifyLayoutInfoPosition(w.MonitorLayoutInfo);
      });
      
      windowManager.RefreshProfile();

      // Retrieve new process information for each window info and validate.
      foreach (var kv in procDict)
      {
        var proc = kv.Key;
        var windowConfig = kv.Value;

        var newRec = MonitorWindowManager.GetWindowRectCurrent(proc.Process);
        Assert.Equal(windowConfig.MonitorLayoutInfo.SizeInfo.Width, newRec.Width);
        Assert.Equal(windowConfig.MonitorLayoutInfo.SizeInfo.Height, newRec.Height);
        Assert.Equal(windowConfig.MonitorLayoutInfo.PositionInfo.X, newRec.Left);
        Assert.Equal(windowConfig.MonitorLayoutInfo.PositionInfo.Y, newRec.Top);
      }
    }

    [Fact]
    [Trait("Priority", "3")]
    public async void MonitorWindowManagerTests_RefreshProfile_StylingInfo()
    {
      var windowManager = ServiceResolver.Resolve<MonitorWindowManager>();

      // Create three process windows.
      var procList = new List<TestProcessWrapper>
      {
        AddResource(TestHelper.CreateWindowProcess()),
        AddResource(TestHelper.CreateWindowProcess()),
        AddResource(TestHelper.CreateWindowProcess())
      };

      var procDict = new Dictionary<TestProcessWrapper, WindowConfig>();

      // Create a WindowConfig objects from our processes.
      var windowConfigList = new List<WindowConfig>();
      procList.ForEach(p =>
      {
        var w = WindowConfig.FromProcess(p.Process, WindowConfigLayoutType.Monitor);
        windowConfigList.Add(w);
        procDict.Add(p, w);
      });

      // Create a profile object to hold our window info and assign
      // to the window manager.
      var profile = TestHelper.CreateNewProfile(windowConfigList);
      windowManager.ActiveProfile = profile;

      // Modify window styling values for each and refresh the profile.
      windowConfigList.ForEach(w => TestHelper.ModifyStylingInfoOpacity(w.StylingInfo));
      windowManager.RefreshProfile();

      await Task.Delay(500).ConfigureAwait(false);

      // Retrieve new process information for each window info and validate.
      foreach (var kv in procDict)
      {
        var proc = kv.Key;
        var windowConfig = kv.Value;
        var currentOpacity = MonitorWindowManager.GetWindowOpacityPercentage(proc.Process);
        Assert.Equal(windowConfig.StylingInfo.WindowOpacityPercentage, currentOpacity);
      }
    }

    [Fact]
    [Trait("Priority", "3")]
    public async void MonitorWindowManagerTests_ApplyWindowConfig_ApplyOnProcessStart()
    {
      var windowManager = ServiceResolver.Resolve<MonitorWindowManager>();

      // Create profile with window info object.
      var profile = TestHelper.CreateNewProfile();
      var windowConfig = profile.Windows.First();
      windowConfig.ApplyOnProcessStart = false;
      windowManager.ActiveProfile = profile;

      // Create associated process.
      var proc = AddResource(TestHelper.CreateWindowProcess(windowConfig.Name));
      await Task.Delay(1000).ConfigureAwait(false);
      var oldRec = MonitorWindowManager.GetWindowRectCurrent(proc.Process);

      // Apply window info via manager.
      windowManager.ApplyWindowConfig(proc.Process, true);
      Task.Delay(1000).Wait();

      // Verify process values are the same.
      var newRec = MonitorWindowManager.GetWindowRectCurrent(proc.Process);
      Assert.Equal(oldRec.Left, newRec.Left);
      Assert.Equal(oldRec.Top, newRec.Top);
      Assert.Equal(oldRec.Width, newRec.Width);
      Assert.Equal(oldRec.Height, newRec.Height);

      // Set flag to alter window process on startup and apply via manager.
      windowConfig.ApplyOnProcessStart = true;
      windowManager.ApplyWindowConfig(proc.Process, true);

      // Verify process values changed.
      newRec = MonitorWindowManager.GetWindowRectCurrent(proc.Process);
      Assert.Equal(windowConfig.MonitorLayoutInfo.PositionInfo.X, newRec.Left);
      Assert.Equal(windowConfig.MonitorLayoutInfo.PositionInfo.Y, newRec.Top);
      Assert.Equal(windowConfig.MonitorLayoutInfo.SizeInfo.Width, newRec.Width);
      Assert.Equal(windowConfig.MonitorLayoutInfo.SizeInfo.Height, newRec.Height);
    }

    #region Helper Methods

    #endregion Helper Methods
  }
}