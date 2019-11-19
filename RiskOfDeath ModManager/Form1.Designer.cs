namespace RiskOfDeath_ModManager
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.availablePanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.activePanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.protocolBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.sideDockPanel = new System.Windows.Forms.Panel();
            this.launchPicBox = new System.Windows.Forms.PictureBox();
            this.exitPicBox = new System.Windows.Forms.PictureBox();
            this.panelPicBox = new System.Windows.Forms.PictureBox();
            this.topDockPanel = new System.Windows.Forms.Panel();
            this.profEditBtn = new System.Windows.Forms.Button();
            this.profNumModsLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.profNameLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.sidePanel = new System.Windows.Forms.Panel();
            this.logoPicBox = new System.Windows.Forms.PictureBox();
            this.addProfPicBox = new System.Windows.Forms.PictureBox();
            this.profsPanelInPanel = new System.Windows.Forms.Panel();
            this.panelPicBoxInPanel = new System.Windows.Forms.PictureBox();
            this.rulesBtn = new System.Windows.Forms.Button();
            this.urlBtn = new System.Windows.Forms.Button();
            this.launchBtn = new System.Windows.Forms.Button();
            this.helpBtn = new System.Windows.Forms.Button();
            this.exitBtn = new System.Windows.Forms.Button();
            this.panelBGWorker = new System.ComponentModel.BackgroundWorker();
            this.sideDockPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.launchPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelPicBox)).BeginInit();
            this.topDockPanel.SuspendLayout();
            this.sidePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.addProfPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelPicBoxInPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // availablePanel
            // 
            this.availablePanel.AutoScroll = true;
            this.availablePanel.BackColor = System.Drawing.Color.Transparent;
            this.availablePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.availablePanel.Location = new System.Drawing.Point(62, 61);
            this.availablePanel.Name = "availablePanel";
            this.availablePanel.Size = new System.Drawing.Size(336, 423);
            this.availablePanel.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(59, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Available Mods";
            // 
            // activePanel
            // 
            this.activePanel.AutoScroll = true;
            this.activePanel.BackColor = System.Drawing.Color.Transparent;
            this.activePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.activePanel.Location = new System.Drawing.Point(404, 61);
            this.activePanel.Name = "activePanel";
            this.activePanel.Size = new System.Drawing.Size(336, 423);
            this.activePanel.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(401, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Active Mods";
            // 
            // protocolBackgroundWorker
            // 
            this.protocolBackgroundWorker.WorkerSupportsCancellation = true;
            this.protocolBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ProtocolBackgroundWorker_DoWork);
            this.protocolBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ProtocolBackgroundWorker_RunWorkerCompleted);
            // 
            // sideDockPanel
            // 
            this.sideDockPanel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.sideDockPanel.Controls.Add(this.launchPicBox);
            this.sideDockPanel.Controls.Add(this.exitPicBox);
            this.sideDockPanel.Controls.Add(this.panelPicBox);
            this.sideDockPanel.Location = new System.Drawing.Point(0, 0);
            this.sideDockPanel.Name = "sideDockPanel";
            this.sideDockPanel.Size = new System.Drawing.Size(56, 487);
            this.sideDockPanel.TabIndex = 10;
            // 
            // launchPicBox
            // 
            this.launchPicBox.BackColor = System.Drawing.Color.Transparent;
            this.launchPicBox.BackgroundImage = global::RiskOfDeath_ModManager.Properties.Resources.launch_btn;
            this.launchPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.launchPicBox.Location = new System.Drawing.Point(3, 378);
            this.launchPicBox.Name = "launchPicBox";
            this.launchPicBox.Size = new System.Drawing.Size(50, 50);
            this.launchPicBox.TabIndex = 13;
            this.launchPicBox.TabStop = false;
            this.launchPicBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launch_MouseDown);
            // 
            // exitPicBox
            // 
            this.exitPicBox.BackColor = System.Drawing.Color.Transparent;
            this.exitPicBox.BackgroundImage = global::RiskOfDeath_ModManager.Properties.Resources.exit_btn;
            this.exitPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.exitPicBox.Location = new System.Drawing.Point(3, 434);
            this.exitPicBox.Name = "exitPicBox";
            this.exitPicBox.Size = new System.Drawing.Size(50, 50);
            this.exitPicBox.TabIndex = 12;
            this.exitPicBox.TabStop = false;
            this.exitPicBox.Click += new System.EventHandler(this.Exit_Click);
            // 
            // panelPicBox
            // 
            this.panelPicBox.BackColor = System.Drawing.Color.Transparent;
            this.panelPicBox.BackgroundImage = global::RiskOfDeath_ModManager.Properties.Resources.menu_btn;
            this.panelPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelPicBox.Location = new System.Drawing.Point(3, 3);
            this.panelPicBox.Name = "panelPicBox";
            this.panelPicBox.Size = new System.Drawing.Size(50, 50);
            this.panelPicBox.TabIndex = 11;
            this.panelPicBox.TabStop = false;
            this.panelPicBox.Click += new System.EventHandler(this.PanelBtns_Click);
            // 
            // topDockPanel
            // 
            this.topDockPanel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.topDockPanel.Controls.Add(this.profEditBtn);
            this.topDockPanel.Controls.Add(this.profNumModsLabel);
            this.topDockPanel.Controls.Add(this.label4);
            this.topDockPanel.Controls.Add(this.profNameLabel);
            this.topDockPanel.Controls.Add(this.label5);
            this.topDockPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.topDockPanel.Location = new System.Drawing.Point(56, 0);
            this.topDockPanel.Name = "topDockPanel";
            this.topDockPanel.Size = new System.Drawing.Size(698, 35);
            this.topDockPanel.TabIndex = 11;
            // 
            // profEditBtn
            // 
            this.profEditBtn.AutoSize = true;
            this.profEditBtn.Location = new System.Drawing.Point(600, 2);
            this.profEditBtn.Name = "profEditBtn";
            this.profEditBtn.Size = new System.Drawing.Size(95, 30);
            this.profEditBtn.TabIndex = 4;
            this.profEditBtn.Text = "Edit Profile";
            this.profEditBtn.UseVisualStyleBackColor = true;
            this.profEditBtn.Click += new System.EventHandler(this.EditProfile_Click);
            // 
            // profNumModsLabel
            // 
            this.profNumModsLabel.AutoSize = true;
            this.profNumModsLabel.Location = new System.Drawing.Point(511, 9);
            this.profNumModsLabel.Name = "profNumModsLabel";
            this.profNumModsLabel.Size = new System.Drawing.Size(27, 20);
            this.profNumModsLabel.TabIndex = 3;
            this.profNumModsLabel.Text = "##";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(375, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Number of Mods:";
            // 
            // profNameLabel
            // 
            this.profNameLabel.AutoSize = true;
            this.profNameLabel.Location = new System.Drawing.Point(69, 9);
            this.profNameLabel.Name = "profNameLabel";
            this.profNameLabel.Size = new System.Drawing.Size(151, 20);
            this.profNameLabel.TabIndex = 1;
            this.profNameLabel.Text = "#PROFILE_NAME#";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "Profile:";
            // 
            // sidePanel
            // 
            this.sidePanel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.sidePanel.Controls.Add(this.logoPicBox);
            this.sidePanel.Controls.Add(this.addProfPicBox);
            this.sidePanel.Controls.Add(this.profsPanelInPanel);
            this.sidePanel.Controls.Add(this.panelPicBoxInPanel);
            this.sidePanel.Controls.Add(this.rulesBtn);
            this.sidePanel.Controls.Add(this.urlBtn);
            this.sidePanel.Controls.Add(this.launchBtn);
            this.sidePanel.Controls.Add(this.helpBtn);
            this.sidePanel.Controls.Add(this.exitBtn);
            this.sidePanel.Location = new System.Drawing.Point(779, 0);
            this.sidePanel.Name = "sidePanel";
            this.sidePanel.Size = new System.Drawing.Size(332, 487);
            this.sidePanel.TabIndex = 12;
            // 
            // logoPicBox
            // 
            this.logoPicBox.BackColor = System.Drawing.Color.Transparent;
            this.logoPicBox.BackgroundImage = global::RiskOfDeath_ModManager.Properties.Resources.rodmm_logo;
            this.logoPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.logoPicBox.Location = new System.Drawing.Point(3, 115);
            this.logoPicBox.Name = "logoPicBox";
            this.logoPicBox.Size = new System.Drawing.Size(50, 225);
            this.logoPicBox.TabIndex = 4;
            this.logoPicBox.TabStop = false;
            // 
            // addProfPicBox
            // 
            this.addProfPicBox.BackColor = System.Drawing.Color.Transparent;
            this.addProfPicBox.BackgroundImage = global::RiskOfDeath_ModManager.Properties.Resources.add_btn;
            this.addProfPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.addProfPicBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.addProfPicBox.Location = new System.Drawing.Point(3, 59);
            this.addProfPicBox.Name = "addProfPicBox";
            this.addProfPicBox.Size = new System.Drawing.Size(50, 50);
            this.addProfPicBox.TabIndex = 10;
            this.addProfPicBox.TabStop = false;
            this.addProfPicBox.Click += new System.EventHandler(this.AddProfile_Click);
            // 
            // profsPanelInPanel
            // 
            this.profsPanelInPanel.AutoScroll = true;
            this.profsPanelInPanel.Location = new System.Drawing.Point(59, 3);
            this.profsPanelInPanel.Name = "profsPanelInPanel";
            this.profsPanelInPanel.Size = new System.Drawing.Size(270, 363);
            this.profsPanelInPanel.TabIndex = 9;
            // 
            // panelPicBoxInPanel
            // 
            this.panelPicBoxInPanel.BackColor = System.Drawing.Color.Transparent;
            this.panelPicBoxInPanel.BackgroundImage = global::RiskOfDeath_ModManager.Properties.Resources.menu_btn;
            this.panelPicBoxInPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelPicBoxInPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelPicBoxInPanel.Location = new System.Drawing.Point(3, 3);
            this.panelPicBoxInPanel.Name = "panelPicBoxInPanel";
            this.panelPicBoxInPanel.Size = new System.Drawing.Size(50, 50);
            this.panelPicBoxInPanel.TabIndex = 6;
            this.panelPicBoxInPanel.TabStop = false;
            this.panelPicBoxInPanel.Click += new System.EventHandler(this.PanelBtns_Click);
            // 
            // rulesBtn
            // 
            this.rulesBtn.BackColor = System.Drawing.SystemColors.Control;
            this.rulesBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rulesBtn.Location = new System.Drawing.Point(166, 373);
            this.rulesBtn.Name = "rulesBtn";
            this.rulesBtn.Size = new System.Drawing.Size(163, 23);
            this.rulesBtn.TabIndex = 8;
            this.rulesBtn.Text = "Rule Setup For Devs";
            this.rulesBtn.UseVisualStyleBackColor = false;
            this.rulesBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RuleSetup_MouseDown);
            // 
            // urlBtn
            // 
            this.urlBtn.BackColor = System.Drawing.SystemColors.Control;
            this.urlBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.urlBtn.Location = new System.Drawing.Point(3, 373);
            this.urlBtn.Name = "urlBtn";
            this.urlBtn.Size = new System.Drawing.Size(163, 23);
            this.urlBtn.TabIndex = 8;
            this.urlBtn.Text = "Link URL Protocol";
            this.urlBtn.UseVisualStyleBackColor = false;
            this.urlBtn.Click += new System.EventHandler(this.LinkManagerURL_Click);
            // 
            // launchBtn
            // 
            this.launchBtn.BackColor = System.Drawing.SystemColors.Control;
            this.launchBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.launchBtn.Location = new System.Drawing.Point(3, 427);
            this.launchBtn.Name = "launchBtn";
            this.launchBtn.Size = new System.Drawing.Size(326, 23);
            this.launchBtn.TabIndex = 7;
            this.launchBtn.Text = "Launch RoR2";
            this.launchBtn.UseVisualStyleBackColor = false;
            this.launchBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launch_MouseDown);
            // 
            // helpBtn
            // 
            this.helpBtn.BackColor = System.Drawing.SystemColors.Control;
            this.helpBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpBtn.Location = new System.Drawing.Point(3, 402);
            this.helpBtn.Name = "helpBtn";
            this.helpBtn.Size = new System.Drawing.Size(326, 23);
            this.helpBtn.TabIndex = 4;
            this.helpBtn.Text = "Help";
            this.helpBtn.UseVisualStyleBackColor = false;
            this.helpBtn.Click += new System.EventHandler(this.Help_Click);
            // 
            // exitBtn
            // 
            this.exitBtn.BackColor = System.Drawing.SystemColors.Control;
            this.exitBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitBtn.Location = new System.Drawing.Point(3, 454);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(326, 23);
            this.exitBtn.TabIndex = 1;
            this.exitBtn.Text = "Exit";
            this.exitBtn.UseVisualStyleBackColor = false;
            this.exitBtn.Click += new System.EventHandler(this.Exit_Click);
            // 
            // panelBGWorker
            // 
            this.panelBGWorker.WorkerReportsProgress = true;
            this.panelBGWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.PanelBGWorker_DoWork);
            this.panelBGWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.PanelBGWorker_ProgressChanged);
            this.panelBGWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.PanelBGWorker_WorkDone);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(754, 487);
            this.Controls.Add(this.sidePanel);
            this.Controls.Add(this.topDockPanel);
            this.Controls.Add(this.sideDockPanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.activePanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.availablePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Risk of Death Mod Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            this.sideDockPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.launchPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelPicBox)).EndInit();
            this.topDockPanel.ResumeLayout(false);
            this.topDockPanel.PerformLayout();
            this.sidePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.addProfPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelPicBoxInPanel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel availablePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel activePanel;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.BackgroundWorker protocolBackgroundWorker;
        private System.Windows.Forms.Panel sideDockPanel;
        private System.Windows.Forms.PictureBox launchPicBox;
        private System.Windows.Forms.PictureBox exitPicBox;
        private System.Windows.Forms.PictureBox panelPicBox;
        private System.Windows.Forms.Panel topDockPanel;
        private System.Windows.Forms.Button profEditBtn;
        private System.Windows.Forms.Label profNumModsLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label profNameLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel sidePanel;
        private System.Windows.Forms.PictureBox logoPicBox;
        private System.Windows.Forms.PictureBox addProfPicBox;
        private System.Windows.Forms.Panel profsPanelInPanel;
        private System.Windows.Forms.PictureBox panelPicBoxInPanel;
        private System.Windows.Forms.Button rulesBtn;
        private System.Windows.Forms.Button urlBtn;
        private System.Windows.Forms.Button launchBtn;
        private System.Windows.Forms.Button helpBtn;
        private System.Windows.Forms.Button exitBtn;
        private System.ComponentModel.BackgroundWorker panelBGWorker;
    }
}

