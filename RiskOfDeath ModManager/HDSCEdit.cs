using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RiskOfDeath_ModManager
{
    public partial class HDSCEdit : Form
    {
        public new string Name
        {
            get { return this.nameTextBox.Text; }
            private set { this.nameTextBox.Text = value; }
        }
        public List<string> UUIDs
        {
            get { return new List<string>(this.uuidTextBox.Lines); }
            private set { this.uuidTextBox.Lines = value.ToArray(); }
        }
        public Dictionary<string, string> Folders
        {
            get
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (HDFolderRuleGrouping g in this.folderPanel.Controls)
                    if (!g.IsEmpty)
                        dict.Add(g.Folder, g.Bep);
                return dict;
            }
            private set
            {
                foreach (KeyValuePair<string, string> kvp in value)
                    AddFilledFolderRule(kvp.Key, kvp.Value);
            }
        }
        public Dictionary<string, string> Files
        {
            get
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (HDFileRuleGrouping g in filePanel.Controls)
                    if (!g.IsEmpty)
                        dict.Add(g.File, g.Bep);
                return dict;
            }
            private set
            {
                foreach (KeyValuePair<string, string> kvp in value)
                    AddFilledFileRule(kvp.Key, kvp.Value);
            }
        }
        public bool PickOne
        {
            get { return this.pickOneCheckBox.Checked; }
            private set { this.pickOneCheckBox.Checked = value; }
        }
        public Dictionary<string, string[]> PickOptions
        {
            get
            {
                Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
                foreach (HDSCEOption g in pickOptionPanel.Controls)
                    if (g.IsComplete)
                        dict.Add(g.Name, new string[] { g.File, g.Bep });
                return dict;
            }
            private set
            {
                foreach (KeyValuePair<string, string[]> kvp in value)
                    AddFilledOption(kvp.Key, kvp.Value[0], kvp.Value[1]);
            }
        }

        public HDSCEdit()
        {
            InitializeComponent();
            this.nameTextBox.Text = "New Rule";
            Pick_CheckChanged(this, EventArgs.Empty);
        }
        public HDSCEdit(bool hideRem)
        {
            InitializeComponent();
            this.nameTextBox.Text = "New Rule";
            this.removeBtn.Enabled = !hideRem;
            this.removeBtn.Visible = !hideRem;
            Pick_CheckChanged(this, EventArgs.Empty);
        }
        public HDSCEdit(string name, List<string> uuids, Dictionary<string, string> folders, Dictionary<string, string> files, bool pick, Dictionary<string, string[]> options)
        {
            InitializeComponent();
            this.Name = name;
            this.UUIDs = uuids;
            this.Folders = folders;
            this.Files = files;
            this.PickOne = pick;
            this.PickOptions = options ?? new Dictionary<string, string[]>();
            Pick_CheckChanged(this, EventArgs.Empty);
        }

        private readonly int[] folocation = new int[] { 3, -106 };
        private int focount = 0;
        private void AddFolderBtn_Click(object sender, EventArgs e)
        {
            this.folderPanel.AutoScrollPosition = new Point(0, 0);
            folderPanel.Controls.Add(new HDFolderRuleGrouping(this) { Location = new Point(folocation[0], folocation[1] += 109) });
            focount++;
            if (focount == 4)
                this.folderPanel.Size = new Size(this.folderPanel.Width + 18, this.folderPanel.Height);
        }
        private void AddFilledFolderRule(string folder, string bep)
        {
            this.folderPanel.AutoScrollPosition = new Point(0, 0);
            folderPanel.Controls.Add(new HDFolderRuleGrouping(this, folder, bep, folocation[0], folocation[1] += 109));
            focount++;
            if (focount == 4)
                this.folderPanel.Size = new Size(this.folderPanel.Width + 18, this.folderPanel.Height);
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
            this.folocation[1] = -106;
            this.focount = 0;
            foreach (KeyValuePair<string, string[]> kvp in d)
                AddFilledFolderRule(kvp.Value[0], kvp.Value[1]);
        }

        private readonly int[] filocation = new int[] { 3, -106 };
        private int ficount = 0;
        private void AddFileBtn_Click(object sender, EventArgs e)
        {
            this.filePanel.AutoScrollPosition = new Point(0, 0);
            filePanel.Controls.Add(new HDFileRuleGrouping(this) { Location = new Point(filocation[0], filocation[1] += 109) });
            ficount++;
            if (ficount == 4)
                this.filePanel.Size = new Size(this.filePanel.Width + 18, this.filePanel.Height);
        }
        private void AddFilledFileRule(string file, string bep)
        {
            this.filePanel.AutoScrollPosition = new Point(0, 0);
            filePanel.Controls.Add(new HDFileRuleGrouping(this, file, bep, filocation[0], filocation[1] += 109));
            ficount++;
            if (ficount == 4)
                this.filePanel.Size = new Size(this.filePanel.Width + 18, this.filePanel.Height);
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
            this.filocation[1] = -106;
            this.ficount = 0;
            foreach (KeyValuePair<string, string[]> kvp in d)
                AddFilledFileRule(kvp.Value[0], kvp.Value[1]);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void AcceptBtn_Click(object sender, EventArgs e)
        {
            if (this.uuidTextBox.Text == null || this.uuidTextBox.Text.Trim() == "")
                MessageBox.Show("There must be at least one applicable UUID");
            else if (this.nameTextBox.Text == null || this.nameTextBox.Text == "")
                MessageBox.Show("Missing rule name");
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        private void Pick_CheckChanged(object sender, EventArgs e)
        {
            this.pickOptionPanel.Visible = this.PickOne;
            this.pickOptionPanel.Enabled = this.PickOne;
            this.addOptionBtn.Visible = this.PickOne;
            this.addOptionBtn.Enabled = this.PickOne;
        }
        private readonly int[] oplocation = new int[] { 3, -124 };
        private int opcount = 0;
        private void AddOption_Click(object sender, EventArgs e)
        {
            this.pickOptionPanel.AutoScrollPosition = new Point(0, 0);
            this.pickOptionPanel.Controls.Add(new HDSCEOption(this) { Location = new Point(oplocation[0], oplocation[1] += 127) });
            opcount++;
            if (opcount == 1)
                this.pickOptionPanel.Size = new Size(this.pickOptionPanel.Width + 18, this.pickOptionPanel.Height);
        }
        private void AddFilledOption(/*Dictionary<string, string[]> dict*/string name, string file, string bep)
        {
            this.pickOptionPanel.AutoScrollPosition = new Point(0, 0);
            this.pickOptionPanel.Controls.Add(new HDSCEOption(this, name, file, bep) { Location = new Point(oplocation[0], oplocation[1] += 127) });
            opcount++;
            if (opcount == 1)
                this.pickOptionPanel.Size = new Size(this.pickOptionPanel.Width + 18, this.pickOptionPanel.Height);
        }
        public void RemoveOption(string name, string file, string bep)
        {
            string s = string.Format("{0}->{1}->{2}", name, file, bep);
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
            foreach (HDSCEOption g in this.pickOptionPanel.Controls)
                dict.Add(string.Format("{0}->{1}->{2}", g.Name, g.File, g.Bep), new string[] { g.Name, g.File, g.Bep });
            dict.Remove(s);
            this.pickOptionPanel.Controls.Clear();
            oplocation[1] = -124;
            if (opcount >= 1)
                this.pickOptionPanel.Size = new Size(this.pickOptionPanel.Width - 18, this.pickOptionPanel.Height);
            opcount = 0;
            foreach (KeyValuePair<string, string[]> kvp in dict)
                AddFilledOption(kvp.Value[0], kvp.Value[1], kvp.Value[2]);
        }
    }

    public class HDSCEOption : GroupBox
    {
        private TextBox optionNameTextBox;
        private Label label3;
        private Label label5;
        private TextBox fileTextBox;
        private Label label6;
        private TextBox bepTextBox;
        private Button removeBtn;
        private HDSCEdit parent;

        public new string Name
        {
            get { return this.optionNameTextBox.Text ?? ""; }
            private set { this.optionNameTextBox.Text = value; }
        }
        public string File
        {
            get { return this.fileTextBox.Text ?? ""; }
            private set { this.fileTextBox.Text = value; }
        }
        public string Bep
        {
            get { return this.bepTextBox.Text ?? ""; }
            private set { this.bepTextBox.Text = value; }
        }
        public bool IsComplete { get { return this.Name != "" && this.File != "" && this.Bep != ""; } }

        public HDSCEOption(HDSCEdit parent)
        {
            InitializeComponent();
            this.parent = parent;
        }
        public HDSCEOption(HDSCEdit parent, string name, string file, string bep)
        {
            InitializeComponent();
            this.parent = parent;
            this.Name = name;
            this.File = file;
            this.Bep = bep;
        }

        private void InitializeComponent()
        {
            this.optionNameTextBox = new TextBox();
            this.label3 = new Label();
            this.label5 = new Label();
            this.fileTextBox = new TextBox();
            this.label6 = new Label();
            this.bepTextBox = new TextBox();
            this.removeBtn = new Button();
            this.SuspendLayout();
            // optionNameTextBox
            this.optionNameTextBox.Location = new Point(84, 13);
            this.optionNameTextBox.Name = "optionNameTextBox";
            this.optionNameTextBox.Size = new Size(100, 20);
            this.optionNameTextBox.TabIndex = 36;
            // label3
            this.label3.AutoSize = true;
            this.label3.Location = new Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new Size(72, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Option Name:";
            // label5
            this.label5.AutoSize = true;
            this.label5.Location = new Point(6, 42);
            this.label5.Name = "label5";
            this.label5.Size = new Size(50, 13);
            this.label5.TabIndex = 39;
            this.label5.Text = "Mod File:";
            // fileTextBox
            this.fileTextBox.Location = new Point(62, 39);
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.Size = new Size(122, 20);
            this.fileTextBox.TabIndex = 38;
            // label6
            this.label6.AutoSize = true;
            this.label6.Location = new Point(6, 68);
            this.label6.Name = "label6";
            this.label6.Size = new Size(61, 13);
            this.label6.TabIndex = 41;
            this.label6.Text = "Bep Folder:";
            // bepTextBox
            this.bepTextBox.Location = new Point(73, 65);
            this.bepTextBox.Name = "bepTextBox";
            this.bepTextBox.Size = new Size(111, 20);
            this.bepTextBox.TabIndex = 40;
            // removeBtn
            this.removeBtn.Location = new Point(9, 91);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new Size(175, 23);
            this.removeBtn.TabIndex = 42;
            this.removeBtn.Text = "Remove";
            this.removeBtn.UseVisualStyleBackColor = true;
            this.removeBtn.Click += new EventHandler(this.Remove_Click);
            // GroupBox
            this.Controls.Add(this.label3);
            this.Controls.Add(this.removeBtn);
            this.Controls.Add(this.optionNameTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.fileTextBox);
            this.Controls.Add(this.bepTextBox);
            this.Controls.Add(this.label5);
            this.Location = new Point(905, 208);
            base.Name = "groupBox1";
            this.Size = new Size(191, 121);
            this.TabIndex = 43;
            this.TabStop = false;
            //
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            this.parent.RemoveOption(this.Name, this.File, this.Bep);
        }
    }
}
