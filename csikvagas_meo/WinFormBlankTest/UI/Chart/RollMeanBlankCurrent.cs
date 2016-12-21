using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormBlankTest.UI.Chart
{
    public partial class RollMeanBlankCurrent : Form
    {

        public static List<double> chartNanoAmpers;
        public static List<string> snOfThetubes;


        public RollMeanBlankCurrent(List<double> nanoAmpers,List<string> sn)
        {

            chartNanoAmpers = nanoAmpers;
            snOfThetubes = sn;

            InitializeComponent();



        }


    }
}
