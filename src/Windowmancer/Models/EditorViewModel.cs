using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Windowmancer.Services;

namespace Windowmancer.Models
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
      _profileManager = App.ServiceResolver.Resolve<ProfileManager>();
      RegisterProperty<string>("WindowTitle");
      Update();
    }

    public void Update()
    {
      this.WindowTitle = $"Windowmancer - {_profileManager.ActiveProfile.Name}";
    }
  }
}
