using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Windowmancer.Core.Models
{
  public class WindowContainer : PropertyNotifyBase, ICloneable
  {
    public string Name
    {
      get => GetProperty<string>();
      set => SetProperty(value);
    }
    
    public int Width
    {
      get => GetProperty<int>();
      set => SetProperty(value);
    }

    public int Height
    {
      get => GetProperty<int>();
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

    public ObservableCollection<DockableWindow> DockedWindows
    {
      get => GetProperty<ObservableCollection<DockableWindow>>();
      set => SetProperty(value);
    }

    public WindowContainer(Screen screen)
    {
      RegisterProperty("Name", screen.DeviceName);
      RegisterProperty("Width", screen.Bounds.Width);
      RegisterProperty("Height", screen.Bounds.Height);
      RegisterProperty("Rows", 1);
      RegisterProperty("Columns", 1);
      RegisterProperty<DockableWindow>("ActiveDockedWindow");
      RegisterProperty("DockedWindows", new ObservableCollection<DockableWindow>());
    }
    
    public WindowContainer(string name, int width, int height)
    {
      RegisterProperty("Name", name);
      RegisterProperty("Width", width);
      RegisterProperty("Height", height);
      RegisterProperty("Rows", 1);
      RegisterProperty("Columns", 1);
      RegisterProperty<DockableWindow>("ActiveDockedWindow");
      RegisterProperty("DockedWindows", new ObservableCollection<DockableWindow>());
    }

    public WindowContainer(string name, int width, int height, int rows, int columns)
    {
      RegisterProperty("Name", name);
      RegisterProperty("Width", width);
      RegisterProperty("Height", height);
      RegisterProperty("Rows", rows);
      RegisterProperty("Columns", columns);
      RegisterProperty<DockableWindow>("ActiveDockedWindow");
      RegisterProperty("DockedWindows", new ObservableCollection<DockableWindow>());
    }

    public object Clone()
    {
      var dc = new WindowContainer(this.Name, this.Width, this.Height);
      foreach (var d in this.DockedWindows)
      {
        dc.DockedWindows.Add(d.Clone() as DockableWindow);
      }
      return dc;
    }
  }
}
