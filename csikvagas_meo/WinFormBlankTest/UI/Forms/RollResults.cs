using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using e77.MeasureBase;
using System.Diagnostics;

namespace WinFormBlankTest.UI.Forms
{
    public partial class RollResults : Form
    {

        #region Variables
        public Dictionary<string, bool> IncludedValues = new Dictionary<string, bool>();
        public double real_avg;
        public List<double> glus = new List<double>();
        object pk_id;
        public List<bool> selected_early_dribble = new List<bool>();
        public List<bool> selected_device_replace = new List<bool>();
        public List<int> selected_codes = new List<int>();
        public List<string> selected_rolls = new List<string>();
        public List<DateTime> selected_end_dates = new List<DateTime>();
        public List<DateTime> selected_start_dates = new List<DateTime>();
        public List<string> selected_computers = new List<string>();
        public List<string> selected_users = new List<string>();
        public List<int> selected_fk_errors_id = new List<int>();
        public List<double> selected_temp = new List<double>();
        public List<double> selected_nano_ampers = new List<double>();
        public List<double> selected_glus = new List<double>();
        public List<string> selected_serial_numbers = new List<string>();
        public List<int> selected_result_ids = new List<int>();
        public List<string> selected_error = new List<string>();
        public List<string> selected_error_text = new List<string>();
        public List<bool> selected_not_h62 = new List<bool>();
        public List<bool> selected_h62 = new List<bool>();
        int altogether_lot_out_count;
        int altogether_lot_not_h62;
        int altogether_lot_h62;
        int altogether_lot_strips;
        double calc_stddev;
        double lot_avg_original;
        double lot_stddev_original;
        double lot_cv_original;

        bool lot_is_valid_original;
        bool lot_cv_valid_original;
        bool lot_avg_valid_original;

        int lot_h62_count_original;
        int lot_not_h62_count_original;
        int strip_count_original;
        bool lot_strip_count_is_valid;
        List<DateTime> lot_date = new List<DateTime>();
        List<int> original_lot_out_of_range_count = new List<int>();

        private DataGridView dataGridselected_roll = new DataGridView();
        private BindingSource bindingselected_rollSource = new BindingSource();
        private Button btOK = new Button();
        private Button btSave = new Button();
        public int i = 0;

        bool lot_strip_count_ok_original;

        object calculated_lot_avg = null;
        object calculated_lot_stddev = null;
        double calculated_lot_cv;
        bool calculated_lot_cv_ok;
        bool calculated_lot_avg_ok;
        object calculated_lot_strip_count = null;
        bool calculated_lot_strip_count_ok;
        object calculated_h62_error_count = null;
        object calculated_not_h62_error_count = null;
        object calculated_lot_out_of_range_strip_count = null;
        bool calculated_lot_out_of_range_strip_count_ok;
        bool calculated_lot_result;
        public bool lot_strip_count_ok;
        public List<object> selected_selected_roll = new List<object>();
        public List<object> skipped_selected_roll = new List<object>();
        object selected_selected_lot_id;


        public LOTResults lotResults;
        private bool H62_error_count_valid;
        private bool Not_H62_error_count_valid;
        private bool lot_is_valid;
        private object selected_roll_out_of_range_strip_count;

        #endregion
        public bool lot_avg_ok;
        public double act_lot_avg;
        delegate void SetLOTResultToSelectedDelegate();
        public void SetLOTResultToSelected()
        {
            if (lotResults.InvokeRequired)
            {
                lotResults.Invoke(new SetLOTResultToSelectedDelegate(SetLOTResultToSelected));
            }
            else
            {
                foreach (Control c in lotResults.Controls)
                {
                    if (c is DataGridView)
                    {
                        foreach (DataGridViewRow row in ((DataGridView)c).Rows)
                        {
                            act_lot_avg = Convert.ToDouble(row.Cells["LOT Átlag"].Value);

                        }

                    }
                }
            }

        }

        delegate void SaveLOTResultDependsSelectedDelegate(NpgsqlConnection conn_in);
        public void SaveLOTResultDependsSelected(NpgsqlConnection conn_in)
        {
            if (lotResults.InvokeRequired)
            {
                lotResults.Invoke(new SaveLOTResultDependsSelectedDelegate(SaveLOTResultDependsSelected), conn_in);
            }
            else
            {
                foreach (Control c in lotResults.Controls)
                {
                    if (c is DataGridView)
                    {
                        foreach (DataGridViewRow row in ((DataGridView)c).Rows)
                        {
                            act_lot_id = Convert.ToString(row.Cells["LOT ID"].Value);
                            act_lot_avg = Convert.ToDouble(row.Cells["LOT Átlag"].Value);
                             

                            act_lot_cv = Convert.ToDouble(row.Cells["LOT CV"].Value);
                            if (Convert.ToString(row.Cells["LOT Átlag Értékelés"].Value) == "Megfelelő")
                                act_lot_avg_ok = true;
                            else
                                act_lot_avg_ok = false;

                            if (Convert.ToString(row.Cells["LOT CV Értékelés"].Value) == "Megfelelő")
                                act_lot_cv_ok = true;
                            else
                                act_lot_cv_ok = false;

                            act_lot_stripcount = Convert.ToInt32(row.Cells["LOT Csíkszám"].Value);
                            if (Convert.ToString(row.Cells["LOT Csíkszám Értékelés"].Value) == "Megfelelő")
                                act_lot_stripcount_ok = true;
                            else
                                act_lot_stripcount_ok = false;

                            act_lot_outofrange_stripcount = Convert.ToInt32(row.Cells["Kieső csíkszám(glu)"].Value);
                            act_lot_h62_count = Convert.ToInt32(row.Cells["H62 hibák száma"].Value);
                            act_lot_not_h62_count = Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value);
                            if (Convert.ToString(row.Cells["LOT Értékelés"].Value) == "Megfelelő")
                                act_lot_is_valid = true;
                            else
                                act_lot_is_valid = false;

                            using (NpgsqlCommand identify_command = new NpgsqlCommand(string.Format("select max(pk_id) from lot_result where lot_id='{0}'", lotid), conn_in))
                            {                                                           //get the pk_id of blank_test_identify
                                fk_lot_result_id = Convert.ToInt32(identify_command.ExecuteScalar());

                                if (fk_lot_result_id == 0)
                                {
                                    Trace.TraceError("fk_lot_result_id is null, query: {0}", identify_command.CommandText);
                                    throw new ArgumentNullException("fk_lot_result_id is null");
                                }
                            }
                            using (NpgsqlCommand identify_command = new NpgsqlCommand(string.Format("update lot_result set modified=True where lot_id='{0}'", lotid), conn_in))
                            {
                                object result = null;
                                result = identify_command.ExecuteNonQuery();

                                if (result == null)
                                {
                                    Trace.TraceError("fk_lot_result_id is null, query: {0}", identify_command.CommandText);
                                    throw new ArgumentNullException("fk_lot_result_id is null");
                                }
                            }
                            using (NpgsqlCommand stddev_command = new NpgsqlCommand(string.Format("SELECT STDDEV(blank_test_result.glu) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where homogenity_test.lot_id='{0}' and blank_test_result.glu<>0  and blank_test_result.code=777 and homogenity_test.invalidate=false", lotid, Program.measureType), conn_in))
                            {
                                object result = null;
                                result = stddev_command.ExecuteScalar();

                                if (result == null || result == DBNull.Value)
                                {
                                    Trace.TraceError("exception while calculating modified lot's stddev, query: {0}", stddev_command.CommandText);
                                    throw new ArgumentNullException("exception while calculating modified lot's stddev");
                                }
                                else
                                    act_lot_stddev = Convert.ToDouble(result);
                            }
                            using (NpgsqlCommand insertModifiedLot = new NpgsqlCommand(string.Format("insert into lot_result_modified(avg,cv,fk_lot_result_id,avg_ok,cv_ok,not_h62_strip_errors,h62_strip_errors,lot_id,lot_strip_count,lot_is_valid,date,invalidate,out_of_range_strip_count,stddev) values({0},{1},{2},{3},{4},{5},{6},'{7}',{8},{9},{10},{11},{12},{13})", act_lot_avg, act_lot_cv, fk_lot_result_id, act_lot_avg_ok, act_lot_cv_ok, act_lot_not_h62_count, act_lot_h62_count, act_lot_id, act_lot_stripcount, act_lot_is_valid, "@date", false, act_lot_outofrange_stripcount,act_lot_stddev), conn_in))
                            {
                                insertModifiedLot.Parameters.AddWithValue("@date", DateTime.Now);
                                object res = insertModifiedLot.ExecuteNonQuery();

                                if (res == null)
                                {
                                    Trace.TraceError("Unsuccessfull insertModifiedLot at calculated_lot_result modified insertModifiedLot statement, query: {0}", insertModifiedLot.CommandText);
                                    throw new ArgumentException(string.Format("Unsuccessfull insertModifiedLot at calculated_lot_result modified insertModifiedLot statement, query: {0}", insertModifiedLot.CommandText));
                                }


                            }
                        }

                    }
                }
            }
        }

