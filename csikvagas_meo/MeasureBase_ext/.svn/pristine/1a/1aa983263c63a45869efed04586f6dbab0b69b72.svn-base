using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using e77.MeasureBase.Data;
using e77.MeasureBase.MeasureEnvironment;
using e77.MeasureBase.MeasureEnvironment.IpThermo;
using e77.MeasureBase.Sql;
using Npgsql;

namespace e77.MeasureBase.Model
{
    public static class SqlHiLevel
    {
        static public string DBConnectionStr { get; set; }

        public const string COLLECTION_KEY_PREFIX_FOR_ONLY_RESULT_ROWS = "sqlstored_";
        public const string COLUMN_NAME_PREFIX_RESULT = "res_";

        /// <summary>
        /// static member for StoreAll: set to true if measure hasn't SN
        /// </summary>
        /// DenySnExsitanceCheck DenySnNotNullCheck
        static public bool DenySnNotNullCheck { get; set; }

        /// <summary>
        /// static member for Load: valid only if callstack contains LoadAll
        /// </summary>
        internal static bool Load_nContinue { get; private set; }

        /// <summary>
        /// static member for Load: valid only if callstack contains LoadAll
        /// Key1: table name
        /// Key2: rowId
        /// Value ISqlDescriptorHierarchy (object at the memory database)
        /// </summary>
        static Dictionary<string, Dictionary<long, ISqlRowDescriptorHierarchy>> _idObjMap;

