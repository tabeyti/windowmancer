namespace Windowmancer.UI
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
      this.NLogTextBox = new System.Windows.Forms.RichTextBox();
      this.ActiveWindowsGridView = new System.Windows.Forms.DataGridView();
      this.PID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.WindowTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ProcessName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Icon = new System.Windows.Forms.DataGridViewImageColumn();
      this.ProfileListBox = new System.Windows.Forms.ListBox();
      this.ProfileGroupBox = new System.Windows.Forms.GroupBox();
      this.ProfileSplitContainer = new System.Windows.Forms.SplitContainer();
      this.WindowConfigsGroupBox = new System.Windows.Forms.GroupBox();
      this.AddWindowConfigButton = new System.Windows.Forms.Button();
      this.SavedWindowsDataGrid = new System.Windows.Forms.DataGridView();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.button1 = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.ActiveWindowsGridView)).BeginInit();
      this.ProfileGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ProfileSplitContainer)).BeginInit();
      this.ProfileSplitContainer.Panel1.SuspendLayout();
      this.ProfileSplitContainer.Panel2.SuspendLayout();
      this.ProfileSplitContainer.SuspendLayout();
      this.WindowConfigsGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.SavedWindowsDataGrid)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // NLogTextBox
      // 
      this.NLogTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
      this.NLogTextBox.Location = new System.Drawing.Point(3, 3);
      this.NLogTextBox.Name = "NLogTextBox";
      this.NLogTextBox.Size = new System.Drawing.Size(495, 363);
      this.NLogTextBox.TabIndex = 1;
      this.NLogTextBox.Text = "";
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
      this.ActiveWindowsGridView.Location = new System.Drawing.Point(504, 3);
      this.ActiveWindowsGridView.MultiSelect = false;
      this.ActiveWindowsGridView.Name = "ActiveWindowsGridView";
      this.ActiveWindowsGridView.ReadOnly = true;
      this.ActiveWindowsGridView.RowHeadersVisible = false;
      this.ActiveWindowsGridView.RowTemplate.Height = 30;
      this.ActiveWindowsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.ActiveWindowsGridView.Size = new System.Drawing.Size(396, 363);
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
      // ProfileListBox
      // 
      this.ProfileListBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ProfileListBox.FormattingEnabled = true;
      this.ProfileListBox.Location = new System.Drawing.Point(3, 16);
      this.ProfileListBox.Name = "ProfileListBox";
      this.ProfileListBox.Size = new System.Drawing.Size(295, 354);
      this.ProfileListBox.TabIndex = 4;
      // 
      // ProfileGroupBox
      // 
      this.ProfileGroupBox.Controls.Add(this.ProfileListBox);
      this.ProfileGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ProfileGroupBox.Location = new System.Drawing.Point(0, 0);
      this.ProfileGroupBox.Name = "ProfileGroupBox";
      this.ProfileGroupBox.Size = new System.Drawing.Size(301, 373);
      this.ProfileGroupBox.TabIndex = 5;
      this.ProfileGroupBox.TabStop = false;
      this.ProfileGroupBox.Text = "Profiles";
      // 
      // ProfileSplitContainer
      // 
      this.ProfileSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ProfileSplitContainer.Location = new System.Drawing.Point(0, 0);
      this.ProfileSplitContainer.Name = "ProfileSplitContainer";
      // 
      // ProfileSplitContainer.Panel1
      // 
      this.ProfileSplitContainer.Panel1.Controls.Add(this.ProfileGroupBox);
      // 
      // ProfileSplitContainer.Panel2
      // 
      this.ProfileSplitContainer.Panel2.Controls.Add(this.WindowConfigsGroupBox);
      this.ProfileSplitContainer.Size = new System.Drawing.Size(903, 373);
      this.ProfileSplitContainer.SplitterDistance = 301;
      this.ProfileSplitContainer.TabIndex = 6;
      // 
      // WindowConfigsGroupBox
      // 
      this.WindowConfigsGroupBox.Controls.Add(this.AddWindowConfigButton);
      this.WindowConfigsGroupBox.Controls.Add(this.SavedWindowsDataGrid);
      this.WindowConfigsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.WindowConfigsGroupBox.Location = new System.Drawing.Point(0, 0);
      this.WindowConfigsGroupBox.Name = "WindowConfigsGroupBox";
      this.WindowConfigsGroupBox.Size = new System.Drawing.Size(598, 373);
      this.WindowConfigsGroupBox.TabIndex = 0;
      this.WindowConfigsGroupBox.TabStop = false;
      this.WindowConfigsGroupBox.Text = "Window Configs";
      // 
      // AddWindowConfigButton
      // 
      this.AddWindowConfigButton.Location = new System.Drawing.Point(7, 340);
      this.AddWindowConfigButton.Name = "AddWindowConfigButton";
      this.AddWindowConfigButton.Size = new System.Drawing.Size(75, 23);
      this.AddWindowConfigButton.TabIndex = 1;
      this.AddWindowConfigButton.Text = "Add";
      this.AddWindowConfigButton.UseVisualStyleBackColor = true;
      this.AddWindowConfigButton.Click += new System.EventHandler(this.AddWindowConfigButton_Click);
      // 
      // SavedWindowsDataGrid
      // 
      this.SavedWindowsDataGrid.AllowUserToAddRows = false;
      this.SavedWindowsDataGrid.AllowUserToDeleteRows = false;
      this.SavedWindowsDataGrid.AllowUserToResizeRows = false;
      this.SavedWindowsDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.SavedWindowsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.SavedWindowsDataGrid.Location = new System.Drawing.Point(2, 16);
      this.SavedWindowsDataGrid.MultiSelect = false;
      this.SavedWindowsDataGrid.Name = "SavedWindowsDataGrid";
      this.SavedWindowsDataGrid.ReadOnly = true;
      this.SavedWindowsDataGrid.RowHeadersVisible = false;
      this.SavedWindowsDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.SavedWindowsDataGrid.Size = new System.Drawing.Size(590, 317);
      this.SavedWindowsDataGrid.TabIndex = 0;
      // 
      // splitContainer1
      // 
      this.splitContainer1.Location = new System.Drawing.Point(12, 12);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.ProfileSplitContainer);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.NLogTextBox);
      this.splitContainer1.Panel2.Controls.Add(this.ActiveWindowsGridView);
      this.splitContainer1.Size = new System.Drawing.Size(903, 746);
      this.splitContainer1.SplitterDistance = 373;
      this.splitContainer1.TabIndex = 7;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(964, 47);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 8;
      this.button1.Text = "button1";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1173, 770);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.splitContainer1);
      this.Name = "MainForm";
      this.Text = "Form1";
      this.Load += new System.EventHandler(this.Form1_Load);
      ((System.ComponentModel.ISupportInitialize)(this.ActiveWindowsGridView)).EndInit();
      this.ProfileGroupBox.ResumeLayout(false);
      this.ProfileSplitContainer.Panel1.ResumeLayout(false);
      this.ProfileSplitContainer.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.ProfileSplitContainer)).EndInit();
      this.ProfileSplitContainer.ResumeLayout(false);
      this.WindowConfigsGroupBox.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.SavedWindowsDataGrid)).EndInit();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion
    private System.Windows.Forms.RichTextBox NLogTextBox;
    private System.Windows.Forms.DataGridView ActiveWindowsGridView;
    private System.Windows.Forms.DataGridViewTextBoxColumn PID;
    private System.Windows.Forms.DataGridViewTextBoxColumn WindowTitle;
    private System.Windows.Forms.DataGridViewTextBoxColumn ProcessName;
    private System.Windows.Forms.DataGridViewImageColumn Icon;
    private System.Windows.Forms.ListBox ProfileListBox;
    private System.Windows.Forms.GroupBox ProfileGroupBox;
    private System.Windows.Forms.SplitContainer ProfileSplitContainer;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.GroupBox WindowConfigsGroupBox;
    private System.Windows.Forms.DataGridView SavedWindowsDataGrid;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button AddWindowConfigButton;
  }
}

