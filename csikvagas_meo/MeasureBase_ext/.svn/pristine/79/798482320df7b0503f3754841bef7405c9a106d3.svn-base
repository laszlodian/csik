using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.Sql;
using Npgsql;

namespace e77.MeasureBase.Model
{
    /// <summary>
    /// Sql table descriptor of a additional table, which has child and/or additional tables (own table hierarchy)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlTableDescriptorAdditionalRoot<T> : SqlTableDescriptorAdditionalRoot_Internal
        where T :  ISqlHiLevel, new()
    {
        public SqlTableDescriptorAdditionalRoot(string tableName_in, IEnumerable<string> columnNames_in,
            IEnumerable<SqlTableDescriptorAdditional> additionalTables_in, ESqlConsistencyType consistencyType_in)
            : base(tableName_in, columnNames_in, additionalTables_in, consistencyType_in)
        {;}

        internal override Type StorableType
        {
            get { return typeof(T); }
        }

        override public ISqlHiLevel CreateStorableClass()
        {
            T obj = new T();
            obj.Sql = new SqlRowDescriptorHierarchy(this, obj);
            return obj;
        }
    }

    public abstract class SqlTableDescriptorAdditionalRoot_Internal : SqlTableDescriptorHierarhyTableBase
    {
        internal SqlTableDescriptorAdditionalRoot_Internal(string tableName_in, IEnumerable<string> columnNames_in,
            IEnumerable<SqlTableDescriptorAdditional> additionalTables_in, ESqlConsistencyType consistencyType_in)
            : base(tableName_in, consistencyType_in, null/*additional processed here*/)
        {
            //add default columns:
            ColumnNames = columnNames_in;

            List<SqlTableDescriptorAdditional> additionals;
            if(additionalTables_in != null)
                additionals = new List<SqlTableDescriptorAdditional>(additionalTables_in);
            else
                additionals = new List<SqlTableDescriptorAdditional>();

            this.AdditionalTables = additionals;

            AssertRecordsNumber = 1;
        }
    }
}
