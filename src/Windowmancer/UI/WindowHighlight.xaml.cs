using System.Threading.Tasks;
using System.Windows;
using Windowmancer.Core.Models;
using Windowmancer.Core.Practices;

namespace Windowmancer.UI
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

    public void UpdateLayout(
      int x, 
      int y, 
      int width, 
      int height)
    {
      this.Width = width;
      this.Height = height;
      this.Left = x;
      this.Top = y;
    }

    public void UpdateLayout(
      int x,
      int y,
      int width,
      int height,
      int timeoutSecs)
    {
      this.Width = width;
      this.Height = height;
      this.Left = x;
      this.Top = y;

      Task.Run(() =>
      {
        Task.Delay(timeoutSecs*1000).Wait();
        Helper.Dispatcher.Invoke(this.Close);
      });
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

