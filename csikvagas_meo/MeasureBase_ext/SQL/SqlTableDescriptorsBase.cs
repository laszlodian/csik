using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.Model;

namespace e77.MeasureBase.Sql
{
    /// <summary>
    /// singleton for virtual overriden support
    /// Can implement SqlHiLevel.ISqlGlobal
    /// </summary>
    public class SqlTableDescriptorsBase 
    {
        public SqlTableDescriptorsBase()
        {
            TheDescriptor = this;
            
            //parameter table: only for HiLevel
            if ( (this is ISqlGlobal))
            {
                if((this as ISqlGlobal).SqlParameterColumns != null)
                {
                    List<SqlTableDescriptorAdditional> additionals = new List<SqlTableDescriptorAdditional>(SqlHiLevel.MainTable.AdditionalTables);
                    
                    additionals.Add(new SqlTableDescriptorAdditionalObj(string.Format("{0}_parameters", SqlHiLevel.MainTable.TableName),
                        (SqlTableDescriptorsBase.TheDescriptor as ISqlGlobal).SqlParameterColumns
                            .Select(item => MeasureConfig.IsForIni(item)
                                ? MeasureConfig.GetIniFileSqlColumnName(item)       //Ini file ID => Sql column name conv.
                                : item), 
                        SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.Unique,
                        delegate() { return MeasureConfig.TheConfig; }));

                    SqlHiLevel.MainTable.AdditionalTables = additionals;
                }

                MeasureConfig.IpThermoConfig = SqlHiLevel.MainTable.IpThermoConfig;
            }
        }

        static SqlTableDescriptorsBase _TheDescriptor;
        public static SqlTableDescriptorsBase TheDescriptor 
        {
            get
            {
                if(_TheDescriptor == null)
                    throw new InvalidOperationException("Create an object descendant of SqlTableDescriptorsBase, before using this property. E.g. 'new SqlTableDescriptorsMeo();//create SQL singleton'");
                return _TheDescriptor;
            }
            
            private set 
            {
                if (_TheDescriptor != null)                
                    throw new InvalidOperationException("SQLTableDescriptor class is singleton. Do not create more.");

                _TheDescriptor = value;
            } 
        }

        /// <summary>
        /// false only if SqlTableDescriptorsBase is not created, which means there is no need for SQL
        /// </summary>
        internal static bool IsTheDescriptorExist
        {
            get {return _TheDescriptor != null;} 
        }

        public static SqlTableDescriptorMainTable_Internal MainTable
        {
            get
            {
                if(!( _TheDescriptor is ISqlGlobal))
                    throw new InvalidOperationException();

                return (SqlTableDescriptorMainTable_Internal) ((_TheDescriptor as ISqlGlobal).SqlAllTable)
                    .Single(item => item is SqlTableDescriptorMainTable_Internal);
            }
        }
    }
}
