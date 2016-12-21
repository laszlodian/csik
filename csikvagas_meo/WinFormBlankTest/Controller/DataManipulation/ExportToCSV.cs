using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WinFormBlankTest.Controller.DataManipulation
{
    public class ExportToCSV
    {
        public DataGridView dataGridView;
        public DataGridView dataGridView2;

        public ExportToCSV(DataGridView dgvToExport, DataGridView dgvToExport2, string filename)
        {

            dataGridView = dgvToExport;
            dataGridView2 = dgvToExport2;

            ToCsV(dataGridView,dataGridView2,string.Format("./ExportedFiles/{0}.csv",filename));



        }
       

        private void ToCsV(DataGridView dGV, DataGridView dGV2, string filename)
        {
            string stOutput = "";
            List<int> visibleColumnIndexes = new List<int>();
            #region Exporting dataGridView

            #region Export titles
            string sHeaders = "";


            foreach (DataGridViewColumn item in dGV.Columns)
            {
                if (item.Visible==true)
                {
                    visibleColumnIndexes.Add(item.Index);
                }
            }
            for (int j = 0; j < dGV.Columns.Count; j++)
            {
                if (visibleColumnIndexes.Contains(j))
                {
                    sHeaders = sHeaders.ToString() + Convert.ToString(dGV.Columns[j].HeaderText) + ";";
                }
             
            }
            
            stOutput += sHeaders + "\r\n";
            #endregion

            #region Export data
            for (int i = 0; i < dGV.RowCount ; i++)
            {
                 
                        string stLine = "";
                        for (int j = 0; j < dGV.Rows[i].Cells.Count; j++)
                        {
                            if (visibleColumnIndexes.Contains(j))
                            {
                                stLine = stLine.ToString() + Convert.ToString(dGV.Rows[i].Cells[j].Value) + ";";
                            }
                            
                        }                          
                
                
                stOutput += stLine + "\r\n";
            }
            #endregion 
            
            #endregion

            stOutput += "\r\n";
            stOutput += "\r\n";

            #region Exporting dataGridView2

            visibleColumnIndexes = new List<int>();
            foreach (DataGridViewColumn item in dGV2.Columns)
            {
                if (item.Visible == true)
                {
                    visibleColumnIndexes.Add(item.Index);
                }
            }
            #region Export titles(dgv2)
             sHeaders = "";
            for (int j = 0; j < dGV2.Columns.Count; j++)
                  if (visibleColumnIndexes.Contains(j))
                            sHeaders = sHeaders.ToString() + Convert.ToString(dGV2.Columns[j].HeaderText) + ";";
        
            stOutput += sHeaders + "\r\n";
            #endregion

            #region Export data(dgv2)

            for (int i = 0; i < dGV2.RowCount; i++)
            {
                string stLine = "";
                for (int j = 0; j < dGV2.Rows[i].Cells.Count; j++)
                    if (visibleColumnIndexes.Contains(dGV2.Rows[i].Cells[j].ColumnIndex))
                      stLine = stLine.ToString() + Convert.ToString(dGV2.Rows[i].Cells[j].Value) + ";";
                stOutput += stLine + "\r\n";
            }

            #endregion 
            #endregion



            Encoding utf16 = Encoding.GetEncoding(1254);
            byte[] output = utf16.GetBytes(stOutput);
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(output, 0, output.Length); //write the encoded file
            bw.Flush();
            bw.Close();
            fs.Close();

        }

    }
}