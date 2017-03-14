using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windowmancer.Configuration;
using Windowmancer.Services;

namespace Windowmancer.Models
{
  public class UserData
  {
    public BindingList<Profile> Profiles { get; set; }
    public string ActiveProfile { get; set; }    
    public KeyComboConfig KeyComboConfig { get; set; }

    private readonly UserConfig _config;

    public UserData(UserConfig config)
    {
      _config = config;
    }

    public void Save()
    {
      try
      {
        //var text = JsonConvert.SerializeObject(this);
        //System.IO.File.WriteAllText(_config.UserDatPath, text);
      }
      catch (Exception e)
      {
        // TODO: Dialog window.
        throw e;
      }
    }
  }
}
