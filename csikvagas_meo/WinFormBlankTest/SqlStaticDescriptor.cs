using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.Sql;
using e77.MeasureBase.Model;

namespace WinFormBlankTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using e77.MeasureBase.Sql;
    using e77.MeasureBase.Model;

    namespace WinFormBlankTest
    {
        /*    public class SqlDescriptorBlankTest : SqlTableDescriptorsBase, ISqlGlobal //, ISqlDynamicDescriptor
           {
              public enum ESqlMainTableColumns { code, glu, wrong_step, measure_id, serial_number, is_check_strip, fk_blank_test_errors_id, nano_amper };

               #region SqlTableDescriptorMainTable

               public static SqlTableDescriptorMainTable<Device> SQL_MAIN_TABLE = new SqlTableDescriptorMainTable<Device>(
                    "blank_test_result", new string[] { "code", "glucose", "wrong_step", "measure_id", "serial_number", "is_check_strip", "fk_blank_test_errors_id", "voltage" },
                    new SqlTableDescriptorAdditional[] {
                   new SqlTableDescriptorAdditionalNoObj("blank_test_environment", new string[] {"user_name","computer_name","start_date","end_date","temperature","fk_blank_test_result_id"}, 
                       SqlTableDescriptorAdditionalLeaf.ESQlAdditionalTableLeafOptions.NullAccepted,
                       delegate(Npgsql.NpgsqlDataReader sqlData_in) 
                       {
                       
                       },
                       delegate() 
                       {
                           Dictionary<string, object> data = new Dictionary<string, object>();
                           data.Add("user_name", MessageCompletedEventArgs.Controller);
                           data.Add("computer_name", MessageCompletedEventArgs.ComputerName);
                           data.Add("measurement_time",DateTime.Now);
                      
                           return data;
                       }), 
                   
               
               }, SqlTableDescriptorEnvironmentId.EIpThermoConfig.ipthermo_possible);

               #endregion


               //public static SqlTableDescriptorHierarhyTableBase[] SQL_ALL_TABLE = 
               //{ 
               //    SQL_MAIN_TABLE

               //};


               //    #region ISqlGlobal Members

               //    public SqlTableDescriptorHierarhyTableBase[] SqlAllTable;
               //    public IEnumerable<string> SqlParameterColumns;

               //    #endregion




               //}



               #region ISqlGlobal Members

               public SqlTableDescriptorHierarhyTableBase[] SqlAllTable
               {
                   get { throw new NotImplementedException(); }
               }

               public IEnumerable<string> SqlParameterColumns
               {
                   get { throw new NotImplementedException(); }
               }

               #endregion
           }*/



    }
}

