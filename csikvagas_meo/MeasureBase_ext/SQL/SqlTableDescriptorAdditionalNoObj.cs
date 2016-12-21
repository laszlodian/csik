using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Sql
{
    /// <summary>
    /// TableDescriptor for Additional Sql Row, where data container object is exist.
    /// </summary>
    public class SqlTableDescriptorAdditionalNoObj : SqlTableDescriptorAdditionalLeaf
    {
        /// <summary>
        /// Callback does not get wapper class as param 
        /// </summary>
        /// <param name="tableName_in"></param>
        /// <param name="columnNames_in"></param>
        /// <param name="isUnique_in"></param>
        /// <param name="loadCallback_in"></param>
        /// <param name="getDataCallback_in"></param>
        /// <param name="nullAccepted_in"></param>
        public SqlTableDescriptorAdditionalNoObj(string tableName_in, string[] columnNames_in, ESQlAdditionalTableLeafOptions options_in,
            LoadDelegate loadCallback_in, GetDataDelegate getDataCallback_in)
            : base(tableName_in, columnNames_in, options_in)
        {
            LoadCallback = loadCallback_in;
            GetDataCallback = getDataCallback_in;
        }

        /// <summary>
        /// Callbacks gets wapper class as param 
        /// </summary>
        /// <param name="tableName_in"></param>
        /// <param name="columnNames_in"></param>
        /// <param name="isUnique_in"></param>
        /// <param name="loadCallback_in"></param>
        /// <param name="getDataCallback_in"></param>
        public SqlTableDescriptorAdditionalNoObj(string tableName_in, string[] columnNames_in, ESQlAdditionalTableLeafOptions options_in,
            LoadParamDelegate loadCallback_in, GetDataParamDelegate getDataCallback_in )
            : base(tableName_in, columnNames_in, options_in)
        {
            LoadParamCallback = loadCallback_in;
            GetDataParamCallback = getDataCallback_in;
        }

        public delegate void LoadDelegate(Npgsql.NpgsqlDataReader sqlData_in);
        public delegate Dictionary<string, object> GetDataDelegate();

        public delegate void LoadParamDelegate(SqlRowDescriptorAdditional forObj_in, Npgsql.NpgsqlDataReader sqlData_in);
        public delegate Dictionary<string, object> GetDataParamDelegate(SqlRowDescriptorAdditional forObj_in);

        internal LoadDelegate LoadCallback {get; private set;}
        internal GetDataDelegate GetDataCallback { get; private set; }

        internal LoadParamDelegate LoadParamCallback { get; private set; }
        internal GetDataParamDelegate GetDataParamCallback { get; private set; }
        
        override internal Dictionary<string, object> GetData(SqlRowDescriptorAdditional wrapper_in)
        {
            if (GetDataCallback != null)
                return GetDataCallback();
            else
                return GetDataParamCallback(wrapper_in);
        }

        override internal void LoadInternal(SqlRowDescriptorAdditional forObj_in, Npgsql.NpgsqlDataReader sqlData_in)
        {
            if (LoadCallback != null)
                LoadCallback(sqlData_in);
            else
                LoadParamCallback(forObj_in, sqlData_in);
        }
    }
}
