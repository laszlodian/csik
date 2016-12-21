using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.ComponentModel;
using System.Drawing;
using e77.MeasureBase;
using e77.MeasureBase.GUI;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using Npgsql;
using WinFormBlankTest.UI.Forms;
using WinFormBlankTest.UI.Panels;
using System.Text;
using WinFormBlankTest.UI.Chart;
using WinFormBlankTest.UI.Forms.Other_Forms;
using WinFormBlankTest.UI.Forms.Classes_for_Show_DataGrid;
using WinFormBlankTest.UI.Forms.Result_Forms_With_DataGrid;
using WinFormBlankTest.Network;
using System.ServiceProcess;


namespace WinFormBlankTest
{
    static class Program
    {
        #region Variables

        public static bool firstFill = false;
        public static List<List<string>> rollids = new List<List<string>>();
        public static List<string> snAndRollID = new List<string>();
        public static int AlltogetherAccuracyStripCount = 0;
        public static double master_calibration;
        public static string master_lot_id;
        public static CounterPanel counter;
        public static string dbConnection;
        public static DetailedPanel[] myForm = new DetailedPanel[20];
        public static UserPanel[] panelForUser = new UserPanel[20];
        public static SerialPort[] portsAvailable = new SerialPort[20];
        public static Form main;
        public static Form user;
        public static int OutOfRangeCount_Homo = 0;
        public static bool LastStepIsOne;
        public static string SN;
        public static bool IsLOTReady;


        public static List<string> SelectedSNIDsInLimitBlank = new List<string>();
        public static List<string> SelectedSNIDsInLimitHomogenity = new List<string>();
        public static List<string> CentralBlankSN = new List<string>();
        public static List<string> CentralHomogenitySN = new List<string>();
        private static int count = 0;

        public static NpgsqlDataReader datar;
        private static int BlankIsValidTubeCount;
        private static int snCountForHomogenity;
        private static int snCountForBlank;
        //private static bool condition;
        public static string SelectedLotToMeasure;

        public static string Roll_ID;
        public static bool IsRollReady;
        public static int Measuring_tubus;
        public static Dictionary<string, int> HomogenityValidTubeCount = new Dictionary<string, int>();

        public static List<string> homogenityAndblankIsValidInRoll = new List<string>();
        public static List<string> homogenityAndblankIsValidInLot = new List<string>();
        public static List<string> blankIsValidInRoll = new List<string>();
        public static List<string> blankIsValidInLot = new List<string>();

        public static List<string> centralLotIDsHomogenity = new List<string>();
        public static List<string> centralRollIDsHomogenity = new List<string>();

        public static List<string> centralLotIDsBlank = new List<string>();
        public static List<string> centralRollIDsBlank = new List<string>();
        // public static List<string> CentralSNIDsForHomogenity=new List<string>();

        public static int centralLOTCount;
        public static string LOT_ID;
        public static Device dev = new Device();
        public static bool IsDialogShown = false;
        public static bool IsBatteryShown = false;
        public static bool IsBarCodeOk = false;
        public static bool IsLogging = false;
        public static int portnumber = 0;
        public static int month;
        public static int TubeCount;
        public static bool Remeasure;
        public static string remeasuredMeasureType;
        public static string measureType;
        public static string centralQuery = "select count(pk_id) from blank_test_averages where blank_is_valid=true";
        public static string valid_blank_test_ids = "select distinct lot_id from blank_test_averages where blank_is_valid=true and invalidate=false and remeasured=false";
        public static BackgroundWorker worker = new BackgroundWorker();
        public static RollMeanBlankCurrent chartofBlank;
        public static bool IsSecondRun = false;
        public static List<string> CentralIDs = new List<string>();
        readonly static object locker = new object();
        public static string BarCode;
        public static double Humidity;
        public static double HTC;
        public static DateTime ExpirityDate;
        public static int Number = 0;



        public static int Round;
        public static string Accuracy_sample_blood_vial_ID;
        public static int BarcodeNumber = 0;
        public static int ValuesID;
        public static double Temperature;
        public static bool firstslot;
        public static int storedGlus;

        public static BindingSource centralBindingSrc = new BindingSource();

        private static bool condition;


        private static Dictionary<string, double> ValueToSN = new Dictionary<string, double>();
        private static Dictionary<string, double> ValueToRollID = new Dictionary<string, double>();
        private static Dictionary<string, string> RollIDToSN = new Dictionary<string, string>();
        private static Dictionary<string, double> DiffToRollID = new Dictionary<string, double>();
        private static Dictionary<string, int> CountInRoll = new Dictionary<string, int>();
        public static int BlankMeasuredStripCount = 0;
        public static int HomogenityMeasuredStripCount = 0;
        private static double correctGluDiffFromAvg;
        public static List<int> HomogenityInRangeGlusCount = new List<int>();
        public static List<double> HomogenityInRangeGlusValue = new List<double>();
        public static int g = 0;
        public static List<string> HomogenityInRangeSNsIDs = new List<string>();
        public static List<string> validRolls;
        public static Accuracy_vials_form acc_vials_form;
        private static int rollCounter;
        public static int PortCount;



        public static double HomogenityLimitAvg;
        public static double BlankLimitAvg;
        public static int sn = 1;
        public static List<double> BlankDiff = new List<double>();
        public static List<double> HommDiff = new List<double>();

        public static List<double> SNnanosAVG = new List<double>();
        public static List<string> CentralSNIDsForHomogenity = new List<string>();
        public static List<double> SNglusAVG = new List<double>();
        public static List<double> SNIDs = new List<double>();
        public static List<double> DiffereneceFromAVGBlank = new List<double>();
        public static Dictionary<string, List<string>> BestRoll = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<double>> BestGlu = new Dictionary<string, List<double>>();

        public static List<double> DiffereneceFromAVGHomogenity = new List<double>();
        public static List<string> snIDOfRoll = new List<string>();
        public static int tubes_count_in_act_roll;
        public static List<string> snThisRoll = new List<string>();


        #endregion

        #region Lists and Dictionaries
        public static Dictionary<double, string> glusAndNanoAmperAVGs = new Dictionary<double, string>();
        public static List<double> AllDiffs = new List<double>();

        private static List<int> skippedIndexes = new List<int>();
        public static List<double> AllGlus = new List<double>();
        private static List<double> TubesAVG = new List<double>();
        public static List<double> glusValues = new List<double>();
        public static List<string> HomogenityInRangeRollsIDs = new List<string>();
        public static List<string> AllRolls = new List<string>();
        public static Dictionary<string, string> lotid_roll_id = new Dictionary<string, string>();
        public static List<string> CentralSNIDForBlank = new List<string>();
        public static List<bool> stripInTubes = new List<bool>();
        public static List<double> glusAVGs = new List<double>();
        public static List<double> nanosAVGs = new List<double>();
        #endregion
        #region Delegate to close window

        delegate void CloseWindowDelegate();
        public static void CloseWindow()
        {
            if (user.InvokeRequired)
            {
                user.Invoke(new CloseWindowDelegate(CloseWindow));
            }
            else
                user.Close();
        }

        #endregion
        #region Constants
        public const string ADMIN_PASSWORD = "/77Erulez";
        public const string MEO_LEADER_PASSWORD = "/abrakadabra";
        public const string BLANK_TEST_ARG = "Blank Teszt";
        public const string HOMOGENITY_TEST_ARG = "Homogenity Teszt";
        public const string ACCURACY_TEST_ARG = "Accuracy Teszt";
        #endregion
        /// <summary>
        /// The main entry point for the application.
        /// </summary>        
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            TraceHelper.SetupListener();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GuiErrorHandling.DefaultUnhandledExceptionEventHandler);
            Application.ThreadException += GuiErrorHandling.DefaultUnhandledExceptionEventHandler;

