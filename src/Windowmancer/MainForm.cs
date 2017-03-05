using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NLog;
using Windowmancer.Services;
using Windowmancer.Models;
using Newtonsoft.Json;
using Microsoft.Practices.Unity;

namespace Windowmancer
{
  public partial class MainForm : Form
  {
    private ManagementEventWatcher _startWatch;
    private ManagementEventWatcher _stopWatch;
    private readonly Dictionary<int, Process> _availableWindowDict = new Dictionary<int, Process>();
    private readonly IUnityContainer _serviceResolver;

    private WindowManager _windowManager;
    private ILogger _logger;


    public MainForm(IUnityContainer serviceResolver)
    {
      _serviceResolver = serviceResolver;

      InitializeComponent();
      Initialize();      
    }

    public void Initialize()
    {
      // TODO: DEBUG

      //var thing = JsonConvert.SerializeObject(profile);
      var profile = new Profile
      {
        Name = "Profile shmofile",
        Windows = new List<WindowInfo>
        {
          new WindowInfo
          {
            LocationInfo = new LocationInfo
            {
              DisplayName = "\\\\.\\DISPLAY2",
              Info = new Position { X = 300, Y = 300 },
              PrimaryDisplay = false
            },
            MatchCriteria = new WindowTitleMatchCreteria(".*WMIC.*"),
            SizeInfo = new SizeInfo { Height = 800, Width = 800 }
          }
        }
      };

      var list = new List<Profile>();
      list.Add(profile);
      var stuff = JsonConvert.SerializeObject(list);

      _windowManager = new WindowManager();
      _windowManager.LoadProfile(profile);
    }

    protected void InternalDispose()
    {
      _windowManager.Dispose();
      _startWatch?.Stop();
      _stopWatch?.Stop();
    }

    public void StartProcessMonitor()
    {
      _startWatch = new ManagementEventWatcher(
        new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
      _startWatch.EventArrived += StartWatch_EventArrived;
      _startWatch.Start();

      _stopWatch = new ManagementEventWatcher(
        new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace "));
      _stopWatch.EventArrived += StopWatch_EventArrived;
      _stopWatch.Start();
    }

    private void AddActiveWindow(Process process)
    {
      if (_availableWindowDict.ContainsKey(process.Id))
      {
        return;
      }
      _availableWindowDict.Add(process.Id, process);

      System.Drawing.Icon ico = null;
      try
      {
        ico = System.Drawing.Icon.ExtractAssociatedIcon(process.MainModule.FileName);
      }
      catch (Exception)
      {
        // ignore.
      }

      if (this.ActiveWindowsGridView.InvokeRequired)
      {
        this.ActiveWindowsGridView.Invoke(
          new MethodInvoker(
            () => ActiveWindowsGridView.Rows.Add(process.Id, process.MainWindowTitle, process.ProcessName, ico)));
      }
      else
      {
        ActiveWindowsGridView.Rows.Add(process.Id, process.MainWindowTitle, process.ProcessName, ico);
      }
    }

    private void RemoveActiveProcess(int proccessId)
    {
      if (_availableWindowDict.ContainsKey(proccessId))
      {
        _availableWindowDict.Remove(proccessId);

        var row = 
        this.ActiveWindowsGridView.Rows
          .Cast<DataGridViewRow>()
          .First(r => int.Parse(r.Cells["PID"].Value.ToString()).Equals(proccessId));

        if (null == row)
        {
          return;
        }

        if (this.ActiveWindowsGridView.InvokeRequired)
        {
          this.ActiveWindowsGridView.Invoke(
            new MethodInvoker(
              () => this.ActiveWindowsGridView.Rows.RemoveAt(row.Index)));
        }
        else
        {
          this.ActiveWindowsGridView.Rows.RemoveAt(row.Index);
        }
      }
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

      try
      { 
        AddActiveWindow(proc);
        _windowManager.ApplyWindowInfo(proc);
      }
      catch (Exception ex)
      {
        _logger.Error(ex.Message);
      }
    }

    private void StopWatch_EventArrived(object sender, EventArrivedEventArgs e)
    {
      var procProps = e.NewEvent.Properties;
      var procId = int.Parse(procProps["ProcessId"].Value.ToString());
      try
      {
        RemoveActiveProcess(procId);
      }
      catch (Exception)
      {
        // ignore
      }
    }
    private void button1_Click(object sender, EventArgs e)
    {
      _windowManager.RefreshProfile();
    }

    private void Display_Click(object sender, EventArgs e)
    {
      foreach (var screen in Screen.AllScreens)
      {
        // For each screen, add the screen properties to a list box.
        _logger.Debug($"Device Name: " + screen.DeviceName + "\n");
        _logger.Debug($"Bounds: " + screen.Bounds.ToString() + "\n");
        _logger.Debug($"Type: " + screen.GetType().ToString() + "\n");
        _logger.Debug($"Working Area: " + screen.WorkingArea.ToString() + "\n");
        _logger.Debug($"Primary Screen: " + screen.Primary.ToString() + "\n");
      }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      // Create logger class to be bound to the richtextbox.
      _logger = LogManager.GetCurrentClassLogger();

      // Change default style of no-icon window procs.
      this.ActiveWindowsGridView.Columns["Icon"].DefaultCellStyle.NullValue = null;

      // Add all active window processes.
      var allProcceses = System.Diagnostics.Process.GetProcesses();
      foreach (var p in allProcceses)
      {
        if (p.MainWindowTitle == string.Empty)
        {
          continue;
        }
        AddActiveWindow(p);
      }
      StartProcessMonitor();
    }
  }
}
