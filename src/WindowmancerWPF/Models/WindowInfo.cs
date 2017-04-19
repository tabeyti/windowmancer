using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace WindowmancerWPF.Models
{
  public class WindowInfo
  {
    public string Name { get; set; }
    public LocationInfo LocationInfo { get; set; }
    public SizeInfo SizeInfo { get; set; }
    public WindowMatchCriteria MatchCriteria { get; set; }
    public bool BringToFront { get; set; }
    public bool IsMatch(Process p)
    {
      return WindowMatch.IsMatch(this.MatchCriteria, p);
    }
  }

  public class WindowLayoutInfo
  {
    public PositionInfo PositionInfo { get; set; }
    public SizeInfo SizeInfo { get; set; }
  }

  public class LocationInfo
  {
    public string DisplayName { get; set; }
    public bool PrimaryDisplay { get; set; }
    public PositionInfo PositionInfo { get; set; }

    public override string ToString()
    {
      return PositionInfo.ToString();
    }
  }

  public class PositionInfo : INotifyPropertyChanged
  {
    private int _x;
    public int X
    {
      get { return _x; }
      set
      {
        if (value != _x)
        {
          _x = value;
          OnPointPropertyChanged("X");
        }
      }
    }
    private int _y;
    public int Y
    {
      get { return _y;  }
      set
      {
        if (value != _y)
        {
          _y = value;
          OnPointPropertyChanged("Y");
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPointPropertyChanged(string prop)
    {      
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));      
    }

    public override string ToString()
    {
      return $"({this.X}, {this.Y})";
    }

    public PositionInfo()
    {
      this.X = this.Y = 0;
    }
  }

  public class SizeInfo : INotifyPropertyChanged
  {
    private int _width;
    private int _height;
    public int Width
    {
      get { return _width; }
      set
      {
        if (value != _width)
        {
          _width = value;
          OnPointPropertyChanged("Width");
        }
      }
    }
    public int Height
    {
      get { return _height; }
      set
      {
        if (value != _height)
        {
          _height = value;
          OnPointPropertyChanged("Height");
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public override string ToString()
    {
      return $"{this.Width}x{this.Height}";
    }

    private void OnPointPropertyChanged(string prop)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
  }

  public enum WindowMatchCriteriaType
  {
    WindowTitle,
    ProcessName,
  }
}
