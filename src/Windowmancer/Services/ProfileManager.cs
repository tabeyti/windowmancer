using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Windowmancer.Configuration;
using Windowmancer.Models;

namespace Windowmancer.Services
{
  public class ProfileManager : IDisposable
  {
    public List<Profile> Profiles { get; set; }
    private readonly ProfileManagerConfig _config;
    private Profile _activeProfile;
    public Profile ActiveProfile
    {
      get
      {
        return _activeProfile;
      }
      set
      {
        _activeProfile = value;
        _windowManager.ActiveProfile = _activeProfile;
      }
    }
    public string ActiveProfileName
    {
      get
      {
        return (this.ActiveProfile == null) ? "None" : this.ActiveProfile.Name;
      }
    }

    private readonly WindowManager _windowManager;

    public ProfileManager(ProfileManagerConfig config, WindowManager windowManager)
    {
      _windowManager = windowManager;
      _config = config;
      Profiles = new List<Profile>();
      Initialize();
    }

    public void Initialize()
    {
      string text;
      try
      {
        text = System.IO.File.ReadAllText(_config.ProfileDatPath);
      }
      catch (Exception e)
      {
        // TODO: Dialog window with blank profile.
        throw e;
      } 
      
      try
      {
        // TODO: Icky. Like a copy constructor. Need better way of saving 
        // this instance and re-instance this class from the saved config.
        dynamic json = JsonConvert.DeserializeObject(text);
        var activeProfileId = json.ActiveProfile.ToString();
        this.Profiles = JsonConvert.DeserializeObject<List<Profile>>(json.Profiles.ToString());

        var profile = Profiles.Find(p => p.Id == activeProfileId);
        this.ActiveProfile = profile ?? Profiles.FirstOrDefault();
      } 
      catch (Exception e)
      {
        // TODO: Dialog window with blank profile.
        throw e;
      }
    }

    public void UpdateActiveProfile(int index)
    {
      if (index < 0 || index >= this.Profiles.Count)
      {
        throw new ExceptionBox($"{this} - Index {index} out of range.");
      }
      this.ActiveProfile = this.Profiles[index];
      _windowManager.ActiveProfile = this.ActiveProfile;
    }

    public void UpdateActiveProfile(string id)
    {
      var profile = this.Profiles.Find(p => p.Id == id);
      if (null == profile)
      {
        throw new ExceptionBox($"{this} - Could not find profile from id {id}.");
      }
      this.ActiveProfile = profile;
      _windowManager.ActiveProfile = this.ActiveProfile;
    }

    public bool AddNewProfile(string name)
    {
      if (Profiles.Any(p => p.Name == name))
      {
        return false;
      }
      var profile = new Profile
      {
        Id = Guid.NewGuid().ToString(),
        Name = name,
        Windows = new System.ComponentModel.BindingList<WindowInfo>()
      };
      Profiles.Add(profile);
      this.ActiveProfile = profile;
      WriteToFile();
      return true;
    }

    public bool AddToActiveProfile(WindowInfo info)
    {
      if (null == info)
      {
        return false;
      }

      this.ActiveProfile.Windows.Add(info);      
      return true;
    }

    public void RemoveFromActiveProfile(WindowInfo info)
    {
      if (null == info)
      {
        return;
      }
      this.ActiveProfile.Windows.Remove(info);
    }

    private void WriteToFile()
    {
      try
      {
        //var text = JsonConvert.SerializeObject(this);
        //System.IO.File.WriteAllText(_config.ProfileDatPath, text);
      }
      catch (Exception e)
      {
        // TODO: Dialog window.
        throw e;
      }
    }

    public void Dispose()
    {
    }
  }
}
