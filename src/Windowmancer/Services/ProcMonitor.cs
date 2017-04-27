using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using NLog;

namespace Windowmancer.Services
{
  public class ProcMonitor : IDisposable
  {
    public event Action<Process> OnNewWindowProcess;
    public event Action<int> OnWindowProcessRemove;

    private ManagementEventWatcher _startWatch;
    private ManagementEventWatcher _stopWatch;

    private readonly WindowManager _windowManager;
    private readonly Dictionary<int, Process> _availableWindowDict = new Dictionary<int, Process>();

    public ProcMonitor(WindowManager windowManager)
    {
      _windowManager = windowManager;
    }

    public void Start()
    {
      _startWatch = new ManagementEventWatcher(
        new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
      var startWatchEvent = new EventArrivedEventHandler(StartWatch_EventArrived);
      _startWatch.EventArrived += startWatchEvent;
      _startWatch.Start();

      _stopWatch = new ManagementEventWatcher(
        new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace "));
      var stopWatchEvent = new EventArrivedEventHandler(StopWatch_EventArrived);
      _stopWatch.EventArrived += stopWatchEvent;
      _stopWatch.Start();
    }

    private void StartWatch_EventArrived(object sender, EventArrivedEventArgs e)
    {
      var procProps = e.NewEvent.Properties;
      var procId = int.Parse(procProps["ProcessId"].Value.ToString());
      Process proc = null;
      try
      {
        proc = Process.GetProcessById(procId);
        if (proc.MainWindowTitle == string.Empty)
          return;
      }
      catch (Exception)
      {
        return;
      }
      AddToActiveWindows(proc);
      _windowManager.ApplyWindowInfo(proc);
    }

    private void StopWatch_EventArrived(object sender, EventArrivedEventArgs e)
    {
      var procProps = e.NewEvent.Properties;
      var procId = int.Parse(procProps["ProcessId"].Value.ToString());
      RemoveActiveProcess(procId);
    }

    private void AddToActiveWindows(Process process)
    {
      if (_availableWindowDict.ContainsKey(process.Id))
      {
        return;
      }
      _availableWindowDict.Add(process.Id, process);
      OnNewWindowProcess?.Invoke(process);
    }

    private void RemoveActiveProcess(int processId)
    {
      if (_availableWindowDict.ContainsKey(processId))
      {
        _availableWindowDict.Remove(processId);
      }
      OnWindowProcessRemove?.Invoke(processId);
    }

    public void Dispose()
    {
      _startWatch?.Stop();
      _stopWatch?.Stop();
    }
  }
}
