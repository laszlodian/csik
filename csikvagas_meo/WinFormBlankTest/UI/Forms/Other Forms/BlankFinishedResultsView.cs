using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Npgsql;
using WinFormBlankTest.Controller.DataManipulation;
using System.IO;
using System.Web.Configuration;


namespace WinFormBlankTest.UI.Forms.Other_Forms
{
    public partial class BlankFinishedResultsView : Form
    {

        #region Variables

        public BindingSource bindingSourceAll = new BindingSource();
        public BindingSource bindingSource1 = new BindingSource();
        private int AllWidth = 0;
        public static string resultString;
        private Series series1;
        public string blankResultQuery;
      
        private string lotid;
        private string rollid;
        private DataGridView dataGridView1 = new DataGridView();
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        public DataTable table = new DataTable();

        private DataGridView dataGridViewAll = new DataGridView();
        private System.Windows.Forms.DataVisualization.Charting.Chart chartAll;
        public DataTable tableAll = new DataTable();
        public DataTable tableEnv = new DataTable("blank_test_environment");
        public DataTable tableErrors = new DataTable("blank_test_errors");
        public DataTable resultTable = new DataTable("blank_test_result");
        private bool no_blank;


        private DataRow row;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        public List<bool> selected_early_dribble = new List<bool>();
        public List<bool> selected_device_replace = new List<bool>();
        public List<int> selected_codes = new List<int>();
        public List<string> selected_rolls = new List<string>();
        public static List<DateTime> selected_end_dates = new List<DateTime>();
        public static List<DateTime> selected_start_dates = new List<DateTime>();
        public static List<string> selected_computers = new List<string>();
        public static List<string> selected_users = new List<string>();
        public static List<int> selected_fk_errors_id = new List<int>();
        public static List<double> selected_temp = new List<double>();
        public static List<double> selected_nano_ampers = new List<double>();
        public static List<double> selected_glus = new List<double>();
        public static List<string> selected_serial_numbers = new List<string>();
        public static List<int> selected_result_ids = new List<int>();
        public static List<string> selected_error = new List<string>();
        public static List<string> selected_error_text = new List<string>();
        public static List<bool> selected_not_h62 = new List<bool>();
        public static List<bool> selected_h62 = new List<bool>();

        #endregion


        #region Constructors
        public BlankFinishedResultsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Main constructor to add two datagrid and the charts
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="roll"></param>
        public BlankFinishedResultsView(string lot, string roll)
        {
            InitializeComponent();

            lotid = lot;
            rollid = roll;

            blankResultQuery = string.Format("Select lot_id,roll_id,blank_is_valid,tube_count_in_one_roll,date,avg From blank_test_averages where lot_id='{0}' and roll_id='{1}' and invalidate=False", lotid, rollid);

            this.AutoSize = false;
            this.WindowState = FormWindowState.Maximized;
            this.Text = string.Format("Blank Test Result of the {0} roll from the {1} lot", rollid, lotid);

            this.Controls["tabControl1"].Controls["tabPage1"].Controls.Add(this.dataGridView1);
            this.Controls["tabControl1"].Controls["tabPage2"].Controls.Add(dataGridViewAll);
            SetCloseButton();

            CollectAllData(lotid, rollid);
            InitializeDataGridView();

            this.Shown += new EventHandler(BlankFinishedResultsView_Shown);
            this.FormClosed += new FormClosedEventHandler(BlankFinishedResultsView_FormClosed);
        }

        private void SetCloseButton()
        {
            Button btClose = new Button();
            btClose.Location = new System.Drawing.Point(Screen.PrimaryScreen.WorkingArea.Width-250, 235);
            btClose.Name = "btExport";
            btClose.Size = new System.Drawing.Size(120, 50);
            btClose.TabIndex = 0;
            btClose.Text = "Bezár";
            btClose.UseVisualStyleBackColor = true;
            btClose.Click += new EventHandler(btClose_Click);
            this.Controls["tabControl1"].Controls["tabPage1"].Controls.Add(btClose); 

        }

        void btClose_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        } 
        #endregion
   
        #region EventHandlers

