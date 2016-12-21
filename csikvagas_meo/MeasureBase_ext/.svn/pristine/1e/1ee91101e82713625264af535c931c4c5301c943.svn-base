using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace e77.MeasureBase.Helpers
{
    public static class MeasureHelper
    {
        static string _directoryOfTheExecutable;
        
        public static string DirectoryOfTheExecutable
        {
            get
            {
                if (_directoryOfTheExecutable == null)
                {
                    string str = Process.GetCurrentProcess().MainModule.FileName;

                    if (str.Contains('\\'))
                        _directoryOfTheExecutable = str.Substring(0, str.LastIndexOf('\\'));
                    else
                        throw new MeasureBaseInternalException(
                            string.Format("MeasureHelper::DirectoryOfTheExecutable() cannot found path from Name= '{0}'",
                            str));
                }

                return _directoryOfTheExecutable;
            }
        }

        
        public static bool IsValidSnFormat(bool product_notDevice_in, string sn_in, out string problemDescription_out)
        {
            Trace.TraceInformation("IsSnFormat('{0}')", sn_in);

            problemDescription_out = string.Empty;
            int requiredLenght = product_notDevice_in ?
                11 : 15;

            if (sn_in.Length != requiredLenght)
            {
                problemDescription_out = string.Format("A sorozatszám hossza {0} karakter, nem {1}",
                    sn_in.Length, requiredLenght);

                Trace.TraceInformation(problemDescription_out);
                return false;
            }

            char invalidCh = ' ';
            bool invalidChar = false;
            foreach(char ch in sn_in)
                if(!char.IsDigit(ch) 
                    && !(ch >= 'A' && ch <= 'Z'))
                {
                    invalidChar = false;
                    invalidCh = ch;
                }
            if(invalidChar)
            {
                problemDescription_out = string.Format("Érvénytelen karakter: '{0}'", invalidCh);
                Trace.TraceInformation(problemDescription_out);
                return false;
            }

            Trace.TraceInformation("IsSnFormat('{0}') retruns true.", sn_in);
            return true;
        }             
    }
}
