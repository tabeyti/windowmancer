using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Windowmancer.Configuration;
using Windowmancer.Models;

namespace Windowmancer.Services
{
  public class ProfileManager 
  {
    public List<Profile> Profiles { get; set; }
    private readonly ProfileManagerConfig _config;

    public ProfileManager(ProfileManagerConfig config)
    {
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

    public void Update(Profile profile)
    {      
      var i = this.Profiles.FindIndex(p => p.Id == profile.Id);
      if (i < 0)
      {
        throw new Exception($"ProfileManager.Update - Could not find profile {profile.Id}.");
      }
      Profiles[i] = profile;
      WriteToFile();
    }

    public bool AddProfile(Profile profile)
    {
      if (Profiles.Any(p => p.Name == profile.Name))
      {
        return false;
      }
      Profiles.Add(profile);
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

    public Profile ActiveProfile { get; set; }

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
  }
}
