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
    public partial class LOTResults : Form
    {
        private DataGridView dataGridView1 = new DataGridView();
        private DataGridView dataGridRoll = new DataGridView();
        private DataGridView dataGridLOT = new DataGridView();

        private BindingSource bindingSource1 = new BindingSource();
        private BindingSource bindingRollSource = new BindingSource();
        private Button btOK = new Button();
        public int i = 0;
        public static Color cellColor;

        /// <summary>
        /// In case of /showall created from calculated rolresults-form-dgv these only lot's data
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="lot_averages"></param>
        /// <param name="lot_averages_is_valid"></param>
        /// <param name="lot_ceve"></param>
        /// <param name="lot_ceve_is_valid"></param>
        /// <param name="test_date"></param>
        /// <param name="stripCount"></param>
        /// <param name="stripCount_is_valid"></param>
        /// <param name="lot_is_valid"></param>
        /// <param name="h62_error_count"></param>
        /// <param name="not_h62_error_count"></param>
        public LOTResults(string lot, double[] lot_averages, bool[] lot_averages_is_valid,
            double[] lot_ceve, bool[] lot_ceve_is_valid,
             DateTime[] test_date, int[] out_of_range_strips,
            int[] stripCount, bool stripCount_is_valid,
            bool[] lot_is_valid,
            int[] h62_error_count, int[] not_h62_error_count)
        {
            InitializeComponent();

            i = 0;

            bindingSource1.Add(
                new HomogenityTest(lot, lot_averages[i], lot_averages_is_valid[i],
            lot_ceve[i], lot_ceve_is_valid[i],
            test_date[i], out_of_range_strips[i],
            stripCount[i], stripCount_is_valid,
            lot_is_valid[i],
            h62_error_count[i], not_h62_error_count[i]));

            dataGridView1.Font = new Font(dataGridView1.Font, FontStyle.Bold);

            this.Load += new System.EventHandler(EnumsAndComboBox_Load_For_All);
            this.FormClosed += new FormClosedEventHandler(LOTResults_FormClosed);
        }

        void LOTResults_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void EnumsAndComboBox_Load_For_All(object sender, System.EventArgs e)
        {

            // Initialize the DataGridView.
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSize = true;
            dataGridView1.DataSource = bindingSource1;

            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "LotID";
            column.Name = "Lot ID";
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Date";
            column.Name = "Dátum";
            column.Width = 120;
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Lot_Averages";
            column.Name = "LOT Átlag";
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Lot_Averages_IsValid";
            column.Name = "LOT Átlag Értékelés";
            column.Width = 150;
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Lot_CV";
            column.Name = "LOT CV";
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Lot_CV_IsValid";
            column.Name = "LOT CV Értékelés";
            column.Width = 150;
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "LotStripCount";
            column.Name = "LOT Csíkszám";
            column.Width = 200;
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "LotStripCountIsValid";
            column.Name = "LOT Csíkszám Értékelés";

            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "LotOutOfRangeStripCount";
            column.Name = "Kieső csíkszám(glu)";
            column.Width = 140;
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Lot_h62_Count";
            column.Name = "H62 hibák száma";
            column.Width = 140;
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Lot_not_h62_Count";
            column.Name = "Nem H62 hibák száma";
            column.Width = 140;
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Lot_Result_is_Valid";
            column.Name = "LOT Értékelés";
            column.Width = 140;
            dataGridView1.Columns.Add(column);

            // Initialize the form.
            this.Controls.Add(dataGridView1);
            this.AutoSize = true;
            this.Text = "LOT eredmények";

            foreach (DataGridViewRow row in dataGridView1.Rows)
                if (Convert.ToString(row.Cells["LOT Értékelés"].Value) == "Megfelelő")
                {
                    row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                }
                else
                    row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;

            foreach (DataGridViewRow row in dataGridView1.Rows)
                if ((Convert.ToDouble(row.Cells["LOT Átlag"].Value) > 6.17)
                    && (Convert.ToDouble(row.Cells["LOT Átlag"].Value) < 6.81))
                {
                    row.Cells["LOT Átlag"].Style.BackColor = Color.Green;
                }
                else
                    row.Cells["LOT Átlag"].Style.BackColor = Color.Red;

            foreach (DataGridViewRow row in dataGridView1.Rows)
                if (Convert.ToString(row.Cells["LOT Átlag Értékelés"].Value) == "Megfelelő")
                {
                    row.Cells["LOT Átlag Értékelés"].Style.BackColor = Color.Green;
                }
                else
                    row.Cells["LOT Átlag Értékelés"].Style.BackColor = Color.Red;

            foreach (DataGridViewRow row in dataGridView1.Rows)
                if (Convert.ToDouble(row.Cells["LOT CV"].Value) < 4.3)
                {
                    row.Cells["LOT CV"].Style.BackColor = Color.Green;
                }
                else
                    row.Cells["LOT CV"].Style.BackColor = Color.Red;

            foreach (DataGridViewRow row in dataGridView1.Rows)
                if (Convert.ToString(row.Cells["LOT CV Értékelés"].Value) == "Megfelelő")
                {
                    row.Cells["LOT CV Értékelés"].Style.BackColor = Color.Green;
                }
                else
                    row.Cells["LOT CV Értékelés"].Style.BackColor = Color.Red;

            foreach (DataGridViewRow row in dataGridView1.Rows)
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) < 244)
                      || (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) > 3259))
                {
                    row.Cells["LOT Csíkszám"].Style.BackColor = Color.Red;
                }
                else
                    row.Cells["LOT Csíkszám"].Style.BackColor = Color.Green;

            foreach (DataGridViewRow row in dataGridView1.Rows)
                if (Convert.ToString(row.Cells["LOT Csíkszám Értékelés"].Value) == "Megfelelő")
                {
                    row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Green;
                }
                else
                    row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;


            #region get Lot level error counting
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {


                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 244)
                           && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 360))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 1
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 1)
                    {
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;

                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 361)
                    && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 475))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Green;


                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 2
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 6)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 476)
                   && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 588))
                {

                    row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 3
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 9)
                    {
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Green;
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 589)
                   && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 699))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 4
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 11)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 700)
                   && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 810))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 5
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 14)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 811)
                  && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 919))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 6
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 17)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 920)
                  && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 1028))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 7
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 20)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 1029)
                  && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 1137))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 8
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 23)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 1138)
                  && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 1245))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 9
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 25)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 1246)
                  && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 1353))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 10
                      && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 28)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 1354)
                  && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 1461))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 11
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 31)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 1462)
                  && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 1568))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 12
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 34)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 1569)
                  && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 1675))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 13
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 36)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 1676)
                  && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 1781))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 14
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 42)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 1782)
                  && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 1888))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"]) <= 15
                        && Convert.ToInt32(row.Cells["H62 hibák száma"]) <= 42)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 1889)
                 && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 1994))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 16
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 45)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 1995)
                 && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 2100))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 17
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 47)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2101)
                 && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 2206))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 18
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 50)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;

                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2207)
                 && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 2312))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"]) <= 19
                        && Convert.ToInt32(row.Cells["H62 hibák száma"]) <= 53)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Green);
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Red);
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2313)
                 && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 2418))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"]) <= 20
                        && Convert.ToInt32(row.Cells["H62 hibák száma"]) <= 56)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Green);
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Red);
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2419)
                 && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 2523))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 21
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 59)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Green);
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Red);
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2524)
                 && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 2629))
                {
                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 22
                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 61)
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                        Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Green);
                    }
                    else
                    {
                        row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                        row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                        Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Red);
                        row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                    }
                    if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2630)
                     && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 2734))
                    {
                        row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                        if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 23
                            && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 64)
                        {
                            row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                            row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                            Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Green);
                        }
                        else
                        {
                            row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                            row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                            Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Red);
                            row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                        }
                    }
                    if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2735)
                     && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 2839))
                    {
                        row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                        if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 24
                            && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 67)
                        {
                            row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                            row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                            Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Green);
                        }
                        else
                        {
                            row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                            row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                            Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Red);
                            row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                        }
                    }
                    if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2840)
                     && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 2944))
                    {
                        row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                        if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 25
                            && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 70)
                        {
                            row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                            row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                            Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Green);
                        }
                        else
                        {
                            row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                            row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                            Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Red);
                            row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                        }
                    }
                    if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2945)
                     && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 3049))
                    {
                        row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                        if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"]) <= 26
                            && Convert.ToInt32(row.Cells["H62 hibák száma"]) <= 72)
                        {
                            row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                            row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                            Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Green);
                        }
                        else
                        {
                            row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                            row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                            Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Red);
                            row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                        }
                    }
                    if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 3050)
                     && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 3154))
                    {
                        row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                        if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 27
                            && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 75)
                        {
                            row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                            row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                            Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Green);
                        }
                        else
                        {
                            row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                            row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                            Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Red);
                            row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                        }
                    }
                    if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 3155)
                     && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 3259))
                    {
                        row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                        if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 28
                            && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 78)
                        {
                            row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                            row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;

                            Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Green);
                        }
                        else
                        {
                            row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                            row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                            Convert.ToString(row.Cells["LOT Értékelés"].Style.BackColor = Color.Red);
                            row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                        }
                    }
            #endregion
                }



            }
        }
    }
}
