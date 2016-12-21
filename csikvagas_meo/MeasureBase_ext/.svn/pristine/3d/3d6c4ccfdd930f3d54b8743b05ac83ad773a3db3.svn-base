using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace e77.MeasureBase.MeterDevices
{
    public class MetexM4640 : ManualRangeSelect
    {
        private string ReqRangePattern;     // Actual pattern
        static MetexM4640()
        {
            RANGE_PATTERNS = new Dictionary<ERange, string>();
            RANGE_PATTERNS.Add(ERange.UDC200m, "DC????.??  mV");
            RANGE_PATTERNS.Add(ERange.UDC2,    "DC??.????   V");
            RANGE_PATTERNS.Add(ERange.UDC20,   "DC???.???   V");
            RANGE_PATTERNS.Add(ERange.UDC200,  "DC????.??   V");
            RANGE_PATTERNS.Add(ERange.UDC1000, "DC?????.?   V");

            RANGE_PATTERNS.Add(ERange.UAC200m, "AC????.??  mV");
            RANGE_PATTERNS.Add(ERange.UAC2,    "AC??.????   V");
            RANGE_PATTERNS.Add(ERange.UAC20,   "AC???.???   V");
            RANGE_PATTERNS.Add(ERange.UAC200,  "AC????.??   V");
            RANGE_PATTERNS.Add(ERange.UAC750,  "AC?????.?   V");

            RANGE_PATTERNS.Add(ERange.IDC2m,   "DC????.??  mA");
            RANGE_PATTERNS.Add(ERange.IDC20m,  "DC??.????  mA");
            RANGE_PATTERNS.Add(ERange.IDC200m, "DC???.???  mA");
            RANGE_PATTERNS.Add(ERange.IDC20,   "DC????.??   A");

            RANGE_PATTERNS.Add(ERange.IAC2m,   "AC????.??  mA");
            RANGE_PATTERNS.Add(ERange.IAC20m,  "AC??.????  mA");
            RANGE_PATTERNS.Add(ERange.IAC200m, "AC???.???  mA");
            RANGE_PATTERNS.Add(ERange.IAC20,   "AC????.??   A");
        }

        public MetexM4640() 
        {
            TerminationStr = null;
            PackageLenght = (13 + 1/*End of line*/) * 4;
        }


        public enum ERange { UDC200m, UDC2, UDC20, UDC200, UDC1000,
                             UAC200m, UAC2, UAC20, UAC200, UAC750,
                             IDC2m, IDC20m, IDC200m, IDC20,
                             IAC2m, IAC20m, IAC200m, IAC20    };
        static Dictionary<ERange, string> RANGE_PATTERNS;

        override public void Init()
        {
            base.Init();
            if (Port != null)
            {
                Port.RtsEnable = false;
                Port.DtrEnable = true;
            }
        }

       
        protected override bool IsRangeOk(string line_in, out string valueStr_out)
        {
            if (line_in.IndexOf('\r') == -1)
            {
                valueStr_out = string.Empty;
                return false;
            }

            return ManualRangeSelect.IsMatch(ReqRangePattern, line_in.Substring(0, line_in.IndexOf('\r')), out valueStr_out); 
        }


        internal protected override bool IsStabilized(MeasureDoneEventArgs e)
        {
            string[] lines = e.PackageContent.Split('\r');

            double[] values = new double[3];
               
            values[0] = e.Value;
            if(double.TryParse(lines[1].Replace(',', '.'),
                    NumberStyles.Any, CultureInfo.InvariantCulture, out values[1])
                && double.TryParse(lines[2].Replace(',', '.'),
                    NumberStyles.Any, CultureInfo.InvariantCulture, out values[2]))
            {
                double diff = values.Max() - values.Min();

                //stabil if the last and curr meas diff smaller than 2 digit:
                return diff < GetRangePrecision * 2;
            }
            else
            {
                Trace.TraceInformation("MetexM4640.IsStabilized() Cant parse '{0}' -> Pocket dropped.", e.PackageContent);
                return false;
            }
        }


        public void SetRange(ERange range_in)
        {
            ReqRangePattern = RANGE_PATTERNS[range_in];
            RequiredRangeName = range_in.ToString();
        }

        override internal protected double GetRangePrecision 
        {
            get
            {
                switch (RequiredRangeName)
                {
                    case "UDC20":
                        return 0.001;
                    case "UDC2":
                        return 0.0001;
                    default:
                        throw new InvalidOperationException(string.Format("RequiredRangeName invalid: '{0}'", RequiredRangeName));
                }
            }
        }

        override public void MeasureOnce()
        {
            base.MeasureOnce();

            if (ReqRangePattern == null)
                throw new Exception("SetRange has not been set");

            Port.Write("D");
        }
    }
}
