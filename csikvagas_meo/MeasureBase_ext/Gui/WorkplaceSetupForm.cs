using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using e77.MeasureBase.Properties;
using e77.MeasureBase.MeasureEnvironment.IpThermo;
using System.Diagnostics;

namespace e77.MeasureBase.GUI
{
    public partial class WorkplaceSetupForm : Form
    {
        const string REG_KEY = @"HKEY_LOCAL_MACHINE\SOFTWARE\e77";
        const string REG_VALUE_ID = "Workplace";
        public WorkplaceSetupForm()
        {
            InitializeComponent();
            UpdateLabel();
            MustSelectValid = true;
        }

        /// <summary>
        /// 0 = invalid
        /// </summary>
        public static int RoomId
        {
            get
            {
                object res = Registry.GetValue(REG_KEY, REG_VALUE_ID, null);

                if (res == null)
                    return 0;
                else
                    return (int)res;
            }
        }

        public static string InfoStr
        {
            get
            {
                object res = Registry.GetValue(REG_KEY, REG_VALUE_ID, null);

                if (res == null)
                    return Resources.WORKPLACE_NOT_SET;
                else
                    return string.Format(Resources.WORKPLACE_INFO, IpThermo.RoomNames[(int)res]);
            }
        }

        private void UpdateLabel()
        {
            _label.Text = WorkplaceSetupForm.InfoStr;
        }

        private void _btnSet_Click(object sender, EventArgs e)
        {
            using(WorkplaceSelectionForm form = new WorkplaceSelectionForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Registry.SetValue(REG_KEY, REG_VALUE_ID, form.RoomId);
                        UpdateLabel();

                        DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Trace.TraceWarning(ex.ReportError());
                        throw new Exception(Resources.ADMINISATATOR_NEEDED);
                    }
                }
            }
        }

        public bool MustSelectValid { get; set; }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (MustSelectValid && RoomId == 0)
            {
                e.Cancel = true;
                errorProvider.SetError(_btnSet, "Válasszon ki helyiséget");
            }
            base.OnClosing(e);
        }
    }
}
