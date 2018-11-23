using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace Co0nSearchC
{
    public partial class F_Main : Form
    {
        public F_Main(bool showhiddenfiles)
        {
            this._showhiddenfiles = showhiddenfiles;
            InitializeComponent();
        }

        private bool _showhiddenfiles = false; // Find hiddenfiles too...

        public C_Settings settings;
        private List<C_FilesIndexer> indexers = new List<C_FilesIndexer>();
        public Boolean ShouldReInitializeAfterSettingsChange = false; // Wenn true, sollten die Sucher reinitialisiert werden. Einstellungen wurde geändert. Wird in anderer F_Settings gesetzt.

        private System.DateTime SearchStarted, SearchEnded; // Für Benchmarking

        private int _items = 0;
        private int _runningthreads = 0;

        // private List<C_FilesIndexerElement> filesfound = new List<C_FilesIndexerElement>(); //DEBUG
        

        private void HandleFolderProcessed(object sender)
        {// Aktualisiert die Anzeige der berabeiteten Ordner
            int processedfolders = 0;
            foreach (C_FilesIndexer indexer in this.indexers)
            {
                processedfolders += indexer.foldersProcessedsoFar;
            }
            this.updateCountLabel("Bisher " + processedfolders.ToString() + " Ordner durchsucht -> " + this._items.ToString() + " Elemente gefunden...");
        }

        //private void HandleItemsFound(object sender, List<String> newitems)
        private void HandleItemsFound(object sender, List<C_FilesIndexerElement> newitems)
        {// Wird jedesmal aufgerufen wenn ein Sucher neue Objekte gefunden hat.
            newitems.Sort();
            this._items = this._items + newitems.Count();

            int processedfolders = 0;
            foreach (C_FilesIndexer indexer in this.indexers)
            {
                processedfolders += indexer.foldersProcessedsoFar;
            }

            /*
            //DEBUG
            if (!this.alreadyInList(this.filesfound, newitems))
            {
                this.filesfound.AddRange(newitems);
            }
            else
            {

            }
            //DEBUG ENDE
            */

            this.updateFileListAndLabels(false, newitems, "Bisher " + processedfolders + " Ordner durchsucht -> " + this._items.ToString() + " Elemente gefunden...", "Suche läuft (in " + this._runningthreads.ToString() + " Basisordnern):");
        }

        /// <summary>
        /// DEBUG: Check for duplicates
        /// </summary>
        /// <param name="list1">list to check in</param>
        /// <param name="list2">list to check for</param>
        /// <returns></returns>
        private bool alreadyInList(List<C_FilesIndexerElement> list1, List<C_FilesIndexerElement> list2)
        {
            foreach (C_FilesIndexerElement elem in list2)
            {
                bool exists= list1.Any(item => item.Name.Equals(elem.Name) && item.Type==item.Type);
                if (exists)
                {
                    return true;
                }
            }

            return false;
        }

        private void HandleSearchAborted(object sender)
        {//Der Thread des Suchers wurde abgebrochen...            
                this._runningthreads--;           
        }

        private void HandleSearchfinished(object sender, String msg)
        { // Wird aufgerufen wenn ein Sucher fertig ist.

            this._runningthreads--;


            Boolean stillonerunning = false; //Mindestens ein Sucher läuft noch
                                             //prüfen ob noch mindestens ein Sucher läuft...
            String state = "";
            if (this._runningthreads > 0)
            {
                stillonerunning = true;
                state = "Suche läuft(in " + this._runningthreads.ToString() + " Basisordnern):";
            }
            else
            {
                state = "Suche beendet - Bereite Ergebnis auf:";
            }


            if (!stillonerunning) // Wenn kein Sucher mehr läuft...
            {

                this.SearchEnded = DateTime.Now;
                TimeSpan ts = this.SearchEnded - this.SearchStarted;

                state = "Suche beendet - Ausführungsdauer (" + Math.Round(ts.TotalSeconds, 2).ToString() + " Sekunden):";
                //state = "Suche beendet - Ausführungsdauer (" + ts.TotalSeconds.ToString() + " Sekunden):";

                int processedfolders = 0;

                List<C_FilesIndexerElement> totalitemsfound = new List<C_FilesIndexerElement>();
                foreach (C_FilesIndexer indexer in this.indexers)
                {
                    processedfolders += indexer.foldersProcessedsoFar;
                    totalitemsfound.AddRange(indexer.FoundItems);
                }



                string result = processedfolders.ToString() + " Ordner durchsucht -> " + totalitemsfound.Count.ToString() + " Elemente gefunden...";

                this.updateFileListAndLabels(true, totalitemsfound, result, state);


            }

        }

        private void HandleSearchStarted(object sender, String msg)
        {
            this.SearchStarted = DateTime.Now;
            
            this._runningthreads++;
        }

        delegate void updateFileListAndLabelsCallback(bool clearlist, List<C_FilesIndexerElement> items, string resultmsg, string statemsg);
        private void updateFileListAndLabels(bool clearlist, List<C_FilesIndexerElement> items, string resultmsg, string statemsg)
        {//Aktualisiert die Dateiliste
            //Falls ein Invoke nötig ist (weil anderer Thread) wird dies entsprechend durchgeführt.
            if (this.lstFiles.InvokeRequired)
            {
                //Invoke nötig
                updateFileListAndLabelsCallback c = new updateFileListAndLabelsCallback(updateFileListAndLabels);
                this.Invoke(c, new object[] { clearlist, items, resultmsg, statemsg });

            }
            else
            {
                //Invoke nicht nötig
                if (clearlist)
                {
                    this.lstFiles.Items.Clear();
                }

                this.lstFiles.Items.AddRange(items.ToArray());

                //Labels aktualisieren
                this.updateCountLabel(resultmsg);
                this.updateStateLabel(statemsg);
            }

        }

        delegate void updatelabelCallback(string msg);
        private void updateStateLabel(string msg)
        {
            if (this.lblState.InvokeRequired)
            {
                //Invoke nötig
                updatelabelCallback c = new updatelabelCallback(updateStateLabel);
                this.Invoke(c, new object[] { msg });

            }
            else
            {
                this.lblState.Text = msg;
            }
        }

        private void updateCountLabel(string msg)
        {
            if (this.lblCount.InvokeRequired)
            {
                //Invoke nötig
                updatelabelCallback c = new updatelabelCallback(updateCountLabel);
                this.Invoke(c, new object[] { msg });

            }
            else
            {
                this.lblCount.Text = msg;
            }
        }

        private void startSearch(String searchfor)
        {
            //this.filesfound=new List<C_FilesIndexerElement>(); //DBEUG
            this.lstFiles.Items.Clear();
            this._items = 0;

            foreach (C_FilesIndexer indexer in this.indexers)
            {

                //this._runningthreads += 1;
                indexer.FindItems(searchfor, true);
            }

            //indexers[0].FindItems(searchfor, true); //TODO: 

        }

        private void getSettings()
        {
            this.settings = new C_Settings();
            this.settings.getAllBaseDirs();
        }

        private void intializeIndexers()
        {//Sucher initialisieren
            this.StopSeachers(); //Laufende Suchen beenden
            this.indexers.Clear(); //Liste der Sucher leeren


            if (this.settings != null)
            {
                if (this.settings.BaseDirs.Count == 0)
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;

                    result = MessageBox.Show("Es sind keine Suchordner definiert. Möchten Sie dies jetz tun?", "Kein Suchordner...", buttons);
                    if (result == DialogResult.Yes)
                    {
                        this.openSettingsDialog();
                    }
                }
                else
                {
                    foreach (String BaseDir in this.settings.BaseDirs)
                    { //Für jedes Basisverzeichnis einen Sucher initilisieren.
                        this.indexers.Add(new C_FilesIndexer(@BaseDir, this._showhiddenfiles));
                    }

                    foreach (C_FilesIndexer indexer in this.indexers)
                    {
                        indexer.OnItemsFound += this.HandleItemsFound;
                        indexer.OnSearchFinished += this.HandleSearchfinished;
                        indexer.OnSearchStarted += this.HandleSearchStarted;
                        indexer.OnFolderProcessed += this.HandleFolderProcessed;
                        indexer.OnSearchAborted += this.HandleSearchAborted;
                    }

                }
            }
            this.ShouldReInitializeAfterSettingsChange = false;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        { // Start der Mainform
            this.getSettings();
            intializeIndexers();

            this.Text = Program.APPNAME + " Version: " + Program.VERSION.ToString() + " (" + Program.VERSIONDATE + ")";
            if (this._showhiddenfiles)
            {
                this.Text += " (include hidden)";
            }
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void StopSeachers()
        {//Beendet laufende Suchen

            this.updateCountLabel("Bisher keine Daten.");
            this.updateStateLabel("Suche inaktiv: Bitte mindestens 2 Zeichen eingeben.");

            if (this.indexers != null)
            {
                //this._runningthreads = 0;
                foreach (C_FilesIndexer indexer in this.indexers)
                {
                    if (indexer != null)
                    {
                        indexer.StopSearch();
                        //this._runningthreads -= 1;
                    }
                }

            }
        }

        /*
        private void DeRegisterEvents()
        {
            foreach (C_FilesIndexer indexer in this.indexers)
            {
                if (indexer != null)
                {                    
                    indexer.OnItemsFound -= this.HandleItemsFound;
                    indexer.OnSearchFinished -= this.HandleSearchfinished;
                    indexer.OnSearchStarted -= this.HandleSearchStarted;
                    indexer.OnFolderProcessed -= this.HandleFolderProcessed;
                }
            }
            
        }
        */

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            
            
        }


        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {// Catches the Form closing Event to ask the user to really quit.
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show("Programm wirklich beeenden?", "Beenden?", buttons);
            if (result == DialogResult.No)
            {
                // cancel the closure of the form.
                e.Cancel = true;
            }
            else
            {
                this.StopSeachers();
            }
        }

        private void suchordnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.openSettingsDialog();
        }

        private void openSettingsDialog()
        {// Zeigt den Einstellungdialog an...

            Form SettingsForm = new F_Settings(this);

            SettingsForm.ShowDialog(); // Als Einstellungsdialog. Andere Forms sind nicht bedienbar, während dieser geöffnet ist.
            //SettingsForm.Show(); // Als separate Form. Andere Forms können gleichzeitig bedient werden.

            if (this.ShouldReInitializeAfterSettingsChange)
            {                
                this.intializeIndexers(); //Sucher neu intialisieren
                this.txtSearch.Text = ""; //Reset Textfield, due to Basefolder change
            }

        }

        private void überDiesesProgrammToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String title = "Über dieses Programm:";
            String msg = Program.APPNAME + " Version: " + Program.VERSION.ToString() + " (" + Program.VERSIONDATE + ")" + "\r\n\r\nCSearch: dient der rekursiven multithreaded Suche nach Dateien und Ordnern in verschiedenen Basisordnern.\r\nDas Ziel ist es, eine schnelle Suche in verschiedenenn Ordnerstrukturen gleichzeitig zu ermöglichen\r\nund dabei ein einfacheres Handling und eines bessere Geschwindigkeit als die integrierte Windowssuche (welche viel detaillierter sucht, aber wesentlich mehr Zeit benötigt) zu bieten.\r\n\r\nCopyright (C) <2018>  <Dennis Marx>\r\n\r\n    This program is free software: you can redistribute it and/or modify\r\n    it under the terms of the GNU General Public License as published by\r\n    the Free Software Foundation, either version 3 of the License, or\r\n    (at your option) any later version.\r\n\r\n    This program is distributed in the hope that it will be useful,\r\n    but WITHOUT ANY WARRANTY; without even the implied warranty of\r\n    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the\r\n    GNU General Public License for more details.\r\n\r\n    You should have received a copy of the GNU General Public License\r\n    along with this program.  If not, see <https://www.gnu.org/licenses/>.\r\n\r\nQuellcode unter:\r\nhttps://github.com/derco0n/CSearch";
            Form AboutForm = new F_About(msg, title);
            AboutForm.ShowDialog();
        }

       

        private void lstFiles_MouseHover(object sender, EventArgs e)
        {
                    }

        private void lstFiles_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void lstFiles_DoubleClick(object sender, EventArgs e)
        {// Die Listbox wurde doppelt angeklickt (und dadurch ein Element auch selektiert). Versuchen den Ordner des Elements zu öffnen....
            try
            {
                C_FilesIndexerElement current = (C_FilesIndexerElement)lstFiles.Items[lstFiles.SelectedIndex];

                if (current.Type == C_FilesIndexerElement.TYPE_FILE)
                {
                    Process.Start(current.folderInfo.Parent.FullName); //Den übergeordneten Pfad des Objekts mit dem Standardprogramm (Explorer) öffnen.
                }
                else if (current.Type == C_FilesIndexerElement.TYPE_FOLDER)
                {
                    Process.Start(current.folderInfo.FullName); //Den Pfad des Objekts mit dem Standardprogramm (Explorer) öffnen.
                }


            }
            catch (Exception ex)
            {

            }

        }

        private void grpResults_Enter(object sender, EventArgs e)
        {

        }

        private void lstFiles_MouseMove(object sender, MouseEventArgs e)
        {
            //Mousepoint hovers Listbox:
            //Determine Item by cursor-position and do something with it...

            /*
            try
            {
                ListBox objListBox = (ListBox)sender;
                int itemIndex = -1;
                
                    if (objListBox.ItemHeight != 0)
                    {
                        itemIndex = e.Y / objListBox.ItemHeight;
                        itemIndex += objListBox.TopIndex;
                    }

                if (itemIndex >= 0)
                {
                    C_FilesIndexerElement highlightedelement = (C_FilesIndexerElement)lstFiles.Items[itemIndex];
                    ToolTip newToolTip = new ToolTip();
                    newToolTip.ShowAlways = true;
                    newToolTip.UseFading = true;
                    newToolTip.Show(highlightedelement.fileInfo.LastWriteTime.ToLocalTime().ToString(), lstFiles);
                }
                
            }
            catch (Exception ex)
            {
            }
            */
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Enter pressed
                this.StopSeachers();

                //Wait for SearchThreads to stop
                foreach (C_FilesIndexer indexer in this.indexers)
                {
                    if (indexer._SearchThread != null && indexer._SearchThread.ThreadState == System.Threading.ThreadState.Running)
                    {
                        indexer._SearchThread.Join();
                    }

                }

                if (this.txtSearch.Text.Length >= 2)
                {
                    this.updateCountLabel("Bisher keine Daten.");
                    this.updateStateLabel("Suche beginnt...");
                    this.startSearch(this.txtSearch.Text);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void changelogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String title = "Changelog:";
            String msg = "";
            msg+="Version 0.144 (20181123):\r\n=========================\r\n- Added:\r\n\t- Changelog\r\n- fixed Bugs:\r\n\t- Itemlist behind statusbar\r\n";
            Form AboutForm = new F_About(msg, title);
            AboutForm.ShowDialog();
        }

        private void lblCount_Click(object sender, EventArgs e)
        {

        }
    }
}
