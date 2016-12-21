using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public class CustomLengthDataProtocol : DataProtocol
    {
        #region Properties and Fields

        public int DefaultDataLength { get; set; }

        #endregion Properties and Fields

        #region Constructors

        public CustomLengthDataProtocol() : base(0) { ;}

        public CustomLengthDataProtocol(IPortProxy port_in) : base(port_in, 0) { ;}

        public CustomLengthDataProtocol(IPortProxy port_in, int defaultDataLength_in)
            : base(port_in, defaultDataLength_in)
        {
            DefaultDataLength = defaultDataLength_in;
        }

        #endregion Constructors

        #region Methods

        public virtual void Receive(out byte[] receiveData_out, int requiredDataLength)
        {
            DataLength = requiredDataLength;
            this.Receive(out receiveData_out);
        }

        #endregion Methods

        #region IDataProtocol Members

        public override byte[] SendData
        {
            get
            {
                return base.SendData;
            }
            set
            {
                DataLength = value.Length;
                base.SendData = value;
            }
        }

        public override void Send(byte[] sendData_in)
        {
            _dataLength = sendData_in.Length;
            base.Send(sendData_in);
        }

        public override void Receive(out byte[] receiveData_out)
        {
            DataLength = DefaultDataLength;
            base.Receive(out receiveData_out);
        }

        #endregion IDataProtocol Members
    }
}