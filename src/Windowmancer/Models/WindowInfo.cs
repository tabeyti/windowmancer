using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Windowmancer.Models
{
  public class WindowInfo
  {
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
    public Position Info { get; set; }
  }

  public class Position
  {
    public int X { get; set; }
    public int Y { get; set; }
  }

  public class SizeInfo
  {
    public int Width { get; set; }
    public int Height { get; set; }
  }

  public class WindowMatchCriteria
  {
    public WindowMatchCriteriaType MatchType { get; }
    public string MatchString { get; set; }
  }

  public static class WindowMatch
  {
    public static bool IsMatch(WindowMatchCriteria criteria, Process p)
    {
      switch(criteria.MatchType)
      {
        case WindowMatchCriteriaType.ProcessName:
          return false;
        case WindowMatchCriteriaType.WindowTitle:
          var m = Regex.Match(p.MainWindowTitle, criteria.MatchString);
          return m.Success;
        default:
          return false;
      }
    }
  }

  public enum WindowMatchCriteriaType
  {
    WindowTitle,
    ProcessName,
  }
}
