using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using e77.MeasureBase;
using e77.MeasureBase.e77Console;
using e77.MeasureBase.GUI;
using e77.MeasureBase.MeasureEnvironment;
using e77.MeasureBase.MeasureEnvironment.IpThermo;
using e77.MeasureBase.Model;
using Npgsql;

namespace e77.MeasureBase.Sql
{
    //public class ArgumentException : Exception
    //{
    //    public ArgumentException()
    //    { }

    //    public ArgumentException(string message_in)
    //        :base(message_in)
    //    {
    //    }

    //    public ArgumentException(string message_in, Exception innerException_in)
    //        : base(message_in, innerException_in)
    //    {
    //    }

    //    public ArgumentException(SerializationInfo info, StreamingContext context)
    //        : base(info, context)
    //    {
    //    }
    //}

    /// <summary>
    /// singletion for EnvironmentId handling : User( LoginName, AccessRight, FullName); Workplace (ComputerName, RoomId), SW version
    /// </summary>
    abstract public class EnvironmentId
    {
        #region Constructors and Initializers

        static EnvironmentId()
        {
            ComputerName = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
            User = new e77User();
        }

        public EnvironmentId(bool console_nGui_in, EDefaultArgs supportedArgs_in)
            : this(console_nGui_in, supportedArgs_in, false)
        {
        }

        public EnvironmentId(bool console_nGui_in, EDefaultArgs supportedArgs_in, bool isMeasureContinuable_in)
        {
            if (TheEnvironmentId != null)
                throw new MeasureBaseException("Singleton");
            TheEnvironmentId = this;

            SupportedArgs = supportedArgs_in;
            IsConsole_nGui = console_nGui_in;
            IsMeasureContinuable = isMeasureContinuable_in;

            ArgumentsFlag = new Dictionary<string, bool>();
            ArgumentsParam = new Dictionary<string, string>();
        }

        internal bool Initialized { get; private set; }

        /// <summary>
        ///     This method initialize the Environment base class and process the arguments
        /// </summary>
        /// <returns>Only Help has been requested, application should be terminated.</returns>
        public virtual bool Init()
        {
            Initialized = true;

            if (GUI.WorkplaceSetupForm.RoomId == 0 &&
                MeasureConfig.IpThermoConfig == SqlTableDescriptorEnvironmentId.EIpThermoConfig.ipthermo_required)
            {
                using (GUI.WorkplaceSetupForm form = new e77.MeasureBase.GUI.WorkplaceSetupForm())
                {
                    form.ShowDialog();
                }
            }

            TheEnvironmentId.RoomId = GUI.WorkplaceSetupForm.RoomId;

            return InitArgs();
        }

        #endregion Constructors and Initializers

        #region argument handling

        [Flags]
        public enum EDefaultArgs
        {
            Load = 1,
            Detailed = 2,
            TestDb = 4
        }

        protected virtual EDefaultArgs SupportedArgs { get; set; }

        public long LoadMeasId = SqlLowLevel.INVALID_ROW_ID;       // define message id;
        public static bool ForceTestDb;

        /// <summary>
        /// Only for console
        /// </summary>
        public static bool Detailed;

        /// <summary>
        /// Key: required/optional flag args, use lower case
        /// </summary>
        public static Dictionary<string, bool> ArgumentsFlag { get; private set; }

        bool isMeasureContinued_nLoaded;

        /// <summary>
        /// Only for continuable measures, see help of constructor.
        /// </summary>
        public bool IsMeasureContinued_nLoaded
        {
            set
            {
                if ((SupportedArgs & EDefaultArgs.Load) != 0)
                    throw new Exception();
                else if (!IsMeasureContinuable)
                    throw new Exception("The Measure is not continuable, do not use this prop.");

                isMeasureContinued_nLoaded = value;
            }

            get
            {
                if ((SupportedArgs & EDefaultArgs.Load) != 0)
                    throw new Exception();
                else if (!IsMeasureContinuable)
                    throw new Exception("The Measure is not continuable, do not use this prop.");

                return isMeasureContinued_nLoaded;
            }
        }

