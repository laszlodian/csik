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

namespace WinFormBlankTest.UI.Forms.Result_Forms_With_DataGrid
{
    public partial class CentralVialSelection : Form
    {
        #region Variables       
        //public DataGridView dgvCentral = new DataGridView();
        public int sn = 0;       
        public List<string> usedSN = new List<string>();
        public List<string> usedBlankSN = new List<string>();
        public int ThaCount = 0;
        public List<string> bestSN = new List<string>();
        //public Button btRendben;     
        public Accuracy_vials_form accuracy_vials_form = Program.acc_vials_form;
        public List<string> RollId = new List<string>();
        public List<string> SNId = new List<string>();
        private string lot_id;

        //public Button btAccuracyFinished = new Button();
        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="lotid"></param>
        /// <param name="BestRoll"></param>
        /// <param name="BestGlu"></param>
        /// <param name="DiffereneceFromAvg"></param>
        /// <param name="rollTubeCount"></param>
        /// <param name="HomogenityInRangeGlusValue"></param>
        /// <param name="HomogenityInRangeSNsIDs"></param>
        /// <param name="HomogenityInRangeRollIDs"></param>
        public CentralVialSelection()
        {
            Instance = this;
            InitializeComponent();

            statDgvCentral = dgvCentral01;
            SetButtonFinished();

            
           

            #region Commented out old code
            //try
            //{


            //    int tubecount = 0;
            //    int firstRollID = 1;
            //    for (int i = 0; i < DiffereneceFromAvg.Count; i++)
            //    {
            //        tubecount++;
            //        if (rollTubeCount[firstRollID - 1] >= tubecount)
            //        {
            //            RollId.Add(HomogenityInRangeRollIDs[i]);
            //            SNId.Add(HomogenityInRangeSNsIDs[i]);
            //            Program.centralBindingSrc.Add(new SNTest(lotid, rollid, snid, differences));
            //        }
            //        else 
            //        {
            //            firstRollID++;
            //            tubecount = 1;
            //            Program.centralBindingSrc.Add(new SNTest(lotid, HomogenityInRangeRollIDs[i], HomogenityInRangeSNsIDs[i], HomogenityInRangeGlusValue[i], Properties.Settings.Default.HOMOGENITY_LIMIT_AVG, Math.Round(Properties.Settings.Default.HOMOGENITY_LIMIT_AVG - HomogenityInRangeGlusValue[i], 4)));
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Trace.TraceError("Central selection exception:{0}", ex.StackTrace);

            //}
            //finally
            //{


            //}

            //dgvCentral.DataSource = Program.centralBindingSrc;
            //dgvCentral.Location = new Point(0,0);
            //dgvCentral.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width,Screen.PrimaryScreen.WorkingArea.Height/2+100);

            //this.Load += new EventHandler(EnumsAndComboBox_Load_For_All);         
            #endregion
        }
        public static DataGridView statDgvCentral;
        public static CentralVialSelection Instance;
        public  void AddRowToDGV(object[] dataRow)
        {
            statDgvCentral.Rows.Add(dataRow);

        }

        public void SaveTheCentralTubesAndFillTheAccuracyVialsForm()
        {

            SaveCentralsToDB();

          
        }
        public void SetButtonFinished()
        {
            btForward.Name = "btForward";
          //  btForward.Size = new System.Drawing.Size(75, 23);
            btForward.TabIndex = 2;
            btForward.Text = "Tovább";
            btForward.UseVisualStyleBackColor = true;
            btForward.Visible = true;
            btForward.Enabled = true;
            btForward.Dock = DockStyle.Bottom;
            btForward.BackColor = Color.Lime;
            btForward.Font = new System.Drawing.Font("Arial Black", 12.0f);
            btForward.Click += new System.EventHandler(nextRound);

        }
        private void nextRound(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Save central Vials data
        /// </summary>
        private void SaveCentralsToDB()
        {
            FillAccuracyVialsForm();

            using (NpgsqlConnection conn_in=new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn_in.Open();
                    Trace.TraceInformation("Connection Opened...");

                    foreach (DataGridViewRow row in dgvCentral01.Rows)
                    {
                        using (NpgsqlCommand insertComm = new NpgsqlCommand(string.Format("insert into central_vials(lot_id,roll_id,sn,difference_from_ref,avg) values('{0}','{1}','{2}',{3},{4})", Program.LOT_ID, row.Cells["roll"].Value, row.Cells["tube"].Value, row.Cells["differences"].Value, Convert.ToDouble(Program.RANGE_MEDIAN + Convert.ToDouble(row.Cells["differences"].Value))), conn_in))
                        {
                            object res = null;
                            res = insertComm.ExecuteNonQuery();
                            if (res == null)
                            {
                                Trace.TraceError("Unsuccesfull insert:Result is null at Saving Central Vials");
                                throw new ArgumentException("Unsuccesfull insert at Central vial selection");
                            }
                        }
                    }//End of foreach
                   
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception at saveCentralsToDb:  {0}", ex.StackTrace);
                    throw;
                }
                finally
                {
                    conn_in.Close();
                    Trace.TraceInformation("connection closed");
                }
            }
        }

