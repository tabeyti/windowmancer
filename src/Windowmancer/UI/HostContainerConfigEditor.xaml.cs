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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Windowmancer.Core.Models.Base;
using Windowmancer.Core.Services.Base;
using Windowmancer.Extensions;
using Xceed.Wpf.Toolkit;
using Panel = System.Windows.Controls.Panel;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for DisplayHelper.xaml
  /// </summary>
  public partial class HostContainerConfigEditor : UserControl
  {
    public Action OnClose { get; set; }
    public Action<HostContainerConfig> OnSave { get; set; }
    public bool DisplayContainersSelectable { get; set; }
    public HostContainerViewModel HostContainerConfigEditorViewModel { get; set; }
    public CanvasViewModel CanvasViewModel { get; set; }

    private readonly SolidColorBrush _defaultBrush = Brushes.Turquoise;
    private DisplayAspectRatio _screenAspectRatio;

    public HostContainerConfigEditor(HostContainerConfig windowContainer, SizeInfo sizeInfo)
    {
      this.HostContainerConfigEditorViewModel = new HostContainerViewModel
      {
        ActiveWindowContainer = windowContainer,
        SizeInfo = sizeInfo
      };
      PreInitialize();
      InitializeComponent();
      PostInitialize();
    }

    private void PreInitialize()
    {
      this.DisplayContainersSelectable = true;
      this.CanvasViewModel = new CanvasViewModel(this.HostContainerConfigEditorViewModel);
    }

    private void PostInitialize()
    {
      this.RowSpinner.ValueChanged += RowColSpinners_ValueChanged;
      this.ColumnSpinner.ValueChanged += RowColSpinners_ValueChanged;
      SizeDisplayHelperBox();
    }
    
    private void RecreateDisplaySectionControl(int rows, int cols)
    {
      // Reset.
      this.DisplayPanelGrid.Children.Clear();
      this.CanvasViewModel.Reset();

      // Create.
      this.CanvasViewModel.Rows = this.HostContainerConfigEditorViewModel.ActiveWindowContainer.Rows;
      this.CanvasViewModel.Columns = this.HostContainerConfigEditorViewModel.ActiveWindowContainer.Columns;
      this.CanvasViewModel.Canvas = new Canvas { Background = _defaultBrush };

      var dockableWindows = this.HostContainerConfigEditorViewModel.ActiveWindowContainer.DockedWindows;
      Enumerable.Range(0, rows).ForEach(r =>
        Enumerable.Range(0, cols).ForEach(c =>
        {
          var i = (cols * r) + c;
          if (i >= dockableWindows.Count)
          {
            return;
          }
          var d = dockableWindows[i];

          // If row/col doesn't fit into grid, place dockable window in next 
          // available slot.
          if (d.Row >= this.CanvasViewModel.Rows || d.Column >= this.CanvasViewModel.Columns)
          {
            var ds = this.CanvasViewModel.GetNextAvailableSection();
            if (null == ds)
            {
              throw new Exception(
                $"RecreateDisplaySectionControl - Could not find another available section on the canvas to place process {d.Process.MainWindowTitle}");
            }
            d.Row = ds.Row;
            d.Column = ds.Column;
          }

          var image = new Image
          {
            Source = Helper.ScreenShotProcessWindow(d.Process),
            ToolTip = $"{d.Process.MainWindowTitle}"
          };
          this.CanvasViewModel.AddImage(image, d);
        }));

      // Add.
      this.DisplayPanelGrid.Children.Add(this.CanvasViewModel.Canvas);
    }
    
    private void SizeDisplayHelperBox()
    {
      _screenAspectRatio = new DisplayAspectRatio(1152, 648);
      if (this.HostContainerConfigEditorViewModel.SizeInfo.Height > this.HostContainerConfigEditorViewModel.SizeInfo.Width)
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
      window.KeyDown -= HostContainerConfigEditor_HandleKeyPress;
      OnClose?.Invoke();
    }
    
    #region Event Methods

    private void HostContainerConfigEditor_HandleKeyPress(object sender, System.Windows.Input.KeyEventArgs e)
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
      window.KeyDown += HostContainerConfigEditor_HandleKeyPress;

      RecreateDisplaySectionControl(
        this.HostContainerConfigEditorViewModel.ActiveWindowContainer.Rows,
        this.HostContainerConfigEditorViewModel.ActiveWindowContainer.Columns);
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
            this.HostContainerConfigEditorViewModel.ActiveWindowContainer.DockedWindows.Count)
        {
          var spinner = (IntegerUpDown)sender;
          spinner.Value = (int)e.OldValue;

          var message =
            "WARNING: Can't change grid size to a section count lesser than the amount of docked windows.";
          var parentWindow = (IToastHost)Window.GetWindow(this);
          if (null == parentWindow)
          {
            Xceed.Wpf.Toolkit.MessageBox.Show(message, "HEY!");
            return;
          }
          parentWindow.ShowMessageToast(message);
          return;
        }
      }

      RecreateDisplaySectionControl(
        this.HostContainerConfigEditorViewModel.ActiveWindowContainer.Rows,
        this.HostContainerConfigEditorViewModel.ActiveWindowContainer.Columns);
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
      OnSave?.Invoke(this.HostContainerConfigEditorViewModel.ActiveWindowContainer);
      Close();
    }
    private void LabelTextBox_OnGotFocus(object sender, RoutedEventArgs e)
    {
      (sender as LabelTextBox)?.BaseTextBox.SelectAll();
    }

    #endregion Event Methods
  }

  public class HostContainerViewModel : PropertyNotifyBase
  {
    public HostContainerConfig ActiveWindowContainer
    {
      get 
      {
        var dc = GetProperty<HostContainerConfig>();
        this.DisplayAspectRatio = new DisplayAspectRatio(Screen.PrimaryScreen);
        return dc;
      }
      set
      {
        this.ActiveDockableWindow = value.DockedWindows.First();
        SetProperty(value);
      } 
    }

    public DockableWindow ActiveDockableWindow
    {
      get => GetProperty<DockableWindow>();
      set => SetProperty(value);
    }

    public DisplayAspectRatio DisplayAspectRatio
    {
      get => GetProperty<DisplayAspectRatio>();
      private set => SetProperty(value);
    }

    public SizeInfo SizeInfo
    {
      get => GetProperty<SizeInfo>();
      set => SetProperty(value);
    }
    
    public HostContainerViewModel()
    {
      RegisterProperty<DockableWindow>("ActiveDockableWindow", null);
      RegisterProperty<HostContainerConfig>("ActiveWindowContainer", null);
      RegisterProperty("DisplayContainers", new ObservableCollection<HostContainerConfig>());
      RegisterProperty<DisplayAspectRatio>("DisplayAspectRatio", null);
      RegisterProperty<SizeInfo>("SizeInfo", null);
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
    
    private readonly HostContainerViewModel _hcViewModel;

    public CanvasViewModel(HostContainerViewModel hcViewModel)
    {
      _hcViewModel = hcViewModel;
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
    /// Moves the provided image to the row/col section of the canvas,
    /// updating the associated DockableWindow object as well.
    /// </summary>
    /// <param name="image">The image to move.</param>
    /// <param name="row">The row index.</param>
    /// <param name="column">The column index.</param>
    public void SetImageToSection(Image image, int row, int column)
    {
      if (row >= this.Rows || column >= this.Columns)
      {
        throw new Exception("SetImageToSection - Cannot move an image to a row/col outside of the current grid.");
      }

      var dockableWindow = GetDockableWindowForImage(image);
      if (null == dockableWindow)
      {
        throw new Exception("No dockable window found for provided image.");
      }
      dockableWindow.Row = row;
      dockableWindow.Column = column;

      var rows = this.Rows;
      var columns = this.Columns;

      var sectionWidth = (int) this.Canvas.ActualWidth / columns;
      var sectionHeight = (int) this.Canvas.ActualHeight / rows;

      var x = column * sectionWidth;
      var y = row * sectionHeight;

      Canvas.SetLeft(image, x);
      Canvas.SetTop(image, y);
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
          Fill = new SolidColorBrush {Color = Color.FromRgb(0, 122, 204)}
        };
        this.Canvas.Children.Add(this.HighlightSection.Rectangle);
      }
      // Ensure our highlight section is in the background, and not covering
      // an image.
      this.HighlightSection.Rectangle.Visibility = Visibility.Visible;
      this.HighlightSection.Rectangle.MoveToBack();

      // Move our highlight section to the cooresponding section.
      Canvas.SetLeft(this.HighlightSection.Rectangle, canvasSection.X);
      Canvas.SetTop(this.HighlightSection.Rectangle, canvasSection.Y);
    }

    /// <summary>
    /// Retrieves a canvas section object associated with the 
    /// provided point location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public CanvasSection GetCanvasSection(int x, int y)
    {
      var rows = this.Rows;
      var columns = this.Columns;
      var sectionWidth = (int) this.Canvas.ActualWidth / columns;
      var sectionHeight = (int) this.Canvas.ActualHeight / rows;

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
        img.MoveToBack();
      }
    }

    /// <summary>
    /// Controls the visibility of the highlight section.
    /// </summary>
    /// <param name="show">True for show, false for not.</param>
    public void ShowHighlightSection(bool show)
    {
      var canvasSection = GetCanvasSection((int) this.MousePosition.X, (int) this.MousePosition.Y);
      UpdateHighlightSection(canvasSection);

      if (show)
      {
        this.HighlightSection.Rectangle.Visibility = Visibility.Visible;
        this.HighlightSection.Rectangle.MoveToBack();
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

    /// <summary>
    /// Initializes a new canvas for our canvas view model.
    /// </summary>
    private void InitializeNewCanvas()
    {
      // Set up events.
      this.Canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
      this.Canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
      this.Canvas.MouseMove += Canvas_MouseMove;

      this.Canvas.Background = Brushes.DarkSlateGray;
      
      // Size and set images to their respective sections on load.
      this.Canvas.Loaded += (o, e) =>
      {
        var yOffset = this.Canvas.ActualHeight / this.Rows;
        var xOffset = this.Canvas.ActualWidth / this.Columns;
        DrawGridLines((int)yOffset, (int)xOffset, this.Canvas);

        foreach (var kv in this.DockableWindowImageDict)
        {
          var image = kv.Value;
          SizeImageToCanvas(image);
          this.Canvas.Children.Add(image);
          SetImageToSection(image, kv.Key.Row, kv.Key.Column);
        }
      };
    }

    /// <summary>
    /// Draws a grid, using the x/y offset values for the section size,
    /// onto the provided canvas.
    /// </summary>
    /// <param name="yoffSet"></param>
    /// <param name="xoffSet"></param>
    /// <param name="mainCanvas"></param>
    private void DrawGridLines(int yoffSet, int xoffSet, Canvas mainCanvas)
    {
      RemoveGraph(mainCanvas);
      var lines = new Image();
      lines.SetValue(Panel.ZIndexProperty, 0);

      //Draw the grid
      var gridLinesVisual = new DrawingVisual();
      var dct = gridLinesVisual.RenderOpen();
      var lightPen = new Pen(new SolidColorBrush(Colors.White), 1);
      var darkPen = new Pen(new SolidColorBrush(Colors.White), 1.5);
      lightPen.Freeze();
      darkPen.Freeze();
      
      var canvasHeight = mainCanvas.ActualHeight;
      var canvasWidth = mainCanvas.ActualWidth;

      int yOffset = yoffSet,
        xOffset = xoffSet,
        rows = (int)(canvasHeight),
        columns = (int)(canvasWidth),
        alternate = yOffset == 5 ? yOffset : 1,
        j = 0;

      //Draw the horizontal lines
      var xPoint = new Point(0, 1);
      var yPoint = new Point(canvasWidth, 1);

      for (var i = j = 0; i <= rows; i++, j++)
      {
        dct.DrawLine(j % alternate == 0 ? lightPen : darkPen, xPoint, yPoint);
        xPoint.Offset(0, yOffset);
        yPoint.Offset(0, yOffset);
      }

      //Draw the vertical lines
      xPoint = new Point(1, 0);
      yPoint = new Point(1, canvasHeight);

      for (var i = j = 0; i <= columns; i++, j++)
      {
        dct.DrawLine(j % alternate == 0 ? lightPen : darkPen, xPoint, yPoint);
        xPoint.Offset(xOffset, 0);
        yPoint.Offset(xOffset, 0);
      }
      dct.Close();

      var bmp = new RenderTargetBitmap((int)canvasWidth,
        (int)canvasHeight, 96, 96, PixelFormats.Pbgra32);
      bmp.Render(gridLinesVisual);
      bmp.Freeze();
      lines.Source = bmp;
      mainCanvas.Children.Add(lines);
    }

    private void RemoveGraph(Canvas mainCanvas)
    {
      foreach (UIElement obj in mainCanvas.Children)
      {
        if (obj is Image)
        {
          mainCanvas.Children.Remove(obj);
          break;
        }
      }
    }

    /// <summary>
    /// Retrieves an available canvas section for usage, meaning a 
    /// canvas section currently not occupied by an image.
    /// </summary>
    /// <returns></returns>
    public CanvasSection GetNextAvailableSection()
    {
      for (var row = 0; row < this.Rows; ++row)
      {
        for (var column = 0; column < this.Columns; ++column)
        {
          if (this.DockableWindowImageDict.Keys.Any(
            dw => dw.Row == row && dw.Column == column))
          {
            continue;
          }
          return new CanvasSection {Row = row, Column = column};
        }
      }
      return null;
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

      // Notify others we are dragging a dockable window.
      _hcViewModel.ActiveDockableWindow = GetDockableWindowForImage(this.DraggedImage);

      // Set dragged image as top and other images to bottom.
      this.DraggedImage.MoveToFront();
      this.MoveAllOtherImagesToBack();
    }

    private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (this.DraggedImage == null) return;
      this.Canvas.ReleaseMouseCapture();
      this.ShowHighlightSection(false);

      // If we are on top of another image, move that image to where our
      // draggable image used to be.
      var canvasSection = GetCanvasSection((int) this.MousePosition.X, (int) this.MousePosition.Y);
      var dockableWindow = GetDockableWindowForImage(this.DraggedImage);
      foreach (var kv in this.DockableWindowImageDict)
      {
        if (kv.Value == this.DraggedImage) continue;

        if (kv.Key.Row == canvasSection.Row && kv.Key.Column == canvasSection.Column)
        {
          SetImageToSection(kv.Value, dockableWindow.Row, dockableWindow.Column);
        }
      }
      SetImageToSection(this.DraggedImage, canvasSection.Row, canvasSection.Column);
      this.DraggedImage = null;
    }

    private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
      if (this.DraggedImage == null) return;

      // Update new mouse position, preventing mouse coordinates from laying outside the canvas bounds.
      var position = e.GetPosition(this.Canvas);
      var offset = position - this.MousePosition;
      position.X = position.X < 0 ? 0 : position.X;
      position.Y = position.Y < 0 ? 0 : position.Y;
      position.X = position.X > this.Canvas.ActualWidth ? this.Canvas.ActualWidth : position.X;
      position.Y = position.Y > this.Canvas.ActualHeight ? this.Canvas.ActualHeight : position.Y;
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
      
      // Get canvas section from mouse-relative position.
      var canvasSection = GetCanvasSection((int)this.MousePosition.X, (int)this.MousePosition.Y);

      // Update highlight section.
      UpdateHighlightSection(canvasSection); 
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
