using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Media;
using MahApps.Metro.Controls;
using WindowmancerWPF.Models;
using WindowmancerWPF.Practices;
using WindowmancerWPF.Services;
using Button = System.Windows.Controls.Button;
using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;

namespace WindowmancerWPF.UI
{
  /// <summary>
  /// Interaction logic for WindowConfigDialog.xaml
  /// </summary>
  /// 
  public partial class WindowConfig
  {
    public PositionInfo Position { get; set; }
    public SizeInfo Size { get; set; }
    public WindowInfo WindowInfo { get; set; }
    public bool DisplayHelperPreview { get; set; }

    private Screen _currentDisplay;
    private DisplaySection _displaySection;
    private WindowHighlight _windowHighlight;
    private Process _process;

    private readonly List<Button> _displaySectionButtons = new List<Button>();
    private readonly SolidColorBrush _defaultBrush = Brushes.Turquoise;

    public WindowConfig()
    {
      PreInitialization();
      InitializeComponent();
      Initialize();
    }

    public WindowConfig(WindowInfo windowInfo)
    {
      this.WindowInfo = windowInfo;
      _process = WindowManager.GetProcess(this.WindowInfo);
      PreInitialization();
      InitializeComponent();
      Initialize();
    }

    public WindowConfig(Process process)
    {
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
      //this.DialogResult = false;
      this.Size = new SizeInfo();
      this.Position = new PositionInfo();
    }

    private void Initialize()
    {
      this.DisplayHelperPreview = false;
      if (null != WindowInfo)
      {
        LoadWindowInfo();
      }
      else if (null != _process)
      {
        LoadProcessInfo();
      }

      this.RowSpinner.ValueChanged += RowColSpinners_ValueChanged;
      this.ColumnSpinner.ValueChanged += RowColSpinners_ValueChanged;

      this.DisplayListBox.ItemsSource = Screen.AllScreens;
      this.DisplayListBox.SelectedItem = _currentDisplay = Screen.PrimaryScreen;
      this.DisplayHelperControl.IsEnabled = false;
    }

    private void LoadProcessInfo()
    {
      var rec = GetWindowRec();
      this.Position.X = rec.Left;
      this.Position.Y = rec.Top;
      this.Size.Width = rec.Width;
      this.Size.Height = rec.Height;

      this.NameTextBox.TextBox = _process.MainWindowTitle;
      this.RegexMatchStringTextBox.TextBox = _process.MainWindowTitle;
      this.MatchByWindowTitleRadioButton.IsChecked = true;
    }

    private void LoadWindowInfo()
    {
      this.Position.X = this.WindowInfo.LayoutInfo.PositionInfo.X;
      this.Position.Y = this.WindowInfo.LayoutInfo.PositionInfo.Y;
      this.Size.Width = this.WindowInfo.LayoutInfo.SizeInfo.Width;
      this.Size.Height = this.WindowInfo.LayoutInfo.SizeInfo.Height;

      this.MatchByWindowTitleRadioButton.IsChecked = true;
      this.NameTextBox.TextBox = this.WindowInfo.Name;
      this.RegexMatchStringTextBox.TextBox = this.WindowInfo.MatchCriteria.MatchString;
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
      //_logger.Info($"{this} - Process {_proc.ProcessName} is minimized. Getting maxmized position/size.");
      rec = Win32.GetPlacement(_process.MainWindowHandle);
      return rec;
    }

    private void ClearDisplaySectionPanel()
    {
      this.DisplayPanel.Children.RemoveRange(0, this.DisplayPanel.Children.Count);
    }

    private void RecreateDisplaySectionControl(int rows, int cols)
    {
      ClearDisplaySectionPanel();
      var grid = new UniformGrid { Rows = rows, Columns = cols };
      _displaySectionButtons.Clear();
      Enumerable.Range(0, rows).ForEach(r =>
        Enumerable.Range(0, cols).ForEach(c =>
        {
          var button = new Button { Content = ((r * cols) + c).ToString(), Background = _defaultBrush, Foreground = Brushes.Black };
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
      this.DisplayPanel.Children.Add(grid);

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
      this.Size.Width = width;
      this.Size.Height = height;
      this.Position.X = x;
      this.Position.Y = y;
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
        WindowManager.ApplyWindowLayout(new WindowLayoutInfo
        {
          PositionInfo = new PositionInfo { X = this.Position.X, Y = this.Position.Y },
          SizeInfo = new SizeInfo { Width = this.Size.Width, Height = this.Size.Height }
        }, _process);
      }

      // Hightlight where process window layout.
      if (null == _windowHighlight)
      {
        _windowHighlight = new WindowHighlight();
      }
      _windowHighlight.UpdateLayout(this.Position.X, this.Position.Y, this.Size.Width, this.Size.Height);
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

    private void RowColSpinners_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      var rows = this.RowSpinner?.Value ?? 1;
      var cols = this.ColumnSpinner?.Value ?? 1;
      RecreateDisplaySectionControl(rows, cols);
    }

    private void DisplaysComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      _currentDisplay = (Screen)this.DisplayListBox.SelectedItem;
      if (_displaySection != null)
      {
        UpdateLayoutValues();
        UpdateScreenHighlight();
      }
    }

    private void OkayButton_Click(object sender, RoutedEventArgs e)
    {
      //this.DialogResult = true;
      //this.Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      //this.Close();
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
  }
}
