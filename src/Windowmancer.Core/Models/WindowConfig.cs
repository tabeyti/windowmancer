using System;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Services;

namespace Windowmancer.Core.Models
{
  public class WindowConfig : PropertyNotifyBase, ICloneable
  {
    public string Name
    {
      get => GetProperty<string>();
      set => SetProperty(value);
    }

    public WindowConfigLayoutType LayoutType
    {
      get => GetProperty<WindowConfigLayoutType>();
      set => SetProperty(value);
    }

    public MonitorLayoutInfo MonitorLayoutInfo
    {
      get => GetProperty<MonitorLayoutInfo>();
      set => SetProperty(value);
    }

    public HostContainerLayoutInfo HostContainerLayoutInfo
    {
      get => GetProperty<HostContainerLayoutInfo>();
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

    public WindowConfig()
    {
      RegisterProperty(nameof(this.Name), "");
      RegisterProperty(nameof(this.ApplyOnProcessStart), true);
      RegisterProperty(nameof(this.LayoutType), WindowConfigLayoutType.Monitor);
      RegisterProperty(nameof(this.MonitorLayoutInfo), new MonitorLayoutInfo());
      RegisterProperty(nameof(this.HostContainerLayoutInfo), new HostContainerLayoutInfo());
      RegisterProperty(nameof(this.MatchCriteria), new WindowMatchCriteria(default(WindowMatchCriteriaType), ""));
      RegisterProperty(nameof(this.StylingInfo), new WindowStylingInfo());
    }

    public WindowConfig(WindowConfigLayoutType layoutType)
    {
      RegisterProperty(nameof(this.Name), "");
      RegisterProperty(nameof(this.ApplyOnProcessStart), true);
      RegisterProperty(nameof(this.LayoutType), layoutType);

      if (layoutType == WindowConfigLayoutType.Monitor)
      {
        RegisterProperty(nameof(this.MonitorLayoutInfo), new MonitorLayoutInfo());
        RegisterProperty<HostContainerLayoutInfo>(nameof(this.HostContainerLayoutInfo));
      }
      else
      {
        RegisterProperty<MonitorLayoutInfo>(nameof(this.MonitorLayoutInfo));
        RegisterProperty(nameof(this.HostContainerLayoutInfo), new HostContainerLayoutInfo());
      }
      
      RegisterProperty(nameof(this.MatchCriteria), new WindowMatchCriteria(default(WindowMatchCriteriaType), ""));
      RegisterProperty(nameof(this.StylingInfo), new WindowStylingInfo());
    }

    public bool IsMatch(Process p)
    {
      return WindowMatch.IsMatch(this.MatchCriteria, p);
    }

    public object Clone()
    {
      return new WindowConfig
      {
        Name = this.Name,
        LayoutType = this.LayoutType,
        ApplyOnProcessStart = this.ApplyOnProcessStart,
        MonitorLayoutInfo = (MonitorLayoutInfo)this.MonitorLayoutInfo?.Clone(),
        HostContainerLayoutInfo= (HostContainerLayoutInfo)this.HostContainerLayoutInfo?.Clone(),
        MatchCriteria = (WindowMatchCriteria)this.MatchCriteria.Clone(),
        StylingInfo = (WindowStylingInfo)this.StylingInfo.Clone()
      };
    }

    /// <summary>
    /// Updates the current instance with values held within the passed instance,
    /// saving to user data the changes.
    /// </summary>
    public void Update(WindowConfig info)
    {
      this.Name = info.Name;
      this.ApplyOnProcessStart = info.ApplyOnProcessStart;
      this.MatchCriteria = info.MatchCriteria;
      this.MonitorLayoutInfo = info.MonitorLayoutInfo;
      this.HostContainerLayoutInfo = info.HostContainerLayoutInfo;
      this.StylingInfo = info.StylingInfo;
      Save();
    }

    /// <summary>
    /// Saves the current values held in the config to user data.
    /// </summary>
    public void Save()
    {
      _userData = _userData ?? Helper.ServiceResolver.Resolve<UserData>();
      _userData.Save();
    }

    public static WindowConfig FromProcess(Process process, WindowConfigLayoutType layoutType)
    {
      var procRec = Win32.GetProcessWindowRec(process);
      var config = new WindowConfig(layoutType)
      {
        Name = process.MainWindowTitle,
        LayoutType = layoutType,
        MatchCriteria = new WindowMatchCriteria { MatchString = process.MainWindowTitle },
        StylingInfo = new WindowStylingInfo
        {
          WindowOpacityPercentage = MonitorWindowManager.GetWindowOpacityPercentage(process)
        }
      };

      // Apply monitor relative size and position if monitor layout given.
      if (layoutType == WindowConfigLayoutType.Monitor)
      {
        config.MonitorLayoutInfo.Update(new MonitorLayoutInfo(procRec.Left,
          procRec.Top,
          procRec.Width,
          procRec.Height));
      }

      return config;
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
      RegisterProperty<uint>(nameof(this.WindowOpacityPercentage), 100);
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
      RegisterProperty(nameof(PositionInfo), new PositionInfo());
      RegisterProperty(nameof(this.SizeInfo), new SizeInfo());
    }

    public MonitorLayoutInfo(int x, int y, int width, int height)
    {
      RegisterProperty(nameof(this.PositionInfo), new PositionInfo(x, y));
      RegisterProperty(nameof(this.SizeInfo), new SizeInfo(width, height));
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
      RegisterProperty<int>(nameof(this.X));
      RegisterProperty<int>(nameof(this.Y));
    }

    public PositionInfo(int x, int y)
    {
      RegisterProperty(nameof(this.X), x);
      RegisterProperty(nameof(this.Y), y);
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
      this.RegisterProperty<int>(nameof(this.Width));
      this.RegisterProperty<int>(nameof(this.Height));
    }

    public SizeInfo(int width, int height)
    {
      this.RegisterProperty(nameof(this.Width), width);
      this.RegisterProperty(nameof(this.Height), height);
    }
  }

  public class HostContainerLayoutInfo : PropertyNotifyBase, ICloneable
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

    public string HostContainerId
    {
      get => GetProperty<string>();
      set => SetProperty(value);
    }

    public HostContainerLayoutInfo()
    {
      RegisterProperty(nameof(this.Row), 0u);
      RegisterProperty(nameof(this.Column), 0u);
      RegisterProperty(nameof(this.HostContainerId), string.Empty);
    }

    public HostContainerLayoutInfo(uint row, uint column, string container)
    {
      RegisterProperty(nameof(this.Row), row);
      RegisterProperty(nameof(this.Column), column);
      RegisterProperty(nameof(this.HostContainerId), container);
    }

    public override string ToString()
    {
      return $"{this.HostContainerId} - {Row},{Column}";
    }

    public object Clone()
    {
      return new HostContainerLayoutInfo(this.Row, this.Column, this.HostContainerId);
    }

    public void Update(uint row, uint column, string hostContainer)
    {
      this.Row = row;
      this.Column = column;
      this.HostContainerId = hostContainer;
    }

    public void Update(HostContainerLayoutInfo layoutInfo)
    {
      this.Row = layoutInfo.Row;
      this.Column = layoutInfo.Column;
      this.HostContainerId = layoutInfo.HostContainerId;
    }
  }

  public enum WindowConfigLayoutType
  {
    Monitor,
    HostContainer
  }
}
