using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Windowmancer.Models
{
  public class WindowInfo
  {
    public string Name { get; set; }
    public LocationInfo LocationInfo { get; set; }
    public SizeInfo SizeInfo { get; set; }
    public WindowMatchCriteria MatchCriteria { get; set; }

    public bool IsMatch(Process p)
    {
      return WindowMatch.IsMatch(this.MatchCriteria, p);
    }
  }

  public class LocationInfo
  {
    public string DisplayName { get; set; }
    public bool PrimaryDisplay { get; set; }
    public PositionInfo PositionInfo { get; set; }

    public override string ToString()
    {
      return $"{this.DisplayName} - {PositionInfo}";
    }
  }

  public class PositionInfo
  {
    public int X { get; set; }
    public int Y { get; set; }

    public override string ToString()
    {
      return $"{this.X},{this.Y}";
    }
  }

  public class SizeInfo
  {
    public int Width { get; set; }
    public int Height { get; set; }

    public override string ToString()
    {
      return $"{this.Width}x{this.Height}";
    }
  }

  public enum WindowMatchCriteriaType
  {
    WindowTitle,
    ProcessName,
  }
}
