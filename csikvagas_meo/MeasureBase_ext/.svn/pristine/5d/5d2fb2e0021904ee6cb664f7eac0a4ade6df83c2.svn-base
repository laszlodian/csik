using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace e77.MeasureBase.Communication
{
    internal class TcpIpEndPointLisPort : TcpSocketLisPort
    {
        public override void Open()
        {
            Socket ret = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEnd = new IPEndPoint(Address, Port);
#if SERVER
            ret.Bind(ipEnd);
            ret.Listen(4);
            ret = ret.Accept(); // Nem biztos hogy jó így
#else
            ret.Connect(ipEnd);
#endif
            socket = ret;
            base.Open();
        }
    }
}