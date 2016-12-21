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
using e77.MeasureBase;
using System.Windows.Input;
using System.Reflection;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.AccessControl;
using WinFormBlankTest.UI.Chart;


namespace WinFormBlankTest.UI.Forms
{
    public partial class ResultForm : Form
    {
            #region Variables


        public DataGridView dataGridViewHomo = new DataGridView();
        public DataGridViewTextBoxColumn txbCell;
        public static List<string> homogenity_roll_id = new List<string>();
        public static List<string> homogenity_lot_id = new List<string>();
        private static bool no_homogenity;    
        
        private List<int> tha_strip_count_in_a_roll = new List<int>();      
        public static List<string> homogenity_error = new List<string>();
        public static List<string> homogenity_error_text = new List<string>();
        public static List<string> homogenity_users = new List<string>();
        public static List<string> homogenity_computers = new List<string>();
        public static List<string> homogenity_sn = new List<string>();
        public static List<bool> homogenity_earlyDribble = new List<bool>();
        public static List<bool> homogenity_deviceReplace = new List<bool>();
        public static List<double> homogenity_glus = new List<double>();
        public static List<bool> homogenity_h62_value = new List<bool>();
        public static List<bool> homogenity_not_h62_value = new List<bool>();
        public static List<double> homogenity_temp = new List<double>();
        public static List<DateTime> homogenity_start_dates = new List<DateTime>();
        public static List<DateTime> homogenity_end_dates = new List<DateTime>();
        public static List<int> homogenity_h62_count = new List<int>();
        public static List<int> homogenity_not_h62_count = new List<int>();

        int not_h62d_error_count = 0;
        int h62d_error_count = 0;
        public TimeSpan tsp = new TimeSpan();
        public int column_number = 0;
        public List<string> measure_type = new List<string>();
        public int dateType = 0;
        public int column_index = 0;
        public int true_items = 0;
        public int false_items = 0;
        public int row_number = 0;
        public List<object> values = new List<object>();
        public List<Type> types = new List<Type>();

        /// <summary>
        /// double érték regex
        /// </summary>
        public Regex doubleReg = new Regex("[0-9]?.[0-9]?");
        /// <summary>
        /// 1-10 számjegyből álló string (lot,roll,sn,fk,pk)
        /// </summary>
        public Regex idReg = new Regex("^[0-9]{1,10}$");
        public int doubleTyped = 0;
        public int intTyped = 0;
        public int stringTyped = 0;
        public int boolTyped = 0;
        public int accepted_count = 0;
        public int not_accepted_count = 0;

        public double avg = 0;
        public double sum = 0;
        public double stddev = 0;
        public double sumOfSquaresOfDifferences = 0;
        public double sd = 0;
        public double cv = 0;
        public int summary_ints = 0;
        public double int_avg = 0;

        public Color color;
        public Color homogenity_color;
        public List<bool> selected_early_dribble= new List<bool>();
        public List<bool> selected_device_replace = new List<bool>();
        public List<int> selected_codes = new List<int>();
        public List<string> selected_rolls = new List<string>();
        public static List<DateTime> selected_end_dates=new List<DateTime>();
        public static List<DateTime> selected_start_dates = new List<DateTime>();
        public static List<string> selected_computers=new List<string>();
        public static List<string> selected_users = new List<string>();
        public static List<int> selected_fk_errors_id=new List<int>();
       public static List<double> selected_temp=new List<double>();
        public static List<double> selected_nano_ampers=new List<double>();
        public static List<double> selected_glus=new List<double>();
        public static List<string> selected_serial_numbers=new List<string>();
        public static List<int> selected_result_ids = new List<int>();
        public static List<string> selected_error = new List<string>();
        public static List<string> selected_error_text = new List<string>();
        public static List<bool> selected_not_h62 = new List<bool>();
        public static List<bool> selected_h62 = new List<bool>();

        public static List<double> homogenity_stddev = new List<double>();
        public static List<bool> homogenity_is_valid = new List<bool>();
        public static List<double> homogenity_avg = new List<double>();
        public static List<double> homogenity_cv = new List<double>();
        public static List<DateTime> homogenity_date = new List<DateTime>();

        public static List<int> homogenity_strip_count = new List<int>();
        public static List<bool> homogenity_strip_count_is_valid = new List<bool>();
        public static List<int> homogenity_out_of_range_strip_count = new List<int>();
        public static List<string> homogenity_h62 = new List<string>();
        public static List<string> homogenity_not_h62 = new List<string>();

        public List<int> wrong_strip_count = new List<int>();

        MenuItem hide_item = new MenuItem();
        MenuItem calculate_item = new MenuItem();
        MenuItem color_item = new MenuItem();


        public static object selected_lot_id;
        public static object selected_roll_id;
        public static object selected_result;
        public static object selected_measurytype;
        public MenuItem object_item;
        public MenuItem remeasure_item;
        public MenuItem showdata_item;
        List<int> invalidate_result_pkid = new List<int>();
        List<int> invalidate_errors_pkid = new List<int>();

        public string lot_id;
        public string roll_id;
        public string measuretype;
        public DataGridViewButtonColumn btnColumn;
        public Button btRemeasure = new Button();

        List<int> result_pk = new List<int>();
        public static string lot_avg_res;
        public static string lot_cv_res;
        public static string lot_stripcount_res;
        public static string stripcount_res;
        public static string lot_out_res;
        public static string roll_avg_res;
        public static string roll_cv_res;
        public static string lot_valid_res;
        public static string roll_valid_res;
        public static string blank_valid_res;
        public static string homo_valid_res;
        public  double roll_avg;
        public static bool lot_avg_ok;
        public static bool roll_avg_ok;
        public static  bool lot_cv_ok;
        public static bool roll_cv_ok;
        public static bool lot_ok;
        public static bool roll_ok;
        public static bool lot_out_ok;
        public static bool blank_ok;
        public static bool homogenity_ok;
        public string res;
        object remeasured_lotid;
        object remeasured_rollid;
        string roll_to_invalidate;
        string lot_to_invalidate;
        string measure_to_invalidate;

        public string result;
        public int i=0;

        private DataGridView dataGridView1 = new DataGridView();
        public static DataGridView dataGridRoll = new DataGridView();
        private DataGridView dataGridLOT = new DataGridView();

        private BindingSource bindingSource1 = new BindingSource();
        private BindingSource bindingRollSource = new BindingSource();
        private Button btOK = new Button();
     
        #endregion

        /// <summary>
        /// In case of /show argument to show blank test and homogenity test results for a lot and possibility to remeasure a roll
        /// </summary>
        /// <param name="LotID"></param>
        /// <param name="rollid"></param>
        /// <param name="blank_test_result">blank</param>
        /// <param name="homogenity_result"></param>
        /// <param name="avg">blank</param>
        /// <param name="homogenity_avg"></param>
        /// <param name="cv">blank</param>
        /// <param name="homogenity_cv"></param>
        /// <param name="date">blank</param>
        /// <param name="homogenity_date"></param>
        /// <param name="measure_type"></param>
        /// <param name="wrong_strip"></param>
        /// <param name="h62"></param>
        /// <param name="not_h62"></param>
        public ResultForm(string LotID, string[] blank_rollid, string[] homogenity_rollid, string[] blank_test_result, string[] homogenity_result, string[] avg,
            string[] homogenity_avg, string[] cv,
            string[] homogenity_cv,string[] date,string[] homogenity_date,string[] measure_type,int[] wrong_strip,string[] h62,string[] not_h62,bool not_h62_ok)
        {
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;
            InitializeComponent();
            Trace.TraceInformation("resultform started, blank results will be added to the table first");
            this.Width = this.dataGridView1.Width;
            this.Update();
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;

            foreach (string blank_result in blank_test_result)
            {
                if (blank_result == "True")
                {
                    result = "Megfelelő";
                    color = Color.Green;
                }
                else
                {
                    result = "Nem megfelelő";
                    color = Color.Red;
                }

                

                bindingSource1.Add(new BlankTest(LotID, blank_rollid[i],result, avg[i], cv[i], date[i], "Blank Check", "------", "------", "------",false));
                i++;
            }

            Trace.TraceInformation("blank rows added, homo result row will be added immediately");
                   
            i = 0;
            foreach (string res in homogenity_result)
            {
               
                if (res == "True")
                {
                    result = "Megfelelő";
                    homogenity_color = Color.Green;
                }
                else
                {
                    result = "Nem megfelelő";
                    homogenity_color = Color.Red;
                }

                try
                {
                    Trace.TraceInformation("set homo values to grid ,now will be started");

                    bindingSource1.Add(
                        new BlankTest(  LotID, homogenity_rollid[i], result,
                            homogenity_avg[i], homogenity_cv[i], homogenity_date[i],
                            "Homogenity Check", Convert.ToString(wrong_strip[i]), h62[i], not_h62[i], not_h62_ok));
                    
                    Trace.TraceInformation("new rows with homo results added");
                }
                catch (Exception e)
                {
                    Trace.TraceError("ex.stacktrace:{0},e.Message:{1},e.Inner:{2},e.Source:{3}",e.StackTrace,e.Message,e.InnerException,e.Source);
                    throw new IndexOutOfRangeException("Kevesebb elem van a megmutatandónál");
                }
                i++;
            }

            Trace.TraceInformation("columns,header,binded properties will be setted");
            this.Width = Screen.PrimaryScreen.WorkingArea.Width-100;
            
             dataGridView1.Font = new Font(dataGridView1.Font, FontStyle.Bold);

             this.Load += new System.EventHandler(EnumsAndComboBox_Load);

            
        }


