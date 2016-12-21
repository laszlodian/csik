using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Diagnostics;
using System.DirectoryServices.Protocols;

namespace e77.MeasureBase.Sql
{
    /// <summary>
    /// class for Operations SQL table
    /// </summary>
    public class Operation : ISqlSaveable, ISqlLoadable
    {
        static Operation()
        {
            if (TheOperation != null)
                throw new MeasureBaseException("Singleton");

            TheOperation = new Operation();

            SqlTableDescriptorEnvironmentId tableDesc = new SqlTableDescriptorEnvironmentId("global_operations",
                new string[] { "date", "sql_id", "sn", "param" },
                new SqlTableDescriptorAdditional[] {
                    new SqlTableDescriptorAdditionalNoObj( "global_operation_ids", new string[] { "description" },
                        SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.Unique,
                        delegate(Npgsql.NpgsqlDataReader sqlData_in)
                        {
                           Operation.TheOperation.OperationDescription = (string)sqlData_in["description"];
                        },
                        delegate()
                        {
                           Dictionary<string, object> data = new Dictionary<string, object>();
                           data.Add("description", Operation.TheOperation.OperationDescription);
                           return data;
                        }),
                    new SqlTableDescriptorAdditionalNoObj( "global_sql_tables", new string[] { "table_name" },
                        SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.Unique,
                        delegate(Npgsql.NpgsqlDataReader sqlData_in)
                        {
                            Operation.TheOperation.SqlTableName = (string)sqlData_in["table_name"];
                        },
                        delegate()
                        {
                           Dictionary<string, object> data = new Dictionary<string, object>();
                           data.Add("table_name", Operation.TheOperation.SqlTableName);
                           return data;
                        }) 
                } );

            TheOperation.Sql = new SqlRowDescriptor(tableDesc, TheOperation);

            TheOperation.Date = DateTime.Now;
        }

        static public Operation TheOperation { get; private set; }

        public long Save(NpgsqlTransaction transaction)
        {
            return Save(transaction.Connection);
        }

        public long Save(NpgsqlConnection connection_in)
        {
            return SqlMiddleLevel.Save(connection_in, this);
        }

        public const int MAX_PARAM_LENGHT = 1024;
        public const int MAX_DESCRIPTION_LENGHT = 1024;
        
        string _SN;
        public string Sn 
        {
            get { return _SN; }
            set 
            {
                if (_SN != value)
                {
                    if (value == string.Empty)
                        value = null; //store null instead of empty string

                    if (value != null && value.Length > 15)
                        throw new ArgumentException(string.Format("SN '{0}' max. lenght is 15.", value));
                 
                    _SN = value;

                    Sql.RowId = SqlLowLevel.INVALID_ROW_ID;
                }
            } 
        }

        public DateTime Date { get; set; }

        string _param;
        public string Param
        {
            get { return _param; }
            set
            {
                if (_param != value)
                {
                    if (value == string.Empty)
                        value = null; //store null instead of empty string

                    if (value != null && value.Length > MAX_PARAM_LENGHT)
                        throw new ArgumentException(string.Format("Param '{0}' max. lenght is {1}.", value, MAX_PARAM_LENGHT));

                    _param = value;

                    Sql.RowId = SqlLowLevel.INVALID_ROW_ID;
                }
            }
        }

        /// <summary>
        /// Virtual Foreign Key - Table Name 
        /// </summary>
        public string SqlTableName { get; set; }

        /// <summary>
        /// Virtual Foreign Key - Sql row ID (pk_id)
        /// </summary>
        public long SqlId { get; set; }
        
        string _operationDescription;
        public string OperationDescription
        {
            get { return _operationDescription; }
            set
            {
                if (_operationDescription != value)
                {
                    if (value.Length > MAX_DESCRIPTION_LENGHT)
                        throw new ArgumentException(string.Format("Description '{0}' max. lenght is {1}.", value, MAX_DESCRIPTION_LENGHT));
                    
                    _operationDescription = value;

                    Sql.RowId = SqlLowLevel.INVALID_ROW_ID;
                }
            }
        }
        #region ISqlSaveable Members

        public void SqlSave(Npgsql.NpgsqlCommand insertCommand_in)
        {
            if(OperationDescription == null || OperationDescription == string.Empty)
                throw new MeasureBaseException("OperationDescription has not been set.");

            insertCommand_in.Parameters.AddWithValue("@sql_id", SqlId);

            insertCommand_in.Parameters.AddWithValue("@date", Date);

            SqlLowLevel.AddWithValueNullable(insertCommand_in.Parameters, "@sn", Sn);
            SqlLowLevel.AddWithValueNullable(insertCommand_in.Parameters, "@param", Param);
        }
        #endregion

        #region ISqlLoadable Members

        public void SqlLoad(NpgsqlDataReader sqlData_in)
        {
            SqlId = (long)sqlData_in["sql_id"];

            Date = (DateTime)sqlData_in["date"];

            if (MeasureBase.Model.MeasureCollectionBase.TheMeasures != null
                && MeasureBase.Model.SqlHiLevel.Load_nContinue)
            {
                MeasureBase.Model.MeasureCollectionBase.TheMeasures.MeasureDate = Date;
            }

            Sn = (string)SqlLowLevel.GetNullable(sqlData_in, "sn");
            Param = (string)SqlLowLevel.GetNullable(sqlData_in, "param");
        }

        #endregion

        #region ISqlDescriptor Members
        public SqlRowDescriptor Sql { get; set; }
        #endregion
    }
}
