using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Windowmancer.Core.Models;
using Windowmancer.Core.Services;
using Windowmancer.Tests.Practices;
using Windowmancer.Tests.Tests.Base;
using Xunit;
using Xunit.Abstractions;

namespace Windowmancer.Tests.Tests
{
  /// <summary>
  /// Validates the API of the ProfileManager service.
  /// </summary>
  public class ProfileManagerTests : TestClassBase
  {
    private readonly UserData _userData;

    public ProfileManagerTests(ITestOutputHelper xunitTestOutputHelper) : base(xunitTestOutputHelper)
    {
      _userData = ServiceResolver.Resolve<UserData>();
      _userData.Enable(false);
    }
    
    [Fact]
    [Trait("Priority", "1")]
    public void ProfileManagerTests_ActiveProfile_AddNew()
    {
      var profileManager = ServiceResolver.Resolve<ProfileManager>();
      var windowManager = ServiceResolver.Resolve<MonitorWindowManager>();

      var originalProfile = profileManager.ActiveProfile;
      profileManager.AddNewProfile(TestHelper.CreateNewProfile(new List<WindowConfig>()));

      Assert.NotEqual(originalProfile, profileManager.ActiveProfile);
      Assert.NotEqual(originalProfile.Id, profileManager.ActiveProfile.Id);
      Assert.Equal(profileManager.ActiveProfile, windowManager.ActiveProfile);
    }

    [Fact]
    [Trait("Priority", "2")]
    public void ProfileManagerTests_ActiveProfile_Update()
    {
      var profileManager = ServiceResolver.Resolve<ProfileManager>();
      var windowManager = ServiceResolver.Resolve<MonitorWindowManager>();

      // Add another profile so we have and save the one we added as the original.
      profileManager.AddNewProfile(TestHelper.CreateNewProfile(new List<WindowConfig>()));
      var originalProfile = profileManager.ActiveProfile;

      // Update active profile to the first profile.
      var profile = profileManager.Profiles.First();
      profileManager.UpdateActiveProfile(profile);

      Assert.NotEqual(originalProfile, profileManager.ActiveProfile);
      Assert.NotEqual(originalProfile.Id, profileManager.ActiveProfile.Id);
      Assert.Equal(profileManager.ActiveProfile, windowManager.ActiveProfile);
    }

    [Fact]
    [Trait("Priority", "2")]
    public void ProfileManagerTests_ActiveProfile_Delete()
    {
      var profileManager = ServiceResolver.Resolve<ProfileManager>();
      var windowManager = ServiceResolver.Resolve<MonitorWindowManager>();

      profileManager.AddNewProfile(TestHelper.CreateNewProfile(new List<WindowConfig>()));
      var firstProfile = profileManager.Profiles.First();
      profileManager.DeleteActiveProfile();

      Assert.Equal(firstProfile, profileManager.ActiveProfile);
      Assert.Equal(firstProfile.Id, profileManager.ActiveProfile.Id);
      Assert.Equal(firstProfile, windowManager.ActiveProfile);
    }

    #region Helper Methods

    #endregion Helper Methods
  }
}
