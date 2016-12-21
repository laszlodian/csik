using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using e77.MeasureBase;

namespace WinFormBlankTest.UI.Panels
{
    public partial class CounterPanel : UserControl
    {

        public static ProgressBar pb = new ProgressBar();
        public string lot_id;
        public int tube_count;
        public static CounterPanel Instance = null;

        public CounterPanel(int tubes)
        {
            InitializeComponent();

            Instance = this;
            tube_count = tubes;
            if (Program.measureType == "blank")
            {
                tbRemaining.Text = string.Format("{0}", tube_count);
            }
            else if (Program.measureType == "homogenity")
            {
                tbRemaining.Text = string.Format("{0}", tube_count * 4);
            }
            SetTubeCountLabel();
            SetNeedToMeasure();
            SetMeasuredCountToNull();

            SetLotId();

            SetRemainingToBegin();
            tbRemaining.Enabled = false;
            this.Dock = DockStyle.Right;
            this.tbLemert.TextChanged += new System.EventHandler(this.tbLemert_TextChanged);
           
        }

        private void AkyProgressBar1_Load(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(ProcessTime)).Start();
        }

        public void ProcessTime()
        {
            Thread.Sleep(2000);
        }

        private void SetRemainingToBegin()
        {
            string needToMeasure = string.Empty;
            if (Program.measureType == "blank")
            {
                needToMeasure = string.Format("{0}", Program.TubeCount);

            }
            else if (Program.measureType == "homogenity")
            {
                needToMeasure = string.Format("{0}", Program.TubeCount * 4);
            }
            NeedToMeasure(needToMeasure);
        }



        delegate void SetRemainingDelegate();
        private void SetRemaining()
        {
            if (tbRemaining.InvokeRequired)
	        {
                tbRemaining.Invoke(new SetRemainingDelegate(SetRemaining));
	        }else
            {
            tbRemaining.Text = string.Format("{0}");
            }
        }

        private void SetNeedToMeasure()
        {
            string needToMeasure = string.Empty;
            if (Program.measureType == "blank")
            {
                needToMeasure = string.Format("{0}", Program.TubeCount);

            }
            else if (Program.measureType == "homogenity")
            {
                needToMeasure = string.Format("{0}", Program.TubeCount * 4);
            }
            NeedToMeasure(needToMeasure);
        }



        delegate void NeedToMeasureDelegate(string needToMeasure);
        private void NeedToMeasure(string needToMeasure)    
        {
            if (tbNeedToMeasure.InvokeRequired)
	        {
                tbNeedToMeasure.Invoke(new NeedToMeasureDelegate(NeedToMeasure),needToMeasure);
	        }else
            {

            tbNeedToMeasure.Text = needToMeasure;
            tbNeedToMeasure.Enabled = false;
            }
        }

        delegate void SetLotIdDelegate();
        private void SetLotId()
        {
            if (tbLot.InvokeRequired)
            {
                tbLot.Invoke(new SetLotIdDelegate(SetLotId));
            }
            else
            {
                tbLot.Text = Program.LOT_ID;
                tbLot.Enabled = false;
            }
        }


        delegate void SetTubeCountLabelDelegate();
        public void SetTubeCountLabel()
        {
            if (tbTubus.InvokeRequired)
            {
                tbTubus.Invoke(new SetTubeCountLabelDelegate(SetTubeCountLabel));
            }
            else
            {
                tbTubus.Text = string.Format("{0}", Program.TubeCount);
                tbTubus.Enabled = false;
            }
        }


        public static int measured_till = 0;

        delegate void SetMeasuredCountToNullDelegate();
        public void SetMeasuredCountToNull()
        {
            if (tbLemert.InvokeRequired)
            {
                tbLemert.Invoke(new SetMeasuredCountToNullDelegate(SetMeasuredCountToNull));
            }
            else
            {
                tbLemert.Text = string.Format("{0}", measured_till);
                tbLemert.Enabled = false;
            }
        }
        private void invalidate_Click(object sender, EventArgs e)
        {
            /// new ShowResult(lotid,).Show();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control c in UserWindow.userTables.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Name == "tbBarcode")
                    {
                        ((TextBox)c).Text = string.Empty;
                        ((TextBox)c).Enabled = true;

                    }
                }
            }
        }

        private void tbLemert_TextChanged(object sender, EventArgs e)
        {
            DecraseRemainingStripCount();
        }

        delegate void DecraseRemainingStripCountDelegate();
        private void DecraseRemainingStripCount()
        {
            if (tbRemaining.InvokeRequired)
            {
                tbRemaining.Invoke(new DecraseRemainingStripCountDelegate(DecraseRemainingStripCount));
            }
            else
            {

                tbRemaining.Text = Convert.ToString(Convert.ToInt32(tbRemaining.Text) - 1);
                tbRemaining.Enabled = false;
            }
        }

        //COMMENT OUT in case of kifagyás
        delegate void AddOneMeasureDelegate();
        public void AddOneMeasure()
        {

            //Commented out until progressbar is working
            //if (pb.InvokeRequired)
            //{
            //    pb.Invoke(new AddOneMeasureDelegate(AddOneMeasure));
            //}
            //else
            //    pb.Value++;
        }
        delegate void IncreaseMeasuredStripCountDelegate();
        public void IncreaseMeasuredStripCount()
        {
            if (tbLemert.InvokeRequired)
                tbLemert.Invoke(new IncreaseMeasuredStripCountDelegate(IncreaseMeasuredStripCount));
            else
            {
                tbLemert.Text = string.Format("{0}", Convert.ToInt32(tbLemert.Text) + 1);

            }
        }

    }
}

