using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using e77.MeasureBase.Configuration;
using e77.MeasureBase.e77Console;
using e77.MeasureBase.Helpers;
using e77.MeasureBase.MeasureEnvironment.IpThermo;
using e77.MeasureBase.Model;
using e77.MeasureBase.Sql;
using e77.MeasureBase.Beep;
using System.IO;
using System.Reflection;
using Npgsql;
using System.Data.Common;
using e77.MeasureBase.GUI;

namespace e77.MeasureBase
{
	/// <summary>
	/// Base class for configuration handling. This base class supports:
	/// -Properties.Settings handling:
    /// (specified columns by ISqlGlobal.SqlParameterColumns are stored/loaded into additional unique table of the measurement main sql table.
	/// -SQL connection to DB, Test/Resleade DB handling
	/// -optional Room ID
	/// </summary>
	public class MeasureConfig : ISqlAdditionalObj
	{
		static MeasureConfig()
		{
			IpThermoConfig = SqlTableDescriptorEnvironmentId.EIpThermoConfig.no_ipthermo;
		}

        private enum EDataBases
        {
            UD2,
            UA3,
            LFR

        }
		private enum EDefaultConfigs
		{
			
			DBSelector,
             UD2ReleaseConnectionString,
            UD2DebugConnectionString,            
			DBUseRelease,
            LFRDebugConnectionString,
            LFRReleaseConnectionString,
            UA3DebugConnectionString,
            UA3ReleaseConnectionString,
            IPThermoConnectionString
			//DBConnection, //used if there is no Debug/Release DB separation.
		};

		public MeasureConfig()
		{      
			if (TheConfig != null)
				throw new InvalidOperationException("MeasureConfig class is singleton. Do not create more.");
			TheConfig = this;
		}

		public static MeasureConfig TheConfig { get; private set; }

		public static SqlTableDescriptorEnvironmentId.EIpThermoConfig IpThermoConfig
		{
			get;
			set;
		}

		public static String DefaultIniFilePartialPath
		{
			get;
			protected set;
		}

		private System.Configuration.ApplicationSettingsBase[] _appConfigs;

