using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windowmancer.Configuration;
using Windowmancer.Extensions;
using Windowmancer.Models;

namespace Windowmancer.Services
{
  public class ProfileManager 
  {
    public List<Profile> Profiles { get; set; }

    //public List<Profile> Profiles { get; set; }
    public string ActiveProfile { get; set; }

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
        this.ActiveProfile = json.ActiveProfile.ToString();
        this.Profiles = JsonConvert.DeserializeObject<List<Profile>>(json.Profiles.ToString());
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

    public bool Add(Profile profile)
    {
      if (Profiles.Any(p => p.Name == profile.Name))
      {
        return false;
      }
      Profiles.Add(profile);
      WriteToFile();
      return true;
    }

    public Profile GetActiveProfile()
    {
      if (null == this.ActiveProfile)
      {
        return null;
      }
      return Profiles.Find(p => p.Id == this.ActiveProfile);
    }

    private void WriteToFile()
    {
      try
      {
        var text = JsonConvert.SerializeObject(this);
        System.IO.File.WriteAllText(_config.ProfileDatPath, text);
      }
      catch (Exception e)
      {
        // TODO: Dialog window.
        throw e;
      }
    }
  }
}
