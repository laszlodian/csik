using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace WinFormBlankTest
{
    public partial class MainWindow : Form
    {
        public static TableLayoutPanel dynamicTableLayoutPanel = new TableLayoutPanel();
     
        public MainWindow()
        {

            
            CreateButton_Click();
            this.FormClosed += new FormClosedEventHandler(frm_FormClosed);
            this.LocationChanged +=new EventHandler(MainWindow_MinimumSizeChanged);

            InitializeComponent();
        }
        private void MainWindow_MinimumSizeChanged(object sender, System.EventArgs e)
        {            
                this.WindowState = FormWindowState.Maximized;              
            
        }
        public void frm_FormClosed(object sender, EventArgs e)
        {
            string[] ports;
            SerialPort port;
            ports = SerialPort.GetPortNames();
            foreach (string item in ports)
            {
                port = new SerialPort(item);
                if (port.IsOpen)
                {
                    port.Close();
                }
            }
            Environment.Exit(Environment.ExitCode);
        }
        private void CreateButton_Click()
        {            
            dynamicTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            dynamicTableLayoutPanel.Name = "TableLayoutPanel1";
            dynamicTableLayoutPanel.Size = new System.Drawing.Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            dynamicTableLayoutPanel.BackColor = Color.LightBlue;
            // Add rows and columns
            dynamicTableLayoutPanel.ColumnCount = 4;
            dynamicTableLayoutPanel.RowCount = 4;
            this.Controls.Add(dynamicTableLayoutPanel);
        }

    }
}
