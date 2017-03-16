using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using BrightIdeasSoftware;
using NLog;
using Windowmancer.Models;
using Windowmancer.Practices;
using Windowmancer.Services;

namespace Windowmancer.UI
{
  public partial class Editor : Form
  {
    private ILogger _logger;
    private readonly ProfileManager _profileManager;
    private readonly WindowManager _windowManager;
    private readonly KeyHookManager _keyHookManager;
    private readonly ProcMonitor _procMonitor;

    public Editor(
      ProfileManager profileManager, 
      WindowManager windowManager,
      KeyHookManager keyHookManager,
      ProcMonitor procMonitor)
    {
      _profileManager = profileManager;
      _windowManager = windowManager;
      _keyHookManager = keyHookManager;
      _procMonitor = procMonitor;

      InitializeComponent();
      Initialize();      
    }

    public void Initialize()
    {
      this.ProfileListDataGridView.DataSource = _profileManager.Profiles;
      this.ProfileListDataGridView.Columns[this.ProfileListDataGridView.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      
      this.WindowConfigsDataGrid.DataSource = _profileManager.ActiveProfile.Windows;
      this.WindowConfigsDataGrid.Columns[this.WindowConfigsDataGrid.ColumnCount-1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

      RemoveToolStripMenuMargins();
    }

    private void ProfileListSelectActiveProfile()
    {
      foreach (DataGridViewRow row in this.ProfileListDataGridView.Rows)
      {
        var p = (Profile)row.DataBoundItem;
        if (p != _profileManager.ActiveProfile) continue;
        row.Selected = true;
        break;
      }
    }

    protected void InternalDispose()
    {
      _windowManager.Dispose();
    }

    public void UpdateActiveProfile(Profile profile)
    {
      if (null == profile)
      {
        throw new ExceptionBox($"{this} - Cannot update with null profile");
      }
      _profileManager.ActiveProfile = profile;
      ProfileListSelectActiveProfile();
      this.WindowConfigsDataGrid.DataSource = profile.Windows;
    }

    private void AddToActiveWindows(Process process)
    {
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

    private void RemoveToolStripMenuMargins()
    {
      foreach (ToolStripMenuItem menuItem in menuStrip1.Items)
      {
        ((ToolStripDropDownMenu)menuItem.DropDown).ShowImageMargin = false;
      }
    }

    private void RemoveActiveProcess(int proccessId)
    {
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

    private WindowInfo ShowWindowConfigDialog(Process process)
    {
      var dialog = new WindowConfigDialog(process);
      dialog.ShowDialog();
      return dialog.WindowInfo;
    }

    private WindowInfo ShowWindowConfigDialog(WindowInfo windowInfo)
    {
      var dialog =  new WindowConfigDialog(windowInfo);
      dialog.ShowDialog();
      return dialog.WindowInfo;
    }

    private void HandleProfileConfigDialog()
    {
      var dialog = new ProfileConfigDialog(_profileManager);
      dialog.ShowDialog();
      ProfileListSelectActiveProfile();
    }

    #region Events

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
      _procMonitor.OnNewWindowProcess += AddToActiveWindows;
      _procMonitor.OnWindowProcessRemove += RemoveActiveProcess;
      _procMonitor.StartProcessMonitor();
      ProfileListSelectActiveProfile();
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
      var dialog = new SettingsDialog(_keyHookManager);
      dialog.ShowDialog();
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

    private void ProfileListDataGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.ProfileListDataGridView.Focused)
      {
        return;
      }

      if (this.ProfileListDataGridView.SelectedRows.Count > 0)
      {
        _profileManager.UpdateActiveProfile(this.ProfileListDataGridView.SelectedRows[0].Index);
        this.WindowConfigsDataGrid.DataSource = _profileManager.ActiveProfile.Windows;
      }      
    }

    private void addToolStripMenuItem_Click(object sender, EventArgs e)
    {
      HandleProfileConfigDialog();
    }

    private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      //var index = this.ProfileListBox.SelectedIndex;
      var index = this.ProfileListDataGridView.SelectedRows[0].Index;
      if (index < 0)
      {
        return;
      }
      _profileManager.DeleteActiveProfile();
      ProfileListSelectActiveProfile();
    }

    private void WindowConfigsContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {      
      var cms = sender as ContextMenuStrip;
      var mousepos = Control.MousePosition;
      if (cms != null)
      {
        var rel_mousePos = cms.PointToClient(mousepos);
        if (cms.ClientRectangle.Contains(rel_mousePos))
        {
          var dgv_rel_mousePos = this.WindowConfigsDataGrid.PointToClient(mousepos);
          var hti = this.WindowConfigsDataGrid.HitTest(dgv_rel_mousePos.X, dgv_rel_mousePos.Y);
          if (hti.RowIndex == -1)
          {
            e.Cancel = true;
          }
        }
        else
        {
          e.Cancel = true;
        }
      }      
    }

    private void ProfileListDataGridView_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
      {
        return;
      }

      var mousepos = Control.MousePosition;
      var dgv_rel_mousePos = this.ProfileListDataGridView.PointToClient(mousepos);
      var hti = this.ProfileListDataGridView.HitTest(dgv_rel_mousePos.X, dgv_rel_mousePos.Y);
        if (hti.RowIndex == -1)
      {
        this.ProfileListBoxContextMenu.Items[1].Enabled = false;
        return;
      }
      this.ProfileListBoxContextMenu.Items[1].Enabled = true;
      this.ProfileListDataGridView.Rows[hti.RowIndex].Selected = true;
      this.ProfileListDataGridView.Focus();
    }
  }
}
