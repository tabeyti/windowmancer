using System;
using System.Collections.ObjectModel;

namespace WindowmancerWPF.Models
{
  public class Profile : PropertyNotifyBase, ICloneable
  {
    public string Name
    {
      get => GetProperty<string>();
      set => SetProperty(value);
    }
    public string Id { get; set; }
    public ObservableCollection<WindowInfo> Windows { get; set; }

    public bool IsActive
    {
      get => GetProperty<bool>();
      set => SetProperty(value);
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

    public object Clone()
    {
      return new Profile { Name = this.Name, Id = this.Id };
    }
  }
}
