using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.Sql;
using e77.MeasureBase;

namespace LabelPrinter
{
    public class SqlRecordSn : ISqlSaveable
    {
        public SqlRecordSn(string tableName_in)
        {
            SqlStaticDescriptorBase staticdesc = new SqlStaticDescriptorBase(tableName_in,
                new string[] { "sn", "is_reference" }, null);
            Sql = new SqlDescriptor(staticdesc, this);
        }

        #region ISqlSaveable Members

        public void SqlSave(Npgsql.NpgsqlCommand insertCommand_in)
        {
            insertCommand_in.Parameters.AddWithValue("@sn", Sn);
            insertCommand_in.Parameters.AddWithValue("@is_reference", false);
        }

        #endregion

        #region ISqlDescriptor Members
        public SqlDescriptor Sql    { get; set; }
        #endregion

        public void SetUp(bool macNeeded_in, string sn)
        {
            this.Sql.RowId = SqlLowLevel.INVALID_ROW_ID;//clear old, for create new 
            _MAC_Needed = macNeeded_in;

            Sn = sn;
            MacStr = string.Empty;
        }

        bool _MAC_Needed;
        
        public string MacStr { get; private set; }
        public string MacStrPure { get; private set; }
        public string Sn { get; private set; }

        public void StoreMac(Npgsql.NpgsqlTransaction transaction_in)
        {
            if (_MAC_Needed)
            {
                string mac, macPure;
                MacAddress.GetMacAddress(transaction_in, this.Sn, out mac, out macPure);
                MacStr = mac;
                MacStrPure = macPure;
            }
        }
    }
}
