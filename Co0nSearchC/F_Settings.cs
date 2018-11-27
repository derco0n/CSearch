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

namespace CSearch
{
    /// <summary>
    /// Einstellungsformular
    /// </summary>
    public partial class F_Settings : Form
    {

        private F_Main caller;
        public Boolean settingschanged=false;
        private bool _ItemsListLoaded = false; //Indicates, that the listbox has loaded completly

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
            this._ItemsListLoaded = false;

            this.lstBaseDirs.Items.Clear();
            this.lstBaseDirs.Items.AddRange(caller.settings.BaseDirs.ToArray());

            for (int counter = 0; counter < this.lstBaseDirs.Items.Count; counter++)
            {
                C_BaseDir temp = (C_BaseDir)this.lstBaseDirs.Items[counter];
                this.lstBaseDirs.SetItemChecked(counter, temp.IsEnabled);
            }



            /*
            foreach (C_BaseDir basedir in this.lstBaseDirs.Items)
            {
                
                this.lstBaseDirs.SetItemChecked(counter, basedir.IsEnabled);
               
                counter++;
            }
            */
            this._ItemsListLoaded = true;


        }

        //private string _selectedMenuItem;
        private int _selectedListIndex;
        private ContextMenuStrip collectionRoundMenuStrip;
        private void defineContextMenu()
        { //defines the lstBox-Contextmenu
            //Define Items for the context, menu
            var toolStripMenuItem1 = new ToolStripMenuItem { Text = "Ordner aktivieren/deaktivieren" };
            toolStripMenuItem1.Click += toolStripMenuEnDisable_Click; //get event handler
            var toolStripMenuItem2 = new ToolStripMenuItem { Text = "Ordner löschen" };
            toolStripMenuItem2.Click += toolStripMenuDelete_Click; //get event handler


            collectionRoundMenuStrip = new ContextMenuStrip();
            collectionRoundMenuStrip.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2 }); //Add all menu entries to the context-menu
            
        }

        private void lstBaseDirs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {//Right Mousebutton->show context-menu
                var index = lstBaseDirs.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    //_selectedMenuItem = lstBaseDirs.Items[index].ToString();
                    _selectedListIndex=(int) index;
                    collectionRoundMenuStrip.Show(Cursor.Position);
                    collectionRoundMenuStrip.Visible = true;
                }
                else
                {
                    collectionRoundMenuStrip.Visible = false;
                }
            }
        }

        private void toolStripMenuEnDisable_Click(object sender, EventArgs e)
        {
            //Clipboard.SetText(_selectedMenuItem);            
            //Clipboard.SetText(lstBaseDirs.Items[this._selectedMenuIndex].ToString());
            CheckState currentstate = this.lstBaseDirs.GetItemCheckState(this._selectedListIndex);
            CheckState newstate = currentstate;
            //invert current state
            if (currentstate == CheckState.Checked)
            {
                newstate = CheckState.Unchecked;
                ItemCheckChanged(false);
            }
            else if (currentstate == CheckState.Unchecked)
            {
                newstate = CheckState.Checked;
                ItemCheckChanged(true);
            }
            this.lstBaseDirs.SetItemCheckState(this._selectedListIndex, newstate); //invert CheckState

            
        }

        private void ItemCheckChanged(Boolean toState) {

            //if (toState == true)
            //{
            C_BaseDir temp = (C_BaseDir)lstBaseDirs.Items[this._selectedListIndex];
            this.caller.settings.setState(temp, toState);
                //}
                settingschanged = true;
                this.FillList();
            
        }

        private void toolStripMenuDelete_Click(object sender, EventArgs e)
        {
            //Clipboard.SetText(_selectedMenuItem);
            C_BaseDir SelectedItem = (C_BaseDir)lstBaseDirs.Items[this._selectedListIndex];
            String msgtext = "Möchten Sie wirklich \"" + SelectedItem.Path + "\" entfernen?";

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(msgtext, "Eintrag entfernen?", buttons, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.caller.settings.removeBaseDir(SelectedItem);

                settingschanged = true;
                this.FillList();
            }

        }

        private void grpBaseDirs_Enter(object sender, EventArgs e)
        {

        }

        

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(this.txtFolder.Text))
            {

                caller.settings.AddBaseDir(new C_BaseDir(this.txtFolder.Text, true));
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
            if (this.lstBaseDirs.SelectedItems.Count > 0)
            {
                String msgtext = "Möchten Sie wirklich " + this.lstBaseDirs.SelectedItems.Count + " Einträge entfernen?";

                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                result = MessageBox.Show(msgtext, "Einträge entfernen?", buttons, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    foreach (C_BaseDir item in this.lstBaseDirs.SelectedItems)
                    {
                        this.caller.settings.removeBaseDir(item);
                    }

                    settingschanged = true;

                    this.FillList();
                }
            }

            
        }

        private void lstBaseDirs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstBaseDirs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this._ItemsListLoaded)
            { //Wait for List to be loaded completly before handling checkstate-changes

                this._selectedListIndex = e.Index;

                if (e.NewValue == CheckState.Checked)
                {
                    this.ItemCheckChanged(true);
                }
                else if (e.NewValue == CheckState.Unchecked)
                {
                    this.ItemCheckChanged(false);
                }
            }
            
        }
    }
}
