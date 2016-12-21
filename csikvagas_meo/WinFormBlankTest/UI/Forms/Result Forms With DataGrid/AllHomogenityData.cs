using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace WinFormBlankTest.UI.Forms.Result_Forms_With_DataGrid
{
    public partial class AllHomogenityData : Form
    {
        private NpgsqlConnection conn;
        private NpgsqlDataAdapter NpAdapter;
        private DataSet dset;
        private DataTable dtsource;
        public AllHomogenityData()
        {
            InitializeComponent();
            ConnectToData();
        }
        public void ConnectToData()
        {
            string DSN;

            DSN = Properties.Settings.Default.DBReleaseConnection;
            if (DSN == "")
                return;

            conn = new NpgsqlConnection(DSN);
            dset = new DataSet("blank_test_result");
            NpAdapter = new NpgsqlDataAdapter();
            NpAdapter.SelectCommand = new NpgsqlCommand("select * from blank_test_result where", conn);
            NpAdapter.Fill(dset, "blank_test_result");
            dtsource = dset.Tables["blank_test_result"];
            DataGridView dview = new DataGridView();
            dview.DataSource = dtsource;

            this.Controls.Add(dview);


        }
    }
}
