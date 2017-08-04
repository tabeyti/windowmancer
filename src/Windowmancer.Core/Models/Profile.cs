using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Windowmancer.Core.Extensions;
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

    private readonly ObservableCollection<WindowInfo> _monitorWindows = new ObservableCollection<WindowInfo>();
    [JsonIgnore]
    public ObservableCollection<WindowInfo> MonitorWindows
    {
      get
      {
        _monitorWindows.Clear();
        this.Windows.ForEach(c =>
        {
          if (c.ContainerLayoutInfo == null) _monitorWindows.Add(c);
        });
        return _monitorWindows;
      }
    }

    private readonly ObservableCollection<WindowInfo> _containerWindows = new ObservableCollection<WindowInfo>();
    [JsonIgnore]
    public ObservableCollection<WindowInfo> ContainerWindows
    {
      get
      {
        _containerWindows.Clear();
        this.Windows.ForEach(c =>
        {
          if (c.ContainerLayoutInfo != null) _containerWindows.Add(c);
        });
        return _containerWindows;
      }
    }

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