       void BlankFinishedResultsView_FormClosed(object sender, FormClosedEventArgs e)
       {
           Environment.Exit(Environment.ExitCode);
       }

       /// <summary>
       /// Export all data from the dgvs (2) after the form is showed
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
       void BlankFinishedResultsView_Shown(object sender, EventArgs e)
       {
           Export();
       } 
       #endregion

       #region Methods
       /// <summary>
       /// This helps to export the datagridview data to csv
       /// </summary>
       private void Export()
       {
           new ExportToCSV(dataGridView1, dataGridViewAll, string.Format("blank_{0}_{1}", lotid, rollid));
       }


       /// <summary>
       /// Create a chart about blank test average alternation
       /// </summary>
       public void CreateAlternationChart()
       {
           string query = string.Format("select date,avg from  blank_test_averages_alternation where lot_id='{0}' and roll_id='{1}' and invalidate=False",lotid,rollid);

           DataTable table = GetData(query);
           chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();

           ChartArea area = new ChartArea();
           Series series = new Series("Blank teszt átlagának alakulása");
           chart1.ChartAreas.Add(area);
           chart1.Series.Add(series);
           chart1.Series[series.Name].ChartType = SeriesChartType.Spline;
           chart1.Series[series.Name].XValueMember = "date";
           chart1.Series[series.Name].XValueMember = "avg";
           Label x_label = new Label();
           BindingSource bindingSrc = new BindingSource();
           DataGridView dgv = new DataGridView();
           dgv.DataSource = table;

           chart1.Dock = DockStyle.Bottom;
           for (int i = 0; i < table.Rows.Count; i++)
           {
               Trace.TraceInformation("X axis values: {0}", table.Rows[i].Field<DateTime>("date"));
               chart1.Series[series.Name].Points.AddXY(Convert.ToString(table.Rows[i].Field<DateTime>("date")), table.Rows[i].ItemArray[1]);

           }
           for (int i = 0; i < chart1.Series[series.Name].Label.Length; i++)
           {
               chart1.Series[series.Name].IsValueShownAsLabel = true;
           }

           chart1.ChartAreas[0].AxisX.Title = "Idő";
           chart1.ChartAreas[0].AxisY.Title = "Átlag(nano amper)";
           chart1.Series[series.Name].MarkerStyle = MarkerStyle.Triangle;

           chart1.ChartAreas[0].Axes[0].IntervalOffsetType = DateTimeIntervalType.NotSet;
           chart1.Series[series.Name].AxisLabel = series1.YValuesPerPoint.ToString();
           chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

           this.Controls["tabControl1"].Controls["tabPage2"].Controls.Add(chart1);
       }

       /// <summary>
       /// Create a dgv for the Result of the test
       /// </summary>
       private void InitializeDataGridView()
       {
           try
           {
               #region Init DatagridView

               dataGridView1.AutoGenerateColumns = true;
               dataGridView1.AllowUserToOrderColumns = true;
               dataGridView1.AllowUserToResizeColumns = true;
               dataGridView1.AllowUserToResizeRows = true;
               dataGridView1.AllowDrop = false;
               #endregion

               #region Binding the database values to the dgv
               bindingSource1.DataSource = GetData(blankResultQuery);
               dataGridView1.DataSource = bindingSource1;
               #endregion

               DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
               column.HeaderText = "Értékelés";
               column.CellTemplate = new DataGridViewTextBoxCell();
               column.ValueType = typeof(string);
               table.Columns.Add(column.Name);

               #region Set display of the dgv
               dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
               dataGridView1.DefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
               DataGridViewColumnCollection colmCollection = new DataGridViewColumnCollection(dataGridView1);
               dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
               dataGridView1.RowHeadersVisible = false;
               dataGridView1.AllowUserToAddRows = false;
               dataGridView1.AutoSize = false;
               dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
               dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
               dataGridView1.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.InsetDouble;
               dataGridView1.BorderStyle = BorderStyle.Fixed3D;
               dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;

               for (int i = 0; i < dataGridView1.Columns.Count; i++)
               {
                   dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
               }
               for (int i = 0; i < dataGridView1.Rows.Count; i++)
               {
                   dataGridView1.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
               }
               #endregion


               #region Set size Of DatagridView
               AllWidth = dataGridView1.RowHeadersWidth;

             // dataGridView1.Height = dataGridView1.Rows[0].Height + dataGridView1.ColumnHeadersHeight + 20;
               for (int i = 0; i < dataGridView1.Columns.Count; i++)
               {
                   AllWidth += dataGridView1.Columns[i].Width;
               }
               dataGridView1.Width = AllWidth + 20;

               //     dataGridView1.Dock = DockStyle.Top;
               dataGridView1.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - dataGridView1.Width) / 2, 0);

