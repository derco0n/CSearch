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
using Co0nUtilZ;

namespace CSearch
{
    public partial class F_Main : Form
    {
        public F_Main(bool showhiddenfiles, bool preview)
        {
            this._showhiddenfiles = showhiddenfiles;
            this._showpreview = preview;
            this.txtSearch = new Co0n_GUI.C_HintTextbox();
            InitializeComponent();

            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.ForeColor = System.Drawing.Color.Gray;
            this.txtSearch.Location = new System.Drawing.Point(3, 16);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceHolderText = "Bitte mindestens zwei Zeichen eingeben und mit Enter bestätigen.";
            this.txtSearch.Size = new System.Drawing.Size(1119, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.Text = "Bitte mindestens zwei Zeichen eingeben und mit Enter bestätigen.";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            this.grpSearch.Controls.Add(this.txtSearch);
        }

        
        
        private Color OriginalBackgroundColor;
        private bool _showhiddenfiles = false; // Find hiddenfiles too...
        private bool _showpreview = false; 

        public C_Settings settings;
        private List<C_FilesIndexer> indexers = new List<C_FilesIndexer>();
        public Boolean ShouldReInitializeAfterSettingsChange = false; // Wenn true, sollten die Sucher reinitialisiert werden. Einstellungen wurde geändert. Wird in anderer F_Settings gesetzt.

        private System.DateTime SearchStarted, SearchEnded; // Für Benchmarking

        private int _items = 0;
        private int _runningthreads = 0;

        // private List<C_FilesIndexerElement> filesfound = new List<C_FilesIndexerElement>(); //DEBUG


        private void setTitle() {
            this.Text = Program.APPNAME + " Version: " + Program.VERSION.ToString() + " (" + Program.VERSIONDATE + ")";
            if (this._showhiddenfiles)
            {
                this.Text += " (include hidden)";
            }
            if (this._showpreview)
            {
                this.Text += " (showing preview)";
            }
            else
            {
                this.Text += " (fast mode - no preview)";
            }

        }

        private void setMenuPreview() {
            if (this._showpreview)
            {
                this.vorschauToolStripMenuItem.Text = "Vorschau abschalten (schneller Modus)...";
            }
            else
            {
                this.vorschauToolStripMenuItem.Text = "Vorschau einschalten (langsam)...";
            }
            
        }

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

            float averange = (float)processedfolders / (float)this._items;

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

                string result = "";
                float averange = (float)processedfolders / (float)Math.Round(ts.TotalSeconds, 2);
                if (totalitemsfound.Count > 0)
                {                    
                    result = processedfolders.ToString() + " Ordner durchsucht (" + Math.Round(averange, 0).ToString() + " Ordner/Sekunde) -> " + totalitemsfound.Count.ToString() + " Elemente gefunden...";
                }
                else {
                    result = processedfolders.ToString() + " Ordner durchsucht (" + Math.Round(averange, 0).ToString() + " Ordner/Sekunde) -> leider keine Elemente gefunden...";
                }

                

                this.updateFileListAndLabels(true, totalitemsfound, result, state);

                //Suchinfo's einfärben
                this.lblState.BackColor = Color.LightGreen;
                this.lblCount.BackColor = Color.LightGreen;
            }
           

        }

