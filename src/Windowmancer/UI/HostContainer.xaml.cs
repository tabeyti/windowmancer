using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using MahApps.Metro.Controls;
using Microsoft.Practices.Unity;
using Windowmancer.Core.Extensions;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Services;
using Windowmancer.Core.Services.Base;
using Windowmancer.Services;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for HostContainer.xaml
  /// </summary>
  public partial class HostContainer : IToastHost
  {
    public int CurrentRowIndex { get; private set; }
    public int CurrentColumnIndex { get; private set; }
    public HostContainerConfig HostContainerConfig { get; set; }

    private ProfileManager ProfileManager { get; set; }

    private static readonly int _titlebarHeight = (int)SystemParameters.WindowCaptionHeight + 10;
    private static readonly string _defaultContainerName = "My Container";
    private readonly bool _stupidFlag = false;

    public HostContainer(HostContainerConfig hostContiner, bool enableEditorOnLoad = false)
    {
      _stupidFlag = enableEditorOnLoad;
      this.HostContainerConfig = hostContiner;
      InitializeComponent();
      Initialize();
    }

    private void Initialize()
    {
      this.ProfileManager = Helper.ServiceResolver.Resolve<ProfileManager>();
      this.Closing += (o, e) =>
      {
        foreach (var dw in this.HostContainerConfig.DockedWindows)
        {
          dw.Process?.Kill();
        }
      };
    }

    public Tuple<int, int> NextDockProcRowColumn()
    {
      var rowIndex = this.CurrentRowIndex;
      var columnIndex = this.CurrentColumnIndex;

      if (columnIndex >= this.HostContainerConfig.Columns)
      {
        if (rowIndex >= this.HostContainerConfig.Rows)
        {
          return null;
        }
        rowIndex++;
        columnIndex = 0;
      }
      return Tuple.Create(rowIndex, columnIndex);
    }

    public void DockProc(Process process, WindowConfig windowConfig)
    {
      DockProc(process);
      HandleWindowConfigEdit(process);
    }

    /// <summary>
    /// Docks the passed process window on the next available row/column
    /// index.
    /// </summary>
    /// <param name="process"></param>
    public void DockProc(Process process)
    {
      if (this.CurrentColumnIndex >= this.HostContainerConfig.Columns)
      {
        if (this.CurrentRowIndex >= this.HostContainerConfig.Rows)
        {
          return;
        }
        this.CurrentRowIndex++;
        this.CurrentColumnIndex = 0;
      }
      DockProc(process, this.CurrentRowIndex, this.CurrentColumnIndex++);
    }

    public void DockProc(Process process, int rowIndex, int columnIndex)
    {
      if (this.HostContainerConfig.DockedWindows.Any(dw => dw.Process.Handle == process.Handle))
      {
        return;
      }

      var windowToDock = new DockableWindow(process) { Row = rowIndex, Column = columnIndex };
      this.HostContainerConfig.DockedWindows.Add(windowToDock);
      while (process.MainWindowHandle == IntPtr.Zero)
      {
        // We try catch here because some windows (e.g. cmd) 
        // do not support the call WaitForInputIdle.
        try
        {
          // Wait for the window to be ready for input.
          process.WaitForInputIdle(1000);
        }
        catch
        {
          // ignore
        }
        process.Refresh();
        if (process.HasExited) return;
        process.Exited += (s, e) =>
        {
          Helper.Dispatcher.Invoke(() =>
          {
            var d = this.HostContainerConfig.DockedWindows.Find(dw => dw.Process.Id == process.Id);
            this.HostContainerConfig.DockedWindows.Remove(d);
          });
        };
      }
     
      // Retrieve the window handle for this host container window.
      var window = Window.GetWindow(this);
      if (null == window)
      {
        throw new Exception($"{this}.RefreshDockableWindow - Could not retrieve the host container window.");
      }
      var wih = new WindowInteropHelper(window);
      windowToDock.ParentHandle = Win32.SetParent(windowToDock.Process.MainWindowHandle, wih.Handle);

      void Resize(object s, SizeChangedEventArgs ev)
      {
        RefreshDockableWindow(windowToDock);
      }

      this.SizeChanged += Resize;
      RefreshDockableWindow(windowToDock);
    }

    public void SetDockableWindowVisibility(bool isVisible)
    {
      foreach (var dw in this.HostContainerConfig.DockedWindows)
      {
        MonitorWindowManager.SetWindowOpacityPercentage(dw.Process, (uint)(isVisible ? 100 : 0));
      }
    }

    public void HandleWindowConfigEdit(Process process)
    {
      SetDockableWindowVisibility(false);
      var flyout = (Flyout)this.FindName("RightFlyout");
      if (flyout == null) return;

      var config = WindowConfig.FromProcess(process, false);
      var wce = new WindowConfigEditor(config, c =>
      {
        this.ProfileManager.AddToActiveProfile(c);
        ShowItemMessageToast(c.Name, "added to window configuration list.");
      });
      wce.OnClose += () => { flyout.IsOpen = false; };

      flyout.Content = wce;
      flyout.IsOpen = true;
    }

    public void HandleHostConfigEdit()
    {
      SetDockableWindowVisibility(false);

      var flyout = this.Flyouts.Items[0] as Flyout;
      if (flyout == null) return;
      var containerConfigEditor = new HostContainerConfigEditor(
        this.HostContainerConfig, new SizeInfo((int)this.ActualWidth, (int)this.ActualHeight))
      {
        DisplayContainersSelectable = false
      };
      containerConfigEditor.OnSave += (dcs) =>
      {
        this.HostContainerConfig.UpdateUserData(dcs);
        RefreshDisplayContainer();
      };
      containerConfigEditor.OnClose += () =>
      {
        flyout.IsOpen = false;
        SetDockableWindowVisibility(true);
      };

      flyout.Content = containerConfigEditor;
      flyout.IsOpen = true;
    }

    private void RefreshDisplayContainer()
    {
      foreach (var d in this.HostContainerConfig.DockedWindows)
      {
        RefreshDockableWindow(d);
      }
    }

    private void RefreshDockableWindow(DockableWindow dockableWindow)
    {
      var screenWidth = this.ActualWidth;
      var screenHeight = this.ActualHeight;

      var totalRows = this.HostContainerConfig.Rows;
      var totalCols = this.HostContainerConfig.Columns;

      var x = (int)(screenWidth / totalCols) * dockableWindow.Column;
      var y = (int)(((screenHeight / totalRows) * dockableWindow.Row) + _titlebarHeight);

      var width = (int)(screenWidth / totalCols);
      var height = (int)(screenHeight / totalRows);

      Win32.MoveWindow(dockableWindow.Process.MainWindowHandle, x, y, (int)width, (int)height, true);
    }

    private void EditButton_OnClick(object sender, RoutedEventArgs e)
    {
      HandleHostConfigEdit();
    }

    private void HostContainer_OnLoaded(object sender, RoutedEventArgs e)
    {
      if (_stupidFlag)
      {
        HandleHostConfigEdit();
      }
    }

    public void ShowMessageToast(string message)
    {
      if (message == null)
      {
        return;
      }

      var flyout = (Flyout)this.FindName("ToastFlyout");
      if (null == flyout)
      {
        throw new Exception($"{this} - Could not find flyout ToastFlyout.");
      }
      this.ToastMessage.Text = message;
      flyout.IsOpen = true;
    }

    public void ShowItemMessageToast(string itemName, string message)
    {
    }

    private void HostContainer_OnClosed(object sender, EventArgs e)
    {
      var hcm = App.ServiceResolver.Resolve<HostContainerManager>();
      hcm.RemoveHostContainer(this);
    }
  }
}