               #endregion

               #region Rename Columns & RePaint Rows

               dataGridView1.Columns["avg"].HeaderText = "Átlag";
               dataGridView1.Columns["roll_id"].HeaderText = "Roll Azonosító";
               dataGridView1.Columns["tube_count_in_one_roll"].HeaderText = "Lemért tubusszám";
               dataGridView1.Columns["date"].HeaderText = "Teszt időpontja";
               dataGridView1.Columns["lot_id"].HeaderText = "Lot azonosító";

               if ((double)dataGridView1.Rows[0].Cells["avg"].Value >= (double)13
                   && (double)dataGridView1.Rows[0].Cells["avg"].Value <= (double)51)
               {
                   dataGridView1.DefaultCellStyle.BackColor = Color.Green;

               }
               else
               {
                   dataGridView1.DefaultCellStyle.BackColor = Color.Red;
               }


               #endregion

               if (Convert.ToBoolean(dataGridView1.Rows[0].Cells["blank_is_valid"].Value))
               {
                   resultString = "Megfelelő";
               }
               else
               {
                   resultString = "Nem Megfelelő";

               }
               dataGridView1.Columns[6].HeaderText = "Blank Teszt Értékelés";
               dataGridView1.Rows[0].Cells[6].Value = resultString;
               dataGridView1.Columns["blank_is_valid"].Visible = false;

