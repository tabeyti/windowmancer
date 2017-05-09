using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Windowmancer.Core.Models
{
  public class HotKeyConfigModel
  {
    public List<ModifierKeys> ModifierKeys { get; set; }
    public Key PrimaryKey { get; set; }

    public HotKeyConfigModel()
    {
      this.ModifierKeys = new List<ModifierKeys>();
      this.PrimaryKey = Key.None;
    }

    public HotKeyConfigModel(IEnumerable<ModifierKeys> modifierKeys, Key primaryKey)
    {
      this.ModifierKeys = modifierKeys.ToList();
      this.PrimaryKey = primaryKey;
    }
  }
}
