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
        return false;
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
    /// Adds a new window container to the active profile.
    /// </summary>
    /// <param name="container">The container to add.</param>
    /// <returns>True of container was added. False if not.</returns>
    public bool AddToActiveProfile(WindowContainer container)
    {
      if (null == container)
      {
        return false;
      }

      if (this.ActiveProfile.Containers.Any(c => 
        c.Name.ToLower().Trim() == container.Name.ToLower().Trim()))
      {
        return false;
      }
      this.ActiveProfile.Containers.Add(container);
      return true;
    }

    /// <summary>
    /// Adds a window info instance to the active profile.
    /// </summary>
    /// <param name="info"></param>
    /// <returns>True of window info was added. False if not.</returns>
    public bool AddToActiveProfile(WindowInfo info)
    {
      if (null == info)
      {
        return false;
      }
      this.ActiveProfile.Windows.Add(info);
      _userData.Save();
      return true;
    }

    /// <summary>
    /// Removes the passed window info instance from the active profile's
    /// list of window info objects.
    /// </summary>
    /// <param name="info"></param>
    public void RemoveFromActiveProfile(WindowInfo info)
    {
      if (null == info)
      {
        return;
      }
      this.ActiveProfile.Windows.Remove(info);
      _userData.Save();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="profile"></param>
    private void DeselectActiveProfile()
    {
      foreach (var p in this.Profiles) { p.IsActive = false; }
    }

    public void Dispose()
    {
    }
  }
}