        /// <summary>
        /// To show this after blank stored(THIS SHOWED AFTER BLANK TEST COMPLETED)
        /// </summary>
        /// <param name="LotID"></param>
        /// <param name="rollid"></param>
        /// <param name="homogenity_result"></param>
        /// <param name="homogenity_avg"></param>
        /// <param name="avg_ok"></param>
        /// <param name="homogenity_cv"></param>
        /// <param name="cv_ok"></param>
        /// <param name="wrong_strip"></param>
        /// <param name="out_is_valid"></param>
        /// <param name="h62"></param>
        /// <param name="h62_ok"></param>
        /// <param name="not_h62"></param>
        /// <param name="not_h62_ok"></param>
        /// 
        public ResultForm(string LotID, string rollid, bool blank_result,
           string blank_avg, bool avg_ok,
           double blank_cv,DateTime dt)
        {

            InitializeComponent(); 
            Trace.TraceInformation("ResultForm");

            if (blank_result)
            {
                result = "Megfelelő";
                homogenity_color = Color.Green;
            }
            else
            {
                result = "Nem megfelelő";
                homogenity_color = Color.Red;
            }

            try
            {
                Trace.TraceInformation("set blank values to grid ,now will be started");

                bindingSource1.Add(
                    new BlankTest(
                        LotID, rollid, result,blank_avg, avg_ok, blank_cv,"Blank Check",dt));

                
                Trace.TraceInformation("new rows with blank results added");
            }
            catch (Exception e)
            {
                Trace.TraceError("Exception in ResultsForm constructor ;ex:{0}", e.StackTrace);
                throw new IndexOutOfRangeException("Kevesebb elem van a megmutatandónál");
            }
            i++;

            Trace.TraceInformation("columns,header,binded properties will be setted");
            dataGridView1.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            this.Load += new System.EventHandler(EnumsAndComboBox_Load);


        }






        /// <summary>
        /// After store homogenity_result(THIS RUNS AFTER HOMOGENITY TEST FINISHED!!)
        /// </summary>
        /// <param name="LotID"></param>
        /// <param name="rollid"></param>
        /// <param name="result"></param>
        /// <param name="avg"></param>
        /// <param name="cv"></param>
        /// <param name="date"></param>
        /// <param name="color"></param>
        public ResultForm(string LotID,string rollid,string result,string avg,string avg_ok,string cv,string cv_ok,string date,string out_of_range_strip_count,string out_ok,
            string strip_count,string h62,string not_h62)
        {
            InitializeComponent();
            Trace.TraceError("ResultForm started after homogenity test finished");           

            bindingSource1.Add(new BlankTest(LotID, rollid, result, avg,avg_ok, cv,cv_ok, date,out_of_range_strip_count,out_ok,strip_count,h62,not_h62));

            dataGridViewHomo.DefaultCellStyle.BackColor = color;

            dataGridViewHomo.Font = new Font(dataGridViewHomo.Font, FontStyle.Bold);

            this.Load += new System.EventHandler(EnumsAndComboBox_Load_After_h_stored);
        }
       
     


        /// <summary>
        /// In case when remeasure, invalidate, or show all data choosed from the contextmenu 
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="roll"></param>
        /// <param name="lot_averages"></param>
        /// <param name="lot_averages_is_valid"></param>
        /// <param name="roll_averages"></param>
        /// <param name="roll_averages_is_valid"></param>
        /// <param name="lot_ceve"></param>
        /// <param name="lot_ceve_is_valid"></param>
        /// <param name="roll_ceve"></param>
        /// <param name="roll_ceve_is_valid"></param>
        /// <param name="test_date"></param>
        /// <param name="stripCount"></param>
        /// <param name="stripCount_is_valid"></param>
        /// <param name="outofrange_stripcount"></param>
        /// <param name="outofrange_is_valid"></param>
        /// <param name="lot_is_valid"></param>
        /// <param name="roll_is_valid"></param>
        /// <param name="homogenity_is_valid"></param>
        /// <param name="blank_is_valid"></param>
        /// <param name="h62_error_count"></param>
        /// <param name="not_h62_error_count"></param>
         public ResultForm(string lot, string[] roll,  double[] lot_averages, bool[] lot_averages_is_valid,
            double[] roll_averages, bool[] roll_averages_is_valid, double[] lot_ceve, bool[] lot_ceve_is_valid,
            double[] roll_ceve, bool[] roll_ceve_is_valid, DateTime[] test_date,
            int[] stripCount, bool[] stripCount_is_valid, int[] outofrange_stripcount, bool[] outofrange_is_valid,
            bool[] lot_is_valid, bool[] roll_is_valid, bool[] homogenity_is_valid, bool[] blank_is_valid,
            int[] h62_error_count, int[] not_h62_error_count)
         {

             InitializeComponent();

             lot_avg_ok = lot_averages_is_valid[0];
             lot_cv_ok=lot_ceve_is_valid[0];

             roll_avg_ok = roll_averages_is_valid[0];
             roll_cv_ok = roll_ceve_is_valid[0];

             lot_out_ok = outofrange_is_valid[0];
             lot_ok = lot_is_valid[0];
             roll_ok = roll_is_valid[0];
             homogenity_ok = homogenity_is_valid[0];
             blank_ok = blank_is_valid[0];


     #region lot cv & avg
             if (lot_averages_is_valid[0])
             {
		            lot_avg_res="Megfelelő";
	          }else
                    lot_avg_res="Nem Megfelelő";

             if (lot_ceve_is_valid[0])
	        {
                 lot_cv_res="Megfelelő";
	        }else
                 lot_cv_res="Nem Megfelelő";
    #endregion

             if (outofrange_is_valid[0])
	        {
                 lot_out_res="Megfelelő";
	        }else
                 lot_out_res="Nem Megfelelő";
              #region roll cv & avg
             if (roll_ceve_is_valid[0])
	            {
                 roll_cv_res="Megfelelő";
	        }else
                 roll_cv_res="Nem Megfelelő";
             if (roll_averages_is_valid[0])
	            {
                 roll_avg_res="Megfelelő";
	            }else
                 roll_avg_res="Nem Megelelő";
             #endregion
             if (stripCount_is_valid[0])
	        {
                 stripcount_res="Megfelelő";
	        }else
                 stripcount_res="Nem Megfelelő";

        #region lot,roll,blank,homogenity
             if (lot_is_valid[0])
	        {
                 lot_valid_res="Megfelelő";
	        }else
               lot_valid_res="Nem Megfelelő";

              if (roll_is_valid[0])
	        {
                 roll_valid_res="Megfelelő";
	        }else
               roll_valid_res="Nem Megfelelő";

              if (blank_is_valid[0])
	        {
                 blank_valid_res="Megfelelő";
	        }else
               blank_valid_res="Nem Megfelelő";
             if (homogenity_is_valid[0])
	        {
                 homo_valid_res="Megfelelő";
	        }else
               homo_valid_res="Nem Megfelelő";
#endregion
             i = 0;
             
             foreach (string roll_id in roll)
             {

                 Trace.TraceInformation("ResultForm-constructor- i ValueType is {0}",i);
                
            bindingRollSource.Add(
                     new HomogenityTest(lot, roll_id, lot_averages[i],                     
                         lot_averages_is_valid[i], lot_avg_res,roll_averages[i],
                         roll_ceve_is_valid[i],roll_cv_res,lot_ceve[i],lot_ceve_is_valid[i],lot_cv_res,roll_ceve[i],roll_ceve_is_valid[i],roll_cv_res, test_date[i],
                         stripCount[0], stripCount_is_valid[0],
                         outofrange_stripcount[i], outofrange_is_valid[i],lot_out_res,lot_is_valid[0],lot_valid_res,
                         roll_is_valid[i],roll_valid_res, homogenity_is_valid[i],homo_valid_res,blank_is_valid[0],blank_valid_res,                      
                         h62_error_count[i], not_h62_error_count[i]));

                 i++;
             }


             i = 0;

             bindingSource1.Add(new HomogenityTest(lot, lot_averages[i], lot_averages_is_valid[i],
             lot_ceve[i], lot_ceve_is_valid[i],
             test_date[i],outofrange_stripcount[i],
             stripCount[i], stripCount_is_valid[i],
             lot_is_valid[i]  ,
             h62_error_count[i], not_h62_error_count[i]));


            dataGridView1.DefaultCellStyle.BackColor = color;
            dataGridView1.Font = new Font(dataGridView1.Font, FontStyle.Bold);            

            this.Load += new System.EventHandler(EnumsAndComboBox_Load_For_All);
         }


