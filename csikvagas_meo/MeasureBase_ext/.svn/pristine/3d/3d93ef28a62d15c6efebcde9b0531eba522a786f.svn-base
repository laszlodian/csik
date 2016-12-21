using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using e77.MeasureBase;
using System.Windows.Forms;
using e77.MeasureBase.Properties;

namespace e77.MeasureBase.e77Console
{
    /// <summary>
    /// Generall exit codes for Console-based measures
    /// </summary>
    public enum e77ConsoleExitCode
    {
        /// <summary>
        /// result has been stored into SQL, and is is OK
        /// </summary>
        MeasureOK = 1,

        /// <summary>
        /// result has been stored into SQL, and is is Failed
        /// </summary>
        MeasureFailed = 2,

        /// <summary>
        /// There is no any SQL
        /// </summary>
        Cancelled = 0x100,

        /// <summary>
        /// Internall measure SW Error
        /// </summary>
        InternalError = 0x101
    }

    public static class EnumExtensions
    {
        public static int ToInt(this Enum enumValue)
        {
            return (int)((object)enumValue);
        }
    }

    static public class ConsoleHelper
    {
        public static string PRESS_ANY_KEY = Resources.ConsoleHelper_PressAnyKey;

        static public void ClearInputBuffer()
        {
            while (Console.KeyAvailable)//empties the buffer
                Console.ReadKey();
        }

        public static void InverseColors()
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = Console.BackgroundColor;
            Console.BackgroundColor = c;
        }

        /// <summary>
        /// Write line to the console at position width colors
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="s"></param>
        /// <param name="ForegroundColor"></param>
        /// <param name="BackgroundColor"></param>
        public static void WriteLineTo(int x, int y, string s, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor)
        {
            Console.BackgroundColor = BackgroundColor;
            WriteLineTo(x, y, string.Format(s), ForegroundColor);
        }

        public static void WriteLineTo(int x, int y, string s, ConsoleColor ForegroundColor)
        {
            Console.ForegroundColor = ForegroundColor;
            WriteLineTo(x, y, string.Format(s));
        }

        public static void WriteLineTo(int x, int y, string s)
        {
            int width = x + s.Length +1;
            int height = y + 1;
            if (Console.BufferWidth < width) Console.BufferWidth = width;
            if (Console.BufferHeight < height) Console.BufferHeight = height;
            if (width > Console.LargestWindowWidth) width = Console.LargestWindowWidth;
            if (height > Console.LargestWindowHeight) height = Console.LargestWindowHeight;
            if (Console.WindowWidth < width) Console.WindowWidth = width;
            if (Console.WindowHeight < height) Console.WindowHeight = height;
            Console.CursorTop = y;
            Console.CursorLeft = x;
            Console.Write(string.Format(s));
        }

