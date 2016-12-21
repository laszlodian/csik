using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace e77.MeasureBase.Reporting
{
    public partial class DateTimeIntarvalForm : Form
    {
        public DateTimeIntarvalForm(DateTimeInterval obj_in)
        {
            DateTimeIntervalObj = new DateTimeInterval(obj_in);
            InitializeComponent();

            //deny times at the future:
            _monthCalendarStart.MaxDate = DateTime.Now.Date;
            _monthCalendarEnd.MaxDate = DateTime.Now.Date + new TimeSpan(1, 0, 0, 0);

            //fill up predefineds:
            _lvPredefined.Items.Add(new ListViewItem("Nincs"));

            foreach (DateTimeInterval.EPredefinedInterval predefined in Enum.GetValues(typeof(DateTimeInterval.EPredefinedInterval)))
            {
                ListViewItem item = new ListViewItem( DateTimeInterval.PredefinedToHungarian(predefined) );
                item.Tag = predefined;
                _lvPredefined.Items.Add(item);
            }

            UpdateUI(true);
        }

        bool _WeAreInUpdateUi;//program triggered modification
        private void UpdateUI(bool updatePredefinedSelection)
        {
            _WeAreInUpdateUi = true;
            try
            {

                if (updatePredefinedSelection)
                {
                    if (DateTimeIntervalObj.PredefinedInterval.HasValue)
                    {
                        foreach (ListViewItem item in _lvPredefined.Items)
                            if (item.Tag != null &&
                                (DateTimeInterval.EPredefinedInterval)item.Tag == DateTimeIntervalObj.PredefinedInterval.Value)
                                item.Selected = true;
                    }
                    else
                        _lvPredefined.Items[0].Selected = true;
                }

                _monthCalendarStart.SelectionStart = DateTimeIntervalObj.Start;

                _monthCalendarEnd.SelectionStart = DateTimeIntervalObj.End < _monthCalendarEnd.MaxDate ?
                    DateTimeIntervalObj.End : _monthCalendarEnd.MaxDate;

                _tbStart.Text = DateTimeIntervalObj.Start.ToShortTimeString();
                _tbEnd.Text = DateTimeIntervalObj.End.ToShortTimeString();

                _lbResult.Text = DateTimeIntervalObj.ToString();
            }
            finally
            {
                _WeAreInUpdateUi = false;
            }
        }

        public DateTimeInterval DateTimeIntervalObj { get; private set; }

        private void _monthCalendarStart_DateChanged(object sender, DateRangeEventArgs e)
        {
            if (_WeAreInUpdateUi)
                return;

            if (e.Start >= DateTimeIntervalObj.End)
                _monthCalendarEnd.SelectionStart = e.Start + new TimeSpan(1, 0, 0, 0);
            DateTimeIntervalObj.Start = e.Start;

            DateTimeIntervalObj._PredefinedInterval = null;

            UpdateUI(true);
        }

        private void _monthCalendarEnd_DateChanged(object sender, DateRangeEventArgs e)
        {
            if (_WeAreInUpdateUi)
                return;

            if (e.Start <= DateTimeIntervalObj.Start)
                DateTimeIntervalObj.Start = e.Start - new TimeSpan(1, 0, 0, 0);

            DateTimeIntervalObj.End = e.Start;

            DateTimeIntervalObj._PredefinedInterval = null;

            UpdateUI(true);
        }

        private void _ButtonOk_Click(object sender, EventArgs e)
        {
            if (EndDateTime.HasValue && StartDateTime.HasValue)
            {
                DateTimeIntervalObj.Start = StartDateTime.Value;
                DateTimeIntervalObj.End = EndDateTime.Value;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        
        void Predefined_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (_lvPredefined.SelectedItems.Count == 1)
            {
                DateTimeIntervalObj.PredefinedInterval = (DateTimeInterval.EPredefinedInterval?)_lvPredefined.SelectedItems[0].Tag;
            }

            if (_WeAreInUpdateUi)
                return;

            UpdateUI(false);
        }

        private void _tbStart_TextChanged(object sender, EventArgs e)
        {
            TimeSpan? ts = GetTime(_tbStart);
            if (ts.HasValue && StartDateTime.HasValue)
            {
                DateTimeIntervalObj.Start = StartDateTime.Value;
                DateTimeIntervalObj._PredefinedInterval = null;
                _lbResult.Text = DateTimeIntervalObj.ToString();
            }
        }

        private void _tbEnd_TextChanged(object sender, EventArgs e)
        {
            TimeSpan? ts = GetTime(_tbEnd );
            if (ts.HasValue && EndDateTime.HasValue)
            {
                DateTimeIntervalObj.End = EndDateTime.Value;
                DateTimeIntervalObj._PredefinedInterval = null;
                _lbResult.Text = DateTimeIntervalObj.ToString();
            }
        }

        private TimeSpan? GetTime(TextBox textBox_in)
        {
            TimeSpan ts;

            textBox_in.Text = textBox_in.Text.Trim();
            if (!TimeSpan.TryParse(textBox_in.Text, out ts))
            {
                _errorProvider.SetError(textBox_in, "Érvénytelen idő (helyes formátum 22:31).");
                return null;
            }
            else 
                _errorProvider.Clear();

            return ts;
        }

        public DateTime? StartDateTime
        {
            get
            {
                TimeSpan? ts = GetTime(_tbStart);
                if (ts != null)
                    return _monthCalendarStart.SelectionStart + ts;
                else
                    return _monthCalendarStart.SelectionStart;
            }
        }

        public DateTime? EndDateTime
        {
            get
            {
                TimeSpan? ts = GetTime(_tbEnd);
                if (ts != null)
                    return _monthCalendarEnd.SelectionStart + ts;
                else
                    return _monthCalendarEnd.SelectionStart;
            }
        }
    }
}
