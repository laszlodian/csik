using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using e77.MeasureBase.Extensions.Hex;

namespace e77.MeasureBase.Communication
{
    public class SerialPortProxy : IPortProxy<SerialPort>, IDisposable
    {
        public enum EStandardBaudRates
        {
            _1200 = 1200,
            _2400 = 2400,
            _4800 = 4800,
            _9600 = 9600,
            _14400 = 14400,
            _19200 = 19200,
            _38400 = 38400,
            _57600 = 57600,
            _115200 = 115200
        };

        public enum EStandardDataBits
        {
            _7 = 7,
            _8 = 8,
        }

#if !DEBUG
        bool IsDebugOn = false;
#else

        //*/
        bool IsDebugOn = true;

        /*/
        bool IsDebugOn = false;
        //*/
#endif

        public SerialPortProxy()
        {
            _port = new SerialPort();
            InitPortAction = new Action<SerialPort>((_port_in) =>
                {
                    _port_in.BaudRate = (int)EStandardBaudRates._9600;
                    _port_in.DataBits = (int)EStandardDataBits._8;
                    _port_in.Parity = Parity.None;
                    _port_in.StopBits = StopBits.One;
                    _port_in.Handshake = Handshake.None;
                    _port_in.WriteTimeout = DefaultWriteTimeout;
                    _port_in.ReadTimeout = DefaultReadTimeout;
                });
        }

        public SerialPortProxy(String portName_in)
            : this()
        {
            PortName = portName_in;
        }

        protected SerialPort _port;
        private SerialPortConfiguration DefaultPortConfig = new SerialPortConfiguration();
        protected const int DefaultReadTimeout = 200; // TODO_HL: MeasureBase.Communication.PortProxy do we need a constant defaults?
        protected const int DefaultWriteTimeout = 500;

        public virtual void Close()
        {
            //PortCollection.TryLockPort(_port, this, PortAllocationTimeout); //TODO_HL: 3 bool visszatérési érték nincs lekezelve, tovább megy akkor is ha nem sikerült az erőforrás lock-olás
            _port.Close();
            //PortCollection.ReleasePort(_port, this);
        }

        /// <summary>
        /// Opens the port (or in a subclass it can include open rutine for the protocol as well)
        /// </summary>
        public virtual void Open()
        {
            //PortCollection.TryLockPort(_port, this, PortAllocationTimeout); //TODO_HL: 3 bool visszatérési érték nincs lekezelve, tovább megy akkor is ha nem sikerült az erőforrás lock-olás
            _port.Open();
            //PortCollection.ReleasePort(_port, this);
        }

        /// <summary>
        /// Initialize BaudRate, DataBits, Parity, StopBit(...) of the Serial port
        /// </summary>
        public Action<SerialPort> InitPortAction { get; set; }

        /// <summary>
        /// Write arbitrary string to the port
        /// </summary>
        /// <param name="s"></param>
        public void Write(string s)
        {
            if (IsDebugOn)
                Debug.WriteLine(String.Format("Write to port({1}):{0}", s, PortName));
            _port.Write(s);
        }

        public void Write(byte[] buffer)
        {
            if (IsDebugOn)
                Debug.WriteLine(String.Format("Write to port({1}):{0}", buffer.ToHexString(), PortName));
            _port.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Writes a specified number of bytes to the serial port using data from a buffer.
        /// </summary>
        /// <param name="buffer">The byte array that contains the data to write to the port.</param>
        /// <param name="offset">The zero-based byte offset in the buffer parameter at which to begin copying bytes to the port.</param>
        /// <param name="count">The number of bytes to write.</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            if (IsDebugOn)
                Debug.WriteLine(String.Format("Write to port({1}):{0}", buffer.ToHexString(), PortName));
            _port.Write(buffer, offset, count);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int ret = _port.Read(buffer, offset, count);
            if (IsDebugOn)
                Debug.WriteLine(String.Format("Read from port({1}):{0}", buffer.ToHexString(), PortName));
            return ret;
        }

        public int ReadTimeout
        {
            get { return _port.ReadTimeout; }
            set { _port.ReadTimeout = value; }
        }

        public int WriteTimeout
        {
            get { return _port.WriteTimeout; }
            set { _port.WriteTimeout = value; }
        }

        public virtual string PortName
        {
            get
            {
                return _port.PortName;
            }
            set
            {
                if (!PortCollection.Instance.ContainsSerialPortByName(value))
                    PortCollection.Instance.Add(new SerialPort(value));
                _port = PortCollection.Instance.GetPort<SerialPort>(value);
            }
        }

        public bool PortExists
        {
            get
            {
                return PortCollection.Instance.IsSerialPortAvailableByName(PortName);
            }
        }

        public void DiscardInBuffer()
        {
            if (_port.BytesToRead != 0)
            {
                Trace.TraceWarning("Receive buffer is not empty on port {1} (Protocol:{0}). Dropped data: '{2}'", this, PortName, _port.ReadExisting());
                _port.DiscardInBuffer();
            }
        }

        /// <summary>
        /// Gives back if the port is open
        /// </summary>
        public virtual bool IsOpen
        {
            get
            {
                return _port.IsOpen;
            }
        }

        public bool DtrEnable
        {
            get
            {
                return _port.DtrEnable;
            }
            set
            {
                _port.DtrEnable = value;
            }
        }

        public bool RtsEnable
        {
            get
            {
                return _port.RtsEnable;
            }
            set
            {
                _port.RtsEnable = value;
            }
        }

        public bool IsReadBufferEmpty
        {
            get
            {
                return _port.BytesToRead == 0;
            }
        }

        public bool IsWriteBufferEmpty
        {
            get
            {
                return _port.BytesToWrite == 0;
            }
        }

        public bool IsBytesToReadImplemented
        {
            get { return true; }
        }

        public int BytesToRead
        {
            get { return _port.BytesToRead; }
        }

        #region IDisposable design pattern for base class imlementing IDisposable

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    if (_port.IsOpen)
                    {
                        //Monitor.TryEnter(_port, PortAllocationTimeout); //TODO_HL: 3 bool visszatérési érték nincs lekezelve, tovább megy akkor is ha nem sikerült az erőforrás lock-olás
                        _port.Close();
                        //Monitor.Exit(_port); //TODO_HL: 4 ellenőrizni: a Dispose után nem biztos hogy le tud futni az Exit
                    }
                    _port.Dispose();
                }

                // Call the appropriate methods to clean up unmanaged resources here.
                // If disposing is false, only the following code is executed.

                // Note disposing has been done.
                disposed = true;
            }
        }

        #endregion IDisposable design pattern for base class imlementing IDisposable

        #region IPortProxy<SerialPort> Members

        public void InitPort()
        {
            this.InitPortAction(_port);
        }

        #endregion IPortProxy<SerialPort> Members

        public byte ReadByte()
        {
            return (byte)_port.ReadByte();
        }

        public void StorePortConfiguration()
        {
            DefaultPortConfig.StoreConfiguration(_port);
        }

        public void RestorePortConfiguration()
        {
            DefaultPortConfig.ConfigurePort(_port);
        }

        public int BaudRate
        {
            get { return _port.BaudRate; }
            set { _port.BaudRate = value; }
        }
    }
}