using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.Sql;
using Npgsql;

namespace e77.MeasureBase.Model
{
    public enum ESqlConsistencyType
    {
        /// <summary>
        /// Consistency check for columns beginning with "res_" 
        /// </summary>
        Normal, 

        /// <summary>
        /// Consistency check for All columns  (except "pk_id")
        /// </summary>
        AllColumns,

        /// <summary>
        /// Consistency check for All columns, and for all collection (rows at SQL and objects in memory)
        /// </summary>
        FullCollection,
    }

    /// <summary>
    /// Generall SQL table descriptor for Hi-Level functions: 
    ///     -Creates storable object support
    ///     -AssertRecord Number support
    ///     -Consistency check support
    ///     -Base hierarchy functions: GetChildrens
    /// </summary>
    public abstract class SqlTableDescriptorHierarhyTableBase : SqlTableDescriptorBase
    {
        internal SqlTableDescriptorHierarhyTableBase(string tableName_in, ESqlConsistencyType consistencyType_in,
            IEnumerable<SqlTableDescriptorAdditional> additionalTables_in)
            : this(tableName_in, consistencyType_in)
        {
            if(additionalTables_in == null)
                AdditionalTables = Enumerable.Empty<SqlTableDescriptorAdditional>();
            else
                AdditionalTables = additionalTables_in;
        }

        internal SqlTableDescriptorHierarhyTableBase(string tableName_in, IEnumerable<string> columnNames_in, ESqlConsistencyType consistencyType_in)
            : base(tableName_in, columnNames_in)
        {
            AdditionalTables = Enumerable.Empty<SqlTableDescriptorAdditional>();

            _consistencyCheckType = consistencyType_in;
        }

        internal SqlTableDescriptorHierarhyTableBase(string tableName_in, ESqlConsistencyType consistencyType_in)
            : base(tableName_in)
        {
            AdditionalTables = Enumerable.Empty<SqlTableDescriptorAdditional>();

            _consistencyCheckType = consistencyType_in;
        }

        internal abstract Type StorableType { get; }
        public abstract ISqlHiLevel CreateStorableClass();

        ESqlConsistencyType _consistencyCheckType;

        /// <summary>
        /// For SQL consistenct check. If it is 
        /// -true: all columns in this table is result.
        /// MeasureBase loads this records to new object, with '_sql' postfix at ColectionKey 
        /// (because we cannot sure that counted value is exist, so we check the 2 collection )
        ///  MeasureBase SqlHiLevel.LoadAll() function will append collectionKey by 
        ///     SqlHiLevel.COLLECTION_KEY_POSTFIX_FOR_ONLY_RESULT_ROWS ("_sqlstored")
        ///     
        /// Note: if OnlyResult == true (SqlHierarcyLoadable) should override object.Equals(object obj_in). You can use slow but safe ReflectionEqual extension method.
        /// </summary>
        public bool ConsistencyFullCollection { get { return (_consistencyCheckType & ESqlConsistencyType.FullCollection) != 0; } }
        
        public bool ConsistencyForAllColumn { get { return (_consistencyCheckType & ESqlConsistencyType.AllColumns) != 0; } }
                
        public IEnumerable<SqlTableDescriptorHierarhyTableChild_Internal>  GetChildren()
        {
            if (SqlTableDescriptorsBase.TheDescriptor is ISqlGlobal == false)
            {
                throw new InvalidOperationException( string.Format(
                    "For SqlHiLevel support derivered class of SqlTableDescriptorsBase should implement ISqlGlobal interface. ('{0}')",
                    this.GetType().ToString()));
            }

            return  (SqlTableDescriptorsBase.TheDescriptor as ISqlGlobal).SqlAllTable    //linq example ;)
                .Where((item, index) => ((item is SqlTableDescriptorHierarhyTableChild_Internal)
                    && ((SqlTableDescriptorHierarhyTableChild_Internal)item).Parent == this))
                .Cast<SqlTableDescriptorHierarhyTableChild_Internal>();
        }

        private int? _AssertRecordsNumber;

        /// <summary>
        /// SQL save sends WrongSqlRecordNumberException, if the saving records number does not equal with thwe specified AssertRecordsNumber.
        /// </summary>
        public int? AssertRecordsNumber
        {
            get { return _AssertRecordsNumber; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Use 'SqlTableDescriptorHierarhyTableBase.AssertRecordsNumber = null;' for deny this check.");
                _AssertRecordsNumber = value;
            }
        }

        /// <summary>
        /// Number of saved or loaded records of this table
        /// </summary>
        public int RecordsNumber { get; internal set; }
    }

