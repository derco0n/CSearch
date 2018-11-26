using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSearch
{
    public class C_BaseDir:IComparable<C_BaseDir>
    {
        private String _Path;
        private Boolean _IsEnabled;
        public static String ENABLEDPREFIX = "SearchBaseDir";
        public static String DISABLEDPREFIX = "DisabledSearchBaseDir";

        public C_BaseDir(String Path, Boolean Enabled)
        {
            this._Path = Path;
            this._IsEnabled = Enabled;
            
        }

        public String Path
        {
            get
            {
                return this._Path;
            }
        }

        public Boolean IsEnabled
        {
            get
            {
                return this._IsEnabled;
            }
        }

        /// <summary>
        /// Enables a Basedir
        /// </summary>
        /// <param name="BaseDir"></param>
        /// <returns>True if success</returns>
        public bool Enable()
        {
            if (!this._IsEnabled)
            {
                this._IsEnabled = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Disables a Basedir
        /// </summary>
        /// <param name="BaseDir"></param>
        /// <returns>True if success</returns>
        public bool Disable()
        {
            if (this._IsEnabled)
            {
                this._IsEnabled = false;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return this._Path;
        }

        public int CompareTo(C_BaseDir other)
        {
            return this._Path.CompareTo(other._Path);
        }
    }
}
