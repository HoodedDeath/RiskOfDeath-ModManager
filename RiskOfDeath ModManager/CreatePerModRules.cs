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
using Newtonsoft.Json;

namespace RiskOfDeath_ModManager
{
    public partial class CreatePerModRules : Form
    {
        public CreatePerModRules()
        {
            InitializeComponent();

            panel1.AutoScrollPosition = new Point(0, 0);
            panel1.Controls.Add(new PerModGrouping() { Location = new Point(location[0], location[1] += 56) });
        }
        private int[] location = new int[] { 3, -53 };
        int count = 0;
        private void Add()
        {
            if (((PerModGrouping)panel1.Controls[count]).IsComplete)
            {
                panel1.AutoScrollPosition = new Point(0, 0);
                panel1.Controls.Add(new PerModGrouping() { Location = new Point(location[0], location[1] += 56) });
                count++;
                if (count == 5)
                {
                    panel1.Size = new Size(panel1.Size.Width + 25, panel1.Size.Height);
                    this.Size = new Size(this.Size.Width + 25, this.Size.Height);
                }
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Add();
        }
        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.AutoScrollPosition = new Point(0, 0);
            panel1.Controls.Clear();
            if (count >= 5)
            {
                panel1.Size = new Size(panel1.Size.Width - 25, panel1.Size.Height);
                this.Size = new Size(this.Size.Width - 25, this.Size.Height);
            }
            count = 0;
            location = new int[] { 3, -53 };
            panel1.Controls.Add(new PerModGrouping() { Location = new Point(location[0], location[1] += 56) });
        }

        private void ImportFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool worry;
            if (count > 0)
                worry = ((PerModGrouping)panel1.Controls[0]).Mod != "" && ((PerModGrouping)panel1.Controls[0]).Game != "";
            else
                worry = false;
            if (worry) worry = !(MessageBox.Show("Importing will clear any items you have entered already. Are you sure you want to continue?", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes);
            if (!worry)
            {
                OpenFileDialog d = new OpenFileDialog
                {
                    Filter = "Json (*.json)|*.json",
                    FileName = "rules.json",
                    Title = "Choose your rules file"
                };
                DialogResult res = d.ShowDialog();
                if (res == DialogResult.OK)
                {
                    ClearToolStripMenuItem_Click(this, EventArgs.Empty);
                    StreamReader sr = new StreamReader(d.OpenFile());
                    Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(sr.ReadToEnd());
                    sr.Close();
                    sr.Dispose();
                    foreach (KeyValuePair<string, string> kvp in dict)
                    {
                        ((PerModGrouping)panel1.Controls[count]).Mod = kvp.Key;
                        ((PerModGrouping)panel1.Controls[count]).Game = kvp.Value;
                        Add();
                    }
                }
                d.Dispose();
            }
        }

