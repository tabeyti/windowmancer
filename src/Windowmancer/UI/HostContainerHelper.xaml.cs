using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
  public partial class HostContainerHelper : UserControl
  {
    public Action OnClose { get; set; }
    public Action<WindowInfo> OnSave { get; set; }
    public bool DisplayContainersSelectable { get; set; }
    public HostContainerViewModel HostContainerHelperViewModel { get; set; }
    public CanvasViewModel CanvasViewModel { get; set; }

    private readonly SolidColorBrush _defaultBrush = Brushes.Turquoise;
    private DisplayAspectRatio _screenAspectRatio;

    public HostContainerHelper(DisplayContainer displayContainer)
    {
      this.CanvasViewModel = new CanvasViewModel();
      this.HostContainerHelperViewModel = new HostContainerViewModel();
      this.HostContainerHelperViewModel.DisplayContainers.Add(displayContainer);
      this.HostContainerHelperViewModel.ActiveDisplayContainer = displayContainer;
      PreInitialize();
      InitializeComponent();
      PostInitialize();
    }

    public HostContainerHelper(List<DisplayContainer> displayContainers)
    {
      if (displayContainers == null || displayContainers.Count <= 0)
      {
        throw new Exception($"{this} - List of display containers must contain at least one item.");
      }

      this.HostContainerHelperViewModel = new HostContainerViewModel();
      displayContainers.ForEach(d =>
      {
        this.HostContainerHelperViewModel.DisplayContainers.Add(d);
      });
      this.HostContainerHelperViewModel.ActiveDisplayContainer = displayContainers.First();

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

      this.DisplayListBox.ItemsSource = this.HostContainerHelperViewModel.DisplayContainers;
      this.DisplayListBox.SelectedItem = this.HostContainerHelperViewModel.ActiveDisplayContainer;
      this.DisplayListBox.IsEnabled = this.DisplayContainersSelectable;
    }
    
    private void RecreateDisplaySectionControl(int rows, int cols)
    {
      ClearDisplaySectionPanel();

      this.CanvasViewModel.Canvas = new Canvas { Background = _defaultBrush };
      this.CanvasViewModel.Canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
      this.CanvasViewModel.Canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
      this.CanvasViewModel.Canvas.MouseMove += Canvas_MouseMove;

      var dockedWindows = this.HostContainerHelperViewModel.ActiveDisplayContainer.DockedWindows;
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
          this.CanvasViewModel.Canvas.Children.Add(image);
          SetImageToSection(image, r, c);
          this.HostContainerHelperViewModel.DockedWindowImageDict.Add(d, image);
        }));

      this.DisplayPanelGrid.Children.Add(this.CanvasViewModel.Canvas);
    }

    private void ClearDisplaySectionPanel()
    {
      this.DisplayPanelGrid.Children.RemoveRange(0, this.DisplayPanelGrid.Children.Count);
      foreach (var image in this.HostContainerHelperViewModel.DockedWindowImageDict.Values)
      {
        image.Visibility = Visibility.Hidden;
      }
      this.HostContainerHelperViewModel.DockedWindowImageDict.Clear();
    }

    private void SizeDisplayHelperBox()
    {
      this.HostContainerHelperViewModel.ActiveDisplayContainer = (DisplayContainer)this.DisplayListBox.SelectedItem;
      _screenAspectRatio = new DisplayAspectRatio(this.HostContainerHelperViewModel.ActiveDisplayContainer);
      if (this.HostContainerHelperViewModel.ActiveDisplayContainer.Height > this.HostContainerHelperViewModel.ActiveDisplayContainer.Width)
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
      var container = this.HostContainerHelperViewModel.ActiveDisplayContainer;
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

      if (image == null || !this.CanvasViewModel.Canvas.CaptureMouse()) return;
      this.CanvasViewModel.MousePosition = e.GetPosition(this.CanvasViewModel.Canvas);
      if (this.CanvasViewModel.HighlightSection.Rectangle != null)
      {
        this.CanvasViewModel.HighlightSection.Rectangle.Visibility = Visibility.Visible;
      }
      this.CanvasViewModel.DraggedImage = image;
      Panel.SetZIndex(this.CanvasViewModel.DraggedImage, 1); // in case of multiple images
    }

    private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (this.CanvasViewModel.DraggedImage == null) return;
      this.CanvasViewModel.Canvas.ReleaseMouseCapture();
      this.CanvasViewModel.HighlightSection.Rectangle.Visibility = Visibility.Hidden;
      SetImageToSection(this.CanvasViewModel.DraggedImage, this.CanvasViewModel.HighlightSection.Row, this.CanvasViewModel.HighlightSection.Column);
      this.CanvasViewModel.DraggedImage = null;
    }

    private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
      if (this.CanvasViewModel.DraggedImage != null)
      {
        var position = e.GetPosition(this.CanvasViewModel.Canvas);
        var offset = position - this.CanvasViewModel.MousePosition;
        this.CanvasViewModel.MousePosition = position;

        var xPos = Canvas.GetLeft(this.CanvasViewModel.DraggedImage) + offset.X;
        var yPos = Canvas.GetTop(this.CanvasViewModel.DraggedImage) + offset.Y;

        // Prevent image from moving outside of the canvas.
        if ((xPos + this.CanvasViewModel.DraggedImage.Width) > this.CanvasViewModel.Canvas.ActualWidth)
        {
          xPos = this.CanvasViewModel.Canvas.ActualWidth - this.CanvasViewModel.DraggedImage.Width;
        }
        else if (xPos < 0)
        {
          xPos = 0;
        }
        if ((yPos + this.CanvasViewModel.DraggedImage.Height) > this.CanvasViewModel.Canvas.ActualHeight)
        {
          yPos = this.CanvasViewModel.Canvas.ActualHeight - this.CanvasViewModel.DraggedImage.Height;
        }
        else if (yPos < 0)
        {
          yPos = 0;
        }
        
        Canvas.SetLeft(this.CanvasViewModel.DraggedImage, xPos);
        Canvas.SetTop(this.CanvasViewModel.DraggedImage, yPos);

        // Position relative to mouse.
        this.CanvasViewModel.CanvasX = (int)Mouse.GetPosition(this.CanvasViewModel.Canvas).X;
        this.CanvasViewModel.CanvasY = (int)Mouse.GetPosition(this.CanvasViewModel.Canvas).Y;
        var centerX = (int)Mouse.GetPosition(this.CanvasViewModel.Canvas).X;
        var centerY = (int)Mouse.GetPosition(this.CanvasViewModel.Canvas).Y;
        
        HighlightDisplaySection((int)centerX, (int)centerY);
      }
    }

    private void HighlightDisplaySection(int x, int y)
    {
      var rows = (int)this.RowSpinner.Value;
      var columns = (int)this.ColumnSpinner.Value;

      var sectionWidth = (int)this.CanvasViewModel.Canvas.ActualWidth / columns;
      var sectionHeight = (int)this.CanvasViewModel.Canvas.ActualHeight / rows;

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
            this.CanvasViewModel.CanvasRow = row;
            this.CanvasViewModel.CanvasColumn = col;
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

    private void HightlightCanvasSection(
      int row, 
      int col, 
      int x,
      int y,
      int width, 
      int height)
    {
      if (this.CanvasViewModel.HighlightSection.Row == row && this.CanvasViewModel.HighlightSection.Column == col)
      {
        return;
      }
      this.CanvasViewModel.HighlightSection.Row = row;
      this.CanvasViewModel.HighlightSection.Column = col;
      if (this.CanvasViewModel.HighlightSection.Rectangle == null)
      {
        this.CanvasViewModel.HighlightSection.Rectangle = new Rectangle
        {
          Width = width,
          Height = height,
          Fill = new SolidColorBrush { Color = Color.FromRgb(255, 242, 0) }
        };
        this.CanvasViewModel.Canvas.Children.Add(this.CanvasViewModel.HighlightSection.Rectangle);
      }
      
      Canvas.SetLeft(this.CanvasViewModel.HighlightSection.Rectangle, x);
      Canvas.SetTop(this.CanvasViewModel.HighlightSection.Rectangle, y);
      Panel.SetZIndex(this.CanvasViewModel.HighlightSection.Rectangle, 0);
    }

    private void SetImageToSection(Image image, int row, int column)
    {
      var rows = this.HostContainerHelperViewModel.ActiveDisplayContainer.Rows;
      var columns = this.HostContainerHelperViewModel.ActiveDisplayContainer.Columns;

      var sectionWidth = (int)this.CanvasViewModel.Canvas.ActualWidth / columns;
      var sectionHeight = (int)this.CanvasViewModel.Canvas.ActualHeight / rows;

      var x = column * sectionWidth;
      var y = row * sectionHeight;

      Canvas.SetLeft(image, x);
      Canvas.SetTop(image, y);
    }

    #endregion Canvas Methods

    private void DisplayHelper_OnLoaded(object sender, RoutedEventArgs e)
    {
      _rowColSpinnerEnabled = false;
      var rows = this.RowSpinner.Value = this.HostContainerHelperViewModel.ActiveDisplayContainer.Rows;
      var columns = this.ColumnSpinner.Value = this.HostContainerHelperViewModel.ActiveDisplayContainer.Columns;
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
      RecreateDisplaySectionControl(
        this.HostContainerHelperViewModel.ActiveDisplayContainer.Rows,
        this.HostContainerHelperViewModel.ActiveDisplayContainer.Columns);
    }

    #endregion Event Methods
  }

  public class HostContainerViewModel : PropertyNotifyBase
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

    public Dictionary<DockedWindow, Image> DockedWindowImageDict { get; set; }
    
    public HostContainerViewModel()
    {
      RegisterProperty<DockedWindow>("ActiveDockedWindow", null);
      RegisterProperty<DisplayContainer>("ActiveDisplayContainer", null);
      RegisterProperty("DisplayContainers", new ObservableCollection<DisplayContainer>());
      RegisterProperty<DisplayAspectRatio>("DisplayAspectRatio", null);

      DockedWindowImageDict = new Dictionary<DockedWindow, Image>();
    }
  }

  public class CanvasViewModel : PropertyNotifyBase
  {
    public Canvas Canvas
    {
      get => GetProperty<Canvas>();
      set => SetProperty(value);
    }

    public Image DraggedImage
    {
      get => GetProperty<Image>();
      set => SetProperty(value);
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

    public HighlightSection HighlightSection
    {
      get => GetProperty<HighlightSection>();
      set => SetProperty(value);
    }

    public Point MousePosition
    {
      get => GetProperty<Point>();
      set => SetProperty(value);
    }

    public CanvasViewModel()
    {
      RegisterProperty<int>("CanvasX");
      RegisterProperty<int>("CanvasY");
      RegisterProperty<int>("CanvasRow");
      RegisterProperty<int>("CanvasColumn");
      RegisterProperty<Canvas>("Canvas", null);
      RegisterProperty<Image>("DraggedImage", null);
      RegisterProperty("HighlightSection", new HighlightSection());
      RegisterProperty("MousePosition", new Point());
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
