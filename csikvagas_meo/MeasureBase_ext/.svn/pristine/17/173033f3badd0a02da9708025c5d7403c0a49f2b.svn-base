using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.Sql;
using Npgsql;
using e77.MeasureBase.Model;
using System.Diagnostics;

namespace e77.MeasureBase.Sql
{
    abstract public class SqlRowDescriptorAdditional : SqlRowDescriptor
    {
        public SqlRowDescriptorAdditional(SqlTableDescriptorAdditional tableDescriptor_in, SqlRowDescriptor parent_in)
            : base(tableDescriptor_in, parent_in.ContainerObject)
        {
            Parent = parent_in;
        }

        public SqlRowDescriptor Parent { get; protected set; }

        new public SqlTableDescriptorAdditional TableDescriptor
        {
            get { return (SqlTableDescriptorAdditional)base.TableDescriptor; }
            protected set { base.TableDescriptor = value; }
        }
    }

    public class SqlRowDescriptorAdditionalLeaf : SqlRowDescriptorAdditional
    {
        public SqlRowDescriptorAdditionalLeaf(SqlTableDescriptorAdditionalLeaf tableDescriptor_in, SqlRowDescriptor parent_in)
            : base(tableDescriptor_in, parent_in)
        { 
            Parent = parent_in;
        }

        new public SqlTableDescriptorAdditionalLeaf TableDescriptor
        {
            get { return (SqlTableDescriptorAdditionalLeaf)base.TableDescriptor; }
            protected set { base.TableDescriptor = value; }
        }
        
        internal void SqlStoreInternal(NpgsqlTransaction transaction_in)
        {
            SqlStoreInternal(transaction_in.Connection, transaction_in);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection_in"></param>
        /// <param name="transaction_in">can be null</param>
        internal void SqlStoreInternal(NpgsqlConnection connection_in, NpgsqlTransaction transaction_in)
        {
            Trace.TraceInformation("SqlStoreInternal() for obj {0}.", this);

            if (this.RowId != SqlLowLevel.INVALID_ROW_ID)
                return; //already sored

            //obtain cells data:
            Dictionary<string, object> data = this.TableDescriptor.GetData(this);

            if( data == null)
            {
                if (((this.TableDescriptor.Options & SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.NullAccepted) != 0))
                    return;
                else
                    throw new Exception(string.Format("NullAccepted is not specified for table descriptor {0}, but the returned data is null.", this.TableDescriptor));
            }

            if ((this.TableDescriptor.Options 
                    & (SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.AlreadyExist 
                      | SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.Unique)) != 0)
            {
                //try to found existing element:
                using (NpgsqlCommand query = new NpgsqlCommand(
                        string.Format("select pk_id from {0} {1}",
                        this.TableDescriptor.TableName,
                        SqlLowLevel.CreateWhereClause(data)), connection_in))
                {
                    //fill parameters:
                    foreach (KeyValuePair<string, object> pair in data)
                        SqlLowLevel.AddWithValueNullable(query.Parameters, pair.Key, pair.Value);

                    using (NpgsqlDataReader dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            this.RowId = (long)Convert.ToInt64(dr["pk_id"]);

                            //error handling: if (dr.Has More Rows) 
                            if (dr.Read())
                            {
                                throw new MeasureBaseInternalException("There is MORE record for {0}", this);
                            }
                        }
                        dr.Close();
                    }                        
                }
            }

            if ((this.TableDescriptor.Options & SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.AlreadyExist) != 0)
            {
                if(this.RowId == SqlLowLevel.INVALID_ROW_ID)
                    throw new SqlNotFoundException(this.TableDescriptor.TableName, data);

                return;
            }

            //create new one
            if (this.RowId == SqlLowLevel.INVALID_ROW_ID)
            {
                string insertCommand = SqlLowLevel.CreateInsertCommand(TableDescriptor);
                using (NpgsqlCommand cmdInsert = new NpgsqlCommand(insertCommand, connection_in))
                {
                    cmdInsert.Transaction = transaction_in;

                    //copy original parameters:
                    foreach (KeyValuePair<string, object> pair in data)
                        cmdInsert.Parameters.AddWithValue(string.Format("@{0}", pair.Key), pair.Value == null ? DBNull.Value : pair.Value);

                    object rowId = cmdInsert.ExecuteScalar();
                    this.RowId = (long)Convert.ToInt64(rowId);
                }
            }

            Trace.TraceInformation("SqlStoreInternal() result this.RowId: {0}", this.RowId);
        }//SqlStoreInternal