        /// <summary>
        /// Key: required/optional param args (e.g.: '/Fruit:aple'), use lower case
        /// </summary>
        public static Dictionary<string, string> ArgumentsParam { get; private set; }

        /// <summary>
        /// Get a value form ArgumentsParam dictionary or null if it isn't there
        /// </summary>
        /// <param name="argKey_in">Key value</param>
        /// <returns>Value or Null</returns>
        public static string getArgumentsParam(string argKey_in)
        {
            string ret = null;
            if (ArgumentsParam.ContainsKey(argKey_in))
                ret = ArgumentsParam[argKey_in];
            return ret;
        }

        /// <summary>
        /// Get a value form ArgumentsParam dictionary or null if it isn't there
        /// </summary>
        /// <param name="argKey_in">Key value</param>
        /// <returns>Value or Null</returns>
        public static int getArgumentsParamInt(string argKey_in)
        {
            int ret = int.MinValue;
            if (ArgumentsParam.ContainsKey(argKey_in))
            {
                string s = getArgumentsParam(argKey_in);
                int.TryParse(s, out ret);
            }
            return ret;
        }

        internal bool IsLoadFormRequired { get; private set; }

        protected virtual List<string> HelpString
        {
            get
            {
                List<string> res = new List<string>();

                if ((SupportedArgs & EDefaultArgs.Load) != 0)
                {
                    if (IsMeasureContinuable)
                        res.Add(Properties.Resources.ARG_HELP_SQL_CONTINUABLE);
                    else
                        res.Add(Properties.Resources.ARG_HELP_SQL);
                }

                if ((SupportedArgs & EDefaultArgs.TestDb) != 0)
                    res.Add(Properties.Resources.ARG_HELP_TEST_DB);

                if ((SupportedArgs & EDefaultArgs.Detailed) != 0)
                    res.Add(Properties.Resources.ARG_HELP_DETAILED);

                res.Add(string.Format(Properties.Resources.ARG_HELP_HELP, ARGS_HELP));

                for (int i = 0; i < res.Count(); i++)
                {
                    res[i] = res[i].Replace("\\n", "\n");
                    res[i] = res[i].Replace("\\t", "\t");
                }
                return res;
            }
        }

        readonly string[] ARGS_HELP = new string[] { "/?", "/s", "/h", "/help" };

