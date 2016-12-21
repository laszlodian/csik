using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
using WinFormBlankTest.Controller.DataManipulation;

namespace WinFormBlankTest.UI.Forms.Other_Forms
{
    public partial class AccuracyFinishedResultView : Form
    {
        public DataGridView dataGridView1=new DataGridView();
        public DataGridView dataGridViewAll = new DataGridView();
        public DataGridView allDataGridView = new DataGridView();

        public AccuracyFinishedResultView()
        {
            InitializeComponent();
        }
        public int accuracy_result_pkid=0;
        public string lot = string.Empty;
        public string master_lot = string.Empty;
        public int accuracy_lot_result_pkid = 0;
        public AccuracyFinishedResultView(string lotid, string master_lotid)
        {
            this.lot = lotid;
            this.master_lot = master_lotid;
           
            #region Get PrimaryKeys
            using (NpgsqlConnection connection = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    connection.Open();

                    using (NpgsqlCommand selectPkId = new NpgsqlCommand(string.Format("select Max(pk_id) from accuracy_results where lot_id='{0}' and master_lot_id='{1}'", lotid, master_lotid), connection))
                    {
                        object result = null;

                        result = selectPkId.ExecuteScalar();

                        if (result == null)
                        {
                            Trace.TraceError("No valid result in AccuracyFinishedResultView query:{0}", selectPkId.CommandText);
                        }
                        else
                            accuracy_result_pkid = Convert.ToInt32(result);

                    }

                    using (NpgsqlCommand getLotPkId = new NpgsqlCommand(string.Format("select Max(pk_id) from accuracy_lot_result where lot_id='{0}' and master_lot_id='{1}'", lotid, master_lotid), connection))
                    {
                        object result = null;

                        result = getLotPkId.ExecuteScalar();

                        if (result == null)
                        {
                            Trace.TraceError("No valid result to get accuracy_lot_result pk_id, query:{0}", getLotPkId.CommandText);
                        }
                        else
                            accuracy_lot_result_pkid = Convert.ToInt32(result);

                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception when try to get values, exception:\n\r{0}", ex.StackTrace);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            } 
            #endregion

            InitializeComponent();

            this.tabControl1.Controls["tabPage1"].Controls.Add(dataGridView1);
            this.tabControl1.Controls["tabPage1"].Controls.Add(dataGridViewAll);
            this.tabControl1.Controls["tabPage2"].Controls.Add(allDataGridView);

            if (accuracy_result_pkid != 0)
            {
                InitializeDataGridView(accuracy_result_pkid);
                InitializeAllDataGridView(accuracy_lot_result_pkid);
            }
            InitializeDataGridViewAllResult();

            this.Shown += new EventHandler(AccuracyFinishedResultView_Shown);
            this.FormClosed += new FormClosedEventHandler(AccuracyFinishedResultView_FormClosed);
        }
        public BindingSource bindingSourceAll = new BindingSource();
        private void InitializeAllDataGridView(int accuracy_lot_result_pkid)
        {
            string AccuracyLotResultQuery = string.Format("select lot_id,lot_accuracy,master_lot_id,outliers_count,outliers_percent,outliers_ok from accuracy_lot_result where pk_id={0}",accuracy_lot_result_pkid);

            #region Init DatagridView

            dataGridViewAll.AutoGenerateColumns = true;
            dataGridViewAll.AllowUserToOrderColumns = true;
            dataGridViewAll.AllowUserToResizeColumns = true;
            dataGridViewAll.AllowUserToResizeRows = true;
            dataGridViewAll.AllowDrop = false;
            #endregion

            #region Binding the database values to the dgv
            bindingSourceAll.DataSource = GetData(AccuracyLotResultQuery);
            dataGridViewAll.DataSource = bindingSourceAll;
            #endregion

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
            #endregion
            int AllWidth = 0;

            #region Set size Of DatagridView

            AllWidth = dataGridViewAll.RowHeadersWidth;

            for (int i = 0; i < dataGridViewAll.Columns.Count; i++)
            {
                AllWidth += dataGridViewAll.Columns[i].Width;
            }
            dataGridViewAll.Width = AllWidth + 20;
            dataGridViewAll.Dock = DockStyle.Bottom;

            this.dataGridViewAll.Anchor = AnchorStyles.Bottom;
            #endregion

           
        }

        public BindingSource bindingSourceAllData = new BindingSource();

        public void InitializeDataGridViewAllResult()
        {
            string AccuracyAllDataQuery = string.Format("select glu,calibrated_glu,lot_id,roll_id,blood_vial_id from accuracy_test where glu<>0 and (lot_id='{0}' or lot_id='{1}')",lot,master_lot);

            #region Init DatagridView

            allDataGridView.AutoGenerateColumns = true;
            allDataGridView.AllowUserToOrderColumns = true;
            allDataGridView.AllowUserToResizeColumns = true;
            allDataGridView.AllowUserToResizeRows = true;
            allDataGridView.AllowDrop = false;
            #endregion

            #region Binding source to datagridview
            bindingSourceAllData.DataSource = GetData(AccuracyAllDataQuery);
            allDataGridView.DataSource = bindingSourceAllData;
            #endregion

            #region Set display of the dgv
            allDataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            allDataGridView.DefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            DataGridViewColumnCollection colmCollection = new DataGridViewColumnCollection(allDataGridView);
            allDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            allDataGridView.RowHeadersVisible = false;
            allDataGridView.AllowUserToAddRows = false;
            allDataGridView.AutoSize = false;
            allDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            allDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            allDataGridView.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.InsetDouble;
            allDataGridView.BorderStyle = BorderStyle.Fixed3D;
            allDataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
            #endregion
            int AllWidth = 0;

            #region Set size Of DatagridView

            AllWidth = dataGridView1.RowHeadersWidth;

            allDataGridView.Height = allDataGridView.Rows[0].Height + allDataGridView.ColumnHeadersHeight + 20;
            for (int i = 0; i < allDataGridView.Columns.Count; i++)
            {
                AllWidth += allDataGridView.Columns[i].Width;
            }
            allDataGridView.Width = AllWidth + 20;

            allDataGridView.Dock = DockStyle.Top;
            
            #endregion

            CreateChartForAllData();
        }
        public System.Windows.Forms.DataVisualization.Charting.Chart chartAll;
        private void CreateChartForAllData()
        {

            string queryForCentral=string.Format("SELECT * FROM accuracy_result_central LEFT JOIN accuracy_result_master ON accuracy_result_central.fk_accuracy_result_master_id = accuracy_result_master.pk_id LEFT JOIN accuracy_test ON accuracy_result_master.fk_accuracy_test_id = accuracy_test.pk_id where (accuracy_test.lot_id='{0}' or accuracy_test.lot_id='{1}') and accuracy_test.glu<>0",lot,master_lot);

            string queryForAll = string.Format("SELECT * FROM accuracy_central_bias LEFT JOIN accuracy_test ON accuracy_central_bias.fk_accuracy_test_id = accuracy_test.pk_id where (accuracy_test.lot_id='{0}' or accuracy_test.lot_id='{1}') ", lot, master_lot);
            DataTable tableForAll = GetData(queryForAll);
            chartAll = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ChartArea area = new ChartArea("ChartArea");
            area.AxisX.Name = "Master Lot glucose average[mmol/l]";
            area.AxisY.Name = "Glucose concentrations [mmol/l]";
            area.AlignmentStyle = AreaAlignmentStyles.All;
            chartAll.ChartAreas.Add(area);
            
            chartAll.Series.Clear();
            Series seriesAll = new Series();
            Series series2 = new Series();
            seriesAll = new Series("Lot accuracy with venous blood");
            seriesAll.ChartType = SeriesChartType.Line;
            seriesAll.Font = new Font("Arial Black", 10, FontStyle.Bold);

            series2 = new Series("Lot accuracy");
            series2.ChartType = SeriesChartType.Line;
            series2.Font = new Font("Arial Black", 10, FontStyle.Bold);

            chartAll.Series.Add(series1.Name);

            chartAll.Series[series1.Name].Points.Clear();
            chartAll.Series[series1.Name].Font = new Font("Arial Black", 10, FontStyle.Bold);

            for (int i = 0; i < allDataGridView.Rows.Count; i++)
            {
                Trace.TraceInformation("X axis values: {0}", allDataGridView.Rows[i].Cells["glu"]);

                chartAll.Series[0].Points.AddXY(allDataGridView.Rows[i].Cells["glu"].Value, allDataGridView.Rows[i].Cells["calibrated_glu"].Value);
	           

            }

            chartAll.ChartAreas[0].AxisX.Name = "Master Lot glucose average[mmol/l]";
            chartAll.ChartAreas[0].AxisY.Name = "Glucose concentrations [mmol/l]";

            chartAll.Dock = DockStyle.Bottom;
            chartAll.ChartAreas[0].AlignWithChartArea = "ChartArea";
            chartAll.ChartAreas[0].Area3DStyle = new ChartArea3DStyle(chartAll.ChartAreas[0]);
            chartAll.ChartAreas[0].AxisX.Title = "Master Lot glucose average[mmol/l]";
            chartAll.ChartAreas[0].AxisY.Title = "Glucose concentrations [mmol/l]";         

            // Set interval of X axis to 1 week, with an offset of 1 day
            chartAll.ChartAreas[0].AxisX.Interval = 1;
             this.tabControl1.Controls["tabPage2"].Controls.Add(chartAll);
        }

        void AccuracyFinishedResultView_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        void AccuracyFinishedResultView_Shown(object sender, EventArgs e)
        {
            Export();
        }
        private void Export()
        {
           // new ExportToCSV(dataGridView1, dataGridViewAll, string.Format("accuracy_{0}_{1}", lot,master_lot));
        }

        
        public BindingSource bindingSource1 = new BindingSource();
        private void InitializeDataGridView(int pkid)
        {

            string AccuracyQuery = string.Format("SELECT lot_id,blood_sample_id,normalized_bias_avg FROM accuracy_result_central where lot_id='{0}'", lot);
            #region Init DatagridView

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.AllowUserToResizeColumns = true;
            dataGridView1.AllowUserToResizeRows = true;
            dataGridView1.AllowDrop = false;
            #endregion

            #region Binding the database values to the dgv
            bindingSource1.DataSource = GetData(AccuracyQuery);
            dataGridView1.DataSource = bindingSource1;
            #endregion

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
            #endregion
            int AllWidth = 0;

            #region Set size Of DatagridView

            AllWidth = dataGridView1.RowHeadersWidth;

            dataGridView1.Height = dataGridView1.Rows[0].Height + dataGridView1.ColumnHeadersHeight + 20;
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                AllWidth += dataGridView1.Columns[i].Width;
            }
            dataGridView1.Width = AllWidth + 20;
            dataGridView1.Height = 200;
            dataGridView1.Dock = DockStyle.Top;

            this.dataGridView1.Anchor = AnchorStyles.Top;
          //  dataGridView1.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - dataGridView1.Width) / 2, 0);

            #endregion
            CreateChart();
        }
        public System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        public Series series1;

        private void CreateChart()
        {

            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ChartArea area = new ChartArea("ChartArea");
            area.AxisX.Name = "Accuracy Teszt";
            area.AxisY.Name = "Bias átlag alakulás a teszt során";

            area.AlignmentStyle = AreaAlignmentStyles.All;
            chart1.ChartAreas.Add(area);
            chart1.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2, dataGridView1.Bottom);
            chart1.Series.Clear();

            series1 = new Series("Accuracy Teszt Átlag");
            series1.ChartType = SeriesChartType.Line;
            series1.Font = new Font("Arial Black", 10, FontStyle.Bold);

            chart1.Series.Add(series1.Name);

            chart1.Series[series1.Name].Points.Clear();
            chart1.Series[series1.Name].Font = new Font("Arial Black", 10, FontStyle.Bold);

            chart1.Size = new Size(500,500);

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                chart1.Series[series1.Name].Points.AddXY(dataGridView1.Columns[2].HeaderText /*+ ":" + dataGridView1.Rows[i].Cells[2].Value*/, dataGridView1.Rows[i].Cells[2].Value);
            }
            

            chart1.ChartAreas[0].AxisX.Name = "Accuracy Teszt Eredmény";
            chart1.ChartAreas[0].AxisY.Name = "BIAS Átlag";


            chart1.ChartAreas[0].AlignWithChartArea = "ChartArea";
            chart1.ChartAreas[0].Area3DStyle = new ChartArea3DStyle(chart1.ChartAreas[0]);
            chart1.ChartAreas[0].AxisX.Title = "Accuracy Teszt Eredmény";
            chart1.ChartAreas[0].AxisY.Title = "BIAS Átlag";
          

            chart1.ChartAreas[0].AxisY.LineColor = Color.DarkBlue;
            chart1.ChartAreas[0].AxisX.LineColor = Color.DarkBlue;          

            // Set interval of X axis to 1 week, with an offset of 1 day
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Weeks;
            chart1.ChartAreas[0].AxisX.IntervalOffset = 1;
            chart1.ChartAreas[0].AxisX.IntervalOffsetType = DateTimeIntervalType.Days;
            chart1.ChartAreas[0].Axes[1].IntervalType = DateTimeIntervalType.Number;


            this.tabControl1.Controls["tabPage1"].Controls.Add(chart1);
        }


        public DataTable table = new DataTable();
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
