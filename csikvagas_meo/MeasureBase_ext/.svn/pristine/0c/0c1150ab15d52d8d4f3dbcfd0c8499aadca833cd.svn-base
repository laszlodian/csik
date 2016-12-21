using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase;
using e77.MeasureBase.Sql;
using Npgsql;
using e77.MeasureBase.Model;

namespace e77.MeasureBase.Sql
{ 
    public class SqlLog : ISqlSaveable 
    {
        public enum ETableColumns { date, description, sql_id, sn };

        static SqlLog()
        {
            new SqlLog();
        }

        public SqlLog()
        {
            if (TheLog != null)
                throw new InvalidOperationException("Singleton");

            SqlTableDescriptorEnvironmentId tableDesc = new SqlTableDescriptorEnvironmentId(
                "global_log",                                                           /// A fő tábla neve
                Enum.GetNames( typeof(ETableColumns)),                                  ///    oszlopai (string lista)
                new SqlTableDescriptorAdditional[] {                                    ///    Al-tábla lista következik
                    new SqlTableDescriptorAdditionalNoObj(                              ///  Első al-tábla
                        "global_log_programs",                                                 /// Al-tábla neve
                        new string[] { "program_name"},                       ///    oszlopai (strimg lista)
                        SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.Unique,///    ez mia afene?
                        delegate(Npgsql.NpgsqlDataReader sqlData_in)                           ///    meg ez is? Talán a beolvasó.
                        {
                             throw new NotImplementedException();
                           //Operation.TheOperation.OperationDescription = (string)sqlData_in["description"];
                        },
                        delegate()                                                             ///    Ez írja ki az altáblát
                        {
                            Dictionary<string, object> data = new Dictionary<string, object>();       ///  Az al-tábla tartalma a data nevű dictionary lesz
                          
                            //program name: count from SwVersion
                            string[] assemblies =  SwVersion.TheSwVersion.SwVersionCurrent.Split(':'); 
                            data.Add("program_name", assemblies[0]);                                  ///     Első oszlop
                          
                            return data;
                        }),
                    new SqlTableDescriptorAdditionalNoObj(                                 ///  Következő al-tábla
                        "global_sql_tables",                                                   /// Al-tábla neve
                        new string[] { "table_name" },                                         ///    oszlopai (strimg lista)
                        SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.Unique,///    ez mia afene?
                        delegate(Npgsql.NpgsqlDataReader sqlData_in)                           ///    meg ez is? Talán a beolvasó.
                        {
                            throw new NotImplementedException();
                            //TheLog.TableName =  (string)SqlLowLevel.GetNullable(sqlData_in, "table_name");
                        },
                        delegate()                                                             ///    Ez írja ki az altáblát
                        {
                           Dictionary<string, object> data = new Dictionary<string, object>();
                           data.Add("table_name",TheLog.TableName);
                           return data;
                        }) });
            this.Sql = new SqlRowDescriptor(tableDesc, this);

            TheLog = this;
        }

        static public SqlLog TheLog  { get; private set;}

        public void SaveLog(string description_in)
        {   
            TableName = string.Empty;
            SqlId = null;
            Description = description_in;
            this.Sql.RowId = SqlLowLevel.INVALID_ROW_ID; //clear last id (avoids already stored exception)

            using (NpgsqlConnection sqlConn = new NpgsqlConnection(MeasureConfig.TheConfig.SqlConnectionStr))
            {
                sqlConn.Open();
                try
                {
                    SqlMiddleLevel.Save(sqlConn, this);
                }
                finally
                {
                    sqlConn.Close();
                }
            }//using (NpgsqlConnection sqlConn = new NpgsqlConnection(DBConnectionStr)) 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="description_in"></param>
        /// <param name="table_name_in">can be null</param>
        /// <param name="sql_id_in">can be null</param>
        /// <param name="sn_in">can be null or empty</param>
        public void SaveLog(NpgsqlTransaction transaction, string description_in, string table_name_in, long? sql_id_in, string sn_in)
        {
            SaveLog(transaction.Connection, description_in, table_name_in, sql_id_in, sn_in);
        }

        public void SaveLog(NpgsqlConnection connection_in, string description_in, string table_name_in, long? sql_id_in, string sn_in)
        {
            SaveLog(connection_in, description_in, table_name_in, sql_id_in, sn_in, true);
        }

        /// <summary>
        /// This function supports global_ tables storage and it is needed if you want to do
        ///anything with this MeasureBase or what add to your Solution?!
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="description_in"></param>
        /// <param name="table_name_in">can be null</param>
        /// <param name="sql_id_in">can be null</param>
        /// <param name="sn_in">can be null or empty</param>
        /// <param name="useCurrentTime_nMeasureTime">Time of the log record is current time or measure time</param>
        public void SaveLog(NpgsqlConnection connection_in, string description_in, string table_name_in, 
            long? sql_id_in, string sn_in, bool useCurrentTime_nMeasureTime)
        {
            TableName = table_name_in;
            if (TableName == string.Empty)
                TableName = null;

            SqlId = sql_id_in;
            if (SqlId.HasValue && sql_id_in.Value == SqlLowLevel.INVALID_ROW_ID)
                throw new ArgumentException("sql_id_in invalid row ID-t tartalmaz. Ha nincs rá szükség null-legyen.");

            if(sn_in == string.Empty)
                sn_in = null;

            if (sn_in != null && sn_in.Length > 15)
                throw new ArgumentException("Az Sn '{0}' sn_in hosszabb 15 karakternél", sn_in);

            Sn = sn_in;


            Description = description_in;
            this.Sql.RowId = SqlLowLevel.INVALID_ROW_ID; //clear last id (avoids already stored exception)
            LastSavedAdditionalDescriptors = new List<SqlRowDescriptor>();
            SqlMiddleLevel.Save(connection_in, null, this, LastSavedAdditionalDescriptors);
        }

        public List<SqlRowDescriptor> LastSavedAdditionalDescriptors
        {
            get;
            private set;
        }

        #region ISqlSaveable Members
        public void SqlSave(Npgsql.NpgsqlCommand insertCommand_in)
        {
            if (MeasureCollectionBase.TheMeasures == null || MeasureCollectionBase.TheMeasures.MeasureDate == null)
            {
                CurrentDateTimeToUse = DateTime.Now;
            }
            else
                CurrentDateTimeToUse = MeasureCollectionBase.TheMeasures.MeasureDate;

            insertCommand_in.Parameters.AddWithValue("@" + ETableColumns.date.ToString(), CurrentDateTimeToUse);

            insertCommand_in.Parameters.AddWithValue("@" + ETableColumns.description.ToString(), this.Description);

            insertCommand_in.Parameters.AddWithValue("@" + ETableColumns.sql_id.ToString(),
                this.SqlId.HasValue ? (object)this.SqlId.Value : null);

            SqlLowLevel.AddWithValueNullable(insertCommand_in.Parameters, "sn", Sn);
        }
        public SqlRowDescriptor Sql { get; set; }
        #endregion

        public string Description   { get; set; }
        public string TableName     { get; set; }
        public long?  SqlId           { get; set; }
        public string Sn { get; set; }
        public bool UseCurrentTime_nMeasureTime { get; internal set; }

        public DateTime CurrentDateTimeToUse { get; set; }
    }
}