        /// <summary>
        /// Clears DB first.
        /// </summary>
        /// <param name="id_in"></param>
        /// <param name="load_nContinue_in"></param>
        static public void LoadAll(long id_in, bool load_nContinue_in)
        {
            SqlHiLevel.LoadAll(id_in, load_nContinue_in, true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id_in"></param>
        /// <param name="load_nContinue_in">false: deny overwrite by load of Operation(User, Workplace, MeasureDate), RoomTemperature</param>
        static public void LoadAll(long id_in, bool load_nContinue_in, bool clearDb_in)
        {
            if (clearDb_in)
                MeasureCollectionBase.TheMeasures.Clear();

            _idObjMap = new Dictionary<string, Dictionary<long, ISqlRowDescriptorHierarchy>>();
            Load_nContinue = load_nContinue_in;

            RecordsNumberReset();

            if (MeasureCollectionBase.TheMeasures == null)
                MainTable.CreateStorableClass();

            MeasureCollectionBase measureCollection = MeasureCollectionBase.TheMeasures;
            measureCollection.IsSqlLoaded = true;
            measureCollection.OnSqlPreLoad();

            if (measureCollection.Sql == null)
                measureCollection.Sql = new SqlRowDescriptorHierarchy(measureCollection);

            using (NpgsqlConnection sqlConn = new NpgsqlConnection(DBConnectionStr))
            {
                sqlConn.Open();

                try
                {
                    measureCollection.Sql.RowId = id_in;

                    QueryOneItem(string.Format("select * from {0} where {1}={2}",
                        measureCollection.Sql.TableDescriptor.TableName, SqlLowLevel.COLUMN_NAME_ID, id_in),
                        null, measureCollection, sqlConn,
                        delegate(NpgsqlDataReader dr)
                        {
                            Operation.TheOperation.Sql.RowId = (long)Convert.ToUInt64(dr[Operation.TheOperation.Sql.TableDescriptor.FKeyToThis]);

                            if (load_nContinue_in)
                            {
                                //optional ip-thermo:
                                if (dr.HasOrdinal("room_temperature")   //has column
                                    && !dr.IsDBNull(dr.GetOrdinal("room_temperature"))) //has value
                                    measureCollection.RoomTemperature = (float)dr["room_temperature"];
                            }

                            SwVersion.TheSwVersion.SqlVersionSqlStored = Convert.ToUInt16(dr["sql_version"]);
                        });

                    measureCollection.OnSqlMainRowLoaded(sqlConn);

                    MeasureRoot mainDatabaseContainer = measureCollection;
                    if (measureCollection.Measures.Count == 1 && !(measureCollection.Measures is ISqlRowDescriptorHierarchy))
                        mainDatabaseContainer = measureCollection.TheMeasure;//only one non-Sql stored measure -> use it

                    //load additional tables of main table
                    LoadAdditionalTables(mainDatabaseContainer, sqlConn, measureCollection);

                    LoadAllChildren(mainDatabaseContainer, sqlConn, (ISqlHiLevel)measureCollection);

                    if (load_nContinue_in)
                        SqlMiddleLevel.Load(sqlConn, Operation.TheOperation);
                }
                finally
                {
                    sqlConn.Close();
                    _idObjMap = null;
                }
            }

            measureCollection.OnSqlLoaded();

            RecordsNumberCheck(false);
        }

        public delegate void DoCallback(NpgsqlDataReader dr_in);

        private static void QueryOneItem(string sqlQueryStr_in, MeasureRoot measureRoot_in, ISqlHiLevel objToLoad_in, NpgsqlConnection sqlConn, DoCallback callback_in)
        {
            using (NpgsqlCommand query = new NpgsqlCommand(sqlQueryStr_in, sqlConn))
            {
                using (NpgsqlDataReader dr = query.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        if (callback_in != null)
                            callback_in(dr);

                        LoadAndStore(measureRoot_in, (ISqlHiLevel)objToLoad_in, dr);

                        //fill RowId`s of additional tables
                        FillRowIdAdditionalTables(objToLoad_in, dr);

                        dr.Close();
                    }
                    else
                    {
                        dr.Close();
                        throw new SqlNoValueException(sqlQueryStr_in);
                    }
                }
            }
        }

        private static void FillRowIdAdditionalTables(ISqlHiLevel objToLoad_in, NpgsqlDataReader dr)
        {
            foreach (SqlRowDescriptorAdditional additional in objToLoad_in.Sql.AdditoinalRecords)
            {
                if (!dr.IsDBNull(dr.GetOrdinal(additional.TableDescriptor.FKeyToThis)))
                    additional.RowId = (long)Convert.ToInt64(dr[additional.TableDescriptor.FKeyToThis]);
                else
                    additional.RowId = SqlLowLevel.INVALID_ROW_ID;
            }
        }

        /// <summary>
        /// Note: reason of separation LoadAndStore and LoadAllChildren: only one Query can be active at same time for a Sql connection
        /// </summary>
        /// <param name="measure_in"></param>
        /// <param name="objToLoad"></param>
        /// <param name="dr_in"></param>
        /// <param name="parentId_in"></param>
        private static void LoadAndStore(MeasureRoot measure_in, ISqlHiLevel objToLoad_in, NpgsqlDataReader dr_in)
        {
            if (objToLoad_in.Sql.TableDescriptor is SqlTableDescriptorHierarhyTableChild_Internal)
                ((SqlTableDescriptorHierarhyTableChild_Internal)objToLoad_in.Sql.TableDescriptor).RecordsNumber++;

            objToLoad_in.Sql.RowId = (long)(Convert.ToUInt64(dr_in[SqlLowLevel.COLUMN_NAME_ID]));

            if (!_idObjMap.ContainsKey(objToLoad_in.Sql.TableDescriptor.TableName))
                _idObjMap.Add(objToLoad_in.Sql.TableDescriptor.TableName, new Dictionary<long, ISqlRowDescriptorHierarchy>());

            _idObjMap[objToLoad_in.Sql.TableDescriptor.TableName].Add(objToLoad_in.Sql.RowId, objToLoad_in);

            if (objToLoad_in.Sql.TableDescriptor is SqlTableDescriptorHierarhyTableChild_Internal)
            {
                SqlTableDescriptorHierarhyTableChild_Internal tableDesc =
                    (SqlTableDescriptorHierarhyTableChild_Internal)objToLoad_in.Sql.TableDescriptor;

                objToLoad_in.Sql.ParentId = (long)(Convert.ToUInt64(dr_in[tableDesc.ColumnName_ForeignKeyToParent]));

                objToLoad_in.Sql.ParentObj = _idObjMap[tableDesc.Parent.TableName][objToLoad_in.Sql.ParentId];
            }

            if (objToLoad_in is ICheckableThreeState)
            {
                if (dr_in["ok"] == DBNull.Value)
                    ((ICheckableThreeState)objToLoad_in).SqlStoredCheckResult = null;
                else
                    ((ICheckableThreeState)objToLoad_in).SqlStoredCheckResult = (bool)dr_in["ok"];
            }
            else if (objToLoad_in is ICheckable)
            {
                ICheckable checkableObj = (ICheckable)objToLoad_in;
                checkableObj.SqlStoredCheckResult = (bool)dr_in["ok"];
            }

            if (((SqlTableDescriptorHierarhyTableBase)objToLoad_in.Sql.TableDescriptor).ConsistencyFullCollection == false)
                objToLoad_in.Sql.SqlLoadedResults = new Dictionary<string, object>();

            string collectionKey;
            objToLoad_in.SqlLoad(dr_in, null, out collectionKey);

            //SQL consistency: load sql stored result
            if (((SqlTableDescriptorHierarhyTableBase)objToLoad_in.Sql.TableDescriptor).ConsistencyFullCollection)
            { //OnlyResult == true -> append '_sql' postfix for collection key:
                collectionKey = string.Format("{0}{1}", COLLECTION_KEY_PREFIX_FOR_ONLY_RESULT_ROWS, collectionKey);
            }
            else
            { //OnlyResult == false -> all column beginning with "res_" added to SqlLoadedResults dictionary
                for (int i = 0; i < dr_in.FieldCount; i++)
                    if (!objToLoad_in.Sql.SqlLoadedResults.ContainsKey(dr_in.GetName(i)) //not added already: in this way the user can override the default load (e.g. Load FlashBuffer manually
                        && ((((SqlTableDescriptorHierarhyTableBase)objToLoad_in.Sql.TableDescriptor).ConsistencyForAllColumn
                                && (dr_in.GetName(i) != "ok" && dr_in.GetName(i) != "fk_parent_id" && dr_in.GetName(i) != "pk_id")) //except FW columna
                            || dr_in.GetName(i).StartsWith(SqlHiLevel.COLUMN_NAME_PREFIX_RESULT)))
                    {
                        objToLoad_in.Sql.SqlLoadedResults.Add(dr_in.GetName(i), dr_in[i]);
                    }
            }

            if (objToLoad_in is MeasureCollectionBase)
            { ;} //do not store it
            else if (objToLoad_in is MeasureBaseClass)
            {
                MeasureCollectionBase.TheMeasures.Measures.Add((MeasureBaseClass)objToLoad_in);
            }
            else
            {
                /* todo_fgy needed for ony one not storable measure?
                //if measure is not ISqlLoadable, it is null here. obtain first here
                if (measure_in == null)
                {
                    measure_in = MeasureCollectionBase.TheMeasures.Measures[0];
                    //assert this is not SQL loadable
                    if (measure_in is ISqlHierarchyLoadable)
                        throw new MeasureBaseInternalException("Measure[0] should be created by it should be created by MeasureCollectionBase.CreateStorableClassFor().");
                }*/

                measure_in.Database_Add(collectionKey, objToLoad_in);
            }
        }

        private static void LoadAllChildren(MeasureRoot measure_in, NpgsqlConnection sqlConn_in, ISqlHiLevel objToLoad_in)
        {
            if (objToLoad_in.Sql.TableDescriptor is SqlTableDescriptorHierarhyTableBase == false)
                throw new ArgumentException("this function can load the SQl hierarchy only (now...). Use SqlHiLevel.Load for sepatate tables.");

            SqlTableDescriptorHierarhyTableBase tableDescriptor =
                (SqlTableDescriptorHierarhyTableBase)objToLoad_in.Sql.TableDescriptor;

            foreach (SqlTableDescriptorHierarhyTableChild_Internal childDescriptor in tableDescriptor.GetChildren())
            {
                List<ISqlHiLevel> childrens = new List<ISqlHiLevel>();
                using (NpgsqlCommand query = new NpgsqlCommand(
                    string.Format("select * from {0} where {1}={2}",
                    childDescriptor.TableName, childDescriptor.ColumnName_ForeignKeyToParent,
                    objToLoad_in.Sql.RowId), sqlConn_in))
                {
                    using (NpgsqlDataReader dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                ISqlHiLevel obj = childDescriptor.CreateStorableClass(dr);

                                LoadAndStore(measure_in, obj, dr);
                                childrens.Add(obj);

                                FillRowIdAdditionalTables(obj, dr);
                            }
                        }
                        else if (childDescriptor.MustBeExist &&
                            childDescriptor.ConsistencyFullCollection == false)
                        {
                            dr.Close();

                            throw new SqlNoValueException(
                                string.Format("table: '{0}', {1} = {2}",
                                childDescriptor.TableName, childDescriptor.ColumnName_ForeignKeyToParent, objToLoad_in.Sql.RowId));
                        }

                        dr.Close();
                    }
                }

                //load childrens
                foreach (ISqlHiLevel child in childrens)
                {
                    //load additional tables
                    LoadAdditionalTables(measure_in, sqlConn_in, child);

                    LoadAllChildren((child is MeasureBaseClass ? (MeasureBaseClass)child : measure_in), sqlConn_in, child);
                }
            }
        }

