﻿using System;
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
  /// <inheritdoc cref="IHostContainerWindow" />
  public partial class HostContainer : IToastHost, IHostContainerWindow
  {
    public HostContainerConfig HostContainerConfig { get; set; }
    
    private int CurrentRowIndex { get; set; }
    private int CurrentColumnIndex { get; set; }
    private ProfileManager ProfileManager { get; set; }

    private static readonly int _titlebarHeight = (int)SystemParameters.WindowCaptionHeight + 10;
    private readonly bool _stupidFlag = false;

    public HostContainer(HostContainerConfig hostContainer, bool enableEditorOnLoad = false)
    {
      _stupidFlag = enableEditorOnLoad;
      hostContainer.IsActive = true;
      this.HostContainerConfig = hostContainer;
      InitializeComponent();
      Initialize();
    }

    private void Initialize()
    {
      this.ProfileManager = Helper.ServiceResolver.Resolve<ProfileManager>();

      // Resolve all window configurations pertaining to
      // this host container and add dockable windows for them.
      var wcm = Helper.ServiceResolver.Resolve<WindowConfigManager>();
      foreach (var w in wcm.GetWindowConfigs(this.HostContainerConfig))
      {
        DockProcInternal(null, w);
      }

      this.Closing += (o, e) =>
      {
        foreach (var dw in this.HostContainerConfig.DockedWindows)
        {
          if (dw.Process == null) continue;

          if (!dw.Process.HasExited)
          {
            dw.Process.Kill();
          }
        }
      };
    }

    public void OpenEditor()
    {
      Helper.Dispatcher.Invoke(HandleHostConfigEdit);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void ActivateWindow()
    {
      Helper.Dispatcher.Invoke(() => this.Activate());
    }

    /// <inheritdoc />
    public void DockNew(Process process, WindowConfig windowConfig)
    {
      Helper.Dispatcher.Invoke(() => DockNewProcInternal(process, windowConfig));
    }

    /// <inhertidoc />
    public void Dock(Process process, WindowConfig windowConfig)
    {
      Helper.Dispatcher.Invoke(() => DockProcInternal(process, windowConfig));
    }

    /// <inheritdoc />
    private void DockNewProcInternal(Process process, WindowConfig windowConfig)
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
      layoutInfo.Row = (uint)this.CurrentRowIndex;
      layoutInfo.Column = (uint)this.CurrentColumnIndex;

      Dock(process, windowConfig);
    }

    private void DockProcInternal(Process process, WindowConfig windowConfig)
    {
      // We are passing an process-empty DockedWindow container. Add it.
      if (null == process)
      {
        this.HostContainerConfig.DockedWindows.Add(new DockableWindow(windowConfig));
        return;
      }

      // Check if we are adding the same process twice.
      if (this.HostContainerConfig.DockedWindows.Any(dw => dw.Process?.Handle == process.Handle))
      {
        throw new Exception("You are docking the same process twice. Just herpin and derpin, aren't ya?");
      }

      DockableWindow dockableWindow;

      // If there is already an existing docked window object for this window config, use it.
      var existingDw = this.HostContainerConfig.DockedWindows.Find(dw => dw.WindowConfig.Equals(windowConfig));
      if (existingDw != null)
      {
        // If this dockable window already has a process tied to it, they are trying to add the same thing twice.
        if (existingDw.Process != null)
        {
          throw new Exception("You are docking the same window config twice. For cryin' out loud!");
        }
        existingDw.Process = process;
        dockableWindow = existingDw;
      }
      else
      {
        dockableWindow = new DockableWindow(process, windowConfig);
        this.HostContainerConfig.DockedWindows.Add(dockableWindow);
      }
      
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
      dockableWindow.ParentHandle = Win32.SetParent(dockableWindow.Process.MainWindowHandle, wih.Handle);

      void Resize(object s, SizeChangedEventArgs ev)
      {
        RefreshDockableWindow(dockableWindow);
      }

      this.SizeChanged += Resize;
      RefreshDockableWindow(dockableWindow);
    }

    /// <inheritdoc />
    public void UnDock(WindowConfig windowConfig)
    {
      var dockableWindow = this.HostContainerConfig.DockedWindows.Find(dw => dw.WindowConfig == windowConfig);
      if (null == dockableWindow) return;

      // If there is a tied process to this window config, just kill it and 
      // cleanup will happen naturally.
      if (dockableWindow.Process != null)
      {
        dockableWindow.Process.Kill();
        return;
      }

      this.HostContainerConfig.DockedWindows.Remove(dockableWindow);
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
          
          // TODO: Hack to update layout info in original config.
          foreach (var wc in this.ProfileManager.ActiveProfile.Windows)
          {
            foreach (var d in config.DockedWindows)
            {
              if (d.WindowConfig.Name == wc.Name)
              {
                var li = d.WindowConfig.HostContainerLayoutInfo;
                wc.HostContainerLayoutInfo.Update(li); 
                wc.Save();
              }
            }
          }
          
          RefreshHostContainer(config);
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

    private void RefreshHostContainer(HostContainerConfig config)
    {
      foreach (var d in config.DockedWindows)
      {
        RefreshDockableWindow(d);
      }
    }

    private void RefreshDockableWindow(DockableWindow dockableWindow)
    {
      if (null == dockableWindow.Process) return;

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

    /// <inheritdoc />
    void IHostContainerWindow.Close()
    {
      this.Close();
    }

    private void HostContainer_OnClosed(object sender, EventArgs e)
    {
      this.HostContainerConfig.DockedWindows.Clear();
      this.HostContainerConfig.IsActive = false;

      var hcm = App.ServiceResolver.Resolve<HostContainerManager>();
      hcm.RemoveHostContainerWindow(this);
    }

    public new void Show()
    {
      Helper.Dispatcher.Invoke(() =>
      {
        this.WindowState = WindowState.Normal;
        base.Show();
      });
    }
  }
}