            SetDefaultDecimalSeparatorToDot();

            ProcessDBHost();
            ProcessAccess(new string[] { args[0] });

            if (args[0] == "/accuracy")
            {
                acc_vials_form = new Accuracy_vials_form();
            }

            Trace.TraceInformation("Blank,Accuracy and Homogenity Test Started...");

            new BlankTestSwVersion();
            Trace.TraceInformation("Application: Blank test and Homogenity test check application ,Sw Version:{0}", BlankTestSwVersion.TheSwVersion.SwVersions);
            Trace.TraceInformation("SwVersion Initialized and trace Started.");

            new BlankTestEnvironment();
            if (BlankTestEnvironment.TheEnvironmentId.Init())
                return;

            Trace.TraceInformation("Environment Initialized");
            GenerateUIAndStartApp(args);
        }

        private static void FillMeasurementTable()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();

                    using (NpgsqlCommand queryOperator = new NpgsqlCommand(string.Format("select pk_id from operators where operator='{0}'", BlankTestEnvironment.Operator), conn))
                    {
                        object res = null;

                        res = queryOperator.ExecuteScalar();

                        if (res == null || res == DBNull.Value)
                        {
                            Trace.TraceWarning("Operator with name {0} not found in database. Operator will be inserted.", BlankTestEnvironment.Operator);
                            using (NpgsqlCommand insertOperator = new NpgsqlCommand(string.Format("insert into operators(operator) values('{0}')", BlankTestEnvironment.Operator), conn))
                            {
                                object result = null;
                                result = insertOperator.ExecuteNonQuery();

                                if (result == null || result == DBNull.Value)
                                {
                                    Trace.TraceError("Unsuccessfull insert to operators table, statement: {0}", insertOperator.CommandText);

                                    throw new SqlInsertException(string.Format("Unsuccessfull insert to operators table, statement: {0}", insertOperator.CommandText));

                                }

                            }
                        }
                        else
                        {
                            Trace.TraceInformation("The operator found in database, pk_id {0}", Convert.ToInt32(res));
                            operator_pk = Convert.ToInt32(res);

                        }

                    }//query and if not present insert operator name


                    using (NpgsqlCommand queryTestType = new NpgsqlCommand(string.Format("select pk_id from test_type where test_name='{0}'", Program.measureType), conn))
                    {
                        object testtype = null;

                        testtype = queryTestType.ExecuteScalar();

                        if (testtype == null || testtype == DBNull.Value)
                        {
                            Trace.TraceError("Nincs ilyen teszt az adatbázisban!");

                            DialogResult dRes = MessageBox.Show(string.Format("Nincs ilyen teszt({0}) az adatbázisban!\r\nBiztosan folytatja a mérést ?!A teszt típus rögzítésre kerül.", Program.measureType), "Nincs ilyen teszt az adatbázisban!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                            if (dRes == DialogResult.No)
                            {
                                Environment.Exit(Environment.ExitCode);
                            }
                            else if (dRes == DialogResult.Yes)
                            {

                                Trace.TraceInformation("Insert to test_type, new test: {0}", Program.measureType);
                                InsertTestType(conn, Program.measureType);
                            }


                        }
                        else
                        {
                            testType_pkid = Convert.ToInt32(testtype);

                        }
                    }
                    using (NpgsqlCommand insertToMeasurement = new NpgsqlCommand(string.Format("insert into measurement(tube_count,fk_test_type_id,date,fk_operators_id) VALUES({0},{1},{2},{3})", Program.TubeCount, testType_pkid, "@date", operator_pk), conn))
                    {
                        object insertRes = null;
                        insertToMeasurement.Parameters.AddWithValue("@date", DateTime.Now);
                        insertRes = insertToMeasurement.ExecuteNonQuery();

                        if (insertRes == null || insertRes == DBNull.Value)
                        {
                            Trace.TraceError("Unsuccessfull insert to measurement table! Statement: {0}", insertToMeasurement.CommandText);
                            throw new SqlInsertException("Unsuccessfull insert to measurement table!");
                        }
                        else
                            Trace.TraceInformation("Successfull insert to measurement table.");


                    }
                    using (NpgsqlCommand getMeasurementPK = new NpgsqlCommand(string.Format("select MAX(pk_id) from measurement"), conn))
                    {
                        object result = null;
                        result = getMeasurementPK.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                        {
                            Trace.TraceError("Unsuccessfull select from measurement table! Statement: {0}", getMeasurementPK.CommandText);
                            throw new SqlInsertException("Unsuccessfull select from measurement table!");
                        }
                        else
                        {
                            Trace.TraceInformation("Successfull select from measurement table.");

                            MeasurementActualPkId = Convert.ToInt32(result);
                            Program.LoggedToMeasurement = true;
                        }

                    }


                }
                catch (Exception ex)
                {
                    Program.LoggedToMeasurement = false;
                    Trace.TraceError("Unsuccessfull insert to measurement table!");
                   // throw new SqlInsertException("Unsuccessfull insert at measurement table or it's dependencies!");
                }
                finally
                {

                    conn.Close();

                }
            }
        }

        private static void InsertTestType(NpgsqlConnection conn, string newTestType_in)
        {
            using (NpgsqlCommand insertTest = new NpgsqlCommand(string.Format("insert to test_type(test_name) VALUES('{0}')", newTestType_in), conn))
            {
                object MaxPk = null;
                object res = null;
                res = insertTest.ExecuteNonQuery();

                if (res == null || res == DBNull.Value)
                {
                    Trace.TraceError("Unsuccessfull insert to measurement table! Statement: {0}", insertTest.CommandText);
                    throw new SqlInsertException("Unsuccessfull insert at measurement table or it's dependencies!");
                }
                else
                {
                    using (NpgsqlCommand maxID = new NpgsqlCommand("select max(pk_id) from test_type", conn))
                    {
                        MaxPk = maxID.ExecuteScalar();
                    }

                    testType_pkid = Convert.ToInt32(MaxPk);
                }

            }
        }



        #region Methods
        static void worker_DoWork(object sender, DoWorkEventArgs e)
        {

        }


        public static void ProcessDBHost()
        {

            if (Properties.Settings.Default.DBConnection.Equals("local"))
            {
                dbConnection = Properties.Settings.Default.DBLocal;
            }
            else if (Properties.Settings.Default.DBConnection.Equals("debug"))
            {
                dbConnection = Properties.Settings.Default.DBDebugConnection;
            }
            else if (Properties.Settings.Default.DBConnection.Equals("release"))
            {
                dbConnection = Properties.Settings.Default.DBReleaseConnection;
            }
            else
                throw new Exception(string.Format("A DBConnection konfigurációs tulajdonság lehetséges értékei: release,debug,local;Jelenleg megadott:{0}",Properties.Settings.Default.DBConnection));

            if (Properties.Settings.Default.CommitChangesNeeded)
            {


                #region SQLConnection section

                QueriesMaxPrimaryKeysThenSetTheSequencesValue(Properties.Settings.Default.DBReleaseConnection,Properties.Settings.Default.DBLocal);
                #endregion
            }
        }

        public static void QueriesMaxPrimaryKeysThenSetTheSequencesValue(string connectionFromGetMaxPkIds,string connectionToSetSequence)
        {
            accuracy_central_bias_pk_id = GetMaxPkIdFromReleaseHost("accuracy_central_bias",Properties.Settings.Default.DBReleaseConnection) + 1;
            accuracy_lot_result_pk_id = GetMaxPkIdFromReleaseHost("accuracy_lot_result",Properties.Settings.Default.DBReleaseConnection) + 1;
            accuracy_result_central_pk_id = GetMaxPkIdFromReleaseHost("accuracy_result_central", Properties.Settings.Default.DBReleaseConnection) + 1;
            accuracy_result_master_pk_id = GetMaxPkIdFromReleaseHost("accuracy_result_master", Properties.Settings.Default.DBReleaseConnection) + 1;
            accuracy_results_pk_id = GetMaxPkIdFromReleaseHost("accuracy_results", Properties.Settings.Default.DBReleaseConnection) + 1;
            accuracy_test_pk_id = GetMaxPkIdFromReleaseHost("accuracy_test", Properties.Settings.Default.DBReleaseConnection) + 1;
            accuracy_values_pk_id = GetMaxPkIdFromReleaseHost("accuracy_values", Properties.Settings.Default.DBReleaseConnection) + 1;
            blank_test_averages_alternation_pk_id = GetMaxPkIdFromReleaseHost("blank_test_averages_alternation", Properties.Settings.Default.DBReleaseConnection) + 1;
            blank_test_averages_pk_id = GetMaxPkIdFromReleaseHost("blank_test_averages", Properties.Settings.Default.DBReleaseConnection) + 1;
            blank_test_environment_pk_id = GetMaxPkIdFromReleaseHost("blank_test_environment", Properties.Settings.Default.DBReleaseConnection) + 1;
            blank_test_errors_pk_id = GetMaxPkIdFromReleaseHost("blank_test_errors", Properties.Settings.Default.DBReleaseConnection) + 1;
            blank_test_identify_pk_id = GetMaxPkIdFromReleaseHost("blank_test_identify", Properties.Settings.Default.DBReleaseConnection) + 1;
            blank_test_result_pk_id = GetMaxPkIdFromReleaseHost("blank_test_result", Properties.Settings.Default.DBReleaseConnection) + 1;
            central_vials_pk_id = GetMaxPkIdFromReleaseHost("central_vials", Properties.Settings.Default.DBReleaseConnection) + 1;
            device_meo_pk_id = GetMaxPkIdFromReleaseHost("device_meo", Properties.Settings.Default.DBReleaseConnection) + 1;
            global_sql_tables_pk_id = GetMaxPkIdFromReleaseHost("global_sql_tables", Properties.Settings.Default.DBReleaseConnection) + 1;
            homogenity_result_alternation_pk_id = GetMaxPkIdFromReleaseHost("homogenity_result_alternation", Properties.Settings.Default.DBReleaseConnection) + 1;
            homogenity_result_pk_id = GetMaxPkIdFromReleaseHost("homogenity_result", Properties.Settings.Default.DBReleaseConnection) + 1;
            homogenity_test_pk_id = GetMaxPkIdFromReleaseHost("homogenity_test", Properties.Settings.Default.DBReleaseConnection) + 1;
            lot_result_modified_pk_id = GetMaxPkIdFromReleaseHost("lot_result_modified", Properties.Settings.Default.DBReleaseConnection) + 1;
            lot_result_pk_id = GetMaxPkIdFromReleaseHost("lot_result", Properties.Settings.Default.DBReleaseConnection) + 1;
            measure_type_pk_id = GetMaxPkIdFromReleaseHost("measure_type", Properties.Settings.Default.DBReleaseConnection) + 1;
            measurement_pk_id = GetMaxPkIdFromReleaseHost("measurement", Properties.Settings.Default.DBReleaseConnection) + 1;
            roll_result_pk_id = GetMaxPkIdFromReleaseHost("roll_result", Properties.Settings.Default.DBReleaseConnection) + 1;
            test_type_pk_id = GetMaxPkIdFromReleaseHost("test_type", Properties.Settings.Default.DBReleaseConnection) + 1;
            operators_pk_id = GetMaxPkIdFromReleaseHost("operators", Properties.Settings.Default.DBReleaseConnection) + 1;
            //  serial_input_pk_id = GetMaxPkIdFromReleaseHost("serial_input");

            SetSequencesValues(accuracy_central_bias_pk_id, "accuracy_central_bias_pk_id_seq",Properties.Settings.Default.DBLocal);
            SetSequencesValues(accuracy_lot_result_pk_id, "accuracy_lot_result_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(accuracy_result_central_pk_id, "accuracy_result_central_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(accuracy_result_master_pk_id, "accuracy_result_master_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(accuracy_results_pk_id, "accuracy_results_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(accuracy_test_pk_id, "accuracy_test_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(accuracy_values_pk_id, "accuracy_values_pk_id_seq", Properties.Settings.Default.DBLocal);

            SetSequencesValues(central_vials_pk_id, "central_vials_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(device_meo_pk_id, "device_meo_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(global_sql_tables_pk_id, "global_sql_tables_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(homogenity_result_alternation_pk_id, "homogenity_result_alternation_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(homogenity_result_pk_id, "homogenity_result_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(homogenity_test_pk_id, "homogenity_test_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(lot_result_modified_pk_id, "lot_result_modified_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(lot_result_pk_id, "lot_result_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(measure_type_pk_id, "measure_type_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(measurement_pk_id, "measurement_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(roll_result_pk_id, "roll_result_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(test_type_pk_id, "test_type_pk_id_seq", Properties.Settings.Default.DBLocal);

            //    SetSequencesValues(serial_input_pk_id, "serial_input_pk_id_seq");
            SetSequencesValues(blank_test_averages_alternation_pk_id, "blank_test_averages_alternation_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(blank_test_averages_pk_id, "blank_test_averages_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(blank_test_environment_pk_id, "blank_test_environment_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(blank_test_errors_pk_id, "blank_test_errors_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(blank_test_identify_pk_id, "blank_test_identify_pk_id_seq", Properties.Settings.Default.DBLocal);
            SetSequencesValues(blank_test_result_pk_id, "blank_test_result_pk_id_seq", Properties.Settings.Default.DBLocal);
        }

        private static void SetSequencesValues(int pk_id_in, string sequence_in,string conn_in)
        {
            using (NpgsqlConnection localConn = new NpgsqlConnection(conn_in))
            {
                try
                {
                    localConn.Open();

                    using (NpgsqlCommand setSequence = new NpgsqlCommand(string.Format("ALTER SEQUENCE {1} RESTART WITH {0}", pk_id_in, sequence_in), localConn))
                    {
                        object res = null;

                        res = setSequence.ExecuteNonQuery();

                        if (res == null || res == DBNull.Value)
                        {
                            throw new Exception(string.Format("Can't set sequence start value for sequence: {0}", sequence_in));
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
                finally
                {
                    localConn.Close();

                }
            }
        }

        private static int GetMaxPkIdFromReleaseHost(string tableName_in,string conn_in)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(conn_in))
            {
                try
                {
                    conn.Open();

                    using (NpgsqlCommand getMaxPkId = new NpgsqlCommand(string.Format("select MAX(pk_id) from {0}", tableName_in), conn))
                    {
                        object res = null;

                        res = getMaxPkId.ExecuteScalar();

                        if (res == null || res == DBNull.Value)
                        {
                          //  throw new Exception(string.Format("Not found pk_id for table: {0}", tableName_in));
                        }
                        return Convert.ToInt32(res);
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
                finally
                {
                    conn.Close();

                }
            }
        }

        public static void ProcessAccess(string[] arguments)
        {
            if (arguments.Contains("$invalidate"))
            {
                BlankTestEnvironment.AccessRights.Add("invalidate");
            }
            if (arguments.Contains("$remeasure"))
            {
                BlankTestEnvironment.AccessRights.Add("remeasure");
            }
            if (arguments.Contains("$showdata"))
            {
                BlankTestEnvironment.AccessRights.Add("showdata");
            }
        }
        private static void ProcessArgs(string[] arguments)
        {
            if (arguments.Length < 1)
            {
                throw new ArgumentException("A /blank,/accuracy vagy /homogenity kapcsolót definiálni kell");
            }
            if (arguments.Contains("/blank"))
            {
                measureType = "blank";
            }
            if (arguments.Contains("/meo"))
            {
                measureType = "meo";
            }
            if (arguments.Contains("/homogenity"))
            {
                measureType = "homogenity";
            }
            if (arguments.Contains("/accuracy"))
            {
                measureType = "accuracy";

            }
            if (arguments.Contains("/show"))
            {
                measureType = "show";
            }
            if (arguments.Contains("/showall"))
            {
                measureType = "showall";
            }
            if (arguments.Contains("/print"))
            {
                measureType = "print";
            }
            if (arguments.Contains("/invalidate"))
            {
                measureType = "invalidate";
            }
        }


        public static int GetMonth()
        {
            month = System.DateTime.Now.Month;
            return month;
        }
        public static void CreateTablePanel()
        {
            MainWindow.dynamicTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            MainWindow.dynamicTableLayoutPanel.Name = "TableLayoutPanel1";
            MainWindow.dynamicTableLayoutPanel.Size = new System.Drawing.Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            MainWindow.dynamicTableLayoutPanel.ColumnCount = 4;
            MainWindow.dynamicTableLayoutPanel.RowCount = 4;
            MainWindow.dynamicTableLayoutPanel.Controls.Add(new CounterPanel(Program.TubeCount));

        }

        public static double StandardDeviation(this IEnumerable<double> values)
        {
            double avg = values.Average();
            return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
        }
        public static void CreateTablePanelForUser()
        {
            UserWindow.userTables.Location = new System.Drawing.Point(0, 0);
            UserWindow.userTables.Name = "TableLayoutPanel1";
            UserWindow.userTables.Size = new System.Drawing.Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            UserWindow.userTables.ColumnCount = 4;
            UserWindow.userTables.RowCount = 5;
        }
        #endregion

        #region Delegates
        delegate void AddTableToMainDelegate();
        public static void AddTableToMain()
        {
            if (main.InvokeRequired)
            {
                main.Invoke(new AddTableToMainDelegate(AddTableToMain));

            }
            else
            {
                MainWindow.dynamicTableLayoutPanel.Controls.Add(myForm[portnumber]);
            }
        }

        delegate void AddDetailedPanelToMainDelegate(DetailedPanel detailedPanel);
        private static void AddDetailedPanelToMain(DetailedPanel detailedPanel)
        {
            if (main.InvokeRequired)
            {
                main.Invoke(new AddDetailedPanelToMainDelegate(AddDetailedPanelToMain));
            }
            else
            {
                main.Controls.Add(myForm[portnumber]);

            }
        }

        delegate void AddCounterPanelDelegate(CounterPanel counter, int tubuscount);
        private static void AddCounterPanel(CounterPanel counter, int tubuscount)
        {
            counter.Show();
            if (UserWindow.userTables.InvokeRequired)
            {
                UserWindow.userTables.Invoke(new AddCounterPanelDelegate(AddCounterPanel), counter, tubuscount);
            }
            else
            {
                if (Properties.Settings.Default.AutoSize)
                {
                    UserWindow.userTables.AutoSize = true;
                }

                for (int i = 0; i < UserWindow.userTables.Controls.Count; i++)
                {
                    UserWindow.userTables.Controls[i].Name = i.ToString();
                }

                counter.Show();
            }
        }
        #endregion

        /// <summary>
        /// This function is Initialize all 16 ports, create the 16 panels for those 
        /// </summary>
        public static void InitAllPort()
        {
            lock (locker)
            {
                SerialPort port;
                string[] ports = new string[20];

                if (Properties.Settings.Default.IsVirtualPorts)
                {
                    ports = new string[] { "COM20", "COM21", "COM22", "COM23", "COM24", "COM25", "COM26", "COM27" };
                }
                else if (Properties.Settings.Default.OpenAllPorts)
                {
                    ports = SerialPort.GetPortNames();
                }
                else
                {
                    ports = new string[] { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10", "COM11", "COM12", "COM13", "COM14", "COM15", "COM16" };
                }
                List<Device> deviceList = new List<Device>();
                Dictionary<UserPanel, string> panelnames = new Dictionary<UserPanel, string>();

                foreach (string item in ports)
                {
                    port = new SerialPort(item);

                    if (!port.IsOpen)
                        port.Open();

                    port.BaudRate = 2400;
                    port.DataBits = 8;
                    port.Parity = Parity.Odd;
                    port.StopBits = StopBits.One;
                    port.Handshake = Handshake.None;
                    port.WriteTimeout = 700;
                    port.ReadTimeout = 700;

                    port.DtrEnable = true;
                    port.RtsEnable = true;

                    portsAvailable[portnumber] = port;
                    panelForUser[portnumber] = new UserPanel(port);

                    UserWindow.userTables.Controls.Add(panelForUser[portnumber]);
                    panelForUser[portnumber].Text = port.PortName;
                    panelForUser[portnumber].Size = new Size(Screen.PrimaryScreen.WorkingArea.Width / 4, Screen.PrimaryScreen.WorkingArea.Height / 5);
                    Device dev = new Device(port);
                    myForm[portnumber] = new DetailedPanel(port, panelForUser[portnumber], dev);
                    myForm[portnumber].Width = 250;
                    myForm[portnumber].Height = 200;

                    AddDetailedPanelToMain(myForm[portnumber]);

                    myForm[portnumber].Text = port.PortName;
                    myForm[portnumber].Show();

                    portnumber++;
                }

                AddCounterPanel(counter, Program.TubeCount);
            }
        }
        public static List<string> validHomogenityLots = new List<string>();
        public static List<string> LotHasNotAccuracyTest = new List<string>();
        /// <summary>
        /// start App and create UI without the Program need to be restarted
        /// </summary>
        public static void GenerateUIAndStartApp(string[] a)
        {
            ProcessArgs(a);

            if (a.Contains(ADMIN_PASSWORD) && !a.Contains(MEO_LEADER_PASSWORD))
            {
                Program.AccessRight = ADMIN_ACCESSRIGHT;
            }
            else if ((!a.Contains(ADMIN_PASSWORD) && a.Contains(MEO_LEADER_PASSWORD)))
            {
                Program.AccessRight = MEO_LEADER_ACCESSRIGHT;
            }
            else
                Program.AccessRight = "null";

            if (Program.Remeasure)
            {
                Program.Remeasure = false;

                if (Program.remeasuredMeasureType == "homogenity")
                {
                    Program.measureType = "homogenity";
                }
                else
                    Program.measureType = "blank";
            }
            if (Program.measureType == "invalidate")
            {
                new LotNumberForm().ShowDialog();
                return;
            }
            if (Program.measureType == "show")
            {
                new LotNumberForm().ShowDialog();
                return;
            }
            else if (Program.measureType == "showall")
            {

                new LotNumberForm().ShowDialog();
                return;
            }
            else if (Program.measureType == "print")
            {
                new LotNumberForm().ShowDialog();
                Environment.Exit(Environment.ExitCode);
            }
            else if (Program.measureType == "meo")
            {
                CreateCentralUIPanel();
                Environment.Exit(Environment.ExitCode);
            }
            else if (Program.measureType != "accuracy")//measurement is blank or homogenity or device_meo
            {
                new TubeCountInRollForm().ShowDialog();
                FillMeasurementTable();
            }

            #region accuracy
            if (Program.measureType == "accuracy")
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
                {
                    try
                    {
                        conn.Open();

                        using (NpgsqlDataReader selectLotsWithNotValidAccuraccy = new NpgsqlCommand(string.Format("select DISTINCT lot_id from homogenity_result where homogenity_is_valid=true and invalidate=False"), conn).ExecuteReader())
                        {
                            if (selectLotsWithNotValidAccuraccy.HasRows)
                            {
                                while (selectLotsWithNotValidAccuraccy.Read())
                                {
                                    validHomogenityLots.Add(Convert.ToString(selectLotsWithNotValidAccuraccy["lot_id"]));
                                }

                            }
                            else
                            {
                                Trace.TraceError("Nincs valid lot a homogenity_result táblában!");
                                throw new SqlNotFoundException("homogenity_result", string.Format("select DISTINCT lot_id from homogenity_result where homogenity_is_valid=true"), string.Empty);
                            }
                            selectLotsWithNotValidAccuraccy.Close();
                        }

                        condition = validHomogenityLots.Count >= 1;

                        CheckValidHomogenityLots(condition);

                        foreach (string act_lot in validHomogenityLots)
                        {


                            using (NpgsqlDataReader selectNotMeasuredLotsInAccuracy = new NpgsqlCommand(string.Format("select lot_id from accuracy_lot_result where lot_id='{0}'", act_lot), conn).ExecuteReader())
                            {
                                if (!selectNotMeasuredLotsInAccuracy.HasRows)
                                {
                                    LotHasNotAccuracyTest.Add(act_lot);

                                }
                                selectNotMeasuredLotsInAccuracy.Close();

                            }
                        }//end of foreach

            #endregion

                        new LotNumberForm(LotHasNotAccuracyTest).ShowDialog();

                        #region Commented out old code
                        //#region Get rolls from the filtered lots

                        //    #region Get roll_id for previously found lot_id where blank_ok=true
                        //    using (NpgsqlCommand centralRolls = new NpgsqlCommand(string.Format("select distinct roll_id from blank_test_averages where invalidate=false and blank_is_valid=true and lot_id='{0}' and remeasured=false", Program.SelectedLotToMeasure), conn))
                        //    {

                        //        using (datar = centralRolls.ExecuteReader())
                        //        {

                        //            if (datar.HasRows)
                        //            {
                        //                while (datar.Read())
                        //                {
                        //                    if (!blankIsValidInRoll.Contains(Convert.ToString(datar["roll_id"])))
                        //                    {
                        //                        blankIsValidInRoll.Add(Convert.ToString(datar["roll_id"]));
                        //                    }

                        //                }
                        //                condition = false;
                        //                datar.Close();
                        //            }
                        //            else
                        //            {
                        //                datar.Close();
                        //                Trace.TraceWarning("No valid blank test for lot_id:{0} ", Program.SelectedLotToMeasure);
                        //            }
                        //        }
                        //    #endregion
                        //    }
                        //    #region For each roll at previously selected lot, and all the roll_id where blank is valid
                        //     homogenityAndblankIsValidInRoll=new List<string>();
                        //       homogenityAndblankIsValidInLot=new List<string>();

                        //    #region Get roll_ids from the selected lot where the first two test is valid
                        //    foreach (string roll in blankIsValidInRoll)
                        //    {

                        //        #region Get That roll_ids where blank_is_ok and check homogenity_result if the same lot and which roll has valid result that contains to that lot
                        //        using (NpgsqlCommand comm_valid_ids_together = new NpgsqlCommand(string.Format("select distinct roll_id from homogenity_result where lot_id='{0}' and roll_id='{1}'and invalidate=false and homogenity_is_valid=true and not_h62_is_valid=true and out_of_range_is_valid=true and remeasured=false", Program.SelectedLotToMeasure, roll), conn))
                        //        {
                        //            using (NpgsqlDataReader dr_valid = comm_valid_ids_together.ExecuteReader())
                        //            {
                        //                if (dr_valid.HasRows)
                        //                {
                        //                    while (dr_valid.Read())
                        //                    {

                        //                        if (!homogenityAndblankIsValidInRoll.Contains(Convert.ToString(dr_valid["roll_id"])))
                        //                        {
                        //                            homogenityAndblankIsValidInRoll.Add(Convert.ToString(dr_valid["roll_id"]));
                        //                        }


                        //                    }
                        //                    dr_valid.Close();
                        //                }
                        //                else
                        //                {
                        //                    dr_valid.Close();
                        //                    Trace.TraceWarning("No Valid result from lot_id={0} and roll_id={1} in Homogenity ", Program.SelectedLotToMeasure, roll);
                        //                }
                        //            }
                        //        }
                        //        #endregion

                        //    } 
                        //#endregion
                        //    #endregion

                        //#endregion
                        //int tubes_in_homogenity_measure;
                        //    foreach (string niceRoll in homogenityAndblankIsValidInRoll)
                        //    {
                        //        using (NpgsqlCommand comm_valid_ids_together = new NpgsqlCommand(string.Format("select tube_count from homogenity_result where lot_id='{0}' and roll_id='{1}'and invalidate=false and remeasured=false and homogenity_is_valid=true and not_h62_is_valid=true and out_of_range_is_valid=true", Program.SelectedLotToMeasure, niceRoll), conn))
                        //        {
                        //            HomogenityValidTubeCount.Add(niceRoll,Convert.ToInt32(comm_valid_ids_together.ExecuteScalar()));
                        //           // snCountForHomogenity = HomogenityValidTubeCount * 4;
                        //        }
                        //        using (NpgsqlCommand comm_valid_ids_together_blank = new NpgsqlCommand(string.Format("select COUNT(sn) from blank_test_result where lot_id='{0}' and roll_id='{1}'and invalidate=false and code=170 and glu<>0", Program.SelectedLotToMeasure, niceRoll), conn))
                        //        {
                        //            BlankIsValidTubeCount = (Convert.ToInt32(comm_valid_ids_together_blank.ExecuteScalar())/2);
                        //            snCountForBlank = BlankIsValidTubeCount * 2;
                        //        }
                        //         HomogenityValidTubeCount.TryGetValue(niceRoll,out tubes_in_homogenity_measure);
                        //         for (int tube_id = 1; tube_id <= tubes_in_homogenity_measure; tube_id++)
                        //         {
                        //             CentralSNIDsForHomogenity.Add(Convert.ToString(tube_id));
                        //         }
                        //         for (int sn = 1; sn <= tubes_in_homogenity_measure; sn++)
                        //            {
                        //                Trace.TraceInformation("sn: {0}", sn);
                        //                using (NpgsqlCommand getBlankTubes = new NpgsqlCommand(string.Format("SELECT AVG(blank_test_result.glu) as AVG FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id where blank_test_result.glu!=0 and homogenity_test.invalidate=false and homogenity_test.strip_ok=True and blank_test_result.invalidate=False and homogenity_test.sn='{2}' and homogenity_test.roll_id='{1}' and homogenity_test.lot_id='{0}' and blank_test_result.code=777 and homogenity_test.invalidate=False and blank_test_result.invalidate=False", Program.SelectedLotToMeasure, niceRoll, sn), conn))
                        //                {
                        //                    Trace.TraceInformation("query: {0}", getBlankTubes.CommandText);
                        //                    using (NpgsqlDataReader dr = getBlankTubes.ExecuteReader())
                        //                    {
                        //                        if (dr.HasRows)
                        //                        {
                        //                            while (dr.Read())
                        //                            {
                        //                                if (dr["AVG"]!=DBNull.Value)
                        //                                {

                        //                                    HomogenityInRangeRollsIDs.Add(niceRoll);
                        //                                    HomogenityInRangeGlusCount.Add(g++);
                        //                                    g++;
                        //                                    HomogenityInRangeGlusValue.Add(Convert.ToDouble(dr["AVG"]));
                        //                                    HomogenityInRangeSNsIDs.Add(string.Format("{0}",sn));
                        //                                    Trace.TraceInformation("AVG: {0}", dr["AVG"]);
                        //                                    AllRolls.Add(niceRoll);
                        //                                    glusAVGs.Add(Convert.ToDouble(dr["AVG"]));

                        //                                }  
                        //                            }
                        //                            dr.Close();
                        //                            stripInTubes = new List<bool>();
                        //                        }
                        //                        else
                        //                        {
                        //                            Trace.TraceError("No result in Program central query: query: {0}", getBlankTubes);
                        //                            dr.Close();
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //                #region tubesAVG At BlankTest
                        //                   for (int serialn = 1; serialn <= snCountForBlank; serialn = serialn + 2)
                        //                {
                        //                        CentralSNIDForBlank.Add(Convert.ToString(serialn));
                        //                        Trace.TraceInformation("sn: {0}", serialn);

                        //                        //SELECT blank_test_result.code,blank_test_result.glu,blank_test_result.invalidate,homogenity_test.strip_ok,homogenity_test.fk_blank_test_result_id,homogenity_test.roll_id,homogenity_test.lot_id,homogenity_test.invalidate,homogenity_test.sn FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id where blank_test_result.glu!=0 and homogenity_test.strip_ok=True and blank_test_result.invalidate=False and homogenity_test.roll_id='1' and homogenity_test.lot_id='111111' and blank_test_result.code=777 
                        //                        using (NpgsqlCommand getTubes = new NpgsqlCommand(string.Format("SELECT AVG(blank_test_result.glu) as Nano FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id where blank_test_result.glu!=0 and homogenity_test.strip_ok=True and blank_test_result.invalidate=False and homogenity_test.roll_id='{1}' and homogenity_test.lot_id='{0}' and blank_test_result.code=777", Program.SelectedLotToMeasure, niceRoll, serialn), conn))
                        //                        {

                        //                            Trace.TraceInformation("query: {0}",getTubes.CommandText);
                        //                            using (NpgsqlDataReader dr = getTubes.ExecuteReader())
                        //                            {
                        //                                if (dr.HasRows)
                        //                                {
                        //                                    while (dr.Read())
                        //                                    {
                        //                                        Trace.TraceInformation("nanoAVG: {0}", dr["Nano"]);                                    
                        //                                        nanosAVGs.Add(Convert.ToDouble(dr["Nano"]));                                                               

                        //                                    }
                        //                                    dr.Close();
                        //                                }
                        //                                else
                        //                                {
                        //                                    Trace.TraceError("No result in Program central query: query: {0}", getTubes);
                        //                                    dr.Close();
                        //                                }
                        //                            }
                        //                        }
                        //                }

                        //                #endregion
                        //                   g = 0;
                        //            }//end of foreach     
                        //            rollids.Add(HomogenityInRangeRollsIDs);   
                        //            validRolls = HomogenityInRangeRollsIDs.Distinct<string>().ToList<string>();
                        //    }//end of while(condition=there is valid lot at blank
                        //    List<double> glus = new List<double>();
                        //List<double> nanos=new List<double>();                            

                        //glus.AddRange(glusAVGs);                           
                        //nanos.AddRange(nanosAVGs);                           

                        //   GetDiffereneceFromAVG(glus,AllRolls, HomogenityLimitAvg, BlankLimitAvg, CentralSNIDsForHomogenity,CentralSNIDForBlank,nanos,DiffereneceFromAVGBlank,DiffereneceFromAVGHomogenity);
                        ////   SelectTheBest24Tubes(Program.LOT_ID);
                        //   SelectTheBestTubes(CentralSNIDsForHomogenity, DiffereneceFromAVGHomogenity, DiffereneceFromAVGBlank); 
                        #endregion

                        GetRollNumberInLot();
                        GetSNCountInRoll();
                        DiscardTubes();
                        CalcAverageForEachSN();

                        CentralGrid = new CentralVialSelection();
                        OrderRollsByCVValues();

                        OrderBySNAveragesDifferenceRangeAverageEachRoll();//Fill CentralVialSelection.dataGridView01
                        CentralVialSelection.Instance.SaveTheCentralTubesAndFillTheAccuracyVialsForm();


                        CentralGrid.ShowDialog();


                        




                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Exception in getting central_lot.Exception:{0},\nMessage:{1}", ex.InnerException, ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                        Trace.TraceInformation("Central query finished, accuracy_vials_form will be shown");
                        Program.TubeCount = 24;//because 24 central tube need to be selected
                        FillMeasurementTable();
                        acc_vials_form.ShowDialog();
                    }

                }//End of NpgsqlConnection
            }//if Program.measureType==accuracy


            if (Program.measureType != "accuracy")
            {
                CreateCentralUIPanel();
            }
            else if (Program.measureType == "accuracy")
            {
                for (int i = 1; i <= 6; i++)
                {
                    BarcodeNumber = 0;
                    for (int j = 1; j <= 3; j++)
                    {
                        #region Set Icon to unmeasured
                        foreach (Control item in UserWindow.userTables.Controls)
                        {
                            if (item is UserPanel)
                            {
                                foreach (Control bt in item.Controls)
                                {
                                    if (bt is Button
                                        && bt.Name == "button1")
                                    {
                                        Image image1 = Properties.Resources._801;
                                        ((Button)bt).Image = image1;
                                    }
                                }
                            }
                        }

                        Trace.TraceInformation("Image is set to unmeasured");
                        acc_vials_form.LOT = Program.LOT_ID;
                        #endregion
                        Program.Round = j;
                        Program.Accuracy_sample_blood_vial_ID = string.Format("{0}", i);

                        MessageCompletedEventArgs.blood_vial_id = Program.Accuracy_sample_blood_vial_ID;

                        Trace.TraceInformation("Blood vial started");
                        if ((i == 1)
                            && (j == 1))
                        {
                            Trace.TraceInformation("CreateCentralUIPanel() method started");
                            CreateCentralUIPanel();
                        }
                        else
                            CreateAccuracyUIPanel();

                        Trace.TraceInformation("Program after CentralUIPanel() method, i:{0};j:{1}", i, j);
                    }
                }
            }//else if measureType=="accuracy"   
        }

        public static List<string> OrderedRollList = new List<string>();
        private static void OrderRollsByCVValues()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();

                    using (NpgsqlDataReader orderByCV = new NpgsqlCommand(string.Format("select roll_id from roll_result where lot_id='{0}' and roll_is_valid=true and invalidate=false order by roll_cv", Program.LOT_ID), conn).ExecuteReader())
                    {
                        if (orderByCV.HasRows)
                        {

                            while (orderByCV.Read())
                            {
                                OrderedRollList.Add(Convert.ToString(orderByCV["roll_id"]));
                            }
                        }
                        orderByCV.Close();
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    conn.Close();
                }

            }
        }

        private static void CheckValidHomogenityLots(bool condition_in)
        {
            if (!condition_in)
            {
                Trace.TraceError("Nincs valid lot a homogenity_result táblában!");

                throw new SqlNotFoundException("homogenity_result", string.Format("select DISTINCT lot_id from homogenity_result where homogenity_is_valid=true"), string.Empty);
            }
        }

        /// <summary>
        /// Sets in the O.S. the Default Decimal separator to '.'
        /// In case of postgres inserts from c# code, when a double (2.23) value wanna be stored
        /// then there will be problem if this separator is not '.' ==> (avg,sd,cv) values(1,23,2,34,0,34)  (more values than target columns)
        /// </summary>
        public static void SetDefaultDecimalSeparatorToDot()
        {
            Trace.TraceInformation("SetDefaultDecimalSeparatorToDot() In Program.cs");

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }
        #region New Central Vial Selection

        public static void GetRollNumberInLot()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();

                    using (NpgsqlCommand getCountOfRolls = new NpgsqlCommand(string.Format("SELECT MAX(homogenity_test.roll_id ) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_errors.not_h62_error=False and blank_test_errors.h62_error=False and blank_test_errors.error='' and blank_test_errors.early_dribble=False and blank_test_errors.device_replace=False and blank_test_result.invalidate=False and blank_test_result.code=777 and homogenity_test.invalidate=False and homogenity_test.lot_id='{0}' and homogenity_test.strip_ok=True and blank_test_result.glu <> 0 ", Program.LOT_ID), conn))
                    {
                        object res = null;
                        res = getCountOfRolls.ExecuteScalar();

                        if (res == null || res == DBNull.Value)
                        {
                            throw new Exception();
                        }
                        else
                            RollCount = Convert.ToInt32(res);
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    conn.Close();

                }
            }

        }
        public static void GetSNCountInRoll()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();
                    for (int i = 1; i <= Program.RollCount; i++)
                    {


                        using (NpgsqlCommand getCountOfSNs = new NpgsqlCommand(string.Format("SELECT MAX((homogenity_test.sn)::int ) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_errors.not_h62_error=False and blank_test_errors.h62_error=False and blank_test_errors.error='' and blank_test_errors.early_dribble=False and blank_test_errors.device_replace=False and blank_test_result.invalidate=False and blank_test_result.code=777 and homogenity_test.invalidate=False and homogenity_test.roll_id='{0}' and homogenity_test.lot_id='{1}' and homogenity_test.strip_ok=True and blank_test_result.glu <> 0 ", i, Program.LOT_ID), conn))
                        {
                            object res = null;
                            res = getCountOfSNs.ExecuteScalar();

                            if (res == null || res == DBNull.Value)
                            {
                                throw new Exception();
                            }
                            else
                                Program.SNCount.Add(i, Convert.ToInt32(res));
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    conn.Close();

                }
            }

        }
        /// <summary>
        /// Tubes with less than 4 valid measure will be discarded
        /// </summary>
        /// <param name="lot_in"></param>
        public static void DiscardTubes()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();
                    for (int i = 1; i <= Program.RollCount; i++)
                    {
                        for (int sn = 1; sn <= Program.SNCount[i]; sn++)
                        {

                            using (NpgsqlCommand getCountOfStrips = new NpgsqlCommand(string.Format("SELECT COUNT(blank_test_result.glu) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_errors.not_h62_error=False and blank_test_errors.h62_error=False and blank_test_errors.error='' and blank_test_errors.early_dribble=False and blank_test_errors.device_replace=False and blank_test_result.invalidate=False and blank_test_result.code=777 and homogenity_test.invalidate=False and homogenity_test.roll_id='{0}' and homogenity_test.lot_id='{1}' and homogenity_test.sn='{2}' and homogenity_test.strip_ok=True and blank_test_result.glu <> 0 ", i, Program.LOT_ID, sn), conn))
                            {
                                object res = null;
                                res = getCountOfStrips.ExecuteScalar();

                                if (res == null || res == DBNull.Value)
                                {
                                    throw new Exception();
                                }
                                else
                                    CheckStripCountInTube(Convert.ToInt32(res), sn, i);
                            }
                        }
                        AvailableTubes.Add(i, ValidSNIDs);
                        ValidSNIDs = new List<int>();
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    conn.Close();

                }
            }


        }

        public static void CalcAverageForEachSN()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();
                    for (int i = 1; i <= Program.RollCount; i++)
                    {
                        foreach (int sn in AvailableTubes[i])
                        {



                            using (NpgsqlCommand getAverageOfSNs = new NpgsqlCommand(string.Format("SELECT AVG(blank_test_result.glu) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_errors.not_h62_error=False and blank_test_errors.h62_error=False and blank_test_errors.error='' and blank_test_errors.early_dribble=False and blank_test_errors.device_replace=False and blank_test_result.invalidate=False and blank_test_result.code=777 and homogenity_test.invalidate=False and homogenity_test.roll_id='{0}' and homogenity_test.lot_id='{1}' and homogenity_test.sn='{2}' and homogenity_test.strip_ok=True and blank_test_result.glu <> 0 ", i, Program.LOT_ID, sn), conn))
                            {
                                object res = null;
                                res = getAverageOfSNs.ExecuteScalar();

                                if (res == null || res == DBNull.Value)
                                {
                                    throw new Exception();
                                }
                                else
                                {

                                    AvgS.Add(sn, Math.Abs(RANGE_MEDIAN - Convert.ToDouble(res)));

                                }
                            }
                        }//end of foreach
                        SN_AVGs.Add(i, AvgS);
                        AvgS = new Dictionary<int, double>();
                    }//end of for
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    conn.Close();

                }
            }

        }

        public static void OrderBySNAveragesDifferenceRangeAverageEachRoll()
        {

            for (int roll = 1; roll <= Program.RollCount; roll++)
            {
                Rolls.Add(roll);
            }
            foreach (int roll in Rolls)
            {
                foreach (KeyValuePair<int, double> item in SN_AVGs[roll].OrderBy(key => key.Value)) //Mr. Prankster-Prankster!-StoryBook-Gangster....
                {
                    SN_AVGs[roll].Remove(item.Key);
                    CentralVialSelection.Instance.AddRowToDGV(new object[] { Program.LOT_ID, roll, item.Key, Math.Round(item.Value, 4) });
                    centralSelectedCount++;
                    break;

                }
            }
            //Ohhh my brother...nothing serious, but I will go by another
            //Cause I make a mill here, make a mill ther, fuck a bitch here, then fuck a bitch there-YEAH!
            while (centralSelectedCount != 24)
            {
                foreach (string roll in OrderedRollList)
                {
                    foreach (KeyValuePair<int, double> item in SN_AVGs[Convert.ToInt32(roll)].OrderBy(key => key.Value))
                    {
                        SN_AVGs[Convert.ToInt32(roll)].Remove(item.Key);
                        if (centralSelectedCount >= 24)
                        {
                            //Gold around his neck 24k, he has it, bitches sucking his dick in 24/7
                            //Hurray !!! We have the 24 tubes for the accuracy test!! yeah...
                            return;
                        }

                        CentralVialSelection.Instance.AddRowToDGV(new object[] { Program.LOT_ID, roll, item.Key, Math.Round(item.Value, 4) });
                        centralSelectedCount++;
                        break;

                    }
                }


            }
            //}


        }

        public static int centralSelectedCount = 0;
        public static List<int> Rolls = new List<int>();
        #region Dictionaries

        public static Dictionary<int, Dictionary<int, double>> SN_AVGs = new Dictionary<int, Dictionary<int, double>>();
        public static Dictionary<int, double> AvgS = new Dictionary<int, double>();

        //<roll_id,sn_id>
        public static Dictionary<int, List<int>> DiscardedTubes = new Dictionary<int, List<int>>();
        //<roll_id,sn_id>
        public static Dictionary<int, List<int>> AvailableTubes = new Dictionary<int, List<int>>();

        #endregion
        private static void CheckStripCountInTube(int stripCount_in, int sn_id, int roll_id)
        {
            if (stripCount_in != 4)
            {
                InValidSNIDs.Add(sn_id);
            }
            else
            {
                ValidSNIDs.Add(sn_id);


            }
        }
        public static List<int> ValidSNIDs = new List<int>();
        public static List<int> InValidSNIDs = new List<int>();

        public const double RANGE_MEDIAN = 6.35;
        //<RollNumber,SNCount>
        public static Dictionary<int, int> SNCount = new Dictionary<int, int>();

        #endregion




        #region Variables
        public static IEnumerable<string> allSN = new List<string>();
        public static int InOneRoundAccuracyStripCount;
        public const string MEO_LEADER_ACCESSRIGHT = "meo_leader";
        public const string ADMIN_ACCESSRIGHT = "admin";
        public static int RollCount;
        private static int operator_pk;
        private static int testType_pkid;
        private static CentralVialSelection CentralVialDataGrid;
        private static CentralVialSelection CentralGrid;
        public static int MeasurementActualPkId;
        private static int accuracy_central_bias_pk_id;
        private static int accuracy_lot_result_pk_id;
        private static int accuracy_result_central_pk_id;
        private static int accuracy_result_master_pk_id;
        private static int accuracy_results_pk_id;
        private static int accuracy_test_pk_id;
        private static int accuracy_values_pk_id;
        private static int blank_test_averages_alternation_pk_id;
        private static int blank_test_averages_pk_id;
        private static int blank_test_environment_pk_id;
        private static int blank_test_errors_pk_id;
        private static int blank_test_identify_pk_id;
        private static int blank_test_result_pk_id;
        private static int central_vials_pk_id;
        private static int device_meo_pk_id;
        private static int global_sql_tables_pk_id;
        private static int homogenity_result_alternation_pk_id;
        private static int homogenity_result_pk_id;
        private static int homogenity_test_pk_id;
        private static int lot_result_modified_pk_id;
        private static int lot_result_pk_id;
        private static int measure_type_pk_id;
        private static int measurement_pk_id;
        private static int roll_result_pk_id;
        private static int test_type_pk_id;
        private static int operators_pk_id;
        private static int serial_input_pk_id;
        public static bool LoggedToMeasurement; 
        #endregion


        /// <summary>
        /// CreateAccuracyUIPanel
        /// </summary>
        private static void CreateAccuracyUIPanel()
        {
            CreateTablePanel();

            main = new MainWindow();
            counter = new CounterPanel(Program.TubeCount);
            Trace.TraceInformation("Panels creation started");
            CreateTablePanelForUser();
            user = new UserWindow();
            Trace.TraceInformation("MainPanel showed");
            // worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            InitAllPortForSecondRun();
            user.Visible = false;
            user.ShowDialog();

        }

        /// <summary>
        /// InitAllPortForSecondRun
        /// </summary>
        private static void InitAllPortForSecondRun()
        {
            SerialPort port;
            string[] ports = new string[20];
            portnumber = 0;
            if (Properties.Settings.Default.IsVirtualPorts)
            {
                ports = new string[] { "COM20", "COM21", "COM22", "COM23", "COM24", "COM25", "COM26", "COM27" };
                Program.portnumber = ports.Length;
            }
            else if (Properties.Settings.Default.OpenAllPorts)
            {
                ports = SerialPort.GetPortNames();
                Program.PortCount = ports.Length;
            }
            else
            {

                ports = new string[] { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10", "COM11", "COM12", "COM13", "COM14", "COM15", "COM16" };
                Program.PortCount = ports.Length;
            }
            List<Device> deviceList = new List<Device>();
            Dictionary<UserPanel, string> panelnames = new Dictionary<UserPanel, string>();

            panelForUser = new UserPanel[20];
            foreach (string item in ports)
            {

                port = new SerialPort(item);
                UserWindow.userTables.Controls[portnumber].Controls["richTextBox1"].Text = string.Empty;

                if (Convert.ToInt32(port.PortName.Substring(3)) > 8)
                {
                    UserWindow.userTables.Controls[portnumber].Controls["tbBarcode"].Text = Program.LOT_ID;
                    UserWindow.userTables.Controls[portnumber].Controls["tbBarcode"].Enabled = false;
                    if (snAndRollID.Count > BarcodeNumber)
                    {
                        UserWindow.userTables.Controls[portnumber].Controls["tbBarcode2"].Text = snAndRollID[BarcodeNumber];
                        UserWindow.userTables.Controls[portnumber].Controls["tbBarcode2"].Enabled = false;
                        BarcodeNumber++;
                    }
                    else
                        Trace.TraceError("BarcodeNumber:{0};portnumber:{1}", BarcodeNumber, portnumber);
                }
                else if (Convert.ToInt32(port.PortName.Substring(3)) <= 8)
                {
                    UserWindow.userTables.Controls[portnumber].Controls["tbBarcode"].Text = Program.master_lot_id;
                    UserWindow.userTables.Controls[portnumber].Controls["tbBarcode"].Enabled = false;
                }
                portnumber++;
            }

        }

        /// <summary>
        /// Create AllPanel for Accuracy
        /// </summary>
        private static void CreateAllPanelForAccuracy()
        {
            Trace.TraceInformation("CreateAllPanelForAccuracy() started");
            CreateTablePanelForUser();
            user = new UserWindow();
            user.ShowDialog();
            user.BringToFront();
        }

        /// <summary>
        /// Create central panels and initAllPort
        /// </summary>
        private static void CreateCentralUIPanel()
        {
            CreateTablePanel();

            main = new MainWindow();
            counter = new CounterPanel(Program.TubeCount);
            Trace.TraceInformation("Panels creation started");
            CreateTablePanelForUser();
            user = new UserWindow();
            Trace.TraceInformation("MainPanel showed");
            // worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            InitAllPort();

            if (Program.measureType != "accuracy")
            {

                foreach (Control item in UserWindow.userTables.Controls)
                {
                    if (item is UserPanel)
                    {
                        foreach (var c in item.Controls)
                        {
                            if (c is TextBox)
                            {
                                if (((TextBox)c).Name == "tbBarcode")
                                {
                                    ((TextBox)c).Text = Program.LOT_ID;
                                    ((TextBox)c).Enabled = false;
                                }
                            }
                        }
                    }
                }
            }
            user.Visible = false;
            user.ShowDialog();
        }

        public static string AccessRight { get; set; }

        public static bool IsLotModified { get; set; }
    }
}
