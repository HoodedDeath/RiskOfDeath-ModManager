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
    public partial class EditProfileForm : Form
    {
        //public string ProfName { get; set; } = "";
        public string ProfName { get { return this.nameTextBox.Text; } set { this.nameTextBox.Text = value; } }
        public List<string> Mods { get; set; } = new List<string>();

        //Add profile
        public EditProfileForm()
        {
            InitializeComponent();
            this.Text = "Add Profile";
            this.exportTextBox.Text = "[]";
            this.deleteBtn.Visible = false;
            this.deleteBtn.Enabled = false;
        }
        //Edit profile
        public EditProfileForm(string name, List<string> mods)
        {
            InitializeComponent();
            this.Text = "Edit Profile";
            this.nameTextBox.Text = name;
            this.importBtn.Visible = false;
            this.importBtn.Enabled = false;

            this.Mods = mods;

            string s = "[";
            if (mods.Count > 0)
            {
                foreach (string sa in mods)
                    s += sa + ",";
                s = s.Substring(0, s.Length - 1) + "]";
            }
            else s += "]";
            this.exportTextBox.Text = s;
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Cancel_Click(null, null);
            else if (e.KeyCode == Keys.Enter)
                Accept_Click(null, null);
        }

        private void CopyExportBtn_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.exportTextBox.Text);
            this.exportBtn.Text = "Copied";
            this.copyBtnBGWorker.RunWorkerAsync();
        }
        private void CopyBtnBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(2000);
        }
        private void CopyBtnBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.exportBtn.Text = "Copy Export String";
        }
        private void CopyBtnBGWorker_WorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            this.exportBtn.Text = "Copy Export String";
        }

        private void Import_Click(object sender, EventArgs e)
        {
            //Check clipboard for appropriate string / popup textbox
            string test = Clipboard.GetText();
            if (test.StartsWith("[") && test.EndsWith("]"))
            {
                //Import from clipboard
                test = test.Substring(1, test.Length - 2);
                string[] arr = test.Split(',');
                foreach (string sa in arr)
                    if (!this.Mods.Contains(sa))
                        this.Mods.Add(sa);
                this.exportTextBox.Text = test;
            }
            else
            {
                //Import from popup
                using (var dialog = new TextBoxDialogBox())
                {
                    DialogResult res = dialog.Show("Input Import String");
                    if (res == DialogResult.OK)
                    {
                        if (dialog.Result.StartsWith("[") && dialog.Result.EndsWith("]"))
                        {
                            string st = dialog.Result;
                            st = st.Substring(1, st.Length - 2);
                            string[] arr = st.Split(',');
                            foreach (string s in arr)
                                if (!this.Mods.Contains(s))
                                    this.Mods.Add(s);
                            this.exportTextBox.Text = dialog.Result;
                        }
                        else
                            MessageBox.Show("Import string invalid. Try again or cancel.");
                    }
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Delete");

            this.DialogResult = DialogResult.Abort;
            Close();
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
        private void Accept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = this.DialogResult == DialogResult.Abort ? MessageBox.Show("Are you sure you want to delete this profile?", "Delete Confirmation", MessageBoxButtons.YesNo) == DialogResult.No : this.DialogResult == DialogResult.OK ? false : MessageBox.Show("Are you sure you want to cancel any changes you have made?", "Cancel Confirmation", MessageBoxButtons.YesNo) == DialogResult.No;
        }
    }
}
