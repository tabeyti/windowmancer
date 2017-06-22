﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Windowmancer.Core.Models
{
  public class DisplayContainer : PropertyNotifyBase, ICloneable
  {
    public string Name
    {
      get => GetProperty<string>();
      set => SetProperty(value);
    }

    public int X
    {
      get => GetProperty<int>();
      set => SetProperty(value);
    }
    public int Y
    {
      get => GetProperty<int>();
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

    public DisplayContainer(Screen screen)
    {
      RegisterProperty("Name", screen.DeviceName);
      RegisterProperty("X", screen.Bounds.X);
      RegisterProperty("Y", screen.Bounds.Y);
      RegisterProperty("Width", screen.Bounds.Width);
      RegisterProperty("Height", screen.Bounds.Height);
      RegisterProperty("Rows", 1);
      RegisterProperty("Columns", 1);
      RegisterProperty<DockableWindow>("ActiveDockedWindow");
      RegisterProperty("DockedWindows", new ObservableCollection<DockableWindow>());
    }
    
    public DisplayContainer(string name, int x, int y, int width, int height)
    {
      RegisterProperty("Name", name);
      RegisterProperty("X", x);
      RegisterProperty("Y", y);
      RegisterProperty("Width", width);
      RegisterProperty("Height", height);
      RegisterProperty("Rows", 1);
      RegisterProperty("Columns", 1);
      RegisterProperty<DockableWindow>("ActiveDockedWindow");
      RegisterProperty("DockedWindows", new ObservableCollection<DockableWindow>());
    }

    public DisplayContainer(string name, int x, int y, int width, int height, int rows, int columns)
    {
      RegisterProperty("Name", name);
      RegisterProperty("X", x);
      RegisterProperty("Y", y);
      RegisterProperty("Width", width);
      RegisterProperty("Height", height);
      RegisterProperty("Rows", rows);
      RegisterProperty("Columns", columns);
      RegisterProperty<DockableWindow>("ActiveDockedWindow");
      RegisterProperty("DockedWindows", new ObservableCollection<DockableWindow>());
    }

    public object Clone()
    {
      var dc = new DisplayContainer(this.Name, this.X, this.Y, this.Width, this.Height);
      foreach (var d in this.DockedWindows)
      {
        dc.DockedWindows.Add(d.Clone() as DockableWindow);
      }
      return dc;
    }
  }
}
