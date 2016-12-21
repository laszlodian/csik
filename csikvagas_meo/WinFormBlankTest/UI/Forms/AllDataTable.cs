using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.Remoting.Messaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;
using Npgsql;
using e77.MeasureBase;
using WinFormBlankTest.UI.Chart;

namespace WinFormBlankTest.UI.Forms
{
    public partial class AllDataTable : Form
    {

        #region Variables

        public Color homogenity_color;
        public int h62_error_count = 0;
        public int not_h62_error_count = 0;
        public BindingSource bindingSrc = new BindingSource();
        public BindingSource bindingSource1 = new BindingSource();
       public DataGridViewButtonCell[] dgvCells = new DataGridViewButtonCell[] { };
       public DataGridViewRow buttonRow = new DataGridViewRow();
        
        object remeasured_lotid;
        object remeasured_rollid;
       string roll_to_invalidate;
        string lot_to_invalidate;
        string measure_to_invalidate;

        public object selected_sn_id;
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

        MenuItem hide_item = new MenuItem();
        MenuItem calculate_item = new MenuItem();
        MenuItem color_item = new MenuItem();
        public object selected_lot_id;
        public object selected_roll_id;
        public object selected_result;
        public object selected_measurytype;
        public MenuItem object_item;
        public MenuItem remeasure_item;
        public MenuItem showdata_item;

        List<int> invalidate_result_pkid = new List<int>();
        List<int> invalidate_errors_pkid = new List<int>();

        public string lot_id;
        public string roll_id;
        public string measuretype;

        public TimeSpan tsp = new TimeSpan();
        public int column_number=0;
        public List<string> measure_type = new List<string>();
        public int dateType = 0;
        public int column_index=0;
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
    
        public int valueForStore;
        public int i;
        public string lotid;
        public string rollid;
        protected static int[] valuesNeededForChart = new int[20];
        private DataGridView dataGridIdentify = new DataGridView();
        public static DataGridView dataGridAll = new DataGridView();
        public static DataGridView dataGridButton = new DataGridView();
        private BindingSource bindingSourceAll = new BindingSource();
        private BindingSource bindingSourceIdentify = new BindingSource();

        #endregion


        /// <summary>
        /// /Homogenity results need to collect another way so need another constructor
        /// </summary>
        /// <param name="result"></param>
        /// <param name="lot"></param>
        /// <param name="roll"></param>
        /// <param name="sn"></param>
        /// <param name="user"></param>
        /// <param name="comp"></param>
        /// <param name="error"></param>
        /// <param name="error_text"></param>
        /// <param name="glu"></param>
        /// <param name="nano_amper"></param>
        /// <param name="not_h62"></param>
        /// <param name="h62"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// 
      public AllDataTable(string lot, string roll, List<string> sn,  List<DateTime> h_date,
            List<string> h_error, List<string> error_text, List<string> user, List<string> computer,
            List<double> glu, List<bool> not_h62_value, List<bool> h62_value)
        {
            InitializeComponent();

            lotid = lot.ToString();
            rollid = roll.ToString();

            i = 0;
            try
            {
                foreach (string act_sn in sn)
                {
                    Trace.TraceInformation("hdate time:{0}", h_date[i]);

                    bindingSourceAll.Add(
                        new ShowAllData(lotid, rollid, act_sn,
                            user[i], computer[i], h_error[i],
                            error_text[i], glu[i],
                            not_h62_value[i], h62_value[i],
                            h_date[i]));
                    i++;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Exception in AllDataTable ex:{0}",ex.StackTrace);
            }
            ///Set the value of these variables to null instead of the next right click will thrown an exception!!
            ClearAllDataGridData(lot, roll, sn,h_date,
                        user, computer, h_error,
                        error_text, glu,
                        not_h62_value, h62_value );
      
            AllDataTable.dataGridAll.Font = new Font(AllDataTable.dataGridAll.Font, FontStyle.Bold);
            if (ResultForm.selected_measurytype.ToString() == "Homogenity Check")
            {
                Load += new System.EventHandler(EnumsAndComboBox_Load);
            }
            else if (ResultForm.selected_measurytype.ToString() == "Blank Check")
                Load += new System.EventHandler(EnumsAndComboBox_Load_Blank);



            SetDesktopLocation(Screen.PrimaryScreen.WorkingArea.Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2);
            AllDataTable.dataGridAll.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;


        }








        /// <summary>
        /// After one row is selected from resultform then all data will be displayed which is in a relationship with that test
        /// </summary>
        /// <param name="result"></param>
        /// <param name="lot"></param>
        /// <param name="roll"></param>
        /// <param name="sn"></param>
        /// <param name="user"></param>
        /// <param name="comp"></param>
        /// <param name="error"></param>
        /// <param name="error_text"></param>
        /// <param name="glu"></param>
        /// <param name="nano_amper"></param>
        /// <param name="not_h62"></param>
        /// <param name="h62"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public AllDataTable(object result,object lot, object roll, List<string> sn, List<string> user, List<string> comp, List<string> error, List<string> error_text,
             List<double> glu, List<double> nano_amper, List<bool> not_h62, List<bool> h62, List<DateTime> start, List<DateTime> end)
        {
            if (dataGridAll.RowCount >= 1)
            {
                dataGridAll = null;
                dataGridAll.Dispose();
            }
            
            InitializeComponent();

            lotid = lot.ToString();
            rollid = roll.ToString();

            i = 0;
         /*   foreach (string act_sn in sn)
            {
                Trace.TraceInformation("start time:{0}",start[i]);
                bindingSourceAll.Add(
                    new ShowAllData(lotid, rollid,  act_sn,
                        user[i], comp[i], glu[i],nano_amper[i],start[i],end[i]));
                
                

                i++;
            }*/
           
          
          




            ///Set the value of these variables to null instead of the next right click will thrown an exception!!
            ClearAllDataGridData(lot, roll,  sn,
                        user, comp, error,error_text,glu,nano_amper,not_h62,h62,start,end);

            AllDataTable.dataGridAll.Font = new Font(AllDataTable.dataGridAll.Font, FontStyle.Bold);
            FormClosing += new FormClosingEventHandler(AllDataTable_FormClosing);


            if (ResultForm.selected_measurytype.ToString()=="Homogenity Check")
            {
                Load += new System.EventHandler(EnumsAndComboBox_Load);
            }
            else if (ResultForm.selected_measurytype.ToString() == "Blank Check")
                Load += new System.EventHandler(EnumsAndComboBox_Load_Blank);

           
        }

