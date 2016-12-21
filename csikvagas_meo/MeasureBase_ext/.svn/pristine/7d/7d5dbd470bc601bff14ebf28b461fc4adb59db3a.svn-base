using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication.Protocol.Elements
{
    [Obsolete("this class is only a stub please do not use it yet!!")]
    public class EchoProtocol : ProtocolComposite
    {
        public EchoProtocol(IPortProxy port_in)
            : base(port_in)
        {
        }

        public override void Send(List<byte> sendDataList_in)
        {
            base.Send(sendDataList_in);

            // receive data
            List<byte> receivedEchoData = new List<byte>();
            base.Receive(receivedEchoData);

            // process received data
            if (receivedEchoData.Count != sendDataList_in.Count)
                throw new EchoException("length mismatch");
            for (int i = 0; i < receivedEchoData.Count; i++)
                if (receivedEchoData[i] != sendDataList_in[i])
                    throw new EchoException("Received echo data does not match sended data");
        }

        public override void Receive(List<byte> receivedDataList_in)
        {
        }

        public override void ProcessReceivedData(List<byte> receivedDataList_in)
        {
        }
    }
}