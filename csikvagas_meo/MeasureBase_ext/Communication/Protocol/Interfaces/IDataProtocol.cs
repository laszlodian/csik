using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public interface IDataProtocol : IProtocolElement
    {
        int DataLength { get; set; }

        byte[] SendData { get; set; }

        byte[] ReceivedData { get; }

        void Send(byte[] sendData_in);

        void Receive(out byte[] receiveData_out);
    }
}