using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using NLog;
using Windowmancer.Configuration;
using Windowmancer.Models;
using Windowmancer.Services;

namespace Windowmancer.UI
{
  public partial class MainForm : Form
  {
    private ManagementEventWatcher _startWatch;
    private ManagementEventWatcher _stopWatch;
    private ProfileManager _profileManager;
    private WindowManager _windowManager;
    private ILogger _logger;
    private readonly Dictionary<int, Process> _availableWindowDict = new Dictionary<int, Process>();
    private readonly IUnityContainer _serviceResolver;

    public MainForm(IUnityContainer serviceResolver)
    {
      _serviceResolver = serviceResolver;

      InitializeComponent();
      Initialize();      
    }

    public void Initialize()
    {
      _profileManager = new ProfileManager(_serviceResolver.Resolve<ProfileManagerConfig>());
      _windowManager = new WindowManager();
      _windowManager.LoadProfile(_profileManager.GetActiveProfile());

      this.ProfileListBox.DisplayMember = "Name";
      this.ProfileListBox.Items.AddRange(_profileManager.Profiles.ToArray());

      SavedWindowsDataGrid.DataSource = _profileManager.GetActiveProfile().Windows;
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

        var row = this.ActiveWindowsGridView.Rows
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

    //private void Display_Click(object sender, EventArgs e)
    //{
    //  foreach (var screen in Screen.AllScreens)
    //  {
    //    // For each screen, add the screen properties to a list box.
    //    _logger.Debug($"Device Name: " + screen.DeviceName + "\n");
    //    _logger.Debug($"Bounds: " + screen.Bounds.ToString() + "\n");
    //    _logger.Debug($"Type: " + screen.GetType().ToString() + "\n");
    //    _logger.Debug($"Working Area: " + screen.WorkingArea.ToString() + "\n");
    //    _logger.Debug($"Primary Screen: " + screen.Primary.ToString() + "\n");
    //  }
    //}

    private void Form1_Load(object sender, EventArgs e)
    {
      // Create logger class to be bound to the richtextbox.
      if (null == _logger)
      {
        _logger = LogManager.GetCurrentClassLogger();
      }

      // Change default style of no-icon window procs.
      var dataGridViewColumn = this.ActiveWindowsGridView.Columns["Icon"];
      if (dataGridViewColumn != null)
        dataGridViewColumn.DefaultCellStyle.NullValue = null;

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

    private void button1_Click(object sender, EventArgs e)
    {
      _profileManager.GetActiveProfile().Windows.RemoveAt(0);
    }

    private void AddWindowConfigButton_Click(object sender, EventArgs e)
    {
      ShowWindowConfigDialog();
    }

    public WindowInfo ShowWindowConfigDialog()
    {
      var procRow = this.ActiveWindowsGridView.SelectedRows[0];
      var proc = Process.GetProcessById((int) procRow.Cells[0].Value);
      var prompt = new WindowConfigDialog(proc)
      {
        StartPosition = FormStartPosition.CenterParent
      };
      prompt.ShowDialog();
      return null;
    }
  }
}
