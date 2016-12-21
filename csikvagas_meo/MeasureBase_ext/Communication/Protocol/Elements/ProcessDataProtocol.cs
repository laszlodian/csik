using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public class ProcessDataProtocol : DataProtocol
    {
        ProcessProtocol _process;

        protected bool ShouldContextProcessed { get; set; }

        public ProcessDataProtocol(Func<List<byte>, bool> processFunction_in, int dataLength_in, bool shouldContextProcessed_in)
            : base(dataLength_in)
        {
            _process = new ProcessProtocol(processFunction_in);
            ShouldContextProcessed = shouldContextProcessed_in;
        }

        public ProcessDataProtocol(IPortProxy port_in, Func<List<byte>, bool> processFunction_in, int dataLength_in)
            : base(port_in, dataLength_in)
        {
            _process = new ProcessProtocol(port_in, processFunction_in);
        }

        public override void ProcessReceivedData(List<byte> receivedDataList_in)
        {
            _process.ProcessReceivedData(ShouldContextProcessed ? receivedDataList_in : _receivedDataList);
        }
    }
}