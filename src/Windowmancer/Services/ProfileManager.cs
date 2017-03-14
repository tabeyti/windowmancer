using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windowmancer.Configuration;
using Windowmancer.Extensions;
using Windowmancer.Models;

namespace Windowmancer.Services
{
  public class ProfileManager : IDisposable
  {
    public BindingList<Profile> Profiles => _userData.Profiles;
    public Profile ActiveProfile
    {
      get { return _activeProfile; }
      set
      {
        _userData.ActiveProfile = value.Id;
        _activeProfile = value;
        _windowManager.ActiveProfile = _activeProfile;
      }
    }
    public string ActiveProfileName => 
      (this.ActiveProfile == null) ? "None" : this.ActiveProfile.Name;

    private Profile _activeProfile;
    private readonly UserData _userData;
    private readonly WindowManager _windowManager;

    public ProfileManager(
      UserData userData, 
      WindowManager windowManager)
    {
      _windowManager = windowManager;
      _userData = userData;
      Initialize();
    }

    public void Initialize()
    {
      var profile = Profiles.Find(p => p.Id == _userData.ActiveProfile);
      this.ActiveProfile = profile ?? Profiles.FirstOrDefault();
      _windowManager.ActiveProfile = this.ActiveProfile;
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
      _userData.Save();
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

    public void Dispose()
    {
    }
  }
}
