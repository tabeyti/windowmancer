using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windowmancer.Core.Extensions;
using Windowmancer.Core.Models;

namespace Windowmancer.Core.Services
{
  public class ProfileManager : PropertyNotifyBase, IDisposable
  {
    public event EventHandler OnActiveProfileUpdate;
    public ObservableCollection<Profile> Profiles => _userData.Profiles;
    public Profile ActiveProfile
    {
      get => GetProperty<Profile>();
      set
      {
        if (value == null)
        {
          return;
        }
        value.IsActive = true;
        _userData.ActiveProfile = value.Id;
        SetProperty(value);
        _monitorWindowManager.ActiveProfile = value;
      }
    }

    private readonly UserData _userData;
    private readonly MonitorWindowManager _monitorWindowManager;

    public ProfileManager(
      UserData userData, 
      MonitorWindowManager monitorWindowManager)
    {
      RegisterProperty<Profile>("ActiveProfile");
      _monitorWindowManager = monitorWindowManager;
      _userData = userData;
      Initialize();
    }

    private void Initialize()
    {
      var profile = Profiles.Find(p => p.Id == _userData.ActiveProfile);
      this.ActiveProfile = profile ?? Profiles.FirstOrDefault();
      _monitorWindowManager.ActiveProfile = this.ActiveProfile;
    }
    
    public void UpdateActiveProfile(Profile newProfile)
    {
      var id = newProfile.Id;
      var profile = this.Profiles.Find(p => p.Id == id);
      DeselectActiveProfile();
      this.ActiveProfile = profile ?? throw new ExceptionBox($"{this} - Could not find profile from id {id}.");
      _monitorWindowManager.ActiveProfile = this.ActiveProfile;
      _userData.Save();
      OnActiveProfileUpdate?.Invoke(this, new EventArgs());
    }

    /// <summary>
    /// Adds a new profile to the list of user profiles.
    /// </summary>
    /// <param name="profile"></param>
    /// <returns></returns>
    public bool AddNewProfile(Profile profile)
    {
      if (Profiles.Any(p => p.Name == profile.Name))
      {
        throw new Exception($"Profile '{profile.Name}' already exists dingus!");
      }
      DeselectActiveProfile();
      this.Profiles.Add(profile);
      this.ActiveProfile = profile;
      _userData.Save();
      OnActiveProfileUpdate?.Invoke(this, new EventArgs());
      return true;
    }

    /// <summary>
    /// Deletes the current active profile, returning the index of the new
    /// active profile.
    /// </summary>
    /// <returns></returns>
    public int DeleteActiveProfile()
    {
      if (null == this.ActiveProfile)
      {
        return -1;
      }

      // Get the index of the new active profile since we are deleting this one.
      var index = this.Profiles.IndexOf(this.ActiveProfile);
      if (index == 0 && this.Profiles.Count == 1)
      {
        index = -1;
      }
      else if (index == this.Profiles.Count - 1)
      {
        index--;
      }
      
      this.Profiles.Remove(this.ActiveProfile);
      this.ActiveProfile = this.Profiles[index];
      this.ActiveProfile.IsActive = true;
      OnActiveProfileUpdate?.Invoke(this, new EventArgs());
      _userData.Save();
      return index;
    }

    /// <summary>
    /// Adds a new host container to the active profile.
    /// </summary>
    /// <param name="container">The container to add.</param>
    /// <returns>True of container was added. False if not.</returns>
    public bool AddToActiveProfile(HostContainerConfig container)
    {
      if (null == container)
      {
        throw new Exception($"You can't pass a null container config ya idgit!");
      }

      if (this.ActiveProfile.HostContainers.Any(c => 
        c.Name.ToLower().Trim() == container.Name.ToLower().Trim()))
      {
        throw new Exception($"{nameof(HostContainerConfig)} of name {container.Name} already exists, ya dingle berry!.");
      }
      this.ActiveProfile.HostContainers.Add(container);
      _userData.Save();
      return true;
    }

    /// <summary>
    /// Adds a window config instance to the active profile.
    /// </summary>
    /// <param name="config"></param>
    /// <returns>True of window config was added. False if not.</returns>
    public bool AddToActiveProfile(WindowConfig config)
    {
      if (null == config)
      {
        throw new Exception("Why are you passing a no window config? Whatsamattawit YOU!");
      }
      if (this.ActiveProfile.Windows.Any(c => c.Name.ToLower().Trim() == config.Name))
      {
        throw new Exception($"{nameof(WindowConfig)} of name {config.Name} already exists. You born in a barn or something? That doesn't make sense. I'm sorry.");
      }
      this.ActiveProfile.Windows.Add(config);
      _userData.Save();
      return true;
    }

    /// <summary>
    /// Removes the passed window config instance from the active profile's
    /// list of window config objects.
    /// </summary>
    /// <param name="config"></param>
    public void RemoveFromActiveProfile(WindowConfig config)
    {
      if (null == config)
      {
        return;
      }
      this.ActiveProfile.Windows.Remove(config);
      _userData.Save();
    }

    /// <summary>
    /// Removes the passed container config instance from the active profile's
    /// list of container config objects.
    /// </summary>
    /// <param name="config"></param>
    public void RemoveFromActiveProfile(HostContainerConfig config)
    {
      if (null == config)
      {
        return;
      }
      this.ActiveProfile.HostContainers.Remove(config);
      _userData.Save();
    }
    public bool IsInActiveProfile(WindowConfig config)
    {
      if (null == config)
      {
        throw new Exception("Why you pass null config? This thing make no sense.");
      }

      return this.ActiveProfile.Windows.Any(c =>
        c.Name.ToLower().Trim() == config.Name.ToLower().Trim());
    }

    public string DefaultWindowConfigName(string prefix = "My Window Config")
    {
      var configs = this.ActiveProfile.Windows;
      var i = 0;
      var name = $"{prefix} 0";

      while (configs.Any(c => c.Name == name))
      {
        name = $"{prefix} {i++}";
      }

      return name;
    }

    private void DeselectActiveProfile()
    {
      foreach (var p in this.Profiles) { p.IsActive = false; }
    }

    public void Dispose()
    {
    }
  }
}