        /// <summary>
        /// Note: modifies cursor position to first char of the specified Top line!
        /// </summary>
        /// <param name="?">top_in</param>
        /// <param name="?">button_in</param>
        public static void ClearArea(int top_in, int button_in)
        {
            StringBuilder emptyLine = new StringBuilder(Console.WindowWidth);
            for (int i = 0; i < Console.WindowWidth; i++)
                emptyLine.Append(' ');

            for (int i = button_in; i >= top_in; i--)
            {
                Console.CursorTop = i;
                Console.CursorLeft = 0;
                Console.Write(emptyLine.ToString());
            }

            Console.CursorLeft = 0;
            Console.CursorTop = top_in;
        }

                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="result_out"></param>
        /// <param name="?"></param>
        /// <returns>true = OK</returns>
        static public bool GetUserInputInt(string message, ref int result_out,
            out bool noAnswareFlag_out, out bool badAnswareFlag_out)
        {
            noAnswareFlag_out = false;
            badAnswareFlag_out = false;
            ConsoleHelper.ClearInputBuffer();

            if (message != null || message != string.Empty)
                ConsoleHelper.InfoMessage(message);

            Console.Write(">");
            string strRes = Console.ReadLine();
            Console.WriteLine();

            if (strRes == null)
                strRes = string.Empty;
            else
                strRes = strRes.Trim();

            if(strRes == string.Empty)
            {
                noAnswareFlag_out = true;
                return false;
            }

            if (int.TryParse(strRes.Trim(), out result_out))
            {
                return true;
            }

            badAnswareFlag_out = true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="minLength">0-no minimal lenght</param>
        /// <returns></returns>
        static public string GetUserInputString(string message_in, int minLength_in)
        {
            ConsoleHelper.ClearInputBuffer();
            Trace.TraceInformation("Console Info Out: '{0}'", message_in);
            Console.Write(message_in);

            string res;
            do
            {
                Console.Write(">");
                res = Console.ReadLine();
                Console.WriteLine();

                Trace.TraceInformation("Console User answer: '{0}'", res != null ? res : "<null>");

                if (res == null || res.Length < minLength_in)
                    ConsoleHelper.WarningMessage(Resources.ERROR_MIN_LENGHT, minLength_in);
            } while (res == null || res.Length < minLength_in);
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="timeout_in">[ms], null for infinite</param>
        /// <returns>null in case of timeout</returns>
        static public ConsoleKeyInfo? GetUserAnswerFor(string message, int? timeout_in)
        {
            ClearInputBuffer();            

            if(message != null && message != string.Empty)
                InfoMessage(message);

            while (!Console.KeyAvailable && ( timeout_in.HasValue == false || timeout_in > 0) )
            {
                if(timeout_in.HasValue)
                    timeout_in -= 250;

                Thread.Sleep(250);
            }

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo res = Console.ReadKey();
                Console.WriteLine();
                return res;
            }
            else
                return null;//timeout
        }

        static public ConsoleKeyInfo GetUserAnswerFor(string message)
        {
            return GetUserAnswerFor(message, int.MaxValue).Value;
        }

        /// <summary>
        /// Provides similar functionality as Forms.MessageBox() 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="buttnos_in">AbortRetryIgnore and Ok is not supported</param>
        /// <param name="default_in">default item</param>
        /// <param name="enterRequired_in"></param>
        /// <param name="positionY_in">optional</param>
        /// <returns></returns>
        static public DialogResult ConsoleMessage(string message_in, MessageBoxButtons buttnos_in, DialogResult? default_in, bool enterRequired_in, int? positionY_in)
        {
            bool requiredOk = false, requiredYes = false, requiredNo = false;
            bool requiredCancel = false, requiredRetry = false;
            switch (buttnos_in)
            {
                case MessageBoxButtons.OKCancel:
                    requiredOk = requiredCancel = true;
                    break;
                case MessageBoxButtons.RetryCancel:
                    requiredRetry = requiredCancel = true;
                    break;
                case MessageBoxButtons.YesNo:
                    requiredYes = requiredNo = true;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    requiredCancel = requiredYes = requiredNo = true;
                    break;
                default:
                    throw new ArgumentException(string.Format("buttnos_in: '{0}' is not supported", buttnos_in));
            }

            ConsoleMenuItem mi;
            List<ConsoleMenuItem> items = new List<ConsoleMenuItem>();

            if (requiredOk)
            {
                mi = new ConsoleMenuItem(Resources.DIALOG_RESULT_OK);
                mi.Tag = DialogResult.OK;
                items.Add(mi);
            }
            if (requiredYes)
            {
                mi = new ConsoleMenuItem(Resources.DIALOG_RESULT_YES);
                mi.Tag = DialogResult.Yes;
                items.Add(mi);
            }
            if (requiredNo)
            {
                mi = new ConsoleMenuItem(Resources.DIALOG_RESULT_NO);
                mi.Tag = DialogResult.No;
                items.Add(mi);
            }
            if (requiredRetry)
            {
                mi = new ConsoleMenuItem(Resources.DIALOG_RESULT_RETRY);
                mi.Tag = DialogResult.Retry;
                items.Add(mi);
            }
            if (requiredCancel)
            {
                mi = new ConsoleMenuItem(Resources.DIALOG_RESULT_CANCEL);
                mi.Tag = DialogResult.Cancel;
                items.Add(mi);
            }

            ConsoleMenu cm = new ConsoleMenu(message_in, string.Empty, requiredCancel, enterRequired_in, 0, items);
            
            int? defaultIndex = null;
            if (default_in.HasValue)
            {
                defaultIndex = items.First(item => (DialogResult)item.Tag == default_in.Value).Index;
            }            
            ConsoleMenuItem cmi = cm.DoMenu(defaultIndex, positionY_in);

            return cmi == null ? DialogResult.Cancel : (DialogResult)cmi.Tag;
        }//ConsoleMessage()

        public static void ErrorMessage(string format, params object[] args)
        {
            ErrorMessage(string.Format(format, args));
        }

        /// <summary>
        /// writes error message to console and warning to trace file
        /// </summary>
        /// <param name="msg"></param>
        public static void ErrorMessage(string msg)
        {
            Trace.TraceWarning("Console Error Out: '{0}'", msg.Trim());
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format(Resources.ERROR_HAPPEND_PARAM, msg));
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void ResultMessage(string msg, bool Ok)
        {
            Trace.TraceInformation("Console Result Out: '{0}' {1}", msg.Trim(), Ok.ToString());
            if (Ok)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format(msg));
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void ResultMessageTo(int x, int y, string msg, bool Ok)
        {
            Trace.TraceInformation("Console Result Out: '{0}' {1}", msg.Trim(), Ok.ToString());
            if (Ok)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;
            ConsoleHelper.WriteLineTo(x, y, string.Format(msg));
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void WarningMessage(string format, params object[] args)
        {
            WarningMessage(string.Format(format, args));
        }
        /// <summary>
        /// writes error message to console and information to trace file
        /// </summary>
        /// <param name="msg"></param>
        public static void WarningMessage(string msg)
        {
            Trace.TraceWarning("Console Warning Out: '{0}'", msg.Trim());

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void InfoMessage(string format, params object[] args)
        {
            InfoMessage(string.Format(format, args));
        }

        /// <summary>
        /// writes error message to console and information to trace file
        /// </summary>
        /// <param name="msg"></param>
        public static void InfoMessage(string msg)
        {
            Trace.TraceInformation("Console Info Out: '{0}'", msg.Trim());
            Console.Write(msg);
            Console.BackgroundColor = ConsoleColor.Black; Console.WriteLine();
        }
    }
}
