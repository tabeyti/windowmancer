﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using NLog;
using Windowmancer.Configuration;
using Windowmancer.Models;
using Windowmancer.Practices;
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

    public MainForm(IUnityContainer serviceResolver, ProfileManager profileManager, WindowManager windowManager)
    {
      _serviceResolver = serviceResolver;
      _profileManager = profileManager;
      _windowManager = windowManager;

      InitializeComponent();
      Initialize();      
    }

    public void Initialize()
    {
      this.ProfileListBox.DisplayMember = "Name";
      this.ProfileListBox.Items.AddRange(_profileManager.Profiles.ToArray());
      this.ProfileListBox.SelectedItem = _profileManager.ActiveProfile;

      this.WindowConfigsDataGrid.DataSource = _profileManager.ActiveProfile.Windows;
      this.WindowConfigsDataGrid.Columns[this.WindowConfigsDataGrid.ColumnCount-1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

    private void AddToActiveWindows(Process process)
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
        ico = Helper.GetSmallIcon(ico);
      }
      catch (Exception e)
      {
        // ignore.
        _logger.Debug(e);
      }

      if (this.ActiveWindowsGridView.InvokeRequired)
      {
        this.ActiveWindowsGridView.Invoke(
          new MethodInvoker(
            () => ActiveWindowsGridView.Rows.Add(ico, process.Id, process.ProcessName, process.MainWindowTitle)));
      }
      else
      {
        ActiveWindowsGridView.Rows.Add(ico, process.Id, process.ProcessName, process.MainWindowTitle);
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

    public WindowInfo ShowWindowConfigDialog(Process process)
    {
      var dialog = new WindowConfigDialog(process);
      dialog.ShowDialog();
      return dialog.WindowInfo;
    }

    public WindowInfo ShowWindowConfigDialog(WindowInfo windowInfo)
    {
      var dialog =  new WindowConfigDialog(windowInfo);
      dialog.ShowDialog();
      return dialog.WindowInfo;
    }

    #region Events

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
        AddToActiveWindows(proc);
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

    private void Form1_Load(object sender, EventArgs e)
    {
      // Create logger class to be bound to the richtextbox.
      if (null == _logger)
      {
        _logger = LogManager.GetCurrentClassLogger();
      }

      // Change default style of no-icon windows.
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
        AddToActiveWindows(p);
      }
      StartProcessMonitor();
    }

    private void ActiveWindowsGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      var procRow = e.RowIndex <= 0 ? this.ActiveWindowsGridView.SelectedRows[0] : this.ActiveWindowsGridView.Rows[e.RowIndex];
      var proc = Process.GetProcessById((int)procRow.Cells[1].Value);
      var windowInfo = ShowWindowConfigDialog(proc);
      if (null == windowInfo)
      {
        return;
      }
      _profileManager.AddToActiveProfile(windowInfo);
    }

    #endregion Events

    private void SavedWindowsDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      var procRow = e.RowIndex <= 0 ? this.WindowConfigsDataGrid.SelectedRows[0] : this.WindowConfigsDataGrid.Rows[e.RowIndex];
      var windowInfo = ShowWindowConfigDialog((WindowInfo)procRow.DataBoundItem);
      if (null == windowInfo)
      {
        return;
      }
      _profileManager.ActiveProfile.Windows[e.RowIndex] = windowInfo;
    }

    private void preferencesToolStripMenuItem1_Click(object sender, EventArgs e)
    {

    }

    private void WindowConfigsDataGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
      {
        return;
      }
      this.WindowConfigsDataGrid.CurrentCell = this.WindowConfigsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
      this.WindowConfigsDataGrid.Rows[e.RowIndex].Selected = true;
      this.WindowConfigsDataGrid.Focus();
    }

    private void editToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var procRow = this.WindowConfigsDataGrid.SelectedRows[0];
      var item = (WindowInfo)procRow.DataBoundItem;
      var windowInfo = ShowWindowConfigDialog(item);
      if (null == windowInfo)
      {
        return;
      }
      _profileManager.ActiveProfile.Windows[procRow.Index] = windowInfo;
    }

    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var procRow = this.WindowConfigsDataGrid.SelectedRows[0];
      var item = (WindowInfo)procRow.DataBoundItem;
      _profileManager.RemoveFromActiveProfile(item);      
    }
  }
}
