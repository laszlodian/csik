using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Npgsql;
using Microsoft.Office.Core;
using WinFormBlankTest.Controller.DataManipulation;
using System.Globalization;

namespace WinFormBlankTest.UI.Forms.Classes_for_Show_DataGrid
{
    public partial class PrintResult : Form
    {        
        #region Variables
        public const int MAX_PW_LENGHT = 10;

        public DataGridView lotDataGrid = new DataGridView();
        public DataGridView rollDataGrid = new DataGridView();
        public DataGridView blankDataGrid = new DataGridView();
        public DataGridView homogenityDataGrid = new DataGridView();
        public DataGridView accuracyDataGrid = new DataGridView();
        public DataGridView humidityDataGrid = new DataGridView();
        public DataGridView accuracyValuesDataGrid = new DataGridView();
        public string lot;

        public List<int> selected_fk_errors_id = new List<int>();
        public List<int> selected_result_ids = new List<int>();
        public List<string> selected_serial_numbers = new List<string>();
        public List<double> selected_nano_ampers = new List<double>();
        public List<double> selected_glus = new List<double>();
        public static DataTable tableRoll = new DataTable();
        public static DataTable tableHomogenity = new DataTable();
        public static DataTable tableBlank = new DataTable();
        public static DataTable tableLot = new DataTable();
        public static DataTable tableAcc2 = new DataTable();
        public static DataTable tableAcc = new DataTable();
        private static DataTable tableAccuracy = new DataTable();
        private DataTable tableAccuracyValues = new DataTable();
        public static DataTable tableEnvironment = new DataTable();
        private int lot_pkid;
        private string lotTableName;
private  string lotTable;
        #endregion

        public PrintResult(string lotid)
        {
            this.lot = lotid;

            InitializeComponent();

            this.Controls.Add(blankDataGrid);
            this.Controls.Add(humidityDataGrid);
            this.Controls.Add(homogenityDataGrid);
            this.Controls.Add(lotDataGrid);
            this.Controls.Add(rollDataGrid);
            this.Controls.Add(accuracyDataGrid);
            this.Controls.Add(accuracyValuesDataGrid);

            InitializeBlankDataGrid();

            this.BringToFront();
            this.FormClosed += new FormClosedEventHandler(PrintResult_FormClosed);
          //  this.Shown += new EventHandler(PrintResult_Shown);
        }

        void PrintResult_Shown(object sender, EventArgs e)
        {
            new ExportToCSV(blankDataGrid,homogenityDataGrid,lot);
        }

        void PrintResult_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
        
