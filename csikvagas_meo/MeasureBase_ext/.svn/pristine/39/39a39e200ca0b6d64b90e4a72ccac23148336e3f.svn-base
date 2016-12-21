using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Npgsql;
using e77.MeasureBase;
using e77.MeasureBase.Model;

namespace e77.MeasureBase.Sql
{
    static public class SqlLowLevel
    {
        public const  string  COLUMN_NAME_ID = "pk_id";

        /// <summary>
        /// in order to avoid int conversion (and problem with  long.MinValue != int.MinValue) try to use always long member storing for SQL Row id in memory.
        /// </summary>
        public const  long    INVALID_ROW_ID = long.MinValue;
        private const string  PW_ID = "Password =";

        /// <summary>
        /// It is syntactically possible to use SQL Server reserved keywords as identifiers and object names in Transact-SQL scripts, this can be done only using delimited identifiers.
        /// Source: http://msdn.microsoft.com/en-us/library/aa238507%28v=sql.80%29.aspx
        /// </summary>
        public static readonly string[] SQL_RESERVED_NAMES = new string[] {
    "add", "except", "percent", "all", "exec", "plan", "alter", "execute", "precision", "and", "exists", "primary", "any", "exit", 
    "print", "as", "fetch", "proc", "asc", "file", "procedure", "authorization", "fillfactor", "public", "backup", "for", "raiserror", "begin", 
    "foreign", "read", "between", "freetext", "readtext", "break", "freetexttable", "reconfigure", "browse", "from", "references", 
    "bulk", "full", "replication", "by", "function", "restore", "cascade", "goto", "restrict", "case", "grant", "return", 
    "check", "group", "revoke", "checkpoint", "having", "right", "close", "holdlock", "rollback", "clustered", "identity", 
    "rowcount", "coalesce", "identity_insert", "rowguidcol", "collate", "identitycol", "rule", "column", "if", "save", 
    "commit", "in", "schema", "compute", "index", "select", "constraint", "inner", "session_user", "contains", "insert", "set", 
    "containstable", "intersect", "setuser", "continue", "into 	shutdown", "convert", "is", "some", "create", "join", "statistics", 
    "cross", "key", "system_user", "current", "kill", "table", "current_date", "left", "textsize", "current_time", "like", "then", 
    "current_timestamp", "lineno", "to", "current_user", "load", "top", "cursor", "national", "tran", "database", "nocheck", "transaction", 
    "dbcc", "nonclustered", "trigger", "deallocate", "not", "truncate", "declare", "null", "tsequal", "default", "nullif", "union", "delete", 
    "of", "unique", "deny", "off", "update", "desc", "offsets", "updatetext", "disk", "on", "use", "distinct", "open", "user", "distributed", 
    "opendatasource", "values", "double", "openquery", "varying", "drop", "openrowset", "view", "dummy", "openxml", "waitfo", "dump", "option", 
    "when", "else", "or", "where", "end", "order", "while", "errlvl", "outer", "with", "escape", "over", "writetext"};
        
        static public string CreateInsertCommand(SqlTableDescriptorBase sqlDesc_in)
        {
            return CreateInsertCommand(sqlDesc_in, null, false);
        }

