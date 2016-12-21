using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using e77.MeasureBase.Communication;
using e77.MeasureBase.Data;

namespace e77.MeasureBase.Electronics
{
    public class MSP430 : Device<MSP430Protocol>
    {
        public delegate FlashBuffer CreateFlashBufferDelegate();

        /// <summary>
        ///
        /// </summary>
        /// <param name="PortName"></param>
        /// <param name="create_in">can be null</param>
        public MSP430(String PortName, CreateFlashBufferDelegate create_in)
        {
            _protocol = new MSP430Protocol(PortName);
            if (create_in != null)
                _flashBuffer = create_in();
            else
                _flashBuffer = new FlashBuffer(BufferMaxSize);
        }

        public MSP430(String PortName)
            : this(PortName, null)
        { ;}

        FlashBuffer _flashBuffer;
        public const int BufferMaxSize = UInt16.MaxValue + 1;

        public FlashBuffer FlashBuffer
        {
            get { return _flashBuffer; }
        }

        private const int PackageSize = 240;
        public delegate void ProgressDelegate(float percent_in);

        public void WriteProgram(ProgressDelegate progressDelgate_in)
        {
            WriteProgram(progressDelgate_in, null, null);
        }

        public void WriteProgram(BackgroundWorker worker_in, DoWorkEventArgs e_in)
        {
            WriteProgram(null, worker_in, e_in);
        }

        private void WriteProgram(ProgressDelegate progressDelgate_in, BackgroundWorker worker_in, DoWorkEventArgs e_in)
        {
            #region Prepare data

            //FlashBuffer.ReadFlashHexFile(".\\optical_head_docureader2.hex"); // HACK-HL: paraméterezhetővé tenni
            if (FlashBuffer.UsedSize == 0)
                throw new ArgumentException("protocol_in.FlashBuffer is empty");

            if (FlashBuffer.IsDirty)
                FlashBuffer.GenerateCRC();

            #endregion Prepare data

            #region Prepare controller

            Trace.WriteLine("Programozó eszköz feltöltése:\n A {0}soros port megnyitása", PortName);
            if (!_protocol.IsOpen)
                _protocol.Open(false, false);
            else
                _protocol.InitProtocol();

            _protocol.InitBST();
            _protocol.ChangeBaud();
            _protocol.MassErase();
            _protocol.SendFFPWD();

            #endregion Prepare controller

            #region Sending data

            //send buffer:
            int Address;
            int StartAddress = 0;
            byte ByteCounter = 0;
            int ProgrammedBytes = 0;
            byte[] buf = new byte[256];

            for (Address = 0; Address < BufferMaxSize && (worker_in == null || !worker_in.CancellationPending); Address++)
            {
                byte Data;

                bool Used = FlashBuffer.GetByte(Address, out Data);

                if (Used)
                {
                    buf[ByteCounter] = Data;
                    ByteCounter++;
                }

                if ((!Used) || (ByteCounter >= PackageSize) || (Address == 0xffff))
                {
                    if (ByteCounter > 0)
                    {
                        ProgrammedBytes += ByteCounter;

                        if (progressDelgate_in != null)
                            progressDelgate_in((100f * ProgrammedBytes) / FlashBuffer.UsedSize);
                        if (worker_in != null)
                            worker_in.ReportProgress((int)((100f * ProgrammedBytes) / FlashBuffer.UsedSize));
                        _protocol.WriteFlash((UInt16)(StartAddress), ByteCounter, buf);
                    }

                    ByteCounter = 0;
                    StartAddress = Address + 1;
                }
            }
            if (worker_in != null && worker_in.CancellationPending) e_in.Cancel = true;

            #endregion Sending data

            #region Finalizing

            _protocol.Reset();

            #endregion Finalizing
        }

        public FlashBuffer ReadProgram(BackgroundWorker worker_in, DoWorkEventArgs e_in, byte[] pwd)
        {
            return ReadProgram(null, worker_in, e_in, pwd);
        }

