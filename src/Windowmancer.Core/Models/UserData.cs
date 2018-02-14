using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Newtonsoft.Json;
using Windowmancer.Core.Configuration;

namespace Windowmancer.Core.Models
{
  public class UserData
  {
    public ObservableCollection<Profile> Profiles { get; set; }
    public string ActiveProfile { get; set; }
    public HotKeyConfigModel HotKeyConfig { get; set; }

    private UserConfig _config;
    private bool _enabled;

    public UserData(UserConfig config)
    {
      _enabled = true;
      this.Profiles = new ObservableCollection<Profile>
      {
        new Profile
        {
          Id = Guid.NewGuid().ToString(),
          Name = "My Profile",
          Windows = new ObservableCollection<WindowConfig>(),
          Containers = new ObservableCollection<HostContainerConfig>()
        }
      };

      this.HotKeyConfig = new HotKeyConfigModel(new[] { ModifierKeys.Control, ModifierKeys.Shift }.ToList(), Key.W);
      _config = config;
    }

    public void SetUserConfig(UserConfig config)
    {
      _config = config;
    }

    public void Enable(bool enable)
    {
      _enabled = enable;
    }

    public void Save()
    {
      if (!_enabled) return;

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
