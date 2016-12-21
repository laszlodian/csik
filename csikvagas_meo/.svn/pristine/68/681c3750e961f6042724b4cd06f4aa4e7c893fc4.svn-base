using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormBlankTest.UI.Forms
{
    public partial class RemeasureForm : Form
    {
        public RemeasureForm(string lotid,string rollid,string measuretype)
        {
            InitializeComponent();

            this.label1.Text = string.Format("A '{0}' számú LOT-ból, az '{1}' számú Roll, '{2}' mérése következik",lotid,rollid,measuretype);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

            Application.Run();
        }

        private void RemeasureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