               CreateAlternationChart();
               CreateChart();

           }
           catch (SqlException ex)
           {
               Trace.TraceError(string.Format("Exception: {0}", ex.StackTrace));
               System.Threading.Thread.CurrentThread.Abort();
           }

       }


       /// <summary>
       /// Creates a chart for all measured strip in this measurement x_axis-tubeIDs;y_axis:nano_amper
       /// </summary>
       /// <param name="selected_lot_id"></param>
       /// <param name="selected_roll_id"></param>
       public void CollectAllData(string selected_lot_id, string selected_roll_id)
       {
           using (NpgsqlConnection conn = new NpgsqlConnection(Properties.Settings.Default.DBReleaseConnection))
           {
               try
               {
                   conn.Open();

                   ///Get values for blank results
                   #region get values from identify table where lot,roll, measuretype is correct(result_id,serial_number)
                   using (NpgsqlCommand getInvalidValuesfromIdentify =
                      new NpgsqlCommand(
                          string.Format("select pk_id from blank_test_identify where invalidate=false and measure_type='blank' and lot_id='{0}' and roll_id='{1}'", selected_lot_id, selected_roll_id), conn))
                   {
                       object res = null;
                       res = getInvalidValuesfromIdentify.ExecuteScalar();

                       if (res == DBNull.Value
                           || res == null)
                       {
                           Trace.TraceWarning("No rows are validated in identify, query:{0}", getInvalidValuesfromIdentify.CommandText);
                           no_blank = true;
                           Trace.TraceWarning("No value for blank test; query: {0}", getInvalidValuesfromIdentify.CommandText);
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

                                       Trace.TraceWarning("No value for blank test; query: {0}", getValuesfromIdentify.CommandText);
                                       dr.Close();
                                   }

                               }

                           }
                       }


                   }

                   #endregion

                   #region get values from all table where selected test result can be found for BLANK test

                   if (!no_blank)
                   {


                       foreach (int act_result_id in selected_result_ids)
                       {



                           using (NpgsqlCommand getValuesfromResults =
                                     new NpgsqlCommand(
                                         string.Format("select * from blank_test_result where code=170 and glu<>0 and invalidate=false and remeasured=false and pk_id={0}", act_result_id), conn))
                           {


                               NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                               adapter.SelectCommand = getValuesfromResults;

                               try
                               {
                                   /*            
                                              tableAll.BeginInit();
                                              tableAll.BeginLoadData();
                                              tableAll.Load( getValuesfromResults.ExecuteReader());
                                              tableAll.AcceptChanges(); 
                                              tableAll.EndLoadData();
                                              tableAll.EndInit();
                                     */
                               }
                               catch (Exception ex)
                               {

                                   throw;
                               }

                               using (NpgsqlDataReader dr = getValuesfromResults.ExecuteReader())
                               {
                                   if (dr.HasRows)
                                   {
                                       while (dr.Read())
                                       {
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



                   }//End of If


                   using (NpgsqlCommand getValuesfromEnv =
                        new NpgsqlCommand(
                   string.Format("select * FROM blank_test_environment LEFT JOIN blank_test_result ON blank_test_environment.fk_blank_test_result_id = blank_test_result.pk_id WHERE blank_test_environment.lot_id='{0}' and blank_test_environment.roll_id='{1}' and blank_test_result.glu<>0 and blank_test_result.code=170 ", selected_lot_id, selected_roll_id), conn))
                   {


                       NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                       adapter.SelectCommand = getValuesfromEnv;


                       DataTable envTable = new DataTable("blank_test_environment");
                       adapter.Fill(envTable);
                       NpgsqlDataReader dar = getValuesfromEnv.ExecuteReader();

                       tableAll.BeginInit();
                       tableAll.BeginLoadData();
                       tableAll.Load(dar);//.Read();// (envTable);
                       tableAll.AcceptChanges();
                       tableAll.EndLoadData();
                       tableAll.EndInit();
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



                   #endregion



               }
               catch (Exception ex)
               {
                   Trace.TraceError("Exception in Get blank test results for show all:BlankFinishedResultsView.CollectAllData(),ex:{0}", ex);
                   throw;
               }
               finally
               {
                   conn.Close();
               }


               #region Init DatagridView
               dataGridViewAll.AutoSize = false;
               dataGridViewAll.AutoGenerateColumns = true;
               dataGridViewAll.AllowUserToOrderColumns = true;
               dataGridViewAll.AllowUserToResizeColumns = true;
               dataGridViewAll.AllowUserToResizeRows = true;
               dataGridViewAll.AllowDrop = false;
               dataGridViewAll.Dock = DockStyle.Top;
               dataGridViewAll.LayoutEngine.Layout(this, new LayoutEventArgs(dataGridViewAll, "Size"));// Manual;

               #endregion

               #region Binding the database values to the dgv

               dataGridViewAll.DataSource = tableAll;
               dataGridViewAll.AutoScrollOffset = new Point(dataGridViewAll.VerticalScrollingOffset, dataGridViewAll.HorizontalScrollingOffset);

               #endregion

               #region Set size Of DatagridView

               AllWidth = 0;
               int AllHeight = dataGridViewAll.ColumnHeadersHeight;
               for (int i = 0; i < dataGridViewAll.RowCount; i++)
               {
                   AllHeight += dataGridViewAll.Rows[i].Height;
               }

               for (int i = 0; i < dataGridViewAll.Columns.Count - 1; i++)
               {

                   AllWidth += dataGridViewAll.Columns[i].Width + dataGridViewAll.RowHeadersWidth;// Cells[i].Size.Width;
               }
               dataGridViewAll.Height = AllHeight;
               dataGridViewAll.Width = AllWidth;
               dataGridViewAll.Location = new Point(0, Screen.PrimaryScreen.WorkingArea.Height / 2 - 80);
               //  dataGridViewAll.Dock = DockStyle.Left;// DockStyle.Right;

               #endregion



               #region Set display of the dgv
               dataGridViewAll.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
               dataGridViewAll.DefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
               DataGridViewColumnCollection colmCollection = new DataGridViewColumnCollection(dataGridView1);
               dataGridViewAll.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
               dataGridViewAll.RowHeadersVisible = false;
               dataGridViewAll.AllowUserToAddRows = false;

               dataGridViewAll.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
               dataGridViewAll.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
               dataGridViewAll.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.InsetDouble;
               dataGridViewAll.BorderStyle = BorderStyle.Fixed3D;
               dataGridViewAll.EditMode = DataGridViewEditMode.EditProgrammatically;



               #endregion
               #region Rename Columns & RePaint Rows
               dataGridViewAll.Columns["remeasured"].HeaderText = "Első mérés";
               dataGridViewAll.Columns["end_date"].HeaderText = "Mérés vége";
               dataGridViewAll.Columns["start_date"].HeaderText = "Mérés kezdete";
               dataGridViewAll.Columns["temperature"].HeaderText = "Hőmérsélet";
               dataGridViewAll.Columns["computer_name"].HeaderText = "Számítógép";
               dataGridViewAll.Columns["user_name"].HeaderText = "Felhasználó";

               dataGridViewAll.Columns["code"].HeaderText = "Használt kóddugó";
               dataGridViewAll.Columns["nano_amper"].HeaderText = "Nano Amper";
               dataGridViewAll.Columns["roll_id"].HeaderText = "Roll azonosító";
               dataGridViewAll.Columns["sn"].HeaderText = "Tubus azonosító";
               dataGridViewAll.Columns["lot_id"].HeaderText = "Lot azonosító";


               dataGridViewAll.Columns["pk_id1"].Visible = false;
               dataGridViewAll.Columns["invalidate1"].Visible = false;
               dataGridViewAll.Columns["remeasured1"].Visible = false;
               dataGridViewAll.Columns["roll_id1"].Visible = false;
               dataGridViewAll.Columns["sn1"].Visible = false;
               dataGridViewAll.Columns["lot_id1"].Visible = false;
               //    dataGridViewAll.Columns["measure_type"].Visible = false;
               dataGridViewAll.Columns["fk_blank_test_result_id"].Visible = false;
               //       dataGridViewAll.Columns["fk_blank_test_identify_id"].Visible = false;
               dataGridViewAll.Columns["glu"].Visible = false;
               dataGridViewAll.Columns["measure_id"].Visible = false;
               dataGridViewAll.Columns["serial_number"].Visible = false;
               dataGridViewAll.Columns["is_check_strip"].Visible = false;
               dataGridViewAll.Columns["fk_blank_test_errors_id"].Visible = false;
               dataGridViewAll.Columns["master_lot"].Visible = false;
               dataGridViewAll.Columns["invalidate"].Visible = false;
               dataGridViewAll.Columns["pk_id"].Visible = false;
               dataGridViewAll.Columns["glu"].Visible = false;

               DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
               cellStyle.BackColor = Color.Blue;
               cellStyle.Alignment = DataGridViewContentAlignment.BottomRight;
               for (int i = 0; i < dataGridViewAll.Rows.Count; i++)
               {
                   if ((double)dataGridViewAll.Rows[i].Cells["nano_amper"].Value >= (double)13
                   && (double)dataGridViewAll.Rows[i].Cells["nano_amper"].Value <= (double)51)
                   {
                       this.dataGridViewAll.Rows[i].DefaultCellStyle.BackColor = Color.Green;

                   }
                   else
                   {

                       tableAll.Rows[i].BeginEdit();
                       dataGridViewAll.Rows[i].Selected = true;

                       cellStyle.BackColor = Color.Red;
                       dataGridViewAll.Rows[i].DefaultCellStyle = cellStyle;
                       dataGridViewAll.Rows[i].DefaultCellStyle.BackColor = Color.SeaGreen;
                       dataGridViewAll.Rows[i].Selected = true;
                       dataGridViewAll.Rows[i].DefaultCellStyle.SelectionBackColor = Color.PeachPuff;
                       //   this.dataGridViewAll.RowsDefaultCellStyle.BackColor = Color.Red;
                       this.dataGridViewAll.Rows[i].InheritedStyle.BackColor = Color.Red;
                       dataGridViewAll.EndEdit();//RowChanged
                       tableAll.Rows[i].AcceptChanges();
                   }
                   dataGridViewAll.Rows[i].DefaultCellStyle.ApplyStyle(dataGridViewAll.Rows[i].DefaultCellStyle);
               }

               dataGridViewAll.Select();

               dataGridViewAll.Refresh();

               dataGridViewAll.Update();
               dataGridViewAll.PerformLayout(dataGridViewAll, "BackColor");
               dataGridViewAll.SuspendLayout();

               #endregion




           }///End of using NpgsqlConnection for blank

           ///Creating chart about all measured item
           CreateAllDataChart();

       }

       /// <summary>
       /// Create a chart with TubeIDs on X axis and nano_amper value on y axis
       /// </summary>
       private void CreateAllDataChart()
       {
           chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();


           ChartArea area = new ChartArea("ChartArea");
           area.AxisX.Name = "Blank Tests";
           area.AxisY.Name = "Average of all the strip values in the measurement";

           area.AlignmentStyle = AreaAlignmentStyles.All;
           chart2.ChartAreas.Add(area);
           chart2.Dock = DockStyle.Bottom;
           chart2.Series.Clear();
           series1 = new Series("Blank Test Averages");
           series1.ChartType = SeriesChartType.Line;
           series1.Font = new Font("Arial Black", 10, FontStyle.Bold);

           chart2.Series.Add(series1.Name);
           chart2.ChartAreas[0].BackColor = Color.LightYellow;
           chart2.Series[series1.Name].Points.Clear();
           chart2.Series[series1.Name].Font = new Font("Arial Black", 10, FontStyle.Bold);

           for (int i = 0; i < dataGridViewAll.Rows.Count; i++)
           {
               chart2.Series[series1.Name].Points.AddXY(dataGridViewAll.Columns["sn"].HeaderText + ":" + dataGridViewAll.Rows[i].Cells["sn"].Value, dataGridViewAll.Rows[i].Cells["nano_amper"].Value);
           }

           chart2.ChartAreas[0].AxisX.Name = "Blank Test Results";
           chart2.ChartAreas[0].AxisY.Name = "Nano Amper";

           chart2.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
           chart2.ChartAreas[0].IsSameFontSizeForAllAxes = true;
           chart2.ChartAreas[0].AlignWithChartArea = "ChartArea";
           chart2.ChartAreas[0].AxisX.Title = string.Format("Csíkok a {0} azonosítójú Roll -ból (LotID:{1})", rollid, lotid);
           chart2.ChartAreas[0].AxisY.Title = "Nano Amper";
           chart2.ChartAreas[0].AxisX.Interval = 10;
           chart2.ChartAreas[0].AxisX.IsMarksNextToAxis = true;
           chart2.ChartAreas[0].AxisX.ScaleView.Size = 10;
           chart2.ChartAreas[0].Axes[0].ScaleView = new AxisScaleView();
           chart2.ChartAreas[0].Axes[1].ScaleView = new AxisScaleView();

           chart2.ChartAreas[0].AxisY.LineColor = Color.DarkBlue;
           chart2.ChartAreas[0].AxisX.LineColor = Color.DarkBlue;

           chart2.ChartAreas[0].AxisY.Maximum = 80;
           chart2.ChartAreas[0].AxisY.StripLines.Add(new StripLine()); ;

           chart2.ChartAreas[0].Axes[1].InterlacedColor = Color.Red;
           chart2.ChartAreas[0].Axes[1].Interval = 38;
           chart2.ChartAreas[0].Axes[1].IntervalOffset = 13;

           StripLine line1 = new StripLine();
           line1.TextLineAlignment = StringAlignment.Center;
           line1.ForeColor = Color.Red;

           chart2.ChartAreas[0].Axes[1].StripLines.Add(line1);
           chart2.ChartAreas[0].Axes[1].StripLines[0].ForeColor = Color.Red;
           this.Controls["tabControl1"].Controls["tabPage1"].Controls.Add(chart2);
       }

       private void CreateChart()
       {
           chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
           ChartArea area = new ChartArea("ChartArea");
           area.AxisX.Name = "Blank Current Teszt";
           area.AxisY.Name = "A lemért csíkok összátlagának alakulása a teszt során";

           area.AlignmentStyle = AreaAlignmentStyles.All;
           chart1.ChartAreas.Add(area);
           chart1.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2, dataGridView1.Bottom);
           chart1.Series.Clear();

           series1 = new Series("Blank Teszt Átlag");
           series1.ChartType = SeriesChartType.Line;
           series1.Font = new Font("Arial Black", 10, FontStyle.Bold);

           chart1.Series.Add(series1.Name);

           chart1.Series[series1.Name].Points.Clear();
           chart1.Series[series1.Name].Font = new Font("Arial Black", 10, FontStyle.Bold);
           chart1.Series[series1.Name].Points.AddXY(dataGridView1.Columns[5].HeaderText + ":" + dataGridView1.Rows[0].Cells[5].Value, dataGridView1.Rows[0].Cells[5].Value);

           chart1.ChartAreas[0].AxisX.Name = "Blank Teszt Eredmény";
           chart1.ChartAreas[0].AxisY.Name = "Átlag (Nano Amper)";


           chart1.ChartAreas[0].AlignWithChartArea = "ChartArea";
           chart1.ChartAreas[0].Area3DStyle = new ChartArea3DStyle(chart1.ChartAreas[0]);
           chart1.ChartAreas[0].AxisX.Title = "Blank Teszt Eredmény";
           chart1.ChartAreas[0].AxisY.Title = "Átlag (Nano Amper)";
           chart1.ChartAreas[0].Axes[0].ScaleView = new AxisScaleView();
           chart1.ChartAreas[0].Axes[1].ScaleView = new AxisScaleView();

           chart1.ChartAreas[0].AxisY.LineColor = Color.DarkBlue;
           chart1.ChartAreas[0].AxisX.LineColor = Color.DarkBlue;

           chart1.ChartAreas[0].AxisY.Maximum = 80;
           chart1.ChartAreas[0].AxisY.StripLines.Add(new StripLine()); ;

           // Set interval of X axis to 1 week, with an offset of 1 day
           chart1.ChartAreas[0].AxisX.Interval = 1;
           chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Weeks;
           chart1.ChartAreas[0].AxisX.IntervalOffset = 1;
           chart1.ChartAreas[0].AxisX.IntervalOffsetType = DateTimeIntervalType.Days;

           chart1.ChartAreas[0].Axes[1].IntervalType = DateTimeIntervalType.Number;


           chart1.ChartAreas[0].Axes[1].InterlacedColor = Color.Red;
           chart1.ChartAreas[0].Axes[1].Interval = 38;
           chart1.ChartAreas[0].Axes[1].IntervalOffset = 13;

           StripLine line1 = new StripLine();
           line1.TextLineAlignment = StringAlignment.Center;
           line1.ForeColor = Color.Red;

           chart1.ChartAreas[0].Axes[1].StripLines.Add(line1);
           chart1.ChartAreas[0].Axes[1].StripLines[0].ForeColor = Color.Red;


           // Enable 3D charts
           /* 
                 chart1.ChartAreas[0].Area3DStyle.Enable3D = true;
                 // Show a 30% perspective
                 chart1.ChartAreas[0].Area3DStyle.Perspective = 30;
                 // Set the X Angle to 30
                 chart1.ChartAreas[0].Area3DStyle.Inclination = 30;
                 // Set the Y Angle to 40
                 chart1.ChartAreas[0].Area3DStyle.Rotation = 40;
     */
           this.Controls["tabControl1"].Controls["tabPage1"].Controls.Add(chart1);
       }

       /// <summary>
       /// Returns the datatable 
       /// </summary>
       /// <param name="sqlCommand"></param>
       /// <returns></returns>
       public DataTable GetData(string sqlCommand)
       {

           using (NpgsqlConnection northwindConnection = new NpgsqlConnection(Properties.Settings.Default.DBReleaseConnection))
           {
               table = new DataTable();
               NpgsqlCommand command = new NpgsqlCommand(sqlCommand, northwindConnection);
               NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
               adapter.SelectCommand = command;

               table.Locale = System.Globalization.CultureInfo.InvariantCulture;
               adapter.Fill(table);

           }
           return table;
       } 
       #endregion
    }
}
