using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace e77.MeasureBase.GUI
{
    public class ClosableControls
    {
        public void Clear()
        {
            if (Controls != null)
            {
                foreach (Control c in Controls)
                    c.VisibleChanged -= new EventHandler(Control_VisibleChanged);

                Controls = null;
            }
        }

        public void Initialize(Control parent_in, Control[] controls_in, int grap_in, bool vertical_in)
        {
            Initialize(parent_in, controls_in, grap_in, vertical_in, grap_in);
        }

        public void Initialize(Control parent_in, Control[] controls_in, int grap_in, bool vertical_in, int lastGrap_in)
        {
            if (vertical_in != true)
                throw new NotImplementedException();

            if (controls_in.Length < 2)
                throw new ArgumentException("It is a collection handling class");

            Parent = parent_in;
            Controls = controls_in;
            Grap = grap_in;

            foreach (Control c in Controls)
                if (!parent_in.Controls.Contains(c))
                    parent_in.Controls.Add(c);

            Control_VisibleChanged(Controls[0], EventArgs.Empty);//initialize positions

            foreach (Control c in Controls)
                c.VisibleChanged += new EventHandler(Control_VisibleChanged);

            LastGrap = lastGrap_in;
        }

        int LastGrap { get; set; }

        void Control_VisibleChanged(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            int index = 0;

            //find modified control:
            for (; index < Controls.Count(); index++)
                if (Controls[index] == control)
                    break;

            //initialize position (of next control)
            int pos = Controls[index].Location.Y;
            if (Controls[index].Visible)
                pos += (Controls[index].Height + Grap);

            //update all control after that:
            for (index++; index < Controls.Count(); index++)
            {
                Controls[index].Location = new System.Drawing.Point( Controls[index].Location.X, pos);
                if (Controls[index].Visible)
                    pos += (Controls[index].Height + Grap);
            }

            Controls[Controls.Count()-1].Height =
                Parent.ClientSize.Height - Controls[Controls.Count() - 1].Top - LastGrap;
        }

        Control Parent { get; set; }
        Control[] Controls { get; set; }
        int Grap { get; set; }
    }
}
