using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Services;
using Windowmancer.Tests.Practices;
using Windowmancer.Tests.Tests.Base;
using Xunit;
using Xunit.Abstractions;

namespace Windowmancer.Tests.Tests
{
  public class WindowManagerTests : TestClassBase
  {
    public WindowManagerTests(ITestOutputHelper xunitTestOutputHelper) : base(xunitTestOutputHelper)
    {
    }

    [Fact]
    public void WindowManagerTests_ApplyWindowLayout_Position()
    {
      var windowTitle = "Some_title";

      var proc = TestHelper.CreateWindowProcess(windowTitle);
      Win32.WaitForProcessWindow(proc);
      
      // Create a WindowInfo object from our create process.
      var windowInfo = WindowInfo.FromProcess(proc);

      var originalLayoutInfo = windowInfo.LayoutInfo;
      var modifiedLayoutInfo = (WindowLayoutInfo)originalLayoutInfo.Clone();

      // Modify position values.
      ModifyLayoutInfoPosition(modifiedLayoutInfo);

      // Apply modified layout via window manager.
      WindowManager.ApplyWindowLayout(modifiedLayoutInfo, proc);

      // Get the process' current position and verify it matches our modified one.
      var newRec = Win32.GetProcessWindowRec(proc);

      Assert.Equal(modifiedLayoutInfo.PositionInfo.X, newRec.Left);
      Assert.Equal(modifiedLayoutInfo.PositionInfo.Y, newRec.Top);

      proc.Kill();
    }

    [Fact]
    public void WindowManagerTests_ApplyWindowLayout_Size()
    {
      var windowTitle = "Some_title";

      var proc = TestHelper.CreateWindowProcess(windowTitle);
      Win32.WaitForProcessWindow(proc);

      // Create a WindowInfo object from our process.
      var windowInfo = WindowInfo.FromProcess(proc);

      var originalLayoutInfo = windowInfo.LayoutInfo;
      var modifiedLayoutInfo = (WindowLayoutInfo)originalLayoutInfo.Clone();

      // Modify the size values.
      ModifyLayoutInfoSize(modifiedLayoutInfo);

      // Apply modified layout via window manager.
      WindowManager.ApplyWindowLayout(modifiedLayoutInfo, proc);

      // Get the process' current position and verify it matches our modified one.
      var newRec = Win32.GetProcessWindowRec(proc);

      Assert.Equal(modifiedLayoutInfo.SizeInfo.Width, newRec.Width);
      Assert.Equal(modifiedLayoutInfo.SizeInfo.Height, newRec.Height);

      proc.Kill();
    }

    public void WindowManagerTests_ApplyWindowInfo_SizeAndPosition()
    {
      var windowTitle = "Some_title";
      var windowManager = ServiceResolver.Resolve<WindowManager>();


      var proc = TestHelper.CreateWindowProcess(windowTitle);
      Win32.WaitForProcessWindow(proc);

      // Create a WindowInfo object from our process.
      var windowInfo = WindowInfo.FromProcess(proc);

      // Create a profile object to hold our window info and assign
      // to the window manager.
      var profile = new Profile
      {
        Id = Guid.NewGuid().ToString(),
        IsActive = true,
        Name = "Test Profile",
        Windows = new ObservableCollection<WindowInfo> {windowInfo}
      };
      windowManager.ActiveProfile = profile;

      var originalLayout = windowInfo.LayoutInfo.Clone();

    }


    #region Helper Methods

    private void ModifyLayoutInfoSize(WindowLayoutInfo layoutInfo)
    {
      layoutInfo.SizeInfo.Width = layoutInfo.SizeInfo.Width + 1;
      layoutInfo.SizeInfo.Height = layoutInfo.SizeInfo.Height + 1;
    }

    private void ModifyLayoutInfoPosition(WindowLayoutInfo layoutInfo)
    {
      layoutInfo.PositionInfo.X = layoutInfo.PositionInfo.X + 1;
      layoutInfo.PositionInfo.Y = layoutInfo.PositionInfo.Y + 1;
    }


    #endregion Helper Methods
  }
}
