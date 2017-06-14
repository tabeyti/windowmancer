using System.ComponentModel;
using System.Windows.Forms;
using Windowmancer.Core.Practices;

namespace Windowmancer.Core.Models
{
  public class DisplayAspectRatio : INotifyPropertyChanged
  {
    public double XRatio { get; set; }
    public double YRatio { get; set; }

    public DisplayAspectRatio(Screen screen)
    {
      Update(screen.Bounds.Width, screen.Bounds.Height);
    }

    public DisplayAspectRatio(DisplayContainer container)
    {
      Update(container.Width, container.Height);
    }

    public void Update(Screen screen)
    {
      Update(screen.Bounds.Width, screen.Bounds.Height);
    }

    public void Update(DisplayContainer container)
    {
      Update(container.Width, container.Height);
    }

    public void Update(int width, int height)
    {
      var nGCD = Helper.GetGreatestCommonDivisor(height, width);
      this.XRatio = width / nGCD;
      this.YRatio = height / nGCD;
      OnPointPropertyChanged("Width");
      OnPointPropertyChanged("Height");
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPointPropertyChanged(string prop)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
  }
}
