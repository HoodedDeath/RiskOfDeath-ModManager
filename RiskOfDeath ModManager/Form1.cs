﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using HoodedDeathHelperLibrary;
using System.Reflection;
using System.Threading;

namespace RiskOfDeath_ModManager
{
    public partial class Form1 : Form
    {
        private const string THUNDERSTORE_PKG = "https://thunderstore.io/api/v1/package/";
        private readonly ModContainer _mods;
        private readonly string _ror2Path = "";
        private readonly bool _hd = false;

        public Form1(string ror2Path, bool nolaunch, bool hd, bool startWithDownload)
        {
            _hd = hd;
            if (ror2Path == null || ror2Path == "" || ror2Path == "#UNKNOWN#")
                throw new ArgumentException();
            else
                this._ror2Path = ror2Path;
            InitializeComponent();
            this._mods = LoadPackages();
            string p = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "uritemp");
            if (startWithDownload && File.Exists(p))
            {
                Console.WriteLine("Download requested from url protocol ...");
                StreamReader sr = new StreamReader(File.OpenRead(p));
                Install(sr.ReadLine());
                sr.Close();
                sr.Dispose();
                File.Delete(p);
            }
            else
                ListPackages();
            Console.WriteLine("Done loading.");
            if (this._mods.THIS_MOD.GetLatestVersion().VersionNumber.IsNewer(this._mods.THIS_VN))
            {
                DialogResult res = MessageBox.Show("There's a new version of Risk of Death available. Would you like to update the app now?", "Update now?", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    //Install update
                    UpdateRoD(null, null);
                }
                else
                {
                    ToolStripMenuItem item = new ToolStripMenuItem
                    {
                        Name = "updateMeToolStripMenuItem",
                        AutoSize = true,
                        Text = "Update RoD"
                    };
                    item.Click += new EventHandler(this.UpdateRoD);
                    this.menuStrip1.Items.Add(item);
                }
            }
            WriteRoamingFiles();
            if (nolaunch)
            {
                this.launchBtn.Enabled = false;
                this.launchBtn.Visible = false;
            }
            protocolBackgroundWorker.RunWorkerAsync();
        }
        private void UpdateRoD(object sender, EventArgs e)
        {
            //Call sister 'updater' program
            string temp = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "updater", "RiskOfDeath Updater.exe");
            Process.Start("explorer.exe", temp);
            Application.Exit();
            throw new CloseEverythingException();
        }
        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            protocolBackgroundWorker.CancelAsync();
            // --ONLY USE IF MODCONTAINER DOWNLOAD STARTS BREAKING--
            //this._mods.Dispose();
        }
        //Makes sure the files in the HoodedDeath folder in the user's Roaming folder exist
        private void WriteRoamingFiles()
        {
            try
            {
                //The HoodedDeath folder in the user's Roaming folder
                string hdFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HoodedDeathApplications");
                //The folder specific to this application, inside the HoodedDeath folder
                string folder = Path.Combine(hdFolder, "RiskOfDeath");
                //The file for telling the Death Game Launcher where the most recently opened executable of this application (incase there are multiple copies)
                string roamingFile = Path.Combine(folder, "RiskOfDeath.txt");
                //The file for letting any user who finds this folder why the RiskOfDeath.txt file is needed for the Death Game Launcher
                string readmeFile = Path.Combine(folder, "readme.txt");
                //Creates the HoodedDeath folder if it does not exist
                if (!Directory.Exists(hdFolder))
                    Directory.CreateDirectory(hdFolder);
                //Creates the Subnautica Options folder if it does not exist
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                //If the RiskOfDeath.txt exists, this deletes it and rewrites it with the current executable's path
                if (File.Exists(roamingFile))
                    File.Delete(roamingFile);
                StreamWriter sw = new StreamWriter(File.OpenWrite(roamingFile));
                sw.Write(Application.ExecutablePath);
                sw.Close();
                sw.Dispose();
                //Creates the readme.txt file if it does not exist
                if (!File.Exists(readmeFile))
                {
                    sw = null; sw = new StreamWriter(File.OpenWrite(readmeFile));
                    sw.Write("Please do NOT delete these files, they are used with the corresponding Death Game Launcher.\r\n\r\nRiskOfDeath.txt is used to tell the game launcher where to find the Subnautica Options application.\r\n\r\nIf Death Game Launcher is not able to find the executable for the RiskOfDeath application, launch RiskOfDeath and it should fix the path issue.");
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (Exception e) { Console.WriteLine("Failed to write roaming files:\n{0}{1}", e.Message, e.StackTrace); }
        }
        private ModContainer LoadPackages()
        {
            WebRequest req = WebRequest.Create(THUNDERSTORE_PKG);
            WebResponse resp = req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            ModContainer ret = new ModContainer(sr.ReadToEnd(), this._ror2Path);
            sr.Close();
            sr.Dispose();
            return ret;
        }
        private void ListPackages()
        {
            this._mods.UpdateModLists();

            this.panel1.AutoScrollPosition = new Point(0, 0);
            this.panel2.AutoScrollPosition = new Point(0, 0);
            this.panel3.AutoScrollPosition = new Point(0, 0);
            this.panel1.Controls.Clear();
            this.panel2.Controls.Clear();
            this.panel3.Controls.Clear();

            int[] aLocation = new int[] { 3, -87 };
            foreach (MiniMod m in this._mods.AvailableMods)
                this.panel1.Controls.Add(new DownloadableGrouping(this, m) { Location = new Point(aLocation[0], aLocation[1] += 90) });

            int[] iLocation = new int[] { 3, -87 };
            foreach (InstalledMod m in this._mods.InstalledMods)
                this.panel2.Controls.Add(new InstalledGrouping(m, this) { Location = new Point(iLocation[0], iLocation[1] += 90) });

            int[] dLocation = new int[] { 3, -16 };
            foreach (InstalledMod m in this._mods.InstalledDependencies)
                this.panel3.Controls.Add(new Label { AutoSize = true, Location = new Point(dLocation[0], dLocation[1] += 19), Text = m.Version.DependencyString, BackColor = SystemColors.Control });
        }

        private void LaunchBtn_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", "steam://rungameid/632360");
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void ModDevs_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    CreatePerModRules form = new CreatePerModRules();
                    form.ShowDialog();
                    form.Dispose();
                    break;
                default:
                    if (_hd)
                    {
                        HD f = new HD();
                        f.ShowDialog();
                        f.Dispose();
                    }
                    else
                    {
                        CreatePerModRules f = new CreatePerModRules();
                        f.ShowDialog();
                        f.Dispose();
                    }
                    break;
            }
        }

        public void Install(string dependencyString)
        {
            this._mods.Download(dependencyString);
            ListPackages();
        }
        public void Update(string dependencyString)
        {
            this._mods.UpdateMod(dependencyString);
            ListPackages();
        }
        public void Uninstall(string dependencyString)
        {
            this._mods.Uninstall(dependencyString);
            ListPackages();
        }

        private void LinkManagerProtocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("This will let you use the \"Install with Mod Manager\" button on ThunderStore to launch this manager and download a mod. If you have another manager set up for that protocol, it will be overwritten. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                string p = Assembly.GetExecutingAssembly().Location;
                StreamWriter sw = new StreamWriter(File.Open(Path.Combine(Path.GetDirectoryName(p), "temp"), FileMode.OpenOrCreate));
                sw.WriteLine(p);
                sw.Flush();
                sw.Close();
                sw.Dispose();
                Process.Start(Path.Combine(Path.GetDirectoryName(p), "RiskOfDeath SetUrlProtocol.exe"));
            }
        }


        private void ProtocolBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while (true)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                try
                {
                    if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "uritemp")))
                        break;
                    Thread.Sleep(500);
                }
                catch (IOException ex) { Console.WriteLine("IOException during Protocol Background Worker.\n{0}{1}", ex.Message, ex.StackTrace); }
                catch (Exception) { }
            }
        }
        private void ProtocolBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine(e.Error.Message + e.Error.StackTrace);
            }
            else if (!e.Cancelled)
            {
                Console.WriteLine("Download requested from url protocol ...");
                StreamReader sr = new StreamReader(File.OpenRead(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "uritemp")));
                Install(sr.ReadLine());
                sr.Close();
                sr.Dispose();
                File.Delete(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "uritemp"));
                protocolBackgroundWorker.RunWorkerAsync();
            }
        }

        private void UpdateBepInExR2APIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._mods.UpdateMod(this._mods.BEP.GetLatestVersion().DependencyString);
            this._mods.UpdateMod(this._mods.R2API.GetLatestVersion().DependencyString);
            ListPackages();
        }
    }
    public class DownloadableGrouping : GroupBox
    {
        private PictureBox iconPictureBox;
        private Label beforeDescLabel;
        private LinkLabel descLinkLabel;
        private Label beforeAuthorLabel;
        private Label authorLabel;
        private Label beforeVerLabel;
        private ComboBox verComboBox;
        private Button downloadBtn;

        private readonly MiniMod mod;
        private readonly Form1 parent;

        public MiniMod GetMod() => this.mod;
        public int VersionIndex() => this.verComboBox.SelectedIndex;

        public DownloadableGrouping()
        {
            InitializeComponent();
        }
        public DownloadableGrouping(Form1 parent, MiniMod m)
        {
            this.parent = parent;
            this.mod = m;
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            // Declarations
            this.iconPictureBox = new PictureBox();
            this.beforeDescLabel = new Label();
            this.descLinkLabel = new LinkLabel();
            this.beforeAuthorLabel = new Label();
            this.authorLabel = new Label();
            this.beforeVerLabel = new Label();
            this.verComboBox = new ComboBox();
            this.downloadBtn = new Button();
            // Icon PictureBox
            this.iconPictureBox.Location = new Point(6, 19);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new Size(60, 60);
            this.iconPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.iconPictureBox.TabIndex = 0;
            this.iconPictureBox.TabStop = false;
            try
            {
                if (mod.Versions[0].IconUrl != null && mod.Versions[0].IconUrl != "")
                    this.iconPictureBox.LoadAsync(mod.Versions[0].IconUrl);
            }
            catch (Exception)
            {
                this.iconPictureBox.Image = Properties.Resources.MissingImage;
            }
            // Before Description Label
            this.beforeDescLabel.AutoSize = true;
            this.beforeDescLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.beforeDescLabel.Location = new Point(72, 20);
            this.beforeDescLabel.Name = "beforeDescLabel";
            this.beforeDescLabel.Size = new Size(63, 13);
            this.beforeDescLabel.TabIndex = 0;
            this.beforeDescLabel.Text = "Description:";
            // Description Label
            this.descLinkLabel.AutoEllipsis = true;
            this.descLinkLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.descLinkLabel.LinkBehavior = LinkBehavior.HoverUnderline;
            this.descLinkLabel.LinkColor = SystemColors.ControlText;
            this.descLinkLabel.Location = new Point(142, 20);
            this.descLinkLabel.Name = "descLinkLabel";
            this.descLinkLabel.Size = new Size(162, 13);
            this.descLinkLabel.TabIndex = 0;
            this.descLinkLabel.TabStop = false;
            this.descLinkLabel.Text = this.mod.Versions[0].Description;
            this.descLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.DescLink_Click);
            this.descLinkLabel.MouseHover += new EventHandler(this.DescLink_MouseHover);
            this.descLinkLabel.MouseLeave += new EventHandler(this.DescLink_MouseLeave);
            // Before Author Label
            this.beforeAuthorLabel.AutoSize = true;
            this.beforeAuthorLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.beforeAuthorLabel.Location = new Point(72, 36);
            this.beforeAuthorLabel.Name = "beforeAuthorLabel";
            this.beforeAuthorLabel.Size = new Size(41, 13);
            this.beforeAuthorLabel.TabIndex = 0;
            this.beforeAuthorLabel.Text = "Author:";
            // Author Label
            this.authorLabel.AutoEllipsis = true;
            this.authorLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.authorLabel.Location = new Point(119, 36);
            this.authorLabel.Name = "authorLabel";
            this.authorLabel.Size = new Size(185, 13);
            this.authorLabel.TabIndex = 0;
            this.authorLabel.Text = this.mod.Owner;
            // Before Version Label
            this.beforeVerLabel.AutoSize = true;
            this.beforeVerLabel.Font = new Font("Microsoft Sans Serif", 8F);
            this.beforeVerLabel.Location = new Point(72, 55);
            this.beforeVerLabel.Name = "beforeVerLabel";
            this.beforeVerLabel.Size = new Size(45, 13);
            this.beforeVerLabel.TabIndex = 0;
            this.beforeVerLabel.Text = "Version:";
            // Version ComboBox
            this.verComboBox.Font = new Font("Microsoft Sans Serif", 8F);
            this.verComboBox.FormattingEnabled = true;
            foreach (MiniVersion v in this.mod.Versions)
                this.verComboBox.Items.Add(v.VersionNumber.ToString());
            this.verComboBox.Location = new Point(123, 52);
            this.verComboBox.Name = "verComboBox";
            this.verComboBox.Size = new Size(100, 21);
            this.verComboBox.TabIndex = 0;
            this.verComboBox.TabStop = false;
            this.verComboBox.Text = "Failed...";
            this.verComboBox.SelectedIndex = 0;
            this.verComboBox.ContextMenuChanged += new EventHandler(this.VersionChanged);
            // Download Button
            this.downloadBtn.Font = new Font("Microsoft Sans Serif", 8F);
            this.downloadBtn.Location = new Point(229, 50);
            this.downloadBtn.Name = "downloadBtn";
            this.downloadBtn.Size = new Size(75, 23);
            this.downloadBtn.TabIndex = 0;
            this.downloadBtn.TabStop = false;
            this.downloadBtn.Text = "Download";
            this.downloadBtn.UseVisualStyleBackColor = true;
            this.downloadBtn.Click += new EventHandler(this.DownloadBtn_Click);
            // GroupBox
            this.BackColor = SystemColors.Control;
            this.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Name = "groupBox1";
            this.Size = new Size(314, 85);
            this.TabIndex = 2;
            this.TabStop = false;
            this.Text = Helper.Truncate(this.mod.Name, 30);
            this.Controls.Add(this.iconPictureBox);
            this.Controls.Add(this.beforeDescLabel);
            this.Controls.Add(this.descLinkLabel);
            this.Controls.Add(this.beforeAuthorLabel);
            this.Controls.Add(this.authorLabel);
            this.Controls.Add(this.beforeVerLabel);
            this.Controls.Add(this.verComboBox);
            this.Controls.Add(this.downloadBtn);
            //Check for deprecated
            if (this.mod.IsDeprecated)
                this.downloadBtn.BackColor = Color.Red;
        }
        private readonly ToolTip tip = new ToolTip();
        private void DescLink_MouseHover(object sender, EventArgs e)
        {
            tip.Show(this.mod.Versions[0].Description, this, ((Control)sender).Location, 5000);
        }
        private void DescLink_MouseLeave(object sender, EventArgs e)
        {
            tip.Hide(this);
        }
        private void DescLink_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string s = string.Format("Name: {0}\nAuthor: {1}\nVersion:{2}\nDescription: {3}", this.mod.Name, this.mod.Owner, this.mod.Versions[0].VersionNumber, this.mod.Versions[0].Description);
            MessageBox.Show(s);
        }
        private void DownloadBtn_Click(object sender, EventArgs e)
        {
            this.parent.Install(this.mod.Versions[this.verComboBox.SelectedIndex].DependencyString);
        }
        private void VersionChanged(object sender, EventArgs e)
        {
            this.descLinkLabel.Text = this.mod.Versions[this.verComboBox.SelectedIndex].Description;
        }
        public DownloadableGrouping GetMe()
        {
            return this;
        }
    }
    public class InstalledGrouping : GroupBox
    {
        private LinkLabel descLinkLabel;
        private Label beforeDescLabel;
        private Button uninstallBtn;
        private Label authorLabel;
        private Label beforeVerLabel;
        private Label beforeAuthorLabel;
        private PictureBox iconPictureBox;
        private LinkLabel versionLinkLabel;
        //
        private readonly InstalledMod mod;
        private readonly Form1 parent;

        public InstalledGrouping(InstalledMod m, Form1 p)
        {
            this.mod = m;
            this.parent = p;
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.uninstallBtn = new Button();
            this.iconPictureBox = new PictureBox();
            this.beforeDescLabel = new Label();
            this.descLinkLabel = new LinkLabel();
            this.authorLabel = new Label();
            this.beforeAuthorLabel = new Label();
            this.beforeVerLabel = new Label();
            this.versionLinkLabel = new LinkLabel();
            this.SuspendLayout();
            // uninstallBtn
            this.uninstallBtn.Font = new Font("Microsoft Sans Serif", 8F);
            this.uninstallBtn.Location = new Point(229, 50);
            this.uninstallBtn.Name = "downloadBtn";
            this.uninstallBtn.Size = new Size(75, 23);
            this.uninstallBtn.TabIndex = 4;
            this.uninstallBtn.Text = "Uninstall";
            this.uninstallBtn.UseVisualStyleBackColor = true;
            this.uninstallBtn.Click += new EventHandler(this.UninstallBtn_Click);
            // iconPictureBox 
            this.iconPictureBox.Location = new Point(6, 19);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new Size(60, 60);
            this.iconPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.iconPictureBox.TabIndex = 3;
            this.iconPictureBox.TabStop = false;
            try
            {
                if (mod.Version.IconUrl != null && mod.Version.IconUrl != "")
                    this.iconPictureBox.LoadAsync(mod.Version.IconUrl);
            }
            catch (Exception)
            {
                this.iconPictureBox.Image = Properties.Resources.MissingImage;
            }
            // beforeDescLabel
            this.beforeDescLabel.AutoSize = true;
            this.beforeDescLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.beforeDescLabel.Location = new Point(72, 20);
            this.beforeDescLabel.Name = "beforeDescLabel";
            this.beforeDescLabel.Size = new Size(63, 13);
            this.beforeDescLabel.TabIndex = 0;
            this.beforeDescLabel.Text = "Description:";
            // descLinkLabel
            this.descLinkLabel.AutoEllipsis = true;
            this.descLinkLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.descLinkLabel.LinkBehavior = LinkBehavior.HoverUnderline;
            this.descLinkLabel.LinkColor = SystemColors.ControlText;
            this.descLinkLabel.Location = new Point(142, 20);
            this.descLinkLabel.Name = "descLinkLabel";
            this.descLinkLabel.Size = new Size(162, 13);
            this.descLinkLabel.TabIndex = 1;
            this.descLinkLabel.TabStop = true;
            this.descLinkLabel.Text = mod.Version.Description ?? "#UNKNOWN_DESCRIPTION#";
            // authorLabel
            this.authorLabel.AutoEllipsis = true;
            this.authorLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.authorLabel.Location = new Point(119, 36);
            this.authorLabel.Name = "authorLabel";
            this.authorLabel.Size = new Size(185, 13);
            this.authorLabel.TabIndex = 6;
            this.authorLabel.Text = mod.Owner ?? "#UNKNOWN_AUTHOR#";
            // beforeAuthorLabel
            this.beforeAuthorLabel.AutoSize = true;
            this.beforeAuthorLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.beforeAuthorLabel.Location = new Point(72, 36);
            this.beforeAuthorLabel.Name = "beforeAuthorLabel";
            this.beforeAuthorLabel.Size = new Size(41, 13);
            this.beforeAuthorLabel.TabIndex = 5;
            this.beforeAuthorLabel.Text = "Author:";
            // beforeVerLabel
            this.beforeVerLabel.AutoSize = true;
            this.beforeVerLabel.Font = new Font("Microsoft Sans Serif", 8F);
            this.beforeVerLabel.Location = new Point(72, 55);
            this.beforeVerLabel.Name = "beforeVerLabel";
            this.beforeVerLabel.Size = new Size(45, 13);
            this.beforeVerLabel.TabIndex = 5;
            this.beforeVerLabel.Text = "Version:";
            // versionLinkLabel
            bool temp = !this.mod.IsUpToDate();
            this.versionLinkLabel.ActiveLinkColor = SystemColors.ControlText;
            this.versionLinkLabel.AutoSize = true;
            this.versionLinkLabel.Font = new Font("Microsoft Sans Serif", 8F);
            this.versionLinkLabel.LinkBehavior = temp ? LinkBehavior.HoverUnderline : LinkBehavior.NeverUnderline;
            this.versionLinkLabel.LinkColor = temp ? Color.FromArgb(255, 0, 0) : SystemColors.ControlText;
            this.versionLinkLabel.Location = new Point(123, 55);
            this.versionLinkLabel.Name = "versionLinkLabel";
            this.versionLinkLabel.Size = new Size(45, 13);
            this.versionLinkLabel.TabIndex = 8;
            this.versionLinkLabel.TabStop = true;
            this.versionLinkLabel.Text = mod.Version.VersionNumber.ToString() ?? "#0.0.0#";
            this.versionLinkLabel.VisitedLinkColor = SystemColors.ControlText;
            this.versionLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.VersionLink_Click);
            //
            this.BackColor = SystemColors.Control;
            this.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Location = new Point(704, 465);
            this.Name = "groupBox1";
            this.Size = new Size(314, 85);
            this.TabIndex = 2;
            this.TabStop = false;
            this.Text = Helper.Truncate(this.mod.Name ?? "#INSTALLED_GROUPING#", 30);
            this.Visible = true;
            this.Controls.Add(this.versionLinkLabel);
            this.Controls.Add(this.uninstallBtn);
            this.Controls.Add(this.iconPictureBox);
            this.Controls.Add(this.beforeDescLabel);
            this.Controls.Add(this.descLinkLabel);
            this.Controls.Add(this.authorLabel);
            this.Controls.Add(this.beforeAuthorLabel);
            this.Controls.Add(this.beforeVerLabel);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private void UninstallBtn_Click(object sender, EventArgs e)
        {
            parent.Uninstall(this.mod.Version.DependencyString);
        }
        private void VersionLink_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            parent.Update(this.mod.Version.DependencyString);
        }
    }
    public class StringEventArgs : EventArgs
    {
        public string MSG { get; private set; }
        public StringEventArgs(string s) : base()
        {
            MSG = s;
        }
    }
}
