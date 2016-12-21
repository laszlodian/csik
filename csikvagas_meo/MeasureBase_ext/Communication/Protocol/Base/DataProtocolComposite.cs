using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public abstract class DataProtocolComposite : ProtocolComposite, IDataProtocol
    {
        #region Fields

        protected abstract IDataProtocol _dataProtocolElement { get; }

        #endregion Fields

        #region Constructors

        public DataProtocolComposite()
            : base()
        {
            ;
        }

        public DataProtocolComposite(IPortProxy port_in)
            : base(port_in)
        { ;}

        #endregion Constructors

        #region IDataProtocol Members

        public int DataLength
        {
            get
            {
                return _dataProtocolElement.DataLength;
            }
            set
            {
                _dataProtocolElement.DataLength = value;
            }
        }

        public byte[] SendData
        {
            get
            {
                return _dataProtocolElement.SendData;
            }
            set
            {
                _dataProtocolElement.SendData = value;
            }
        }

        public byte[] ReceivedData
        {
            get
            {
                return _dataProtocolElement.ReceivedData;
            }
        }

        public virtual void Send(byte[] sendData_in)
        {
            SendData = sendData_in;
            Send();
        }

        public void Receive(out byte[] receiveData_out)
        {
            Receive(_receivedDataList);
            receiveData_out = _receivedDataList.ToArray();
        }

        #endregion IDataProtocol Members
    }
}