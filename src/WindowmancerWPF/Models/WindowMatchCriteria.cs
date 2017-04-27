using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WindowmancerWPF.Models
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
      RegisterProperty("MatchType", WindowMatchCriteriaType.WindowTitle);
      RegisterProperty("MatchString", "");
    }

    public WindowMatchCriteria(WindowMatchCriteriaType type, string matchString)
    {
      RegisterProperty("MatchType", type);
      RegisterProperty("MatchString", matchString);
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
