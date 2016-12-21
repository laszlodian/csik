using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace e77.MeasureBase
{
    public static class Conversations
    {
        public static readonly NumberFormatInfo DOT_DECIMAL_SEPARATOR;
        static Conversations()
        {
            DOT_DECIMAL_SEPARATOR = new NumberFormatInfo();
            DOT_DECIMAL_SEPARATOR.CurrencyDecimalSeparator = ".";
        }        

        /// <summary>
        /// this list is same as result (downcast), but must be copied at .NET3.5 
        /// These conversion and copy will be unnecessary at .NET4.0 (as I heared)
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="list_in"></param>
        /// <returns></returns>
        public  static List<TTo> DownCastAll<TFrom, TTo>(List<TFrom> list_in)
            where TFrom : TTo
        {
            return new List <TTo> (list_in.Cast<TTo>());
        }

        public static T SimpleConverter<T>(object o)
        {
            return (T)o;
        }
       
    }
}
