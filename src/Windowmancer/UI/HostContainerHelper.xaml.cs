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
      this.HostContainerHelperViewModel = new HostContainerViewModel();
      this.HostContainerHelperViewModel.DisplayContainers.Add(displayContainer);
      this.HostContainerHelperViewModel.ActiveDisplayContainer = displayContainer;
      PreInitialize();
      InitializeComponent();
      this.CanvasViewModel = new CanvasViewModel(this.DebugTextBox);
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
      // Clear.
      this.DisplayPanelGrid.Children.Clear();
      this.CanvasViewModel.Clear();

      // Create.
      this.CanvasViewModel.Rows = this.HostContainerHelperViewModel.ActiveDisplayContainer.Rows;
      this.CanvasViewModel.Columns = this.HostContainerHelperViewModel.ActiveDisplayContainer.Columns;
      this.CanvasViewModel.Canvas = new Canvas { Background = _defaultBrush };
      this.CanvasViewModel.Canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
      this.CanvasViewModel.Canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
      this.CanvasViewModel.Canvas.MouseMove += Canvas_MouseMove;

      var dockableWindows = this.HostContainerHelperViewModel.ActiveDisplayContainer.DockedWindows;
      Enumerable.Range(0, rows).ForEach(r =>
        Enumerable.Range(0, cols).ForEach(c =>
        {
          var i = (cols * r) + c;
          if (i >= dockableWindows.Count)
          {
            return;
          }
          var d = dockableWindows[i];
          var image = new Image {Source = Helper.ScreenShotProcessWindow(d.Process)};
          this.CanvasViewModel.AddImage(image, d);
        }));

      // Add.
      this.DisplayPanelGrid.Children.Add(this.CanvasViewModel.Canvas);
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

      // Set dragged image as top and other images to bottom.
      Panel.SetZIndex(this.CanvasViewModel.DraggedImage, 1);
      this.CanvasViewModel.MoveAllImagesToBack();
    }

    private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (this.CanvasViewModel.DraggedImage == null) return;
      this.CanvasViewModel.Canvas.ReleaseMouseCapture();
      this.CanvasViewModel.HighlightSection.Rectangle.Visibility = Visibility.Hidden;
      this.CanvasViewModel.MoveImageToSection(
        this.CanvasViewModel.DraggedImage, 
        this.CanvasViewModel.HighlightSection.Row, 
        this.CanvasViewModel.HighlightSection.Column);
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
        var centerX = (int)Mouse.GetPosition(this.CanvasViewModel.Canvas).X;
        var centerY = (int)Mouse.GetPosition(this.CanvasViewModel.Canvas).Y;
        
        // Update canvas section items.
        this.CanvasViewModel.UpdateCanvasSection(centerX, centerY);
      }
    }
    
    #endregion Canvas Methods

    private void DisplayHelper_OnLoaded(object sender, RoutedEventArgs e)
    {
      RecreateDisplaySectionControl(
        this.HostContainerHelperViewModel.ActiveDisplayContainer.Rows,
        this.HostContainerHelperViewModel.ActiveDisplayContainer.Columns);
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

    public DockableWindow ActiveDockableWindow
    {
      get => GetProperty<DockableWindow>();
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
    
    public HostContainerViewModel()
    {
      RegisterProperty<DockableWindow>("ActiveDockableWindow", null);
      RegisterProperty<DisplayContainer>("ActiveDisplayContainer", null);
      RegisterProperty("DisplayContainers", new ObservableCollection<DisplayContainer>());
      RegisterProperty<DisplayAspectRatio>("DisplayAspectRatio", null);
    }
  }

  public class CanvasViewModel : PropertyNotifyBase
  {
    public Canvas Canvas
    {
      get => GetProperty<Canvas>();
      set
      {
        SetProperty(value);

        // Size and set images to their respective sections on load.
        value.Loaded += (o, e) =>
        {
          foreach (var kv in this.DockableWindowImageDict)
          {
            var image = kv.Value;
            SizeImageToCanvas(image);
            this.Canvas.Children.Add(image);
            MoveImageToSection(image, kv.Key.Row, kv.Key.Column);
          }
        };
      }
    }

    public Image DraggedImage
    {
      get => GetProperty<Image>();
      set => SetProperty(value);
    }

    public int Rows
    {
      get => GetProperty<int>();
      set => SetProperty(value);
    }

    public int Columns
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

    public Dictionary<DockableWindow, Image> DockableWindowImageDict { get; set; }

    // TODO: Debug
    private readonly RichTextBox _debugRichTextBox;

    public CanvasViewModel(RichTextBox debugTextBox)
    {
      // TODO: Debug
      _debugRichTextBox = debugTextBox;

      RegisterProperty<int>("CanvasX");
      RegisterProperty<int>("CanvasY");
      RegisterProperty<int>("Rows");
      RegisterProperty<int>("Columns");
      RegisterProperty<Canvas>("Canvas", null);
      RegisterProperty<Image>("DraggedImage", null);
      RegisterProperty("HighlightSection", new HighlightSection());
      RegisterProperty("MousePosition", new Point());
      this.DockableWindowImageDict = new Dictionary<DockableWindow, Image>();
    }

    public void MoveImageToSection(
      Image image, 
      int row, 
      int column)
    {
      var rows = this.Rows;
      var columns = this.Columns;

      var sectionWidth = (int)this.Canvas.ActualWidth / columns;
      var sectionHeight = (int)this.Canvas.ActualHeight / rows;

      var x = column * sectionWidth;
      var y = row * sectionHeight;

      Canvas.SetLeft(image, x);
      Canvas.SetTop(image, y);
    }

    public void UpdateCanvasSection(int x, int y)
    {
      var canvasSection = GetCanvasSection(x, y);
      var dockableWindow = GetDockableWindowForImage(this.DraggedImage);

      // If we are on the same section, forgetaboutit.
      if (null == canvasSection || (this.HighlightSection.Row == canvasSection.Row && 
        this.HighlightSection.Column == canvasSection.Column))
      {
        return;
      }

      // If dragged image is over another image, move the other onto the vaccant space.
      foreach (var kv in this.DockableWindowImageDict)
      {
        if (kv.Value == this.DraggedImage) continue;
        if (canvasSection.Row != kv.Key.Row || canvasSection.Column != kv.Key.Column) continue;

        // Move found image to the current image section.
        var row = kv.Key.Row = dockableWindow.Row;
        var col = kv.Key.Column = dockableWindow.Column;
        MoveImageToSection(kv.Value, row, col);
        break;
      }

      // Our image is over a new section, so let's update the row/col indices
      // and create a new highlight section if none.
      this.HighlightSection.Row = canvasSection.Row;
      this.HighlightSection.Column = canvasSection.Column;
      if (this.HighlightSection.Rectangle == null)
      {
        this.HighlightSection.Rectangle = new Rectangle
        {
          Width = canvasSection.Width,
          Height = canvasSection.Height,
          Fill = new SolidColorBrush { Color = Color.FromRgb(255, 242, 0) }
        };
        this.Canvas.Children.Add(this.HighlightSection.Rectangle);
      }

      // Update docked window container with new row/col indices.
      dockableWindow.Row = canvasSection.Row;
      dockableWindow.Column = canvasSection.Column;

      // Move our highlight section to the cooresponding section.
      Canvas.SetLeft(this.HighlightSection.Rectangle, canvasSection.X);
      Canvas.SetTop(this.HighlightSection.Rectangle, canvasSection.Y);

      // Ensure our highlight section is in the background, and not covering
      // an image.
      MoveAllImagesToFront();
      Panel.SetZIndex(this.HighlightSection.Rectangle, 0);
      this.HighlightSection.Rectangle.Visibility = Visibility.Visible;
    }

    /// <summary>
    /// Retrieves a canvas section object associated with the 
    /// provided point.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public CanvasSection GetCanvasSection(int x, int y)
    {
      var rows = this.Rows;
      var columns = this.Columns;
      var sectionWidth = (int)this.Canvas.ActualWidth / columns;
      var sectionHeight = (int)this.Canvas.ActualHeight / rows;

      // Retrive the cooresponding canvas section based on the x/y coordinates.
      for (var row = 0; row < rows; ++row)
      {
        for (var col = 0; col < columns; ++col)
        {
          var xStart = sectionWidth * (col);
          var xEnd = xStart + sectionWidth;

          var yStart = sectionHeight * row;
          var yEnd = yStart + sectionHeight;

          if (x < xStart || x > xEnd || y < yStart || y > yEnd) continue;
          return new CanvasSection
          {
            X = xStart,
            Y = yStart,
            Row = row,
            Column = col,
            Width = sectionWidth,
            Height = sectionHeight
          };
        }
      }
      return null;
    }

    public void AddImage(Image image, DockableWindow dockableWindow)
    {
      this.DockableWindowImageDict.Add(dockableWindow, image);
    }

    public void SizeImageToCanvas(Image image)
    {
      var screenWidth = this.Canvas.ActualWidth;
      var screenHeight = this.Canvas.ActualHeight;
      image.Width = (screenWidth / this.Columns);
      image.Height = (screenHeight / this.Rows);
    }

    public void Clear()
    {
      foreach (var image in this.DockableWindowImageDict.Values)
      {
        image.Visibility = Visibility.Hidden;
      }
      this.DockableWindowImageDict.Clear();
      this.HighlightSection.Rectangle = null;
    }

    public void MoveAllImagesToBack()
    {
      foreach (var img in this.DockableWindowImageDict.Values)
      {
        Panel.SetZIndex(img, 0);
      }
    }

    public void MoveAllImagesToFront()
    {
      foreach (var img in this.DockableWindowImageDict.Values)
      {
        Panel.SetZIndex(img, 1);
      }
    }

    private DockableWindow GetDockableWindowForImage(Image image)
    {
      return (from kv in this.DockableWindowImageDict
              where kv.Value == image
              select kv.Key).FirstOrDefault();
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

  public class CanvasSection
  {
    public int X { get; set; }
    public int Y { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
  }
}
