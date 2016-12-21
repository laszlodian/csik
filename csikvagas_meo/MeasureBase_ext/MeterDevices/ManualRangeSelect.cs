using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

namespace e77.MeasureBase.MeterDevices
{
    abstract public class ManualRangeSelect : MeterDeviceBase
    {
        public ManualRangeSelect()
        {
            InvalidRangeWnd = new InvalidRangeWindow();
            //irc = new InvalidRange();
        }

        /// <summary>
        /// For UI message
        /// </summary>
        protected string RequiredRangeName { get; set; }

        abstract internal protected double GetRangePrecision { get; }


        protected abstract bool IsRangeOk(string line_in, out string valueStr_out);

        public InvalidRangeWindow InvalidRangeWnd { get; private set; }

        override public void PocketReceived(string arrivedPocket_in)
        {
            Trace.TraceInformation("ManualRangeSelect.LineReceived(string arrivedLine_in = {0})", arrivedPocket_in);

            string valueStr;
            if (IsRangeOk(arrivedPocket_in, out valueStr))
            {
                HideForm();

                double value;
                if(double.TryParse(valueStr.Replace(',', '.'),
                        NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                {
                    OnMeasureDone(value, arrivedPocket_in);
                }
                else
                    Trace.TraceInformation("ManualRangeSelect.PocketReceived() Cant parse {0}. Pocked dropped.", valueStr);
            }
            else
            {
                Trace.TraceInformation("Showing Invalid Range Form");
                ShowForm(arrivedPocket_in);
            }
        }

        delegate void StringDelegate(string measuredLine_in);
        public void ShowForm(string measuredLine_in)
        {
            if (!InvalidRangeWnd.Dispatcher.CheckAccess())
            {
                InvalidRangeWnd.Dispatcher.Invoke(new StringDelegate(ShowForm), new object[] { measuredLine_in });
                return;
            }

            InvalidRangeWnd.Message = string.Format("Kérem váltson méréshatárt. Cél: {0}, Pillanatnyi mért érték: {1}",
                RequiredRangeName, measuredLine_in);

            InvalidRangeWnd.Show();
        }

        delegate void VoidDelegate();
        public void HideForm()
        {
            if (!InvalidRangeWnd.Dispatcher.CheckAccess())
            {
                InvalidRangeWnd.Dispatcher.Invoke(new VoidDelegate(this.HideForm));
                return;
            }

            InvalidRangeWnd.Hide();
        }
        
        /// <summary>
        /// Ellenőrzi, hogy a mért érték a kívánt méréshatárban található-e. Ha igen, a mért értéket is visszaadja.
        /// </summary>
        /// <param name="pattern_in"> A méréshatár "minta", pl "DC????.?? mA"</param>
        /// <param name="value_in">A kapott mért érték, pl "DC-024.31 mA"</param>
        /// <param name="valueStr_out">A tényleges érték:  "-024.31 mA"</param>
        /// <returns></returns>
        static public bool IsMatch(string pattern_in, string value_in, out string valueStr_out)
        {
            if (pattern_in == null)
                throw new ArgumentException();

            if (value_in == null)
                throw new ArgumentException();

            if (pattern_in.Length == value_in.Length)
            {
                for (int i = 0; i < pattern_in.Length; i++)
                {
                    if ((pattern_in[i] != value_in[i]) && (pattern_in[i] != '?'))
                    {
                        valueStr_out = string.Empty;
                        return false;
                    }
                }

                int tol = pattern_in.IndexOf("?");
                int ig = pattern_in.LastIndexOf("?");
                valueStr_out = value_in.Substring(tol, ig - tol + 1);

                return true;
            }
            valueStr_out = string.Empty;
            return false;
        }

        public override void StopContinousMeasure()
        {            
            base.StopContinousMeasure();
            HideForm();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                InvalidRangeWnd.Close();
            }

            base.Dispose(disposing);
        }
    }
}
