﻿using System;
using Microsoft.Practices.ObjectBuilder2;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Media;
using Windowmancer.Core.Extensions;
using Windowmancer.Core.Models;
using Windowmancer.Core.Services;
using Button = System.Windows.Controls.Button;
using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for WindowConfigLayoutEditor.xaml
  /// </summary>
  public partial class WindowConfigLayoutEditor : Window
  {
    public MonitorLayoutInfo MonitorLayoutInfo { get; set; }
    public HostContainerLayoutInfo HostContainerLayoutInfo { get; set; }


    // Binding objects.
    public DisplayAspectRatio ScreenAspectRatio { get; set; }
    public bool DisplayHelperPreview { get; set; }
    public MonitorLayoutInfo OriginalLayoutInfo { get; set; }

    private static Screen _screen;
    private static DisplayHelperSection _displayHelperSection = new DisplayHelperSection();
    private WindowHighlight _windowHighlight;
    private Process _process;

    private readonly List<Button> _displaySectionButtons = new List<Button>();
    private readonly SolidColorBrush _defaultBrush = Brushes.Turquoise;

    public WindowConfigLayoutEditor()
    {
      PreInitialization();
      InitializeComponent();
      Initialize();
    }

    private void PreInitialization()
    {
      this.ScreenAspectRatio = new DisplayAspectRatio(Screen.PrimaryScreen);
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enable"></param>
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

    private void ClearDisplaySectionPanel()
    {
      this.DisplayPanelGrid.Children.RemoveRange(0, this.DisplayPanelGrid.Children.Count);
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

      this.MonitorLayoutInfo.SizeInfo.Width = width;
      this.MonitorLayoutInfo.SizeInfo.Height = height;
      this.MonitorLayoutInfo.PositionInfo.X = x;
      this.MonitorLayoutInfo.PositionInfo.Y = y;
    }

    private void DisplaySection_OnClick(object sender, EventArgs e)
    {
      // Reset the previous button highlight.
      _displaySectionButtons.ForEach(b => b.Background = _defaultBrush);

      // Highlight clicked button.
      var button = (Button)sender;
      button.Background = Brushes.Yellow;
      _displayHelperSection = (DisplayHelperSection)button.Tag;
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

    private void DisplaysComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      _screen = (Screen) this.DisplayListBox.SelectedItem;
      this.ScreenAspectRatio = new DisplayAspectRatio(_screen);
      if (_screen.Bounds.Height > _screen.Bounds.Width)
      {
        this.DisplayPanel.Height = this.DisplayPanel.MaxHeight;
        this.DisplayPanel.Width = this.DisplayPanel.MaxHeight *
                                  (this.ScreenAspectRatio.XRatio / this.ScreenAspectRatio.YRatio);
      }
      else
      {
        this.DisplayPanel.Width = this.DisplayPanel.MaxWidth;
        this.DisplayPanel.Height = this.DisplayPanel.MaxWidth *
                                   (this.ScreenAspectRatio.YRatio / this.ScreenAspectRatio.XRatio);
      }
      UpdateScreenHighlight();
    }

    private void LayoutHelperExpander_OnExpanded(object sender, RoutedEventArgs e)
    {
      var expander = (sender as System.Windows.Controls.Expander);
      if (expander?.IsExpanded == null) return;
      EnableDisableLayoutHelper(expander.IsExpanded);
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

    private void SetDisplayHelperLayoutButton_OnClick(object sender, RoutedEventArgs e)
    {
      UpdateLayoutValuesFromDisplayHelper();
    }
  }
}
  

