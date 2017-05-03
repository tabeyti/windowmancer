using System;
using System.Diagnostics;
using Microsoft.Practices.Unity;

namespace Windowmancer.Models
{
  public class WindowInfo : PropertyNotifyBase, ICloneable
  {
    public string Name
    {
      get => GetProperty<string>();
      set => SetProperty(value);
    }

    public WindowLayoutInfo LayoutInfo
    {
      get => GetProperty<WindowLayoutInfo>();
      set => SetProperty(value);
    }

    public WindowMatchCriteria MatchCriteria
    {
      get => GetProperty<WindowMatchCriteria>();
      set => SetProperty(value);
    }

    public bool BringToFront
    {
      get => GetProperty<bool>();
      set => SetProperty(value);
    }

    private UserData _userData;

    public WindowInfo()
    {
      RegisterProperty<string>("Name");
      RegisterProperty<string>("BringToFront");
      RegisterProperty<WindowLayoutInfo>("LayoutInfo");
      RegisterProperty<WindowMatchCriteria>("MatchCriteria");

      this.Name = "";
      this.LayoutInfo = new WindowLayoutInfo();
      this.MatchCriteria = new WindowMatchCriteria(default(WindowMatchCriteriaType), "");
      this.BringToFront = true;
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
        BringToFront = this.BringToFront,
        LayoutInfo = (WindowLayoutInfo)this.LayoutInfo.Clone(),
        MatchCriteria = (WindowMatchCriteria)this.MatchCriteria.Clone()
      };
    }

    /// <summary>
    /// Updates the current instance with values held within the passed instance,
    /// saving to user data the changes.
    /// </summary>
    public void Update(WindowInfo info)
    {
      this.Name = info.Name;
      this.BringToFront = info.BringToFront;
      this.MatchCriteria = info.MatchCriteria;
      this.LayoutInfo = info.LayoutInfo;
      _userData = _userData ?? App.ServiceResolver.Resolve<UserData>();
      _userData.Save();
    }
  }

  public class WindowLayoutInfo : PropertyNotifyBase, ICloneable
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

    public WindowLayoutInfo()
    {
      RegisterProperty<PositionInfo>("PositionInfo");
      RegisterProperty<SizeInfo>("SizeInfo");
    }

    public WindowLayoutInfo(int x, int y, int width, int height)
    {
      RegisterProperty<PositionInfo>("PositionInfo", new PositionInfo(x, y));
      RegisterProperty<SizeInfo>("SizeInfo", new SizeInfo(width, height));
    }

    public override string ToString()
    {
      return $"pos: {this.PositionInfo} - size: {this.SizeInfo}";
    }

    public object Clone()
    {
      return new WindowLayoutInfo
      {
        PositionInfo = (PositionInfo)this.PositionInfo.Clone(),
        SizeInfo = (SizeInfo)this.SizeInfo.Clone()
      };
    }

    public void Update(WindowLayoutInfo info)
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
      RegisterProperty<int>("X", x);
      RegisterProperty<int>("Y", y);
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
      this.RegisterProperty<int>("Width", width);
      this.RegisterProperty<int>("Height", height);
    }
  }
}
