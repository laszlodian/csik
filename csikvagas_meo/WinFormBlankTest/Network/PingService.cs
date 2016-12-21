using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using e77.MeasureBase;
using Npgsql;


namespace WinFormBlankTest.Network
{
    public class PingService : ServiceBase
    {

        #region Variables
        public Thread threadTask;
        public readonly string releaseConnStr = Properties.Settings.Default.DBReleaseConnection;
        public int reloadConfigFrequency=Properties.Settings.Default.ReloadConfigFrequency;
        public static PingService Instance;
        public DataTable dtHosts = new DataTable();      // Hosts to be checked 
        private int runnerCounter=0;
        private object data = null;
        private bool _isNetworkOnline=false;
        private bool hostStatusResult;
        public List<bool> StatusList=new List<bool>();
        private bool currentStatus;
        private bool statusStored=false;

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PingService()
        {
            Instance = this;

            NetworkChange.NetworkAvailabilityChanged+= new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
            _isNetworkOnline = NetworkInterface.GetIsNetworkAvailable();
            
            OnStart();
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            _isNetworkOnline = e.IsAvailable;
        }

        /// <summary>
        /// Starts the MainTask() function in a
        /// seperatly thread
        /// </summary>
        /// <param name="args"></param>
        protected void OnStart()
        {
            RefreshSettings();

            threadTask = new Thread(MainTask);
            threadTask.SetApartmentState(ApartmentState.MTA);      
            threadTask.Priority=ThreadPriority.Highest;
            threadTask.Name="PingThread";
            threadTask.ManagedThreadId.AddWithValue(Properties.Settings.Default.ThreadID);
            threadTask.DisableComObjectEagerCleanup();
            ExecutionContext executionContext=ExecutionContext.Capture();
            //Start thread
            threadTask.Start();

           
            while (threadTask.IsAlive)
            {
                

                switch (threadTask.ThreadState)
                {
                    case ThreadState.AbortRequested:
                     //   threadTask.Abort();
                      //  OnStop();
                        break;
                    case ThreadState.Aborted:
                      //  OnStart();
                        break;
                    case ThreadState.Background:
                        break;
                    case ThreadState.Running:
                     //   CompressedStack.Capture();
                     //   CompressedStack compressedStack=CompressedStack.GetCompressedStack().CreateCopy();
                        //runnerCounter++;
                       
                        break;
                    case ThreadState.StopRequested:
                        OnStop();
                        break;
                    case ThreadState.Stopped:
                     //   if (runnerCounter < 10 && !threadTask.IsAlive)
                    //        OnStart();
                        //else 
                        //   continue;
                                                   
                        break;
                    case ThreadState.SuspendRequested:
                     //       threadTask.Suspend();
                           
                        break;
                    case ThreadState.Suspended:
                      //  threadTask.Resume();
                        break;
                    case ThreadState.Unstarted:
                     //   OnStart();
                        break;
                    case ThreadState.WaitSleepJoin:
                      //  threadTask.Interrupt();
                        break;
                    default:
                        break;
                }
            }
        }

       


        /// <summary>
        /// This task will be started as a seperatly thread
        /// </summary>
        private void MainTask()
        {
            while (runnerCounter <= 10)
            {
                TryPing();
                runnerCounter++;
                // Sleep 900ms to be sure at least one Tick happens on each second
                Thread.Sleep(900);
            }
            OnStop();
        }

        /// <summary>
        /// Shutdowns the started thread about network check
        /// </summary>
        protected override void OnStop()
        {
            threadTask.Abort();
            
            if (_isNetworkOnline && CheckFinalResult(Properties.Settings.Default.HostToCheckNetwork))
            {
                MessageCompletedEventArgs.networkIsOK = true; 
            }else
                MessageCompletedEventArgs.networkIsOK = false; 
        }

