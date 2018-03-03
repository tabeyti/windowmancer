using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using Windowmancer.WPF.Tests.Models;

namespace Windowmancer.WPF.Tests.Practices
{
  public static class TestHelper
  {
    public static string TestHelperProcess => $"{AppDomain.CurrentDomain.BaseDirectory}\\Resources\\WindowTitle.exe";

    public static TestProcessWrapper CreateWindowProcess(string title = null)
    {
      title = title ?? CreateWindowTitle();

      // Before creating process, kill all active window processes
      // matching this window title (don't want duplicates)
      foreach (var proc in Process.GetProcesses())
      {
        if (proc.MainWindowTitle != string.Empty && proc.MainWindowTitle.Contains(title))
        {
          proc.Kill();
        }
      }

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

    public static bool RecsMatch(Win32.Win32_Rect rec1, Win32.Win32_Rect rec2)
    {
      return rec1.Width == rec2.Width &&
             rec1.Height == rec2.Height;
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
    public static Profile CreateNewProfile(List<WindowConfig> WindowConfigs = null)
    {
      WindowConfigs = WindowConfigs ?? new List<WindowConfig> {CreateNewWindowConfig()};
      var profile = new Profile
      {
        Id = Guid.NewGuid().ToString(),
        IsActive = true,
        Name = $"TestProfile_{_profileIncrement++}",
        Windows = new ObservableCollection<WindowConfig>()
      };
      WindowConfigs.ForEach(w => profile.Windows.Add(w));
      return profile;
    }

    public static WindowConfig CreateNewWindowConfig(string name = null)
    {
      name = name ?? CreateWindowTitle();
      var sizeVal = _modifyLayoutRand.Next(200, 500);
      var posVal = _modifyLayoutRand.Next(0, 1024);

      return new WindowConfig
      {
        Name = name,
        ApplyOnProcessStart = true,
        MonitorLayoutInfo = new MonitorLayoutInfo
        {
          SizeInfo = new SizeInfo(sizeVal, sizeVal),
          PositionInfo = new PositionInfo(posVal, posVal)
        },
        MatchCriteria = new WindowMatchCriteria(WindowMatchCriteriaType.WindowTitle, name)
      };
    }

    /// <summary>
    /// Creates a window title based on a counter.
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
    public static void ModifyLayoutInfoSize(MonitorLayoutInfo layoutInfo)
    {
      layoutInfo.SizeInfo.Width = layoutInfo.SizeInfo.Width + _modifyLayoutRand.Next(1, 10);
      layoutInfo.SizeInfo.Height = layoutInfo.SizeInfo.Height + _modifyLayoutRand.Next(1, 10);
    }

    /// <summary>
    /// Modifies the passed layout position with a random value on both X and Y.
    /// </summary>
    public static void ModifyLayoutInfoPosition(MonitorLayoutInfo layoutInfo)
    {
      layoutInfo.PositionInfo.X = layoutInfo.PositionInfo.X + _modifyLayoutRand.Next(1, 10);
      layoutInfo.PositionInfo.Y = layoutInfo.PositionInfo.Y + _modifyLayoutRand.Next(1, 10);
    }

    /// <summary>
    /// Modifies the passed styling opacity with a random value.
    /// </summary>
    private static readonly Random _modifyStylingRand = new Random();
    public static void ModifyStylingInfoOpacity(WindowStylingInfo stylingInfo)
    {
      stylingInfo.WindowOpacityPercentage = (uint)_modifyStylingRand.Next(0, 100);
    }
  }
}
