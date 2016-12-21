using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public abstract class TcpSocketLisPort : TcpLisPortBase
    {
        public override IPAddress Address
        {
            get
            {
                return base.Address;
            }
            set
            {
                EConnectionStatus formerConnectionStatus = ConnectionStatus;
                //bool formerIsOpenStatus = IsOpen;
                if (formerConnectionStatus != EConnectionStatus.Closed)
                    Close();
                ChangeAddrOffline(value, Port);
                if (formerConnectionStatus != EConnectionStatus.Closed)
                    Open();
            }
        }

        protected override EConnectionStatus CheckConnectionStatus()
        {
            try
            {
                if (!socket.Connected)
                    Close();
            }
            catch (NullReferenceException ex)
            {
                Close();
            }
            //catch (SocketException ex)
            //{
            //    Close();
            //}
            return base.ConnectionStatus;
        }

        protected virtual void ChangeAddrOffline(IPAddress Address_in, int Port_in)
        {
            base.Address = Address_in;
            base.Port = Port_in;
        }

        public override int Port
        {
            get
            {
                return base.Port;
            }
            set
            {
                EConnectionStatus formerConnectionStatus = ConnectionStatus;
                //bool formerIsOpenStatus = IsOpen;
                // this solution (checking IsOpen value) left a leak in the code: if ConnectionStatus is BeginAccepting the isOpen field is false, but the connection should be closed
                if (formerConnectionStatus != EConnectionStatus.Closed)
                    Close();
                ChangeAddrOffline(Address, value);
                if (formerConnectionStatus != EConnectionStatus.Closed)
                    Open();
            }
        }

        public Socket socket;//host

        public override void Close()
        {
            ConnectionStatus = EConnectionStatus.Closed;
            base.Close();
            if (socket != null)
                socket.Close();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            byte[] sendbuffer = buffer.Where((b, i) => i >= offset).ToArray();
            socket.Send(sendbuffer, count, SocketFlags.None);// TODO_HL: HDA utánanézni
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback)
        {
            try
            {
                return socket.BeginReceive(buffer, offset, count, SocketFlags.None, callback, this);
            }
            catch (ObjectDisposedException ex)
            {
                Close();
                return null;
            }
            catch (SocketException ex)
            {
                Close();
                return null;
            }
        }

        public int EndReceive(IAsyncResult asyncResult)
        {
            return socket.EndReceive(asyncResult);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            byte[] receivebuffer = new byte[count];
            int readBytes = socket.Receive(receivebuffer, count, SocketFlags.None);
            for (int i = offset; i < offset + count; i++)
                buffer[i] = receivebuffer[i - offset];
            return readBytes;
        }

        public override void Dispose()
        {
            base.Dispose();
            socket.Disconnect(false);
        }

        //public override bool IsOpen
        //{
        //    get { return socket != null ? socket.Connected : false; }
        //}

        public override int ReadTimeout
        {
            get
            {
                return socket.ReceiveTimeout;
            }
            set
            {
                socket.ReceiveTimeout = value;
            }
        }

        public override int WriteTimeout
        {
            get
            {
                return socket.SendTimeout;
            }
            set
            {
                socket.SendTimeout = value;
            }
        }

        public override int BytesToRead
        {
            get
            {
                return socket.Available;
            }
        }

        public override bool IsBytesToReadImplemented
        {
            get { return true; }
        }
    }
}