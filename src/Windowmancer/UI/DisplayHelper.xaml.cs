using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using UserControl = System.Windows.Controls.UserControl;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for DisplayHelper.xaml
  /// </summary>
  public partial class DisplayHelper : UserControl
  {
    public bool DisplayHelperPreview { get; set; }
    private ObservableCollection<DisplayContainer> DisplayContainers { get; }

    private DisplayContainer _activeDisplayContainer;

    public DisplaySection DisplaySection { get; private set; }

    private Process _process;
    private WindowHighlight _windowHighlight;
    private readonly List<Button> _displaySectionButtons = new List<Button>();
    private readonly SolidColorBrush _defaultBrush = Brushes.Turquoise;
    private ScreenAspectRatio _screenAspectRatio;

    public DisplayHelper()
    {
      this.DisplaySection = new DisplaySection();
      this.DisplayContainers = new ObservableCollection<DisplayContainer>();
      foreach (var s in Screen.AllScreens)
      {
        this.DisplayContainers.Add(new DisplayContainer(s));
      }

      PreInitialize();
      InitializeComponent();
      PostInitialize();
    }

    public DisplayHelper(List<DisplayContainer> displayContainers)
    {
      this.DisplaySection = new DisplaySection();
      this.DisplayContainers = new ObservableCollection<DisplayContainer>();
      displayContainers.ForEach(d => DisplayContainers.Add(d));

      PreInitialize();
      InitializeComponent();
      PostInitialize();
    }

    private void PreInitialize()
    {
      _screenAspectRatio = new ScreenAspectRatio(Screen.PrimaryScreen);
    }

    private void PostInitialize()
    {
      this.RowSpinner.ValueChanged += RowColSpinners_ValueChanged;
      this.ColumnSpinner.ValueChanged += RowColSpinners_ValueChanged;

      this.DisplayListBox.ItemsSource = this.DisplayContainers;
      _activeDisplayContainer = DisplayContainers.First();
      this.DisplayListBox.SelectedItem = _activeDisplayContainer;
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

      var layoutInfo = DisplaySection.GetLayoutInfo(_activeDisplayContainer);

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
        return ds.ColumnIndex == DisplaySection.ColumnIndex &&
               ds.RowIndex == DisplaySection.RowIndex;
      }) ?? _displaySectionButtons.First();

      DisplaySection = (DisplaySection)dsb.Tag;

      // Select first button.
      dsb.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
    }

    /// <summary>
    /// Updates our WindowInfo's layout values with values gathered
    /// from the current display section and screen.
    /// </summary>
    private void UpdateLayoutValuesFromDisplayHelper()
    {
      var screenWidth = _activeDisplayContainer.Width;
      var screenHeight = _activeDisplayContainer.Height;

      var row = DisplaySection.RowIndex;
      var col = DisplaySection.ColumnIndex;

      var totalRows = DisplaySection.TotalRows;
      var totalCols = DisplaySection.TotalColumns;

      var x = (screenWidth / totalCols) * col + _activeDisplayContainer.X;
      var y = (screenHeight / totalRows) * row + _activeDisplayContainer.Y;

      var width = (screenWidth / totalCols);
      var height = (screenHeight / totalRows);
    }

    private void ClearDisplaySectionPanel()
    {
      this.DisplayPanelGrid.Children.RemoveRange(0, this.DisplayPanelGrid.Children.Count);
    }

    #region Event Methods

    private void DisplayHelper_OnLoaded(object sender, RoutedEventArgs e)
    {
      _rowColSpinnerEnabled = false;
      this.RowSpinner.Value = DisplaySection.TotalRows;
      this.ColumnSpinner.Value = DisplaySection.TotalColumns;
      _rowColSpinnerEnabled = true;
      RecreateDisplaySectionControl(DisplaySection.TotalRows, DisplaySection.TotalColumns);
    }

    private void SetDisplayHelperLayoutButton_OnClick(object sender, RoutedEventArgs e)
    {
      UpdateLayoutValuesFromDisplayHelper();
    }

    private void DisplaySection_OnClick(object sender, EventArgs e)
    {
      // Reset the previous button highlight.
      _displaySectionButtons.ForEach(b => b.Background = _defaultBrush);

      // Highlight clicked button.
      var button = (Button)sender;
      button.Background = Brushes.Yellow;
      DisplaySection = (DisplaySection)button.Tag;
      UpdateScreenHighlight();
    }

    private void DisplayListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      _activeDisplayContainer = (DisplayContainer)this.DisplayListBox.SelectedItem;
      _screenAspectRatio = new ScreenAspectRatio(_activeDisplayContainer);
      if (_activeDisplayContainer.Height > _activeDisplayContainer.Width)
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

    #endregion Event Methods
  }
}
