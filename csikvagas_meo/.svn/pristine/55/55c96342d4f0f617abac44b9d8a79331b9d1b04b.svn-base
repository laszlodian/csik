using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using WinFormBlankTest.UI.Chart;
using Npgsql;
using e77.MeasureBase.MeasureEnvironment.IpThermo;

namespace WinFormBlankTest.UI.Forms
{

    public partial class TubeCountInRollForm : Form
    {
        public bool _IsFormatOk;
        private System.Windows.Forms.DialogResult res;
        private  bool values_needed;
        public double temperature;
        public double humidity;
        public static string LOT_ID;

        public TubeCountInRollForm()
        {
            InitializeComponent();
           
            this.Text = "MEO mérési körülmények adatai";

            tbLOT.Text = Properties.Settings.Default.PREVIOUSLY_LOT_NUMBER;
            FillTempAndHumidityAsIPThermo();
            
        }

        private void FillTempAndHumidityAsIPThermo()
        {
            try
            {
                if (BlankTestEnvironment.room_id != 0)
                {
                    temperature = IpThermo.GetTemperature(BlankTestEnvironment.room_id);

                }
                else
                {
                    temperature = 0;
                    Trace.TraceError(string.Format("RoomID has not been set, roomID: {0}", BlankTestEnvironment.room_id));

                }
                if (BlankTestEnvironment.room_id != 0)
                {
                    humidity = IpThermo.GetHumidity(BlankTestEnvironment.room_id);
                }
                else
                {
                    humidity = 0;
                    Trace.TraceError(string.Format("RoomID has not been set, roomID: {0}", BlankTestEnvironment.room_id));
                }
            }
            catch (Exception)
            {

                Trace.TraceError("IPThermo is not working");
            }
            finally
            {
                tbTemperature.Text = Convert.ToString(Math.Round(temperature,2));
                tbHumidity.Text = Convert.ToString(Math.Round(humidity,2));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckValues())
            {


                LOT_ID = tbLOT.Text;
                Regex lotRegex=new Regex("^[0-9]{6}[A]{1}$");
                if (lotRegex.IsMatch(LOT_ID))
                {
                    DialogResult res= MessageBox.Show(string.Format("Az általános LOT azonosító 6db számjegy majd egy 'A' betű, az Ön által megadott {0} nem ilyen formátumú. \r\n Biztosan folytatja a mérést?!",LOT_ID),"Megfelelő Lot azonosító?!",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

                    if (res == System.Windows.Forms.DialogResult.No)
                    {
                        errorText = "Hibás LOT azonosító";
                        return;
                    }
                }

                Program.LOT_ID= TubeCountInRollForm.LOT_ID;
                Properties.Settings.Default.PREVIOUSLY_LOT_NUMBER = Program.LOT_ID;
                Properties.Settings.Default.Save();

                Trace.TraceInformation("cbText: {0}",cbTubeCount.Text);
                if (cbTubeCount.Text.Equals(string.Empty))
                {
                    Program.TubeCount = Convert.ToInt32(lbTubeCount.Text);
                    
                }else
                    Program.TubeCount = Convert.ToInt32(cbTubeCount.Text);
               


                this.Hide();

              
            }
        }


        public string errorText = string.Empty;
        /// <summary>
        /// Check the values was given from the operator
        /// </summary>
        /// <returns></returns>
        private bool CheckValues()
        {
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    if (item.Text == string.Empty
                        && (cbTubeCount.Text==string.Empty))
                    {
                        values_needed = true;
                    }
                }
            }
           
            double humidity = 0;
            double temperature = 0;

            tbHumidity.Text = tbHumidity.Text.Replace(',','.');
            tbTemperature.Text = tbTemperature.Text.Replace(',', '.');

            double.TryParse(tbHumidity.Text, out humidity);
            double.TryParse(tbTemperature.Text, out temperature);

            if ((Convert.ToDouble(humidity) < 20)
                      || (Convert.ToDouble(humidity) > 45))
            {
                errorText = "A páratartalomnak 20 és 45 RH% közé kell esni!";
            }
            if ((Convert.ToDouble(temperature) < 20)
                || (Convert.ToDouble(temperature) > 26))
            {
                errorText = "A Hőmérsékletnek 20 és 26 celsius fok közé kell esni!";
            }
            if ((humidity == 0)
                || (temperature == 0))
            {
                if (humidity == 0)
                {
                    errorText = string.Format("Nem megfelelő formátum:{0}", tbHumidity.Text);
                }
                else if (temperature == 0)
                {
                    errorText = string.Format("Nem megfelelő formátum:{0}", tbTemperature.Text);
                }


            }
            if (!values_needed)
            {
              
                if ((errorText == string.Empty))
                {
                            Program.Humidity = Convert.ToDouble(tbHumidity.Text);
                            Program.Temperature = Convert.ToDouble(tbTemperature.Text);
                            warningLabel.Visible = false;
                            button1.Enabled = true;

                            return true;
                }                
                else
                {
                    MessageBox.Show(string.Format("{0}", errorText));
                    errorText = string.Empty;
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Minden értéket meg kell adni!");
                values_needed = false;
                return false;
                
            }
        }
        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _IsFormatOk = (Regex.IsMatch(lbTubeCount.Text, "^[0-9]*$"));
         
            if (_IsFormatOk
                && lbTubeCount.Text != string.Empty)
            {
                if (Convert.ToInt32(lbTubeCount.Text) < 13)
                {
                    warningLabel.Text = "A legkevesebb tubusszám minimum 13 tubus!";
                    warningLabel.Visible = true;

                }
                else
                {
                    warningLabel.Visible = false;
                    button1.Enabled = true;
                }
            }else 

                {
                    warningLabel.Text = "Csak számokat írhat be!";
                    warningLabel.Visible = true;
                    button1.Enabled = false;
                   // lbTubeCount.Text = string.Empty;

                }
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null,EventArgs.Empty);
                foreach (Control item in UserWindow.userTables.Controls)
                {
                    if (item is TextBox)
                    {
                        if (((TextBox)item).Name=="tbBarcode")
                        {
                            ((TextBox)item).Text=Program.LOT_ID;
                            ((TextBox)item).Enabled = false;
                        }
                    }
                }
            }
        }

        private void TubeCountInRollForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Trace.TraceInformation("ESC was pressed then application is shut down");

                res=MessageBox.Show("Biztosan kilép a programból?","Kilépés",MessageBoxButtons.YesNo);

                if (res == System.Windows.Forms.DialogResult.Yes)
                {

                    Environment.Exit(Environment.ExitCode);
                }
                
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            _IsFormatOk = (Regex.IsMatch(lbTubeCount.Text, "[0-9]"));

            if (!_IsFormatOk)
            {
                warningLabel.Text = "Csak számokat írhat be!";
                warningLabel.Visible = true;
                lbTubeCount.Text = string.Empty;
                
            }else if (_IsFormatOk)
	            {
                    if (Convert.ToInt32(lbTubeCount.Text) < 13)
                    {
                        warningLabel.Text = "A legkevesebb tubusszám minimum 13 tubus!";
                        warningLabel.Visible = true;
                    }
                   

	            }
        }

        private void tbLOT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                tbLOT.Enabled = false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            tbLOT.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tbLOT.Enabled = true;
            tbLOT.Text = string.Empty;
            tbLOT.Focus();
        }

        private void lbTubeCount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender,e);
            }
        }
    }
}
