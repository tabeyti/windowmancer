using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Windowmancer.Core.Models;

namespace Windowmancer.UI
{
  /// <summary>
  /// Configuration dialog for the window host container.
  /// </summary>
  public partial class ContainerConfig : UserControl
  {
    public Action OnClose { get; set; }

    public Action<WindowContainer> OnSave { get; set; }

    public WindowContainer Container { get; set; }

    public ContainerConfig(Action<WindowContainer> onSave)
    {
      this.OnSave = onSave;
      PreInitialize();
      InitializeComponent();
    }

    public ContainerConfig(WindowContainer container, Action<WindowContainer> onSave)
    {
      this.OnSave = onSave;
      this.Container = (WindowContainer)container.Clone();
      PreInitialize();
      InitializeComponent();
      Initialize();
    }

    private void PreInitialize()
    {
      if (null == this.Container)
      {
        this.Container = new WindowContainer();
      }
    }
    private void Initialize()
    {
      this.ContainerNameTextBox.Text = this.Container.Name;
    }

    private void Close()
    {
      var window = Window.GetWindow(this);
      if (window == null)
      {
        throw new Exception($"{this} - Could locate active window to unbind the KeyDown listener.");
      }
      window.KeyDown -= ProfileConfig_HandleKeyPress;
      OnClose?.Invoke();
    }

    private void ContainerConfig_OnLoaded(object sender, RoutedEventArgs e)
    {
      var window = Window.GetWindow(this);
      if (window == null)
      {
        throw new Exception($"{this} - Could locate active window to bind the KeyDown listener.");
      }
      window.KeyDown += ProfileConfig_HandleKeyPress;
    }
    
    private void ProfileConfig_HandleKeyPress(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key != Key.Escape)
      {
        return;
      }
      Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void OkayButton_Click(object sender, RoutedEventArgs e)
    {
      ForceDataValidation();
      OnSave?.Invoke(this.Container);
      Close();
    }

    /// <summary>
    /// Forces an update to the data binding on a text box which has focus.
    /// </summary>
    private static void ForceDataValidation()
    {
      var textBox = Keyboard.FocusedElement as TextBox;
      var be = textBox?.GetBindingExpression(TextBox.TextProperty);
      if (be != null && !textBox.IsReadOnly && textBox.IsEnabled)
      {
        be.UpdateSource();
      }
    }
  }
}
