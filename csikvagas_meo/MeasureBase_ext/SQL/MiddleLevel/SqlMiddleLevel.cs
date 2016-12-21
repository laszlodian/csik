using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Diagnostics;
using e77.MeasureBase.Model;

namespace e77.MeasureBase.Sql
{
    static public class SqlMiddleLevel
    {        
        static public void Load(NpgsqlConnection sqlConnection_in, ISqlLoadable obj_in)
        {//CodeDuplication_SqlLoad

            //show after load(=initialize) Trace.TraceInformation("SqlMiddleLevel.Load() for obj {0}.", obj_in);

            using (NpgsqlCommand query = new NpgsqlCommand(
                    string.Format("select * from {0} where {1}={2}",
                    obj_in.Sql.TableDescriptor.TableName, SqlLowLevel.COLUMN_NAME_ID,
                    obj_in.Sql.RowId), sqlConnection_in))
            {
                using (NpgsqlDataReader dr = query.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read(); //only one line because ColumnName_ID (pk_id) should be unique

                        //fill RowId`s of additional tables
                        foreach (SqlRowDescriptorAdditional additional in obj_in.Sql.AdditoinalRecords)
                            additional.RowId = (long)Convert.ToInt64(dr[additional.TableDescriptor.FKeyToThis]);

                        obj_in.SqlLoad(dr);
                    }
                    else
                        throw new SqlNoValueException(
                            string.Format("table: '{0}', {1} = {2}",
                            obj_in.Sql.TableDescriptor.TableName,
                            SqlLowLevel.COLUMN_NAME_ID,
                            obj_in.Sql.RowId));

                    dr.Close();
                }
            }

            Trace.TraceInformation("SqlMiddleLevel.Load() object '{0}' loaded.", obj_in);

            foreach (SqlRowDescriptorAdditional additional in obj_in.Sql.AdditoinalRecords)
            {
                Trace.TraceInformation("Loading Additional: {0}", additional);

                if (additional is SqlRowDescriptorAdditionalLeaf)
                    ((SqlRowDescriptorAdditionalLeaf)additional).SqlLoadInternal(sqlConnection_in);
                else
                    throw new InvalidOperationException("Onyl sql HiLevel support hierarchy (SqlDescriptorAdditionalLeaf vs. SqlDescriptorAdditionalHierarchy).");
            }
        }

        static public long Save(NpgsqlTransaction transaction_in, ISqlSaveable obj_in)
        {
            return Save(transaction_in.Connection, transaction_in, obj_in, null);
        }

        /// <summary>
        /// No transaction support.
        /// </summary>
        /// <param name="connection_in"></param>
        /// <param name="obj_in"></param>
        /// <returns></returns>
        static public long Save(NpgsqlConnection connection_in, ISqlSaveable obj_in)
        {
            return Save(connection_in, null, obj_in, null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection_in"></param>
        /// <param name="transaction_in">can be null</param>
        /// <param name="obj_in">to store</param>
        /// <param name="savedDescriptors_out">collect the saved addidional tables for rollback support ( if the list is not null)</param>
        /// <returns></returns>
        static public long Save(NpgsqlConnection connection_in, NpgsqlTransaction transaction_in, ISqlSaveable obj_in, List<SqlRowDescriptor> savedDescriptors_out)
        {//CodeDuplication_SqlSave
            Trace.TraceInformation("SqlMiddleLevel.Save() for obj {0}.", obj_in);

            List<string> additionalForeignKeyColumns = new List<string>();
            foreach (SqlRowDescriptorAdditional additional in obj_in.Sql.AdditoinalRecords)
            {
                additionalForeignKeyColumns.Add(additional.TableDescriptor.FKeyToThis);

                if (additional is SqlRowDescriptorAdditionalLeaf)
                {
                    bool wasRowId = additional.RowId != SqlLowLevel.INVALID_ROW_ID;
                    ((SqlRowDescriptorAdditionalLeaf)additional).SqlStoreInternal(connection_in, null);
                    if (savedDescriptors_out  != null &&
                        !wasRowId && additional.RowId != SqlLowLevel.INVALID_ROW_ID)//there was not RowId, but there is now a new
                        savedDescriptors_out.Add(additional);
                }
            }

            using (NpgsqlCommand cmdInsert = new NpgsqlCommand(
                    SqlLowLevel.CreateInsertCommand(obj_in.Sql.TableDescriptor, additionalForeignKeyColumns),
                    connection_in))
            {
                cmdInsert.Transaction = transaction_in;

                //fill fkey to additional records:
                foreach (SqlRowDescriptorAdditional additional in obj_in.Sql.AdditoinalRecords)
                    cmdInsert.Parameters.AddWithValue(
                        "@" + additional.TableDescriptor.FKeyToThis, additional.RowId);

                if (MeasureCollectionBase.TheMeasures == null || MeasureCollectionBase.TheMeasures.MeasureDate == null)
                {
                    if (obj_in.Sql.TableDescriptor is SqlTableDescriptorEnvironmentId)
                        cmdInsert.Parameters.AddWithValue("@date", DateTime.Now);
                }
                else
                {
                    if (obj_in.Sql.TableDescriptor is SqlTableDescriptorEnvironmentId)
                        cmdInsert.Parameters.AddWithValue("@date", MeasureCollectionBase.TheMeasures.MeasureDate);
                }
                obj_in.SqlSave(cmdInsert);

                obj_in.Sql.RowId = (long)Convert.ToInt64(cmdInsert.ExecuteScalar());
            }

            return obj_in.Sql.RowId;
        }

        public static T[] QueryAColumn<T>(string connectionString_in, string query_in)
        {
            T[] res = null;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString_in))
            {
                connection.Open();
                try
                {
                    res = QueryAColumn<T>(connection, query_in);
                }
                finally
                {
                    connection.Close();
                }
            }
            return res;
        }

