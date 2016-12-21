using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.Sql;

namespace e77.MeasureBase.Model
{
    public interface ISqlRowDescriptorHierarchy 
    {
        SqlRowDescriptorHierarchy Sql { get; set; }
    }

    public class SqlRowDescriptorHierarchy : SqlRowDescriptor
    {
        /// <summary>
        /// In order to runtime deny Sql save for object implementing ISqlHiLevel interface
        /// </summary>
        public static SqlRowDescriptorHierarchy EMPTY = new SqlRowDescriptorHierarchy();

        /// <summary>
        /// only for static SqlRowDescriptorHierarchy.EMPTY
        /// </summary>
        internal SqlRowDescriptorHierarchy() : base(null, null) { ;}

        /// <summary>
        /// SqlTableDescriptor is automatically filled, by searching SqlHiLevel.AllTables for the appropriate item.
        /// </summary>
        /// <param name="containerObj_in"></param>
        public SqlRowDescriptorHierarchy(ISqlRowDescriptorHierarchy containerObj_in)
            : this(SqlHiLevel.FindTableDescriptor(containerObj_in), (containerObj_in as ISqlRowDescriptorHierarchy))
        {; }

        public SqlRowDescriptorHierarchy(SqlTableDescriptorHierarhyTableBase tableDescriptor_in, ISqlRowDescriptorHierarchy containerObj_in)
            : base(tableDescriptor_in, containerObj_in)
        {
            ParentId = SqlLowLevel.INVALID_ROW_ID;
        }        

        private long _ParentId;

        ISqlRowDescriptorHierarchy _parentObj;
        /// <summary>
        /// If there is only one parent item this property can remains null (FW will fount this item by searching TableDescritptors.
        /// If there are more parent, set it exactly. (and create Sql obj manually by 'obj_in.Sql = new SqlDescriptorHierarchy(obj_in);' (generaly SqlHiLevel.SaveAll creates it)
        /// </summary>
        public ISqlRowDescriptorHierarchy ParentObj
        { 
            get { return _parentObj; }
            set
            {
                if (!(this.TableDescriptor is SqlTableDescriptorHierarhyTableChild_Internal))
                    throw new InvalidOperationException("Only childrens can have Parent object ");

                _parentObj = value;
            }
        }

        /// <summary>
        /// SqlLowLevel.INVALID_ROW_ID == long.MinValue == invalid(e.g. not loaded or not stored yet).
        /// This property is invalid in case of non hieararchy table (when TableDescriptor is NOT SqlTableDescriptorHierarhyTableBase only SqlTableDescriptorBase)
        /// Reason: I don't want to remove this property to a descendant class (e.g. SqlDescriptorHierarchy), in order to easy to use the SQL, in this way we not need to set the hierarchy info 2 times (static and non-static descriptor).
        /// </summary>
        public long ParentId
        {
            get
            {
                if (TableDescriptor is e77.MeasureBase.Model.SqlTableDescriptorHierarhyTableBase == false)
                {
                    throw new ArgumentException("This is not Hierarchy descriptor. See the description of this property.");
                }
                return _ParentId;
            }
            set { _ParentId = value; }
        }
                
        new public SqlTableDescriptorHierarhyTableBase TableDescriptor 
        {
            get { return (SqlTableDescriptorHierarhyTableBase)base.TableDescriptor; }
            protected set { base.TableDescriptor = value; }
        }

        public override string ToString()
        {
            return string.Format("SqlRowDescriptorHierarchy: RowId: {0}, ParentID: {1}; TableDescriptor:'{2}'",
                RowId, ParentId, TableDescriptor);
        }

        #region For SqlHiLevel / Sql Consistency check of partial result records

        private Dictionary<string, object> _SqlLoadedResults;

        /// <summary>
        /// Main or child table: for contains only partial result, where colum name 'res_xxx' ( SqlHiLevel.COLUMN_NAME_PREFIX_RESULT )
        /// Additional table:   If all data is result (consistency check needed) use  ESQlAdditionalTableLeafOptions.OnlyResult
        /// 
        /// Reason of public access level: In case of loading custom object from SQL, needs to add the obj to this list in order ot avoid default object load
        /// </summary>
        public Dictionary<string, object> SqlLoadedResults
        {
            get
            {
                return _SqlLoadedResults; //null ref exception if SqlHiLevel.LoadAll has not been called (or SqlHiLevel is not used at all)
            }

            set
            {
                if (false == this.TableDescriptor is e77.MeasureBase.Model.SqlTableDescriptorHierarhyTableBase
                    || ((e77.MeasureBase.Model.SqlTableDescriptorHierarhyTableBase)this.TableDescriptor).ConsistencyFullCollection)
                    throw new MeasureBaseInternalException(string.Format("SqlLoadedResults can be used only for 'OnlyResult == false'. Problem with table: {0}",
                        this.TableDescriptor.TableName));

                _SqlLoadedResults = value;
            }
        }

        #endregion
    }
}
