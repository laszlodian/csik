using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication.Protocol
{
    public class DifferentSendReceiveProtocolComposite : ProtocolComposite, IProtocolElement, IProtocolComposite
    {
        protected ProtocolElement _sendProtocol;

        public ProtocolElement SendProtocol
        {
            get
            {
                return _sendProtocol;
            }
        }

        protected ProtocolElement _receiveProtocol;

        public ProtocolElement ReceiveProtocol
        {
            get
            {
                return _receiveProtocol;
            }
        }
        public DifferentSendReceiveProtocolComposite(IPortProxy port_in, ProtocolElement sendProtocol_in, ProtocolElement recProtocol_in)
            : base(port_in)
        {
            _sendProtocol = sendProtocol_in;
            _receiveProtocol = recProtocol_in;
            _sendProtocol.DefaultPort = port_in;
            _receiveProtocol.DefaultPort = port_in;
        }
        public DifferentSendReceiveProtocolComposite(ProtocolElement sendProtocol_in, ProtocolElement recProtocol_in)
            : base()
        {
            _sendProtocol = sendProtocol_in;
            _receiveProtocol = recProtocol_in;
        }

        #region IProtocolElement Members

        /// <summary>
        /// Timeout before the first data arrives
        /// </summary>
        public override int ReceiveBeginTimeout
        {
            get
            {
                return _receiveProtocol.ReceiveBeginTimeout;
            }
            set
            {
                _receiveProtocol.ReceiveBeginTimeout = value;
            }
        }

        /// <summary>
        /// Timeout between data segments
        /// </summary>
        public override int ReceiveTimeout
        {
            get
            {
                return _receiveProtocol.ReceiveTimeout;
            }
            set
            {
                _receiveProtocol.ReceiveTimeout = value;
            }
        }

        /// <summary>
        /// Timeout for sending
        /// </summary>
        public override int SendTimeout
        {
            get
            {
                return _sendProtocol.SendTimeout;
            }
            set
            {
                _sendProtocol.SendTimeout = value;
            }
        }

        public override void PrepareSendData(List<byte> sendDataList_in)
        {
            _sendProtocol.PrepareSendData(sendDataList_in);
        }

        public override void ProcessReceivedData(List<byte> receivedDataList_in)
        {
            _receiveProtocol.ProcessReceivedData(receivedDataList_in);
        }

        public override void ProcessReceivedContentData()
        {
            //foreach (IProtocolElement element in _receiveProtocol.ProtocolElements)
            //    element.ProcessReceivedData(_receivedDataList);
            throw new NotImplementedException();
        }

        public override void Send(List<byte> sendDataList_in)
        {
            _sendProtocol.Send(sendDataList_in);
        }

        public override void Receive(List<byte> receivedDataList_in)
        {
            _receiveProtocol.Receive(receivedDataList_in);
        }

        #endregion IProtocolElement Members

        ///// <summary>
        ///// Adds element to both send and receive protocol
        ///// </summary>
        ///// <param name="protocolElement_in"></param>
        //public override void Add(IProtocolElement protocolElement_in)
        //{
        //    _sendProtocol.Add(protocolElement_in);
        //    _receiveProtocol.Add(protocolElement_in);
        //}

        ///// <summary>
        ///// Adds element to both send and receive protocol
        ///// </summary>
        ///// <param name="protocolElement_in"></param>
        //public override void Add(IProtocolElement protocolElement_in, string protocolElementName_in)
        //{
        //    _sendProtocol.Add(protocolElement_in, protocolElementName_in);
        //    _receiveProtocol.Add(protocolElement_in, protocolElementName_in);
        //}
    }
}