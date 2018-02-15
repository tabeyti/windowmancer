using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Windowmancer.Core.Models
{
  public class HostContainerConfig : PropertyNotifyBase, ICloneable
  {
    public string Id
    {
      get => GetProperty<string>();
      set => SetProperty(value);
    }

    public string Name
    {
      get => GetProperty<string>();
      set => SetProperty(value);
    }
    
    public int Rows
    {
      get => GetProperty<int>();
      set => SetProperty(value);
    }

    public int Columns
    {
      get => GetProperty<int>();
      set => SetProperty(value);
    }

    [JsonIgnore]
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
      RegisterProperty(nameof(this.Id), Guid.NewGuid().ToString());
    }

    public HostContainerConfig(string name)
    {
      RegisterProperty(nameof(this.Name), name);
      RegisterProperty(nameof(this.Rows), 1);
      RegisterProperty(nameof(this.Columns), 1);
      RegisterProperty(nameof(this.DockedWindows), new ObservableCollection<DockableWindow>());
      RegisterProperty(nameof(this.Id), Guid.NewGuid().ToString());
    }

    public HostContainerConfig(Screen screen)
    {
      RegisterProperty(nameof(this.Name), screen.DeviceName);
      RegisterProperty(nameof(this.Rows), 1);
      RegisterProperty(nameof(this.Columns), 1);
      RegisterProperty(nameof(this.DockedWindows), new ObservableCollection<DockableWindow>());
      RegisterProperty(nameof(this.Id), Guid.NewGuid().ToString());
    }

    public HostContainerConfig(string name, int rows, int columns)
    {
      RegisterProperty(nameof(this.Name), name);
      RegisterProperty(nameof(this.Rows), rows);
      RegisterProperty(nameof(this.Columns), columns);
      RegisterProperty(nameof(this.DockedWindows), new ObservableCollection<DockableWindow>());
      RegisterProperty(nameof(this.Id), Guid.NewGuid().ToString());
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
