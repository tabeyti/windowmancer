using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Windows.Media;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Extensions;
using System.Threading.Tasks;
using System.Collections.Generic;
using NLog;

namespace Windowmancer.Core.Services
{
  public class ProcessMonitor : IDisposable
  {
    public Action<Process> OnNewWindowProcess;
    public Action<int> OnWindowProcessRemove;
    public ObservableCollection<MonitoredProcess> ActiveWindowProcs { get; set; }

    private ManagementEventWatcher _startWatch;
    private ManagementEventWatcher _stopWatch;
    private readonly WindowConfigManager _windowConfigManager;
    private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

    public ProcessMonitor(WindowConfigManager windowConfigManager)
    {
      _windowConfigManager = windowConfigManager;
      this.ActiveWindowProcs = new ObservableCollection<MonitoredProcess>();
    }

    public void Start()
    {
      GetAllActiveWindowProcesses();

      const string computerName = "localhost";
      var scope = new ManagementScope($"\\\\{computerName}\\root\\CIMV2", null);
      scope.Connect();

      const string processStartedQuery = "Select * From __InstanceCreationEvent Within 1 " +
                                         "Where TargetInstance ISA 'Win32_Process' ";
      const string processStoppedQuery = "Select * From __InstanceDeletionEvent Within 1 " +
                                         "Where TargetInstance ISA 'Win32_Process' ";

      _startWatch = new ManagementEventWatcher(scope, new EventQuery(processStartedQuery));
      _startWatch.EventArrived += StartWatch_EventArrived;
      _startWatch.Start();

      _stopWatch = new ManagementEventWatcher(scope, new EventQuery(processStoppedQuery));
      _stopWatch.EventArrived += StopWatch_EventArrived;
      _stopWatch.Start();
    }

    private void GetAllActiveWindowProcesses()
    {
      var allProcceses = System.Diagnostics.Process.GetProcesses();
      foreach (var p in allProcceses)
      {
        if (p.MainWindowTitle == string.Empty) { continue; }
        AddToActiveWindowProcs(p);
      }
    }


    private readonly object _enterLock = new object();
    private readonly List<int> _enterCache = new List<int>();

    private void StartWatch_EventArrived(object sender, EventArrivedEventArgs e)
    {
      var obj = ((ManagementBaseObject) e.NewEvent.Properties["TargetInstance"].Value);
      var procId = Convert.ToInt32(obj["ProcessId"]);
      Process proc;
      try
      {
        proc = Process.GetProcessById(procId);
        if (proc.MainWindowTitle == string.Empty) { return; }
      }
      catch (Exception)
      {
        return;
      }

      // If this a legit window process, let's add it to the list.
      // We lock here because sometimes duplicate PID entries come in
      // if processes are started rapidly, so we keep a cache of the last
      // 1 second of PIDs and if a dup is found, we ignore it.
      lock (_enterLock)
      {
        if (_enterCache.Contains(proc.Id))
        {
          _logger.Debug($"Duplicate PID {proc.Id} - {proc.MainWindowTitle}. Skipping...");
          _enterCache.ForEach(c => _logger.Debug($"\t- {c}"));
          return;
        }
      
        _logger.Debug($"Adding {proc.Id} - {proc.MainWindowTitle}");
        _enterCache.Add(proc.Id);
      }

      AddToActiveWindowProcs(proc);
      _windowConfigManager.ApplyWindowConfig(proc, true);

      // Remove the PID from the cache after X amount of time
      Func<int, Task> removeFromEnterCache = async (int pid) =>
      {
        await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
        lock (_enterLock) { _enterCache.Remove(pid); }
      };
      removeFromEnterCache(proc.Id);      
    }

    private void StopWatch_EventArrived(object sender, EventArrivedEventArgs e)
    {
      var obj = ((ManagementBaseObject) e.NewEvent.Properties["TargetInstance"].Value);
      var procId = Convert.ToInt32(obj["ProcessId"]);
      lock (_enterCache)
      {
        RemoveActiveProcess(procId);
      }
    }

    private void AddToActiveWindowProcs(Process process)
    {
      if (this.ActiveWindowProcs.Any(p => p.Id == process.Id)) { return; }
      try
      {
        Helper.Dispatcher.Invoke(delegate
        {
          this.ActiveWindowProcs.Add(new MonitoredProcess(process));
        });
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      OnNewWindowProcess?.Invoke(process);
    }

    private void RemoveActiveProcess(int processId)
    {
      Helper.Dispatcher.Invoke(delegate
      {
        this.ActiveWindowProcs.Remove(p => p.Id == processId);
      }); 
      OnWindowProcessRemove?.Invoke(processId);
    }

    public void Dispose()
    {
      _startWatch?.Stop();
      _stopWatch?.Stop();
    }
  }

  public class MonitoredProcess
  {
    public ImageSource Icon { get; }
    public int Id => _process.Id;

    public string Name => _process.ProcessName;

    public string WindowTitle => _process.MainWindowTitle;

    private readonly Process _process;
    
    public MonitoredProcess(Process process)
    {
      _process = process;
      Icon = Helper.GetProcessIconImageSource(_process);
    }

    public Process GetProcess()
    {
      return _process;
    }
  }
}
