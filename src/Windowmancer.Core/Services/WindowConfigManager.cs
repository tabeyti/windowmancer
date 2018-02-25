namespace Windowmancer.Core.Services
{
  public class WindowConfigManager
  {
    private ProfileManager ProfileManager { get; set; }

    public WindowConfigManager(
      ProfileManager profileManager)
    {
      this.ProfileManager = profileManager;
    }
  }
}
