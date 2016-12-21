using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public class Checksum : DataProtocol
    {
        readonly protected ICheckSum _checksumAlg;

        public Checksum(ICheckSum checkSumAlg_in)
            :base(checkSumAlg_in.Length)
        {
            _checksumAlg = checkSumAlg_in;
        }

        public Checksum(IPortProxy port_in, ICheckSum checkSumAlg_in)
            : base(port_in, checkSumAlg_in.Length)
        {
            _checksumAlg = checkSumAlg_in;
        }

        public override void PrepareSendData(List<byte> sendDataList_in)
        {
            _checksumAlg.AddChecksum(sendDataList_in);
        }

        public override void ProcessReceivedData(List<byte> receivedDataList_in)
        {
            if (!_checksumAlg.Check(receivedDataList_in))
                throw new CheckSumException();
        }
    }
}