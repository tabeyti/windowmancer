using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Media;
using WindowmancerWPF.Models;
using Button = System.Windows.Controls.Button;
using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;

namespace WindowmancerWPF.UI
{
  /// <summary>
  /// Interaction logic for WindowConfigDialog.xaml
  /// </summary>
  /// 
  public partial class WindowConfigDialog : Window
  {
    public PositionInfo Position { get; set; }
    public SizeInfo Size { get; set; }

    private Screen _currentDisplay;
    private DisplaySection _currentDisplaySection;
    private ScreenHighlight _currentHighlight;

    private readonly List<Button> _displaySectionButtons = new List<Button>();
    private readonly SolidColorBrush _defaultBrush = Brushes.Turquoise;

    public WindowConfigDialog()
    {
      this.Position = new PositionInfo { X = 1, Y = 1 };
      this.Size = new Models.SizeInfo { Width = 1, Height = 1 };

      InitializeComponent();
      Initialize();
    }

    protected override void OnClosed(EventArgs e)
    {
      base.OnClosed(e);
      _currentHighlight?.Close();
    }

    private void Initialize()
    {
      this.RowSpinner.ValueChanged += RowColSpinners_ValueChanged;
      this.ColumnSpinner.ValueChanged += RowColSpinners_ValueChanged;

      this.DisplaysComboBox.ItemsSource = Screen.AllScreens;
      this.DisplaysComboBox.SelectedItem = _currentDisplay = Screen.PrimaryScreen;

      RecreateDisplaySectorControl(1, 1);
    }

    private void RecreateDisplaySectorControl(int rows, int cols)
    {
      this.DisplayPanel.Children.RemoveRange(0, this.DisplayPanel.Children.Count);

      var grid = new UniformGrid { Rows = rows, Columns = cols };
      _displaySectionButtons.Clear();
      Enumerable.Range(0, rows).ForEach(r =>
        Enumerable.Range(0, cols).ForEach(c =>
        {
          var button = new Button { Content = ((r * cols) + c).ToString(), Background = _defaultBrush };
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

      var row = _currentDisplaySection.RowIndex;
      var col = _currentDisplaySection.ColumnIndex;

      var totalRows = _currentDisplaySection.TotalRows;
      var totalCols = _currentDisplaySection.TotalColumns;

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
      _currentDisplaySection = (DisplaySection)button.Tag;
      UpdateLayoutValues();
      UpdateScreenHighlight();
    }

    private void UpdateScreenHighlight()
    {
      if (null == _currentHighlight)
      {
        _currentHighlight = new ScreenHighlight();
        _currentHighlight.Show();
      }
      _currentHighlight.UpdateLayout(this.Position.X, this.Position.Y, this.Size.Width, this.Size.Height);
    }

    private void RowColSpinners_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      int rows = this.RowSpinner?.Value ?? 1;
      int cols = this.ColumnSpinner?.Value ?? 1;
      RecreateDisplaySectorControl(rows, cols);
    }

    private void DisplaysComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      _currentDisplay = (Screen)this.DisplaysComboBox.SelectedItem;
      if (_currentDisplaySection != null)
      {
        UpdateLayoutValues();
        UpdateScreenHighlight();
      }
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
