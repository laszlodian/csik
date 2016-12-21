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
    public partial class BlankResult : Form
    {
       
        private DataGridView dataGridView1 = new DataGridView();
        private DataGridView dataGridRoll = new DataGridView();
        private DataGridView dataGridLOT = new DataGridView();

        private BindingSource bindingSource1 = new BindingSource();
        private BindingSource bindingRollSource = new BindingSource();
        private Button btOK = new Button();
        public int i = 0;
        public string res;

        public BlankResult(string lot,string roll,string meas_type,double[] glu,double[] nano_amp,int[] sn,string[] bar,bool[] remeas)
        {
            InitializeComponent();
            
            int i = 0;

            foreach (double glucose in glu)
            {
                bindingRollSource.Add(
                    new BlankTest(lot, roll, meas_type, glu[i], nano_amp[i], sn[i], bar[i], remeas[i]));
                i++;
            }
                




        }


        public BlankResult(string lot,string[] roll, double[] blank_averages, bool[] blank_is_valid,
            double[] blank_ceve, 
             DateTime[] test_date,
            int[]tube_count,
            double[] blank_stddev)
         {
             InitializeComponent();
            
            int i=0;

             foreach (string act_roll in roll)
             {
                 if (blank_is_valid[i])
	            {
		            res="Megfelelő";
	             }else
                     res="Nem megfelelő";

                 bindingSource1.Add(
                     new BlankTest(lot, act_roll,
                         Convert.ToDouble(blank_averages[i]), Convert.ToDouble(blank_ceve[i]),
                        Convert.ToDateTime(test_date[i]), Convert.ToInt32(tube_count[i]),
                        Convert.ToBoolean(blank_is_valid[i]),res));
                 i++;
             }    
             

            dataGridView1.Font = new Font(dataGridView1.Font, FontStyle.Bold);            

            this.Load += new System.EventHandler(EnumsAndComboBox_Load_For_All);
        }

        private void EnumsAndComboBox_Load_For_All(object sender, System.EventArgs e)
        {

            // Initialize the DataGridView.
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSize = true;
            dataGridView1.DataSource = bindingSource1;

            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "LotID";
            column.Name = "Lot azonosító";
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "RollID";
            column.Name = "Roll azonosító";
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "BlankDate";
            column.Name = "Dátum";
            column.Width = 120;
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "BlankAVG";
            column.Name = "Átlag";            
            dataGridView1.Columns.Add(column);

            
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "BlankCV";
            column.Name = "CV";            
            dataGridView1.Columns.Add(column);
                        

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "BlankTubeCount";
            column.Name = "Tubus szám";            
            dataGridView1.Columns.Add(column); 

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Result";
            column.Name = "Értékelés";
            column.Width = 140;
            dataGridView1.Columns.Add(column);
           
            // Initialize the form.
            this.Controls.Add(dataGridView1);
            this.AutoSize = true;
            this.Text = "Blank teszt eredmények";

            foreach (DataGridViewRow row in dataGridView1.Rows)
                if (Convert.ToString(row.Cells["Értékelés"].Value) == "Megfelelő")
                {
                    row.DefaultCellStyle.BackColor = Color.Green;
                }
                else
                    row.DefaultCellStyle.BackColor = Color.Red;
           

        }
    }
}
