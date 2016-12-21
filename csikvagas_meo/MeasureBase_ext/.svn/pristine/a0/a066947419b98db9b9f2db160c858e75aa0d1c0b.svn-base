using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public class EndSignDataProtocol : DataProtocol
    {
        public byte[] EndSign
        {
            get;
            protected set;
        }

        public EndSignDataProtocol(IPortProxy port_in, string endSign_in)
            : base(port_in, endSign_in.Length)
        {
            List<byte> endSign = new List<byte>();
            foreach (char c in endSign_in)
            {
                endSign.Add((byte)c);
            }
        }

        public EndSignDataProtocol(byte[] endSign_in)
            : base(endSign_in.Length)
        {
            EndSign = endSign_in;
        }

        public EndSignDataProtocol(IPortProxy port_in, byte[] endSign_in)
            : base(port_in, endSign_in.Length)
        {
            EndSign = endSign_in;
        }

        // TODO_HL: 2 Missing IDataProtocol and IProtocolElement overrides!!

        protected override void ReceiveRawData()
        {
            ReceiveRawData(ReceiveTimeout, _receivedDataList);
        }

        protected void ReceiveRawData(int receivingTimeout, List<byte> receiveDataList)
        {
            ReceiveRawData(receivingTimeout, receiveDataList, _defaultPort);
        }

        protected void ReceiveRawData(int receivingTimeout, List<byte> receiveDataList, IPortProxy port_in)
        {
            receiveDataList.Clear();
            int ReceivingTimeout = receivingTimeout;
            byte[] receiveBuffTmp = new byte[RECEIVE_BUFF_SIZE];
            while (ReceivingTimeout >= 0)
            {
                if (!port_in.IsReadBufferEmpty)
                {
                    ReceivingTimeout = receivingTimeout;

                    //count current package size
                    //int packageLenght = Math.Min(RECEIVE_BUFF_SIZE, _port.BytesToRead);
                    //if (requiredDataLenght != -1)
                    //{
                    //    packageLenght = Math.Min(packageLenght,
                    //        (requiredDataLenght) - receiveDataList.Count);
                    //}

                    //read current package
                    int readed = port_in.Read(receiveBuffTmp, 0,
                        1);

                    //do not add invalid data at the end of the buffer
                    //IEnumerable<byte> validBytes =
                    //    receiveBuffTmp.Where((byteValue, index) => index < readed);
                    if (readed == 1)
                        receiveDataList.Add(receiveBuffTmp[0]);   //adds only the valid bytes

                    if (receiveDataList.Where((b, i) => i >= receiveDataList.Count - EndSign.Count()).SequenceEqual(EndSign))
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
    }
}