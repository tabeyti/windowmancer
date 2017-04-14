using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windowmancer.Models;
using Windowmancer.Services;

namespace Windowmancer.UI
{
  public partial class ProfileConfigDialog : Form
  {
    private readonly ProfileManager _profileManager;
    private readonly Profile _profile;

    public ProfileConfigDialog(ProfileManager profileManager, Profile profile = null)
    {
      _profile = profile;
      _profileManager = profileManager;
      InitializeComponent();
      Initialize();
    }

    private void Initialize()
    {
      if (null == _profile)
      {
        return;
      }

      this.ProfileNameTextBox.Text = _profile.Name;
    }

    private void SaveProfile()
    {
      // If this is a new profile add it.
      if (null == _profile)
      {
        var result = _profileManager.AddNewProfile(this.ProfileNameTextBox.Text);
        return;
      }
      
      // Update the existing profile.
      _profileManager.UpdateActiveProfileName(this.ProfileNameTextBox.Text);
    }

    #region Events

    private void CancelButton_Click(object sender, EventArgs e)
    {
      this.Dispose();
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
      SaveProfile();
      this.Dispose();
    }

    #endregion Events
  }
}
