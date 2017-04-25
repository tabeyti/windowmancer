﻿using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using WindowmancerWPF.Models;
using WindowmancerWPF.Practices;
using WindowmancerWPF.Services;
using Button = System.Windows.Controls.Button;
using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;
using MahApps.Metro.Controls.Dialogs;

namespace WindowmancerWPF.UI
{
  /// <summary>
  /// Interaction logic for WindowConfigDialog.xaml
  /// </summary>
  /// 
  public partial class WindowConfig
  {
    public event Action OnClose;

    public Action<WindowInfo> OnSave;

    // Binding objects.
    public bool DisplayHelperPreview { get; set; }
    public ScreenAspectRatio ScreenAspectRatio { get; set; }
    public WindowInfo WindowInfo { get; set; }

    private Screen _currentDisplay;
    private DisplaySection _displaySection;
    private WindowHighlight _windowHighlight;
    private Process _process;

    private readonly List<Button> _displaySectionButtons = new List<Button>();
    private readonly SolidColorBrush _defaultBrush = Brushes.Turquoise;

    public WindowConfig(Action<WindowInfo> onSave)
    {
      this.OnSave = onSave;
      PreInitialization();
      InitializeComponent();
      Initialize();
    }

    public WindowConfig(WindowInfo windowInfo, Action<WindowInfo> onSave)
    {
      this.OnSave = onSave;
      this.WindowInfo = (WindowInfo)windowInfo?.Clone();
      PreInitialization();
      InitializeComponent();
      Initialize();
    }

    public WindowConfig(Process process, Action<WindowInfo> onSave)
    {
      this.OnSave = onSave;
      _process = process;
      PreInitialization();
      InitializeComponent();
      Initialize();
    }

    /// <summary>
    /// Initializes data bound objects and other dependencies of xaml prior
    /// to component initialization.
    /// </summary>
    private void PreInitialization()
    {
      this.ScreenAspectRatio = new ScreenAspectRatio(Screen.PrimaryScreen);

      if (_process != null)
      {
        var procRec = Helper.GetProcessWindowRec(_process);
        this.WindowInfo = new WindowInfo
        {
          Name = _process.MainWindowTitle,
          LayoutInfo = new WindowLayoutInfo(
            procRec.Left, 
            procRec.Top, 
            procRec.Width, 
            procRec.Height),
          MatchCriteria = new WindowMatchCriteria { MatchString = _process.MainWindowTitle }
        };
      }
      else if (this.WindowInfo == null)
      {
        this.WindowInfo = new WindowInfo();
        _process = WindowManager.GetProcess(this.WindowInfo);
      }
    }

    private void Initialize()
    {
      this.DisplayHelperPreview = false;
      this.RowSpinner.ValueChanged += RowColSpinners_ValueChanged;
      this.ColumnSpinner.ValueChanged += RowColSpinners_ValueChanged;

      this.DisplayListBox.ItemsSource = Screen.AllScreens;
      this.DisplayListBox.SelectedItem = _currentDisplay = Screen.PrimaryScreen;
      this.DisplayHelperControl.IsEnabled = false;
    }

    private void LoadProcessInfo()
    {
      var rec = GetWindowRec();
      //this.WindowInfo.LayoutInfo.PositionInfo.X = rec.Left;

      //this.Position.X = rec.Left;
      //this.Position.Y = rec.Top;
      //this.Size.Width = rec.Width;
      //this.Size.Height = rec.Height;

      this.NameTextBox.Text = _process.MainWindowTitle;
      this.RegexMatchStringTextBox.Text = _process.MainWindowTitle;
      this.MatchByWindowTitleRadioButton.IsChecked = true;
    }

    private void LoadWindowInfo()
    {
      this.MatchByWindowTitleRadioButton.IsChecked = true;
      this.NameTextBox.Text = this.WindowInfo.Name;
      this.RegexMatchStringTextBox.Text = this.WindowInfo.MatchCriteria.MatchString;
    }

