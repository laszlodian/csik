using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace e77.MeasureBase.Communication
{
    public class LegacyProtocol : DataProtocolComposite
    {
        #region Fields and Properties

        protected const String cMainDataName = "MainData";

        //protected const int PortAllocationTimeout = 10000; // TODO_HL: 3 lehet hogy ki kellene tenni globális változónak

        //public bool TraceEnable { get; set; }

        /// <summary>
        /// Size of Header and checksum
        /// </summary>
        //protected int PackageFrameSize
        //{
        //    get { return _syncHeader.Length + _checksumAlg.Length; }
        //}

        //protected readonly byte[] _syncHeader;

        //readonly protected ICheckSum _checksumAlg;

        #endregion Fields and Properties

        #region Constructors

        //private Protocol()
        //{
        //    IsInBufferDiscarded = true;
        //}

        //public Protocol(byte[] syncHeader_in, ICheckSum checksumAlg_in)
        //    : base()
        //{
        //    InitProtocolElements(syncHeader_in, checksumAlg_in);
        //}

        public LegacyProtocol(IPortProxy port_in, byte[] syncHeader_in, ICheckSum checksumAlg_in)
            : base(port_in)
        {
            InitProtocolElements(port_in, syncHeader_in, checksumAlg_in);
        }

        private void InitProtocolElements(IPortProxy port_in, byte[] syncHeader_in, ICheckSum checksumAlg_in)
        {
            Add(new SyncHeader(port_in, syncHeader_in));
            Add(new CustomLengthDataProtocol(port_in), cMainDataName);
            Add(new Checksum(port_in, checksumAlg_in));
        }

        #endregion Constructors

        #region Methods

        #endregion Methods

        #region Send - Receive Functions

        //public void SendProtocolMessage(byte[] sendData)
        //{
        //    //PortCollection.TryLockPort(_port, this, PortAllocationTimeout); //TODO_HL: 3 bool visszatérési érték nincs lekezelve, tovább megy akkor is ha nem sikerült az erőforrás lock-olás
        //    //try
        //    //{
        //    SendMessage(sendData);
        //    //}
        //    //finally
        //    //{
        //    //    PortCollection.ReleasePort(_port, this);
        //    //}
        //}

        //public virtual void Send(byte[] sendData_in)
        //{
        //    ((CustomLengthDataProtocol)this["CustomLengthDataProtocol"]).SendData = sendData_in;
        //    Send();
        //}

        /// <summary>
        /// Receives data, with preset checksum and header
        /// </summary>
        /// <param name="receive_data">pure receive data, without checksum and header</param>
        /// <param name="requiredDataLenght">Only the pure data, without checksum and header. Use -1 for unknown size (receiving finishes at timeout) </param>
        /// <param name="receiveBeginTimeout">time after send finished and firsy receive byte[ms]</param>
        /// <param name="receivingTimeout">between receiving chunks [ms]</param>
        /// <seealso cref="DeviceProtocol"/>
        //public virtual void ReceiveProtocolMessage(out byte[] receiveData, int requiredDataLenght, int receiveBeginTimeout, int receivingTimeout)
        //{
        //    //PortCollection.TryLockPort(_port, this, PortAllocationTimeout); //TODO_HL: 3 bool visszatérési érték nincs lekezelve, tovább megy akkor is ha nem sikerült az erőforrás lock-olás
        //    //try
        //    //{
        //    //wait for beginning of receive
        //    WaitForReceiveBegin(receiveBeginTimeout);

        //    List<byte> receiveDataList = new List<byte>();
        //    //skip header
        //    ReceiveHeader(receiveDataList);

        //    //collect receive data until timeout ellaped
        //    ReceiveData(requiredDataLenght, receivingTimeout, receiveDataList);

        //    //Process received data. Checksum
        //    receiveData = ProcessReceivedMessage(receiveDataList);
        //    //}
        //    //finally
        //    //{
        //    //    PortCollection.ReleasePort(_port, this);
        //    //}
        //}

        public virtual void Receive(out byte[] receiveData_out, int requiredDataLenght)
        {
            _dataProtocolElement.DataLength = requiredDataLenght;
            Receive();
            receiveData_out = _dataProtocolElement.ReceivedData;
        }

        /// <summary>
        /// Sends and receives data, with preset checksum and header
        /// </summary>
        /// <param name="send_data">pure send data, without checksum and header</param>
        /// <param name="receive_data">pure receive data, without checksum and header</param>
        /// <param name="requiredDataLenght">Only the pure data, without checksum and header. Use -1 for unknown size (receiving finishes at timeout) </param>
        /// <param name="receiveBeginTimeout">time after send finished and firsy receive byte[ms]</param>
        /// <param name="receivingTimeout">between receiving chunks [ms]</param>
        /// <seealso cref="DeviceProtocol"/>
        public virtual void SendReceiveProtocolMessage(byte[] sendData, out byte[] receiveData, int requiredDataLenght, int receiveBeginTimeout, int receivingTimeout)
        {
            #region Former implementation
            //PortCollection.TryLockPort(_port, this, PortAllocationTimeout); //TODO_HL: 3 bool visszatérési érték nincs lekezelve, tovább megy akkor is ha nem sikerült az erőforrás lock-olás
            //try
            //{
            //todo_fgy: full receive timeout needed?
            //todo_fgy: protection for Rx buff (infinite receiving)
            #endregion Former implementation

            ReceiveBeginTimeout = receiveBeginTimeout;
            ReceiveTimeout = receivingTimeout;
            SendReceive(sendData, out receiveData, requiredDataLenght);

            #region Former implementation
            //collects send data
            //SendMessage(sendData);

            ////wait for beginning of receive
            //WaitForReceiveBegin(receiveBeginTimeout);

            //List<byte> receiveDataList = new List<byte>();
            ////skip header
            //ReceiveHeader(receiveDataList);

            ////collect receive data until timeout ellaped
            //ReceiveData(requiredDataLenght, receivingTimeout, receiveDataList);

            ////Process received data. Checksum
            //receiveData = ProcessReceivedMessage(receiveDataList);
            //}
            //finally
            //{
            //    PortCollection.ReleasePort(_port, this);
            //}
            #endregion Former implementation
        }

        public virtual void SendReceive(byte[] sendData, out byte[] receiveData, int requiredDataLenght)
        {
            Send(sendData);
            Receive(out receiveData, requiredDataLenght);
        }

        //protected byte[] ProcessReceivedMessage(List<byte> receiveDataList)
        //{
        //    byte[] receiveData;
        //    if (_checksumAlg.Check(receiveDataList))
        //    {
        //        //remove checksum and header
        //        receiveData = receiveDataList.Where((byteValue, index) =>
        //                    (index >= _syncHeader.Length && index < receiveDataList.Count - _checksumAlg.Length))
        //                                     .ToArray();
        //    }
        //    else
        //    {
        //        receiveData = null;
        //        throw new CheckSumException(string.Format("Checksum error. Received data '{0}'", receiveDataList));
        //    }
        //    return receiveData;
        //}

        //protected void ReceiveRawData(int requiredDataLenght, int receivingTimeout, List<byte> receiveDataList)
        //{
        //    int ReceivingTimeout = receivingTimeout;
        //    byte[] receiveBuffTmp = new byte[RECEIVE_BUFF_SIZE];
        //    while (ReceivingTimeout >= 0)
        //    {
        //        if (_port.BytesToRead != 0)
        //        {
        //            ReceivingTimeout = receivingTimeout;

        //            //count current package size
        //            int packageLenght = Math.Min(RECEIVE_BUFF_SIZE, _port.BytesToRead);
        //            if (requiredDataLenght != -1)
        //            {
        //                packageLenght = Math.Min(packageLenght,
        //                    (requiredDataLenght) - receiveDataList.Count);
        //            }

        //            //read current package
        //            int readed = _port.Read(receiveBuffTmp, 0,
        //                packageLenght);

        //            //do not add invalid data at the end of the buffer
        //            IEnumerable<byte> validBytes =
        //                receiveBuffTmp.Where((byteValue, index) => index < readed);

        //            receiveDataList.AddRange(validBytes);   //adds only the valid bytes

        //            if (requiredDataLenght != -1 &&
        //                receiveDataList.Count == requiredDataLenght)
        //                break; //all bytes has been arrived
        //        }
        //        else
        //        {
        //            ReceivingTimeout -= 5;
        //            System.Threading.Thread.Sleep(5);
        //        }
        //    }

        //    if (ReceivingTimeout < 0)
        //        throw new TimeoutException(string.Format("Receiving timeout has been elapsed. Full (with header) received data lenght: {0}, content: {1}",
        //            receiveDataList.Count, receiveDataList.ItemsToString()));
        //}

        //protected void ReceiveData(int requiredDataLenght, int receivingTimeout, List<byte> receiveDataList)
        //{
        //    if (requiredDataLenght == -1)
        //        ReceiveRawData(requiredDataLenght, receivingTimeout, receiveDataList);
        //    else
        //        ReceiveRawData(requiredDataLenght + PackageFrameSize, receivingTimeout, receiveDataList);
        //}

        //protected void ReceiveHeader(List<byte> receiveDataList)
        //{
        //    int error = 0;
        //    int i = 0;
        //    while (i < _syncHeader.Length)
        //    {
        //        byte c = (byte)_port.ReadByte();
        //        if (c == _syncHeader[i])
        //            i++;
        //        else
        //        {
        //            i = 0;
        //            if (error++ == 12)
        //                throw new HeaderException(string.Format("Cannot found header: {0}.", _syncHeader.ItemsToString())); //todo_fgy : 12 is AssertHeaderStart (-1 = deny assert check)
        //        }
        //    } //todo_fgy do timeout check

        //    //add header in order to checksum
        //    receiveDataList.AddRange(_syncHeader);
        //}

        //protected void WaitForReceiveBegin(int receiveBeginTimeout)
        //{
        //    int BeginReadTimeout = receiveBeginTimeout;
        //    while (_port.BytesToRead == 0 && BeginReadTimeout >= 0)
        //    {
        //        System.Threading.Thread.Sleep(5);
        //        BeginReadTimeout -= 5;
        //    }

        //    if (_port.BytesToRead == 0)
        //    {
        //        throw new TimeoutException("There is no any received data before timeout has been ellapsed.");
        //    }
        //}

        //protected void SendMessage(byte[] sendData)
        //{
        //    List<byte> sendDataList = new List<byte>(sendData.Length + PackageFrameSize);
        //    sendDataList.AddRange(_syncHeader);
        //    sendDataList.AddRange(sendData);
        //    _checksumAlg.AddChecksum(sendDataList);

        //    //clear read buffer
        //    if (_port.BytesToRead != 0 && IsInBufferDiscarded)
        //    {
        //        Trace.TraceWarning("Receive buffer is not empty. Dropped data: '{0}'", _port.ReadExisting());
        //        _port.DiscardInBuffer();
        //    }

        //    _port.Write(sendDataList.ToArray(), 0, sendDataList.Count);

        //    //wait for send finished
        //    int WriteTimeout = _port.WriteTimeout;
        //    while (_port.BytesToWrite != 0 && WriteTimeout >= 0)
        //    {
        //        System.Threading.Thread.Sleep(5);
        //        WriteTimeout -= 5;
        //    }
        //}

        #endregion Send - Receive Functions

        public override string ToString()
        {
            return string.Format("Protocol on Port {0}. (Header: {1})", _defaultPort.PortName, ((SyncHeader)this["SyncHeader"]).Header.ItemsToString());
        }

        protected override IDataProtocol _dataProtocolElement
        {
            get
            {
                return (IDataProtocol)this[cMainDataName];
            }
        }
    }

    //public abstract class ProtocolLayerBase : IDataProtocol
    //{
    //    protected abstract IDataProtocol _dataProtocol { get; }

    //    #region IDataProtocol Members

    //    public virtual int DataLength
    //    {
    //        get
    //        {
    //            return _dataProtocol.DataLength;
    //        }
    //        set
    //        {
    //            _dataProtocol.DataLength = value;
    //        }
    //    }

    //    public virtual byte[] SendData
    //    {
    //        get
    //        {
    //            return _dataProtocol.SendData;
    //        }
    //        set
    //        {
    //            _dataProtocol.SendData = value;
    //        }
    //    }

    //    public virtual byte[] ReceivedData
    //    {
    //        get { return _dataProtocol.ReceivedData; }
    //    }

    //    public virtual void Send(byte[] sendData_in)
    //    {
    //        SendData = sendData_in;
    //        _dataProtocol.Send();
    //    }

    //    public virtual void Receive(out byte[] receiveData_out)
    //    {
    //        _dataProtocol.Receive();
    //        receiveData_out = ReceivedData;
    //    }

    //    #endregion IDataProtocol Members
    //}

    //public abstract class ProtocolLayerComposite : ProtocolLayerBase
    //{
    //    DataProtocolComposite

    //}
}