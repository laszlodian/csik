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
    public partial class HomogenityTestResult : Form
    {
        private DataSet dset;
        private NpgsqlConnection conn;
        private NpgsqlDataAdapter NpAdapter;
        private DataTable dtsource;
        private string roll;
        private string lot;

        public DataGridView dview;
       
        private int dataviews;


        public HomogenityTestResult(string lotid, string rollid)
        {
            lot = lotid;
            roll=rollid;

            
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
            dset = new DataSet("homogenity_result");
            NpAdapter = new NpgsqlDataAdapter();
            NpAdapter.SelectCommand = new NpgsqlCommand(string.Format("select * from homogenity_result where lot_id='{0}' and roll_id='{1}'",lot,roll), conn);
            NpAdapter.Fill(dset, "homogenity_result");
            dtsource = dset.Tables["homogenity_result"];

            dview = new DataGridView();

            dview.DataSource = dtsource;

         #region Define columns
            
            
            dtsource.Columns[0].ColumnName = "Lot ID";
            
            

            /* 
            column = new DataGridViewTextBoxColumn();
            column.Name = "Roll ID";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dview.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "Mérés típusa";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dview.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "Eredmény";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dview.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "Átlag";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dview.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "Átlag Értékelés";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dview.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "CV";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dview.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "CV Értékelés";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dview.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "Dátum";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dview.Columns.Add(column);


            column = new DataGridViewTextBoxColumn();
            column.Name = "Kieső csíkszám(glucose)";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dview.Columns.Add(column);


            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Homo_h62";
            column.Name = "H62 hibák száma";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dview.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Homo_not_h62";
            column.Name = "Nem H62 hibák száma";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dview.Columns.Add(column);

            btnColumn = new DataGridViewButtonColumn();
            btnColumn.HeaderText = "Adatok mentése(xls)";
            btnColumn.Text = "Adatok mentése(xls)";
            btnColumn.UseColumnTextForButtonValue = true;
            btnColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dview.Columns.Add(btnColumn);
             * */
            #endregion

            // Initialize the form.
            #region Init the form and the datagrid
           
            dview.Dock = DockStyle.Fill;       
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            dview.Name = "dview";


             //  dview.SetBounds(0, 0, Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);

               this.AutoSize = false;
               dview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
               this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Auto;
             
            
            #endregion

            this.Controls.Add(dview);
           dataviews = Controls.Find("dview", true).Length;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);

        }
    }
}
