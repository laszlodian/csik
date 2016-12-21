using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.Sql;

namespace e77.MeasureBase.Sql
{
    /// <summary>
    /// Interface for container object for SQL Additional record. 
    /// If you does not has object for the additional Sql record, use SqlTableDescriptorAdditionalNoObj.
    /// </summary>
    public interface ISqlAdditionalObj
    {
        Dictionary<string, object> GetSqlData();
        void SqlLoad(Npgsql.NpgsqlDataReader sqlData_in);
    }

    /// <summary>
    /// TableDescriptor for Additional Sql Row, where data container object is exist.
    /// </summary>
    public class SqlTableDescriptorAdditionalObj : SqlTableDescriptorAdditionalLeaf
    {
        /// <summary>
        /// Obtains the ISqlAdditionalObj container object for SQL record.
        /// </summary>
        /// <returns>Container object for SQL record</returns>
        public delegate ISqlAdditionalObj GetObjectDelegate();
        /// <summary>
        /// Obtains the ISqlAdditionalObj container object for SQL record.
        /// FW provides SqlDescriptorAdditional warpper as parameter
        /// </summary>
        /// <returns>Container object for SQL record</returns>
        public delegate ISqlAdditionalObj GetObjectDelegateParam(SqlRowDescriptorAdditional forThis_in);
        
        /// <summary>
        /// Callback does not has any parameter.
        /// </summary>
        /// <param name="tableName_in"></param>
        /// <param name="columnNames_in"></param>
        /// <param name="isUnique_in"></param>
        /// <param name="getObjectCallback_in"></param>
        public SqlTableDescriptorAdditionalObj(string tableName_in, IEnumerable<string> columnNames_in, ESQlAdditionalTableLeafOptions options_in, GetObjectDelegate getObjectCallback_in)
            : base(tableName_in, columnNames_in, options_in)
        {
            GetStaticObjectCallback = getObjectCallback_in;
        }

        /// <summary>
        /// Callback function gets SqlDescriptorAdditional warpper as parameter
        /// </summary>
        /// <param name="tableName_in"></param>
        /// <param name="columnNames_in"></param>
        /// <param name="isUnique_in"></param>
        /// <param name="getObjectCallback_in"></param>
        public SqlTableDescriptorAdditionalObj(string tableName_in, IEnumerable<string> columnNames_in, ESQlAdditionalTableLeafOptions options_in, GetObjectDelegateParam getObjectCallback_in)
            : base(tableName_in, columnNames_in, options_in)
        {
            GetStaticObjectParamCallback = getObjectCallback_in;
        }
                
        internal GetObjectDelegate GetStaticObjectCallback;
        internal GetObjectDelegateParam GetStaticObjectParamCallback;

        internal ISqlAdditionalObj Wrapper(SqlRowDescriptorAdditional wrapper_in)
        {
            if (GetStaticObjectCallback != null)
                return GetStaticObjectCallback();
            else
                return GetStaticObjectParamCallback(wrapper_in);
        }

        override internal Dictionary<string, object> GetData(SqlRowDescriptorAdditional wrapper_in)
        {
            return Wrapper(wrapper_in).GetSqlData();
        }

        override internal void LoadInternal(SqlRowDescriptorAdditional forObj_in, Npgsql.NpgsqlDataReader sqlData_in)
        {
            Wrapper(forObj_in).SqlLoad(sqlData_in);
        }

        
    }
}
