namespace Windowmancer.UI
{
    partial class Editor
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
      this.ActiveWindowsGridView = new System.Windows.Forms.DataGridView();
      this.IconHeader = new System.Windows.Forms.DataGridViewImageColumn();
      this.PID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ProcessName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.WindowTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ProfileListBoxContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.ProfileGroupBox = new System.Windows.Forms.GroupBox();
      this.ProfileSplitContainer = new System.Windows.Forms.SplitContainer();
      this.WindowConfigsGroupBox = new System.Windows.Forms.GroupBox();
      this.WindowConfigsDataGrid = new System.Windows.Forms.DataGridView();
      this.WindowConfigsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.MainFormSplitContainer = new System.Windows.Forms.SplitContainer();
      this.ActiveWindowsGroupBox = new System.Windows.Forms.GroupBox();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.preferencesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.ProfileListDataGridView = new System.Windows.Forms.DataGridView();
      ((System.ComponentModel.ISupportInitialize)(this.ActiveWindowsGridView)).BeginInit();
      this.ProfileListBoxContextMenu.SuspendLayout();
      this.ProfileGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ProfileSplitContainer)).BeginInit();
      this.ProfileSplitContainer.Panel1.SuspendLayout();
      this.ProfileSplitContainer.Panel2.SuspendLayout();
      this.ProfileSplitContainer.SuspendLayout();
      this.WindowConfigsGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.WindowConfigsDataGrid)).BeginInit();
      this.WindowConfigsContextMenu.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.MainFormSplitContainer)).BeginInit();
      this.MainFormSplitContainer.Panel1.SuspendLayout();
      this.MainFormSplitContainer.Panel2.SuspendLayout();
      this.MainFormSplitContainer.SuspendLayout();
      this.ActiveWindowsGroupBox.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ProfileListDataGridView)).BeginInit();
      this.SuspendLayout();
      // 
      // ActiveWindowsGridView
      // 
      this.ActiveWindowsGridView.AllowUserToAddRows = false;
      this.ActiveWindowsGridView.AllowUserToDeleteRows = false;
      this.ActiveWindowsGridView.AllowUserToResizeRows = false;
      this.ActiveWindowsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.ActiveWindowsGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
      this.ActiveWindowsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ActiveWindowsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IconHeader,
            this.PID,
            this.ProcessName,
            this.WindowTitle});
      this.ActiveWindowsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ActiveWindowsGridView.Location = new System.Drawing.Point(3, 16);
      this.ActiveWindowsGridView.MultiSelect = false;
      this.ActiveWindowsGridView.Name = "ActiveWindowsGridView";
      this.ActiveWindowsGridView.ReadOnly = true;
      this.ActiveWindowsGridView.RowHeadersVisible = false;
      this.ActiveWindowsGridView.RowTemplate.Height = 30;
      this.ActiveWindowsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.ActiveWindowsGridView.Size = new System.Drawing.Size(886, 292);
      this.ActiveWindowsGridView.TabIndex = 3;
      this.ActiveWindowsGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ActiveWindowsGridView_CellMouseDoubleClick);
      // 
      // IconHeader
      // 
      this.IconHeader.HeaderText = "Icon";
      this.IconHeader.Name = "IconHeader";
      this.IconHeader.ReadOnly = true;
      this.IconHeader.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
      this.IconHeader.Width = 53;
      // 
      // PID
      // 
      this.PID.FillWeight = 49.24402F;
      this.PID.HeaderText = "PID";
      this.PID.Name = "PID";
      this.PID.ReadOnly = true;
      this.PID.Width = 50;
      // 
      // ProcessName
      // 
      this.ProcessName.FillWeight = 150.3063F;
      this.ProcessName.HeaderText = "Process Name";
      this.ProcessName.Name = "ProcessName";
      this.ProcessName.ReadOnly = true;
      this.ProcessName.Width = 101;
      // 
      // WindowTitle
      // 
      this.WindowTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.WindowTitle.FillWeight = 176.4406F;
      this.WindowTitle.HeaderText = "Window Title";
      this.WindowTitle.Name = "WindowTitle";
      this.WindowTitle.ReadOnly = true;
      // 
      // ProfileListBoxContextMenu
      // 
      this.ProfileListBoxContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.deleteToolStripMenuItem1});
      this.ProfileListBoxContextMenu.Name = "ProfileListBoxContextMenu";
      this.ProfileListBoxContextMenu.Size = new System.Drawing.Size(108, 48);
      // 
      // addToolStripMenuItem
      // 
      this.addToolStripMenuItem.Name = "addToolStripMenuItem";
      this.addToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
      this.addToolStripMenuItem.Text = "Add";
      this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
      // 
      // deleteToolStripMenuItem1
      // 
      this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
      this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
      this.deleteToolStripMenuItem1.Text = "Delete";
      this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem1_Click);
      // 
      // ProfileGroupBox
      // 
      this.ProfileGroupBox.BackColor = System.Drawing.SystemColors.Control;
      this.ProfileGroupBox.Controls.Add(this.ProfileListDataGridView);
      this.ProfileGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ProfileGroupBox.Location = new System.Drawing.Point(0, 0);
      this.ProfileGroupBox.Name = "ProfileGroupBox";
      this.ProfileGroupBox.Size = new System.Drawing.Size(223, 288);
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
      this.ProfileSplitContainer.Size = new System.Drawing.Size(892, 288);
      this.ProfileSplitContainer.SplitterDistance = 223;
      this.ProfileSplitContainer.TabIndex = 6;
      // 
      // WindowConfigsGroupBox
      // 
      this.WindowConfigsGroupBox.BackColor = System.Drawing.SystemColors.Control;
      this.WindowConfigsGroupBox.Controls.Add(this.WindowConfigsDataGrid);
      this.WindowConfigsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.WindowConfigsGroupBox.Location = new System.Drawing.Point(0, 0);
      this.WindowConfigsGroupBox.Name = "WindowConfigsGroupBox";
      this.WindowConfigsGroupBox.Size = new System.Drawing.Size(665, 288);
      this.WindowConfigsGroupBox.TabIndex = 0;
      this.WindowConfigsGroupBox.TabStop = false;
      this.WindowConfigsGroupBox.Text = "Window Configs";
      // 
      // WindowConfigsDataGrid
      // 
      this.WindowConfigsDataGrid.AllowUserToAddRows = false;
      this.WindowConfigsDataGrid.AllowUserToDeleteRows = false;
      this.WindowConfigsDataGrid.AllowUserToResizeRows = false;
      this.WindowConfigsDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.WindowConfigsDataGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
      this.WindowConfigsDataGrid.BackgroundColor = System.Drawing.SystemColors.Window;
      this.WindowConfigsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.WindowConfigsDataGrid.ContextMenuStrip = this.WindowConfigsContextMenu;
      this.WindowConfigsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.WindowConfigsDataGrid.EnableHeadersVisualStyles = false;
      this.WindowConfigsDataGrid.Location = new System.Drawing.Point(3, 16);
      this.WindowConfigsDataGrid.MultiSelect = false;
      this.WindowConfigsDataGrid.Name = "WindowConfigsDataGrid";
      this.WindowConfigsDataGrid.ReadOnly = true;
      this.WindowConfigsDataGrid.RowHeadersVisible = false;
      this.WindowConfigsDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.WindowConfigsDataGrid.Size = new System.Drawing.Size(659, 269);
      this.WindowConfigsDataGrid.TabIndex = 0;
      this.WindowConfigsDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SavedWindowsDataGrid_CellDoubleClick);
      this.WindowConfigsDataGrid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.WindowConfigsDataGrid_CellMouseDown);
      // 
      // WindowConfigsContextMenu
      // 
      this.WindowConfigsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem});
      this.WindowConfigsContextMenu.Name = "WindowConfigsContextMenu";
      this.WindowConfigsContextMenu.Size = new System.Drawing.Size(108, 48);
      this.WindowConfigsContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.WindowConfigsContextMenu_Opening);
      // 
      // editToolStripMenuItem
      // 
      this.editToolStripMenuItem.Name = "editToolStripMenuItem";
      this.editToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
      this.editToolStripMenuItem.Text = "Edit";
      this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
      // 
      // deleteToolStripMenuItem
      // 
      this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
      this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
      this.deleteToolStripMenuItem.Text = "Delete";
      this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
      // 
      // MainFormSplitContainer
      // 
      this.MainFormSplitContainer.BackColor = System.Drawing.SystemColors.Control;
      this.MainFormSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.MainFormSplitContainer.Location = new System.Drawing.Point(0, 24);
      this.MainFormSplitContainer.Name = "MainFormSplitContainer";
      this.MainFormSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // MainFormSplitContainer.Panel1
      // 
      this.MainFormSplitContainer.Panel1.Controls.Add(this.ProfileSplitContainer);
      // 
      // MainFormSplitContainer.Panel2
      // 
      this.MainFormSplitContainer.Panel2.Controls.Add(this.ActiveWindowsGroupBox);
      this.MainFormSplitContainer.Size = new System.Drawing.Size(892, 603);
      this.MainFormSplitContainer.SplitterDistance = 288;
      this.MainFormSplitContainer.TabIndex = 7;
      // 
      // ActiveWindowsGroupBox
      // 
      this.ActiveWindowsGroupBox.Controls.Add(this.ActiveWindowsGridView);
      this.ActiveWindowsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ActiveWindowsGroupBox.Location = new System.Drawing.Point(0, 0);
      this.ActiveWindowsGroupBox.Name = "ActiveWindowsGroupBox";
      this.ActiveWindowsGroupBox.Size = new System.Drawing.Size(892, 311);
      this.ActiveWindowsGroupBox.TabIndex = 4;
      this.ActiveWindowsGroupBox.TabStop = false;
      this.ActiveWindowsGroupBox.Text = "Active Windows";
      // 
      // menuStrip1
      // 
      this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preferencesToolStripMenuItem,
            this.helpToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(892, 24);
      this.menuStrip1.TabIndex = 8;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // preferencesToolStripMenuItem
      // 
      this.preferencesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preferencesToolStripMenuItem1});
      this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
      this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
      this.preferencesToolStripMenuItem.Text = "Preferences";
      // 
      // preferencesToolStripMenuItem1
      // 
      this.preferencesToolStripMenuItem1.Name = "preferencesToolStripMenuItem1";
      this.preferencesToolStripMenuItem1.Size = new System.Drawing.Size(116, 22);
      this.preferencesToolStripMenuItem1.Text = "Settings";
      this.preferencesToolStripMenuItem1.Click += new System.EventHandler(this.preferencesToolStripMenuItem1_Click);
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.documentationToolStripMenuItem,
            this.toolStripSeparator1,
            this.aboutToolStripMenuItem});
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
      this.helpToolStripMenuItem.Text = "Help";
      // 
      // documentationToolStripMenuItem
      // 
      this.documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
      this.documentationToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
      this.documentationToolStripMenuItem.Text = "Documentation";
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(154, 6);
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
      this.aboutToolStripMenuItem.Text = "About";
      // 
      // ProfileListDataGridView
      // 
      this.ProfileListDataGridView.AllowUserToAddRows = false;
      this.ProfileListDataGridView.AllowUserToDeleteRows = false;
      this.ProfileListDataGridView.AllowUserToResizeRows = false;
      this.ProfileListDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.ProfileListDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
      this.ProfileListDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
      this.ProfileListDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ProfileListDataGridView.ColumnHeadersVisible = false;
      this.ProfileListDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ProfileListDataGridView.EnableHeadersVisualStyles = false;
      this.ProfileListDataGridView.Location = new System.Drawing.Point(3, 16);
      this.ProfileListDataGridView.MultiSelect = false;
      this.ProfileListDataGridView.Name = "ProfileListDataGridView";
      this.ProfileListDataGridView.ReadOnly = true;
      this.ProfileListDataGridView.RowHeadersVisible = false;
      this.ProfileListDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.ProfileListDataGridView.Size = new System.Drawing.Size(217, 269);
      this.ProfileListDataGridView.TabIndex = 5;
      // 
      // Editor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Control;
      this.ClientSize = new System.Drawing.Size(892, 627);
      this.Controls.Add(this.MainFormSplitContainer);
      this.Controls.Add(this.menuStrip1);
      this.Cursor = System.Windows.Forms.Cursors.Arrow;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "Editor";
      this.Text = "Windowmancer";
      this.Load += new System.EventHandler(this.Form1_Load);
      ((System.ComponentModel.ISupportInitialize)(this.ActiveWindowsGridView)).EndInit();
      this.ProfileListBoxContextMenu.ResumeLayout(false);
      this.ProfileGroupBox.ResumeLayout(false);
      this.ProfileSplitContainer.Panel1.ResumeLayout(false);
      this.ProfileSplitContainer.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.ProfileSplitContainer)).EndInit();
      this.ProfileSplitContainer.ResumeLayout(false);
      this.WindowConfigsGroupBox.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.WindowConfigsDataGrid)).EndInit();
      this.WindowConfigsContextMenu.ResumeLayout(false);
      this.MainFormSplitContainer.Panel1.ResumeLayout(false);
      this.MainFormSplitContainer.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.MainFormSplitContainer)).EndInit();
      this.MainFormSplitContainer.ResumeLayout(false);
      this.ActiveWindowsGroupBox.ResumeLayout(false);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ProfileListDataGridView)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion
    private System.Windows.Forms.DataGridView ActiveWindowsGridView;
    private System.Windows.Forms.GroupBox ProfileGroupBox;
    private System.Windows.Forms.SplitContainer MainFormSplitContainer;
    private System.Windows.Forms.GroupBox WindowConfigsGroupBox;
    private System.Windows.Forms.DataGridView WindowConfigsDataGrid;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem documentationToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    private System.Windows.Forms.DataGridViewImageColumn IconHeader;
    private System.Windows.Forms.DataGridViewTextBoxColumn PID;
    private System.Windows.Forms.DataGridViewTextBoxColumn ProcessName;
    private System.Windows.Forms.DataGridViewTextBoxColumn WindowTitle;
    private System.Windows.Forms.ContextMenuStrip WindowConfigsContextMenu;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ContextMenuStrip ProfileListBoxContextMenu;
    private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
    private System.Windows.Forms.GroupBox ActiveWindowsGroupBox;
    private System.Windows.Forms.SplitContainer ProfileSplitContainer;
    public System.Windows.Forms.DataGridView ProfileListDataGridView;
  }
}

