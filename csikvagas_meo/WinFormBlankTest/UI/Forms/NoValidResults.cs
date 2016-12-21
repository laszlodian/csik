﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormBlankTest.UI.Forms
{
    public partial class NoValidResults : Form
    {
        public NoValidResults(string lot,string roll)
        {
            InitializeComponent();
            this.Text = string.Format("Nincs érvényes eredmény eltárolva a {0} számú LOT-ból a '{1}' számú Roll esetén",lot,roll);
            label1.Text = string.Format("Nincs érvényes eredmény eltárolva a {0} számú LOT-ból a '{1}' számú Roll esetén", lot, roll);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
