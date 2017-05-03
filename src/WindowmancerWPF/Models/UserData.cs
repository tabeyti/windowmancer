﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Newtonsoft.Json;
using WindowmancerWPF.Configuration;
using WindowmancerWPF.Services;

namespace WindowmancerWPF.Models
{
  public class UserData
  {
    public ObservableCollection<Profile> Profiles { get; set; }
    public string ActiveProfile { get; set; }
    public HotKeyConfig HotKeyConfig { get; set; }
    private UserConfig _config;

    public UserData(UserConfig config)
    {
      this.Profiles = new ObservableCollection<Profile>
      {
        new Profile
        {
          Id = Guid.NewGuid().ToString(),
          Name = "My Profile",
          Windows = new ObservableCollection<WindowInfo>()
        }
      };

      this.HotKeyConfig = new HotKeyConfig(new[] { ModifierKeys.Control, ModifierKeys.Shift }.ToList(), Key.W);
      _config = config;
    }

    public void SetUserConfig(UserConfig config)
    {
      _config = config;
    }

    public void Save()
    {
      try
      {
        var text = JsonConvert.SerializeObject(this, Formatting.Indented);
        System.IO.File.WriteAllText(_config.UserDataPath, text);
      }
      catch (Exception e)
      {
        throw new ExceptionBox(e);
      }
    }
  }
}
