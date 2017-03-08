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
      this.ProcessNameRadioButton = new System.Windows.Forms.RadioButton();
      this.WindowTitleRadioButton = new System.Windows.Forms.RadioButton();
      this.WindowSizeGroupBox = new System.Windows.Forms.GroupBox();
      this.WindowSizeBoxHeight = new System.Windows.Forms.NumericUpDown();
      this.WindowSizeBoxWidth = new System.Windows.Forms.NumericUpDown();
      this.WindowPostitionGroupBox = new System.Windows.Forms.GroupBox();
      this.WindowLocationBoxY = new System.Windows.Forms.NumericUpDown();
      this.WindowLocationBoxX = new System.Windows.Forms.NumericUpDown();
      this.MatchStringGroupBox = new System.Windows.Forms.GroupBox();
      this.MatchStringTextBox = new System.Windows.Forms.TextBox();
      this.WindowConfigDialogSaveButton = new System.Windows.Forms.Button();
      this.WindowConfigDialogCancelButton = new System.Windows.Forms.Button();
      this.DisplayLabel = new System.Windows.Forms.Label();
      this.WindowLocationDisplayComboBox = new System.Windows.Forms.ComboBox();
      this.WindowMatchGroupBox.SuspendLayout();
      this.WindowSizeGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.WindowSizeBoxHeight)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowSizeBoxWidth)).BeginInit();
      this.WindowPostitionGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxY)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxX)).BeginInit();
      this.MatchStringGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // WindowMatchGroupBox
      // 
      this.WindowMatchGroupBox.Controls.Add(this.ProcessNameRadioButton);
      this.WindowMatchGroupBox.Controls.Add(this.WindowTitleRadioButton);
      this.WindowMatchGroupBox.Location = new System.Drawing.Point(12, 169);
      this.WindowMatchGroupBox.Name = "WindowMatchGroupBox";
      this.WindowMatchGroupBox.Size = new System.Drawing.Size(200, 48);
      this.WindowMatchGroupBox.TabIndex = 0;
      this.WindowMatchGroupBox.TabStop = false;
      this.WindowMatchGroupBox.Text = "Match By";
      // 
      // ProcessNameRadioButton
      // 
      this.ProcessNameRadioButton.AutoSize = true;
      this.ProcessNameRadioButton.Location = new System.Drawing.Point(100, 20);
      this.ProcessNameRadioButton.Name = "ProcessNameRadioButton";
      this.ProcessNameRadioButton.Size = new System.Drawing.Size(94, 17);
      this.ProcessNameRadioButton.TabIndex = 2;
      this.ProcessNameRadioButton.TabStop = true;
      this.ProcessNameRadioButton.Text = "Process Name";
      this.ProcessNameRadioButton.UseVisualStyleBackColor = true;
      // 
      // WindowTitleRadioButton
      // 
      this.WindowTitleRadioButton.AutoSize = true;
      this.WindowTitleRadioButton.Location = new System.Drawing.Point(7, 20);
      this.WindowTitleRadioButton.Name = "WindowTitleRadioButton";
      this.WindowTitleRadioButton.Size = new System.Drawing.Size(87, 17);
      this.WindowTitleRadioButton.TabIndex = 1;
      this.WindowTitleRadioButton.TabStop = true;
      this.WindowTitleRadioButton.Text = "Window Title";
      this.WindowTitleRadioButton.UseVisualStyleBackColor = true;
      // 
      // WindowSizeGroupBox
      // 
      this.WindowSizeGroupBox.Controls.Add(this.WindowSizeBoxHeight);
      this.WindowSizeGroupBox.Controls.Add(this.WindowSizeBoxWidth);
      this.WindowSizeGroupBox.Location = new System.Drawing.Point(12, 12);
      this.WindowSizeGroupBox.Name = "WindowSizeGroupBox";
      this.WindowSizeGroupBox.Size = new System.Drawing.Size(200, 49);
      this.WindowSizeGroupBox.TabIndex = 1;
      this.WindowSizeGroupBox.TabStop = false;
      this.WindowSizeGroupBox.Text = "Window Size";
      // 
      // WindowSizeBoxHeight
      // 
      this.WindowSizeBoxHeight.Dock = System.Windows.Forms.DockStyle.Right;
      this.WindowSizeBoxHeight.Location = new System.Drawing.Point(102, 16);
      this.WindowSizeBoxHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.WindowSizeBoxHeight.Name = "WindowSizeBoxHeight";
      this.WindowSizeBoxHeight.Size = new System.Drawing.Size(95, 20);
      this.WindowSizeBoxHeight.TabIndex = 1;
      // 
      // WindowSizeBoxWidth
      // 
      this.WindowSizeBoxWidth.Dock = System.Windows.Forms.DockStyle.Left;
      this.WindowSizeBoxWidth.Location = new System.Drawing.Point(3, 16);
      this.WindowSizeBoxWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.WindowSizeBoxWidth.Name = "WindowSizeBoxWidth";
      this.WindowSizeBoxWidth.Size = new System.Drawing.Size(86, 20);
      this.WindowSizeBoxWidth.TabIndex = 0;
      // 
      // WindowPostitionGroupBox
      // 
      this.WindowPostitionGroupBox.Controls.Add(this.WindowLocationDisplayComboBox);
      this.WindowPostitionGroupBox.Controls.Add(this.DisplayLabel);
      this.WindowPostitionGroupBox.Controls.Add(this.WindowLocationBoxY);
      this.WindowPostitionGroupBox.Controls.Add(this.WindowLocationBoxX);
      this.WindowPostitionGroupBox.Location = new System.Drawing.Point(12, 67);
      this.WindowPostitionGroupBox.Name = "WindowPostitionGroupBox";
      this.WindowPostitionGroupBox.Size = new System.Drawing.Size(200, 96);
      this.WindowPostitionGroupBox.TabIndex = 2;
      this.WindowPostitionGroupBox.TabStop = false;
      this.WindowPostitionGroupBox.Text = "Window Position";
      // 
      // WindowLocationBoxY
      // 
      this.WindowLocationBoxY.Dock = System.Windows.Forms.DockStyle.Right;
      this.WindowLocationBoxY.Location = new System.Drawing.Point(102, 16);
      this.WindowLocationBoxY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.WindowLocationBoxY.Name = "WindowLocationBoxY";
      this.WindowLocationBoxY.Size = new System.Drawing.Size(95, 20);
      this.WindowLocationBoxY.TabIndex = 1;
      // 
      // WindowLocationBoxX
      // 
      this.WindowLocationBoxX.Dock = System.Windows.Forms.DockStyle.Left;
      this.WindowLocationBoxX.Location = new System.Drawing.Point(3, 16);
      this.WindowLocationBoxX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.WindowLocationBoxX.Name = "WindowLocationBoxX";
      this.WindowLocationBoxX.Size = new System.Drawing.Size(86, 20);
      this.WindowLocationBoxX.TabIndex = 0;
      // 
      // MatchStringGroupBox
      // 
      this.MatchStringGroupBox.Controls.Add(this.MatchStringTextBox);
      this.MatchStringGroupBox.Location = new System.Drawing.Point(12, 223);
      this.MatchStringGroupBox.Name = "MatchStringGroupBox";
      this.MatchStringGroupBox.Size = new System.Drawing.Size(200, 54);
      this.MatchStringGroupBox.TabIndex = 3;
      this.MatchStringGroupBox.TabStop = false;
      this.MatchStringGroupBox.Text = "Regex Match String";
      // 
      // MatchStringTextBox
      // 
      this.MatchStringTextBox.Location = new System.Drawing.Point(7, 20);
      this.MatchStringTextBox.Name = "MatchStringTextBox";
      this.MatchStringTextBox.Size = new System.Drawing.Size(187, 20);
      this.MatchStringTextBox.TabIndex = 0;
      // 
      // WindowConfigDialogSaveButton
      // 
      this.WindowConfigDialogSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.WindowConfigDialogSaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.WindowConfigDialogSaveButton.Location = new System.Drawing.Point(412, 254);
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
      this.WindowConfigDialogCancelButton.Location = new System.Drawing.Point(331, 254);
      this.WindowConfigDialogCancelButton.Name = "WindowConfigDialogCancelButton";
      this.WindowConfigDialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.WindowConfigDialogCancelButton.TabIndex = 5;
      this.WindowConfigDialogCancelButton.Text = "Cancel";
      this.WindowConfigDialogCancelButton.UseVisualStyleBackColor = true;
      this.WindowConfigDialogCancelButton.Click += new System.EventHandler(this.WindowConfigDialogCancelButton_Click);
      // 
      // DisplayLabel
      // 
      this.DisplayLabel.AutoSize = true;
      this.DisplayLabel.Location = new System.Drawing.Point(3, 51);
      this.DisplayLabel.Name = "DisplayLabel";
      this.DisplayLabel.Size = new System.Drawing.Size(41, 13);
      this.DisplayLabel.TabIndex = 3;
      this.DisplayLabel.Text = "Display";
      // 
      // WindowLocationDisplayComboBox
      // 
      this.WindowLocationDisplayComboBox.FormattingEnabled = true;
      this.WindowLocationDisplayComboBox.Location = new System.Drawing.Point(6, 68);
      this.WindowLocationDisplayComboBox.Name = "WindowLocationDisplayComboBox";
      this.WindowLocationDisplayComboBox.Size = new System.Drawing.Size(188, 21);
      this.WindowLocationDisplayComboBox.Sorted = true;
      this.WindowLocationDisplayComboBox.TabIndex = 4;
      // 
      // WindowConfigDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(499, 289);
      this.ControlBox = false;
      this.Controls.Add(this.WindowConfigDialogCancelButton);
      this.Controls.Add(this.WindowConfigDialogSaveButton);
      this.Controls.Add(this.MatchStringGroupBox);
      this.Controls.Add(this.WindowPostitionGroupBox);
      this.Controls.Add(this.WindowSizeGroupBox);
      this.Controls.Add(this.WindowMatchGroupBox);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "WindowConfigDialog";
      this.Text = "Window Configuration";
      this.Load += new System.EventHandler(this.WindowConfigDialog_Load);
      this.WindowMatchGroupBox.ResumeLayout(false);
      this.WindowMatchGroupBox.PerformLayout();
      this.WindowSizeGroupBox.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.WindowSizeBoxHeight)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowSizeBoxWidth)).EndInit();
      this.WindowPostitionGroupBox.ResumeLayout(false);
      this.WindowPostitionGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxY)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowLocationBoxX)).EndInit();
      this.MatchStringGroupBox.ResumeLayout(false);
      this.MatchStringGroupBox.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private GroupBox WindowMatchGroupBox;
    private GroupBox WindowSizeGroupBox;
    private NumericUpDown WindowSizeBoxHeight;
    private NumericUpDown WindowSizeBoxWidth;
    private GroupBox WindowPostitionGroupBox;
    private NumericUpDown WindowLocationBoxY;
    private NumericUpDown WindowLocationBoxX;
    private RadioButton ProcessNameRadioButton;
    private RadioButton WindowTitleRadioButton;
    private GroupBox MatchStringGroupBox;
    private TextBox MatchStringTextBox;
    private Button WindowConfigDialogSaveButton;
    private Button WindowConfigDialogCancelButton;
    private Label DisplayLabel;
    private ComboBox WindowLocationDisplayComboBox;
  }
}