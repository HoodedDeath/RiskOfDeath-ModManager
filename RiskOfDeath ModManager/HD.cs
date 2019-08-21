using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HoodedDeathHelperLibrary;
using Newtonsoft.Json;

namespace RiskOfDeath_ModManager
{
    public partial class HD : Form
    {
        public HD()
        {
            InitializeComponent();
        }

        private readonly int[] sclocation = new int[] { 3, -78 };
        private int sccount = 0;
        private void AddSpecialCase()
        {
            this.specialCasePanel.AutoScrollPosition = new Point(0, 0);
            specialCasePanel.Controls.Add(new HDSCGrouping(this) { Location = new Point(sclocation[0], sclocation[1] += 81) });
            sccount++;
            if (sccount == 5)
                this.specialCasePanel.Size = new Size(this.specialCasePanel.Width + 18, this.specialCasePanel.Height);
        }
        private void AddFilledSpecialCase(HDSCJson sc)
        {
            this.specialCasePanel.AutoScrollPosition = new Point(0, 0);
            this.specialCasePanel.Controls.Add(new HDSCGrouping(this, sc, sclocation[0], sclocation[1] += 81));
            sccount++;
            if (sccount == 5)
                this.specialCasePanel.Size = new Size(this.specialCasePanel.Width + 18, this.specialCasePanel.Height);
        }
        private void AddSC_Click(object sender, EventArgs e)
        {
            using (HDSCEdit form = new HDSCEdit(true))
            {
                DialogResult res = form.ShowDialog();
                if (res == DialogResult.OK)
                    AddFilledSpecialCase(new HDSCJson { Name = form.Name, UUIDS = form.UUIDs, Folders = form.Folders, Files = form.Files, PickOneOfX = form.PickOne, PickOptions = form.PickOptions });
                form.Dispose();
            }
        }
        public void RemoveSC(string name, List<string> uuids, Dictionary<string, string> folders, Dictionary<string, string> files, bool pick)
        {
            string s = new HDSCJson() { Name = name, UUIDS = uuids, Folders = folders, Files = files, PickOneOfX = pick }.ToString();
            Dictionary<string, HDSCJson> d = new Dictionary<string, HDSCJson>();
            foreach (HDSCGrouping g in specialCasePanel.Controls)
            {
                HDSCJson h = new HDSCJson() { Name = g.Text, UUIDS = g.UUIDS, Folders = g.Folders, Files = g.Files, PickOneOfX = g.PickOne };
                d.Add(h.ToString(), h);
            }
            d.Remove(s);
            //List out
            specialCasePanel.Controls.Clear();
            this.sclocation[1] = -78;
            this.sccount = 0;
            foreach (KeyValuePair<string, HDSCJson> kvp in d)
                AddFilledSpecialCase(kvp.Value);
        }

        private readonly int[] frlocation = new int[] { 3, -106 };
        private int frcount = 0;
        private void AddFolderRule()
        {
            this.folderPanel.AutoScrollPosition = new Point(0, 0);
            folderPanel.Controls.Add(new HDFolderRuleGrouping(this) { Location = new Point(frlocation[0], frlocation[1] += 109) });
            frcount++;
            if (frcount == 4)
                this.folderPanel.Size = new Size(this.folderPanel.Width + 18, this.folderPanel.Height);
        }
        private void AddFilledFolderRule(string folder, string bep)
        {
            this.folderPanel.AutoScrollPosition = new Point(0, 0);
            folderPanel.Controls.Add(new HDFolderRuleGrouping(this, folder, bep, frlocation[0], frlocation[1] += 109));
            frcount++;
            if (frcount == 4)
                this.folderPanel.Size = new Size(this.folderPanel.Width + 18, this.folderPanel.Height);
        }
        private void AddFolder_Click(object sender, EventArgs e)
        {
            AddFolderRule();
        }
        public void RemoveFolder(string folder, string bep)
        {
            string s = string.Format("{0}->{1}", folder, bep);
            Dictionary<string, string[]> d = new Dictionary<string, string[]>();
            foreach (HDFolderRuleGrouping g in this.folderPanel.Controls)
                d.Add(string.Format("{0}->{1}", g.Folder, g.Bep), new string[] { g.Folder, g.Bep });
            d.Remove(s);
            //List out
            folderPanel.Controls.Clear();
            this.frlocation[1] = -106;
            this.frcount = 0;
            foreach (KeyValuePair<string, string[]> kvp in d)
                AddFilledFolderRule(kvp.Value[0], kvp.Value[1]);
        }