        private static void LoadAdditionalTables(MeasureRoot measure_in, NpgsqlConnection sqlConn_in, ISqlHiLevel obj_in)
        {
            foreach (SqlRowDescriptorAdditional additional in obj_in.Sql.AdditoinalRecords)
            {
                if (additional is SqlRowDescriptorAdditionalLeaf)
                {
                    ((SqlRowDescriptorAdditionalLeaf)additional).SqlLoadInternal(sqlConn_in); //does store for consistency too
                    measure_in.LoadedAdditionalLeafTables.Add((SqlRowDescriptorAdditionalLeaf)additional);
                }
                else
                {
                    SqlTableDescriptorAdditionalHierarchy tableDesc = additional.TableDescriptor as SqlTableDescriptorAdditionalHierarchy;

                    ISqlHiLevel obj = tableDesc.AssociatedRootTable.CreateStorableClass();

                    if (additional.RowId != SqlLowLevel.INVALID_ROW_ID)
                    {
                        QueryOneItem(string.Format("select * from {0} where {1}={2}",
                            additional.TableDescriptor.TableName, SqlLowLevel.COLUMN_NAME_ID, additional.RowId),
                            measure_in, obj, sqlConn_in, null/*delegate(NpgsqlDataReader dr) { ;}*/);

                        LoadAllChildren(measure_in, sqlConn_in, obj);
                    }
                    else if (additional.TableDescriptor.NullAccepted == false)
                        throw new NotFoundException(string.Format("Item of the Additional Table {0} must be valid (NullAccepted == false), the parent's ({1})foreign key is empty",
                            additional, obj_in));
                }
            }
        }

