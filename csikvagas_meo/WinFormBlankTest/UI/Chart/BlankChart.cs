using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WinFormBlankTest.UI.Chart
{
    public partial class BlankChart : Form
    {
       public string[] seriesArray;

       public double[] pointsArray;

        public BlankChart(string[] tubeIDs,double[] tubeValues)
        {
            InitializeComponent();

            seriesArray = tubeIDs;
            pointsArray = tubeValues;

            this.Load += new EventHandler(BlankChart_Load);
        }

        void BlankChart_Load(object sender, EventArgs e)
        {
       
           

            // Set palette.
            this.chart1.Palette = ChartColorPalette.SeaGreen;
            
            // Set title.
            this.chart1.Titles.Add("Values of tubes in this measurement");

            

            #region set limits of the grid(2 horizontal line)
            // Enable scale breaks
            chart1.ChartAreas[0].AxisY.ScaleBreakStyle.Enabled = true;

            // Set the scale break type
            chart1.ChartAreas[0].AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Straight;

            // Set the spacing gap between the lines of the scale break (as a percentage of y-axis)
            chart1.ChartAreas[0].AxisY.ScaleBreakStyle.Spacing = 0;

            // Set the line width of the scale break
            chart1.ChartAreas[0].AxisY.ScaleBreakStyle.LineWidth = 2;

            // Set the color of the scale break
            chart1.ChartAreas[0].AxisY.ScaleBreakStyle.LineColor = Color.Red;

            // Show scale break if more than 25% of the chart is empty space
            chart1.ChartAreas[0].AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 25;

            #endregion
            chart1.ChartAreas[0].AxisX.Minimum = 1;
            chart1.ChartAreas[0].AxisX.Title = "Tubes in the measurement";
            chart1.ChartAreas[0].AxisY.Title = "Nano Ampers";

            chart1.ChartAreas[0].AxisY.StripLines.Add(new StripLine
            {
                BorderDashStyle = ChartDashStyle.Dash,
                BorderColor = Color.Red,
                Interval = 13//Here is your y value
            });

            // For an X-Y plot, add X-Y coordinates for each point.
            // This assume that dat is a double[] or a List<double>.
            // Series["MyGraph"] is a series added in the form designer.
            for (int i = 1; i < pointsArray.Length; i++)
            {
                // Add X and Y values for a point.
                chart1.Series["Series1"].Points.AddXY(i, pointsArray[i]);
            }
           
        }
    }
}
