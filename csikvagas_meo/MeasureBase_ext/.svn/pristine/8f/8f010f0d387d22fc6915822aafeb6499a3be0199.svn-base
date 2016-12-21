using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using e77.MeasureBase.Sql;

namespace e77.MeasureBase.Sql
{
    abstract public class SwVersion : ISqlAdditionalObj
    {
        private enum SqlColumns { sw_version }

        public SwVersion()
        {
            if (TheSwVersion != null)
                throw new Exception("Singleton");

            TheSwVersion = this;

            TheSwVersion.SqlVersionCurrent = 0;
        }

        bool _versionTraced;

        public void TraceVersion()
        {
            _versionTraced = true;
            Trace.TraceInformation("Current Sw Version: {0}", SwVersion.TheSwVersion.SwVersionCurrent);
        }

        ~SwVersion()
        {
            if (!_versionTraced)
                TraceVersion();
        }

        public static SwVersion TheSwVersion { get; internal set; }

        public string SwVersionCurrent
        {
            get
            {
                return SwVersions;
            }
        }

        public string SwVersionSqlStored { get; set; }

        /// <summary>
        /// increased in case of SQL stucture modification
        /// </summary>
        public int SqlVersionCurrent { get; internal set; }

        public int SqlVersionSqlStored { get; set; }

        #region ISqlAdditionalObj Members

        public Dictionary<string, object> GetSqlData()
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            res.Add(SqlColumns.sw_version.ToString(), SwVersionCurrent);
            return res;
        }

        public void SqlLoad(Npgsql.NpgsqlDataReader sqlData_in)
        {
            SwVersionSqlStored = (string)sqlData_in[SqlColumns.sw_version.ToString()];
        }

        /// <summary>
        /// retruns string.Empty if consiostent
        /// </summary>
        public string SwInconsistencyErrorString
        {
            get
            {
                StringBuilder res = new StringBuilder();

                if (SwVersionCurrent != SwVersionSqlStored)
                {
                    res.AppendFormat("A mérést más verziószámú program készítette. Ezzel a verzióval nem garantált a sikeres előállítás.\n");
                    res.AppendFormat("Futó program verziója:    {0}\n", SwVersionCurrent);
                    res.AppendFormat("Készítő program verziója: {0}", SwVersionSqlStored);
                }

                return res.ToString();
            }
        }

        #endregion ISqlAdditionalObj Members

        /// <summary>
        /// Override this function at all assemply, by this code:
        ///     return string.Format("{0}:{1}/{2}",
        ///             System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
        ///             System.Reflection.Assembly.GetExecutingAssembly().GetName().Version,
        ///             base.SwVersions);
        /// </summary>
        public virtual string SwVersions
        {
            get
            {
                return string.Format("{0}:{1}",
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            }
        }
    }
}