using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Co0nSearchC
{
    /// <summary>
    /// Einstellungsformular
    /// </summary>
    public partial class F_Settings : Form
    {

        private F_Main caller;
        public Boolean settingschanged=false;

        public F_Settings(object caller)
        {
            InitializeComponent();
            this.defineContextMenu();
            try
            {
                this.caller = (F_Main)caller;
                this.FillList();
            }
            catch (Exception ex)
            {
                //Dies sollte niemals passieren...
                MessageBox.Show("Die Einstellungen konnten nicht aus der Hauptmaske übernommen werden. Abbruch! \r\n\r\n" + ex.ToString() + "\r\n\r\n" + ex.StackTrace, "Schwerwiegender Fehler!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }  
            

        }
                
        private void FillList()
        {
            
            this.lstBaseDirs.Items.Clear();
            this.lstBaseDirs.Items.AddRange(caller.settings.BaseDirs.ToArray());    
           
        }

        private string _selectedMenuItem;
        private ContextMenuStrip collectionRoundMenuStrip;
        private void defineContextMenu()
        {
            var toolStripMenuItem1 = new ToolStripMenuItem { Text = "Ordner aktivieren/deaktivieren" };
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            var toolStripMenuItem2 = new ToolStripMenuItem { Text = "Ordner löschen" };
            toolStripMenuItem2.Click += toolStripMenuItem2_Click;
            collectionRoundMenuStrip = new ContextMenuStrip();
            collectionRoundMenuStrip.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2 });
            
        }

        private void lstBaseDirs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var index = lstBaseDirs.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                _selectedMenuItem = lstBaseDirs.Items[index].ToString();
                collectionRoundMenuStrip.Show(Cursor.Position);
                collectionRoundMenuStrip.Visible = true;
            }
            else
            {
                collectionRoundMenuStrip.Visible = false;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_selectedMenuItem);            
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_selectedMenuItem);
        }

        private void grpBaseDirs_Enter(object sender, EventArgs e)
        {

        }

        

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(this.txtFolder.Text))
            {
                caller.settings.AddBaseDir(this.txtFolder.Text);
                this.FillList();
                settingschanged = true;
            }
            else
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;                
                MessageBox.Show("Der angegebene Ordner \""+this.txtFolder.Text+"\" existiert nicht,\r\nes ist kein Datenträger eingelegt oder Ihnen fehlt die Berechtigung darauf zuzugreifen.\r\nDer Ordner wurde nicht übernommen!", "Ordner existiert nicht", buttons, MessageBoxIcon.Warning);
            }
        }

        private void btnSearchNewFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderselect = new FolderBrowserDialog();
            folderselect.ShowDialog();
            this.txtFolder.Text = folderselect.SelectedPath;

            settingschanged = true;

            this.btnAddFolder.PerformClick(); // Den Button zum übernehmen des Ordners anklicken... // Bei Bedarf auskommentieren.
        }

        private void F_Settings_FormClosing(object sender, FormClosingEventArgs e)
        {// Dialogfenster wird geschlossen

            if (settingschanged) { 
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                result = MessageBox.Show("Die Einstellungen wurden geändert.\r\nSollen diese übernommen werden?", "Einstellungen geändert.", buttons, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.caller.settings.putAllBaseDirs(); //Änderungen wirklich in die Registry schreiben
                    this.caller.ShouldReInitializeAfterSettingsChange = true;
                    
                }
                else
                {
                    this.caller.settings.getAllBaseDirs(); //Ursprünglichen Stand aus der Registry neu laden.
                }
            }
        }

        private void btnRemoveFolders_Click(object sender, EventArgs e)
        {
            String msgtext = "Möchten Sie wirklich " + this.lstBaseDirs.SelectedItems.Count +" Einträge entfernen?";

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(msgtext, "Einträge entfernen?", buttons, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                foreach (String item in this.lstBaseDirs.SelectedItems)
                {
                    this.caller.settings.removeBaseDir(item);
                }

                settingschanged = true;

                this.FillList();
            }

            
        }

        private void lstBaseDirs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

      
    }
}