        /*
        private static MeasureBaseInternal GetContainerMeasure(ISqlHierarchyLoadable obj_in)
        {
            if(MeasureCollectionBase.TheMeasures.Measures.Count == 1) //only one measure
            {
                return MeasureCollectionBase.TheMeasures.TheMeasure; //put anything into it
            }

            SqlStaticDescriptorBase pointer = obj_in.Sql.StaticDescriptor;
            do
            {
                if (MeasureCollectionBase.TheMeasures.Sql.StaticDescriptor == pointer)
                    return MeasureCollectionBase.TheMeasures;
                else if(MeasureCollectionBase.TheMeasures.Measures[0] is ISqlDescriptor
                    && (MeasureCollectionBase.TheMeasures.Measures[0] as ISqlDescriptor).Sql.StaticDescriptor == pointer)
                    return ((me)obj_in .n ;
            }
            while( (obj_in.Sql.StaticDescriptor is SqlStaticDescriptorHierarhyTableChild)
                && (obj_in.Sql.StaticDescriptor as SqlStaticDescriptorHierarhyTableChild).Parent
            /*
            if (obj_in != null && obj_in is MeasureBaseClass && obj_in is ISqlDescriptor)
                return obj_in;
            else
                return obj_in.MyMeasures;* /
        }*/

        /// <summary>
        /// for Rollback support, static member for Save: valid only if callstack contains StoreAll
        /// todo_fgy ! add global environment tables to _savedDescriptors too
        /// </summary>
        static List<SqlRowDescriptor> _savedDescriptors;

        static string _operationDescription;

        /// <summary>
        /// - Operation.TheOperation.Sn + Param: If sn or param needed at operation table, set it before calling this func. 
        /// (Else set null or string.Empty.)
        /// - Operation.TheOperation.Date will be owerriden by MeasureDate;
        /// </summary>
        /// <param name="operationDescription_in"></param>
        static public long StoreAll(string operationDescription_in, UInt16 sqlVersion_in)
        {
            long mainTableID = SqlLowLevel.INVALID_ROW_ID;

            SwVersion.TheSwVersion.SqlVersionCurrent = sqlVersion_in;
            _operationDescription = operationDescription_in;
            MeasureCollectionBase measuresCollection = MeasureCollectionBase.TheMeasures;

            if (!DenySnNotNullCheck &&
                (measuresCollection.SN == null || measuresCollection.SN == string.Empty))
                throw new Exception("Please set SN before SQL save or set DenySnNotNullCheck to true.");

            _savedDescriptors = new List<SqlRowDescriptor>();
            RecordsNumberReset();

            using (NpgsqlConnection sqlConn = new NpgsqlConnection(DBConnectionStr))
            {
                sqlConn.Open();
                using (NpgsqlTransaction transaction = sqlConn.BeginTransaction())
                {
                    try
                    {
                        mainTableID = StoreDatabasesInternal(transaction, measuresCollection);

                        if (!SqlTableDescriptorsBase.MainTable.SqlLogDenied)
                        {
                            //Log record:
                            string logString;
                            if (measuresCollection is ICheckableBase)
                                logString = string.Format("A mérés eredménye ({0}) letárolva",
                                    (measuresCollection as ICheckableBase).CheckResult.LocalizedStr());
                            else
                                logString = "A mérés eredménye letárolva";
                            SqlLog.TheLog.SaveLog(transaction.Connection, logString, SqlHiLevel.MainTable.TableName,
                                measuresCollection.Sql.RowId, measuresCollection.SN, false);
                            _savedDescriptors.AddRange(SqlLog.TheLog.LastSavedAdditionalDescriptors);
                        }

                        RecordsNumberCheck(true); //can send WrongSqlRecordNumberException
                        transaction.Commit();
                    }
                    catch
                    {
                        //Rollback modified descriptors:
                        foreach (SqlRowDescriptor obj in _savedDescriptors)
                            obj.RowId = SqlLowLevel.INVALID_ROW_ID;

                        transaction.Rollback();
                        sqlConn.Close();

                        throw;
                    }
                } //using transaction
                sqlConn.Close();
            }//using (NpgsqlConnection sqlConn = new NpgsqlConnection(DBConnectionStr))

            measuresCollection.OnSqlStored();
            return mainTableID;
        }//StoreAll()

