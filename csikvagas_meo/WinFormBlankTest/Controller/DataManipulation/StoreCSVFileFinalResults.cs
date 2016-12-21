using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

using System.Reflection; // for the Missing.Value

namespace WinFormBlankTest.Controller.DataManipulation
{
    public class StoreCSVFileFinalResults
    {




        private void StoreCSVFile()

        {

            Excel.Application excelApp = new Excel.Application();

            string myPath = @"C:\ExcelReport.xls";



            excelApp.Workbooks.Open(myPath, Missing.Value, Missing.Value,

                                      Missing.Value, Missing.Value,

                                      Missing.Value, Missing.Value,

                                      Missing.Value, Missing.Value,

                                      Missing.Value, Missing.Value,

                                      Missing.Value, Missing.Value,

                                      Missing.Value, Missing.Value);



            int row = 12; // starting row, after my header





            excelApp.Cells[12, 2] = "sor1oszlop2";

            excelApp.Cells[12, 3] = "sor1oszlop3";

            excelApp.Cells[12, 4] = "sor1oszlop4";







            //these are some cleanup calls that I found in another example..

            System.Threading.Thread.Sleep(1000);

            excelApp.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            GC.Collect();

            GC.WaitForPendingFinalizers();

            System.Threading.Thread.Sleep(30000);

        }
    }
}
