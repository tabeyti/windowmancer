﻿using System;
using System.Diagnostics;
using System.Linq;
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
      var rec = GetWindowRec();

      // Size.
      this.InvokeControl(() => this.WindowSizeBoxWidth.Value = rec.Width);
      this.InvokeControl(() => this.WindowSizeBoxHeight.Value = rec.Height);

      // PositionInfo.
      this.InvokeControl(() => this.WindowLocationBoxX.Value = rec.Left);
      this.InvokeControl(() => this.WindowLocationBoxY.Value = rec.Top);

      // Config name defaults to window title.
      this.WindowConfigNameTextBox.Text = _proc.MainWindowTitle;

      // Match text defaults to window title.
      this.WindowMatchStringTextBox.Text = _proc.MainWindowTitle;

      // Display process is on.
      var screen = Screen.FromHandle(_proc.MainWindowHandle);
      this.WindowLocationDisplayComboBox.SelectedItem = screen;
    }

    private Win32.RECT GetWindowRec()
    {
      var info = new Win32.WINDOWINFO();
      var rec = new Win32.RECT();

      Win32.GetWindowInfo(_proc.MainWindowHandle, ref info);
      rec = info.rcWindow;

      if (!Win32.IsIconic(_proc.MainWindowHandle))
      {
        return rec;
      }
      _logger.Info($"{this} - Process {_proc.ProcessName} is minimized. Getting maxmized position/size.");
      rec = Win32.GetPlacement(_proc.MainWindowHandle);
      return rec;
    }

    private void SaveWindowInfo()
    {
      var matchType = WindowMatchCriteriaType.WindowTitle;
      var radioButton = this.WindowMatchGroupBox.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked);
      if (radioButton == null)
      {
        throw new ExceptionBox($"{this} - Could not locate a selected match type radio button.");
      }
      if (radioButton.Name == "WindowProcessNameRadioButton")
      {
        matchType = WindowMatchCriteriaType.ProcessName;
      }
      else if (radioButton.Name == "WindowTitleRadioButton")
      {
        matchType = WindowMatchCriteriaType.WindowTitle;
      }

      this.WindowInfo = new WindowInfo
      {
        LocationInfo = new LocationInfo
        {
          DisplayName = this.WindowLocationDisplayComboBox.Text,
          PositionInfo = new PositionInfo { X = (int)this.WindowLocationBoxX.Value, Y = (int)this.WindowLocationBoxY.Value}
        },
        SizeInfo = new SizeInfo
        {
          Width = (int)this.WindowSizeBoxWidth.Value,
          Height = (int)this.WindowSizeBoxHeight.Value,
        },
        MatchCriteria = new WindowMatchCriteria(matchType, this.WindowMatchStringTextBox.Text),
        Name = this.WindowConfigNameTextBox.Text,
      };
    }

    private void WindowConfigDialogSaveButton_Click(object sender, EventArgs e)
    {
      SaveWindowInfo();
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

    private void WindowPositionAbsoluteRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      
    }
  }
}