        public static long StoreASqlStorable(NpgsqlTransaction transaction_in,
            MeasureRoot measure_in, string collectionKey_in,
            ISqlHiLevel obj_in, long parentId_in, Dictionary<SqlTableDescriptorBase, long> sqlHierarchy_inout)
        {
            if (obj_in.Sql.RowId != SqlLowLevel.INVALID_ROW_ID)
            {
                Trace.TraceWarning("Ignore SQL store - item: '{0}'; SQL desctiptor of Item: {1}", obj_in, obj_in.Sql);
                return obj_in.Sql.RowId;
            }                              

            obj_in.Sql.ParentId = parentId_in;

            //CodeDuplication_SqlSave
            //create dynamic column names:
            List<string> dynamicColumns = new List<string>();
            if (obj_in is ICheckableBase)
                dynamicColumns.Add("ok");
            foreach (SqlRowDescriptorAdditional additional in obj_in.Sql.AdditoinalRecords)
            {
                additional.RowId = SqlLowLevel.INVALID_ROW_ID; //SqlAdditionalAutoInvalidate
                dynamicColumns.Add(additional.TableDescriptor.FKeyToThis);

                if (additional is SqlRowDescriptorAdditionalLeaf)
                {
                    bool wasRowId = additional.RowId != SqlLowLevel.INVALID_ROW_ID;
                    ((SqlRowDescriptorAdditionalLeaf)additional).SqlStoreInternal(transaction_in);
                    if (!wasRowId && additional.RowId != SqlLowLevel.INVALID_ROW_ID)//there was not RowId, but there is now a new
                        _savedDescriptors.Add(additional);
                }
                else
                {
                    ISqlRowDescriptorHierarchy obj = SqlTableDescriptorAdditionalHierarchy.AllAdditionalHierarchyObjectMap[(SqlTableDescriptorAdditionalHierarchy)(additional.TableDescriptor)];

                    if (obj != null)
                    {
                        additional.RowId = StoreASqlStorable(transaction_in, measure_in,
                            SqlTableDescriptorAdditionalHierarchy.AllAdditionalHierarchyObjectCollectionKeyMap[(SqlTableDescriptorAdditionalHierarchy)(additional.TableDescriptor)],
                            (ISqlHiLevel)((SqlRowDescriptorAdditionalHierarchy)additional).TheObject, SqlLowLevel.INVALID_ROW_ID/*not available yet*/, sqlHierarchy_inout);
                        _savedDescriptors.Add(additional);
                    }
                    else if (additional.TableDescriptor.NullAccepted == false)
                        throw new NotFoundException(string.Format("Item of the Additional Table {0} must be valid (NullAccepted == false), the parent's ({1}) foreign key is empty",
                                additional, obj_in));
                }
            }

            long? mainTableID = null;
            if (obj_in.Sql.TableDescriptor is SqlTableDescriptorMainTable_Internal)
            {
                //store operation needed preset pk_id of main table, in order to store operation first
                //obtain next main rowId, in order to add it to operations table
                using (NpgsqlCommand query = new NpgsqlCommand(
                                 string.Format("SELECT NEXTVAL('{0}')", obj_in.Sql.TableDescriptor.SquenceName), transaction_in.Connection))
                {
                    mainTableID = (long)query.ExecuteScalar();
                }

                if (!(obj_in.Sql.TableDescriptor as SqlTableDescriptorMainTable_Internal).GlobalOperationDenied)
                {
                    //store operation
                    Operation.TheOperation.OperationDescription = _operationDescription;
                    Operation.TheOperation.SqlTableName = obj_in.Sql.TableDescriptor.TableName;
                    Operation.TheOperation.SqlId = mainTableID.Value;
                    Operation.TheOperation.Date = MeasureCollectionBase.TheMeasures.MeasureDate;

                    if (MeasureCollectionBase.TheMeasures.Measures.Count <= 1) //add SN if possible
                        Operation.TheOperation.Sn = MeasureCollectionBase.TheMeasures.SN;

                    Operation.TheOperation.Save(transaction_in);//create and set RowId
                }
            }

            //Insert Main Row:
            string insertCommandString = SqlLowLevel.CreateInsertCommand(obj_in.Sql.TableDescriptor, dynamicColumns.ToArray(), mainTableID.HasValue);
            using (NpgsqlCommand cmdInsert = new NpgsqlCommand(insertCommandString, transaction_in.Connection))
            {
                cmdInsert.Transaction = transaction_in;

                //fill fkey to additional records:
                foreach (SqlRowDescriptorAdditional additional in obj_in.Sql.AdditoinalRecords)
                    cmdInsert.Parameters.AddWithValue("@" + additional.TableDescriptor.FKeyToThis, additional.RowId == SqlLowLevel.INVALID_ROW_ID ? (object)DBNull.Value : additional.RowId);

                if (obj_in.Sql.TableDescriptor is SqlTableDescriptorMainTable_Internal)
                {
                    SqlTableDescriptorMainTable_Internal desc = (obj_in.Sql.TableDescriptor as SqlTableDescriptorMainTable_Internal);
                    //there is no parent: main table: store default columns
                    cmdInsert.Parameters.AddWithValue("@pk_id", mainTableID.Value);
                    cmdInsert.Parameters.AddWithValue(string.Format("@{0}", Operation.TheOperation.Sql.TableDescriptor.FKeyToThis),
                        Operation.TheOperation.Sql.RowId);

                    cmdInsert.Parameters.AddWithValue("@sql_version", SwVersion.TheSwVersion.SqlVersionCurrent);

                    //optional IpThermo:
                    if (MeasureConfig.IpThermoConfig != SqlTableDescriptorEnvironmentId.EIpThermoConfig.no_ipthermo)
                    {
                        if (desc.IpThermoConfig == SqlTableDescriptorEnvironmentId.EIpThermoConfig.ipthermo_possible
                            && GUI.WorkplaceSetupForm.RoomId == 0)
                        {
                            cmdInsert.Parameters.AddWithValue("@room_temperature", null);
                        }
                        else
                            cmdInsert.Parameters.AddWithValue("@room_temperature", (obj_in as MeasureCollectionBase).RoomTemperature);
                    }
                }
                else if (obj_in.Sql.TableDescriptor is SqlTableDescriptorHierarhyTableChild_Internal)
                {
                    SqlTableDescriptorHierarhyTableChild_Internal desc = (SqlTableDescriptorHierarhyTableChild_Internal)obj_in.Sql.TableDescriptor;
                    //there is parent: add parent id
                    cmdInsert.Parameters.AddWithValue("@" + desc.ColumnName_ForeignKeyToParent,
                        obj_in.Sql.ParentId);

                    //count records number:
                    desc.RecordsNumber++;
                }
                else if (obj_in.Sql.TableDescriptor is SqlTableDescriptorAdditionalRoot_Internal)
                {
                    ;
                }
                else
                    throw new MeasureBaseInternalException("Invalid SqlTableDescriptor. Should be main or child.");

                if (obj_in is ICheckableBase)
                {
                    ICheckableBase checkableObj = (ICheckableBase)obj_in;
                    if (checkableObj.CheckResult.HasValue == false && ((obj_in is ICheckableThreeState) == false))
                        throw new InvalidOperationException(string.Format("Object {0} is ICheckable, but it is not checked before calling Sql StoreAll()",
                            checkableObj));

                    if ((obj_in is ICheckableThreeState) == false)
                        cmdInsert.Parameters.AddWithValue("@ok", ((ICheckable)obj_in).CheckResult.Value);
                    else
                        SqlLowLevel.AddWithValueNullable(cmdInsert.Parameters, "ok", ((ICheckableThreeState)obj_in).CheckResult);
                }

                obj_in.SqlSave(cmdInsert, measure_in, collectionKey_in);
                object rowId= cmdInsert.ExecuteScalar();
             
                obj_in.Sql.RowId = (long)Convert.ToInt64(rowId);
                _savedDescriptors.Add(obj_in.Sql);

                sqlHierarchy_inout[obj_in.Sql.TableDescriptor] = obj_in.Sql.RowId;
            }//using cmdInsert

            if (obj_in is ISqlPostStoreDescriptor)
                ((ISqlPostStoreDescriptor)obj_in).SqlPostStore(transaction_in);

            return obj_in.Sql.RowId;
        }

