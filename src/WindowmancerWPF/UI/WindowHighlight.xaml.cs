using System.Windows;
using WindowmancerWPF.Models;

namespace WindowmancerWPF.UI
{
  /// <summary>
  /// Interaction logic for ScreenHighlight.xaml
  /// </summary>
  public partial class WindowHighlight : Window
  {
    public WindowHighlight()
    {
      InitializeComponent();
    }

    public void UpdateLayout(int x, int y, int width, int height)
    {
      this.Width = width;
      this.Height = height;
      this.Left = x;
      this.Top = y;
    }

    public void UpdateLayout(WindowLayoutInfo layoutInfo)
    {
      UpdateLayout(
        layoutInfo.PositionInfo.X,
        layoutInfo.PositionInfo.Y,
        layoutInfo.SizeInfo.Width,
        layoutInfo.SizeInfo.Height);
    }
  }
}

