using System;
using System.Linq;
using Windowmancer.Core.Services;

namespace Windowmancer.Core.Models
{
  public class PreferencesModel : ICloneable
  {
    public HotKeyConfigModel HotKeyConfig { get; set; }

    public PreferencesModel() { }

    public PreferencesModel(HotKeyConfigModel hotKeyConfig)
    {
      this.HotKeyConfig = hotKeyConfig;
    }

    public object Clone()
    {
      return new PreferencesModel
      {
        HotKeyConfig = new HotKeyConfigModel
        {
          PrimaryKey = this.HotKeyConfig.PrimaryKey,
          ModifierKeys = this.HotKeyConfig.ModifierKeys.ToList()
        }
      };
    }
  }
}