        private bool CheckFinalResult(string host_in)
        {
            using (NpgsqlConnection cnn = new NpgsqlConnection(Properties.Settings.Default.DBLocal))
            {
                try
                {
                    cnn.Open();

                    using (NpgsqlDataReader getStatus = new NpgsqlCommand(string.Format("SELECT status FROM ping_log WHERE host='{0}'", host_in), cnn).ExecuteReader())
                    {

                        if (getStatus.HasRows)
                        {
                            while (getStatus.Read())
                            {

                                StatusList.Add(Convert.ToBoolean(getStatus["status"]));

                            }
                        }
                        getStatus.Close();

                        foreach (bool item in StatusList)
                        {
                            if (!item)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Exception: {0}", ex.Message));
                    TraceHelper.TraceApart(ex, this.GetType().Name);

                }
                finally
                {
                    cnn.Close();

                }
            }
            return false;
        }

        private void TryPing()
        {
            // CurrentSecond is the number of seconds passed after midnight
            long CurrentSecond = (long)DateTime.Now.Subtract(DateTime.Today).TotalSeconds;

            // Eventually reload configuration
            if (CurrentSecond % reloadConfigFrequency == 0)
            {
                RefreshSettings();
            }

            // Analyze the list of hosts to be pinged 
            foreach (DataRow dr in dtHosts.Rows)
            {
                // Check if time is come to ping the specific host
                //if (CurrentSecond % (int)dr["ping_freq"] == 0)
                //{
                    // Ping the specific host
                    string Host = Convert.ToString(dr["host"]);
                    bool IsAlive = HostIsAlive(Convert.ToString(dr["host"]));
                    bool LastStatus;
                    if (statusStored)
                    {
                        LastStatus= HostLastStatus(Host, currentStatus && _isNetworkOnline);
                    }
                  

                    //if (LastStatus == false || (LastStatus == true & !IsAlive) || (LastStatus == false & IsAlive))
                    //{
                        StoreStatusTransition(Host, IsAlive);
                        statusStored = true;
                    //}
                //}

            }

        }
        /// <summary>
        /// Check host is alive with a
        /// simple ping command
        /// </summary>
        /// <param name="host_in"></param>
        /// <returns></returns>
        private bool HostIsAlive(string host_in)
        {
            Ping pingSender = new Ping();
            PingReply reply;
            try
            {
                reply = pingSender.Send(host_in);
                if (reply.Status == IPStatus.Success)
                {
                    currentStatus = true;
                    return true;
                }
                else
                {
                    currentStatus = false;
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Returns the last(previous) status of the "pinging host"
        /// command result
        /// </summary>
        /// <param name="host_in"></param>
        /// <param name="status_in"></param>
        /// <returns></returns>
        private bool HostLastStatus(string host_in, bool status_in)
        {

            bool lastStatus = false;


            using (NpgsqlConnection cnn = new NpgsqlConnection(Properties.Settings.Default.DBLocal))
            {
                try
                {
                    cnn.Open();

                    using (NpgsqlCommand getStatus = new NpgsqlCommand(string.Format("SELECT status FROM ping_log WHERE host='{0}' ORDER BY recording_date DESC", host_in), cnn))
                    {
                        object obj = null;
                        obj = getStatus.ExecuteScalar();

                        if (obj == null || obj == DBNull.Value)
                        {
                            MessageBox.Show(string.Format("obj is null at: {0}", getStatus.CommandText));
                            throw new Exception(string.Format("obj is null at: {0}", getStatus.CommandText));
                        }
                        else
                            lastStatus = Convert.ToBoolean(obj);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Exception: {0}", ex.Message));
                    TraceHelper.TraceApart(ex, this.GetType().Name);

                }
                finally
                {
                    cnn.Close();

                }
                return lastStatus;
            }


        }

        /// <summary>
        /// Stores the actual host reachchable status to database
        /// for further checking
        /// </summary>
        /// <param name="host_in"></param>
        /// <param name="isAlive_in"></param>
        private void StoreStatusTransition(string host_in, bool isAlive_in)
        {
            using (NpgsqlConnection storeStatus = new NpgsqlConnection(Properties.Settings.Default.DBLocal))
            {
                try
                {
                    storeStatus.Open();

                    using (NpgsqlCommand cm = new NpgsqlCommand(string.Format("INSERT INTO ping_log(host, status, recording_date) VALUES ('{0}', {1}, {2})", host_in, isAlive_in, "@date"), storeStatus))
                    {
                        object res = null;
                        cm.Parameters.AddWithValue("@date", DateTime.Now);
                        res = cm.ExecuteNonQuery();

                        if (res == null || res == DBNull.Value)
                        {

                            MessageBox.Show("unsuccessfull insert to ping");
                        }
                        else
                            TraceHelper.TraceApart(cm, Instance);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                finally
                {

                    storeStatus.Close();
                }

            }


        }



        private void RefreshSettings()
        {
            reloadConfigFrequency = Properties.Settings.Default.ReloadConfigFrequency;

            NpgsqlDataAdapter daHosts = new NpgsqlDataAdapter("SELECT host,ping_freq FROM hostlist WHERE do_ping=True AND is_host=True", Properties.Settings.Default.DBLocal);

            dtHosts.Clear();

            daHosts.Fill(dtHosts);
        }

        private void InitializeComponent()
        {
            // 
            // PingService
            // 
            this.ServiceName = "PingService";
        }
    }
}