    private Win32.RECT GetWindowRec()
    {
      var info = new Win32.WINDOWINFO();
      var rec = new Win32.RECT();

      Win32.GetWindowInfo(_process.MainWindowHandle, ref info);
      rec = info.rcWindow;

      if (!Win32.IsIconic(_process.MainWindowHandle))
      {
        return rec;
      }
      rec = Win32.GetPlacement(_process.MainWindowHandle);
      return rec;
    }

    private void ClearDisplaySectionPanel()
    {
      this.DisplayPanelGrid.Children.RemoveRange(0, this.DisplayPanelGrid.Children.Count);
    }

    private void RecreateDisplaySectionControl(int rows, int cols)
    {
      ClearDisplaySectionPanel();
      var grid = new UniformGrid { Rows = rows, Columns = cols };
      _displaySectionButtons.Clear();
      Enumerable.Range(0, rows).ForEach(r =>
        Enumerable.Range(0, cols).ForEach(c =>
        {
          var button = new Button
          {
            Content = ((r * cols) + c).ToString(),
            Background = _defaultBrush,
            Foreground = Brushes.Black,
            Style = (Style)FindResource("SquareButtonStyle")
        };
          button.Click += DisplaySection_OnClick;
          button.Tag = new DisplaySection
          {
            RowIndex = r,
            ColumnIndex = c,
            TotalRows = rows,
            TotalColumns = cols
          };
          _displaySectionButtons.Add(button);
          grid.Children.Add(button);
        })
      );
      this.DisplayPanelGrid.Children.Add(grid);

      // Select first button.
      _displaySectionButtons.First().RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
    }

    private void UpdateLayoutValues()
    {
      var screen = _currentDisplay;

      var screenWidth = screen.Bounds.Width;
      var screenHeight = screen.Bounds.Height;

      var row = _displaySection.RowIndex;
      var col = _displaySection.ColumnIndex;

      var totalRows = _displaySection.TotalRows;
      var totalCols = _displaySection.TotalColumns;

      var x = (screenWidth / totalCols) * col + screen.Bounds.X;
      var y = (screenHeight / totalRows) * row + screen.Bounds.Y;

      var width = (screenWidth / totalCols);
      var height = (screenHeight / totalRows);

      this.WindowInfo.LayoutInfo.SizeInfo.Width = width;
      this.WindowInfo.LayoutInfo.SizeInfo.Height = height;
      this.WindowInfo.LayoutInfo.PositionInfo.X = x;
      this.WindowInfo.LayoutInfo.PositionInfo.Y = y;
    }

    private void SaveConfig()
    {
      var matchType = this.MatchByProcesNameRadioButton.IsChecked.Value ?
        WindowMatchCriteriaType.ProcessName : WindowMatchCriteriaType.WindowTitle;

      this.WindowInfo.Name = this.NameTextBox.Text;
      this.WindowInfo.MatchCriteria = new WindowMatchCriteria(matchType,
        this.RegexMatchStringTextBox.Text);
      this.WindowInfo.LayoutInfo.PositionInfo.X = (int)this.XSpinner.Value;
      this.WindowInfo.LayoutInfo.PositionInfo.Y = (int)this.YSpinner.Value;
      this.WindowInfo.LayoutInfo.SizeInfo.Width = (int)this.WidthSpinner.Value;
      this.WindowInfo.LayoutInfo.SizeInfo.Height = (int)this.HeightSpinner.Value;
    }

    private void DisplaySection_OnClick(object sender, EventArgs e)
    {
      // Reset the previous button highlight.
      _displaySectionButtons.ForEach(b => b.Background = _defaultBrush);

      // Highlight clicked button.
      var button = (Button)sender;
      button.Background = Brushes.Yellow;
      _displaySection = (DisplaySection)button.Tag;
      UpdateLayoutValues();
      UpdateScreenHighlight();
    }

    private void UpdateScreenHighlight()
    {
      if (!this.DisplayHelperPreview)
      {
        return;
      }

      // Move process window if process is active.
      if (null != _process)
      {
        WindowManager.ApplyWindowLayout(this.WindowInfo.LayoutInfo, _process);
      }

      // Hightlight where process window layout.
      if (null == _windowHighlight)
      {
        _windowHighlight = new WindowHighlight();
      }
      _windowHighlight.UpdateLayout(this.WindowInfo.LayoutInfo);
      _windowHighlight.Show();
    }

