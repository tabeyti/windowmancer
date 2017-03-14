using System;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using Windowmancer.Configuration;
using Windowmancer.Services;

namespace Windowmancer.Models
{
  public class UserData
  {
    public BindingList<Profile> Profiles { get; set; }
    public string ActiveProfile { get; set; }
    public KeyComboConfig KeyComboConfig { get; set; }
    private UserConfig _config;

    public UserData(UserConfig config)
    {
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
