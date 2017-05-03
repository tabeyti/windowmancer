using System;
using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;

namespace Windowmancer.Models
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
      _userData = _userData ?? App.ServiceResolver.Resolve<UserData>();
      this.Name = profile.Name;
      _userData.Save();
    }

    public object Clone()
    {
      return new Profile { Name = this.Name, Id = this.Id };
    }
  }
}
