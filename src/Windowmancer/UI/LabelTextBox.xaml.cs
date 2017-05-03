using System.Windows;
using System.Windows.Controls;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for LabelTextBox.xaml
  /// </summary>
  public partial class LabelTextBox
  {
    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label",
        typeof(string),
        typeof(LabelTextBox),
        new FrameworkPropertyMetadata("Unnamed Label"));

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
        typeof(string),
        typeof(LabelTextBox),
        new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public LabelTextBox()
    {
      InitializeComponent();
      Root.DataContext = this;
    }

    public string Label
    {
      get => (string)GetValue(LabelProperty);
      set => SetValue(LabelProperty, value);
    }

    public string Text
    {
      get => (string)GetValue(TextProperty);
      set => SetValue(TextProperty, value);
    }
  }
}

