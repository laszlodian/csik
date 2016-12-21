using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.Sql;
using e77.MeasureBase.MeasureEnvironment;

namespace e77.MeasureBase.Sql
{
    /// <summary>
    /// Static SQL descriptors of EnvironmentTables
    /// </summary>
    /// <remarks></remarks>
    internal static class EnvironmentTables
    {
        static EnvironmentTables()
        {
            WorkplacesTable = new SqlTableDescriptorAdditionalNoObj("global_workplaces",
                new string[] { "computer_name", "ipthermo_probe_id" }, 
                SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.Unique,
                delegate(Npgsql.NpgsqlDataReader sqlData_in)
                {
                    if (!sqlData_in.IsDBNull(sqlData_in.GetOrdinal("ipthermo_probe_id")))
                        EnvironmentId.TheEnvironmentId.RoomId = (int)sqlData_in["ipthermo_probe_id"];

                    EnvironmentId.ComputerName = (string)sqlData_in["computer_name"];
                },
                delegate()
                {
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("ipthermo_probe_id", EnvironmentId.TheEnvironmentId.RoomId);
                    data.Add("computer_name", EnvironmentId.ComputerName);
                    return data;
                });

            UsersTable = new SqlTableDescriptorAdditionalNoObj("global_users", new string[] { "user_name", "fullname", "access_right" },
                SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.Unique,
               delegate(Npgsql.NpgsqlDataReader sqlData_in)
               {
                   e77User.TheUser.Name = (string)sqlData_in["user_name"];
                   e77User.TheUser.FullName = (string)sqlData_in["fullname"];
                   e77User.TheUser.SetAccessRights((string)sqlData_in["access_right"]);
               },
               delegate()
               {
                   Dictionary<string, object> data = new Dictionary<string, object>();
                   data.Add("user_name", e77User.TheUser.Name);
                   if (!e77User.LdapDenied)
                        data.Add("fullname", e77User.TheUser.FullName);
                   else
                       data.Add ("fullname", "N/A");

                   data.Add("access_right", e77User.TheUser.ValidAccessRights.ItemsToString()); 
                   return data;
               });
        }

        public static SqlTableDescriptorAdditionalLeaf WorkplacesTable { get; private set; }
        public static SqlTableDescriptorAdditionalLeaf UsersTable { get; private set; }
    }
}
