using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Diagnostics;
using e77.MeasureBase.MeasureEnvironment.IpThermo;

namespace WinFormBlankTest.UI.Forms
{
    public partial class Accuracy_vials_form : Form
    {
        private bool values_needed;
        public static List<string> selected_sn_and_roll = new List<string>();
        public double temperature;
        public double humidity;


        public Accuracy_vials_form()
        {
            InitializeComponent();

            tbLOTID.Text = Program.LOT_ID;
            FillTempAndHumidityAsIPThermo();
            tbLOTID.Text = LOT;
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
                tbTemp.Text = Convert.ToString(Math.Round(temperature, 2));
                tbHumidity.Text = Convert.ToString(Math.Round(humidity, 2));
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    if (item.Text == string.Empty)
                    {
                        values_needed = true;
                    }
                }
            }
            string errorText = string.Empty;
            double humidity = 0;
            double htc = 0;
            double master_calib = 0;
            DateTime expirty = DateTime.MinValue;

            double.TryParse(tbHumidity.Text, out humidity);
            double.TryParse(tbHCT.Text, out htc);
            double.TryParse(tbMasterCalib.Text, out master_calib);
            DateTime.TryParse(Convert.ToString(dateTimePicker1.Value), out expirty);

            if (expirty <= DateTime.Now)
            {
                errorText = string.Format("Nem megfelelő a lejárati idő: {0}", expirty);
            }
            if ((humidity == 0)
                || (htc == 0)
                || (master_calib == 0))
            {
                if (humidity == 0)
                {
                    errorText = string.Format("Nem megfelelő formátum:{0}", tbHumidity.Text);
                }
                else if (htc == 0)
                {
                    errorText = string.Format("Nem megfelelő formátum:{0}", tbHCT.Text);
                }
                else if (master_calib == 0)
                {
                    errorText = string.Format("Nem megfelelő formátum:{0}", tbMasterCalib.Text);
                }

            }
            if (!values_needed)
            {
                if ((Convert.ToDouble(tbHumidity.Text) < 20)
                          || (Convert.ToDouble(tbHumidity.Text) > 45))
                {
                    errorText = "A páratartalomnak 20 és 45 RH% közé kell esni!";
                }
                if ((Convert.ToDouble(tbHCT.Text) > 45)
                    || (Convert.ToDouble(tbHCT.Text) < 39))
                {
                    errorText = "A HCT-nek 39 és 45 százalék közé kell esni!";
                }
                if ((Convert.ToDouble(tbTemp.Text) < 20)
                      || (Convert.ToDouble(tbTemp.Text) > 26))
                {
                    errorText = "A hőmérsékletnek 20 és 26 celsius fok közé kell esni!";
                }
                if ((errorText == string.Empty))
                {
                    Trace.TraceInformation("ErrorText is Empty, all values are valid");
                    using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
                    {
                        try
                        {
                            conn.Open();
                            Trace.TraceInformation("Connection is Opened");
                            Program.Humidity = Convert.ToDouble(tbHumidity.Text);
                            Program.HTC = Convert.ToDouble(tbHCT.Text);
                            Program.LOT_ID = Convert.ToString(tbLOTID.Text);
                            Program.master_calibration = Convert.ToDouble(tbMasterCalib.Text);
                            Program.master_lot_id = tbMasterLotID.Text;
                            Program.ExpirityDate = dateTimePicker1.Value;
                            Program.Temperature = Convert.ToDouble(tbTemp.Text);

                            Trace.TraceInformation("Values initialized");
                            using (NpgsqlCommand insertVialValues = new NpgsqlCommand(
                                string.Format("insert into accuracy_values(humidity,htc,master_lot_id,master_calibration,measured_lot_id,test_date,expirity_date,temperature) values({0:0.00},{1:0.00},'{2}',{3:0.00000000},'{4}',{5},'{6}',{7:0.00})", Math.Round(Program.Humidity,2), Math.Round(Program.HTC,2), Program.master_lot_id, Math.Round(Program.master_calibration,8), Program.LOT_ID, "@test_date",Program.ExpirityDate,Program.Temperature), conn))
                            {
                                object res = null;
                                insertVialValues.Parameters.AddWithValue("@test_date", DateTime.Now);
                                res = insertVialValues.ExecuteNonQuery();
                                Trace.TraceInformation("Accuracy Values Inserted successfull");
                                if (res == null)
                                {
                                    Trace.TraceError("Unsuccessful insert to accuracy_values, insert statement: {0}", insertVialValues.CommandText);
                                }
                                else
                                    Trace.TraceInformation("Given values inserted to accuracy_values table");
                            }
                            using (NpgsqlCommand getvaluesid=new NpgsqlCommand(string.Format("select max(pk_id) from accuracy_values"),conn))
                            {
                                object res = null;
                                res = getvaluesid.ExecuteScalar();

                                if (res == null)
                                {
                                    Trace.TraceError("Unsuccessfull query to get latest accuracy_values inserted pk_id, query:{0}", getvaluesid.CommandText);
                                }
                                else
                                    Program.ValuesID = Convert.ToInt32(res);
                            }
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError("Exception at storing accuracy_values ex:{0}", ex.StackTrace);
                            
                        }
                        finally
                        {
                            conn.Close();

                        }
                    }
                }
                else
                {
                    MessageBox.Show(string.Format("{0}", errorText));
                    errorText = string.Empty;
                }
            }
            else
            {
                MessageBox.Show("Minden értéket meg kell adni!");
                values_needed = false;
                
            }
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (CheckBarcodeIsValid(((TextBox)sender).Text))
                {
                    Program.snAndRollID.Add(((TextBox)sender).Text);
                    errorLabel.Visible = false;
                    ((TextBox)sender).Enabled = false;
                }
                else
                {
                    ((TextBox)sender).Enabled = true;
                    errorLabel.Visible = true;
                }
            }
        }
        int panelNumber = 1;
        private bool CheckBarcodeIsValid(string p)
        {
            
            if (p.Contains(' '))
            {
               
                panelNumber++;
                
                return true;
            }
            else
                return false;

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Control item in this.groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).Enabled = true;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        public string LOT { get; set; }
    }
}
