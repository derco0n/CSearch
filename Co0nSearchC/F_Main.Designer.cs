using Co0n_GUI;

namespace CSearch
{
    partial class F_Main
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Main));
            this.lstFiles = new System.Windows.Forms.ListBox();
            //this.txtSearch = new System.Windows.Forms.TextBox();
            this.txtSearch = new Co0n_GUI.C_HintTextbox("Bitte mindestens zwei Zeichen eingeben und mit Enter bestätigen.");
            this.lblCount = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.programmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.einstellungenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.suchordnerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.überToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.überDiesesProgrammToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changelogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.grpResults = new System.Windows.Forms.GroupBox();
            this.grpState = new System.Windows.Forms.GroupBox();
            this.lblState = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.grpResults.SuspendLayout();
            this.grpState.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstFiles
            // 
            this.lstFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.HorizontalScrollbar = true;
            this.lstFiles.Location = new System.Drawing.Point(3, 16);
            this.lstFiles.Margin = new System.Windows.Forms.Padding(2);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(1119, 532);
            this.lstFiles.Sorted = true;
            this.lstFiles.TabIndex = 0;
            this.lstFiles.SelectedIndexChanged += new System.EventHandler(this.lstFiles_SelectedIndexChanged);
            this.lstFiles.SelectedValueChanged += new System.EventHandler(this.lstFiles_SelectedValueChanged);
            this.lstFiles.DoubleClick += new System.EventHandler(this.lstFiles_DoubleClick);
            this.lstFiles.MouseHover += new System.EventHandler(this.lstFiles_MouseHover);
            this.lstFiles.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstFiles_MouseMove);
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Location = new System.Drawing.Point(3, 16);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(1119, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCount.Location = new System.Drawing.Point(3, 31);
            this.lblCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(105, 13);
            this.lblCount.TabIndex = 2;
            this.lblCount.Text = "Bisher keine Dateien";
            this.lblCount.Click += new System.EventHandler(this.lblCount_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programmToolStripMenuItem,
            this.einstellungenToolStripMenuItem,
            this.überToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1125, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // programmToolStripMenuItem
            // 
            this.programmToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.beendenToolStripMenuItem});
            this.programmToolStripMenuItem.Name = "programmToolStripMenuItem";
            this.programmToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.programmToolStripMenuItem.Text = "Programm";
            // 
            // beendenToolStripMenuItem
            // 
            this.beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
            this.beendenToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.beendenToolStripMenuItem.Text = "Beenden";
            this.beendenToolStripMenuItem.Click += new System.EventHandler(this.beendenToolStripMenuItem_Click);
            // 
            // einstellungenToolStripMenuItem
            // 
            this.einstellungenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.suchordnerToolStripMenuItem});
            this.einstellungenToolStripMenuItem.Name = "einstellungenToolStripMenuItem";
            this.einstellungenToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.einstellungenToolStripMenuItem.Text = "Einstellungen";
            // 
            // suchordnerToolStripMenuItem
            // 
            this.suchordnerToolStripMenuItem.Name = "suchordnerToolStripMenuItem";
            this.suchordnerToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.suchordnerToolStripMenuItem.Text = "Suchordner";
            this.suchordnerToolStripMenuItem.Click += new System.EventHandler(this.suchordnerToolStripMenuItem_Click);
            // 
            // überToolStripMenuItem
            // 
            this.überToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.überDiesesProgrammToolStripMenuItem,
            this.changelogToolStripMenuItem});
            this.überToolStripMenuItem.Name = "überToolStripMenuItem";
            this.überToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.überToolStripMenuItem.Text = "Über";
            // 
            // überDiesesProgrammToolStripMenuItem
            // 
            this.überDiesesProgrammToolStripMenuItem.Name = "überDiesesProgrammToolStripMenuItem";
            this.überDiesesProgrammToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.überDiesesProgrammToolStripMenuItem.Text = "Über dieses Programm";
            this.überDiesesProgrammToolStripMenuItem.Click += new System.EventHandler(this.überDiesesProgrammToolStripMenuItem_Click);
            // 
            // changelogToolStripMenuItem
            // 
            this.changelogToolStripMenuItem.Name = "changelogToolStripMenuItem";
            this.changelogToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.changelogToolStripMenuItem.Text = "Changelog";
            this.changelogToolStripMenuItem.Click += new System.EventHandler(this.changelogToolStripMenuItem_Click);
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.txtSearch);
            this.grpSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSearch.Location = new System.Drawing.Point(0, 24);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new System.Drawing.Size(1125, 52);
            this.grpSearch.TabIndex = 6;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Suche:";
            // 
            // grpResults
            // 
            this.grpResults.Controls.Add(this.lstFiles);
            this.grpResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpResults.Location = new System.Drawing.Point(0, 76);
            this.grpResults.Name = "grpResults";
            this.grpResults.Size = new System.Drawing.Size(1125, 551);
            this.grpResults.TabIndex = 7;
            this.grpResults.TabStop = false;
            this.grpResults.Text = "Ergebnisse:";
            this.grpResults.Enter += new System.EventHandler(this.grpResults_Enter);
            // 
            // grpState
            // 
            this.grpState.Controls.Add(this.lblState);
            this.grpState.Controls.Add(this.lblCount);
            this.grpState.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpState.Location = new System.Drawing.Point(0, 627);
            this.grpState.Name = "grpState";
            this.grpState.Size = new System.Drawing.Size(1125, 47);
            this.grpState.TabIndex = 8;
            this.grpState.TabStop = false;
            this.grpState.Text = "Status:";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblState.Location = new System.Drawing.Point(3, 16);
            this.lblState.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(75, 13);
            this.lblState.TabIndex = 3;
            this.lblState.Text = "Suche inaktiv.";
            // 
            // F_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1125, 674);
            this.Controls.Add(this.grpResults);
            this.Controls.Add(this.grpSearch);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.grpState);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(450, 500);
            this.Name = "F_Main";
            this.Text = "CSearch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            this.grpResults.ResumeLayout(false);
            this.grpState.ResumeLayout(false);
            this.grpState.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem programmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem einstellungenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem suchordnerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem überToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem überDiesesProgrammToolStripMenuItem;
        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.GroupBox grpResults;
        private System.Windows.Forms.GroupBox grpState;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem changelogToolStripMenuItem;
    }
}

