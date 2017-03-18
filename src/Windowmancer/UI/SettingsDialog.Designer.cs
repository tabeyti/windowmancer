namespace Windowmancer.UI
{
  partial class SettingsDialog
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
      this.HotKeyGroupBox = new System.Windows.Forms.GroupBox();
      this.SaveButton = new System.Windows.Forms.Button();
      this.CancelButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // HotKeyGroupBox
      // 
      this.HotKeyGroupBox.Location = new System.Drawing.Point(13, 13);
      this.HotKeyGroupBox.Name = "HotKeyGroupBox";
      this.HotKeyGroupBox.Size = new System.Drawing.Size(229, 51);
      this.HotKeyGroupBox.TabIndex = 0;
      this.HotKeyGroupBox.TabStop = false;
      this.HotKeyGroupBox.Text = "Global Hot Key";
      // 
      // SaveButton
      // 
      this.SaveButton.Location = new System.Drawing.Point(166, 92);
      this.SaveButton.Name = "SaveButton";
      this.SaveButton.Size = new System.Drawing.Size(75, 23);
      this.SaveButton.TabIndex = 1;
      this.SaveButton.Text = "Save";
      this.SaveButton.UseVisualStyleBackColor = true;
      this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
      // 
      // CancelButton
      // 
      this.CancelButton.Location = new System.Drawing.Point(85, 92);
      this.CancelButton.Name = "CancelButton";
      this.CancelButton.Size = new System.Drawing.Size(75, 23);
      this.CancelButton.TabIndex = 2;
      this.CancelButton.Text = "Cancel";
      this.CancelButton.UseVisualStyleBackColor = true;
      this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
      // 
      // SettingsDialog
      // 
      this.AcceptButton = this.SaveButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(258, 127);
      this.Controls.Add(this.CancelButton);
      this.Controls.Add(this.SaveButton);
      this.Controls.Add(this.HotKeyGroupBox);
      this.Name = "SettingsDialog";
      this.ShowIcon = false;
      this.Text = "Settings";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox HotKeyGroupBox;
    private System.Windows.Forms.Button SaveButton;
    private System.Windows.Forms.Button CancelButton;
  }
}