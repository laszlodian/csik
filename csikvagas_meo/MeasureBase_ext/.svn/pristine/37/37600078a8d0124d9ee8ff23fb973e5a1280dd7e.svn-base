using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;

namespace e77.MeasureBase.Sql.MiddleLevel
{
    public class MappingTable<T>
    {
        /// <summary>
        /// Loads a Mapping sql table into dictionary
        /// </summary>
        /// <param name="tableName_in"></param>
        /// <param name="columnName_in"></param>
        /// <param name="emptyVal_in">DbNull value will be replaced by tis</param>
        public void Init(string tableName_in, string columnName_in, T emptyVal_in)
        {
            Data = new Dictionary<int,T>();
            using (NpgsqlConnection sqlConn = new NpgsqlConnection(MeasureConfig.TheConfig.SqlConnectionStr))
            {
                sqlConn.Open();

                try
                {
                    using (NpgsqlCommand query = new NpgsqlCommand(string.Format(
                        "select pk_id, {1} from {0}", tableName_in, columnName_in), sqlConn))
                    {
                        using (NpgsqlDataReader dr = query.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    if( dr[columnName_in] == DBNull.Value)
                                        Data.Add((int)dr["pk_id"], emptyVal_in);
                                    else
                                        Data.Add((int)dr["pk_id"], (T)dr[columnName_in]);
                                }

                                dr.Close();
                            }
                            else
                            {
                                dr.Close();
                            }
                        }
                    }
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public Dictionary<int, T> Data { get; private set; }

    }
}