        #region Commented out old code
        //private void EnumsAndComboBox_Load_For_All(object sender, System.EventArgs e)
        //{

        //    // Initialize the DataGridView.
        //    dgvCentral.AutoGenerateColumns = false;
        //    dgvCentral.AutoSize = true;
        //    dgvCentral.DataSource = Program.centralBindingSrc;

        //    DataGridViewColumn column = new DataGridViewTextBoxColumn();
        //    column.DataPropertyName = "CentralLot";
        //    column.Name = "Lot ID";
        //    column.Width = 120;
        //    dgvCentral.Columns.Add(column);/*1*/

        //    column = new DataGridViewTextBoxColumn();
        //    column.DataPropertyName = "CentralRoll";
        //    column.Name = "Roll ID";
        //    column.Width = 120;
        //    dgvCentral.Columns.Add(column);/*2*/

        //    column = new DataGridViewTextBoxColumn();
        //    column.DataPropertyName = "CentralSN";
        //    column.Name = "Tubus azonosító";
        //    column.Width = 120;
        //    dgvCentral.Columns.Add(column);/*3*/

        //    column = new DataGridViewTextBoxColumn();
        //    column.DataPropertyName = "TubeAVG";
        //    column.Name = "Tubus átlag(glu)(4csík)";
        //    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
        //    dgvCentral.Columns.Add(column);/*4*/

        //    column = new DataGridViewTextBoxColumn();
        //    column.DataPropertyName = "AVGLimit_Homogenity";
        //    column.Name = "Homogenity Limit Átlag";
        //    column.Width = 120;
        //    dgvCentral.Columns.Add(column);

        //    column = new DataGridViewTextBoxColumn();
        //    column.DataPropertyName = "DiffFromHomogenityAVG";
        //    column.Name = "Eltérés a Homogenity Limit Átlagtól";
        //    column.Width = 120;
        //    dgvCentral.Columns.Add(column);

        //    // Initialize the form.
        //    this.Controls.Add(dgvCentral);
        //    this.AutoSize = true;
        //    this.Text = "Central tubusok és értékeik";




        //}

        #endregion

        delegate void FillAccuracyVialsFormDelgate();
        public void FillAccuracyVialsForm()
        {
            int lk = 1;
            if (accuracy_vials_form.InvokeRequired)
            {
                accuracy_vials_form.Invoke(new FillAccuracyVialsFormDelgate(FillAccuracyVialsForm));
            }else
                foreach (Control tb in accuracy_vials_form.Controls["groupBox1"].Controls)
                {
                    if (tb is TextBox)
                    {
                        if (tb.Name.StartsWith(string.Format("textBox")))
                        {
                           


                                tb.Text = string.Format("{0} {1}", dgvCentral01.Rows[lk - 1].Cells["tube"].Value, dgvCentral01.Rows[lk - 1].Cells["roll"].Value);
                                Program.snAndRollID.Add(string.Format("{0} {1}", dgvCentral01.Rows[lk - 1].Cells["tube"].Value, dgvCentral01.Rows[lk - 1].Cells["roll"].Value));
                                tb.Enabled = false;
                                lk++;
                            
                        }
                    }
                }
        }

        //public void SetButtonCalc()
        //{
        //    btRendben = new Button();
        //    btRendben.Location = new Point(dgvCentral.Width / 2, dgvCentral.Height );
        //    btRendben.Name = "btRendben";
        //    btRendben.Size = new System.Drawing.Size(100, 35);
        //    btRendben.TabIndex = 2;
        //    btRendben.BackColor = Color.LightBlue;
        //    btRendben.Font = new System.Drawing.Font("Arial Black", 12.0f);
        //    btRendben.Text = "Tovább >>";
        //    btRendben.Width = 100;
        //    btRendben.UseVisualStyleBackColor = true;
        //    btRendben.Click += new EventHandler(btRendben_Click);
        //    this.Controls.Add(btRendben);
        //}

        //void btRendben_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}
    }
}