        /// <summary>
        /// stores all 1..3 Databases (by calling StoreADatabase)
        /// </summary>
        /// <param name="transaction_in"></param>
        /// <param name="measures_in"></param>
        static private long StoreDatabasesInternal(NpgsqlTransaction transaction_in, MeasureCollectionBase measures_in)
        {
            long mainTableID = SqlLowLevel.INVALID_ROW_ID;

            if (measures_in.Sql == null)
                measures_in.Sql = new SqlRowDescriptorHierarchy(measures_in);
            else if (measures_in.Sql != SqlRowDescriptorHierarchy.EMPTY)
                measures_in.Sql.RowId = SqlLowLevel.INVALID_ROW_ID; //force store

            //Key: static descriptor
            //Value: Sql ID of that row
            //! see release note:  RN_SqlHiLevel.SingleParent
            Dictionary<SqlTableDescriptorBase, long> sqlHierarchy = new Dictionary<SqlTableDescriptorBase, long>();

            if (IsStorable(measures_in))
            {
                mainTableID = StoreASqlStorable(transaction_in, null, null,
                    (ISqlHiLevel)measures_in, SqlLowLevel.INVALID_ROW_ID, sqlHierarchy);
                Trace.TraceInformation("SQL: main table rowId= {0}", ((ISqlRowDescriptorHierarchy)measures_in).Sql.RowId);
            }
            else
                throw new InvalidOperationException("a mérés nem letárolható");

            StoreADatabase(transaction_in, sqlHierarchy, measures_in);

            //long noneDynamicParentTable = mainTableID;//Table of measures or measure
            foreach (MeasureBaseClass meas in measures_in.Measures)
            {
                if (IsStorable(meas))
                {
                    StoreASqlStorable(transaction_in, meas, null,
                            (ISqlHiLevel)meas, mainTableID, sqlHierarchy);

                    Trace.TraceInformation("SQL: measure rowId= {0}", ((ISqlRowDescriptorHierarchy)meas).Sql.RowId);
                }

                StoreADatabase(transaction_in, sqlHierarchy, meas);
            }//foreach measures

            /*SqlAdditional:
            foreach (ISqlDescriptor additionalObj in measures_in.SqlAdditional)
            {
                if (IsStoreRequired(additionalObj))
                    StoreASqlStorable(transaction_in, null,
                        null, (ISqlHiLevel)additionalObj, mainTableID);
            }*/

            return mainTableID;
        }

