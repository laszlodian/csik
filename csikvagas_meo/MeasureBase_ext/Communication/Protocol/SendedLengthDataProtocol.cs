using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public class SendedLengthDataProtocol : HeaderedDataProtocol
    {
        public enum EProtocolElements
        {
            Length,
            Data
        }

        public SendedLengthDataProtocol(int lengthWidth_in)
            : base()
        {
            Add(new DataProtocol(lengthWidth_in), EProtocolElements.Length.ToString());
            Add(new CustomLengthDataProtocol(), EProtocolElements.Data.ToString());
            Init(lengthWidth_in);
        }

        public SendedLengthDataProtocol(IPortProxy port_in, int lengthWidth_in)
            : base(port_in)
        {
            Add(new DataProtocol(port_in, lengthWidth_in), EProtocolElements.Length.ToString());
            Add(new CustomLengthDataProtocol(port_in), EProtocolElements.Data.ToString());
            Init(lengthWidth_in);
        }

        private void Init(int lengthWidth_in)
        {
            DataLengthPointer = 0;
            DataLengthWidth = lengthWidth_in;
        }

        protected override DataProtocol _header
        {
            get { return (DataProtocol)this[EProtocolElements.Length.ToString()]; }
        }

        protected override IDataProtocol _dataProtocolElement
        {
            get { return (IDataProtocol)this[EProtocolElements.Data.ToString()]; }
        }
    }
}