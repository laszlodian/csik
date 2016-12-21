using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.IO.Ports;

namespace WinFormBlankTest
{
    public partial class DetailedPanel : Panel
    {
        public delegate void MessageCompletedEventHandler(MessageCompletedEventArgs a, Panel currForm);
        public event MessageCompletedEventHandler MessageComplete;
        public static Thread myThread;
        public byte[] receivedPackets = new byte[11];

        public int month;
        public SerialPort[] portsAvailable = new SerialPort[16];
        public BackgroundWorker worker = new BackgroundWorker();

        public UserPanel usersPanel;
        public Device TheDevice=new Device();
        public DetailedPanel ThePanel;
        
        public delegate void NoteWriteEventHandler(NoteWriteEventArgs na, string text, string portname);
        public event NoteWriteEventHandler NoteWriting;

        public DetailedPanel()
        {
        }

        public DetailedPanel(SerialPort _port, UserPanel _usersPanel, Device dev)
        {
            _port.ErrorReceived += new SerialErrorReceivedEventHandler(ErrorReceived);

            this.MessageComplete += new MessageCompletedEventHandler(Process);
            this.NoteWriting += new NoteWriteEventHandler(WriteNote);

            DoubleBuffered = true;
            usersPanel = _usersPanel;
            ThePanel = this;
            TheDevice = dev;

            worker.DoWork += backgroundWorker1_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            InitializeComponent();

            this.Name = _port.PortName;

            worker.RunWorkerAsync(_port);

           // richTextBox1.TextChanged += RichTextBox1_TextChanged;
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            //RemoveLines(sender as RichTextBox, 250);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            { ;}
            if (e.Error != null)
            {
               
                WriteNote(new NoteWriteEventArgs(), "\nKommunikációs hiba!\nHiba a sorosporti kommunikációban!\n\nException after main thread completed!!!\n", this.Name);
                Trace.TraceError("\nKommunikációs hiba\nError Received in SerialPort\n\nException after main thread completed!!!\n");
            }
            if (e.Result != null)
                Trace.TraceInformation("BackgroundWorker FINISHED SUCCESSFULL..");
        }

        public void Process(MessageCompletedEventArgs ar, Panel actPanel)
        {

        }
        public void ErrorReceived(object sender, SerialErrorReceivedEventArgs arg)
        {
            if (Program.measureType != "meo")
            {
                new Thread(() => MessageBox.Show(string.Format("\nKommunikációs (paritásbit)hiba!\nKezdje Újra a mérést a {0} porton!\n", this.Name)));    
            }
            
            WriteNote(new NoteWriteEventArgs(),"\nKommunikációs hiba!\nParitás-hiba a sorosporti kommunikációban\nHibás paritásbit\n",this.Name);
            Trace.TraceError("\nKommunikációs hiba!\nError Received in SerialPort\nHibás paritásbit\n");
        }
        delegate void AppendTextDelegate(string value, Panel myForm);
        public void AppendText(string text, Panel myForm)
        {
            RichTextBox current = new RichTextBox();
            ((DetailedPanel)myForm).DoubleBuffered = true;
            foreach (Control c in myForm.Controls)
                if (c is RichTextBox)
                    current = ((RichTextBox)c);
            if (myForm.InvokeRequired)
            {
                myForm.Invoke(new AppendTextDelegate(AppendText), text, myForm);

            }
            else
            {
                //RemoveLines(current,50);               
                current.AppendText(text);  

            }
        }


        delegate void RemoveLinesDelegate(TextBoxBase textBox, int maxLine);
        void RemoveLines(TextBoxBase textBox, int maxLine)
        {
            if (textBox.InvokeRequired)
            {
                textBox.Invoke(new RemoveLinesDelegate(RemoveLines), textBox, maxLine);
            }
            else
            {
                int plusLines = textBox.Lines.Length - maxLine;
                if (plusLines > 0)
                {
                    int start_index = textBox.GetFirstCharIndexFromLine(0);
                    int count = textBox.GetFirstCharIndexFromLine(plusLines) - start_index;
                    textBox.Text = textBox.Text.Remove(start_index, count);
                }
            }
        }


        delegate void ChangeTextDelegate(string value, Panel myForm);
        public void ChangeText(string text, Panel myForm)
        {
            RichTextBox current = new RichTextBox();
            foreach (Control c in myForm.Controls)
                if (c is RichTextBox)
                    current = ((RichTextBox)c);

            if (myForm.InvokeRequired)
            {
                myForm.Invoke(new ChangeTextDelegate(ChangeText), text, myForm);

            }
            else
            {               
                current.Text = text;
              
            }
        }

        delegate void SetLabelVisibleDelegate();
        public void SetLabelVisible()
        {
            if (usersPanel.InvokeRequired)
            {
                usersPanel.Invoke(new SetLabelVisibleDelegate(SetLabelVisible));
            }
            else
                foreach (Control lb in usersPanel.Controls)
                {
                    if (lb is Label)
                    {                       
                            ((Label)lb).Visible = true;                        
                       
                    }
                }
        }

        delegate void SetLabelInvisibleDelegate();
        public void SetLabelInvisible()
        {
            if (usersPanel.InvokeRequired)
            {
                usersPanel.Invoke(new SetLabelInvisibleDelegate(SetLabelInvisible));
            }
            else
                foreach (Control lb in usersPanel.Controls)
                {
                    if (lb is Label)
                    {
                        if (((Label)lb).Name == "errorLabel")
                        {
                            ((Label)lb).Visible = false;
                        }
                    }
                }
        }