    private void DisableScreenHighlight()
    {
      _windowHighlight?.Close();
    }

    private void EnableDisplayHelper()
    {
      this.DisplayHelperControl.IsEnabled = true;
      RecreateDisplaySectionControl((int)this.RowSpinner.Value, (int)this.ColumnSpinner.Value);
    }

    private void DisableDisplayHelper()
    {
      this.DisplayHelperControl.IsEnabled = false;
      ClearDisplaySectionPanel();
      _windowHighlight?.Close();
      _windowHighlight = null;
    }

    private void Close()
    {
      var window = Window.GetWindow(this);
      if (window == null)
      {
        throw new Exception("WindowConfig - Could locate active window to unbind the KeyDown listener.");
      }
      window.KeyDown -= WindowConfig_HandleKeyPress;
      OnClose?.Invoke();
    }

    private void RowColSpinners_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      var rows = this.RowSpinner?.Value ?? 1;
      var cols = this.ColumnSpinner?.Value ?? 1;
      RecreateDisplaySectionControl(rows, cols);
    }

    private void DisplaysComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      _currentDisplay = (Screen)this.DisplayListBox.SelectedItem;

      this.ScreenAspectRatio = new ScreenAspectRatio(_currentDisplay);

      var length = _currentDisplay.Bounds.Height > _currentDisplay.Bounds.Width ? _currentDisplay.Bounds.Height : _currentDisplay.Bounds.Width;

      if (_currentDisplay.Bounds.Height > _currentDisplay.Bounds.Width)
      {
        this.DisplayPanel.Height = this.DisplayPanel.MaxHeight;
        this.DisplayPanel.Width = this.DisplayPanel.MaxHeight * (this.ScreenAspectRatio.XRatio/this.ScreenAspectRatio.YRatio);
      }
      else
      {
        this.DisplayPanel.Width = this.DisplayPanel.MaxWidth;
        this.DisplayPanel.Height = this.DisplayPanel.MaxWidth * (this.ScreenAspectRatio.YRatio/ this.ScreenAspectRatio.XRatio);
      }
      
      if (_displaySection == null) return;
      UpdateLayoutValues();
      UpdateScreenHighlight();
    }

    private void OkayButton_Click(object sender, RoutedEventArgs e)
    {
      SaveConfig();
      OnSave?.Invoke(this.WindowInfo);
      Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      if (this.LayoutTab.IsSelected)
      {
        this.WindowLayoutGroupBox.IsEnabled = false;
        EnableDisplayHelper();
        return;
      }
      this.WindowLayoutGroupBox.IsEnabled = true;
      DisableDisplayHelper();
    }

    private void PreviewCheckBox_OnChecked(object sender, RoutedEventArgs e)
    {
      if (this.PreviewCheckBox.IsChecked.Value)
      {
        UpdateScreenHighlight();
        return;
      }
      DisableScreenHighlight();
    }

    private void WindowConfig_OnLoaded(object sender, RoutedEventArgs e)
    {
      var window = Window.GetWindow(this);
      if (window == null)
      {
        throw new Exception("WindowConfig - Could locate active window to bind the KeyDown listener.");
      }
      window.KeyDown += WindowConfig_HandleKeyPress;
    }

    private void WindowConfig_HandleKeyPress(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key != Key.Escape)
      {
        return;
      }
      Close();
    }
  }

  class DisplaySection
  {
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public int TotalRows { get; set; }
    public int TotalColumns { get; set; }

    public DisplaySection()
    {
    }

    public DisplaySection(int rowIndex, int columnIndex, int totalRows, int totalColumns)
    {
      this.RowIndex = rowIndex;
      this.ColumnIndex = columnIndex;
      this.TotalRows = totalRows;
      this.TotalColumns = totalColumns;
    }
  }
}
