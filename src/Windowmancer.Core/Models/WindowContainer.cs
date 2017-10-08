using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Windowmancer.Core.Models
{
  public class WindowContainer : PropertyNotifyBase, ICloneable
  {
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

    public WindowContainer()
    {
      RegisterProperty("Name", "");
      RegisterProperty("Rows", 1);
      RegisterProperty("Columns", 1);
      RegisterProperty("DockedWindows", new ObservableCollection<DockableWindow>());
    }

    public WindowContainer(string name)
    {
      RegisterProperty("Name", name);
      RegisterProperty("Rows", 1);
      RegisterProperty("Columns", 1);
      RegisterProperty("DockedWindows", new ObservableCollection<DockableWindow>());
    }

    public WindowContainer(Screen screen)
    {
      RegisterProperty("Name", screen.DeviceName);
      RegisterProperty("Width", screen.Bounds.Width);
      RegisterProperty("Height", screen.Bounds.Height);
      RegisterProperty("Rows", 1);
      RegisterProperty("Columns", 1);
      RegisterProperty("DockedWindows", new ObservableCollection<DockableWindow>());
    }

    public WindowContainer(string name, int rows, int columns)
    {
      RegisterProperty("Name", name);
      RegisterProperty("Rows", rows);
      RegisterProperty("Columns", columns);
      RegisterProperty("DockedWindows", new ObservableCollection<DockableWindow>());
    }

    public object Clone()
    {
      var dc = new WindowContainer(this.Name);
      foreach (var d in this.DockedWindows)
      {
        dc.DockedWindows.Add(d.Clone() as DockableWindow);
      }
      return dc;
    }
  }
}
