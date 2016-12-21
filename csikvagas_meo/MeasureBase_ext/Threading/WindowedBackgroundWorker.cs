using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using e77.MeasureBase.GUI;
using e77.MeasureBase.GUI.DialogWindows;

namespace e77.MeasureBase.Threading
{
    public class WindowedBackgroundWorker : BackgroundWorker
    {
        #region Fields and Properties

        ProgressWindow _pw;

        private Window _owner;

        public Window Owner
        {
            get { return _owner; }
            set
            {
                if (_owner != value)
                {
                    _owner = value;
                }
            }
        }

        #endregion Fields and Properties

        #region Constructors

        public WindowedBackgroundWorker() : base() { }

        public WindowedBackgroundWorker(Window owner_in)
            : base()
        {
            this._owner = owner_in;
        }

        #endregion Constructors

        #region Event Handlers

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _pw = new ProgressWindow(_owner);
                if (this.WorkerSupportsCancellation)
                    _pw.Closed += new EventHandler(_pw_Closed);
                else
                    _pw.bt_Cancel.IsEnabled = false;
                _pw.Show();
            }));
            base.OnDoWork(e);
        }

        private void _pw_Closed(object sender, EventArgs e)
        {
            this.CancelAsync();
        }

        protected override void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (e != null)
            {
                _pw.ChangeProgressBarRemotely(e.ProgressPercentage);
                if (e.UserState != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _pw.Text = e.UserState.ToString();
                    }));
            }
            base.OnProgressChanged(e);
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            base.OnRunWorkerCompleted(e);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _pw.Close();
            }));
        }

        #endregion Event Handlers
    }
}