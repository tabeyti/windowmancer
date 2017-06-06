using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using MahApps.Metro.Controls;
using Windowmancer.Core.Practices;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for WindowHostContainer.xaml
  /// </summary>
  public partial class WindowHostContainer : MetroWindow
  {
    private readonly Dictionary<IntPtr, DockedWindow> _dockedWindowsDict = new Dictionary<IntPtr, DockedWindow>();

    public int Rows { get; set; }
    public int Columns { get; set; }
    public int CurrentRowIndex { get; private set; }
    public int CurrentColumnIndex { get; private set; }

    private static readonly int _titlebarHeight = (int)SystemParameters.WindowCaptionHeight + 10;

    public string ContainerLabel { get; private set; }

    public WindowHostContainer()
    {
      this.Rows = this.Columns = 1;
      InitializeComponent();

      this.Closing += (o,e) =>
      {
        foreach (var kv in _dockedWindowsDict)
        {
          kv.Value.Process?.Kill();
        }
      };
    }

    public WindowHostContainer(string containerLabel, int rows, int columns)
    {
      this.ContainerLabel = containerLabel;
      this.Rows = rows;
      this.Columns = columns;
    }

    public void DockProc(Process process, int rowIndex, int columnIndex)
    {
      if (_dockedWindowsDict.ContainsKey(process.Handle))
      {
        return;
      }

      var windowToDock = new DockedWindow(process);
      _dockedWindowsDict.Add(process.Handle, windowToDock);
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
      DockIt(windowToDock, rowIndex, columnIndex);
    }

    /// <summary>
    /// Docks the passed process window on the next available row/column
    /// index.
    /// </summary>
    /// <param name="process"></param>
    public void DockProc(Process process)
    {
      if (this.CurrentColumnIndex >= this.Columns)
      {
        if (this.CurrentRowIndex >= this.Rows)
        {
          return;
        }
        this.CurrentRowIndex++;
        this.CurrentColumnIndex = 0;
      }
      DockProc(process, this.CurrentRowIndex, this.CurrentColumnIndex++);
    }

    private void DockIt(DockedWindow windowToDock, int rowIndex, int columnIndex)
    {
      // Retrieve the window handle for this host container window.
      var window = Window.GetWindow(this);
      if (null == window)
      {
        throw new Exception($"{this}.DockIt - Could not retrieve the host container window.");
      }
      var wih = new WindowInteropHelper(window);
      windowToDock.ParentHandle = Win32.SetParent(windowToDock.Process.MainWindowHandle, wih.Handle);

      void Resize(object s, SizeChangedEventArgs ev)
      {
        MoveChildWindow(windowToDock, rowIndex, columnIndex);
      }
      this.SizeChanged += Resize;
      MoveChildWindow(windowToDock, rowIndex, columnIndex);
    }

    private void MoveChildWindow(DockedWindow dockedWindow, int rowIndex, int columnIndex)
    {
      var screenWidth = this.ActualWidth;
      var screenHeight = this.ActualHeight;

      var totalRows = this.Rows;
      var totalCols = this.Columns;

      var x = (int)(screenWidth / totalCols) * columnIndex;
      var y = (int)(((screenHeight / totalRows) * rowIndex) + _titlebarHeight);

      var width = (int)(screenWidth / totalCols);
      var height = (int)(screenHeight / totalRows);

      Win32.MoveWindow(dockedWindow.Process.MainWindowHandle, x, y, (int)width, (int)height, true);
    }
  }

  public class DockedWindow
  {
    public Process Process { get; set; }
    public IntPtr ParentHandle { get; set; }

    public DockedWindow() { }
    public DockedWindow(Process process)
    {
      this.Process = process;
    }
  }
}
