using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using e77.MeasureBase.Data;
using e77.MeasureBase.Model;
using e77.MeasureBase.Sql;
using Npgsql;

namespace e77.MeasureBase.Model
{
    /// <summary>
    /// the following interfaces can be handled at database items:
    /// -ISqlHiLevel                        Hi-level SQL Save/Load support: object will be stored at SQL DB
    /// -IMyMeasure                         FW sets IMyMeasure.MyMeasure
    /// -ICollectionKey                     objects implements ICollectionKey can be added to database directly
    /// -ICheckable, ICheckableThreeState   Measure(s) API: DoCheckAll() -> virtual bool OnCheckAll(); event: CheckAll
    /// -ICountable                         Measure(s) API: virtual OnCount(); delegate: bool Count();
    /// -IPreProcessable, IPostProcessable
    /// -IChildInfo                         The result of this interface will be appeares as childrens at the InfoForm
    /// </summary>
    public class MeasureRoot : INotifyCollectionChanged
    {
        internal MeasureRoot()
        {
            LoadedAdditionalLeafTables = new List<SqlRowDescriptorAdditionalLeaf>();
            _Database.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChangedHandler);
        }

        private void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs arg_in)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, arg_in);
        }

        public MeasureCollectionBase MyMeasures
        {
            get { return MeasureCollectionBase.TheMeasures; }
        }

        private HierarchyMultiValueDictionary<object> _Database =
            new HierarchyMultiValueDictionary<object>();

        private StringBuilder _AllInputData = new StringBuilder(2048); //for diagnostic purpose

        /// <summary>
        /// For store measure input data. It is added to "virtual string AllTraceInfo;"
        /// </summary>
        public StringBuilder AllInputData
        {
            get { return _AllInputData; }
        }

        #region Database API

        /// <summary>
        /// Returns all collection keys.
        /// </summary>
        public List<string> Database_Keys
        {
            get { return _Database.Keys; }
        }

        public bool Database_ContainsKey(string key_in)
        {
            return _Database.ContainsKey(key_in);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="collectionKey_in"></param>
        /// <returns>Gets items belonging to the collection key.</returns>
        public List<object> Database_GetItems(string collectionKey_in)
        {
            return _Database[collectionKey_in];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parentKey_in"></param>
        /// <returns>all collection key, which begins with parentKey_in</returns>
        public List<string> Database_GetAllChildrenCollectionKeys(string parentKey_in)
        {
            return _Database.GetAllChildrenId(parentKey_in);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parentKey_in"></param>
        /// <returns>ALL children with collection key, which collection key begins with parentKey_in.</returns>
        public List<KeyValuePair<string, object>>
            Database_GetAllChildrenWithCollectionKeys(string parentKey_in)
        {
            List<KeyValuePair<string, object>> res = new List<KeyValuePair<string, object>>();

            foreach (string key in _Database.Keys)
                if (key.StartsWith(parentKey_in))
                    foreach (object item in Database_GetItems(key))
                        res.Add(new KeyValuePair<string, object>(key, item));

            return res;
        }

        public List<T> Database_GetAllChildrenWithType<T>(string partialKey_in)
        {
            List<T> res = new List<T>();

            foreach (string key in _Database.GetAllChildrenId(partialKey_in))
                res.AddRange(Database_GetChildrenWithType<T>(key));
            return res;
        }

        public List<object> Database_GetAllChildren(string partialKey_in)
        {
            return _Database.GetAllChildren(partialKey_in);
        }

        public List<N> Database_GetAllChildrenCastTo<N>(string partialKey_in)
        {
            return _Database.GetAllChildren(partialKey_in).ConvertAll<N>(Conversations.SimpleConverter<N>);
        }

        public void Database_AddRange(string collectionKey_in, IEnumerable<object> objs_in)
        {
            foreach (object o in objs_in)
                Database_Add(collectionKey_in, o);
        }

        #region Customize Database Add checks

        /// <summary>
        /// See RN_SAME_TYPE_CONSISTENCY_CHECK: (consistency check does not support/working correctly for different type now...)
        /// </summary>
        public bool Database_Add_DenySameTypeCheck { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key_in"></param>
        /// <param name="overwrite_in">true: old value will be overwriten, else exception thrown. 
		/// Used only if return value is true.</param>
        /// <returns>true: this database key sould contains single element only</returns>
        protected virtual bool Database_AddIsSingle(string key_in, out bool overwrite_in)
        {
            overwrite_in = false;
            return false;
        }

        #endregion Customize Database Add checks

        public virtual void Database_Add(ICollectionKey obj_in)
        {
            Database_Add(obj_in.CollectionKey, obj_in);
        }


        /// <summary>
        /// This function supports all of the global_ tables handling 
        /// with SqlDescreptor and without too
        /// So you will get one more row (about of the current measure) in every global tables
        /// this is you looking for....
        /// </summary>
        /// <param name="collectionKey_in"></param>
        /// <param name="obj_in"></param>
        public virtual void Database_Add(string collectionKey_in, object obj_in)
        {
            if (obj_in == null)
                throw new ArgumentNullException("obj_in");

            // *** Initialize ***
            //set IMyMeasure.MyMeasure = this
            if (obj_in is IMyMeasure)
            {
                if (this is MeasureBaseClass)
                    MeasureCollectionBase.TheMeasures.Measures.Add((obj_in as IMyMeasure).MyMeasure = (this as MeasureBaseClass));
                else
                    throw new InvalidOperationException("IMyMeasure interface can contains MeasureBase, not MeasureCollectionBase. There is no critical reason for it, only I thought MeasureBase is better/more usable. If you use this class at database of MeassureCollection, you can reach the containing database class by 'MeassureCollectionBase.TheMeassures'.");
            }

            //Set ISqlDescriptor.Sql if needed
            if (SqlTableDescriptorsBase.IsTheDescriptorExist && //is SQL needed at all for this measure?
                obj_in is ISqlRowDescriptorHierarchy && (obj_in as ISqlRowDescriptorHierarchy).Sql == null)
            {
                if (SqlHiLevel.IsHiLevelSupportEnabled)
                {
                    (obj_in as ISqlRowDescriptorHierarchy).Sql = new SqlRowDescriptorHierarchy(obj_in as ISqlRowDescriptorHierarchy);

                    //check additional hierarcy:
                    SqlTableDescriptorAdditionalRoot_Internal associatedRootTable = (obj_in as ISqlRowDescriptorHierarchy).Sql.TableDescriptor as SqlTableDescriptorAdditionalRoot_Internal;
                    if (associatedRootTable != null)
                    {
                        SqlTableDescriptorAdditionalHierarchy desc = SqlTableDescriptorAdditionalHierarchy.AllAdditionalRoots[associatedRootTable];

                        SqlTableDescriptorAdditionalHierarchy.AllAdditionalHierarchyObjectMap[desc] = obj_in as ISqlRowDescriptorHierarchy;
                        SqlTableDescriptorAdditionalHierarchy.AllAdditionalHierarchyObjectCollectionKeyMap[desc] = collectionKey_in;
                    }
                }
                else
                {
                    throw new InvalidOperationException("Only HiLevel SQL support can set ISqlDescriptor.Sql automatically. You have to set it manually if you don't use HiLevel SQL support.");
                }
            }

            // *** Check ***
            bool overwrite = false;
            if (_Database.ContainsKey(collectionKey_in == string.Empty ? collectionKey_in=Convert.ToString((ICollectionKey)obj_in.GetType() ): collectionKey_in))
            {
                //type check:
                if (!Database_Add_DenySameTypeCheck
                    && _Database[collectionKey_in][0].GetType() != obj_in.GetType())
                {
                    throw new ArgumentException(string.Format(
                        "Type mismatch at collectionKey_in: '{0}'. Currently adding type: {1}, containing: {2}. (use Database_Add_DenySameTypeCheck=true; for ignore this check",
                        collectionKey_in, obj_in.GetType(), _Database[collectionKey_in][0].GetType()));
                }

                if (Database_AddIsSingle(collectionKey_in, out overwrite))
                    if (overwrite)
                        _Database[collectionKey_in].Clear();
                    else
                        throw new InvalidOperationException(string.Format(
                            "Cannot add item '{0}' into collection key: '{1}', because this Single collection already contains item: '{2}'",
                                obj_in, collectionKey_in, _Database[collectionKey_in][0]));
            }

            //check implementing IComparable in case of FullRow SQL check
            if (SqlTableDescriptorsBase.IsTheDescriptorExist //is SQL needed at all for this measure?
                && obj_in is ISqlHiLevel
                && ((obj_in as ISqlHiLevel).Sql.TableDescriptor is SqlTableDescriptorHierarhyTableBase)
                && ((obj_in as ISqlHiLevel).Sql.TableDescriptor as SqlTableDescriptorHierarhyTableBase).ConsistencyFullCollection
                && !(obj_in is IComparable))
            {
                throw new InvalidOperationException(string.Format(
                    "Object ({0}) for Sql full row ({1}) consistency check have to implement IComparable interface. \n -1 +1 is not essential, we use it only dor difference detection (object.Equal is not good, because base implemetation returns ReferenceEqual)",
                    obj_in, (obj_in as ISqlHiLevel).Sql.TableDescriptor.TableName));
            }

            _Database.Add(collectionKey_in, obj_in);
			
        }

        /// <summary>
        /// returns all T type item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key_in"></param>
        /// <returns></returns>
        public List<T> Database_GetChildrenWithType<T>(string key_in)
        {
            List<T> res = new List<T>();
            foreach (object obj in Database_GetItems(key_in))
                if (obj is T)
                    res.Add((T)obj);
            return res;
        }

        /// <summary>
        /// cast all items to T type, throws exception if not castable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key_in"></param>
        /// <returns></returns>
        public List<T> Database_GetChildrenCastTo<T>(string key_in)
        {
            List<T> res = _Database[key_in].ConvertAll<T>
                (Conversations.SimpleConverter<T>);
            return res;
        }

        public List<KeyValuePair<string, object>>
            Database_GetChildrenWithPosition(string key_in)
        {
            List<KeyValuePair<string, object>> res = new List<KeyValuePair<string, object>>();

            foreach (object item in _Database[key_in])
                res.Add(new KeyValuePair<string, object>(key_in, item));

            return res;
        }

        /// <summary>
        /// Clears all measure specific data
        /// </summary>
        virtual public void Clear()
        {
            _Database.Clear();
            _AllInputData = new StringBuilder(2048);

            SN = string.Empty;        // Gyári szám (egyedi E77 belső azonosító)

            if (this is ICheckableBase)
            {
                (this as ICheckableBase).CheckResult = null;
                (this as ICheckableBase).SqlStoredCheckResult = null;
            }

            //clear Sql.RowId
            if (this is ISqlRowDescriptorHierarchy && (this as ISqlRowDescriptorHierarchy).Sql != null)
                (this as ISqlRowDescriptorHierarchy).Sql.RowId = SqlLowLevel.INVALID_ROW_ID;
            if (this is ISqlRowDescriptor && (this as ISqlRowDescriptor).Sql != null)
                (this as ISqlRowDescriptor).Sql.RowId = SqlLowLevel.INVALID_ROW_ID;

            LoadedAdditionalLeafTables.Clear();
        }

        /// <summary>
        /// removes items with key_in
        /// </summary>
        /// <param name="partialKey_in"></param>
        public void Database_RemoveItems(string key_in)
        {
            _Database.RemoveItems(key_in);
        }

        /// <summary>
        /// removes an item with key_in
        /// </summary>
        /// <param name="partialKey_in"></param>
        public void Database_RemoveItem(string key_in, object item_in)
        {
            _Database.RemoveItem(key_in, item_in);
        }

        public void Database_RemoveItem(ICollectionKey item_in)
        {
            _Database.RemoveItem(item_in.CollectionKey, item_in);
        }

        /// <summary>
        /// removes all items beginning with partialKey_in
        /// </summary>
        /// <param name="partialKey_in"></param>
        public void Database_RemoveAllItems(string partialKey_in)
        {
            _Database.RemoveAllItems(partialKey_in);
        }

        public event EventHandler Count;

        /// <summary>
        /// Virtual function, base implementation calls this.Count event, and Count() function of all ICountable object at Database
        /// </summary>
        public virtual void OnCount()
        {
            if (Count != null && Count.GetInvocationList().Length > 0)
                Count.Invoke(this, EventArgs.Empty);

            foreach (object obj in _Database.Values)
            {
                if (obj is ICountable)
                    ((ICountable)obj).Count();
            }
        }

        #endregion Database API

        #region ICheckable and consistency check

        /// <summary>
        /// Virtual function, base implementation calls OnCheck() function
        /// of all ICheckable object at Database,
        /// and checks SQL consistency
        /// Sets this.FailedSteps property
        /// </summary>
        /// <returns>OK</returns>
        public virtual bool? DoCheckAll()
        {
            if (this is ICheckableBase == false)
            {
                throw new InvalidOperationException("Calling the CheckAll() nededs implement ICheckable interface. If you dont set SQL table for this class (does not implement ISqlHiLevel) you have to set ICheckable.SqlSoredResult manually before calling this function.");
            }

            return OnCheck();
        }

        public virtual void CheckSqlConsistency()
        {
            if (!MeasureCollectionBase.TheMeasures.IsSqlLoaded)
                throw new InvalidOperationException("This measure has not been SQL loaded at this time");

            //check measure ownself
            if (this is ISqlHiLevel)
                if (((this as ISqlHiLevel).Sql.TableDescriptor as SqlTableDescriptorHierarhyTableBase).ConsistencyFullCollection)
                {
                    throw new NotSupportedException();
                }
                else /*can contains result:*/
                {
                    SqlConsistencyCheckPartial(string.Empty, new List<object>(new object[] { this }));
                }

            //check database items
            foreach (string collectionKey in _Database.Keys)
            {
                Trace.TraceInformation("MeasureRoot.CheckSqlConsistency() for key '{0}'", collectionKey);
                List<object> items = _Database[collectionKey];
                //RN_SAME_TYPE_CONSISTENCY_CHECK:
                if (items[0] is ISqlHiLevel &&
                    ((items[0] as ISqlHiLevel).Sql.TableDescriptor is SqlTableDescriptorHierarhyTableBase))
                {
                    if (((items[0] as ISqlHiLevel).Sql.TableDescriptor as SqlTableDescriptorHierarhyTableBase).ConsistencyFullCollection)
                    {
                        SqlConsistencyCheckFull(collectionKey, items);
                    }
                    else /*can contains result:*/
                    {
                        SqlConsistencyCheckPartial(collectionKey, items);
                    }
                }

                foreach (object obj in items)
                    if (obj is ICheckableBase)
                        SqlConsistencyCheckResult(obj as ICheckableBase);
            }//for all Datables.Keys

            foreach (SqlRowDescriptorAdditionalLeaf additionalLeaf in LoadedAdditionalLeafTables)
            {
                if (additionalLeaf.RowId != SqlLowLevel.INVALID_ROW_ID)
                    additionalLeaf.CheckConsistency();
                else //fk to additional is null => no consistency check needed
                    Debug.Assert((additionalLeaf.TableDescriptor.Options & SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.NullAccepted) != 0);
            }

            if (this is ICheckableBase)
                SqlConsistencyCheckResult(this as ICheckableBase);
        }

        private void SqlConsistencyCheckFull(string collectionKey_in, List<object> items_in)
        {
            if (collectionKey_in.StartsWith(SqlHiLevel.COLLECTION_KEY_PREFIX_FOR_ONLY_RESULT_ROWS))
            {
                string realtimeKey = collectionKey_in.Substring(SqlHiLevel.COLLECTION_KEY_PREFIX_FOR_ONLY_RESULT_ROWS.Length,
                    collectionKey_in.Length - SqlHiLevel.COLLECTION_KEY_PREFIX_FOR_ONLY_RESULT_ROWS.Length);
                if (!_Database.Keys.Contains(realtimeKey))
                {
                    throw new SqlResultInconsistencyException(string.Format("There is no counted data for {0}, but there are {1} items stored in SQL: {2}",
                        realtimeKey, _Database[collectionKey_in].Count, _Database[collectionKey_in].ItemsToString()));
                }
            }
            else
            {
                List<object> origCollection = items_in;
                List<object> sqlStoredCollection = _Database[string.Format("{0}{1}", SqlHiLevel.COLLECTION_KEY_PREFIX_FOR_ONLY_RESULT_ROWS, collectionKey_in)];

                //check items Count:
                if (origCollection.Count != sqlStoredCollection.Count)
                {
                    StringBuilder errorMsg = new StringBuilder();
                    errorMsg.AppendFormat("Sql stored collection inconsistency for collection: {0}.\n", collectionKey_in);
                    errorMsg.AppendFormat("Sql stored collection (Count: {0}) - {1}\n", sqlStoredCollection.Count, sqlStoredCollection.ItemsToString());
                    errorMsg.AppendFormat("Referenced collection (Count: {0})", origCollection.Count);

                    Trace.TraceInformation("Additional info for SqlInconsistencyException: counted values: {0}", origCollection.ItemsToString());

                    throw new SqlResultInconsistencyException(errorMsg.ToString());
                }

                //check items Content:
                for (int i = 0; i < origCollection.Count; i++)
                {
                    Trace.TraceInformation("Sql full consistency checking for {0}.", origCollection[i]);
                    bool founded = false;

                    foreach (object sqlObj in sqlStoredCollection)
                    {
                        if (((IComparable)origCollection[i]).CompareTo(sqlObj) == 0)
                        {
                            founded = true;
                            break;
                        }
                    }

                    if (!founded)
                    {
                        StringBuilder errorMsg = new StringBuilder();
                        errorMsg.AppendFormat("Sql stored collection inconsistency for collection: {0}, Item: {1}.\n",
                            collectionKey_in, i);
                        errorMsg.AppendFormat("Referenced item: {0}\n", origCollection[i]);

                        errorMsg.AppendFormat("Sql stored items: \n{0}\n", sqlStoredCollection.ItemsToString("\n"));
                        throw new SqlResultInconsistencyException(errorMsg.ToString());
                    }
                }
            }
        }

        private void SqlConsistencyCheckResult(ICheckableBase obj_in)
        {
            if (MeasureCollectionBase.TheMeasures.IsSqlLoaded
                //only for SQL loaded items:
                && (((obj_in is ISqlRowDescriptorHierarchy) && (obj_in as ISqlRowDescriptorHierarchy).Sql.RowId != SqlLowLevel.INVALID_ROW_ID)
                    || ((obj_in is ISqlRowDescriptor) && (obj_in as ISqlRowDescriptor).Sql.RowId != SqlLowLevel.INVALID_ROW_ID))
                && obj_in.SqlStoredCheckResult != obj_in.CheckResult)
            {
                throw new SqlCheckableInconsistencyException(obj_in);
            }
        }

        private void SqlConsistencyCheckPartial(string collectionKey, List<object> items)
        {
            if (!(items[0] is ISqlHiLevel))
                throw new InvalidOperationException("Consistency check needs ISqlHiLevel too");

            IEnumerable<string> resultColumns = (items[0] as ISqlHiLevel).Sql.SqlLoadedResults.Keys;//(items[0] as ISqlHiLevel).Sql.SqlLoadedResults.Keys.Where( item => item.StartsWith(SqlHiLevel.COLUMN_NAME_PREFIX_RESULT));

            if (resultColumns.GetEnumerator().MoveNext()) //there is any result (
            {
                //to much log Trace.TraceInformation("Sql consistency checking for {0}. Partial check for columns: {1}", collectionKey, resultColumns.ItemsToString());

                using (NpgsqlCommand tmpBuffer = new NpgsqlCommand())//storage for runtime counted values
                {
                    foreach (object item in items)
                    {
                        tmpBuffer.Parameters.Clear();

                        (item as ISqlHiLevel).SqlSave(tmpBuffer, this, collectionKey);//load runtime counted values

                        //check consistency:
                        foreach (string resultColumn in resultColumns)
                        {
                            if (resultColumn == SqlLowLevel.COLUMN_NAME_ID || resultColumn == "ok")
                                continue;

                            if (tmpBuffer.Parameters.Contains(resultColumn))
                            {
                                bool isSame;

                                if (tmpBuffer.Parameters[resultColumn].Value is float && (item as ISqlHiLevel).Sql.SqlLoadedResults[resultColumn] is float)
                                    isSame = ((float)tmpBuffer.Parameters[resultColumn].Value)
                                            .IsEqual((float)(item as ISqlHiLevel).Sql.SqlLoadedResults[resultColumn]);
                                else if (((item as ISqlHiLevel).Sql.SqlLoadedResults[resultColumn] is int)
                                    || ((item as ISqlHiLevel).Sql.SqlLoadedResults[resultColumn] is long))
                                {//sql stored int/long can be: signed/unsigned int, short, byte, etc. at memory. Cast memory:
                                    isSame = Convert.ToInt64((item as ISqlHiLevel).Sql.SqlLoadedResults[resultColumn]) == Convert.ToInt64(tmpBuffer.Parameters[resultColumn].Value);
                                }
                                else if ((item as ISqlHiLevel).Sql.SqlLoadedResults[resultColumn] is IComparable
                                    && (item as ISqlHiLevel).Sql.SqlLoadedResults[resultColumn].GetType() == tmpBuffer.Parameters[resultColumn].Value.GetType())
                                {
                                    isSame = ((item as ISqlHiLevel).Sql.SqlLoadedResults[resultColumn] as IComparable).CompareTo(tmpBuffer.Parameters[resultColumn].Value) == 0;
                                }
                                else if ((item as ISqlHiLevel).Sql.SqlLoadedResults[resultColumn] == DBNull.Value) //sql stored is null
                                {
                                    isSame = true; /*is there a little leak... 
                                    * but we must to accept null value at DB, because adding new 'res_' colun into an existing DB, 
                                    * leads to the old records contains null value. We have to avoid InconsistencyException in that case...*/
                                }
                                else
                                    isSame = (tmpBuffer.Parameters[resultColumn].Value.ToString() ==
                                        (item as ISqlHiLevel).Sql.SqlLoadedResults[resultColumn].ToString());

                                //reason of usage of ToString(): (object)0f != (object)0f
                                if (!isSame)
                                {
                                    StringBuilder errorMsg = new StringBuilder();
                                    errorMsg.AppendFormat("Sql stored collection inconsistency for collection: {0} \nColumn: {1}.\n", collectionKey, resultColumn);
                                    errorMsg.AppendFormat("Item: {0}. SqlRowId={1}\n", item, (item as ISqlHiLevel).Sql.RowId);
                                    errorMsg.AppendFormat("Sql stored value: {0}\n", (item as ISqlHiLevel).Sql.SqlLoadedResults[resultColumn]);
                                    errorMsg.AppendFormat("Counted value: {0}", tmpBuffer.Parameters[resultColumn].Value);

                                    throw new SqlResultInconsistencyException(errorMsg.ToString());
                                }
                            }
                            else
                            {
                                throw new MeasureBaseInternalException(string.Format("ResultColumn {0} cannot be founeded at returned parameters of {1}.SqlSave ",
                                    resultColumn, item.GetType().Name));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Virtual function, base implementation calls Check()
        /// function of all ICheckable object at Database.
        /// Sets this.FailedSteps property.
        ///
        /// Exceptions:
        /// WrongCheckNumberException       assertCheckNumber_in is specified, and failed
        /// SqlCheckedStateInconsistency    measure is SQL loaded, but 'ok' column state is inconsistent
        ///
        /// </summary>
        /// <param name="assertCheckNumber_in">optional assertation for all checked item. Sends WrongCheckNumberException exception if assertation failed.</param>
        /// <returns>OK</returns>
        public virtual bool? OnCheck()
        {
            Trace.TraceInformation("MeasureRoot.OnCheck() for obj {0}", this);
            int checkedNumber = 0;
            bool? res = true;
            FailedSteps = new List<ICheckableBase>();

            //do check for ICheckable, and check SQl consistency of ICheckable.CheckResult
            foreach (object obj in _Database.Values)
            {
                if (obj is ICheckableBase)
                {
                    CheckACheckable(ref checkedNumber, ref res, obj, false);
                }
            }

            CheckACheckable(ref checkedNumber, ref res, this, res == false);

            if (CheckEvent != null && CheckEvent.GetInvocationList() != null)
            {
                SuccessfulEventArgs arg = new SuccessfulEventArgs();
                arg.Successful = res.HasValue && res.Value;
                CheckEvent.Invoke(this, arg);
                if (!arg.Successful)
                    res &= false;
            }

            if (CheckedItemNumberAssert.HasValue)
            {
                Trace.TraceInformation("Assert checked number: {0}", CheckedItemNumberAssert.Value);
                if (CheckedItemNumberAssert.Value != checkedNumber)
                    throw new WrongCheckNumberException(this, CheckedItemNumberAssert.Value, checkedNumber);
            }

            Trace.TraceInformation("MeasureRoot.OnCheck() result {0}", res.LocalizedStr());

            return res;
        }

        private void CheckACheckable(ref int checkedNumber, ref bool? res_inout, object obj, bool forcedFailed_in)
        {
            //      Trace.TraceInformation("MeasureRoot.CheckACheckable for Obj.Hash={4} 
            //(ref int checkedNumber = {0}, ref bool? res_inout  = {1}, object obj  = {2}, bool forcedFailed_in = {3})",
            //checkedNumber, res_inout.LocalizedStr(), obj, forcedFailed_in, obj.GetHashCode());
            checkedNumber++;

            ICheckableBase checkable = (ICheckableBase)obj;
            bool? bRes = null;
            if (obj is ICheckableThreeState)
                bRes = ((ICheckableThreeState)obj).Check();
            else
                bRes = ((ICheckable)obj).Check();

            if (forcedFailed_in)
                bRes = false;

            checkable.CheckResult = bRes;

            if (MeasureCollectionBase.TheMeasures.IsSqlLoaded
                && checkable.SqlStoredCheckResult != checkable.CheckResult
                && !(checkable is MeasureRoot)) //measures will be consistency checked later, when its steps has been processed
            {
                throw new SqlCheckableInconsistencyException(checkable);
            }

            ExtensionMethodsNullableBool.MergeSubResult(ref res_inout, bRes);

			if (bRes.HasValue && !bRes.Value)
			{
				OK = false;
				FailedSteps.Add(checkable);
			}
			else
				OK = true;

			
            Trace.TraceInformation("MeasureRoot.CheckACheckable finished.  checkedNumber = {0}, res_inout  = {1}", checkedNumber, res_inout.LocalizedStr());
        }

        public delegate void CheckEventHandler(object sender, SuccessfulEventArgs e);

        public event CheckEventHandler CheckEvent;

        /// <summary>
        /// if specified, the FW sends SqlCheckableInconsistencyException if the number of called ICheckeble.Check()
        /// is not same as CheckedItemNumberAssert.Value
        /// </summary>
        public int? CheckedItemNumberAssert;

        public List<ICheckableBase> FailedSteps { get; private set; }

        /// <summary>
        /// set in case of SQL load,
        /// store for Consistency check (besause this items are not present in this._Database
        /// </summary>
        internal List<SqlRowDescriptorAdditionalLeaf> LoadedAdditionalLeafTables { get; private set; }

        #endregion ICheckable and consistency check

        public event SuccessfulEventHandler PreProcess;

        /// <summary>
        /// Called after this measure counted, the Load/Count/Preprocess can be retryable by this function
        /// Virtual function, base implementation calls PreProcess event, and PreProcess() function 
		///of all IPreProcessable object at Database
        /// </summary>
        /// <returns>PreProcess OK.</returns>
        public virtual bool OnPreProcess()
        {
            Trace.TraceInformation("MeasureRoot.OnPreProcess() for obj {0}", this);
            bool ok = true;
            if (PreProcess != null && PreProcess.GetInvocationList().Length > 0)
            {
                SuccessfulEventArgs arg = new SuccessfulEventArgs();
                PreProcess.Invoke(this, arg);
                ok = arg.Successful;
            }

            foreach (object obj in _Database.Values)
                if (obj is IPreProcessable)
                    ok &= ((IPreProcessable)obj).PreProcess();

            return ok;
        }

        public virtual bool OnPostProcess()
        {
            bool ok = true;
            foreach (object obj in _Database.Values)
                if (obj is IPostProcessable)
                    ok &= ((IPostProcessable)obj).PostProcess();

            return ok;
        }

        virtual public string AllTraceInfo
        {
            get
            {
                StringBuilder res = new StringBuilder();

                res.AppendFormat("{0}\n", GetType().Name);
                res.AppendLine(this._Database.AllTraceInfo);

                res.AppendLine("\nAllInputData:\n");
                res.AppendLine(_AllInputData.ToString());

                return res.ToString();
            }
        }
		public static bool OK { get; set; }
        virtual public string SN { get; set; }

	}
}