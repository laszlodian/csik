using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using e77.MeasureBase.Communication;
using e77.MeasureBase.Data;

namespace e77.MeasureBase.Electronics
{
    [global::System.Serializable]
    public class MSP430Exception : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MSP430Exception() { }

        public MSP430Exception(string message) : base(message) { }

        public MSP430Exception(string message, Exception inner) : base(message, inner) { }

        protected MSP430Exception(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [global::System.Serializable]
    public class SyncException : MSP430Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public SyncException() { }

        public SyncException(string message) : base(message) { }

        public SyncException(string message, Exception inner) : base(message, inner) { }

        protected SyncException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [global::System.Serializable]
    public class NoAcknowledgeException : MSP430Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NoAcknowledgeException() { }

        public NoAcknowledgeException(string message) : base(message) { }

        public NoAcknowledgeException(string message, Exception inner) : base(message, inner) { }

        protected NoAcknowledgeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    public class MSP430Protocol : SerialPortProxy, IProtocol
    {
        protected const int ACK_TIMEOUT_MULTIPLYER = 10;

        private byte[] Buffer = new byte[260];
        private bool tstInvert;
        private bool rstInvert;
        // TODO_HL: 10 ez akár lehetne publikus és akkor közvetlenül nem férnének a porthoz
        // egy másik megközelítés pedig hogy egy nyilvános "proxy"-t húzok a port köré és azt adom ki

        public int BSLFamily { get; protected set; }

        public int BSLVersion { get; protected set; }

        //public bool IsOpen { get { return _port.IsOpen; } }

        /// <summary>
        /// Close Port
        /// </summary>
        public virtual void Close()
        {
            if (_port != null && _port.IsOpen)
                _port.Close();
        }

        //public MSP430Protocol()
        //    : base(new byte[0], new NoCheckSum())
        //{
        //}
        public MSP430Protocol(String PortName)
            : base(PortName)
        //: base(new SerialPortProxy(PortName))
        {
            this.InitPortAction = (_port_in) =>
                {
                    _port_in.Parity = Parity.Even;
                    _port_in.BaudRate = 9600;
                    _port_in.StopBits = StopBits.One;
                    _port_in.DataBits = 8;
                    _port_in.Handshake = Handshake.None;

                    _port_in.WriteTimeout = DefaultWriteTimeout;
                    _port_in.ReadTimeout = DefaultReadTimeout;
                };
        }

        //public new SerialPortProxy _port
        //{
        //    get { return (SerialPortProxy)base._port; }
        //    protected set { base._port = value; }
        //}

        /// <summary>
        /// Open Port
        /// </summary>
        /// <param name="Port">Port Name</param>
        /// <param name="TSTInvert">Invert Programming pin control</param>
        /// <param name="RSTInvert">Invert Reset pin control</param>
        /// <returns>successfulness</returns>
        internal void Open(bool TSTInvert_in, bool RSTInvert_in)
        {
            InitProtocol();
            tstInvert = TSTInvert_in;
            rstInvert = RSTInvert_in;
            if (!_port.IsOpen)
                _port.Open();
        }

        /// <summary>
        /// Set prog pin to HI
        /// </summary>
        private void TSThi()
        {
            _port.RtsEnable = tstInvert;
        }

        /// <summary>
        /// Set prog pin to Lo
        /// </summary>
        private void TSTlo()
        {
            _port.RtsEnable = !tstInvert;
        }

        /// <summary>
        /// Set Reset pin to Hi
        /// </summary>
        private void RSThi()
        {
            _port.DtrEnable = rstInvert;
        }

        /// <summary>
        /// Set Reset pin to Lo
        /// </summary>
        private void RSTlo()
        {
            _port.DtrEnable = !rstInvert;
        }

        /// <summary>
        /// Receive 1 byte data from Serial port
        /// </summary>
        /// <param name="b">received byte</param>
        /// <returns>received byte</returns>
        private byte rxbyte()
        {
            return (byte)(_port.ReadByte());
        }

        /// <summary>
        /// Send 1 byte data from Serial port
        /// </summary>
        /// <param name="b">transmittable byte</param>
        private void txbyte(byte data)
        {
            byte[] b = new byte[1];
            b[0] = data;
            _port.Write(b, 0, 1);
        }

        /// <summary>
        ///  Reset Processor (after it the proc starts to work normally; thus it quits the programming mode)
        /// </summary>
        public void Reset()
        {
            RSThi();
            TSThi();
            Thread.Sleep(250);
            RSTlo();
            Thread.Sleep(20);
            RSThi();
            Thread.Sleep(250);
        }

        /// <summary>
        /// Receive Packet
        /// </summary>
        /// <param name="nn">number of received bytes (netto or brutto)</param>
        /// <returns>Comm succesfulness</returns>
        private void ReceivePacket(int nn)
        {
            byte chkeven = 0xff;
            byte chkodd = 0xff;
            int n;
            for (n = 0; n < nn + 2; n++)
            {
                Buffer[n] = rxbyte();

                if ((n & 1) == 0)
                {
                    chkeven ^= Buffer[n];
                }
                else
                {
                    chkodd ^= Buffer[n];
                }
            }

            if ((chkeven != 0) || (chkodd != 0))
                throw new Exception(string.Format("Checksum error. chkeven= {0}, chkodd = {1}, Received data '{2}'",
                    chkeven, chkodd, Buffer.ItemsToString()));
        }

        /// <summary>
        /// Send Packet
        /// </summary>
        /// <param name="nn">number of sended bytes (netto or brutto)</param>
        private void SendPacket(int nn)
        {
            // CheckSum számítás
            byte chkeven = 0xff;
            byte chkodd = 0xff;
            int n;
            for (n = 0; n < nn; n++)
            {
                if ((n & 1) == 0)
                {
                    chkeven ^= Buffer[n];
                }
                else
                {
                    chkodd ^= Buffer[n];
                }
            }
            Buffer[nn] = chkeven;
            Buffer[nn + 1] = chkodd;

            // Adat kiírása
            _port.Write(Buffer, 0, nn + 2);
        }

        /// <summary>
        /// Receive Acknowledge
        /// </summary>
        /// <returns></returns>
        private void RecAck()
        {
            if (!TryRecAck())
                throw new NoAcknowledgeException();
        }

        private bool TryRecAck()
        {
            return (rxbyte() == 0x90); // 0xA0 azt jelenti hogy "valami hülyeséget" küldtünk a procnak
        }

        /// <summary>
        /// Syncronise with the processor
        /// </summary>
        /// <remarks>It should be sent before every packet</remarks>
        /// <returns></returns>
        private void Sync()
        {
            txbyte(0x80);
            //RecAck();
            if (!TryRecAck()) throw new SyncException();
        }

        public bool TestConnection()
        {
            InitBST();
            txbyte(0x80);
            return TryRecAck();
        }

        /// <summary>
        /// BootStrap init - enter to programming mode
        /// </summary>
        public void InitBST()
        {
            _port.BaudRate = 9600;
            RSThi();
            TSThi();
            Thread.Sleep(250);
            RSTlo(); Thread.Sleep(20);
            TSTlo(); Thread.Sleep(20);
            TSThi(); Thread.Sleep(20);
            TSTlo(); Thread.Sleep(20);
            RSThi(); Thread.Sleep(20);
            TSThi(); Thread.Sleep(20);
            Thread.Sleep(250);

            _port.DiscardInBuffer();
        }

        /// <summary>
        /// Bootstrap Loader information
        /// </summary>
        /// <remarks>It writes the global BSLFamily, BSLVersion variables</remarks>
        /// <returns>Successfulnes</returns>
        public void ReadBSL()
        {
            Buffer[0x00] = 0x80;
            Buffer[0x01] = 0x1e;
            Buffer[0x02] = 0x04;
            Buffer[0x03] = 0x04;
            Buffer[0x04] = 0x00;
            Buffer[0x05] = 0x00;
            Buffer[0x06] = 0x00;
            Buffer[0x07] = 0x00;

            Sync();
            SendPacket(8);
            ReceivePacket(20);

            BSLFamily = (Buffer[4] << 8) + Buffer[5];
            BSLVersion = (Buffer[14] << 8) + Buffer[15];
        }

        /// <summary>
        /// Mass Erease
        /// </summary>
        /// <returns>Successfulnes</returns>
        public void MassErase()
        {
            Buffer[0x00] = 0x80;
            Buffer[0x01] = 0x18;
            Buffer[0x02] = 0x04;
            Buffer[0x03] = 0x04;
            Buffer[0x04] = 0x00;
            Buffer[0x05] = 0x00;
            Buffer[0x06] = 0x00;
            Buffer[0x07] = 0x00;

            Sync();
            SendPacket(8);

            _port.ReadTimeout *= ACK_TIMEOUT_MULTIPLYER; //HACK: Erease method needs more time than the original timeout time
            RecAck();
            _port.ReadTimeout /= ACK_TIMEOUT_MULTIPLYER;
        }

        /// <summary>
        /// Change BaudRate
        /// </summary>
        /// <returns>Successfulnes</returns>
        public void ChangeBaud()
        {
            Trace.WriteLine("ChangeBaud()");
            Buffer[0x00] = 0x80;
            Buffer[0x01] = 0x20;
            Buffer[0x02] = 0x04;
            Buffer[0x03] = 0x04;
            Buffer[0x04] = 0x00;
            Buffer[0x05] = 0xc8;
            Buffer[0x06] = 0x02;
            Buffer[0x07] = 0x00;

            Sync();
            SendPacket(8);
            RecAck();
            _port.BaudRate = 38400;
        }

        /// <summary>
        /// Send Password
        /// </summary>
        /// <param name="pwd">password</param>
        public void SendPWD(byte[] pwd)
        {
            if (pwd.Length > 32)
                throw new ArgumentException();

            Buffer[0x00] = 0x80;
            Buffer[0x01] = 0x10;
            Buffer[0x02] = 0x24;
            Buffer[0x03] = 0x24;
            Buffer[0x04] = 0x00;
            Buffer[0x05] = 0x00;
            Buffer[0x06] = 0x00;
            Buffer[0x07] = 0x00;

            byte i;
            for (i = 0; i < 32; i++)
            {
                Buffer[i + 8] = pwd[i];
            }

            Sync();
            SendPacket(40);
            RecAck();
        }

        /// <summary>
        /// Send 0xFF password
        /// </summary>
        /// <remarks>it is 32 times 0xFF; should be sended to a blank processor</remarks>
        /// <returns>password accepted</returns>
        public void SendFFPWD()
        {
            byte[] pwd = new byte[32]
        {
            255,255,255,255,255,255,255,255,
            255,255,255,255,255,255,255,255,
            255,255,255,255,255,255,255,255,
            255,255,255,255,255,255,255,255
        };
            SendPWD(pwd);
        }

        /// <summary>
        /// Read from flash
        /// </summary>
        /// <param name="address_in">Adress</param>
        /// <param name="lenght_in">number of bytes</param>
        /// <param name="buffer_out"> out buffer</param>
        /// <remarks>the password should be sent prior to this command</remarks>
        /// <returns>successfulness</returns>
        public void ReadFlash(UInt16 address_in, byte lenght_in, ref byte[] buffer_out)
        {
            Buffer[0x00] = 0x80;
            Buffer[0x01] = 0x14;
            Buffer[0x02] = 0x04;
            Buffer[0x03] = 0x04;
            Buffer[0x04] = (Byte)(address_in & 255);
            Buffer[0x05] = (Byte)((address_in >> 8) & 255);
            Buffer[0x06] = lenght_in;
            Buffer[0x07] = 0x00;

            Sync();
            SendPacket(8);
            ReceivePacket(lenght_in + 4);
            //RecAck();

            byte i;
            for (i = 0; i < lenght_in; i++)
            {
                buffer_out[i] = Buffer[i + 4];
            }
        }

        /// <summary>
        /// Write to flash
        /// </summary>
        /// <param name="Address">Adress</param>
        /// <param name="nn">number of bytes</param>
        /// <param name="OutBuffer"> out buffer</param>
        /// <returns>successfulness</returns>
        public void WriteFlash(UInt16 Address, byte DataLength, byte[] InBuffer)
        {
            if (DataLength % 2 == 1)
                throw new ArgumentException("The DataLength must be even.");

            // Prepare Data
            //   Header
            Buffer[0x00] = 0x80;
            Buffer[0x01] = 0x12;
            Buffer[0x02] = (Byte)(DataLength + 4);
            Buffer[0x03] = (Byte)(DataLength + 4);
            Buffer[0x04] = (Byte)(Address & 255);
            Buffer[0x05] = (Byte)((Address >> 8) & 255);
            Buffer[0x06] = DataLength;
            Buffer[0x07] = 0x00;
            //   Data
            byte i;
            for (i = 0; i < DataLength; i++)
            {
                Buffer[i + 8] = InBuffer[i];
            }

            // Send Data
            Sync();
            SendPacket(DataLength + 8);
            RecAck();
        }

        #region IProtocol Members

        public void InitProtocol()
        {
            this.InitPort();
        }

        #endregion IProtocol Members
    }
}