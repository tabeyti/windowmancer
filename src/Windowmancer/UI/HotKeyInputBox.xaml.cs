//http://stackoverflow.com/questions/2136431/how-do-i-read-custom-keyboard-shortcut-from-user-in-wpf

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Windowmancer.Services;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for LabelTextBox.xaml
  /// </summary>
  public partial class HotKeyInputBox : UserControl
  {
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register("TextProperty",
        typeof(string),
        typeof(TextBox),
        new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    private HotKeyConfig _hotKeys = null;

    public HotKeyInputBox()
    {
      InitializeComponent();
      Root.DataContext = this;
    }
    
    public string Text
    {
      get => (string)GetValue(TextProperty);
      set => SetValue(TextProperty, value);
    }

    public HotKeyConfig GetHotKeyConfig()
    {
      return _hotKeys;
    }

    public void SetHotKeyConfig(HotKeyConfig config)
    {
      _hotKeys = config;
      this.BaseTextBox.Text = GetHotKeyString(_hotKeys.ModifierKeys, _hotKeys.PrimaryKey);
    }

    /// <summary>
    /// Builds and returns the HotKey string based on the 
    /// keyboard's modifyer keys and the passed primary key.
    /// </summary>
    public static string GetHotKeyString(List<ModifierKeys> modifierKeys, Key primaryKey)
    {
      // Build the shortcut key name.
      var shortcutText = new StringBuilder();

      modifierKeys.ForEach(m =>
      {
        switch (m)
        {
          case ModifierKeys.Control:
            shortcutText.Append("Ctrl+");
            break;
          case ModifierKeys.Shift:
            shortcutText.Append("Shift+");
            break;
          case ModifierKeys.Alt:
            shortcutText.Append("aLT+");
            break;
        }
      });
      shortcutText.Append(primaryKey.ToString());
      return shortcutText.ToString();
    }

    private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      e.Handled = true;

      // Fetch the actual shortcut key.
      var key = (e.Key == Key.System ? e.SystemKey : e.Key);

      // If Enter or Escape was pressed, ignore it to allow
      // other controls to capture the key event.
      if (key == Key.Escape || key == Key.Enter)
      {
        e.Handled = false;
        return;
      }

      // Ignore modifier keys.
      if (key == Key.LeftShift || key == Key.RightShift
          || key == Key.LeftCtrl || key == Key.RightCtrl
          || key == Key.LeftAlt || key == Key.RightAlt
          || key == Key.LWin || key == Key.RWin)
      {
        return;
      }

      _hotKeys = new HotKeyConfig();
      if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
      {
        _hotKeys.ModifierKeys.Add(ModifierKeys.Control);
      }
      if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
      {
        _hotKeys.ModifierKeys.Add(ModifierKeys.Shift);
      }
      if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
      {
        _hotKeys.ModifierKeys.Add(ModifierKeys.Alt);
      }
      _hotKeys.PrimaryKey = e.Key;

      // Update the text box.
      this.BaseTextBox.Text = GetHotKeyString(_hotKeys.ModifierKeys, e.Key);
    }
  }
}

