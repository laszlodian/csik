using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using e77.MeasureBase.MeasureEnvironment;
using System.Windows.Forms;
using Npgsql;

namespace e77.MeasureBase
{
   public class TraceHelper : IDisposable
    {
        const string FILE_LISTENER_NAME = "77E_FileListener";
        const string TRACE_FILE_NAME_END = "trace";
        const string LAST_TRACE_FILE_NAME = "last_" + TRACE_FILE_NAME_END;

        const string TRACE_FILE_EXTENSION = "txt";

        const string GLOBAL_TRACE_DIR_NAME = "trace";

#region Singleton 
		static TraceHelper _theOnlyOne = new TraceHelper();
        
        protected internal static TraceHelper TheOnlyOne
        {
            get { return TraceHelper._theOnlyOne; }
        }
         
	    private TraceHelper() {; }
#endregion

        string _TraceFileDirectory = string.Empty;
        bool _FileListenerActive;

        bool _SaveTraceFile; //see MarkTraceFileForSave()
        string SaveReason { get; set; }

        /// <summary>
        /// Marks trace file for create a copy with timestamp as file name, in order to store the current trace file (e.g. in case of unhandled exception, we can store the trace)
        /// This function closes the listener, in order to copy the file
        /// </summary>
        /// <param name="reason_in">will be traced and used for trace file name</param>
        static public void MarkTraceFileForStore(string reason_in)
        {
            if (Model.MeasureCollectionBase.TheMeasures != null)
                Trace.TraceInformation(Model.MeasureCollectionBase.TheMeasures.DetailedTraceInfo);

            TheOnlyOne._SaveTraceFile = true;
            TheOnlyOne.SaveReason = reason_in;
        }

        static public void MarkTraceFileForStore(string reason_in, bool immediateSave_in)
        {
            MarkTraceFileForStore(reason_in);
            if(immediateSave_in)
                TheOnlyOne.SaveTrace();
        }

        static public bool HasTraceFileMarkedForStore
        {
            get { return TheOnlyOne._SaveTraceFile; }
        }

       static public void TraceApart(Exception ex,object classInstance)
       {
           Trace.TraceError("Exception at {0} ex.Message: {1}", classInstance.GetType().Name,ex.Message);
           Trace.TraceError("Exception at {0} ex.Data: {1}", classInstance.GetType().Name, ex.Data);
           Trace.TraceError("Exception at {0} ex.InnerException: {1}", classInstance.GetType().Name, ex.InnerException);
           Trace.TraceError("Exception at {0} ex.StackTrace: {1}", classInstance.GetType().Name, ex.StackTrace);
           Trace.TraceError("Exception at {0} ex.Source: {1}", classInstance.GetType().Name, ex.Source);

       }

       static public void TraceApart( NpgsqlCommand sqlStatement, object classInstance)
       {
           Trace.TraceError("Exception at {0} SqlCommand CommandText: {1}", classInstance.GetType().Name, sqlStatement.CommandText);
           Trace.TraceError("Exception at {0} SqlCommand Connection: {1}", classInstance.GetType().Name, sqlStatement.Connection);
           Trace.TraceError("Exception at {0} SqlCommand LastInsertedOID: {1}", classInstance.GetType().Name, sqlStatement.LastInsertedOID);
           Trace.TraceError("Exception at {0} SqlCommand Parameters: {1}", classInstance.GetType().Name, sqlStatement.Parameters);
           Trace.TraceError("Exception at {0} SqlCommand UpdatedRowSource: {1}", classInstance.GetType().Name, sqlStatement.UpdatedRowSource);

       }

        public static string TraceFileDirectory
        {
            get { return TheOnlyOne._TraceFileDirectory; }
        }
        
        public bool FileListenerActive
        {
            get { return _FileListenerActive; }
        }

        public TraceOptions TraceOutputOptions
        {
            set
            {
                foreach (TraceListener l in Trace.Listeners)
                    l.TraceOutputOptions = value;
            }
        }

        /// <summary>
        /// creates TextWriterTraceListener, to file "%MyDocuments%\{ApplicationName}\last_trace.txt"
        /// </summary>
        /// <returns></returns>
        static public bool SetupListener()
        {
            return TheOnlyOne.SetupListener_NoneStatic(string.Empty);
        }

