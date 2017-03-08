using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NLog;
using Windowmancer.Extensions;
using Windowmancer.Models;
using Windowmancer.Practices;
using Windowmancer.Pratices;
using Windowmancer.Services;

namespace Windowmancer.UI
{
  public partial class WindowConfigDialog : Form
  { 
    public WindowInfo WindowInfo { get; private set; }

    private readonly Process _proc;
    private ILogger _logger;

    public WindowConfigDialog(Process proc)
    {
      _proc = proc;
      InitializeComponent();
      Initialize();
    }

    private void Initialize()
    {
      // Displays.
      this.WindowLocationDisplayComboBox.DisplayMember = "DeviceName";
      foreach (var s in Screen.AllScreens)
      {
        this.WindowLocationDisplayComboBox.Items.Add(s);
      }
      this.WindowLocationDisplayComboBox.SelectedIndex = 0;
    }
    
    private  void LoadWindowInfo()
    {
      var info = new Win32.WINDOWINFO();
      var rec = new Win32.RECT();
      Win32.GetWindowInfo(_proc.MainWindowHandle, ref info);
      rec = info.rcWindow;

      if (rec.Left < 0) { rec.Left = 0; }
      if (rec.Top < 0) { rec.Top = 0; }
      if (rec.Right < 0) { rec.Right = 0; }
      if (rec.Bottom < 0) { rec.Bottom = 0; }
      _logger.Info($"{info.rcWindow.Left},{info.rcWindow.Top},{info.rcWindow.Right},{info.rcWindow.Bottom}");

      // Size.
      this.InvokeControl(() => this.WindowSizeBoxWidth.Value = rec.Width);
      this.InvokeControl(() => this.WindowSizeBoxHeight.Value = rec.Height);

      // Position.
      this.InvokeControl(() => this.WindowLocationBoxX.Value = rec.Left);
      this.InvokeControl(() => this.WindowLocationBoxY.Value = rec.Top);

      // Match text defaults to window title.
      this.MatchStringTextBox.Text = _proc.MainWindowTitle;

      // Display process is on.
      var screen = Screen.FromHandle(_proc.MainWindowHandle);
      this.WindowLocationDisplayComboBox.SelectedItem = screen;
    }

    private void SaveWindowInfo()
    {
      //var info = new WindowInfo()
      //{
      //  LocationInfo = new LocationInfo
      //  {
          
      //  }
      //}
    }

    private void WindowConfigDialogSaveButton_Click(object sender, EventArgs e)
    {
      this.Dispose();
    }

    private void WindowConfigDialogCancelButton_Click(object sender, EventArgs e)
    {
      this.WindowInfo = null;
      this.Dispose();
    }

    private void WindowConfigDialog_Load(object sender, EventArgs e)
    {
      _logger = LogManager.GetCurrentClassLogger();
      LoadWindowInfo();
    }
  }
}
