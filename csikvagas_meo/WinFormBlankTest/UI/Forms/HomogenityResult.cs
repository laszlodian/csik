using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using e77.MeasureBase;
using Npgsql;

namespace WinFormBlankTest.UI.Forms
{
    public partial class HomogenityResult : Form
    {
        private DataGridView dataGridView1 = new DataGridView();
        private DataGridView dataGridRoll = new DataGridView();
        private DataGridView dataGridLOT = new DataGridView();

        private BindingSource bindingSource1 = new BindingSource();
        private BindingSource bindingRollSource = new BindingSource();
        private Button btOK = new Button();
        public int i = 0;
        public string res;
        public Color color;
        public Color homogenity_color;
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


        public List<string> values = new List<string>();

        public List<Type> types = new List<Type>(); 

        public HomogenityResult(string lot, string[] roll, double[] averages, bool[] is_valid,
            double[] ceve,
             DateTime[] test_date,
            int[] h62,int[] not_h62,
            int[] out_of_range_strip_count,int[] strip_c,
            int[] tube_count,
            double[] stddev)
        {           
             InitializeComponent();            
            int i=0;
             foreach (string act_roll in roll)
             {
                 if (is_valid[i])
	            {
		            res="Megfelelő";
	             }else
                     res="Nem megfelelő";
                 try
                 {
                     bindingSource1.Add(
                         new HomogenityTest(lot,res,roll[i],averages[i],ceve[i],
                            test_date[i], Convert.ToInt32(tube_count[i]),
                            strip_c[i],
                            h62[i], not_h62[i],
                            out_of_range_strip_count[i],
                            Convert.ToDouble(stddev[i]), Convert.ToBoolean(is_valid[i])
                            ));
                 }
                 catch (Exception ex)
                 {
                     Trace.TraceError("exception:{0}",ex.StackTrace);
                 }
                 i++;
             }

             this.Width = dataGridRoll.Width;
             dataGridRoll.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView1_ColumnHeaderMouseClick);
            dataGridView1.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            dataGridRoll.CellMouseClick += new DataGridViewCellMouseEventHandler(dataGridRoll_CellMouseClick);
            this.dataGridView1.CellMouseDown += new DataGridViewCellMouseEventHandler(dataGridView1_CellMouseDown);
            this.dataGridView1.Anchor = AnchorStyles.None;
            this.Load += new System.EventHandler(EnumsAndComboBox_Load_For_All);
        }

        /// <summary>
        /// if right mouseclick happened then a local contextmenu shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DataGridViewCell c = (sender as DataGridView)[e.ColumnIndex, e.RowIndex];
                if (!c.Selected)
                {
                    c.DataGridView.ClearSelection();
                    c.DataGridView.CurrentCell = c;
                    c.Selected = true;
                }

                selected_lot_id = dataGridView1.Rows[e.RowIndex].Cells["Lot azonosító"].Value;
                selected_roll_id = dataGridView1.Rows[e.RowIndex].Cells["Roll azonosító"].Value;
                selected_result = dataGridView1.Rows[e.RowIndex].Cells["Eredmény"].Value;
                selected_measurytype = dataGridView1.Rows[e.RowIndex].Cells["Mérés típusa"].Value;


