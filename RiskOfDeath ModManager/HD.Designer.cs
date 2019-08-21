namespace RiskOfDeath_ModManager
{
    partial class HD
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
            this.specialCasePanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.addSCBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.folderPanel = new System.Windows.Forms.Panel();
            this.addFolderBtn = new System.Windows.Forms.Button();
            this.filePanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.addFileBtn = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uuidTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // specialCasePanel
            // 
            this.specialCasePanel.AutoScroll = true;
            this.specialCasePanel.Location = new System.Drawing.Point(12, 47);
            this.specialCasePanel.Name = "specialCasePanel";
            this.specialCasePanel.Size = new System.Drawing.Size(186, 405);
            this.specialCasePanel.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Special Case Rules";
            // 
            // addSCBtn
            // 
            this.addSCBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addSCBtn.Location = new System.Drawing.Point(12, 458);
            this.addSCBtn.Name = "addSCBtn";
            this.addSCBtn.Size = new System.Drawing.Size(186, 30);
            this.addSCBtn.TabIndex = 10;
            this.addSCBtn.Text = "Add Special Case";
            this.addSCBtn.UseVisualStyleBackColor = true;
            this.addSCBtn.Click += new System.EventHandler(this.AddSC_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(222, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 20);
            this.label4.TabIndex = 16;
            this.label4.Text = "Folder Rules";
            // 
            // folderPanel
            // 
            this.folderPanel.AutoScroll = true;
            this.folderPanel.Location = new System.Drawing.Point(222, 47);
            this.folderPanel.Name = "folderPanel";
            this.folderPanel.Size = new System.Drawing.Size(208, 405);
            this.folderPanel.TabIndex = 17;
            // 
            // addFolderBtn
            // 
            this.addFolderBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addFolderBtn.Location = new System.Drawing.Point(222, 458);
            this.addFolderBtn.Name = "addFolderBtn";
            this.addFolderBtn.Size = new System.Drawing.Size(208, 30);
            this.addFolderBtn.TabIndex = 18;
            this.addFolderBtn.Text = "Add Folder";
            this.addFolderBtn.UseVisualStyleBackColor = true;
            this.addFolderBtn.Click += new System.EventHandler(this.AddFolder_Click);
            // 
            // filePanel
            // 
            this.filePanel.AutoScroll = true;
            this.filePanel.Location = new System.Drawing.Point(454, 47);
            this.filePanel.Name = "filePanel";
            this.filePanel.Size = new System.Drawing.Size(208, 405);
            this.filePanel.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(454, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 18;
            this.label2.Text = "File Rules";
            // 
            // addFileBtn
            // 
            this.addFileBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addFileBtn.Location = new System.Drawing.Point(454, 458);
            this.addFileBtn.Name = "addFileBtn";
            this.addFileBtn.Size = new System.Drawing.Size(208, 30);
            this.addFileBtn.TabIndex = 20;
            this.addFileBtn.Text = "Add File";
            this.addFileBtn.UseVisualStyleBackColor = true;
            this.addFileBtn.Click += new System.EventHandler(this.AddFile_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.importToolStripMenuItem,
            this.clearAllToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(917, 24);
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.ImportToolStripMenuItem_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.ClearAllToolStripMenuItem_Click);
            // 
            // uuidTextBox
            // 
            this.uuidTextBox.AcceptsReturn = true;
            this.uuidTextBox.Location = new System.Drawing.Point(668, 47);
            this.uuidTextBox.Multiline = true;
            this.uuidTextBox.Name = "uuidTextBox";
            this.uuidTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.uuidTextBox.Size = new System.Drawing.Size(234, 441);
            this.uuidTextBox.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(664, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 20);
            this.label3.TabIndex = 30;
            this.label3.Text = "Excluded UUIDs";
            // 
            // HD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 498);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.uuidTextBox);
            this.Controls.Add(this.addFileBtn);
            this.Controls.Add(this.filePanel);
            this.Controls.Add(this.addFolderBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.folderPanel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.addSCBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.specialCasePanel);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HD";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "HD";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel specialCasePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button addSCBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel folderPanel;
        private System.Windows.Forms.Button addFolderBtn;
        private System.Windows.Forms.Panel filePanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button addFileBtn;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.TextBox uuidTextBox;
        private System.Windows.Forms.Label label3;
    }
}