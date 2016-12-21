using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using e77.MeasureBase.Model;

namespace e77.MeasureBase.Sql
{ 
    /// <summary>
    /// SqlDescriptor of a SQL record. In order to use Middle-Level SQL handling.
    /// </summary>
    public class SqlRowDescriptor
    {
        public SqlRowDescriptor(SqlTableDescriptorBase staticDescriptor_in, ISqlRowDescriptor containerObj_in)
            : this(staticDescriptor_in, (object) containerObj_in) { ;}

        internal SqlRowDescriptor(SqlTableDescriptorBase tableDescriptor_in, object containerObj_in)
        {
            RowId = SqlLowLevel.INVALID_ROW_ID;
            TableDescriptor = tableDescriptor_in;
            ContainerObject = containerObj_in;

            AdditoinalRecords = new List<SqlRowDescriptorAdditional>();
            if (tableDescriptor_in != null && tableDescriptor_in.AdditionalTables != null)
            {
                foreach (SqlTableDescriptorAdditional tableDesc in tableDescriptor_in.AdditionalTables)
                {
                    if (tableDesc is SqlTableDescriptorAdditionalLeaf)
                        AdditoinalRecords.Add(new SqlRowDescriptorAdditionalLeaf((SqlTableDescriptorAdditionalLeaf)tableDesc, this));
                    else
                        AdditoinalRecords.Add(new SqlRowDescriptorAdditionalHierarchy((SqlTableDescriptorAdditionalHierarchy)tableDesc, this));
                }
            };
        }

        /// <summary>
        /// SqlLowLevel.INVALID_ROW_ID == long.MinValue == invalid
        /// </summary>
        public long RowId { get; set; }

        /* it is not needed, just set this.Sql == null
        /// <summary>
        /// Sql store or not
        /// </summary>
        public bool Enabled { get; private set; }*/

        public SqlTableDescriptorBase TableDescriptor { get; protected set; }

        /// <summary>
        /// ISqlDescriptor (if this is exactly SqlDescriptor) or ISqlDescriptorHierarchy (if this is SqlDescriptorHierarchy)
        /// </summary>
        public object ContainerObject { get; private set; }

        
        public List<SqlRowDescriptorAdditional> AdditoinalRecords { get; internal set; }

        /// <summary>
        /// Invalidates the RowId of all Additional (rows of) tables, in order to force saving the SQL record.
        /// </summary>
        /// <param name="tableName_in"></param>
        [Obsolete("Not required, Additional tables are invalidated automatically at Sql.StoreAll")] //see SqlAdditionalAutoInvalidate
        public void InvalidateAdditional(string tableName_in)
        {            
            AdditoinalRecords.Single(item => item.TableDescriptor.TableName == tableName_in).RowId = SqlLowLevel.INVALID_ROW_ID;
        }

        public readonly long INVALID_ROW_ID = SqlLowLevel.INVALID_ROW_ID;

        public override string ToString()
        {
            return string.Format("SqlDescriptor (RowId= {0}), StaticDescriptor= {1}, Container= {2}",
                this.RowId, this.TableDescriptor, this.ContainerObject);
        }
    }
}