        private readonly int[] firlocation = new int[] { 3, -106 };
        private int fircount = 0;
        private void AddFileRule()
        {
            this.filePanel.AutoScrollPosition = new Point(0, 0);
            filePanel.Controls.Add(new HDFileRuleGrouping(this) { Location = new Point(firlocation[0], firlocation[1] += 109) });
            fircount++;
            if (fircount == 4)
                this.filePanel.Size = new Size(this.filePanel.Width + 18, this.filePanel.Height);
        }
        private void AddFilledFileRule(string file, string bep)
        {
            this.filePanel.AutoScrollPosition = new Point(0, 0);
            filePanel.Controls.Add(new HDFileRuleGrouping(this, file, bep, firlocation[0], firlocation[1] += 109));
            fircount++;
            if (fircount == 4)
                this.filePanel.Size = new Size(this.filePanel.Width + 18, this.filePanel.Height);
        }
        private void AddFile_Click(object sender, EventArgs e)
        {
            AddFileRule();
        }
        public void RemoveFile(string file, string bep)
        {
            string s = string.Format("{0}->{1}", file, bep);
            Dictionary<string, string[]> d = new Dictionary<string, string[]>();
            foreach (HDFileRuleGrouping g in this.filePanel.Controls)
                d.Add(string.Format("{0}->{1}", g.File, g.Bep), new string[] { g.File, g.Bep });
            d.Remove(s);
            //List out
            filePanel.Controls.Clear();
            this.firlocation[1] = -106;
            this.fircount = 0;
            foreach (KeyValuePair<string, string[]> kvp in d)
                AddFilledFileRule(kvp.Value[0], kvp.Value[1]);
        }

        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool worry = false;
            if (sccount > 0)
                worry |= ((HDSCGrouping)specialCasePanel.Controls[0]).NumOfFiles != 0 || ((HDSCGrouping)specialCasePanel.Controls[0]).NumOfFolders != 0 || ((HDSCGrouping)specialCasePanel.Controls[0]).NumOfUUIDs != 0;
            if (frcount > 0)
                worry |= ((HDFolderRuleGrouping)folderPanel.Controls[0]).Folder != "" || ((HDFolderRuleGrouping)folderPanel.Controls[0]).Bep != "";
            if (fircount > 0)
                worry |= ((HDFileRuleGrouping)filePanel.Controls[0]).File != "" || ((HDFileRuleGrouping)filePanel.Controls[0]).Bep != "";
            if (worry) worry = !(MessageBox.Show("Importing will clear any items you have entered already. Are you sure you want to continue?", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes);
            if (!worry)
            {
                OpenFileDialog d = new OpenFileDialog
                {
                    Filter = "Json (*.json)|*.json",
                    FileName = "RuleSet.json",
                    Title = "Choose your rules file"
                };
                DialogResult res = d.ShowDialog();
                if (res == DialogResult.OK)
                {
                    ClearAll();
                    StreamReader sr = new StreamReader(d.OpenFile());
                    HDJson j = JsonConvert.DeserializeObject<HDJson>(sr.ReadToEnd());
                    sr.Close();
                    sr.Dispose();
                    foreach (HDSCJson sc in j.SpecialCases)
                        AddFilledSpecialCase(sc);
                    foreach (KeyValuePair<string, string> kvp in j.Folders)
                        AddFilledFolderRule(kvp.Key, kvp.Value);
                    foreach (KeyValuePair<string, string> kvp in j.Files)
                        AddFilledFileRule(kvp.Key, kvp.Value);
                    this.uuidTextBox.Lines = j.Exclusions.ToArray();
                }
                d.Dispose();
            }
        }

