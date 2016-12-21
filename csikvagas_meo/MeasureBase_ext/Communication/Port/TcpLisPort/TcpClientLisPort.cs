using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public class TcpClientLisPort : TcpLisPortBase
    {
        public TcpClient myClient = new TcpClient();
        public NetworkStream myStream;//client

        public override void Open()
        {
            base.Open();

            #region client
            myClient.Connect(Address, Port);
            myStream = myClient.GetStream();

            //byte[] bytesFromStream = asciEn.GetBytes(myStream.ToString());
            #endregion client
        }

        public override void Close()
        {
            base.Close();
            myStream.Close();
            myClient.Close();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            myStream.Write(buffer, offset, count);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return myStream.Read(buffer, offset, count);
        }

        public override void Dispose()
        {
            base.Dispose();
            myStream.Dispose();
        }

        //public override bool IsOpen
        //{
        //    get { return myClient.Connected; }
        //}

        public override int ReadTimeout
        {
            get
            {
                return myStream.ReadTimeout;
            }
            set
            {
                myStream.ReadTimeout = value;
            }
        }

        public override int WriteTimeout
        {
            get
            {
                return myStream.WriteTimeout;
            }
            set
            {
                myStream.WriteTimeout = value;
            }
        }

        public override bool IsBytesToReadImplemented
        {
            get { return true; }
        }

        public override int BytesToRead
        {
            get
            {
                return (int)(myStream.Length - myStream.Position);
            }
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback)
        {
            throw new NotImplementedException();
        }
    }
}