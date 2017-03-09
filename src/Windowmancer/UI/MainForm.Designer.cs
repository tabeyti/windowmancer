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
      this.ActiveWindowsGridView = new System.Windows.Forms.DataGridView();
      this.PID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.WindowTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ProcessName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Icon = new System.Windows.Forms.DataGridViewImageColumn();
      this.ProfileListBox = new System.Windows.Forms.ListBox();
      this.ProfileGroupBox = new System.Windows.Forms.GroupBox();
      this.ProfileSplitContainer = new System.Windows.Forms.SplitContainer();
      this.WindowConfigsGroupBox = new System.Windows.Forms.GroupBox();
      this.SavedWindowsDataGrid = new System.Windows.Forms.DataGridView();
      this.MainFormSplitContainer = new System.Windows.Forms.SplitContainer();
      ((System.ComponentModel.ISupportInitialize)(this.ActiveWindowsGridView)).BeginInit();
      this.ProfileGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ProfileSplitContainer)).BeginInit();
      this.ProfileSplitContainer.Panel1.SuspendLayout();
      this.ProfileSplitContainer.Panel2.SuspendLayout();
      this.ProfileSplitContainer.SuspendLayout();
      this.WindowConfigsGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.SavedWindowsDataGrid)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.MainFormSplitContainer)).BeginInit();
      this.MainFormSplitContainer.Panel1.SuspendLayout();
      this.MainFormSplitContainer.Panel2.SuspendLayout();
      this.MainFormSplitContainer.SuspendLayout();
      this.SuspendLayout();
      // 
      // ActiveWindowsGridView
      // 
      this.ActiveWindowsGridView.AllowUserToAddRows = false;
      this.ActiveWindowsGridView.AllowUserToDeleteRows = false;
      this.ActiveWindowsGridView.AllowUserToResizeRows = false;
      this.ActiveWindowsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ActiveWindowsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.ActiveWindowsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ActiveWindowsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PID,
            this.WindowTitle,
            this.ProcessName,
            this.Icon});
      this.ActiveWindowsGridView.Location = new System.Drawing.Point(3, 3);
      this.ActiveWindowsGridView.MultiSelect = false;
      this.ActiveWindowsGridView.Name = "ActiveWindowsGridView";
      this.ActiveWindowsGridView.ReadOnly = true;
      this.ActiveWindowsGridView.RowHeadersVisible = false;
      this.ActiveWindowsGridView.RowTemplate.Height = 30;
      this.ActiveWindowsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.ActiveWindowsGridView.Size = new System.Drawing.Size(886, 304);
      this.ActiveWindowsGridView.TabIndex = 3;
      this.ActiveWindowsGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ActiveWindowsGridView_CellMouseDoubleClick);
      // 
      // PID
      // 
      this.PID.HeaderText = "PID";
      this.PID.Name = "PID";
      this.PID.ReadOnly = true;
      this.PID.Width = 50;
      // 
      // WindowTitle
      // 
      this.WindowTitle.HeaderText = "Window Title";
      this.WindowTitle.Name = "WindowTitle";
      this.WindowTitle.ReadOnly = true;
      this.WindowTitle.Width = 94;
      // 
      // ProcessName
      // 
      this.ProcessName.HeaderText = "Process Name";
      this.ProcessName.Name = "ProcessName";
      this.ProcessName.ReadOnly = true;
      this.ProcessName.Width = 101;
      // 
      // Icon
      // 
      this.Icon.HeaderText = "Icon";
      this.Icon.Name = "Icon";
      this.Icon.ReadOnly = true;
      this.Icon.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.Icon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
      this.Icon.Width = 53;
      // 
      // ProfileListBox
      // 
      this.ProfileListBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ProfileListBox.FormattingEnabled = true;
      this.ProfileListBox.Location = new System.Drawing.Point(3, 16);
      this.ProfileListBox.Name = "ProfileListBox";
      this.ProfileListBox.Size = new System.Drawing.Size(247, 294);
      this.ProfileListBox.TabIndex = 4;
      // 
      // ProfileGroupBox
      // 
      this.ProfileGroupBox.Controls.Add(this.ProfileListBox);
      this.ProfileGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ProfileGroupBox.Location = new System.Drawing.Point(0, 0);
      this.ProfileGroupBox.Name = "ProfileGroupBox";
      this.ProfileGroupBox.Size = new System.Drawing.Size(253, 313);
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
      this.ProfileSplitContainer.Size = new System.Drawing.Size(892, 313);
      this.ProfileSplitContainer.SplitterDistance = 253;
      this.ProfileSplitContainer.TabIndex = 6;
      // 
      // WindowConfigsGroupBox
      // 
      this.WindowConfigsGroupBox.Controls.Add(this.SavedWindowsDataGrid);
      this.WindowConfigsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.WindowConfigsGroupBox.Location = new System.Drawing.Point(0, 0);
      this.WindowConfigsGroupBox.Name = "WindowConfigsGroupBox";
      this.WindowConfigsGroupBox.Size = new System.Drawing.Size(635, 313);
      this.WindowConfigsGroupBox.TabIndex = 0;
      this.WindowConfigsGroupBox.TabStop = false;
      this.WindowConfigsGroupBox.Text = "Window Configs";
      // 
      // SavedWindowsDataGrid
      // 
      this.SavedWindowsDataGrid.AllowUserToAddRows = false;
      this.SavedWindowsDataGrid.AllowUserToDeleteRows = false;
      this.SavedWindowsDataGrid.AllowUserToResizeRows = false;
      this.SavedWindowsDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.SavedWindowsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.SavedWindowsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.SavedWindowsDataGrid.Location = new System.Drawing.Point(3, 16);
      this.SavedWindowsDataGrid.MultiSelect = false;
      this.SavedWindowsDataGrid.Name = "SavedWindowsDataGrid";
      this.SavedWindowsDataGrid.ReadOnly = true;
      this.SavedWindowsDataGrid.RowHeadersVisible = false;
      this.SavedWindowsDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.SavedWindowsDataGrid.Size = new System.Drawing.Size(629, 294);
      this.SavedWindowsDataGrid.TabIndex = 0;
      // 
      // MainFormSplitContainer
      // 
      this.MainFormSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.MainFormSplitContainer.Location = new System.Drawing.Point(0, 0);
      this.MainFormSplitContainer.Name = "MainFormSplitContainer";
      this.MainFormSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // MainFormSplitContainer.Panel1
      // 
      this.MainFormSplitContainer.Panel1.Controls.Add(this.ProfileSplitContainer);
      // 
      // MainFormSplitContainer.Panel2
      // 
      this.MainFormSplitContainer.Panel2.Controls.Add(this.ActiveWindowsGridView);
      this.MainFormSplitContainer.Size = new System.Drawing.Size(892, 627);
      this.MainFormSplitContainer.SplitterDistance = 313;
      this.MainFormSplitContainer.TabIndex = 7;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(892, 627);
      this.Controls.Add(this.MainFormSplitContainer);
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
      this.MainFormSplitContainer.Panel1.ResumeLayout(false);
      this.MainFormSplitContainer.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.MainFormSplitContainer)).EndInit();
      this.MainFormSplitContainer.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion
    private System.Windows.Forms.DataGridView ActiveWindowsGridView;
    private System.Windows.Forms.DataGridViewTextBoxColumn PID;
    private System.Windows.Forms.DataGridViewTextBoxColumn WindowTitle;
    private System.Windows.Forms.DataGridViewTextBoxColumn ProcessName;
    private System.Windows.Forms.DataGridViewImageColumn Icon;
    private System.Windows.Forms.ListBox ProfileListBox;
    private System.Windows.Forms.GroupBox ProfileGroupBox;
    private System.Windows.Forms.SplitContainer ProfileSplitContainer;
    private System.Windows.Forms.SplitContainer MainFormSplitContainer;
    private System.Windows.Forms.GroupBox WindowConfigsGroupBox;
    private System.Windows.Forms.DataGridView SavedWindowsDataGrid;
  }
}

