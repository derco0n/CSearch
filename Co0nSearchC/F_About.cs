using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSearch
{
    public partial class F_About : Form
    {
        private String _text = "";
        private String _title = "";

        public F_About(String text, String title)
        {
            this._text = text;
            this._title = title;
            InitializeComponent();
        }

        


        private void F_About_Load(object sender, EventArgs e)
        {
            //this.lblAbout.Text = this._text;
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Cursor = Cursors.Arrow;
            this.txtInfo.GotFocus += txtInfo_GotFocus;
        
            this.txtInfo.Text = this._text;
            this.Text = this._title;
        }

        private void txtInfo_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtInfo_GotFocus(object sender, EventArgs e)
        {
            ((TextBox)sender).Parent.Focus();
        }

    }
}