        private void InitializeBlankDataGrid()
        {
          
           using (NpgsqlConnection conn = new NpgsqlConnection(Properties.Settings.Default.DBReleaseConnection))
           {
               try
               {
                   conn.Open();
                   
                   ///Get values for blank results
                   #region get values from identify table where lot,roll, measuretype is correct(result_id,serial_number)

                   using (NpgsqlCommand averagesComm=new NpgsqlCommand(string.Format("select lot_id,roll_id,avg,blank_is_valid,date from blank_test_averages where lot_id='{0}' and invalidate=False",lot),conn))
                   {
                       NpgsqlDataAdapter dAdapter = new NpgsqlDataAdapter(averagesComm);
                       
                       dAdapter.Fill(tableBlank);
                   }
                   using (NpgsqlCommand averagesComm = new NpgsqlCommand(string.Format("select temperature,humidity from blank_test_environment where lot_id='{0}' and invalidate=False", lot), conn))
                   {
                       NpgsqlDataAdapter dAdapter = new NpgsqlDataAdapter(averagesComm);

                       dAdapter.Fill(tableEnvironment);
                   }
                   using (NpgsqlCommand rollCommand = new NpgsqlCommand(string.Format("select * from roll_result where lot_id='{0}' and invalidate=False", lot), conn))
                   {
                       NpgsqlDataAdapter dAdapter = new NpgsqlDataAdapter(rollCommand);

                       dAdapter.Fill(tableRoll);
                   }
                   using (NpgsqlCommand homogenityComm = new NpgsqlCommand(string.Format("select * from homogenity_result where lot_id='{0}' and invalidate=False and included=true", lot), conn))
                   {
                       NpgsqlDataAdapter dAdapter = new NpgsqlDataAdapter(homogenityComm);

                       dAdapter.Fill(tableHomogenity);
                   }
                   using (NpgsqlCommand getPKID=new NpgsqlCommand(string.Format("select MAX(pk_id) from lot_result where lot_id='{0}' and invalidate=False",lot),conn))
                   {
                       object res = null;

                       res = getPKID.ExecuteScalar();
                       if (res!=null)
                       {
                           lot_pkid = Convert.ToInt32(res);    
                       }                       
                   }
                 
                    using (NpgsqlCommand get_lot_result_pk_id = new NpgsqlCommand(string.Format("select modified from lot_result where pk_id='{0}' and invalidate=false",lot_pkid), conn))
                    {
                       bool modified = Convert.ToBoolean(get_lot_result_pk_id.ExecuteScalar());

                       Program.IsLotModified = modified;
                       if (modified == true)
                        {                              
                            lotTable = "lot_result_modified";
                        }else
                           lotTable = "lot_result";
                    }                   

                   using (NpgsqlCommand lotCommand= new NpgsqlCommand(string.Format("select * from {1} where pk_id=(select max(pk_id) from {1} where lot_id='{0}')", lot, lotTable), conn))
                   {
                       NpgsqlDataAdapter dAdapter = new NpgsqlDataAdapter(lotCommand);

                       dAdapter.Fill(tableLot);
                   }
                   int accValuesPkId = 0;
                   using (NpgsqlCommand accuracyValuesPkid=new NpgsqlCommand(string.Format("select max(pk_id) from accuracy_values where measured_lot_id='{0}'",lot),conn))
                   {
                       object result = null;

                       result = accuracyValuesPkid.ExecuteScalar();

                       if (result == null)
                       {
                           Trace.TraceError("Result is null when trying to  get max pkid from accuracy_values");
                       }
                       else
                           accValuesPkId =Convert.ToInt32( result);
                   }
                   using (NpgsqlCommand accComm = new NpgsqlCommand(string.Format("select * from accuracy_values where pk_id={0}", accValuesPkId), conn))
                   {
                       NpgsqlDataAdapter dAdapter = new NpgsqlDataAdapter(accComm);

                       dAdapter.Fill(tableAccuracyValues);
                   }
                   int accuracy_pkid = 0;
                   using (NpgsqlCommand accPkIdComm=new NpgsqlCommand(string.Format("select max(pk_id) from accuracy_lot_result where lot_id='{0}'",lot),conn))
                   {
                       object res = null;

                       res = accPkIdComm.ExecuteScalar();
                       if (res == null)
                       {
                           Trace.TraceError("Res is null when trying to detect max pkid in accuracy lot result");
                       }
                       else
                           accuracy_pkid = Convert.ToInt32(res);
                   }
                   using (NpgsqlCommand accComm = new NpgsqlCommand(string.Format("select * from accuracy_lot_result where pk_id={0}", accuracy_pkid), conn))
                   {
                       NpgsqlDataAdapter dAdapter = new NpgsqlDataAdapter(accComm);

                       dAdapter.Fill(tableAccuracy);
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
              }///NpgsqlConnection Close

               #region Init DatagridView
               blankDataGrid.AutoSize = false;
               blankDataGrid.AutoGenerateColumns = true;
               blankDataGrid.AllowUserToOrderColumns = true;
               blankDataGrid.AllowUserToResizeColumns = true;
               blankDataGrid.AllowUserToResizeRows = true;
               blankDataGrid.AllowDrop = false;
               blankDataGrid.Dock = DockStyle.Top;
               blankDataGrid.LayoutEngine.Layout(this, new LayoutEventArgs(blankDataGrid, "Size"));// Manual;

               #endregion

               #region Binding the database values to the dgv

               blankDataGrid.DataSource = tableBlank;
               blankDataGrid.AutoScrollOffset = new Point(blankDataGrid.VerticalScrollingOffset, blankDataGrid.HorizontalScrollingOffset);

               #endregion

               #region Set size Of DatagridView

               int AllWidth = 0;
               int AllHeight = blankDataGrid.ColumnHeadersHeight;
               for (int i = 0; i < blankDataGrid.RowCount; i++)
               {
                   AllHeight += blankDataGrid.Rows[i].Height;
               }

               for (int i = 0; i < blankDataGrid.Columns.Count - 1; i++)
               {

                   AllWidth += blankDataGrid.Columns[i].Width + blankDataGrid.RowHeadersWidth;
               }
               blankDataGrid.Height = AllHeight;
               blankDataGrid.Width = AllWidth;
               blankDataGrid.Location = new Point(0, Screen.PrimaryScreen.WorkingArea.Height / 2 - 80);
               blankDataGrid.Dock = DockStyle.Top;// DockStyle.Right;

               #endregion

               #region Set display of the dgv
               blankDataGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
               blankDataGrid.DefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
               DataGridViewColumnCollection colmCollection = new DataGridViewColumnCollection(blankDataGrid);
               blankDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
               blankDataGrid.RowHeadersVisible = false;
               blankDataGrid.AllowUserToAddRows = false;

               blankDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
               blankDataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
               blankDataGrid.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.InsetDouble;
               blankDataGrid.BorderStyle = BorderStyle.Fixed3D;
               blankDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;



               #endregion
               #region Rename Columns & RePaint Rows
               

               DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
               cellStyle.BackColor = Color.Blue;
               cellStyle.Alignment = DataGridViewContentAlignment.BottomRight;
               for (int i = 0; i < blankDataGrid.Rows.Count; i++)
               {
                   if ((double)blankDataGrid.Rows[i].Cells["avg"].Value >= (double)13
                   && (double)blankDataGrid.Rows[i].Cells["avg"].Value <= (double)51)
                   {
                       this.blankDataGrid.Rows[i].DefaultCellStyle.BackColor = Color.Green;

                   }
                   else
                   {

                       tableBlank.Rows[i].BeginEdit();
                       blankDataGrid.Rows[i].Selected = true;

                       cellStyle.BackColor = Color.Red;
                       blankDataGrid.Rows[i].DefaultCellStyle = cellStyle;
                       blankDataGrid.Rows[i].DefaultCellStyle.BackColor = Color.SeaGreen;
                       blankDataGrid.Rows[i].Selected = true;
                       blankDataGrid.Rows[i].DefaultCellStyle.SelectionBackColor = Color.PeachPuff;
                       //   this.blankDataGrid.RowsDefaultCellStyle.BackColor = Color.Red;
                       this.blankDataGrid.Rows[i].InheritedStyle.BackColor = Color.Red;
                       blankDataGrid.EndEdit();//RowChanged
                       tableBlank.Rows[i].AcceptChanges();
                   }
                   blankDataGrid.Rows[i].DefaultCellStyle.ApplyStyle(blankDataGrid.Rows[i].DefaultCellStyle);
               }

               blankDataGrid.Select();

               blankDataGrid.Refresh();

               blankDataGrid.Update();
               blankDataGrid.PerformLayout(blankDataGrid, "BackColor");
               blankDataGrid.SuspendLayout();

               #endregion

               InitHomogenityDatagridView();
               this.Controls.Add(blankDataGrid);

              /// new CreateExcelReport();
           }

        private void InitHomogenityDatagridView()
        {

            #region Init DatagridView
            homogenityDataGrid.AutoSize = false;
            homogenityDataGrid.AutoGenerateColumns = true;
            homogenityDataGrid.AllowUserToOrderColumns = true;
            homogenityDataGrid.AllowUserToResizeColumns = true;
            homogenityDataGrid.AllowUserToResizeRows = true;
            homogenityDataGrid.AllowDrop = false;
            homogenityDataGrid.Dock = DockStyle.Top;
            homogenityDataGrid.LayoutEngine.Layout(this, new LayoutEventArgs(homogenityDataGrid, "Size"));// Manual;

            #endregion

            #region Binding the database values to the dgv

            homogenityDataGrid.DataSource = tableHomogenity;
            homogenityDataGrid.AutoScrollOffset = new Point(homogenityDataGrid.VerticalScrollingOffset, homogenityDataGrid.HorizontalScrollingOffset);

            #endregion

            #region Set size Of DatagridView

            int AllWidth = 0;
            int AllHeight = homogenityDataGrid.ColumnHeadersHeight;
            for (int i = 0; i < homogenityDataGrid.RowCount; i++)
            {
                AllHeight += homogenityDataGrid.Rows[i].Height;
            }

            for (int i = 0; i < homogenityDataGrid.Columns.Count - 1; i++)
            {

                AllWidth += homogenityDataGrid.Columns[i].Width + homogenityDataGrid.RowHeadersWidth;
            }
            homogenityDataGrid.Height = AllHeight;
            homogenityDataGrid.Width = AllWidth;
            homogenityDataGrid.Location = new Point(0, Screen.PrimaryScreen.WorkingArea.Height / 2 - 80);
            homogenityDataGrid.Dock = DockStyle.Top;// DockStyle.Right;

            #endregion



            #region Set display of the dgv
            homogenityDataGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            homogenityDataGrid.DefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            DataGridViewColumnCollection colmCollection = new DataGridViewColumnCollection(homogenityDataGrid);
            homogenityDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            homogenityDataGrid.RowHeadersVisible = false;
            homogenityDataGrid.AllowUserToAddRows = false;

            homogenityDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            homogenityDataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            homogenityDataGrid.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.InsetDouble;
            homogenityDataGrid.BorderStyle = BorderStyle.Fixed3D;
            homogenityDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;



            #endregion
            this.Controls.Add(homogenityDataGrid);
            homogenityDataGrid.Select();

            homogenityDataGrid.Refresh();

            homogenityDataGrid.Update();
            homogenityDataGrid.PerformLayout(homogenityDataGrid, "BackColor");
            homogenityDataGrid.SuspendLayout();
            
            InitLotDatagridView();            
            InitAccuracyDatagridView();
        }
        private void InitLotDatagridView()
        {

            #region Init DatagridView
            lotDataGrid.AutoSize = false;
            lotDataGrid.AutoGenerateColumns = true;
            lotDataGrid.AllowUserToOrderColumns = true;
            lotDataGrid.AllowUserToResizeColumns = true;
            lotDataGrid.AllowUserToResizeRows = true;
            lotDataGrid.AllowDrop = false;
            lotDataGrid.Dock = DockStyle.Top;
            lotDataGrid.LayoutEngine.Layout(this, new LayoutEventArgs(lotDataGrid, "Size"));// Manual;

            #endregion

            #region Binding the database values to the dgv

            lotDataGrid.DataSource = tableLot;
            lotDataGrid.AutoScrollOffset = new Point(lotDataGrid.VerticalScrollingOffset, lotDataGrid.HorizontalScrollingOffset);

            #endregion

            #region Set size Of DatagridView

            int AllWidth = 0;
            int AllHeight = lotDataGrid.ColumnHeadersHeight;
            for (int i = 0; i < lotDataGrid.RowCount; i++)
            {
                AllHeight += lotDataGrid.Rows[i].Height;
            }

            for (int i = 0; i < lotDataGrid.Columns.Count - 1; i++)
            {

                AllWidth += lotDataGrid.Columns[i].Width + lotDataGrid.RowHeadersWidth;
            }
            lotDataGrid.Height = AllHeight;
            lotDataGrid.Width = AllWidth;
            lotDataGrid.Location = new Point(0, Screen.PrimaryScreen.WorkingArea.Height / 2 - 80);
            lotDataGrid.Dock = DockStyle.Top;// DockStyle.Right;

            #endregion



            #region Set display of the dgv
            lotDataGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            lotDataGrid.DefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            DataGridViewColumnCollection colmCollection = new DataGridViewColumnCollection(lotDataGrid);
            lotDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            lotDataGrid.RowHeadersVisible = false;
            lotDataGrid.AllowUserToAddRows = false;

            lotDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            lotDataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            lotDataGrid.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.InsetDouble;
            lotDataGrid.BorderStyle = BorderStyle.Fixed3D;
            lotDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;



            #endregion
            InitRollDataGridView();
        }

        private void InitRollDataGridView()
        {

            #region Init DatagridView
            rollDataGrid.AutoSize = false;
            rollDataGrid.AutoGenerateColumns = true;
            rollDataGrid.AllowUserToOrderColumns = true;
            rollDataGrid.AllowUserToResizeColumns = true;
            rollDataGrid.AllowUserToResizeRows = true;
            rollDataGrid.AllowDrop = false;
            rollDataGrid.Dock = DockStyle.Top;
            rollDataGrid.LayoutEngine.Layout(this, new LayoutEventArgs(rollDataGrid, "Size"));// Manual;

            #endregion

            #region Binding the database values to the dgv

            rollDataGrid.DataSource = tableRoll;
            rollDataGrid.AutoScrollOffset = new Point(rollDataGrid.VerticalScrollingOffset, rollDataGrid.HorizontalScrollingOffset);

            #endregion

            #region Set size Of DatagridView

            int AllWidth = 0;
            int AllHeight = rollDataGrid.ColumnHeadersHeight;
            for (int i = 0; i < rollDataGrid.RowCount; i++)
            {
                AllHeight += rollDataGrid.Rows[i].Height;
            }

            for (int i = 0; i < rollDataGrid.Columns.Count - 1; i++)
            {

                AllWidth += rollDataGrid.Columns[i].Width + rollDataGrid.RowHeadersWidth;
            }
            rollDataGrid.Height = AllHeight;
            rollDataGrid.Width = AllWidth;
            rollDataGrid.Location = new Point(0, Screen.PrimaryScreen.WorkingArea.Height / 2 - 80);
            rollDataGrid.Dock = DockStyle.Top;// DockStyle.Right;

            #endregion



            #region Set display of the dgv
            rollDataGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            rollDataGrid.DefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            DataGridViewColumnCollection colmCollection = new DataGridViewColumnCollection(rollDataGrid);
            rollDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            rollDataGrid.RowHeadersVisible = false;
            rollDataGrid.AllowUserToAddRows = false;

            rollDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            rollDataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            rollDataGrid.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.InsetDouble;
            rollDataGrid.BorderStyle = BorderStyle.Fixed3D;
            rollDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;



            #endregion
        }
        
        private void InitAccuracyDatagridView()
        {

            #region Init DatagridView
            accuracyDataGrid.AutoSize = false;
            accuracyDataGrid.AutoGenerateColumns = true;
            accuracyDataGrid.AllowUserToOrderColumns = true;
            accuracyDataGrid.AllowUserToResizeColumns = true;
            accuracyDataGrid.AllowUserToResizeRows = true;
            accuracyDataGrid.AllowDrop = false;
            accuracyDataGrid.Dock = DockStyle.Top;
            accuracyDataGrid.LayoutEngine.Layout(this, new LayoutEventArgs(accuracyDataGrid, "Size"));// Manual;

            #endregion

            #region Binding the database values to the dgv

            accuracyDataGrid.DataSource = tableAccuracy;
            accuracyDataGrid.AutoScrollOffset = new Point(accuracyDataGrid.VerticalScrollingOffset, accuracyDataGrid.HorizontalScrollingOffset);

            #endregion

            #region Set size Of DatagridView

            int AllWidth = 0;
            int AllHeight = accuracyDataGrid.ColumnHeadersHeight;
            for (int i = 0; i < accuracyDataGrid.RowCount; i++)
            {
                AllHeight += accuracyDataGrid.Rows[i].Height;
            }

            for (int i = 0; i < accuracyDataGrid.Columns.Count - 1; i++)
            {

                AllWidth += accuracyDataGrid.Columns[i].Width + accuracyDataGrid.RowHeadersWidth;
            }
            accuracyDataGrid.Height = AllHeight;
            accuracyDataGrid.Width = AllWidth;
            accuracyDataGrid.Location = new Point(0, Screen.PrimaryScreen.WorkingArea.Height / 2 - 80);
            accuracyDataGrid.Dock = DockStyle.Top;// DockStyle.Right;

            #endregion



            #region Set display of the dgv
            accuracyDataGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            accuracyDataGrid.DefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            DataGridViewColumnCollection colmCollection = new DataGridViewColumnCollection(accuracyDataGrid);
            accuracyDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            accuracyDataGrid.RowHeadersVisible = false;
            accuracyDataGrid.AllowUserToAddRows = false;

            accuracyDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            accuracyDataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            accuracyDataGrid.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.InsetDouble;
            accuracyDataGrid.BorderStyle = BorderStyle.Fixed3D;
            accuracyDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;



            #endregion

            InitAccuracyValuesDatagridView();
        }
        private void InitAccuracyValuesDatagridView()
        {

            #region Init DatagridView
            accuracyValuesDataGrid.AutoSize = false;
            accuracyValuesDataGrid.AutoGenerateColumns = true;
            accuracyValuesDataGrid.AllowUserToOrderColumns = true;
            accuracyValuesDataGrid.AllowUserToResizeColumns = true;
            accuracyValuesDataGrid.AllowUserToResizeRows = true;
            accuracyValuesDataGrid.AllowDrop = false;
            accuracyValuesDataGrid.Dock = DockStyle.Top;
            accuracyValuesDataGrid.LayoutEngine.Layout(this, new LayoutEventArgs(accuracyValuesDataGrid, "Size"));// Manual;

            #endregion

            #region Binding the database values to the dgv

            accuracyValuesDataGrid.DataSource = tableAccuracyValues;
            accuracyValuesDataGrid.AutoScrollOffset = new Point(accuracyValuesDataGrid.VerticalScrollingOffset, accuracyValuesDataGrid.HorizontalScrollingOffset);

            #endregion

            #region Set size Of DatagridView

            int AllWidth = 0;
            int AllHeight = accuracyValuesDataGrid.ColumnHeadersHeight;
            for (int i = 0; i < accuracyValuesDataGrid.RowCount; i++)
            {
                AllHeight += accuracyValuesDataGrid.Rows[i].Height;
            }

            for (int i = 0; i < accuracyValuesDataGrid.Columns.Count - 1; i++)
            {

                AllWidth += accuracyValuesDataGrid.Columns[i].Width + accuracyValuesDataGrid.RowHeadersWidth;
            }
            accuracyValuesDataGrid.Height = AllHeight;
            accuracyValuesDataGrid.Width = AllWidth;
            accuracyValuesDataGrid.Location = new Point(0, Screen.PrimaryScreen.WorkingArea.Height / 2 - 80);
            accuracyValuesDataGrid.Dock = DockStyle.Top;// DockStyle.Right;

            #endregion

            #region Set display of the dgv
            accuracyValuesDataGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            accuracyValuesDataGrid.DefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            DataGridViewColumnCollection colmCollection = new DataGridViewColumnCollection(accuracyValuesDataGrid);
            accuracyValuesDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            accuracyValuesDataGrid.RowHeadersVisible = false;
            accuracyValuesDataGrid.AllowUserToAddRows = false;

            accuracyValuesDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            accuracyValuesDataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            accuracyValuesDataGrid.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.InsetDouble;
            accuracyValuesDataGrid.BorderStyle = BorderStyle.Fixed3D;
            accuracyValuesDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;



            #endregion

            InitEnvironmentDataGridView();

            new StoreCSVFileFinalResults();
            ExportToExcel(tableBlank,tableHomogenity,tableLot,tableRoll,tableAccuracyValues,tableAccuracy,tableEnvironment);
            
        }

        private void InitEnvironmentDataGridView()
        {
            #region Init DatagridView
            humidityDataGrid.AutoSize = true;
            humidityDataGrid.AutoGenerateColumns = true;
            humidityDataGrid.AllowUserToOrderColumns = true;
            humidityDataGrid.AllowUserToResizeColumns = true;
            humidityDataGrid.AllowUserToResizeRows = true;
            humidityDataGrid.AllowDrop = false;
            humidityDataGrid.Dock = DockStyle.Top;
            humidityDataGrid.LayoutEngine.Layout(this, new LayoutEventArgs(humidityDataGrid, "Size"));// Manual;

            #endregion

            #region Binding the database values to the dgv

            humidityDataGrid.DataSource = tableEnvironment;
            humidityDataGrid.AutoScrollOffset = new Point(humidityDataGrid.VerticalScrollingOffset, humidityDataGrid.HorizontalScrollingOffset);

            #endregion

            #region Set size Of DatagridView

            int AllWidth = 0;
            int AllHeight = humidityDataGrid.ColumnHeadersHeight;
            for (int i = 0; i < humidityDataGrid.RowCount; i++)
            {
                AllHeight += humidityDataGrid.Rows[i].Height;
            }

            for (int i = 0; i < humidityDataGrid.Columns.Count - 1; i++)
            {

                AllWidth += humidityDataGrid.Columns[i].Width + humidityDataGrid.RowHeadersWidth;
            }
            humidityDataGrid.Height = AllHeight;
            humidityDataGrid.Width = AllWidth;
            humidityDataGrid.Location = new Point(0, Screen.PrimaryScreen.WorkingArea.Height / 2 - 80);
            humidityDataGrid.Dock = DockStyle.Top;// DockStyle.Right;

            #endregion



            #region Set display of the dgv
            humidityDataGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            humidityDataGrid.DefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            DataGridViewColumnCollection colmCollection = new DataGridViewColumnCollection(humidityDataGrid);
            humidityDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            humidityDataGrid.RowHeadersVisible = false;
            humidityDataGrid.AllowUserToAddRows = false;

            humidityDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            humidityDataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            humidityDataGrid.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.InsetDouble;
            humidityDataGrid.BorderStyle = BorderStyle.Fixed3D;
            humidityDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;

            this.Controls.Add(humidityDataGrid);

            #endregion
        }

        /// <summary>
        /// Export Database values to Excel Format
        /// </summary>
        /// <param name="tableBlank"></param>
        /// <param name="tableHomogenity"></param>
        /// <param name="tableLot"></param>
        /// <param name="tableRoll"></param>
        /// <param name="tableAccuracyValues"></param>
        /// <param name="tableAccuracy"></param>
        /// <param name="tableEnvironment"></param>
        private void ExportToExcel(DataTable tableBlank, DataTable tableHomogenity, DataTable tableLot, DataTable tableRoll, DataTable tableAccuracyValues, DataTable tableAccuracy,DataTable tableEnvironment)
        {

            string path = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(string.Format("{0}\\ITB_final_results_", path));
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet = null;
                System.Globalization.CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                
                object misValue = System.Reflection.Missing.Value;

                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us"); //change in order to "Workbooks.Add" API crash               
               
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                //Export LOT_ID
                xlWorkSheet.Cells[4, 5] = Convert.ToString(blankDataGrid.Rows[0].Cells[0].Value);
                //Export 777
                xlWorkSheet.Cells[5, 5] = "777";
                //Expot Expiration time
                xlWorkSheet.Cells[6, 5] = Convert.ToString(accuracyValuesDataGrid.Rows[0].Cells[7].Value);
                //Export Blank Test Date
                xlWorkSheet.Cells[12, 3] = Convert.ToString(blankDataGrid.Rows[0].Cells[4].Value);
                //Export Temperature
                xlWorkSheet.Cells[16, 4] = Convert.ToString(humidityDataGrid.Rows[0].Cells[0].Value);
                //Export Humidity
                xlWorkSheet.Cells[17, 4] = Convert.ToString(humidityDataGrid.Rows[0].Cells[1].Value);
                //Export blanktest averages
                int i = 0;
                foreach (DataGridViewRow row in blankDataGrid.Rows)
                {
                    xlWorkSheet.Cells[22 + i, 1] = Convert.ToString(lot);
                    xlWorkSheet.Cells[22 + i, 2] = string.Format("{0}",i+1);
                    xlWorkSheet.Cells[22 + i, 3] = Convert.ToString(row.Cells[2].Value);
                   
                    i++;
                }                                     
                //Export homogenity values
                i = 0;
                //homogenity date
                xlWorkSheet.Cells[42 , 3] = Convert.ToString(homogenityDataGrid.Rows[0].Cells[9].Value);
                //Export Temperature
                xlWorkSheet.Cells[46, 4] = Convert.ToString(humidityDataGrid.Rows[0].Cells[0].Value);
                //Export Humidity
                xlWorkSheet.Cells[47, 4] = Convert.ToString(humidityDataGrid.Rows[0].Cells[1].Value);
                foreach (DataGridViewRow row in homogenityDataGrid.Rows)
                {                        
                    //homogenity_roll id
                    xlWorkSheet.Cells[53 + i, 1] = Convert.ToString(row.Cells[1].Value);
                    //homogenity avg
                    xlWorkSheet.Cells[53 + i, 2] = Convert.ToString(row.Cells[4].Value);
                    //homogenity cv
                    xlWorkSheet.Cells[53 + i, 3] = Convert.ToString(row.Cells[6].Value);
                    //homogenity outofrangeStripCount
                    xlWorkSheet.Cells[53 + i, 4] = Convert.ToString(row.Cells[11].Value);
                    //homogenity not h62 error count
                    xlWorkSheet.Cells[53 + i, 5] = Convert.ToString(row.Cells[7].Value);
                    //homogenity h62 error count
                  //  xlWorkSheet.Cells[54 + i, 6] = Convert.ToString(row.Cells[8].Value);                         
                    i++;
                }
                if (Program.IsLotModified)
                {
                    //lot_avg
                    xlWorkSheet.Cells[71, 2] = Convert.ToString(lotDataGrid.Rows[0].Cells[0].Value);
                    //lot_cv
                    xlWorkSheet.Cells[73, 2] = Convert.ToString(lotDataGrid.Rows[0].Cells[1].Value);

                }
                else
                {
                    //lot_avg
                    xlWorkSheet.Cells[71, 2] = Convert.ToString(lotDataGrid.Rows[0].Cells[0].Value);
                    //lot_cv
                    xlWorkSheet.Cells[73, 2] = Convert.ToString(lotDataGrid.Rows[0].Cells[2].Value);
                }
            //Accuracy Check
                //Get Master Lot Data
            //Master Lot LotID                
            xlWorkSheet.Cells[89,3]=Convert.ToString(accuracyValuesDataGrid.Rows[0].Cells[3].Value);
            //Master LOT Calibration
            xlWorkSheet.Cells[91, 3] = Convert.ToString(accuracyValuesDataGrid.Rows[0].Cells[4].Value);
            //HTC érték
            xlWorkSheet.Cells[87, 4] = Convert.ToString(accuracyValuesDataGrid.Rows[0].Cells[2].Value);
            //Humidity Values
            xlWorkSheet.Cells[86, 4] = Convert.ToString(accuracyValuesDataGrid.Rows[0].Cells[1].Value);
            //Test datetime
            xlWorkSheet.Cells[81, 3] = Convert.ToString(accuracyValuesDataGrid.Rows[0].Cells[6].Value);
           //Accuracy Temperature
            xlWorkSheet.Cells[85, 4] = Convert.ToString(accuracyValuesDataGrid.Rows[0].Cells[8].Value);           
            //LOT Accuracy
            xlWorkSheet.Cells[94, 3] = Convert.ToString(accuracyDataGrid.Rows[0].Cells[3].Value);   

                string fullFileName=string.Empty;

                fullFileName = string.Format("{0}\\ExportedFiles\\Export_{1}", path,lot);
                xlWorkBook.SaveAs(fullFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, /*Read PW:*/ misValue,
                    /*write PW:*/ misValue,
                    misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();  
             
                System.Threading.Thread.CurrentThread.CurrentCulture = Oldci;
            
                ReleaseComObject(xlWorkSheet);
                ReleaseComObject(xlWorkBook);
                ReleaseComObject(xlApp);              
                    
               this.Cursor = Cursors.Default;

               MessageBox.Show(string.Format("Az exportálás megtörtént\r\nElérési út: {0}\\ExportedFiles\\Export_{1}", path,lot));
               this.Close();
        }
        
        /// <summary>
        /// Release COM objects safetly
        /// </summary>
        /// <param name="obj"></param>
        private void ReleaseComObject(object obj)
        {
            try
            {
                if (obj != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
            }
            catch (Exception ex)
            { 
               Trace.TraceError("Exception Occured while releasing object " + ex.ToString());
            }
        }
     /*   private void InitAccuracyValuesDataGrid()
        {
            #region Init DatagridView
            accuracyDataGrid.AutoSize = false;
            accuracyDataGrid.AutoGenerateColumns = true;
            accuracyDataGrid.AllowUserToOrderColumns = true;
            accuracyDataGrid.AllowUserToResizeColumns = true;
            accuracyDataGrid.AllowUserToResizeRows = true;
            accuracyDataGrid.AllowDrop = false;
            accuracyDataGrid.Dock = DockStyle.Top;
            accuracyDataGrid.LayoutEngine.Layout(this, new LayoutEventArgs(accuracyDataGrid, "Size"));// Manual;

            #endregion

            #region Binding the database values to the dgv

            accuracyDataGrid.DataSource = tableAccuracyValues;
            accuracyDataGrid.AutoScrollOffset = new Point(accuracyDataGrid.VerticalScrollingOffset, accuracyDataGrid.HorizontalScrollingOffset);

            #endregion

            #region Set size Of DatagridView

            int AllWidth = 0;
            int AllHeight = accuracyDataGrid.ColumnHeadersHeight;
            for (int i = 0; i < accuracyDataGrid.RowCount; i++)
            {
                AllHeight += accuracyDataGrid.Rows[i].Height;
            }

            for (int i = 0; i < accuracyDataGrid.Columns.Count - 1; i++)
            {

                AllWidth += accuracyDataGrid.Columns[i].Width + accuracyDataGrid.RowHeadersWidth;
            }
            accuracyDataGrid.Height = AllHeight;
            accuracyDataGrid.Width = AllWidth;
            accuracyDataGrid.Location = new Point(0, Screen.PrimaryScreen.WorkingArea.Height / 2 - 80);
            accuracyDataGrid.Dock = DockStyle.Top;// DockStyle.Right;

            #endregion



            #region Set display of the dgv
            accuracyDataGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            accuracyDataGrid.DefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            DataGridViewColumnCollection colmCollection = new DataGridViewColumnCollection(accuracyDataGrid);
            accuracyDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font(new FontFamily("Arial Black"), 9, FontStyle.Bold);
            accuracyDataGrid.RowHeadersVisible = false;
            accuracyDataGrid.AllowUserToAddRows = false;

            accuracyDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            accuracyDataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            accuracyDataGrid.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.InsetDouble;
            accuracyDataGrid.BorderStyle = BorderStyle.Fixed3D;
            accuracyDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;



            #endregion



        }*/
        
    }
}
