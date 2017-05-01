//http://stackoverflow.com/questions/2136431/how-do-i-read-custom-keyboard-shortcut-from-user-in-wpf

using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WindowmancerWPF.UI
{
  /// <summary>
  /// Interaction logic for LabelTextBox.xaml
  /// </summary>
  public partial class HotKeyInputBox
  {
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
        typeof(string),
        typeof(LabelTextBox),
        new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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

    private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      e.Handled = true;

      // Fetch the actual shortcut key.
      var key = (e.Key == Key.System ? e.SystemKey : e.Key);

      // Ignore modifier keys.
      if (key == Key.LeftShift || key == Key.RightShift
          || key == Key.LeftCtrl || key == Key.RightCtrl
          || key == Key.LeftAlt || key == Key.RightAlt
          || key == Key.LWin || key == Key.RWin)
      {
        return;
      }

      // Build the shortcut key name.
      var shortcutText = new StringBuilder();
      if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
      {
        shortcutText.Append("Ctrl+");
      }
      if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
      {
        shortcutText.Append("Shift+");
      }
      if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
      {
        shortcutText.Append("Alt+");
      }
      shortcutText.Append(key.ToString());

      // Update the text box.
      this.BaseTextBox.Text = shortcutText.ToString();

    }
  }
}

