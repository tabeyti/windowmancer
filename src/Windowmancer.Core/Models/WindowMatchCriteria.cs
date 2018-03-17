using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Windowmancer.Core.Models
{
  public class WindowMatchCriteria : PropertyNotifyBase, ICloneable
  {
    public WindowMatchCriteriaType MatchType
    {
      get => GetProperty<WindowMatchCriteriaType>();
      set => SetProperty(value);
    }

    public string MatchString
    {
      get => GetProperty<string>();
      set => SetProperty(value);
    }

    public WindowMatchCriteria()
    {
      RegisterProperty(nameof(this.MatchType), WindowMatchCriteriaType.WindowTitle);
      RegisterProperty(nameof(this.MatchString), "");
    }

    public WindowMatchCriteria(WindowMatchCriteriaType type, string matchString)
    {
      RegisterProperty(nameof(this.MatchType), type);
      RegisterProperty(nameof(this.MatchString), matchString);
    }

    public override string ToString()
    {
      return $"{this.MatchType} - {this.MatchString}";
    }
    
    public object Clone()
    {
      return new WindowMatchCriteria(this.MatchType, this.MatchString);
    }
  }

  public static class WindowMatch
  {
    public static bool IsMatch(WindowMatchCriteria criteria, Process p)
    {
      switch (criteria.MatchType)
      {
        case WindowMatchCriteriaType.ProcessName:
          return false;
        case WindowMatchCriteriaType.WindowTitle:
          if (criteria.MatchString == string.Empty) return false;
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