         #region Methods


        /// <summary>
        /// For add columns and init the dgv for ROLL Results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         private void EnumsAndComboBox_Load_For_All(object sender, System.EventArgs e)
         {

             // Initialize the DataGridView.
             dataGridRoll.AutoGenerateColumns = false;
             dataGridRoll.AutoSize = true;
             dataGridRoll.DataSource = bindingSource1;

             DataGridViewColumn column = new DataGridViewTextBoxColumn();
             column.DataPropertyName = "LotID";
             column.Name = "Lot ID";
             column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
             dataGridRoll.Columns.Add(column);

             column = new DataGridViewTextBoxColumn();
             column.DataPropertyName = "RollID";
             column.Name = "Roll ID";
             column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
             dataGridRoll.Columns.Add(column);

             column = new DataGridViewTextBoxColumn();
             column.DataPropertyName = "Roll_Averages";
             column.Name = "Roll Átlag";
             column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
             dataGridRoll.Columns.Add(column);             

             column = new DataGridViewTextBoxColumn();
             column.DataPropertyName = "Roll_Averages_IsValid";
             column.Name = "Roll Átlag Értékelés";
             column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
             dataGridRoll.Columns.Add(column);  

             column = new DataGridViewTextBoxColumn();
             column.DataPropertyName = "Roll_CV";
             column.Name = "Roll CV";
             column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
             dataGridRoll.Columns.Add(column);

             column = new DataGridViewTextBoxColumn();
             column.DataPropertyName = "Roll_CV_IsValid";
             column.Name = "Roll CV Értékelés";
             column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
             dataGridRoll.Columns.Add(column);      

             column = new DataGridViewTextBoxColumn();
             column.DataPropertyName = "Date";
             column.Name = "Dátum";
             column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
             dataGridRoll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "MeasureType";
            column.Name = "Mérés típus";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridRoll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "OutOfRange";
            column.Name = "Kieső csíkszám(glucose)";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridRoll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "H62_count";
            column.Name = "H62 hibák száma";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridRoll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Not_H62_count";
            column.Name = "Nem H62 hibák száma";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridRoll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Lot_Averages";
            column.Name = "LOT Átlag";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridLOT.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Lot_Averages_IsValid";
            column.Name = "LOT Átlag Értékelés";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridLOT.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "LOT_CV";
            column.Name = "LOT CV";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridLOT.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "LOT_CV_IsValid";
            column.Name = "LOT CV Értékelés";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridLOT.Columns.Add(column);

           // AdjustWidth(true);
            dataGridLOT.ResumeLayout();

             // Initialize the form.
            dataGridLOT.Dock = DockStyle.Fill;
           // this.Size = new Size(dataGridLOT.Width + 50, dataGridLOT.Height + 150);
             this.Controls.Add(dataGridLOT);
             this.Controls.Add(dataGridRoll);         
             this.Text = "Csíkvágás eredmények";
             this.MinimizeBox = false;
             this.MaximizeBox = false;
             dataGridLOT.Size = new Size(dataGridLOT.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + dataGridLOT.FirstDisplayedScrollingColumnHiddenWidth + dataGridLOT.FirstDisplayedCell.OwningColumn.Width, dataGridLOT.Rows.GetRowsHeight(DataGridViewElementStates.Visible) + dataGridLOT.ColumnHeadersHeight + dataGridLOT.ColumnHeadersHeight);
             dataGridLOT.LayoutEngine.Layout(dataGridLOT, new LayoutEventArgs(dataGridLOT, "Size"));

             foreach (DataGridViewRow row in dataGridLOT.Rows)
                 if (row.Cells[2].Value.ToString() == "Nem megfelelő")
                 {
                     row.DefaultCellStyle.BackColor = Color.Red;
                 }
                 else
                     row.DefaultCellStyle.BackColor = Color.Green;


             dataGridLOT.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridRoll_CellMouseClickAll);
             //     dataGridView1.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView1_ColumnHeaderMouseClick);

             dataGridLOT.CellMouseClick += new DataGridViewCellMouseEventHandler(dataGridRoll_CellMouseClickAll);

             this.dataGridLOT.CellMouseDown += new DataGridViewCellMouseEventHandler(dataGridView1_ColumnHeaderMouseClickedForAll);

             Trace.TraceError("resultF set columns 520 row");

             foreach (DataGridViewRow row in dataGridLOT.Rows)
                 if (row.Cells[2].Value.ToString() == "Nem megfelelő")
                 {
                     row.DefaultCellStyle.BackColor = Color.Red;
                 }
                 else
                     row.DefaultCellStyle.BackColor = Color.Green;


             dataGridLOT.Dock = DockStyle.Top;
             dataGridLOT.AutoSize = false;
             this.Size = new Size(dataGridLOT.Width + 20, dataGridLOT.Height + 20);

             this.Size = new Size(this.ClientSize.Width, this.ClientSize.Height + 20);

             this.StartPosition = FormStartPosition.CenterScreen;
             dataGridLOT.PerformLayout();
             this.PerformLayout();


             ///Add OK button to close the form
             SetButtonOk();
         }


        /// <summary>
         
        /// This method is to handle the remeasure, invalidate and show all roll data request
        /// /in case of /show argument
        /// 
        /// IN CASE BLANK TEST FINISHED!!
        /// IN CASE OF /SHOW TO DISPLAY TEST RESULTS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
    private void EnumsAndComboBox_Load(object sender, System.EventArgs e)
         {
             #region add columns to the datagridview
             
        // Initialize the DataGridView.
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.AutoSize = false;
        dataGridView1.DataSource = bindingSource1;

        
        DataGridViewColumn column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "LotID";
        column.Name = "Lot ID";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridView1.Columns.Add(column);
        
        
        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "RollID";
        column.Name = "Roll ID";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridView1.Columns.Add(column);

        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "Result";
        column.Name = "Eredmény";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridView1.Columns.Add(column);

        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "Averages";
        column.Name = "Átlag";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridView1.Columns.Add(column);

        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "CV";
        column.Name = "CV(%)";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridView1.Columns.Add(column);

        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "Date";
        column.Name = "Dátum";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;        
        dataGridView1.Columns.Add(column);
        
        column = new DataGridViewTextBoxColumn();        
        column.DataPropertyName = "MeasureType";        
        column.Name = "Mérés típusa";        
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridView1.Columns.Add(column);

        #region Adding measure dependent columns
        if (Program.measureType != "blank")
            {


                column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "OutOfRange";
                column.Name = "Kieső csíkszám(glucose)";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "H62_count";
                column.Name = "H62 hibák száma";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Not_H62_count";
                column.Name = "Nem H62 hibák száma";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns.Add(column);
            }
        #endregion

        btnColumn = new DataGridViewButtonColumn();
        btnColumn.HeaderText = "Adatok mentése(xls)";
        btnColumn.Text = "Adatok mentése(xls)";
        btnColumn.UseColumnTextForButtonValue = true;
        btnColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridView1.Columns.Add(btnColumn);
       
        #endregion

        // Initialize the form.
       
      //  AdjustWidth();  
            
        this.Controls.Add(dataGridView1);
       
        if (Program.measureType == "blank")
        {
            this.Text = "Blank Teszt eredményei";
        }else if (Program.measureType == "homogenity")
	    {
            this.Text = "Homogenitás Teszt eredményei";
        }
        else if (Program.measureType == "show")
        {
            this.Text = "Csíkvágás eredmények";
        }


        dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
        dataGridView1.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView1_ColumnHeaderMouseClickForDgv1);
        dataGridView1.CellMouseClick += new DataGridViewCellMouseEventHandler(dataGridRoll_CellMouseClick1);
        this.dataGridView1.CellMouseDown +=new DataGridViewCellMouseEventHandler(dataGridView1_CellMouseDownForDgv1);    
               

