using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Practices.ObjectBuilder2;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;
using UserControl = System.Windows.Controls.UserControl;
using System.Windows.Input;
using System.Windows.Shapes;
using Windowmancer.Core.Models.Base;
using Windowmancer.Extensions;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Core.Converters;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for DisplayHelper.xaml
  /// </summary>
  public partial class HostContainerHelper : UserControl
  {
    public Action OnClose { get; set; }
    public Action<List<DisplayContainer>> OnSave { get; set; }
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
      // Reset.
      this.DisplayPanelGrid.Children.Clear();
      this.CanvasViewModel.Reset();

      // Create.
      this.CanvasViewModel.Rows = this.HostContainerHelperViewModel.ActiveDisplayContainer.Rows;
      this.CanvasViewModel.Columns = this.HostContainerHelperViewModel.ActiveDisplayContainer.Columns;
      this.CanvasViewModel.Canvas = new Canvas { Background = _defaultBrush };

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

    private void Close()
    {
      var window = Window.GetWindow(this);
      if (window == null)
      {
        throw new Exception("WindowConfig - Could locate active window to unbind the KeyDown listener.");
      }
      window.KeyDown -= HostContainerHelper_HandleKeyPress;
      OnClose?.Invoke();
    }

    private void Save()
    {
    }

    #region Event Methods

    private void HostContainerHelper_HandleKeyPress(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key != Key.Escape)
      {
        return;
      }
      Close();
    }

    private void DisplayHelper_OnLoaded(object sender, RoutedEventArgs e)
    {
      var window = Window.GetWindow(this);
      if (window == null)
      {
        throw new Exception("WindowConfig - Could locate active window to bind the KeyDown listener.");
      }
      window.KeyDown += HostContainerHelper_HandleKeyPress;

      this.CanvasViewModel = new CanvasViewModel();
      RecreateDisplaySectionControl(
        this.HostContainerHelperViewModel.ActiveDisplayContainer.Rows,
        this.HostContainerHelperViewModel.ActiveDisplayContainer.Columns);
  }
    
    private void DisplayListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      SizeDisplayHelperBox();
    }

    private void SetDisplayHelperLayoutButton_OnClick(object sender, RoutedEventArgs e)
    {
    }
    
    private bool _rowColSpinnerEnabled = true;
    private void RowColSpinners_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      if (!_rowColSpinnerEnabled) return;
      if ((int) e.NewValue < (int) e.OldValue)
      {
        var currentNumSections = this.RowSpinner.Value * this.ColumnSpinner.Value;
        if (currentNumSections <
            this.HostContainerHelperViewModel.ActiveDisplayContainer.DockedWindows.Count)
        {
          var spinner = (IntegerUpDown)sender;
          spinner.Value = (int)e.OldValue;
          Xceed.Wpf.Toolkit.MessageBox.Show(
            "Can't change grid size to a section count lesser than the amount of docked windows. Main bitch out yo league-to-ahhh; Side bithc outa yo leangue-to-ahhh", "HEY!");
          return;
        }
      }

      RecreateDisplaySectionControl(
        this.HostContainerHelperViewModel.ActiveDisplayContainer.Rows,
        this.HostContainerHelperViewModel.ActiveDisplayContainer.Columns);
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
      Save();
      OnSave?.Invoke(this.HostContainerHelperViewModel.DisplayContainers.ToList());
      Close();
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
        InitializeNewCanvas();
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
    
    public CanvasViewModel()
    {
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

    /// <summary>
    /// Moves the provided image to the row/col section of the canvas.
    /// </summary>
    /// <param name="image">The image to move.</param>
    /// <param name="row">The row index.</param>
    /// <param name="column">The column index.</param>
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

    private void UpdateCanvasSection(int x, int y)
    {
      var canvasSection = GetCanvasSection(x, y);
      var dockableWindow = GetDockableWindowForImage(this.DraggedImage);

      // If we are on the same section, forgetaboutit. 
      if (null == canvasSection || (dockableWindow.Row == canvasSection.Row && 
        dockableWindow.Column == canvasSection.Column))
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
      UpdateHighlightSection(canvasSection);

      // Update docked window container with new row/col indices.
      dockableWindow.Row = canvasSection.Row;
      dockableWindow.Column = canvasSection.Column;
    }

    private void UpdateHighlightSection(CanvasSection canvasSection)
    {
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
          Fill = new SolidColorBrush {Color = Color.FromRgb(255, 242, 0)}
        };
        this.Canvas.Children.Add(this.HighlightSection.Rectangle);
      }
      // Ensure our highlight section is in the background, and not covering
      // an image.
      MoveAllImagesToFront();
      this.HighlightSection.Rectangle.Visibility = Visibility.Visible;
      this.HighlightSection.Rectangle.SetBottom();

      // Move our highlight section to the cooresponding section.
      Canvas.SetLeft(this.HighlightSection.Rectangle, canvasSection.X);
      Canvas.SetTop(this.HighlightSection.Rectangle, canvasSection.Y);
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

      // Retrieve the cooresponding canvas section based on the provided coordinates.
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

    /// <summary>
    /// Adds an image to our dockable window to image dictionary.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="dockableWindow"></param>
    public void AddImage(Image image, DockableWindow dockableWindow)
    {
      this.DockableWindowImageDict.Add(dockableWindow, image);
    }

    /// <summary>
    /// Sizes the passed image to our canvas grid.
    /// </summary>
    /// <param name="image"></param>
    private void SizeImageToCanvas(Image image)
    {
      var screenWidth = this.Canvas.ActualWidth;
      var screenHeight = this.Canvas.ActualHeight;

      image.Stretch = Stretch.Fill;
      image.Width = (screenWidth / this.Columns);
      image.Height = (screenHeight / this.Rows);
    }

    /// <summary>
    /// Resets our canvas grid and associated images.
    /// </summary>
    public void Reset()
    {
      foreach (var image in this.DockableWindowImageDict.Values)
      {
        image.Visibility = Visibility.Hidden;
      }
      this.DockableWindowImageDict.Clear();
      this.HighlightSection.Rectangle = null;
    }

    /// <summary>
    /// Moves all dockable window images to the back.
    /// </summary>
    public void MoveAllOtherImagesToBack()
    {
      foreach (var img in this.DockableWindowImageDict.Values)
      {
        if (img == this.DraggedImage) continue;
        img.SetBottom();
      }
    }

    /// <summary>
    /// Moves all dockable window images to the front.
    /// </summary>
    public void MoveAllImagesToFront()
    {
      foreach (var img in this.DockableWindowImageDict.Values)
      {
        img.SetTop();
      }
    }

    /// <summary>
    /// Controls the visibility of the highlight section.
    /// </summary>
    /// <param name="show">True for show, false for not.</param>
    public void ShowHighlightSection(bool show)
    {
      var canvasSection = GetCanvasSection((int)this.MousePosition.X, (int)this.MousePosition.Y);
      UpdateHighlightSection(canvasSection);

      if (show)
      {
        this.HighlightSection.Rectangle.Visibility = Visibility.Visible;
        this.HighlightSection.Rectangle.SetBottom();
      }
      else
      {
        this.HighlightSection.Rectangle.Visibility = Visibility.Hidden;
      }
    }

    /// <summary>
    /// Retrieves the associated dockable window object for the passed image.
    /// </summary>
    /// <param name="image">The image for which the user wants the dockable window for.</param>
    /// <returns>The dockable window for the image.</returns>
    private DockableWindow GetDockableWindowForImage(Image image)
    {
      return (from kv in this.DockableWindowImageDict
              where Equals(kv.Value, image)
              select kv.Key).FirstOrDefault();
    }

    private void InitializeNewCanvas()
    {
      this.Canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
      this.Canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
      this.Canvas.MouseMove += Canvas_MouseMove;

      // Size and set images to their respective sections on load.
      this.Canvas.Loaded += (o, e) =>
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

    #region Canvas Methods

    private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      // Grab image and current (relative) canvas mouse position.
      var image = e.Source as Image;
      if (image == null || !this.Canvas.CaptureMouse()) return;
      this.MousePosition = e.GetPosition(this.Canvas);
      this.ShowHighlightSection(true);
      this.DraggedImage = image;

      // Set dragged image as top and other images to bottom.
      this.DraggedImage.SetTop();
      this.MoveAllOtherImagesToBack();
    }

    private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (this.DraggedImage == null) return;
      this.Canvas.ReleaseMouseCapture();
      this.ShowHighlightSection(false);
      this.MoveImageToSection(
        this.DraggedImage,
        this.HighlightSection.Row,
        this.HighlightSection.Column);
      this.DraggedImage = null;
    }

    private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
      if (this.DraggedImage == null) return;

      // Update new mouse position.
      var position = e.GetPosition(this.Canvas);
      var offset = position - this.MousePosition;
      this.MousePosition = position;

      // Prevent image from moving outside of the canvas.
      var xPos = Canvas.GetLeft(this.DraggedImage) + offset.X;
      var yPos = Canvas.GetTop(this.DraggedImage) + offset.Y;
      if ((xPos + this.DraggedImage.Width) > this.Canvas.ActualWidth)
      {
        xPos = this.Canvas.ActualWidth - this.DraggedImage.Width;
      }
      else if (xPos < 0)
      {
        xPos = 0;
      }
      if ((yPos + this.DraggedImage.Height) > this.Canvas.ActualHeight)
      {
        yPos = this.Canvas.ActualHeight - this.DraggedImage.Height;
      }
      else if (yPos < 0)
      {
        yPos = 0;
      }
      Canvas.SetLeft(this.DraggedImage, xPos);
      Canvas.SetTop(this.DraggedImage, yPos);

      // Position relative to mouse.
      var centerX = (int)Mouse.GetPosition(this.Canvas).X;
      var centerY = (int)Mouse.GetPosition(this.Canvas).Y;

      // Update canvas section items.
      UpdateCanvasSection(centerX, centerY);
    }

    #endregion Canvas Methods
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

  public class CanvasSection : DisplaySectionBase
  {
  }
}
