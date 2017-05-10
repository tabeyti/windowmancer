using System;
using System.Collections.Generic;
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

    [Fact]
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
    public void WindowManagerTests_ApplyLayout_NoValueChange()
    {
      var proc = AddResource(TestHelper.CreateWindowProcess());

      // Create a WindowInfo object from our create process.
      var windowInfo = WindowInfo.FromProcess(proc.Process);

      // Apply modified layout via window manager.
      WindowManager.ApplyWindowLayout(windowInfo.LayoutInfo, proc.Process);

      // Get the process' current position and verify it matches our modified one.
      var newRec = Win32.GetProcessWindowRec(proc.Process);
      Assert.Equal(windowInfo.LayoutInfo.PositionInfo.X, newRec.Left);
      Assert.Equal(windowInfo.LayoutInfo.PositionInfo.Y, newRec.Top);
    }

    #region Helper Methods

    #endregion Helper Methods
  }
}