        foreach (DataGridViewRow row in dataGridView1.Rows)
            if (row.Cells[2].Value.ToString() == "Nem megfelelő")
            {
                row.DefaultCellStyle.BackColor = Color.Red;
            }
            else
                row.DefaultCellStyle.BackColor = Color.Green;

        this.MinimizeBox = false;
        this.MaximizeBox = false;
        dataGridView1.Size = new Size(dataGridView1.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + dataGridView1.FirstDisplayedScrollingColumnHiddenWidth + dataGridView1.FirstDisplayedCell.OwningColumn.Width, dataGridView1.Rows.GetRowsHeight(DataGridViewElementStates.Visible) + dataGridView1.ColumnHeadersHeight + dataGridView1.ColumnHeadersHeight);
        dataGridView1.LayoutEngine.Layout(dataGridView1, new LayoutEventArgs(dataGridView1, "Size"));        
        dataGridView1.Dock = DockStyle.Top;
        dataGridView1.AutoSize = false;
        dataGridView1.DefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
        this.AutoSize = false;
        //this.Size = new Size(this.ClientSize.Width,this.ClientSize.Height+20);       
        this.Size = new Size(this.dataGridView1.Width, this.dataGridView1.Height + 20);       
        this.StartPosition = FormStartPosition.CenterScreen;
    //    this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, Screen.PrimaryScreen.WorkingArea.Height / 2);


        dataGridView1.PerformLayout();
        this.PerformLayout();


        //Is this needed??
        SetButtonOk();

    }

    void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Adatok mentése(xls)")
        {
            DataGridViewCell c = (sender as DataGridView)[e.ColumnIndex, e.RowIndex];
            if (!c.Selected)
            {
                c.DataGridView.ClearSelection();
                c.DataGridView.CurrentCell = c;
                c.Selected = true;
            }

            selected_lot_id = dataGridView1.Rows[e.RowIndex].Cells["Lot ID"].Value;
            selected_roll_id = dataGridView1.Rows[e.RowIndex].Cells["Roll ID"].Value;
            selected_result = dataGridView1.Rows[e.RowIndex].Cells["Eredmény"].Value;
            selected_measurytype = dataGridView1.Rows[e.RowIndex].Cells["Mérés típusa"].Value;


            showdata_item_Click(sender,EventArgs.Empty);


            ExportDataToCSV();
        }
    }

    protected virtual bool IsFileinUse(FileInfo file)
    {
        FileStream stream = null;

        try
        {
            stream = file.Open(FileMode.Open, FileAccess.ReadWrite);
        }
        catch (IOException)
        {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
          
            return true;
        }
        finally
        {
            if (stream != null)
                stream.Close();
        }
        return false;
    }


    public StreamWriter wr;
    public void ExportDataToCSV()
    {

        #region Test result to csv
        int cols;       
           try{

                //open file 
               using (StreamWriter wr = new StreamWriter(string.Format("{0}_{1}.csv", selected_lot_id, Program.measureType)))
               {

                   //determine the number of columns and write columns to file 
                   cols = dataGridView1.Columns.Count;
                   for (int i = 0; i < cols; i++)
                   {
                       wr.Write(dataGridView1.Columns[i].Name.ToString().ToUpper() + ";");
                   }
                   wr.WriteLine();

                   //write rows to excel file
                   for (int i = 0; i < (dataGridView1.Rows.Count); i++)
                   {
                       for (int j = 0; j < cols; j++)
                       {
                           if (dataGridView1.Rows[i].Cells[j].Value != null)
                           {
                               wr.Write(dataGridView1.Rows[i].Cells[j].Value + ";");
                           }
                           else
                           {
                               wr.Write(";");
                           }
                       }
                       wr.WriteLine();
                       wr.WriteLine();
                   }

                   //close file
                   //DO NOT Close the writer to avoid one of the two table's data lost
                   //wr.Close();

                   wr.WriteLine();

                   //determine the number of columns and write columns to file 
                   cols = AllDataTable.dataGridAll.Columns.Count;
                   for (int i = 0; i < cols; i++)
                   {
                       wr.Write(AllDataTable.dataGridAll.Columns[i].Name.ToString().ToUpper() + ";");
                   }
                   wr.WriteLine();

                   //write rows to csv file
                   for (int i = 0; i < AllDataTable.dataGridAll.Rows.Count; i++)
                   {
                       for (int j = 0; j < cols; j++)
                       {
                           if (AllDataTable.dataGridAll.Rows[i].Cells[j].Value != null)
                           {
                               wr.Write(AllDataTable.dataGridAll.Rows[i].Cells[j].Value + ";");
                           }
                           else
                           {
                               wr.Write(";");
                           }
                       }

                       wr.WriteLine();
                   }

                   //close file
                   wr.Close();
               }
           }catch(FileLoadException ex)
           {
             MessageBox.Show(string.Format("Nem nyitható meg a fájl:{0}_{1}.csv\nAz összesített eredmények térolása sikertelen", selected_lot_id, Program.measureType));
               
           }
        #endregion

            #region All result to csv
            cols =0;
        

      

       MessageBox.Show(string.Format("{0}_{1}_{2}.csv file created successfull", selected_lot_id, Program.measureType,DateTime.Now.Millisecond));
                
    }
            
    
           
        
         
        #endregion
    