        /// <summary>
        /// creates TextWriterTraceListener, to file "%MyDocuments%\{ApplicationName}\{subDir_in}\last_trace.txt"
        /// subDir_in is optional, and only one livel is supported
        /// Creates directories automatically.
        /// 
        /// You can modify TraceOptions by e.g. TraceHelper.TraceOutputOptions = TraceOptions.Timestamp;
        /// 
        /// -Stores the last 20 trace
        /// -Stores trace automatically if exception has been occured
        /// -Deletes trace files older than 3 mounths
        /// 
        /// </summary>
        /// <param name="subDir_in">optional subdirectory</param>
        /// <returns>true - OK</returns>
        static public bool SetupListener(string subDir_in)
        {
            return TheOnlyOne.SetupListener_NoneStatic(subDir_in);
        }

        private bool SetupListener_NoneStatic(string subDir_in)
        {
            bool res = true;
            bool listenerAdded = false;

            if(subDir_in.Contains('\\') || subDir_in.Contains('/'))
                throw new ArgumentException( string.Format("Only one subrir is supported now. ({0})",
                    subDir_in));

            try
            {
                Trace.AutoFlush = true; //without this there will be no items added after the first Flush() call (as I see)

                //application dir:
                string directory = CreateDirectory(
                                System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                                System.Reflection.Assembly.GetEntryAssembly().GetName().Name);
                
                //subdir:
                directory = CreateDirectory(directory, subDir_in);

                string logFileFullName = string.Format("{0}\\{1}", directory, LAST_TRACE_FILE_NAME);

                KeepLastTraces(logFileFullName); //can throw already using exception (by another instance of this application.)

                logFileFullName = string.Format("{0}.{1}", logFileFullName, TRACE_FILE_EXTENSION);

                //open last trace overwrite
                TextWriterTraceListener listener = new TextWriterTraceListener(
                    new StreamWriter(logFileFullName, false,
                        new UTF8Encoding(true)), FILE_LISTENER_NAME);
                
                                                
                Trace.Listeners.Add(listener); //file created here
                listenerAdded = true;

                Trace.TraceInformation(string.Format("{2} Started At {0} {1}",
                    DateTime.Now.ToShortDateString(),
                    DateTime.Now.ToShortTimeString(),
                    System.Reflection.Assembly.GetEntryAssembly().GetName().Name));

                _TraceFileDirectory = directory;
                _FileListenerActive = true;

                DeteteOldFiles();
                
            }
            catch (Exception e)
            {
                //remove the file listener:
                if (listenerAdded)
                    Trace.Listeners.Remove(FILE_LISTENER_NAME);

                Trace.TraceWarning( "Cannot create file listener.\n{0}",
                    e.ReportError(true));

                res = false;
            }
            return res;
        }

        private static string CreateDirectory(string directory_in, string subDir_in)
        {
            if (subDir_in != string.Empty)
            {
                directory_in = string.Format("{0}\\{1}", directory_in, subDir_in);
                if (!Directory.Exists(directory_in))
                    Directory.CreateDirectory(directory_in);
            }
            return directory_in;
        }

        /// <summary>
        /// Sores last traces with '-{last nr}' postfix, e.g. -2 = last 2nd trace
        /// </summary>
        /// <param name="logFileFullName_in"></param>
        private void KeepLastTraces(string logFileFullName_in)
        {
            const int keepLastTraceNr = 20;

            //Delete last: 
            if (File.Exists(GetFullFileNameOf(logFileFullName_in, keepLastTraceNr)))
                File.Delete(GetFullFileNameOf(logFileFullName_in, keepLastTraceNr));

            //Copy the list of traces (from  -1 to -keepLastTraceNr-1 (-19))
            for(int i = keepLastTraceNr-1; i > 0; i--)
            {
                FileInfo fi = new FileInfo(GetFullFileNameOf(logFileFullName_in, i));
 
                if (fi.Exists)
                    fi.MoveTo(GetFullFileNameOf(logFileFullName_in, i + 1));
            }

            //Copy LastTrace to LastTrace-1
            FileInfo fiLast = new FileInfo(string.Format("{0}.{1}", logFileFullName_in, TRACE_FILE_EXTENSION));

            if (fiLast.Exists)
                fiLast.MoveTo(GetFullFileNameOf(logFileFullName_in, 1)); //can throw already using exception (by another instance of this application.)
        }

