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
        public F_Main()
        {
            InitializeComponent();
        }

        public C_Settings settings;
        private List<C_FilesIndexer> indexers = new List<C_FilesIndexer>();
        public Boolean ShouldReInitializeAfterSettingsChange = false; // Ween true, sollten die Sucher reinitialisiert werden. Einstellungen wurde geändert. Wird in anderer F_Settings gesetzt.

        private System.DateTime SearchStarted, SearchEnded; // Für Benchmarking

        private int _items = 0;
        private int _runningthreads = 0;

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

            this.updateFileListAndLabels(false, newitems, "Bisher " + processedfolders + " Ordner durchsucht -> " + this._items.ToString() + " Elemente gefunden...", "Suche läuft (in " + this._runningthreads.ToString() + " Basisordnern):");
        }

        private void HandleSearchfinished(object sender, String msg)
        { // Wird aufgerufen wenn ein Sucher fertig ist.

            this._runningthreads -= 1;


            Boolean stillonerunning = false; //Mindestens ein Sucher läuft noch
            //prüfen ob noch mindestens ein Sucher läuft...
            if (this._runningthreads > 0)
            {
                stillonerunning = true;
                String state = "Suche läuft(in " + this._runningthreads.ToString() + " Basisordnern):";
            }


            if (!stillonerunning) // Wenn kein Sucher mehr läuft...
            {

                this.SearchEnded = DateTime.Now;
                TimeSpan ts = this.SearchEnded - this.SearchStarted;

                String state = "Suche beendet - Ausführungsdauer (" + ts.TotalSeconds.ToString() + " Sekunden):";

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
            this.lstFiles.Items.Clear();

            foreach (C_FilesIndexer indexer in this.indexers)
            {

                this._runningthreads += 1;
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
                        this.indexers.Add(new C_FilesIndexer(@BaseDir));
                    }

                    foreach (C_FilesIndexer indexer in this.indexers)
                    {
                        indexer.OnItemsFound += this.HandleItemsFound;
                        indexer.OnSearchFinished += this.HandleSearchfinished;
                        indexer.OnSearchStarted += this.HandleSearchStarted;
                        indexer.OnFolderProcessed += this.HandleFolderProcessed;
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
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void StopSeachers()
        {//Beendet laufende Suchen
            if (this.indexers != null)
            {
                this._runningthreads = 0;
                foreach (C_FilesIndexer indexer in this.indexers)
                {
                    if (indexer != null)
                    {
                        indexer.StopSearch();
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
            this.StopSeachers();

            if (this.txtSearch.Text.Length > 3)
            {
                this.lblCount.Text = "Neue Suche...";
                this.startSearch(this.txtSearch.Text);
            }
        }


        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show("Programm wirklich beeenden?", "Beenden?", buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                this.StopSeachers();
                this.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
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
            }

        }

        private void überDiesesProgrammToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form AboutForm = new F_About();
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

        private void lblCount_Click(object sender, EventArgs e)
        {

        }
    }
}
