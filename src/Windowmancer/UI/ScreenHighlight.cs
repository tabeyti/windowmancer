using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Windowmancer.UI
{
  public partial class ScreenHighlight : Form
  {
    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cX, int cY, uint uFlags);

    private Rectangle _innerRectangle;
    private Rectangle _outerRectangle;
    private int _borderWidth;
    private readonly Color _fillColor = Color.Yellow;
    private readonly Color _borderColor = Color.Yellow;

    public ScreenHighlight()
    {
      InitializeComponent();
      this.BackColor = _fillColor;
      this.ForeColor = _borderColor;
      base.FormBorderStyle = FormBorderStyle.None;
      base.MaximizeBox = false;
      base.MinimizeBox = false;
      base.ShowInTaskbar = false;
      base.StartPosition = FormStartPosition.Manual;
      base.Text = "";
    }

    public void Highlight(Screen screen)
    {
      Highlight(screen, 5);
    }

    public void Highlight(Screen screen, int borderWidth)
    {
      var x = screen.Bounds.X;
      var y = screen.Bounds.Y;
      var size = screen.Bounds.Size;
      var rectangle = new Rectangle(new Point(x, y), size);

      _borderWidth = borderWidth;
      SetHighlightSizeLocation(rectangle);
      SetWindowPos(this.Handle, new IntPtr(-1), 0, 0, 0, 0, 0x43);
      Show();
    }

    public void Highlight(Rectangle rectangle)
    {
      Highlight(rectangle, 5);
    }

    public void Highlight(Rectangle rectangle, int borderWidth)
    {
      _borderWidth = borderWidth;
      SetHighlightSizeLocation(rectangle);
      SetWindowPos(this.Handle, new IntPtr(-1), 0, 0, 0, 0, 0x43);
      Show();
    }
    
    public void SetHighlightSizeLocation(Rectangle rectangle)
    {
      this.TopMost = true;
      _outerRectangle = new Rectangle(new Point(0, 0), rectangle.Size);
      _innerRectangle = new Rectangle(new Point(_borderWidth, _borderWidth), rectangle.Size - new Size(_borderWidth * 2, _borderWidth * 2));
      var region = new Region(_outerRectangle);
      region.Exclude(_innerRectangle);
      base.Location = rectangle.Location;
      base.Size = _outerRectangle.Size;
      base.Region = region;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      var outerRectangle = new Rectangle(_outerRectangle.Left, _outerRectangle.Top, _outerRectangle.Width, _outerRectangle.Height);
      var innerRectangle = new Rectangle(_innerRectangle.Left, _innerRectangle.Top, _innerRectangle.Width, _innerRectangle.Height);
      e.Graphics.DrawRectangle(new Pen(ForeColor), innerRectangle);
      e.Graphics.DrawRectangle(new Pen(ForeColor), outerRectangle);
    }

    public new void Show()
    {
      SetWindowPos(base.Handle, IntPtr.Zero, 0, 0, 0, 0, 0x53);
    }
  }
}
