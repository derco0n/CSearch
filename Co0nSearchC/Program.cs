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
        public static float VERSION = 0.121f;
        public static String VERSIONDATE = "20181120";
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new F_Main());
        }
    }
}