        internal void SqlLoadInternal(NpgsqlConnection sqlConn_in)
        {
            Trace.TraceInformation("SqlLoadInternal() for obj {0}.", this);

            if (this.RowId == SqlLowLevel.INVALID_ROW_ID)
            {
                if ((this.TableDescriptor.Options & SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.NullAccepted) != 0)
                    return;

                throw new MeasureBaseInternalException("RowId must be set before calling this function. (It is stored in parent table fk_{0}_id )", this.TableDescriptor.TableName);
            }

            using (NpgsqlCommand query = new NpgsqlCommand(
                string.Format("select * from {0} where {1}={2}",
                this.TableDescriptor.TableName, SqlLowLevel.COLUMN_NAME_ID, this.RowId), sqlConn_in))
            {
                using (NpgsqlDataReader dr = query.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        this.TableDescriptor.LoadInternal(this, dr);

                        this.SqlLoadedResults = new Dictionary<string, object>();
                        //add all cels if ...TableLeafOptions.OnlyResult or where column name begins with "res_"
                        for (int i = 0; i < dr.FieldCount; i++)
                            if ( ((this.TableDescriptor.Options & SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.OnlyResult) != 0)
                            || dr.GetName(i).StartsWith(SqlHiLevel.COLUMN_NAME_PREFIX_RESULT))
                            {
                                SqlLoadedResults.Add(dr.GetName(i), dr[i]);
                            }

                        if (dr.Read())
                            throw new MeasureBaseInternalException("There is MORE record for {0}", this);

                    }
                    else
                        throw new MeasureBaseInternalException("There is no stored record for {0}.", this);
                }
            }//using query
        }

        /// <summary>
        /// For consistency check
        /// </summary>
        internal Dictionary<string, object> SqlLoadedResults { get; private set; }

        internal void CheckConsistency()
        {
            //obtain current cells data:
            Dictionary<string, object> current = this.TableDescriptor.GetData(this);

            foreach(KeyValuePair<string, object> loaded in SqlLoadedResults)
            {
                if (loaded.Key == SqlLowLevel.COLUMN_NAME_ID)
                    continue;

                if (current.ContainsKey(loaded.Key))
                {
                    bool isSame;

                    if (loaded.Value is float && current[loaded.Key] is float)
                        isSame = ((float)loaded.Value).IsEqual((float)current[loaded.Key]);
                    else if (loaded.Value == DBNull.Value)
                        isSame = true; //soted value is null => deny consistency check, because  newly added columns of old measures contans null...
                    else
                        isSame = (loaded.Value.ToString() == current[loaded.Key].ToString());                    

                    //reason of usage of ToString(): (object)0f != (object)0f
                    if (!isSame)
                    {
                        StringBuilder errorMsg = new StringBuilder();
                        errorMsg.AppendFormat("Sql stored collection inconsistency for table: {0} rowId: {1}, Column: {2}.\n", 
                            this.TableDescriptor.TableName, this.RowId, loaded.Key);
                        errorMsg.AppendFormat("Sql stored value: {0}\n", loaded.Value);
                        errorMsg.AppendFormat("Counted value: {0}", current[loaded.Key]);

                        throw new SqlResultInconsistencyException(errorMsg.ToString());
                    }
                }
                else
                    throw new MeasureBaseInternalException(string.Format("ResultColumn {0} cannot be founeded at returned parameters of {1}.GetData ",
                                    loaded.Key, this));
            }
        }

        public override string ToString()
        {
            return string.Format("SqlRowDescriptorAdditionalLeaf object (RowId= {0}), TableDescriptor= {1}, Parent= {2}",
                this.RowId, this.TableDescriptor, this.Parent);
        }
    }
}
