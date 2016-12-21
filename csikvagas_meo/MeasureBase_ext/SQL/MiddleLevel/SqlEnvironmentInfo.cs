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
    /// FullUserId= User( LoginName, AccessRight, FullName); Workplace (ComputerName, RoomId), SW version
    /// </summary>
    public class SqlEnvironmentInfo : SqlStaticDescriptorBase
    {
        public enum EFixMainTableColumns { room_temperature, date };

        public enum EIpThermoConfig
        {
            no_ipthermo,
            ipthermo_possible,
            ipthermo_required
        };

        public SqlEnvironmentInfo(string tableName_in, IEnumerable<string> columnNames_in, 
                IEnumerable<SqlStaticDescriptorAdditional> additionalTables_in, SqlEnvironmentInfo.EIpThermoConfig ipThermoConfig_in)
            : base(tableName_in)
        {
            IpThermoConfig = ipThermoConfig_in;
            SetupFullUserIdRecord(this, columnNames_in, additionalTables_in, ipThermoConfig_in);
        }

        internal SqlEnvironmentInfo.EIpThermoConfig IpThermoConfig { get; set; }
    
        /// <summary>
        /// Needs configured object of MeasureCollectionBase.
        /// </summary>
        /// <param name="this_in"></param>
        /// <param name="columnNames_in"></param>
        /// <param name="additionalTables_in"></param>
        /// <param name="ipThermoConfig_in"></param>
        internal static void SetupFullUserIdRecord(SqlStaticDescriptorBase this_in, IEnumerable<string> columnNames_in, 
            IEnumerable<SqlStaticDescriptorAdditional> additionalTables_in, SqlEnvironmentInfo.EIpThermoConfig ipThermoConfig_in)
        {
            //add default columns:
            List<string> columns = new List<string>(columnNames_in);

            columns.AddRange(Enum.GetNames(typeof(SqlEnvironmentInfo.EFixMainTableColumns)));

            if (ipThermoConfig_in == SqlEnvironmentInfo.EIpThermoConfig.no_ipthermo)
                columns.Remove(SqlEnvironmentInfo.EFixMainTableColumns.room_temperature.ToString());

            this_in.ColumnNames = columns;

            List<SqlStaticDescriptorAdditional> additionals;
            if (additionalTables_in != null)
                additionals = new List<SqlStaticDescriptorAdditional>(additionalTables_in);
            else
                additionals = new List<SqlStaticDescriptorAdditional>();

            //append default additional tables:
            additionals.Add(new SqlStaticDescriptorAdditionalObj("ud2_sw_versions", new string[] { "sw_version", "sql_version" },
                SqlStaticDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.Unique, delegate() { return SwVersion.TheSwVersion; }));
            additionals.Add(EnvironmentTables.WorkplacesTable);
            additionals.Add(EnvironmentTables.UsersTable);
            //parameter table will be added by SqlStaticDescriptorsBase()

            this_in.AdditionalTables = additionals;

            this_in.ForceMainTableMembers = true;
        }

        internal static void StoreMainTableColumns(SqlDescriptor obj_in, NpgsqlCommand cmdInsert)
        {
            DateTime date = MeasureCollectionBase.TheMeasures.MeasureDate;
            SqlEnvironmentInfo.EIpThermoConfig ipConf;
            if (obj_in.StaticDescriptor is SqlTableStaticDescriptorMainTable_Internal)
            {
                ipConf = ((SqlTableStaticDescriptorMainTable_Internal)obj_in.StaticDescriptor).IpThermoConfig;
            }
            else if (obj_in.StaticDescriptor is SqlEnvironmentInfo)
            {
                ipConf = (obj_in.StaticDescriptor as SqlEnvironmentInfo).IpThermoConfig;
            }
            else
                throw new InvalidConfigurationException("bad type");


            if (ipConf != SqlEnvironmentInfo.EIpThermoConfig.no_ipthermo)
            {
                if (ipConf == SqlEnvironmentInfo.EIpThermoConfig.ipthermo_possible
                    && MeasureConfig.TheConfig.RoomId == 0)
                {
                    cmdInsert.Parameters.AddWithValue('@' + SqlEnvironmentInfo.EFixMainTableColumns.room_temperature.ToString(),
                        null);
                }
                else
                    cmdInsert.Parameters.AddWithValue('@' + SqlEnvironmentInfo.EFixMainTableColumns.room_temperature.ToString(),
                        Rooms.GetIpThermoValue(MeasureConfig.TheConfig.RoomId));
            }

            if (date == DateTime.MinValue)
                throw new MeasureBaseInternalException("MeasureDate has not been set");

            cmdInsert.Parameters.AddWithValue('@' + SqlEnvironmentInfo.EFixMainTableColumns.date.ToString(),
                date);
        }
    }
}
