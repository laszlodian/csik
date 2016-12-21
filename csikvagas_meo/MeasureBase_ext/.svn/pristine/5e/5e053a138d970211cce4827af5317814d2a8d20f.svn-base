using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace e77.MeasureBase.GUI
{
    public partial class LoadMeasureSelectorForm : Form
    {
        const string TABLE_NAMESPACE_ID = "table_namespace";

        /// <summary>
        /// Example:
        /// using (LoadSelectorForm form = new LoadSelectorForm(SqlTableDescriptorsBase.MainTable.TableName))
        /// {
        ///     if(form.ShowDialog() == DialogResult.OK)
        ///         LoadFromSql(form.SqlId);
        /// }
        /// </summary>
        /// <param name="mainTabeName_in"></param>
        public LoadMeasureSelectorForm(string mainTabeName_in)
        {
            MainTableName = mainTabeName_in;
            InitializeComponent();

            _sqlQueryPostfix = string.Empty;
            UpdateUI();
        }                  
      
        /// <summary>
        /// Example:
        /// using (LoadSelectorForm form = new LoadSelectorForm(DeviceBase.TheDevice.Name,SqlTableDescriptorsBase.MainTable.TableName,DeviceBase.TheDevice.CurrentWorkphase.Name))
        /// {
        ///     if(form.ShowDialog() == DialogResult.OK)
        ///         LoadFromSql(form.SqlId);
        /// }
        /// </summary>
        /// <param name="mainTabeName_in">The MainTable of the measurement</param>
        /// <param name="deviceName_in">The 3 character long devicename</param>
        /// <param name="workphaseName_in">The name of the current running workphase</param>
        public LoadMeasureSelectorForm(string deviceName_in, string mainTableName_in, string workphaseName_in)
        {
            MainTableName = mainTableName_in;
            InitializeComponent();

            _sqlQueryPostfix = string.Empty;
            UpdateUI(deviceName_in, workphaseName_in);
        }
        public void LoadSelectorForm(string deviceName_in, string mainTableName_in, string workphaseName_in)
        {
            MainTableName = mainTableName_in;
            InitializeComponent();

            _sqlQueryPostfix = string.Empty;
            UpdateUI(deviceName_in, workphaseName_in);
        }       
        public void UpdateUI(string deviceName_in, string workPhaseName_in)
        {
            string sqlQuery = string.Format("SELECT {0}.pk_id, " +
                                                  "global_users.fullname, " +
                                                  "{0}.ok, " +
                                                  "global_operations.sn, " +
                                                  "global_operations.date " +
                                            "FROM {0} " +
                                            "LEFT JOIN global_operations ON {0}.fk_global_operations_id = global_operations.pk_id " +
                                            "LEFT JOIN global_users ON global_operations.fk_global_users_id = global_users.pk_id ",
                                 MainTableName, _sqlQueryPostfix);

            if (workPhaseName_in.Split('_')[1] == "backpanel")
            {
                sqlQuery = string.Format("SELECT {0}.pk_id, " +
                                                  "global_users.fullname, " +
                                                  "{0}.ok, " +
                                                  "{0}.fk_{2}_back_panel_sn_id, " +
                                                  "{2}_back_panel_sn.sn, " +
                                                  "global_operations.date " +
                                            "FROM {0} " +
                                            "LEFT JOIN global_operations ON {0}.fk_global_operations_id = global_operations.pk_id " +
                                            "LEFT JOIN global_users ON global_operations.fk_global_users_id = global_users.pk_id " +
                "LEFT JOIN {2}_back_panel_sn ON {0}.fk_{2}_back_panel_sn_id = {2}_back_panel_sn.pk_id " +
                "WHERE {2}_back_panel_sn.pk_id = {0}.fk_{2}_back_panel_sn_id",
                                 MainTableName, _sqlQueryPostfix, deviceName_in);
            }
            else if (workPhaseName_in.Split('_')[1] == "mainpanel")
            {
                sqlQuery = string.Format("SELECT {0}.pk_id, " +
                                                  "global_users.fullname, " +
                                                  "{0}.ok, " +
                                                  "{0}.fk_{2}_main_panel_sn_id, " +
                                                  "{2}_main_panel_sn.sn, " +
                                                  "global_operations.date " +
                                            "FROM {0} " +
                                            "LEFT JOIN global_operations ON {0}.fk_global_operations_id = global_operations.pk_id " +
                                            "LEFT JOIN global_users ON global_operations.fk_global_users_id = global_users.pk_id " +
                "LEFT JOIN {2}_main_panel_sn ON {0}.fk_{2}_main_panel_sn_id = {2}_main_panel_sn.pk_id " +
                "WHERE {2}_main_panel_sn.pk_id = {0}.fk_{2}_main_panel_sn_id",
                                 MainTableName, _sqlQueryPostfix, deviceName_in);
            }
            else if (workPhaseName_in.Split('_')[1] == "motorpanel")
            {
                sqlQuery = string.Format("SELECT {0}.pk_id, " +
                                                  "global_users.fullname, " +
                                                  "{0}.ok, " +
                                                  "{0}.fk_{2}_motor_panel_sn_id, " +
                                                  "{2}_motor_panel_sn.sn, " +
                                                  "global_operations.date " +
                                            "FROM {0} " +
                                            "LEFT JOIN global_operations ON {0}.fk_global_operations_id = global_operations.pk_id " +
                                            "LEFT JOIN global_users ON global_operations.fk_global_users_id = global_users.pk_id " +
                "LEFT JOIN {2}_motor_panel_sn ON {0}.fk_{2}_motor_panel_sn_id = {2}_motor_panel_sn.pk_id " +
                "WHERE {2}_motor_panel_sn.pk_id = {0}.fk_{2}_motor_panel_sn_id",
                                 MainTableName, _sqlQueryPostfix, deviceName_in);
            }


            using (NpgsqlConnection conn = new NpgsqlConnection(MeasureConfig.TheConfig.SqlConnectionStr))
            {
                using (DataSet dset = new DataSet(TABLE_NAMESPACE_ID))
                using (NpgsqlDataAdapter NpAdapter = new NpgsqlDataAdapter())
                using (NpAdapter.SelectCommand = new NpgsqlCommand(sqlQuery, conn))
                {
                    NpAdapter.Fill(dset, MainTableName);
                    DataTable tableData = dset.Tables[MainTableName];
                    _dataGridView.DataSource = tableData;
                }
            }
        }

        public void UpdateUI()
        {
            string sqlQuery = string.Format("SELECT {0}.pk_id, "+
                                                  "global_users.fullname, "+
                                                  "{0}.ok, "+
                                                  "global_operations.sn, "+
                                                  "global_operations.date "+ 
                                            "FROM {0} "+
                                            "LEFT JOIN global_operations ON {0}.fk_global_operations_id = global_operations.pk_id "+
                                            "LEFT JOIN global_users ON global_operations.fk_global_users_id = global_users.pk_id ",
                                 MainTableName, _sqlQueryPostfix);
            
            using (NpgsqlConnection conn = new NpgsqlConnection(MeasureConfig.TheConfig.SqlConnectionStr))
            {
                using (DataSet dset = new DataSet(TABLE_NAMESPACE_ID))
                using (NpgsqlDataAdapter NpAdapter = new NpgsqlDataAdapter())
                using (NpAdapter.SelectCommand = new NpgsqlCommand(sqlQuery, conn))
                {
                    NpAdapter.Fill(dset, MainTableName);
                    DataTable tableData = dset.Tables[MainTableName];
                    _dataGridView.DataSource = tableData;
                }
            }
        }

        string MainTableName { get; set; }


        /// <summary>
        /// Optional Filter for a directly joined table
        /// </summary>
        /// <param name="joinedTableName_in">main table must has fk_{joinedTableName_in}_id column</param>
        /// <returns></returns>
        public void JoinedTableFilter(string joinedTableName_in, string whereCondition_in)
        {
            /* e.g.:
             * LEFT JOIN ua3_terminal_workphases ON ua3_terminal.fk_ua3_terminal_workphases_id = ua3_terminal_workphases.pk_id 
                ua3_terminal_workphases.name = 'meo_fasttest' */

            _sqlQueryPostfix = string.Format(" LEFT JOIN {0} ON {1}.fk_{0}_id == {0}.pk_id WHERE {2}",
                joinedTableName_in, MainTableName, whereCondition_in);
        }
        string _sqlQueryPostfix;


        public long SqlId 
        { 
            get; 
            private set; 
        }

        private void DoubleClickHandler(object sender, EventArgs e)
        {
            SqlId = Convert.ToInt64(_dataGridView.SelectedRows[0].Cells[0].Value);
            DialogResult = DialogResult.OK;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            long res;
            if (long.TryParse(_tbSqlId.Text, out res))
            {
                bool found = false;
                foreach (DataGridViewRow row in _dataGridView.Rows)
                {
                    if (Convert.ToInt64(row.Cells[0].Value) == res)
                        found = true;
                }

                if (found)
                {
                    SqlId = res;
                    _buttonOk.Enabled = true;
                    errorProvider1.Clear();
                }
                else
                {
                    errorProvider1.SetError(_tbSqlId, Properties.Resources.LOAD_MEASURE_SELECT_FORM_ERROR_INFO_NOT_FOUND);
                    _buttonOk.Enabled = false;
                }

            }
            else
            {
                errorProvider1.SetError(_tbSqlId, Properties.Resources.LOAD_MEASURE_SELECT_FORM_ERROR_INFO_NOT_NUMBER);
                _buttonOk.Enabled = false;
            }
        }
    }
}
