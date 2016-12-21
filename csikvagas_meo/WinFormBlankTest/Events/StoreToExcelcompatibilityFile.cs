using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using stdole;

namespace WinFormBlankTest.Events
{
    class StoreToExcelcompatibilityFile
    {
        
        private string WrongStep;
        private string ErrorText;
        private bool NotH62;
        private bool H62;
        private bool EarlyDribble;
        private bool DeviceReplace;
        private bool Remeasureded;
        private string LotID;
        private bool Invalidate;
        private string RollID;
        private string TubeSN;

        public StoreToExcelcompatibilityFile(string wrong_step, string error_text, bool not_h62, bool h62, bool earlyDribble, bool deviceReplace, bool remeasured, bool invalidate, string lot_id, string roll_id, string tube_sn)
        {
            // TODO: Complete member initialization
            this.WrongStep = wrong_step;
            this.ErrorText = error_text;
            this.NotH62 = not_h62;
            this.H62 = h62;
            this.EarlyDribble= earlyDribble;
            this.DeviceReplace = deviceReplace;
            this.Remeasureded = remeasured;
            this.Invalidate = invalidate;
            this.LotID = lot_id;
            this.RollID = roll_id;
            this.TubeSN = tube_sn;

            
        }
        public StreamWriter wr;
        private void ExportDataToCSV()
        {
        }
    }
}