		private System.Configuration.ApplicationSettingsBase AppConfigMain
		{
			get { return _appConfigs[0]; }
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="config_in">First item must be the main config (containing Sql Connection strings and RoomId). Following used only for SQL parameter table.</param>
		public virtual void Init(System.Configuration.ApplicationSettingsBase[] config_in)
        {
            if (EnvironmentId.TheEnvironmentId == null)
                throw new Exception("EnvironmentId.TheEnvironmentId has not been created.");
            else if (!EnvironmentId.TheEnvironmentId.Initialized)
                throw new Exception("EnvironmentId.TheEnvironmentId.Init() has not been called.");

            _appConfigs = config_in;     

            #region UD2 Database Connection String Configuration
            if ((GetConfigValueOf("DBSelector").ToString() == "UD2"))
            {

                UsedDatabase = EDataBases.UD2;


                if (EnvironmentId.ForceTestDb || !Convert.ToBoolean(GetConfigValueOf(EDefaultConfigs.DBUseRelease.ToString())))
                {

                    SqlConnectionStr = e77.MeasureBase.Sql.SqlLowLevel.DecryptPw((string)AppConfigMain[EDefaultConfigs.UD2DebugConnectionString.ToString()]);
                    SqlIsReleaseDb = false;
                    Trace.TraceInformation("Test DB");
                    Trace.TraceInformation("SQL connection: {0}", SqlLowLevel.RemovePasswordFromSqlConnectionString(SqlConnectionStr));
                    WinFormTestDbControl = new TestDbControl();
                    WPF_TestDBControl = new GUI.WPF.WpfTestDbControl();


                    if (EnvironmentId.TheEnvironmentId.IsConsole_nGui)
                    {
                        ConsoleColor y = ConsoleColor.Yellow;
                        ConsoleColor b = ConsoleColor.Black;

                        Console.BackgroundColor = y; Console.Write("              "); Console.BackgroundColor = b; Console.WriteLine();
                        Console.BackgroundColor = y; Console.Write("   ");
                        Console.BackgroundColor = b;
                        ConsoleHelper.WarningMessage("Test DB");
                        Console.BackgroundColor = y; Console.Write("              "); Console.BackgroundColor = b; Console.WriteLine();
                    }

                }
                else if (!EnvironmentId.ForceTestDb && Convert.ToBoolean(GetConfigValueOf(EDefaultConfigs.DBUseRelease.ToString())))
                {
                    SqlConnectionStr = e77.MeasureBase.Sql.SqlLowLevel.DecryptPw((string)AppConfigMain[EDefaultConfigs.UD2ReleaseConnectionString.ToString()]);
                    SqlIsReleaseDb = true;
                    Trace.TraceInformation("Release DB");
                    Trace.TraceInformation("SQL connection: {0}", SqlLowLevel.RemovePasswordFromSqlConnectionString(SqlConnectionStr));
                    WinFormTestDbControl = null;
                    WPF_TestDBControl = null;

                    if (EnvironmentId.TheEnvironmentId.IsConsole_nGui)
                    {
                        ConsoleColor y = ConsoleColor.Yellow;
                        ConsoleColor b = ConsoleColor.Black;

                        Console.BackgroundColor = y; Console.Write("              "); Console.BackgroundColor = b; Console.WriteLine();
                        Console.BackgroundColor = y; Console.Write("   ");
                        Console.BackgroundColor = b;
                        ConsoleHelper.WarningMessage("Release DB");
                        Console.BackgroundColor = y; Console.Write("              "); Console.BackgroundColor = b; Console.WriteLine();
                    }
                }


            } 
            #endregion
            #region LFR Dastabase Connection String Configuration
            else if (GetConfigValueOf("DBSelector").ToString() == "LFR")
           
            {        
                UsedDatabase = EDataBases.LFR;



                if (EnvironmentId.ForceTestDb || !Convert.ToBoolean(GetConfigValueOf(EDefaultConfigs.DBUseRelease.ToString())))
                {

                    SqlConnectionStr = e77.MeasureBase.Sql.SqlLowLevel.DecryptPw((string)AppConfigMain[EDefaultConfigs.LFRDebugConnectionString.ToString()]);
                    SqlIsReleaseDb = false;
                    Trace.TraceInformation("Test DB");
                    Trace.TraceInformation("SQL connection: {0}", SqlLowLevel.RemovePasswordFromSqlConnectionString(SqlConnectionStr));
                    WinFormTestDbControl = new TestDbControl();
                    WPF_TestDBControl = new GUI.WPF.WpfTestDbControl();

                    if (EnvironmentId.TheEnvironmentId.IsConsole_nGui)
                    {
                        ConsoleColor y = ConsoleColor.Yellow;
                        ConsoleColor b = ConsoleColor.Black;

                        Console.BackgroundColor = y; Console.Write("              "); Console.BackgroundColor = b; Console.WriteLine();
                        Console.BackgroundColor = y; Console.Write("   ");
                        Console.BackgroundColor = b;
                        ConsoleHelper.WarningMessage("Test DB");
                        Console.BackgroundColor = y; Console.Write("              "); Console.BackgroundColor = b; Console.WriteLine();
                    }

                }
                else if (!EnvironmentId.ForceTestDb && Convert.ToBoolean(GetConfigValueOf(EDefaultConfigs.DBUseRelease.ToString())))
                {
                    SqlConnectionStr = e77.MeasureBase.Sql.SqlLowLevel.DecryptPw((string)AppConfigMain[EDefaultConfigs.LFRReleaseConnectionString.ToString()]);
                    SqlIsReleaseDb = true;
                    Trace.TraceInformation("Release DB");
                    Trace.TraceInformation("SQL connection: {0}", SqlLowLevel.RemovePasswordFromSqlConnectionString(SqlConnectionStr));
                    WinFormTestDbControl = null;
                    WPF_TestDBControl = null;

                    if (EnvironmentId.TheEnvironmentId.IsConsole_nGui)
                    {
                        ConsoleColor y = ConsoleColor.Yellow;
                        ConsoleColor b = ConsoleColor.Black;

                        Console.BackgroundColor = y; Console.Write("              "); Console.BackgroundColor = b; Console.WriteLine();
                        Console.BackgroundColor = y; Console.Write("   ");
                        Console.BackgroundColor = b;
                        ConsoleHelper.WarningMessage("Release DB");
                        Console.BackgroundColor = y; Console.Write("              "); Console.BackgroundColor = b; Console.WriteLine();
                    }
                }
            } 
            #endregion
            #region UA3 Database connection String Configuration
            else if (GetConfigValueOf("DBSelector").ToString() == "UA3")
            {

                UsedDatabase = EDataBases.UA3;

                if (EnvironmentId.ForceTestDb || !Convert.ToBoolean(GetConfigValueOf(EDefaultConfigs.DBUseRelease.ToString())))
                {

                    SqlConnectionStr = e77.MeasureBase.Sql.SqlLowLevel.DecryptPw((string)AppConfigMain[EDefaultConfigs.UA3DebugConnectionString.ToString()]);
                    SqlIsReleaseDb = false;
                    Trace.TraceInformation("Test DB");
                    Trace.TraceInformation("SQL connection: {0}", SqlLowLevel.RemovePasswordFromSqlConnectionString(SqlConnectionStr));
                    WinFormTestDbControl = new TestDbControl();
                    WPF_TestDBControl = new GUI.WPF.WpfTestDbControl();


                    if (EnvironmentId.TheEnvironmentId.IsConsole_nGui)
                    {
                        ConsoleColor y = ConsoleColor.Yellow;
                        ConsoleColor b = ConsoleColor.Black;

                        Console.BackgroundColor = y; Console.Write("              "); Console.BackgroundColor = b; Console.WriteLine();
                        Console.BackgroundColor = y; Console.Write("   ");
                        Console.BackgroundColor = b;
                        ConsoleHelper.WarningMessage("Test DB");
                        Console.BackgroundColor = y; Console.Write("              "); Console.BackgroundColor = b; Console.WriteLine();
                    }

                }
                else if (!EnvironmentId.ForceTestDb && Convert.ToBoolean(GetConfigValueOf(EDefaultConfigs.DBUseRelease.ToString())))
                {
                    SqlConnectionStr = e77.MeasureBase.Sql.SqlLowLevel.DecryptPw((string)AppConfigMain[EDefaultConfigs.UA3ReleaseConnectionString.ToString()]);
                    SqlIsReleaseDb = true;
                    Trace.TraceInformation("Release DB");
                    Trace.TraceInformation("SQL connection: {0}", SqlLowLevel.RemovePasswordFromSqlConnectionString(SqlConnectionStr));
                    WinFormTestDbControl = null;
                    WPF_TestDBControl = null;


                    if (EnvironmentId.TheEnvironmentId.IsConsole_nGui)
                    {
                        ConsoleColor y = ConsoleColor.Yellow;
                        ConsoleColor b = ConsoleColor.Black;

                        Console.BackgroundColor = y; Console.Write("              "); Console.BackgroundColor = b; Console.WriteLine();
                        Console.BackgroundColor = y; Console.Write("   ");
                        Console.BackgroundColor = b;
                        ConsoleHelper.WarningMessage("Release DB");
                        Console.BackgroundColor = y; Console.Write("              "); Console.BackgroundColor = b; Console.WriteLine();
                    }
                } 
            #endregion

            }

            SqlHiLevel.DBConnectionStr = SqlConnectionStr;

            if (ContainsMainProperty("IpThermoSQLConnectionStr"))
                IpThermo.ConnectionStr = (string)this.AppConfigMain["IpThermoSQLConnectionStr"];
            else if (ContainsMainProperty("IPThermoConnectionString"))
                IpThermo.ConnectionStr = (string)this.AppConfigMain["IPThermoConnectionString"];

            if (ContainsMainProperty(BeepHelper.CONFIGNAME_BEEP_OUTPUT))
                BeepHelper.Output = (BeepHelper.EBeepType)MeasureConfig.TheConfig.GetConfigValueOf("BeepOutput");

            if (EnvironmentId.TheEnvironmentId.IsLoadFormRequired)
                EnvironmentId.TheEnvironmentId.ShowLoadForm();

        }

		/// <summary>
		/// clears Measure-specific data
		/// </summary>
		public virtual void Clear()
		{
			IniFileConfigs.Clear();
		}

		public object GetConfigValueOf(string format_in, params object[] args_in)
		{
			return GetConfigValueOf(string.Format(format_in, args_in));
		}

		public object GetConfigValueOf(string name_in)
		{
			return FindConfigOfProperty(name_in)[name_in];
		}

		public void SetConfigValueOf(string name_in, object value_in)
		{
			SetConfigValueOf(name_in, value_in, false);
		}

		public void SetConfigValueOf(string name_in, object value_in, bool save_in)
		{
			FindConfigOfProperty(name_in)[name_in] = value_in;
			FindConfigOfProperty(name_in).Save();
		}

		/// <summary>
		/// Searches the first ApplicationSettingsBase object, which contains the specified property.
		/// </summary>
		/// <param name="propertyName_in"></param>
		/// <returns></returns>
		private ApplicationSettingsBase FindConfigOfProperty(string propertyName_in)
		{
			foreach (ApplicationSettingsBase conf in this._appConfigs)
				foreach (System.Configuration.SettingsProperty p in conf.Properties)
					if (p.Name == propertyName_in)
						return conf;
			throw new NotFoundException("Config Property has not found: {0}", propertyName_in);
		}

		private bool ContainsConfigOfProperty(string propertyName_in)
		{
			foreach (ApplicationSettingsBase conf in this._appConfigs)
				foreach (System.Configuration.SettingsProperty p in conf.Properties)
					if (p.Name == propertyName_in)
						return true;
			return false;
		}

		private bool ContainsMainProperty(string propertyName_in)
		{
			foreach (System.Configuration.SettingsProperty p in this.AppConfigMain.Properties)
				if (p.Name == propertyName_in)
					return true;
			return false;
		}

		[Obsolete("Use the other Init with ApplicationSettingsBase for new applications.")]//this func is used by UD2.Merokeppesseg.exe + UD2.Reference.exe
		public virtual void Init(int room_id)
		{
			//            RoomId = room_id;
			EnvironmentId.TheEnvironmentId.RoomId = room_id;
		}

		[Obsolete("Use the Init(ApplicationSettingsBase ..) function for new applications.")]//this func is used by UD2.Merokeppesseg.exe + UD2.Reference.exe
		public void InitSql(bool releaseDb_in, string releaseDbConnection, string debugDbConnection)
		{
			SqlIsReleaseDb = releaseDb_in;
			if (releaseDb_in)
			{
				Trace.TraceInformation("Release DB");
				SqlConnectionStr = e77.MeasureBase.Sql.SqlLowLevel.DecryptPw(releaseDbConnection);
			}
			else
			{
				Trace.TraceInformation("Test DB");
				SqlConnectionStr = e77.MeasureBase.Sql.SqlLowLevel.DecryptPw(debugDbConnection);
			}
		}

		public override string ToString()
		{
			StringBuilder res = new StringBuilder();
			res.AppendFormat("Config obj: '{0}'.", GetType().Name);

			if (SqlConnectionStr == null)
				res.Append("SQL connection has not initialized. ");
			else
			{
				if (SqlIsReleaseDb)
					res.AppendFormat("Release Database. ");
				else
					res.AppendFormat("Debug Database. ");

				res.AppendFormat("SQL connection: {0}", SqlLowLevel.RemovePasswordFromSqlConnectionString(SqlConnectionStr));

				res.AppendLine();
			}

			res.AppendFormat(GUI.WorkplaceSetupForm.InfoStr);

			return res.ToString();
		}

		/// <summary>
		/// Can be release or test DB, depends on this.SqlIsReleaseDb
		/// </summary>
		public string SqlConnectionStr { get; private set; }

		/// <summary>
		/// Contains the release/test DB flag
		/// </summary>
		/// <value>true: release</value>
		public bool SqlIsReleaseDb { get; private set; }

		#region ISqlAdditionalObj Members

		virtual public Dictionary<string, object> GetSqlData()
		{
			Dictionary<string, object> res = new Dictionary<string, object>();
			foreach (string param in SqlHiLevel.SqlParameterColumns)
			{
				if (IsForIni(param))
				{
					//Name tag: ToCamelCase:
					string[] paramIds = GetConfigId(param).Split('@');
					paramIds[0] = paramIds[0].ToCamelCase();
					paramIds[1] = paramIds[1].ToCamelCase();
					//remove '_' prefix before digit:
					if (paramIds[1][0] == '_' && char.IsDigit(paramIds[1][1]))
						paramIds[1] = paramIds[1].Substring(1);

					res.Add(GetIniFileSqlColumnName(param), IniFileConfigs[paramIds.ItemsToString("@")]);
				}
				else if (_iniSqlTransponationDict != null && _iniSqlTransponationDict.ContainsValue(param))
				{
					//res.Add(param, IniFileConfigs[GetConfigId(GetTransponedIniIDName(param))]);
					string s = GetConfigId(GetTransponedIniIDName(param));
					if (IniFileConfigs.ContainsKey(s))
						res.Add(param, IniFileConfigs[s]);
					else
						res.Add(param, null);
				}
				else
					res.Add(param, GetConfigValueOf(param.ToCamelCase()));
			}

			return res;
		}

		virtual public void SqlLoad(Npgsql.NpgsqlDataReader sqlData_in)
		{
			foreach (string param in SqlHiLevel.SqlParameterColumns)
			{
				if (IsForIni(param))
				{
					string[] paramIds = GetConfigId(param).Split('@');
					paramIds[0] = paramIds[0].ToCamelCase();
					paramIds[1] = paramIds[1].ToCamelCase();
					//Add '_' prefix before digit:
					if (char.IsDigit(paramIds[1][0]))
						paramIds[1] = '_' + paramIds[1];

					IniFileConfigs[paramIds.ItemsToString("@")]
						= sqlData_in[GetIniFileSqlColumnName(param)].ToString();
				}
				else
				{
					string camelCaseParam = param.ToCamelCase();
					if (sqlData_in[param] is short && GetConfigValueOf(camelCaseParam) is int)
						SetConfigValueOf(camelCaseParam, Convert.ToInt32(sqlData_in[param])); //smallint at SQL loaded as int
					else
						SetConfigValueOf(camelCaseParam, sqlData_in[param]);
				}
			}
		}

		#endregion ISqlAdditionalObj Members

		#region INI File config

		internal protected static bool IsForIni(string columnId_in)
		{
			return columnId_in.Contains('@');
		}

		/// <summary>
		/// Key: ini parameter ID
		/// Value: sql column name
		/// </summary>
		protected static Dictionary<string, string> _iniSqlTransponationDict = new Dictionary<string, string>();

		//public Dictionary<string, string> IniSqlTransponationDict
		//{ 
		//    get{ return _iniSqlTransponationDict; }
		//}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="IniLabel_in">Ini parameter ID</param>
		/// <param name="SQLColumn_in">SQL column name</param>
		public static void IniSqlDictionaryAdd(string IniLabel_in, string SQLColumn_in)
		{
			_iniSqlTransponationDict.Add(IniLabel_in, SQLColumn_in);
		}

		/// <summary>
		/// Get Sql Column name from ini parameter name
		/// </summary>
		/// <param name="iniConfigId_in">parameter name in Ini file (with @)</param>
		/// <returns>Sql column name</returns>
		internal protected static string GetIniFileSqlColumnName(string iniConfigId_in)
		{

			string ret = GetTransponedSqlColumnName(iniConfigId_in);
			if (ret == null)
			{
				string[] subitems = ParseIniFileConfigId(iniConfigId_in);
				ret = string.Format("{0}_{1}",
					subitems[2].FromCamelCase(),
					subitems[3].ToLower()
					).SqlColumnName_DigitPrefix();
			}

			return ret;
		}

    
		private static string GetTransponedSqlColumnName(string iniConfigId_in)
		{
			if (_iniSqlTransponationDict == null || _iniSqlTransponationDict.Count == 0)
				return null;
			else
				return _iniSqlTransponationDict[iniConfigId_in];
		}

        /// <summary>
        /// Get ini parameter name from sql column name
        /// </summary>
        /// <param name="sqlColumnName_in">Sql column name</param>
        /// <returns>Parameter name in ini file</returns>
		private static string GetTransponedIniIDName(string sqlColumnName_in)
		{
			if (_iniSqlTransponationDict == null || _iniSqlTransponationDict.Count == 0)
				return null;
			else
			{
				IEnumerable<KeyValuePair<string, string>> res = _iniSqlTransponationDict.Where((kvp) => kvp.Value == sqlColumnName_in);
				if (res.Count() == 0)
					return null;
				else
					return res.Single().Key;
			}
		}

		private static string[] ParseIniFileConfigId(string iniConfigId_in)
		{
			string[] subitems = iniConfigId_in.Split('@');
			if (subitems.Count() != 4)
				throw new Exception(string.Format("Invalid Ini file config: {0}. Valid format: {type:Int,Float,Str,Double}@{relative ini file name}@{Category}@{Name}", iniConfigId_in));
			return subitems;
		}

		public enum EIniFiledDtaTypes { Str, Bool, Int, Float, Double }

		/// <summary>
		/// IniParamDescriptor
		/// Ini Parameter descriptor
		/// </summary>
		/// <param name="dataType_in">EIniFiledDtaTypes {Str, Int, Float}</param>
		/// <param name="partialPath_in">Search ini at Exe's path and the partial</param>
		/// <param name="category">Category at INI file</param>
		/// <param name="name">Name at categoty of INI</param>
		/// <returns>Param descriptor ({dataType}@{partialPath}@{cat}@{name})</returns>
		public static string CreateIniParamDescriptor(EIniFiledDtaTypes dataType_in, string partialPath_in, string category, string name)
		{
			return string.Format("{0}@{1}@{2}@{3}", dataType_in, partialPath_in, category, name);
		}

		/// <summary>
		/// IniParamDescriptor
		/// Ini Parameter descriptor
		/// </summary>
		/// <param name="dataType_in">EIniFiledDtaTypes {Str, Int, Float}</param>
		/// <param name="category">Category at INI file</param>
		/// <param name="name">Name at categoty of INI</param>
		/// <returns>Param descriptor ({dataType}@{partialPath}@{cat}@{name})</returns>
		/// <remarks>It uses the <paramref name="DefaultIniFilePartialPath"/> property as partial path</remarks>
		public static string CreateIniParamDescriptor(EIniFiledDtaTypes dataType_in, string category, string name)
		{
			return CreateIniParamDescriptor(dataType_in, DefaultIniFilePartialPath, category, name);
		}

		public int GetIniFileInt(string complexParamName)
		{
			string[] s;
			if (IsForIni(complexParamName))
			{
				s = complexParamName.Split('@');
			}
			else throw new Exception(string.Format("Complex INI param name format error: {0}, '@' not found.", complexParamName));

			if (s.Count() == 2)
				return Convert.ToInt32(GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Int, DefaultIniFilePartialPath, s[0], s[1])));