        private string GetFullFileNameOf(string logFileFullName_in, int postfix_in)
        {
            return string.Format("{0}-{1}.{2}", logFileFullName_in, postfix_in.ToString("D2"), TRACE_FILE_EXTENSION);
        }
               
        /// <summary>
        /// Create a copy with timestamp as file name, in order to store the current trace file (e.g. in case of unhandled exception, we can store the trace)
        /// </summary>
        private void SaveTrace()
        {
            if (_FileListenerActive) //means was active here, because the listener is finalized when the Dispose called by the framework
            {
                string inputFileName = string.Format("{0}\\{1}.{2}", _TraceFileDirectory, LAST_TRACE_FILE_NAME, TRACE_FILE_EXTENSION);

                if(File.Exists(inputFileName))
                {
                    //store current trace into user dir:
                    try
                    {
                        File.Copy(inputFileName,
                            string.Format("{0}\\{1}_{2}.{3}",
                                    _TraceFileDirectory,
                                    DateTime.Now.ToString("yyyyMMddHHmmss"), SaveReason, TRACE_FILE_EXTENSION));
                    }
                    catch (Exception) 
                    {
                        //MessageBox.Show(ex.Message);
                    }

                    //try to store current trace into global trace dir:
                    //1. try to find global 'trace' dir
                    string directory = System.Reflection.Assembly.GetEntryAssembly().Location;
                    while(directory.LastIndexOf('\\') != -1)
                    {
                        directory = directory.Substring(0, directory.LastIndexOf('\\')); //get parent
                        if(Directory.Exists( string.Format("{0}\\{1}", directory, GLOBAL_TRACE_DIR_NAME)))
                        {
                            //trace dir founded
                            try
                            {
                                File.Copy(inputFileName,
                                    string.Format("{0}\\{1}\\{2}_{3}_{4}_{5}.{6}", 
                                        directory, GLOBAL_TRACE_DIR_NAME,
                                        System.Reflection.Assembly.GetEntryAssembly().ManifestModule,
                                        e77User.TheUser == null ? string.Empty : e77User.TheUser.Name,
                                        DateTime.Now.ToString("yyyyMMddHHmmss"), 
                                        SaveReason, 
                                        TRACE_FILE_EXTENSION));
                            }
                            catch(Exception ex) 
                            {
                                MessageBox.Show(ex.Message);
                            }
                            break;
                        }
                    }                    
                }

                _SaveTraceFile = false;
                SaveReason = null;
            }
        }

        #region Delete Old Files
        Thread _deleteOldFilesThread;
        void DeteteOldFiles()
        {
            _deleteOldFilesThread = new Thread(new ThreadStart(DeleteOldFilesThread));
            _deleteOldFilesThread.IsBackground = true;
            _deleteOldFilesThread.Start();
        }

        void DeleteOldFilesThread()
        {
            try
            {
                var query = Directory.GetFiles(_TraceFileDirectory, string.Format("*.{0}", TRACE_FILE_EXTENSION))
                    .Where(item => DateTime.Now - File.GetLastWriteTime(item) > new TimeSpan(3 * 30, 0, 0, 0, 0));

                foreach (string oldFile in query)
                {
                    try
                    {
                        File.Delete(oldFile);
                    }
                    catch (Exception e)
                    {
                        Trace.TraceInformation("Can't delete file {0}. Ecxeption: {1}", oldFile, e.Message);
                    }
                }

                Trace.TraceInformation("DeleteOldFiles finished, deleted files = {0}.", query.ItemsToString());
            }
            catch (Exception e)
            {
                Trace.TraceWarning( "Exception at DeteteOldFilesThread:{0}", e.ReportError());
            }
        } 
        #endregion
   

#region base Dispose pattern       
        //there is no need to dispose, it is for save a copy of the trace file at the end of the application

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if (disposing) 
                { //managed resource:
                }

                if (_FileListenerActive && _SaveTraceFile)
                    SaveTrace();

                disposed = true;
            }
        }

        ~TraceHelper()
        {
            Dispose(false);
        }

        #endregion
    }
}
