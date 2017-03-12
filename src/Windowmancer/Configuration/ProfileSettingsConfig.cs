using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windowmancer.Models;

namespace Windowmancer.Configuration
{
  public class ProfileSettingsConfig
  {
    public bool RefreshOnProcessStart { get; set; }
    public bool UseHotKey { get; set; }
    public List<KeyInfo> HotKeyCombo { get; set; }
  }
}
