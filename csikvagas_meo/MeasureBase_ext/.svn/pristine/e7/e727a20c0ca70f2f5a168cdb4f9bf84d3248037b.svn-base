using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;


namespace e77.MeasureBase.Communication
{
    public class SerialLisPort : SerialPortProxy, ILisPort
    {
        public SerialLisPort()
        {
        }

        public const string PortNamePropertyName = "PortName";

        public override string PortName
        {
            get
            {
                return base.PortName;
            }
            set
            {
                if (base.PortName != value)
                {
                    base.PortName = value;
                    OnPropertyChanged(PortNamePropertyName);
                }
            }
        }

        #region ILisPort
        IAsyncResult _asyncResult;

        public IAsyncResult BeginRead(byte[] buffer_in, int offset_in, int count_in, AsyncCallback callback_in)
        {
            _port.DataReceived += new SerialDataReceivedEventHandler(SerialLisPort_DataReceived);
            return _asyncResult = _port.BaseStream.BeginRead(buffer_in, offset_in, count_in, callback_in, this);
        }

        private void SerialLisPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_asyncResult != null)
            {
                _port.BaseStream.EndRead(_asyncResult);
                _asyncResult = null;
            }
        }

        public void WriteByte(byte data_in)
        {
            base.Write(new byte[] { data_in }, 0, 1);
        }

        public string ReadLine()
        {
            return _port.ReadLine();
        }

        #endregion ILisPort

        #region INotifyPropertyChanged Members

        public void OnPropertyChanged(String propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged Members
    }
}