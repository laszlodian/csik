using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Diagnostics;

namespace e77.MeasureBase.InputWrapper
{
    /// <summary>
    /// port or file
    /// </summary>
    public abstract class InputWrapperBase : IDisposable
    {

        bool _IgnoreEmptyLines;

        public bool IgnoreEmptyLines
        {
            get { return _IgnoreEmptyLines; }
            set { _IgnoreEmptyLines = value; }
        }

        string[] _IgnoreLinesStartWith;

        public string[] IgnoreLinesStartWith
        {
            get { return _IgnoreLinesStartWith; }
            set { _IgnoreLinesStartWith = value; }
        }

        public bool IsPort
        {
            get { return (this is InputWrapperPort); }
        }

        public bool IsMemory
        {
            get { return (this is InputWrapperMemory); }
        }

        public bool IsFile
        {
            get { return (this is InputWrapperFile); }
        }

        abstract public string Name
        {
            get;
        }

        public virtual void Close()
        { ;}

        /// <summary>
        /// Set initial state (e.g. start position for file, discard buffer for port)
        /// </summary>
        public virtual void Reset()
        { ;}

        /// <summary>
        /// Disabled by defaulut (null) create list object for the collecting
        /// </summary>
        public List<string> CollectedLines { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>null = End Of Input (== timeout in case of serial port)</returns>
        protected abstract string ReadLineInternal();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>null if EOF or timeout </returns>
        public string ReadLine()
        {
            string line = string.Empty;
            while( line == string.Empty )
            {
                line = ReadLineInternal();
                if( line == null )
                    return null; //End of input

                if (CollectedLines != null)
                    CollectedLines.Add(line);

                if (!IgnoreEmptyLines && line == string.Empty)
                    return string.Empty;//empty line

                if(IgnoreLinesStartWith != null)
                    foreach(string ignore in IgnoreLinesStartWith)
                        if(line.StartsWith(ignore))
                            line = string.Empty; //ignore = read again
            }//while (line != string.Empty)

            AddCalculatedChecksum(line);
            return line;
        }

        public void AddCalculatedChecksum(string line)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(line);

            /*checksum Trace:
            Trace.TraceInformation("Checksum Orig = {0}", Checksum); 
            Trace.TraceInformation("Encoded bytes = {0}", encodedBytes.ItemsToString()); */

            foreach (byte b in encodedBytes)
                Checksum += b;

            Checksum += (byte)0x0a;
            //checksum Trace: Trace.TraceInformation("Checksum = {0}, after line = '{1}'", Checksum, line); 
        }

        public byte Checksum { get; set; }
                
    #region IDisposable design pattern for base class imlementing IDisposable

        protected bool _Disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._Disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    //dispose should be done by descendant classes
                }
                // Call the appropriate methods to clean up unmanaged resources here.
                // If disposing is false, only the following code is executed.

                // Note disposing has been done.
                _Disposed = true;
            }
        }
#endregion IDisposable

        public override string ToString()
        {
            return string.Format("InputWrapper. IsFile: {0}", IsFile);
        }
    }
}
