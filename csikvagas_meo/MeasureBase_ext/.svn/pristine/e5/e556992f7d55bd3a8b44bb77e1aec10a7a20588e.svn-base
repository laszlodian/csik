using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace e77.MeasureBase.InputWrapper
{
    public class InputWrapperFile : InputWrapperBase
    {
        StreamReader _File;

        public void InitFile(string fileName_in, Encoding encoding_in)
        {
            Trace.TraceInformation("Input Wrapper: InitFile {0}", fileName_in);
            _File = new StreamReader(fileName_in, encoding_in);
            fileName = fileName_in;
        }

        string fileName;
        public override string Name
        {
            get { return fileName; }
        }

        override public void Close()
        {
            if (_File != null)
            {
                _File.Close();
                _File.Dispose();
                _File = null;
            }
        }

        public override void Reset()
        {
            _File.BaseStream.Seek(0, SeekOrigin.Begin);
        }

        protected override string ReadLineInternal()
        {
            return _File.ReadLine();
        }

        protected override void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    _File.Close();
                    _File.Dispose();
                }

                // If there are unmanaged resources to release,
                // they need to be released here.
            }
            _Disposed = true;

            // If it is available, make the call to the
            // base class's Dispose(Boolean) method
            base.Dispose(disposing);
        }



    }
}
