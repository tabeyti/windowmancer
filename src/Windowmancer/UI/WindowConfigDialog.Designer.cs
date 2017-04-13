using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Windowmancer.UI
{
  partial class WindowConfigDialog
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.WindowMatchGroupBox = new System.Windows.Forms.GroupBox();
      this.WindowProcessNameRadioButton = new System.Windows.Forms.RadioButton();
      this.WindowTitleRadioButton = new System.Windows.Forms.RadioButton();
      this.WindowSizeBoxHeight = new System.Windows.Forms.NumericUpDown();
      this.WindowSizeBoxWidth = new System.Windows.Forms.NumericUpDown();
      this.WindowLocationBoxY = new System.Windows.Forms.NumericUpDown();
      this.WindowLocationBoxX = new System.Windows.Forms.NumericUpDown();
      this.MatchStringGroupBox = new System.Windows.Forms.GroupBox();
      this.WindowMatchStringTextBox = new System.Windows.Forms.TextBox();
      this.WindowConfigDialogSaveButton = new System.Windows.Forms.Button();
      this.WindowConfigDialogCancelButton = new System.Windows.Forms.Button();
      this.WindowConfigNameGroupBox = new System.Windows.Forms.GroupBox();
      this.WindowConfigNameTextBox = new System.Windows.Forms.TextBox();
      this.BringToFrontCheckBox = new System.Windows.Forms.CheckBox();
      this.WindowLayoutBox = new System.Windows.Forms.GroupBox();
      this.WindowLayoutLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
      this.DisplayRadioButton = new System.Windows.Forms.RadioButton();
      this.AbsoluteRadioButton = new System.Windows.Forms.RadioButton();
      this.DisplayHelperPanel = new Windowmancer.UI.DisplayHelperPanel();
      this.WindowMatchGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.WindowSizeBoxHeight)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowSizeBoxWidth)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxY)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxX)).BeginInit();
      this.MatchStringGroupBox.SuspendLayout();
      this.WindowConfigNameGroupBox.SuspendLayout();
      this.WindowLayoutBox.SuspendLayout();
      this.WindowLayoutLayoutPanel.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.SuspendLayout();
      // 
      // WindowMatchGroupBox
      // 
      this.WindowMatchGroupBox.Controls.Add(this.WindowProcessNameRadioButton);
      this.WindowMatchGroupBox.Controls.Add(this.WindowTitleRadioButton);
      this.WindowMatchGroupBox.Location = new System.Drawing.Point(285, 448);
      this.WindowMatchGroupBox.Name = "WindowMatchGroupBox";
      this.WindowMatchGroupBox.Size = new System.Drawing.Size(268, 48);
      this.WindowMatchGroupBox.TabIndex = 0;
      this.WindowMatchGroupBox.TabStop = false;
      this.WindowMatchGroupBox.Text = "Match By";
      // 
      // WindowProcessNameRadioButton
      // 
      this.WindowProcessNameRadioButton.AutoSize = true;
      this.WindowProcessNameRadioButton.Location = new System.Drawing.Point(96, 16);
      this.WindowProcessNameRadioButton.Name = "WindowProcessNameRadioButton";
      this.WindowProcessNameRadioButton.Size = new System.Drawing.Size(94, 17);
      this.WindowProcessNameRadioButton.TabIndex = 2;
      this.WindowProcessNameRadioButton.TabStop = true;
      this.WindowProcessNameRadioButton.Text = "Process Name";
      this.WindowProcessNameRadioButton.UseVisualStyleBackColor = true;
      // 
      // WindowTitleRadioButton
      // 
      this.WindowTitleRadioButton.AutoSize = true;
      this.WindowTitleRadioButton.Location = new System.Drawing.Point(3, 16);
      this.WindowTitleRadioButton.Name = "WindowTitleRadioButton";
      this.WindowTitleRadioButton.Size = new System.Drawing.Size(87, 17);
      this.WindowTitleRadioButton.TabIndex = 1;
      this.WindowTitleRadioButton.TabStop = true;
      this.WindowTitleRadioButton.Text = "Window Title";
      this.WindowTitleRadioButton.UseVisualStyleBackColor = true;
      // 
      // WindowSizeBoxHeight
      // 
      this.WindowSizeBoxHeight.Location = new System.Drawing.Point(89, 3);
      this.WindowSizeBoxHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.WindowSizeBoxHeight.Name = "WindowSizeBoxHeight";
      this.WindowSizeBoxHeight.Size = new System.Drawing.Size(79, 20);
      this.WindowSizeBoxHeight.TabIndex = 1;
      // 
      // WindowSizeBoxWidth
      // 
      this.WindowSizeBoxWidth.Location = new System.Drawing.Point(88, 3);
      this.WindowSizeBoxWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.WindowSizeBoxWidth.Name = "WindowSizeBoxWidth";
      this.WindowSizeBoxWidth.Size = new System.Drawing.Size(80, 20);
      this.WindowSizeBoxWidth.TabIndex = 0;
      // 
      // WindowLocationBoxY
      // 
      this.WindowLocationBoxY.Location = new System.Drawing.Point(3, 3);
      this.WindowLocationBoxY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.WindowLocationBoxY.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
      this.WindowLocationBoxY.Name = "WindowLocationBoxY";
      this.WindowLocationBoxY.Size = new System.Drawing.Size(79, 20);
      this.WindowLocationBoxY.TabIndex = 1;
      // 
      // WindowLocationBoxX
      // 
      this.WindowLocationBoxX.Location = new System.Drawing.Point(3, 3);
      this.WindowLocationBoxX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.WindowLocationBoxX.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
      this.WindowLocationBoxX.Name = "WindowLocationBoxX";
      this.WindowLocationBoxX.Size = new System.Drawing.Size(80, 20);
      this.WindowLocationBoxX.TabIndex = 0;
      // 
      // MatchStringGroupBox
      // 
      this.MatchStringGroupBox.Controls.Add(this.WindowMatchStringTextBox);
      this.MatchStringGroupBox.Location = new System.Drawing.Point(8, 448);
      this.MatchStringGroupBox.Name = "MatchStringGroupBox";
      this.MatchStringGroupBox.Size = new System.Drawing.Size(274, 48);
      this.MatchStringGroupBox.TabIndex = 3;
      this.MatchStringGroupBox.TabStop = false;
      this.MatchStringGroupBox.Text = "Regex Match String";
      // 
      // WindowMatchStringTextBox
      // 
      this.WindowMatchStringTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.WindowMatchStringTextBox.Location = new System.Drawing.Point(3, 16);
      this.WindowMatchStringTextBox.Name = "WindowMatchStringTextBox";
      this.WindowMatchStringTextBox.Size = new System.Drawing.Size(268, 20);
      this.WindowMatchStringTextBox.TabIndex = 0;
      // 
      // WindowConfigDialogSaveButton
      // 
      this.WindowConfigDialogSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.WindowConfigDialogSaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.WindowConfigDialogSaveButton.Location = new System.Drawing.Point(474, 531);
      this.WindowConfigDialogSaveButton.Name = "WindowConfigDialogSaveButton";
      this.WindowConfigDialogSaveButton.Size = new System.Drawing.Size(75, 23);
      this.WindowConfigDialogSaveButton.TabIndex = 4;
      this.WindowConfigDialogSaveButton.Text = "Save";
      this.WindowConfigDialogSaveButton.UseVisualStyleBackColor = true;
      this.WindowConfigDialogSaveButton.Click += new System.EventHandler(this.WindowConfigDialogSaveButton_Click);
      // 
      // WindowConfigDialogCancelButton
      // 
      this.WindowConfigDialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.WindowConfigDialogCancelButton.Location = new System.Drawing.Point(393, 531);
      this.WindowConfigDialogCancelButton.Name = "WindowConfigDialogCancelButton";
      this.WindowConfigDialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.WindowConfigDialogCancelButton.TabIndex = 5;
      this.WindowConfigDialogCancelButton.Text = "Cancel";
      this.WindowConfigDialogCancelButton.UseVisualStyleBackColor = true;
      this.WindowConfigDialogCancelButton.Click += new System.EventHandler(this.WindowConfigDialogCancelButton_Click);
      // 
      // WindowConfigNameGroupBox
      // 
      this.WindowConfigNameGroupBox.Controls.Add(this.WindowConfigNameTextBox);
      this.WindowConfigNameGroupBox.Location = new System.Drawing.Point(12, 13);
      this.WindowConfigNameGroupBox.Name = "WindowConfigNameGroupBox";
      this.WindowConfigNameGroupBox.Size = new System.Drawing.Size(541, 54);
      this.WindowConfigNameGroupBox.TabIndex = 6;
      this.WindowConfigNameGroupBox.TabStop = false;
      this.WindowConfigNameGroupBox.Text = "Name";
      // 
      // WindowConfigNameTextBox
      // 
      this.WindowConfigNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.WindowConfigNameTextBox.Location = new System.Drawing.Point(3, 16);
      this.WindowConfigNameTextBox.Name = "WindowConfigNameTextBox";
      this.WindowConfigNameTextBox.Size = new System.Drawing.Size(535, 20);
      this.WindowConfigNameTextBox.TabIndex = 0;
      // 
      // BringToFrontCheckBox
      // 
      this.BringToFrontCheckBox.AutoSize = true;
      this.BringToFrontCheckBox.Checked = true;
      this.BringToFrontCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.BringToFrontCheckBox.Location = new System.Drawing.Point(8, 502);
      this.BringToFrontCheckBox.Name = "BringToFrontCheckBox";
      this.BringToFrontCheckBox.Size = new System.Drawing.Size(86, 17);
      this.BringToFrontCheckBox.TabIndex = 7;
      this.BringToFrontCheckBox.Text = "Bring to front";
      this.BringToFrontCheckBox.UseVisualStyleBackColor = true;
      // 
      // WindowLayoutBox
      // 
      this.WindowLayoutBox.Controls.Add(this.WindowLayoutLayoutPanel);
      this.WindowLayoutBox.Controls.Add(this.DisplayRadioButton);
      this.WindowLayoutBox.Controls.Add(this.AbsoluteRadioButton);
      this.WindowLayoutBox.Controls.Add(this.DisplayHelperPanel);
      this.WindowLayoutBox.Location = new System.Drawing.Point(12, 73);
      this.WindowLayoutBox.Name = "WindowLayoutBox";
      this.WindowLayoutBox.Size = new System.Drawing.Size(541, 369);
      this.WindowLayoutBox.TabIndex = 9;
      this.WindowLayoutBox.TabStop = false;
      this.WindowLayoutBox.Text = "Window Layout";
      // 
      // WindowLayoutLayoutPanel
      // 
      this.WindowLayoutLayoutPanel.Controls.Add(this.groupBox1);
      this.WindowLayoutLayoutPanel.Controls.Add(this.groupBox2);
      this.WindowLayoutLayoutPanel.Location = new System.Drawing.Point(7, 42);
      this.WindowLayoutLayoutPanel.Name = "WindowLayoutLayoutPanel";
      this.WindowLayoutLayoutPanel.Size = new System.Drawing.Size(511, 51);
      this.WindowLayoutLayoutPanel.TabIndex = 10;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.flowLayoutPanel1);
      this.groupBox1.Location = new System.Drawing.Point(3, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(200, 45);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Position (x,y)";
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Controls.Add(this.WindowLocationBoxY);
      this.flowLayoutPanel1.Controls.Add(this.WindowSizeBoxWidth);
      this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(194, 26);
      this.flowLayoutPanel1.TabIndex = 0;
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.flowLayoutPanel2);
      this.groupBox2.Location = new System.Drawing.Point(209, 3);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(200, 45);
      this.groupBox2.TabIndex = 10;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Size (width, height)";
      // 
      // flowLayoutPanel2
      // 
      this.flowLayoutPanel2.Controls.Add(this.WindowLocationBoxX);
      this.flowLayoutPanel2.Controls.Add(this.WindowSizeBoxHeight);
      this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 16);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new System.Drawing.Size(194, 26);
      this.flowLayoutPanel2.TabIndex = 0;
      // 
      // DisplayRadioButton
      // 
      this.DisplayRadioButton.AutoSize = true;
      this.DisplayRadioButton.Location = new System.Drawing.Point(7, 99);
      this.DisplayRadioButton.Name = "DisplayRadioButton";
      this.DisplayRadioButton.Size = new System.Drawing.Size(59, 17);
      this.DisplayRadioButton.TabIndex = 10;
      this.DisplayRadioButton.Text = "Display";
      this.DisplayRadioButton.UseVisualStyleBackColor = true;
      this.DisplayRadioButton.CheckedChanged += new System.EventHandler(this.DisplayRadioButton_CheckedChanged);
      // 
      // AbsoluteRadioButton
      // 
      this.AbsoluteRadioButton.AutoSize = true;
      this.AbsoluteRadioButton.Checked = true;
      this.AbsoluteRadioButton.Location = new System.Drawing.Point(7, 19);
      this.AbsoluteRadioButton.Name = "AbsoluteRadioButton";
      this.AbsoluteRadioButton.Size = new System.Drawing.Size(66, 17);
      this.AbsoluteRadioButton.TabIndex = 9;
      this.AbsoluteRadioButton.TabStop = true;
      this.AbsoluteRadioButton.Text = "Absolute";
      this.AbsoluteRadioButton.UseVisualStyleBackColor = true;
      this.AbsoluteRadioButton.CheckedChanged += new System.EventHandler(this.AbsoluteRadioButton_CheckedChanged);
      // 
      // DisplayHelperPanel
      // 
      this.DisplayHelperPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.DisplayHelperPanel.Enabled = false;
      this.DisplayHelperPanel.Location = new System.Drawing.Point(6, 122);
      this.DisplayHelperPanel.MinimumSize = new System.Drawing.Size(475, 4);
      this.DisplayHelperPanel.Name = "DisplayHelperPanel";
      this.DisplayHelperPanel.Size = new System.Drawing.Size(518, 241);
      this.DisplayHelperPanel.TabIndex = 8;
      this.DisplayHelperPanel.OnRectangleChanged += new System.EventHandler(this.DisplayHelperPanel_OnRectangleChanged);
      // 
      // WindowConfigDialog
      // 
      this.AcceptButton = this.WindowConfigDialogSaveButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(561, 566);
      this.ControlBox = false;
      this.Controls.Add(this.WindowLayoutBox);
      this.Controls.Add(this.BringToFrontCheckBox);
      this.Controls.Add(this.WindowConfigNameGroupBox);
      this.Controls.Add(this.WindowConfigDialogCancelButton);
      this.Controls.Add(this.WindowConfigDialogSaveButton);
      this.Controls.Add(this.MatchStringGroupBox);
      this.Controls.Add(this.WindowMatchGroupBox);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "WindowConfigDialog";
      this.ShowIcon = false;
      this.Text = "Window Configuration";
      this.Load += new System.EventHandler(this.WindowConfigDialog_Load);
      this.WindowMatchGroupBox.ResumeLayout(false);
      this.WindowMatchGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.WindowSizeBoxHeight)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowSizeBoxWidth)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxY)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxX)).EndInit();
      this.MatchStringGroupBox.ResumeLayout(false);
      this.MatchStringGroupBox.PerformLayout();
      this.WindowConfigNameGroupBox.ResumeLayout(false);
      this.WindowConfigNameGroupBox.PerformLayout();
      this.WindowLayoutBox.ResumeLayout(false);
      this.WindowLayoutBox.PerformLayout();
      this.WindowLayoutLayoutPanel.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private GroupBox WindowMatchGroupBox;
    private NumericUpDown WindowSizeBoxHeight;
    private NumericUpDown WindowSizeBoxWidth;
    private NumericUpDown WindowLocationBoxY;
    private NumericUpDown WindowLocationBoxX;
    private RadioButton WindowProcessNameRadioButton;
    private RadioButton WindowTitleRadioButton;
    private GroupBox MatchStringGroupBox;
    private TextBox WindowMatchStringTextBox;
    private Button WindowConfigDialogSaveButton;
    private Button WindowConfigDialogCancelButton;
    private GroupBox WindowConfigNameGroupBox;
    private TextBox WindowConfigNameTextBox;
    private CheckBox BringToFrontCheckBox;
    private DisplayHelperPanel DisplayHelperPanel;
    private GroupBox WindowLayoutBox;
    private RadioButton DisplayRadioButton;
    private RadioButton AbsoluteRadioButton;
    private FlowLayoutPanel WindowLayoutLayoutPanel;
    private GroupBox groupBox1;
    private FlowLayoutPanel flowLayoutPanel1;
    private GroupBox groupBox2;
    private FlowLayoutPanel flowLayoutPanel2;
  }
}