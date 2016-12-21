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
    public partial class HomogenityResultsView : Form
    {

        #region Variables

        public  List<double> homogenity_stddev = new List<double>();
        public  List<bool> homogenity_is_valid = new List<bool>();
        public  List<double> homogenity_avg = new List<double>();
        public  List<double> homogenity_cv = new List<double>();
        public  List<DateTime> homogenity_date = new List<DateTime>();

        public  List<int> homogenity_strip_count = new List<int>();
        public  List<bool> homogenity_strip_count_is_valid = new List<bool>();
        public  List<int> homogenity_out_of_range_strip_count = new List<int>();
        public  List<string> homogenity_h62 = new List<string>();
        public  List<string> homogenity_not_h62 = new List<string>();
        public  List<string> homogenity_error = new List<string>();
        public  List<string> homogenity_error_text = new List<string>();
        public  List<string> homogenity_users = new List<string>();
        public  List<string> homogenity_computers = new List<string>();
        public  List<string> homogenity_sn = new List<string>();
        public  List<bool> homogenity_earlyDribble = new List<bool>();
        public  List<bool> homogenity_deviceReplace = new List<bool>();
        public  List<double> homogenity_glus = new List<double>();
        public  List<bool> homogenity_h62_value = new List<bool>();
        public  List<bool> homogenity_not_h62_value = new List<bool>();
        public  List<double> homogenity_temp = new List<double>();
        public  List<DateTime> homogenity_start_dates = new List<DateTime>();
        public  List<DateTime> homogenity_end_dates = new List<DateTime>();
        public  List<int> homogenity_h62_count = new List<int>();
        public  List<int> homogenity_not_h62_count = new List<int>();
        public BindingSource bindingSourceAll = new BindingSource();
        public BindingSource bindingSource1 = new BindingSource();
        private int AllWidth = 0;
        public static string resultString;
        private Series series1;
        public string homogenityResultQuery;
        public string AllBlankResultQuery;
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

        private DataRow row;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        public List<bool> selected_early_dribble = new List<bool>();
        public List<bool> selected_device_replace = new List<bool>();
        public List<int> selected_codes = new List<int>();
        public List<string> selected_rolls = new List<string>();
        public  List<DateTime> selected_end_dates = new List<DateTime>();
        public  List<DateTime> selected_start_dates = new List<DateTime>();
        public  List<string> selected_computers = new List<string>();
        public  List<string> selected_users = new List<string>();
        public  List<int> selected_fk_errors_id = new List<int>();
        public  List<double> selected_temp = new List<double>();
        public  List<double> selected_nano_ampers = new List<double>();
        public  List<double> selected_glus = new List<double>();
        public  List<string> selected_serial_numbers = new List<string>();
        public  List<int> selected_result_ids = new List<int>();
        public  List<string> selected_error = new List<string>();
        public  List<string> selected_error_text = new List<string>();
        public  List<bool> selected_not_h62 = new List<bool>();
        public  List<bool> selected_h62 = new List<bool>();

        public List<string> homogenity_lot_id = new List<string>();
        public List<string> homogenity_roll_id = new List<string>();
        public bool no_homogenity = false;
        public DataTable homogenityTable = new DataTable();
        private int AllHeight;
        #endregion


        public HomogenityResultsView()
        {
            InitializeComponent();
        }





        public HomogenityResultsView(string lot, string roll) 
        {
            InitializeComponent();
    
            lotid = lot;
            rollid = roll;

            homogenityResultQuery = string.Format("Select * From homogenity_result where lot_id='{0}' and roll_id='{1}' and invalidate=false", lotid, rollid);
            

            this.AutoSize = false;
            this.WindowState = FormWindowState.Maximized;
            this.Text = string.Format("Homogenity Teszt Eredménye a {0} azonosítójú roll a {1} lot-ból",rollid,lotid);

            SetExitButton();
          
           this.Controls["tabControl1"].Controls["tabPage1"].Controls.Add(this.dataGridView1);
           this.Controls["tabControl1"].Controls["tabPage1"].Controls.Add(btExit); 
           this.Controls["tabControl1"].Controls["tabPage2"].Controls.Add(dataGridViewAll);
           foreach (TabPage _Page in this.tabControl1.TabPages)
           {
               _Page.AutoScroll = true;
               _Page.AutoScrollMargin = new System.Drawing.Size(20, 20);
               _Page.AutoScrollMinSize = new System.Drawing.Size(_Page.Width, _Page.Height);
           }
            CollectAllData(lotid, rollid);
            InitializeDataGridView();

            
            this.Shown += new EventHandler(BlankFinishedResultsView_Shown);
            this.FormClosed += new FormClosedEventHandler(HomogenityResultsView_FormClosed);
        }
        public Button btExit;
        private void SetExitButton()
        {
           btExit = new Button();
            btExit.Text = "Program bezárása";
            btExit.Dock = DockStyle.Left;
            btExit.ForeColor = Color.Red;
            btExit.Font = new System.Drawing.Font("Georgia", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btExit.MouseClick += new MouseEventHandler(btExit_MouseClick);
        }

        void btExit_MouseClick(object sender, MouseEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }


        

        private void CollectAllData(string lotid, string rollid)
        {
            #region get homogenity results

            homogenity_lot_id = new List<string>();
            homogenity_roll_id = new List<string>();
            Trace.TraceInformation("Empty the roll_id and get all the rollid from homo_result");

            using (NpgsqlConnection connect = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    connect.Open();

                    #region check if there is a homogenity result for this lotid and rollid
                    using (NpgsqlCommand get_ids = new NpgsqlCommand(string.Format("select distinct lot_id,roll_id from homogenity_result where lot_id='{0}' and invalidate=false", lotid), connect))
                    {
                        using (NpgsqlDataReader dr = get_ids.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    homogenity_roll_id.Add(Convert.ToString(dr["roll_id"]));
                                    homogenity_lot_id.Add(Convert.ToString(dr["lot_id"]));

                                }
                                dr.Close();
                            }
                            else
                            {

                                dr.Close();
                                Trace.TraceInformation(string.Format("Nincs Homogenitás teszt eredmény a {0} Lot-nál", lotid));
                                no_homogenity = true;

                            }

                        }
                    }
                    #endregion

                    int i = 0;
                    #region there is homogenity result
                    if (!no_homogenity)
                    {
                        #region get rows which are invalidated and skip that rows in the query to collect data

                                using (NpgsqlCommand getValuesfromResults =
                                          new NpgsqlCommand(
                                              string.Format("select homogenity_test.lot_id,homogenity_test.roll_id,homogenity_test.strip_ok,homogenity_test.sn,blank_test_result.glu,blank_test_result.code,blank_test_errors.error_text,blank_test_errors.not_h62_error,blank_test_errors.early_dribble,blank_test_errors.device_replace,blank_test_errors.h62_error from homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE (blank_test_result.glu<>0 or blank_test_errors.error_text<>'') and homogenity_test.invalidate=False and blank_test_result.code=777 and blank_test_result.lot_id='{0}' and blank_test_result.roll_id='{1}'", lotid, rollid), connect))
                                {
                                           NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                                             adapter.SelectCommand = getValuesfromResults;

                                             try
                                             {
                                                 
                                                homogenityTable.BeginInit();
                                                homogenityTable.BeginLoadData();
                                                homogenityTable.Load( getValuesfromResults.ExecuteReader());
                                                homogenityTable.AcceptChanges(); 
                                                homogenityTable.EndLoadData();
                                                homogenityTable.EndInit();
                                             }
                                             catch (Exception ex)
                                             {

                                                 throw;
                                             }
                                
                            }                        
                    }

                        #endregion

                    #region Get Environment & results data
                    using (NpgsqlCommand getValuesfromEnv =
                                     new NpgsqlCommand(
                                string.Format("select * FROM blank_test_environment LEFT JOIN blank_test_result ON blank_test_environment.fk_blank_test_result_id = blank_test_result.pk_id WHERE blank_test_environment.lot_id='{0}'  and blank_test_environment.invalidate=false and blank_test_environment.roll_id='{1}' and blank_test_result.code=777 and blank_test_environment.invalidate=false", lotid, rollid), connect))
                                {
                                    using (NpgsqlDataReader dr = getValuesfromEnv.ExecuteReader())
                                    {
                                        if (dr.HasRows)
                                        {
                                            while (dr.Read())
                                            {
                                                homogenity_users.Add(Convert.ToString(dr["user_name"]));
                                                homogenity_computers.Add(Convert.ToString(dr["computer_name"]));
                                                homogenity_start_dates.Add(Convert.ToDateTime(dr["start_date"]));
                                                homogenity_end_dates.Add(Convert.ToDateTime(dr["end_date"]));
                                                homogenity_temp.Add(Convert.ToDouble(dr["temperature"]));

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
                    Trace.TraceError("Exception in Get homogenity test results for show all:HomogenityResultsView(),ex:{0}", ex);
                    throw;
                }
                finally
                {
                    connect.Close();
                }

                    #endregion
            }//NpgsqlConnection for homogenity                 
            #endregion

            CreateAllDatatGridView();

            Trace.TraceInformation("data collection finished... results form will be displayed ");
        }

        private void CreateAllDatatGridView()
        {
            #region Init DatagridView

            dataGridViewAll.AutoGenerateColumns = true;
            dataGridViewAll.AllowUserToOrderColumns = true;
            dataGridViewAll.AllowUserToResizeColumns = true;
            dataGridViewAll.AllowUserToResizeRows = true;
            dataGridViewAll.AllowDrop = false;
            #endregion

            #region Binding the database values to the dgv

            dataGridViewAll.DataSource = homogenityTable;
            dataGridViewAll.AutoScrollOffset = new Point(dataGridViewAll.VerticalScrollingOffset, dataGridViewAll.HorizontalScrollingOffset);

              
            #endregion

            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.HeaderText = "Értékelés";
            column.CellTemplate = new DataGridViewTextBoxCell();
            column.ValueType = typeof(string);

            homogenityTable.Columns.Add(column.Name);

            #region Set display of the dgv
            dataGridViewAll.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewAll.DefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            DataGridViewColumnCollection colmCollection = new DataGridViewColumnCollection(dataGridViewAll);
            dataGridViewAll.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            dataGridViewAll.RowHeadersVisible = false;
            dataGridViewAll.AllowUserToAddRows = false;
            dataGridViewAll.AutoSize = false;
            dataGridViewAll.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewAll.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewAll.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.InsetDouble;
            dataGridViewAll.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewAll.EditMode = DataGridViewEditMode.EditProgrammatically;

            for (int i = 0; i < dataGridViewAll.Columns.Count; i++)
            {
                dataGridViewAll.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            }
            for (int i = 0; i < dataGridViewAll.Rows.Count; i++)
            {
                dataGridViewAll.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            }
            #endregion


            #region Set size Of DatagridView
            AllWidth = dataGridViewAll.RowHeadersWidth;

            AllHeight =dataGridViewAll.ColumnHeadersHeight + 20;

            for (int i = 0; i < dataGridViewAll.Rows.Count; i++)
            {
                AllHeight += dataGridViewAll.Rows[i].Height;
            }
            for (int i = 0; i < dataGridViewAll.Columns.Count; i++)
            {
                AllWidth += dataGridViewAll.Columns[i].Width;
            }
            dataGridViewAll.Width = AllWidth + 20;
            dataGridViewAll.Height = AllHeight;
           dataGridViewAll.Dock = DockStyle.Top;
            //   dataGridViewAll.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width-dataGridViewAll.Width)/2, 0);

            #endregion

            #region Rename Columns & RePaint Rows

            dataGridViewAll.Columns["code"].HeaderText = "Használt kóddugó";
            dataGridViewAll.Columns["roll_id"].HeaderText = "Roll Azonosító";
            dataGridViewAll.Columns["sn"].HeaderText = "Tubus azonosító";
           // dataGridViewAll.Columns["date"].HeaderText = "Teszt időpontja";
            dataGridViewAll.Columns["lot_id"].HeaderText = "Lot azonosító";
            dataGridViewAll.Columns["glu"].HeaderText = "Glükóz érték";
        
            dataGridViewAll.Columns["not_h62_error"].HeaderText = "Nem H62-es hiba történt";
            dataGridViewAll.Columns["h62_error"].HeaderText = "H62-es hiba történt";
            dataGridViewAll.Columns["early_dribble"].HeaderText = "Korai cseppentés történt";
            dataGridViewAll.Columns["device_replace"].HeaderText = "Készülékcsere történt";
            //dataGridViewAll.Columns["error"].HeaderText = "Hibakód";
            dataGridViewAll.Columns["error_text"].HeaderText = "Hibaleírás";


       /*     if (Convert.ToBoolean(dataGridViewAll.Rows[0].Cells["homogenity_is_valid"].Value))
            {
                dataGridViewAll.DefaultCellStyle.BackColor = Color.Green;
            }
            else
            {
                dataGridViewAll.DefaultCellStyle.BackColor = Color.Red;
            }*/

            #endregion

            if ((Convert.ToDouble(dataGridViewAll.Rows[0].Cells["glu"].Value) >= 5.4)
                &&(Convert.ToDouble(dataGridViewAll.Rows[0].Cells["glu"].Value)  <= 7.3))
            {
                resultString = "Megfelelő";
            }
            else
            {
                resultString = "Nem Megfelelő";


            }
            dataGridViewAll.ScrollBars = ScrollBars.Vertical;
     
            #region Set visibility of columns
            dataGridViewAll.Columns[dataGridViewAll.Columns.Count - 1].HeaderText = "Homogenity Teszt Értékelés";
            dataGridViewAll.Rows[0].Cells[dataGridViewAll.Columns.Count - 1].Value = resultString;
            //dataGridViewAll.Columns["invalidate"].Visible = false;
            //dataGridViewAll.Columns["pk_id1"].Visible = false;
            //dataGridViewAll.Columns["remeasured"].Visible = false;
            //dataGridViewAll.Columns["sn1"].Visible = false;
            //dataGridViewAll.Columns["fk_blank_test_errors_id"].Visible = false;
            //dataGridViewAll.Columns["invalidate1"].Visible = false;
            //dataGridViewAll.Columns["is_check_strip"].Visible = false;
            //dataGridViewAll.Columns["lot_id1"].Visible = false;
            //dataGridViewAll.Columns["master_lot"].Visible = false;
            //dataGridViewAll.Columns["measure_id"].Visible = false;
            //dataGridViewAll.Columns["nano_amper"].Visible = false;
            //dataGridViewAll.Columns["remeasured1"].Visible = false;
            //dataGridViewAll.Columns["roll_id1"].Visible = false;
            //dataGridViewAll.Columns["serial_number"].Visible = false; 
            #endregion

          //  CreateColumnChart();
            CreateTestChart();
        }
        public int stripNumber = 0;
    private void AddFakeData()
    {
       
        // add your points as normal
        int stripCount = 0;
     
        for (int k = 0; k <= dataGridViewAll.Rows.Count/4; k++)
        {
            if ((stripCount <= dataGridViewAll.Rows.Count)
                   && (stripNumber + 3 < dataGridViewAll.Rows.Count))
            {
                testChart.Series["Series1"].Points.AddXY(10, 10);
                testChart.Series["Series1"].Points[testChart.Series["Series1"].Points.Count - 1].IsEmpty = true;
            }

             stripCount++; 
               
             if ((stripCount <= dataGridViewAll.Rows.Count)
                    &&(stripNumber+3<dataGridViewAll.Rows.Count))
             {
                    testChart.Series["Series1"].Points.AddXY(stripCount, dataGridViewAll.Rows[stripNumber].Cells["glu"].Value);
                    testChart.Series["Series1"].Points.AddXY(stripCount,dataGridViewAll.Rows[stripNumber + 1].Cells["glu"].Value);
                    testChart.Series["Series1"].Points.AddXY(stripCount, dataGridViewAll.Rows[stripNumber + 2].Cells["glu"].Value);
                    testChart.Series["Series1"].Points.AddXY(stripCount, dataGridViewAll.Rows[stripNumber + 3].Cells["glu"].Value);
             }

            stripNumber = stripNumber + 4;   
        }
                  
       
    }

    private void ConfigureChartSettings()
    {
        //Set point chart type
        testChart.Series["Series1"].ChartType = SeriesChartType.Point;

        //Set the market size
        testChart.Series["Series1"].MarkerSize = 8;

        //Set the marker shape to a circle
        testChart.Series["Series1"].MarkerStyle = MarkerStyle.Diamond;

        //X and Y values are both between -10 and 10 so set the x and y axes accordingly
        testChart.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
        testChart.ChartAreas["ChartArea1"].AxisX.Maximum = dataGridViewAll.Rows.Count/4+1;
        testChart.ChartAreas["ChartArea1"].AxisY.Minimum = 4;
        testChart.ChartAreas["ChartArea1"].AxisY.Maximum = 9;

        //Set the titles of the X and Y axes
        testChart.ChartAreas["ChartArea1"].AxisX.Title = "Lemért csíkok";
        testChart.ChartAreas["ChartArea1"].AxisY.Title = "Glükóz érték";

        //Set the Intervals of the X and Y axes, 
        testChart.ChartAreas["ChartArea1"].AxisX.Interval = 1;
        testChart.ChartAreas["ChartArea1"].AxisY.Interval = 1;

        //Set the MajorGrid interval to 5.
        testChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = 0;
        testChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.Interval = 0;

        //Set the MinorGrid interval to 1.
        testChart.ChartAreas["ChartArea1"].AxisX.MinorGrid.Interval = 0;
        testChart.ChartAreas["ChartArea1"].AxisY.MinorGrid.Interval = 0;

        //Set the MinorGrid style to Dot so that it is less obstructive.
        testChart.ChartAreas["ChartArea1"].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
        testChart.ChartAreas["ChartArea1"].AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dot;

        //Enable the major and minor grids.
        testChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = true;
        testChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
        testChart.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
        testChart.ChartAreas["ChartArea1"].AxisY.MinorGrid.Enabled = false;
      

    }
    private double SelectionStart = double.NaN;
    private Point SelectionStartPoint = new Point();
    private void chart1_Click(object sender, System.EventArgs e)
    {
        // Set input focus to the chart control
        testChart.Focus();
      
        // Set the selection start variable to that of the current position
        SelectionStart = testChart.ChartAreas["ChartArea1"].CursorY.Position;
     
        SetView();
    }
        private DataPoint    selectedDataPoint = null;

    /// <summary>
    /// Mouse Down Event
    /// </summary>
    private void chart1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        // Call Hit Test Method
        HitTestResult hitResult = testChart.HitTest( e.X, e.Y );

        // Initialize currently selected data point
        selectedDataPoint = null;
        if( hitResult.ChartElementType == ChartElementType.DataPoint )
        {
            selectedDataPoint = (DataPoint)hitResult.Object;

            // Show point value as label
           // selectedDataPoint.IsValueShownAsLabel = true;

            // Set cursor shape
            testChart.Cursor = Cursors.SizeNS;
        }
    }

    /// <summary>
    /// Mouse Move Event
    /// </summary>
    private void chart1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        // Check if data point selected
        if(selectedDataPoint != null)
        {
            // Mouse coordinates should not be outside of the chart 
            int coordinate = e.Y;
            if(coordinate < 0)
                coordinate = 0;
            if(coordinate > testChart.Size.Height - 1)
                coordinate = testChart.Size.Height - 1;

            // Calculate new Y value from current cursor position
            double yValue = testChart.ChartAreas["ChartArea1"].AxisY.PixelPositionToValue(coordinate);
            yValue = Math.Min(yValue, testChart.ChartAreas["ChartArea1"].AxisY.Maximum);
            yValue = Math.Max(yValue, testChart.ChartAreas["ChartArea1"].AxisY.Minimum);

            // Update selected point Y value
            selectedDataPoint.YValues[0] = yValue;

            // Invalidate chart
            testChart.Invalidate();
        }
        else
        {
            // Set different shape of cursor over the data points
            HitTestResult hitResult = testChart.HitTest( e.X, e.Y );
            if( hitResult.ChartElementType == ChartElementType.DataPoint )
            {
                testChart.Cursor = Cursors.Hand;
            }
            else
            {
                testChart.Cursor = Cursors.Default;
            }
        }
    }

    /// <summary>
    /// Mouse Up Event
    /// </summary>
    private void chart1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        // Initialize currently selected data point
        if (selectedDataPoint != null)
        {
            // Hide point label
            selectedDataPoint.IsValueShownAsLabel = false;

            // reset selected object
            selectedDataPoint = null;

            // Invalidate chart
            testChart.Invalidate();

            // Reset cursor style
            testChart.Cursor = Cursors.Default;
        }
    }

    private void SetView()
    {
        // Keep the cursor from leaving the max and min axis points
        if (testChart.ChartAreas["ChartArea1"].CursorX.Position < 0)
            testChart.ChartAreas["ChartArea1"].CursorX.Position = 0;

        else if (testChart.ChartAreas["ChartArea1"].CursorX.Position > 75)
            testChart.ChartAreas["ChartArea1"].CursorX.Position = 75;
        // Keep the cursor from leaving the max and min axis points
        if (testChart.ChartAreas["ChartArea1"].CursorY.Position < 0)
            testChart.ChartAreas["ChartArea1"].CursorY.Position = 0;

        else if (testChart.ChartAreas["ChartArea1"].CursorY.Position > 8)
            testChart.ChartAreas["ChartArea1"].CursorX.Position = 7.5;


        // Move the view to keep the cursor visible
        if (testChart.ChartAreas["ChartArea1"].CursorX.Position < testChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Position)
            testChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Position = testChart.ChartAreas["ChartArea1"].CursorX.Position;

        else if ((testChart.ChartAreas["ChartArea1"].CursorX.Position >
            (testChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Position + testChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Size)))
        {
            testChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Position =
                (testChart.ChartAreas["ChartArea1"].CursorX.Position - testChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Size);
        }
        // Move the view to keep the cursor visible
        if (testChart.ChartAreas["ChartArea1"].CursorY.Position < testChart.ChartAreas["ChartArea1"].AxisY.ScaleView.Position)
            testChart.ChartAreas["ChartArea1"].AxisY.ScaleView.Position = testChart.ChartAreas["ChartArea1"].CursorY.Position;

        else if ((testChart.ChartAreas["ChartArea1"].CursorY.Position >
            (testChart.ChartAreas["ChartArea1"].AxisY.ScaleView.Position + testChart.ChartAreas["ChartArea1"].AxisY.ScaleView.Size)))
        {
            testChart.ChartAreas["ChartArea1"].AxisY.ScaleView.Position =
                (testChart.ChartAreas["ChartArea1"].CursorY.Position - testChart.ChartAreas["ChartArea1"].AxisY.ScaleView.Size);
        }
    }


        public System.Windows.Forms.DataVisualization.Charting.Chart testChart;
        private void CreateTestChart()
        {
           
            testChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            Series s = new Series("Series1");
            ChartArea c=new ChartArea("ChartArea1");
            testChart.ChartAreas.Add(c);
            testChart.Series.Add(s);
            s.YValuesPerPoint = 4;
            // Create a new random number generator
            // Set chart type
            testChart.Series["Series1"].ChartType = SeriesChartType.Bubble;

            // Set bubble shape
            testChart.Series["Series1"].MarkerStyle = MarkerStyle.Square;

            testChart.Series["Series1"].MarkerColor = Color.Green;
            // Set max bubble size
            testChart.Series["Series1"]["BubbleMaxSize"] = "20";

            // Show bubble size as point labels
            //testChart.Series["Series1"].IsValueShownAsLabel = true;
            testChart.Series["Series1"]["BubbleUseSizeForLabel"] = "true";

            // Set bubble size scale
            testChart.Series["Series1"]["BubbleScaleMin"] = "0";
            testChart.Series["Series1"]["BubbleScaleMax"] = "100";

            testChart.Series["Series1"].YValuesPerPoint = 4;

             ConfigureChartSettings();

             AddFakeData();
             testChart.Dock = DockStyle.Bottom;
             // Zoom into the X axis
          //   testChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(1, 3);
             testChart.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoom(1, 3);

             testChart.ChartAreas["ChartArea1"].CursorX.LineColor = Color.LightBlue;
             testChart.ChartAreas["ChartArea1"].CursorY.LineColor = Color.LightBlue;
             testChart.ChartAreas["ChartArea1"].CursorX.Interval = 0.5;
             testChart.ChartAreas["ChartArea1"].CursorY.Interval = 0.1;
             // Enable range selection and zooming end user interface
             testChart.ChartAreas["ChartArea1"].CursorX.IsUserEnabled = true;
             testChart.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;
             testChart.ChartAreas["ChartArea1"].CursorY.IsUserEnabled = true;
             testChart.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
             testChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = false;       
             testChart.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = true;
             testChart.ChartAreas["ChartArea1"].AxisX.ScrollBar.Enabled = false;
             testChart.ChartAreas["ChartArea1"].AxisX.ScrollBar.Enabled = true;
             testChart.ChartAreas["ChartArea1"].AxisY.ScrollBar.IsPositionedInside = true;
           
             // Set the minimum view size to 2 weeks
             testChart.ChartAreas["ChartArea1"].AxisX.ScaleView.MinSize = 1;
         //    testChart.ChartAreas["ChartArea1"].AxisY.ScaleView.MinSize = 3;
          //   Title chartTitle = new Title("Title");
        //     testChart.Titles.Add(chartTitle);
             testChart.ChartAreas["ChartArea1"].AxisY.ScaleView.ZoomReset();

            // testChart.Titles[0].Text = "Lemért értékek tubusonként:\n";
             this.Controls["tabControl1"].Controls["tabPage1"].Controls.Add(testChart);
             this.testChart.Click += new EventHandler(chart1_Click);
            this.testChart.AxisViewChanged+=new EventHandler<ViewEventArgs>(chart1_AxisViewChanged);
           // this.testChart.GetToolTipText += new EventHandler<ToolTipEventArgs>(testChart_GetToolTipText);
            this.testChart.PostPaint+=new EventHandler<ChartPaintEventArgs>(chart1_PostPaint);

            testChart.ChartAreas[0].BackColor = Color.LightCyan;
       //     this.testChart.MouseDown+=new MouseEventHandler(chart1_MouseDown);
        //    this.testChart.MouseUp+=new MouseEventHandler(chart1_MouseUp);
         //   this.testChart.MouseMove+=new MouseEventHandler(chart1_MouseMove);
        }

        void testChart_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            // Check selected chart element and set tooltip text
            switch (e.HitTestResult.ChartElementType)
            {
                case ChartElementType.Axis:
                    e.Text = e.HitTestResult.Axis.Name;
                    break;
                
                case ChartElementType.DataPoint:
                    e.Text = string.Format("Érték: {0}",e.HitTestResult.ChartArea.AxisY.GetPosition(e.Y));//HitTestResult.PointIndex.ToString();
                    break;
               
                
                case ChartElementType.Title:
                    e.Text = "Diagramm Cím";
                    break;
            }

        }

        private void chart1_PostPaint(object sender, System.Windows.Forms.DataVisualization.Charting.ChartPaintEventArgs e)
        {
            if (e.ChartElement is ChartArea)
            {

                ChartArea area = (ChartArea)e.ChartElement;

                double max;
                double min;
                double xMax;
                double xMin;

                // Get Graphics object from chart
                Graphics graph = e.ChartGraphics.Graphics;

                // Convert X and Y values to screen position
                DrawALineToChartArea(e, area, graph, 5.4, 5.4);
                DrawALineToChartArea(e, area, graph, 7.3, 7.3);
            }
        }

        private void DrawALineToChartArea(System.Windows.Forms.DataVisualization.Charting.ChartPaintEventArgs e, ChartArea area, Graphics graph, double upperLimit, double lowerLimit)
        {
            float pixelYMax = (float)e.ChartGraphics.GetPositionFromAxis(area.Name, AxisName.Y, upperLimit);
            float pixelXMax = (float)e.ChartGraphics.GetPositionFromAxis(area.Name, AxisName.X, testChart.Right);
            float pixelYMin = (float)e.ChartGraphics.GetPositionFromAxis(area.Name, AxisName.Y, lowerLimit);
            float pixelXMin = (float)e.ChartGraphics.GetPositionFromAxis(area.Name, AxisName.X, 0);

            PointF point1 = PointF.Empty;
            PointF point2 = PointF.Empty;

            // Set Maximum and minimum points
            point1.X = pixelXMax;
            point1.Y = pixelYMax;
            point2.X = pixelXMin;
            point2.Y = pixelYMin;

            // Convert relative coordinates to absolute coordinates.
            point1 = e.ChartGraphics.GetAbsolutePoint(point1);
            point2 = e.ChartGraphics.GetAbsolutePoint(point2);

            // Draw connection line
            graph.DrawLine(new Pen(Color.Red, 3), point1, point2);

        }
        private void chart1_AxisViewChanged(object sender, System.Windows.Forms.DataVisualization.Charting.ViewEventArgs e)
        {
            // Axis View is reseted
            if (double.IsNaN(e.NewPosition) && double.IsNaN(e.NewSize))
            {
                // Set Title
                SetTitle(0, 28);
            }
            // Axis view is active
            else
            {
                // Set Title
                SetTitle(e.NewPosition, e.NewSize);
            }

        }

        private void SetTitle(double position, double size)
        {
            // Set view position to the title
            //testChart.Titles[0].Text += position.ToString();

        }

        private void ProcessSelect(System.Windows.Forms.MouseEventArgs me)
        {
            // Process keyboard keys
            if (me.Button== MouseButtons.Left)
            {
                // Make sure the selection start value is assigned
                if (this.SelectionStart == double.NaN)
                    this.SelectionStart = testChart.ChartAreas["ChartArea1"].CursorX.Position;

                // Set the new cursor position 
                testChart.ChartAreas["ChartArea1"].CursorX.Position += testChart.ChartAreas["ChartArea1"].CursorX.Interval;
            }
            else if (me.Button == MouseButtons.Left)
            {
                // Make sure the selection start value is assigned
                if (this.SelectionStart == double.NaN)
                    this.SelectionStart = testChart.ChartAreas["ChartArea1"].CursorX.Position;

                // Set the new cursor position 
                testChart.ChartAreas["ChartArea1"].CursorX.Position -= testChart.ChartAreas["ChartArea1"].CursorX.Interval;
            }

            // If the cursor is outside the view, set the view
            // so that the cursor can be seen
            SetView();


            testChart.ChartAreas["ChartArea1"].CursorX.SelectionStart = this.SelectionStart;
            testChart.ChartAreas["ChartArea1"].CursorX.SelectionEnd = testChart.ChartAreas["ChartArea1"].CursorX.Position;
        }

      
     

        void HomogenityResultsView_FormClosed(object sender, FormClosedEventArgs e)
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

        /// <summary>
        /// This helps to export the datagridview data to csv
        /// </summary>
       private void  Export()
       {
           new ExportToCSV(dataGridView1,dataGridViewAll, string.Format("homogenity_{0}_{1}",lotid,rollid));
       }

        public void CreateAlternationChart()
        {
            string query = string.Format("select start_date,glu from  blank_test_result Left join blank_test_environment on blank_test_environment.fk_blank_test_result_id=blank_test_result.pk_id WHERE  blank_test_result.code=777 and blank_test_result.invalidate=false and blank_test_environment.lot_id='{0}' and blank_test_environment.roll_id='{1}'", lotid, rollid);

            DataTable table=GetData(query);
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();

            ChartArea area = new ChartArea();
            Series series = new Series("Homogenity teszt átlagának alakulása");

            chart1.ChartAreas.Add(area);
            chart1.Series.Add( series);
            chart1.Series[series.Name].ChartType=SeriesChartType.Spline;
            chart1.Series[series.Name].XValueMember="start_date";
            chart1.Series[series.Name].XValueMember = "glu";
            Label x_label=new Label();
            BindingSource bindingSrc=new BindingSource();         
            DataGridView dgv = new DataGridView();
            dgv.DataSource=table;
       
            chart1.Dock = DockStyle.Bottom;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                Trace.TraceInformation("X axis values: {0}", table.Rows[i].Field<DateTime>("start_date"));
                chart1.Series[series.Name].Points.AddXY(Convert.ToString(table.Rows[i].Field<DateTime>("start_date")), table.Rows[i].ItemArray[1]);
                       
            }
            for (int i = 0; i < chart1.Series[series.Name].Label.Length; i++)
            {
                chart1.Series[series.Name].IsValueShownAsLabel = true;
            }
            chart1.ChartAreas[0].AxisX.Title = "Mérés időpontja";
            chart1.ChartAreas[0].AxisY.Title = "Glükóz érték";
            chart1.Series[series.Name].MarkerStyle = MarkerStyle.Diamond;
            
            chart1.ChartAreas[0].Axes[0].IntervalOffsetType = DateTimeIntervalType.NotSet;
            chart1.Series[series.Name].AxisLabel = series.YValuesPerPoint.ToString();
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

                bindingSource1.DataSource = GetData(homogenityResultQuery);
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
                AllWidth=dataGridView1.RowHeadersWidth;
                
                dataGridView1.Height = dataGridView1.Rows[0].Height + dataGridView1.ColumnHeadersHeight+20;
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    AllWidth += dataGridView1.Columns[i].Width;
                }
                dataGridView1.Width = AllWidth+20;
              
                dataGridView1.Dock = DockStyle.Top;
             //   dataGridView1.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width-dataGridView1.Width)/2, 0);

                #endregion


              

               
                #region Rename Columns & RePaint Rows

                dataGridView1.Columns["strip_count_in_one_roll"].HeaderText = "Lemért csíkok száma";
                dataGridView1.Columns["roll_id"].HeaderText = "Roll Azonosító";
                dataGridView1.Columns["tube_count"].HeaderText = "Lemért tubusszám";
                dataGridView1.Columns["date"].HeaderText = "Teszt időpontja";
                dataGridView1.Columns["lot_id"].HeaderText = "Lot azonosító";
                dataGridView1.Columns["avg"].HeaderText = "Átlag";
                dataGridView1.Columns["cv"].HeaderText = "CV(%)";
                dataGridView1.Columns["not_h62_error_count"].HeaderText = "Nem H62-es hibk száma";
                dataGridView1.Columns["out_of_range_strip_count"].HeaderText = "Kieső csíkok száma";
                if (Convert.ToBoolean(dataGridView1.Rows[0].Cells["homogenity_is_valid"].Value))
                {
                    dataGridView1.DefaultCellStyle.BackColor = Color.Green;

                }
                else
                {
                    dataGridView1.DefaultCellStyle.BackColor = Color.Red;
                }


                #endregion

                if (Convert.ToBoolean(dataGridView1.Rows[0].Cells["homogenity_is_valid"].Value))
                {
                    resultString = "Megfelelő";
                }
                else
                {
                    resultString = "Nem Megfelelő";


                }
                dataGridView1.Columns[dataGridView1.Columns.Count-1].HeaderText="Homogenity Teszt Értékelés";
                dataGridView1.Rows[0].Cells[dataGridView1.Columns.Count - 1].Value = resultString;
                dataGridView1.Columns["homogenity_is_valid"].Visible = false;
                dataGridView1.Columns["stddev"].Visible = false;
                dataGridView1.Columns["not_h62_is_valid"].Visible = false;
                dataGridView1.Columns["h62_errors_count"].Visible = false;
                dataGridView1.Columns["out_of_range_is_valid"].Visible = false;
                dataGridView1.Columns["invalidate"].Visible = false;
                dataGridView1.Columns["remeasured"].Visible = false;
                dataGridView1.Columns["pk_id"].Visible = false;
              
                dataGridView1.Columns["fk_blank_test_identify_id"].Visible = false;
         
                CreateAlternationChart();
               CreateChart();
                
            }
            catch (SqlException ex)
            {
                MessageBox.Show(string.Format("Exception: {0}", ex.StackTrace));
                System.Threading.Thread.CurrentThread.Abort();
            }
           
        }
       

        /// <summary>
        /// Create a chart with TubeIDs on X axis and nano_amper value on y axis
        /// </summary>
        private void CreateAllDataChart()
        {
            chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();

            string query = string.Format("select stddev,date from homogenity_result_alternation where lot_id='{0}' and roll_id='{1}'",lotid,rollid);
            ChartArea area = new ChartArea("ChartArea");
            area.AxisX.Name = "Homogenity Teszt";
            area.AxisY.Name = "Roll szórásának(glükóz) változása";

            DataTable allHomogenityDataTable = new DataTable();

            allHomogenityDataTable = GetData(query);

            area.AlignmentStyle = AreaAlignmentStyles.All;

            chart2.ChartAreas.Add(area);
            chart2.Dock = DockStyle.Bottom;

            chart2.Series.Clear();

            series1 = new Series("Roll Averages");
            series1.ChartType = SeriesChartType.Line;
            series1.Font = new Font("Arial Black", 10, FontStyle.Bold);

            chart2.Series.Add(series1.Name);
            chart2.ChartAreas[0].BackColor = Color.LightYellow;
            chart2.Series[series1.Name].Points.Clear();
            chart2.Series[series1.Name].Font = new Font("Arial Black", 10, FontStyle.Bold);

            for (int i = 0; i < allHomogenityDataTable.Rows.Count; i++)
            {
                Trace.TraceInformation("X axis values: {0}", allHomogenityDataTable.Rows[i].Field<DateTime>("date"));
                chart2.Series[series1.Name].Points.AddXY(Convert.ToString(allHomogenityDataTable.Rows[i].Field<DateTime>("date")), allHomogenityDataTable.Rows[i].ItemArray[1]);

            }
           
            chart2.Series[series1.Name].IsValueShownAsLabel = true;
            chart2.ChartAreas[0].AxisX.Name = "Homogenity Teszt Eredményei";
            chart2.ChartAreas[0].AxisY.Name = "Roll szórásának(glükóz érték) változása";    
            chart2.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
     
            chart2.ChartAreas[0].IsSameFontSizeForAllAxes = true;
            chart2.ChartAreas[0].AlignWithChartArea = "ChartArea";
          chart2.ChartAreas[0].AxisX.Title = string.Format("Csíkok a {0} azonosítójú Roll -ból (LotID:{1})",rollid,lotid);
          chart2.ChartAreas[0].AxisY.Title = "Roll szórásának(glükóz érték) változása";
            chart2.ChartAreas[0].AxisX.Interval = 10;
            chart2.ChartAreas[0].AxisX.IsMarksNextToAxis = true;
            chart2.ChartAreas[0].AxisX.ScaleView.Size = 10;
            chart2.ChartAreas[0].Axes[0].ScaleView = new AxisScaleView();
            chart2.ChartAreas[0].Axes[1].ScaleView = new AxisScaleView();
            chart2.ChartAreas[0].AxisY.LineColor = Color.DarkBlue;
            chart2.ChartAreas[0].AxisX.LineColor = Color.DarkBlue;
            chart2.ChartAreas[0].AxisY.Maximum = 80;           
            chart2.ChartAreas[0].Axes[1].StripLines[0].ForeColor = Color.Red;

          this.Controls["tabControl1"].Controls["tabPage1"].Controls.Add(chart2);
        }

        /// <summary>
        /// Creates a chart for all measured strip in this measurement x_axis-tubeIDs;y_axis:nano_amper
        /// </summary>
        /// <param name="selected_lot_id"></param>
        /// <param name="selected_roll_id"></param>
        System.Windows.Forms.DataVisualization.Charting.Chart columnChart;
        private void CreateColumnChart()
        {
            columnChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ChartArea area = new ChartArea("ChartArea");
            area.AxisY.Name = "Glükózérték";
            area.AxisX.Name = "Lemért csíkok";

            area.AlignmentStyle = AreaAlignmentStyles.All;
            columnChart.ChartAreas.Add(area);
         //   columnChart.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2, dataGridViewAll.Bottom);
            columnChart.Series.Clear();
            columnChart.Dock = DockStyle.Bottom;

            Series seriesOne;
            Series seriesTwo;

            seriesOne = new Series("Glükózérték");
            seriesOne.ChartType = SeriesChartType.Bubble;
            seriesOne.Font = new Font("Arial Black", 10, FontStyle.Bold);
            seriesOne.MarkerStep = 1;
       
            seriesOne.YValueType = ChartValueType.Double;
            seriesOne.XValueType = ChartValueType.Int32;
            

      /*      seriesTwo = new Series("Glükózértékek");
            seriesTwo.ChartType = SeriesChartType.Point;
            seriesTwo.Font = new Font("Arial Black", 10, FontStyle.Bold);
            seriesTwo.MarkerStep = 1;*/
          
        //    seriesTwo.YValueType = ChartValueType.Double;
       //     seriesTwo.XValueType = ChartValueType.Int32;
            
            columnChart.Series.Add(seriesOne.Name);
        //    columnChart.Series.Add(seriesTwo.Name);

            columnChart.Series[seriesOne.Name].YValuesPerPoint = 2;
            columnChart.Series[seriesOne.Name].Points.Clear();
            columnChart.Series[seriesOne.Name].Font = new Font("Arial Black", 10, FontStyle.Bold);
            columnChart.Series[seriesOne.Name].IsValueShownAsLabel = true;
            columnChart.Series[seriesOne.Name].MarkerSize = 5;
            columnChart.Series[seriesOne.Name].MarkerStyle = MarkerStyle.Diamond;
/*
            columnChart.Series[seriesTwo.Name].YValuesPerPoint = 2;
            columnChart.Series[seriesTwo.Name].Points.Clear();
            columnChart.Series[seriesTwo.Name].Font = new Font("Arial Black", 10, FontStyle.Bold);
            columnChart.Series[seriesTwo.Name].IsValueShownAsLabel = true;
            columnChart.Series[seriesTwo.Name].MarkerSize = 15;
            columnChart.Series[seriesTwo.Name].MarkerStyle = MarkerStyle.Square;*/

            double act_x_value;
            int stripCount=0;
            for (int i = 0; i <= dataGridViewAll.Rows.Count-2;i=i+2)
            {
                stripCount++;               
          
                columnChart.Series[seriesOne.Name].Points.AddXY("1. tubus",new object[]{dataGridViewAll.Rows[i].Cells["glu"].Value, dataGridViewAll.Rows[i+1].Cells["glu"].Value });
             //   columnChart.Series[seriesTwo.Name].Points.AddY(Convert.ToDouble(dataGridViewAll.Rows[i + 2].Cells["glu"].Value), Convert.ToDouble(dataGridViewAll.Rows[i + 3].Cells["glu"].Value));
            }

           
           
            columnChart.ChartAreas[0].AxisY.Name = "Glükózérték";
            columnChart.ChartAreas[0].AxisX.Name = "Lemért csíkok";

   //         columnChart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
    //        columnChart.ChartAreas[0].AlignWithChartArea = "ChartArea";
            
            columnChart.ChartAreas[0].AxisY.Title = "Glükózértéke";
            columnChart.ChartAreas[0].AxisX.Title = "Lemért csíkok";
            columnChart.ChartAreas[0].Axes[0].ScaleView = new AxisScaleView();
            columnChart.ChartAreas[0].Axes[1].ScaleView = new AxisScaleView();
            columnChart.ChartAreas[0].Axes.Initialize();
            
            columnChart.ChartAreas[0].Axes[1].InterlacedColor = Color.Red;
            columnChart.ChartAreas[0].Axes[1].IsInterlaced = true;
            columnChart.ChartAreas[0].AxisY.IsStartedFromZero = false;

            columnChart.ChartAreas[0].AxisY.LineColor = Color.DarkBlue;
            columnChart.ChartAreas[0].AxisX.LineColor = Color.DarkBlue;

            columnChart.ChartAreas[0].AxisX.Maximum = 30;
            columnChart.ChartAreas[0].AxisY.Maximum = 8;
            columnChart.ChartAreas[0].AxisY.Minimum = 3;

            StripLine upperLimitLine = new StripLine();
            upperLimitLine.TextLineAlignment = StringAlignment.Center;
            upperLimitLine.ForeColor = Color.Red;
            upperLimitLine.Interval = 7.3;
            
            StripLine lowerLimitLine = new StripLine();
            lowerLimitLine.TextLineAlignment = StringAlignment.Center;
            lowerLimitLine.ForeColor = Color.Red;
            lowerLimitLine.Interval = 5.4;

            columnChart.ChartAreas[0].AxisY.StripLines.Add(upperLimitLine); 
            columnChart.ChartAreas[0].AxisY.StripLines.Add(lowerLimitLine);
            columnChart.ChartAreas[0].AxisY.StripLines.SuspendUpdates();
            // Set interval of X axis to 1 week, with an offset of 1 day
            columnChart.ChartAreas[0].AxisX.Interval = 1;         
            columnChart.ChartAreas[0].AxisX.IntervalOffset = 1;
          

            columnChart.ChartAreas[0].Axes[1].IntervalAutoMode = IntervalAutoMode.FixedCount;
           
            columnChart.ChartAreas[0].Axes[1].InterlacedColor = Color.Red;
            columnChart.ChartAreas[0].Axes[1].Interval = 1.9;
            columnChart.ChartAreas[0].Axes[1].IntervalOffset = 5.4;
            columnChart.ChartAreas[0].Axes[1].ScaleBreakStyle.MaxNumberOfBreaks = 2;
            columnChart.ChartAreas[0].Axes[1].ScaleBreakStyle.LineWidth = 2;
            columnChart.ChartAreas[0].Axes[1].ScaleBreakStyle.LineColor = Color.Red;
            columnChart.ChartAreas[0].Axes[1].ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Straight;
            area.AxisY.LineDashStyle=ChartDashStyle.Solid;
            
            columnChart.ChartAreas[0].Axes[1].IntervalAutoMode = IntervalAutoMode.FixedCount;

            StripLine line1 = new StripLine();
            line1.TextLineAlignment = StringAlignment.Center;
            line1.ForeColor = Color.Red;
            line1.Interval = 7.3;
            columnChart.ChartAreas[0].Axes[1].StripLines.Add(line1);
            columnChart.ChartAreas[0].Axes[1].StripLines[0].ForeColor = Color.Red;

            StripLine line2 = new StripLine();
            line2.TextLineAlignment = StringAlignment.Center;
            line2.ForeColor = Color.Red;
            line2.Interval=5.4;
            columnChart.ChartAreas[0].Axes[1].StripLines.Add(line2);
            columnChart.ChartAreas[0].Axes[1].StripLines[0].ForeColor = Color.Red;
            

            // Enable 3D charts
             
                  //columnChart.ChartAreas[0].Area3DStyle.Enable3D = true;
                  //// Show a 30% perspective
                  //columnChart.ChartAreas[0].Area3DStyle.Perspective = 45;
                  //// Set the X Angle to 30
                  //columnChart.ChartAreas[0].Area3DStyle.Inclination = 30;
                  //// Set the Y Angle to 40
                  //columnChart.ChartAreas[0].Area3DStyle.Rotation = 40;
            Graphics g=CreateGraphics();
            g.DrawLines(new Pen(Color.Red),new Point[]{new Point(0,5),new Point(columnChart.Right,5),new Point(0,7),new Point(columnChart.Right,7)});
            columnChart.Update();
            this.Controls["tabControl1"].Controls["tabPage1"].Controls.Add(columnChart);
        }


        /// <summary>
        /// Create chart of homogenity test
        /// </summary>
        private void CreateChart()
        {
            chart1=new System.Windows.Forms.DataVisualization.Charting.Chart();

            ChartArea area = new ChartArea("ChartArea");
            area.AxisX.Name = "Homogenity Teszt";
            area.AxisY.Name = "Teszt Eredményei";            
         
            area.AlignmentStyle = AreaAlignmentStyles.All;

             chart1.ChartAreas.Add(area);
             chart1.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width/2-50, dataGridView1.Bottom);
             chart1.Width += 150;
             chart1.Series.Clear();

            series1 = new Series("Homogenity Teszt Eredményei");
            series1.ChartType = SeriesChartType.Line;
            series1.Font = new Font("Arial Black", 10,FontStyle.Bold);
            
            chart1.Series.Add(series1.Name);
            
            chart1.Series[series1.Name].Points.Clear();
            chart1.Series[series1.Name].Font = new Font("Arial Black", 10, FontStyle.Bold);
            chart1.Series[series1.Name].Points.AddXY(dataGridView1.Columns["avg"].HeaderText + ":" + dataGridView1.Rows[0].Cells["avg"].Value, dataGridView1.Rows[0].Cells["avg"].Value);
            chart1.Series[series1.Name].Points.AddXY(dataGridView1.Columns["cv"].HeaderText + ":" + dataGridView1.Rows[0].Cells["cv"].Value, dataGridView1.Rows[0].Cells["cv"].Value);
            chart1.Series[series1.Name].Points.AddXY(dataGridView1.Columns["not_h62_error_count"].HeaderText + ":" + dataGridView1.Rows[0].Cells["not_h62_error_count"].Value, dataGridView1.Rows[0].Cells["not_h62_error_count"].Value);
            chart1.Series[series1.Name].Points.AddXY(dataGridView1.Columns["strip_count_in_one_roll"].HeaderText + ":" + dataGridView1.Rows[0].Cells["strip_count_in_one_roll"].Value, dataGridView1.Rows[0].Cells["strip_count_in_one_roll"].Value);


            chart1.ChartAreas[0].AxisX.Name = "Homogenity Teszt";
            chart1.ChartAreas[0].AxisY.Name = "Teszt Eredményei";


            chart1.ChartAreas[0].AlignWithChartArea = "ChartArea";
            chart1.ChartAreas[0].Area3DStyle = new ChartArea3DStyle(chart1.ChartAreas[0]);
            chart1.ChartAreas[0].AxisX.Title = "Homogenity Teszt Eredmények";
            //chart1.ChartAreas[0].AxisY.Title = "Average (Nano Amper)";
            chart1.ChartAreas[0].Axes[0].ScaleView = new AxisScaleView();
            chart1.ChartAreas[0].Axes[1].ScaleView = new AxisScaleView();

            chart1.ChartAreas[0].AxisY.LineColor = Color.DarkBlue;
            chart1.ChartAreas[0].AxisX.LineColor = Color.DarkBlue;

            chart1.ChartAreas[0].AxisY.Maximum = 80;
            chart1.ChartAreas[0].AxisY.StripLines.Add(new StripLine());;

            // Set interval of X axis to 1 week, with an offset of 1 day
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.IntervalType =  DateTimeIntervalType.Weeks;
            chart1.ChartAreas[0].AxisX.IntervalOffset =  1;
            chart1.ChartAreas[0].AxisX.IntervalOffsetType = DateTimeIntervalType.Days;

             

            StripLine line1=new StripLine();
            line1.TextLineAlignment=StringAlignment.Center;
            line1.ForeColor = Color.Red;

            chart1.ChartAreas[0].Axes[1].StripLines.Add(line1);
            chart1.ChartAreas[0].Axes[1].StripLines[0].ForeColor=Color.Red;
       
            
            // Enable 3D charts
      /* 
            chart1.ChartAreas[0].Area3DStyle.Enable3D = true;
            // Show a 30% perspective
            chart1.ChartAreas[0].Area3DStyle.Perspective = 50;
            // Set the X Angle to 30
            chart1.ChartAreas[0].Area3DStyle.Inclination = 30;
            // Set the Y Angle to 40
            chart1.ChartAreas[0].Area3DStyle.Rotation = 40;
*/
            this.Controls["tabControl1"].Controls["tabPage2"].Controls.Add(chart1);
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
    }
}
