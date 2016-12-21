using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using e77.MeasureBase;
using System.Diagnostics;

namespace e77.MeasureBase.MeasureEnvironment.IpThermo
{
    static public class IpThermo
    {
        static string _connectionStr = "Server = 77prod; Database = ipthermo; User ID = gyartas; Password = 1234Gyartas";
        const string TemperatureQuery =
                            "select measure.measure_value, measure_type_precisions.precision_value, measure.measure_datetime " +
                            "from measure " +
//for probe_address, but we use probe_id   "LEFT JOIN probe ON measure.probe_id = probe.probe_id " +
                            "LEFT JOIN measure_type_precisions ON  measure.measure_type_code = measure_type_precisions.measure_type_id " +
                            "where measure.probe_id=:roomId and measure.measure_type_code = 1 and " +
                            "measure.measure_datetime >= :fromdate and measure.measure_datetime <= :todate " +
                            "order by measure.measure_datetime desc limit 1 offset 0;";
        const string HumidityQuery =
                           "select measure.measure_value, measure_type_precisions.precision_value, measure.measure_datetime " +
                           "from measure " +          
                           "LEFT JOIN measure_type_precisions ON  measure.measure_type_code = measure_type_precisions.measure_type_id " +
                           "where measure.probe_id=:roomId and measure.measure_type_code = 2 and " +
                           "measure.measure_datetime >= :fromdate and measure.measure_datetime <= :todate " +
                           "order by measure.measure_datetime desc limit 1 offset 0;";

        public static string ConnectionStr
        {
            set { IpThermo._connectionStr = value; }
        }

        /// <summary>
        /// null = uninitialized
        /// </summary>
        static SortedDictionary<int, string> _RoomNames;

        public static SortedDictionary<int, string> RoomNames
        {
            get
            {
                if (_RoomNames != null && _RoomNames.Count() != 0)
                    return _RoomNames;

                _RoomNames = new SortedDictionary<int, string>();

                using (NpgsqlConnection sqlConn = new NpgsqlConnection(_connectionStr))
                {
                    sqlConn.Open();

                    try
                    {
                        using (NpgsqlCommand room = new NpgsqlCommand("select probe_id, probe_name from probe", sqlConn))
                        {
                            using (NpgsqlDataReader dr = room.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                        _RoomNames.Add((int)dr["probe_id"], (string)dr["probe_name"]);

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
                }
                return _RoomNames;
            }            
        }

        public static float GetTemperature(int roomId_in)
        {
            return GetTemperature(roomId_in, 1);
        }

        public static float GetTemperature(int roomId_in, int fromLastXHour_in)
        {
            return GetTemperature(roomId_in, fromLastXHour_in, DateTime.Now);
        }

        /// <summary>
        /// exception: SqlNoValueException if there is no valid value within the specified time
        /// </summary>
        /// <param name="roomId_in">probe_id of probe table of IpThermo database.</param>
        /// <param name="fromLastXHour_in"></param>
        /// <returns></returns>
        public static float GetTemperature(int roomId_in, int fromLastXHour_in, DateTime time_in)
        {
            Trace.TraceInformation("GetTemperature(int roomId_in = {0}, int fromLastXHour_in = {1}, DateTime time_in = {2})",
                roomId_in, fromLastXHour_in, time_in);
            float res = float.MinValue;
            using (NpgsqlConnection sqlConn = new NpgsqlConnection(_connectionStr)) //MeasureConfig.Init function set it if there is 'IpThermoSQLConnectionStr' XML config
            {
                sqlConn.Open();

                try
                {
                    using (NpgsqlCommand temperature = new NpgsqlCommand(TemperatureQuery, sqlConn))
                    {
                        temperature.Parameters.AddWithValue(":roomId", roomId_in);
                        temperature.Parameters.AddWithValue(":fromdate",
                            (time_in - new TimeSpan(fromLastXHour_in, 0, 0)));
                        temperature.Parameters.AddWithValue(":todate", time_in);

                        Trace.TraceInformation("GetTemperature() from {0} to {1}", 
                            (time_in - new TimeSpan(fromLastXHour_in, 0, 0)),
                            time_in );

                        using (NpgsqlDataReader dr = temperature.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                int value = int.Parse((string)dr["measure_value"]);
                                int precision = (int)dr["precision_value"];

                                res = (float)value / precision;
                                dr.Close();
                            }
                            else
                            {
                                dr.Close();

                                throw new SqlNoValueException(
                                    string.Format("RoomID: {0}", roomId_in), time_in, fromLastXHour_in);
                            }
                        }
                    }
                }
                finally
                {
                    sqlConn.Close();
                }
            }
            
            return res;
        }
        public static float GetHumidity(int roomId_in)
        {
            return GetHumidity(roomId_in, 1);
        }

        public static float GetHumidity(int roomId_in, int fromLastXHour_in)
        {
            return GetHumidity(roomId_in, fromLastXHour_in, DateTime.Now);
        }

        /// <summary>
        /// exception: SqlNoValueException if there is no valid value within the specified time
        /// </summary>
        /// <param name="roomId_in">probe_id of probe table of IpThermo database.</param>
        /// <param name="fromLastXHour_in"></param>
        /// <returns></returns>
        public static float GetHumidity(int roomId_in, int fromLastXHour_in, DateTime time_in)
        {
            Trace.TraceInformation("GetHumidity(int roomId_in = {0}, int fromLastXHour_in = {1}, DateTime time_in = {2})",
                roomId_in, fromLastXHour_in, time_in);
            float res = float.MinValue;
            using (NpgsqlConnection sqlConn = new NpgsqlConnection(_connectionStr)) //MeasureConfig.Init function set it if there is 'IpThermoSQLConnectionStr' XML config
            {
                sqlConn.Open();

                try
                {
                    using (NpgsqlCommand humidity = new NpgsqlCommand(HumidityQuery, sqlConn))
                    {
                        humidity.Parameters.AddWithValue(":roomId", roomId_in);
                        humidity.Parameters.AddWithValue(":fromdate",
                            (time_in - new TimeSpan(fromLastXHour_in, 0, 0)));
                        humidity.Parameters.AddWithValue(":todate", time_in);

                        Trace.TraceInformation("GetHumidity() from {0} to {1}",
                            (time_in - new TimeSpan(fromLastXHour_in, 0, 0)),
                            time_in);

                        using (NpgsqlDataReader dr = humidity.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                int value = int.Parse((string)dr["measure_value"]);
                                int precision = (int)dr["precision_value"];

                                res = (float)value / precision;
                                dr.Close();
                            }
                            else
                            {
                                dr.Close();

                                throw new SqlNoValueException(
                                    string.Format("RoomID: {0}", roomId_in), time_in, fromLastXHour_in);
                            }
                        }
                    }
                }
                finally
                {
                    sqlConn.Close();
                }
            }

            return res;
        }        
    }
}
