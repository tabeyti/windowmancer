using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windowmancer.Models;

namespace Windowmancer.UI
{
  public partial class SettingsDialog : Form
  {
    public HotKeyInputBox HotKeyInputBox { get; set; }

    public readonly UserData _userData;

    public SettingsDialog(UserData userData)
    {
      _userData = userData;
      InitializeComponent();
      Initialize();
    }

    public void Initialize()
    {
      this.HotKeyInputBox = new HotKeyInputBox();
      this.HotKeyInputBox.Dock = DockStyle.Fill;
      this.HotKeyInputBox.SetHotkey(_userData.GlobalHotKeyCombo.KeyCombination.Select(k => k.Key).ToList());
      this.HotKeyGroupBox.Controls.Add(this.HotKeyInputBox);
    }
  }
}

