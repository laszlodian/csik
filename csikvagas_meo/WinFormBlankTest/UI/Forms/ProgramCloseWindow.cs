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

namespace WinFormBlankTest.UI.Forms
{
    public partial class ProgramCloseWindow : Form
    {

        Stopwatch sw;

        public Stopwatch Stopper
        {
            get
            {

                return sw;
            }

            set
            {
                sw = value;
            }
        }


        public ProgramCloseWindow(Stopwatch stopper)
        {
            InitializeComponent();

            this.Load += new EventHandler(ProgramCloseWindow_Load);

            sw = stopper;
            while (this.sw.IsRunning)
	            {
                    label1.Text = string.Format("A program {0} másodperc múlva bezáródik!!", new TimeSpan(0, 0, 10) - sw.Elapsed);

                    if (sw.Elapsed >= new TimeSpan(0, 0, 10))
                    {
                        this.Close();
                        Program.CloseWindow();
                        Environment.Exit(Environment.ExitCode);
                    } 
	            }
            

            
        }

        void ProgramCloseWindow_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = null;
            this.BackColor = Color.Red;
            Thread.Sleep(2000);
            this.BackColor = Color.Transparent;
            this.BackgroundImage = Properties.Resources.elektronik;
        }


        //Don't close it now
        private void button2_Click(object sender, EventArgs e)
        {
            sw.Reset();
            sw.Start();

            this.Hide();
            this.Visible = false;

            if (sw.Elapsed >= new TimeSpan(0,2,0))
            {
                this.BringToFront();
                this.Visible = true;
            }

        }


        /// <summary>
        /// Close it Now
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.CloseWindow();
            Environment.Exit(Environment.ExitCode);
        }
    }
}