                ShowContextMenu(c, selected_lot_id, selected_roll_id, selected_result, selected_measurytype);

            }
        }


        void invalidate_item_Click(object sender, EventArgs e)
        {
            //hide current datagrid
            this.WindowState = FormWindowState.Minimized;

            //Warn the operator abourt remeasured result must be remeasured instantly!!
            new Dialoge(selected_lot_id.ToString(), selected_roll_id.ToString(), measuretype, true).ShowDialog();

            this.WindowState = FormWindowState.Maximized;
            ///set the selected roll remeasured
            SetInvalidatedClickedRoll(Convert.ToString(selected_lot_id), Convert.ToString(selected_roll_id), Convert.ToString(selected_measurytype));

            this.Close();

            //start a new measurement to remeasure
            switch (measuretype)
            {
                case "blank":
                    Program.measureType = "blank";
                    break;
                case "homogenity":
                    Program.measureType = "homogenity";
                    break;
                case "accuracy":
                    Program.measureType = "accuracy";
                    break;

            }

            Program.GenerateUIAndStartApp(new string[] { Program.measureType });
        }
        public void SetInvalidatedClickedRoll(string lot_id, string roll_id, string measuretype)
        {
            #region get stored measure_type
            if (measuretype.Contains("lank"))
            {
                measuretype = "blank";

            }
            else if (measuretype.Contains("omogenity"))
            {
                measuretype = "homogenity";
            }


            #endregion
            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();

                    #region set remeasured to true at averages row

                    using (NpgsqlCommand invalidate = new NpgsqlCommand(
                        string.Format("UPDATE blank_test_averages SET invalidate=true WHERE lot_id='{0}' and roll_id='{1}'", lot_id, roll_id), conn))
                    {
                        object res = null;
                        res = invalidate.ExecuteNonQuery();

                        if (res == DBNull.Value
                            || res == null)
                        {
                            Trace.TraceError("null value for invalidate: {0}", invalidate.CommandText);
                            throw new ArgumentException("null value for invalidate: {0}", invalidate.CommandText);
                        }

                        Trace.TraceInformation("Invalidated roll in identify: {0}", remeasured_rollid);
                    }
                    #endregion


                    Trace.TraceInformation("in identify table the results with the roll,lot id and meastype is set remeasured=true");

                    #region set remeasured to true at identify columns

                    using (NpgsqlCommand invalidate = new NpgsqlCommand(
                        string.Format("UPDATE blank_test_identify SET invalidate=true WHERE lot_id='{0}' and roll_id='{1}' and measure_type='{2}'", lot_id, roll_id, measuretype), conn))
                    {
                        object res = null;
                        res = invalidate.ExecuteNonQuery();

                        if (res == DBNull.Value
                            || res == null)
                        {
                            Trace.TraceError("null value for invalidate: {0}", invalidate.CommandText);
                            throw new ArgumentException("null value for invalidate: {0}", invalidate.CommandText);
                        }

                        Trace.TraceInformation("Invalidated roll in identify: {0}", remeasured_rollid);
                    }
                    #endregion

                    #region get result pkid columns
                    using (NpgsqlCommand invalidate = new NpgsqlCommand(
                        string.Format("SELECT fk_blank_test_result_id from blank_test_identify WHERE lot_id='{0}' and roll_id='{1}' and measure_type='{2}'", lot_id, roll_id, measuretype), conn))
                    {
                        using (NpgsqlDataReader dr = invalidate.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    invalidate_result_pkid.Add(Convert.ToInt32(dr["fk_blank_test_result_id"]));

                                }
                                dr.Close();
                            }
                            else
                            {

                                dr.Close();
                                MessageBox.Show("Nincs megfelelő eredmény");

                                throw new SqlNoValueException("Nincs megfelelő eredmény");
                            }

                        }

                        Trace.TraceInformation("Invalidated pk in result: {0}", invalidate_result_pkid.ItemsToString());
                    }
                    #endregion

                    Trace.TraceInformation("in results table the results with the roll,lot id is set invalidate=true");

                    #region set remeasured to true in result and environment table

                    foreach (int pk in invalidate_result_pkid)
                    {
                        using (NpgsqlCommand setRemeasureResults =
                            new NpgsqlCommand(
                                string.Format("UPDATE blank_test_result SET invalidate=true WHERE pk_id={0}", Convert.ToInt32(pk)), conn))
                        {
                            object res = null;
                            res = setRemeasureResults.ExecuteNonQuery();

                            if (res == DBNull.Value)
                            {
                                Trace.TraceError("Unsuccesfull update in result table");
                            }
                        }
                        Trace.TraceInformation("remeasured results in results table are setted to true");

                        using (NpgsqlCommand invalidate = new NpgsqlCommand(
                            string.Format("UPDATE blank_test_environment SET invalidate=true WHERE lot_id='{0}' and roll_id='{1}' and fk_blank_test_result_id={2}", lot_id, roll_id, pk), conn))
                        {
                            object res = null;
                            res = invalidate.ExecuteNonQuery();

                            if (res == DBNull.Value)
                            {
                                Trace.TraceError("null value for invalidate: {0}", invalidate.CommandText);
                                throw new ArgumentException("null value for invalidate: {0}", invalidate.CommandText);
                            }

                            Trace.TraceInformation("Invalidated roll in result: {0}", roll_id);
                        }
                        invalidate_errors_pkid = new List<int>();
                    #endregion

                        #region get errors columns
                        using (NpgsqlCommand invalidate = new NpgsqlCommand(
                            string.Format("SELECT fk_blank_test_errors_id from blank_test_result WHERE lot_id='{0}' and roll_id='{1}' and pk_id={2}", lot_id, roll_id, pk), conn))
                        {
                            using (NpgsqlDataReader dr = invalidate.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        invalidate_errors_pkid.Add(Convert.ToInt32(dr["fk_blank_test_errors_id"]));

                                    }
                                    dr.Close();
                                }
                                else
                                {

                                    dr.Close();
                                    MessageBox.Show("Nincs megfelelő eredmény");

                                    throw new SqlNoValueException("Nincs megfelelő eredmény");
                                }

                            }

                            Trace.TraceInformation("Invalidated roll in result: {0}", remeasured_rollid);
                        }
                    }

                        #endregion

                    #region set remeasured errors table and measure_type table

                    foreach (int actpk in invalidate_errors_pkid)
                    {
                        using (NpgsqlCommand invErrors = new NpgsqlCommand(string.Format("update blank_test_errors set invalidate=true WHERE pk_id={0}", actpk), conn))
                        {
                            object res = null;
                            res = invErrors.ExecuteNonQuery();

                            if (res == DBNull.Value)
                            {
                                Trace.TraceError("Unsuccesfull update in result table");
                            }
                        }
                        Trace.TraceInformation("remeasured results in errors table are setted to true");



                    }
                    #endregion


                    if (measuretype.Equals("homogenity"))
                    {
                        Trace.TraceInformation("in homogenity_result table the results with the roll,lot id and meastype is set invalidate=true");

                        #region  set remeasured to true at homogenity_result columns

                        using (NpgsqlCommand invalidate = new NpgsqlCommand(
                            string.Format("UPDATE homogenity_result SET invalidate=true WHERE lot_id = '{0}' and roll_id='{1}'", lot_id, roll_id), conn))
                        {
                            object res = null;
                            res = invalidate.ExecuteNonQuery();

                            if (res == DBNull.Value)
                            {
                                Trace.TraceError("null value for invalidate: {0}", invalidate.CommandText);
                                throw new ArgumentException("null value for invalidate: {0}", invalidate.CommandText);
                            }

                            Trace.TraceInformation("Invalidated roll in identify: {0}", remeasured_rollid);
                        }
                        #endregion

                        Trace.TraceInformation("in homogenity_test table the results with the roll,lot id and meastype is set remeasured=true");

                        #region  set remeasured to true at homogenity_test columns
                        using (NpgsqlCommand invalidate = new NpgsqlCommand(
                            string.Format("UPDATE homogenity_test SET invalidate=true WHERE lot_id = '{0}' and roll_id='{1}'", lot_id, roll_id), conn))
                        {
                            object res = null;
                            res = invalidate.ExecuteNonQuery();

                            if (res == DBNull.Value)
                            {
                                Trace.TraceError("null value for invalidate: {0}", invalidate.CommandText);
                                throw new ArgumentException("null value for invalidate: {0}", invalidate.CommandText);
                            }

                            Trace.TraceInformation("Invalidated roll in identify: {0}", remeasured_rollid);
                        }

                        #endregion

                        Trace.TraceInformation("in roll_result table the results with the roll,lot id and meastype is set remeasured=true");
                        #region  set remeasured to true at roll result columns

                        using (NpgsqlCommand invalidate = new NpgsqlCommand(
                            string.Format("UPDATE roll_result SET invalidate=true WHERE lot_id = '{0}' and roll_id='{1}'", lot_id, roll_id), conn))
                        {
                            object res = null;
                            res = invalidate.ExecuteNonQuery();

                            if (res == DBNull.Value)
                            {
                                Trace.TraceError("null value for invalidate: {0}", invalidate.CommandText);
                                throw new ArgumentException("null value for invalidate: {0}", invalidate.CommandText);
                            }

                            Trace.TraceInformation("Invalidated roll in identify: {0}", remeasured_rollid);
                        }

                        #endregion

                        ///Not used when only a roll need to remeasure
                        Trace.TraceInformation("in lot_result table the results with the roll,lot id and meastype is set remeasured=true");
                        #region  set remeasured to true at roll result columns

                    }


                    if (measuretype.Equals("blank"))
                    {

                        Trace.TraceInformation("in blank_test_averages table the results with the roll,lot id and meastype is set remeasured=true");
                        #region  set remeasured to true at blank_test_averages columns
                        using (NpgsqlCommand invalidate = new NpgsqlCommand(
                            string.Format("UPDATE blank_test_averages SET invalidate=true WHERE lot_id = '{0}' and roll_id='{1}'", remeasured_lotid, remeasured_rollid), conn))
                        {
                            object res = null;
                            res = invalidate.ExecuteNonQuery();

                            if (res == DBNull.Value)
                            {
                                Trace.TraceError("null value for invalidate: {0}", invalidate.CommandText);
                                throw new ArgumentException("null value for invalidate: {0}", invalidate.CommandText);
                            }

                            Trace.TraceInformation("Invalidated roll in identify: {0}", remeasured_rollid);
                        }

                        #endregion
                    }

                    Trace.TraceInformation("in results and error table the results with the roll,lot id and meastype is set remeasured=true");
                    //didn't invalidated table lot_result

                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception at ResultForm buttonclick, ex:{0}", ex.StackTrace);
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }

        }///SetInvalidateRoll


        /// <summary>
        /// Remeasure roll item clicked on the contextMenu 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void remeasure_item_Click(object sender, EventArgs e)
        {
            //hide current datagrid
            this.WindowState = FormWindowState.Minimized;

            //Warn the operator abourt remeasured result must be remeasured instantly!!
            new Dialoge(selected_lot_id.ToString(), selected_roll_id.ToString(), measuretype).ShowDialog();

            this.WindowState = FormWindowState.Maximized;
            ///set the selected roll remeasured
            SetRemeasuredClickedRoll(Convert.ToString(selected_lot_id), Convert.ToString(selected_roll_id), Convert.ToString(selected_measurytype));

            this.Close();

            //start a new measurement to remeasure
            switch (measuretype)
            {
                case "blank":
                    Program.measureType = "blank";
                    break;
                case "homogenity":
                    Program.measureType = "homogenity";
                    break;
                case "accuracy":
                    Program.measureType = "accuracy";
                    break;

            }

            Program.GenerateUIAndStartApp(new string[] { Program.measureType });

        }


        /// <summary>
        /// Show all data item clicked on the contextmenu 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void showdata_item_Click(object sender, EventArgs e)
        {


            #region In ncase of selected row

            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();

                    #region get values from identify table where lot,roll, measuretype is correct(result_id,serial_number)
                    using (NpgsqlCommand getInvalidValuesfromIdentify =
                       new NpgsqlCommand(
                           string.Format("select pk_id from blank_test_identify where invalidate=false and remeasured=false and measure_type='blank' and lot_id='{0}' and roll_id='{1}'", selected_lot_id, selected_roll_id), conn))
                    {
                        object res = null;
                        res = getInvalidValuesfromIdentify.ExecuteScalar();

                        if (res == DBNull.Value
                            || res == null)
                        {
                            Trace.TraceWarning("No rows are validated in identify, query:{0}", getInvalidValuesfromIdentify.CommandText);
                        }
                        else
                        {
                            Trace.TraceWarning("All needed rows are validated in identify, query{0}", getInvalidValuesfromIdentify.CommandText);

                            using (NpgsqlCommand getValuesfromIdentify =
                                new NpgsqlCommand(
                                      string.Format("select distinct fk_blank_test_result_id,serial_number from blank_test_identify where invalidate=false and remeasured=false and measure_type='blank' and lot_id='{0}' and roll_id='{1}'", selected_lot_id, selected_roll_id), conn))
                            {
                                using (NpgsqlDataReader dr = getValuesfromIdentify.ExecuteReader())
                                {
                                    if (dr.HasRows)
                                    {
                                        while (dr.Read())
                                        {

                                            selected_result_ids.Add(Convert.ToInt32(dr["fk_blank_test_result_id"]));
                                        }
                                        dr.Close();
                                    }
                                    else
                                    {
                                        Trace.TraceWarning("No value for query: {0}", getValuesfromIdentify.CommandText);
                                        dr.Close();
                                    }

                                }

                            }
                        }


                    }

                    #endregion

                    #region get values from all table where selected test result can be found

                    foreach (int act_result_id in selected_result_ids)
                    {
                        #region get rows which are invalidated and skip that rows in the query to collect data
                        using (NpgsqlCommand getInvalidValuesfromResults =
                           new NpgsqlCommand(
                               string.Format("select pk_id from blank_test_result where invalidate=false and remeasured=false and pk_id={0}", act_result_id), conn))
                        {
                            object res = null;
                            res = getInvalidValuesfromResults.ExecuteScalar();

                            if (res == DBNull.Value
                                || (res == null))///Then no row is invalid
                            {
                                Trace.TraceWarning("Rows where result_id:{1} are invalidated in result, query:{0}", getInvalidValuesfromResults.CommandText, act_result_id);

                            }
                            else///the act row is valid get that
                            {

                                Trace.TraceWarning("All needed rows are validated in identify, query{0}", getInvalidValuesfromResults.CommandText);

                                using (NpgsqlCommand getValuesfromResults =
                                          new NpgsqlCommand(
                                              string.Format("select * from blank_test_result where invalidate=false and remeasured=false and pk_id={0}", act_result_id), conn))
                                {


                                    using (NpgsqlDataReader dr = getValuesfromResults.ExecuteReader())
                                    {
                                        if (dr.HasRows)
                                        {
                                            while (dr.Read())
                                            {

                                                selected_codes.Add(Convert.ToInt32(dr["code"]));
                                                selected_rolls.Add(Convert.ToString(dr["roll_id"]));
                                                selected_serial_numbers.Add(Convert.ToString(dr["sn"]));
                                                selected_glus.Add(Convert.ToDouble(dr["glu"]));
                                                selected_nano_ampers.Add(Convert.ToDouble(dr["nano_amper"]));
                                                selected_fk_errors_id.Add(Convert.ToInt32(dr["fk_blank_test_errors_id"]));
                                            }
                                            dr.Close();
                                        }
                                        else
                                        {
                                            dr.Close();
                                            Trace.TraceWarning("No value for query: {0}", getValuesfromResults.CommandText);

                                        }

                                    }

                                }
                            }

                        }

                    }
                    using (NpgsqlCommand getInvalidatedfromEnv =
                       new NpgsqlCommand(
                           string.Format("select pk_id from blank_test_environment where remeasured=false and invalidate=false and lot_id='{0}' and roll_id='{1}'", selected_lot_id, selected_roll_id), conn))
                    {
                        object res = null;
                        res = getInvalidatedfromEnv.ExecuteScalar();

                        if (res == DBNull.Value
                            || (res == null))
                        {
                            Trace.TraceWarning("Rows where result_id:{1} are invalidated in envronment, query:{0}", getInvalidatedfromEnv.CommandText);


                        }
                        else
                        {

                            Trace.TraceWarning("All needed rows are validated in identify, query{0}", getInvalidatedfromEnv.CommandText);

                            using (NpgsqlCommand getValuesfromEnv =
                                 new NpgsqlCommand(
                            string.Format("select * from blank_test_environment where invalidate=false and remeasured=false and lot_id='{0}' and roll_id='{1}'", selected_lot_id, selected_roll_id), conn))
                            {
                                using (NpgsqlDataReader dr = getValuesfromEnv.ExecuteReader())
                                {
                                    if (dr.HasRows)
                                    {
                                        while (dr.Read())
                                        {
                                            selected_users.Add(Convert.ToString(dr["user_name"]));
                                            selected_computers.Add(Convert.ToString(dr["computer_name"]));
                                            selected_start_dates.Add(Convert.ToDateTime(dr["start_date"]));
                                            selected_end_dates.Add(Convert.ToDateTime(dr["end_date"]));
                                            selected_temp.Add(Convert.ToDouble(dr["temperature"]));

                                        }
                                        dr.Close();
                                    }
                                    else
                                    {
                                        Trace.TraceWarning("No value for query: {0}", getValuesfromEnv.CommandText);
                                        dr.Close();
                                    }

                                }

                            }
                        }
                    }

                    foreach (int act_error_id in selected_fk_errors_id)
                    {
                        using (NpgsqlCommand getInvalidValuesfromErrors =
                           new NpgsqlCommand(
                               string.Format("select * from blank_test_errors where invalidate=false and remeasured=false and pk_id={0}", act_error_id), conn))
                        {
                            object res = null;

                            res = getInvalidValuesfromErrors.ExecuteScalar();

                            if (res == DBNull.Value
                                || (res == null))
                            {

                                Trace.TraceWarning("Rows where error_id:{1} are invalidated in envronment, query:{0}", getInvalidValuesfromErrors.CommandText, act_error_id);

                            }
                            else
                            {


                                using (NpgsqlCommand getValuesfromErrors =
                                                             new NpgsqlCommand(
                                                                     string.Format("select * from blank_test_errors where invalidate=false and remeasured=false and pk_id={0}", act_error_id), conn))
                                {
                                    using (NpgsqlDataReader dr = getValuesfromErrors.ExecuteReader())
                                    {
                                        if (dr.HasRows)
                                        {
                                            while (dr.Read())
                                            {
                                                selected_error.Add(Convert.ToString(dr["error"]));
                                                selected_error_text.Add(Convert.ToString(dr["error_text"]));
                                                selected_not_h62.Add(Convert.ToBoolean(dr["not_h62_error"]));
                                                selected_h62.Add(Convert.ToBoolean(dr["h62_error"]));
                                                selected_device_replace.Add(Convert.ToBoolean(dr["device_replace"]));
                                                selected_early_dribble.Add(Convert.ToBoolean(dr["early_dribble"]));
                                            }
                                            dr.Close();
                                        }
                                        else
                                        {
                                            Trace.TraceWarning("No value for query: {0}", getValuesfromErrors.CommandText);
                                            dr.Close();
                                        }

                                    }

                                }
                            }
                        }


                    }
                        #endregion
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    conn.Close();
                }


            }//NpgsqlConnection

                    #endregion

            CreateAllDataTable();

            #endregion
        }



        private void CreateAllDataTable()
        {       ///This datagrid shows all the measurement of the selected roll
            new AllDataTable(selected_result, selected_lot_id, selected_roll_id, selected_serial_numbers,
                selected_users, selected_computers, selected_error, selected_error_text, selected_glus, selected_nano_ampers,
                selected_not_h62, selected_h62, selected_start_dates, selected_end_dates).Show();

            ClearGridVariables(selected_result, selected_lot_id, selected_roll_id, selected_serial_numbers, selected_users, selected_computers, selected_error, selected_error_text, selected_glus, selected_nano_ampers, selected_not_h62, selected_h62, selected_start_dates, selected_end_dates);

        }
        /// <summary>
        /// Empty the Lists to avoid data-mixing
        /// </summary>
        /// <param name="selected_result"></param>
        /// <param name="selected_lot_id"></param>
        /// <param name="selected_roll_id"></param>
        /// <param name="selected_serial_numbers"></param>
        /// <param name="selected_users"></param>
        /// <param name="selected_computers"></param>
        /// <param name="selected_error"></param>
        /// <param name="selected_error_text"></param>
        /// <param name="selected_glus"></param>
        /// <param name="selected_nano_ampers"></param>
        /// <param name="selected_not_h62"></param>
        /// <param name="selected_h62"></param>
        /// <param name="selected_start_dates"></param>
        /// <param name="selected_end_dates"></param>
        private void ClearGridVariables(object selected_result, object selected_lot_id, object selected_roll_id, List<string> selected_serial_numbers,
            List<string> selected_users, List<string> selected_computers, List<string> selected_error, List<string> selected_error_text,
            List<double> selected_glus, List<double> selected_nano_ampers, List<bool> selected_not_h62, List<bool> selected_h62,
            List<DateTime> selected_start_dates, List<DateTime> selected_end_dates)
        {
            selected_result = null;
            selected_lot_id = null;
            selected_serial_numbers = new List<string>();

            selected_users = new List<string>();
            selected_computers = new List<string>();
            selected_error = new List<string>();
            selected_error_text = new List<string>();

            selected_glus = new List<double>();
            selected_nano_ampers = new List<double>();
            selected_not_h62 = new List<bool>();
            selected_h62 = new List<bool>();

            selected_start_dates = new List<DateTime>();
            selected_end_dates = new List<DateTime>();


        }


        /// <summary>
        /// Set Invalidate(Remeasured) the results of the roll in case of selected from the list, and start a new measurement 
        /// </summary>
        public void SetRemeasuredClickedRoll(string lot_id, string roll_id, string measuretype)
        {
            #region get stored measure_type
            if (measuretype.Contains("lank"))
            {
                measuretype = "blank";

            }
            else if (measuretype.Contains("omogenity"))
            {
                measuretype = "homogenity";
            }


            #endregion
            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();

                    #region set remeasured to true at averages row

                    using (NpgsqlCommand invalidate = new NpgsqlCommand(
                        string.Format("UPDATE blank_test_averages SET remeasured=true WHERE lot_id='{0}' and roll_id='{1}'", lot_id, roll_id), conn))
                    {
                        object res = null;
                        res = invalidate.ExecuteNonQuery();

                        if (res == DBNull.Value
                            || res == null)
                        {
                            Trace.TraceError("null value for invalidate: {0}", invalidate.CommandText);
                            throw new ArgumentException("null value for invalidate: {0}", invalidate.CommandText);
                        }

                        Trace.TraceInformation("Invalidated roll in identify: {0}", remeasured_rollid);
                    }
                    #endregion


                    Trace.TraceInformation("in identify table the results with the roll,lot id and meastype is set remeasured=true");

                    #region set remeasured to true at identify columns

                    using (NpgsqlCommand invalidate = new NpgsqlCommand(
                        string.Format("UPDATE blank_test_identify SET remeasured=true WHERE lot_id='{0}' and roll_id='{1}' and measure_type='{2}'", lot_id, roll_id, measuretype), conn))
                    {
                        object res = null;
                        res = invalidate.ExecuteNonQuery();

                        if (res == DBNull.Value
                            || res == null)
                        {
                            Trace.TraceError("null value for invalidate: {0}", invalidate.CommandText);
                            throw new ArgumentException("null value for invalidate: {0}", invalidate.CommandText);
                        }

                        Trace.TraceInformation("Invalidated roll in identify: {0}", remeasured_rollid);
                    }
                    #endregion

                    #region get result pkid columns
                    using (NpgsqlCommand invalidate = new NpgsqlCommand(
                        string.Format("SELECT fk_blank_test_result_id from blank_test_identify WHERE lot_id='{0}' and roll_id='{1}' and measure_type='{2}'", lot_id, roll_id, measuretype), conn))
                    {
                        using (NpgsqlDataReader dr = invalidate.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    invalidate_result_pkid.Add(Convert.ToInt32(dr["fk_blank_test_result_id"]));

                                }
                                dr.Close();
                            }
                            else
                            {

                                dr.Close();
                                MessageBox.Show("Nincs megfelelő eredmény");

                                throw new SqlNoValueException("Nincs megfelelő eredmény");
                            }

                        }

                        Trace.TraceInformation("Invalidated pk in result: {0}", invalidate_result_pkid.ItemsToString());
                    }
                    #endregion

                    Trace.TraceInformation("in results table the results with the roll,lot id is set remeasured=true");

                    #region set remeasured to true in result and environment table

                    foreach (int pk in invalidate_result_pkid)
                    {
                        using (NpgsqlCommand setRemeasureResults = new NpgsqlCommand(string.Format("UPDATE blank_test_result SET remeasured=true WHERE pk_id={0}", Convert.ToInt32(pk)), conn))
                        {
                            object res = null;
                            res = setRemeasureResults.ExecuteNonQuery();

                            if (res == DBNull.Value)
                            {
                                Trace.TraceError("Unsuccesfull update in result table");
                            }
                        }
                        Trace.TraceInformation("remeasured results in results table are setted to true");

                        using (NpgsqlCommand invalidate = new NpgsqlCommand(
                            string.Format("UPDATE blank_test_environment SET remeasured=true WHERE lot_id='{0}' and roll_id='{1}' and fk_blank_test_result_id={2}", lot_id, roll_id, pk), conn))
                        {
                            object res = null;
                            res = invalidate.ExecuteNonQuery();

                            if (res == DBNull.Value)
                            {
                                Trace.TraceError("null value for invalidate: {0}", invalidate.CommandText);
                                throw new ArgumentException("null value for invalidate: {0}", invalidate.CommandText);
                            }

                            Trace.TraceInformation("Invalidated roll in result: {0}", roll_id);
                        }
                        invalidate_errors_pkid = new List<int>();
                    #endregion

                        #region get errors columns
                        using (NpgsqlCommand invalidate = new NpgsqlCommand(
                            string.Format("SELECT fk_blank_test_errors_id from blank_test_result WHERE lot_id='{0}' and roll_id='{1}' and pk_id={2}", lot_id, roll_id, pk), conn))
                        {
                            using (NpgsqlDataReader dr = invalidate.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        invalidate_errors_pkid.Add(Convert.ToInt32(dr["fk_blank_test_errors_id"]));

                                    }
                                    dr.Close();
                                }
                                else
                                {

                                    dr.Close();
                                    MessageBox.Show("Nincs megfelelő eredmény");

                                    throw new SqlNoValueException("Nincs megfelelő eredmény");
                                }

                            }

                            Trace.TraceInformation("Invalidated roll in result: {0}", remeasured_rollid);
                        }
                    }

                        #endregion

                    #region set remeasured errors table and measure_type table

                    foreach (int actpk in invalidate_errors_pkid)
                    {
                        using (NpgsqlCommand invErrors = new NpgsqlCommand(string.Format("update blank_test_errors set remeasured=true WHERE pk_id={0}", actpk), conn))
                        {
                            object res = null;
                            res = invErrors.ExecuteNonQuery();

                            if (res == DBNull.Value)
                            {
                                Trace.TraceError("Unsuccesfull update in result table");
                            }
                        }
                        Trace.TraceInformation("remeasured results in errors table are setted to true");



                    }
                    #endregion


                    if (measuretype.Equals("homogenity"))
                    {
                        Trace.TraceInformation("in homogenity_result table the results with the roll,lot id and meastype is set remeasured=true");

                        #region  set remeasured to true at homogenity_result columns

                        using (NpgsqlCommand invalidate = new NpgsqlCommand(
                            string.Format("UPDATE homogenity_result SET remeasured = true WHERE lot_id = '{0}' and roll_id='{1}'", lot_id, roll_id), conn))
                        {
                            object res = null;
                            res = invalidate.ExecuteNonQuery();

                            if (res == DBNull.Value)
                            {
                                Trace.TraceError("null value for invalidate: {0}", invalidate.CommandText);
                                throw new ArgumentException("null value for invalidate: {0}", invalidate.CommandText);
                            }

                            Trace.TraceInformation("Invalidated roll in identify: {0}", remeasured_rollid);
                        }
                        #endregion

                        Trace.TraceInformation("in homogenity_test table the results with the roll,lot id and meastype is set remeasured=true");

                        #region  set remeasured to true at homogenity_test columns
                        using (NpgsqlCommand invalidate = new NpgsqlCommand(
                            string.Format("UPDATE homogenity_test SET remeasured = true WHERE lot_id = '{0}' and roll_id='{1}'", lot_id, roll_id), conn))
                        {
                            object res = null;
                            res = invalidate.ExecuteNonQuery();

                            if (res == DBNull.Value)
                            {
                                Trace.TraceError("null value for invalidate: {0}", invalidate.CommandText);
                                throw new ArgumentException("null value for invalidate: {0}", invalidate.CommandText);
                            }

                            Trace.TraceInformation("Invalidated roll in identify: {0}", remeasured_rollid);
                        }

                        #endregion

                        Trace.TraceInformation("in roll_result table the results with the roll,lot id and meastype is set remeasured=true");
                        #region  set remeasured to true at roll result columns

                        using (NpgsqlCommand invalidate = new NpgsqlCommand(
                            string.Format("UPDATE roll_result SET remeasured = true WHERE lot_id = '{0}' and roll_id='{1}'", lot_id, roll_id), conn))
                        {
                            object res = null;
                            res = invalidate.ExecuteNonQuery();

                            if (res == DBNull.Value)
                            {
                                Trace.TraceError("null value for invalidate: {0}", invalidate.CommandText);
                                throw new ArgumentException("null value for invalidate: {0}", invalidate.CommandText);
                            }

                            Trace.TraceInformation("Invalidated roll in identify: {0}", remeasured_rollid);
                        }

                        #endregion

                        ///Not used when only a roll need to remeasure
                        Trace.TraceInformation("in lot_result table the results with the roll,lot id and meastype is set remeasured=true");
                        #region  set remeasured to true at roll result columns

                    }


                    if (measuretype.Equals("blank"))
                    {

                        Trace.TraceInformation("in blank_test_averages table the results with the roll,lot id and meastype is set remeasured=true");
                        #region  set remeasured to true at blank_test_averages columns
                        using (NpgsqlCommand invalidate = new NpgsqlCommand(
                            string.Format("UPDATE blank_test_averages SET remeasured = true WHERE lot_id = '{0}' and roll_id='{1}'", remeasured_lotid, remeasured_rollid), conn))
                        {
                            object res = null;
                            res = invalidate.ExecuteNonQuery();

                            if (res == DBNull.Value)
                            {
                                Trace.TraceError("null value for invalidate: {0}", invalidate.CommandText);
                                throw new ArgumentException("null value for invalidate: {0}", invalidate.CommandText);
                            }

                            Trace.TraceInformation("Invalidated roll in identify: {0}", remeasured_rollid);
                        }

                        #endregion
                    }

                    Trace.TraceInformation("in results and error table the results with the roll,lot id and meastype is set remeasured=true");
                    //didn't invalidated table lot_result

                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception at ResultForm buttonclick, ex:{0}", ex.StackTrace);
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }

        }///SetRemeasuredRoll



       public MenuItem object_item;
       public MenuItem remeasure_item;
       public MenuItem showdata_item;
          /// <summary>
        /// Show a contextmenu on the cell which has been clicked by right mousebutton
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="selected_lot_id"></param>
        /// <param name="selected_roll_id"></param>
        /// <param name="selected_result"></param>
        public void ShowContextMenu(DataGridViewCell actCell,object selected_lot_id, object selected_roll_id, object selected_result,object measure_type)
        {
            DataGridViewCell currentCell = actCell;
            if (currentCell != null)
            {
                #region Create a local contextmenu
                ContextMenu cm = new ContextMenu();

               object_item = new MenuItem((string.Format("Invalidálás(LOT ID:{0} ,Roll ID:{1})",selected_lot_id,selected_roll_id)));
                 remeasure_item = new MenuItem((string.Format("Újramérés(LOT ID:{0} ,Roll ID:{1})",selected_lot_id,selected_roll_id)));
                 showdata_item = new MenuItem(string.Format("Adatok mutatása(LOT ID:{0} ,Roll ID:{1})",selected_lot_id,selected_roll_id));

                 remeasure_item.Click += new EventHandler(remeasure_item_Click);
                showdata_item.Click += new EventHandler(showdata_item_Click);
                object_item.Click += new EventHandler(invalidate_item_Click);
                
                cm.GetContextMenu();
                cm.MenuItems.Add(showdata_item);
                cm.MenuItems.Add(remeasure_item);
                cm.MenuItems.Add(object_item);
        
                #endregion


                #region show contextmenu
                Rectangle r = currentCell.DataGridView.GetCellDisplayRectangle(currentCell.ColumnIndex, currentCell.RowIndex, false);
                Point p = new Point(r.X + r.Width, r.Y + r.Height);
        
                
                cm.Show(currentCell.DataGridView,p);
                #endregion



       
            }
        }

        

        public void dataGridRoll_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DataGridViewCell c = (sender as DataGridView)[e.ColumnIndex, e.RowIndex];
                if (!c.Selected)
                {
                    c.DataGridView.ClearSelection();
                    c.DataGridView.CurrentCell = c;
                    c.Selected = true;
                }

                selected_lot_id = dataGridRoll.Rows[e.RowIndex].Cells["Lot azonosító"].Value;
                selected_roll_id = dataGridRoll.Rows[e.RowIndex].Cells["Roll azonosító"].Value;
                selected_result = dataGridRoll.Rows[e.RowIndex].Cells["Eredmény"].Value;
                selected_measurytype = dataGridRoll.Rows[e.RowIndex].Cells["Mérés típusa"].Value;


                ShowContextMenu(c, selected_lot_id, selected_roll_id, selected_result, selected_measurytype);
            }
        }



             private void EnumsAndComboBox_Load_For_All(object sender, System.EventArgs e)
        {

            // Initialize the DataGridView.
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSize = false;
            dataGridView1.DataSource = bindingSource1;
            dataGridView1.Dock = DockStyle.Fill;

            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "LotID";
            column.Name = "Lot azonosító";//1
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "RollID";
            column.Name = "Roll azonosító";//2
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Date";//3
            column.Name = "Dátum";
            column.Width = 120;
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "HAverages";//4
            column.Name = "Átlag";            
            dataGridView1.Columns.Add(column);

            
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "HCV";//5
            column.Name = "CV";            
            dataGridView1.Columns.Add(column);
                        

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "TubeCount";
            column.Name = "Tubus szám";        //6    
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "StripCount";
            column.Name = "Csík szám";//7
            dataGridView1.Columns.Add(column); 

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "NotH62ErrorCount";
            column.Name = "Nem h62 hibák száma";
            column.Width = 140;//8
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "H62ErrorCount";
            column.Name = "H62 hibák száma";
            column.Width = 140;//9
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Result";
            column.Name = "Értékelés";
            column.Width = 140;//10
            dataGridView1.Columns.Add(column);

            // Initialize the form.
            this.Controls.Add(dataGridView1);
            this.AutoSize = false;
            this.Text = "Homogenity teszt eredmények";
            //this.Width = dataGridView1.Width;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
               
                if (row.Cells[9].Value == "Megfelelő")
                {
                    row.DefaultCellStyle.BackColor = Color.Green;
                }
                else
                    row.DefaultCellStyle.BackColor = Color.Red;
            }
            double columnsWidth = 0.0;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                columnsWidth += col.Width;
            }

            this.Width = Convert.ToInt32(columnsWidth);

        }


             /// <summary>
             /// The click on the header of the dgv changes the sort of the items
             /// </summary>
             /// <param name="sender"></param>
             /// <param name="e"></param>
             private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
             {

                 ///Firtst check if the click was  sended by rightmouse-button
                 if (e.ColumnIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right)
                 {
                     ShowContextMenu(AllDataTable.dataGridAll.Columns[e.ColumnIndex].HeaderCell);

                 }

                 //get the current column details
                 string strColumnName = AllDataTable.dataGridAll.Columns[e.ColumnIndex].Name;
                 SortOrder strSortOrder = getSortOrder(e.ColumnIndex);

                 ///  AllDataTable.dataGridAll.Sort(AllDataTable.dataGridAll.Columns[e.ColumnIndex],ListSortDirection.Ascending);

                 AllDataTable.dataGridAll.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = strSortOrder;
             }


             MenuItem hide_item;
             MenuItem calculate_item;
             MenuItem color_item;
             DataGridViewCell currCell;
             /// <summary>
             /// Show local or ContextMenu where the right mousebutton pressed
             /// </summary>
             /// <param name="actCell"></param>
             /// <param name="selected_lot_id"></param>
             /// <param name="selected_roll_id"></param>
             /// <param name="selected_result"></param>
             /// <param name="measure_type"></param>
             public void ShowContextMenu(DataGridViewCell actCell)
             {
                 DataGridViewCell currentCell = actCell;
                 if (currentCell != null)
                 {
                     #region Create a local contextmenu
                     ContextMenu cm = new ContextMenu();

                     hide_item = new MenuItem((
                         string.Format("Oszlop elrejtése:{0} oszlop", actCell.OwningColumn.HeaderCell.Value)));
                     calculate_item = new MenuItem((
                         string.Format("Értékekből avg,stddev,cv vagy összeg számolás:{0} oszlop", actCell.OwningColumn.HeaderCell.Value)));
                     color_item = new MenuItem(
                         string.Format("Adatok kiemelése színnel, {0} oszlop", actCell.OwningColumn.HeaderCell.Value));

                     hide_item.Click += new EventHandler(hide_item_Click);
                     calculate_item.Click += new EventHandler(calculate_item_Click);
                     color_item.Click += new EventHandler(color_item_Click);

                     cm.GetContextMenu();
                     cm.MenuItems.Add(hide_item);
                     cm.MenuItems.Add(calculate_item);
                     cm.MenuItems.Add(color_item);

                     #endregion


                     #region show contextmenu
                     Rectangle r = currentCell.DataGridView.GetCellDisplayRectangle(
                         currentCell.ColumnIndex, currentCell.RowIndex, false);
                     Point p = new Point(r.X + r.Width, r.Y + r.Height);

                     currCell = currentCell;
                     cm.Show(currentCell.DataGridView, p);
                     #endregion




                 }
             }
             int column_index;
             int column_number;
             /// <summary>
             /// In case of select this item from the menu it will color showhigh the column
             /// </summary>
             /// <param name="sender"></param>
             /// <param name="e"></param>
             private void color_item_Click(object sender, EventArgs e)
             {

                 column_index = currCell.ColumnIndex;

                 foreach (DataGridViewRow row in dataGridRoll.Rows)
                 {
                     DataGridViewCellStyle st = row.Cells[column_index].Style;

                     if (st.BackColor == Color.Magenta)
                     {
                         st.BackColor = Color.Green;
                     }
                     else
                         st.BackColor = Color.Magenta;

                 }

             }




             int row_number;
             int doubleTyped;
             int stringTyped;
             int boolTyped;
             int intTyped;
             int dateType;
             private int not_h62d_error_count;
             private int not_accepted_count;
             private int accepted_count;
             private int false_items;
             private int summary_ints;
             private int int_avg;
             private int true_items;
             private int h62d_error_count;
             private double avg;
             private double sd;
             private double cv;
             TimeSpan tsp;
             List<string> measure_type = new List<string>();
             private double sumOfSquaresOfDifferences;
             private double sum;
             private double stddev;
             private object selected_lot_id;
             private object selected_measurytype;
             private object selected_result;
             private object selected_roll_id;
             private object[] remeasured_rollid;

             List<int> invalidate_result_pkid = new List<int>();
             List<int> invalidate_errors_pkid = new List<int>();
             private string remeasured_lotid;
             private string measuretype;
             /// <summary>
             /// Selected this from the contextmenu this will calculate avg,stddev,cv from double
             /// will count,sum and avg the values if integer types are in the column
             /// in case of bool it will count the true and the false values
             /// if string then it can check that the value is Megfelelő or Nem Megfelelő
             /// in case of Datetime it will count a time period between the first and last
             /// </summary>
             /// <param name="sender"></param>
             /// <param name="e"></param>
             public void calculate_item_Click(object sender, EventArgs e)
             {

                 #region get column index

                 if (sender is DataGridViewColumn)
                 {
                     column_number = ((DataGridViewColumn)sender).Index;
                 }
                 else if (sender is DataGridViewCell)
                 {
                     column_number = ((DataGridViewCell)(sender)).ColumnIndex;
                 }

                 #endregion



                 #region get values of the column and the type of them
                 if ((dataGridRoll.Rows[2].HeaderCell.Value.Equals("Nano Amper")
                     || dataGridRoll.Rows[2].HeaderCell.Value.Equals("Glucose érték")))
                 {
                     #region get values of the column and the type of them
                     foreach (DataGridViewRow row in AllDataTable.dataGridAll.Rows)
                     {
                         types.Add(row.Cells[column_index].ValueType);
                         values.Add(Convert.ToString(row.Cells[column_index].Value));

                         row_number++;
                     }
                     foreach (Type T in types)
                     {
                         if (T == typeof(double))
                         {
                             doubleTyped++;
                         }
                         else if (T == typeof(int))
                         {
                             intTyped++;
                         }
                         else if (T == typeof(string))
                         {
                             stringTyped++;

                         }
                         else if (T == typeof(bool))
                         {
                             boolTyped++;
                         }
                         else if (T == typeof(DateTime))
                         {
                             dateType++;

                         }
                     }
                     #endregion
                 }
                 else if ((dataGridRoll.Rows[2].HeaderCell.Value.Equals("H62 hiba történt")
                    || dataGridRoll.Rows[2].HeaderCell.Value.Equals("Nem H62 hiba történt")))
                 {
                     #region get values of the column and the type of them
                     foreach (DataGridViewRow row in AllDataTable.dataGridAll.Rows)
                     {
                         types.Add(row.Cells[column_index].ValueType);
                         values.Add(Convert.ToString(row.Cells[column_index].Value));

                         row_number++;
                     }
                     foreach (Type T in types)
                     {
                         if (T == typeof(double))
                         {
                             doubleTyped++;
                         }
                         else if (T == typeof(int))
                         {
                             intTyped++;
                         }
                         else if (T == typeof(string))
                         {
                             stringTyped++;

                         }
                         else if (T == typeof(bool))
                         {
                             boolTyped++;
                         }
                         else if (T == typeof(DateTime))
                         {
                             dateType++;

                         }
                     }
                     #endregion
                 }
                 else if ((dataGridRoll.Rows[2].HeaderCell.Value.Equals("Nem H62 hiba történt")))
                 {
                     #region get values of the column and the type of them
                     foreach (DataGridViewRow row in AllDataTable.dataGridAll.Rows)
                     {
                         types.Add(row.Cells[column_index].ValueType);
                         values.Add(Convert.ToString(row.Cells[column_index].Value));

                         row_number++;
                     }
                     foreach (Type T in types)
                     {
                         if (T == typeof(double))
                         {
                             doubleTyped++;
                         }
                         else if (T == typeof(int))
                         {
                             intTyped++;
                         }
                         else if (T == typeof(string))
                         {
                             stringTyped++;

                         }
                         else if (T == typeof(bool))
                         {
                             boolTyped++;
                         }
                         else if (T == typeof(DateTime))
                         {
                             dateType++;

                         }

                     }
                     foreach (string item in values)
                     {
                         if (values.Equals("Nem történt"))
                         {

                         }
                         else if (values.Equals("Történt"))
                         {

                             not_h62d_error_count++;
                         }
                     }
                     #endregion
                 }
                 else if ((dataGridRoll.Rows[2].HeaderCell.Value.Equals("H62 hiba történt")))
                 {
                     #region get values of the column and the type of them
                     foreach (DataGridViewRow row in AllDataTable.dataGridAll.Rows)
                     {
                         types.Add(row.Cells[column_index].ValueType);
                         values.Add(Convert.ToString(row.Cells[column_index].Value));

                         row_number++;
                     }
                     foreach (Type T in types)
                     {
                         if (T == typeof(double))
                         {
                             doubleTyped++;
                         }
                         else if (T == typeof(int))
                         {
                             intTyped++;
                         }
                         else if (T == typeof(string))
                         {
                             stringTyped++;

                         }
                         else if (T == typeof(bool))
                         {
                             boolTyped++;
                         }
                         else if (T == typeof(DateTime))
                         {
                             dateType++;

                         }
                     }
                     foreach (string item in values)
                     {
                         if (values.Equals("Nem történt"))
                         {

                         }
                         else if (values.Equals("Történt"))
                         {

                             h62d_error_count++;
                         }
                     }
                     #endregion
                 }
                 #region check if all cell has the same type
                 ///All cell in that column has a double value
                 if (doubleTyped == row_number)
                 {
                     foreach (object item in values)
                     {
                         #region calculate avg

                         avg += Convert.ToDouble(item);

                         #endregion

                     }
                     ///The sum allocated with the count of items
                     avg = avg / row_number;

                     foreach (object item in values)
                     {
                         ///After we have avg ,then can calculate stddev
                         #region calculate stddev

                         ///So each item decreased with the average and get on the second square
                         sumOfSquaresOfDifferences = ((Convert.ToDouble(item) - avg) * (Convert.ToDouble(item) - avg));

                         ///Get the summary of these
                         sum += sumOfSquaresOfDifferences;

                         ///then the summarry of decreased item values will be allocated with item count
                         sd = Math.Sqrt(sum / values.Count);
                         #endregion

                     }

                     ///CV is simple stddev/avg (*100)
                     cv = (stddev / avg) * 100;

                     ///All of the item is integer
                 }
                 else if (intTyped == row_number)
                 {
                     foreach (object item in values)
                     {
                         summary_ints += Convert.ToInt32(item);
                     }
                     int_avg = summary_ints / row_number;

                     ///All item is bool so we can count each the true and the false values
                 }
                 else if (boolTyped == row_number)
                 {
                     foreach (object item in values)
                     {
                         if (Convert.ToBoolean(item))
                         {
                             true_items++;
                         }
                         else
                             false_items++;
                     }
                     ///Each item is string, like "Megfelelő" ,an ID or an error_text
                 }
                 else if (stringTyped == row_number)
                 {
                     foreach (object item in values)
                     {
                         if (Convert.ToString(item).Equals("Megfelelő"))
                         {
                             accepted_count++;
                         }
                         else if (Convert.ToString(item).Equals("Nem Megfelelő"))
                         {
                             not_accepted_count++;
                         }
                     }
                 }
                 else if (dateType == row_number)
                 {
                     tsp = Convert.ToDateTime(values[0]).Date - Convert.ToDateTime(values[row_number]).Date;
                 }
                 #endregion

                 new CalculatedColumnValuesForm(avg, sd, cv,
                     int_avg, summary_ints, accepted_count, not_accepted_count,
                     not_h62d_error_count, h62d_error_count,
                     tsp, dataGridRoll.Columns[column_index].Name).Show();


             }



             /// <summary>
             /// a helyimenüből erre kattintva az oszlop fejlécén akkor eltünteti az oszlopot
             /// </summary>
             /// <param name="sender"></param>
             /// <param name="e"></param>
             private void hide_item_Click(object sender, EventArgs e)
             {
                 int column_number = 0; ;

                 #region get column index

                 if (sender is DataGridViewColumn)
                 {
                     column_number = ((DataGridViewColumn)sender).Index;
                 }
                 else if (sender is DataGridViewCell)
                 {
                     column_number = ((DataGridViewCell)(sender)).ColumnIndex;
                 }

                 #endregion
                 dataGridRoll.Columns.Remove(dataGridRoll.Columns[column_number]);

                 dataGridRoll.Columns[column_number].Dispose();

             }





             /// <summary>
             /// Get the current sort order of the column and return it
             /// set the new SortOrder to the columns.
             /// </summary>
             /// <param name="columnIndex"></param>
             /// <returns>SortOrder of the current column</returns>
             private SortOrder getSortOrder(int columnIndex)
             {
                 if (AllDataTable.dataGridAll.Columns[columnIndex].HeaderCell.SortGlyphDirection == SortOrder.None ||
                     AllDataTable.dataGridAll.Columns[columnIndex].HeaderCell.SortGlyphDirection == SortOrder.Descending)
                 {
                     AllDataTable.dataGridAll.Columns[columnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                     return SortOrder.Ascending;
                 }
                 else
                 {
                     AllDataTable.dataGridAll.Columns[columnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                     return SortOrder.Descending;
                 }
             }




             /// <summary>
             /// The click on the header of the dgv changes the sort of the items
             /// </summary>
             /// <param name="sender"></param>
             /// <param name="e"></param>
             public void dataGridView1_ColumnHeaderMouseClicked(object sender, DataGridViewCellMouseEventArgs e)
             {

                 ///Firtst check if the click was  sended by rightmouse-button
                 if (e.ColumnIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right)
                 {
                     //ShowContextMenu(dataGridView1.Columns[e.ColumnIndex].HeaderCell);
                     ShowContextMenu(dataGridRoll.Columns[e.ColumnIndex].HeaderCell);

                 }

                 //get the current column details
                 //string strColumnName = dataGridView1.Columns[e.ColumnIndex].Name;
                 string strColumnName = dataGridRoll.Columns[e.ColumnIndex].Name;
                 SortOrder strSortOrder = getSortOrder(e.ColumnIndex);

                 ///  dataGridAll.Sort(dataGridAll.Columns[e.ColumnIndex],ListSortDirection.Ascending);

                 //dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = strSortOrder;
                 dataGridRoll.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = strSortOrder;
             }

                 #endregion
                        #endregion
                        #endregion

    }
    
}