        private void SaveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (PerModGrouping g in panel1.Controls)
                if (g.IsComplete)
                    dict.Add(g.Mod, g.Game);
            if (dict.Count > 0)
            {
                SaveFileDialog d = new SaveFileDialog
                {
                    Filter = "Json (*.json)|*.json",
                    FileName = "rules.json",
                    Title = "Save your rules file"
                };
                DialogResult res = d.ShowDialog();
                if (res == DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(d.OpenFile());
                    sw.WriteLine(JsonConvert.SerializeObject(dict, Formatting.Indented));
                    sw.Close();
                    sw.Dispose();
                }
                d.Dispose();
            }
            else
                MessageBox.Show("No values to save.");
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = "This form will allow you to easily write custom rules for how your mod should be installed.\n\nMod Folder: The folder inside the compressed file of your mod. All contents of this folder will be copied to the folder inside Risk of Rain 2 or BepInEx directory given in Game Folder.\n\nGame Folder: The folder inside which the contents of Mod Folder will be copied to.\n\nValid folders and corresponding path:\n\t- BepInEx - <RoR2 installation>/BepInEx - Works well if your folder contains the same folder structure as the BepInEx folder\n\t- plugins - <RoR2 installation>/BepInEx/plugins - the plugins folder inside BepInEx, where most mod files need to go\n\t- patchers - <RoR2 installation>/BepInEx/patchers - These are more advanced types of plugins that need to access Mono.Cecil to edit .dll files during runtime\n\t- monomod - <RoR2 installation>/BepInEx/monomod - MonoMod patches get placed in here\n\t- core -<RoR2 installation>/BepInEx/core - Core BepInEx .dll files. Your mod likely does not need to go here\n\t- data - <RoR2 installation>/Risk of Rain 2_Data - For some asset replacement mods\n\t- language - <RoR2 installation>/Risk of Rain 2_Data/Language - Language text files for replacing text in the game\n\t- EN-US - <RoR2 installation>/Risk of Rain 2_Data/Language/en - Text replacement for English\n\t- managed - <RoR2 installation>/Risk of Rain 2_Data/Managed\n\n!! Note !!\nIf you're going to declare custom rules for your mod, please make sure all the folders in the base directory of your mod have rules and there are no loose files in the base directory that are needed for the mod. The rules you declare will cause the download routine to only act on folders given in your rules.\n\n!! When you don't need to worry about any of this !!\n\t- When your mod's compressed file contains the dll files for your mod with no subfolders, these dll files will be assumed to belong in BepInEx/plugins\n\t- When your compressed file contains only folders following the folder structure in either BepInEx or Risk of Rain 2_Data (example: when your file contains only folders like \"Risk of Rain 2_Data\", \"BepInEx\", \"plugins\", etc.)\n\t- When your compressed file contains a folder (name of this folder does not matter) which contains either just dll files that belong in BepInEx/plugins, or a folder named the same as a folder in the BepInEx folder";
            new MessageForm().Show(s, "Mod rules help");
        }
    }
    public class PerModGrouping : GroupBox
    {
        private readonly Label modLabel = new Label();
        private readonly TextBox gameTextBox = new TextBox();
        private readonly Label gameLabel = new Label();
        private readonly TextBox modTextBox = new TextBox();

        public string Mod { get { return modTextBox.Text.Trim() ?? ""; } set { modTextBox.Text = value; } }
        public string Game { get { return gameTextBox.Text.Trim() ?? ""; } set { gameTextBox.Text = value; } }
        public bool IsEmpty { get { return this.Mod == "" || this.Game == ""; } }
        public bool IsComplete { get { return this.Mod != "" && this.Game != ""; } }

        public PerModGrouping()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // modLabel
            // 
            this.modLabel.AutoSize = true;
            this.modLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.modLabel.Location = new Point(6, 22);
            this.modLabel.Name = "modLabel";
            this.modLabel.Size = new Size(63, 13);
            this.modLabel.TabIndex = 0;
            this.modLabel.Text = "Mod Folder:";
            // 
            // modTextBox
            // 
            this.modTextBox.Location = new Point(75, 19);
            this.modTextBox.Name = "modTextBox";
            this.modTextBox.Size = new Size(100, 20);
            this.modTextBox.TabIndex = 3;
            // 
            // gameTextBox
            // 
            this.gameTextBox.Location = new Point(257, 19);
            this.gameTextBox.Name = "gameTextBox";
            this.gameTextBox.Size = new Size(100, 20);
            this.gameTextBox.TabIndex = 5;
            // 
            // gameLabel
            // 
            this.gameLabel.AutoSize = true;
            this.gameLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gameLabel.Location = new Point(181, 22);
            this.gameLabel.Name = "gameLabel";
            this.gameLabel.Size = new Size(70, 13);
            this.gameLabel.TabIndex = 4;
            this.gameLabel.Text = "Game Folder:";
            // 
            // groupBox1
            // 
            this.Controls.Add(this.gameTextBox);
            this.Controls.Add(this.modLabel);
            this.Controls.Add(this.gameLabel);
            this.Controls.Add(this.modTextBox);
            this.Location = new Point(590, 262);
            this.Name = "groupBox1";
            this.Size = new Size(364, 50);
            this.TabIndex = 2;
            this.TabStop = false;
            //
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
