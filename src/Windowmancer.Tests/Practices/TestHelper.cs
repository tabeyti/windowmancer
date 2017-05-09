using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Windowmancer.Tests.Practices
{
  public static class TestHelper
  {
    public static string TestHelperProcess => $"{AppDomain.CurrentDomain.BaseDirectory}\\Resources\\WindowTitle.exe";

    public static Process CreateWindowProcess(string title = null)
    {
      if (null == title)
      {
        title = "DEFAULT TITLE";
      }

      var processInfo = new ProcessStartInfo
      {
        Arguments = title,
        CreateNoWindow = false,
        FileName = TestHelperProcess,
      };
      var process = new Process { StartInfo = processInfo };
      process.Start();

      // Delay to allow process window to appear, otherwise all layout queries of the process
      // (e.g. size and position) will give 0 values.
      Task.Delay(500).Wait();

      return process;
    }
  }
}
