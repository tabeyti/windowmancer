using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Media;
using Microsoft.Practices.ObjectBuilder2;
using Windowmancer.Core.Extensions;
using Windowmancer.Core.Models;
using Windowmancer.Core.Services;
using Button = System.Windows.Controls.Button;
using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;
using UserControl = System.Windows.Controls.UserControl;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for DisplayHelper.xaml
  /// </summary>
  public partial class DisplayHelper : UserControl
  {
    public bool DisplayHelperPreview { get; set; }
    public WindowLayoutInfo LayoutInfo { get; set; }
    
    private static Screen _screen;
    private static DisplaySection _displaySection = new DisplaySection();

    private WindowLayoutInfo _originalLayoutInfo;
    private Process _process;
    private WindowHighlight _windowHighlight;
    private readonly List<Button> _displaySectionButtons = new List<Button>();
    private readonly SolidColorBrush _defaultBrush = Brushes.Turquoise;
    private ScreenAspectRatio _screenAspectRatio; 

    public DisplayHelper()
    {
      InitializeComponent();
      Initialize();
    }

    private void Initialize()
    {
      this.RowSpinner.ValueChanged += RowColSpinners_ValueChanged;
      this.ColumnSpinner.ValueChanged += RowColSpinners_ValueChanged;
    }

    private void DisableScreenHighlight()
    {
      _windowHighlight?.Close();
      _windowHighlight = null;
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

      var dsb = _displaySectionButtons.Find(d =>
      {
        var ds = (DisplaySection)d.Tag;
        return ds.ColumnIndex == _displaySection.ColumnIndex &&
               ds.RowIndex == _displaySection.RowIndex;
      }) ?? _displaySectionButtons.First();

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

      this.LayoutInfo.SizeInfo.Width = width;
      this.LayoutInfo.SizeInfo.Height = height;
      this.LayoutInfo.PositionInfo.X = x;
      this.LayoutInfo.PositionInfo.Y = y;
    }

    private void ClearDisplaySectionPanel()
    {
      this.DisplayPanelGrid.Children.RemoveRange(0, this.DisplayPanelGrid.Children.Count);
    }

    private void SetDisplayHelperLayoutButton_OnClick(object sender, RoutedEventArgs e)
    {
      UpdateLayoutValuesFromDisplayHelper();
    }

    private void WindowLayoutPreviewCheckBox_OnChecked(object sender, RoutedEventArgs e)
    {
      if (this.DisplayHelperPreview)
      {
        (_process != null).RunIfTrue(() =>
        {
          this._originalLayoutInfo = WindowInfo.FromProcess(_process).LayoutInfo;
        });
        UpdateScreenHighlight();
        return;
      }

      // If unchecked, then apply the original layout.
      (_process != null).RunIfTrue(() =>
      {
        WindowManager.ApplyWindowLayout(this._originalLayoutInfo, _process);
      });
      DisableScreenHighlight();
    }

    private void DisplaySection_OnClick(object sender, EventArgs e)
    {
      // Reset the previous button highlight.
      _displaySectionButtons.ForEach(b => b.Background = _defaultBrush);

      // Highlight clicked button.
      var button = (Button)sender;
      button.Background = Brushes.Yellow;
      _displaySection = (DisplaySection)button.Tag;
      UpdateScreenHighlight();
    }

    private void DisplayListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      _screen = (Screen)this.DisplayListBox.SelectedItem;
      _screenAspectRatio = new ScreenAspectRatio(_screen);
      if (_screen.Bounds.Height > _screen.Bounds.Width)
      {
        this.DisplayPanel.Height = this.DisplayPanel.MaxHeight;
        this.DisplayPanel.Width = this.DisplayPanel.MaxHeight * (_screenAspectRatio.XRatio / _screenAspectRatio.YRatio);
      }
      else
      {
        this.DisplayPanel.Width = this.DisplayPanel.MaxWidth;
        this.DisplayPanel.Height = this.DisplayPanel.MaxWidth * (_screenAspectRatio.YRatio / _screenAspectRatio.XRatio);
      }
      UpdateScreenHighlight();
    }

    private bool _rowColSpinnerEnabled = true;
    private void RowColSpinners_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      if (!_rowColSpinnerEnabled) return;

      var rows = this.RowSpinner?.Value ?? 1;
      var cols = this.ColumnSpinner?.Value ?? 1;
      RecreateDisplaySectionControl(rows, cols);
    }
  }
}
