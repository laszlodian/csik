using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using e77.MeasureBase.Sql;

namespace e77.MeasureBase.Model
{
    #region Exceptions
    public class SqlInconsistencyException : MeasureBaseException
    {
        public SqlInconsistencyException(string msg_in)
            : base(msg_in)
        { ; }

        public SqlInconsistencyException(string format_in, params object[] params_in) :
            this(string.Format(format_in, params_in))
        { ; }
    }

    public class SqlResultInconsistencyException : SqlInconsistencyException
    {
        public SqlResultInconsistencyException(string msg_in)
            : base(msg_in)
        { ;}

        public SqlResultInconsistencyException(string format_in, params object[] params_in)
            : base(format_in, params_in)
        { ;}
    } 

    public class SqlCheckableInconsistencyException : SqlResultInconsistencyException
    {
        public SqlCheckableInconsistencyException(ICheckableBase obj_in)
            : base(string.Format("CheckedState inconsistency between counted ({0}) and SQL stored ({1}) result. Obj: {2}",
                obj_in.CheckResult, obj_in.SqlStoredCheckResult, obj_in))
        {
            Object = obj_in;
        }

        ICheckableBase Object { get; set; }
    }
    #endregion

    public interface ISqlPostStoreDescriptor : ISqlHiLevel //todo_fgy arch: use Event (?) at SqlDescriptor 
    {
        void SqlPostStore(Npgsql.NpgsqlTransaction transaction_in);
    }

    public interface ISqlHiLevel : ISqlRowDescriptorHierarchy
    {
           /// <summary>
        /// suggested casts: 
        ///     -smallint by Convert.ToUInt16(...) or ToInt16()
        ///     -real by (float)
        ///     -Dictionary hierarhcy: use FlattenDictionaryHierarchy at SqlSave
        /// </summary>
        /// <param name="sqlData_in"></param>
        /// <param name="measure_in"></param>
        /// <param name="collectionKey_out"></param>
        void SqlLoad(NpgsqlDataReader sqlData_in, MeasureRoot measure_in, out string collectionKey_out);
    
        /// <summary>
        ///
        /// </summary>
        /// <param name="insertCommand_in"></param>
        /// <param name="measure_in"></param>
        /// <param name="collectionKey_in">of this object. (optionally used information)</param>
        /// <returns></returns>
        void SqlSave(NpgsqlCommand insertCommand_in, MeasureRoot measure_in, string collectionKey_in);
    }

    /// <summary>
    /// For Hi-level Sql support (ISqlHiLevel) derivered class from SqlTableDescriptorsBase should implement this interface.
    /// </summary>
    public interface ISqlGlobal
    {
        /// <summary>
        /// Main and childs tables (AdditionalRoot's not needed)
        /// Note:
        /// The TableDescrtiptor finder search this list forward. So If you want to sore descendant class into different table, 
        /// the descendant's table must be the first, else the base's table will be founded.
        /// </summary>
        SqlTableDescriptorHierarhyTableBase[] SqlAllTable { get; }

        /// <summary>
        /// If specified, parameter table will be automatically generated, and filled with these Configuration properties.
        /// In this case the FW will search MeasureConfig._appConfigs array sequencially for find the apropriate config obj. (MeasureConfig.FindConfigOfProperty)
        /// Use null for turn it off.
        /// 
        /// Ini File: columnName (IniConfigId)= {IniFileName}@{Category}@{ConfigName}
        ///  SQL Colum name: categoty@ConfigName 
        ///  In Order to use Ini files set this
        /// </summary>
        IEnumerable<string> SqlParameterColumns { get; }
    }

    /// <summary>
    /// For dynamic tables, where the class can be belonging to more tables.
    /// In this case the FW does not uses the default table descriptor searching, FW uses the table descriptor provided by this interface.
    /// SqlTableDescriptorsBase can implement this interface.
    /// </summary>
    public interface ISqlDynamicTableDescriptor : ISqlGlobal
    {
        /// <summary>
        /// Optionally overwrite default table descriptor search alg: look throught SqlHiLevel.AllTables
        /// </summary>
        /// <returns>null: use arig search alg.</returns>
        SqlTableDescriptorHierarhyTableBase GetDescriptorFor(object obj_in);
    }
    
    internal interface ISqlMainTable : ISqlHiLevel, ISqlRowDescriptorHierarchy
    {
        //float? RoomTemperature { get; }
    }
    
    /*dynamic parent tables
 public interface ISqlGrouppedChildDescriptor : ISqlDescriptor
 {
     List<IHierarchyDataItem> CreateParentTables(List<SqlTableDescriptorChild> parentTables);
 }*/


}
