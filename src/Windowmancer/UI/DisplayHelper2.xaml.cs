using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Microsoft.Practices.ObjectBuilder2;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using UserControl = System.Windows.Controls.UserControl;
using System.Windows.Input;
using System.Windows.Shapes;
using Panel = System.Windows.Controls.Panel;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for DisplayHelper.xaml
  /// </summary>
  public partial class DisplayHelper2 : UserControl
  {
    public Action OnClose { get; set; }
    public Action<WindowInfo> OnSave { get; set; }
    public bool DisplayContainersSelectable { get; set; }
    public DisplayHelperViewModel DisplayHelperViewModel { get; set; }

    private readonly SolidColorBrush _defaultBrush = Brushes.Turquoise;
    private DisplayAspectRatio _screenAspectRatio;

    public DisplayHelper2(DisplayContainer displayContainer)
    {
      this.DisplayHelperViewModel = new DisplayHelperViewModel();
      this.DisplayHelperViewModel.DisplayContainers.Add(displayContainer);
      this.DisplayHelperViewModel.ActiveDisplayContainer = displayContainer;
      PreInitialize();
      InitializeComponent();
      PostInitialize();
    }

    public DisplayHelper2(List<DisplayContainer> displayContainers)
    {
      if (displayContainers == null || displayContainers.Count <= 0)
      {
        throw new Exception($"{this} - List of display containers must contain at least one item.");
      }

      this.DisplayHelperViewModel = new DisplayHelperViewModel();
      displayContainers.ForEach(d =>
      {
        this.DisplayHelperViewModel.DisplayContainers.Add(d);
      });
      this.DisplayHelperViewModel.ActiveDisplayContainer = displayContainers.First();

      PreInitialize();
      InitializeComponent();
      PostInitialize();
    }

    private void PreInitialize()
    {
      this.DisplayContainersSelectable = true;
    }

    private void PostInitialize()
    {
      this.RowSpinner.ValueChanged += RowColSpinners_ValueChanged;
      this.ColumnSpinner.ValueChanged += RowColSpinners_ValueChanged;

      this.DisplayListBox.ItemsSource = this.DisplayHelperViewModel.DisplayContainers;
      this.DisplayListBox.SelectedItem = this.DisplayHelperViewModel.ActiveDisplayContainer;
      this.DisplayListBox.IsEnabled = this.DisplayContainersSelectable;
    }
    
    // TODO: Debug
    private Canvas _canvas;
    private Image _draggedImage = null;
    private Point _mousePosition;
    private void RecreateDisplaySectionControl(int rows, int cols)
    {
      ClearDisplaySectionPanel();

      _canvas = new Canvas { Background = _defaultBrush };
      _canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
      _canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
      _canvas.MouseMove += Canvas_MouseMove;


      var dockedWindows = this.DisplayHelperViewModel.ActiveDisplayContainer.DockedWindows;
      Enumerable.Range(0, rows).ForEach(r =>
        Enumerable.Range(0, cols).ForEach(c =>
        {
          var i = (rows * r) + c;
          if (i >= dockedWindows.Count)
          {
            return;
          }
          var d = dockedWindows[i];
          var image = new Image {Source = Helper.ScreenShotProcessWindow(d.Process)};
          SizeImageToCanvas(image);
          SetImageToSection(image, r, c);
        _canvas.Children.Add(image);
      }));

      this.DisplayPanelGrid.Children.Add(_canvas);
    }

    private void ClearDisplaySectionPanel()
    {
      this.DisplayPanelGrid.Children.RemoveRange(0, this.DisplayPanelGrid.Children.Count);
    }

    private void SizeDisplayHelperBox()
    {
      this.DisplayHelperViewModel.ActiveDisplayContainer = (DisplayContainer)this.DisplayListBox.SelectedItem;
      _screenAspectRatio = new DisplayAspectRatio(this.DisplayHelperViewModel.ActiveDisplayContainer);
      if (this.DisplayHelperViewModel.ActiveDisplayContainer.Height > this.DisplayHelperViewModel.ActiveDisplayContainer.Width)
      {
        this.DisplayPanel.Height = this.DisplayPanel.MaxHeight;
        this.DisplayPanel.Width = this.DisplayPanel.MaxHeight * (_screenAspectRatio.XRatio / _screenAspectRatio.YRatio);
      }
      else
      {
        this.DisplayPanel.Width = this.DisplayPanel.MaxWidth;
        this.DisplayPanel.Height = this.DisplayPanel.MaxWidth * (_screenAspectRatio.YRatio / _screenAspectRatio.XRatio);
      }
    }

    #region Event Methods

    #region Canvas Methods

    private void SizeImageToCanvas(Image image)
    {
      var screenWidth = this.DisplayPanelGrid.ActualWidth;
      var screenHeight = this.DisplayPanelGrid.ActualHeight;

      // TODO: Debug
      var container = this.DisplayHelperViewModel.ActiveDisplayContainer;
      var totalRows = container.Rows;
      var totalCols = container.Columns;

      var width = (screenWidth / totalCols);
      var height = (screenHeight / totalRows);

      image.Width = width;
      image.Height = height;
    }

    private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var image = e.Source as Image;

      if (image == null || !_canvas.CaptureMouse()) return;
      _mousePosition = e.GetPosition(_canvas);
      if (_highlightSection.Rectangle != null)
      {
        _highlightSection.Rectangle.Visibility = Visibility.Visible;
      }
      _draggedImage = image;
      Panel.SetZIndex(_draggedImage, 1); // in case of multiple images
    }

    private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (_draggedImage == null) return;
      _canvas.ReleaseMouseCapture();
      _highlightSection.Rectangle.Visibility = Visibility.Hidden;
      SetImageToSection(_draggedImage, _highlightSection.Row, _highlightSection.Column);
      _draggedImage = null;
    }

    private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
      if (_draggedImage != null)
      {
        var position = e.GetPosition(_canvas);
        var offset = position - _mousePosition;
        _mousePosition = position;

        var xPos = Canvas.GetLeft(_draggedImage) + offset.X;
        var yPos = Canvas.GetTop(_draggedImage) + offset.Y;

        // Prevent image from moving outside of the canvas.
        if ((xPos + _draggedImage.Width) > _canvas.ActualWidth)
        {
          xPos = _canvas.ActualWidth - _draggedImage.Width;
        }
        else if (xPos < 0)
        {
          xPos = 0;
        }
        if ((yPos + _draggedImage.Height) > _canvas.ActualHeight)
        {
          yPos = _canvas.ActualHeight - _draggedImage.Height;
        }
        else if (yPos < 0)
        {
          yPos = 0;
        }
        
        Canvas.SetLeft(_draggedImage, xPos);
        Canvas.SetTop(_draggedImage, yPos);

        // Position relative to mouse.
        this.DisplayHelperViewModel.CanvasX = (int)Mouse.GetPosition(_canvas).X;
        this.DisplayHelperViewModel.CanvasY = (int)Mouse.GetPosition(_canvas).Y;
        var centerX = (int)Mouse.GetPosition(_canvas).X;
        var centerY = (int)Mouse.GetPosition(_canvas).Y;
        
        HighlightDisplaySection((int)centerX, (int)centerY);
      }
    }

    private void HighlightDisplaySection(int x, int y)
    {
      var rows = (int)this.RowSpinner.Value;
      var columns = (int)this.ColumnSpinner.Value;

      var sectionWidth = (int)_canvas.ActualWidth / columns;
      var sectionHeight = (int)_canvas.ActualHeight / rows;

      for (int row = 0; row < rows; ++row)
      {
        for (int col = 0; col < columns; ++col)
        {
          var xStart = sectionWidth * (col);
          var xEnd = xStart + sectionWidth;

          var yStart = sectionHeight * row;
          var yEnd = yStart + sectionHeight;
          
          if (x >= xStart && x <= xEnd &&
              y >= yStart && y <= yEnd)
          {
            this.DisplayHelperViewModel.CanvasRow = row;
            this.DisplayHelperViewModel.CanvasColumn = col;
            HightlightCanvasSection(
              row, 
              col, 
              xStart,
              yStart,
              sectionWidth, 
              sectionHeight);
            return;
          }
        }
      }
    }

    private readonly HighlightSection _highlightSection = new HighlightSection();

    private void HightlightCanvasSection(
      int row, 
      int col, 
      int x,
      int y,
      int width, 
      int height)
    {
      if (_highlightSection.Row == row && _highlightSection.Column == col)
      {
        return;
      }
      _highlightSection.Row = row;
      _highlightSection.Column = col;
      if (_highlightSection.Rectangle == null)
      {
        _highlightSection.Rectangle = new Rectangle
        {
          Width = width,
          Height = height,
          Fill = new SolidColorBrush { Color = Color.FromRgb(255, 242, 0) }
        };
        _canvas.Children.Add(_highlightSection.Rectangle);
      }
      
      Canvas.SetLeft(_highlightSection.Rectangle, x);
      Canvas.SetTop(_highlightSection.Rectangle, y);
      Panel.SetZIndex(_highlightSection.Rectangle, 0);
    }

    private void SetImageToSection(Image image, int row, int column)
    {
      var rows = (int)this.RowSpinner.Value;
      var columns = (int)this.ColumnSpinner.Value;

      var sectionWidth = (int)_canvas.ActualWidth / columns;
      var sectionHeight = (int)_canvas.ActualHeight / rows;

      var x = column * sectionWidth;
      var y = row * sectionHeight;

      Canvas.SetLeft(image, x);
      Canvas.SetTop(image, y);
    }

    #endregion Canvas Methods

    private void DisplayHelper_OnLoaded(object sender, RoutedEventArgs e)
    {
      _rowColSpinnerEnabled = false;
      var rows = this.RowSpinner.Value = this.DisplayHelperViewModel.ActiveDisplayContainer.Rows;
      var columns = this.ColumnSpinner.Value = this.DisplayHelperViewModel.ActiveDisplayContainer.Columns;
      _rowColSpinnerEnabled = true;
      RecreateDisplaySectionControl((int)rows, (int)columns);
    }

    private void SetDisplayHelperLayoutButton_OnClick(object sender, RoutedEventArgs e)
    {
      
    }

    private void DisplayListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      SizeDisplayHelperBox();
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

  public class DisplayHelperViewModel : PropertyNotifyBase
  {
    public DisplayContainer ActiveDisplayContainer
    {
      get 
      {
        var dc = GetProperty<DisplayContainer>();
        this.DisplayAspectRatio = new DisplayAspectRatio(dc);
        return dc;
      }
      set => SetProperty(value);
    }

    public DockedWindow ActiveDockedWindow
    {
      get => GetProperty<DockedWindow>();
      set => SetProperty(value);
    }

    public HighlightSection HighlightSection
    {
      get => GetProperty<HighlightSection>();
      set => SetProperty(value);
    }

    public ObservableCollection<DisplayContainer> DisplayContainers
    {
      get => GetProperty<ObservableCollection<DisplayContainer>>();
      set => SetProperty(value);
    }

    public DisplayAspectRatio DisplayAspectRatio
    {
      get => GetProperty<DisplayAspectRatio>();
      private set => SetProperty(value);
    }

    public int CanvasX
    {
      get => GetProperty<int>();
      set => SetProperty(value);
    }

    public int CanvasY
    {
      get => GetProperty<int>();
      set => SetProperty(value);
    }

    public int CanvasRow
    {
      get => GetProperty<int>();
      set => SetProperty(value);
    }

    public int CanvasColumn
    {
      get => GetProperty<int>();
      set => SetProperty(value);
    }

    public DisplayHelperViewModel()
    {
      RegisterProperty<int>("CanvasX");
      RegisterProperty<int>("CanvasY");
      RegisterProperty<int>("CanvasRow");
      RegisterProperty<int>("CanvasColumn");
      RegisterProperty<DockedWindow>("ActiveDockedWindow", null);
      RegisterProperty<DisplayContainer>("ActiveDisplayContainer", null);
      RegisterProperty("DisplayContainers", new ObservableCollection<DisplayContainer>());
      RegisterProperty<DisplayAspectRatio>("DisplayAspectRatio", null); 
    }
  }

  // TODO: Debug
  public class HighlightSection
  {
    public Rectangle Rectangle { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }

    public HighlightSection()
    {
      this.Row = -1;
      this.Column = -1;
    }
  }
}
