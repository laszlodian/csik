using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using e77.MeasureBase.Sql;

namespace e77.MeasureBase.MeasureEnvironment
{
    /*todo_fgy obsolete (IpThermo Mapping Table)-> removed
    public static class Rooms
    {  
        const string MAPPING_TABLE_NAME = "global_rooms";
        const string COLUMN_NAME_IP_THERMO_WORKPLACE = "ip_thermo_workplace_id";
        const string COLUMN_NAME_NAME = "name";

        //Note1: these dictionaries must be consistent: should contains same keys collection (see 'Note1' in this file)
        static Dictionary<int, int?> _IpThermoMappingTable; //null == not initialized
        static SortedDictionary<int, string> _RoomNames = new SortedDictionary<int, string>();
        static Dictionary<int, float?> _TemperaturesCache = new Dictionary<int, float?>();

        /// <summary>
        /// if there is no IpThermo at that room, temperature will returns with INVALID_TEMPERATURE (-300)
        /// </summary>
        static public bool AcceptUnknownTemperature { get; set; }

        private static Dictionary<int, int?> IpThermoMappingTable
        {
            get
            {
                InitRoomsTableIfNeeded();
                
                return _IpThermoMappingTable;
            }
        }

        private static void InitializeMappingTable()
        {
            _IpThermoMappingTable = new Dictionary<int, int?>();
            using (NpgsqlConnection sqlConn = new NpgsqlConnection(MeasureConfig.TheConfig.SqlConnectionStr))
            {
                sqlConn.Open();
                try
                {
                    using (NpgsqlCommand room = new NpgsqlCommand(string.Format(
                        "select {0}, {1}, {2} from {3}",
                            SqlLowLevel.COLUMN_NAME_ID,
                            COLUMN_NAME_IP_THERMO_WORKPLACE, 
                            COLUMN_NAME_NAME,
                            MAPPING_TABLE_NAME),
                        sqlConn))
                    {
                        using (NpgsqlDataReader dr = room.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    //Note1: fill 3 dictionaries at same time, with same keys:

                                    int roomId = (int)dr[SqlLowLevel.COLUMN_NAME_ID];
                                    if (dr.IsDBNull(dr.GetOrdinal(COLUMN_NAME_IP_THERMO_WORKPLACE)))
                                        _IpThermoMappingTable.Add(roomId, null);
                                    else
                                        _IpThermoMappingTable.Add(roomId, (int)dr[COLUMN_NAME_IP_THERMO_WORKPLACE]);

                                    _RoomNames.Add(roomId,
                                        (string)dr[COLUMN_NAME_NAME]);
                                }
                                dr.Close();
                            }
                            else
                            {
                                dr.Close();
                                throw new SqlNoValueException("Rooms at probe table");
                            }
                        }
                    }
                }
                finally
                {
                    sqlConn.Close();
                }
            }//using (NpgsqlConnection sqlConn
        }

        public static bool HasIpThermoRoomId(int fixRoomId_in)
        {
            InitRoomsTableIfNeeded();

            return IpThermoMappingTable[fixRoomId_in].HasValue;
        }

        public static int GetIpThermoRoomId(int fixRoomId_in)
        {
            InitRoomsTableIfNeeded();

            return _IpThermoMappingTable[fixRoomId_in].Value;
        }

        public static float? GetIpThermoValue(int fixRoomId_in)
        {
            if(!_TemperaturesCache.ContainsKey(fixRoomId_in))
            {
                if (AcceptUnknownTemperature && _IpThermoMappingTable[fixRoomId_in] == null)
                    _TemperaturesCache[fixRoomId_in] = null;
                else
                    _TemperaturesCache[fixRoomId_in] = MeasureEnvironment.IpThermo.IpThermo.GetTemperature(
                        Rooms.GetIpThermoRoomId(fixRoomId_in), 3);
            }

            return _TemperaturesCache[fixRoomId_in];
        }

        public static string GetRoomName(int id_in)
        {
            if (_IpThermoMappingTable == null) //check initialized
                InitializeMappingTable(); //do init

            return _RoomNames[id_in];
        }

        public static string GetRoomNameSafe(int id_in)
        {
            return IpThermoMappingTable.ContainsKey(id_in) ?
                _RoomNames[id_in]: string.Empty;
        }

        public static bool ContainsRoomId(int roomId_in)
        {
            InitRoomsTableIfNeeded();

            return _IpThermoMappingTable.ContainsKey(roomId_in);
        }

        /// <summary>
        /// Note: please do not modify the returned collection.
        /// Key: room_address
        /// Value: probe_name
        /// </summary>
        public static SortedDictionary<int, string> RoomNames
        {
            get 
            {
                InitRoomsTableIfNeeded();

                return _RoomNames; 
            }
        }

        private static void InitRoomsTableIfNeeded()
        {
            if (_IpThermoMappingTable == null) //check initialized
                InitializeMappingTable(); //do init
        }

    }*/
}
