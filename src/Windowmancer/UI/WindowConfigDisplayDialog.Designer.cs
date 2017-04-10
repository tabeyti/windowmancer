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
      ((System.ComponentModel.ISupportInitialize)(this.NumRowsSpinner)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.NumColsSpinner)).BeginInit();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Location = new System.Drawing.Point(96, 36);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(344, 233);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "groupBox1";
      // 
      // NumRowsSpinner
      // 
      this.NumRowsSpinner.Location = new System.Drawing.Point(146, 316);
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
      this.NumRowsSpinner.Size = new System.Drawing.Size(49, 20);
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
      this.NumColsSpinner.Location = new System.Drawing.Point(146, 342);
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
      this.NumColsSpinner.Size = new System.Drawing.Size(49, 20);
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
      this.NumRowsLabel.Location = new System.Drawing.Point(93, 318);
      this.NumRowsLabel.Name = "NumRowsLabel";
      this.NumRowsLabel.Size = new System.Drawing.Size(34, 13);
      this.NumRowsLabel.TabIndex = 3;
      this.NumRowsLabel.Text = "Rows";
      // 
      // NumColsLabel
      // 
      this.NumColsLabel.AutoSize = true;
      this.NumColsLabel.Location = new System.Drawing.Point(93, 344);
      this.NumColsLabel.Name = "NumColsLabel";
      this.NumColsLabel.Size = new System.Drawing.Size(47, 13);
      this.NumColsLabel.TabIndex = 4;
      this.NumColsLabel.Text = "Columns";
      // 
      // DisplayComboBox
      // 
      this.DisplayComboBox.FormattingEnabled = true;
      this.DisplayComboBox.Location = new System.Drawing.Point(96, 276);
      this.DisplayComboBox.Name = "DisplayComboBox";
      this.DisplayComboBox.Size = new System.Drawing.Size(121, 21);
      this.DisplayComboBox.TabIndex = 5;
      // 
      // WindowConfigDisplayDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(566, 416);
      this.Controls.Add(this.DisplayComboBox);
      this.Controls.Add(this.NumColsLabel);
      this.Controls.Add(this.NumRowsLabel);
      this.Controls.Add(this.NumColsSpinner);
      this.Controls.Add(this.NumRowsSpinner);
      this.Controls.Add(this.groupBox1);
      this.Name = "WindowConfigDisplayDialog";
      this.Text = "WindowConfigDisplayDialog";
      this.Load += new System.EventHandler(this.WindowConfigDisplayDialog_Load);
      ((System.ComponentModel.ISupportInitialize)(this.NumRowsSpinner)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.NumColsSpinner)).EndInit();
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
  }
}