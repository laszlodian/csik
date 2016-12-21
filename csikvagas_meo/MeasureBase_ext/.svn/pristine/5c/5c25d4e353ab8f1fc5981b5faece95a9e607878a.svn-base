using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace e77.MeasureBase
{
    public partial class AkyProgressBar : UserControl
    {
        public AkyProgressBar()
        {
            InitializeComponent();
        }


        private float status;
        public float Status
        {
            get { return status; }
            set { status = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);



            int xm = (int)(Math.Min(1, Math.Max(0, status)) * ClientSize.Width);


            //e.Graphics.DrawRectangle(new Pen(Color.Black), new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1));

            e.Graphics.FillRectangle(new SolidBrush(ForeColor), new Rectangle(0, 0, xm, ClientSize.Height));
            e.Graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(xm, 0, ClientSize.Width - xm, ClientSize.Height));
        }

    }
}
