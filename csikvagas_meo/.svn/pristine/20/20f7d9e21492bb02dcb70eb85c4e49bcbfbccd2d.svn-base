using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Diagnostics;
namespace WinFormBlankTest.UI.Chart
{
    partial class RollMeanBlankCurrent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
           
            
            // 
            // chart1
            // 
            this.chart1.BorderSkin.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.Name = "Vials Values of Roll, and Roll Average(red) after Blank Current Test";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = string.Format("{0} Roll ID",new Device().Roll);
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(0);
            this.chart1.Name=string.Empty;;
            this.chart1.Name = "chart1";
            series1.ChartArea = "RollMeanBlank";
            series1.Legend = "Legend1";
            series1.Name = "Series1";

            ///Add x-y pontokat a rácshoz
           for (int i = 0; i < snOfThetubes.Count; i++)
			{
			    series1.Points.AddXY(snOfThetubes[i],chartNanoAmpers[i]);
            }

            
            chart1.ChartAreas[0].AxisY.Minimum=1;

            
           //#region set limits of the grid(2 horizontal line)
           //// Enable scale breaks
           //chart1.ChartAreas[0].AxisY.ScaleBreakStyle.Enabled = true;

           //// Set the scale break type
           //chart1.ChartAreas[0].AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Straight;

           //// Set the spacing gap between the lines of the scale break (as a percentage of y-axis)
           //chart1.ChartAreas[0].AxisY.ScaleBreakStyle.Spacing = 0;

           //// Set the line width of the scale break
           //chart1.ChartAreas[0].AxisY.ScaleBreakStyle.LineWidth = 2;

           //// Set the color of the scale break
           //chart1.ChartAreas[0].AxisY.ScaleBreakStyle.LineColor = Color.Red;

           //// Show scale break if more than 25% of the chart is empty space
           //chart1.ChartAreas[0].AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 25;

           //#endregion

           



            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(948, 457);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // RollMeanBlankCurrent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 457);
            this.Controls.Add(this.chart1);
            this.Name = "RollMeanBlankCurrent";
            this.Text = "Vials Values of Roll, and Roll Average(red) after Blank Current Test";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}