using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace e77.MeasureBase.Beep
{
    public partial class BeepConfigForm : Form
    { 
        public BeepConfigForm()
        {
            InitializeComponent();
            RadioButtons = new RadioButton[] { _rbNone, _rbSoundCard, _rbBeep };
        }

        RadioButton[] RadioButtons;

        public  BeepHelper.EBeepType Output
        {
            get
            {
                int i;
                for (i = 0; !RadioButtons[i].Checked; i++)
                    ;
                return (BeepHelper.EBeepType)i;                       
            }

            set
            {
                RadioButtons[(int)value].Checked = true;
            }
        }
    }
}
