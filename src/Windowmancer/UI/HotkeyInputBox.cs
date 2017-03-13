// HotkeyInputBox 0.1 (c) 2011 Richard Z.H. Wang
// MIT licensed.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windowmancer.Models;

namespace Windowmancer.UI
{
  public class HotKeyInputBox : TextBox
  {
    public HotKeyInputBox() { }

    #region Properties to hide from the designer
    [Browsable(false)]
    public new string[] Lines { get { return new string[] { Text }; } private set { base.Lines = value; } }
    [Browsable(false)]
    public override bool Multiline { get { return false; } }
    [Browsable(false)]
    public new char PasswordChar { get; set; }
    [Browsable(false)]
    public new ScrollBars ScrollBars { get; set; }
    [Browsable(false)]
    public override bool ShortcutsEnabled { get { return false; } }
    [Browsable(false)]
    public override string Text { get { return base.Text; } set { base.Text = value; } }
    [Browsable(false)]
    public new bool WordWrap { get; set; }
    #endregion

    #region Focus detection - use this to stop hotkeys being triggered in your code
    private static Control FindFocusedControl(Control control)
    {
      var container = control as ContainerControl;
      while (container != null)
      {
        control = container.ActiveControl;
        container = control as ContainerControl;
      }
      return control;
    }
    public bool IsFocused { get { return FindFocusedControl(Form.ActiveForm) == this; } }
    public static bool TypeIsFocused { get { return FindFocusedControl(Form.ActiveForm) is HotKeyInputBox; } }
    #endregion

    private HotKey _hotkey = new HotKey();

    public Keys KeyCode { get { return _hotkey.KeyCode; } set { _hotkey.KeyCode = value; } }
    public bool Windows { get { return _hotkey.Windows; } set { _hotkey.Windows = value; } }
    public bool Control { get { return _hotkey.Control; } set { _hotkey.Control = value; } }
    public bool Alt { get { return _hotkey.Alt; } set { _hotkey.Alt = value; } }
    public bool Shift { get { return _hotkey.Shift; } set { _hotkey.Shift = value; } }

    public void SetHotkey(List<Keys> keys)
    {
      var hotKey = new HotKey();
      foreach (var k in keys)
      {
        switch (k)
        {
          case Keys.Shift:
            hotKey.Shift = true;
            break;
          case Keys.Alt:
            hotKey.Alt = true;
            break;
          case Keys.Control:
            hotKey.Control = true;
            break;
          default:
            hotKey.KeyCode = k;
            break;
        }
      }
      _hotkey = hotKey;
      RefreshText();
    }

    public HotKey GetHotkey()
    {
      return _hotkey.Clone();
    }
    
    public List<Keys> GetHotKeys()
    {
      var keyInfo = new List<Keys>();
      if (_hotkey.Alt)
      {
        keyInfo.Add(Keys.Alt);
      }
      if (_hotkey.Shift)
      {
        keyInfo.Add(Keys.Shift);
      }
      if (_hotkey.Control)
      {
        keyInfo.Add(Keys.Control);
      }
      keyInfo.Add(_hotkey.KeyCode);
      return keyInfo;
    }

    public void Reset()
    {
      KeyCode = Keys.None;
      Windows = false;
      Control = false;
      Alt = false;
      Shift = false;
    }

    private void RefreshText()
    {
      Text = _hotkey.ToString(true);
      Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      RefreshText();
      base.OnPaint(e);
    }

    public event EventHandler HotkeyChanged;
    protected virtual void OnHotkeyChanged(EventArgs e)
    {
      if (HotkeyChanged != null)
        HotkeyChanged(this, e);
    }

    const int WM_KEYDOWN = 0x100;
    const int WM_KEYUP = 0x101;
    const int WM_CHAR = 0x102;
    const int WM_SYSCHAR = 0x106;
    const int WM_SYSKEYDOWN = 0x104;
    const int WM_SYSKEYUP = 0x105;
    const int WM_IME_CHAR = 0x286;

    private int _keysPressed = 0;

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      return false;
    }

    protected override bool ProcessKeyMessage(ref Message m)
    {
      if (m.Msg == WM_KEYUP || m.Msg == WM_SYSKEYUP)
      {
        _keysPressed--;

        if (_keysPressed == 0)
          OnHotkeyChanged(new EventArgs());
      }

      if (m.Msg != WM_CHAR && m.Msg != WM_SYSCHAR && m.Msg != WM_IME_CHAR)
      {
        KeyEventArgs e = new KeyEventArgs(((Keys)((int)((long)m.WParam))) | ModifierKeys);

        if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
          this.Reset();
        else
        {
          // Print Screen doesn't seem to be part of WM_KEYDOWN/WM_SYSKEYDOWN...
          if (m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN || e.KeyCode == Keys.PrintScreen)
          {
            // Start over if we had no keys pressed, or have a selection (since it's always select all)
            if (_keysPressed < 1 || SelectionLength > 0)
              this.Reset();

            //if (e.KeyCode )
            //    this.Windows = true;
            
            this.Control = e.Control;
            this.Shift = e.Shift;
            this.Alt = e.Alt;

            switch (e.KeyCode)
            {
              case Keys.ShiftKey:
              case Keys.ControlKey:
              case Keys.Alt:
                break;
              default:
                this.KeyCode = e.KeyCode;
                break;
            }
            _keysPressed++;
          }
        }

        // Pretty readable output
        RefreshText();

        // Select the end of our textbox
        Select(TextLength, 0);
      }
      return true;
    }

    public class HotKey 
    {
      public Keys KeyCode { get; set; }
      public bool Windows { get; set; }
      public bool Alt { get; set; }
      public bool Control { get; set; }
      public bool Shift { get; set; }

      public HotKey()
      {
      }

      public HotKey(
        Keys keyCode, 
        bool windows = false, 
        bool alt = false, 
        bool control = false,
        bool shift = false)
      {
        this.KeyCode = keyCode;
        this.Windows = windows;
        this.Alt = alt;
        this.Control = control;
        this.Shift = shift;
      }

      public HotKey Clone()
      {
        return new HotKey
        {
          KeyCode = this.KeyCode,
          Windows = this.Windows,
          Alt = this.Alt,
          Control = this.Control,
          Shift = this.Shift
        };
      }

      public string ToString(bool flag)
      {
        var str = "";
        if (this.Alt)
        {
          str += "Alt + ";
        }
        if (this.Control)
        {
          str += "Ctrl + ";
        }
        if (this.Shift)
        {
          str += "Shift + ";
        }
        if ((int)this.KeyCode > 0)
        {
          str += this.KeyCode.ToString();
        }
        return str;
      }
    }
  }
}
