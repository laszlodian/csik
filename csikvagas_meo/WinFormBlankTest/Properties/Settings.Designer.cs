﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WinFormBlankTest.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool OpenAllPorts {
            get {
                return ((bool)(this["OpenAllPorts"]));
            }
            set {
                this["OpenAllPorts"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Server =77fejl-teszt;Port= 5432; Database = csikvagas;User ID = csikvagasadmin;Pa" +
            "ssword =8841Csikvagasadmin")]
        public string DBDebugConnection {
            get {
                return ((string)(this["DBDebugConnection"]));
            }
            set {
                this["DBDebugConnection"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Server = 77prod; Database = ipthermo; User ID = gyartas; Password = 1234Gyartas")]
        public string IpThermoConnectionString {
            get {
                return ((string)(this["IpThermoConnectionString"]));
            }
            set {
                this["IpThermoConnectionString"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Server =77i;Port= 5432; Database = csikvagas;User ID = csikvagasadmin;Password =8" +
            "841Csikvagasadmin")]
        public string DBReleaseConnection {
            get {
                return ((string)(this["DBReleaseConnection"]));
            }
            set {
                this["DBReleaseConnection"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool BlockForBarcode {
            get {
                return ((bool)(this["BlockForBarcode"]));
            }
            set {
                this["BlockForBarcode"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<SerializableConnectionString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <ConnectionString>Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Properties\DataSources\csikvagas.mdf;Integrated Security=True;User Instance=True</ConnectionString>
  <ProviderName>System.Data.SqlClient</ProviderName>
</SerializableConnectionString>")]
        public string csikvagasConnectionString {
            get {
                return ((string)(this["csikvagasConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("e77")]
        public string MasterPassword {
            get {
                return ((string)(this["MasterPassword"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=DONLACKOMAGE-PC\\SQLEXPRESS;Initial Catalog=master;Integrated Security" +
            "=True")]
        public string MSSQLConnectionString {
            get {
                return ((string)(this["MSSQLConnectionString"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("release")]
        public string COM_ports {
            get {
                return ((string)(this["COM_ports"]));
            }
            set {
                this["COM_ports"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("local")]
        public string DBConnection {
            get {
                return ((string)(this["DBConnection"]));
            }
            set {
                this["DBConnection"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool AutoFillBarcodes {
            get {
                return ((bool)(this["AutoFillBarcodes"]));
            }
            set {
                this["AutoFillBarcodes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ClosePortsWhenFormClosed {
            get {
                return ((bool)(this["ClosePortsWhenFormClosed"]));
            }
            set {
                this["ClosePortsWhenFormClosed"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool IsVirtualPorts {
            get {
                return ((bool)(this["IsVirtualPorts"]));
            }
            set {
                this["IsVirtualPorts"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoSize {
            get {
                return ((bool)(this["AutoSize"]));
            }
            set {
                this["AutoSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("7.3")]
        public double HOMOGENITY_UPPER_LIMIT {
            get {
                return ((double)(this["HOMOGENITY_UPPER_LIMIT"]));
            }
            set {
                this["HOMOGENITY_UPPER_LIMIT"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5.4")]
        public double HOMOGENITY_LOWER_LIMIT {
            get {
                return ((double)(this["HOMOGENITY_LOWER_LIMIT"]));
            }
            set {
                this["HOMOGENITY_LOWER_LIMIT"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("51")]
        public int BLANK_UPPER_LIMIT {
            get {
                return ((int)(this["BLANK_UPPER_LIMIT"]));
            }
            set {
                this["BLANK_UPPER_LIMIT"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("13")]
        public int BLANK_LOWER_LIMIT {
            get {
                return ((int)(this["BLANK_LOWER_LIMIT"]));
            }
            set {
                this["BLANK_LOWER_LIMIT"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("6.35")]
        public double HOMOGENITY_LIMIT_AVG {
            get {
                return ((double)(this["HOMOGENITY_LIMIT_AVG"]));
            }
            set {
                this["HOMOGENITY_LIMIT_AVG"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("32")]
        public double BLANK_LIMIT_AVG {
            get {
                return ((double)(this["BLANK_LIMIT_AVG"]));
            }
            set {
                this["BLANK_LIMIT_AVG"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double ACCEPTABLE_BLANK_DIFFERENCE {
            get {
                return ((double)(this["ACCEPTABLE_BLANK_DIFFERENCE"]));
            }
            set {
                this["ACCEPTABLE_BLANK_DIFFERENCE"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double ACCEPTABLE_HOMOGENITY_DIFFERENCE {
            get {
                return ((double)(this["ACCEPTABLE_HOMOGENITY_DIFFERENCE"]));
            }
            set {
                this["ACCEPTABLE_HOMOGENITY_DIFFERENCE"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool CONTINOUSLY_FILL_BARCODES {
            get {
                return ((bool)(this["CONTINOUSLY_FILL_BARCODES"]));
            }
            set {
                this["CONTINOUSLY_FILL_BARCODES"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string PREVIOUSLY_LOT_NUMBER {
            get {
                return ((string)(this["PREVIOUSLY_LOT_NUMBER"]));
            }
            set {
                this["PREVIOUSLY_LOT_NUMBER"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("6.2")]
        public double MEO_CHECK_MIN {
            get {
                return ((double)(this["MEO_CHECK_MIN"]));
            }
            set {
                this["MEO_CHECK_MIN"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("7.2")]
        public double MEO_CHECK_MAX {
            get {
                return ((double)(this["MEO_CHECK_MAX"]));
            }
            set {
                this["MEO_CHECK_MAX"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool AUTO_CLOSE_WINDOW {
            get {
                return ((bool)(this["AUTO_CLOSE_WINDOW"]));
            }
            set {
                this["AUTO_CLOSE_WINDOW"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool CHECK_MEO {
            get {
                return ((bool)(this["CHECK_MEO"]));
            }
            set {
                this["CHECK_MEO"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("24")]
        public int MEO_CHECK_VALID_TIMERANGE_IN_HOURS {
            get {
                return ((int)(this["MEO_CHECK_VALID_TIMERANGE_IN_HOURS"]));
            }
            set {
                this["MEO_CHECK_VALID_TIMERANGE_IN_HOURS"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AccestRightsInUse {
            get {
                return ((bool)(this["AccestRightsInUse"]));
            }
            set {
                this["AccestRightsInUse"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Server = localhost; Port = 5432; Database = csikvagas; User ID = csikvagasadmin;P" +
            "assword = 8841Csikvagasadmin")]
        public string DBLocal {
            get {
                return ((string)(this["DBLocal"]));
            }
            set {
                this["DBLocal"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("c:\\DBArchiver.exe")]
        public string DBUploaderScriptPath {
            get {
                return ((string)(this["DBUploaderScriptPath"]));
            }
            set {
                this["DBUploaderScriptPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Server =77fejl-teszt;Port= 5432; Database = csikvagas;User ID = csikvagasadmin;Pa" +
            "ssword =8841Csikvagasadmin")]
        public string Setting213 {
            get {
                return ((string)(this["Setting213"]));
            }
            set {
                this["Setting213"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Server = localhost; Port = 5432; Database = csikvagas; User ID = csikvagasadmin;P" +
            "assword = 8841Csikvagasadmin")]
        public string Setting {
            get {
                return ((string)(this["Setting"]));
            }
            set {
                this["Setting"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Server =77i;Port= 5432; Database = csikvagas;User ID = csikvagasadmin;Password =8" +
            "841Csikvagasadmin")]
        public string Setting1 {
            get {
                return ((string)(this["Setting1"]));
            }
            set {
                this["Setting1"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("c:\\DBSchemaRestore.exe")]
        public string DBSchemaRestoreScript {
            get {
                return ((string)(this["DBSchemaRestoreScript"]));
            }
            set {
                this["DBSchemaRestoreScript"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool CommitChangesNeeded {
            get {
                return ((bool)(this["CommitChangesNeeded"]));
            }
            set {
                this["CommitChangesNeeded"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("300")]
        public int ReloadConfigFrequency {
            get {
                return ((int)(this["ReloadConfigFrequency"]));
            }
            set {
                this["ReloadConfigFrequency"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("213")]
        public int ThreadID {
            get {
                return ((int)(this["ThreadID"]));
            }
            set {
                this["ThreadID"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("77i.77el.hu")]
        public string HostToCheckNetwork {
            get {
                return ((string)(this["HostToCheckNetwork"]));
            }
            set {
                this["HostToCheckNetwork"] = value;
            }
        }
    }
}
