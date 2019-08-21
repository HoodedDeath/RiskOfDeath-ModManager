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
            ListPackages(this._mods.ModsToList);
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
            return new ModContainer(sr.ReadToEnd(), this._ror2Path);
        }
        private void ListPackages(List<MiniMod> mods)
        {
            int[] location = new int[] { 3, -87 };
            foreach (MiniMod m in mods.ToArray())
                this.panel1.Controls.Add(new DownloadableGrouping(this, m) { Location = new Point(location[0], location[1] += 90) });
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
                    _ = new CreatePerModRules().ShowDialog();
                    break;
                default:
                    if (_hd)
                        _ = new HD().ShowDialog();
                    else
                        _ = new CreatePerModRules().ShowDialog();
                    break;
            }
        }

        private int[] loc = new int[] { 3, -87 };
        public void MoveMe(DownloadableGrouping d)
        {
            d.Location = new Point(loc[0], loc[1] += 90);
            this.panel2.Controls.Add(d);
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
            this.parent.AllMods.Download(this.mod.Versions[this.verComboBox.SelectedIndex].DependencyString);
            this.parent.MoveMe(this);
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
        public DownloadableGrouping GetMe()
        {
            return this;
        }
    }
}
