using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using e77.MeasureBase;
using e77.MeasureBase.MeasureEnvironment;
using e77.MeasureBase.Properties;
using e77.MeasureBase.Sql;

namespace e77.MeasureBase.GUI
{
    public partial class TestDbControl : UserControl
    {
        public TestDbControl()   :base()
        {
            InitializeComponent();
            //HACK for WPF
            if (DesignMode
               || LicenseManager.UsageMode == LicenseUsageMode.Designtime) //for WPF host
                return;
            _tooltip = new ToolTip();
            _tooltip.SetToolTip(this._label, "A mérés a teszt adatbázist használja, csak tesztelési célra.");
        }

        ToolTip _tooltip;

        protected override void OnParentChanged(EventArgs e)
        {
            if (DesignMode 
                || LicenseManager.UsageMode == LicenseUsageMode.Designtime) //for WPF host
                return;
            base.OnParentChanged(e);
            if (!MeasureConfig.TheConfig.SqlIsReleaseDb)
            //ldap_info turned off || e77User.LdapDenied)
            {
                StringBuilder strRes = new StringBuilder();

                if (!MeasureConfig.TheConfig.SqlIsReleaseDb)
                    strRes.Append(Resources.TEST_DATABASE);

                /*todo: swow LDAP denyed (! sometimes it is normal, e.g. Analyticon)
                if (false /*ldap_info turned off e77User.LdapDenied* /)
                {
                    if (strRes.Length > 0)
                        strRes.Append(", ");

                    strRes.Append("LDAP Tiltva.");
                }*/
                _label.Text = strRes.ToString();

                this.BringToFront();
            }
            else
                this.Hide();
        }
    }
}