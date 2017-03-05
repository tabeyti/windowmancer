using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windowmancer.Models;

namespace Windowmancer.Services
{
  public class ProfileManager 
  {
    private readonly List<Profile> _profileList;

    public ProfileManager()
    {
      _profileList = new List<Profile>();
      Initialize();
    }

    public void Initialize()
    {
      //var datPath = "dat.json";
      //_profileDict.Add(profile);
    }

    public void Update(Profile profile)
    {      
      var i = _profileList.FindIndex(p => p.Id == profile.Id);
      if (i < 0)
      {
        throw new Exception($"ProfileManager.Update - Could not find profile {profile.Id}.");
      }
      _profileList[i] = profile;
      WriteToFile();
    }

    public bool Add(Profile profile)
    {
      if (_profileList.Any(p => p.Name == profile.Name))
      {
        return false;
      }
      _profileList.Add(profile);
      WriteToFile();
      return true;
    }

    private void WriteToFile()
    {

    }
  }
}
