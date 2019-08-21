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
    public partial class MessageForm : Form
    {
        public MessageForm()
        {
            InitializeComponent();
        }
        public void Show(string text)
        {
            richTextBox1.Text = text;
            base.ShowDialog();
        }
        public void Show(string text, string title)
        {
            this.Text = title;
            Show(text);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
    }
}
