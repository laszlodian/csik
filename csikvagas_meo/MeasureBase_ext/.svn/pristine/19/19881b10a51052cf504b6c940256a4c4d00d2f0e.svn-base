using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public class PatternMatchDataProtocol : DataProtocol
    {
        public List<byte> MatchPattern { get; set; }

        public PatternMatchDataProtocol(byte[] matchPattern_in)
            : base(matchPattern_in.Length)
        {
            Construcor(matchPattern_in);
        }

        public PatternMatchDataProtocol(IPortProxy port_in, byte[] matchPattern_in)
            : base(port_in, matchPattern_in.Length)
        {
            Construcor(matchPattern_in);
        }

        private void Construcor(byte[] matchPattern_in)
        {
            MatchPattern = new List<byte>();
            MatchPattern.AddRange(matchPattern_in);
        }

        public override void ProcessReceivedData(List<byte> receivedDataList_in)
        {
            for (int i = 0; i < DataLength; i++)
                if (_receivedDataList[i] != MatchPattern[i])
                    throw new CommunicationException("Received data does not match the required pattern");
        }
    }
}