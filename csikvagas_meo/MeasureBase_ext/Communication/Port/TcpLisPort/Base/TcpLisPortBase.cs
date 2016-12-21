#define LISTENER

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using GalaSoft.MvvmLight;

namespace e77.MeasureBase.Communication
{
    public abstract class TcpLisPortBase : ObservableObject, ILisPort
    {
        Timer _timer;

        /// <summary>
        /// The time between two connection checks (and possible reconnection tries) in milliseconds
        /// </summary>
        protected const int CheckConnectionInterval = 10000;

        public TcpLisPortBase()
        {
            NetworkInterface localInterface =
                NetworkInterface.GetAllNetworkInterfaces()
                                .FirstOrDefault((ni) =>
                                     ni.Name.ToLower().Contains("helyi")
                                    || ni.Name.ToLower().Contains("local"));
            if (localInterface != null)
            {
                IPInterfaceProperties iprop = localInterface.GetIPProperties();
                UnicastIPAddressInformation NetworkInterfaceAddress = iprop.UnicastAddresses.FirstOrDefault((uai) => uai.Address.AddressFamily == AddressFamily.InterNetwork);
                if (iprop.UnicastAddresses.Count != 0 && NetworkInterfaceAddress != null)
                    Address = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault((ip) => ip.ToString() == NetworkInterfaceAddress.Address.ToString());
            }
            
            //*/
            

            // TODO_HL: add these properties to measure base settings

            /*/
            if (Address == null)
                Address = IPAddress.Parse(Properties.Settings.Default.LocalAddress);

            Port = Properties.Settings.Default.PortNumber;
            //*/

            _timer = new Timer(CheckConnectionInterval);
            _timer.Elapsed += new ElapsedEventHandler((o, e) =>
            {
                ConnectionStatus = CheckConnectionStatus();
                if (ConnectionStatus == EConnectionStatus.Closed)
                    Open();
            });
            _timer.AutoReset = true;
            _timer.Start();
        }

        //protected void StartPortConnectionWatchdog()
        //{
        //    if (_timer != null && !_timer.Enabled)
        //        //_timer.Start()
        //        ;
        //}

        protected virtual EConnectionStatus CheckConnectionStatus()
        {
            return ConnectionStatus;
        }

        public enum EConnectionStatus
        {
            Closed,
            Listening,
            BeginAccepting,
            Synced,
        }

        #region ConnectionStatus
        /// <summary>
        /// The <see cref="ConnectionStatus" /> property's name.
        /// </summary>
        public const string ConnectionStatusPropertyName = "ConnectionStatus";

        private EConnectionStatus _conStat = EConnectionStatus.Closed;

        /// <summary>
        /// Sets and gets the ConnectionStatus property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public virtual EConnectionStatus ConnectionStatus
        {
            get
            {
                return _conStat;
            }

            set
            {
                if (_conStat == value)
                {
                    return;
                }

                //RaisePropertyChanging(ConnectionStatusPropertyName);
                _conStat = value;
                RaisePropertyChanged(ConnectionStatusPropertyName);
            }
        }

        #endregion ConnectionStatus

        #region Address
        /// <summary>
        /// The <see cref="Address" /> property's name.
        /// </summary>
        public const string AddressPropertyName = "Address";

        private IPAddress _ipAddr = null;

        /// <summary>
        /// Sets and gets the Address property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public virtual IPAddress Address
        {
            get
            {
                return _ipAddr;
            }

            set
            {
                if (_ipAddr == value)
                {
                    return;
                }

                //RaisePropertyChanging(AddressPropertyName);
                _ipAddr = value;
                RaisePropertyChanged(AddressPropertyName);
                // TODO_HL: add these properties to measure base settings
                //Properties.Settings.Default.LocalAddress = Address.ToString();
            }
        }

        #endregion Address

        #region Port
        /// <summary>
        /// The <see cref="Port" /> property's name.
        /// </summary>
        public const string PortPropertyName = "Port";

        private int _port = 3120;

        /// <summary>
        /// Port: Number of the IP port
        /// Sets and gets the Port property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public virtual int Port
        {
            get
            {
                return _port;
            }

            set
            {
                if (_port == value)
                {
                    return;
                }

                //RaisePropertyChanging(PortPropertyName);
                _port = value;
                RaisePropertyChanged(PortPropertyName);
                // TODO_HL: add these properties to measure base settings
                //Properties.Settings.Default.PortNumber = _port;
            }
        }

        #endregion Port

        #region ILisPort Members

        public string PortName
        {
            get
            {
                return String.Format("{0}:{1}", Address, Port);
            }

            set
            {
                String[] portnameString = value.Split(':');
                Address = IPAddress.Parse(portnameString[0]);
                Port = int.Parse(portnameString[1]);
            }
        }

        public void InitPort()
        {
        }

        public virtual void Open()
        {
            //StartPortConnectionWatchdog();
            IsOpen = true;
        }

        public virtual void Close()
        {
            IsOpen = false;
        }

        public byte ReadByte()
        {
            byte[] buffer = new byte[1];
            this.Read(buffer, 0, 1);
            return buffer[0];
        }

        public void WriteByte(byte data_in)
        {
            this.Write(new byte[] { data_in }, 0, 1);
        }

        #endregion ILisPort Members

        #region IPortProxy Members

        public void DiscardInBuffer()
        {
            if (IsBytesToReadImplemented)
            {
                byte[] buffer = new byte[this.BytesToRead];
                this.Read(buffer, 0, this.BytesToRead);
            }
            else
            {
                while (!this.IsReadBufferEmpty)
                    this.ReadByte();
            }
        }

        public virtual void Dispose()
        {
            Close();
        }

        #region IsOpen
        /// <summary>
        /// The <see cref="IsOpen" /> property's name.
        /// </summary>
        public const string IsOpenPropertyName = "IsOpen";

        private bool _isOpen = false;

        /// <summary>
        /// Sets and gets the IsOpen property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public virtual bool IsOpen
        {
            get
            {
                return _isOpen;
            }

            protected set
            {
                if (_isOpen == value)
                {
                    return;
                }

                //RaisePropertyChanging(IsOpenPropertyName);
                _isOpen = value;
                RaisePropertyChanged(IsOpenPropertyName);
            }
        }

        #endregion IsOpen

        public abstract void Write(byte[] buffer, int offset, int count);

        public abstract int Read(byte[] buffer, int offset, int count);

        public abstract int ReadTimeout { get; set; }

        public abstract int WriteTimeout { get; set; }

        public abstract bool IsBytesToReadImplemented { get; }

        public abstract int BytesToRead { get; }

        public virtual bool IsReadBufferEmpty
        {
            get
            {
                if (IsBytesToReadImplemented)
                    return BytesToRead == 0;
                else
                    throw new NotImplementedException();
            }
        }

        public virtual bool IsWriteBufferEmpty
        {
            get
            {
                return true;
            }
        }

        #endregion IPortProxy Members

        #region ILisPort Members

        public abstract IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback);

        #endregion ILisPort Members
    }
}