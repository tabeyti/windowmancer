using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WindowmancerWPF.Models
{
  public class Profile
  {
    public string Name { get; set; }
    public string Id { get; set; }
    public ObservableCollection<WindowInfo> Windows { get; set; }
  }
}
