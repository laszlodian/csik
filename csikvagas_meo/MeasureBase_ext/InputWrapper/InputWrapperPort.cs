using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO.Ports;

using e77.MeasureBase.Communication;

namespace e77.MeasureBase.InputWrapper
{
    public class InputWrapperPort : InputWrapperBase
    {
        SerialPort _Port;
        string portName;

        public void InitComPort(string comPortName_in, int baudRate_in, int timeout_in, int readBufferSize_in, Encoding encoding_in)
        {
            InitComPort(comPortName_in, timeout_in, readBufferSize_in);
            _Port.BaudRate = baudRate_in;

            if(encoding_in != null)
                _Port.Encoding = encoding_in;

            _Port.DiscardInBuffer();
        }

        /// <summary>
        /// if the default NewLine (0x10, 0x13) is not acceptable. Else keep 0 value;
        /// </summary>
        public char CustomEndOfLineSign { get; set; }

        public override string Name
        {
            get { return portName; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comPortName_in"></param>
        /// <param name="timeout_in">[ms] or System.IO.Ports.SerialPort.InfiniteTimeout</param>
        /// <param name="readBufferSize_in">0= default</param>
        public void InitComPort(string comPortName_in, int timeout_in, int readBufferSize_in)
        {
            Trace.TraceInformation("Input Wrapper: InitComPort {0}", comPortName_in);

            portName = comPortName_in;
            _Port = new SerialPort(comPortName_in);
            _Port.ReadTimeout = timeout_in;

            if (readBufferSize_in != 0)
                _Port.ReadBufferSize = readBufferSize_in;
            _Port.Open();
        }


        public override void Reset()
        {
            DiscardReadBuffer();
        }

        public void DiscardReadBuffer()
        {
            _Port.DiscardInBuffer();
        }

        override public void Close()
        {
            if (_Port != null && _Port.IsOpen)
            {
                _Port.Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    if (_Port.IsOpen)
                        _Port.Close();
                    _Port.Dispose();
                }

                // If there are unmanaged resources to release,
                // they need to be released here.
            }
            _Disposed = true;

            // If it is available, make the call to the
            // base class's Dispose(Boolean) method
            base.Dispose(disposing);
        }

        protected override string ReadLineInternal()
        {
            if (CustomEndOfLineSign == 0)
                return _Port.ReadLine();
            else
            {
                StringBuilder buffer = new StringBuilder();

                while (true)
                {
                    try
                    {
                        int b = _Port.ReadByte();
                        if (b == -1  // end of the stream has been read
                            || CustomEndOfLineSign == b)
                            return buffer.ToString();
                        buffer.Append((char)b);
                    }
                    catch (TimeoutException)
                    {
                        return buffer.Length > 0 ? buffer.ToString() : null/*END OF input*/;
                    }                    
                }
            }
        }

        public int ReadTimeout
        {
            get { return _Port.ReadTimeout; }
            set { _Port.ReadTimeout = value; }
        }

        public void ClearPortBuffer()
        {
            if (_Port.BytesToRead != 0)
                _Port.DiscardInBuffer();
        }
    }
}
