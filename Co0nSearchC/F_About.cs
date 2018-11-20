using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Co0nSearchC
{
    public partial class F_About : Form
    {
        public F_About()
        {
            InitializeComponent();
        }

        private String abouttext = Program.APPNAME + " Version: " + Program.VERSION.ToString() + " ("+Program.VERSIONDATE+")" + "\r\n\r\nDieses Programm dient der rekursiven Suche nach Dateien und Ordnern in verschiedenen Basisordnern.\r\nDas Ziel ist es, eine schnelle Suche in verschiedenenn Ordnerstrukturen gleichzeitig zu ermöglichen\r\nund dabei ein besseres Handling als die integrierte Windowssuche zu bieten.\r\n\r\nDies ist freie Software, welche unter der GPLv3 lizenziert ist.\r\n\r\nQuellcode unter:\r\nhttps://github.com/derco0n/CSearch";

        private void F_About_Load(object sender, EventArgs e)
        {
            this.lblAbout.Text = this.abouttext;
        }
    }
}
