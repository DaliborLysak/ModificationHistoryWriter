namespace ModificationHistoryWriterForm
{
    partial class ModificationHistoryWriterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModificationHistoryWriterForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonFormat = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCopyToClipboard = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonShowPattern = new System.Windows.Forms.ToolStripButton();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.richTextBoxInput = new System.Windows.Forms.RichTextBox();
            this.listBoxFiles = new System.Windows.Forms.ListBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonFormat,
            this.toolStripButtonCopyToClipboard,
            this.toolStripSeparator1,
            this.toolStripButtonClear,
            this.toolStripSeparator2,
            this.toolStripButtonSave,
            this.toolStripSeparator3,
            this.toolStripButtonShowPattern});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStripActions";
            // 
            // toolStripButtonFormat
            // 
            this.toolStripButtonFormat.AutoSize = false;
            this.toolStripButtonFormat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFormat.Image = global::ModificationHistoryWriterForm.Properties.Resources.Arrow_Right;
            this.toolStripButtonFormat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFormat.Name = "toolStripButtonFormat";
            this.toolStripButtonFormat.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFormat.Text = "Format";
            this.toolStripButtonFormat.Click += new System.EventHandler(this.toolStripButtonFormat_Click);
            // 
            // toolStripButtonCopyToClipboard
            // 
            this.toolStripButtonCopyToClipboard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCopyToClipboard.Image = global::ModificationHistoryWriterForm.Properties.Resources.Basket;
            this.toolStripButtonCopyToClipboard.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCopyToClipboard.Name = "toolStripButtonCopyToClipboard";
            this.toolStripButtonCopyToClipboard.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonCopyToClipboard.Text = "Copy To Clipboard";
            this.toolStripButtonCopyToClipboard.Click += new System.EventHandler(this.toolStripButtonCopyToClipboard_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonClear
            // 
            this.toolStripButtonClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonClear.Image = global::ModificationHistoryWriterForm.Properties.Resources.Delete;
            this.toolStripButtonClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClear.Name = "toolStripButtonClear";
            this.toolStripButtonClear.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonClear.Text = "Clear";
            this.toolStripButtonClear.Click += new System.EventHandler(this.toolStripButtonClear_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = global::ModificationHistoryWriterForm.Properties.Resources.Save;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = "Save";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonShowPattern
            // 
            this.toolStripButtonShowPattern.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonShowPattern.Image = global::ModificationHistoryWriterForm.Properties.Resources.Contact_Card;
            this.toolStripButtonShowPattern.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowPattern.Name = "toolStripButtonShowPattern";
            this.toolStripButtonShowPattern.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonShowPattern.Text = "Show Pattern";
            this.toolStripButtonShowPattern.ToolTipText = "Show Pattern";
            this.toolStripButtonShowPattern.Click += new System.EventHandler(this.toolStripButtonShowPattern_Click);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 25);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.richTextBoxInput);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.listBoxFiles);
            this.splitContainerMain.Size = new System.Drawing.Size(800, 425);
            this.splitContainerMain.SplitterDistance = 106;
            this.splitContainerMain.TabIndex = 1;
            // 
            // richTextBoxInput
            // 
            this.richTextBoxInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxInput.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxInput.Name = "richTextBoxInput";
            this.richTextBoxInput.Size = new System.Drawing.Size(800, 106);
            this.richTextBoxInput.TabIndex = 0;
            this.richTextBoxInput.Text = "";
            // 
            // listBoxFiles
            // 
            this.listBoxFiles.AllowDrop = true;
            this.listBoxFiles.BackColor = System.Drawing.SystemColors.Info;
            this.listBoxFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxFiles.FormattingEnabled = true;
            this.listBoxFiles.ItemHeight = 15;
            this.listBoxFiles.Location = new System.Drawing.Point(0, 0);
            this.listBoxFiles.Name = "listBoxFiles";
            this.listBoxFiles.Size = new System.Drawing.Size(800, 315);
            this.listBoxFiles.TabIndex = 0;
            this.listBoxFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBoxFiles_DragDrop);
            this.listBoxFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBoxFiles_DragEnter);
            // 
            // ModificationHistoryWriterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ModificationHistoryWriterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modification History Writer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModificationHistoryWriterForm_FormClosing);
            this.Shown += new System.EventHandler(this.ModificationHistoryWriterForm_Shown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ToolStrip toolStrip1;
        private SplitContainer splitContainerMain;
        private RichTextBox richTextBoxInput;
        private ListBox listBoxFiles;
        private ToolStripButton toolStripButtonFormat;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripButtonShowPattern;
        private ToolStripButton toolStripButtonClear;
        private ToolStripButton toolStripButtonCopyToClipboard;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton toolStripButtonSave;
        private ToolStripSeparator toolStripSeparator3;
    }
}