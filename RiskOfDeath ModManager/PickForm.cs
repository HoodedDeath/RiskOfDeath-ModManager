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
    public partial class PickForm : Form
    {
        public KeyValuePair<string, string[]> SelectedKVP { get; private set; } = new KeyValuePair<string, string[]>();
        private int[] location = new int[] { 4, -28 };
        public PickForm(string title, Dictionary<string, string[]> options)
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            this.Text = title;
            foreach (KeyValuePair<string, string[]> kvp in options)
                this.panel1.Controls.Add(new KVPRadioBtn(kvp, location[0], location[1] += 32, this));
        }

        public void RadioChanged(object sender, EventArgs e)
        {
            bool b = false;
            foreach (KVPRadioBtn k in this.panel1.Controls)
                b |= k.Checked;
            this.acceptBtn.Enabled = b;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void AcceptBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            foreach (KVPRadioBtn k in this.panel1.Controls)
                if (k.Checked)
                    this.SelectedKVP = k.KVP;
            Close();
        }
    }

    class KVPRadioBtn : RadioButton
    {
        public KeyValuePair<string, string[]> KVP { get; private set; } = new KeyValuePair<string, string[]>();
        private PickForm parent;
        public KVPRadioBtn(KeyValuePair<string, string[]> kvp, int locX, int locY, PickForm parent)
        {
            InitializeComponent();
            this.Text = kvp.Key;
            this.KVP = kvp;
            this.Location = new Point(locX, locY);
            this.parent = parent;
        }

        private void InitializeComponent()
        {
            this.Name = "radioButton1";
            this.Size = new Size(382, 24);
            this.TabIndex = 0;
            this.TabStop = true;
            this.UseVisualStyleBackColor = true;
            this.AutoEllipsis = true;
            this.Checked = false;
            this.CheckedChanged += new EventHandler(this.Handler);
        }
        private void Handler(object sender, EventArgs e)
        {
            parent.RadioChanged(this, e);
        }
    }
}
