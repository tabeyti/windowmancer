using System;
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

    [Fact]
    [Trait("Priority", "1")]
    public void WindowManagerTests_ApplyWindowLayout_Position()
    {
      var proc = AddResource(TestHelper.CreateWindowProcess());
      
      // Create a WindowInfo object from our create process.
      var windowInfo = WindowInfo.FromProcess(proc.Process);

      var originalLayoutInfo = windowInfo.LayoutInfo;
      var modifiedLayoutInfo = (WindowLayoutInfo)originalLayoutInfo.Clone();

      // Modify position values.
      TestHelper.ModifyLayoutInfoPosition(modifiedLayoutInfo);

      // Apply modified layout via window manager.
      WindowManager.ApplyWindowLayout(modifiedLayoutInfo, proc.Process);

      // Get the process' current position and verify it matches our modified one.
      var newRec = Win32.GetProcessWindowRec(proc.Process);
      Assert.Equal(modifiedLayoutInfo.PositionInfo.X, newRec.Left);
      Assert.Equal(modifiedLayoutInfo.PositionInfo.Y, newRec.Top);
    }

    [Fact]
    [Trait("Priority", "1")]
    public void WindowManagerTests_ApplyWindowLayout_Size()
    {
      var proc = AddResource(TestHelper.CreateWindowProcess());

      // Create a WindowInfo object from our process.
      var windowInfo = WindowInfo.FromProcess(proc.Process);

      var originalLayoutInfo = windowInfo.LayoutInfo;
      var modifiedLayoutInfo = (WindowLayoutInfo)originalLayoutInfo.Clone();

      // Modify the size values.
      TestHelper.ModifyLayoutInfoSize(modifiedLayoutInfo);

      // Apply modified layout via window manager.
      WindowManager.ApplyWindowLayout(modifiedLayoutInfo, proc.Process);

      // Get the process' current position and verify it matches our modified one.
      var newRec = Win32.GetProcessWindowRec(proc.Process);
      Assert.Equal(modifiedLayoutInfo.SizeInfo.Width, newRec.Width);
      Assert.Equal(modifiedLayoutInfo.SizeInfo.Height, newRec.Height);
    }

    [Trait("Priority", "1")]
    public void WindowManagerTests_ApplyLayout_NoValueChange()
    {
      var proc = AddResource(TestHelper.CreateWindowProcess());

      // Create WindowInfo object from our create process.
      var windowInfo = WindowInfo.FromProcess(proc.Process);

      // Apply current layout, no change, via window manager.
      WindowManager.ApplyWindowLayout(windowInfo.LayoutInfo, proc.Process);

      // Get the process' current position and verify it is the same.
      var newRec = Win32.GetProcessWindowRec(proc.Process);
      Assert.Equal(windowInfo.LayoutInfo.PositionInfo.X, newRec.Left);
      Assert.Equal(windowInfo.LayoutInfo.PositionInfo.Y, newRec.Top);
    }

    [Fact]
    [Trait("Priority", "2")]
    public void WindowManagerTests_RefreshProfile_SizeAndPosition()
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
        TestHelper.ModifyLayoutInfoSize(w.LayoutInfo);
        TestHelper.ModifyLayoutInfoPosition(w.LayoutInfo);
      });
      
      windowManager.RefreshProfile();

      // Retrieve new process information for each window info and validate.
      foreach (var kv in procDict)
      {
        var proc = kv.Key;
        var windowInfo = kv.Value;

        var newRec = Win32.GetProcessWindowRec(proc.Process);
        Assert.Equal(windowInfo.LayoutInfo.SizeInfo.Width, newRec.Width);
        Assert.Equal(windowInfo.LayoutInfo.SizeInfo.Height, newRec.Height);
        Assert.Equal(windowInfo.LayoutInfo.PositionInfo.X, newRec.Left);
        Assert.Equal(windowInfo.LayoutInfo.PositionInfo.Y, newRec.Top);
      }
    }

    [Fact]
    [Trait("Priority", "2")]
    public void WindowManagerTests_ApplyWindowInfo_ApplyOnProcessStart()
    {
      var windowManager = ServiceResolver.Resolve<WindowManager>();

      // Create profile with window info object.
      var profile = TestHelper.CreateNewProfile();
      var windowInfo = profile.Windows.First();
      windowInfo.ApplyOnProcessStart = false;
      windowManager.ActiveProfile = profile;

      // Create associated process.
      var proc = AddResource(TestHelper.CreateWindowProcess(windowInfo.Name));
      Task.Delay(1000).Wait();
      var oldRec = Win32.GetProcessWindowRec(proc.Process);

      // Apply window info via manager.
      windowManager.ApplyWindowInfo(proc.Process, true);
      Task.Delay(1000).Wait();

      // Verify process values are the same.
      var newRec = Win32.GetProcessWindowRec(proc.Process);
      Assert.Equal(oldRec.Left, newRec.Left);
      Assert.Equal(oldRec.Top, newRec.Top);
      Assert.Equal(oldRec.Width, newRec.Width);
      Assert.Equal(oldRec.Height, newRec.Height);

      // Set flag to alter window process on startup and apply via manager.
      windowInfo.ApplyOnProcessStart = true;
      windowManager.ApplyWindowInfo(proc.Process, true);

      // Verify process values changed.
      newRec = Win32.GetProcessWindowRec(proc.Process);
      Assert.Equal(windowInfo.LayoutInfo.PositionInfo.X, newRec.Left);
      Assert.Equal(windowInfo.LayoutInfo.PositionInfo.Y, newRec.Top);
      Assert.Equal(windowInfo.LayoutInfo.SizeInfo.Width, newRec.Width);
      Assert.Equal(windowInfo.LayoutInfo.SizeInfo.Height, newRec.Height);
    }

    #region Helper Methods

    #endregion Helper Methods
  }
}