        void AllDataTable_FormClosing(object sender, FormClosingEventArgs e)
        {

            selected_lot_id = null;
            selected_roll_id = null;
            selected_sn_id = null;
            ClearPropertiesShowAllData();

            dataGridAll.Dispose();
            dataGridAll = null;
            Controls.Remove(dataGridAll);
            if (OwnedForms.Length >= 1)
            {
                OwnedForms[i].Close();    
            }
            
        }

        private void ClearPropertiesShowAllData()
        {
            ShowAllData.ClearAllProperty();
        }

       

        /// <summary>
        /// Set invalidate the value of these variables to at when the next click happened an empty list can get values from db_tables IN CASE OF BLANK TEST
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="roll"></param>
        /// <param name="sn"></param>
        /// <param name="user"></param>
        /// <param name="comp"></param>
        /// <param name="error"></param>
        /// <param name="error_text"></param>
        /// <param name="glu"></param>
        /// <param name="nano_amper"></param>
        /// <param name="not_h62"></param>
        /// <param name="h62"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void ClearAllDataGridData(object lot, object roll, List<string> sn, List<string> user, List<string> comp,
            List<string> error, List<string> error_text, List<double> glu, List<double> nano_amper,
            List<bool> not_h62, List<bool> h62, List<DateTime> start, List<DateTime> end)
        {
            lot = null;
            roll = null;

            sn = new List<string>();
            user = new List<string>();
            comp = new List<string>();

          
            nano_amper = new List<double>();

            not_h62 = new List<bool>();
            h62 = new List<bool>();
            start = new List<DateTime>();
            end = new List<DateTime>();



        }


        


