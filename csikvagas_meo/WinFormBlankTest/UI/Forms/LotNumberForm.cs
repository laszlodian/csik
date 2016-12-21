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
using System.Windows.Input;
using WinFormBlankTest.UI.Forms.Classes_for_Show_DataGrid;

namespace WinFormBlankTest.UI.Forms
{
    public partial class LotNumberForm : Form
    {
        public static PrintResult printres;
        public List<string> lotid_list = new List<string>();
        public List<string> lot_ids = new List<string>();

        /// <summary>
        /// With argument /show and after a row selected the details displayed
        /// </summary>
        /// <param name="lotid"></param>
        /// <param name="rollid"></param>
        /// <param name="meastype"></param>
        public LotNumberForm(string lotid, string rollid, string meastype)
        {           
            this.CenterToScreen();

            new ShowResult(lotid,rollid,meastype);
        }

        /// <summary>
        /// In case of accuracy measure
        /// </summary>
        /// <param name="blank_homogenity_valid_lot"></param>
        public LotNumberForm(List<string> blank_homogenity_valid_lot)
        {
            InitializeComponent();

            this.CenterToScreen();

            button1.Click += new EventHandler(button1_Click);
            if (Program.measureType == "accuracy")
            {
                SetLotIDsToShowPanel(blank_homogenity_valid_lot);
                
            }

            if (Program.measureType == "print")
            {
                GetAvailableIDsInAllTestSucceed();
            }
            else if ((Program.measureType!="print")
                    &&(Program.measureType!="accuracy"))
            {
                GetAvailableLot_IDs();
            }
               

        }

        /// <summary>
        /// Supported arguments: /show and /showall
        /// </summary>
        public LotNumberForm()
        {
            InitializeComponent();

            this.CenterToScreen();
            
            button1.Click +=new EventHandler(button1_Click);


            if (Program.measureType == "print")
            {
                GetAvailableIDsInAllTestSucceed();
            }else
                GetAvailableLot_IDs();
            
        }

        public List<string> homogenityIsValid2 = new List<string>();
        public List<string> allTestIsValid= new List<string>();
        private void GetAvailableIDsInAllTestSucceed()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();

                    #region Get valid blank results
                    using (NpgsqlCommand getLotIds = new NpgsqlCommand(string.Format("select distinct lot_id from blank_test_averages where invalidate=false"), conn))
                    {
                        using (NpgsqlDataReader dr = getLotIds.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    lot_ids.Add(Convert.ToString(dr["lot_id"]));

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

                    #region Get valid Homogenity results where blank is valid
                    foreach (string lot in lot_ids)
                    {

                        using (NpgsqlCommand get2TestValid = new NpgsqlCommand(string.Format("select distinct lot_id from homogenity_result where lot_id='{0}' and homogenity_is_valid=true", lot), conn))
                        {

                            using (NpgsqlDataReader dr = get2TestValid.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        homogenityIsValid2.Add(Convert.ToString(dr["lot_id"]));
                                    }
                                }

                            }


                        }

                    } 
                    #endregion

                    #region Get Accuracy valid (lot_accuracy_ok) results where the blank and homogenity test is valid too
                    foreach (string item in homogenityIsValid2)
                    {
                        using (NpgsqlCommand getAllTestValid = new NpgsqlCommand(string.Format("select distinct lot_id from accuracy_lot_result where lot_id='{0}'", item), conn))
                        {
                            using (NpgsqlDataReader dr = getAllTestValid.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        allTestIsValid.Add(Convert.ToString(dr["lot_id"]));
                                    }
                                    dr.Close();
                                }
                            }
                        }
                    } 
                    #endregion
                    if (allTestIsValid.Count >= 1)
                    {
                        SetLotIDsToShowPanelInCaseOfPrint(allTestIsValid);
                    }
                    else
                        MessageBox.Show("Nincs olyan LOT amelyik mind három teszten megfelelt volna");
                   
                    
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception in get lot ids: ex: {0}", ex.StackTrace);
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public BindingSource source = new BindingSource();
        delegate void SetLotIDsToShowPanelInCaseOfPrintDelegate(List<string> allTestIsValid);
        private void SetLotIDsToShowPanelInCaseOfPrint(List<string> allTestIsValid)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetLotIDsToShowPanelInCaseOfPrintDelegate(SetLotIDsToShowPanel), allTestIsValid);
            }
            else
            {
                source.DataSource = allTestIsValid;
                cbLOTID.DataSource = source;
            }
        }

        void tbLOTIDs_Enter(object sender, EventArgs e)
        {
            MessageBox.Show("tblotids enter");

        }

        private void GetAvailableLot_IDs()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();

                    using (NpgsqlCommand getLotIds=new NpgsqlCommand(string.Format("select distinct lot_id from blank_test_identify where invalidate=false"),conn))
                    {
                        using (NpgsqlDataReader dr = getLotIds.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    lot_ids.Add(Convert.ToString(dr["lot_id"]));
                                  
                                }
                                dr.Close();
                            }
                            else
                            {
                                dr.Close();

                            }

                        }
                    }

                    foreach (string lot in lot_ids)
                    {

                        lotid_list.Add(lot);
                        
                    }

                    SetLotIDsToShowPanel(lotid_list);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception in get lot ids: ex: {0}",ex.StackTrace);
                    throw;
                }
            }
        }
        delegate void SetLotIDsToShowPanelDelegate(List<string> lotid);
        private void SetLotIDsToShowPanel(List<string> lotid)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetLotIDsToShowPanelDelegate(SetLotIDsToShowPanel), lotid);
            }
            else
                cbLOTID.DataSource = lotid;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            lot_nr = Convert.ToString(cbLOTID.SelectedValue);
            Program.LOT_ID=lot_nr;
            foreach (Control item in UserWindow.userTables.Controls)
            {
                if (item is TextBox)
                {
                    if (((TextBox)item).Name == "tbBarcode")
                    {
                        ((TextBox)item).Text = Program.LOT_ID;
                        ((TextBox)item).Enabled = false;
                    }
                }
            }
            if (Program.measureType=="accuracy")
            {
                Program.SelectedLotToMeasure = lot_nr;
                this.Close();
            }
            if (Program.measureType=="invalidate")
            {

            }

            if (Program.measureType == "showall")
            {
                if (Program.measureType=="showall")
                {
                    new ShowResult(lot_nr, true);
                    this.Close();
                }
                

                Program.measureType = "N/A";
            }
            else if (Program.measureType == "show")
            {
                if (Program.measureType=="show")
                {

                    new ShowResult(lot_nr);
                    this.Close();
                }
                
                Program.measureType = "N/A";
            }
            else if (Program.measureType=="print")
            {
               printres=new PrintResult(lot_nr);
                printres.Visible = false;
               
                Environment.Exit(Environment.ExitCode);
            }
            

            
        }

        public string lot_nr;

        

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
        
    }
}
