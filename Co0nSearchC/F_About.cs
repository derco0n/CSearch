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

        private String abouttext = Program.APPNAME + " Version: " + Program.VERSION.ToString() + " ("+Program.VERSIONDATE+")" + "\r\n\r\nCSearch: dient der rekursiven multithreaded Suche nach Dateien und Ordnern in verschiedenen Basisordnern.\r\nDas Ziel ist es, eine schnelle Suche in verschiedenenn Ordnerstrukturen gleichzeitig zu ermöglichen\r\nund dabei ein besseres Handling als die integrierte Windowssuche zu bieten.\r\n\r\nCopyright (C) <2018>  <Dennis Marx>\r\n\r\n    This program is free software: you can redistribute it and/or modify\r\n    it under the terms of the GNU General Public License as published by\r\n    the Free Software Foundation, either version 3 of the License, or\r\n    (at your option) any later version.\r\n\r\n    This program is distributed in the hope that it will be useful,\r\n    but WITHOUT ANY WARRANTY; without even the implied warranty of\r\n    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the\r\n    GNU General Public License for more details.\r\n\r\n    You should have received a copy of the GNU General Public License\r\n    along with this program.  If not, see <https://www.gnu.org/licenses/>.\r\n\r\nQuellcode unter:\r\nhttps://github.com/derco0n/CSearch";

        private void F_About_Load(object sender, EventArgs e)
        {
            this.lblAbout.Text = this.abouttext;
        }
    }
}
