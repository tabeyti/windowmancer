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
using Microsoft.Practices.Unity;
using Windowmancer.Core.Extensions;
using Windowmancer.Core.Models;
using Windowmancer.Core.Services;
using Button = System.Windows.Controls.Button;
using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for WindowConfigEditor.xaml
  /// </summary>
  /// 
  public partial class WindowConfigEditor
  {
    public Action OnClose;

    public Action<WindowConfig> OnSave;

    // Binding objects.
    public bool DisplayHelperPreview { get; set; }
    public bool WindowStylingPreview { get; set; }
    public DisplayAspectRatio ScreenAspectRatio { get; set; }
    public WindowConfig WindowConfig { get; set; }
    public MonitorLayoutInfo OriginalLayoutInfo { get; set; }
    public uint OriginalOpacity { get; set; }
    public ProfileManager ProfileManager { get; set; }

    private static Screen _screen;
    private static DisplayHelperSection _displayHelperSection = new DisplayHelperSection();
    private WindowHighlight _windowHighlight;
    private Process _process;

    private readonly List<Button> _displaySectionButtons = new List<Button>();
    private readonly SolidColorBrush _defaultBrush = Brushes.Turquoise;

    public WindowConfigEditor(Action<WindowConfig> onSave)
    {
      this.OnSave = onSave;
      PreInitialization();
      InitializeComponent();
      Initialize();
    }

    public WindowConfigEditor(WindowConfig windowInfo, Action<WindowConfig> onSave)
    {
      this.OnSave = onSave;
      this.WindowConfig = (WindowConfig)windowInfo?.Clone();
      PreInitialization();
      InitializeComponent();
      Initialize();
    }

    public WindowConfigEditor(Process process, Action<WindowConfig> onSave)
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
      this.ScreenAspectRatio = new DisplayAspectRatio(Screen.PrimaryScreen);
      this.ProfileManager = App.ServiceResolver.Resolve<ProfileManager>();
      if (_process != null)
      {
        this.WindowConfig = WindowConfig.FromProcess(_process);
      }
      else
      {
        this.WindowConfig = this.WindowConfig ?? new WindowConfig();
        _process = MonitorWindowManager.GetProcess(this.WindowConfig);
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
          button.Tag = new DisplayHelperSection
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
        var ds = (DisplayHelperSection)d.Tag;
        return ds.ColumnIndex == _displayHelperSection.ColumnIndex &&
               ds.RowIndex == _displayHelperSection.RowIndex;
      }) ?? _displaySectionButtons.First();

      _displayHelperSection = (DisplayHelperSection)dsb.Tag;

      // Select first button.
      dsb.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
    }

    /// <summary>
    /// Updates our WindowConfig's layout values with values gathered
    /// from the current display section and screen.
    /// </summary>
    private void UpdateLayoutValuesFromDisplayHelper()
    {
      var screenWidth = _screen.Bounds.Width;
      var screenHeight = _screen.Bounds.Height;

      var row = _displayHelperSection.RowIndex;
      var col = _displayHelperSection.ColumnIndex;

      var totalRows = _displayHelperSection.TotalRows;
      var totalCols = _displayHelperSection.TotalColumns;

      var x = (screenWidth / totalCols) * col + _screen.Bounds.X;
      var y = (screenHeight / totalRows) * row + _screen.Bounds.Y;

      var width = (screenWidth / totalCols);
      var height = (screenHeight / totalRows);

      this.WindowConfig.MonitorLayoutInfo.SizeInfo.Width = width;
      this.WindowConfig.MonitorLayoutInfo.SizeInfo.Height = height;
      this.WindowConfig.MonitorLayoutInfo.PositionInfo.X = x;
      this.WindowConfig.MonitorLayoutInfo.PositionInfo.Y = y;
    }

    /// <summary>
    /// Saves all input data into the WindowConfig instance.
    /// </summary>
    private void SaveConfig()
    {
      var matchType = this.MatchByProcesNameRadioButton.IsChecked.Value ?
        WindowMatchCriteriaType.ProcessName : WindowMatchCriteriaType.WindowTitle;

      this.WindowConfig.Name = this.NameTextBox.Text;
      this.WindowConfig.MatchCriteria.MatchType = matchType;
      this.WindowConfig.MatchCriteria.MatchString = this.RegexMatchStringTextBox.Text;
      this.WindowConfig.MonitorLayoutInfo.PositionInfo.X = (int)this.XSpinner.Value;
      this.WindowConfig.MonitorLayoutInfo.PositionInfo.Y = (int)this.YSpinner.Value;
      this.WindowConfig.MonitorLayoutInfo.SizeInfo.Width = (int)this.WidthSpinner.Value;
      this.WindowConfig.MonitorLayoutInfo.SizeInfo.Height = (int)this.HeightSpinner.Value;
    }

    private void DisplaySection2_OnClick(object sender, EventArgs e)
    {
      // Reset the previous button highlight.
      _displaySectionButtons.ForEach(b => b.Background = _defaultBrush);

      // Highlight clicked button.
      var button = (Button)sender;
      button.Background = Brushes.Yellow;
      _displayHelperSection = (DisplayHelperSection)button.Tag;
      UpdateScreenHighlight();
    }

    private void UpdateScreenHighlight()
    {
      if (!this.DisplayHelperPreview)
      {
        return;
      }

      var layoutInfo = _displayHelperSection.GetLayoutInfo(_screen);

      // Move process window if process is active.
      if (null != _process)
      {
        MonitorWindowManager.ApplyWindowLayout(layoutInfo, _process);
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
        this.RowSpinner.Value = _displayHelperSection.TotalRows;
        this.ColumnSpinner.Value = _displayHelperSection.TotalColumns;
        _rowColSpinnerEnabled = true;
        RecreateDisplaySectionControl(_displayHelperSection.TotalRows, _displayHelperSection.TotalColumns);
        return;
      }

      ClearDisplaySectionPanel();
      if (this.WindowLayoutPreviewCheckBox.IsChecked.Value)
      {
        this.WindowLayoutPreviewCheckBox.IsChecked = false;
        DisableScreenHighlight();
      }
    }

    private void Close()
    {
      // Uncheck previews, restoring old window values.
      this.WindowStylingPreviewCheckBox.IsChecked = false;
      this.WindowLayoutPreviewCheckBox.IsChecked = false;

      var window = Window.GetWindow(this);
      if (window == null)
      {
        throw new Exception("WindowConfigEditor - Could locate active window to unbind the KeyDown listener.");
      }
      window.KeyDown -= WindowConfigEditor_HandleKeyPress;
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
      this.ScreenAspectRatio = new DisplayAspectRatio(_screen);
      if (_screen.Bounds.Height > _screen.Bounds.Width)
      {
        this.DisplayPanel.Height = this.DisplayPanel.MaxHeight;
        this.DisplayPanel.Width = this.DisplayPanel.MaxHeight * (this.ScreenAspectRatio.XRatio / this.ScreenAspectRatio.YRatio);
      }
      else
      {
        this.DisplayPanel.Width = this.DisplayPanel.MaxWidth;
        this.DisplayPanel.Height = this.DisplayPanel.MaxWidth * (this.ScreenAspectRatio.YRatio / this.ScreenAspectRatio.XRatio);
      }
      UpdateScreenHighlight();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
      SaveConfig();
      OnSave?.Invoke(this.WindowConfig);
      Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void WindowLayoutPreviewCheckBox_OnChecked(object sender, RoutedEventArgs e)
    {
      if (this.DisplayHelperPreview)
      {
        (_process != null).RunIfTrue(() =>
        {
          this.OriginalLayoutInfo = WindowConfig.FromProcess(_process).MonitorLayoutInfo;
        });
        UpdateScreenHighlight();
        return;
      }

      // If unchecked, then apply the original layout.
      (_process != null).RunIfTrue(() =>
      {
        MonitorWindowManager.ApplyWindowLayout(this.OriginalLayoutInfo, _process);
      });
      DisableScreenHighlight();
    }

    private void WindowConfigEditor_OnLoaded(object sender, RoutedEventArgs e)
    {
      var window = Window.GetWindow(this);
      if (window == null)
      {
        throw new Exception("WindowConfigEditor - Could locate active window to bind the KeyDown listener.");
      }
      window.KeyDown += WindowConfigEditor_HandleKeyPress;
    }

    private void WindowConfigEditor_HandleKeyPress(object sender, System.Windows.Input.KeyEventArgs e)
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

    private void LabelTextBox_OnGotFocus(object sender, RoutedEventArgs e)
    {
      (sender as LabelTextBox)?.BaseTextBox.SelectAll();
    }

    private void WindowOpacitySlider_OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
      if (e.Delta > 0)
      {
        WindowOpacitySlider.Value = WindowOpacitySlider.Value + 1;
      }
      else
      {
        WindowOpacitySlider.Value = WindowOpacitySlider.Value - 1;
      }
    }

    private void WindowOpacityValueTextBox_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      if (this.WindowStylingPreview && null != _process)
      {
        MonitorWindowManager.SetWindowOpacityPercentage(_process, (uint)WindowOpacitySlider.Value);
      }
    }

    private void WindowStylingPreviewCheckBox_OnChecked(object sender, RoutedEventArgs e)
    {
      if (this.WindowStylingPreview)
      {
        (_process != null).RunIfTrue(() => this.OriginalOpacity = MonitorWindowManager.GetWindowOpacityPercentage(_process));
        MonitorWindowManager.SetWindowOpacityPercentage(_process, (uint)WindowOpacitySlider.Value);
      }
      else
      {
        (_process != null).RunIfTrue(() => MonitorWindowManager.SetWindowOpacityPercentage(_process, this.OriginalOpacity));
      }
    }
  }

  public class DisplayHelperSection
  {
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public int TotalRows { get; set; }
    public int TotalColumns { get; set; }

    public DisplayHelperSection()
    {
      this.RowIndex = this.ColumnIndex = 0;
      this.TotalRows = this.TotalColumns = 1;
    }

    public DisplayHelperSection(int rowIndex, int columnIndex, int totalRows, int totalColumns)
    {
      this.RowIndex = rowIndex;
      this.ColumnIndex = columnIndex;
      this.TotalRows = totalRows;
      this.TotalColumns = totalColumns;
    }

    public MonitorLayoutInfo GetLayoutInfo(Screen screen)
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

      return new MonitorLayoutInfo(x, y, width, height);
    }
  }
}

