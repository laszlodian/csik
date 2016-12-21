using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace e77.MeasureBase.Communication
{
    public class TcpListenerLisPort : TcpSocketLisPort
    {
        TcpListener _listener;

        public TcpListenerLisPort()
        {
            ConnectionStatus = EConnectionStatus.Closed;
            _listener = new TcpListener(Address, Port);
        }

        protected override void ChangeAddrOffline(IPAddress Address_in, int Port_in)
        {
            base.ChangeAddrOffline(Address_in, Port_in);
            if (Address_in != null)
                _listener = new TcpListener(Address_in, Port_in);
            else
                _listener = null;
        }

        IAsyncResult _asyncRes;

        public override void Open()
        {
            //StartPortConnectionWatchdog();
            if (ConnectionStatus == EConnectionStatus.Closed)
            {
                try
                {
                    _listener.Start();
                    ConnectionStatus = EConnectionStatus.BeginAccepting;
                    _asyncRes = _listener.BeginAcceptSocket(AcceptSocketCallback, this);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
                        throw; // IP cím változtatáskor dobja!!!
                    ConnectionStatus = EConnectionStatus.Closed;
                    _listener.Stop();
                }
            }
        }

        private void AcceptSocketCallback(IAsyncResult asyncRes_in)
        {
            if (ConnectionStatus == EConnectionStatus.BeginAccepting)
            {
                try
                {
                    socket = _listener.EndAcceptSocket(asyncRes_in);
                    ConnectionStatus = EConnectionStatus.Synced;
                    IsOpen = true;
                }
                catch (ObjectDisposedException ex) { }
            }
        }

        public override void Close()
        {
            base.Close();
            /*
            if (_asyncRes != null)
                myList.EndAcceptSocket(_asyncRes);
            //*/

            _listener.Stop();
        }
    }
}