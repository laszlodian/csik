using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.Sql;
using Npgsql;
using e77.MeasureBase;
using e77.MeasureBase.Model;
using System.Diagnostics;

namespace e77.MeasureBase
{
    public class MacAddress
    {
        public const string TABLE_NAME = "global_mac_addresses";
        const string SEQUENCE_NAME = "global_mac_addresses_pk_id_seq";
        const int MAC_LENGHT = 6;   //bytes

        public static long GetMacAddress( NpgsqlTransaction transaction_in, string forSn_in, out string macAddress_out, out string macAddressPure_out)
        {
            long res = GetMacAddress(transaction_in.Connection, forSn_in, out macAddress_out, out macAddressPure_out);

            SqlLog.TheLog.SaveLog(transaction_in, "MAC created", TABLE_NAME, res, forSn_in);
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection_in"></param>
        /// <param name="forSn_in"></param>
        /// <param name="macAddress_out">'-' separated</param>
        /// <param name="macAddressPure_out">without '-' separator</param>
        /// <returns>MAC as integer == pk_id </returns>
        public static long GetMacAddress(NpgsqlConnection connection_in, string forSn_in, out string macAddress_out, out string macAddressPure_out)
        {
            ulong nextMAC;
            using (NpgsqlCommand query = new NpgsqlCommand(
                    string.Format("SELECT NEXTVAL('{0}')", SEQUENCE_NAME), connection_in))
            {
                nextMAC = (ulong)((long)query.ExecuteScalar());
            }

            //create mac str:
            ulong nextMacTmp = nextMAC;
            byte[] address = new byte[MAC_LENGHT];
            for(int i = MAC_LENGHT - 1; i >= 0 ; i--)
            {
                address[i] = ((byte)(nextMacTmp & 0xff));
                nextMacTmp >>= 8;
            }

            StringBuilder macStr = new StringBuilder();
            StringBuilder macStrPure = new StringBuilder();
            foreach (byte b in address)
            {
                if (macStr.Length > 0)
                    macStr.Append('-');

                macStr.Append(b.ToString("x2"));
                macStrPure.Append(b.ToString("x2"));
            }

            macAddress_out = macStr.ToString();
            macAddressPure_out = macStrPure.ToString();

            using (NpgsqlCommand insertQuery = new NpgsqlCommand(
                   string.Format("INSERT INTO {0} (pk_id, mac_address, mac_address_pure, sn) VALUES ({1}, '{2}', '{3}', '{4}')",
                    TABLE_NAME, nextMAC, macAddress_out,  macAddressPure_out, forSn_in), connection_in))
            {
                insertQuery.ExecuteNonQuery();
            }
            
            Trace.TraceInformation(string.Format("MAC Address '{0}' created", macAddress_out));
            return (long)nextMAC;
        }
    }
}
