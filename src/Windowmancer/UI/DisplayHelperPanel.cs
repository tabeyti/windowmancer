using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Windowmancer.UI
{
  public partial class DisplayHelperPanel : UserControl
  {
    public Rectangle Rectangle => GetRectangle();
    public event EventHandler OnRectangleChanged;
        
    private readonly List<Button> _displaySectionButtons = new List<Button>();
    private Screen _currentScreen;
    private DisplaySection _currentDisplaySection;
    private ScreenHighlight _screenHighlight;

    public DisplayHelperPanel()
    {
      InitializeComponent();
    }

    private void DisplayHelperPanel_Load(object sender, EventArgs e)
    {
    }

    public void Start()
    {
      InitializeMonitorList();
      FillMonitorDisplaySections();
    }

    private void InitializeMonitorList()
    {
      this.DisplaysListBox.DisplayMember = "DeviceName";

      Screen primaryScreen = null;
      foreach (var screen in Screen.AllScreens)
      {
        if (screen.Primary)
        {
          primaryScreen = screen;
        }

        this.DisplaysListBox.Items.Add(screen);
      }
      this.DisplaysListBox.SelectedItem = _currentScreen = primaryScreen;
    }

    private void FillMonitorDisplaySections()
    {
      if (this.groupBox1.Controls.Count > 0)
      {
        this.groupBox1.Controls.RemoveAt(0);
      }
      _displaySectionButtons.Clear();
      var tp = new TableLayoutPanel
      {
        RowCount = (int)this.NumRowsSpinner.Value,
        ColumnCount = (int)this.NumColsSpinner.Value,
        Dock = DockStyle.Fill,
        CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
      };

      var colsComplete = false;
      var num = 0;
      for (int r = 0; r < tp.RowCount; ++r)
      {
        tp.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        for (int c = 0; c < tp.ColumnCount; ++c)
        {
          if (!colsComplete)
          {
            tp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
          }
          var button = new Button
          {
            Text = num++.ToString(),
            Dock = DockStyle.Fill,
            AutoSize = true,
            Tag = new DisplaySection(r, c, tp.RowCount, tp.ColumnCount)
          };
          button.Click += DisplaySectionButton_OnClick;
          tp.Controls.Add(button);
          _displaySectionButtons.Add(button);
        }
        colsComplete = true;
      }
      // Make first section button the default selection.
      _displaySectionButtons.First().PerformClick();
      this.groupBox1.Controls.Add(tp);

      if (null != _currentScreen)
      {
        this.groupBox1.Text = _currentScreen.DeviceName;
      }
    }

    private void RefreshWindowHighlight()
    {
      _screenHighlight?.Dispose();
      _screenHighlight = new ScreenHighlight();
      var rec = GetRectangle();
      _screenHighlight.Highlight(rec);
      OnRectangleChanged(this, new EventArgs());
    }

    private Rectangle GetRectangle()
    {
      var screen = _currentScreen;

      var screenWidth = screen.Bounds.Width;
      var screenHeight = screen.Bounds.Height;

      var row = _currentDisplaySection.RowIndex;
      var col = _currentDisplaySection.ColumnIndex;

      var totalRows = _currentDisplaySection.TotalRows;
      var totalCols = _currentDisplaySection.TotalColumns;

      var x = (screenWidth / totalCols) * col + screen.Bounds.X;
      var y = (screenHeight / totalRows) * row + screen.Bounds.Y;

      var width = (screenWidth / totalCols);
      var height = (screenHeight / totalRows);
      return new Rectangle(x, y, width, height);
    }

    private void DisplaySectionButton_OnClick(object sender, EventArgs e)
    {
      // Reset color on all display section buttons.
      _displaySectionButtons.ForEach(b => b.BackColor = SystemColors.Control);

      // Make the selected button have custom color for visibility.
      var button = ((Button)sender);
      button.BackColor = Color.Gold;
      _currentDisplaySection = (DisplaySection)button.Tag;
      RefreshWindowHighlight();
    }

    private void NumRowsSpinner_ValueChanged(object sender, EventArgs e)
    {
      FillMonitorDisplaySections();
    }

    private void NumColsSpinner_ValueChanged(object sender, EventArgs e)
    {
      FillMonitorDisplaySections();
    }

    private void DisplaysListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (null == _currentDisplaySection)
      {
        return;
      }

      _currentScreen = (Screen)this.DisplaysListBox.SelectedItem;
      this.groupBox1.Text = _currentScreen.DeviceName;
      RefreshWindowHighlight();
    }
  }

  class DisplaySection
  {
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public int TotalRows { get; set; }
    public int TotalColumns { get; set; }

    public DisplaySection()
    {
    }

    public DisplaySection(int rowIndex, int columnIndex, int totalRows, int totalColumns)
    {
      this.RowIndex = rowIndex;
      this.ColumnIndex = columnIndex;
      this.TotalRows = totalRows;
      this.TotalColumns = totalColumns;
    }
  }
}
