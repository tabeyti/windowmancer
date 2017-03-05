namespace Windowmancer
{
    partial class MainForm
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
            InternalDispose();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      this.ProcessButton = new System.Windows.Forms.Button();
      this.NLogTextBox = new System.Windows.Forms.RichTextBox();
      this.Display = new System.Windows.Forms.Button();
      this.ActiveWindowsGridView = new System.Windows.Forms.DataGridView();
      this.PID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.WindowTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ProcessName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Icon = new System.Windows.Forms.DataGridViewImageColumn();
      this.ProfileListView = new System.Windows.Forms.ListView();
      ((System.ComponentModel.ISupportInitialize)(this.ActiveWindowsGridView)).BeginInit();
      this.SuspendLayout();
      // 
      // ProcessButton
      // 
      this.ProcessButton.Location = new System.Drawing.Point(1086, 12);
      this.ProcessButton.Name = "ProcessButton";
      this.ProcessButton.Size = new System.Drawing.Size(75, 23);
      this.ProcessButton.TabIndex = 0;
      this.ProcessButton.Text = "Process";
      this.ProcessButton.UseVisualStyleBackColor = true;
      this.ProcessButton.Click += new System.EventHandler(this.button1_Click);
      // 
      // NLogTextBox
      // 
      this.NLogTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
      this.NLogTextBox.Location = new System.Drawing.Point(12, 363);
      this.NLogTextBox.Name = "NLogTextBox";
      this.NLogTextBox.Size = new System.Drawing.Size(495, 155);
      this.NLogTextBox.TabIndex = 1;
      this.NLogTextBox.Text = "";
      // 
      // Display
      // 
      this.Display.Location = new System.Drawing.Point(1086, 41);
      this.Display.Name = "Display";
      this.Display.Size = new System.Drawing.Size(75, 23);
      this.Display.TabIndex = 2;
      this.Display.Text = "Display";
      this.Display.UseVisualStyleBackColor = true;
      this.Display.Click += new System.EventHandler(this.Display_Click);
      // 
      // ActiveWindowsGridView
      // 
      this.ActiveWindowsGridView.AllowUserToAddRows = false;
      this.ActiveWindowsGridView.AllowUserToDeleteRows = false;
      this.ActiveWindowsGridView.AllowUserToResizeRows = false;
      this.ActiveWindowsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ActiveWindowsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ActiveWindowsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PID,
            this.WindowTitle,
            this.ProcessName,
            this.Icon});
      this.ActiveWindowsGridView.Location = new System.Drawing.Point(514, 12);
      this.ActiveWindowsGridView.MultiSelect = false;
      this.ActiveWindowsGridView.Name = "ActiveWindowsGridView";
      this.ActiveWindowsGridView.ReadOnly = true;
      this.ActiveWindowsGridView.RowHeadersVisible = false;
      this.ActiveWindowsGridView.RowTemplate.Height = 30;
      this.ActiveWindowsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.ActiveWindowsGridView.Size = new System.Drawing.Size(458, 506);
      this.ActiveWindowsGridView.TabIndex = 3;
      // 
      // PID
      // 
      this.PID.HeaderText = "PID";
      this.PID.Name = "PID";
      this.PID.ReadOnly = true;
      // 
      // WindowTitle
      // 
      this.WindowTitle.HeaderText = "Window Title";
      this.WindowTitle.Name = "WindowTitle";
      this.WindowTitle.ReadOnly = true;
      // 
      // ProcessName
      // 
      this.ProcessName.HeaderText = "Process Name";
      this.ProcessName.Name = "ProcessName";
      this.ProcessName.ReadOnly = true;
      // 
      // Icon
      // 
      this.Icon.HeaderText = "Icon";
      this.Icon.Name = "Icon";
      this.Icon.ReadOnly = true;
      this.Icon.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.Icon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
      // 
      // ProfileListView
      // 
      this.ProfileListView.Location = new System.Drawing.Point(12, 12);
      this.ProfileListView.Name = "ProfileListView";
      this.ProfileListView.Size = new System.Drawing.Size(495, 345);
      this.ProfileListView.TabIndex = 4;
      this.ProfileListView.UseCompatibleStateImageBehavior = false;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1173, 536);
      this.Controls.Add(this.ProfileListView);
      this.Controls.Add(this.ActiveWindowsGridView);
      this.Controls.Add(this.Display);
      this.Controls.Add(this.NLogTextBox);
      this.Controls.Add(this.ProcessButton);
      this.Name = "Form1";
      this.Text = "Form1";
      this.Load += new System.EventHandler(this.Form1_Load);
      ((System.ComponentModel.ISupportInitialize)(this.ActiveWindowsGridView)).EndInit();
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ProcessButton;
    private System.Windows.Forms.RichTextBox NLogTextBox;
    private System.Windows.Forms.Button Display;
    private System.Windows.Forms.DataGridView ActiveWindowsGridView;
    private System.Windows.Forms.DataGridViewTextBoxColumn PID;
    private System.Windows.Forms.DataGridViewTextBoxColumn WindowTitle;
    private System.Windows.Forms.DataGridViewTextBoxColumn ProcessName;
    private System.Windows.Forms.DataGridViewImageColumn Icon;
    private System.Windows.Forms.ListView ProfileListView;
  }
}