        /// <summary>
        /// Command line parameters processing
        /// </summary>
        /// <returns>Only Help has been requested, application should be terminated.</returns>
        private bool InitArgs()
        {
            string[] consoleArguments = System.Environment.GetCommandLineArgs();
            string[] consoleArgumentsLower = System.Environment.GetCommandLineArgs().ToLower();

            Trace.TraceInformation("Process params: {0}", consoleArguments.ItemsToString());

            for (int i = 1; i < consoleArgumentsLower.Count(); i++)
            {
                if ((consoleArguments[i].Contains(':')) || (consoleArguments[i].Contains('=')))
                {
                    string key;
                    string value;
                    if (consoleArguments[i].Contains(':'))
                        consoleArguments[i].Cut2Strings(':', out key, out value, false, false, false, true, true);
                    else
                        consoleArguments[i].Cut2Strings('=', out key, out value, false, false, false, true, true);

                    string keyLowerCase = key.ToLower();

                    if (((SupportedArgs & EDefaultArgs.Load) != 0)
                        && ((!IsMeasureContinuable && keyLowerCase == "/sql")
                                || IsMeasureContinuable && (keyLowerCase == "/sql_load" || keyLowerCase == "/sql_continue")))
                    {
                        if (IsMeasureContinuable)
                            isMeasureContinued_nLoaded = keyLowerCase == "/sql_continue";

                        ParseSqlId(value);
                    }
                    else
                    {
                        if (ArgumentsParam.ContainsKey(keyLowerCase))
                        {
                            ArgumentsParam[keyLowerCase] = value;
                        }                          
                        else
                            throw new ArgumentException(string.Format("Invalid argument: '{0}'. Possible arguments: {1}",
                                consoleArguments[i], ArgumentsParam.Keys.ItemsToString()), consoleArgumentsLower[i]);
                        
                    }
                }
                else
                {
                    if (ARGS_HELP.Contains(consoleArgumentsLower[i]))  ///   /?  /h
                    {
                        ShowHelp();
                        return true;
                    }
                    else if (((SupportedArgs & EDefaultArgs.TestDb) != 0)
                        && consoleArgumentsLower[i] == "/tesztdb")
                    {
                        ForceTestDb = true;
                    }
                    else if (((SupportedArgs & EDefaultArgs.Detailed) != 0)
                        && consoleArgumentsLower[i] == "/detailed")
                    {
                        Detailed = true;
                    }
                    else
                    {
                        if (ArgumentsFlag.ContainsKey(consoleArgumentsLower[i]))
                            ArgumentsFlag[consoleArgumentsLower[i]] = true;
                     
                        else
                            throw new ArgumentException(string.Format("Invalid argument: '{0}'. Possible flag arguments: {1} {2}",
                                consoleArgumentsLower[i], ArgumentsFlag.Keys.ItemsToString(), ARGS_HELP.ItemsToString()), consoleArgumentsLower[i]);
                     
                    }
                }
            }

            //if (SqlTableDescriptorsBase.TheDescriptor != null
            //    && SqlTableDescriptorsBase.TheDescriptor is ISqlGlobal)
            //{
            //    mainTableName = SqlTableDescriptorsBase.MainTable.TableName;
            //}

            List<string> keys = new List<string>(ArgumentsParam.Keys);
            foreach (string key in keys)
                if (ArgumentsParam[key] == null)
                    ArgumentsParam.Remove(key);

            keys = new List<string>(ArgumentsFlag.Keys);
            foreach (string key in keys)
                if (ArgumentsFlag[key] == false)
                    ArgumentsFlag.Remove(key);

            return false;
        }

        internal void ShowLoadForm()
        {
            if (SqlTableDescriptorsBase.TheDescriptor != null
                && SqlTableDescriptorsBase.TheDescriptor is ISqlGlobal)
            {
                string mainTableName = SqlTableDescriptorsBase.MainTable.TableName;

                using (LoadMeasureSelectorForm form = new LoadMeasureSelectorForm(mainTableName))
                {
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        LoadMeasId = form.SqlId;
                }
            }
        }

        private void ParseSqlId(string value_in)
        {
            if (value_in.ToLower() == "x")
                IsLoadFormRequired = true;
            else
                LoadMeasId = long.Parse(value_in);
        }

        private void ShowHelp()
        {
            StringBuilder helpMsg = new StringBuilder();
            foreach (string s in HelpString)
                helpMsg.AppendFormat("{0}\n", s);

            if (IsConsole_nGui)
                Console.Write(helpMsg.ToString());
            else
                MessageBox.Show(helpMsg.ToString(),
                    string.Format("{0} - Program argumentumok", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            Thread.Sleep(2000);
        }

        #endregion argument handling

        /// <summary>
        /// All AccessRight for this Application
        /// First item is the root access right (execute) for the Applicaion (checking is not implemented now)
        /// </summary>
        protected internal abstract string[] ALL_ACCESS_RIGHTS { get; }

        public bool IsConsole_nGui { get; private set; }

        public bool IsMeasureContinuable { get; private set; }

        public static EnvironmentId TheEnvironmentId { get; set; }

        int? _roomId;

        public int? RoomId
        {
            get { return _roomId; }
            internal set
            {
                _roomId = value;

                if (MeasureCollectionBase.TheMeasures != null && !MeasureCollectionBase.TheMeasures.IsSqlLoaded)
                {
                    if (EnvironmentId.TheEnvironmentId.RoomId != 0)
                    {
                        MeasureCollectionBase.TheMeasures.RoomTemperature = IpThermo.GetTemperature(EnvironmentId.TheEnvironmentId.RoomId.Value);
                    }
                    else
                    {
                        MeasureCollectionBase.TheMeasures.RoomTemperature = null;
                    }
                }
            }
        }

        static public string ComputerName { get; internal set; }

        static public e77User User { get; internal set; }
    }
}