        private FlashBuffer ReadProgram(ProgressDelegate progressDelgate_in, BackgroundWorker worker_in, DoWorkEventArgs e_in, byte[] pwd)
        {
            #region Prepare data

            FlashBuffer fb = new FlashBuffer(BufferMaxSize);
            ////FlashBuffer.ReadFlashHexFile(".\\optical_head_docureader2.hex"); // HACK-HL: paraméterezhetővé tenni
            //if (FlashBuffer.UsedSize == 0)
            //    throw new ArgumentException("protocol_in.FlashBuffer is empty");

            //if (FlashBuffer.IsDirty)
            //    FlashBuffer.GenerateCRC();

            #endregion Prepare data

            #region Prepare controller

            Trace.WriteLine("Programozó eszköz feltöltése:\n A {0}soros port megnyitása", PortName);
            if (!_protocol.IsOpen)
                _protocol.Open(false, false);
            else
                _protocol.InitProtocol();

            _protocol.InitBST();
            _protocol.ChangeBaud();
            //_protocol.MassErase();

            _protocol.SendPWD(pwd);

            #endregion Prepare controller

            #region Sending data

            //send buffer:

            int StartAddress = 0xC000;
            byte ReadBlockSize = 128;
            int StopAddress = 0x10000;
            int ReadAdressLength = StopAddress - StartAddress;

            byte[] buf = new byte[256];
            if (StopAddress > BufferMaxSize)
                throw new ArgumentOutOfRangeException("Stop address is over the physical address range");

            for (int Address = StartAddress; Address < StopAddress && (worker_in == null || !worker_in.CancellationPending); Address += ReadBlockSize)
            {
                if (progressDelgate_in != null)
                    progressDelgate_in((100f * (Address - StartAddress)) / ReadAdressLength);
                if (worker_in != null)
                    worker_in.ReportProgress((int)((100f * (Address - StartAddress)) / ReadAdressLength));

                if ((Address + ReadBlockSize <= StopAddress))
                {
                    _protocol.ReadFlash((UInt16)(Address), ReadBlockSize, ref buf);
                    fb.SetRange(Address, buf, ReadBlockSize);
                }
                else
                {
                    byte blockSize = (byte)(StopAddress - Address);
                    _protocol.ReadFlash((UInt16)(Address), blockSize, ref buf);
                    fb.SetRange(Address, buf, blockSize);
                }
            }
            if (worker_in != null && worker_in.CancellationPending) e_in.Cancel = true;

            #endregion Sending data

            #region Finalizing

            _protocol.Reset();
            return fb;

            #endregion Finalizing
        }

        public void EreaseProgram()
        {
            Trace.WriteLine("Programozó eszköz feltöltése:\n A {0}soros port megnyitása", PortName);
            if (!_protocol.IsOpen)
                _protocol.Open(false, false);
            else
                _protocol.InitProtocol();

            _protocol.InitBST();
            //_protocol.ChangeBaud();
            _protocol.MassErase();
            _protocol.SendFFPWD();
            _protocol.Reset();
        }

        public void StorePortConfiguration()
        {
            _protocol.StorePortConfiguration();
        }

        public void RestorePortConfiguration()
        {
            _protocol.RestorePortConfiguration();
        }

        public string PortName
        {
            get { return _protocol.PortName; }
            //set { _protocol.PortName = value; }
        }

        public override string CollectionKey
        {
            get { return "MSP430"; }
        }

        public override bool TestConnection()
        {
            if (!_protocol.IsOpen)
                _protocol.Open(false, false);
            else
                InitConnection();
            bool ret = _protocol.TestConnection();
            _protocol.Reset();
            System.Threading.Thread.Sleep(250);
            // Do not close the port
            // http://stackoverflow.com/questions/7348580/serialport-unauthorizedaccessexception
            // point:  The normal practice is to open the port when your app starts and not close it until it terminates.
            //_protocol.Close();
            return ret;
        }

        public override void ResetDevice()
        {
            _protocol.Reset();
        }

        public override void InitConnection()
        {
            _protocol.InitProtocol();
        }

        public override void CloseConnection()
        {
            if (_protocol.IsOpen)
                _protocol.Close();
        }
    }
}