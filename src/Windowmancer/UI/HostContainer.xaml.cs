using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using MahApps.Metro.Controls;
using Windowmancer.Core.Extensions;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Services;
using Windowmancer.Core.Services.Base;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for HostContainer.xaml
  /// </summary>
  public partial class HostContainer : IToastHost
  {
    public int CurrentRowIndex { get; private set; }
    public int CurrentColumnIndex { get; private set; }
    public HostContainerConfig WindowContainer { get; set; }

    private static readonly int _titlebarHeight = (int)SystemParameters.WindowCaptionHeight + 10;

    public HostContainer(HostContainerConfig windowContiner)
    {
      this.WindowContainer = windowContiner;
      InitializeComponent();
    }

    public HostContainer()
    {
      this.WindowContainer = new HostContainerConfig("Default", 1, 1);
      Initialize();
    }

    private void Initialize()
    {
      this.Closing += (o, e) =>
      {
        foreach (var dw in this.WindowContainer.DockedWindows)
        {
          dw.Process?.Kill();
        }
      };
    }

    /// <summary>
    /// Docks the passed process window on the next available row/column
    /// index.
    /// </summary>
    /// <param name="process"></param>
    public void DockProc(Process process)
    {
      if (this.CurrentColumnIndex >= this.WindowContainer.Columns)
      {
        if (this.CurrentRowIndex >= this.WindowContainer.Rows)
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
      if (this.WindowContainer.DockedWindows.Any(dw => dw.Process.Handle == process.Handle))
      {
        return;
      }

      var windowToDock = new DockableWindow(process) { Row = rowIndex, Column = columnIndex };
      this.WindowContainer.DockedWindows.Add(windowToDock);
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
            var d = this.WindowContainer.DockedWindows.Find(dw => dw.Process.Id == process.Id);
            this.WindowContainer.DockedWindows.Remove(d);
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
    
    private void RefreshDisplayContainer()
    {
      foreach (var d in this.WindowContainer.DockedWindows)
      {
        RefreshDockableWindow(d);
      }
    }

    private void RefreshDockableWindow(DockableWindow dockableWindow)
    {
      var screenWidth = this.ActualWidth;
      var screenHeight = this.ActualHeight;

      var totalRows = this.WindowContainer.Rows;
      var totalCols = this.WindowContainer.Columns;

      var x = (int)(screenWidth / totalCols) * dockableWindow.Column;
      var y = (int)(((screenHeight / totalRows) * dockableWindow.Row) + _titlebarHeight);

      var width = (int)(screenWidth / totalCols);
      var height = (int)(screenHeight / totalRows);

      Win32.MoveWindow(dockableWindow.Process.MainWindowHandle, x, y, (int)width, (int)height, true);
    }

    public void SetDockableWindowVisibility(bool isVisible)
    {
      foreach (var dw in this.WindowContainer.DockedWindows)
      {
        MonitorWindowManager.SetWindowOpacityPercentage(dw.Process, (uint)(isVisible ? 100 : 0));
      }
    }

    private void EditButton_OnClick(object sender, RoutedEventArgs e)
    {
      SetDockableWindowVisibility(false);

      var flyout = this.Flyouts.Items[0] as Flyout;
      if (flyout == null) return;
      var hostContainerHelper = new HostContainerConfigEditor(
        this.WindowContainer, 
        new SizeInfo((int)this.ActualWidth, (int)this.ActualHeight))
      { 
        DisplayContainersSelectable = false
      };
      hostContainerHelper.OnSave += (dcs) =>
      {
        this.WindowContainer = dcs;
        RefreshDisplayContainer();
      };
      hostContainerHelper.OnClose += () => 
      {
        flyout.IsOpen = false;
        SetDockableWindowVisibility(true);
      };

      flyout.Content = hostContainerHelper;
      flyout.IsOpen = true;
    }

    private void HostContainer_OnLoaded(object sender, RoutedEventArgs e)
    {
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
  }
}