        private void HandleSearchStarted(object sender, String msg)
        {//Wird beim Start einer neuen Suche (von jedem Thread) aufgerufen                

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

                if (this._showpreview || clearlist)
                {//Wenn die Suche beendet ist oder der User Suchergebnisse angezeigt haben möchte
                    this.lstFiles.Items.AddRange(items.ToArray());
                    this.lstFiles.ClearSelected();
                }

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
        /// <summary>
        /// Startet einen neue Suche
        /// </summary>
        /// <param name="searchfor"></param>
        private void startSearch(String searchfor)
        {
            //this.filesfound=new List<C_FilesIndexerElement>(); //DEBUG
            this.lstFiles.Items.Clear();
            this._items = 0;

            foreach (C_FilesIndexer indexer in this.indexers)
            {

                //this._runningthreads += 1;
                indexer.FindItems(searchfor, true);
                this.lblState.Text = "Suche läuft(in " + this._runningthreads.ToString() + " Basisordnern):";
            }

            //Suchinfo's einfärben
            this.lblState.BackColor = Color.Yellow;
            this.lblCount.BackColor = Color.Yellow;

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
                    foreach (C_BaseDir BaseDir in this.settings.BaseDirs)
                    { //Für jedes aktive Basisverzeichnis einen Sucher initilisieren.
                        if (BaseDir.IsEnabled)
                        {
                            this.indexers.Add(new C_FilesIndexer(@BaseDir.Path, this._showhiddenfiles));
                        }
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
            this.OriginalBackgroundColor = this.lblState.BackColor;

            this.setTitle();
            this.setMenuPreview();
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

                //Wait for SearchThreads to stop
                foreach (C_FilesIndexer indexer in this.indexers)
                {
                    if (indexer._SearchThread != null && indexer._SearchThread.ThreadState == System.Threading.ThreadState.Running)
                    {
                        indexer._SearchThread.Join();
                    }

                }

            }
            //Suchinfo's einfärben
            this.lblState.BackColor = this.OriginalBackgroundColor;
            this.lblCount.BackColor = this.OriginalBackgroundColor;
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

            /* //Commented out, because it prevents Windows 10 from shutting down... :/ Thx Microsoft.
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
            */

            this.StopSeachers();
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
            String msg = Program.APPNAME + " Version: " + Program.VERSION.ToString() + " (" + Program.VERSIONDATE + ")" + "\r\n\r\nCSearch: dient der rekursiven multithreaded Suche nach Dateien und Ordnern in verschiedenen Basisordnern.\r\nDas Ziel ist es, eine schnelle Suche in verschiedenenn Ordnerstrukturen gleichzeitig zu ermöglichen\r\nund dabei ein einfacheres Handling und eines bessere Geschwindigkeit als die integrierte Windowssuche (welche viel detaillierter sucht, aber wesentlich mehr Zeit benötigt) zu bieten.\r\n\r\nDeveloped by <Dennis Marx> <2019>\r\n\r\n    This program is free software: you can redistribute it and/or modify\r\n    it under the terms of the GNU General Public License as published by\r\n    the Free Software Foundation, either version 3 of the License, or\r\n    (at your option) any later version.\r\n\r\n    This program is distributed in the hope that it will be useful,\r\n    but WITHOUT ANY WARRANTY; without even the implied warranty of\r\n    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the\r\n    GNU General Public License for more details.\r\n\r\n    You should have received a copy of the GNU General Public License\r\n    along with this program.  If not, see <https://www.gnu.org/licenses/>.\r\n\r\nQuellcode unter:\r\nhttps://github.com/derco0n/CSearch";
            Form AboutForm = new F_About(msg, title);
            AboutForm.ShowDialog();
        }

       

        private void lstFiles_MouseHover(object sender, EventArgs e)
        {
            this.lstFiles.Focus();
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
           // if (this._runningthreads == 0) { 
                //Nur wenn kein Suchthread läuft...

                //Index des Item unterm Mauszeiger ermitteln
                Point point = lstFiles.PointToClient(Cursor.Position);
                int index = lstFiles.IndexFromPoint(point);
                if (index < 0)
                {
                    return;
                }

                //Wenn der Index positiv ist, also existiert, dieses Element auswählen...
                //...wodurch es markiert wird...
                lstFiles.SelectedIndex = index;
                

            //}

            
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {//Wenn im Suchfeld jemand ENTER drückt
                //Enter pressed
                this.StopSeachers(); //Laufende Suchthreads beenden                

                if (this.txtSearch.Text.Length >= 2)
                {// Wenn der neue Suchbgeriff mindestens 2 Zeichen hat, neue Suche starten.
                    this.updateCountLabel("Bisher keine Daten.");
                    this.updateStateLabel("Suche beginnt...");
                    this.startSearch(this.txtSearch.Text);
                }
                //Verhindern dass ENTER weitere Auswirkungen hat (erneute Ereignisbehandlung oder Zeilenumbrug um Suchstring)
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void changelogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String title = "Changelog:";
            String msg = "";
            msg += "Version 0.162 (20190927):\r\n=========================\r\n- fixed Bugs:\r\n\t- Setting Labels to green when all (not just one) searchers are finished.\r\n- Added:\r\n\t- Massive speedup (about 295%) due to disabled Preview\r\n\t- Preview toggleable in menu\r\n\r\n";
            msg += "Version 0.160 (20190904):\r\n=========================\r\n- Added:\r\n\t- Averange folders per second\r\n\t- Colored Statustext while searching\r\n\t- Highlighting Listelement while pointing with Mouse\r\n\r\n";
            msg += "Version 0.151 (20181126):\r\n=========================\r\n- fixed Bugs:\r\n\t- Stopping searchers (e.g. when changing folders) and waiting for them to finish \r\n\r\n";
            msg += "Version 0.150 (20181126):\r\n=========================\r\n- Added:\r\n\t- En-/Disabling of Searchdirectories\r\n- fixed Bugs:\r\n\t- Fixed wrong namespaces in source code\r\n\r\n";
            msg += "Version 0.144 (20181123):\r\n=========================\r\n- Added:\r\n\t- Changelog\r\n- fixed Bugs:\r\n\t- Itemlist behind statusbar\r\n";
            Form AboutForm = new F_About(msg, title);
            AboutForm.ShowDialog();
        }

        private void vorschauToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._showpreview = !this._showpreview; //Toggle Preview on/off
            this.setTitle();
            this.setMenuPreview();
        }

        private void lblCount_Click(object sender, EventArgs e)
        {

        }
    }
}
