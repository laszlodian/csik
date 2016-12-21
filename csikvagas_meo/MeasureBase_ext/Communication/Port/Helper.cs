using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public enum EStandardBaudRates { _1200 = 1200, _2400 = 2400, _4800 = 4800, _9600 = 9600, _14400 = 14400, _19200 = 19200, _38400 = 38400, _57600 = 57600, _115200 = 115200 }
    /*
    public static class CommunicationHelper
    {
        /// <summary>
        /// System.ArgumentNullException: value_in is null.
        /// System.FormatException: value_in is not in the correct format.
        /// System.OverflowException: value_in s represents a number less than System.Int32.MinValue or greater than System.Int32.MaxValue.
        /// System.InvalidCastException integer at value_in is not match any standard Baud rate
        /// </summary>
        /// <param name="obj_in"></param>
        /// <param name="value_in"></param>
        public static void Parse(this StandardBaudRates obj_in, string value_in)
        {
            obj_in = (e77.MeasureBase.Communication.StandardBaudRates)int.Parse(value_in);
        }
    }*/
}