        private void ClearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all items?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                ClearAll();
        }
        private void ClearAll()
        {
            //Special cases
            specialCasePanel.Controls.Clear();
            sclocation[1] = -78;
            sccount = 0;
            //Folders
            folderPanel.Controls.Clear();
            frlocation[1] = -106;
            frcount = 0;
            //Files
            filePanel.Controls.Clear();
            firlocation[1] = -106;
            fircount = 0;
            //
            uuidTextBox.Lines = new string[0];
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDJson j = new HDJson() { Exclusions = new List<string>(uuidTextBox.Lines) };
            foreach (HDSCGrouping sc in specialCasePanel.Controls)
                if (!sc.IsEmpty)
                    j.SpecialCases.Add(new HDSCJson { Files = sc.Files, Folders = sc.Folders, UUIDS = sc.UUIDS, Name = sc.Text, PickOneOfX = sc.PickOne, PickOptions = sc.PickOptions });
            foreach (HDFileRuleGrouping fi in filePanel.Controls)
                if (!fi.IsEmpty)
                    j.Files.Add(fi.File, fi.Bep);
            foreach (HDFolderRuleGrouping fo in folderPanel.Controls)
                if (!fo.IsEmpty)
                    j.Folders.Add(fo.Folder, fo.Bep);
            SaveFileDialog d = new SaveFileDialog
            {
                Filter = "Json (*.json)|*.json",
                FileName = "RuleSet.json",
                Title = "Save your rules file"
            };
            DialogResult res = d.ShowDialog();
            if (res == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(d.OpenFile());
                sw.WriteLine(JsonConvert.SerializeObject(j, Formatting.Indented));
                sw.Close();
                sw.Dispose();
            }
            d.Dispose();
        }
    }

    public class HDJson
    {
        [JsonProperty("special")]
        public List<HDSCJson> SpecialCases = new List<HDSCJson>();
        [JsonProperty("folders")]
        public Dictionary<string, string> Folders = new Dictionary<string, string>();
        [JsonProperty("files")]
        public Dictionary<string, string> Files = new Dictionary<string, string>();
        [JsonProperty("exclusions")]
        public List<string> Exclusions = new List<string>();
    }
    public class HDSCJson
    {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("uuids")]
        public List<string> UUIDS;
        [JsonProperty("pick_one_of_x")]
        public bool PickOneOfX;
        [JsonProperty("pick_options")]
        public Dictionary<string, string[]> PickOptions = new Dictionary<string, string[]>();
        [JsonProperty("folders")]
        public Dictionary<string, string> Folders = new Dictionary<string, string>();
        [JsonProperty("files")]
        public Dictionary<string, string> Files = new Dictionary<string, string>();

        public new string ToString()
        {
            return string.Format("{0}->{1}->{2}->{3}", Name, Helper.ArrToStr(UUIDS.ToArray()), Helper.DictionaryToString<string, string>(Folders), Helper.DictionaryToString<string, string>(Files));
        }
    }

    public class HDSCGrouping : GroupBox
    {
        private Label uuidLabel;
        private Label fileLabel;
        private Label folderLabel;
        private Button editBtn;
        private string _text = "Blank";
        private readonly HD parent;

        public Dictionary<string, string> Folders { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Files { get; private set; } = new Dictionary<string, string>();
        public List<string> UUIDS { get; private set; } = new List<string>();
        public bool PickOne { get; private set; }
        public Dictionary<string, string[]> PickOptions { get; private set; } = new Dictionary<string, string[]>();
        //public bool IsEmpty { get { return this.NumOfUUIDs == 0 || NumOfFiles == 0 || NumOfFolders == 0; } }
        public bool IsEmpty { get { return !(Text != "" && NumOfUUIDs > 0 && (NumOfFiles > 0 || NumOfFolders > 0 || (/*PickOne &&*/ PickOptions.Count > 0))); } }

        public new string Text
        {
            get
            {
                return this._text ?? "";
            }
            set
            {
                this._text = value;
                base.Text = Helper.Truncate(value, 20);
            }
        }
        public int NumOfUUIDs
        {
            get
            {
                return this.UUIDS.Count;
            }
            /*set
            {
                this._numUUID = value;
                this.uuidLabel.Text = string.Format("Matching UUIDs: {0}", value);
            }*/
        }
        public int NumOfFiles
        {
            get
            {
                return this.Files.Count;
            }
            /*set
            {
                this._numFile = value;
                this.fileLabel.Text = string.Format("File Rules: {0}", value);
            }*/
        }
        public int NumOfFolders
        {
            get
            {
                return this.Folders.Count;
            }
            /*set
            {
                this._numFolder = value;
                this.folderLabel.Text = string.Format("Folder Rules: {0}", value);
            }*/
        }

        public HDSCGrouping(HD parent)
        {
            this.parent = parent;
            InitializeComponent();
            this.Text = "New Rule";
        }
        public HDSCGrouping(HD parent, HDSCJson sc)
        {
            this.parent = parent;
            InitializeComponent();
            this.Text = sc.Name;
            UpdateUUIDs(sc.UUIDS);
            UpdateNumFiles(sc.Files);
            UpdateNumFolders(sc.Folders);
            this.PickOne = sc.PickOneOfX;
            this.PickOptions = sc.PickOptions;
        }
        public HDSCGrouping(HD parent, HDSCJson sc, int xLoc, int yLoc)
        {
            this.parent = parent;
            InitializeComponent();
            this.Text = sc.Name;
            UpdateUUIDs(sc.UUIDS);
            UpdateNumFiles(sc.Files);
            UpdateNumFolders(sc.Folders);
            this.PickOne = sc.PickOneOfX;
            this.PickOptions = sc.PickOptions;
            this.Location = new Point(xLoc, yLoc);
        }
        private void UpdateUUIDs(List<string> ids)
        {
            this.UUIDS = ids;
            this.uuidLabel.Text = string.Format("Matching UUIDs: {0}", this.NumOfUUIDs);
        }
        private void UpdateNumFiles(Dictionary<string, string> dict)
        {
            this.Files = dict;
            this.fileLabel.Text = string.Format("File Rules: {0}", this.NumOfFiles);
        }
        private void UpdateNumFolders(Dictionary<string, string> dict)
        {
            this.Folders = dict;
            this.folderLabel.Text = string.Format("Folder Rules: {0}", this.NumOfFolders);
        }

        private void InitializeComponent()
        {
            this.uuidLabel = new Label();
            this.fileLabel = new Label();
            this.folderLabel = new Label();
            this.editBtn = new Button();
            this.SuspendLayout();
            // uuidLabel
            this.uuidLabel.AutoSize = true;
            this.uuidLabel.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.uuidLabel.Location = new Point(6, 16);
            this.uuidLabel.Name = "uuidLabel";
            this.uuidLabel.Size = new Size(128, 16);
            this.uuidLabel.TabIndex = 2;
            this.uuidLabel.Text = string.Format("Matching UUIDs: {0}", this.NumOfUUIDs);
            this.uuidLabel.MouseHover += new EventHandler(this.UUID_MouseHover);
            this.uuidLabel.MouseLeave += new EventHandler(this.Global_MouseLeave);
            // fileLabel
            this.fileLabel.AutoSize = true;
            this.fileLabel.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.fileLabel.Location = new Point(6, 48);
            this.fileLabel.Name = "fileLabel";
            this.fileLabel.Size = new Size(91, 16);
            this.fileLabel.TabIndex = 4;
            this.fileLabel.Text = string.Format("File Rules: {0}", this.NumOfFiles);
            this.fileLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.fileLabel.MouseHover += new EventHandler(this.File_MouseHover);
            this.fileLabel.MouseLeave += new EventHandler(this.Global_MouseLeave);
            // folderLabel
            this.folderLabel.AutoSize = true;
            this.folderLabel.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.folderLabel.Location = new Point(6, 32);
            this.folderLabel.Name = "folderLabel";
            this.folderLabel.Size = new Size(108, 16);
            this.folderLabel.TabIndex = 5;
            this.folderLabel.Text = string.Format("Folder Rules: {0}", this.NumOfFolders);
            this.folderLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.folderLabel.MouseHover += new EventHandler(this.Folder_MouseHover);
            this.folderLabel.MouseLeave += new EventHandler(this.Global_MouseLeave);
            // editBtn
            this.editBtn.Location = new Point(120, 41);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new Size(41, 23);
            this.editBtn.TabIndex = 6;
            this.editBtn.Text = "Edit";
            this.editBtn.UseVisualStyleBackColor = true;
            this.editBtn.Click += new EventHandler(this.Edit_Click);
            // GroupBox
            this.Controls.Add(this.editBtn);
            this.Controls.Add(this.uuidLabel);
            this.Controls.Add(this.folderLabel);
            this.Controls.Add(this.fileLabel);
            this.Location = new Point(736, 160);
            this.Name = "groupBox1";
            this.Size = new Size(174, 75);
            this.TabIndex = 7;
            this.TabStop = false;
            //this.Text = /*this._text; // */"This is a long test string that will cut off after 20 characters";
            this.MouseHover += new EventHandler(this.Name_MouseHover);
            this.MouseLeave += new EventHandler(this.Global_MouseLeave);
            //
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            using (HDSCEdit form = new HDSCEdit(this.Text, this.UUIDS, this.Folders, this.Files, this.PickOne, this.PickOptions))
            {
                DialogResult res = form.ShowDialog();
                if (res == DialogResult.OK)
                {
                    this.Text = form.Name;
                    UpdateUUIDs(form.UUIDs);
                    UpdateNumFolders(form.Folders);
                    UpdateNumFiles(form.Files);
                    this.PickOne = form.PickOne;
                    this.PickOptions = form.PickOptions;
                }
                else if (res == DialogResult.Abort)
                    this.parent.RemoveSC(this.Text, this.UUIDS, this.Folders, this.Files, this.PickOne);
                form.Dispose();
            }
        }
        private readonly ToolTip tip = new ToolTip();
        private void Name_MouseHover(object sender, EventArgs e)
        {
            tip.Show(this.Text, this, 5000);
        }
        private void UUID_MouseHover(object sender, EventArgs e)
        {
            tip.Show("NaN", this, 5000);
        }
        private void File_MouseHover(object sender, EventArgs e)
        {
            tip.Show("NaN", this, 5000);
        }
        private void Folder_MouseHover(object sender, EventArgs e)
        {
            tip.Show("NaN", this, 5000);
        }
        private void Global_MouseLeave(object sender, EventArgs e)
        {
            tip.Hide(this);
        }
    }
    public class HDFolderRuleGrouping : GroupBox
    {

        private Label folderLabel;
        private TextBox folderTextBox;
        private Label bepLabel;
        private TextBox bepTextBox;
        private Button removeBtn;
        private object parent;
        private bool hd;


        public string Folder { get { return this.folderTextBox.Text; } set { this.folderTextBox.Text = value; } }
        public string Bep { get { return this.bepTextBox.Text; } set { this.bepTextBox.Text = value; } }
        public bool IsEmpty { get { return this.Folder == null || this.Folder == "" || this.Bep == null || this.Bep == ""; } }

        public HDFolderRuleGrouping(HD parent)
        {
            this.hd = true;
            this.parent = parent;
            InitializeComponent();
        }
        public HDFolderRuleGrouping(HD parent, string folder, string bep, int xLoc, int yLoc)
        {
            this.hd = true;
            this.parent = parent;
            InitializeComponent();
            Folder = folder;
            Bep = bep;
            Location = new Point(xLoc, yLoc);
        }
        public HDFolderRuleGrouping(HDSCEdit parent)
        {
            this.hd = false;
            this.parent = parent;
            InitializeComponent();
        }
        public HDFolderRuleGrouping(HDSCEdit parent, string folder, string bep, int xLoc, int yLoc)
        {
            this.hd = false;
            this.parent = parent;
            InitializeComponent();
            Folder = folder;
            Bep = bep;
            Location = new Point(xLoc, yLoc);
        }

        private void InitializeComponent()
        {
            this.folderLabel = new Label();
            this.folderTextBox = new TextBox();
            this.bepLabel = new Label();
            this.bepTextBox = new TextBox();
            this.removeBtn = new Button();
            this.SuspendLayout();
            // folderLabel
            this.folderLabel.AutoSize = true;
            this.folderLabel.Location = new Point(6, 22);
            this.folderLabel.Name = "folderLabel";
            this.folderLabel.Size = new Size(70, 13);
            this.folderLabel.TabIndex = 0;
            this.folderLabel.Text = "Folder Name:";
            // folderTextBox
            this.folderTextBox.Location = new Point(82, 19);
            this.folderTextBox.Name = "folderTextBox";
            this.folderTextBox.Size = new Size(108, 20);
            this.folderTextBox.TabIndex = 11;
            // bepLabel
            this.bepLabel.AutoSize = true;
            this.bepLabel.Location = new Point(6, 48);
            this.bepLabel.Name = "bepLabel";
            this.bepLabel.Size = new Size(78, 13);
            this.bepLabel.TabIndex = 12;
            this.bepLabel.Text = "Corrosponding:";
            // bepTextBox
            this.bepTextBox.Location = new Point(90, 45);
            this.bepTextBox.Name = "bepTextBox";
            this.bepTextBox.Size = new Size(100, 20);
            this.bepTextBox.TabIndex = 13;
            // removeBtn
            this.removeBtn.Location = new Point(9, 71);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new Size(181, 23);
            this.removeBtn.TabIndex = 14;
            this.removeBtn.Text = "Remove";
            this.removeBtn.UseVisualStyleBackColor = true;
            this.removeBtn.Click += new EventHandler(this.Remove_Click);
            // groupBox
            this.Controls.Add(this.folderLabel);
            this.Controls.Add(this.removeBtn);
            this.Controls.Add(this.folderTextBox);
            this.Controls.Add(this.bepTextBox);
            this.Controls.Add(this.bepLabel);
            this.Location = new Point(698, 442);
            this.Name = "groupBox2";
            this.Size = new Size(198, 103);
            this.TabIndex = 15;
            this.TabStop = false;
            //
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            if (this.hd)
                ((HD)this.parent).RemoveFolder(this.Folder, this.Bep);
            else
                ((HDSCEdit)this.parent).RemoveFolder(this.Folder, this.Bep);
        }
    }
    public class HDFileRuleGrouping : GroupBox
    {
        private Label fileLabel;
        private TextBox fileTextBox;
        private Label bepLabel;
        private TextBox bepTextBox;
        private Button removeBtn;
        private object parent;
        private bool hd;

        public string File { get { return this.fileTextBox.Text; } set { this.fileTextBox.Text = value; } }
        public string Bep { get { return this.bepTextBox.Text; } set { this.bepTextBox.Text = value; } }
        public bool IsEmpty { get { return this.File == null || this.File == "" || this.Bep == null || this.Bep == ""; } }

        public HDFileRuleGrouping(HD parent)
        {
            this.hd = true;
            this.parent = parent;
            InitializeComponent();
        }
        public HDFileRuleGrouping(HD parent, string file, string bep, int xLoc, int yLoc)
        {
            this.hd = true;
            this.parent = parent;
            InitializeComponent();
            File = file;
            Bep = bep;
            Location = new Point(xLoc, yLoc);
        }
        public HDFileRuleGrouping(HDSCEdit parent)
        {
            this.hd = false;
            this.parent = parent;
            InitializeComponent();
        }
        public HDFileRuleGrouping(HDSCEdit parent, string file, string bep, int xLoc, int yLoc)
        {
            this.hd = false;
            this.parent = parent;
            InitializeComponent();
            File = file;
            Bep = bep;
            Location = new Point(xLoc, yLoc);
        }
        private void InitializeComponent()
        {
            this.fileLabel = new Label();
            this.fileTextBox = new TextBox();
            this.bepLabel = new Label();
            this.bepTextBox = new TextBox();
            this.removeBtn = new Button();
            this.SuspendLayout();
            // 
            // fileLabel
            // 
            this.fileLabel.AutoSize = true;
            this.fileLabel.Location = new Point(6, 22);
            this.fileLabel.Name = "fileLabel";
            this.fileLabel.Size = new Size(44, 13);
            this.fileLabel.TabIndex = 0;
            this.fileLabel.Text = "File Ext:";
            // 
            // folderTextBox
            // 
            this.fileTextBox.Location = new Point(56, 19);
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.Size = new Size(134, 20);
            this.fileTextBox.TabIndex = 11;
            // 
            // bepLabel
            // 
            this.bepLabel.AutoSize = true;
            this.bepLabel.Location = new Point(6, 48);
            this.bepLabel.Name = "bepLabel";
            this.bepLabel.Size = new Size(78, 13);
            this.bepLabel.TabIndex = 12;
            this.bepLabel.Text = "Corrosponding:";
            // 
            // bepTextBox
            // 
            this.bepTextBox.Location = new Point(90, 45);
            this.bepTextBox.Name = "bepTextBox";
            this.bepTextBox.Size = new Size(100, 20);
            this.bepTextBox.TabIndex = 13;
            // 
            // removeBtn
            // 
            this.removeBtn.Location = new Point(9, 71);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new Size(181, 23);
            this.removeBtn.TabIndex = 14;
            this.removeBtn.Text = "Remove";
            this.removeBtn.UseVisualStyleBackColor = true;
            this.removeBtn.Click += new EventHandler(this.Remove_Click);
            // groupBox
            this.Controls.Add(this.fileLabel);
            this.Controls.Add(this.removeBtn);
            this.Controls.Add(this.fileTextBox);
            this.Controls.Add(this.bepTextBox);
            this.Controls.Add(this.bepLabel);
            this.Location = new Point(698, 442);
            this.Name = "groupBox2";
            this.Size = new Size(198, 103);
            this.TabIndex = 15;
            this.TabStop = false;
            //
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            if (this.hd)
                ((HD)this.parent).RemoveFile(this.File, this.Bep);
            else
                ((HDSCEdit)this.parent).RemoveFile(this.File, this.Bep);
        }
    }
}
