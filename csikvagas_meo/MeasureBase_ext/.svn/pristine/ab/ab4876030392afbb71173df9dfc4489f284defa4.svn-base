using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using e77.MeasureBase.Model;
using Npgsql;
using e77.MeasureBase.MeasureEnvironment;

namespace e77.MeasureBase.Sql
{
    /// <summary>
    /// Sql descriptor for rows containing the 4 environment id columns:
    /// User( LoginName, AccessRight, FullName); Workplace (ComputerName, RoomId), SW version, Sql table and Program name
    /// </summary>
    public class SqlTableDescriptorEnvironmentId : SqlTableDescriptorBase
    {        
        public enum EIpThermoConfig
        {
            no_ipthermo,
            /// <summary>
            /// 0 = invalid
            /// </summary>
            [Obsolete("Possible is not user-frendly now. Need specification... (currently shows selection form each time)")]
            ipthermo_possible,
            ipthermo_required
        };

        public SqlTableDescriptorEnvironmentId(string tableName_in, IEnumerable<string> columnNames_in, 
                IEnumerable<SqlTableDescriptorAdditional> additionalTables_in)
            : base(tableName_in)
        {
           // IpThermoConfig = ipThermoConfig_in;
            SetupFullUserIdRecord(this, columnNames_in, additionalTables_in);
        }

        //internal SqlDescriptorEnvironmentId.EIpThermoConfig IpThermoConfig { get; set; }

    
        /// <summary>
        /// Needs configured object of MeasureCollectionBase.
        /// </summary>
        /// <param name="this_in"></param>
        /// <param name="columnNames_in"></param>
        /// <param name="additionalTables_in"></param>
        /// <param name="ipThermoConfig_in"></param>
        internal static void SetupFullUserIdRecord(SqlTableDescriptorBase this_in, IEnumerable<string> columnNames_in, 
            IEnumerable<SqlTableDescriptorAdditional> additionalTables_in)
        {
            //add default columns:
            List<string> columns = new List<string>(columnNames_in);
                        
          
            columns.AddRange( new string[] { "fk_global_sw_versions_id", "fk_global_workplaces_id", "fk_global_users_id","date" } );

            this_in.ColumnNames = columns.Distinct();

            List<SqlTableDescriptorAdditional> additionals;
            if (additionalTables_in != null)
                additionals = new List<SqlTableDescriptorAdditional>(additionalTables_in);
            else
                additionals = new List<SqlTableDescriptorAdditional>();

            //append default additional tables:
            additionals.Add(new SqlTableDescriptorAdditionalObj("global_sw_versions", new string[] { "sw_version" },
                SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.Unique, delegate() { return SwVersion.TheSwVersion; }));
            additionals.Add(EnvironmentTables.WorkplacesTable);
            additionals.Add(EnvironmentTables.UsersTable);
            //parameter table will be added by SqlTableDescriptorsBase()

            this_in.AdditionalTables = additionals;
        }
    }
}
