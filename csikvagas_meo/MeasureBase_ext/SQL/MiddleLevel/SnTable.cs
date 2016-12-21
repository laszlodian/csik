using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Diagnostics;

namespace e77.MeasureBase.Sql.MiddleLevel
{
    public static class SnTable
    {
        /// <summary>
        /// Get the is_reference column value from an sn table
        /// </summary>
        /// <param name="tableName_in"></param>
        /// <param name="sn_in"></param>
        /// <param name="exist_out"></param>
        /// <param name="isReference_out"></param>
        public static void GetSnInfo(string tableName_in, string sn_in, out bool exist_out, out bool isReference_out)
        {
            Trace.TraceInformation("GetSnInfo for {0} / {1}", tableName_in, sn_in);

            exist_out = false;
            isReference_out = false;

            using (NpgsqlConnection sqlConn = new NpgsqlConnection(MeasureConfig.TheConfig.SqlConnectionStr))
            {
                sqlConn.Open();
                using (NpgsqlCommand query = new NpgsqlCommand(string.Format(
                    "SELECT is_reference FROM {0} where sn = '{1}'", tableName_in, sn_in), sqlConn))
                {
                    using (NpgsqlDataReader dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            exist_out = true;
                            dr.Read();

                            if (!dr.IsDBNull(dr.GetOrdinal("is_reference"))
                                && (bool)dr["is_reference"])
                                isReference_out = true;
                        }
                        dr.Close();
                    }
                }
                sqlConn.Close();
            }

            Trace.TraceInformation("GetSnInfo results: exist_out: {0}, isReference_out: {1}", exist_out, isReference_out);
        }
    }
}