    /// <summary>
    /// Child SQL table descriptor.
    /// Child records has foreign key to parent: (if parent table has a foreign key to child: see SqlTableDescriptorAdditional)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlTableDescriptorHierarhyTableChild<T> : SqlTableDescriptorHierarhyTableChild_Internal
        where T : ISqlHiLevel, new()
    {
        /// <summary>
        /// Constructor 1.
        /// </summary>
        /// <param name="parentTable_in">Szülő tábla</param>
        /// <param name="tableName_in">Tábla neve</param>
        /// <param name="consistencyType_in"></param>
        /// <param name="columnNames_in">Oszlop elnevezések (lista)</param>
        /// <param name="additionalTables_in">Additional tábla</param>
        public SqlTableDescriptorHierarhyTableChild(SqlTableDescriptorHierarhyTableBase parentTable_in,
            string tableName_in, ESqlConsistencyType consistencyType_in, IEnumerable<string> columnNames_in,
            IEnumerable<SqlTableDescriptorAdditional> additionalTables_in /*= true*/)
            : base(parentTable_in, tableName_in, consistencyType_in, columnNames_in, additionalTables_in, true)
        { ;}

        /// <summary>
        /// Constructor 2.
        /// </summary>
        /// <param name="parentTable_in">Szülő tábla</param>
        /// <param name="tableName_in">Tábla neve</param>
        /// <param name="consistencyType_in"></param>
        /// <param name="columnNames_in">Oszlop elnevezések (lista)</param>
        /// <param name="additionalTables_in">Additional tábla</param>
        /// <param name="mustBeExist_in">Szükséges?</param>
        public SqlTableDescriptorHierarhyTableChild(SqlTableDescriptorHierarhyTableBase parentTable_in,
            string tableName_in, ESqlConsistencyType consistencyType_in, IEnumerable<string> columnNames_in, 
            IEnumerable<SqlTableDescriptorAdditional> additionalTables_in, bool mustBeExist_in)
            : base(parentTable_in, tableName_in, consistencyType_in, columnNames_in, additionalTables_in, mustBeExist_in)
        { ;}

        /// <summary>
        /// Constructor 3.
        /// </summary>
        /// <param name="parentTable_in">Szülő tábla</param>
        /// <param name="tableName_in">Tábla neve</param>
        /// <param name="consistencyType_in"></param>
        /// <param name="columnNames_in">Oszlop elnevezések (lista)</param>
        /// <param name="additionalTables_in">Additional tábla</param>
        /// <param name="mustBeExist_in">Szükséges?</param>
        /// <param name="recordNumber_in">Rekordszám tároláshoz, visszatöltéshez
        ///    SqlHiLevel.StoreAll() and LoadAll will check the stored rows number, and send exception if not equal. Use the other constructor is this check is not needed.</param>
        public SqlTableDescriptorHierarhyTableChild(SqlTableDescriptorHierarhyTableBase parentTable_in,
            string tableName_in, ESqlConsistencyType consistencyType_in, IEnumerable<string> columnNames_in,
            IEnumerable<SqlTableDescriptorAdditional> additionalTables_in, bool mustBeExist_in, int recordNumber_in)
            : base(parentTable_in, tableName_in, consistencyType_in, columnNames_in, additionalTables_in, mustBeExist_in, recordNumber_in)
        { ;}

        internal override Type StorableType
        {
            get { return typeof(T); }
        }

        public delegate T CreateStorableClassDelegate(NpgsqlDataReader dr_in);
        public CreateStorableClassDelegate CreateStorableClassRuntime;

        override public ISqlHiLevel CreateStorableClass(NpgsqlDataReader dr_in) 
        {
            T obj;
            if (CreateStorableClassRuntime != null)
                obj = CreateStorableClassRuntime(dr_in);
            else            
                obj = new T();

            obj.Sql = new SqlRowDescriptorHierarchy(this, obj);
            return obj;
        }
    }

    public abstract class SqlTableDescriptorHierarhyTableChild_Internal : SqlTableDescriptorHierarhyTableBase
    {
        internal SqlTableDescriptorHierarhyTableChild_Internal(SqlTableDescriptorHierarhyTableBase parentTable_in,
            string tableName_in, ESqlConsistencyType consistencyType_in, IEnumerable<string> columnNames_in,
            IEnumerable<SqlTableDescriptorAdditional> additionalUnique_in, bool mustBeExist_in)
            : base(tableName_in, consistencyType_in, additionalUnique_in)
        {
            List<string> columns = new List<string>(columnNames_in);

            //columns.Add(SqlLowLevel.COLUMN_NAME_ID);
            columns.Add(ColumnName_ForeignKeyToParent);
            ColumnNames = columns;
            
            Parent = parentTable_in;

            MustBeExist = mustBeExist_in;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentTable_in"></param>
        /// <param name="tableName_in"></param>
        /// <param name="columnNames_in"></param>
        /// <param name="recordNumber_in">SqlHiLevel.StoreAll() and LoadAll will check the stored rows number, and send exception if not equal. Use the other constructor is this check is not needed.</param>
        /// <param name="mustBeExist_in">false: zero number of child is accepted, else SqlNoValueException</param>
        public SqlTableDescriptorHierarhyTableChild_Internal(SqlTableDescriptorHierarhyTableBase parentTable_in,
            string tableName_in, ESqlConsistencyType consistencyType_in, IEnumerable<string> columnNames_in,
            IEnumerable<SqlTableDescriptorAdditional> additionalTables_in, bool mustBeExist_in, int recordNumber_in)
            : this(parentTable_in, tableName_in, consistencyType_in, columnNames_in, additionalTables_in, mustBeExist_in)
        {
            AssertRecordsNumber = recordNumber_in;
        }

        public SqlTableDescriptorHierarhyTableBase Parent { get; private set; }
        public bool MustBeExist { get; set; }

        public string ColumnName_ForeignKeyToParent
        {
            get { return "fk_parent_id"; }
        }

        override public ISqlHiLevel CreateStorableClass()
        {
            throw new MeasureBaseInternalException("use 'CreateStorableClass(NpgsqlDataReader dr_in)'");
        }

        abstract public ISqlHiLevel CreateStorableClass(NpgsqlDataReader dr_in);

        public override string ToString()
        {
            return string.Format("{0}, AssertRecordsNumber: {1}, RecordsNumber: {2}",
                base.ToString(), AssertRecordsNumber, RecordsNumber);
        }
    }
    
