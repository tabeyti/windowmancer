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

namespace Windowmancer.UI
{
  public partial class SettingsDialog : Form
  {
    public HotKeyInputBox HotKeyInputBox { get; set; }

    public SettingsDialog()
    {
      InitializeComponent();
      Initialize();
    }

    public void Initialize()
    {
      this.HotKeyInputBox = new HotKeyInputBox();
      this.HotKeyInputBox.Dock = DockStyle.Fill;
      this.HotKeyGroupBox.Controls.Add(this.HotKeyInputBox);
    }
  }
}

