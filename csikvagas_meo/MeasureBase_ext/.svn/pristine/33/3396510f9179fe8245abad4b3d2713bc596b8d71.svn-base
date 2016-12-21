using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Reporting
{
    public class DateTimeInterval
    {
        public enum EPredefinedInterval { Today, ThisWeek }

        public DateTimeInterval()
        { ;}

        public DateTimeInterval(EPredefinedInterval predef_in)
        {
            PredefinedInterval = predef_in;
        }

        public DateTimeInterval(DateTimeInterval obj_in)
        {
            CopyFrom(obj_in);
        }

        private void CopyFrom(DateTimeInterval obj_in)
        {
            this.PredefinedInterval = obj_in.PredefinedInterval;

            this.Start = obj_in.Start;
            this.End = obj_in.End;
        }
        
        public EPredefinedInterval? PredefinedInterval
        {
            get { return _PredefinedInterval; }
            set
            {
                if (_PredefinedInterval == value)
                    return;

                _PredefinedInterval = value;
                if (value.HasValue)
                {
                    DateTime dateTimeNow = DateTime.Now;
                    DateTime dateTimeNowDate = dateTimeNow.Date;
                    
                    switch (value.Value)
                    {
                        case EPredefinedInterval.Today:
                            Start = dateTimeNowDate;
                            End = dateTimeNowDate + new TimeSpan(1, 0, 0, 0);
                            break;
                        case EPredefinedInterval.ThisWeek:
                            int elapsedDays = dateTimeNow.DayOfWeek - DayOfWeek.Monday;
                            if (elapsedDays < 0) //sunday == 0, monday == 1
                                elapsedDays = 6;
                            Start = dateTimeNowDate - new TimeSpan(-elapsedDays, 0, 0, 0);
                            End = dateTimeNowDate + new TimeSpan(1, 0, 0, 0); //this day
                            break;

                        default:
                            throw new InvalidOperationException( string.Format("'{0}' unknown type", value.Value) );
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>changed</returns>
        public bool ShowSettingDialog() 
        {
            DateTimeIntarvalForm form = new DateTimeIntarvalForm(this);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CopyFrom(form.DateTimeIntervalObj);
                return true;
            }
            return false;
        }

        public static string PredefinedToHungarian(EPredefinedInterval? predef_in)
        {
            if (predef_in.HasValue)
            {
                switch (predef_in.Value)
                {
                    case EPredefinedInterval.Today:
                        return "Ma";
                    case EPredefinedInterval.ThisWeek:
                        return "Ez a hét";
                    default:
                        throw new InvalidOperationException(
                            string.Format("'{0}' unknown type", predef_in.Value));
                }
            }
            else
                return "Nincs";
        }

        public override string ToString()
        {
            if (Start == DateTime.MinValue)
                throw new InvalidOperationException("DateTimeInterval object has not been initialized");

            StringBuilder res = new StringBuilder();
            string resultFormatStr = string.Empty;

            if (_PredefinedInterval.HasValue)
            {
                switch (_PredefinedInterval.Value)
                {
                    case EPredefinedInterval.Today:
                        return string.Format("Ma ({0})", Start.ToShortDateString());
                    case EPredefinedInterval.ThisWeek:
                        return string.Format("Ez a hét ({0} - {1})",
                            Start.ToShortDateString(),
                            End.ToShortDateString());
                    default:
                        throw new InvalidOperationException(
                            string.Format("'{0}' unknown type", _PredefinedInterval.Value));

                }
            }
            else
            {   //not predefined
                return string.Format("{0} - {1}", Start.ToString("f"), End.ToString("f")); // 'f' = The Full Date Short Time
            }
        }

        internal EPredefinedInterval? _PredefinedInterval;

        public DateTime Start { get; set; } // accepted time >= _Start
        public DateTime End { get; set; } // accepted time < _End
    }
}