    /// <summary>
    /// Main SQL table:
    /// -the row id of the record is the measure UID
    /// -FW Added Columns:
    ///     -fk_global_operations_id (Workplace, User, Sw and datetime info)        
    ///         can be denied by DenyGlobalOperation
    ///         
    ///     -'ok' column, when the StorableType class implements ICheckableBase. In this case the value of the 'ok' cell is the result of the measurement.
    ///     
    ///     -'room_temperature', if ipThermoConfig != no_ipthermo 
    ///     
    ///     -'sql_version': integer for identify the SQL version of measure, application set it when SqlHiLevel.StoreAll() has been called. It is not checked by FW.
    ///         can be denied by DenySqlVersion
    /// -Fw Add SqlLog in case of SaveAll. 
    ///         can be denied by DenySqlLog
    ///     
    /// Note: one measurement can save only one row into this table (AssertRecordsNumber = 1;)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlTableDescriptorMainTable<T> : SqlTableDescriptorMainTable_Internal
        where T : MeasureCollectionBase, ISqlHiLevel, new()
    {
        public SqlTableDescriptorMainTable(string tableName_in, IEnumerable<string> columnNames_in,
            IEnumerable<SqlTableDescriptorAdditional> additionalTables_in, SqlTableDescriptorEnvironmentId.EIpThermoConfig ipThermoConfig)
            : base(tableName_in, columnNames_in, additionalTables_in, ipThermoConfig)
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

    public abstract class SqlTableDescriptorMainTable_Internal : SqlTableDescriptorHierarhyTableBase
    {
        /// <summary>
        /// fk_global_operations_id     can be denied by DenyGlobalOperation
        /// sql_version                 can be denied by DenySqlVersion
        /// </summary>
        public enum EFixMainTableColumns { fk_global_operations_id, sql_version }; //fk_global_operations_id is not auto added, because ot is not direct additional table, because there is bidirectional foreign key between these tables.

        internal SqlTableDescriptorMainTable_Internal(string tableName_in, IEnumerable<string> columnNames_in,
            IEnumerable<SqlTableDescriptorAdditional> additionalTables_in, SqlTableDescriptorEnvironmentId.EIpThermoConfig ipThermoConfig)
            : base(tableName_in, ESqlConsistencyType.Normal, additionalTables_in)
        {
            _IpThermoConfig = ipThermoConfig;

            List<string> columns = new List<string>(columnNames_in);
            columns.AddRange(Enum.GetNames(typeof(EFixMainTableColumns)));
            if(_IpThermoConfig != SqlTableDescriptorEnvironmentId.EIpThermoConfig.no_ipthermo)
                columns.Add("room_temperature");

            ColumnNames = columns.ToArray();
                                    
            AssertRecordsNumber = 1;
        }

        SqlTableDescriptorEnvironmentId.EIpThermoConfig _IpThermoConfig;

        /// <summary>
        /// Context of this will be copied to "static MeasureConfig.IpThermoConfig" by SqlTableDescriptorsBase constructor.
        /// </summary>
        public SqlTableDescriptorEnvironmentId.EIpThermoConfig IpThermoConfig
        {
            get { return _IpThermoConfig; }
            protected set { _IpThermoConfig = value; }
        }

        internal bool GlobalOperationDenied { get; private set; }
        
        /// <summary>
        /// SqlHiLevel without Global Operation table
        /// </summary>
        public void DenyGlobalOperation()
        { 
            this.ColumnNames = ColumnNames.Where(item => item != EFixMainTableColumns.fk_global_operations_id.ToString());
            GlobalOperationDenied = true;
        }

        internal bool SqlVersionDenied { get; private set; }
        /// <summary>
        /// SqlHiLevel without Global Operation table
        /// </summary>
        public void DenySqlVersion()
        {
            this.ColumnNames = ColumnNames.Where(item => item != EFixMainTableColumns.sql_version.ToString());
            SqlVersionDenied = true;
        }

        internal bool SqlLogDenied { get; private set; }
        /// <summary>
        /// SqlHiLevel without Global Operation table
        /// </summary>
        public void DenyDenySqlLog()
        {
            SqlLogDenied = true;
        }
    }
}
