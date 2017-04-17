using System.Windows;

namespace WindowmancerWPF.UI
{
  /// <summary>
  /// Interaction logic for ScreenHighlight.xaml
  /// </summary>
  public partial class ScreenHighlight : Window
  {
    public ScreenHighlight()
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
  }
}
