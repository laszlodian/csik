using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using e77.MeasureBase.Helpers;

namespace e77.MeasureBase.Configuration
{
    /// <summary>
    /// Only Low-Level functions. HiLevel is attached into MeasureConfig
    /// </summary>
    public class IniFile
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName_in">pure filename (e.g.: ua3.ini) beside exe or full file name</param>
        public IniFile(string fileName_in)
        {
            if (!fileName_in.Contains(Path.PathSeparator))
                FullFileName = string.Format("{0}\\{1}", 
                    MeasureHelper.DirectoryOfTheExecutable, fileName_in);
            else
                FullFileName = fileName_in;

            if (!System.IO.File.Exists(FullFileName))
                throw new ArgumentException(string.Format("File not found: '{0}'", FullFileName));
           
            Trace.TraceInformation("IniFile({0})", FullFileName);
        } 

        public string FullFileName { get; protected set; }

        [DllImport("KERNEL32.DLL",   EntryPoint = "GetPrivateProfileStringW",
            SetLastError=true,  CharSet=CharSet.Unicode, ExactSpelling=true,  CallingConvention=CallingConvention.StdCall)]
        private static extern int GetPrivateProfileString( string lpAppName,
            string lpKeyName,string lpDefault, string lpReturnString,  int nSize, string lpFilename);

        [DllImport("KERNEL32.DLL", EntryPoint = "WritePrivateProfileStringW",
            SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int WritePrivateProfileString( string lpAppName,
          string lpKeyName, string lpString, string lpFilename);


        /// <summary>
        /// Returns with a List 
        /// </summary>
        /// <returns></returns>
        public List<string> GetCategories()
        {
            string returnString = new string(' ', 65536);
            GetPrivateProfileString(null, null, null, returnString, 65536, FullFileName);
            List<string> result = new List<string>(returnString.Split('\0'));
            result.RemoveRange(result.Count - 2, 2);
            return result;
        }

        public List<string> GetKeys(string category)
        {
            string returnString = new string(' ', 32768);
            GetPrivateProfileString(category, null, null, returnString, 32768, FullFileName);
            List<string> result = new List<string>(returnString.Split('\0'));
            result.RemoveRange(result.Count-2,2);
            return result;
        }

        public static string GetIniFileString(string path_in, string category, string key, string defaultValue)
        {
            string returnString = new string(' ', 1024);
            GetPrivateProfileString(category, key, defaultValue, returnString, 1024, path_in);
            return returnString.Split('\0')[0];
        }

        public string GetIniFileString(string category, string key, string defaultValue)
        { 
            string returnString = new string(' ', 1024);
            GetPrivateProfileString(category, key, defaultValue, returnString, 1024, FullFileName);
            return returnString.Split('\0')[0];
        }

        public int GetIniFileInt(string category, string key, string defaultValue)
        {
            return int.Parse(GetIniFileString(category, key, defaultValue));
        }

        public float GetIniFileFloat(string category, string key, string defaultValue)
        {
            return float.Parse(GetIniFileString(category, key, defaultValue));
        }

        public double GetIniFileDouble(string category, string key, string defaultValue)
        {
            return double.Parse(GetIniFileString(category, key, defaultValue));
        }
        
        public bool GetIniFileBool(string category, string key, string defaultValue)
        {
            return bool.Parse(GetIniFileString(category, key, defaultValue));
        }

    }
}
