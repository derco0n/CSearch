using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Co0nUtilZ;
using Microsoft.Win32;

namespace CSearch
{
    /// <summary>
    /// Defines possible settings
    /// </summary>
    public class C_Settings
    {

        #region variables
        private C_RegistryHelper _regHelper;
        private List<C_BaseDir> _baseDirs = new List<C_BaseDir>();
        private String _EnabledBaseDirPrefix=C_BaseDir.ENABLEDPREFIX; //Prefix for Enabled folders
        private String _DisabledBaseDirPrefix = C_BaseDir.DISABLEDPREFIX; //Prefix for Disabled folders
        private String _Instancename = "Main";
        #endregion

        #region constructor
        public C_Settings()
        {
            this._regHelper = new C_RegistryHelper(Registry.CurrentUser, @"SOFTWARE\DMARX-IT\Co0nSearch");
        }
        #endregion

        #region properties
        //Returns a String-List of all BaseDirectories saved in RAM
        public List<C_BaseDir> BaseDirs
        {// Property BaseDirs
            get
            {
                return this._baseDirs;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Removes all Basedirectories saved in Registry
        /// </summary>
        private void dropAllBaseDirs()
        {
            List<String> result = this._regHelper.ListValues(this._Instancename);            

            foreach (String res in result)
            {
                if (res.StartsWith(this._EnabledBaseDirPrefix) || res.StartsWith(this._DisabledBaseDirPrefix)) // Wenn der gefundene Schlüssel mit dem gesuchten Präfix übereinstimmt.
                {
                    this._regHelper.dropValue(this._Instancename, res);
                }
            }
            
        }

        /// <summary>
        /// Removes a Base-Directory from the Basedirectories saved in RAM
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool removeBaseDir(C_BaseDir Value) // Entfernt einen Basisordner ... muss anschließend noch mit "putAllBaseDirs()" geschrieben werden.
        {
            try
            {
                if (this._baseDirs != null)
                {
                    if (this._baseDirs.Contains(Value))
                    {
                        this._baseDirs.Remove(Value);
                        return true;
                    }
                   
                }
            }
            catch
            {
                
            }
            return false;
        }

        /// <summary>
        /// Adds a new Path to the Basedirectories saved in RAM
        /// </summary>
        /// <param name="Value">Basedir-Path</param>
        /// <returns>True on success</returns>
        public bool AddBaseDir(C_BaseDir Value) // Fügt einen Basisordner hinzu ... muss anschließend noch mit "putAllBaseDirs()" geschrieben werden.
        {
            try
            {
                if (this._baseDirs != null)
                {
                    if (!this._baseDirs.Contains(Value)) //Pfad hinzufügen wenn er nicht bereits existiert.
                    {
                        this._baseDirs.Add(Value);
                        return true;
                    }
                    
                }
            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// Read alls BaseDirectories from Registry
        /// </summary>
        public void getAllBaseDirs()
        {
            List<String> result = this._regHelper.ListValues(this._Instancename);

            this._baseDirs.Clear();

            foreach (String res in result)
            {
                if (res.StartsWith(this._EnabledBaseDirPrefix)) // Wenn der gefundene Schlüssel mit dem gesuchten Präfix für aktivierte übereinstimmt.
                {//Enabled Folders
                    String value = this._regHelper.ReadSettingFromRegistry(this._Instancename, res);
                    this._baseDirs.Add(new C_BaseDir(value, true));
                }
                else if (res.StartsWith(this._DisabledBaseDirPrefix)) // Wenn der gefundene Schlüssel mit dem gesuchten Präfix für deaktivierte Ordner übereinstimmt.
                {//Disabled Folders
                    String value = this._regHelper.ReadSettingFromRegistry(this._Instancename, res);
                    this._baseDirs.Add(new C_BaseDir(value, false));
                }
            }

            this._baseDirs.Sort(); //Sort results by their name

        }

        /// <summary>
        /// Writes all defined Basedirectories to Registry
        /// </summary>
        public void putAllBaseDirs()
        {
            
            if (this._baseDirs.Count > 0)
            {
                this._baseDirs.Sort();
                this.dropAllBaseDirs(); //... und dort löschen.

                //Anschließend die aktuelle definierten, zwischengepseicherten in die Registry schreiben.
                int counterenabled=0;
                int counterdisabled = 0;
                foreach (C_BaseDir Value in this._baseDirs)
                //foreach (String Value in currentbasedirs)
                {
                    if (Value.IsEnabled)
                    {//Enabled folders
                        this._regHelper.WriteSettingToRegistry(this._Instancename, this._EnabledBaseDirPrefix + counterenabled.ToString(), Value.Path);
                        counterenabled++;
                    }
                    else if (!Value.IsEnabled)
                    {//Disabled folders
                        this._regHelper.WriteSettingToRegistry(this._Instancename, this._DisabledBaseDirPrefix + counterdisabled.ToString(), Value.Path);
                        counterdisabled++;
                    }
                }
            }

        }

        /// <summary>
        /// Sets the Enabled/Disabled state of a specific item in RAM
        /// </summary>
        /// <param name="value"></param>
        /// <param name="Enable"></param>
        /// <returns></returns>
        public bool setState(C_BaseDir value, Boolean Enable)
        {
            foreach (C_BaseDir bdir in this._baseDirs)
            //foreach (String Value in currentbasedirs)
            {
                if (bdir.Equals(value))
                {
                    if (Enable)
                    {
                        return bdir.Enable();
                    }
                    else
                    {
                        return bdir.Disable();
                    }
                }

            }


            return false; //Item not found
        }
        #endregion


    }
}
