using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
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

    private readonly ObservableCollection<MonitorWindowInfoView> _monitorWindows = new ObservableCollection<MonitorWindowInfoView>();
    [JsonIgnore]
    public ObservableCollection<MonitorWindowInfoView> MonitorWindows
    {
      get
      {
        _monitorWindows.Clear();
        this.Windows.ForEach(c =>
        {
          if (c.ContainerLayoutInfo == null) _monitorWindows.Add(new MonitorWindowInfoView(c));
        });
        return _monitorWindows;
      }
    }

    private readonly ObservableCollection<ContainerWindowInfoView> _containerWindows = new ObservableCollection<ContainerWindowInfoView>();
    [JsonIgnore]
    public ObservableCollection<ContainerWindowInfoView> ContainerWindows
    {
      get
      {
        _containerWindows.Clear();
        this.Windows.ForEach(c =>
        {
          if (c.ContainerLayoutInfo != null) _containerWindows.Add(new ContainerWindowInfoView(c));
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

  public class WindowInfoView
  {
    public string Name => _windowInfo.Name;
    public WindowMatchCriteria MatchCriteria => _windowInfo.MatchCriteria;
    public bool ApplyOnProcessStart => _windowInfo.ApplyOnProcessStart;
    public WindowStylingInfo StylingInfo => _windowInfo.StylingInfo;

    protected readonly WindowInfo _windowInfo;

    public WindowInfoView(WindowInfo windowInfo)
    {
      _windowInfo = windowInfo;
    }
  }

  /// <summary>
  /// View class for WindowInfo objects, specific to monitor layout information.
  /// </summary>
  public class MonitorWindowInfoView : WindowInfoView
  {
    public MonitorLayoutInfo MonitorLayoutInfo => _windowInfo.MonitorLayoutInfo;

    public MonitorWindowInfoView(WindowInfo windowInfo) : base(windowInfo)
    {
    }
  }

  /// <summary>
  /// View class for WindowInfo objects, specific to container layout information.
  /// </summary>
  public class ContainerWindowInfoView : WindowInfoView
  {
    public ContainerLayoutInfo ContainerLayoutInfo => _windowInfo.ContainerLayoutInfo;

    public ContainerWindowInfoView(WindowInfo windowInfo) : base(windowInfo)
    {
    }
  }
}
