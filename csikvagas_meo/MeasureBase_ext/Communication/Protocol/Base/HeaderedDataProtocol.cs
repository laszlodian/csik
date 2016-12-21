using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public abstract class HeaderedDataProtocol : DataProtocolComposite
    {
        protected abstract DataProtocol _header { get; }

        public int DataLengthPointer { get; set; }

        protected int _dataLengthWidth;

        public int DataLengthWidth
        {
            get
            {
                return _dataLengthWidth;
            }
            set
            {
                if (value > 4)
                    throw new ArgumentOutOfRangeException("DataLengthWidth value cannot be higher than 4");
                _dataLengthWidth = value;
            }
        }

        protected int _maxDataLength = 0;

        public int MaxDataLength
        {
            get
            {
                if (_maxDataLength == 0)
                {
                    if (DataLengthWidth > 3)
                        _maxDataLength = Int32.MaxValue;
                    else
                    {
                        int ret = 1;
                        for (int i = 0; i < DataLengthWidth; i++)
                            ret *= 256;
                        _maxDataLength = ret;
                    }
                }
                return _maxDataLength;
            }
        }

        protected int DataLengthInHeader
        {
            get
            {
                int ret = 0;
                for (int i = DataLengthPointer + DataLengthWidth - 1; i >= DataLengthPointer; i--) // Big endian
                {
                    ret <<= 8;
                    ret += _header.ReceivedData[i];
                }
                return ret;
            }
            set
            {
                if (value > MaxDataLength)
                    throw new ArgumentOutOfRangeException(String.Format("Value({0}) cannot be higher than maximum({1})!", value, MaxDataLength));
                //int tmp = value;
                byte[] header = HeaderLength < DataLengthPointer + DataLengthWidth ? new byte[DataLengthPointer + DataLengthWidth] : new byte[HeaderLength];
                for (int i = 0; i < _header.SendData.Length; i++)
                    header[i] = _header.SendData[i];

                for (int i = DataLengthPointer; i < DataLengthPointer + DataLengthWidth; i++) // Big endian
                {
                    header[i] = (byte)(value % 256);
                    value >>= 8;
                }
                _header.SendData = header;
            }
        }

        public virtual int HeaderLength
        {
            get
            {
                return _header.DataLength;
            }
            set
            {
                _header.DataLength = value;
            }
        }

        public HeaderedDataProtocol() : base() { ;}

        public HeaderedDataProtocol(IPortProxy port_in)
            : base(port_in)
        {
        }

        public override void PrepareSendData(List<byte> sendDataList_in)
        {
            if (_protocolElements.IndexOf(_header) > _protocolElements.IndexOf(_dataProtocolElement))
                throw new InvalidOperationException("Header element must precede Main Data element!");
            //if (_dataProtocolElement.DataLength != _header.SendData[DataLengthPointer])
            //    throw new ArgumentException("Data length contained by Header element does not match the length of the Main Data element");
            DataLength = _dataProtocolElement.SendData.Length;
            DataLengthInHeader = DataLength;
            base.PrepareSendData(sendDataList_in);
        }

        public override void Receive(List<byte> receivedDataList_in)
        {
            if (_protocolElements.IndexOf(_header) > _protocolElements.IndexOf(_dataProtocolElement))
                throw new InvalidOperationException("Header element must precede Main Data element!");
            foreach (IProtocolElement element in _protocolElements)
            {
                element.Receive(receivedDataList_in);
                if (element == _header)
                    _dataProtocolElement.DataLength = DataLengthInHeader;
            }
            if (receivedDataList_in != _receivedDataList) // TODO_HL: 10 could use a better object equality check
                receivedDataList_in.AddRange(_receivedDataList);
        }
    }
}