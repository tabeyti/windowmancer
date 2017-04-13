﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Windowmancer.Models;

namespace Windowmancer.UI
{
  public partial class WindowConfigDisplayDialog : Form
  {
    public LocationInfo LocationInfo { get; private set; }
    public SizeInfo SizeInfo { get; private set; }

    private readonly List<Button> _DisplayAreaButtons = new List<Button>();
    private Screen _currentScreen;
    private DisplayArea _currentDisplayArea;
    private ScreenHighlight _screenHighlight;

    public WindowConfigDisplayDialog()
    {
      InitializeComponent();
    }

    private void WindowConfigDisplayDialog_Load(object sender, EventArgs e)
    {
      InitializeMonitorList();
      FillMonitorDisplayAreas();
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

    private void FillMonitorDisplayAreas()
    {
      if (this.groupBox1.Controls.Count > 0)
      {
        this.groupBox1.Controls.RemoveAt(0);
      }
      _DisplayAreaButtons.Clear();
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
            Tag = new DisplayArea(r, c, tp.RowCount, tp.ColumnCount)
          };
          button.Click += DisplayAreaButton_OnClick;
          tp.Controls.Add(button);
          _DisplayAreaButtons.Add(button);
        }
        colsComplete = true;
      }
      // Make first section button the default selection.
      _DisplayAreaButtons.First().PerformClick();
      this.groupBox1.Controls.Add(tp);

      if (null != _currentScreen)
      {
        this.groupBox1.Text = _currentScreen.DeviceName;
      }
    }

    private void ApplyWindowInfo(bool withSave = false)
    {
      var rectangle = GetRectangle();
      this.WidthTextBox.Text = rectangle.Width.ToString();
      this.HeightTextBox.Text = rectangle.Height.ToString();
      this.XTextBox.Text = rectangle.X.ToString();
      this.YTextBox.Text = rectangle.Y.ToString();

      if (!withSave)
      {
        return;
      }
      
      this.LocationInfo = new LocationInfo
      {
        PositionInfo = new PositionInfo
        {
          X = rectangle.X,
          Y = rectangle.Y,
        },
        DisplayName = _currentScreen.DeviceName
      };
      this.SizeInfo = new SizeInfo
      {
        Width = rectangle.Width,
        Height = rectangle.Height
      };
    }

    private void RefreshWindowHighlight()
    {
      _screenHighlight?.Dispose();
      _screenHighlight = new ScreenHighlight();
      var rec = GetRectangle();
      _screenHighlight.Highlight(rec);
    }
    
    private Rectangle GetRectangle()
    {
      var screen = _currentScreen;

      var screenWidth = screen.Bounds.Width;
      var screenHeight = screen.Bounds.Height;

      var row = _currentDisplayArea.RowIndex;
      var col = _currentDisplayArea.ColumnIndex;

      var totalRows = _currentDisplayArea.TotalRows;
      var totalCols = _currentDisplayArea.TotalColumns;

      var x = (screenWidth / totalCols) * col + screen.Bounds.X;
      var y = (screenHeight / totalRows) * row + screen.Bounds.Y;

      var width = (screenWidth / totalCols);
      var height = (screenHeight / totalRows);
      return new Rectangle(x, y, width, height);
    }

    private void DisplayAreaButton_OnClick(object sender, EventArgs e)
    {
      // Reset color on all display section buttons.
      _DisplayAreaButtons.ForEach(b => b.BackColor = SystemColors.Control);

      // Make the selected button have custom color for visibility.
      var button = ((Button) sender);
      button.BackColor = Color.Gold;
      _currentDisplayArea = (DisplayArea)button.Tag;
      ApplyWindowInfo();
      RefreshWindowHighlight();
    }

    private void NumRowsSpinner_ValueChanged(object sender, EventArgs e)
    {
      FillMonitorDisplayAreas();
    }

    private void NumColsSpinner_ValueChanged(object sender, EventArgs e)
    {
      FillMonitorDisplayAreas();
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
      ApplyWindowInfo(true);
      DisposeEverything();
    }

    public void DisposeEverything()
    {
      _screenHighlight?.Dispose();
      this.Dispose();
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
      DisposeEverything();
    }

    private void DisplaysListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (null == _currentDisplayArea)
      {
        return;
      }

      _currentScreen = (Screen)this.DisplaysListBox.SelectedItem;
      this.groupBox1.Text = _currentScreen.DeviceName;
      ApplyWindowInfo();
      RefreshWindowHighlight();
    }
  }

  class DisplayArea
  {
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public int TotalRows { get; set; }
    public int TotalColumns { get; set; }

    public DisplayArea()
    {
    }

    public DisplayArea(int rowIndex, int columnIndex, int totalRows, int totalColumns)
    {
      this.RowIndex = rowIndex;
      this.ColumnIndex = columnIndex;
      this.TotalRows = totalRows;
      this.TotalColumns = totalColumns;
    }
  }
}
