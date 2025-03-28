namespace ToolConfigEditor
{
    partial class EditListBox
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
            tableLayoutPanel1 = new TableLayoutPanel();
            newEntryTextBox = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            addEntryButton = new Button();
            removeEntryButton = new Button();
            entryListBox = new ListBox();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.Controls.Add(newEntryTextBox, 0, 0);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 1, 0);
            tableLayoutPanel1.Controls.Add(entryListBox, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(200, 100);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // newEntryTextBox
            // 
            newEntryTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            newEntryTextBox.Location = new Point(3, 8);
            newEntryTextBox.Name = "newEntryTextBox";
            newEntryTextBox.Size = new Size(94, 23);
            newEntryTextBox.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Controls.Add(addEntryButton);
            flowLayoutPanel1.Controls.Add(removeEntryButton);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(103, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(94, 34);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // addEntryButton
            // 
            addEntryButton.AutoSize = true;
            addEntryButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            addEntryButton.Location = new Point(3, 3);
            addEntryButton.Name = "addEntryButton";
            addEntryButton.Size = new Size(25, 25);
            addEntryButton.TabIndex = 0;
            addEntryButton.Text = "+";
            addEntryButton.UseVisualStyleBackColor = true;
            // 
            // removeEntryButton
            // 
            removeEntryButton.AutoSize = true;
            removeEntryButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            removeEntryButton.Location = new Point(34, 3);
            removeEntryButton.Name = "removeEntryButton";
            removeEntryButton.Size = new Size(22, 25);
            removeEntryButton.TabIndex = 1;
            removeEntryButton.Text = "-";
            removeEntryButton.UseVisualStyleBackColor = true;
            // 
            // entryListBox
            // 
            tableLayoutPanel1.SetColumnSpan(entryListBox, 2);
            entryListBox.Dock = DockStyle.Fill;
            entryListBox.FormattingEnabled = true;
            entryListBox.Location = new Point(3, 43);
            entryListBox.Name = "entryListBox";
            entryListBox.Size = new Size(194, 326);
            entryListBox.TabIndex = 2;
            // 
            // EditListBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(tableLayoutPanel1);
            MinimumSize = new Size(200, 100);
            Name = "EditListBox";
            Size = new Size(200, 100);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TextBox newEntryTextBox;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button addEntryButton;
        private Button removeEntryButton;
        private ListBox entryListBox;
    }
}
