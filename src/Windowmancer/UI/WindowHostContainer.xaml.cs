using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using MahApps.Metro.Controls;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Services;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for WindowHostContainer.xaml
  /// </summary>
  public partial class WindowHostContainer : MetroWindow
  {
    public int CurrentRowIndex { get; private set; }
    public int CurrentColumnIndex { get; private set; }

    private static readonly int _titlebarHeight = (int)SystemParameters.WindowCaptionHeight + 10;

    public string ContainerLabel { get; private set; }

    public DisplayContainer DisplayContainer { get; set; }

    public WindowHostContainer(DisplayContainer displayContiner)
    {
      this.DisplayContainer = displayContiner;
      InitializeComponent();
    }

    public WindowHostContainer()
    {
      this.DisplayContainer = new DisplayContainer("Default", 0, 0, 1, 1);
      Initialize();
    }

    private void Initialize()
    {
      this.Closing += (o, e) =>
      {
        foreach (var dw in this.DisplayContainer.DockedWindows)
        {
          dw.Process?.Kill();
        }
      };
    }

    public void DockProc(Process process, int rowIndex, int columnIndex)
    {
      if (this.DisplayContainer.DockedWindows.Any(dw => dw.Process.Handle == process.Handle))
      {
        return;
      }

      var windowToDock = new DockedWindow(process)
      {
        Row = rowIndex,
        Column = columnIndex
      };


      this.DisplayContainer.DockedWindows.Add(windowToDock);
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
        if (process.HasExited)
        {
          return;
        }
      }
      // Dock it, bitch!
      RefreshDockedWindow(windowToDock);
    }

    /// <summary>
    /// Docks the passed process window on the next available row/column
    /// index.
    /// </summary>
    /// <param name="process"></param>
    public void DockProc(Process process)
    {
      if (this.CurrentColumnIndex >= this.DisplayContainer.Columns)
      {
        if (this.CurrentRowIndex >= this.DisplayContainer.Rows)
        {
          return;
        }
        this.CurrentRowIndex++;
        this.CurrentColumnIndex = 0;
      }
      DockProc(process, this.CurrentRowIndex, this.CurrentColumnIndex++);
    }

    private void RefreshDockedWindow(DockedWindow windowToDock)
    {
      // Retrieve the window handle for this host container window.
      var window = Window.GetWindow(this);
      if (null == window)
      {
        throw new Exception($"{this}.RefreshDockedWindow - Could not retrieve the host container window.");
      }
      var wih = new WindowInteropHelper(window);
      windowToDock.ParentHandle = Win32.SetParent(windowToDock.Process.MainWindowHandle, wih.Handle);

      void Resize(object s, SizeChangedEventArgs ev)
      {
        MoveChildWindow(windowToDock, windowToDock.Row, windowToDock.Column);
      }
      this.SizeChanged += Resize;
      MoveChildWindow(windowToDock, windowToDock.Row, windowToDock.Column);
    }

    private void MoveChildWindow(DockedWindow dockedWindow, int rowIndex, int columnIndex)
    {
      var screenWidth = this.ActualWidth;
      var screenHeight = this.ActualHeight;

      var totalRows = this.DisplayContainer.Rows;
      var totalCols = this.DisplayContainer.Columns;

      var x = (int)(screenWidth / totalCols) * columnIndex;
      var y = (int)(((screenHeight / totalRows) * rowIndex) + _titlebarHeight);

      var width = (int)(screenWidth / totalCols);
      var height = (int)(screenHeight / totalRows);

      Win32.MoveWindow(dockedWindow.Process.MainWindowHandle, x, y, (int)width, (int)height, true);
    }

    public void SetChildWindowsVisible(bool isVisible)
    {
      foreach (var dw in this.DisplayContainer.DockedWindows)
      {
        WindowManager.SetWindowOpacityPercentage(dw.Process, (uint)(isVisible ? 100 : 0));
      }
    }

    private void EditButton_OnClick(object sender, RoutedEventArgs e)
    {
      SetChildWindowsVisible(false);

      var flyout = this.Flyouts.Items[0] as Flyout;
      if (flyout == null) return;
      var displayHelper = new HostContainerHelper(this.DisplayContainer)
      {
        DisplayContainersSelectable = false
      };
      flyout.Content = displayHelper;
      flyout.IsOpen = true;
    }

    private void WindowHostContainer_OnLoaded(object sender, RoutedEventArgs e)
    {
    }
  }
}
