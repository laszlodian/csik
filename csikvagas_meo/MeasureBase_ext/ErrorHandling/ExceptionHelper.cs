using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase
{
    public static class ExceptionHandlingHelpers
    {
        /// <summary>
        /// Creates an long error string of an exception.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string ReportError(this Exception e)
        {
            return ReportError(e, false/*keep current store settings*/);
        }

        /// <summary>
        /// Creates detailed info of an exception.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="markTracefileForStore_in"></param>
        /// <returns></returns>
        public static string ReportError(this Exception e, bool markTracefileForStore_in)
        {
            if (markTracefileForStore_in)
                TraceHelper.MarkTraceFileForStore(e.GetType().ToString());

            StringBuilder s = new StringBuilder(2048);
            s.AppendFormat("{0}, Source: {1}\n", e.GetType().Name, e.Source);
            s.AppendLine(e.Message);
            
            if (e is Npgsql.NpgsqlException)
            {
                s.AppendFormat("\nSql parancs: '{0}'", (e as Npgsql.NpgsqlException).ErrorSql);
            }
            
            s.Append("\nCall Stack:\n");
            s.AppendLine(e.StackTrace);

            Exception iterator = e.InnerException;
            while (iterator != null)
            {
                s.AppendLine();
                s.AppendLine("InnerException:");

                s.AppendLine(iterator.ReportError());
                s.AppendLine();

                iterator = iterator.InnerException;
            }
            return s.ToString();
        }
    }

}
