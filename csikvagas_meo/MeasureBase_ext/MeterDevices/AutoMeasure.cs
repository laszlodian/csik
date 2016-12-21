using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using e77.MeasureBase.Beep;

namespace e77.MeasureBase.MeterDevices
{
    public class AutoMeasure
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myMeter_in"></param>
        /// <param name="minAcceptedVal_in"></param>
        /// <param name="maxAcceptedDiff">Send event if the difference of last 3 items smaller than this val</param>
        public AutoMeasure(MeterDeviceBase myMeter_in, double minAcceptedVal_in, double maxAcceptedDiff)
        {
            MyMeter = myMeter_in;
            MinAcceptedVal = minAcceptedVal_in;
            MinAcceptedDiff = maxAcceptedDiff;
        }

        internal void MyMeter_MeasureDone(object sender, MeasureDoneEventArgs e)
        {
            if (e.Value > MinAcceptedVal)
            {
                if (!WaitForEmpty)
                {
                    if( MyMeter.IsStabilized(e) )
                    {
                        WaitForEmpty = true;//deny more event for this measure

                        if (MeasureDone != null)
                            MeasureDone.Invoke(this, e); 
                    }
                }
                else
                {
                    //waiting for disconect
                }
            }
            else
            {
                if(WaitForEmpty)
                    BeepHelper.BeepOk();

                WaitForEmpty = false;
            }
        }

        /// <summary>
        /// In order to avoid multiple measure messages for one measure, we deny eventy until zero has not been measured 
        /// </summary>
        internal bool WaitForEmpty { get; set; }

        internal void StartMeasure()
        {
            WaitForEmpty = true;
        }

        internal void StopMeasure()
        {
            MeasureDone = null;
        }

        public event MeasureDoneEventHandler MeasureDone;

        public MeterDeviceBase MyMeter { get; private set; }
        double MinAcceptedVal { get; set; }
        double MinAcceptedDiff { get; set; }
    }
}