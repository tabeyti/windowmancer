using System.Collections.ObjectModel;

namespace WindowmancerWPF.Models
{
  public class Profile : PropertyNotifyBase
  {
    public string Name
    {
      get { return GetProperty<string>();  }
      set { SetProperty(value); }
    }
    public string Id { get; set; }
    public ObservableCollection<WindowInfo> Windows { get; set; }

    public bool IsActive
    {
      get { return GetProperty<bool>(); }
      set { SetProperty(value); }
    }

    public Profile()
    {
      RegisterProperty<string>("Name");
      RegisterProperty<bool>("IsActive");
    }

    public void Update(Profile profile)
    {
      this.Name = profile.Name;
    }
  }
}
