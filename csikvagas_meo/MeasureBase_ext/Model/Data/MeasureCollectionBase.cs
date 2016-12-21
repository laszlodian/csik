using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using e77.MeasureBase.e77Console;
using e77.MeasureBase.MeasureEnvironment;
using e77.MeasureBase.MeasureEnvironment.IpThermo;
using e77.MeasureBase.Model;
using e77.MeasureBase.Sql;
using Npgsql;

namespace e77.MeasureBase.Model
{
    public abstract class MeasureCollectionBase : MeasureRoot, ISqlMainTable
    {
        static protected MeasureCollectionBase _TheMeasures = null;

        static public MeasureCollectionBase TheMeasures
        {
            get { return _TheMeasures; }
        }

        static public bool IsCreated
        {
            get { return _TheMeasures != null; }
        }

        /// <summary>
        /// Type cast: use 'new' keyword in derivered class for making type conversion possible: e.g.
        ///     new public MeasureMeo TheMeasure { get {return  (MeasureMeo)base.TheMeasure; } }
        /// </summary>
        public MeasureBaseClass TheMeasure
        {
            get
            {
                if (this.Measures == null || this.Measures.Count != 1)
                    throw new InvalidOperationException("get_TheMeasure is valid only if there are exactly one measure.\nIn case of Sql loading if the Measure itself is not SqlLoadable you should create TheMeasure at overriden function of MeasureBase.SqlLoad(). E.g.: 'this.Measures.Add(new MeasureMeo());'");
                return this.Measures[0];
            }
        }

        public MeasureCollectionBase()
        {
            Trace.TraceInformation("MeasureCollectionBase()");
            SwVersion.TheSwVersion.TraceVersion();

            if (_TheMeasures != null)
                throw new InvalidOperationException("MeasureCollection class is singleton. Do not create more.");
            _TheMeasures = this;

            MeasureDate = DateTime.Now;

            if (EnvironmentId.TheEnvironmentId == null)
                throw new Exception("EnvironmentId.TheEnvironmentId has not been created.");
            else if (!EnvironmentId.TheEnvironmentId.Initialized)
                throw new Exception("EnvironmentId.TheEnvironmentId.Init() has not been called.");

            try
            {
                if (EnvironmentId.TheEnvironmentId != null
                    && EnvironmentId.TheEnvironmentId.RoomId.HasValue
                    && EnvironmentId.TheEnvironmentId.RoomId.Value != 0
                    && MeasureConfig.IpThermoConfig != SqlTableDescriptorEnvironmentId.EIpThermoConfig.no_ipthermo)
                        RoomTemperature = IpThermo.GetTemperature(EnvironmentId.TheEnvironmentId.RoomId.Value);
            }
            catch (Exception)
            {
                if (MeasureConfig.IpThermoConfig
                    == SqlTableDescriptorEnvironmentId.EIpThermoConfig.ipthermo_required)
                    throw;
            }
        }

        List<MeasureBaseClass> _Measures = new List<MeasureBaseClass>();

        public List<MeasureBaseClass> Measures
        {
            get { return _Measures; }
            set { _Measures = value; }
        }

        /// <summary>
        /// Only for submeasure
        /// </summary>
        /// <returns></returns>
        protected internal virtual MeasureBaseClass CreateMeasure()
        {
            throw new NotImplementedException(); 
        }

        /// <summary>
        /// creation time of MeasureCollectionBase object
        /// </summary>
        public DateTime MeasureDate { get; protected internal set; }

        public override void CheckSqlConsistency()
        {
            Trace.TraceInformation("MeasureCollectionBase.CheckSqlConsistency()");
            base.CheckSqlConsistency();
            foreach (MeasureBaseClass measure in Measures)
                measure.CheckSqlConsistency();
        }

        public override bool? DoCheckAll()
        {
            Trace.TraceInformation("MeasureCollectionBase.DoCheckAll()");
            bool? bRes = (Measures.Count > 0);
            bool? bTmp;
            foreach (MeasureBaseClass measure in Measures)
            {
                bTmp = measure.DoCheckAll();
                ExtensionMethodsNullableBool.MergeSubResult(ref bRes, bTmp);
            }
            bTmp = base.DoCheckAll();
            ExtensionMethodsNullableBool.MergeSubResult(ref bRes, bTmp);

            ICheckableBase thisICheckable = (this as ICheckableBase);
            if (thisICheckable != null)
            {
                thisICheckable.CheckResult = bRes;
            }

            //result consistency check for SQL sored results:
            if (MeasureCollectionBase.TheMeasures.IsSqlLoaded)
            {
                CheckSqlConsistency();
            }//IsSqlLoaded

            Trace.TraceInformation("MeasureCollectionBase.DoCheckAll() result: {0}", bRes.LocalizedStr());
            return bRes;
        }

        public override bool OnPreProcess()
        {
            Trace.TraceInformation("MeasureCollectionBase.OnPreProcess()");
            bool bRes = base.OnPreProcess();
            foreach (MeasureBaseClass measure in Measures)
                bRes &= measure.OnPreProcess();

            Trace.TraceInformation("MeasureCollectionBase.OnPreProcess() result: {0}", bRes.LocalizedStr());
            return bRes;
        }

