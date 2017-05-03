using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windowmancer.Services;

namespace Windowmancer.Models
{
  public class PreferencesModel : ICloneable
  {
    public HotKeyConfig HotKeyConfig { get; set; }

    public PreferencesModel() { }

    public PreferencesModel(HotKeyConfig hotKeyConfig)
    {
      this.HotKeyConfig = hotKeyConfig;
    }

    public object Clone()
    {
      return new PreferencesModel
      {
        HotKeyConfig = new HotKeyConfig
        {
          PrimaryKey = this.HotKeyConfig.PrimaryKey,
          ModifierKeys = this.HotKeyConfig.ModifierKeys.ToList()
        }
      };
    }
  }
}
