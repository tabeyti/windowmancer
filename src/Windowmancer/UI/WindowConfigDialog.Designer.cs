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
      this.WindowPostitionGroupBox = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.WindowLocationBoxY = new System.Windows.Forms.NumericUpDown();
      this.WindowLocationBoxX = new System.Windows.Forms.NumericUpDown();
      this.MatchStringGroupBox = new System.Windows.Forms.GroupBox();
      this.WindowMatchStringTextBox = new System.Windows.Forms.TextBox();
      this.WindowConfigDialogSaveButton = new System.Windows.Forms.Button();
      this.WindowConfigDialogCancelButton = new System.Windows.Forms.Button();
      this.WindowConfigNameGroupBox = new System.Windows.Forms.GroupBox();
      this.WindowConfigNameTextBox = new System.Windows.Forms.TextBox();
      this.BringToFrontCheckBox = new System.Windows.Forms.CheckBox();
      this.DisplayHelperButton = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.WindowMatchGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.WindowSizeBoxHeight)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowSizeBoxWidth)).BeginInit();
      this.WindowPostitionGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxY)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxX)).BeginInit();
      this.MatchStringGroupBox.SuspendLayout();
      this.WindowConfigNameGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // WindowMatchGroupBox
      // 
      this.WindowMatchGroupBox.Controls.Add(this.WindowProcessNameRadioButton);
      this.WindowMatchGroupBox.Controls.Add(this.WindowTitleRadioButton);
      this.WindowMatchGroupBox.Location = new System.Drawing.Point(12, 228);
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
      this.WindowSizeBoxHeight.Location = new System.Drawing.Point(112, 85);
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
      this.WindowSizeBoxWidth.Location = new System.Drawing.Point(6, 85);
      this.WindowSizeBoxWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.WindowSizeBoxWidth.Name = "WindowSizeBoxWidth";
      this.WindowSizeBoxWidth.Size = new System.Drawing.Size(80, 20);
      this.WindowSizeBoxWidth.TabIndex = 0;
      // 
      // WindowPostitionGroupBox
      // 
      this.WindowPostitionGroupBox.Controls.Add(this.label3);
      this.WindowPostitionGroupBox.Controls.Add(this.WindowSizeBoxHeight);
      this.WindowPostitionGroupBox.Controls.Add(this.DisplayHelperButton);
      this.WindowPostitionGroupBox.Controls.Add(this.WindowSizeBoxWidth);
      this.WindowPostitionGroupBox.Controls.Add(this.label1);
      this.WindowPostitionGroupBox.Controls.Add(this.WindowLocationBoxY);
      this.WindowPostitionGroupBox.Controls.Add(this.WindowLocationBoxX);
      this.WindowPostitionGroupBox.Location = new System.Drawing.Point(12, 73);
      this.WindowPostitionGroupBox.Name = "WindowPostitionGroupBox";
      this.WindowPostitionGroupBox.Size = new System.Drawing.Size(268, 149);
      this.WindowPostitionGroupBox.TabIndex = 2;
      this.WindowPostitionGroupBox.TabStop = false;
      this.WindowPostitionGroupBox.Text = "Window Layout";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(4, 21);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(26, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "X/Y";
      // 
      // WindowLocationBoxY
      // 
      this.WindowLocationBoxY.Location = new System.Drawing.Point(112, 37);
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
      this.WindowLocationBoxX.Location = new System.Drawing.Point(6, 37);
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
      this.MatchStringGroupBox.Location = new System.Drawing.Point(12, 282);
      this.MatchStringGroupBox.Name = "MatchStringGroupBox";
      this.MatchStringGroupBox.Size = new System.Drawing.Size(274, 54);
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
      this.WindowConfigDialogSaveButton.Location = new System.Drawing.Point(208, 371);
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
      this.WindowConfigDialogCancelButton.Location = new System.Drawing.Point(127, 371);
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
      this.WindowConfigNameGroupBox.Size = new System.Drawing.Size(271, 54);
      this.WindowConfigNameGroupBox.TabIndex = 6;
      this.WindowConfigNameGroupBox.TabStop = false;
      this.WindowConfigNameGroupBox.Text = "Name";
      // 
      // WindowConfigNameTextBox
      // 
      this.WindowConfigNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.WindowConfigNameTextBox.Location = new System.Drawing.Point(3, 16);
      this.WindowConfigNameTextBox.Name = "WindowConfigNameTextBox";
      this.WindowConfigNameTextBox.Size = new System.Drawing.Size(265, 20);
      this.WindowConfigNameTextBox.TabIndex = 0;
      // 
      // BringToFrontCheckBox
      // 
      this.BringToFrontCheckBox.AutoSize = true;
      this.BringToFrontCheckBox.Checked = true;
      this.BringToFrontCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.BringToFrontCheckBox.Location = new System.Drawing.Point(12, 342);
      this.BringToFrontCheckBox.Name = "BringToFrontCheckBox";
      this.BringToFrontCheckBox.Size = new System.Drawing.Size(86, 17);
      this.BringToFrontCheckBox.TabIndex = 7;
      this.BringToFrontCheckBox.Text = "Bring to front";
      this.BringToFrontCheckBox.UseVisualStyleBackColor = true;
      // 
      // DisplayHelperButton
      // 
      this.DisplayHelperButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.DisplayHelperButton.Location = new System.Drawing.Point(3, 111);
      this.DisplayHelperButton.Name = "DisplayHelperButton";
      this.DisplayHelperButton.Size = new System.Drawing.Size(259, 32);
      this.DisplayHelperButton.TabIndex = 8;
      this.DisplayHelperButton.Text = "Display Helper";
      this.DisplayHelperButton.UseVisualStyleBackColor = true;
      this.DisplayHelperButton.Click += new System.EventHandler(this.button1_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(4, 69);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(71, 13);
      this.label3.TabIndex = 9;
      this.label3.Text = "Width/Height";
      // 
      // WindowConfigDialog
      // 
      this.AcceptButton = this.WindowConfigDialogSaveButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(295, 406);
      this.ControlBox = false;
      this.Controls.Add(this.BringToFrontCheckBox);
      this.Controls.Add(this.WindowConfigNameGroupBox);
      this.Controls.Add(this.WindowConfigDialogCancelButton);
      this.Controls.Add(this.WindowConfigDialogSaveButton);
      this.Controls.Add(this.MatchStringGroupBox);
      this.Controls.Add(this.WindowPostitionGroupBox);
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
      this.WindowPostitionGroupBox.ResumeLayout(false);
      this.WindowPostitionGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxY)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxX)).EndInit();
      this.MatchStringGroupBox.ResumeLayout(false);
      this.MatchStringGroupBox.PerformLayout();
      this.WindowConfigNameGroupBox.ResumeLayout(false);
      this.WindowConfigNameGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private GroupBox WindowMatchGroupBox;
    private NumericUpDown WindowSizeBoxHeight;
    private NumericUpDown WindowSizeBoxWidth;
    private GroupBox WindowPostitionGroupBox;
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
    private Label label1;
    private CheckBox BringToFrontCheckBox;
    private Button DisplayHelperButton;
    private Label label3;
  }
}