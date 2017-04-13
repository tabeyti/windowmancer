namespace Windowmancer.UI
{
  partial class DisplayHelperPanel
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.DisplaysListBox = new System.Windows.Forms.ListBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.NumRowsLabel = new System.Windows.Forms.Label();
      this.NumRowsSpinner = new System.Windows.Forms.NumericUpDown();
      this.NumColsLabel = new System.Windows.Forms.Label();
      this.GridConfigGroupBox = new System.Windows.Forms.GroupBox();
      this.NumColsSpinner = new System.Windows.Forms.NumericUpDown();
      ((System.ComponentModel.ISupportInitialize)(this.NumRowsSpinner)).BeginInit();
      this.GridConfigGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.NumColsSpinner)).BeginInit();
      this.SuspendLayout();
      // 
      // DisplaysListBox
      // 
      this.DisplaysListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.DisplaysListBox.FormattingEnabled = true;
      this.DisplaysListBox.Location = new System.Drawing.Point(3, 3);
      this.DisplaysListBox.Name = "DisplaysListBox";
      this.DisplaysListBox.Size = new System.Drawing.Size(108, 238);
      this.DisplaysListBox.TabIndex = 23;
      this.DisplaysListBox.SelectedIndexChanged += new System.EventHandler(this.DisplaysListBox_SelectedIndexChanged);
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Location = new System.Drawing.Point(117, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(355, 183);
      this.groupBox1.TabIndex = 19;
      this.groupBox1.TabStop = false;
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
      // NumColsLabel
      // 
      this.NumColsLabel.AutoSize = true;
      this.NumColsLabel.Location = new System.Drawing.Point(194, 21);
      this.NumColsLabel.Name = "NumColsLabel";
      this.NumColsLabel.Size = new System.Drawing.Size(47, 13);
      this.NumColsLabel.TabIndex = 4;
      this.NumColsLabel.Text = "Columns";
      // 
      // GridConfigGroupBox
      // 
      this.GridConfigGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.GridConfigGroupBox.Controls.Add(this.NumRowsLabel);
      this.GridConfigGroupBox.Controls.Add(this.NumRowsSpinner);
      this.GridConfigGroupBox.Controls.Add(this.NumColsLabel);
      this.GridConfigGroupBox.Controls.Add(this.NumColsSpinner);
      this.GridConfigGroupBox.Location = new System.Drawing.Point(119, 192);
      this.GridConfigGroupBox.Name = "GridConfigGroupBox";
      this.GridConfigGroupBox.Size = new System.Drawing.Size(353, 55);
      this.GridConfigGroupBox.TabIndex = 24;
      this.GridConfigGroupBox.TabStop = false;
      this.GridConfigGroupBox.Text = "Grid Config";
      // 
      // NumColsSpinner
      // 
      this.NumColsSpinner.Location = new System.Drawing.Point(247, 19);
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
      // DisplayHelperPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.DisplaysListBox);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.GridConfigGroupBox);
      this.MinimumSize = new System.Drawing.Size(475, 0);
      this.Name = "DisplayHelperPanel";
      this.Size = new System.Drawing.Size(475, 250);
      this.Load += new System.EventHandler(this.DisplayHelperPanel_Load);
      ((System.ComponentModel.ISupportInitialize)(this.NumRowsSpinner)).EndInit();
      this.GridConfigGroupBox.ResumeLayout(false);
      this.GridConfigGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.NumColsSpinner)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListBox DisplaysListBox;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label NumRowsLabel;
    private System.Windows.Forms.NumericUpDown NumRowsSpinner;
    private System.Windows.Forms.Label NumColsLabel;
    private System.Windows.Forms.GroupBox GridConfigGroupBox;
    private System.Windows.Forms.NumericUpDown NumColsSpinner;
  }
}
