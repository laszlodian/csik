using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO.Ports;

namespace e77.MeasureBase.MeterDevices
{   
    /// <summary>
    /// Interaction logic for MeterSetupWindow.xaml
    /// </summary>
    public partial class MeterSetupWindow : Window
    {
        public MeterSetupWindow(MeterDeviceBase myMeter_in)
        {
            //todo! remove default SN
            MyMeter = myMeter_in;

            if (MyMeter.MustBeExist)
                _buttonCancel.IsEnabled = false;

            InitializeComponent();
            this.Closing += new System.ComponentModel.CancelEventHandler(MeterSetupWindow_Closing);
            
            _tbSN.Focus();
        }

        MeterDeviceBase MyMeter;
                
        void MeterSetupWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (manualClose)
                return;
            else if (MyMeter.MustBeExist || _tbSN.Text.Length > 0) //this meter can be ingnored
            {
                if (!CheckAll())
                {
                    e.Cancel = true;
                }
            }
        }

        bool CheckAll()
        {
            bool res = true;
            if (!UpdateSnValidity())
            {
                _tbSN.Focus();
                res = false;
            }
            else
                MeasureConfig.TheConfig.SetConfigValueOf(string.Format("{0}_LastSn", MyMeter.NameId), this.Sn);

            if (this.PortName == null && SerialPort.GetPortNames().Length > 0)
            {
                _cbPorts.Focus();
                _cbPorts.Background = Brushes.Orange;
                res = false;
            }
            else
            {
                MeasureConfig.TheConfig.SetConfigValueOf(string.Format("{0}_LastPort", MyMeter.NameId), this.PortName);
            }

            return res;
        }

        public bool UpdateSnValidity()
        {
            string snStateStr;
            IsValid = MeterDB.CheckMeter(_tbSN.Text, out snStateStr, true);
            _tbSnState.Text = snStateStr;
            return IsValid;
        }

        public bool IsValid{ get; private set;}
        
        public string Sn
        {
            get { return _tbSN.Text; }
            set { _tbSN.Text = value; }
        }

        public string PortName
        {
            get { return (string)_cbPorts.SelectedItem; }
            set { _cbPorts.SelectedItem = value; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string lastPort = (string)MeasureConfig.TheConfig.GetConfigValueOf("{0}_LastPort", MyMeter.NameId); //incase of exception: add this string config into any config XML file
            foreach (string portName in SerialPort.GetPortNames())
            {
                _cbPorts.Items.Add(portName);

                if (portName == lastPort)
                    _cbPorts.SelectedIndex = _cbPorts.Items.Count - 1;
            }

            _tbSN.Text = (string)MeasureConfig.TheConfig.GetConfigValueOf("{0}_LastSn", MyMeter.NameId); //incase of exception: add this string config into any config XML file
        }

        private void SnLostFocus(object sender, RoutedEventArgs e)
        {
            UpdateSnValidity();
        }

        private void _buttonOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        bool manualClose;
        private void _buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            manualClose = true;
            IsValid = false;
            this.Close();
        }
    }
}