        public Dictionary<string, bool> selected_rolls_from_table = new Dictionary<string, bool>();
        public bool roll_avg_ok;
        public double act_roll_avg;
        delegate void GetRollResultToSelectedDelegate();
        public void GetRollResultToSelected()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new GetRollResultToSelectedDelegate(GetRollResultToSelected));
            }
            else
            {
                foreach (Control c in this.Controls)
                {
                    if (c is DataGridView)
                    {
                        foreach (DataGridViewRow row in ((DataGridView)c).Rows)
                        {
                            act_roll_selected = Convert.ToBoolean(row.Cells["Értékelés a LOT-nál"].Value);

                            if (!act_roll_selected)
                            {
                                selected_rolls_from_table.Add(Convert.ToString(row.Cells["Roll ID"].Value), false);
                            }
                            else
                            {
                                selected_rolls_from_table.Add(Convert.ToString(row.Cells["Roll ID"].Value), true);
                            }
                            roll_cv_from_table.Add(Convert.ToDouble(row.Cells["Roll CV"].Value));
                        }

                    }
                }
            }

        }


        /// <summary>
        /// Show selected_roll-and-Lot Results in case /showall
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="selected_roll"></param>
        /// <param name="selected_roll_averages"></param>
        /// <param name="selected_roll_averages_is_valid"></param>
        /// <param name="selected_roll_ceve"></param>
        /// <param name="selected_roll_ceve_is_valid"></param>
        /// <param name="test_date"></param>
        /// <param name="stripCount"></param>
        /// <param name="outofrange_stripcount"></param>
        /// <param name="selected_roll_is_valid"></param>
        public RollResults(string lot, string[] selected_roll,
            double[] selected_roll_averages, bool[] selected_roll_averages_is_valid,
            double[] selected_roll_ceve, bool[] selected_roll_ceve_is_valid, DateTime[] test_date,
            int[] stripCount, int[] outofrange_stripcount,
           bool[] selected_roll_is_valid)
        {

            lotid = lot;
            rollids = selected_roll.ToList();
            roll_avgs = selected_roll_averages.ToList();
            roll_avgs_ok = selected_roll_averages_is_valid.ToList();
            roll_cvs = selected_roll_ceve.ToList();
            roll_cvs_ok = selected_roll_ceve_is_valid.ToList();
            roll_date = test_date.ToList();
            roll_stripcount = stripCount.ToList();
            roll_outstripcount = outofrange_stripcount.ToList();
            rolls_ok = selected_roll_is_valid.ToList();

            InitializeComponent();


            this.Load += new System.EventHandler(EnumsAndComboBox_Load_For_All);
            this.FormClosed += new FormClosedEventHandler(RollResults_FormClosed);
            i = 0;
            foreach (string selected_roll_id in selected_roll)
            {
                bindingselected_rollSource.Add(
                    new HomogenityTest(lot, selected_roll[i], selected_roll_averages[i],
                        selected_roll_averages_is_valid[i], selected_roll_ceve[i],
                        selected_roll_ceve_is_valid[i], test_date[i],
                        stripCount[i], outofrange_stripcount[i], selected_roll_is_valid[i]));

                i++;
            }
            dataGridselected_roll.Font = new Font(dataGridselected_roll.Font, FontStyle.Bold);

            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                conn.Open();

                try
                {
                    string lotTable;
                    #region Get LOT result
                    using (NpgsqlCommand get_lot_result_pk_id = new NpgsqlCommand(string.Format("select max(pk_id) from lot_result where lot_id='{0}' and invalidate=false", lot), conn))
                    {
                        pk_id = get_lot_result_pk_id.ExecuteScalar();

                        if (pk_id == DBNull.Value)
                        {
                            throw new SqlNoValueException(string.Format("Nincs eredménye a lekérdezésnek: query: {0}", get_lot_result_pk_id.CommandText));
                        }
                    }
                    using (NpgsqlCommand get_lot_result_pk_id = new NpgsqlCommand(string.Format("select modified from lot_result where pk_id='{0}' and invalidate=false", pk_id), conn))
                    {
                       bool modified = Convert.ToBoolean(get_lot_result_pk_id.ExecuteScalar());

                       Program.IsLotModified = modified;
                       if (modified == true)
                        {
                            //if (Program.AccessRight != Program.ADMIN_ACCESSRIGHT || Program.AccessRight != Program.MEO_LEADER_ACCESSRIGHT)
                         //   {
                           //     btSave.Enabled = false;    
                           // }
                            
                            lotTable = "lot_result_modified";
                        }else
                           lotTable = "lot_result";
                    }
                    using (NpgsqlCommand get_lot_res = new NpgsqlCommand(string.Format("select * from {1} where pk_id=(select max(pk_id) from {1} where lot_id='{0}')", lotid, lotTable), conn))
                    {
                        using (NpgsqlDataReader dr = get_lot_res.ExecuteReader())
                        {

                            bool lot_cv_ok;

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    lot_cv_ok = (Convert.ToBoolean(dr["cv_ok"]));
                                    lot_avg_ok = (Convert.ToBoolean(dr["avg_ok"]));
                                    lot_avg_original = (Convert.ToDouble(dr["avg"]));
                                    lot_cv_original = (Convert.ToDouble(dr["cv"]));
                                    lot_is_valid_original = (Convert.ToBoolean(dr["lot_is_valid"]));
                                   
                                    lot_stddev_original = Convert.ToDouble(dr["stddev"]);
                                    lot_h62_count_original = (Convert.ToInt32(dr["h62_strip_errors"]));
                                    lot_not_h62_count_original = (Convert.ToInt32(dr["not_h62_strip_errors"]));
                                    lot_date.Add(Convert.ToDateTime(dr["date"]));
                                    strip_count_original = (Convert.ToInt32(dr["lot_strip_count"]));
                                    original_lot_out_of_range_count.Add(Convert.ToInt32(dr["out_of_range_strip_count"]));


                                    if ((Convert.ToInt32(dr["lot_strip_count"]) >= 244)
                                        && (Convert.ToInt32(dr["lot_strip_count"]) <= 3259))
                                    {
                                        lot_strip_count_ok_original = true;
                                    }



                                    if ((Convert.ToInt32(dr["lot_strip_count"]) >= 244)
                                        && (Convert.ToInt32(dr["lot_strip_count"])) <= 3259)
                                    {
                                        lot_strip_count_ok = true;
                                    }
                                    else
                                        lot_strip_count_ok = false;


                                    lot_cv_valid_original = lot_cv_ok;
                                    lot_strip_count_is_valid = lot_strip_count_ok;

                                }
                                dr.Close();
                            }
                            else
                            {
                                dr.Close();

                            }

                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception in selected_rollResults: exception: {0}", ex.StackTrace);

                }
                finally
                { conn.Close(); }
            }


            #region ShowResults
            lotResults = new LOTResults(lot, new double[] { lot_avg_original }, new bool[] { lot_avg_ok },
                 new double[] { lot_cv_original }, new bool[] { lot_cv_valid_original },
                lot_date.ToArray(), original_lot_out_of_range_count.ToArray(),
                new int[] { strip_count_original }, lot_strip_count_ok_original,
                new bool[] { lot_is_valid_original }, new int[] { lot_h62_count_original }, new int[] { lot_not_h62_count_original });

            lotResults.Show();
            #endregion
        }

        #region Methods
        void RollResults_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void EnumsAndComboBox_Load_For_All(object sender, System.EventArgs e)
        {

            // Initialize the DataGridView.
            dataGridselected_roll.AutoGenerateColumns = false;
            dataGridselected_roll.AutoSize = true;
            dataGridselected_roll.DataSource = bindingselected_rollSource;

            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "LotID";
            column.Name = "Lot ID";
            dataGridselected_roll.Columns.Add(column);/*1*/

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "RollID";
            column.Name = "Roll ID";
            dataGridselected_roll.Columns.Add(column);/*2*/

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Roll_Averages";
            column.Name = "Roll Átlag";
            dataGridselected_roll.Columns.Add(column);/*3*/

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Roll_Averages_IsValid";
            column.Name = "Roll Átlag Értékelés";
            column.Width = 120;
            dataGridselected_roll.Columns.Add(column);/*4*/

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Roll_CV";
            column.Name = "Roll CV";
            dataGridselected_roll.Columns.Add(column);/*5*/

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Roll_CV_IsValid";
            column.Name = "Roll CV Értékelés";
            column.Width = 120;
            dataGridselected_roll.Columns.Add(column);/*6*/

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "RollDate";
            column.Name = "Dátum";
            column.Width = 120;
            dataGridselected_roll.Columns.Add(column);/*7*/

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "StripCount";
            column.Name = "Lemért csíkszám";
            column.Width = 180;
            dataGridselected_roll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "OutOfRangeStripCount";
            column.Name = "Kieső csíkszám(glu)";
            column.Width = 180;
            dataGridselected_roll.Columns.Add(column);/*8*/

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Result";
            column.Name = "Roll Értékelés";
            column.Width = 100;
            dataGridselected_roll.Columns.Add(column);/*9*/

            column = new DataGridViewCheckBoxColumn();
            column.DataPropertyName = "Select";
            column.Name = "Értékelés a LOT-nál";

            column.Width = 160;

            dataGridselected_roll.Columns.Add(column);


            // Initialize the form.
            this.Controls.Add(dataGridselected_roll);
            this.AutoSize = true;
            this.Text = "Roll eredmények";

            
            GetIncludedValuesFromDatabase();
                   bool isChecked;
            foreach (DataGridViewRow row in dataGridselected_roll.Rows)
            {
                if (IncludedValues.TryGetValue(Convert.ToString(row.Cells["Roll ID"].Value), out isChecked))
                    row.Cells["Értékelés a LOT-nál"].Value = isChecked;
            }

            foreach (DataGridViewRow row in dataGridselected_roll.Rows)
            {
                if (Convert.ToString(row.Cells["Roll Értékelés"].Value) == "Megfelelő")
                {
                    row.Cells["Roll Értékelés"].Style.BackColor = Color.Green;
                }
                else
                    row.Cells["Roll Értékelés"].Style.BackColor = Color.Red;
            }
            SetLOTResultToSelected();
            double diff = (Convert.ToDouble(act_lot_avg) / 100) * 5;

            foreach (DataGridViewRow row in dataGridselected_roll.Rows)
            {
                row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Gray;
            }


            foreach (DataGridViewRow row in dataGridselected_roll.Rows)
            {
                while (row.Cells["Kieső csíkszám(glu)"].Style.BackColor == Color.Gray)
                {


                    if ((Convert.ToDouble(row.Cells["Roll Átlag"].Value) + diff > Convert.ToDouble(act_lot_avg))
                     && (Convert.ToDouble(row.Cells["Roll Átlag"].Value) - diff < Convert.ToDouble(act_lot_avg)))
                    {
                        row.Cells["Roll Átlag"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Roll Átlag"].Style.BackColor = Color.Red;
                    }

                    if (Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) > 49)
                    {
                        row.Cells["Lemért csíkszám"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Lemért csíkszám"].Style.BackColor = Color.Red;
                    }
                    if ((Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) > 49)
                        && (Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) < 71)
                        && row.Cells["Lemért csíkszám"].Style.BackColor != Color.Red
                        && (Convert.ToInt32(row.Cells["Kieső csíkszám(glu)"].Value) <= 1)
                        && true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Green;
                    }
                    else if (true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Red;
                    }
                    if ((Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) > 72)
                        && (Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) < 94)
                        && (Convert.ToInt32(row.Cells["Kieső csíkszám(glu)"].Value) <= 2)
                        && true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Green;
                    }
                    else if (true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Red;
                    }
                    else if ((Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) > 95)
                        && (Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) < 116)
                        && row.Cells["Lemért csíkszám"].Style.BackColor != Color.Red
                        && (Convert.ToInt32(row.Cells["Kieső csíkszám(glu)"].Value) <= 3)
                        && true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Green;
                    }
                    else if (true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Red;
                    }
                    else if ((Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) > 117)
                       && (Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) < 139)
                       && row.Cells["Lemért csíkszám"].Style.BackColor != Color.Red
                       && (Convert.ToInt32(row.Cells["Kieső csíkszám(glu)"].Value) <= 4)
                        && true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Green;
                    }
                    else if (true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Red;
                    }
                    else if ((Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) > 140)
                        && (Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) < 161)
                        && row.Cells["Lemért csíkszám"].Style.BackColor != Color.Red
                        && (Convert.ToInt32(row.Cells["Kieső csíkszám(glu)"].Value) <= 5)
                        && true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Green;
                    }
                    else if (true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Red;
                    }
                    else if ((Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) > 162)
                        && (Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) < 183)
                        && row.Cells["Lemért csíkszám"].Style.BackColor != Color.Red
                        && (Convert.ToInt32(row.Cells["Kieső csíkszám(glu)"].Value) <= 6)
                        && true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Green;
                    }
                    else if (true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Red;
                    }
                    else if ((Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) > 184)
                       && (Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) < 204)
                       && row.Cells["Lemért csíkszám"].Style.BackColor != Color.Red
                       && (Convert.ToInt32(row.Cells["Kieső csíkszám(glu)"].Value) <= 7)
                        && true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Green;
                    }
                    else if (true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Red;
                    }
                    else if ((Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) > 205)
                        && (Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) < 226)
                        && row.Cells["Lemért csíkszám"].Style.BackColor != Color.Red
                        && (Convert.ToInt32(row.Cells["Kieső csíkszám(glu)"].Value) <= 8)
                        && true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Green;
                    }
                    else if (true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Red;
                    }
                    else if ((Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) > 227)
                       && (Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) < 248)
                       && row.Cells["Lemért csíkszám"].Style.BackColor != Color.Red
                       && (Convert.ToInt32(row.Cells["Kieső csíkszám(glu)"].Value) <= 9)
                        && true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Green;
                    }
                    else if (true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Red;
                    }
                    else if ((Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) > 249)
                        && (Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) < 269)
                        && row.Cells["Lemért csíkszám"].Style.BackColor != Color.Red
                        && (Convert.ToInt32(row.Cells["Kieső csíkszám(glu)"].Value) <= 10)
                        && true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Green;
                    }
                    else if (true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Red;
                    }
                    else if ((Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) > 270)
                        && (Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) < 291)
                        && row.Cells["Lemért csíkszám"].Style.BackColor != Color.Red
                        && (Convert.ToInt32(row.Cells["Kieső csíkszám(glu)"].Value) <= 11)
                        && true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Green;
                    }
                    else if (true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Red;
                    }
                    else if ((Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) > 292)
                        && (Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) < 312)
                        && row.Cells["Lemért csíkszám"].Style.BackColor != Color.Red
                        && (Convert.ToInt32(row.Cells["Kieső csíkszám(glu)"].Value) <= 12)
                        && true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Green;
                    }
                    else if (true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Red;
                    }
                    else if ((Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) > 313)
                        && (Convert.ToInt32(row.Cells["Lemért csíkszám"].Value) < 333)
                        && row.Cells["Lemért csíkszám"].Style.BackColor != Color.Red
                        && (Convert.ToInt32(row.Cells["Kieső csíkszám(glu)"].Value) <= 13)
                        && true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Green;
                    }
                    else if (true)
                    {
                        row.Cells["Kieső csíkszám(glu)"].Style.BackColor = Color.Red;
                    }

                }
            }




            foreach (DataGridViewRow row in dataGridselected_roll.Rows)
                if (Convert.ToString(row.Cells["Roll Átlag Értékelés"].Value) == "Megfelelő")
                {
                    row.Cells["Roll Átlag Értékelés"].Style.BackColor = Color.Green;
                    row.Cells["Roll Átlag"].Style.BackColor = Color.Green;
                }
                else
                {
                    row.Cells["Roll Átlag Értékelés"].Style.BackColor = Color.Red;
                    row.Cells["Roll Átlag"].Style.BackColor = Color.Red;
                }
            foreach (DataGridViewRow row in dataGridselected_roll.Rows)
                if (Convert.ToDouble(row.Cells["Roll CV"].Value) < 4.3)
                {
                    row.Cells["Roll CV"].Style.BackColor = Color.Green;
                }
                else
                    row.Cells["Roll CV"].Style.BackColor = Color.Red;

            foreach (DataGridViewRow row in dataGridselected_roll.Rows)
            {
                if (Convert.ToString(row.Cells["Roll CV Értékelés"].Value) == "Megfelelő")
                {
                    row.Cells["Roll CV Értékelés"].Style.BackColor = Color.Green;
                }
                else
                    row.Cells["Roll CV Értékelés"].Style.BackColor = Color.Red;

            }
            SetButtonCalc();


        }

        private void GetIncludedValuesFromDatabase()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {

                try
                {
                    conn.Open();

                    using (NpgsqlCommand getIncludedValues = new NpgsqlCommand(string.Format("select roll_id,included from homogenity_result where lot_id='{0}'", lotid), conn))
                    {
                        using (NpgsqlDataReader dr = getIncludedValues.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    IncludedValues.Add(Convert.ToString(dr["roll_id"]), Convert.ToBoolean(dr["included"]));
                                }

                            }
                            dr.Close();
                        }

                    }

                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception while getting included column value, ex:{0}", ex);
                    throw new Exception(string.Format("Exception while getting included column value, ex:{0}", ex));
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public void SetButtonCalc()
        {
            btOK.Location = new Point(dataGridselected_roll.Width / 2, dataGridselected_roll.Height + 200);
            btOK.Name = "button1";
            btOK.Size = new System.Drawing.Size(80, 23);
            btOK.TabIndex = 2;
            btOK.Text = "Újraszámolás";
            btOK.UseVisualStyleBackColor = true;
            btOK.Click += new System.EventHandler(this.button1_Click);
            this.Controls.Add(btOK);

            btSave.Location = new Point(dataGridselected_roll.Width / 2 + 100, dataGridselected_roll.Height + 200 + 35);
            btSave.Name = "button1";
            btSave.Size = new System.Drawing.Size(80, 23);
            btSave.TabIndex = 2;
            btSave.Text = "Mentés";
            btSave.UseVisualStyleBackColor = true;
            btSave.Click += new System.EventHandler(this.buttonSave_Click);
          
        //    if ((Program.AccessRight == Program.ADMIN_ACCESSRIGHT || Program.AccessRight == Program.MEO_LEADER_ACCESSRIGHT) || !Properties.Settings.Default.AccestRightsInUse)
          //  {
                this.Controls.Add(btSave);
            //}
          
        }
        public Button btExit;
        public void SetButtonExit()
        {
            btExit.Location = new Point(dataGridselected_roll.Width / 2 - 100, dataGridselected_roll.Height + 200);
            btExit.Name = "button1";
            btExit.Size = new System.Drawing.Size(80, 23);
            btExit.TabIndex = 2;
            btExit.Text = "Program bezárása";
            btExit.UseVisualStyleBackColor = true;
            btExit.ForeColor = Color.Red;
            btExit.Click += new System.EventHandler(this.button2_Click);
            this.Controls.Add(btExit);
        }

        /// <summary>
        /// Click on button save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            GetRollResultToSelected();
            UpdateUnSelectedRolls();

            MessageBox.Show("Lot értékek sikeresen módosítva az adatbázisban");
            //btSave.Enabled = false;
        }

        public void UpdateUnSelectedRolls()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();

                    foreach (string selected in selected_rolls_from_table.Keys)
                    {
                        Included = selected_rolls_from_table[selected];
                        rollids.Add(selected);


                        if (!Included)
                        {


                            using (NpgsqlCommand updateHomogenity = new NpgsqlCommand(string.Format("update homogenity_result set included=false where roll_id='{0}' and lot_id='{1}'", selected, lotid), conn))
                            {

                                object result = updateHomogenity.ExecuteNonQuery();

                                if (Convert.ToInt32(result) == 0)
                                {
                                    Trace.TraceError("Unsuccessfull update in UpdateUnSelectedRolls(), query: {0}", updateHomogenity.CommandText);
                                }
                                else
                                    Trace.TraceInformation("Successfull update in UpdateUnSelectedRolls(), query: {0}", updateHomogenity.CommandText);

                            }



                        }
                    }///end of foreach                             


                    SaveLOTResultDependsSelected(conn);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                { conn.Close(); }

            }


        }







        /// <summary>
        /// Click on button calculates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //Get Selected rows selected_rollID
            GetSelectedselected_rollID();

            //set Lot values to 0 if no selected_roll selected
            if (selected_selected_roll.Count <= 0)
            {
                SetLotValuesToNull();
                selected_selected_roll = new List<object>();
                selected_selected_lot_id = null;
            }
            else
            {
                if (selected_selected_roll.Count >= 1)
                {
                    Trace.TraceInformation("Selected roll count:{0}", selected_selected_roll.Count);
                    Calc_Lot_values(selected_selected_lot_id, selected_selected_roll);

                    Trace.TraceInformation("Calculated lot values: lot_id:{0} lot_avg:{1}/lot_avg_ok:{2} lot_cv:{3}/lot_cv_is_ok:{4} lot_strip_count{5}/lot_strip_count_ok:{6} lot_out_of_range_strip_count:{7}/lot_h62_strip_count:{8}/lot_not_h62_strip_count:{9}", selected_selected_lot_id, calculated_lot_avg, calculated_lot_avg_ok, calculated_lot_cv, calculated_lot_cv_ok, altogether_lot_strips, calculated_lot_strip_count_ok, calculated_lot_out_of_range_strip_count, altogether_lot_h62, altogether_lot_not_h62);
                    SetCalculatedValuesForLot(Convert.ToString(selected_selected_lot_id), Convert.ToDouble(real_avg), calculated_lot_avg_ok,
                        Convert.ToDouble(calculated_lot_cv), calculated_lot_cv_ok,
                        Convert.ToInt32(altogether_lot_strips), calculated_lot_strip_count_ok,
                        Convert.ToInt32(calculated_lot_out_of_range_strip_count),
                        Convert.ToInt32(altogether_lot_h62), Convert.ToInt32(altogether_lot_not_h62),
                        Convert.ToString(calculated_lot_result));

                    selected_selected_roll = new List<object>();
                    selected_selected_lot_id = null;
                }
            }
        }

        #endregion
        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        #region Delegates

        delegate void SetCalculatedValuesForLotDelegate();
        private void SetCalculatedValuesForLot(string selected_lot_id, double lot_avg, bool lot_avg_ok,
            double lot_cv, bool lot_cv_ok, int lot_strip_count, bool lot_strip_count_ok,
            int lot_out_of_range_strip_count,
            int h62_error_count, int not_h62_error_count, string lot_result)
        {
            if (lotResults.InvokeRequired)
            {
                lotResults.Invoke(new SetLOTResultToSelectedDelegate(GetSelectedselected_rollID));

            }
            else
            {

                lotResults.Hide();
                if (lot_result == "Megfelelő")
                {
                    lotisok = true;
                }
                else
                    lotisok = false;
                if ((calculated_lot_cv_ok)
                        && (calculated_lot_avg_ok)
                        && (calculated_lot_strip_count_ok))
                {
                    calculated_lot_result = true;
                }
                else
                    calculated_lot_result = false;
                lotResults = new LOTResults(selected_lot_id, new double[] { Math.Round(real_avg, 2) }, new bool[] { calculated_lot_avg_ok }, new double[] { Math.Round(calculated_lot_cv, 2) }, new bool[] { calculated_lot_cv_ok }, new DateTime[] { calculated_date }, new int[] { altogether_lot_out_count }, new int[] { altogether_lot_strips }, calculated_lot_strip_count_ok, new bool[] { calculated_lot_result }, new int[] { altogether_lot_h62 }, new int[] { altogether_lot_not_h62 });
                lotResults.Show();
                foreach (Control dgv in lotResults.Controls)
                {
                    if (dgv is DataGridView)
                    {
                        foreach (DataGridViewRow row in ((DataGridView)dgv).Rows)
                        {
                            if (Convert.ToString(row.Cells["LOT Értékelés"].Value) == "Megfelelő")
                            {
                                row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                            }
                            else
                                row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;

                            if ((Convert.ToDouble(row.Cells["LOT Átlag"].Value) > 6.17)
                                && (Convert.ToDouble(row.Cells["LOT Átlag"].Value) < 6.81))
                            {
                                row.Cells["LOT Átlag"].Style.BackColor = Color.Green;
                            }
                            else
                                row.Cells["LOT Átlag"].Style.BackColor = Color.Red;


                            if (Convert.ToString(row.Cells["LOT Átlag Értékelés"].Value) == "Megfelelő")
                            {
                                row.Cells["LOT Átlag Értékelés"].Style.BackColor = Color.Green;
                            }
                            else
                                row.Cells["LOT Átlag Értékelés"].Style.BackColor = Color.Red;


                            if (Convert.ToDouble(row.Cells["LOT CV"].Value) < 4.3)
                            {
                                row.Cells["LOT CV"].Style.BackColor = Color.Green;
                                row.Cells["LOT CV Értékelés"].Style.BackColor = Color.Green;

                            }
                            else
                            {
                                row.Cells["LOT CV"].Style.BackColor = Color.Red;
                                row.Cells["LOT CV Értékelés"].Style.BackColor = Color.Red;

                            }



                            if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) < 244)
                                  || (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) > 3259))
                            {
                                row.Cells["LOT Csíkszám"].Style.BackColor = Color.Red;
                                //   row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                                //   row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                                row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;
                                row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
                                row.Cells["LOT Értékelés"].Value = "Nem Megfelelő";
                            }
                            else
                            {
                                row.Cells["LOT Csíkszám"].Style.BackColor = Color.Green;
                                row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Green;
                            }
                            if (Convert.ToString(row.Cells["LOT Csíkszám Értékelés"].Value) == "Megfelelő")
                            {
                                row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Green;
                            }
                            else
                                row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Red;


                            #region get Lot level error counting



                            if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 244)
                                       && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 360))
                            {
                                row.Cells["LOT Csíkszám Értékelés"].Style.BackColor = Color.Green;

                                if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 1
                                    && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 1)
                                {
                                    row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Green;
                                    row.Cells["H62 hibák száma"].Style.BackColor = Color.Green;
                                    row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;

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

                                if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"]) <= 6
                                    && Convert.ToInt32(row.Cells["H62 hibák száma"]) <= 17)
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
                                    row.Cells["Nem H62 hibák száma"].Style.BackColor = Color.Red;
                                    row.Cells["H62 hibák száma"].Style.BackColor = Color.Red;
                                    row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;
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
                            if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2313)
                             && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 2418))
                            {
                                row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                                if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"]) <= 20
                                    && Convert.ToInt32(row.Cells["H62 hibák száma"]) <= 56)
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
                            if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2419)
                             && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 2523))
                            {
                                row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                                if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 21
                                    && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 59)
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
                            if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2524)
                             && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 2629))
                            {
                                row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                                if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 22
                                    && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 61)
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
                                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2630)
                                 && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 2734))
                                {
                                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 23
                                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 64)
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
                                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2735)
                                 && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 2839))
                                {
                                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 24
                                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 67)
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
                                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2840)
                                 && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 2944))
                                {
                                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 25
                                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 70)
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
                                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 2945)
                                 && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 3049))
                                {
                                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"]) <= 26
                                        && Convert.ToInt32(row.Cells["H62 hibák száma"]) <= 72)
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
                                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 3050)
                                 && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 3154))
                                {
                                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 27
                                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 75)
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
                                if ((Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) >= 3155)
                                 && (Convert.ToInt32(row.Cells["LOT Csíkszám"].Value) <= 3259))
                                {
                                    row.Cells["LOT Csíkszám Értékelés"].Value = Color.Green;

                                    if (Convert.ToInt32(row.Cells["Nem H62 hibák száma"].Value) <= 28
                                        && Convert.ToInt32(row.Cells["H62 hibák száma"].Value) <= 78)
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


                            }//end of if
                            DataGridViewTextBoxCell tbEditControl = new DataGridViewTextBoxCell();
                            if (Convert.ToString(row.Cells["LOT Értékelés"].Value) == "Megfelelő")
                            {
                                row.Cells["LOT Értékelés"].Style.BackColor = Color.Green;
                            }
                            else
                                row.Cells["LOT Értékelés"].Style.BackColor = Color.Red;


                            lot_glus = new List<double>();
                            real_avg = 0;
                            calculated_lot_avg = 0;
                            calculated_lot_avg_ok = new bool();
                            calculated_lot_cv = new double();
                            calculated_lot_cv_ok = new bool();

                            altogether_lot_h62 = new int();
                            altogether_lot_not_h62 = new int();
                            altogether_lot_out_count = 0;
                            altogether_lot_strips = 0;

                            calculated_lot_out_of_range_strip_count = new object();
                            calculated_lot_result = new bool();
                            calculated_lot_out_of_range_strip_count = new int();
                            calculated_lot_out_of_range_strip_count_ok = new bool();

                            dgv.Refresh();
                            dgv.Update();
                        }
                    }

                }//end of foreach
                SetLOTResultToSelected();
                double diff = (Convert.ToDouble(act_lot_avg) / 100) * 5;
                foreach (DataGridViewRow row in dataGridselected_roll.Rows)
                {
                    if ((Convert.ToDouble(row.Cells["Roll Átlag"].Value) + diff > Convert.ToDouble(act_lot_avg))
                     && (Convert.ToDouble(row.Cells["Roll Átlag"].Value) - diff < Convert.ToDouble(act_lot_avg)))
                    {
                        row.Cells["Roll Átlag"].Style.BackColor = Color.Green;
                        row.Cells["Roll Átlag Értékelés"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Roll Átlag"].Style.BackColor = Color.Red;
                        row.Cells["Roll Átlag Értékelés"].Style.BackColor = Color.Red;
                    }
                }
                i = 0;
                foreach (DataGridViewRow row in dataGridselected_roll.Rows)
                {
                    if (row.Cells["Roll Átlag Értékelés"].Style.BackColor == Color.Green)
                    {
                        row.Cells["Roll Átlag Értékelés"].Value = "Megfelelő";
                        roll_avgs_ok[i] = true;
                    }
                    else
                    {
                        row.Cells["Roll Átlag Értékelés"].Value = "Nem Megfelelő";
                        roll_avgs_ok[i] = false;
                    }
                    i++;
                }
                i = 0;
                foreach (DataGridViewRow row in dataGridselected_roll.Rows)
                {
                    if (row.Cells["Roll CV Értékelés"].Style.BackColor == Color.Green)
                    {
                        row.Cells["Roll CV Értékelés"].Value = "Megfelelő";
                        roll_cvs_ok[i] = true;
                    }
                    else
                    {
                        row.Cells["Roll CV Értékelés"].Value = "Nem Megfelelő";
                        roll_cvs_ok[i] = false;
                    }
                    i++;
                }


            }
        }

        delegate void SetLotValuesToNullDelegate();
        private void SetLotValuesToNull()
        {
            if (lotResults.InvokeRequired)
            {
                lotResults.Invoke(new SetLOTResultToSelectedDelegate(GetSelectedselected_rollID));

            }
            else
            {
                foreach (Control dgv in lotResults.Controls)
                {
                    if (dgv is DataGridView)
                    {
                        foreach (DataGridViewRow row in ((DataGridView)dgv).Rows)
                        {
                            row.Cells["Lot ID"].Value = "----";
                            row.Cells["Dátum"].Value = DateTime.MinValue;
                            row.Cells["LOT Átlag"].Value = 0;
                            row.Cells["LOT Átlag Értékelés"].Value = false;
                            row.Cells["LOT CV"].Value = 0;
                            row.Cells["LOT CV Értékelés"].Value = false;
                            row.Cells["LOT Csíkszám"].Value = 0;
                            row.Cells["LOT Csíkszám Értékelés"].Value = false;
                            row.Cells["Kieső csíkszám(glu)"].Value = 0;
                            row.Cells["H62 hibák száma"].Value = 0;
                            row.Cells["Nem H62 hibák száma"].Value = 0;
                            row.Cells["LOT Értékelés"].Value = "----";

                            row.DefaultCellStyle.BackColor = Color.White;
                        }
                    }
                }
            }
        }

        delegate void GetSelectedselected_rollIDDelegate();
        private void GetSelectedselected_rollID()
        {
            if (lotResults.InvokeRequired)
            {
                lotResults.Invoke(new SetLOTResultToSelectedDelegate(GetSelectedselected_rollID));
            }
            else
            {
                foreach (DataGridViewRow row in dataGridselected_roll.Rows)
                {
                    DataGridViewCheckBoxCell cell = row.Cells["Értékelés a LOT-nál"] as DataGridViewCheckBoxCell;
                    selected_selected_lot_id = row.Cells["Lot ID"].Value;
                    if (Convert.ToBoolean(cell.Value) == true)
                    {

                        selected_selected_roll.Add(row.Cells["Roll ID"].Value);
                    }
                    else
                        skipped_selected_roll.Add(row.Cells["Roll ID"].Value);
                }

            }

        }

                            #endregion

        #region Calculate Lot Values

        object selected_result = null;
        object selected_lot_id = null;
        private object lot_valid_strip_count;
        private object lot_valid_strip_count2;
        public double sd_lot;
        /// <summary>
        /// Calculate the lot values depending on which roll is selected
        /// </summary>
        /// <param name="selected_selected_lot_id"></param>
        public void Calc_Lot_values(object selected_selected_lot_id, List<object> selected_rolls)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    connection.Open();
                    ///get selected rolls values from a lot
                    altogether_lot_strips = 0;
                    altogether_lot_out_count = 0;
                    altogether_lot_not_h62 = 0;
                    altogether_lot_h62 = 0;
                    foreach (string selected_roll_id in selected_rolls)
                    {

                        #region Calculate values for selected selected_rolls

                        using (NpgsqlCommand getGlus = new NpgsqlCommand(
                            string.Format("SELECT blank_test_result.glu,blank_test_result.roll_id,blank_test_result.code,blank_test_result.lot_id,blank_test_result.invalidate,homogenity_test.strip_ok,homogenity_test.invalidate,homogenity_test.pk_id,homogenity_test.sn,blank_test_errors.error_text FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_result.code=777 and blank_test_result.roll_id='{1}' and blank_test_result.lot_id='{0}' and blank_test_result.glu<>0 and homogenity_test.invalidate=False ", selected_selected_lot_id, selected_roll_id), connection))
                        {//Select STDDEV(blank_test_result.glu) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id where blank_test_identify.roll_id='{0}' and blank_test_identify.lot_id='{1}' and blank_test_result.glu<>0 and temperature_ok=true and blank_test_identify.measure_type='{2}' and blank_test_identify.invalidate=false", Users_Form.dev.Roll, Users_Form.dev.LOT_ID, Program.measureType), lot_connection))

                            using (NpgsqlDataReader dr = getGlus.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        lot_glus.Add(Convert.ToDouble(dr["glu"]));
                                        Trace.TraceInformation("Selected lot glus: {0}", Convert.ToDouble(dr["glu"]));
                                    }
                                    dr.Close();
                                }
                                else
                                {
                                    dr.Close();
                                    throw new SqlNoValueException(string.Format("Nincs megfelelő eredmény a következőre:{0}", getGlus.CommandText));
                                }

                            }
                        }
                        #endregion

                        using (NpgsqlCommand getsd = new NpgsqlCommand(
                           string.Format("SELECT STDDEV(blank_test_result.glu)  FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_result.code=777 and blank_test_result.roll_id='{1}' and blank_test_result.lot_id='{0}' and blank_test_result.glu<>0 and homogenity_test.invalidate=False ", selected_selected_lot_id, selected_roll_id), connection))
                        {
                            object res = null;
                            res = getsd.ExecuteScalar();
                            sd_lot = Convert.ToDouble(res);
                            Trace.TraceInformation("sd{0}", res);
                        }

                        #region Get out of range strip count in lot
                        using (NpgsqlCommand getOUT = new NpgsqlCommand(
                                       string.Format("SELECT COUNT(homogenity_test.strip_ok) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id where homogenity_test.invalidate=False and blank_test_result.glu<>0 and blank_test_result.invalidate=False and homogenity_test.strip_ok=False and homogenity_test.lot_id='{0}' and blank_test_result.roll_id='{1}'", selected_selected_lot_id, selected_roll_id), connection))
                        {
                            object out_count = null;
                            out_count = getOUT.ExecuteScalar();

                            if ((out_count == DBNull.Value)
                                || (out_count == null))
                            {
                                Trace.TraceError("out_count is null, query: {0}", getOUT.CommandText);
                                throw new ArgumentNullException("out_count is null");
                            }
                            altogether_lot_out_count += Convert.ToInt32(out_count);
                            Trace.TraceInformation("Out count:{0} /altogether_lot_out_count:{1}", out_count, altogether_lot_out_count);
                        }
                        calculated_lot_out_of_range_strip_count = altogether_lot_out_count;
                        Trace.TraceInformation("Calculated lot out of range strip count:{0}", calculated_lot_out_of_range_strip_count);
                        #endregion

                        #region Get strip count
                        /*             strip_count_01 = string.Format("SELECT COUNT(homogenity_test.pk_id) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE (blank_test_errors.error='' and blank_test_result.glu<>0) and homogenity_test.lot_id='{0}'  and blank_test_result.code=777 and homogenity_test.roll_id='{2}' and homogenity_test.invalidate=false ", Program.LOT_ID, Program.measureType, Users_Form.dev.Roll);
                            strip_count_02 = string.Format("SELECT COUNT(homogenity_test.pk_id) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE blank_test_errors.error_text<>'' and homogenity_test.lot_id='{0}' and blank_test_result.code=777 and homogenity_test.roll_id='{2}' and homogenity_test.invalidate=false ", Program.LOT_ID, Program.measureType, Users_Form.dev.Roll); */
                        string strip_count_01 =
                            string.Format("SELECT COUNT(homogenity_test.pk_id) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE (blank_test_errors.error='' and blank_test_result.glu<>0) and homogenity_test.lot_id='{0}'  and homogenity_test.roll_id='{1}'  and blank_test_result.code=777 and homogenity_test.roll_id='{1}' and homogenity_test.invalidate=false", selected_selected_lot_id, selected_roll_id);

                        string strip_count_02 =
                            string.Format("SELECT COUNT(homogenity_test.pk_id) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE blank_test_errors.error_text<>'' and homogenity_test.lot_id='{0}' and homogenity_test.roll_id='{1}'  and blank_test_result.code=777 and homogenity_test.roll_id='{1}' and homogenity_test.invalidate=false ", selected_selected_lot_id, selected_roll_id);

                        using (NpgsqlCommand stripCount1 = new NpgsqlCommand(strip_count_01, connection))      //get valid strip count in two step)
                        {
                            lot_valid_strip_count = stripCount1.ExecuteScalar();
                        }
                        using (NpgsqlCommand stripCount2 = new NpgsqlCommand(strip_count_02, connection))
                        {
                            lot_valid_strip_count2 = stripCount2.ExecuteScalar();
                        }
                        calculated_lot_strip_count = Convert.ToInt32(lot_valid_strip_count) + Convert.ToInt32(lot_valid_strip_count2);
                        altogether_lot_strips += Convert.ToInt32(calculated_lot_strip_count);
                        Trace.TraceInformation("Calcualted lot strip count:{0}/Alltogether lot_strips:{1}", calculated_lot_strip_count, altogether_lot_strips);
                        #endregion

                        #region Get Error Count In selected selected_rolls

                        using (NpgsqlCommand error_count = new NpgsqlCommand(
                            string.Format("SELECT count(blank_test_errors.not_h62_error) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_identify.measure_type='homogenity' and blank_test_result.lot_id='{0}'  and blank_test_errors.lot_id='{0}' and blank_test_identify.lot_id='{0}' and blank_test_identify.roll_id='{1}'  and blank_test_errors.not_h62_error=True and blank_test_identify.invalidate=false and blank_test_result.invalidate=false", selected_selected_lot_id, selected_roll_id), connection))
                        {
                            calculated_not_h62_error_count = error_count.ExecuteScalar();

                            if ((calculated_not_h62_error_count == null)
                                || (calculated_not_h62_error_count == DBNull.Value))
                            {
                                Trace.TraceError("error_count is null, query: {0}", error_count.CommandText);
                                throw new ArgumentNullException("error_count is null");
                            }
                            altogether_lot_not_h62 += Convert.ToInt32(calculated_not_h62_error_count);
                        }
                        using (NpgsqlCommand error_count = new NpgsqlCommand(
                            string.Format("SELECT count(blank_test_errors.h62_error) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_identify.measure_type='homogenity' and blank_test_result.lot_id='{0}' and blank_test_identify.lot_id='{0}'  and blank_test_identify.roll_id='{1}'  and blank_test_errors.h62_error=True and blank_test_identify.invalidate=false and blank_test_result.invalidate=false", selected_selected_lot_id, selected_roll_id), connection))
                        {
                            calculated_h62_error_count = error_count.ExecuteScalar();

                            if ((calculated_h62_error_count == null)
                                || (calculated_h62_error_count == DBNull.Value))
                            {
                                Trace.TraceError("error_count is null, query: {0}", error_count.CommandText);
                                throw new ArgumentNullException("error_count is null");
                            }
                            altogether_lot_h62 += Convert.ToInt32(calculated_h62_error_count);
                        }
                        #endregion
                    }//End of foreach

                    ///Calculate selected roll's averages As lot averages
                    real_avg = lot_glus.Average();
                    ////calc lot sd
                    double avg = real_avg;
                    double sd = Math.Sqrt(lot_glus.Average(v => Math.Pow(v - real_avg, 2)));
                    double real_stddev = Math.Round(Math.Sqrt(lot_glus.Average(v => Math.Pow(v - real_avg, 2))), 2);
                    Trace.TraceInformation("{0}", real_stddev);
                    ////calc lot cv
                    calculated_lot_cv = (Convert.ToDouble(real_stddev) / Convert.ToDouble(real_avg)) * 100;

        #endregion

                    #region Get stripcount, avg,cv if valid

                    #region get out of range result
                    if ((Convert.ToInt32(calculated_lot_out_of_range_strip_count) >= 1)
                       && (altogether_lot_strips >= 49)
                       && (altogether_lot_strips <= 71))
                    {
                        calculated_lot_out_of_range_strip_count_ok = true;
                    }
                    if ((Convert.ToInt32(calculated_lot_out_of_range_strip_count) >= 2)
                        && (altogether_lot_strips >= 72)
                        && (altogether_lot_strips <= 94))
                    {
                        calculated_lot_out_of_range_strip_count_ok = true;
                    }
                    if ((Convert.ToInt32(calculated_lot_out_of_range_strip_count) >= 3)
                       && (altogether_lot_strips >= 95)
                       && (altogether_lot_strips <= 116))
                    {
                        calculated_lot_out_of_range_strip_count_ok = true;
                    }
                    if ((Convert.ToInt32(calculated_lot_out_of_range_strip_count) >= 4)
                       && (altogether_lot_strips >= 117)
                       && (altogether_lot_strips <= 139))
                    {
                        calculated_lot_out_of_range_strip_count_ok = true;
                    }
                    if ((Convert.ToInt32(calculated_lot_out_of_range_strip_count) >= 5)
                        && (altogether_lot_strips >= 140)
                        && (altogether_lot_strips <= 161))
                    {
                        calculated_lot_out_of_range_strip_count_ok = true;
                    }
                    if ((Convert.ToInt32(calculated_lot_out_of_range_strip_count) >= 6)
                        && (altogether_lot_strips >= 162)
                        && (altogether_lot_strips <= 183))
                    {
                        calculated_lot_out_of_range_strip_count_ok = true;
                    }
                    if ((Convert.ToInt32(calculated_lot_out_of_range_strip_count) >= 7)
                        && (altogether_lot_strips >= 184)
                        && (altogether_lot_strips <= 204))
                    {
                        calculated_lot_out_of_range_strip_count_ok = true;
                    }
                    if ((Convert.ToInt32(calculated_lot_out_of_range_strip_count) >= 8)
                        && (altogether_lot_strips >= 205)
                        && (altogether_lot_strips <= 226))
                    {
                        calculated_lot_out_of_range_strip_count_ok = true;
                    }
                    if ((Convert.ToInt32(calculated_lot_out_of_range_strip_count) >= 9)
                        && (altogether_lot_strips >= 227)
                        && (altogether_lot_strips <= 248))
                    {
                        calculated_lot_out_of_range_strip_count_ok = true;
                    }
                    if ((Convert.ToInt32(calculated_lot_out_of_range_strip_count) >= 10)
                        && (altogether_lot_strips >= 249)
                        && (altogether_lot_strips <= 269))
                    {
                        calculated_lot_out_of_range_strip_count_ok = true;
                    }
                    if ((Convert.ToInt32(calculated_lot_out_of_range_strip_count) >= 11)
                        && (altogether_lot_strips >= 270)
                        && (altogether_lot_strips <= 291))
                    {
                        calculated_lot_out_of_range_strip_count_ok = true;
                    }
                    if ((Convert.ToInt32(calculated_lot_out_of_range_strip_count) >= 12)
                        && (altogether_lot_strips >= 292)
                        && (altogether_lot_strips <= 312))
                    {
                        calculated_lot_out_of_range_strip_count_ok = true;
                    }
                    if ((Convert.ToInt32(calculated_lot_out_of_range_strip_count) >= 13)
                        && (altogether_lot_strips >= 313)
                        && (altogether_lot_strips <= 333))
                    {
                        calculated_lot_out_of_range_strip_count_ok = true;
                    }
                    #endregion

                    if ((Convert.ToInt32(altogether_lot_strips) < 244)
                        || (Convert.ToInt32(altogether_lot_strips) > 3259))
                    {
                        calculated_lot_strip_count_ok = false;

                    }
                    else
                        calculated_lot_strip_count_ok = true;

                    if ((Convert.ToDouble(real_avg) > 6.17)
                         && (Convert.ToDouble(real_avg) < 6.81))
                    {
                        calculated_lot_avg_ok = true;
                    }
                    else
                        calculated_lot_avg_ok = false;
                    #endregion


                    if (calculated_lot_cv < 4.3)
                    {
                        calculated_lot_cv_ok = true;
                    }
                    else
                        calculated_lot_cv_ok = false;

                    if ((calculated_lot_cv_ok)
                        && (calculated_lot_avg_ok)
                        && (calculated_lot_out_of_range_strip_count_ok)
                        && (calculated_lot_strip_count_ok))
                    {
                        calculated_lot_result = true;
                    }
                    else
                        calculated_lot_result = false;

                    using (NpgsqlCommand getdate = new NpgsqlCommand(string.Format("Select max(date) from lot_result where lot_id='{0}'", selected_selected_lot_id), connection))
                    {
                        object res = null;
                        res = getdate.ExecuteScalar();

                        calculated_date = Convert.ToDateTime(res);

                    }
                    #region Get Max Pk_Id from homogenity result
                    using (NpgsqlCommand fk_homo = new NpgsqlCommand(string.Format("select max(pk_id) from homogenity_result;"), connection))
                    {
                        max_pk_homores = fk_homo.ExecuteScalar();

                        if (max_pk_homores == DBNull.Value
                            || max_pk_homores == null)
                        {
                            Trace.TraceError("no max pkid in homogenity result");
                            throw new ArgumentNullException("No value for max pk_id in homogenity result..");
                        }
                    }
                    #endregion

                    lot_glus = new List<double>();
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception at CalcAVGforLot(): \nStackTrace:{0}\nInnerException:{1}\nMessage:{2}", ex.StackTrace, ex.InnerException, ex.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            ///roll cv calculation
            //calculated_roll_cv = (Convert.ToDouble(sd_roll) / Convert.ToDouble(calced_roll_avg)) * 100;

            ///Store calculated values to db
            //ShowLotResult(calculated_lot_avg, calc_stddev, Convert.ToInt32(max_pk_homores), calced_roll_avg, calculated_roll_cv, sd_roll);
        }
        public object max_pk_homores = null;
        public List<double> lot_glus = new List<double>();
        public double stddev;




        /// <summary>
        /// Store calculated values to db
        /// </summary>
        /// <param name="lot_avg"></param>
        /// <param name="lot_stddev"></param>
        /// <param name="homogenityPKey"></param>
        /// <param name="selected_roll_avg"></param>
        /// <param name="selected_roll_cv"></param>
        /// <param name="selected_roll_stddev"></param>   
        public void ShowLotResult(object lot_avg, object lot_stddev, int homogenityPKey, object selected_roll_avg, object selected_roll_cv, object selected_roll_stddev)
        {

            object lot_valid_strip_count;
            object lot_valid_strip_count2;
            object res = null;

            List<double> glus = new List<double>();
            List<string> selected_rolls = new List<string>();
            StringBuilder queryFilter = new StringBuilder();
            bool avg_ok;
            bool cv_ok;
            object fk_lot_res_id;
            object not_h62_error_count_in_lot;
            object h62_error_count_in_lot;
            object lot_ok_strip_count;
            object lot_ok_strip_count2;

            int tha_strip_count_in_a_lot;
            bool selected_roll_is_valid;


            Trace.TraceInformation("ShowLotResult() method started in RollResult");
            using (NpgsqlConnection lot_connection = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    lot_connection.Open();

                    #region get LOT average
                    using (NpgsqlCommand command =
                        new NpgsqlCommand(
                        string.Format("SELECT AVG(blank_test_result.glu) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_identify.lot_id='{0}' and blank_test_identify.invalidate=False and blank_test_identify.remeasured=False and blank_test_result.remeasured=False and blank_test_result.lot_id='{0}' and blank_test_result.invalidate=False and blank_test_errors.remeasured=False and blank_test_errors.invalidate=False and blank_test_result.lot_id='{0}' and blank_test_result.invalidate=False and blank_test_result.remeasured=False and blank_test_result.invalidate=False and blank_test_errors.remeasured=False and blank_test_errors.invalidate=False and blank_test_result.lot_id='{0}' and blank_test_result.invalidate=False and blank_test_errors.remeasured=False and blank_test_identify.lot_id='{0}' and blank_test_identify.remeasured=False and blank_test_identify.invalidate=False", Convert.ToString(selected_selected_lot_id)), lot_connection))
                    {
                        calculated_lot_avg = null;
                        calculated_lot_avg = command.ExecuteScalar();

                        if ((calculated_lot_avg == DBNull.Value)
                            || (calculated_lot_avg == null)
                            || (Convert.ToInt32(calculated_lot_avg) == 0))
                        {
                            Trace.TraceError(string.Format("Avg is null, query:{0}", command.CommandText));
                            throw new ArgumentNullException("Avg is null");
                        }
                        Trace.TraceInformation("Lot AVG is {0}", calculated_lot_avg);
                    #endregion



                        #region get LOT STDDEV
                        using (NpgsqlCommand stddev_comm = new NpgsqlCommand(
                            string.Format("SELECT STDDEV(blank_test_result.glu) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_identify.lot_id='{0}' and blank_test_identify.invalidate=False and blank_test_identify.remeasured=False and blank_test_result.remeasured=False and blank_test_result.lot_id='{0}' and blank_test_result.invalidate=False and blank_test_errors.remeasured=False and blank_test_errors.invalidate=False and blank_test_result.lot_id='{0}' and blank_test_result.invalidate=False and blank_test_result.remeasured=False and blank_test_result.invalidate=False and blank_test_errors.remeasured=False and blank_test_errors.invalidate=False and blank_test_result.lot_id='{0}' and blank_test_result.invalidate=False and blank_test_errors.remeasured=False and blank_test_identify.lot_id='{0}' and blank_test_identify.remeasured=False and blank_test_identify.invalidate=False", selected_selected_lot_id), lot_connection))
                        {

                            calculated_lot_stddev = null;
                            calculated_lot_stddev = stddev_comm.ExecuteScalar();

                            if ((calculated_lot_stddev == DBNull.Value)
                                || (calculated_lot_stddev == null))
                            {
                                Trace.TraceError("stddev is null, query: {0}", stddev_comm.CommandText);
                                throw new SqlNoValueException(string.Format("STDDEV query for lot is null or has no value, query:{0}", stddev_comm.CommandText));
                            }
                        }
                        Trace.TraceInformation("Lot STDDEV is {0}", calculated_lot_stddev);
                        #endregion


                        ///calc lot cv
                        calculated_lot_cv = (Convert.ToDouble(calculated_lot_stddev) / Convert.ToDouble(calculated_lot_avg)) * 100;
                        Trace.TraceInformation("Lot CV is {0}", calculated_lot_cv);


                        Trace.TraceInformation("Calculating Lot_avg_is_valid and Lot_cv_is_valid");
                        if ((Convert.ToDouble(calculated_lot_avg) > 6.17)
                            && (Convert.ToDouble(calculated_lot_avg) < 6.81))
                        {
                            avg_ok = true;
                        }
                        else
                            avg_ok = false;

                        if (calculated_lot_cv < 4.3)
                        {
                            cv_ok = true;
                        }
                        else
                            cv_ok = false;



                        #region Get strip count

                        string strip_count_01 = string.Format("SELECT COUNT(blank_test_identify.pk_id) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE (blank_test_errors.error_text='' and blank_test_result.glu<>0) and blank_test_result.lot_id='{0}' and blank_test_identify.lot_id='{0}'  and blank_test_identify.measure_type='homogenity' and blank_test_identify.invalidate=false and blank_test_result.invalidate=false", selected_selected_lot_id);
                        string strip_count_02 = string.Format("SELECT COUNT(blank_test_identify.pk_id) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE blank_test_errors.error_text<>''  and blank_test_result.lot_id='{0}' and blank_test_identify.lot_id='{0}'  and blank_test_identify.measure_type='homogenity' and blank_test_identify.invalidate=false and blank_test_result.invalidate=false", selected_selected_lot_id);

                        using (NpgsqlCommand stripCount1 = new NpgsqlCommand(strip_count_01, lot_connection))      //get valid strip count in two step)
                        {
                            lot_valid_strip_count = stripCount1.ExecuteScalar();
                        }
                        using (NpgsqlCommand stripCount2 = new NpgsqlCommand(strip_count_02, lot_connection))
                        {
                            lot_valid_strip_count2 = stripCount2.ExecuteScalar();
                        }
                        int lot_strip_count = Convert.ToInt32(lot_valid_strip_count) + Convert.ToInt32(lot_valid_strip_count2);

                        #endregion

                        #region get Lot error count

                        using (NpgsqlCommand error_count = new NpgsqlCommand(
                            string.Format("SELECT count(blank_test_errors.not_h62_error) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id  where blank_test_identify.measure_type='homogenity' and blank_test_errors.h62_error=True and blank_test_identify.lot_id='{0}' and blank_test_identify.invalidate=False and blank_test_identify.remeasured=False and blank_test_result.remeasured=False and blank_test_result.lot_id='{0}' and blank_test_result.invalidate=False and blank_test_errors.remeasured=False and blank_test_errors.invalidate=False", selected_selected_lot_id), lot_connection))
                        {
                            not_h62_error_count_in_lot = error_count.ExecuteScalar();

                            if ((not_h62_error_count_in_lot == null)
                                || (not_h62_error_count_in_lot == DBNull.Value))
                            {
                                Trace.TraceError("error_count is null, query: {0}", error_count.CommandText);
                                throw new ArgumentNullException("error_count is null");
                            }
                        }
                        Trace.TraceInformation("Lot level error count");
                        using (NpgsqlCommand error_count = new NpgsqlCommand(
                            string.Format("SELECT count(blank_test_errors.h62_error) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_identify.measure_type='homogenity' and blank_test_errors.not_h62_error=True and blank_test_identify.lot_id='{0}' and blank_test_identify.invalidate=False and blank_test_identify.remeasured=False and blank_test_result.remeasured=False and blank_test_result.lot_id='{0}' and blank_test_result.invalidate=False and blank_test_errors.remeasured=False and blank_test_errors.invalidate=False", selected_selected_lot_id), lot_connection))
                        {
                            h62_error_count_in_lot = error_count.ExecuteScalar();

                            if ((h62_error_count_in_lot == null)
                                || (h62_error_count_in_lot == DBNull.Value))
                            {
                                Trace.TraceError("error_count is null, query: {0}", error_count.CommandText);
                                throw new ArgumentNullException("error_count is null");
                            }
                        }
                        Trace.TraceInformation("H62 error count in LOT:{0}", h62_error_count_in_lot);

                        #endregion

                        #region get lot strip count

                        Trace.TraceInformation("Calculating Lot strip count");
                        string lot_strip_count_01 = string.Format("SELECT COUNT(blank_test_identify.pk_id) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE (blank_test_errors.error_text='' and blank_test_result.glu<>0) and blank_test_identify.selected_lot_id='{0}' and blank_test_result.lot_id='{0}' and blank_test_identify.measure_type='homogenity' and blank_test_identify.invalidate=false and blank_test_identify.remeasured=false and blank_test_result.invalidate=false and blank_test_result.remeasured=false", selected_selected_lot_id);
                        string lot_strip_count_02 = string.Format("SELECT COUNT(blank_test_identify.pk_id) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE blank_test_errors.error_text<>'' and blank_test_identify.lot_id='{0}'  and blank_test_result.lot_id='{0}'  and blank_test_identify.measure_type='homogenity' and blank_test_identify.invalidate=false and blank_test_identify.remeasured=false  and blank_test_result.invalidate=false and blank_test_result.remeasured=false", selected_selected_lot_id);

                        using (NpgsqlCommand stripCount1 = new NpgsqlCommand(strip_count_01, lot_connection))      //get valid strip count in two step)
                        {
                            lot_valid_strip_count = stripCount1.ExecuteScalar();
                        }
                        using (NpgsqlCommand stripCount2 = new NpgsqlCommand(strip_count_02, lot_connection))
                        {
                            lot_valid_strip_count2 = stripCount2.ExecuteScalar();
                        }
                        tha_strip_count_in_a_lot = Convert.ToInt32(lot_valid_strip_count) + Convert.ToInt32(lot_valid_strip_count2);
                        Trace.TraceInformation("Strip count in a LOT:{0}", tha_strip_count_in_a_lot);

                        #endregion
                        Trace.TraceInformation("Calculating Lot out of range strip count");
                        using (NpgsqlCommand getOUT =
                            new NpgsqlCommand(
                            string.Format("SELECT count(homogenity_test.strip_ok) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where homogenity_test.strip_ok=False and  blank_test_result.invalidate=False and blank_test_result.lot_id='{0}' and homogenity_test.lot_id='{0}' and homogenity_test.invalidate=False and blank_test_errors.invalidate=False and blank_test_errors.remeasured=False ", selected_selected_lot_id), lot_connection))
                        {

                            object out_count_in_lot = null;
                            out_count_in_lot = getOUT.ExecuteScalar();

                            if ((out_count_in_lot == DBNull.Value)
                                || (out_count_in_lot == null))
                            {
                                Trace.TraceError("out_count is null, query: {0}", getOUT.CommandText);
                                throw new ArgumentNullException("out_count is null");
                            }
                            altogether_lot_out_count += Convert.ToInt32(out_count_in_lot);
                        }
                        Trace.TraceInformation("Out of range strip count in a LOT:{0}", altogether_lot_out_count);


                        #region get Lot level error counting


                        if ((tha_strip_count_in_a_lot < 244)
                            || (tha_strip_count_in_a_lot > 3259))
                        {
                            H62_error_count_valid = false;
                            Not_H62_error_count_valid = false;
                            lot_strip_count_ok = false;
                            lot_is_valid = false;
                            Trace.TraceError("Impossible stripCount:{0}", tha_strip_count_in_a_lot);
                        }
                        else if ((tha_strip_count_in_a_lot >= 244)
                                   && (tha_strip_count_in_a_lot <= 360))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 1
                                && Convert.ToInt32(h62_error_count_in_lot) <= 1)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;
                                lot_is_valid = true;
                            }
                            else
                            {
                                lot_strip_count_ok = false;
                                lot_is_valid = false;
                            }
                        }
                        if ((tha_strip_count_in_a_lot >= 361)
                            && (tha_strip_count_in_a_lot <= 475))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 2
                                && Convert.ToInt32(h62_error_count_in_lot) <= 6)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                            {
                                lot_strip_count_ok = false;
                                lot_is_valid = false;
                            }
                        }
                        if ((tha_strip_count_in_a_lot >= 476)
                           && (tha_strip_count_in_a_lot <= 588))
                        {

                            lot_strip_count_ok = true;
                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 3
                                && Convert.ToInt32(h62_error_count_in_lot) <= 9)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 589)
                           && (tha_strip_count_in_a_lot <= 699))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 4
                                && Convert.ToInt32(h62_error_count_in_lot) <= 11)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 700)
                           && (tha_strip_count_in_a_lot <= 810))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 5
                                && Convert.ToInt32(h62_error_count_in_lot) <= 14)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 811)
                          && (tha_strip_count_in_a_lot <= 919))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 6
                                && Convert.ToInt32(h62_error_count_in_lot) <= 17)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 920)
                          && (tha_strip_count_in_a_lot <= 1028))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 7
                                && Convert.ToInt32(h62_error_count_in_lot) <= 20)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 1029)
                          && (tha_strip_count_in_a_lot <= 1137))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 8
                                && Convert.ToInt32(h62_error_count_in_lot) <= 23)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 1138)
                          && (tha_strip_count_in_a_lot <= 1245))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 9
                                && Convert.ToInt32(h62_error_count_in_lot) <= 25)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 1246)
                          && (tha_strip_count_in_a_lot <= 1353))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 10
                                && Convert.ToInt32(h62_error_count_in_lot) <= 28)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 1354)
                          && (tha_strip_count_in_a_lot <= 1461))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 11
                                && Convert.ToInt32(h62_error_count_in_lot) <= 31)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 1462)
                          && (tha_strip_count_in_a_lot <= 1568))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 12
                                && Convert.ToInt32(h62_error_count_in_lot) <= 34)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 1569)
                          && (tha_strip_count_in_a_lot <= 1675))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 13
                                && Convert.ToInt32(h62_error_count_in_lot) <= 36)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 1676)
                          && (tha_strip_count_in_a_lot <= 1781))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 14
                                && Convert.ToInt32(h62_error_count_in_lot) <= 42)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 1782)
                          && (tha_strip_count_in_a_lot <= 1888))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 15
                                && Convert.ToInt32(h62_error_count_in_lot) <= 42)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 1889)
                         && (tha_strip_count_in_a_lot <= 1994))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 16
                                && Convert.ToInt32(h62_error_count_in_lot) <= 45)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 1995)
                         && (tha_strip_count_in_a_lot <= 2100))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 17
                                && Convert.ToInt32(h62_error_count_in_lot) <= 47)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;
                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 2101)
                         && (tha_strip_count_in_a_lot <= 2206))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 18
                                && Convert.ToInt32(h62_error_count_in_lot) <= 50)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;
                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 2207)
                         && (tha_strip_count_in_a_lot <= 2312))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 19
                                && Convert.ToInt32(h62_error_count_in_lot) <= 53)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;
                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 2313)
                         && (tha_strip_count_in_a_lot <= 2418))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 20
                                && Convert.ToInt32(h62_error_count_in_lot) <= 56)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;
                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 2419)
                         && (tha_strip_count_in_a_lot <= 2523))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 21
                                && Convert.ToInt32(h62_error_count_in_lot) <= 59)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;
                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 2524)
                         && (tha_strip_count_in_a_lot <= 2629))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 22
                                && Convert.ToInt32(h62_error_count_in_lot) <= 61)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;
                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 2630)
                         && (tha_strip_count_in_a_lot <= 2734))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 23
                                && Convert.ToInt32(h62_error_count_in_lot) <= 64)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;
                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 2735)
                         && (tha_strip_count_in_a_lot <= 2839))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 24
                                && Convert.ToInt32(h62_error_count_in_lot) <= 67)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;
                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 2840)
                         && (tha_strip_count_in_a_lot <= 2944))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 25
                                && Convert.ToInt32(h62_error_count_in_lot) <= 70)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;
                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 2945)
                         && (tha_strip_count_in_a_lot <= 3049))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 26
                                && Convert.ToInt32(h62_error_count_in_lot) <= 72)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 3050)
                         && (tha_strip_count_in_a_lot <= 3154))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 27
                                && Convert.ToInt32(h62_error_count_in_lot) <= 75)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }
                        if ((tha_strip_count_in_a_lot >= 3155)
                         && (tha_strip_count_in_a_lot <= 3259))
                        {
                            lot_strip_count_ok = true;

                            if (Convert.ToInt32(not_h62_error_count_in_lot) <= 28
                                && Convert.ToInt32(h62_error_count_in_lot) <= 78)
                            {
                                H62_error_count_valid = true;
                                Not_H62_error_count_valid = true;

                                lot_is_valid = true;
                            }
                            else
                                lot_is_valid = false;
                        }


                    }
                }
                        #endregion
                catch (Exception ex)
                {
                    Trace.TraceError("Exception in SaveLotResult, ex: InnerException:{0}\nStackTrace:{1}", ex.InnerException, ex.StackTrace);
                }
                finally
                {
                    lot_connection.Close();
                }

            }

        }//end of lot result storing to db

        /// <summary>
        /// Calc roll avg
        /// </summary>
        public double calced_roll_avg;
        private DateTime calculated_date;
        private bool lotisok;
        private string lotid;
        private List<string> rollids = new List<string>();
        private List<double> roll_avgs = new List<double>();
        private List<bool> roll_avgs_ok = new List<bool>();
        private List<double> roll_cvs = new List<double>();
        private List<DateTime> roll_date = new List<DateTime>();
        private List<bool> roll_cvs_ok = new List<bool>();
        private List<bool> rolls_ok = new List<bool>();
        private List<int> roll_outstripcount = new List<int>();
        private List<int> roll_stripcount = new List<int>();
        private Color clr;
        private object fk_homo_res_id;
        private bool act_roll_selected;
        private List<double> roll_cv_from_table = new List<double>();
        private bool act_lot_is_valid;
        private int act_lot_not_h62_count;
        private double act_lot_cv;
        private int act_lot_outofrange_stripcount;
        private int act_lot_h62_count;
        private int act_lot_stripcount;
        private bool act_lot_cv_ok;
        private double act_lot_stddev;
        private DateTime act_lot_date;
        private string act_lot_id;
        private bool act_lot_stripcount_ok;
        private bool act_lot_avg_ok;
        private int fk_lot_result_id;





        public int StripCount { get; set; }

        public bool Not_h62_is_valid { get; set; }

        public bool Homogenity_Is_Valid { get; set; }

        public bool Out_of_range_valid { get; set; }

        public bool OutAndStripCountAndError_ok { get; set; }

        public int Averages_ID { get; set; }

        public bool Included { get; set; }
    }

    enum column
    {
    }
}
        #endregion