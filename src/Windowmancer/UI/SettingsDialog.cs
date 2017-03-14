using System;
using System.Linq;
using System.Windows.Forms;
using Windowmancer.Services;

namespace Windowmancer.UI
{
  public partial class SettingsDialog : Form
  {
    public HotKeyInputBox HotKeyInputBox { get; set; }

    public readonly KeyHookManager _keyHookManager;

    public SettingsDialog(KeyHookManager keyHookManager)
    {
      _keyHookManager = keyHookManager;
      InitializeComponent();
      Initialize();
    }

    public void Initialize()
    {
      this.HotKeyInputBox = new HotKeyInputBox { Dock = DockStyle.Fill };
      this.HotKeyInputBox.SetHotkey(_keyHookManager.KeyComboConfig.KeyCombination.Select(k => k.Key).ToList());
      this.HotKeyGroupBox.Controls.Add(this.HotKeyInputBox);
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
      this.Dispose();
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
      var keys = this.HotKeyInputBox.GetHotKeys();
      _keyHookManager.UpdateKeyComboConfig(new KeyComboConfig(keys));
      this.Dispose();
    }
  }
}

