using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public class ProtocolArbitraryDataLength : HeaderedDataProtocol
    {
        #region Properties

        protected enum EProtocolElements
        {
            SyncHeader,
            CommandHeader,
            Data,
            Checksum
        }

        protected IProtocolElement this[EProtocolElements index]
        {
            get { return this[index.ToString()]; }
        }

        #endregion Properties

        // Todo_HL: MeasureBase.Communication.ProtocolArbitraryDataLength add constructors, add command-header

        #region Constructors

        //public ProtocolArbitraryDataLength(byte[] syncHeader_in, int commandHeaderLength_in, ICheckSum checksumAlg_in)
        //    : base()
        //{
        //    InitProtocol(portName_in,syncHeader_in, commandHeaderLength_in, checksumAlg_in);
        //}

        public ProtocolArbitraryDataLength(IPortProxy port_in, byte[] syncHeader_in, int commandHeaderLength_in, ICheckSum checksumAlg_in)
            : base(port_in)
        {
            InitProtocol(port_in, syncHeader_in, commandHeaderLength_in, checksumAlg_in);
        }

        private void InitProtocol(IPortProxy port_in, byte[] syncHeader_in, int commandHeaderLength_in, ICheckSum checksumAlg_in)
        {
            Add(new SyncHeader(port_in, syncHeader_in));
            Add(new DataProtocol(port_in, commandHeaderLength_in), EProtocolElements.CommandHeader.ToString());
            Add(new CustomLengthDataProtocol(port_in), EProtocolElements.Data.ToString());
            Add(new Checksum(port_in, checksumAlg_in));
            ReceiveBeginTimeout = DefaultReadTimeout;
            ReceiveTimeout = DefaultReadTimeout;
        }

        #endregion Constructors

        public void SendReceiveCustomLengthProtocolMessage(byte[] sendData, out byte[] receiveData)
        {
            _header.SendData = sendData.ToList().GetRange(0, _header.DataLength).ToArray();
            _dataProtocolElement.SendData = sendData.ToList().GetRange(_header.DataLength, sendData.Length - _header.DataLength).ToArray();
            Send();
            Receive();
            //HACK: to have the same output as it was before the refactoring
            // receiveData = Data.ReceivedData;
            List<byte> ret = new List<byte>();
            ret.AddRange(_header.ReceivedData);
            ret.AddRange(_dataProtocolElement.ReceivedData);
            receiveData = ret.ToArray();
        }

        public void SendReceiveProtocolMessage(byte[] sendData, out byte[] receiveData, int requiredDataLenghtPointer, int receiveBeginTimeout, int receivingTimeout)
        {
            ReceiveBeginTimeout = receiveBeginTimeout;
            ReceiveTimeout = receivingTimeout;
            _header.SendData = sendData.ToList().GetRange(0, _header.DataLength).ToArray();
            _dataProtocolElement.SendData = sendData.ToList().GetRange(_header.DataLength, sendData.Length - _header.DataLength).ToArray();
            Send();
            Receive();
            //HACK: to have the same output as it was before the refactoring
            // receiveData = Data.ReceivedData;
            List<byte> ret = new List<byte>();
            ret.AddRange(_header.ReceivedData);
            ret.AddRange(_dataProtocolElement.ReceivedData);
            receiveData = ret.ToArray();
        }

        //public void SendReceiveCustomLengthProtocolMessage(byte[] sendData, out byte[] receiveData)
        //{
        //    SendReceiveCustomLengthProtocolMessage(sendData, out receiveData, DefaultReceiveBeginTimeout, DefaultReceiveTimeout);
        //}

        //public void SendReceiveCustomLengthProtocolMessage(byte[] sendData, out byte[] receiveData, int receiveBeginTimeout, int receivingTimeout)
        //{
        //    SendReceiveCustomLengthProtocolMessage(sendData, out receiveData, RequiredDataLengthPointer, receiveBeginTimeout, receivingTimeout);
        //}

        //public void SendReceiveCustomLengthProtocolMessage(byte[] sendData, out byte[] receiveData, int requiredDataLenghtPointer, int receiveBeginTimeout, int receivingTimeout)
        //{
        //    //PortCollection.TryLockPort(_port, this, PortAllocationTimeout); //TODO_HL: 3 bool visszatérési érték nincs lekezelve, tovább megy akkor is ha nem sikerült az erőforrás lock-olás
        //    //try
        //    //{
        //    //todo_fgy: full receive timeout needed?
        //    //todo_fgy: protection for Rx buff (infinite receiving)

        //    //collects send data
        //    SendMessage(sendData);

        //    //wait for beginning of receive
        //    WaitForReceiveBegin(receiveBeginTimeout);

        //    List<byte> receiveDataList = new List<byte>();
        //    //skip header
        //    ReceiveHeader(receiveDataList);

        //    // collect receive data until data length is reached or timeout is ellapsed
        //    ReceiveRawData(requiredDataLenghtPointer + 1 + _syncHeader.Length, receivingTimeout, receiveDataList);

        //    // collect remain data until timeout is ellapsed
        //    ReceiveData(receiveDataList[requiredDataLenghtPointer + _syncHeader.Length] + HeaderLength, receivingTimeout, receiveDataList);

        //    //Process received data. Checksum
        //    receiveData = ProcessReceivedMessage(receiveDataList);
        //    //}
        //    //finally
        //    //{
        //    //    PortCollection.ReleasePort(_port, this);
        //    //}
        //}

        protected override DataProtocol _header
        {
            get
            {
                return (DataProtocol)this[EProtocolElements.CommandHeader];
            }
        }

        protected override IDataProtocol _dataProtocolElement
        {
            get
            {
                return (IDataProtocol)this[EProtocolElements.Data];
            }
        }
    }
}