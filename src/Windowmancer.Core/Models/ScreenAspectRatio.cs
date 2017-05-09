using System.ComponentModel;
using System.Windows.Forms;
using Windowmancer.Core.Practices;

namespace Windowmancer.Core.Models
{
  public class ScreenAspectRatio : INotifyPropertyChanged
  {
    public double XRatio { get; set; }
    public double YRatio { get; set; }

    public ScreenAspectRatio(Screen screen)
    {
      Update(screen);
    }

    public void Update(Screen screen)
    {
      var nGCD = Helper.GetGreatestCommonDivisor(screen.Bounds.Height, screen.Bounds.Width);
      this.XRatio = screen.Bounds.Width / nGCD;
      this.YRatio = screen.Bounds.Height / nGCD;
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