        public override bool OnPostProcess()
        {
            Trace.TraceInformation("MeasureCollectionBase.OnPostProcess()");
            bool bRes = base.OnPostProcess();
            foreach (MeasureBaseClass measure in Measures)
                bRes &= measure.OnPostProcess();

            Trace.TraceInformation("MeasureCollectionBase.OnPostProcess() result: {0}", bRes.LocalizedStr());
            return bRes;
        }

        /// <summary>
        /// additional (non-sql hierarchy, not checked, ) SQL clases
        /// </summary>
        //SqlAdditional: public List<ISqlDescriptor> SqlAdditional { get; set; }

        public SqlRowDescriptorHierarchy Sql { get; set; }

        public abstract void SqlSave(Npgsql.NpgsqlCommand insertCommand_in, MeasureRoot measure_in, string collectionKey_in);

        public abstract void SqlLoad(NpgsqlDataReader sqlData_in, MeasureRoot measure_in, out string collectionKey_out);

        virtual public string DetailedTraceInfo
        {
            get
            {
                StringBuilder str = new StringBuilder(2048);
                str.AppendFormat("Detailed trace info: {0}\n", this.ToString());
                str.AppendLine(AllTraceInfo);

                foreach (MeasureRoot meas in _Measures)
                {
                    str.AppendLine(meas.AllTraceInfo);
                }
                return str.ToString();
            }
        }

        public event EventHandler SqlPreLoad;

        internal protected virtual void OnSqlPreLoad()
        {
            Trace.TraceInformation("MeasureCollectionBase.OnSqlPreload()");
            if (SqlPreLoad != null && SqlPreLoad.GetInvocationList().Length > 0)
                SqlPreLoad.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// when the main row loaded (before loading additional tables)
        /// </summary>
        public event EventHandler SqlMainRowLoaded;

        internal protected virtual void OnSqlMainRowLoaded(NpgsqlConnection sqlConn)
        {
            Trace.TraceInformation("MeasureCollectionBase.OnSqlThisRowLoaded()");
            if (SqlMainRowLoaded != null && SqlPreLoad.GetInvocationList().Length > 0)
                SqlMainRowLoaded.Invoke(this, EventArgs.Empty);
        }

        public bool IsSqlLoaded { get; internal set; }

        public event EventHandler SqlLoaded;

        internal protected virtual void OnSqlLoaded()
        {
            Trace.TraceInformation("MeasureCollectionBase.OnSqlLoaded()");
            if (SqlLoaded != null && SqlLoaded.GetInvocationList().Length > 0)
                SqlLoaded.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler SqlStored;

        internal protected virtual void OnSqlStored()
        {
            Trace.TraceInformation("MeasureCollectionBase.OnSqlStored(). RowId = {0}", this.Sql.RowId);
            if (SqlStored != null && SqlStored.GetInvocationList().Length > 0)
                SqlStored.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// valid if (((SqlTableDescriptorMain)Sql.TableDescriptor).IpThermoConfig !=  EIpThermoConfig.no_ipthermo)
        /// Default initialization constructor of MeasureCollectionBase sets up the current temperature (and setup Measure Date).
        /// If Measure date has been modified, it is not enought. Use UpdateRoomTemperature() for
        ///
        /// </summary>
        public float? RoomTemperature { get; internal set; }

        /// <summary>
        /// Queries temperature based on this.MeasureDate.
        /// </summary>
        public void UpdateRoomTemperature()
        {
            try
            {
                Trace.TraceInformation("UpdateRoomTemperature(date = {0}) = {1:F2}", this.MeasureDate, RoomTemperature);
                RoomTemperature = IpThermo.GetTemperature(EnvironmentId.TheEnvironmentId.RoomId.Value, 1, this.MeasureDate);
            }
            catch (SqlNoValueException ex)
            {
                string timeMsg = string.Format("Az IpThermo adatát a {0} időre kéri le. ", this.MeasureDate.ToString("yyyy-MM-dd HH:mm:ss"));
                if (EnvironmentId.TheEnvironmentId.IsConsole_nGui)
                    ConsoleHelper.WarningMessage(timeMsg);
                else
                    Trace.TraceWarning(timeMsg);
                throw ex;
            }
        }

        public override string SN
        {
            get
            {
                if (_Measures.Count == 0)
                    return base.SN;
                else if (_Measures.Count == 1)
                    return TheMeasure.SN;
                else
                {
                    return null; //used for store log too, where more measure with moere sn is normal.
                    //throw new InvalidOperationException("there are more measures, you should obtain SN of the specifix measure, not MeasureCollection.SN");
                }
            }

            set
            {
                Trace.TraceInformation("MeasureCollectionBase.SN = {0}", value);
                if (_Measures.Count == 0)
                    base.SN = value;
                else if (_Measures.Count == 1)
                    TheMeasure.SN = value;
                else
                    throw new InvalidOperationException("There are more measures, you should obtain SN of the specifix measure, not MeasureCollection.SN");
            }
        }

        /// <summary>
        /// Clears all measure specific data
        /// </summary>
        /// <remarks>
        /// Needed when the measurement is restarted during the same run.
        /// </remarks>
        public override void Clear()
        {
            Trace.TraceInformation("MeasureCollectionBase.Clear()");
            base.Clear();

            _Measures.Clear();

            MeasureConfig.TheConfig.Clear();

            MeasureCollectionBase.TheMeasures.MeasureDate = DateTime.Now; //update time (for new measure)
            IsSqlLoaded = false;
        }
    }
}