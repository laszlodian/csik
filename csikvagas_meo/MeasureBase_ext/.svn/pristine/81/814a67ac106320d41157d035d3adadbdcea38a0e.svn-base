using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace e77.MeasureBase.MeterDevices
{
    public class MeasureDoneEventArgs : EventArgs
    {
        public double Value { get; set; }
        public string PackageContent { get; set; }
        public MeasureDoneEventArgs(double value_in, string packageContent_in)
        {
            Value = value_in;
            PackageContent = packageContent_in;
        }
    }
        
    public delegate void MeasureDoneEventHandler(object sender, MeasureDoneEventArgs e);

    /// <summary>
    /// SQL: there is a char[] column for SN of all used meters. Column name: MeterDeviceBase.NameId.FromCamelCase(). Max Lenght: MAX_SN_LENGHT 
    /// </summary>
    public abstract class MeterDeviceBase : IDisposable
    {
        static public List<MeterDeviceBase> AllMeters = new List<MeterDeviceBase>();

        public MeterDeviceBase()
        {
            PackageLenght = -1;
            TerminationStr = "\r";
            AllMeters.Add(this);
        }

        public MeterDeviceBase(int? subId_in)
            :this()
        {
            SubId = subId_in;
        }

        
        public string Sn { get; set; }
        
        /// <summary>
        /// ID == NameId == DeviceNameId + Optional SubId (in case of more similar device is used)
        /// </summary>
        public int? SubId { get; private set; }

        /// <summary>
        /// ID == NameId == DeviceNameId + Optional SubId (in case of more similar device is used)
        /// </summary>
        public string NameId 
        {
            get
            {
                return SubId.HasValue
                    ? string.Format("{0}/{1}", DeviceNameId, SubId.Value)
                    : DeviceNameId;
            }
        }

        virtual public string DeviceNameId 
        {
            get { return this.GetType().Name; }
        }

        internal protected virtual bool IsStabilized(MeasureDoneEventArgs e)
        {
            throw new NotImplementedException("Override this function ifauto measure required for this device");
        }

        protected SerialPort Port { get; set; }
        protected StringBuilder CollectedValue = new StringBuilder();
        public float MeasValue;
        //public int MessageLenght;

        protected int PackageLenght { get; set; }
        protected string TerminationStr { get; set; }

        //public char TerminationChar2;

        AutoMeasure autoMeasure;
        public AutoMeasure AutoMeasure
        {
            get
            {
                return autoMeasure;
            }
            set
            {
                if (autoMeasure != value)
                {
                    if (autoMeasure != null)
                    {
                        autoMeasure.StopMeasure();
                        this.MeasureDone -= new MeasureDoneEventHandler(autoMeasure.MyMeter_MeasureDone);                        
                    }

                    autoMeasure = value;

                    if (autoMeasure != null)
                        this.MeasureDone += new MeasureDoneEventHandler(autoMeasure.MyMeter_MeasureDone);
                }
            }
        }

        System.Timers.Timer MeasureStartTimer;

        protected void OnMeasureDone(double value_in, string packageContent_in)
        {
            if(MeasureDone != null)
                MeasureDone.Invoke(this, new MeasureDoneEventArgs(value_in, packageContent_in));
        }

        public event MeasureDoneEventHandler MeasureDone;
   
        const char nullchar = (char)0x00;

        /// <summary>
        /// false by default: in this case user can cancel Init Window -> Measure can working without that meter. 
        ///     (in this case the Sn column of Sql should be nullable.
        /// </summary>
        public bool MustBeExist { get; set; }

        /// <summary>
        /// Can be false, only if MustBeExist is false.
        /// In this case the measure start API's will not send any error, just does nothing.
        /// </summary>
        public bool IsExist { get; set; }

        virtual public void Init()
        {
            MeterSetupWindow msw = new MeterSetupWindow(this);
            msw.ShowDialog();

            if (!msw.IsValid)
            {
                System.Diagnostics.Debug.Assert(!MustBeExist);
                IsExist = false;
                //todo: add menu item into Tools, for showing this Window again
            }
            else
            {
                Sn = msw.Sn;
                
                Port = new SerialPort(msw.PortName);
                InitPort(9600, Parity.None, 7, StopBits.Two);
                Port.Open();

                Port.DataReceived += new SerialDataReceivedEventHandler(Port_DataReceived);

                MeasureStartTimer = new System.Timers.Timer();
                MeasureStartTimer.Elapsed += new System.Timers.ElapsedEventHandler(MeasureStartTimer_Elapsed);
                IsExist = true;
            }
        }
        //for avoid RX data processing after stop
        bool RxAccepting = false;
        public virtual void MeasureOnce()
        {
            if (!IsExist)
                return;
            //makes empty buffer:
            Port.ReadExisting();
            lock(CollectedValue)
                CollectedValue = new StringBuilder();

            RxAccepting = true;
        }

        public void StartContinousMeasure(int period_in)
        {
            
            if (!IsExist)
                return;

            RxAccepting = true;

            Trace.TraceInformation("MeterDeviceBase.StartContinousMeasure(period_in = {0})", period_in);
            if (AutoMeasure != null)
                AutoMeasure.StartMeasure();

            MeasureStartTimer.Interval =  period_in;
            MeasureStartTimer.Start();
        }

        virtual public void StopContinousMeasure()
        {
            if (!IsExist)
                return;
            RxAccepting = false;

            Trace.TraceInformation("MeterDeviceBase.StopContinousMeasure()");
            MeasureStartTimer.Stop();
        }

        protected void InitPort(int baud_in, Parity parity_in, int dataBits, StopBits stopBits_in)
        {
            Port.BaudRate = baud_in;
            Port.Parity = parity_in;
            Port.DataBits = dataBits;
            Port.StopBits = stopBits_in;
        }


        // Serial port Received Event Handler
        private void Port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (!RxAccepting)
            {
                if(CollectedValue.Length > 0)
                    CollectedValue = new StringBuilder();
                return;
            }

            string arrivedPocket = Port.ReadExisting();
            //Trace.TraceInformation("MeterDeviceBase.Port_DataReceived(arrivedLine = '{0}') on Port {1}", arrivedLine, Port.PortName);
            bool PocketReady = false;

            lock (CollectedValue)
            {
                SerialPort port = (SerialPort)sender;
                CollectedValue.Append(arrivedPocket);
                

                if (PackageLenght == -1)
                {
                    int endOfLineIndex = CollectedValue.ToString().IndexOf(TerminationStr);
                    if (endOfLineIndex > 1) //ignores empty lines too
                    {
                        arrivedPocket = CollectedValue.ToString().Substring(0, endOfLineIndex);
                        CollectedValue = CollectedValue.Remove(0, endOfLineIndex + TerminationStr.Length);
                        PocketReady = true;
                    }
                }
                else
                {
                    if (TerminationStr != null)
                        throw new InvalidConfigurationException("Do not use TerminationStr if PackageLenght is using.");

                    if (CollectedValue.Length >= PackageLenght)
                    {
                        arrivedPocket = CollectedValue.ToString().Substring(0, PackageLenght);
                        CollectedValue = new StringBuilder();
                        PocketReady = true;
                    }
                }
            }

            if (PocketReady)
            {
                PocketReceived(arrivedPocket);
                
                if (!MeasureStartTimer.Enabled)//stop Rx in case of only one measure
                    RxAccepting = false;
            }
           
        }

        abstract public void PocketReceived(string arrivedLine_in);

        virtual protected void MeasureStartTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (MeasureStartTimer.Enabled)//avoid timer event after timer stopped...
                MeasureOnce();
        }

        #region IDisposable design pattern for base class imlementing IDisposable
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    IsExist = false;

                    StopContinousMeasure();
                    if(MeasureStartTimer != null)
                        MeasureStartTimer.Dispose();

                    // Dispose managed resources.
                    //freeze.... if (Port.IsOpen)                        Port.Close();
                    if(Port != null)
                        Port.Dispose();
                }

                // Call the appropriate methods to clean up unmanaged resources here.
                // If disposing is false, only the following code is executed.

                // Note disposing has been done.
                disposed = true;
            }
        }

        #endregion IDisposable design pattern for base class imlementing IDisposable
    }
}
