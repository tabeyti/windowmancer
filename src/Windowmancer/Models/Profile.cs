using System.ComponentModel;

namespace Windowmancer.Models
{
  public class Profile
  {
    public string Name { get; set; }
    [Browsable(false)]
    public string Id { get; set; }
    [Browsable(false)]
    public BindingList<WindowInfo> Windows { get; set; }
  }
}
