using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WinFormBlankTest.UI.Forms
{

    /// <summary>
    /// A form to warn users,operators to remeasure and invalidate a measured roll is very dangerous!!
    /// </summary>
    public partial class Dialoge : Form
    {
        public ProgressBar pb = new ProgressBar();
        public string lot;
        public string roll;
        public string measure;

        /// <summary>
        /// Remeasure Warning
        /// </summary>
        /// <param name="lotid"></param>
        /// <param name="rollid"></param>
        /// <param name="measureType"></param>
        public Dialoge(string lotid,string rollid, string measureType)
        {
            lot = lotid;
            roll = rollid;
            measure = measureType;

            InitializeComponent();
            this.label1.Text = string.Format("Biztos benne, hogy az '{0}' számú Lot-ból a '{1}' Roll-t minden {2} mérés eredményét eldobja?!",lot,roll,measure);

        }

        /// <summary>
        /// In case of invalidation
        /// </summary>
        /// <param name="lotid"></param>
        /// <param name="rollid"></param>
        /// <param name="measureType"></param>
        public Dialoge(string lotid, string rollid, string measureType,bool invalidate)
        {
            lot = lotid;
            roll = rollid;
            measure = measureType;

            InitializeComponent();
            this.label1.Text = string.Format("Biztos benne, hogy az '{0}' számú Lot-ból a '{1}' Roll-t minden {2} mérés eredményét invalidálja?!", lot, roll, measure);

           
        }
      

                

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            object res = null;

            res = MessageBox.Show(string.Format("Biztosan érvénytelenné teszi a {0} Lot,{0} Roll összes eredményét?", lot, roll), "Figyelmeztetés!", MessageBoxButtons.YesNo);
            if (DialogResult.No == (DialogResult)res)
            {
                new Thread(FormClosed).Start();

                SetProgressBar();
                this.Controls.Add(pb);

                Thread proggress = new Thread(new ThreadStart(AddValueToProgressBar));
                proggress.Start();

                

               
               
            }
            else
            {
                SetProgressBar();
                this.Controls.Add(pb);

                Thread proggress = new Thread(new ThreadStart(AddValueToProgressBar));
                proggress.Start();

                Thread.Sleep(1000);


                this.label1.Image.Dispose();
                this.label1.Font = new Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Point);
                this.label1.Text =
                    string.Format("A mért adatok kiszűrése folyamatban, néhány pillanat múlva elkezdődik a {0} méréstípus a {1} számú Lot, {2} azonosítójú tekercséből.", measure, lot, roll);

                new Thread(FormClosed).Start();


            }
        }

        public void FormClosed()
        {
            Thread.Sleep(2000);
            label1.Text = "Előkészített eredmények zárolása..";
            Thread.Sleep(2000);
            label1.Text = "Nem történt érvénytelenítés egyetlen eredményen sem.";
            Thread.Sleep(3000);

            this.Dispose();
            this.Close();
        }    

        public void SetProgressBar()
        {
            pb.Maximum = 100;
            pb.Minimum = 0;
            pb.Location = new Point(this.Width / 2, this.Height / 2);
            pb.Style = ProgressBarStyle.Blocks;
            pb.Size = PreferredSize;
            pb.ForeColor = Color.LawnGreen;
            pb.Dock = DockStyle.Top;
        }
        public void AddValueToProgressBar()
        {

            while (pb.Value != pb.Maximum)
            {
                pb.Value += 11;
                Thread.Sleep(700);


                if (pb.Value >= 30)
                {
                    this.label1.Text = string.Format("A {0} számú Roll {1} mérésének előkészítése...",roll,measure);
                }
                if (pb.Value >= 50)
                {
                    this.label1.Text = string.Format("Mérés előkészítve, kérem vegyen elő a {0} számú Lot-ból, {1} azonosítójú tekercset, a {2} méréshez!",lot,roll,measure);
                }

            }

            this.Dispose();
            this.Close();
            
        }
        
    }
}