        public static T[] QueryAColumn<T>(NpgsqlConnection connection_in, string query_in, Dictionary<string,object> params_in)
        {
            Trace.TraceInformation("SqlMiddleLevel.QueryAColumn() query:'{0}'.", query_in);
            
            List<T>  res = new List<T>();
            using (NpgsqlCommand query = new NpgsqlCommand(query_in, connection_in))
            {
                if (params_in != null)
                {
                    Trace.TraceInformation("SqlMiddleLevel.QueryAColumn() params:'{0}'.", params_in.ItemsToString());

                    foreach (KeyValuePair<string, object> param in params_in)
                        query.Parameters.AddWithValueNullable(param.Key, param.Value);
                }

                using (NpgsqlDataReader dr = query.ExecuteReader())
                {
                    if (dr.HasRows)
                        while (dr.Read())
                            res.Add((T)dr[0]);
                }
            }
            Trace.TraceInformation("SqlMiddleLevel.QueryAColumn() result:'{0}'.", res.ItemsToString());
            return res.ToArray();
        }

        public static T[] QueryAColumn<T>(NpgsqlConnection connection_in, string query_in)
        {
            return QueryAColumn<T>(connection_in, query_in, null);
        }

        public static object QueryScalar(NpgsqlConnection connection_in, string query_in)
        {
            return QueryScalar(connection_in, query_in, null);
        }

        public static object QueryScalar(NpgsqlConnection connection_in, string query_in, Dictionary<string, object> params_in)
        {
            object res = null;
            Trace.TraceInformation("SqlMiddleLevel.QueryScalar()  query:'{0}'", query_in);
            using (NpgsqlCommand query = new NpgsqlCommand(query_in, connection_in))
            {
                if (params_in != null)
                {
                    Trace.TraceInformation("SqlMiddleLevel.QueryScalar() params:'{0}'.", params_in.ItemsToString());

                    foreach (KeyValuePair<string, object> param in params_in)
                        query.Parameters.AddWithValueNullable(param.Key, param.Value);
                }

                res = query.ExecuteScalar();
                Trace.TraceInformation("SqlMiddleLevel.QueryScalar() result: '{0}'", res);
            }
            return res;
        }

        public static object QueryScalar(string connection_str, string query_in)
        {
            object res = null;
            using (NpgsqlConnection connection = new NpgsqlConnection(connection_str))
            {
                connection.Open();
                try
                {
                    Trace.TraceInformation("SqlMiddleLevel.QueryScalar() query:'{0}'", query_in);
                    using (NpgsqlCommand query = new NpgsqlCommand(query_in, connection))
                    {
                        res = QueryScalar(connection, query_in);
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return res;
        }

        public static object QueryScalar(string query_in)
        {
            return QueryScalar(MeasureConfig.TheConfig.SqlConnectionStr, query_in);
        }
    }
}
