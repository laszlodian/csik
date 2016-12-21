using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public class ProcessProtocol : ProtocolElement, IProtocolElement
    {
        private Func<List<byte>, bool> processFunction { get; set; }

        public ProcessProtocol(Func<List<byte>, bool> processFunction_in)
            : base()
        {
            processFunction = processFunction_in;
        }

        public ProcessProtocol(IPortProxy port_in, Func<List<byte>, bool> processFunction_in)
            : base(port_in)
        {
            processFunction = processFunction_in;
        }

        public override void ProcessReceivedData(List<byte> receivedDataList_in)
        {
            if (!processFunction(receivedDataList_in))
                throw new CommunicationException("Received data does not match the required pattern");
        }

        public override void Receive(List<byte> receivedDataList_in)
        {
        }

        public override void Send(List<byte> sendDataList_in)
        {
        }

        public override void PrepareSendData(List<byte> sendDataList_in)
        {
            // TODO_HL: MeasureBase.Communication.ProcessProtocol add to this class to accept 2 types of process functions
            // one is to prepare send
            // one is to process receive
        }
    }
}