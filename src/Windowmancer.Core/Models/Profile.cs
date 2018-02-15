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

    public bool IsActive
    {
      get => GetProperty<bool>();
      set => SetProperty(value);
    }

    public string Id { get; set; }
    public ObservableCollection<WindowConfig> Windows { get; set; }
    public ObservableCollection<HostContainerConfig> HostContainers { get; set; }
    
    private UserData _userData = null;

    /// <summary>
    /// Constructor.
    /// </summary>
    public Profile()
    {
      RegisterProperty<string>(nameof(Name));
      RegisterProperty<bool>(nameof(this.IsActive));
      this.HostContainers = new ObservableCollection<HostContainerConfig>();
    }

    /// <summary>
    /// Updates the profile.
    /// </summary>
    /// <param name="profile"></param>
    public void Update(Profile profile)
    {
      _userData = _userData ?? Helper.ServiceResolver.Resolve<UserData>();
      this.Name = profile.Name;
      _userData.Save();
    }

    /// <summary>
    /// Clones the profile instance.
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
      return new Profile
      {
        Name = this.Name,
        Id = this.Id,
        Windows =  this.Windows,
        HostContainers = this.HostContainers
      };
    }
  }
}
