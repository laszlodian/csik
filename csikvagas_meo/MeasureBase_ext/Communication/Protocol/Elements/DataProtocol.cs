using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public class DataProtocol : ProtocolElement, IProtocolElement, IDataProtocol
    {
        #region Fields
        protected int _dataLength;

        #endregion Fields

        #region Constructors and Initiators

        public DataProtocol(int dataLength_in)
            : base()
        {
            DataLength = dataLength_in;
        }

        public DataProtocol(IPortProxy port_in, int dataLength_in)
            : base(port_in)
        {
            DataLength = dataLength_in;
        }

        #endregion Constructors and Initiators

        #region Methods

        protected virtual void ReceiveRawData()
        {
            ReceiveRawData(_dataLength, ReceiveTimeout, _receivedDataList);
        }

        protected void ReceiveRawData(int requiredDataLenght, int receivingTimeout, List<byte> receiveDataList)
        {
            ReceiveRawData(requiredDataLenght, receivingTimeout, receiveDataList, _defaultPort);
        }

        protected void ReceiveRawData(int requiredDataLenght, int receivingTimeout, List<byte> receiveDataList, IPortProxy port_in)
        {
            if (requiredDataLenght < 0)
                throw new InvalidOperationException();
            receiveDataList.Clear();
            int ReceivingTimeout = receivingTimeout;
            byte[] receiveBuffTmp = new byte[RECEIVE_BUFF_SIZE];
            while (ReceivingTimeout >= 0)
            {
                if (!port_in.IsReadBufferEmpty)
                {
                    ReceivingTimeout = receivingTimeout;

                    //count current package size
                    int packageLenght = Math.Min(RECEIVE_BUFF_SIZE, port_in.BytesToRead);
                    if (requiredDataLenght != -1)
                    {
                        packageLenght = Math.Min(packageLenght,
                            (requiredDataLenght) - receiveDataList.Count);
                    }

                    //read current package
                    int readed = port_in.Read(receiveBuffTmp, 0,
                        packageLenght);

                    //do not add invalid data at the end of the buffer
                    IEnumerable<byte> validBytes =
                        receiveBuffTmp.Where((byteValue, index) => index < readed);

                    receiveDataList.AddRange(validBytes);   //adds only the valid bytes

                    if (requiredDataLenght != -1 &&
                        receiveDataList.Count == requiredDataLenght)
                        break; //all bytes has been arrived
                }
                else
                {
                    ReceivingTimeout -= 5;
                    System.Threading.Thread.Sleep(5);
                }
            }

            if (ReceivingTimeout < 0)
                throw new TimeoutException(string.Format("Receiving timeout has been elapsed. Full (with header) received data lenght: {0}, content: {1}",
                    receiveDataList.Count, receiveDataList.ItemsToString()));
        }

        #endregion Methods

        #region IDataProtocol Members

        public int DataLength
        {
            get
            {
                return _dataLength;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Length cannot be negative number");
                _dataLength = value;
            }
        }

        public virtual byte[] SendData
        {
            get
            {
                return _sendDataList.ToArray();
            }
            set
            {
                if (value.Length != DataLength)
                    throw new ArgumentException(String.Format("Actual data size({0}) is not equivalent with the required data size({1})", value.Length, _dataLength));
                _sendDataList.Clear();
                _sendDataList.AddRange(value);
            }
        }

        public virtual byte[] ReceivedData
        {
            get
            {
                return _receivedDataList.ToArray();
            }
        }

        public virtual void Send(byte[] sendData_in)
        {
            SendData = sendData_in;
            PrepareSendData(_sendDataList);
            base.Send(_sendDataList);
        }

        public virtual void Receive(out byte[] receiveData_out)
        {
            Receive(_receivedDataList);
            receiveData_out = ReceivedData;
        }

        #endregion IDataProtocol Members

        #region IProtocolElement Members

        public override void ProcessReceivedData(List<byte> receivedDataList_in)
        {
        }

        public override void Receive(List<byte> receivedDataList_in)
        {
            base.Receive(receivedDataList_in);
            ReceiveRawData();
            if (receivedDataList_in != _receivedDataList)
                receivedDataList_in.AddRange(_receivedDataList);
        }

        #endregion IProtocolElement Members
    }
}