        static public string CreateInsertCommand(SqlTableDescriptorBase sqlDesc_in, IEnumerable<string> additional_columns)
        {
            return CreateInsertCommand(sqlDesc_in, additional_columns, false);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlDesc_in"></param>
        /// <param name="additional_columns"></param>
        /// <param name="forcedSqlId_in">'pk_id' is added if it is true</param>
        /// <returns></returns>
        static public string CreateInsertCommand(SqlTableDescriptorBase sqlDesc_in, IEnumerable<string> additional_columns, bool forcedSqlId_in)
        {
            List<string> columns = new List<string>();
            columns.AddRange(sqlDesc_in.ColumnNames);
            if(additional_columns != null)
                columns.AddRange(additional_columns);

            if (forcedSqlId_in)
                columns.Add("pk_id");

            return CreateInsertCommand(sqlDesc_in.TableName,
                columns.Distinct(), SqlLowLevel.COLUMN_NAME_ID);
        }

        static public object GetNullable(this NpgsqlDataReader obj_in, string columnName_in)
        {
            if ( !obj_in.HasOrdinal(columnName_in)
                || obj_in.IsDBNull(obj_in.GetOrdinal(columnName_in)))
                return null;
            else
                return obj_in[columnName_in];
        }

        static public void AddWithValueNullable(this NpgsqlParameterCollection obj_in, string columnName_in, object value_in)
        {
            obj_in.AddWithValue(columnName_in, value_in != null ? value_in : (object)DBNull.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <param name="idColumnName_in">string.Empty - no ID, query can be executed by ExecuteNonQuery. Else the column can be asked by ExecuteScalar</param>
        /// <returns></returns>
        static public string CreateInsertCommand(string tableName, System.Collections.IEnumerable columns, string idColumnName_in)
        {
            return string.Format("INSERT INTO \"{0}\" ({1}) VALUES ({2}){3}", 
                            tableName,
                            columns.ItemsToString(", "),
                            columns.ItemsToString(", ", "@"),
                            idColumnName_in != string.Empty ?
                                string.Format(" RETURNING {0}", idColumnName_in) 
                                : string.Empty);
        }

        /// <summary>
        /// Creates WhereClause for sql query. Example:
        /// CreateWhereClause( new string[] {"c1", "c2"}) // => "where c1 = @c1 and c2 = @c2"
        /// Release note: this method is not usable for null parameter, it needs 'is' instead of '=' ("column_name is null") use CreateWhereClause(Dictionary params_in) if null parameter is required.
        /// </summary>
        /// <param name="columns_in"></param>
        /// <returns></returns>
        static public string CreateWhereClause(IEnumerable<string> columns_in)
        {
            StringBuilder res = new StringBuilder();
            res.Append("where ");
            bool firstItemAdded = false;
            foreach (string c in columns_in)
            {
                if (!firstItemAdded)
                    firstItemAdded = true;
                else
                    res.Append("and ");

                res.AppendFormat("{0} = @{0} ", c);
            }
            return res.ToString();
        }

        /// <summary>
        /// Creates Null-parameter compatible WhereClause for sql query, 
        /// CreateWhereClause( myDict ) // => "where col1 = @c1 and c2 is null"
        /// </summary>
        /// <param name="parameters_in">Key: column name, Value: used only for checking null or not null.</param>
        /// <returns></returns>
        static public string CreateWhereClause(Dictionary<string, object> parameters_in)
        {
            StringBuilder res = new StringBuilder();
            res.Append("where ");
            bool firstItemAdded = false;
            foreach (KeyValuePair<string, object> param in parameters_in)
            {
                if (!firstItemAdded)
                    firstItemAdded = true;
                else
                    res.Append("and ");

                if(param.Value != null)
                    res.AppendFormat("{0} = @{0} ", param.Key);
                else
                    res.AppendFormat("{0} is NULL ", param.Key);
            }
            return res.ToString();
        }


        static public void AddItemsToSqlInsert(NpgsqlCommand cmd, System.Collections.IDictionary dictionary_in, string keyPrefix_in)
        {
            foreach (System.Collections.DictionaryEntry pair in dictionary_in)
            {
                if (pair.Value is System.Collections.IDictionary)
                    throw new NotSupportedException("Multilevel is not supported, use HierarchyDatabaseHelper.FlattenDictionaryHierarchy");

                cmd.Parameters.AddWithValue(string.Format("@{0}{1}", keyPrefix_in, pair.Key), pair.Value);
            }
        }

        public static string RemovePasswordFromSqlConnectionString(string sqlConnectionString_in)
        {
            StringBuilder res = new StringBuilder();
            string[] sqlConnArray = sqlConnectionString_in.Split(';');
            foreach (string str in sqlConnArray)
                if (str.TrimStart().StartsWith("Password"))
                    res.Append(" Password = ***");
                else
                    res.AppendFormat("{0};", str);

            return res.ToString();
        }
        
        public static string DecryptPw(string connectionString_in)
        {
            int pwPos = connectionString_in.IndexOf(PW_ID);
            if (pwPos == -1)
                throw new NotFoundException( string.Format("SQL connection string '{0}' does not contains '{1}'",
                    connectionString_in, PW_ID));

            string pw = connectionString_in.Substring(pwPos + PW_ID.Length, connectionString_in.Length - pwPos - PW_ID.Length).Trim();

            return string.Format("{0}{1}",connectionString_in.Substring(0, pwPos + PW_ID.Length), CryptString(pw, false, 0/*not used for decrypt*/));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pw_in"></param>
        /// <param name="encrypt_not_descript_in"></param>
        /// <param name="algId_in">used only if encrypt_not_descript_in == true</param>
        /// <returns></returns>
        public static string CryptString(string pw_in, bool encrypt_not_descript_in, int algId_in)
        {
            if (!encrypt_not_descript_in)
            {
                algId_in = (int)(pw_in[0] - '0');
            }


            char[] buff = encrypt_not_descript_in ?
                pw_in.ToCharArray() :
                pw_in.Substring(1, pw_in.Length - 1).ToCharArray();//ignore ID
            

            string pw;

            switch(algId_in)
            {
                case 0: //no crypt
                    pw = new string(buff);
                    break;
                case 1:
                    int iPos = 0;
                    foreach (char ch in buff)
                    {
                        if(encrypt_not_descript_in)
                            buff[iPos] += (char)(iPos++ & 3);
                        else
                            buff[iPos] -= (char)(iPos++ & 3);
                    }
                    pw = new string(buff);
                    break;
                default:
                    throw new NotImplementedException(string.Format( "Nem létező titkosítás: {0}", algId_in));
            }
            
            return (encrypt_not_descript_in ? algId_in.ToString() : string.Empty)  //first char is the AlgId
                + pw;
        }
    }
}
