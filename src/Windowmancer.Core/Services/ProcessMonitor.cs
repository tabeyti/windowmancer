using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Windows.Media;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Extensions;

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
        if (p.MainWindowTitle == string.Empty)
        {
          continue;
        }
        AddToActiveWindowProcs(p);
      }
    }

    private readonly object _watchLock = new object();
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
      lock (_watchLock)
      {
        AddToActiveWindowProcs(proc);
        _windowConfigManager.ApplyWindowConfig(proc, true);
      }
    }

    private void StopWatch_EventArrived(object sender, EventArrivedEventArgs e)
    {
      var obj = ((ManagementBaseObject) e.NewEvent.Properties["TargetInstance"].Value);
      var procId = Convert.ToInt32(obj["ProcessId"]);
      lock (_watchLock)
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
