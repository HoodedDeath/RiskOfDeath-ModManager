namespace RiskOfDeath_ModManager
{
    partial class HDSCEdit
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
            this.filePanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.folderPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.uuidTextBox = new System.Windows.Forms.TextBox();
            this.acceptBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.removeBtn = new System.Windows.Forms.Button();
            this.addFolderBtn = new System.Windows.Forms.Button();
            this.addFileBtn = new System.Windows.Forms.Button();
            this.pickOneCheckBox = new System.Windows.Forms.CheckBox();
            this.pickOptionPanel = new System.Windows.Forms.Panel();
            this.addOptionBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // filePanel
            // 
            this.filePanel.AutoScroll = true;
            this.filePanel.Location = new System.Drawing.Point(244, 64);
            this.filePanel.Name = "filePanel";
            this.filePanel.Size = new System.Drawing.Size(208, 405);
            this.filePanel.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(244, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 22;
            this.label2.Text = "File Rules";
            // 
            // folderPanel
            // 
            this.folderPanel.AutoScroll = true;
            this.folderPanel.Location = new System.Drawing.Point(12, 64);
            this.folderPanel.Name = "folderPanel";
            this.folderPanel.Size = new System.Drawing.Size(208, 405);
            this.folderPanel.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 20);
            this.label4.TabIndex = 20;
            this.label4.Text = "Folder Rules";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.Location = new System.Drawing.Point(12, 12);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(92, 20);
            this.nameLabel.TabIndex = 24;
            this.nameLabel.Text = "Rule Name:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(110, 12);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(110, 20);
            this.nameTextBox.TabIndex = 25;
            this.nameTextBox.Text = "New Rule";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(321, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 20);
            this.label1.TabIndex = 26;
            this.label1.Text = "Matching UUIDs:";
            // 
            // uuidTextBox
            // 
            this.uuidTextBox.AcceptsReturn = true;
            this.uuidTextBox.Location = new System.Drawing.Point(476, 14);
            this.uuidTextBox.Multiline = true;
            this.uuidTextBox.Name = "uuidTextBox";
            this.uuidTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.uuidTextBox.Size = new System.Drawing.Size(234, 120);
            this.uuidTextBox.TabIndex = 28;
            // 
            // acceptBtn
            // 
            this.acceptBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.acceptBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acceptBtn.Location = new System.Drawing.Point(599, 439);
            this.acceptBtn.Name = "acceptBtn";
            this.acceptBtn.Size = new System.Drawing.Size(111, 30);
            this.acceptBtn.TabIndex = 29;
            this.acceptBtn.Text = "Save";
            this.acceptBtn.UseVisualStyleBackColor = true;
            this.acceptBtn.Click += new System.EventHandler(this.AcceptBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelBtn.Location = new System.Drawing.Point(476, 439);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(111, 30);
            this.cancelBtn.TabIndex = 30;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // removeBtn
            // 
            this.removeBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeBtn.Location = new System.Drawing.Point(476, 475);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new System.Drawing.Size(234, 30);
            this.removeBtn.TabIndex = 31;
            this.removeBtn.Text = "Remove";
            this.removeBtn.UseVisualStyleBackColor = true;
            this.removeBtn.Click += new System.EventHandler(this.RemoveBtn_Click);
            // 
            // addFolderBtn
            // 
            this.addFolderBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addFolderBtn.Location = new System.Drawing.Point(12, 475);
            this.addFolderBtn.Name = "addFolderBtn";
            this.addFolderBtn.Size = new System.Drawing.Size(208, 30);
            this.addFolderBtn.TabIndex = 32;
            this.addFolderBtn.Text = "Add";
            this.addFolderBtn.UseVisualStyleBackColor = true;
            this.addFolderBtn.Click += new System.EventHandler(this.AddFolderBtn_Click);
            // 
            // addFileBtn
            // 
            this.addFileBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addFileBtn.Location = new System.Drawing.Point(244, 475);
            this.addFileBtn.Name = "addFileBtn";
            this.addFileBtn.Size = new System.Drawing.Size(208, 30);
            this.addFileBtn.TabIndex = 33;
            this.addFileBtn.Text = "Add";
            this.addFileBtn.UseVisualStyleBackColor = true;
            this.addFileBtn.Click += new System.EventHandler(this.AddFileBtn_Click);
            // 
            // pickOneCheckBox
            // 
            this.pickOneCheckBox.AutoSize = true;
            this.pickOneCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pickOneCheckBox.Location = new System.Drawing.Point(476, 140);
            this.pickOneCheckBox.Name = "pickOneCheckBox";
            this.pickOneCheckBox.Size = new System.Drawing.Size(180, 24);
            this.pickOneCheckBox.TabIndex = 34;
            this.pickOneCheckBox.Text = "Pick one out of X files";
            this.pickOneCheckBox.UseVisualStyleBackColor = true;
            this.pickOneCheckBox.CheckedChanged += new System.EventHandler(this.Pick_CheckChanged);
            // 
            // pickOptionPanel
            // 
            this.pickOptionPanel.AutoScroll = true;
            this.pickOptionPanel.Location = new System.Drawing.Point(476, 170);
            this.pickOptionPanel.Name = "pickOptionPanel";
            this.pickOptionPanel.Size = new System.Drawing.Size(204, 227);
            this.pickOptionPanel.TabIndex = 35;
            // 
            // addOptionBtn
            // 
            this.addOptionBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addOptionBtn.Location = new System.Drawing.Point(476, 403);
            this.addOptionBtn.Name = "addOptionBtn";
            this.addOptionBtn.Size = new System.Drawing.Size(204, 30);
            this.addOptionBtn.TabIndex = 44;
            this.addOptionBtn.Text = "Add Option";
            this.addOptionBtn.UseVisualStyleBackColor = true;
            this.addOptionBtn.Click += new System.EventHandler(this.AddOption_Click);
            // 
            // HDSCEdit
            // 
            this.AcceptButton = this.acceptBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(720, 517);
            this.Controls.Add(this.addOptionBtn);
            this.Controls.Add(this.pickOptionPanel);
            this.Controls.Add(this.pickOneCheckBox);
            this.Controls.Add(this.addFileBtn);
            this.Controls.Add(this.addFolderBtn);
            this.Controls.Add(this.removeBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.acceptBtn);
            this.Controls.Add(this.uuidTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.filePanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.folderPanel);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HDSCEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "HDSCEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel filePanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel folderPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox uuidTextBox;
        private System.Windows.Forms.Button acceptBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button removeBtn;
        private System.Windows.Forms.Button addFolderBtn;
        private System.Windows.Forms.Button addFileBtn;
        private System.Windows.Forms.CheckBox pickOneCheckBox;
        private System.Windows.Forms.Panel pickOptionPanel;
        private System.Windows.Forms.Button addOptionBtn;
    }
}