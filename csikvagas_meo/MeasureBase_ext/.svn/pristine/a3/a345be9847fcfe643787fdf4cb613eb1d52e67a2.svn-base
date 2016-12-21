using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using e77.MeasureBase.Properties;
using System.Windows.Threading;

namespace e77.MeasureBase.GUI
{
    public static class GuiErrorHandling
    {
        
        /// <summary>
        /// Example:
        ///     AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GuiErrorHandling.DefaultUnhandledExceptionEventHandler);
        ///     Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(GuiErrorHandling.DefaultUnhandledExceptionEventHandler);
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void DefaultUnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
        {
            DefaultUnhandledExceptionEventHandler(sender, new System.Threading.ThreadExceptionEventArgs((Exception)e.ExceptionObject));
        }

        /// <summary>
        /// Unhandled exception catching in WPF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <sample>
        /// AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GuiErrorHandling.DefaultUnhandledExceptionEventHandler);
        /// this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(GuiErrorHandling.DefaultUnhandledExceptionEventHandler);
        /// </sample>
        public static void DefaultUnhandledExceptionEventHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            DefaultUnhandledExceptionEventHandler(sender, new System.Threading.ThreadExceptionEventArgs((Exception)e.Exception));
            e.Handled = true;
        }

        /// <summary>
        /// Example:    Application.ThreadException += GuiErrorHandling.DefaultUnhandledExceptionEventHandler;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void DefaultUnhandledExceptionEventHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Trace.TraceError("Unhandled exception:");
            Trace.TraceError(e.Exception.ReportError(true));

            MessageBox.Show(e.Exception.Message, Resources.ERROR_HAPPEND, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }


    }
}
