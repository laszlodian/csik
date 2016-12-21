using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public class SyncHeader : ProtocolElement
    {
        protected const int _defaultAssertHeaderStart = 12;

        /// <summary>
        /// It contains the maximum of bytes that can be asserted by the protocol before the Receive method raises a HeaderException.
        /// -1 means assert check is switched off
        /// </summary>
        public int AssertHeaderStart
        {
            get;
            set;
        }

        public byte[] Header
        {
            get
            {
                return _sendDataList.ToArray();
            }
            protected set
            {
                if (_sendDataList.Any())
                    _sendDataList.Clear();
                _sendDataList.AddRange(value);
            }
        }

        public SyncHeader(IPortProxy port_in, byte[] syncHeader_in)
            : base(port_in)
        {
            Init(syncHeader_in);
        }

        public SyncHeader(byte[] syncHeader_in)
            : base()
        {
            Init(syncHeader_in);
        }

        private void Init(byte[] syncHeader_in)
        {
            Header = syncHeader_in;
            AssertHeaderStart = _defaultAssertHeaderStart;
        }

        public override void ProcessReceivedData(List<byte> receivedDataList_in)
        {
            for (int i = 0; i < Header.Length; i++)
                if (receivedDataList_in[i] != Header[i])
                    throw new HeaderException("Header was not found during the message processing");
        }

        public override void Receive(List<byte> receivedDataList_in)
        {
            Receive(receivedDataList_in, _defaultPort);
        }

        public virtual void Receive(List<byte> receivedDataList_in, IPortProxy port_in)
        {
            base.Receive(receivedDataList_in);
            receivedDataList_in.Clear();
            int error = 0;
            int i = 0;
            while (i < Header.Length)
            {
                byte[] buffer = new byte[2];
                port_in.Read(buffer, 0, 1);
                byte c = buffer[0];
                if (c == Header[i])
                    i++;
                else
                {
                    i = 0;
                    if (AssertHeaderStart != -1 && error++ == AssertHeaderStart)
                        throw new HeaderException(string.Format("Cannot found header: {0}.", Header.ItemsToString()));
                }
            } //todo do timeout check

            //add header in order to checksum
            receivedDataList_in.AddRange(Header);
        }
    }
}