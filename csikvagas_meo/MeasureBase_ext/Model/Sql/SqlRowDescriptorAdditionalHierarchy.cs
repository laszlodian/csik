using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.Sql;
    
namespace e77.MeasureBase.Model
{
    public class SqlRowDescriptorAdditionalHierarchy : SqlRowDescriptorAdditional
    {
        public SqlRowDescriptorAdditionalHierarchy(SqlTableDescriptorAdditionalHierarchy tableDescriptor_in, SqlRowDescriptor parent_in)
            : base(tableDescriptor_in, parent_in)
        {
        }

        /// <summary>
        /// can be only one
        /// </summary>
        public ISqlRowDescriptorHierarchy TheObject 
        {
            get
            {
                return SqlTableDescriptorAdditionalHierarchy.AllAdditionalHierarchyObjectMap[this.TableDescriptor];
            }
        }

        /// <summary>
        /// Invalid cas exception: can be internal Fw error: 
        /// this class should contain only SqlTableDescriptorAdditionalLeaf, SqlTableDescriptorAdditionalHierarchy
        /// should be stored by SqlTableDescriptorAdditionalHierarchy 
        /// </summary>
        new public SqlTableDescriptorAdditionalHierarchy TableDescriptor
        {
            get { return (SqlTableDescriptorAdditionalHierarchy)base.TableDescriptor; }
            protected set { base.TableDescriptor = value; }
        }

    }
}
