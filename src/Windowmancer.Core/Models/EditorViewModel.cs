using Microsoft.Practices.Unity;
using Windowmancer.Core.Practices;
using Windowmancer.Core.Services;

namespace Windowmancer.Core.Models
{
  public class EditorViewModel : PropertyNotifyBase
  {
    public string WindowTitle
    {
      get => GetProperty<string>();
      set => SetProperty(value);
    }

    private readonly ProfileManager _profileManager;

    public EditorViewModel()
    {
      _profileManager = Helper.ServiceResolver.Resolve<ProfileManager>();
      RegisterProperty<string>(nameof(this.WindowTitle));
      Update();
    }

    public void Update()
    {
      this.WindowTitle = $"Windowmancer - {_profileManager.ActiveProfile.Name}";
    }
  }
}
