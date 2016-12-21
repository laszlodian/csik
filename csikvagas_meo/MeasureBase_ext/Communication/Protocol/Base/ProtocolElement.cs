using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    /// <summary>
    /// Describes the base of a standard protocol element that contains sending and receiving methods
    /// </summary>
    public abstract class ProtocolElement : IProtocolElement, IDisposable
    {
        #region Properties and Fields
        protected const int DefaultReadTimeout = 200; // TODO_HL: MeasureBase.Communication.ProtocolElement we need a constant defaults?
        protected const int DefaultWriteTimeout = 500;
        protected const int RECEIVE_BUFF_SIZE = 100;

        /// <summary>
        /// internal data list intended use for agregating sending data
        /// </summary>
        protected List<byte> _sendDataList;

        /// <summary>
        /// internal data list intended use for agregating receiving data
        /// </summary>
        protected List<byte> _receivedDataList;

        protected IPortProxy _defaultPort;

        public virtual IPortProxy DefaultPort
        {
            get
            {
                return _defaultPort;
            }
            set
            {
                _defaultPort = value;
            }
        }

        public virtual bool PortExists
        {
            get
            {
                return this.DefaultPort != null;
            }
        }

        #endregion Properties and Fields

        #region Constructors and Initiators

        public ProtocolElement()
        {
            _sendDataList = new List<byte>();
            _receivedDataList = new List<byte>();
            IsInBufferDiscarded = true;
            Name = this.GetType().Name;
            // TODO_HL: 1 kisimítani!!
            //HACK
            ReceiveBeginTimeout = DefaultReadTimeout;
            ReceiveTimeout = DefaultReadTimeout;
            SendTimeout = DefaultWriteTimeout;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="portName_in">Name of the port (ex. COM1)</param>
        public ProtocolElement(IPortProxy defaultPort_in)
            : this()
        {
            _defaultPort = defaultPort_in;
        }

        #endregion Constructors and Initiators

        #region Methods

        /// <summary>
        /// Waits until the first byte arrives
        /// </summary>
        protected void WaitForReceiveBegin()
        {
            WaitForReceiveBegin(ReceiveBeginTimeout);
        }

        /// <summary>
        /// Waits until the first byte arrives
        /// </summary>
        /// <param name="receiveBeginTimeout">timeout value in milliseconds</param>
        protected void WaitForReceiveBegin(int receiveBeginTimeout, IPortProxy port_in)
        {
            int BeginReadTimeout = receiveBeginTimeout;
            while (port_in.IsReadBufferEmpty && BeginReadTimeout >= 0)
            {
                System.Threading.Thread.Sleep(5);
                BeginReadTimeout -= 5;
            }

            if (port_in.IsReadBufferEmpty)
            {
                throw new TimeoutException("There is no any received data before timeout has been ellapsed.");
            }
        }

        /// <summary>
        /// Waits until the first byte arrives
        /// </summary>
        /// <param name="receiveBeginTimeout">timeout value in milliseconds</param>
        protected void WaitForReceiveBegin(int receiveBeginTimeout)
        {
            this.WaitForReceiveBegin(receiveBeginTimeout, _defaultPort);
        }

        #endregion Methods

        #region IProtocolElement Members

        /// <summary>
        /// Name of the protocol element
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// if it is true the receive buffer is discarded before the sending begins
        /// </summary>
        public virtual bool IsInBufferDiscarded { get; set; }

        /// <summary>
        /// Timeout for sending
        /// </summary>
        public virtual int SendTimeout { get; set; }

        /// <summary>
        /// Timeout before the first data arrives
        /// </summary>
        public virtual int ReceiveBeginTimeout { get; set; }

        /// <summary>
        /// Timeout between data segments
        /// </summary>
        public virtual int ReceiveTimeout { get; set; }

        /// <summary>
        /// Sends arbitrary byte list to the port
        /// </summary>
        /// <param name="sendDataList_in"></param>
        public virtual void Send(List<byte> sendDataList_in, IPortProxy port_in)
        {
            //clear read buffer
            if (IsInBufferDiscarded)
                port_in.DiscardInBuffer();

            port_in.Write(sendDataList_in.ToArray(), 0, sendDataList_in.Count);

            //wait for send finished
            int WriteTimeout = SendTimeout;
            while (!port_in.IsWriteBufferEmpty && WriteTimeout >= 0)
            {
                System.Threading.Thread.Sleep(5);
                WriteTimeout -= 5;
            }
        }

        /// <summary>
        /// Sends arbitrary byte list to the port
        /// </summary>
        /// <param name="sendDataList_in"></param>
        public virtual void Send(List<byte> sendDataList_in)
        {
            this.Send(sendDataList_in, _defaultPort);
        }

        /// <summary>
        /// Agregates sending data to sendDataList_in
        /// </summary>
        /// <param name="sendDataList_in">Context variable (usually contains the whole sending context)</param>
        public virtual void PrepareSendData(List<byte> sendDataList_in)
        {
            if (sendDataList_in != _sendDataList)
                sendDataList_in.AddRange(_sendDataList);
        }

        /// <summary>
        /// Receives data from port and adds it to <paramref name="receivedDataList_in"/>
        /// Overriden in a subclass: the base function of the base class has to be called prior to any port operations
        /// </summary>
        /// <param name="receivedDataList_in">data context for received data</param>
        public virtual void Receive(List<byte> receivedDataList_in)
        {
            _receivedDataList.Clear();
            WaitForReceiveBegin();
        }

        /// <summary>
        /// Processes receive data
        /// </summary>
        /// <param name="receivedDataList_in">Received data</param>
        public abstract void ProcessReceivedData(List<byte> receivedDataList_in);

        public void InitProtocol()
        {
            DefaultPort.InitPort();
        }

        #endregion IProtocolElement Members

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
                    if (_defaultPort.IsOpen)
                    {
                        //Monitor.TryEnter(_port, PortAllocationTimeout); //TODO_HL: 3 bool visszatérési érték nincs lekezelve, tovább megy akkor is ha nem sikerült az erőforrás lock-olás
                        _defaultPort.Close();
                        //Monitor.Exit(_port); //TODO_HL: 4 ellenőrizni: a Dispose után nem biztos hogy le tud futni az Exit
                    }
                    _defaultPort.Dispose();
                }

                // Call the appropriate methods to clean up unmanaged resources here.
                // If disposing is false, only the following code is executed.

                // Note disposing has been done.
                disposed = true;
            }
        }

        #endregion IDisposable design pattern for base class imlementing IDisposable
    }
}