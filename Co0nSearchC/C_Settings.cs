using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Co0nUtilZ;
using Microsoft.Win32;

namespace Co0nSearchC
{
    /// <summary>
    /// Defines possible settings
    /// </summary>
    public class C_Settings
    {

        #region variables
        private C_RegistryHelper _regHelper;
        private List<String> _baseDirs = new List<string>();
        private String _BaseDirPrefix="SearchBaseDir";
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
        public List<String> BaseDirs
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
                if (res.StartsWith(this._BaseDirPrefix)) // Wenn der gefundene Schlüssel mit dem gesuchten Präfix übereinstimmt.
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
        public bool removeBaseDir(String Value) // Entfernt einen Basisordner ... muss anschließend noch mit "putAllBaseDirs()" geschrieben werden.
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
        /// <returns></returns>
        public bool AddBaseDir(String Value) // Fügt einen Basisordner hinzu ... muss anschließend noch mit "putAllBaseDirs()" geschrieben werden.
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
                if (res.StartsWith(this._BaseDirPrefix)) // Wenn der gefundene Schlüssel mit dem gesuchten Präfix übereinstimmt.
                {
                    String value = this._regHelper.ReadSettingFromRegistry(this._Instancename, res);
                    this._baseDirs.Add(value);
                }
            }

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
                int counter=0;
                foreach (String Value in this._baseDirs)
                //foreach (String Value in currentbasedirs)
                {
                    this._regHelper.WriteSettingToRegistry(this._Instancename, this._BaseDirPrefix+counter.ToString(), Value);
                    counter++;
                }
            }

        }
        #endregion


    }
}
