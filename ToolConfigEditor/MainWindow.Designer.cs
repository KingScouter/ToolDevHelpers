namespace ToolConfigEditor
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            mainContentSplitContainer = new SplitContainer();
            toolListToolStripContainer = new ToolStripContainer();
            toolsListBox = new ListBox();
            toolListMenuStrip = new MenuStrip();
            addToolMenuItem = new ToolStripMenuItem();
            copyToolMenuItem = new ToolStripMenuItem();
            deleteToolMenuItem = new ToolStripMenuItem();
            titleLabel = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            additionalPagesLabel = new Label();
            additionalPagesTextBox = new TextBox();
            exePathLabel = new Label();
            exePathTextBox = new TextBox();
            remoteServerUrlLabel = new Label();
            remoteServerUrlTextBox = new TextBox();
            portLabel = new Label();
            portTextBox = new TextBox();
            useHttpsLabel = new Label();
            nameLabel = new Label();
            nameTextBox = new TextBox();
            keywordLabel = new Label();
            keywordTextBox = new TextBox();
            useHttpsCheckBox = new CheckBox();
            toolBarMenuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator = new ToolStripSeparator();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            statusBarLabel = new ToolStripStatusLabel();
            mainContentToolStripContainer = new ToolStripContainer();
            ((System.ComponentModel.ISupportInitialize)mainContentSplitContainer).BeginInit();
            mainContentSplitContainer.Panel1.SuspendLayout();
            mainContentSplitContainer.Panel2.SuspendLayout();
            mainContentSplitContainer.SuspendLayout();
            toolListToolStripContainer.ContentPanel.SuspendLayout();
            toolListToolStripContainer.TopToolStripPanel.SuspendLayout();
            toolListToolStripContainer.SuspendLayout();
            toolListMenuStrip.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            toolBarMenuStrip.SuspendLayout();
            statusStrip1.SuspendLayout();
            mainContentToolStripContainer.BottomToolStripPanel.SuspendLayout();
            mainContentToolStripContainer.ContentPanel.SuspendLayout();
            mainContentToolStripContainer.TopToolStripPanel.SuspendLayout();
            mainContentToolStripContainer.SuspendLayout();
            SuspendLayout();
            // 
            // mainContentSplitContainer
            // 
            mainContentSplitContainer.Dock = DockStyle.Fill;
            mainContentSplitContainer.FixedPanel = FixedPanel.Panel1;
            mainContentSplitContainer.Location = new Point(0, 0);
            mainContentSplitContainer.Name = "mainContentSplitContainer";
            // 
            // mainContentSplitContainer.Panel1
            // 
            mainContentSplitContainer.Panel1.Controls.Add(toolListToolStripContainer);
            // 
            // mainContentSplitContainer.Panel2
            // 
            mainContentSplitContainer.Panel2.Controls.Add(titleLabel);
            mainContentSplitContainer.Panel2.Controls.Add(tableLayoutPanel2);
            mainContentSplitContainer.Panel2.Padding = new Padding(20, 20, 20, 0);
            mainContentSplitContainer.Size = new Size(752, 606);
            mainContentSplitContainer.SplitterDistance = 255;
            mainContentSplitContainer.TabIndex = 1;
            // 
            // toolListToolStripContainer
            // 
            // 
            // toolListToolStripContainer.ContentPanel
            // 
            toolListToolStripContainer.ContentPanel.Controls.Add(toolsListBox);
            toolListToolStripContainer.ContentPanel.Size = new Size(255, 582);
            toolListToolStripContainer.Dock = DockStyle.Fill;
            toolListToolStripContainer.Location = new Point(0, 0);
            toolListToolStripContainer.Name = "toolListToolStripContainer";
            toolListToolStripContainer.Size = new Size(255, 606);
            toolListToolStripContainer.TabIndex = 5;
            toolListToolStripContainer.Text = "toolStripContainer2";
            // 
            // toolListToolStripContainer.TopToolStripPanel
            // 
            toolListToolStripContainer.TopToolStripPanel.Controls.Add(toolListMenuStrip);
            // 
            // toolsListBox
            // 
            toolsListBox.Dock = DockStyle.Fill;
            toolsListBox.FormattingEnabled = true;
            toolsListBox.Location = new Point(0, 0);
            toolsListBox.Name = "toolsListBox";
            toolsListBox.Size = new Size(255, 582);
            toolsListBox.TabIndex = 0;
            toolsListBox.SelectedValueChanged += ToolsListBoxOnSelectedValueChanged;
            // 
            // toolListMenuStrip
            // 
            toolListMenuStrip.Dock = DockStyle.None;
            toolListMenuStrip.Items.AddRange(new ToolStripItem[] { addToolMenuItem, copyToolMenuItem, deleteToolMenuItem });
            toolListMenuStrip.Location = new Point(0, 0);
            toolListMenuStrip.Name = "toolListMenuStrip";
            toolListMenuStrip.Size = new Size(255, 24);
            toolListMenuStrip.TabIndex = 0;
            toolListMenuStrip.Text = "menuStrip2";
            // 
            // addToolMenuItem
            // 
            addToolMenuItem.Name = "addToolMenuItem";
            addToolMenuItem.Size = new Size(41, 20);
            addToolMenuItem.Text = "Add";
            addToolMenuItem.Click += AddToolMenuItemOnClick;
            // 
            // copyToolMenuItem
            // 
            copyToolMenuItem.Name = "copyToolMenuItem";
            copyToolMenuItem.Size = new Size(47, 20);
            copyToolMenuItem.Text = "Copy";
            copyToolMenuItem.Click += CopyToolMenuItemOnClick;
            // 
            // deleteToolMenuItem
            // 
            deleteToolMenuItem.Name = "deleteToolMenuItem";
            deleteToolMenuItem.Size = new Size(52, 20);
            deleteToolMenuItem.Text = "Delete";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(321, 50);
            titleLabel.TabIndex = 1;
            titleLabel.Text = "Tool configuration";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tableLayoutPanel2.Controls.Add(additionalPagesLabel, 0, 6);
            tableLayoutPanel2.Controls.Add(additionalPagesTextBox, 1, 6);
            tableLayoutPanel2.Controls.Add(exePathLabel, 0, 5);
            tableLayoutPanel2.Controls.Add(exePathTextBox, 1, 5);
            tableLayoutPanel2.Controls.Add(remoteServerUrlLabel, 0, 4);
            tableLayoutPanel2.Controls.Add(remoteServerUrlTextBox, 1, 4);
            tableLayoutPanel2.Controls.Add(portLabel, 0, 3);
            tableLayoutPanel2.Controls.Add(portTextBox, 1, 3);
            tableLayoutPanel2.Controls.Add(useHttpsLabel, 0, 2);
            tableLayoutPanel2.Controls.Add(nameLabel, 0, 1);
            tableLayoutPanel2.Controls.Add(nameTextBox, 1, 1);
            tableLayoutPanel2.Controls.Add(keywordLabel, 0, 0);
            tableLayoutPanel2.Controls.Add(keywordTextBox, 1, 0);
            tableLayoutPanel2.Controls.Add(useHttpsCheckBox, 1, 2);
            tableLayoutPanel2.Dock = DockStyle.Bottom;
            tableLayoutPanel2.Location = new Point(20, 130);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 7;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.Size = new Size(453, 476);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // additionalPagesLabel
            // 
            additionalPagesLabel.AutoSize = true;
            additionalPagesLabel.Dock = DockStyle.Fill;
            additionalPagesLabel.Location = new Point(3, 300);
            additionalPagesLabel.Name = "additionalPagesLabel";
            additionalPagesLabel.Size = new Size(152, 176);
            additionalPagesLabel.TabIndex = 12;
            additionalPagesLabel.Text = "Additional Pages";
            // 
            // additionalPagesTextBox
            // 
            additionalPagesTextBox.Dock = DockStyle.Fill;
            additionalPagesTextBox.Location = new Point(161, 303);
            additionalPagesTextBox.Name = "additionalPagesTextBox";
            additionalPagesTextBox.Size = new Size(289, 23);
            additionalPagesTextBox.TabIndex = 13;
            // 
            // exePathLabel
            // 
            exePathLabel.AutoSize = true;
            exePathLabel.Dock = DockStyle.Fill;
            exePathLabel.Location = new Point(3, 250);
            exePathLabel.Name = "exePathLabel";
            exePathLabel.Size = new Size(152, 50);
            exePathLabel.TabIndex = 10;
            exePathLabel.Text = "Exe Path";
            // 
            // exePathTextBox
            // 
            exePathTextBox.Dock = DockStyle.Fill;
            exePathTextBox.Location = new Point(161, 253);
            exePathTextBox.Name = "exePathTextBox";
            exePathTextBox.Size = new Size(289, 23);
            exePathTextBox.TabIndex = 11;
            // 
            // remoteServerUrlLabel
            // 
            remoteServerUrlLabel.AutoSize = true;
            remoteServerUrlLabel.Dock = DockStyle.Fill;
            remoteServerUrlLabel.Location = new Point(3, 200);
            remoteServerUrlLabel.Name = "remoteServerUrlLabel";
            remoteServerUrlLabel.Size = new Size(152, 50);
            remoteServerUrlLabel.TabIndex = 8;
            remoteServerUrlLabel.Text = "Remote Server URL";
            // 
            // remoteServerUrlTextBox
            // 
            remoteServerUrlTextBox.Dock = DockStyle.Fill;
            remoteServerUrlTextBox.Location = new Point(161, 203);
            remoteServerUrlTextBox.Name = "remoteServerUrlTextBox";
            remoteServerUrlTextBox.Size = new Size(289, 23);
            remoteServerUrlTextBox.TabIndex = 9;
            // 
            // portLabel
            // 
            portLabel.AutoSize = true;
            portLabel.Dock = DockStyle.Fill;
            portLabel.Location = new Point(3, 150);
            portLabel.Name = "portLabel";
            portLabel.Size = new Size(152, 50);
            portLabel.TabIndex = 6;
            portLabel.Text = "Port";
            // 
            // portTextBox
            // 
            portTextBox.Dock = DockStyle.Fill;
            portTextBox.Location = new Point(161, 153);
            portTextBox.Name = "portTextBox";
            portTextBox.Size = new Size(289, 23);
            portTextBox.TabIndex = 7;
            // 
            // useHttpsLabel
            // 
            useHttpsLabel.AutoSize = true;
            useHttpsLabel.Dock = DockStyle.Fill;
            useHttpsLabel.Location = new Point(3, 100);
            useHttpsLabel.Name = "useHttpsLabel";
            useHttpsLabel.Size = new Size(152, 50);
            useHttpsLabel.TabIndex = 4;
            useHttpsLabel.Text = "Use HTTPS";
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Dock = DockStyle.Fill;
            nameLabel.Location = new Point(3, 50);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(152, 50);
            nameLabel.TabIndex = 2;
            nameLabel.Text = "Name";
            // 
            // nameTextBox
            // 
            nameTextBox.Dock = DockStyle.Fill;
            nameTextBox.Location = new Point(161, 53);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(289, 23);
            nameTextBox.TabIndex = 3;
            // 
            // keywordLabel
            // 
            keywordLabel.AutoSize = true;
            keywordLabel.Dock = DockStyle.Fill;
            keywordLabel.Location = new Point(3, 0);
            keywordLabel.Name = "keywordLabel";
            keywordLabel.Size = new Size(152, 50);
            keywordLabel.TabIndex = 0;
            keywordLabel.Text = "Keyword";
            // 
            // keywordTextBox
            // 
            keywordTextBox.Dock = DockStyle.Fill;
            keywordTextBox.Location = new Point(161, 3);
            keywordTextBox.Name = "keywordTextBox";
            keywordTextBox.Size = new Size(289, 23);
            keywordTextBox.TabIndex = 1;
            // 
            // useHttpsCheckBox
            // 
            useHttpsCheckBox.AutoSize = true;
            useHttpsCheckBox.Location = new Point(161, 103);
            useHttpsCheckBox.Name = "useHttpsCheckBox";
            useHttpsCheckBox.Size = new Size(15, 14);
            useHttpsCheckBox.TabIndex = 14;
            useHttpsCheckBox.UseVisualStyleBackColor = true;
            // 
            // toolBarMenuStrip
            // 
            toolBarMenuStrip.Dock = DockStyle.None;
            toolBarMenuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            toolBarMenuStrip.Location = new Point(0, 0);
            toolBarMenuStrip.Name = "toolBarMenuStrip";
            toolBarMenuStrip.Size = new Size(752, 24);
            toolBarMenuStrip.TabIndex = 2;
            toolBarMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, toolStripSeparator, saveToolStripMenuItem, saveAsToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Image = (Image)resources.GetObject("newToolStripMenuItem.Image");
            newToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newToolStripMenuItem.Size = new Size(146, 22);
            newToolStripMenuItem.Text = "&New";
            newToolStripMenuItem.Click += NewMenuItemOnClick;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Image = (Image)resources.GetObject("openToolStripMenuItem.Image");
            openToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openToolStripMenuItem.Size = new Size(146, 22);
            openToolStripMenuItem.Text = "&Open";
            openToolStripMenuItem.Click += OpenMenuItemOnClick;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new Size(143, 6);
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Image = (Image)resources.GetObject("saveToolStripMenuItem.Image");
            saveToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(146, 22);
            saveToolStripMenuItem.Text = "&Save";
            saveToolStripMenuItem.Click += SaveMenuItemOnClick;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(146, 22);
            saveAsToolStripMenuItem.Text = "Save &As";
            saveAsToolStripMenuItem.Click += SaveAsMenuItemOnClick;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(146, 22);
            exitToolStripMenuItem.Text = "E&xit";
            // 
            // statusStrip1
            // 
            statusStrip1.Dock = DockStyle.None;
            statusStrip1.Items.AddRange(new ToolStripItem[] { statusBarLabel });
            statusStrip1.Location = new Point(0, 0);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(752, 22);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // statusBarLabel
            // 
            statusBarLabel.Name = "statusBarLabel";
            statusBarLabel.Size = new Size(0, 17);
            // 
            // mainContentToolStripContainer
            // 
            // 
            // mainContentToolStripContainer.BottomToolStripPanel
            // 
            mainContentToolStripContainer.BottomToolStripPanel.Controls.Add(statusStrip1);
            // 
            // mainContentToolStripContainer.ContentPanel
            // 
            mainContentToolStripContainer.ContentPanel.Controls.Add(mainContentSplitContainer);
            mainContentToolStripContainer.ContentPanel.Size = new Size(752, 606);
            mainContentToolStripContainer.Dock = DockStyle.Fill;
            mainContentToolStripContainer.Location = new Point(0, 0);
            mainContentToolStripContainer.Name = "mainContentToolStripContainer";
            mainContentToolStripContainer.Size = new Size(752, 652);
            mainContentToolStripContainer.TabIndex = 4;
            mainContentToolStripContainer.Text = "toolStripContainer1";
            // 
            // mainContentToolStripContainer.TopToolStripPanel
            // 
            mainContentToolStripContainer.TopToolStripPanel.Controls.Add(toolBarMenuStrip);
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(752, 652);
            Controls.Add(mainContentToolStripContainer);
            MainMenuStrip = toolBarMenuStrip;
            Name = "MainWindow";
            Text = "Form1";
            mainContentSplitContainer.Panel1.ResumeLayout(false);
            mainContentSplitContainer.Panel2.ResumeLayout(false);
            mainContentSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)mainContentSplitContainer).EndInit();
            mainContentSplitContainer.ResumeLayout(false);
            toolListToolStripContainer.ContentPanel.ResumeLayout(false);
            toolListToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            toolListToolStripContainer.TopToolStripPanel.PerformLayout();
            toolListToolStripContainer.ResumeLayout(false);
            toolListToolStripContainer.PerformLayout();
            toolListMenuStrip.ResumeLayout(false);
            toolListMenuStrip.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            toolBarMenuStrip.ResumeLayout(false);
            toolBarMenuStrip.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            mainContentToolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            mainContentToolStripContainer.BottomToolStripPanel.PerformLayout();
            mainContentToolStripContainer.ContentPanel.ResumeLayout(false);
            mainContentToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            mainContentToolStripContainer.TopToolStripPanel.PerformLayout();
            mainContentToolStripContainer.ResumeLayout(false);
            mainContentToolStripContainer.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer mainContentSplitContainer;
        private ListBox toolsListBox;
        private TableLayoutPanel tableLayoutPanel2;
        private Label additionalPagesLabel;
        private TextBox additionalPagesTextBox;
        private Label exePathLabel;
        private TextBox exePathTextBox;
        private Label remoteServerUrlLabel;
        private TextBox remoteServerUrlTextBox;
        private Label portLabel;
        private TextBox portTextBox;
        private Label useHttpsLabel;
        private Label nameLabel;
        private TextBox nameTextBox;
        private Label keywordLabel;
        private TextBox keywordTextBox;
        private Label titleLabel;
        private MenuStrip toolBarMenuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private CheckBox useHttpsCheckBox;
        private StatusStrip statusStrip1;
        private ToolStripContainer mainContentToolStripContainer;
        private ToolStripStatusLabel statusBarLabel;
        private ToolStripContainer toolListToolStripContainer;
        private MenuStrip toolListMenuStrip;
        private ToolStripMenuItem addToolMenuItem;
        private ToolStripMenuItem copyToolMenuItem;
        private ToolStripMenuItem deleteToolMenuItem;
    }
}
