using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using Windowmancer.Tests.Models;

namespace Windowmancer.Tests.Practices
{
  public static class TestHelper
  {
    public static string TestHelperProcess => $"{AppDomain.CurrentDomain.BaseDirectory}\\Resources\\WindowTitle.exe";

    public static TestProcessWrapper CreateWindowProcess(string title = null)
    {
      title = title ?? CreateWindowTitle();
      var processInfo = new ProcessStartInfo
      {
        Arguments = title,
        CreateNoWindow = false,
        FileName = TestHelperProcess,
      };
      var process = new Process { StartInfo = processInfo };
      process.Start();

      // Delay to allow processWrapper window to appear, otherwise all layout queries of the processWrapper
      // (e.g. size and position) will give 0 values.
      Win32.WaitForProcessWindow(process);
      Task.Delay(500).Wait();

      // Return and updated process (with window title in it)
      return GetUpdatedWindowProcess(new TestProcessWrapper(process));
    }

    /// <summary>
    /// Gets and updated instance of the passed process.
    /// </summary>
    /// <param name="processWrapper"></param>
    /// <returns></returns>
    public static TestProcessWrapper GetUpdatedWindowProcess(TestProcessWrapper processWrapper)
    {
      return new TestProcessWrapper(Process.GetProcesses().ToList().Find(p => p.Id == processWrapper.Id));
    }

    private static uint _profileIncrement = 1;
    public static Profile CreateNewProfile(List<WindowInfo> windowInfos)
    {
      var profile = new Profile
      {
        Id = Guid.NewGuid().ToString(),
        IsActive = true,
        Name = $"TestProfile_{_profileIncrement++}",
        Windows = new ObservableCollection<WindowInfo>()
      };
      windowInfos.ForEach(w => profile.Windows.Add(w));
      return profile;
    }

    /// <summary>
    /// Creates a window title based on an incrementer value.
    /// </summary>
    private static uint _windowTitleIncrement = 1;
    public static string CreateWindowTitle()
    {
      return $"Test_Window_{_windowTitleIncrement++}";
    }

    /// <summary>
    /// Modifies the passed layout size with a random value on both width and height.
    /// </summary>
    private static readonly Random _modifyLayoutRand = new Random();
    public static void ModifyLayoutInfoSize(WindowLayoutInfo layoutInfo)
    {
      layoutInfo.SizeInfo.Width = layoutInfo.SizeInfo.Width + _modifyLayoutRand.Next(1, 10);
      layoutInfo.SizeInfo.Height = layoutInfo.SizeInfo.Height + _modifyLayoutRand.Next(1, 10);
    }

    /// <summary>
    /// Modifies the passed layout position with a random value on both X and Y.
    /// </summary>
    public static void ModifyLayoutInfoPosition(WindowLayoutInfo layoutInfo)
    {
      layoutInfo.PositionInfo.X = layoutInfo.PositionInfo.X + _modifyLayoutRand.Next(1, 10);
      layoutInfo.PositionInfo.Y = layoutInfo.PositionInfo.Y + _modifyLayoutRand.Next(1, 10);
    }

  }
}
