using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.Sql;
using e77.MeasureBase.Model;

namespace e77.MeasureBase.Sql
{
    /// <summary>
    /// Additional Table: parent table has a foreign key to the additional table.  
    ///     => only one additional records can be assigned to the parent record.
    ///     
    /// (Another case the ChildTable, when the child has foreign key to parent: see SqlTableDescriptorHierarhyTableChild<T>)
    ///     
    /// 2 types of Leaf Additional Table implemented: 
    ///     -SqlTableDescriptorAdditionalObj:  There is container object for SQL record, object must implements the IAdditionalObj interface
    ///     -SqlTableDescriptorAdditional:     There is not container object, only 2 callback needed for SqlTableDescriptorAdditionalNoObj class
    ///     
    /// SQL Note: 
    /// -at additional table, if isUnique_in = true: use unique costraints for all columns except pk_id. (in order to do 2nd check by SQL server)
    /// -parent table: create fkey fkey_{unique table name} to uniq table pk_id
    /// 
    /// Release note: consistency check has not been implemented 
    /// </summary>
    abstract public class SqlTableDescriptorAdditional : SqlTableDescriptorBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName_in"></param>
        /// <param name="columnNames_in"></param>
        /// <param name="nullAccepted_in">The foreign key to the additional table can be null or not.</param>
        public SqlTableDescriptorAdditional(string tableName_in, IEnumerable<string> columnNames_in, bool nullAccepted_in)
            : base(tableName_in, columnNames_in)
        {
            NullAccepted = nullAccepted_in;
        }
                
        public bool NullAccepted { get; private set;}
    }

    abstract public class SqlTableDescriptorAdditionalLeaf : SqlTableDescriptorAdditional
    {
        [Flags]
        public enum ESQlAdditionalTableLeafOptions 
        { 
            /// <summary>
            /// Doesn't create new row if same row already exist at DB.
            /// </summary>
            Unique = 1, 
            /// <summary>
            /// The specified row must be exist in DB. (Unique falg explicitly defined in this case)
            /// </summary>
            AlreadyExist = 2,  
            
            /// <summary>
            /// same as ESqlConsistencyType.AllColumn, else ESqlConsistencyType.Normal (ESqlConsistencyType.FullCollection is not applicable for AdditionalTables )
            /// Only for SQL hi level. See comment of SqlTableDescriptorHierarchy.OnlyResult
            /// </summary>
            OnlyResult = 8,
            /// <summary>
            /// The foreign key to the additional table can be null or not.
            /// Save: GetData callback can returns with null, in this case the additional table will be not saved
            /// Load: ignored, if fkey is null 
            /// </summary>
            NullAccepted = 16,
        }
        public SqlTableDescriptorAdditionalLeaf(string tableName_in, IEnumerable<string> columnNames_in, ESQlAdditionalTableLeafOptions options_in)
            : base(tableName_in, columnNames_in, (options_in & ESQlAdditionalTableLeafOptions.NullAccepted) != 0)
        {
            Options = options_in;
        }

        public ESQlAdditionalTableLeafOptions Options { internal get; set; }

        public bool IsUnique 
        {
            get { return (Options & ESQlAdditionalTableLeafOptions.Unique) != 0; }
        }

        abstract internal Dictionary<string, object> GetData(SqlRowDescriptorAdditional wrapper_in);
        abstract internal void LoadInternal(SqlRowDescriptorAdditional forObj_in, Npgsql.NpgsqlDataReader sqlData_in);
        
    }
}
