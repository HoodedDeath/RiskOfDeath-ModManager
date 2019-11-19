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
            this.sidePanel.BringToFront();
            this.sidePanel.Location = new Point(-334, 0);
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
                DialogResult res = MessageBox.Show("There's a new version of Risk of Death MM available. Would you like to update now?", "Update now?", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    //Install update
                    //UpdateRoD(null, null);
                    UpdateSelf(null, null);
                }
                else
                {
                    /*ToolStripMenuItem item = new ToolStripMenuItem
                    {
                        Name = "updateMeToolStripMenuItem",
                        AutoSize = true,
                        Text = "Update RoD"
                    };
                    item.Click += new EventHandler(this.UpdateRoD);
                    this.menuStrip1.Items.Add(item);*/
                    AddUpdateBtn();
                }
            }
            WriteRoamingFiles();
            if (nolaunch)
            {
                this.launchBtn.Enabled = false;
                this.launchBtn.Visible = false;
            }
            //Display profiles
            UpdateProfiles();
            //
            protocolBackgroundWorker.RunWorkerAsync();
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

            this.availablePanel.AutoScrollPosition = new Point(0, 0);
            this.activePanel.AutoScrollPosition = new Point(0, 0);
            //this.panel3.AutoScrollPosition = new Point(0, 0);
            this.availablePanel.Controls.Clear();
            this.activePanel.Controls.Clear();
            //this.panel3.Controls.Clear();

            int[] aLocation = new int[] { 3, -87 };
            foreach (MiniMod m in this._mods.AvailableMods)
                this.availablePanel.Controls.Add(new DownloadableGrouping(this, m) { Location = new Point(aLocation[0], aLocation[1] += 90) });

            int[] iLocation = new int[] { 3, -87 };
            foreach (InstalledMod m in this._mods.InstalledMods)
                this.activePanel.Controls.Add(new InstalledGrouping(m, this) { Location = new Point(iLocation[0], iLocation[1] += 90) });

            /*int[] dLocation = new int[] { 3, -16 };
            foreach (InstalledMod m in this._mods.InstalledDependencies)
                this.panel3.Controls.Add(new Label { AutoSize = true, Location = new Point(dLocation[0], dLocation[1] += 19), Text = m.Version.DependencyString, BackColor = SystemColors.Control });*/
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

        // Listen for thunderstore mod manager url requests
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
        //

        // Slide Side Panel
        private bool panel_is_out = false;
        private int panel_loc = 0;
        private readonly List<Control> panel_controls = new List<Control>();
        private void PanelBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            panel_loc = sidePanel.Location.X;
            BackgroundWorker worker = sender as BackgroundWorker;
            int l = 0;
            for (int i = 0; i < 338; i++)
            {
                worker.ReportProgress(panel_is_out ? panel_loc - i : panel_loc + i);
                Thread.Sleep(l == 0 && false ? 1 : 0);
                l = (l + 1) % 5;
            }
            panel_is_out = !panel_is_out;
        }
        private void PanelBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            sidePanel.Location = new Point(e.ProgressPercentage, sidePanel.Location.Y);
            sideDockPanel.Visible = sidePanel.Location.X < -250;
        }
        private void PanelBGWorker_WorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show(e.Error.Message + e.Error.StackTrace);
            sidePanel.Controls.AddRange(panel_controls.ToArray());
            panel_controls.Clear();
            availablePanel.Show();
        }
        private void PanelBtns_Click(object sender, EventArgs e)
        {
            if (!panelBGWorker.IsBusy)
            {
                profsPanelInPanel.AutoScrollPosition = new Point(0, 0);
                foreach (Control c in sidePanel.Controls)
                    panel_controls.Add(c);
                sidePanel.Controls.Clear();
                availablePanel.Hide();
                panelBGWorker.RunWorkerAsync();
            }
        }
        //Side Panel Functionality
        private void UpdateProfiles()
        {
            ListProfiles();
            UpdateProfileDetails();
        }
        private void ListProfiles()
        {
            int[] loc = new int[] { 3, -76 };
            profsPanelInPanel.Controls.Clear();
            profsPanelInPanel.AutoScrollPosition = new Point(0, 0);
            foreach (KeyValuePair<string, Profile> kvp in this._mods.Profiles)
                profsPanelInPanel.Controls.Add(new ProfileGrouping(this, kvp.Value, kvp.Key) { Location = new Point(loc[0], loc[1] += 79) });
        }
        private void AddUpdateBtn()
        {
            this.profsPanelInPanel.Size = new Size(profsPanelInPanel.Width, profsPanelInPanel.Height - 26);
            Button updateBtnInPanel = new Button
            {
                BackColor = SystemColors.Control,
                Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(3, 346),
                Name = "updateBtnInPanel",
                Size = new Size(326, 23),
                TabIndex = 5,
                Text = "Update",
                UseVisualStyleBackColor = false
            };
            updateBtnInPanel.Click += new EventHandler(this.UpdateSelf);
            this.sidePanel.Controls.Add(updateBtnInPanel);
        }
        private void UpdateSelf(object sender, EventArgs e)
        {
            //Call sister 'updater' program
            //string temp = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "updater", "RiskOfDeath Updater.exe");
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "updater", "RiskOfDeath Updater.exe")))
            {
                MessageBox.Show("Updater program file cannot be found.", "Updater Not Found", MessageBoxButtons.OK);
                return;
            }
            Process.Start("explorer.exe", Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "updater", "RiskOfDeath Updater.exe") /*temp*/);
            Application.Exit();
            throw new CloseEverythingException();
        }
        /*private void Launch_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", "steam://rungameid/632360");
        }*/
        private void Launch_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    this._mods.InstallMods();
                    Console.WriteLine("Launching RoR2 ...");
                    Process.Start("explorer.exe", "steam://rungameid/632360");
                    Thread.Sleep(30000);
                    Console.WriteLine("Awaiting RoR2 exit to uninstall mods ...");
                    while (true)
                    {
                        int i = (int)Registry.GetValue("HKEY_CURRENT_USER\\Software\\Valve\\Steam", "RunningAppID", -1);
                        if (i != 632360)
                            break;
                        Thread.Sleep(5000);
                    }
                    Console.WriteLine("RoR2 no longer running, uninstalling mods ...");
                    this._mods.UninstallMods();
                    break;
                case MouseButtons.Right:
                    ContextMenu menu = new ContextMenu();
                    menu.MenuItems.Add("Install mods without launch", new EventHandler(this.LaunchRMB_Install));
                    menu.MenuItems.Add("Uninstall mods", new EventHandler(this.LaunchRMB_Uninstall));
                    menu.Show(this, new Point(e.X, e.Y));
                    //MessageBox.Show("Launch RightClick");
                    break;
            }
        }
        private void LaunchRMB_Install(object sender, EventArgs e)
        {
            this._mods.InstallMods();
            MessageBox.Show("Mod profile installed. Use the uninstall button in the context menu to uninstall later.");
        }
        private void LaunchRMB_Uninstall(object sender, EventArgs e)
        {
            this._mods.UninstallMods();
            MessageBox.Show("Mod profile uninstalled.");
        }
        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
        private void RuleSetup_MouseDown(object sender, MouseEventArgs e)
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
        private void LinkManagerURL_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("This will let you use the \"Install with Mod Manager\" button on ThunderStore to launch this manager and download a mod. If you have another manager set up for that protocol, it will be overwritten. Administrator rights are required. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo);
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
        private void Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show("HelpBtn");
        }
        private void AddProfile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("AddProfileBtn");
            using (EditProfileForm form = new EditProfileForm())
            {
                DialogResult res = form.ShowDialog();
                if (res == DialogResult.OK)
                {
                    this._mods.AddNewProfile(form.ProfName, form.Mods);
                    UpdateProfiles();
                }
                else if (res != DialogResult.Cancel)
                    Console.WriteLine("+\n+\n+\nidk wtf just happened, EditProfileForm in add mode should only return DialogResult of OK, or Cancel\n+\n+\n+");
            }
        }
        private void EditProfile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("EditProfileBtn");
            using (EditProfileForm form = new EditProfileForm(this._mods.CurrentProfile.Name, this._mods.CurrentProfile.Mods))
            {
                DialogResult res = form.ShowDialog();
                if (res == DialogResult.OK)
                {
                    this._mods.EditCurrentProfile(form.ProfName);
                    UpdateProfiles();
                }
                else if (res == DialogResult.Abort)
                {
                    this._mods.DeleteCurrentProfile();
                    UpdateProfiles();
                }
                else if (res != DialogResult.Cancel)
                    Console.WriteLine("+\n+\n+\nidk wtf just happen, EditProfileForm in edit mode should only return a DialogResult of Ok, Abort, or Cancel\n+\n+\n+");
            }
        }
        public void DeleteProfile(ProfileGrouping g)
        {
            this._mods.DeleteProfile(g.ID);
            UpdateProfiles();
        }
        public void SetProfile(ProfileGrouping g, bool fromPanel)
        {
            if (this._mods.ChangeProfile(g.ID))
            {
                UpdateProfileDetails();
                if (fromPanel)
                    PanelBtns_Click(null, null);
            }
            else
                MessageBox.Show(string.Format("An error occured while trying to change profile. Profile details:\nID: '{0}'\nName: '{1}'\nMods: {2}", g.ID, g.Name, Helper.ArrToStr(g.Mods)));
        }
        private void UpdateProfileDetails()
        {
            Profile p = this._mods.CurrentProfile;
            this.profNameLabel.Text = p.Name;
            this.profNumModsLabel.Text = p.Mods.Count.ToString();
        }
        //
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
    public class ProfileGrouping : GroupBox
    {
        private Label numModsLabel;
        private Label label1;
        private PictureBox deletePicBox;
        private PictureBox loadPicBox;
        private readonly Form1 parent;
        public string ID { get; private set; } = "#NO_ID#";

        /*public ProfileGrouping(Form1 parent)
        {
            InitializeComponent();
            this.parent = parent;
            UpdateModLabel();
        }
        public ProfileGrouping(Form1 parent, string name)
        {
            InitializeComponent();
            this.parent = parent;
            this.Text = name;
            UpdateModLabel();
        }
        public ProfileGrouping(Form1 parent, string name, params string[] mods)
        {
            InitializeComponent();
            this.parent = parent;
            this.Text = name;
            //this.Mods = new List<string>(mods);
            AddMods(mods);
            //UpdateModLabel();
        }
        public ProfileGrouping(Form1 parent, string id, string name, List<string> mods)
        {
            InitializeComponent();
            this.parent = parent;
            this.Text = name;
            AddMods(mods);
            this.ID = id;
        }*/
        public ProfileGrouping(Form1 parent, Profile profile, string id)
        {
            InitializeComponent();
            this.parent = parent;
            this.Text = profile.Name;
            AddMods(profile.Mods);
            this.ID = id;
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            numModsLabel = new Label();
            label1 = new Label();
            loadPicBox = new PictureBox();
            deletePicBox = new PictureBox();
            // deletePicBox
            this.deletePicBox.BackColor = Color.Transparent;
            this.deletePicBox.BackgroundImage = Properties.Resources.delete_btn;
            this.deletePicBox.BackgroundImageLayout = ImageLayout.Zoom;
            this.deletePicBox.Location = new Point(134, 16);
            this.deletePicBox.Name = "deletePicBox";
            this.deletePicBox.Size = new Size(50, 50);
            this.deletePicBox.TabIndex = 6;
            this.deletePicBox.TabStop = false;
            this.deletePicBox.Click += new EventHandler(DeleteMe);
            // loadPicBox
            this.loadPicBox.BackColor = Color.Transparent;
            this.loadPicBox.BackgroundImage = Properties.Resources.load_profile;
            this.loadPicBox.BackgroundImageLayout = ImageLayout.Zoom;
            this.loadPicBox.Location = new Point(190, 16);
            this.loadPicBox.Name = "loadPicBox";
            this.loadPicBox.Size = new Size(50, 50);
            this.loadPicBox.TabIndex = 5;
            this.loadPicBox.TabStop = false;
            this.loadPicBox.Click += new EventHandler(LoadMe);
            // label1
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(16, 21);
            this.label1.Name = "label1";
            this.label1.Size = new Size(79, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "# of Mods";
            // numModsLabel
            this.numModsLabel.AutoSize = true;
            this.numModsLabel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.numModsLabel.Location = new Point(16, 41);
            this.numModsLabel.Name = "numModsLabel";
            this.numModsLabel.Size = new Size(27, 20);
            this.numModsLabel.TabIndex = 5;
            this.numModsLabel.Text = "##";
            // groupBox
            this.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.BackColor = SystemColors.ActiveBorder;
            this.Controls.Add(this.deletePicBox);
            this.Controls.Add(this.loadPicBox);
            this.Controls.Add(this.numModsLabel);
            this.Controls.Add(this.label1);
            this.Location = new Point(622, 130);
            this.Name = "groupBox1";
            this.Size = new Size(246, 73);
            this.TabIndex = 4;
            this.TabStop = false;
            this.Text = "#PROFILE_NAME#";
            //
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public List<string> Mods { get; private set; } = new List<string>();
        public void RemoveMod(string dependencyString)
        {
            this.Mods.Remove(dependencyString);
            UpdateModLabel();
        }
        public void AddMod(string dependencyString)
        {
            if (!this.Mods.Contains(dependencyString))
                this.Mods.Add(dependencyString);
            UpdateModLabel();
        }
        public void AddMods(params string[] deps)
        {
            foreach (string s in deps)
                if (!this.Mods.Contains(s))
                    this.Mods.Add(s);
            UpdateModLabel();
        }
        public void AddMods(List<string> deps)
        {
            foreach (string s in deps)
                if (!this.Mods.Contains(s))
                    this.Mods.Add(s);
            UpdateModLabel();
        }
        private void UpdateModLabel()
        {
            this.numModsLabel.Text = this.Mods.Count.ToString();
        }

        private void LoadMe(object sender, EventArgs e)
        {
            this.parent.SetProfile(this, true);
        }
        private void DeleteMe(object sender, EventArgs e)
        {
            this.parent.DeleteProfile(this);
        }

        private string _text = "";
        public new string Text
        {
            get { return _text; }
            set { _text = value; base.Text = Helper.Truncate(value, 19); }
        }
    }
}
