using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Services;

namespace Windowmancer.Core.Models
{
  public class WindowInfo : PropertyNotifyBase, ICloneable
  {
    public string Name
    {
      get => GetProperty<string>();
      set => SetProperty(value);
    }

    public MonitorLayoutInfo MonitorLayoutInfo
    {
      get => GetProperty<MonitorLayoutInfo>();
      set => SetProperty(value);
    }

    public ContainerLayoutInfo ContainerLayoutInfo
    {
      get => GetProperty<ContainerLayoutInfo>();
      set => SetProperty(value);
    }

    public WindowMatchCriteria MatchCriteria
    {
      get => GetProperty<WindowMatchCriteria>();
      set => SetProperty(value);
    }

    public bool ApplyOnProcessStart
    {
      get => GetProperty<bool>();
      set => SetProperty(value);
    }

    public WindowStylingInfo StylingInfo
    {
      get => GetProperty<WindowStylingInfo>();
      set => SetProperty(value);
    }

    private UserData _userData;

    public WindowInfo()
    {
      RegisterProperty<string>("Name");
      RegisterProperty<string>("ApplyOnProcessStart");
      RegisterProperty<MonitorLayoutInfo>("MonitorLayoutInfo");
      RegisterProperty<ContainerLayoutInfo>("ContainerLayoutInfo");
      RegisterProperty<WindowMatchCriteria>("MatchCriteria");
      RegisterProperty<WindowStylingInfo>("StylingInfo");

      this.Name = "";
      this.MonitorLayoutInfo = new MonitorLayoutInfo();
      this.MatchCriteria = new WindowMatchCriteria(default(WindowMatchCriteriaType), "");
      this.ApplyOnProcessStart = true;
      this.StylingInfo = new WindowStylingInfo();
    }

    public bool IsMatch(Process p)
    {
      return WindowMatch.IsMatch(this.MatchCriteria, p);
    }

    public object Clone()
    {
      return new WindowInfo
      {
        Name = this.Name,
        ApplyOnProcessStart = this.ApplyOnProcessStart,
        MonitorLayoutInfo = (MonitorLayoutInfo)this.MonitorLayoutInfo.Clone(),
        MatchCriteria = (WindowMatchCriteria)this.MatchCriteria.Clone(),
        StylingInfo = (WindowStylingInfo)this.StylingInfo.Clone()
      };
    }

    /// <summary>
    /// Updates the current instance with values held within the passed instance,
    /// saving to user data the changes.
    /// </summary>
    public void Update(WindowInfo info)
    {
      this.Name = info.Name;
      this.ApplyOnProcessStart = info.ApplyOnProcessStart;
      this.MatchCriteria = info.MatchCriteria;
      this.MonitorLayoutInfo = info.MonitorLayoutInfo;
      this.StylingInfo = info.StylingInfo;
      _userData = _userData ?? Helper.ServiceResolver.Resolve<UserData>();
      _userData.Save();
    }

    public static WindowInfo FromProcess(Process process)
    {
      var procRec = Win32.GetProcessWindowRec(process);
      return new WindowInfo
      {
        Name = process.MainWindowTitle,
        MonitorLayoutInfo = new MonitorLayoutInfo(
          procRec.Left,
          procRec.Top,
          procRec.Width,
          procRec.Height),
        MatchCriteria = new WindowMatchCriteria { MatchString = process.MainWindowTitle },
        StylingInfo = new WindowStylingInfo
        {
          WindowOpacityPercentage = MonitorWindowManager.GetWindowOpacityPercentage(process)
        }
      };
    }
  }

  public class WindowStylingInfo : PropertyNotifyBase, ICloneable
  {
    public uint WindowOpacityPercentage
    {
      get => GetProperty<uint>();
      set => SetProperty(value);
    }

    public WindowStylingInfo()
    {
      RegisterProperty<uint>("WindowOpacityPercentage", 100);
    }

    public override string ToString()
    {
      return $"Opacity:{this.WindowOpacityPercentage}";
    }

    public object Clone()
    {
      return new WindowStylingInfo
      {
        WindowOpacityPercentage = (uint)this.WindowOpacityPercentage
      };
    }
  }

  public class MonitorLayoutInfo : PropertyNotifyBase, ICloneable
  {
    public PositionInfo PositionInfo
    {
      get => GetProperty<PositionInfo>();
      set => SetProperty(value);
    }

    public SizeInfo SizeInfo
    {
      get => GetProperty<SizeInfo>();
      set => SetProperty(value);
    }

    public MonitorLayoutInfo()
    {
      RegisterProperty<PositionInfo>("PositionInfo");
      RegisterProperty<SizeInfo>("SizeInfo");
    }

    public MonitorLayoutInfo(int x, int y, int width, int height)
    {
      RegisterProperty("PositionInfo", new PositionInfo(x, y));
      RegisterProperty("SizeInfo", new SizeInfo(width, height));
    }

    public override string ToString()
    {
      return $"pos: {this.PositionInfo} - size: {this.SizeInfo}";
    }

    public object Clone()
    {
      return new MonitorLayoutInfo
      {
        PositionInfo = (PositionInfo)this.PositionInfo.Clone(),
        SizeInfo = (SizeInfo)this.SizeInfo.Clone()
      };
    }

    public void Update(MonitorLayoutInfo info)
    {
      this.PositionInfo = info.PositionInfo;
      this.SizeInfo = info.SizeInfo;
    }
  }
  
  public class PositionInfo : PropertyNotifyBase, ICloneable
  {
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

    public override string ToString()
    {
      return $"({this.X}, {this.Y})";
    }

    public object Clone()
    {
      return new PositionInfo { X = this.X, Y = this.Y };
    }

    public PositionInfo()
    {
      RegisterProperty<int>("X");
      RegisterProperty<int>("Y");
    }

    public PositionInfo(int x, int y)
    {
      RegisterProperty("X", x);
      RegisterProperty("Y", y);
    }
  }

  public class SizeInfo : PropertyNotifyBase, ICloneable
  {
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

    public override string ToString()
    {
      return $"{this.Width}x{this.Height}";
    }

    public object Clone()
    {
      return new SizeInfo { Width = this.Width, Height = this.Height };
    }

    public SizeInfo()
    {
      this.RegisterProperty<int>("Width");
      this.RegisterProperty<int>("Height");
    }

    public SizeInfo(int width, int height)
    {
      this.RegisterProperty("Width", width);
      this.RegisterProperty("Height", height);
    }
  }

  public class ContainerLayoutInfo : PropertyNotifyBase, ICloneable
  {
    public uint Row
    {
      get => GetProperty<uint>();
      set => SetProperty(value);
    }

    public uint Column
    {
      get => GetProperty<uint>();
      set => SetProperty(value);
    }

    public string Container
    {
      get => GetProperty<string>();
      set => SetProperty(value);
    }

    public ContainerLayoutInfo()
    {
      RegisterProperty<uint>("Row");
      RegisterProperty<uint>("Column");
      RegisterProperty<string>("Container");
    }

    public ContainerLayoutInfo(uint row, uint column, string container)
    {
      RegisterProperty("Row", row);
      RegisterProperty("Column", column);
      RegisterProperty("Container", container);
    }

    public override string ToString()
    {
      return $"Row: {Row} - Column: {Column}";
    }

    public object Clone()
    {
      return new ContainerLayoutInfo(this.Row, this.Column, this.Container);
    }
  }

}
