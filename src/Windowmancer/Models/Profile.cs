using System.ComponentModel;

namespace Windowmancer.Models
{
  public class Profile
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public BindingList<WindowInfo> Windows { get; set; }
  }
}
