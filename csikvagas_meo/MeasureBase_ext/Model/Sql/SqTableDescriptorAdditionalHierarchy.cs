using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using e77.MeasureBase.Sql;

namespace e77.MeasureBase.Model
{
    /// <summary>
    /// Only for SqlHiLevel.
    /// For specify additional table (use this class at parent table descriptor), which has child and/or additional tables (has own table hierarchy)
    /// 
    /// Example:
    ///      Parent Table static descriptor:   new SqlTableDescriptorAdditional[] {new SqlTableDescriptorAdditionalHierarchy(SQL_TABLE_CALIBRATION, true)}
    ///      
    /// Table static descriptor: public static SqlTableDescriptorAdditionalRoot<CalibrationStep> SQL_TABLE_CALIBRATION =
    ///        new SqlTableDescriptorAdditionalRoot<CalibrationStep>(
    ///        "ua3_head_calibration_calibration", Enum.GetNames(typeof(ESqlTableColumnsCalibration)), null, ESqlConsistencyType.AllColumn);
    ///        
    /// 
    /// Additional table can contains child table:
    ///   public static SqlTableDescriptorHierarhyTableChild<CalibrationStepItem> SQL_TABLE_CALIBRATION_ITEMS =
    ///        new SqlTableDescriptorHierarhyTableChild<CalibrationStepItem>(
    ///        SQL_TABLE_CALIBRATION, "ua3_head_calibration_calibration_items", 
    ///        ESqlConsistencyType.Normal, Enum.GetNames(typeof(ESqlTableColumnsCalibrationItems)), null);
    /// </summary>
    public class SqlTableDescriptorAdditionalHierarchy : SqlTableDescriptorAdditional
    {
        internal static Dictionary<SqlTableDescriptorAdditionalRoot_Internal, SqlTableDescriptorAdditionalHierarchy> AllAdditionalRoots = new Dictionary<SqlTableDescriptorAdditionalRoot_Internal, SqlTableDescriptorAdditionalHierarchy>();

        internal static Dictionary<SqlTableDescriptorAdditionalHierarchy, ISqlRowDescriptorHierarchy> AllAdditionalHierarchyObjectMap = new Dictionary<SqlTableDescriptorAdditionalHierarchy, ISqlRowDescriptorHierarchy>();
        internal static Dictionary<SqlTableDescriptorAdditionalHierarchy, string> AllAdditionalHierarchyObjectCollectionKeyMap = new Dictionary<SqlTableDescriptorAdditionalHierarchy, string>();


        public SqlTableDescriptorAdditionalHierarchy(SqlTableDescriptorAdditionalRoot_Internal associatedRootTable_in, bool nullAccepted_in)
            : base(associatedRootTable_in.TableName, associatedRootTable_in.ColumnNames, nullAccepted_in)
        {
            AssociatedRootTable = associatedRootTable_in;
            AllAdditionalRoots.Add(associatedRootTable_in, this);

            AllAdditionalHierarchyObjectMap.Add(this, null);
            AllAdditionalHierarchyObjectCollectionKeyMap.Add(this, null);
        }

        internal SqlTableDescriptorAdditionalRoot_Internal AssociatedRootTable { get; set; }
    }
}
