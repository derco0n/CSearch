using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Co0nSearchC
{
    public static class Program
    {
        
        public static String APPNAME = "CSearch";
        public static float VERSION = 0.144f;
        public static String VERSIONDATE = "20181123";
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool includehiddenfiles = false;

            if (args.Contains("--hidden"))
            {//this Parameter includes files and folders with "hidden-flag" in search result...
                includehiddenfiles = true;
            }           


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new F_Main(includehiddenfiles));
        }
    }
}
