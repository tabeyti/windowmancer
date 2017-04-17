using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowmancerWPF.Models
{
  public class WindowMatchCriteria
  {
    public WindowMatchCriteriaType MatchType { get; }
    public string MatchString { get; }

    public WindowMatchCriteria(WindowMatchCriteriaType type, string matchString)
    {
      this.MatchType = type;
      this.MatchString = matchString;
    }

    public override string ToString()
    {
      return $"{this.MatchType} - {this.MatchString}";
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
}
