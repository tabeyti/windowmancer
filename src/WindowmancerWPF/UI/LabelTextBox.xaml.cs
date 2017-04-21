using System.Windows.Controls;

namespace WindowmancerWPF.UI
{
  /// <summary>
  /// Interaction logic for LabelTextBox.xaml
  /// </summary>
  public partial class LabelTextBox : UserControl
  {
    public LabelTextBox()
    {
      InitializeComponent();
    }

    private string _localLabel = "";
    private string _localText = "";

    public string Label
    {
      get { return _localLabel; }
      set
      {
        _localLabel = value;
        BaseLabel.Content = value;
      }
    }

    public string Text
    {
      get { return _localText; }
      set
      {
        _localText = value;
        BaseTextBox.Text = value;
      }
    }
  }
}

