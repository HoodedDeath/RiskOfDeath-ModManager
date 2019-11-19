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
    public partial class TextBoxDialogBox : Form
    {
        public string Result { get { return this.textBox1.Text; } }

        public TextBoxDialogBox()
        {
            InitializeComponent();
        }

        public new DialogResult Show()
        {
            return base.ShowDialog();
        }
        public DialogResult Show(string caption)
        {
            this.Text = caption;
            return base.ShowDialog();
        }

        private void Accept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    Accept_Click(null, null);
                    break;
                case Keys.Escape:
                    Cancel_Click(null, null);
                    break;
            }
        }
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK && (this.textBox1.Text == null || this.textBox1.Text.Trim() == ""))
            {
                MessageBox.Show("Please enter a value", "No Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
        }
    }
}
