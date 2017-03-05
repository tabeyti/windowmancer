using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Windowmancer.Models
{
  public class WindowInfo
  {
    public LocationInfo LocationInfo { get; set; }
    public SizeInfo SizeInfo { get; set; }
    public IWindowMatchCreteria MatchCriteria { get; set; }
    
    public bool IsMatch(Process p)
    {
      return MatchCriteria.IsMatch(p);
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

  public interface IWindowMatchCreteria
  {
    WindowMatchCriteriaType MatchType { get; }
    string MatchString { get; set; }
    bool IsMatch(Process p);
  }

  public class WindowTitleMatchCreteria : IWindowMatchCreteria
  {
    public WindowMatchCriteriaType MatchType => WindowMatchCriteriaType.WindowTitle;

    public string MatchString { get; set; }
    
    public WindowTitleMatchCreteria(string titleRegex)
    {
      MatchString = titleRegex;
    }

    public bool IsMatch(Process p)
    {
      var m = Regex.Match(p.MainWindowTitle, this.MatchString);
      return m.Success;
    }
  }

  public enum WindowMatchCriteriaType
  {
    WindowTitle,
    ProcessName,
  }
}
