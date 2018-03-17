using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Windowmancer.Core.Practices;

namespace Windowmancer.Core.Models
{
  public class HostContainerConfig : PropertyNotifyBase, ICloneable
  {
    /// <summary>
    /// The ID of the container.
    /// </summary>
    public string Id
    {
      get => GetProperty<string>();
      set => SetProperty(value);
    }

    /// <summary>
    /// The name of the host container.
    /// </summary>
    public string Name
    {
      get => GetProperty<string>();
      set => SetProperty(value);
    }
    
    /// <summary>
    /// The number of rows.
    /// </summary>
    public int Rows
    {
      get => GetProperty<int>();
      set => SetProperty(value);
    }

    /// <summary>
    /// The number of columns.
    /// </summary>
    public int Columns
    {
      get => GetProperty<int>();
      set => SetProperty(value);
    }

    /// <summary>
    /// Indicates whether the container is active.
    /// </summary>
    [JsonIgnore]
    public bool IsActive
    {
      get => GetProperty<bool>();
      set => SetProperty(value);
    }

    /// <summary>
    /// List of docked windows for the container.
    /// </summary>
    [JsonIgnore]
    // Used specifically for HostContainer.xaml.cs.
    public ObservableCollection<DockableWindow> DockedWindows
    {
      get => GetProperty<ObservableCollection<DockableWindow>>();
      set => SetProperty(value);
    }

    public HostContainerConfig()
    {
      RegisterProperty(nameof(this.Name), "");
      RegisterProperty(nameof(this.Rows), 1);
      RegisterProperty(nameof(this.Columns), 1);
      RegisterProperty(nameof(this.DockedWindows), new ObservableCollection<DockableWindow>());
      RegisterProperty(nameof(this.IsActive), false);
      RegisterProperty(nameof(this.Id), Guid.NewGuid().ToString());
    }

    public HostContainerConfig(string name)
    {
      RegisterProperty(nameof(this.Name), name);
      RegisterProperty(nameof(this.Rows), 1);
      RegisterProperty(nameof(this.Columns), 1);
      RegisterProperty(nameof(this.DockedWindows), new ObservableCollection<DockableWindow>());
      RegisterProperty(nameof(this.IsActive), false);
      RegisterProperty(nameof(this.Id), Guid.NewGuid().ToString());
    }

    public HostContainerConfig(Screen screen)
    {
      RegisterProperty(nameof(this.Name), screen.DeviceName);
      RegisterProperty(nameof(this.Rows), 1);
      RegisterProperty(nameof(this.Columns), 1);
      RegisterProperty(nameof(this.DockedWindows), new ObservableCollection<DockableWindow>());
      RegisterProperty(nameof(this.IsActive), false);
      RegisterProperty(nameof(this.Id), Guid.NewGuid().ToString());
    }

    public HostContainerConfig(string name, int rows, int columns)
    {
      RegisterProperty(nameof(this.Name), name);
      RegisterProperty(nameof(this.Rows), rows);
      RegisterProperty(nameof(this.Columns), columns);
      RegisterProperty(nameof(this.DockedWindows), new ObservableCollection<DockableWindow>());
      RegisterProperty(nameof(this.IsActive), false);
      RegisterProperty(nameof(this.Id), Guid.NewGuid().ToString());
    }

    private UserData _userData;
    public void Update(HostContainerConfig config)
    {
      this.Name = config.Name;
      this.Rows = config.Rows;
      this.Columns = config.Columns;
      _userData = _userData ?? Helper.ServiceResolver.Resolve<UserData>();
      _userData.Save();
    }

    public object Clone()
    {
      var dc = new HostContainerConfig
      {
        Id = this.Id,
        Name = this.Name,
        Rows = this.Rows,
        Columns = this.Columns
      };

      foreach (var d in this.DockedWindows)
      {
        dc.DockedWindows.Add(d.Clone() as DockableWindow);
      }
      return dc;
    }
  }
}
