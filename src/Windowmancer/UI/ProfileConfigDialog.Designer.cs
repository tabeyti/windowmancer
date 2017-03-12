namespace Windowmancer.UI
{
  partial class ProfileConfigDialog
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
      this.NameGroupBox = new System.Windows.Forms.GroupBox();
      this.ProfileNameTextBox = new System.Windows.Forms.TextBox();
      this.CancelButton = new System.Windows.Forms.Button();
      this.SaveButton = new System.Windows.Forms.Button();
      this.NameGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // NameGroupBox
      // 
      this.NameGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.NameGroupBox.AutoSize = true;
      this.NameGroupBox.Controls.Add(this.ProfileNameTextBox);
      this.NameGroupBox.Location = new System.Drawing.Point(13, 13);
      this.NameGroupBox.Name = "NameGroupBox";
      this.NameGroupBox.Size = new System.Drawing.Size(289, 53);
      this.NameGroupBox.TabIndex = 0;
      this.NameGroupBox.TabStop = false;
      this.NameGroupBox.Text = "Name";
      // 
      // ProfileNameTextBox
      // 
      this.ProfileNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ProfileNameTextBox.Location = new System.Drawing.Point(3, 16);
      this.ProfileNameTextBox.Name = "ProfileNameTextBox";
      this.ProfileNameTextBox.Size = new System.Drawing.Size(283, 20);
      this.ProfileNameTextBox.TabIndex = 1;
      // 
      // CancelButton
      // 
      this.CancelButton.Location = new System.Drawing.Point(146, 80);
      this.CancelButton.Name = "CancelButton";
      this.CancelButton.Size = new System.Drawing.Size(75, 23);
      this.CancelButton.TabIndex = 1;
      this.CancelButton.Text = "Cancel";
      this.CancelButton.UseVisualStyleBackColor = true;
      this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
      // 
      // SaveButton
      // 
      this.SaveButton.Location = new System.Drawing.Point(227, 80);
      this.SaveButton.Name = "SaveButton";
      this.SaveButton.Size = new System.Drawing.Size(75, 23);
      this.SaveButton.TabIndex = 2;
      this.SaveButton.Text = "Save";
      this.SaveButton.UseVisualStyleBackColor = true;
      this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
      // 
      // ProfileConfigDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(314, 115);
      this.Controls.Add(this.SaveButton);
      this.Controls.Add(this.CancelButton);
      this.Controls.Add(this.NameGroupBox);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ProfileConfigDialog";
      this.RightToLeftLayout = true;
      this.Text = "Profile";
      this.NameGroupBox.ResumeLayout(false);
      this.NameGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox NameGroupBox;
    private System.Windows.Forms.TextBox ProfileNameTextBox;
    private System.Windows.Forms.Button CancelButton;
    private System.Windows.Forms.Button SaveButton;
  }
}