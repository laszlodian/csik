using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using e77.MeasureBase.Sql;
using System.Windows;

namespace e77.MeasureBase.MeterDevices
{
    public static class MeterDB
    {
        public const int MAX_SN_LENGHT = 12;
        public const int MIN_SN_LENGHT = 6;
        public static readonly DateTime VALID_DATE_INVALID = new DateTime(9999, 12, 31);

        public const int ValidationExpiringWarningDays = 5;
        public static bool CheckMeter(string sn_in, out string stateStr_out, bool validationExpiringWarningEnabled_in)
        {
            stateStr_out = string.Empty;

            if (sn_in == null || sn_in.Length < MIN_SN_LENGHT)
                stateStr_out = string.Format("Műszerazonosító hossza legalább {0} karakter.", MIN_SN_LENGHT);
            else if (sn_in.Length > MAX_SN_LENGHT)
                stateStr_out = string.Format("Műszerazonosító hossza legfeljebb {0} karakter.", MAX_SN_LENGHT);
            else
            {   //query
                using (NpgsqlConnection con = new NpgsqlConnection("Server = 77i; Database = global; User ID = global; Password = 2234Global"))
                {
                    con.Open();
                    object validDateObj = SqlMiddleLevel.QueryScalar(con, string.Format("SELECT ervenyes FROM meroeszkozok WHERE leltariszam = '{0}';", sn_in));
                    con.Close();

                    if (validDateObj == null)
                        stateStr_out = "Ez a műszer nem található az adatbázisban";
                    else
                    {
                        DateTime validDate = (DateTime)validDateObj;
                        if (validDate == VALID_DATE_INVALID)
                            stateStr_out = "Ez a műszer nem kalibrálható, igy nem használható fel.";
                        else if (validDate < DateTime.Now)
                            stateStr_out = string.Format("A műszer kalibrációja {0} lejárt.", validDate.ToShortDateString());
                        else if (validationExpiringWarningEnabled_in
                            && (validDate + new TimeSpan(ValidationExpiringWarningDays, 0, 0, 0) < DateTime.Now))
                            MessageBox.Show(
                                string.Format("A műszer kalibrációja {0} lejár.", validDate.ToShortDateString()),
                                "Figyelmeztetés lejáró kalibrácóra", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }

            if (stateStr_out == string.Empty)
            {
                stateStr_out = "OK";
                return true;
            }
            return false;
        }
    }


}
