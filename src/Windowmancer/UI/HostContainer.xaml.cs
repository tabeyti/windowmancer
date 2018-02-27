using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using MahApps.Metro.Controls;
using Microsoft.Practices.ObjectBuilder2;
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
    public HostContainerConfig HostContainerConfig { get; set; }
    
    private int CurrentRowIndex { get; set; }
    private int CurrentColumnIndex { get; set; }
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

    public Tuple<uint, uint> NextDockProcRowColumn()
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
      return Tuple.Create((uint)rowIndex, (uint)columnIndex);
    }

    public void DockNewProc(Process process)
    {
      var xy = NextDockProcRowColumn();
      if (null == xy)
      {
        throw new Exception($"No more space to put {process.ProcessName} in container {this.HostContainerConfig.Name}");
      }
      
      var config = WindowConfig.FromProcess(process, Core.Models.WindowConfigLayoutType.HostContainer);
      config.HostContainerLayoutInfo.Update(xy.Item1, xy.Item2, this.HostContainerConfig.Name);
      
      DockProc(process, config);

      // The the layout information for the config, then 
      // open the window config editor.
      this.ProfileManager.AddToActiveProfile(config);
      HandleWindowConfigEdit(config);
    }

    /// <summary>
    /// Docks the passed process window on the next available row/column
    /// index.
    /// </summary>
    /// <param name="process"></param>
    /// <param name="windowConfig"></param>
    private void DockProc(Process process, WindowConfig windowConfig)
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

      var layoutInfo = windowConfig.HostContainerLayoutInfo;
      layoutInfo.Row = (uint) this.CurrentRowIndex;
      layoutInfo.Column = (uint) this.CurrentColumnIndex;
      
      if (this.HostContainerConfig.DockedWindows.Any(dw => dw.Process.Handle == process.Handle))
      {
        throw new Exception("You are docking the same window twice. Just herpin and derpin, aren't ya?");
      }

      var windowToDock = new DockableWindow(process, windowConfig);
      this.HostContainerConfig.DockedWindows.Add(windowToDock);
      while (process.MainWindowHandle == IntPtr.Zero)
      {
        // We try catch here because some windows (e.g. cmd) 
        // do not support the call WaitForInputIdle.
        try { process.WaitForInputIdle(1000); } catch { }
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

    private void SetDockableWindowVisibility(bool isVisible)
    {
      foreach (var dw in this.HostContainerConfig.DockedWindows)
      {
        MonitorWindowManager.SetWindowOpacityPercentage(dw.Process, (uint)(isVisible ? 100 : 0));
      }
    }

    private void HandleWindowConfigEdit(WindowConfig config)
    {
      SetDockableWindowVisibility(false);
      var flyout = (Flyout)this.FindName("RightFlyout");
      if (flyout == null) return;

      var wce = new WindowConfigEditor(config)
      {
        OnSave = c =>
        {
          config.Update(c);
          ShowItemMessageToast(config.Name, "added to window configuration list.");
        },
        OnClose = () =>
        {
          flyout.IsOpen = false;
          SetDockableWindowVisibility(true);
        }
      };

      flyout.Content = wce;
      flyout.IsOpen = true;
    }

    public void HandleHostConfigEdit()
    {
      SetDockableWindowVisibility(false);

      var flyout = (Flyout) this.FindName("RightFlyout");
      if (flyout == null) return;
      var containerConfigEditor = new HostContainerConfigEditor(
        this.HostContainerConfig, new SizeInfo((int) this.ActualWidth, (int) this.ActualHeight))
      {
        DisplayContainersSelectable = false,
        OnSave = (config) =>
        {
          // Update host container info.
          this.HostContainerConfig.Update(config);
          RefreshDisplayContainer();

          // TODO: Hack to update layout info in original config.
          foreach (var wc in this.ProfileManager.ActiveProfile.Windows)
          {
            foreach (var d in config.DockedWindows)
            {
              if (d.WindowConfig.Name == wc.Name)
              {
                var li = d.WindowConfig.HostContainerLayoutInfo;
                wc.HostContainerLayoutInfo = new HostContainerLayoutInfo(li.Row, li.Column, li.HostContainerId);
                wc.Save();
              }
            }
          }
        }
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
      if (message == null) return;

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
