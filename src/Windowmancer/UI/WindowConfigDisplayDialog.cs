using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Windowmancer.UI
{
  public partial class WindowConfigDisplayDialog : Form
  {
    private readonly List<Button> _displaySectionButtons = new List<Button>();

    public WindowConfigDisplayDialog()
    {
      InitializeComponent();
    }

    private void WindowConfigDisplayDialog_Load(object sender, EventArgs e)
    {
      FillDisplaySections();
    }

    private void FillDisplaySections()
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
            Text = "",
            Dock = DockStyle.Fill,
            AutoSize = true
          };
          
          tp.Controls.Add(button);
          _displaySectionButtons.Add(button);
        }
        colsComplete = true;
      }
      this.groupBox1.Controls.Add(tp);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      //base.OnPaint(e);
      //Graphics g = e.Graphics;
      //using (Pen selPen = new Pen(Color.Blue))
      //{
      //  g.DrawRectangle(selPen, 10, 10, 50, 50);
      //}
    }

    private void NumRowsSpinner_ValueChanged(object sender, EventArgs e)
    {
      FillDisplaySections();
    }

    private void NumColsSpinner_ValueChanged(object sender, EventArgs e)
    {
      FillDisplaySections();
    }
  }
}
