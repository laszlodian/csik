using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

namespace e77.MeasureBase.Beep
{

    /// <summary>
    /// Customizable Output for BeepHelper.
    /// Needs "BeepOutput" integer config value, at Application config.
    /// </summary>
    public static class BeepHelper
    {
        #region Variables
        public const string CONFIGNAME_BEEP_OUTPUT = "BeepOutput";
        static bool _speakerErrorReported;
        static public EBeepType Output;
        private static object lockObject = new object();
        private static object lockObject2 = new object();
        #endregion

        [Serializable]
        public enum EBeepType { None, Speaker, System };

        static public void Setup()
        {                     
            using (BeepConfigForm form = new BeepConfigForm())
            {
                form.Output = Output;
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Output = form.Output;
                    MeasureConfig.TheConfig.SetConfigValueOf("BeepOutput", (int)Output, true);
                }
            }
        }        

        #region BeepException
        [Serializable]
        public class BeepException : Exception
        {
            public BeepException() { }
            public BeepException(string message) : base(message) { }
            public BeepException(string message, Exception inner) : base(message, inner) { }
            protected BeepException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context)
            { }
        }
        #endregion
            
        /// <summary>
        /// 
        /// </summary>
        /// <param name="frequency"></param>
        /// <param name="duration"></param>
        static public void Beep(int frequency, int duration)
        {
            lock (lockObject)
            {      //TODO LDIAN HIGH PRIO -NSIVAK

                switch ((EBeepType)Enum.Parse(typeof(EBeepType), Properties.Settings.Default.BeepType))
                {
                    case EBeepType.None:
                        Trace.TraceInformation("Beeper type is set to: {0}", "None");
                        break;
                    case EBeepType.Speaker:
                        Trace.TraceInformation("Beeper type is set to: {0}", "Speaker");
                        if ((BeeperIO.IsDriverOK()) && (frequency > 18))
                        {
                            frequency = 1193181 / frequency;

                            BeeperIO.Out(0x43, 0xB6);
                            BeeperIO.Out(0x42, (byte)(frequency));
                            BeeperIO.Out(0x42, (byte)(frequency >> 8));

                            BeeperIO.Out(0x61, (byte)(BeeperIO.In(0x61) | 0x03));
                            Thread.Sleep(duration);
                            BeeperIO.Out(0x61, (byte)(BeeperIO.In(0x61) & 0xfc));
                        }
                        else
                        {
                            Thread.Sleep(duration);
                        }
                        break;
                    case EBeepType.System:
                        if (frequency > 0)
                        {
                            Trace.TraceInformation("Beeper type is set to: {0}", "System");
                            Console.Beep(frequency, duration);
                        }
                        else
                        {
                            Thread.Sleep(duration);
                        }
                        break;
                    default:
                        {
                            Trace.TraceError("Exception about EBeepType is not Sytem, Speaker");
                            throw new BeepException("Exception at Beeper static Constructor!");
                        }
                }
            }
        }

        static public bool CanUseSpeaker
        {
            get
            {
                return BeeperIO.IsDriverOK();
            }
        }

        public static void BeepOk()
        {
            lock (lockObject2)
            {
                Thread.Sleep(1000);
                for (int index = 0; index < 6; ++index)
                    Beep((int)(1000.0 * Math.Pow(2.0, (double)index / 12.0)), 200);
                Thread.Sleep(1000);
            }
        }

        public static void BeepError()
        {
            lock (lockObject2)
            {
                Thread.Sleep(1000);
                for (int index = 0; index < 3; ++index)
                {
                    Beep(1800, 200);
                    Beep(1500, 200);
                }
                Thread.Sleep(1000);
            }
        }

        public static void BeepErrorShort()
        {
            lock (lockObject2)
            {
                Beep(1800, 100);
                Beep(1500, 100);
            }
        }  

        //protected virtual void BeepDone()
        //{
        //    BeepHelper(800, 100);  //TODO NSIVAK Create BeepDone in Bepper....deeper and deeper
        //}

        //protected virtual void BeepOk()
        //{
        //    BeepHelper(800, 100);           //TODO NSIVAK Create BeepDone in Bepper....deeper and deeper
        //    BeepHelper(1200, 100);
        //}

        //public virtual void BeepFailed()
        //{
        //    BeepHelper(800, 50);
        //    BeepHelper(600, 50);
        //}

        //protected virtual void BeepShortError()
        //{
        //    //TODO NSIVAK Create BeepDone in Bepper....deeper and deeper
        //}


        //protected virtual void BeepError()
        //{
        //    //TODO NSIVAK Create BeepDone in Bepper....deeper and deeper
        //    BeepHelper(800, 50);
        //    BeepHelper(600, 50);
        //}
    }
}