#endregion


    public void AdjustWidth()
    {
        Control horizontal = dataGridView1.Controls[0]; // Horizontal scroll bar.
        Control vertical = dataGridView1.Controls[1]; // Vertical scroll bar.
        dataGridView1.Width = dataGridView1.PreferredSize.Width - vertical.Width + 1;
        dataGridView1.Height = dataGridView1.PreferredSize.Height - horizontal.Height + 1;
    }

    public void AdjustWidth(bool is_LOT)
    {
        Control horizontal = dataGridLOT.Controls[0]; // Horizontal scroll bar.
        Control vertical = dataGridLOT.Controls[1]; // Vertical scroll bar.
        dataGridLOT.Width = dataGridLOT.PreferredSize.Width - vertical.Width + 1;
        dataGridLOT.Height = dataGridLOT.PreferredSize.Height - horizontal.Height + 1;
    }




    
    public void ExportDataToCSV_Homogenity()
    {

        #region Test result to csv
        int cols;
        try
        {

            //open file 
            using (StreamWriter wr = new StreamWriter(string.Format("{0}_{1}.csv", selected_lot_id, Program.measureType)))
            {

                //determine the number of columns and write columns to file 
                cols = dataGridViewHomo.Columns.Count;
                for (int i = 0; i < cols; i++)
                {
                    wr.Write(dataGridViewHomo.Columns[i].Name.ToString().ToUpper() + ";");
                }
                wr.WriteLine();

                //write rows to excel file
                for (int i = 0; i < (dataGridViewHomo.Rows.Count); i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (dataGridViewHomo.Rows[i].Cells[j].Value != null)
                        {
                            wr.Write(dataGridViewHomo.Rows[i].Cells[j].Value + ";");
                        }
                        else
                        {
                            wr.Write(";");
                        }
                    }
                    wr.WriteLine();
                    wr.WriteLine();
                }

                //close file
                //DO NOT Close the writer to avoid one of the two table's data lost
                //wr.Close();

                wr.WriteLine();

                //determine the number of columns and write columns to file 
                cols = AllDataTable.dataGridAll.Columns.Count;
                for (int i = 0; i < cols; i++)
                {
                    wr.Write(AllDataTable.dataGridAll.Columns[i].Name.ToString().ToUpper() + ";");
                }
                wr.WriteLine();

                //write rows to csv file
                for (int i = 0; i < AllDataTable.dataGridAll.Rows.Count; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (AllDataTable.dataGridAll.Rows[i].Cells[j].Value != null)
                        {
                            wr.Write(AllDataTable.dataGridAll.Rows[i].Cells[j].Value + ";");
                        }
                        else
                        {
                            wr.Write(";");
                        }
                    }

                    wr.WriteLine();
                }

                //close file
                wr.Close();
            }
        }
        catch (FileLoadException ex)
        {
            MessageBox.Show(string.Format("Nem nyitható meg a fájl:{0}_{1}.csv\nAz összesített eredmények térolása sikertelen", selected_lot_id, Program.measureType));

        }
        #endregion

        #region All result to csv
        cols = 0;




        MessageBox.Show(string.Format("{0}_{1}_{2}.csv file created successfull", selected_lot_id, Program.measureType, DateTime.Now.Millisecond));

    }
        /// <summary>
        /// Cellclick handler for homo values stored
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
    void dataGridViewHomo_CellClick(object sender, DataGridViewCellEventArgs e)
    {

        Trace.TraceInformation("Cellclick handler for homogenity result");

        if (dataGridViewHomo.Columns[e.ColumnIndex].HeaderText == "Adatok mentése(xls)")
        {
            DataGridViewCell c = (sender as DataGridView)[e.ColumnIndex, e.RowIndex];
            if (!c.Selected)
            {
                c.DataGridView.ClearSelection();
                c.DataGridView.CurrentCell = c;
                c.Selected = true;
            }

            selected_lot_id = dataGridViewHomo.Rows[e.RowIndex].Cells["Lot ID"].Value;
            selected_roll_id = dataGridViewHomo.Rows[e.RowIndex].Cells["Roll ID"].Value;
            selected_result = dataGridViewHomo.Rows[e.RowIndex].Cells["Eredmény"].Value;
            selected_measurytype = dataGridViewHomo.Rows[e.RowIndex].Cells["Mérés típusa"].Value;


            showdata_item_Click(sender, EventArgs.Empty);


            ExportDataToCSV_Homogenity();
        }
    }
    /// <summary>
        /// After homo stored    
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
    private void EnumsAndComboBox_Load_After_h_stored(object sender, System.EventArgs e)
    {
        #region add columns to the datagridview
        // Initialize the DataGridView.
        dataGridViewHomo.AutoGenerateColumns = false;
        dataGridViewHomo.AutoSize = true;
        dataGridViewHomo.DataSource = bindingSource1;


        DataGridViewColumn column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "LotID";
        column.Name = "Lot ID";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewHomo.Columns.Add(column);


        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "RollID";
        column.Name = "Roll ID";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewHomo.Columns.Add(column);

        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "After_H_MeasureType";
        column.Name = "Mérés típusa";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewHomo.Columns.Add(column);

        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "Result";
        column.Name = "Eredmény";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewHomo.Columns.Add(column);

        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "After_H_AVG";
        column.Name = "Átlag";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewHomo.Columns.Add(column);

        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "After_H_AVG_ok";
        column.Name = "Átlag Értékelés";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewHomo.Columns.Add(column);

        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "After_H_CV";
        column.Name = "CV";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewHomo.Columns.Add(column);

        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "After_H_CV_ok";
        column.Name = "CV Értékelés";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewHomo.Columns.Add(column);

        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "Date";
        column.Name = "Dátum";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewHomo.Columns.Add(column);


        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "After_H_OutOfRange";
        column.Name = "Kieső csíkszám(glucose)";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewHomo.Columns.Add(column);


        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "Homo_h62";
        column.Name = "H62 hibák száma";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewHomo.Columns.Add(column);

        column = new DataGridViewTextBoxColumn();
        column.DataPropertyName = "Homo_not_h62";
        column.Name = "Nem H62 hibák száma";
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewHomo.Columns.Add(column);

        btnColumn = new DataGridViewButtonColumn();
        btnColumn.HeaderText = "Adatok mentése(xls)";
        btnColumn.Text = "Adatok mentése(xls)";
        btnColumn.UseColumnTextForButtonValue = true;
        btnColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewHomo.Columns.Add(btnColumn);

        // Initialize the form.
        #region Init the form and the datagrid

        dataGridViewHomo.Dock = DockStyle.Fill;       
        this.Controls.Add(dataGridViewHomo);
        this.MinimizeBox = false;
        this.MaximizeBox = false;
        dataGridViewHomo.Size = new Size(dataGridViewHomo.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + dataGridViewHomo.FirstDisplayedScrollingColumnHiddenWidth + dataGridViewHomo.FirstDisplayedCell.OwningColumn.Width, dataGridViewHomo.Rows.GetRowsHeight(DataGridViewElementStates.Visible) + dataGridViewHomo.ColumnHeadersHeight + dataGridViewHomo.ColumnHeadersHeight);
        dataGridViewHomo.LayoutEngine.Layout(dataGridViewHomo, new LayoutEventArgs(dataGridViewHomo, "Size"));
        dataGridViewHomo.Dock = DockStyle.Top;
        dataGridViewHomo.AutoSize = false;
        this.Size = new Size(dataGridViewHomo.Width + 20, dataGridViewHomo.Height + 20);
       // this.Size = new Size(this.ClientSize.Width, this.ClientSize.Height + 20);
        this.StartPosition = FormStartPosition.Manual;
        this.Location = new Point(0, 0);

        #endregion


        if (Program.measureType == "blank")
        {
            this.Text = "Blank Teszt eredményei";
        }
        else if (Program.measureType == "homogenity")
        {
            this.Text = "Homogenitás Teszt eredményei";
        }
        else if (Program.measureType == "show")
        {
            this.Text = "Csíkvágás eredmények";
        }

        dataGridViewHomo.CellClick += new DataGridViewCellEventHandler(dataGridViewHomo_CellClick);
        dataGridViewHomo.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView1_ColumnHeaderMouseClickForDgv1);
   //     dataGridRoll.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridViewHomo_ColumnHeaderMouseClickRoll);
    //  AllDataTable.dataGridAll.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridViewHomo_ColumnHeaderMouseClickedForAll);
    //  AllDataTable.dataGridAll.CellMouseClick += new DataGridViewCellMouseEventHandler(dataGridRoll_CellMouseClickAll);
      //  dataGridRoll.CellMouseClick += new DataGridViewCellMouseEventHandler(dataGridRoll_CellMouseClickForRoll);
        this.dataGridViewHomo.CellMouseDown += new DataGridViewCellMouseEventHandler(dataGridView1_CellMouseDownForDgvHomo);
       
        Trace.TraceError("Click Listeners activated");

        foreach (DataGridViewRow row in dataGridViewHomo.Rows)
            if (Convert.ToString(row.Cells["Eredmény"].Value) == "Nem Megfelelő")
            {
                row.DefaultCellStyle.BackColor = Color.Red;
            }
            else
                row.DefaultCellStyle.BackColor = Color.Green;

        dataGridViewHomo.PerformLayout();
        this.PerformLayout();

        ///Add OK button to the form
       // SetButtonOk();
    }

  public void dataGridRoll_CellMouseClickAll(object sender, DataGridViewCellMouseEventArgs e)
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

            selected_lot_id = AllDataTable.dataGridAll.Rows[e.RowIndex].Cells["Lot azonosító"].Value;
            selected_roll_id = AllDataTable.dataGridAll.Rows[e.RowIndex].Cells["Roll azonosító"].Value;
            selected_result = AllDataTable.dataGridAll.Rows[e.RowIndex].Cells["Eredmény"].Value;
            selected_measurytype = AllDataTable.dataGridAll.Rows[e.RowIndex].Cells["Mérés típusa"].Value;


            ShowContextMenu(c, selected_lot_id, selected_roll_id, selected_result, selected_measurytype);
        }
    }

    void dataGridRoll_CellMouseClick1(object sender, DataGridViewCellMouseEventArgs e)
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

            selected_lot_id = dataGridView1.Rows[e.RowIndex].Cells["Lot ID"].Value;
            selected_roll_id = dataGridView1.Rows[e.RowIndex].Cells["Roll ID"].Value;
            selected_result = dataGridView1.Rows[e.RowIndex].Cells["Eredmény"].Value;
            selected_measurytype = dataGridView1.Rows[e.RowIndex].Cells["Mérés típusa"].Value;


            ShowContextMenu(c, selected_lot_id, selected_roll_id, selected_result, selected_measurytype);
        }
    }

   #region Methods


    void dataGridRoll_CellMouseClickForRoll(object sender, DataGridViewCellMouseEventArgs e)
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


    /// <summary>
    /// if right mouseclick happened then a local contextmenu shown on datagridview1
    /// 
    /// //this runs after blank stored then all data show called
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void dataGridView1_CellMouseDownForDgv1(object sender, DataGridViewCellMouseEventArgs e)
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

            selected_lot_id = dataGridView1.Rows[e.RowIndex].Cells["Lot ID"].Value;
            selected_roll_id = dataGridView1.Rows[e.RowIndex].Cells["Roll ID"].Value;
            selected_result = dataGridView1.Rows[e.RowIndex].Cells["Eredmény"].Value;
            selected_measurytype = dataGridView1.Rows[e.RowIndex].Cells["Mérés típusa"].Value;


            ShowContextMenu(c, selected_lot_id, selected_roll_id, selected_result, selected_measurytype);

        }
    }

    /// <summary>
    /// if right mouseclick happened then a local contextmenu shown on datagridviewHomo
    /// 
    /// //this runs after homo stored then all data show called
    /// </summary>
    /// <param name="sender"></param>    
        DataGridViewColumn column = new DataGridViewTextBoxColumn();       
    /// <param name="e"></param>
    public void dataGridView1_CellMouseDownForDgvHomo(object sender, DataGridViewCellMouseEventArgs e)
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

            selected_lot_id = dataGridViewHomo.Rows[e.RowIndex].Cells["Lot ID"].Value;
            selected_roll_id = dataGridViewHomo.Rows[e.RowIndex].Cells["Roll ID"].Value;
            selected_result = dataGridViewHomo.Rows[e.RowIndex].Cells["Eredmény"].Value;
            selected_measurytype = dataGridViewHomo.Rows[e.RowIndex].Cells["Mérés típusa"].Value;


            ShowContextMenu(c, selected_lot_id, selected_roll_id, selected_result, selected_measurytype);

        }
    }

    /// <summary>
        /// if right mouseclick happened then a local contextmenu shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
    public void  dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
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

                         selected_lot_id=AllDataTable.dataGridAll.Rows[e.RowIndex].Cells["Lot ID"].Value;
                         selected_roll_id = AllDataTable.dataGridAll.Rows[e.RowIndex].Cells["Roll ID"].Value;
                         selected_result = AllDataTable.dataGridAll.Rows[e.RowIndex].Cells["Eredmény"].Value;
                         selected_measurytype = AllDataTable.dataGridAll.Rows[e.RowIndex].Cells["Mérés típusa"].Value;


                         ShowContextMenu(c,selected_lot_id,selected_roll_id,selected_result,selected_measurytype);
                        
            }
        }
       
        
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
            new Dialoge(selected_lot_id.ToString(),selected_roll_id.ToString(),measuretype).ShowDialog();

            this.WindowState = FormWindowState.Maximized;
            ///set the selected roll remeasured
            SetRemeasuredClickedRoll(Convert.ToString(selected_lot_id),Convert.ToString(selected_roll_id),Convert.ToString(selected_measurytype));

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
        /// Show all data item clicked on the contextmenu on a cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void showdata_item_Click(object sender, EventArgs e)
        {


            #region In case of selected row

            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();

                    ///Get values for blank results
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
                            #region get rows which are invalidated and skip that rows in the query to collect data
                            using (NpgsqlCommand getInvalidValuesfromResults =
                               new NpgsqlCommand(
                                   string.Format("select pk_id from blank_test_result where invalidate=false and remeasured=false and pk_id={0}", act_result_id), conn))
                            {
                                object res = null;
                                res = getInvalidValuesfromResults.ExecuteScalar();

                                if (res == DBNull.Value
                                    || (res == null))///Then no row is valid
                                {
                                    Trace.TraceWarning("Rows where result_id:{1} are invalidated in result, query:{0}", getInvalidValuesfromResults.CommandText, act_result_id);

                                }
                                else///the act row is valid get that
                                {

                                    Trace.TraceWarning("All needed rows are validated in identify, query{0}", getInvalidValuesfromResults.CommandText);

                                    using (NpgsqlCommand getValuesfromResults =
                                              new NpgsqlCommand(
                                                  string.Format("select * from blank_test_result where glu<>0 and invalidate=false and remeasured=false and pk_id={0}", act_result_id), conn))
                                    {


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

                            }

                        }//End of Foreach

                        new BlankChart(selected_serial_numbers.ToArray(),selected_nano_ampers.ToArray()).Show();

                        using (NpgsqlCommand getInvalidatedfromEnv =
                           new NpgsqlCommand(
                               string.Format("select pk_id from blank_test_environment where remeasured=false and invalidate=false and lot_id='{0}' and roll_id='{1}'", selected_lot_id, selected_roll_id), conn))
                        {
                            object res = null;
                            res = getInvalidatedfromEnv.ExecuteScalar();

                            if (res == DBNull.Value //the rows are invalid
                                || (res == null))
                            {
                                Trace.TraceWarning("Rows where result_id:{1} are invalidated in enivronment, query:{0}", getInvalidatedfromEnv.CommandText);


                            }
                            else//the rows are valid
                            {

                                Trace.TraceWarning("All needed rows are validated in identify, query{0}", getInvalidatedfromEnv.CommandText);
                                /* SELECT FROM blank_test_environment LEFT JOIN blank_test_result ON blank_test_environment.fk_blank_test_result_id = blank_test_result.pk_id 
                                 * where 
                                 * blank_test_environment.lot_id='lot1' and blank_test_environment.roll_id='1' and blank_test_result.glu<>0 */
                                using (NpgsqlCommand getValuesfromEnv =
                                     new NpgsqlCommand(
                                string.Format("select * FROM blank_test_environment LEFT JOIN blank_test_result ON blank_test_environment.fk_blank_test_result_id = blank_test_result.pk_id WHERE blank_test_environment.lot_id='{0}' and blank_test_environment.roll_id='{1}' and blank_test_result.glu<>0 ", selected_lot_id, selected_roll_id), conn))
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

                            #endregion

                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception in Get blank test results for show all:ResultsForm.showdata_item_click(),ex:{0}", ex);
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }///End of using NpgsqlConnection for blank

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
                        using (NpgsqlCommand get_ids = new NpgsqlCommand(string.Format("select distinct lot_id,roll_id from homogenity_result where lot_id='{0}' and invalidate=false", selected_lot_id), connect))
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
                                    Trace.TraceInformation(string.Format("Nincs Homogenitás teszt eredmény a {0} Lot-nál",selected_lot_id));
                                    no_homogenity = true;

                                }

                            }
                        }
                        #endregion

                        int i = 0;
                        #region there is homogenity result
                        if (!no_homogenity)
                        {

                            #region get homogenity result for one roll

                            Trace.TraceInformation("first get values from homo_res table");
                            foreach (string actroll in homogenity_roll_id)
                            {
                                using (NpgsqlCommand get_homogenity = new NpgsqlCommand(
                                    string.Format("select * from homogenity_result where invalidate=false and lot_id='{0}' and roll_id='{1}'", selected_lot_id, selected_roll_id), connect))
                                {
                                    using (NpgsqlDataReader dr = get_homogenity.ExecuteReader())
                                    {
                                        if (dr.HasRows)
                                        {
                                            while (dr.Read())
                                            {

                                               
                                                homogenity_avg.Add(Convert.ToDouble(dr["avg"]));
                                                homogenity_cv.Add(Convert.ToDouble(dr["cv"]));
                                                homogenity_is_valid.Add(Convert.ToBoolean(dr["homogenity_is_valid"]));
                                                homogenity_date.Add(Convert.ToDateTime(dr["date"]));
                                                homogenity_stddev.Add(Convert.ToDouble(dr["stddev"]));
                                                homogenity_out_of_range_strip_count.Add(Convert.ToInt32(dr["out_of_range_strip_count"]));
                                                homogenity_strip_count.Add(Convert.ToInt32(dr["strip_count_in_one_roll"]));
                                                homogenity_not_h62_count.Add(Convert.ToInt32(dr["not_h62_error_count"]));
                                                homogenity_h62_count.Add(Convert.ToInt32(dr["h62_errors_count"]));
                                            }
                                            dr.Close();
                                        }
                                        else
                                        {
                                            dr.Close();

                                        }

                                    }
                                }

                              


                                i++;

                            }///end of foreach
                            #endregion

                            ///Get values for homogemnity results
                            #region get values from identify table where lot,roll, measuretype is correct(result_id,serial_number)
                           
                            #endregion

                            #region get values from all table where selected test result can be found for homogenity test

                          //  foreach (int act_result_id in selected_roll_id)
                          //  {
                                #region get rows which are invalidated and skip that rows in the query to collect data

                                using (NpgsqlCommand getInvalidValuesfromResults =
                                   new NpgsqlCommand(
                                       string.Format("select pk_id from blank_test_result where invalidate=false  and lot_id='{0}'and roll_id='{1}'", selected_lot_id,selected_roll_id), connect))
                                {
                                    object res = null;
                                    res = getInvalidValuesfromResults.ExecuteScalar();

                                    if (res == DBNull.Value
                                        || (res == null))///Then no row is valid
                                    {
                                        Trace.TraceWarning("Rows where roll_id:{1} are invalidated in result, query:{0}", getInvalidValuesfromResults.CommandText, selected_roll_id);

                                    }
                                    else///the act row is valid get that
                                    {

                                        Trace.TraceWarning("All needed rows are validated in identify, query{0}", getInvalidValuesfromResults.CommandText);
                                        /*TODO:and (blank_test_result.glu<>0 or blank_test_errors.error_text<>'')*/
                                        using (NpgsqlCommand getValuesfromResults =
                                                  new NpgsqlCommand(
                                                      string.Format("select * from blank_test_result LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE blank_test_result.invalidate=False and blank_test_result.code=777 and blank_test_result.lot_id='{0}' and blank_test_result.roll_id='{1}'", selected_lot_id,selected_roll_id), connect))
                                        {
                                            using (NpgsqlDataReader dr = getValuesfromResults.ExecuteReader())
                                            {
                                                if (dr.HasRows)
                                                {
                                                    while (dr.Read())
                                                    {

                                                        homogenity_sn.Add(Convert.ToString(dr["sn"]));
                                                        homogenity_glus.Add(Convert.ToDouble(dr["glu"]));
                                                        homogenity_error.Add(Convert.ToString(dr["error"]));
                                                        homogenity_error_text.Add(Convert.ToString(dr["error_text"]));
                                                        homogenity_h62_value.Add(Convert.ToBoolean(dr["h62_error"]));
                                                        homogenity_not_h62_value.Add(Convert.ToBoolean(dr["not_h62_error"]));
                                                        homogenity_earlyDribble.Add(Convert.ToBoolean(dr["early_dribble"]));
                                                        homogenity_deviceReplace.Add(Convert.ToBoolean(dr["device_replace"]));
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

                               

                                
                                #endregion
                            
                            using (NpgsqlCommand getInvalidatedfromEnv =
                                  new NpgsqlCommand(
                                      string.Format("select pk_id from blank_test_environment where  invalidate=false and lot_id='{0}' and roll_id='{1}'", selected_lot_id, selected_roll_id), connect))
                            {
                                object res = null;
                                res = getInvalidatedfromEnv.ExecuteScalar();

                                if (res == DBNull.Value //the rows are invalid
                                    || (res == null))
                                {

                                    Trace.TraceWarning("Rows where roll_id:{1} are invalidated in enivronment, query:{0}", getInvalidatedfromEnv.CommandText,selected_roll_id);


                                }
                                else//the rows are valid
                                {

                                    Trace.TraceWarning("All needed rows are validated in identify, query{0}", getInvalidatedfromEnv.CommandText);
                                    /* SELECT FROM blank_test_environment LEFT JOIN blank_test_result ON blank_test_environment.fk_blank_test_result_id = blank_test_result.pk_id 
                                     * where 
                                     * blank_test_environment.lot_id='lot1' and blank_test_environment.roll_id='1' and blank_test_result.glu<>0 */
                                    using (NpgsqlCommand getValuesfromEnv =
                                         new NpgsqlCommand(
                                    string.Format("select * FROM blank_test_environment LEFT JOIN blank_test_result ON blank_test_environment.fk_blank_test_result_id = blank_test_result.pk_id WHERE blank_test_environment.lot_id='{0}'  and blank_test_environment.invalidate=false and blank_test_environment.roll_id='{1}' and blank_test_result.code=777 and blank_test_environment.invalidate=false", selected_lot_id, selected_roll_id), connect))
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
                                }
                          //  }

                        }///End of Has homogenity result//End of if (!no_homogenity)
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Exception in Get homogenity test results for show all:ResultsForm.showdata_item_click(),ex:{0}", ex);
                        throw;
                    }
                    finally
                    {
                        connect.Close();
                    }

                        #endregion
                }//NpgsqlConnection for homogenity                 
                #endregion

                Trace.TraceInformation("data collection finished... results form will be displayed ");

                #region Show collected data

                if (no_blank
                    && no_homogenity)
                {
                    new NoValidResults(selected_lot_id.ToString(),selected_roll_id.ToString()).Show();
                }
                CreateAllDataTable();

                        


                #endregion

                ///Displaying each measurement values in a chart
                
            
        }
            #endregion

        delegate void CloseAllDataTableDelegate();
            public void CloseAllDataTable()
            {
                if (AllDataTable.dataGridAll.InvokeRequired)
                {
                    AllDataTable.dataGridAll.Invoke(new CloseAllDataTableDelegate(CloseAllDataTable));
                }
                else if (OwnedForms.Length >= 1)
                {
                    AllDataTable.dataGridAll = null;

                    for (int i = 0; i < OwnedForms.Length; i++)
                    {
                        this.OwnedForms[i].Close();
                    }
                }

            }

        private static void CreateAllDataTable()
        {
            if (Program.measureType == "blank" 
                || Program.measureType=="show")
            {
                AllDataTable allTable;

                ///This datagrid shows all the measurement of the selected roll in case of BLANK
                allTable=new AllDataTable(selected_result, selected_lot_id, selected_roll_id, selected_serial_numbers,
                    selected_users, selected_computers, selected_error, selected_error_text, selected_glus, selected_nano_ampers,
                    selected_not_h62, selected_h62, selected_start_dates, selected_end_dates);

            //    new RollMeanBlankCurrent(selected_nano_ampers,selected_serial_numbers).ShowDialog();

                allTable.Show();
                allTable.Dock = DockStyle.Top;
                allTable.BringToFront();
                allTable.Focus();
            }
            else if (Program.measureType == "homogenity")
            {
                AllDataTable allTable;

                ///This datagrid shows all the measurement of the selected roll in case of HOMOGENITY
                allTable = new AllDataTable(selected_lot_id.ToString(), selected_roll_id.ToString(), homogenity_sn,
                     homogenity_start_dates,
                     homogenity_error, homogenity_error_text, homogenity_users, homogenity_computers, homogenity_glus, homogenity_not_h62_value, homogenity_h62_value);


                Trace.TraceInformation("Show allDatatable for Homogenity");
               allTable.ShowDialog();
               allTable.Dock = DockStyle.Top;
               allTable.BringToFront();
               allTable.Focus();
            }
           


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
        /// <parstatic am name="selected_end_dates"></param>
        private static void ClearGridVariables(object selected_result, object selected_lot_id, object selected_roll_id, List<string> selected_serial_numbers, 
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
        public DataGridViewCell currCell;
        private static bool no_blank=false;
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
                    values.Add(Convert.ToDouble(row.Cells[column_index].Value));

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
                    values.Add(Convert.ToDouble(row.Cells[column_index].Value));

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
                    values.Add(Convert.ToDouble(row.Cells[column_index].Value));

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
                    }///ha 1-10 számjegyből álló a string lehet lot_id,roll_id,sn
                    else if (idReg.IsMatch((Convert.ToString(item))))
                    {           ///csak 1,2 vagy 3 db számból áll
                        if (new Regex("^[0-9]{1,3}$").IsMatch(Convert.ToString(item)))
                        {
                            ///pontosan 3 számból áll (lehet code)
                            if (new Regex("^[0-9]{3}$").IsMatch(Convert.ToString(item)))
                            {

                            }

                        }
                    }       ///csak betűkből áll, nincs benne szóköz vagy más karakter:username,measure_type
                    else if (new Regex("^[a-z]?$").IsMatch(Convert.ToString(item)))
                    {
                        ///csak b-vel vagy h-val kezdődő és k val vagy y-al végződő betűket tartalmazó string:measure_type
                        if (new Regex("^[b|h][a-z]*[k|y]$").IsMatch(Convert.ToString(item))
                            && new Regex("[a-z]{5}").IsMatch(Convert.ToString(item)))//csak blank
                        {
                            measure_type.Add("blank");


                        }
                        else if (new Regex("^[b|h][a-z]*[k|y]$").IsMatch(Convert.ToString(item))
                            && new Regex("[a-z]{9}").IsMatch(Convert.ToString(item)))//csak homogenity
                        {
                            measure_type.Add("homogenity");
                        }
                    }
                }
            }
            else if (dateType == row_number)
            {
                tsp = Convert.ToDateTime(values[0]).Date - Convert.ToDateTime(values[row_number]).Date;
            }
            #endregion

            new CalculatedColumnValuesForm(avg, sd, cv,
                int_avg,summary_ints, accepted_count,not_accepted_count,
                not_h62d_error_count,h62d_error_count,
                tsp,dataGridRoll.Columns[column_index].Name).Show();


        }

        /// <summary>
        /// The click on the header of the dgv changes the sort of the items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_ColumnHeaderMouseClickRoll(object sender, DataGridViewCellMouseEventArgs e)
        {

            ///Firtst check if the click was  sended by rightmouse-button
            if (e.ColumnIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ShowContextMenu(dataGridRoll.Columns[e.ColumnIndex].HeaderCell);

            }

            //get the current column details
            string strColumnName = dataGridRoll.Columns[e.ColumnIndex].Name;
            SortOrder strSortOrder = getSortOrderRoll(e.ColumnIndex);

            ///  AllDataTable.dataGridAll.Sort(AllDataTable.dataGridAll.Columns[e.ColumnIndex],ListSortDirection.Ascending);

            dataGridRoll.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = strSortOrder;
        }


        /// <summary>
        /// The click on the header of the dgv changes the sort of the items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_ColumnHeaderMouseClickForDgv1(object sender, DataGridViewCellMouseEventArgs e)
        {

            ///Firtst check if the click was  sended by rightmouse-button
            if (e.ColumnIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ShowContextMenu(dataGridViewHomo.Columns[e.ColumnIndex].HeaderCell);

            }

            //get the current column details
            string strColumnName = dataGridViewHomo.Columns[e.ColumnIndex].Name;
            SortOrder strSortOrder = getSortOrder1(e.ColumnIndex);

            ///  AllDataTable.dataGridAll.Sort(AllDataTable.dataGridAll.Columns[e.ColumnIndex],ListSortDirection.Ascending);

            dataGridViewHomo.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = strSortOrder;
        }

        /// <summary>
        /// Get the current sort order of the column and return it
        /// set the new SortOrder to the columns.
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns>SortOrder of the current column</returns>
        private SortOrder getSortOrderAll(int columnIndex)
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
        /// Get the current sort order of the column and return it
        /// set the new SortOrder to the columns.
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns>SortOrder of the current column</returns>
        private SortOrder getSortOrderRoll(int columnIndex)
        {
            if (dataGridRoll.Columns[columnIndex].HeaderCell.SortGlyphDirection == SortOrder.None ||
                dataGridRoll.Columns[columnIndex].HeaderCell.SortGlyphDirection == SortOrder.Descending)
            {
                dataGridRoll.Columns[columnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                return SortOrder.Ascending;
            }
            else
            {
                dataGridRoll.Columns[columnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                return SortOrder.Descending;
            }
        }

        /// <summary>
        /// Get the current sort order of the column and return it
        /// set the new SortOrder to the columns.
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns>SortOrder of the current column</returns>
        private SortOrder getSortOrder1(int columnIndex)
        {
            if (dataGridView1.Columns[columnIndex].HeaderCell.SortGlyphDirection == SortOrder.None ||
                dataGridView1.Columns[columnIndex].HeaderCell.SortGlyphDirection == SortOrder.Descending)
            {
                dataGridView1.Columns[columnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                return SortOrder.Ascending;
            }
            else
            {
                dataGridView1.Columns[columnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                return SortOrder.Descending;
            }
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
        /// The click on the header of the dgv changes the sort of the items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dataGridView1_ColumnHeaderMouseClickedForRoll(object sender, DataGridViewCellMouseEventArgs e)
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
            SortOrder strSortOrder = getSortOrderRoll(e.ColumnIndex);

            ///  dataGridAll.Sort(dataGridAll.Columns[e.ColumnIndex],ListSortDirection.Ascending);

      //dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = strSortOrder;
            dataGridRoll.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = strSortOrder;
        }

        /// <summary>
        /// The click on the header of the dgv changes the sort of the items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dataGridView1_ColumnHeaderMouseClickedForAll(object sender, DataGridViewCellMouseEventArgs e)
        {

            ///Firtst check if the click was  sended by rightmouse-button
            if (e.ColumnIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //ShowContextMenu(dataGridView1.Columns[e.ColumnIndex].HeaderCell);
                ShowContextMenu(AllDataTable.dataGridAll.Columns[e.ColumnIndex].HeaderCell);

            }

            //get the current column details
            //string strColumnName = dataGridView1.Columns[e.ColumnIndex].Name;
            string strColumnName = AllDataTable.dataGridAll.Columns[e.ColumnIndex].Name;
            SortOrder strSortOrder = getSortOrderAll(e.ColumnIndex);

            ///  dataGridAll.Sort(dataGridAll.Columns[e.ColumnIndex],ListSortDirection.Ascending);

            //dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = strSortOrder;
            AllDataTable.dataGridAll.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = strSortOrder;
        }


   #endregion


        /// <summary>
       /// Set Invalidate(Remeasured) the results of the roll in case of selected from the list, and start a new measurement 
       /// </summary>
     public void SetRemeasuredClickedRoll(string lot_id,string roll_id,string measuretype)
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
            using (NpgsqlConnection conn=new NpgsqlConnection(Program.dbConnection))
                {
                    try
                    {
                        conn.Open();

                        #region set remeasured to true at averages row

                        using (NpgsqlCommand invalidate = new NpgsqlCommand(
                            string.Format("UPDATE blank_test_averages SET remeasured=true WHERE lot_id='{0}' and roll_id='{1}'", selected_lot_id, roll_id), conn))
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
                            string.Format("UPDATE blank_test_identify SET remeasured=true WHERE lot_id='{0}' and roll_id='{1}' and measure_type='{2}'",selected_lot_id,roll_id,measuretype), conn))
                        {
                            object res = null;
                           res = invalidate.ExecuteNonQuery();

                            if (res == DBNull.Value
                                || res == null)
                            {
                                Trace.TraceError("null value for invalidate: {0}",invalidate.CommandText);
                                throw new ArgumentException("null value for invalidate: {0}", invalidate.CommandText);
                            }

                            Trace.TraceInformation("Invalidated roll in identify: {0}",remeasured_rollid);
                        }
                        #endregion

                        #region get result pkid columns
                        using (NpgsqlCommand invalidate = new NpgsqlCommand(
                            string.Format("SELECT fk_blank_test_result_id from blank_test_identify WHERE lot_id='{0}' and roll_id='{1}' and measure_type='{2}'", selected_lot_id, roll_id,measuretype), conn))
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
                                string.Format("UPDATE blank_test_environment SET remeasured=true WHERE lot_id='{0}' and roll_id='{1}' and fk_blank_test_result_id={2}", selected_lot_id, roll_id, pk), conn))
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
                                string.Format("SELECT fk_blank_test_errors_id from blank_test_result WHERE lot_id='{0}' and roll_id='{1}' and pk_id={2}", selected_lot_id, roll_id, pk), conn))
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
                            using (NpgsqlCommand invErrors = new NpgsqlCommand(string.Format("update blank_test_errors set remeasured=true WHERE pk_id={0}",actpk), conn))
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
                                string.Format("UPDATE homogenity_result SET remeasured = true WHERE lot_id = '{0}' and roll_id='{1}'", selected_lot_id, roll_id), conn))
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
                                string.Format("UPDATE homogenity_test SET remeasured = true WHERE lot_id = '{0}' and roll_id='{1}'", selected_lot_id, roll_id), conn))
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
                                string.Format("UPDATE roll_result SET remeasured = true WHERE lot_id = '{0}' and roll_id='{1}'", selected_lot_id, roll_id), conn))
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


        /// <summary>
        /// Add the button OK to the form
        /// </summary>
     public void SetButtonOk()
    {
       
        btOK.Location=this.DesktopLocation;
         btOK.Name = "button1";
         btOK.Size = new System.Drawing.Size(75, 23);
         btOK.TabIndex = 2;
         btOK.Text = "OK";
         btOK.UseVisualStyleBackColor = true;
         btOK.Click += new System.EventHandler(this.button1_Click);
         this.Controls.Add(btOK);
     }


     /// <summary>
     /// Click on close button
     /// </summary>
     /// <param name="sender"></param>
     /// <param name="e"></param>
     private void button1_Click(object sender, EventArgs e)
     {
         this.Close();
     }
     #endregion

           

        /// <summary>
        /// Close the form when ESC pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
     private void ResultForm_KeyDown(object sender, KeyEventArgs e)
     {
         if (e.KeyCode == Keys.Escape)
         {

             this.Close();

             
         }

     }
   #endregion

    }
  #endregion
        #endregion
        #endregion

}
