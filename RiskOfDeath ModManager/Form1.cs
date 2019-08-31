using System;
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
using Newtonsoft.Json;
using HoodedDeathHelperLibrary;

namespace RiskOfDeath_ModManager
{
    public partial class Form1 : Form
    {
        private const string THUNDERSTORE_PKG = "https://thunderstore.io/api/v1/package/";
        private readonly ModContainer _mods;
        private readonly string _ror2Path = "";
        private readonly bool _hd = false;
        public readonly ModContainer AllMods;

        public Form1(string ror2Path, bool nolaunch, bool hd)
        {
            _hd = hd;
            if (ror2Path == null || ror2Path == "" || ror2Path == "#UNKNOWN#")
                throw new ArgumentException();
            else
                this._ror2Path = ror2Path;
            InitializeComponent();
            this._mods = LoadPackages();
            //this._mods.UpdateModLists();
            ListPackages();
            groupBox1.Text = Helper.Truncate(groupBox1.Text, 30);
            Console.WriteLine("Done loading.");
            //
            AllMods = _mods;
            //
            WriteRoamingFiles();

            if (nolaunch)
            {
                this.launchBtn.Enabled = false;
                this.launchBtn.Visible = false;
            }
        }
        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            // --ONLY USE IF MODCONTAINER DOWNLOAD STARTS BREAKING--
            //this.AllMods.Dispose();
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
            catch (Exception e) { MessageBox.Show(e.Message); } //Shows a MessageBox about any exception was thrown
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
            List<string> temp = new List<string>();
            foreach (InstalledMod im in this._mods.InstalledDependencies)
                if (temp.Find((x) => x.Contains(im.LongName)) == null)
                    temp.Add(im.Version.DependencyString);
                else
                {
                    string s = im.LongName;
                    string test = null;
                    foreach (InstalledMod m in this._mods.InstalledDependencies)
                    {
                        if (test != null)
                            break;
                        if (m.LongName == s)
                            test = m.LongName;
                    }
                    test = temp.Find((x) => x.Contains(test));
                    if (im.Version.VersionNumber.IsNewer(this._mods.FindMod(test).VersionNumber))
                    {
                        temp.Remove(test);
                        temp.Add(im.Version.DependencyString);
                    }
                }
            //Label depLabel = new Label { AutoSize = true, Name = "depLabel", Size = new Size(153, 13), TabIndex = 10 };
            foreach (string s in temp)
                this.panel3.Controls.Add(new Label { AutoSize = true, Location = new Point(dLocation[0], dLocation[1] += 19), Text = s });

            /*int[] location = new int[] { 3, -87 };
            foreach (MiniMod m in mods.ToArray())
                this.panel1.Controls.Add(new DownloadableGrouping(this, m) { Location = new Point(location[0], location[1] += 90) });*/
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(descLinkLabel.Text);
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

        //private readonly int[] loc = new int[] { 3, -87 };
        /*public void MoveMe(DownloadableGrouping d)
        {
            //d.Location = new Point(loc[0], loc[1] += 90);
            //this.panel2.Controls.Add(d);
            this.panel1.Controls.Remove(d);
            Version v = this._mods.FindMod(d.GetMod().Versions[d.VersionIndex()].DependencyString);
            //InstalledMod i = new InstalledMod(v.ParentMod, v);
            //InstalledGrouping g = new InstalledGrouping(i, this) { Location = new Point(loc[0], loc[1] += 90) };
            this.panel2.AutoScrollPosition = new Point(0, 0);
            this.panel2.Controls.Add(new InstalledGrouping(new InstalledMod(v.ParentMod, v), this) { Location = new Point(loc[0], loc[1] += 90) });

        }

        public void UninstallMe(InstalledGrouping inst)
        {

        }*/

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
        //Change to download through ModContainer instance in Form1
        private void DownloadBtn_Click(object sender, EventArgs e)
        {
            this.parent.Install(this.mod.Versions[this.verComboBox.SelectedIndex].DependencyString);
            /*this.parent.AllMods.Download(this.mod.Versions[this.verComboBox.SelectedIndex].DependencyString);
            this.parent.MoveMe(this);*/
            //this.Dispose();
            /*
            Console.WriteLine(string.Format("Downloading: {0}-{1}-{2} ...", this.mod.Owner, this.mod.Name, this.mod.Versions[this.verComboBox.SelectedIndex].VersionNumber));
            using (var client = new WebClient())
            {
                string s = string.Format("{0}-{1}-{2}.", this.mod.Owner, this.mod.Name, this.mod.Versions[this.verComboBox.SelectedIndex].VersionNumber);
                client.DownloadFile(this.mod.Versions[this.verComboBox.SelectedIndex].DownloadUrl, s + "tmp");
                File.Move(s + "tmp", "downloads\\" + s + "zip");
            }
            Console.WriteLine("done");
            */
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
            //this.versionLinkLabel.LinkBehavior = LinkBehavior.HoverUnderline;
            this.versionLinkLabel.LinkBehavior = temp ? LinkBehavior.HoverUnderline : LinkBehavior.NeverUnderline;
            //this.versionLinkLabel.LinkColor = SystemColors.ControlText;
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
            this.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Location = new Point(704, 465);
            this.Name = "groupBox1";
            this.Size = new Size(314, 85);
            this.TabIndex = 2;
            this.TabStop = false;
            this.Text = Helper.Truncate(this.mod.Name ?? "#INSTALLED_GROUPING#", 30);
            /*try { this.Text = Helper.Truncate(this.mod.Name, 30); }
            catch (NullReferenceException) { this.Text = Helper.Truncate("#INSTALLED_GROUPING#", 30); }*/
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
            //parent.UninstallMe(this);
            parent.Uninstall(this.mod.Version.DependencyString);
        }

        private void VersionLink_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            parent.Update(this.mod.Version.DependencyString);
        }
    }
}
