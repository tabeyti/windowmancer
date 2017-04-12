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
      this.DisplaysListBox = new System.Windows.Forms.ListBox();
      this.GridConfigGroupBox = new System.Windows.Forms.GroupBox();
      ((System.ComponentModel.ISupportInitialize)(this.NumRowsSpinner)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.NumColsSpinner)).BeginInit();
      this.WindowInfoGroupBox.SuspendLayout();
      this.GridConfigGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Location = new System.Drawing.Point(132, 13);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(373, 258);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "groupBox1";
      // 
      // NumRowsSpinner
      // 
      this.NumRowsSpinner.Location = new System.Drawing.Point(59, 19);
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
      this.NumRowsSpinner.Size = new System.Drawing.Size(91, 20);
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
      this.NumColsSpinner.Location = new System.Drawing.Point(59, 48);
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
      this.NumColsSpinner.Size = new System.Drawing.Size(91, 20);
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
      this.NumRowsLabel.Location = new System.Drawing.Point(6, 21);
      this.NumRowsLabel.Name = "NumRowsLabel";
      this.NumRowsLabel.Size = new System.Drawing.Size(34, 13);
      this.NumRowsLabel.TabIndex = 3;
      this.NumRowsLabel.Text = "Rows";
      // 
      // NumColsLabel
      // 
      this.NumColsLabel.AutoSize = true;
      this.NumColsLabel.Location = new System.Drawing.Point(6, 50);
      this.NumColsLabel.Name = "NumColsLabel";
      this.NumColsLabel.Size = new System.Drawing.Size(47, 13);
      this.NumColsLabel.TabIndex = 4;
      this.NumColsLabel.Text = "Columns";
      // 
      // XTextBox
      // 
      this.XTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.XTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.XTextBox.Enabled = false;
      this.XTextBox.Location = new System.Drawing.Point(59, 19);
      this.XTextBox.Name = "XTextBox";
      this.XTextBox.Size = new System.Drawing.Size(91, 20);
      this.XTextBox.TabIndex = 6;
      this.XTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 19);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(14, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "X";
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 45);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(14, 13);
      this.label2.TabIndex = 9;
      this.label2.Text = "Y";
      // 
      // YTextBox
      // 
      this.YTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.YTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.YTextBox.Enabled = false;
      this.YTextBox.Location = new System.Drawing.Point(59, 45);
      this.YTextBox.Name = "YTextBox";
      this.YTextBox.Size = new System.Drawing.Size(91, 20);
      this.YTextBox.TabIndex = 8;
      this.YTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 97);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(38, 13);
      this.label3.TabIndex = 13;
      this.label3.Text = "Height";
      // 
      // HeightTextBox
      // 
      this.HeightTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.HeightTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.HeightTextBox.Enabled = false;
      this.HeightTextBox.Location = new System.Drawing.Point(59, 97);
      this.HeightTextBox.Name = "HeightTextBox";
      this.HeightTextBox.Size = new System.Drawing.Size(91, 20);
      this.HeightTextBox.TabIndex = 12;
      this.HeightTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // label4
      // 
      this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 71);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(35, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Width";
      // 
      // WidthTextBox
      // 
      this.WidthTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.WidthTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.WidthTextBox.Enabled = false;
      this.WidthTextBox.Location = new System.Drawing.Point(59, 71);
      this.WidthTextBox.Name = "WidthTextBox";
      this.WidthTextBox.Size = new System.Drawing.Size(91, 20);
      this.WidthTextBox.TabIndex = 10;
      this.WidthTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // WindowInfoGroupBox
      // 
      this.WindowInfoGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.WindowInfoGroupBox.Controls.Add(this.label1);
      this.WindowInfoGroupBox.Controls.Add(this.label3);
      this.WindowInfoGroupBox.Controls.Add(this.XTextBox);
      this.WindowInfoGroupBox.Controls.Add(this.HeightTextBox);
      this.WindowInfoGroupBox.Controls.Add(this.YTextBox);
      this.WindowInfoGroupBox.Controls.Add(this.label4);
      this.WindowInfoGroupBox.Controls.Add(this.label2);
      this.WindowInfoGroupBox.Controls.Add(this.WidthTextBox);
      this.WindowInfoGroupBox.Location = new System.Drawing.Point(511, 114);
      this.WindowInfoGroupBox.Name = "WindowInfoGroupBox";
      this.WindowInfoGroupBox.Size = new System.Drawing.Size(156, 128);
      this.WindowInfoGroupBox.TabIndex = 14;
      this.WindowInfoGroupBox.TabStop = false;
      this.WindowInfoGroupBox.Text = "Position/Size Info";
      // 
      // SaveButton
      // 
      this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.SaveButton.Location = new System.Drawing.Point(591, 248);
      this.SaveButton.Name = "SaveButton";
      this.SaveButton.Size = new System.Drawing.Size(75, 23);
      this.SaveButton.TabIndex = 15;
      this.SaveButton.Text = "Okay";
      this.SaveButton.UseVisualStyleBackColor = true;
      this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
      // 
      // CancelButton
      // 
      this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.CancelButton.Location = new System.Drawing.Point(510, 248);
      this.CancelButton.Name = "CancelButton";
      this.CancelButton.Size = new System.Drawing.Size(75, 23);
      this.CancelButton.TabIndex = 16;
      this.CancelButton.Text = "Cancel";
      this.CancelButton.UseVisualStyleBackColor = true;
      this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
      // 
      // DisplaysListBox
      // 
      this.DisplaysListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.DisplaysListBox.FormattingEnabled = true;
      this.DisplaysListBox.Location = new System.Drawing.Point(12, 13);
      this.DisplaysListBox.Name = "DisplaysListBox";
      this.DisplaysListBox.Size = new System.Drawing.Size(108, 251);
      this.DisplaysListBox.TabIndex = 17;
      this.DisplaysListBox.SelectedIndexChanged += new System.EventHandler(this.DisplaysListBox_SelectedIndexChanged);
      // 
      // GridConfigGroupBox
      // 
      this.GridConfigGroupBox.Controls.Add(this.NumRowsLabel);
      this.GridConfigGroupBox.Controls.Add(this.NumRowsSpinner);
      this.GridConfigGroupBox.Controls.Add(this.NumColsLabel);
      this.GridConfigGroupBox.Controls.Add(this.NumColsSpinner);
      this.GridConfigGroupBox.Location = new System.Drawing.Point(511, 13);
      this.GridConfigGroupBox.Name = "GridConfigGroupBox";
      this.GridConfigGroupBox.Size = new System.Drawing.Size(156, 95);
      this.GridConfigGroupBox.TabIndex = 18;
      this.GridConfigGroupBox.TabStop = false;
      this.GridConfigGroupBox.Text = "Grid Config";
      // 
      // WindowConfigDisplayDialog
      // 
      this.AcceptButton = this.SaveButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(675, 276);
      this.ControlBox = false;
      this.Controls.Add(this.GridConfigGroupBox);
      this.Controls.Add(this.DisplaysListBox);
      this.Controls.Add(this.CancelButton);
      this.Controls.Add(this.SaveButton);
      this.Controls.Add(this.WindowInfoGroupBox);
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
      this.GridConfigGroupBox.ResumeLayout(false);
      this.GridConfigGroupBox.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.NumericUpDown NumRowsSpinner;
    private System.Windows.Forms.NumericUpDown NumColsSpinner;
    private System.Windows.Forms.Label NumRowsLabel;
    private System.Windows.Forms.Label NumColsLabel;
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
    private System.Windows.Forms.ListBox DisplaysListBox;
    private System.Windows.Forms.GroupBox GridConfigGroupBox;
  }
}