			if ((s.Count() == 3) || (s.Count() == 4))
				return Convert.ToInt32(GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Int, s[1], s[2], s[3])));
			throw new Exception(string.Format("Complex INI param name format error: {0}, 3 pieces of '@' not found.", complexParamName));

		}

		public int GetIniFileInt(string partialPath_in, string category, string name)
		{
			return Convert.ToInt32(GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Int, partialPath_in, category, name)));
		}

		public int GetIniFileInt(string category, string name)
		{
			return Convert.ToInt32(GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Int, DefaultIniFilePartialPath, category, name)));
		}

		public float GetIniFileFloat(string partialPath_in, string category, string name)
		{
			object o = GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Float, partialPath_in, category, name));
			return Convert.ToSingle(o);
		}

		public float GetIniFileFloat(string category, string name)
		{
			object o = GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Float, DefaultIniFilePartialPath, category, name));
			return Convert.ToSingle(o);
		}


		public double GetIniFileDouble(string partialPath_in, string category, string name)
		{
			object o = GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Double, partialPath_in, category, name));
			double d = Convert.ToDouble(o);
			return d;
		}

		public double GetIniFileDouble(string category, string name)
		{
			object o = GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Double, DefaultIniFilePartialPath, category, name));
			double d = Convert.ToDouble(o);
			return d;
		}

		public double GetIniFileDouble(string complexParamName)
		{
			string[] s;
			if (IsForIni(complexParamName))
			{
				s = complexParamName.Split('@');
			}
			else throw new Exception(string.Format("Complex INI param name format error: {0}, '@' not found.", complexParamName));

			if (!(s.Count() == 4))
				throw new Exception(string.Format("Complex INI param name format error: {0}, 4 pieces of '@' not found.", complexParamName));

			return Convert.ToDouble(GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Double, s[1], s[2], s[3])));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="partialPath_in"></param>
		/// <param name="category"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool IniFileContains(string partialPath_in, string category, string name)
		{
			return (IniFileConfigs.ContainsKey(GetConfigId(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Str, partialPath_in, category, name))));
			//            return Convert.ToString(GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Str, partialPath_in, category, name)));
		}

		public bool IniFileContains(string category, string name)
		{
			return (IniFileConfigs.ContainsKey(GetConfigId(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Str, DefaultIniFilePartialPath, category, name))));
			//            return Convert.ToString(GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Str, DefaultIniFilePartialPath, category, name)));
		}

		public bool IniFileContains(string complexParamName)
		{
			string[] s;
			if (IsForIni(complexParamName))
			{
				s = complexParamName.Split('@');
			}
			else throw new Exception(string.Format("Complex INI param name format error: {0}, '@' not found.", complexParamName));

			if (!(s.Count() == 4))
				throw new Exception(string.Format("Complex INI param name format error: {0}, 4 pieces of '@' not found.", complexParamName));

			return (IniFileConfigs.ContainsKey(GetConfigId(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Str, s[1], s[2], s[3]))));
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="partialPath_in"></param>
		/// <param name="category"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public string GetIniFileString(string partialPath_in, string category, string name)
		{
			return Convert.ToString(GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Str, partialPath_in, category, name)));
		}

		public string GetIniFileString(string category, string name)
		{
			return Convert.ToString(GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Str, DefaultIniFilePartialPath, category, name)));
		}

		public string GetIniFileString(string complexParamName)
		{
			string[] s;
			if (IsForIni(complexParamName))
			{
				s = complexParamName.Split('@');
			}
			else throw new Exception(string.Format("Complex INI param name format error: {0}, '@' not found.", complexParamName));

			if (!(s.Count() == 4))
				throw new Exception(string.Format("Complex INI param name format error: {0}, 4 pieces of '@' not found.", complexParamName));

			return Convert.ToString(GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Str, s[1], s[2], s[3])));
		}

		public bool GetIniFileBool(string complexParamName)
		{
			string[] s;
			if (IsForIni(complexParamName))
			{
				s = complexParamName.Split('@');
			}
			else throw new Exception(string.Format("Complex INI param name format error: {0}, '@' not found.", complexParamName));

			if (s.Count() == 2)
				return Convert.ToBoolean(GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Int, DefaultIniFilePartialPath, s[0], s[1])));

			if ((s.Count() == 3) || (s.Count() == 4))
				return Convert.ToBoolean(GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Int, s[1], s[2], s[3])));
			throw new Exception(string.Format("Complex INI param name format error: {0}, 3 pieces of '@' not found.", complexParamName));
		}

		public bool GetIniFileBool(string partialPath_in, string category, string name)
		{
			return Convert.ToBoolean(GetIniFileConfig(MeasureConfig.CreateIniParamDescriptor(EIniFiledDtaTypes.Bool, partialPath_in, category, name)));
		}

		public byte[] GetIniFileByteArray(string complexParamName)
		{
			string HexString = GetIniFileString(complexParamName);
			HexString = HexString.Replace("0x", "");
			HexString = HexString.Replace("$", "");

			int NumberChars = HexString.Length;
			if ((NumberChars & 1) == (1)) HexString = "0" + HexString;
			NumberChars = HexString.Length;
			byte[] bytes = new byte[NumberChars / 2];
			for (int i = 0; i < NumberChars; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
			}

			return bytes.Reverse().ToArray();

		}

		protected int _lineCounter = 0;
		public string GetIniSectionFirstLine(string sectionName)
		{
			_lineCounter = 1;
			return GetIniSectionLine(sectionName, _lineCounter);
		}

		public int GetIniSectionLineCount(string sectionName)
		{
			int ret = 0;
			string s = GetIniSectionFirstLine(sectionName);
			while (s != null)
			{
				ret++;
				s = GetIniSectionNextLine(sectionName);
			}
			return ret;
		}

		public string GetIniSectionNextLine(string sectionName)
		{
			_lineCounter++;
			return GetIniSectionLine(sectionName, _lineCounter);
		}

        [System.Obsolete("This function is a very good sample for the worst thinkin', the front opposite of the OO view")]
		public string GetIniSectionLine(string sectionName, int lineNo)
		{
			string ret = null;
			if (lineNo > 0)
			{
				if (iniList.Count == 0) GetIniFileString("dcont_tester", sectionName, "unitname");
				if (iniList.Count > 0)
				{
					string _sect = string.Format("[{0}]", sectionName);
					int i = 0;
					while ((i < iniList.Count) && !(iniList[i].Contains(_sect)))
					{
						i++;
					}

					if (i + lineNo < iniList.Count)
					{
						int j = i + 1;
						while ((j < iniList.Count) && (iniList[j][0] != '[') && (j < i + lineNo))
						{
							j++;
						}
						if ((j == i + lineNo) && (iniList[j][0] != '['))
							ret = (iniList[j]);
					}
				}
			}
			return ret;
		}



		public struct SectionPair
		{
			public String Section;
			public String Key;
		}

		/// <summary>
		/// Fills IniFileConfigs
		/// </summary>
		/// <param name="iniParamDescriptor_in">see CreateIniParamDescriptor()</param>
		/// <returns></returns>
		public object GetIniFileConfig(string iniParamDescriptor_in)
		{
			String currentRoot = null;
			String[] keyPair = null;


			string[] subitems = ParseIniFileConfigId(iniParamDescriptor_in);
			if (subitems[1] == "") throw new FileNotFoundException("The INI file not specified!");

			string _iniFName = string.Format("{0}\\{1}.ini", MeasureHelper.DirectoryOfTheExecutable, subitems[1]);

			if (!IniFileConfigs.ContainsKey(GetConfigId(iniParamDescriptor_in)))
			{
				if (MeasureCollectionBase.TheMeasures.IsSqlLoaded)
					throw new NotFoundException(iniParamDescriptor_in);
				else
				{ //load ini file
					///***********************   Az egészet berántja egyszerre, INILIST-be és IniFileConfigs-ba is
					string iniFName = _iniFName;
					if (File.Exists(_iniFName))
					{
						iniList.Clear();
						try
						{
							inifile = new StreamReader(iniFName, System.Text.Encoding.Default);
							while (!(inifile.EndOfStream))
							{
								string strLine = inifile.ReadLine();
								if (strLine.Contains("//")) strLine = strLine.Substring(0, strLine.IndexOf("//"));
								if (strLine.Contains("/*")) strLine = strLine.Substring(0, strLine.IndexOf("/*"));
								if (strLine.Contains(";")) strLine = strLine.Substring(0, strLine.IndexOf(";"));
								if ((strLine.Contains("[")) && (strLine.IndexOf("[") > 1)) strLine = strLine.Substring(0, strLine.IndexOf("["));
								strLine = strLine.Trim();
								if (strLine.Length > 0)
								{
									iniList.Add(strLine);
									if (strLine.StartsWith("[") && strLine.EndsWith("]"))
									{
										currentRoot = strLine.Substring(1, strLine.Length - 2);
									}
									else
									{
										if (strLine.Contains("="))
										{
											keyPair = strLine.Split(new char[] { '=' }, 2);
											keyPair[0] = keyPair[0].Trim();
											keyPair[1] = keyPair[1].Trim();

											SectionPair sectionPair;
											String value = null;

											if (currentRoot == null)
												currentRoot = "ROOT";

											sectionPair.Section = currentRoot;
											sectionPair.Key = keyPair[0];

											if (keyPair[1].Length > 0)
												value = keyPair[1];

											// keyPairs.Add(sectionPair, value);
											string _key = sectionPair.Section + '@' + keyPair[0];
											if (!IniFileConfigs.ContainsKey(_key))
												IniFileConfigs[_key] = value;
										}
									}
								}
							}
						}
						catch (Exception e)
						{
							throw e;
						}
						finally
						{
							if (iniFName != null) inifile.Close();
						}
					}
					else
					{
						throw new FileNotFoundException("The INI file not found!", iniFName);
					}
				}
			}
			object o;
			string s2;
			try
			{
				s2 = IniFileConfigs[GetConfigId(iniParamDescriptor_in)];
			}
			catch (Exception e)
			{
				throw new Exception(string.Format("{0} INI file hiba. {1}", e.Message, iniParamDescriptor_in));
			}
			if (subitems[0].ToLower() == "str")
				o = s2;
			else if (subitems[0].ToLower() == "bool")
				o = bool.Parse(s2);
			else if (subitems[0].ToLower() == "int")
			{
				try
				{
					bool hex = false;
					if (s2.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase)) { s2 = s2.Substring(2); hex = true; }
					if (s2.StartsWith("$", StringComparison.CurrentCultureIgnoreCase)) { s2 = s2.Substring(1); hex = true; }
					if (hex) { o = Convert.ToUInt32(s2, 16); }
					else o = int.Parse(s2);
				}
				catch (Exception e)
				{
					throw new System.ArgumentException("A paraméter nem konvertálható számmá. (Hex: 0x.. vagy $..)", s2, e);
				}
			}
			else if (subitems[0].ToLower() == "float")
				o = float.Parse(s2, CultureInfo.InvariantCulture);
			else if (subitems[0].ToLower() == "double")
				o = double.Parse(s2, CultureInfo.InvariantCulture);
			else
				throw new Exception(string.Format("Invalid Ini file config: {0}. Invalid data format. Valid DataType should be: Bool, Int, Float, Double, Str", subitems[0]));
			return o;
		}

		/// <summary>
		/// Cuts first 2 '@'
		/// </summary>
		/// <param name="configIdStr_in"></param>
		/// <returns></returns>
		private string GetConfigId(string configIdStr_in)
		{
			string s = configIdStr_in.Substring(configIdStr_in.IndexOf('@', configIdStr_in.IndexOf('@') + 1) + 1);
			return s;
		}


		/// <summary>
		/// Data chache for ini file parameters
		/// key: ConfigIdStr (witout path:"in order to make possible dynamic ini file.  Use GetConfigId)
		/// value: readed obj.
		/// </summary>
		Dictionary<string, string> IniFileConfigs = new Dictionary<string, string>();

		private StreamReader inifile;

		/// <summary>
		/// Data chache for ini file lines
		///    Discard remarks marked "//" or ";"
		///    Discard spaces on line edges
		/// </summary>
		List<string> iniList = new List<string>();

		#endregion INI File config

		public bool TestRound { get; set; }

        private EDataBases UsedDatabase { get; set; }

        /// <summary>
        /// In case of test database usage this controls could be added to the Form(WinForm) or MainWindow(WPF)
        /// else if Release DB used then these two property are null
        /// </summary>
        #region Test Database UI Controls
        public static TestDbControl WinFormTestDbControl { get; set; }

        public static GUI.WPF.WpfTestDbControl WPF_TestDBControl { get; set; } 
        #endregion
    }

}
