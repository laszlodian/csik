﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Timers;
using System.Diagnostics;
using WinFormBlankTest.UI.Forms;
using WinFormBlankTest.UI.Forms.Other_Forms;
using WinFormBlankTest.UI.Panels;

namespace WinFormBlankTest
{
    public partial class UserWindow : Form
    {
        #region Variables
        public UserPanel uPanel;
        public Graphics myGC=userTables.CreateGraphics();
        public static TableLayoutPanel userTables = new TableLayoutPanel();
        public static SerialPort[] portsAvailable;
        public static Panel[] myPanel;
       
        Bitmap Background, BackgroundTemp;
        public int number = 0; 
        #endregion
       
        /// <summary>
        /// Default Constructor
        /// </summary>
        public UserWindow()
        {           
            userTables.Visible = false;
            userTables.ForeColor = Color.Black;
            userTables.Dock = DockStyle.Fill;
            AutoScroll = true;            

            CreateButton_Click();

            if (Program.measureType!="accuracy")
            {
                this.FormClosed += new FormClosedEventHandler(frm_FormClosed);

            }
            else if (Program.measureType=="accuracy")
            {
                this.FormClosed+=new FormClosedEventHandler(UserWindow_FormClosed);
            }
            
            this.DoubleBuffered = true;
          
            InitializeComponent();

            Initialize(); 

            userTables.Visible = true;           

            this.Text = string.Format("{0} Check", Program.measureType.ToUpper());

            if (Program.measureType=="accuracy")
            {
                this.Text = string.Format("{0} Check - Vérminta:{1} - Kör:{2}",Program.measureType.ToUpper(),Program.Accuracy_sample_blood_vial_ID,Program.Round);
                SetButtonFinished();
                this.Controls.Add(btAccuracyFinished);
            }

        }

        public void SetButtonFinished()
        {
            btAccuracyFinished.Name = "btAccuracyFinished";
            btAccuracyFinished.Size = new System.Drawing.Size(75, 28);
            btAccuracyFinished.TabIndex = 2;
            btAccuracyFinished.Text = "Következő 16 mérés";
            btAccuracyFinished.UseVisualStyleBackColor = true;
            btAccuracyFinished.BackColor = Color.Lime;
            btAccuracyFinished.Font = new System.Drawing.Font("Arial Black", 12.0f);
            btAccuracyFinished.Visible = false;
            btAccuracyFinished.Enabled = true;
            btAccuracyFinished.Dock = DockStyle.Bottom;
            
            btAccuracyFinished.Click += new System.EventHandler(nextRound);
            
        }

        private void nextRound(object sender, EventArgs e)
        {
            this.Close();
        }


        AccuracyFinishedResultView accuracyResultView;
        private void buttonFinishedClick(object sender, EventArgs e)
        {
            accuracyResultView = new AccuracyFinishedResultView(Program.LOT_ID,Program.master_lot_id);
            accuracyResultView.ShowDialog();
           
        }
        public Button btAccuracyFinished = new Button();
   
        public SerialPort uPanelPort;
  
        void UserWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
        }

        protected override void Dispose(bool disposing)
        {

            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
            BackgroundTemp.Dispose();
            Background.Dispose();
        }

        private void Initialize()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            string AppPath = Application.StartupPath;
            BackgroundTemp = new Bitmap(Properties.Resources.elektronik);
            Background = new Bitmap(BackgroundTemp, BackgroundTemp.Width, BackgroundTemp.Height);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            #region Commented out but interesting
            /*   myGC = userTables.CreateGraphics();
           
            myGC.DrawImageUnscaled(Background, 0, 0);

            myGC = Graphics.FromImage(Properties.Resources.elektronik);
            myGC.DrawImage(Properties.Resources.elektronik,userTables.Location);
            userTables.BackgroundImage = Properties.Resources.elektronik;
           */

            #endregion
        }
        delegate void SetOpacityDelegate(object sender, ElapsedEventArgs elapsedArg);
        public void SetOpacity(object sender, EventArgs elapsedArg)
        {
            this.Opacity += 10;
        }

        public void frm_FormClosed(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.ClosePortsWhenFormClosed)
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
            }        
            
           Environment.Exit(Environment.ExitCode);
        }


        /// <summary>
        /// Initialize tablelayout panel
        /// </summary>
        private void CreateButton_Click()
        {
            userTables.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            userTables.Size = userTables.MinimumSize;
           
            userTables.Location = new System.Drawing.Point(Screen.PrimaryScreen.WorkingArea.Width - userTables.Width, Screen.PrimaryScreen.WorkingArea.Height - userTables.Height);
            userTables.Name = "TableLayoutPanel1";
            userTables.ForeColor = Color.Azure;
            userTables.AutoSize = false;
            userTables.Dock=DockStyle.Fill;
            // Add rows and columns
            userTables.ColumnCount = 4;
            userTables.RowCount = 4;
          
            this.Controls.Add(userTables);

            this.Controls.Add(new CounterPanel(Program.TubeCount));

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }
       
        private void UserWindow_Shown(object sender, EventArgs e)
        {
            foreach (Control item in userTables.Controls)
            {
                if (item is RichTextBox)
                {
                    ((RichTextBox)(item)).Text = string.Empty;
                }
            }
        }

        
    }
}
