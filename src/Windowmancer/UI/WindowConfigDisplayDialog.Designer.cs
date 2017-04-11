namespace Windowmancer.UI
{
  partial class WindowConfigDisplayDialog
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

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
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.NumRowsSpinner = new System.Windows.Forms.NumericUpDown();
      this.NumColsSpinner = new System.Windows.Forms.NumericUpDown();
      this.NumRowsLabel = new System.Windows.Forms.Label();
      this.NumColsLabel = new System.Windows.Forms.Label();
      this.DisplayComboBox = new System.Windows.Forms.ComboBox();
      this.XTextBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.YTextBox = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.HeightTextBox = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.WidthTextBox = new System.Windows.Forms.TextBox();
      this.WindowInfoGroupBox = new System.Windows.Forms.GroupBox();
      this.SaveButton = new System.Windows.Forms.Button();
      this.CancelButton = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.NumRowsSpinner)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.NumColsSpinner)).BeginInit();
      this.WindowInfoGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Location = new System.Drawing.Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(420, 222);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "groupBox1";
      // 
      // NumRowsSpinner
      // 
      this.NumRowsSpinner.Location = new System.Drawing.Point(279, 240);
      this.NumRowsSpinner.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.NumRowsSpinner.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.NumRowsSpinner.Name = "NumRowsSpinner";
      this.NumRowsSpinner.ReadOnly = true;
      this.NumRowsSpinner.Size = new System.Drawing.Size(47, 20);
      this.NumRowsSpinner.TabIndex = 1;
      this.NumRowsSpinner.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.NumRowsSpinner.ValueChanged += new System.EventHandler(this.NumRowsSpinner_ValueChanged);
      // 
      // NumColsSpinner
      // 
      this.NumColsSpinner.Location = new System.Drawing.Point(385, 241);
      this.NumColsSpinner.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.NumColsSpinner.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.NumColsSpinner.Name = "NumColsSpinner";
      this.NumColsSpinner.ReadOnly = true;
      this.NumColsSpinner.Size = new System.Drawing.Size(47, 20);
      this.NumColsSpinner.TabIndex = 2;
      this.NumColsSpinner.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.NumColsSpinner.ValueChanged += new System.EventHandler(this.NumColsSpinner_ValueChanged);
      // 
      // NumRowsLabel
      // 
      this.NumRowsLabel.AutoSize = true;
      this.NumRowsLabel.Location = new System.Drawing.Point(239, 242);
      this.NumRowsLabel.Name = "NumRowsLabel";
      this.NumRowsLabel.Size = new System.Drawing.Size(34, 13);
      this.NumRowsLabel.TabIndex = 3;
      this.NumRowsLabel.Text = "Rows";
      // 
      // NumColsLabel
      // 
      this.NumColsLabel.AutoSize = true;
      this.NumColsLabel.Location = new System.Drawing.Point(332, 243);
      this.NumColsLabel.Name = "NumColsLabel";
      this.NumColsLabel.Size = new System.Drawing.Size(47, 13);
      this.NumColsLabel.TabIndex = 4;
      this.NumColsLabel.Text = "Columns";
      // 
      // DisplayComboBox
      // 
      this.DisplayComboBox.FormattingEnabled = true;
      this.DisplayComboBox.Location = new System.Drawing.Point(12, 240);
      this.DisplayComboBox.Name = "DisplayComboBox";
      this.DisplayComboBox.Size = new System.Drawing.Size(198, 21);
      this.DisplayComboBox.TabIndex = 5;
      this.DisplayComboBox.SelectedIndexChanged += new System.EventHandler(this.DisplayComboBox_SelectedIndexChanged);
      // 
      // XTextBox
      // 
      this.XTextBox.Enabled = false;
      this.XTextBox.Location = new System.Drawing.Point(52, 19);
      this.XTextBox.Name = "XTextBox";
      this.XTextBox.Size = new System.Drawing.Size(142, 20);
      this.XTextBox.TabIndex = 6;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(11, 22);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(14, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "X";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(11, 48);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(14, 13);
      this.label2.TabIndex = 9;
      this.label2.Text = "Y";
      // 
      // YTextBox
      // 
      this.YTextBox.Enabled = false;
      this.YTextBox.Location = new System.Drawing.Point(52, 45);
      this.YTextBox.Name = "YTextBox";
      this.YTextBox.Size = new System.Drawing.Size(142, 20);
      this.YTextBox.TabIndex = 8;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(11, 100);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(38, 13);
      this.label3.TabIndex = 13;
      this.label3.Text = "Height";
      // 
      // HeightTextBox
      // 
      this.HeightTextBox.Enabled = false;
      this.HeightTextBox.Location = new System.Drawing.Point(52, 97);
      this.HeightTextBox.Name = "HeightTextBox";
      this.HeightTextBox.Size = new System.Drawing.Size(142, 20);
      this.HeightTextBox.TabIndex = 12;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(11, 74);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(35, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Width";
      // 
      // WidthTextBox
      // 
      this.WidthTextBox.Enabled = false;
      this.WidthTextBox.Location = new System.Drawing.Point(52, 71);
      this.WidthTextBox.Name = "WidthTextBox";
      this.WidthTextBox.Size = new System.Drawing.Size(142, 20);
      this.WidthTextBox.TabIndex = 10;
      // 
      // WindowInfoGroupBox
      // 
      this.WindowInfoGroupBox.Controls.Add(this.label1);
      this.WindowInfoGroupBox.Controls.Add(this.label3);
      this.WindowInfoGroupBox.Controls.Add(this.XTextBox);
      this.WindowInfoGroupBox.Controls.Add(this.HeightTextBox);
      this.WindowInfoGroupBox.Controls.Add(this.YTextBox);
      this.WindowInfoGroupBox.Controls.Add(this.label4);
      this.WindowInfoGroupBox.Controls.Add(this.label2);
      this.WindowInfoGroupBox.Controls.Add(this.WidthTextBox);
      this.WindowInfoGroupBox.Location = new System.Drawing.Point(438, 12);
      this.WindowInfoGroupBox.Name = "WindowInfoGroupBox";
      this.WindowInfoGroupBox.Size = new System.Drawing.Size(200, 128);
      this.WindowInfoGroupBox.TabIndex = 14;
      this.WindowInfoGroupBox.TabStop = false;
      this.WindowInfoGroupBox.Text = "Position/Size Info";
      // 
      // SaveButton
      // 
      this.SaveButton.Location = new System.Drawing.Point(563, 240);
      this.SaveButton.Name = "SaveButton";
      this.SaveButton.Size = new System.Drawing.Size(75, 23);
      this.SaveButton.TabIndex = 15;
      this.SaveButton.Text = "Okay";
      this.SaveButton.UseVisualStyleBackColor = true;
      this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
      // 
      // CancelButton
      // 
      this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.CancelButton.Location = new System.Drawing.Point(482, 240);
      this.CancelButton.Name = "CancelButton";
      this.CancelButton.Size = new System.Drawing.Size(75, 23);
      this.CancelButton.TabIndex = 16;
      this.CancelButton.Text = "Cancel";
      this.CancelButton.UseVisualStyleBackColor = true;
      // 
      // WindowConfigDisplayDialog
      // 
      this.AcceptButton = this.SaveButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.CancelButton;
      this.ClientSize = new System.Drawing.Size(647, 268);
      this.ControlBox = false;
      this.Controls.Add(this.CancelButton);
      this.Controls.Add(this.SaveButton);
      this.Controls.Add(this.WindowInfoGroupBox);
      this.Controls.Add(this.DisplayComboBox);
      this.Controls.Add(this.NumColsLabel);
      this.Controls.Add(this.NumRowsLabel);
      this.Controls.Add(this.NumColsSpinner);
      this.Controls.Add(this.NumRowsSpinner);
      this.Controls.Add(this.groupBox1);
      this.Name = "WindowConfigDisplayDialog";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Display Helper";
      this.Load += new System.EventHandler(this.WindowConfigDisplayDialog_Load);
      ((System.ComponentModel.ISupportInitialize)(this.NumRowsSpinner)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.NumColsSpinner)).EndInit();
      this.WindowInfoGroupBox.ResumeLayout(false);
      this.WindowInfoGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.NumericUpDown NumRowsSpinner;
    private System.Windows.Forms.NumericUpDown NumColsSpinner;
    private System.Windows.Forms.Label NumRowsLabel;
    private System.Windows.Forms.Label NumColsLabel;
    private System.Windows.Forms.ComboBox DisplayComboBox;
    private System.Windows.Forms.TextBox XTextBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox YTextBox;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox HeightTextBox;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox WidthTextBox;
    private System.Windows.Forms.GroupBox WindowInfoGroupBox;
    private System.Windows.Forms.Button SaveButton;
    private System.Windows.Forms.Button CancelButton;
  }
}