using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormBlankTest.UI.Forms
{
    public partial class CalculatedColumnValuesForm : Form
    {

        public string measure;

        public CalculatedColumnValuesForm(
            double avg,double sd,double cv,
            double int_avg,int summary_ints,int accepted_count,int not_accepted_count,
            int not_h62,int h62,
           TimeSpan tsp,string col_name)
        {
           
            InitializeComponent();
            #region in case of double alues  were in the column
            colname.Text = col_name;
            colavg.Text = avg.ToString();
            colstddev.Text = sd.ToString();
            colCV.Text = cv.ToString();
            #endregion

            #region in case of int type cells were in the column
            colavg.Text = int_avg.ToString();
            colsum.Text = summary_ints.ToString();
            #endregion

            #region in case boolean types
            acceptcols.Text = accepted_count.ToString();
            not_acceptcol.Text = not_accepted_count.ToString();
            #endregion

            timeSpendedCol.Text = tsp.ToString();



        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            this.Visible = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;

        }

        public new void Close()
        {
            base.Close();
            this.Dispose();

        }

        
    }
}
