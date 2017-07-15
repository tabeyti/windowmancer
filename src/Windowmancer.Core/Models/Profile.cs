using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using Windowmancer.Core.Practices;

namespace Windowmancer.Core.Models
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

    public ObservableCollection<WindowContainer> Containers { get; set; }
    
    private UserData _userData = null;

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
      _userData = _userData ?? Helper.ServiceResolver.Resolve<UserData>();
      this.Name = profile.Name;
      _userData.Save();
    }

    public object Clone()
    {
      return new Profile
      {
        Name = this.Name,
        Id = this.Id,
        Windows =  this.Windows,
        Containers = this.Containers
      };
    }
  }
}
