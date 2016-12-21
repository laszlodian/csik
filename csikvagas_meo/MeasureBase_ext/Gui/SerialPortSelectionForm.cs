using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace e77.MeasureBase.GUI
{
    public partial class SerialPortSelectionForm : Form
    {
        const int MAX_LENGHT = 200;
        public SerialPortSelectionForm()
        {
            InitializeComponent();

            foreach (string port in SerialPort.GetPortNames())
            {
                SerialPort sp = new SerialPort(port);
                sp.BaudRate = 115200;
                sp.Parity = Parity.None;

                try
                {
                    sp.Open();

                }
                catch
                {
                    ;
                }

                ListViewItem item = new ListViewItem( new string[] {"", sp.IsOpen ? "Nyitva" : "Foglalt", ""});
                item.Text = port;
                if (sp.IsOpen)
                {
                    sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                    item.Tag = sp;
                }
                else
                    sp.Dispose();

                _listView.Items.Add(item);
            }
        }

        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new DataReceivedDelegate(DataReceived), (SerialPort)sender);
        }

        delegate void DataReceivedDelegate(SerialPort sp_in);
        public void DataReceived(SerialPort sp_in)
        {
            string str = sp_in.ReadExisting();

            foreach (ListViewItem item in _listView.Items)
            {
                if (item.Text == sp_in.PortName)
                {
                    str =  string.Format("{0}{1}", item.SubItems[2].Text, str);
                    if (str.Length > MAX_LENGHT)
                        str = str.Substring(str.Length - MAX_LENGHT);

                    item.SubItems[2].Text = str;
                    return;//OK
                }
            }

            throw new MeasureBaseInternalException("Port '{0}' is not founded", sp_in.PortName);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            foreach(ListViewItem item in _listView.Items)
                if (item.Tag != null)
                {
                    ((SerialPort)item.Tag).Close();
                    ((SerialPort)item.Tag).Dispose();
                }
            base.OnClosing(e);
        }

        
    }
}