        private static void StoreADatabase(NpgsqlTransaction transaction_in,
            Dictionary<SqlTableDescriptorBase, long> sqlHierarchy_inout, MeasureRoot meas)
        {
            //dynamic parent tables meas.PreStore();

            //dynamic table string lastKey = "valami ami nincs és nem is lesz";//for parent/children detection
            //long dynamicParent = noneDynamicParentTable;
            foreach (string key in meas.Database_Keys)
            {
                foreach (KeyValuePair<string, object> pair in meas.Database_GetChildrenWithPosition(key))
                    if (IsStoreRequired(pair.Value) &&
                        ((ISqlHiLevel)pair.Value).Sql.TableDescriptor is SqlTableDescriptorHierarhyTableChild_Internal) //else AdditionalRoot, which has been stored by parent
                    {
                        if (((ISqlHiLevel)pair.Value).Sql.TableDescriptor is SqlTableDescriptorHierarhyTableBase == false)
                            throw new ArgumentException("this function can store the SQl hierarchy only (now...). Use SqlHiLevel.Save for sepatate tables.");

                        SqlTableDescriptorHierarhyTableChild_Internal tableDescriptor =
                            (SqlTableDescriptorHierarhyTableChild_Internal)((ISqlHiLevel)pair.Value).Sql.TableDescriptor;

                        /*
                        if (!key.StartsWith(lastKey)) //new branch, new dynamic tables
                            dynamicParent = noneDynamicParentTable;
                        //else = is a child of dynamic table?, use last dynamicParent ID */

                        long parentId;
                        if (((ISqlHiLevel)pair.Value).Sql.ParentObj != null)
                            parentId = ((ISqlHiLevel)pair.Value).Sql.ParentObj.Sql.RowId;
                        else
                        {
                            //only one parent item:
                            List<object> possibleParentObjs = meas.Database_GetAllChildren(string.Empty).FindAll(item => (item is ISqlHiLevel)
                                //     && (item as ISqlHiLevel).Sql != null
                                && (item as ISqlHiLevel).Sql.TableDescriptor == tableDescriptor.Parent);

                            if (possibleParentObjs.Count > 1)
                                throw new InvalidOperationException(string.Format("There are more possible parent for item {0}. Please specify them exactly by ISqlDescriptorHierarchy.ParentObj property.",
                                    tableDescriptor));

                            parentId = sqlHierarchy_inout[tableDescriptor.Parent];
                        }

                        /* Trace.TraceInformation("get SQL parent ID for {0} => Table: {1}, ID: {2}",
                            ((ISqlStoreDescriptor)pair.Value).Sql.TableDescriptor.TableName,
                            ((ISqlStoreDescriptor)pair.Value).Sql.TableDescriptor.Parent.TableName,
                            parentId); */

                        StoreASqlStorable(transaction_in, meas, pair.Key,
                            (ISqlHiLevel)pair.Value, parentId/*dynamicParent*/, sqlHierarchy_inout);

                        //Trace.TraceInformation("created SQL ID => {0}", id);
                    }//if SQL strore
            }
        }

        static internal bool IsHiLevelSupportEnabled
        {
            get
            {
                return ((SqlTableDescriptorsBase.TheDescriptor is ISqlGlobal));
            }
        }

