using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public interface IProtocolElement : IProtocol
    {
        String Name { get; set; }

        IPortProxy DefaultPort { get; set; }

        bool IsInBufferDiscarded { get; }

        int SendTimeout { get; set; }

        int ReceiveBeginTimeout { get; set; }

        int ReceiveTimeout { get; set; }

        void Send(List<byte> sendDataList_in);

        void PrepareSendData(List<byte> sendDataList_in);

        void Receive(List<byte> receivedDataList_in);

        void ProcessReceivedData(List<byte> receivedDataList_in);
    }
}