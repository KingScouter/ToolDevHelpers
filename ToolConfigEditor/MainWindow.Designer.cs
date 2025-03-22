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
            splitContainer1 = new SplitContainer();
            toolsListBox = new ListBox();
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
            textBox12 = new TextBox();
            nameLabel = new Label();
            nameTextBox = new TextBox();
            keywordLabel = new Label();
            keywordTextBox = new TextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(toolsListBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(titleLabel);
            splitContainer1.Panel2.Controls.Add(tableLayoutPanel2);
            splitContainer1.Panel2.Padding = new Padding(20, 20, 20, 0);
            splitContainer1.Size = new Size(752, 652);
            splitContainer1.SplitterDistance = 255;
            splitContainer1.TabIndex = 1;
            // 
            // toolsListBox
            // 
            toolsListBox.Dock = DockStyle.Fill;
            toolsListBox.FormattingEnabled = true;
            toolsListBox.Location = new Point(0, 0);
            toolsListBox.Name = "toolsListBox";
            toolsListBox.Size = new Size(255, 652);
            toolsListBox.TabIndex = 0;
            toolsListBox.SelectedValueChanged += ToolsListBoxOnSelectedValueChanged;
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
            tableLayoutPanel2.Controls.Add(additionalPagesTextBox, 0, 6);
            tableLayoutPanel2.Controls.Add(exePathLabel, 0, 5);
            tableLayoutPanel2.Controls.Add(exePathTextBox, 0, 5);
            tableLayoutPanel2.Controls.Add(remoteServerUrlLabel, 0, 4);
            tableLayoutPanel2.Controls.Add(remoteServerUrlTextBox, 1, 4);
            tableLayoutPanel2.Controls.Add(portLabel, 0, 3);
            tableLayoutPanel2.Controls.Add(portTextBox, 1, 3);
            tableLayoutPanel2.Controls.Add(useHttpsLabel, 0, 2);
            tableLayoutPanel2.Controls.Add(textBox12, 1, 2);
            tableLayoutPanel2.Controls.Add(nameLabel, 0, 1);
            tableLayoutPanel2.Controls.Add(nameTextBox, 1, 1);
            tableLayoutPanel2.Controls.Add(keywordLabel, 0, 0);
            tableLayoutPanel2.Controls.Add(keywordTextBox, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Bottom;
            tableLayoutPanel2.Location = new Point(20, 176);
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
            // textBox12
            // 
            textBox12.Dock = DockStyle.Fill;
            textBox12.Location = new Point(161, 103);
            textBox12.Name = "textBox12";
            textBox12.Size = new Size(289, 23);
            textBox12.TabIndex = 5;
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
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(752, 652);
            Controls.Add(splitContainer1);
            Name = "MainWindow";
            Text = "Form1";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
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
        private TextBox textBox12;
        private Label nameLabel;
        private TextBox nameTextBox;
        private Label keywordLabel;
        private TextBox keywordTextBox;
        private Label titleLabel;
    }
}
