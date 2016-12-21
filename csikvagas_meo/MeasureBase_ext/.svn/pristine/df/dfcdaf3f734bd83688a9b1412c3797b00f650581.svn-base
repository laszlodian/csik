using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;

namespace e77.MeasureBase.Sql
{
    public interface ISqlRowDescriptor
    {
        SqlRowDescriptor Sql { get; set; }
    }

    public interface ISqlLoadable : ISqlRowDescriptor
    {
        void SqlLoad(NpgsqlDataReader sqlData_in);
    }

    public interface ISqlSaveable : ISqlRowDescriptor
    {
        /// <summary>
        /// !do not forget to set the Sql.RowId
        /// </summary>
        /// <param name="insertCommand_in"></param>
        /// <param name="measure_in"></param>
        /// <param name="collectionKey_in"></param>
        /// <returns></returns>
        void SqlSave(NpgsqlCommand insertCommand_in);
    }

    public interface ISql : ISqlLoadable, ISqlSaveable  { }
}
