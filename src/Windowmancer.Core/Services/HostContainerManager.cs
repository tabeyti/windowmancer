using System;
using Windowmancer.Core.Models;

namespace Windowmancer.Core.Services
{
  public class HostContainerManager : IDisposable
  {
    public Profile ActiveProfile => _profileManager.ActiveProfile;

    private readonly ProfileManager _profileManager;

    #region Constructors

    public HostContainerManager(ProfileManager profileManager)
    {
      _profileManager = profileManager;
    }

    #endregion Constructors

    public void Dispose()
    {
    }
  }
}