        public void ReadPort(SerialPort _port)
        {
            int dataCount;
            byte[] portBuffer = new byte[11];
            int index = 0;
            AppendText(string.Format("\nWaiting for data on port:{0}\n", _port.PortName), this);
           
            _port.DiscardInBuffer();
            _port.DiscardOutBuffer();

            while (true)
            {
                #region CheckBarcode
                if ((!usersPanel.dev.BarCodeOK) && (Properties.Settings.Default.BlockForBarcode))
                {
                    Trace.TraceInformation("{0}", usersPanel.dev.BarCode);
                    SetLabelVisible();
                    while (!usersPanel.dev.BarCodeOK)
                    {
                       
                        Thread.Sleep(20);
                    }
                    Trace.TraceInformation("{0}", usersPanel.dev.BarCode);
                    SetLabelInvisible();
                    _port.DiscardInBuffer();
                }
                #endregion

                if (!_port.IsOpen)
                {
                    _port.Open();
                }
                #region Inner while
                while (!this.TheDevice.PacketIsReady)
                {

                    dataCount = _port.BytesToRead;      //get the count of readable bytes
                    
                        if (dataCount > 0)
                        {
                            _port.Read(portBuffer, 0, 1);           //receive the first byte

                            #region check header
                            if ((portBuffer[0] == ((byte)170)))       //check header byte 1    
                            {
                                this.TheDevice.headerFirstIsOk = true;
                                dataCount = _port.BytesToRead;
                                if (dataCount > 0)           //if any byte to receive
                                {
                                    _port.Read(portBuffer, 1, 1);       //receive the second byte
                                    if ((portBuffer[1] == ((byte)85)))       //check header byte 2 
                                    {
                                        
                                        this.TheDevice.headerSecondIsOk = true;
                                        dataCount = _port.BytesToRead;
                                        ReadRemainingBytes(_port, dataCount, portBuffer);

                                    }
                                    else
                                    {
                                        Trace.TraceError("Second byte is not 85 secondbyte:{0}", portBuffer[1]);
                                        this.TheDevice.headerSecondIsOk = false;
                                        this.TheDevice.headerFirstIsOk = false;
                                    }
                                }
                            }
                            else
                            {
                                Trace.TraceError("First byte is not 170, firstbyte:{0}",portBuffer[0]);
                                this.TheDevice.headerFirstIsOk = false;
                                
                            }
                            #endregion
                            if (this.TheDevice.headerSecondIsOk
                                && this.TheDevice.headerFirstIsOk
                                && this.TheDevice.PacketIsPreProcessed)
                                 SetReceivedBytes( portBuffer,  index);


                        }
                        Thread.Sleep(10);
                    
                }//inner while

                #endregion

                  #region Process

                     Trace.TraceInformation("Data processing started");
                    MessageComplete(new MessageCompletedEventArgs(receivedPackets, this, (UserPanel)usersPanel), this);

                    this.TheDevice.headerSecondIsOk = false;
                    this.TheDevice.headerFirstIsOk = false;
                    this.TheDevice.PacketIsReady = false;
             
                    index = 0;
                    receivedPackets = new byte[11];
                    Trace.TraceInformation("Data processing finished");
                
                    Thread.Sleep(10);

                  #endregion
                
            }//outer while   
            
        }
         
        public void ReadRemainingBytes(SerialPort _port, int dataCount, byte[] portBuffer)                    
        {      
            int timeout = 10;
            while ((dataCount<9) && (timeout>0))
            {                            
                dataCount = _port.BytesToRead;
                Thread.Sleep(12);
                timeout--;
            }
            if (dataCount >= 9)
            {
                _port.Read(portBuffer, 2, 9);       //read the other 9 bytes from datapackage     
              
                this.TheDevice.PacketIsPreProcessed = true;
            }
            else
            {  
                this.TheDevice.wrong_step="Timeout";
                WriteNote(new NoteWriteEventArgs(), "Timeout after the HEADER of the data package received.", this.Name);                
                Trace.TraceError("Timeout after the HEADER of the data package received.");
            }
        }
        void SetReceivedBytes( byte[] portBuffer,  int index)
        {
            AppendText("\nHeader OK", this);
            for (int i = 0; i < portBuffer.Length; i++)
            {
                index++;
                receivedPackets[i] = portBuffer[i];
            }
            this.TheDevice.PacketIsReady = true;
            this.TheDevice.PacketIsPreProcessed = false;
            this.TheDevice.headerFirstIsOk = false;
            this.TheDevice.headerSecondIsOk = false;
            portBuffer = new byte[11];
        }
        public void WriteNote(NoteWriteEventArgs na, string txt, string port)
        {
            if (!this.TheDevice.isNoteing)
            {
                this.TheDevice.isNoteing = true;
                GetMonth();
                string path = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));

                if (!Directory.Exists(string.Format("{0}\\..\\..\\Logs", path)))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(string.Format("{0}\\..\\..\\", path), "Logs"));
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(string.Format("{0}\\..\\..\\Logs\\{2}_{1}.txt", path, month, port), true))
                {
                    file.WriteLine(string.Format("{0}-{1}", DateTime.Now.ToString(), txt));
                }
                this.TheDevice.isNoteing =false;
            }
        }
        public int GetMonth()
        {
            month = System.DateTime.Now.Month;
            return month;
        }
        public void tbBarcode_textchanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            ChangeText(string.Empty, this);
        }
        
        public SerialPort myPort;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument is SerialPort)
                myPort = (SerialPort)e.Argument;
            
                ReadPort(myPort);
        }

    }
}