        /// <summary>
        /// Set invalidate the value of these variables to at when the next click happened an empty list can get values from db_tables IN CASE OF HOMOGENITY
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="roll"></param>
        /// <param name="sn"></param>
        /// <param name="user"></param>
        /// <param name="comp"></param>
        /// <param name="error"></param>
        /// <param name="error_text"></param>
        /// <param name="glu"></param>
        /// <param name="nano_amper"></param>
        /// <param name="not_h62"></param>
        /// <param name="h62"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void ClearAllDataGridData(object lot, object roll, List<string> sn,
            List<DateTime> date, List<string> user, List<string> comp,
            List<string> error, List<string> error_text, List<double> glu,            
            List<bool> not_h62, List<bool> h62)
        {
            lot = null;
            roll = null;

            sn = new List<string>();
            user = new List<string>();
            comp = new List<string>();

            error = new List<string>();
            error_text = new List<string>();
            glu = new List<double>();
          //  nano_amper = new List<double>();

            not_h62 = new List<bool>();
            h62 = new List<bool>();
            date = new List<DateTime>();
            

        }

       

        #region Methods

        /// <summary>
        /// Add columns and set properties of datagrid and this form HOMO!!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnumsAndComboBox_Load_Blank(object sender, System.EventArgs e)
        {
            #region add columns to the datagridview
           
            // Initialize the DataGridView.  
           AllDataTable.dataGridAll.AutoSize = false;
           AllDataTable.dataGridAll.AutoGenerateColumns = false;
            AllDataTable.dataGridAll.DataSource = bindingSourceAll;
         /*   AllDataTable.dataGridAll.AllowUserToOrderColumns = true;
            AllDataTable.dataGridAll.AllowUserToResizeColumns = true;           
            AllDataTable.dataGridAll.ShowEditingIcon = true;
            AllDataTable.dataGridAll.EnableHeadersVisualStyles = true;
            AllDataTable.dataGridAll.ShowCellToolTips = true;*/
          
          
            #region Columns defined and added to the grid
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "LOT";
            column.Name = "Lot ID";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "ROLL";
            column.Name = "Roll ID";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "SN";
            column.Name = "Tubus azonosító";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "User";
            column.Name = "Felhasználó";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Computer";
            column.Name = "Számítógép";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            if (Program.measureType == "hompogenity")
            {
                
                column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Error";
                column.Name = "Hiba";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                AllDataTable.dataGridAll.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "ErrorText";
                column.Name = "Hiba megnevezése";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                AllDataTable.dataGridAll.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "H62";
                column.Name = "H62 hiba történt";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                AllDataTable.dataGridAll.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Not_H62";
                column.Name = "Nem H62 hiba történt";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                AllDataTable.dataGridAll.Columns.Add(column);


                column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Glu";
                column.Name = "Glucose érték";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                AllDataTable.dataGridAll.Columns.Add(column);
            }

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "NanoAmper";
            column.Name = "Nano Amper";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "StartTime";
            column.Name = "Mérés kezdete";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "EndTime";
            column.Name = "Mérés vége";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);


            #endregion
            #endregion

            // Initialize the form. 
            #region Init the form and the dtagrid

            this.AutoSize = false;
            Controls.Add(AllDataTable.dataGridAll);
            Text = string.Format("Csíkvágás eredményei a {0} számú LOT, a {1} Roll esetén", lotid, rollid);
            BackgroundImage = Properties.Resources.elektronik;
            BackgroundImageLayout = ImageLayout.Tile;
            dataGridAll.AutoSize = false;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            dataGridAll.Size = new Size(dataGridAll.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + dataGridAll.FirstDisplayedScrollingColumnHiddenWidth + dataGridAll.FirstDisplayedCell.OwningColumn.Width, dataGridAll.Rows.GetRowsHeight(DataGridViewElementStates.Visible) + dataGridAll.ColumnHeadersHeight + dataGridAll.ColumnHeadersHeight);
            dataGridAll.LayoutEngine.Layout(dataGridAll, new LayoutEventArgs(dataGridAll, "Size"));
            dataGridAll.Dock = DockStyle.Top;
            this.Size = new Size(dataGridAll.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + dataGridAll.FirstDisplayedScrollingColumnHiddenWidth + dataGridAll.FirstDisplayedCell.OwningColumn.Width + 20, dataGridAll.Rows.GetRowsHeight(DataGridViewElementStates.Visible) + dataGridAll.ColumnHeadersHeight + dataGridAll.ColumnHeadersHeight + 20);
        
            this.StartPosition = FormStartPosition.Manual;
          
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width-this.Width)/2, Screen.PrimaryScreen.WorkingArea.Height / 2);

           // AdjustWidth();
            
            dataGridAll.PerformLayout();
            this.PerformLayout();

            #endregion


            #region event handlers
            AllDataTable.dataGridAll.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView1_ColumnHeaderMouseClick);
            AllDataTable.dataGridAll.CellMouseDown += new DataGridViewCellMouseEventHandler(dataGridView1_CellMouseDown);          
            #endregion


            Trace.TraceInformation("Sign in to the eventhandlers AllDataTable()");

            AllDataTable.dataGridAll.ShowEditingIcon = true;
            this.ShowIcon = true;
            this.Icon = Properties.Resources._77logo;

            foreach (DataGridViewRow actrow in AllDataTable.dataGridAll.Rows)
            {
                if (Convert.ToInt32(actrow.Cells["Nano Amper"].Value) > (double)51
                    || Convert.ToInt32(actrow.Cells["Nano Amper"].Value) < (double)13)
                {
                    actrow.DefaultCellStyle.BackColor = Color.Red;
                }
                else
                    actrow.DefaultCellStyle.BackColor = Color.Green;
            }

            this.Load += new EventHandler(AllDataTable_Load);

        }


        public void AdjustWidth()
        {
            Control horizontal = AllDataTable.dataGridAll.Controls[0]; // Horizontal scroll bar.
            Control vertical = AllDataTable.dataGridAll.Controls[1]; // Vertical scroll bar.
            AllDataTable.dataGridAll.Width = AllDataTable.dataGridAll.PreferredSize.Width - vertical.Width + 1;
            AllDataTable.dataGridAll.Height = AllDataTable.dataGridAll.PreferredSize.Height - horizontal.Height + 1;
        }

        public void AdjustWidth(bool is_LOT)
        {
            Control horizontal = dataGridButton.Controls[0]; // Horizontal scroll bar.
            Control vertical = dataGridButton.Controls[1]; // Vertical scroll bar.
            dataGridButton.Width = dataGridButton.PreferredSize.Width - vertical.Width + 1;
            dataGridButton.Height = dataGridButton.PreferredSize.Height - horizontal.Height + 1;
        }

        /// <summary>
        /// Add columns and set properties of datagrid and this form HOMO!!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnumsAndComboBox_Load(object sender, System.EventArgs e)
        {
            #region add columns to the datagridview
            // Initialize the DataGridView.

           AllDataTable.dataGridAll.AutoGenerateColumns = true;
            AllDataTable.dataGridAll.AutoSize = false;
            AllDataTable.dataGridAll.DataSource = bindingSourceAll;
      //      AllDataTable.dataGridAll.AllowUserToOrderColumns = true;
        //   AllDataTable.dataGridAll.AllowUserToResizeColumns = true;        
           //AllDataTable.dataGridAll.ShowEditingIcon = true;        
           //AllDataTable.dataGridAll.EnableHeadersVisualStyles = true;
           //AllDataTable.dataGridAll.ShowCellToolTips = true;
      
          
            #region Columns defined and added to the grid
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "LOT";
            column.Name = "Lot ID";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "ROLL";
            column.Name = "Roll ID";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "SN";
            column.Name = "Tubus azonosító";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "User";
            column.Name = "Felhasználó";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Computer";
            column.Name = "Számítógép";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Error";
            column.Name = "Hiba";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "ErrorText";
            column.Name = "Hiba megnevezése";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "H62";
            column.Name = "H62 hiba történt";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Not_H62";
            column.Name = "Nem H62 hiba történt";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);


            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Glu";
            column.Name = "Glucose érték";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "NanoAmper";
            column.Name = "Nano Amper";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "StartTime";
            column.Name = "Mérés kezdete";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);
            
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "EndTime";
            column.Name = "Mérés vége";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            AllDataTable.dataGridAll.Columns.Add(column);

           

            #endregion
            #endregion

            // Initialize the form.
            try
            {
             /*   AllDataTable.dataGridAll.AutoSize = false;
                //   dataGridAll.Size = new Size(dataGridAll.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + dataGridAll.FirstDisplayedScrollingColumnHiddenWidth + dataGridAll.FirstDisplayedCell.OwningColumn.Width, dataGridAll.Rows.GetRowsHeight(DataGridViewElementStates.Visible) + dataGridAll.ColumnHeadersHeight + dataGridAll.ColumnHeadersHeight);
                
                BackgroundImage = Properties.Resources.elektronik;
                BackgroundImageLayout = ImageLayout.Tile;
                Controls.Add(AllDataTable.dataGridAll);
                AutoSize = false;
                dataGridAll.AutoSize = false;
                this.ShowIcon = true;
                this.Icon = Properties.Resources._77logo;
                Text = string.Format("Csíkvágás eredményei a {0} számú LOT, a {1} Roll esetén", lotid, rollid);
                //AllDataTable.dataGridAll.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
                BackgroundImage = Properties.Resources.elektronik;
                BackgroundImageLayout = ImageLayout.Tile;
                dataGridAll.AutoSize = false;
                this.MinimizeBox = false;
                this.MaximizeBox = false;
                dataGridAll.Size = new Size(dataGridAll.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + dataGridAll.FirstDisplayedScrollingColumnHiddenWidth + dataGridAll.FirstDisplayedCell.OwningColumn.Width, dataGridAll.Rows.GetRowsHeight(DataGridViewElementStates.Visible) + dataGridAll.ColumnHeadersHeight + dataGridAll.ColumnHeadersHeight);
          */
            //    dataGridAll.LayoutEngine.Layout(dataGridAll, new LayoutEventArgs(dataGridAll, "Size"));
            //    dataGridAll.Dock = DockStyle.Top;
           //     this.Size = new Size(dataGridAll.Width + 20, dataGridAll.Height + 20);
               // this.Size = new Size(this.ClientSize.Width+20, this.ClientSize.Height + 20);
           //     this.StartPosition = FormStartPosition.Manual;
           //     this.Location = new Point(0, 0);



            //    dataGridAll.PerformLayout();
            //    this.PerformLayout();

            //    AllDataTable.dataGridAll.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView1_ColumnHeaderMouseClick);
            //    AllDataTable.dataGridAll.CellMouseDown += new DataGridViewCellMouseEventHandler(dataGridView1_CellMouseDown);
                // AllDataTable.dataGridAll.CellMouseDown += new DataGridViewCellMouseEventHandler(cell_Click);     
                Trace.TraceInformation("Sign in to the eventhandlers AllDataTable()");
            }
            catch (Exception exce)
            {
                Trace.TraceError("Exception at init: ex:{0}",exce.StackTrace);
            }
            
          
            foreach (DataGridViewRow actrow  in AllDataTable.dataGridAll.Rows)
          {
              if (Convert.ToInt32(actrow.Cells["Nano Amper"].Value)>51
                  || Convert.ToInt32(actrow.Cells["Nano Amper"].Value)<13)
              {
                  actrow.DefaultCellStyle.BackColor = Color.Red;
              }else
                  actrow.DefaultCellStyle.BackColor = Color.Green;
          }
            
        
            
       
            
          //  this.Load += new EventHandler(AllDataTable_Load);          
        }



        /// <summary>
        /// In case of row is selected and all data show needed       
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllDataTable_Load(object sender, System.EventArgs e)
         {
            
             

             Trace.TraceInformation("AllDataTable_Load executed");
             
         }

        public DataGridViewCell currCell;
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

                hide_item = new MenuItem((string.Format("Oszlop elrejtése:{0} oszlop", actCell.OwningColumn.HeaderCell.Value)));
                calculate_item = new MenuItem((string.Format("Értékekből avg,stddev,cv vagy összeg számolás:{0} oszlop", actCell.OwningColumn.HeaderCell.Value)));
                color_item = new MenuItem(string.Format("Adatok kiemelése színnel, {0} oszlop", actCell.OwningColumn.HeaderCell.Value));

                hide_item.Click += new EventHandler(hide_item_click);
                calculate_item.Click += new EventHandler(calculate_item_click);
                color_item.Click += new EventHandler(color_item_Click);

                cm.GetContextMenu();
                cm.MenuItems.Add(hide_item);
                cm.MenuItems.Add(calculate_item);
                cm.MenuItems.Add(color_item);

                #endregion


                #region show contextmenu
                Rectangle r = currentCell.DataGridView.GetCellDisplayRectangle(currentCell.ColumnIndex, currentCell.RowIndex, false);
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

            foreach (DataGridViewRow row in dataGridAll.Rows)
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
        public void calculate_item_click(object sender, EventArgs e)
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
            if ((dataGridAll.Rows[2].HeaderCell.Value.Equals("Nano Amper")
                || dataGridAll.Rows[2].HeaderCell.Value.Equals("Glucose érték")))
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
            else if ((dataGridAll.Rows[2].HeaderCell.Value.Equals("H62 hiba történt")
               || dataGridAll.Rows[2].HeaderCell.Value.Equals("Nem H62 hiba történt")))
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
            else if ((dataGridAll.Rows[2].HeaderCell.Value.Equals("Nem H62 hiba történt")))
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

                        not_h62_error_count++;
                    }
                }
                #endregion
            }
            else if ((dataGridAll.Rows[2].HeaderCell.Value.Equals("H62 hiba történt")))
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

                        h62_error_count++;
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
                int_avg, summary_ints, accepted_count, not_accepted_count,
                not_h62_error_count, h62_error_count,
                tsp, dataGridAll.Columns[column_index].Name).Show();


        }
       
        private void hide_item_click(object sender, System.EventArgs e)
        {
            //column_index=(()e.GetType());


            dataGridAll.Columns.Remove(dataGridAll.Columns[column_index]);
           
        }

        public void mouse_method(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// Listener for datagridALL
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
                         selected_roll_id= AllDataTable.dataGridAll.Rows[e.RowIndex].Cells["Roll ID"].Value;
                         selected_sn_id = AllDataTable.dataGridAll.Rows[e.RowIndex].Cells["Tubus azonosító"].Value;    
                
                         ShowContextMenu(c,selected_lot_id,selected_roll_id,selected_result,selected_measurytype,selected_sn_id);
                        
            }
        }
        /// <summary>
        /// Show a contextmenu on the cell which has been clicked by right mousebutton
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="selected_lot_id"></param>
        /// <param name="selected_roll_id"></param>
        /// <param name="selected_result"></param>
        public void ShowContextMenu(DataGridViewCell actCell,object selected_lot_id, object selected_roll_id, object selected_result,
                        object measure_type, object selected_sn_id)
        {
            DataGridViewCell currentCell = actCell;
            if (currentCell != null)
            {
                #region Create a local contextmenu
                ContextMenu cm = new ContextMenu();

                object_item = new MenuItem((
                    string.Format("Invalidálás(LOT ID:{0} ,Roll ID:{1},Tube_SN:{2})", selected_lot_id, selected_roll_id, selected_sn_id)));
               remeasure_item = new MenuItem((string.Format("Újramérés(LOT ID:{0} ,Roll ID:{1},Tube_SN:{2})", selected_lot_id, selected_roll_id, selected_sn_id)));
               showdata_item = new MenuItem(string.Format("Adatok mutatása(LOT ID:{0} ,Roll ID:{1},Tube_SN:{2})", selected_lot_id, selected_roll_id,selected_sn_id));


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

        private void remeasure_item_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void showdata_item_Click(object sender, EventArgs e)
        {
            #region In ncase of selected row,sn,lot

            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();

                    #region get values from identify table where lot,roll, measuretype is correct(result_id,serial_number)
                    using (NpgsqlCommand getInvalidValuesfromIdentify =
                       new NpgsqlCommand(
                           string.Format("select pk_id from blank_test_identify where invalidate=false and remeasured=false and measure_type='blank' and lot_id='{0}' and roll_id='{1}' and serial_number='{2}'", selected_lot_id, selected_roll_id,selected_sn_id), conn))
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
                               string.Format("select pk_id from blank_test_result where invalidate=false and remeasured=false and lot_id='{0}' and roll_id='{1}' and lot_id='{2}'", selected_roll_id,selected_roll_id,selected_sn_id), conn))
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
                                              string.Format("select * from blank_test_result where invalidate=false and remeasured=false and lot_id='{0}' and roll_id='{1}' and sn='{2}'", selected_lot_id,selected_roll_id,selected_sn_id), conn))
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
                           string.Format("select pk_id from blank_test_environment where remeasured=false and invalidate=false and lot_id='{0}' and roll_id='{1}' and sn='{2}'", selected_lot_id, selected_roll_id, selected_sn_id), conn))
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
                            string.Format("select * from blank_test_environment where invalidate=false and remeasured=false and lot_id='{0}' and roll_id='{1}' and sn='{1}'", selected_lot_id, selected_roll_id,selected_sn_id), conn))
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
                               string.Format("select * from blank_test_errors where invalidate=false and remeasured=false and lot_id='{0}' and roll_id='{1}' and sn='{2}'", selected_lot_id,selected_roll_id,selected_sn_id), conn))
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
                                                                     string.Format("select * from blank_test_errors where invalidate=false and remeasured=false and lot_id='{0}' and roll_id='{1}' and sn='{2}'", selected_lot_id, selected_roll_id, selected_sn_id), conn))
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

        private void invalidate_item_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void invalidate_item_clicked(object sender, EventArgs e)
        {
            //hide current datagrid
            this.WindowState = FormWindowState.Minimized;



            //Warn the operator about remeasured result must be remeasured instantly!!
            new Dialoge(selected_lot_id.ToString(), selected_roll_id.ToString(), measuretype,true).ShowDialog();

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


        /// <summary>
        /// this will set invalidated all results with actual lot, and roll id
        /// </summary>
        /// <param name="lot_id"></param>
        /// <param name="roll_id"></param>
        /// <param name="measuretype"></param>
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
        void remeasure_item_clicked(object sender, EventArgs e)
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
        /// Show all data item clicked on the contextmenu 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void showdata_item_MoseDown(object sender, EventArgs e)
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
                                   string.Format("select pk_id from blank_test_environment where remeasured=false and invalidate=false and lot_id='{0}' and roll_id='{1}'", selected_lot_id,selected_roll_id), conn))
                            {
                                object res = null;
                                res = getInvalidatedfromEnv.ExecuteScalar();

                                if (res == DBNull.Value
                                    || (res == null))
                                {
                                    Trace.TraceWarning("Rows where result_id:{1} are invalidated in envronment, query:{0}", getInvalidatedfromEnv.CommandText);


                                }else
                                {
                                       
                                        Trace.TraceWarning("All needed rows are validated in identify, query{0}", getInvalidatedfromEnv.CommandText);

                                        using (NpgsqlCommand getValuesfromEnv =
                                             new NpgsqlCommand(
                                        string.Format("select * from blank_test_environment where invalidate=false and remeasured=false and lot_id='{0}' and roll_id='{1}'", selected_lot_id,selected_roll_id), conn))
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

            CreateAllSNWithRollDataTable();

            #endregion
        }
        ///This datagrid shows all the measurement of the selected roll,sn
        private void CreateAllSNWithRollDataTable()
        {
            throw new NotImplementedException();
        }

                #endregion


        ///This datagrid shows all the measurement of the selected roll
        private void CreateAllDataTable()
        {     
               new AllDataTable(selected_result,selected_lot_id,selected_roll_id,selected_serial_numbers,selected_users,selected_computers,selected_error,selected_error_text,selected_glus,selected_nano_ampers,selected_not_h62,selected_h62,selected_start_dates,selected_end_dates).Show();
           // ClearAllDataGridData(selected_result, selected_lot_id, selected_roll_id, selected_serial_numbers, selected_users, selected_computers, selected_error, selected_error_text, selected_glus, selected_nano_ampers, selected_not_h62, selected_h62, selected_start_dates, selected_end_dates);
        }     
 

        /// <summary>
        /// This method set to remeasured=true for those results whiches lot and roll ID sre the delectrf
        /// </summary>
        /// <param name="lot_id"></param>
        /// <param name="roll_id"></param>
        /// <param name="measuretype"></param>
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

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        #endregion

         /// <summary>
        /// In case of select this item from the menu it will color showhigh the column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void color_item_Clicks(object sender, EventArgs e)
        {
            
		        column_index=currCell.ColumnIndex;
	        
            foreach (DataGridViewRow row in AllDataTable.dataGridAll.Rows)
            {
                DataGridViewCellStyle st=row.Cells[column_index].Style;

                if (st.BackColor==Color.Magenta)
                {
                    st.BackColor = Color.Green;
                }else
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
               column_index= ((DataGridViewColumn)sender).Index;
            }
            else if (sender is DataGridViewCell)
	        {
                column_index = ((DataGridViewCell)(sender)).ColumnIndex;
            }

            #endregion


            if ((dataGridAll.Rows[2].HeaderCell.Value.Equals("Nano Amper")
                ||dataGridAll.Rows[2].HeaderCell.Value.Equals("Glucose érték")))               
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
            else if ((dataGridAll.Rows[2].HeaderCell.Value.Equals("H62 hiba történt")
               || dataGridAll.Rows[2].HeaderCell.Value.Equals("Nem H62 hiba történt")))  
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
            }else if((dataGridAll.Rows[2].HeaderCell.Value.Equals("Nem H62 hiba történt")))
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
                        
                            not_h62_error_count++;
                        }
                    }
              #endregion
            }else if ((dataGridAll.Rows[2].HeaderCell.Value.Equals("H62 hiba történt")))
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
                        
                            h62_error_count++;
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

                    avg+=Convert.ToDouble(item);

                    #endregion
                   
	            }
                ///The sum allocated with the count of items
                avg=avg/row_number;

                foreach (object item in values)
	            {
                ///After we have avg ,then can calculate stddev
                 #region calculate stddev
                           
                            ///So each item decreased with the average and get on the second square
                         sumOfSquaresOfDifferences = ((Convert.ToDouble(item) - avg) * (Convert.ToDouble(item) - avg));
                        
                             ///Get the summary of these
                            sum+=sumOfSquaresOfDifferences;

                            ///then the summarry of decreased item values will be allocated with item count
                         sd = Math.Sqrt(sum / values.Count); 
                 #endregion
        	
                }

                ///CV is simple stddev/avg (*100)
                cv=(stddev/avg)*100;
            
                ///All of the item is integer
            }else if (intTyped == row_number)
	        {
                foreach (object item in values)
	                {
                        summary_ints+=Convert.ToInt32(item);
	                }
                int_avg=summary_ints/row_number;
		     
                ///All item is bool so we can count each the true and the false values
	        }else if (boolTyped == row_number)
	        {
                foreach (object item in values)
	            {
                    if(Convert.ToBoolean(item))
                    {
                        true_items++;
                    }else
                        false_items++;
	            }
                ///Each item is string, like "Megfelelő" ,an ID or an error_text
	        }else if (stringTyped == row_number)
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
                tsp=Convert.ToDateTime(values[0]).Date - Convert.ToDateTime(values[row_number]).Date;               
            }
            #endregion

            new CalculatedColumnValuesForm(avg,sd,cv,int_avg,summary_ints,accepted_count,not_accepted_count,
                not_h62_error_count,h62_error_count,tsp,AllDataTable.dataGridAll.Columns[column_index].HeaderText).Show();

        }

        private DataGridViewCell clicked_Cell;
      
        /// <summary>
        /// a helyimenüből erre kattintva az oszlop fejlécén akkor eltünteti az oszlopot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hide_item_Click(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dataGridAll.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                clicked_Cell =
                    dataGridAll.Rows[hit.RowIndex].Cells[hit.ColumnIndex];
            }
            //Remove the column from the DataGridView1
            dataGridAll.Columns.Remove(clicked_Cell.OwningColumn);
        }

        void dataGridAll_ColumnRemoved(object sender, DataGridViewColumnEventArgs e)
        {
            throw new NotImplementedException();
        }

        public DataGridViewTextBoxColumn txbCell;
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
       /// At the load of the form the preparing for the chart display
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)        {
/*
            double[] roll_avg=new double[]{};

            // Data arrays.
            double[] rollAvgArray = { roll_avg[0],roll_avg[1],roll_avg[2],roll_avg[3]};
           
            int[] pointsOfTubes = { 1, 3, 5, 7, 9, 11, 13,15,17,19 };

            // Set palette.
            chart1.Palette = ChartColorPalette.SeaGreen;
               
            // Set title.
            chart1.Titles.Add("Blank Current Tests Averages");

            // Add series.
            for (int i = 0; i < rollAvgArray.Length; i++)
            {
                // Add series.
                Series series = chart1.Series.Add( Convert.ToString(rollAvgArray[i]));

                // Add point.
                series.Points.Add(pointsOfTubes[i]);
            }*/
        }

                        #endregion
        /// <summary>
        /// Grafikon megrajzolása
        /// </summary>
        /// <param name="graph"></param>
       /* private void DrawGraph(Graphics graph)
        {
            // Draw BarGraph
            graph = CreateGraphics();
            SolidBrush brush = new SolidBrush(Color.Blue);
            Rectangle rect1 = new Rectangle(100, Convert.ToInt32(chart1), 50, (600 - Convert.ToInt32(chart1)));
            graph.FillRectangle(brush, rect1);

            // Write Qso Value
            FontStyle style3 = FontStyle.Regular;
            Font arial3 = new Font(new FontFamily("Arial"), 12, style3);
            brush.Color = Color.Black;
            graph.DrawString(valuesNeededForChart[1].ToString(), arial3, brush, 112, (float)Convert.ToDouble(chart1)- 20);
            
            // Draw BarGraph
            SolidBrush brush2 = new SolidBrush(Color.BlanchedAlmond);
            Rectangle rect2 = new Rectangle(170, Convert.ToInt32(chart2), 50, (int)(600 - Convert.ToInt32(chart2)));
            graph.FillRectangle(brush2, rect2);

            // Write Qso Value
            brush.Color = Color.Black;
            graph.DrawString(valuesNeededForChart[2].ToString(), arial3, brush, 182, (float)Convert.ToDouble(chart2) - 20);

            // Draw BarGraph
            SolidBrush brush3 = new SolidBrush(Color.ForestGreen);
            Rectangle rect3 = new Rectangle(240, Convert.ToInt32(chart3), 50, (int)(600 - Convert.ToInt32(chart3)));
            graph.FillRectangle(brush3, rect3);

            // Write Qso Value
            brush.Color = Color.Black;
            graph.DrawString(valuesNeededForChart[3].ToString(), arial3, brush, 252,  (float)Convert.ToDouble(chart3) - 20);

            // Draw BarGraph
            SolidBrush brush4 = new SolidBrush(Color.Brown);
            Rectangle rect4 = new Rectangle(310,Convert.ToInt32(chart4), 50, (600 - Convert.ToInt32(chart4)));
            graph.FillRectangle(brush4, rect4);

            // Write Qso Value
            brush.Color = Color.Black;
            graph.DrawString(valuesNeededForChart[4].ToString(), arial3, brush, 322, (float)Convert.ToDouble(chart4) - 20);

            // Draw BarGraph
            SolidBrush brush5 = new SolidBrush(Color.DarkMagenta);
            Rectangle rect5 = new Rectangle(380, Convert.ToInt32(chart5), 50, (600 - Convert.ToInt32(chart5)));
            graph.FillRectangle(brush5, rect5);

            // Write Qso Value
            brush.Color = Color.Black;
            graph.DrawString(valuesNeededForChart[5].ToString(), arial3, brush, 392,  (float)Convert.ToDouble(chart5) - 20);

            // Draw BarGraph
            SolidBrush brush6 = new SolidBrush(Color.BlanchedAlmond);
            Rectangle rect6 = new Rectangle(450, Convert.ToInt32(chart6), 50, (600 - Convert.ToInt32(chart6)));
            graph.FillRectangle(brush6, rect6);

            // Write Qso Value
            brush.Color = Color.Black;
            graph.DrawString(valuesNeededForChart[6].ToString(), arial3, brush, 462,  (float)Convert.ToDouble(chart6) - 20);

            // Draw BarGraph
            SolidBrush brush7 = new SolidBrush(Color.DarkGreen);
            Rectangle rect7 = new Rectangle(520, Convert.ToInt32(chart7), 50, (600 - Convert.ToInt32(chart7)));
            graph.FillRectangle(brush7, rect7);

            // Write Qso Value
            brush.Color = Color.Black;
            graph.DrawString(valuesNeededForChart[7].ToString(), arial3, brush, 532,  (float)Convert.ToDouble(chart7) - 20);

            // Draw BarGraph
            SolidBrush brush8 = new SolidBrush(Color.Gold);
            Rectangle rect8 = new Rectangle(590, Convert.ToInt32(chart8), 50, (600 -Convert.ToInt32(chart8)));
            graph.FillRectangle(brush8, rect8);

            // Write Qso Value
            brush.Color = Color.Black;
            graph.DrawString(valuesNeededForChart[8].ToString(), arial3, brush, 602, (float)Convert.ToDouble(chart8)  - 20);

            // Draw BarGraph
            SolidBrush brush9 = new SolidBrush(Color.BlueViolet);
            Rectangle rect9 = new Rectangle(660, Convert.ToInt32(chart9), 50, (600 - Convert.ToInt32(chart9)));
            graph.FillRectangle(brush9, rect9);

            // Write Qso Value
            brush.Color = Color.Black;
            graph.DrawString(valuesNeededForChart[9].ToString(), arial3, brush, 672, (float)Convert.ToDouble(chart9) - 20);

            // Draw BarGraph
            SolidBrush brush10 = new SolidBrush(Color.Firebrick);
            Rectangle rect10 = new Rectangle(730, Convert.ToInt32(chart10), 50, (600 - Convert.ToInt32(chart10)));
            graph.FillRectangle(brush10, rect10);

            // Write Qso Value
            brush.Color = Color.Black;
            graph.DrawString(valuesNeededForChart[10].ToString(), arial3, brush, 742, (float)Convert.ToDouble(chart10) - 20);

            // Draw BarGraph
            SolidBrush brush11 = new SolidBrush(Color.BlanchedAlmond);
            Rectangle rect11 = new Rectangle(800, Convert.ToInt32(chart11), 50, (600 - Convert.ToInt32(chart11)));
            graph.FillRectangle(brush11, rect11);

            // Write Qso Value
            brush.Color = Color.Black;
            graph.DrawString(valuesNeededForChart[11].ToString(), arial3, brush, 812, (float)Convert.ToDouble(chart11) - 20);

            // Draw BarGraph
            SolidBrush brush12 = new SolidBrush(Color.DarkOrange);
            Rectangle rect12 = new Rectangle(870, valuesNeededForChart[12], 50, (600 - Convert.ToInt32(chart12)));
            graph.FillRectangle(brush12, rect12);

            // Write Qso Value
            brush.Color = Color.Black;
            graph.DrawString(valuesNeededForChart[13].ToString(), arial3, brush, 882, (float)Convert.ToDouble(chart12) - 20);
        }*/

        /// <summary>
        /// Event handler for cell mouse down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cell_Click(object sender, DataGridViewCellMouseEventArgs e)
        {
            

        }
        Button btOK=new Button();
        /// <summary>
        /// Add the button OK to the form
        /// </summary>
        public void SetButtonOk()
        {
            Trace.TraceError("setbuttonok started 871 row");
            btOK.Location = new Point(AllDataTable.dataGridAll.Width / 2, AllDataTable.dataGridAll.Height + 200);
            btOK.Name = "button1";
            btOK.Size = new System.Drawing.Size(75, 23);
            btOK.TabIndex = 2;
            btOK.Text = "OK";
            btOK.UseVisualStyleBackColor = true;
            btOK.Click += new System.EventHandler(button1_Click);
            Controls.Add(btOK);
        }

        public void FormClosed(object sender,FormClosedEventArgs e)
        {
            Close();
            base.Show();
            
        }
        /// <summary>
        /// Click on close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
                        #endregion
    }
}
