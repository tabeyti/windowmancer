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
      this.displayHelperPanel1 = new Windowmancer.UI.DisplayHelperPanel();
      this.WindowMatchGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.WindowSizeBoxHeight)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowSizeBoxWidth)).BeginInit();
      this.WindowPostitionGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxY)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxX)).BeginInit();
      this.MatchStringGroupBox.SuspendLayout();
      this.WindowConfigNameGroupBox.SuspendLayout();
      this.WindowLayoutBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // WindowMatchGroupBox
      // 
      this.WindowMatchGroupBox.Controls.Add(this.WindowProcessNameRadioButton);
      this.WindowMatchGroupBox.Controls.Add(this.WindowTitleRadioButton);
      this.WindowMatchGroupBox.Location = new System.Drawing.Point(289, 469);
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
      this.WindowSizeBoxHeight.Location = new System.Drawing.Point(277, 37);
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
      this.WindowSizeBoxWidth.Location = new System.Drawing.Point(389, 37);
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
      this.WindowPostitionGroupBox.Controls.Add(this.WindowSizeBoxHeight);
      this.WindowPostitionGroupBox.Controls.Add(this.WindowSizeBoxWidth);
      this.WindowPostitionGroupBox.Controls.Add(this.WindowLocationBoxY);
      this.WindowPostitionGroupBox.Controls.Add(this.WindowLocationBoxX);
      this.WindowPostitionGroupBox.Location = new System.Drawing.Point(12, 394);
      this.WindowPostitionGroupBox.Name = "WindowPostitionGroupBox";
      this.WindowPostitionGroupBox.Size = new System.Drawing.Size(475, 75);
      this.WindowPostitionGroupBox.TabIndex = 2;
      this.WindowPostitionGroupBox.TabStop = false;
      this.WindowPostitionGroupBox.Text = "Window Layout";
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
      this.WindowLocationBoxX.Location = new System.Drawing.Point(7, 37);
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
      this.MatchStringGroupBox.Location = new System.Drawing.Point(12, 469);
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
      this.WindowConfigDialogSaveButton.Location = new System.Drawing.Point(725, 528);
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
      this.WindowConfigDialogCancelButton.Location = new System.Drawing.Point(644, 528);
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
      this.BringToFrontCheckBox.Location = new System.Drawing.Point(12, 523);
      this.BringToFrontCheckBox.Name = "BringToFrontCheckBox";
      this.BringToFrontCheckBox.Size = new System.Drawing.Size(86, 17);
      this.BringToFrontCheckBox.TabIndex = 7;
      this.BringToFrontCheckBox.Text = "Bring to front";
      this.BringToFrontCheckBox.UseVisualStyleBackColor = true;
      // 
      // WindowLayoutBox
      // 
      this.WindowLayoutBox.Controls.Add(this.displayHelperPanel1);
      this.WindowLayoutBox.Location = new System.Drawing.Point(12, 73);
      this.WindowLayoutBox.Name = "WindowLayoutBox";
      this.WindowLayoutBox.Size = new System.Drawing.Size(486, 266);
      this.WindowLayoutBox.TabIndex = 9;
      this.WindowLayoutBox.TabStop = false;
      this.WindowLayoutBox.Text = "Window Layout";
      // 
      // displayHelperPanel1
      // 
      this.displayHelperPanel1.Location = new System.Drawing.Point(7, 19);
      this.displayHelperPanel1.MinimumSize = new System.Drawing.Size(475, 0);
      this.displayHelperPanel1.Name = "displayHelperPanel1";
      this.displayHelperPanel1.Size = new System.Drawing.Size(475, 241);
      this.displayHelperPanel1.TabIndex = 8;
      // 
      // WindowConfigDialog
      // 
      this.AcceptButton = this.WindowConfigDialogSaveButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(812, 563);
      this.ControlBox = false;
      this.Controls.Add(this.WindowLayoutBox);
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
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxY)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxX)).EndInit();
      this.MatchStringGroupBox.ResumeLayout(false);
      this.MatchStringGroupBox.PerformLayout();
      this.WindowConfigNameGroupBox.ResumeLayout(false);
      this.WindowConfigNameGroupBox.PerformLayout();
      this.WindowLayoutBox.ResumeLayout(false);
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
    private CheckBox BringToFrontCheckBox;
    private DisplayHelperPanel displayHelperPanel1;
    private GroupBox WindowLayoutBox;
  }
}