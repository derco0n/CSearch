namespace CSearch
{
    public partial class F_Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpBaseDirs = new System.Windows.Forms.GroupBox();
            this.btnRemoveFolders = new System.Windows.Forms.Button();
            this.btnAddFolder = new System.Windows.Forms.Button();
            this.btnSearchNewFolder = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.lstBaseDirs = new System.Windows.Forms.CheckedListBox();
            this.grpBaseDirs.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBaseDirs
            // 
            this.grpBaseDirs.Controls.Add(this.btnRemoveFolders);
            this.grpBaseDirs.Controls.Add(this.btnAddFolder);
            this.grpBaseDirs.Controls.Add(this.btnSearchNewFolder);
            this.grpBaseDirs.Controls.Add(this.txtFolder);
            this.grpBaseDirs.Controls.Add(this.lstBaseDirs);
            this.grpBaseDirs.Location = new System.Drawing.Point(13, 13);
            this.grpBaseDirs.Name = "grpBaseDirs";
            this.grpBaseDirs.Size = new System.Drawing.Size(1093, 585);
            this.grpBaseDirs.TabIndex = 0;
            this.grpBaseDirs.TabStop = false;
            this.grpBaseDirs.Text = "Basis-Suchordner";
            this.grpBaseDirs.Enter += new System.EventHandler(this.grpBaseDirs_Enter);
            // 
            // btnRemoveFolders
            // 
            this.btnRemoveFolders.Location = new System.Drawing.Point(907, 74);
            this.btnRemoveFolders.Name = "btnRemoveFolders";
            this.btnRemoveFolders.Size = new System.Drawing.Size(180, 23);
            this.btnRemoveFolders.TabIndex = 4;
            this.btnRemoveFolders.Text = "-> Ausgewählte Ordner entfernen";
            this.btnRemoveFolders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveFolders.UseVisualStyleBackColor = true;
            this.btnRemoveFolders.Click += new System.EventHandler(this.btnRemoveFolders_Click);
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.Location = new System.Drawing.Point(906, 45);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Size = new System.Drawing.Size(181, 23);
            this.btnAddFolder.TabIndex = 3;
            this.btnAddFolder.Text = "-> als Suchordner hinzufügen.";
            this.btnAddFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddFolder.UseVisualStyleBackColor = true;
            this.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);
            // 
            // btnSearchNewFolder
            // 
            this.btnSearchNewFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchNewFolder.Location = new System.Drawing.Point(7, 19);
            this.btnSearchNewFolder.Name = "btnSearchNewFolder";
            this.btnSearchNewFolder.Size = new System.Drawing.Size(231, 23);
            this.btnSearchNewFolder.TabIndex = 2;
            this.btnSearchNewFolder.Text = "Suchordner auswählen...";
            this.btnSearchNewFolder.UseVisualStyleBackColor = true;
            this.btnSearchNewFolder.Click += new System.EventHandler(this.btnSearchNewFolder_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(8, 48);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(892, 20);
            this.txtFolder.TabIndex = 1;
            // 
            // lstBaseDirs
            // 
            this.lstBaseDirs.CheckOnClick = true;
            this.lstBaseDirs.FormattingEnabled = true;
            this.lstBaseDirs.Location = new System.Drawing.Point(8, 74);
            this.lstBaseDirs.Name = "lstBaseDirs";
            this.lstBaseDirs.Size = new System.Drawing.Size(892, 484);
            this.lstBaseDirs.TabIndex = 0;
            this.lstBaseDirs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstBaseDirs_ItemCheck);
            this.lstBaseDirs.SelectedIndexChanged += new System.EventHandler(this.lstBaseDirs_SelectedIndexChanged);
            this.lstBaseDirs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstBaseDirs_MouseDown);
            // 
            // F_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 610);
            this.Controls.Add(this.grpBaseDirs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "F_Settings";
            this.Text = "CSearch - Einstellungen: Suchordner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F_Settings_FormClosing);
            this.grpBaseDirs.ResumeLayout(false);
            this.grpBaseDirs.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBaseDirs;
        private System.Windows.Forms.Button btnRemoveFolders;
        private System.Windows.Forms.Button btnAddFolder;
        private System.Windows.Forms.Button btnSearchNewFolder;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.CheckedListBox lstBaseDirs;
    }
}