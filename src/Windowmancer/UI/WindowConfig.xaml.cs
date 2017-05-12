using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Windowmancer.Core.Models;
using Windowmancer.Core.Services;
using Button = System.Windows.Controls.Button;
using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for WindowConfigDialog.xaml
  /// </summary>
  /// 
  public partial class WindowConfig
  {
    public Action OnClose;

    public Action<WindowInfo> OnSave;

    // Binding objects.
    public bool DisplayHelperPreview { get; set; }
    public ScreenAspectRatio ScreenAspectRatio { get; set; }
    public WindowInfo WindowInfo { get; set; }

    private static Screen _screen;
    private static DisplaySection _displaySection = new DisplaySection();
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
        this.WindowInfo = WindowInfo.FromProcess(_process);
      }
      else
      {
        this.WindowInfo = this.WindowInfo?? new WindowInfo();
        _process = WindowManager.GetProcess(this.WindowInfo);
      }
    }

    private void Initialize()
    {
      EnableDisableLayoutHelper(false); 
      this.RowSpinner.ValueChanged += RowColSpinners_ValueChanged;
      this.ColumnSpinner.ValueChanged += RowColSpinners_ValueChanged;

      this.DisplayListBox.ItemsSource = Screen.AllScreens;

      _screen = _screen ?? Screen.PrimaryScreen;
      this.DisplayListBox.SelectedItem = _screen;
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
            Style = (Style)FindResource("SquareButtonStyle"),
            IsEnabled = true
        };
          button.Click += DisplaySection2_OnClick;
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

      var dsb = _displaySectionButtons.Find(d =>
      {
        var ds = (DisplaySection) d.Tag;
        return ds.ColumnIndex == _displaySection.ColumnIndex &&
               ds.RowIndex == _displaySection.RowIndex;
      });

      _displaySection = (DisplaySection)dsb.Tag;

      // Select first button.
      dsb.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
    }

    /// <summary>
    /// Updates our WindowInfo's layout values with values gathered
    /// from the current display section and screen.
    /// </summary>
    private void UpdateLayoutValuesFromDisplayHelper()
    {
      var screenWidth = _screen.Bounds.Width;
      var screenHeight = _screen.Bounds.Height;

      var row = _displaySection.RowIndex;
      var col = _displaySection.ColumnIndex;

      var totalRows = _displaySection.TotalRows;
      var totalCols = _displaySection.TotalColumns;

      var x = (screenWidth / totalCols) * col + _screen.Bounds.X;
      var y = (screenHeight / totalRows) * row + _screen.Bounds.Y;

      var width = (screenWidth / totalCols);
      var height = (screenHeight / totalRows);

      this.WindowInfo.LayoutInfo.SizeInfo.Width = width;
      this.WindowInfo.LayoutInfo.SizeInfo.Height = height;
      this.WindowInfo.LayoutInfo.PositionInfo.X = x;
      this.WindowInfo.LayoutInfo.PositionInfo.Y = y;
    }

    /// <summary>
    /// Saves all input data into the WindowInfo instance.
    /// </summary>
    private void SaveConfig()
    {
      var matchType = this.MatchByProcesNameRadioButton.IsChecked.Value ?
        WindowMatchCriteriaType.ProcessName : WindowMatchCriteriaType.WindowTitle;

      this.WindowInfo.Name = this.NameTextBox.Text;
      this.WindowInfo.MatchCriteria.MatchType = matchType;
      this.WindowInfo.MatchCriteria.MatchString = this.RegexMatchStringTextBox.Text;
      this.WindowInfo.LayoutInfo.PositionInfo.X = (int)this.XSpinner.Value;
      this.WindowInfo.LayoutInfo.PositionInfo.Y = (int)this.YSpinner.Value;
      this.WindowInfo.LayoutInfo.SizeInfo.Width = (int)this.WidthSpinner.Value;
      this.WindowInfo.LayoutInfo.SizeInfo.Height = (int)this.HeightSpinner.Value;
    }

    private void DisplaySection2_OnClick(object sender, EventArgs e)
    {
      // Reset the previous button highlight.
      _displaySectionButtons.ForEach(b => b.Background = _defaultBrush);

      // Highlight clicked button.
      var button = (Button)sender;
      button.Background = Brushes.Yellow;
      _displaySection = (DisplaySection)button.Tag;
      UpdateScreenHighlight();
    }

    private void UpdateScreenHighlight()
    {
      if (!this.DisplayHelperPreview)
      {
        return;
      }

      var layoutInfo = _displaySection.GetLayoutInfo(_screen);

      // Move process window if process is active.
      if (null != _process)
      {
        WindowManager.ApplyWindowLayout(layoutInfo, _process);
      }

      var window = Window.GetWindow(this);
      window?.Activate();

      // Hightlight where process window layout.
      if (null == _windowHighlight)
      {
        _windowHighlight = new WindowHighlight();
      }

      _windowHighlight.UpdateLayout(layoutInfo);
      _windowHighlight.Show();
    }

    private void DisableScreenHighlight()
    {
      _windowHighlight?.Close();
      _windowHighlight = null;
    }
    
    private void EnableDisableLayoutHelper(bool enable)
    {
      if (enable)
      {
        _rowColSpinnerEnabled = false;
        this.RowSpinner.Value = _displaySection.TotalRows;
        this.ColumnSpinner.Value = _displaySection.TotalColumns;
        _rowColSpinnerEnabled = true;
        RecreateDisplaySectionControl(_displaySection.TotalRows, _displaySection.TotalColumns);
        return;
      }
      
      ClearDisplaySectionPanel();
      if (this.PreviewCheckBox.IsChecked.Value)
      {
        this.PreviewCheckBox.IsChecked = false;
        DisableScreenHighlight();
      }
    }
    
    private void Close()
    {
      var window = Window.GetWindow(this);
      if (window == null)
      {
        throw new Exception("WindowConfig - Could locate active window to unbind the KeyDown listener.");
      }
      window.KeyDown -= WindowConfig_HandleKeyPress;
      _windowHighlight?.Close();
      OnClose?.Invoke();
    }

    private bool _rowColSpinnerEnabled = true;
    private void RowColSpinners_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      if (!_rowColSpinnerEnabled) return;

      var rows = this.RowSpinner?.Value ?? 1;
      var cols = this.ColumnSpinner?.Value ?? 1;
      RecreateDisplaySectionControl(rows, cols);
    }

    private void DisplaysComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      _screen = (Screen)this.DisplayListBox.SelectedItem;

      this.ScreenAspectRatio = new ScreenAspectRatio(_screen);

      var length = _screen.Bounds.Height > _screen.Bounds.Width ? _screen.Bounds.Height : _screen.Bounds.Width;

      if (_screen.Bounds.Height > _screen.Bounds.Width)
      {
        this.DisplayPanel.Height = this.DisplayPanel.MaxHeight;
        this.DisplayPanel.Width = this.DisplayPanel.MaxHeight * (this.ScreenAspectRatio.XRatio/this.ScreenAspectRatio.YRatio);
      }
      else
      {
        this.DisplayPanel.Width = this.DisplayPanel.MaxWidth;
        this.DisplayPanel.Height = this.DisplayPanel.MaxWidth * (this.ScreenAspectRatio.YRatio/ this.ScreenAspectRatio.XRatio);
      }
      UpdateScreenHighlight();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
      SaveConfig();
      OnSave?.Invoke(this.WindowInfo);
      Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
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
    
    private void LayoutHelperExpander_OnExpanded(object sender, RoutedEventArgs e)
    {
      var expander = (sender as System.Windows.Controls.Expander);
      if (expander?.IsExpanded == null) return;
      EnableDisableLayoutHelper(expander.IsExpanded);
    }

    private void SetDisplayHelperLayoutButton_OnClick(object sender, RoutedEventArgs e)
    {
      UpdateLayoutValuesFromDisplayHelper();
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
      this.RowIndex = this.ColumnIndex = 0;
      this.TotalRows = this.TotalColumns = 1;
    }

    public DisplaySection(int rowIndex, int columnIndex, int totalRows, int totalColumns)
    {
      this.RowIndex = rowIndex;
      this.ColumnIndex = columnIndex;
      this.TotalRows = totalRows;
      this.TotalColumns = totalColumns;
    }

    public WindowLayoutInfo GetLayoutInfo(Screen screen)
    {
      var screenWidth = screen.Bounds.Width;
      var screenHeight = screen.Bounds.Height;

      var row = this.RowIndex;
      var col = this.ColumnIndex;

      var totalRows = this.TotalRows;
      var totalCols = this.TotalColumns;

      var x = (screenWidth / totalCols) * col + screen.Bounds.X;
      var y = (screenHeight / totalRows) * row + screen.Bounds.Y;

      var width = (screenWidth / totalCols);
      var height = (screenHeight / totalRows);

      return new WindowLayoutInfo(x, y, width, height);
    }
  }
}

