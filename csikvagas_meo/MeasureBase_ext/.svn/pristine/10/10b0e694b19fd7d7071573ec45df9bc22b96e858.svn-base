using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.ComponentModel;

namespace e77.MeasureBase
{
    public class PortConfiguration
    {
        #region Properties
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public bool DiscardNull { get; set; }
        public Handshake Handshake { get; set; }
        public String NewLine { get; set; }
        public Parity Parity { get; set; }
        public byte ParityReplace { get; set; }
        public String PortName { get; set; }
        public int ReadBufferSize { get; set; }
        public int ReadTimeout { get; set; }
        public int ReceivedBytesThreshold { get; set; }
        public StopBits StopBits { get; set; }
        public int WriteBufferSize { get; set; }
        public int WriteTimeout { get; set; }
        #endregion

        #region Constructors
        public PortConfiguration()
        {

        }
        public PortConfiguration(SerialPort port_in)
        {
            StoreConfiguration(port_in);
        }
        #endregion

        #region Methods
        public void StoreConfiguration(SerialPort port_in)
        {
            this.BaudRate = port_in.BaudRate;
            this.DataBits = port_in.DataBits;
            this.DiscardNull = port_in.DiscardNull;
            this.Handshake = port_in.Handshake;
            this.NewLine = port_in.NewLine;
            this.Parity = port_in.Parity;
            this.ParityReplace = port_in.ParityReplace;
            this.PortName = port_in.PortName;
            this.ReadBufferSize = port_in.ReadBufferSize;
            this.ReadTimeout = port_in.ReadTimeout;
            this.ReceivedBytesThreshold = port_in.ReceivedBytesThreshold;
            this.StopBits = port_in.StopBits;
            this.WriteBufferSize = port_in.WriteBufferSize;
            this.WriteTimeout = port_in.WriteTimeout;
        }

        public void ConfigurePort(SerialPort port)
        {
            port.BaudRate = this.BaudRate;
            port.DataBits = this.DataBits;
            port.DiscardNull = this.DiscardNull;
            port.Handshake = this.Handshake;
            port.NewLine = this.NewLine;
            port.Parity = this.Parity;
            port.ParityReplace = this.ParityReplace;
            //if (!port.IsOpen) port.PortName = this.PortName;
            //if (!port.IsOpen) port.ReadBufferSize = this.ReadBufferSize;
            port.ReadTimeout = this.ReadTimeout;
            port.ReceivedBytesThreshold = this.ReceivedBytesThreshold;
            port.StopBits = this.StopBits;
            //if (!port.IsOpen) port.WriteBufferSize = this.WriteBufferSize;
            port.WriteTimeout = this.WriteTimeout;
        }
        #endregion
    }
    //class PortState : PortConfiguration
    //{
    //    #region Properties
    //    public bool BreakState { get; set; }
    //    public int BytesToRead { get; set; }
    //    public int BytesToWrite { get; set; }
    //    public bool CDHolding { get; set; }
    //    public bool CtsHolding { get; set; }
    //    public bool DsrHolding { get; set; }
    //    public bool DtrEnable { get; set; }
    //    public Encoding Encoding { get; set; }
    //    public bool IsOpen { get; set; }
    //    public bool RtsEnable { get; set; }
    //    #endregion

    //    public PortState(SerialPort port_in)
    //        : base(port_in)
    //    {
    //        this.BreakState = port_in.BreakState;
    //        this.BytesToRead = port_in.BytesToRead;
    //        this.BytesToWrite = port_in.BytesToWrite;
    //        this.CDHolding = port_in.CDHolding;
    //        this.CtsHolding = port_in.CtsHolding;
    //        this.DsrHolding = port_in.DsrHolding;
    //        this.DtrEnable = port_in.DtrEnable;
    //        this.Encoding = port_in.Encoding;
    //        this.IsOpen = port_in.IsOpen;
    //        this.RtsEnable = port_in.RtsEnable;
    //    }
    //    public void ConfigurePort(SerialPort port)
    //    {
    //        base.ConfigurePort(port);
    //        port.BreakState = this.BreakState;
    //        //port.BytesToRead = this.BytesToRead;
    //        //port.BytesToWrite = this.BytesToWrite;
    //        //port.CDHolding = this.CDHolding;
    //        //port.CtsHolding = this.CtsHolding;
    //        //port.DsrHolding = this.DsrHolding;
    //        port.DtrEnable = this.DtrEnable;
    //        port.Encoding = this.Encoding;
    //        //port.IsOpen = this.IsOpen;
    //        port.RtsEnable = this.RtsEnable;      

    //    }
    //}

    //class PortSnapshoot : PortState
    //{
    //    public Stream BaseStream { get; set; }
    //    public IContainer Container { get; set; }
    //    public ISite Site { get; set; }

    //    public PortSnapshoot(SerialPort port_in)
    //        : base(port_in)
    //    {
    //        this.BaseStream = new MemoryStream(port_in.BaseStream
    //        this.Container = port_in.Container;
    //        this.Site = port_in.Site;
    //    }
    //}
}
