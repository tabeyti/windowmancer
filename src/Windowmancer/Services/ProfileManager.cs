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
    private readonly List<Profile> _profileDict;

    public ProfileManager()
    {
      _profileDict = new List<Profile>();
      Initialize();
    }

    public void Initialize()
    {
      //var datPath = "dat.json";
      //_profileDict.Add(profile);
    }

    public bool Add(Profile profile)
    {
      if (_profileDict.Any(p => p.Name == profile.Name))
      {
        return false;
      }
      _profileDict.Add(profile);
      return true;
    }
  }
}