        static internal SqlTableDescriptorHierarhyTableBase[] AllTables
        {
            get
            {
                if (SqlTableDescriptorsBase.TheDescriptor is ISqlGlobal)
                    return (SqlTableDescriptorsBase.TheDescriptor as ISqlGlobal).SqlAllTable;
                else
                    throw new InvalidOperationException("The SqlTableDescriptorsBase.TheDescriptor must implement ISqlGlobal interface for Hi-Level SQL support");
            }
        }

        static public SqlTableDescriptorMainTable_Internal MainTable
        {
            get
            {
                if (SqlTableDescriptorsBase.TheDescriptor is ISqlGlobal)
                    return (SqlTableDescriptorMainTable_Internal)((SqlTableDescriptorsBase.TheDescriptor as ISqlGlobal).SqlAllTable.First(item => (item is SqlTableDescriptorMainTable_Internal)));
                else
                    throw new InvalidOperationException("The SqlTableDescriptorsBase.TheDescriptor must implement ISqlGlobal interface for Hi-Level SQL support");
            }
        }

        static public IEnumerable<string> SqlParameterColumns
        {
            get
            {
                if (SqlTableDescriptorsBase.TheDescriptor is ISqlGlobal)
                    return (SqlTableDescriptorsBase.TheDescriptor as ISqlGlobal).SqlParameterColumns;
                else
                    throw new InvalidOperationException("The SqlTableDescriptorsBase.TheDescriptor must implement ISqlGlobal interface for Hi-Level SQL support");
            }
        }

        static private void RecordsNumberReset()
        {
            foreach (SqlTableDescriptorBase table in AllTables)
                if (table is SqlTableDescriptorHierarhyTableChild_Internal)
                    ((SqlTableDescriptorHierarhyTableChild_Internal)table).RecordsNumber = 0;
        }

        static private void RecordsNumberCheck(bool saveNotLoad_in)
        {
            Trace.TraceInformation("RecordsNumberCheck");

            foreach (SqlTableDescriptorBase table in AllTables)
            {
                StringBuilder strMsg = new StringBuilder();
                strMsg.AppendFormat("Table {0} ", table.TableName);

                if (table is SqlTableDescriptorHierarhyTableChild_Internal)
                {
                    SqlTableDescriptorHierarhyTableChild_Internal typeTable = (SqlTableDescriptorHierarhyTableChild_Internal)table;
                    if (typeTable.AssertRecordsNumber.HasValue)
                    {
                        strMsg.AppendFormat(" assert records number: {0}", typeTable.AssertRecordsNumber.Value);
                        if (typeTable.AssertRecordsNumber.Value != typeTable.RecordsNumber)
                        {
                            throw new WrongSqlRecordNumberException(saveNotLoad_in, typeTable);
                        }
                    }
                    else
                    {
                        strMsg.AppendFormat(" no records number check. ({0} {1})",
                            saveNotLoad_in ? "Saved" : "Loaded", typeTable.RecordsNumber);
                    }
                }
                else
                    strMsg.Append(" no records number check, because it is not SqlTableDescriptorHierarhyTableChild.");

                Trace.TraceInformation(strMsg.ToString());
            }
        }

        public static SqlTableDescriptorHierarhyTableBase FindTableDescriptor(object obj_in)
        {
            SqlTableDescriptorHierarhyTableBase tableDesc = null;
            if (SqlTableDescriptorsBase.TheDescriptor is ISqlDynamicTableDescriptor)
            {
                tableDesc = (SqlTableDescriptorsBase.TheDescriptor as ISqlDynamicTableDescriptor).GetDescriptorFor(obj_in);
            }

            if (tableDesc == null)
                try
                {
                    tableDesc = SqlHiLevel.AllTables.First(
                        item => (item.StorableType.IsInstanceOfType(obj_in)));
                }
                catch (Exception) { ;}

            if (tableDesc == null)
                throw new InvalidOperationException(string.Format(
                    "Cannot found StaticSqlDescriptor storing {0} type.", obj_in.GetType()));

            return tableDesc;
        }

        static private bool IsStorable(object obj_in)
        {
            if (!(obj_in is ISqlHiLevel))
                return false;

            //auto set ISqlDescriptor.Sql:
            if (((ISqlHiLevel)obj_in).Sql == null)
                ((ISqlHiLevel)obj_in).Sql = new SqlRowDescriptorHierarchy(obj_in as ISqlHiLevel);

            return true;
        }

        static private bool IsStoreRequired(object obj_in)
        {
            return (IsStorable(obj_in)
                && ((ISqlRowDescriptorHierarchy)obj_in).Sql.RowId == SqlLowLevel.INVALID_ROW_ID); //and has not stored already
        }
    }
}