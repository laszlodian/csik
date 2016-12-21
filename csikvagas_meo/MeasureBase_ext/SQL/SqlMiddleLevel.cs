using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;

namespace e77.MeasureBase.Sql
{
    static public class SqlMiddleLevel
    {        
        static public void Load(NpgsqlConnection sqlConnection_in, ISqlLoadable obj_in)
        {//CodeDuplication_SqlLoad
            using (NpgsqlCommand query = new NpgsqlCommand(
                    string.Format("select * from {0} where {1}={2}",
                    obj_in.Sql.StaticDescriptor.TableName, SqlLowLevel.COLUMN_NAME_ID,
                    obj_in.Sql.RowId), sqlConnection_in))
            {
                using (NpgsqlDataReader dr = query.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read(); //only one line because ColumnName_ID (pk_id) should be unique

                        //fill RowId`s of additional tables
                        foreach (SqlDescriptorAdditional additional in obj_in.Sql.AdditoinalRecords)
                            additional.RowId = (long)Convert.ToInt64(dr[additional.StaticDescriptor.ColumnName_ParentFKeyToThis]);

                        obj_in.SqlLoad(dr);
                    }
                    else
                        throw new SqlNoValueException(
                            string.Format("table: '{0}', {1} = {2}",
                            obj_in.Sql.StaticDescriptor.TableName,
                            SqlLowLevel.COLUMN_NAME_ID,
                            obj_in.Sql.RowId));

                    dr.Close();
                }
            }

            //load additional tables of main table
            foreach (SqlDescriptorAdditional additional in obj_in.Sql.AdditoinalRecords)
            {
                if (additional is SqlDescriptorAdditionalLeaf)
                    ((SqlDescriptorAdditionalLeaf)additional).SqlLoadInternal(sqlConnection_in);
                else
                    throw new InvalidOperationException("Onyl sql HiLevel support hierarchy (SqlDescriptorAdditionalLeaf vs. SqlDescriptorAdditionalHierarchy).");
            }
        }

        static public long Save(NpgsqlTransaction transaction_in, ISqlSaveable obj_in)
        {//CodeDuplication_SqlSave
            List<string> additionalForeignKeyColumns = new List<string>();
            foreach (SqlDescriptorAdditional additional in obj_in.Sql.AdditoinalRecords)
            {
                additionalForeignKeyColumns.Add(additional.StaticDescriptor.ColumnName_ParentFKeyToThis);

                if(additional is SqlDescriptorAdditionalLeaf)
                    ((SqlDescriptorAdditionalLeaf)additional).SqlStoreInternal(transaction_in);
            }

            using (NpgsqlCommand cmdInsert = new NpgsqlCommand(
                    SqlLowLevel.CreateInsertCommand(obj_in.Sql.StaticDescriptor, additionalForeignKeyColumns),
                    transaction_in.Connection))
            {
                cmdInsert.Transaction = transaction_in;

                //fill fkey to additional records:
                foreach (SqlDescriptorAdditional additional in obj_in.Sql.AdditoinalRecords)
                    cmdInsert.Parameters.AddWithValue(
                        "@" + additional.StaticDescriptor.ColumnName_ParentFKeyToThis, additional.RowId);
                
                obj_in.SqlSave(cmdInsert);

                obj_in.Sql.RowId = (long)Convert.ToInt64(cmdInsert.ExecuteScalar());
            }

            return obj_in.Sql.RowId;
        }

        public static string[] QueryAColumn(string connectionString_in, string query_in)
        {
            string[] res = null;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString_in))
            {
                connection.Open();
                try
                {
                    res = QueryAColumn(connection, query_in);
                }
                finally
                {
                    connection.Close();
                }
            }
            return res;
        }

        public static string[] QueryAColumn(NpgsqlConnection connection_in, string query_in)
        {
            List<string>  res = new List<string>();
            using (NpgsqlCommand query = new NpgsqlCommand(query_in, connection_in))
            {
                using (NpgsqlDataReader dr = query.ExecuteReader())
                {
                    if (dr.HasRows)
                        while (dr.Read())
                            res.Add((string)dr[0]);
                }
            }
            return res.ToArray();
        }
    }
}
