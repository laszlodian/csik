﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using e77.MeasureBase;
using e77.MeasureBase.MeasureEnvironment.IpThermo;
using Npgsql;
using WinFormBlankTest.UI.Forms;

using WinFormBlankTest.UI.Forms.Other_Forms;
using WinFormBlankTest.UI.Panels;
using System.IO.Ports;
using System.Net.NetworkInformation;
using WinFormBlankTest.Network;
using System.ServiceProcess;


namespace WinFormBlankTest
{
    public enum ECommands
    {
        stripWait = 128,
        stripIn = 136,
        dropWait = 144,
        dropDetect = 152,
        usedStrip = 160,
        meterOff = 168,
        batFlag = 176,
        code = 192,
        error_h = 32,
        error_e = 96,

    }


    public class MessageCompletedEventArgs : EventArgs
    {

        #region Constants

        public const int ACCURACY_STRIPCOUNT = 48;
        public const int MASTER_STRIPCOUNT = 24;
        public const int CENTRAL_STRIPCOUNT = 24;

        #endregion

        #region Variables

        public double master_sub_avg;
        public List<double> masters_calibrated_glu = new List<double>();
        public List<double> bias_glu = new List<double>();
        public List<int> bias_glu_id = new List<int>();
        public double central_avg = 0;
        public double central_stddev = 0;
        public double central_cv = 0;
        public double central_bias = 0;
        int altogether_lot_out_count;
        public int command = 0;
        public int code = 0;
        public string wrong_step = string.Empty;
        public int error_e;
        public int error_h;
        public int glu;

        public object roll_out_of_range_strip_count = 0;

        public double accuracy_lot_accuracy = 0;
        public double accuracy_lot_avg = 0;
        public double accuracy_lot_sd = 0;
        public double accuracy_lot_cv = 0;
        public bool not_h62_is_valid;
        public bool out_of_range_valid;
        public string stipCount_part1, stipCount_part2;
        public object masters_avg = null;

        public double master_avg = 0;
        public double master_stddev = 0;
        public double master_cv = 0;
        public double master_bias = 0;
        public int calculatedCheckSum;

        public double percent_outside_bias;
        public object bias_value;
        public int accuracyStripCount;
        public DialogResult result;

        public List<int> mastersIDs = new List<int>();
        public List<double> masters_glus = new List<double>();
        public List<DateTime> masters_date = new List<DateTime>();
        public static int month;
        public Label battery_label = new Label();

        public int outsideCount;
        public string controllerUser = Environment.GetEnvironmentVariable("USERNAME");
        public string machineName = Environment.GetEnvironmentVariable("COMPUTERNAME");

        public long lotid;

        public DetailedPanel ThePanel;
        public UserPanel Users_Form;

        public DateTime start_date = DateTime.MinValue;
        public DateTime end_date = DateTime.MinValue;
        public bool startTimeStored;

        public bool isMeo;

        public double temperature;

        #endregion

        #region Property



        public static string Controller
        {
            get
            {
                return Environment.GetEnvironmentVariable("USERNAME");
            }

        }
        public static string ComputerName
        {
            get
            {
                return Environment.GetEnvironmentVariable("COMPUTERNAME");
            }

        }
        public static byte Data_low;
        public static byte Data_high;
        public int Data;

        public int MeasureID;
        public static byte MeasureID_low { get; set; }
        public static byte MeasureID_high { get; set; }
        public int Calculated_MeasureID;

        public static int SerialNumber;
        public static byte SerialNumber_LL { get; set; }
        public static byte SerialNumber_LH { get; set; }
        public static byte SerialNumber_HL { get; set; }
        public static byte SerialNumber_HH { get; set; }
        public long Calculated_SerialNumber;

        public static byte CRC { get; set; }

        public CounterPanel counterPenel;
        #endregion


        #region Delegates


        #region Unused - TODO: Remove this carefully
        //delegate void SetLabelVisibleDelegate();
        //public void SetLabelVisible()
        //{
        //    if (Users_Form.InvokeRequired)
        //    {
        //        Users_Form.Invoke(new SetLabelVisibleDelegate(SetLabelVisible));
        //    }
        //    else
        //        foreach (Control lb in Users_Form.Controls)
        //        {
        //            if (lb is Label)
        //            {

        //                ((Label)lb).Visible = true;


        //            }
        //        }
        //} 




        //delegate void AddMEasuredStripCountToCounterDelegate();
        //public void AddMEasuredStripCountToCounter()
        //{
        //    if (Program.counter.InvokeRequired)
        //    {
        //        Program.counter.Invoke(new AddMEasuredStripCountToCounterDelegate(AddMEasuredStripCountToCounter));
        //    }
        //    else
        //        foreach (Control lb in Program.counter.Controls)
        //        {
        //            if ((lb is TextBox)
        //                && ((TextBox)lb).Name.Equals("tbLemert"))
        //            {
        //         //       ((TextBox)lb).Text = Convert.ToString(Convert.ToInt32(((TextBox)lb).Text) + 1);
        //            }

        //            if ((lb is TextBox)
        //                && ((TextBox)lb).Name.Equals("tbRemaining"))
        //            {
        //            //    ((TextBox)lb).Text = Convert.ToString(Convert.ToInt32(((TextBox)lb).Text) - 1);
        //            }
        //        }
        //}




        //delegate void SetFocusDelegate();
        //public void SetFocus()
        //{
        //    if (Users_Form.InvokeRequired)
        //    {
        //        Users_Form.Invoke(new SetFocusDelegate(SetFocus));
        //    }
        //    else
        //    {
        //        foreach (Control tb in Users_Form.Controls)
        //        {
        //            if (tb is TextBox)
        //            {
        //                ((TextBox)tb).Text = string.Empty;
        //                ((TextBox)tb).Enabled = true;
        //                ((TextBox)tb).Focus();
        //            }
        //        }
        //    }
        //} 
        #endregion



        delegate void AppendUserTextDelegate(string value, Panel actForm, Color color);
        public void AppendTextToUser(string txt, Panel myForm, Color color)
        {

            Trace.TraceWarning("AppendTextToUser method in MessageCompletedEventArgs class");
            RichTextBox current = new RichTextBox();
            foreach (Control c in myForm.Controls)
                if (c is RichTextBox)
                {
                    current = (RichTextBox)c;
                    if (current.InvokeRequired)
                    {
                        current.Invoke(new AppendUserTextDelegate(AppendTextToUser), txt, myForm, color);

                    }
                    else
                    {

                        AppendingWhenInvoked(txt, color, c);
                    }
                }

        }

        delegate void ChangeTextDelegate(string value, Panel myForm);
        public void ChangeText(string text, Panel myForm)
        {
            Trace.TraceWarning("ChangeText method in MessageCompletedEventArgs class");
            RichTextBox current = new RichTextBox();
            foreach (Control c in myForm.Controls)
                if (c is RichTextBox)
                    current = ((RichTextBox)c);

            if ((myForm as UserPanel).richTextBox1.InvokeRequired)
            {
                (myForm as UserPanel).richTextBox1.Invoke(new ChangeTextDelegate(ChangeText), text, myForm);

            }
            else
            {
                current.Text = text;
            }
        }


        delegate void ShowIconDelegate(Panel actForm, Label lb);
        public void ShowIcon(Panel myForm, Label lb)
        {
            Trace.TraceWarning("ChangeText method in MessageCompletedEventArgs class");
            if (myForm.InvokeRequired)
            {
                myForm.Invoke(new ShowIconDelegate(ShowIcon), myForm, lb);
            }
            else
            {
                myForm.Controls.Add(lb);
                lb.BringToFront();
            }
        }

        delegate void HideIconDelegate(Panel actForm, Label lb);
        public void HideIcon(Panel myForm, Label lb)
        {
            Trace.TraceWarning("HideIcon method in MessageCompletedEventArgs class");
            if (myForm.InvokeRequired)
            {
                myForm.Invoke(new HideIconDelegate(HideIcon), myForm, lb);
            }
            else
            {
                foreach (Control c in myForm.Controls)
                {
                    if (c is Label)
                    {
                        ((Label)c).Dispose();
                        ((Label)c).Visible = false;
                    }
                }

            }
        }

        delegate void AppendTextDelegate(string value, Panel actForm, Color color);
        public void AppendTextInBox(string txt, Panel myForm, Color color)
        {
            Trace.TraceWarning("AppendTextInBox method in MessageCompletedEventArgs class");
            foreach (Control c in myForm.Controls)
                if (c is RichTextBox)
                {
                    if (c.InvokeRequired)
                    {
                        c.Invoke(new AppendTextDelegate(AppendTextInBox), txt, myForm, color);

                    }
                    else
                    {

                        AppendingWhenInvoked(txt, color, c);
                    }
                }
        }

        delegate void DisablingDelegate();
        public void disableTextBox()
        {
            Trace.TraceWarning("disableTextBox() in MessageCompletedEventArgs class");
            if (ThePanel.richTextBox1.InvokeRequired)
            {
                ThePanel.richTextBox1.Invoke(new DisablingDelegate(disableTextBox));
            }
            else
            {
                foreach (Control rt in ThePanel.Controls)
                {
                    if (rt is RichTextBox)
                    {
                        ((RichTextBox)rt).Enabled = false;
                        ((RichTextBox)rt).ReadOnly = true;
                    }

                }
            }
        }

        delegate void SetButtonEnabledDelegate();
        public void SetButtonEnabled()
        {
            Trace.TraceWarning("SetButtonEnabled() in MessageCompletedEventArgs class");
            if (UserWindow.userTables.Controls["btAccuracyFinished"].InvokeRequired)
            {
                UserWindow.userTables.Invoke(new SetButtonEnabledDelegate(SetButtonEnabled));
            }
            else
                foreach (Control c in UserWindow.userTables.Controls)
                {
                    if (c is Button)
                    {
                        if (((Button)c).Name == "btAccuracyFinished")
                        {
                            ((Button)c).Enabled = true;
                        }
                    }
                }
        }
        #endregion


        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="receivedData"></param>
        /// <param name="actForm"></param>
        /// <param name="_userPanel"></param>
        public MessageCompletedEventArgs(byte[] receivedData, DetailedPanel actForm, UserPanel _userPanel)
            : base()
        {


            #region Get portname
            String actPort;
            actPort = actForm.Name;

            ThePanel = actForm;
            Users_Form = _userPanel;
            #endregion
            if (Program.LoggedToMeasurement)
            {
                StoreSerialInput(receivedData, actPort, Program.MeasurementActualPkId);
            }


            #region Sorting received data
            Data_low = receivedData[2];
            Data_high = receivedData[3];
            MeasureID_low = receivedData[4];
            MeasureID_high = receivedData[5];
            SerialNumber_LL = receivedData[6];
            SerialNumber_LH = receivedData[7];
            SerialNumber_HL = receivedData[8];
            SerialNumber_HH = receivedData[9];
            CRC = receivedData[10];
            #endregion

            #region Get Data,MeasureId,Serialnumber
            Data = (Data_high << 8) + Data_low;
            MeasureID = (MeasureID_high << 8) + MeasureID_low;
            SerialNumber = (SerialNumber_HH << 24) + (SerialNumber_HL << 16) + (SerialNumber_LH << 8) + (SerialNumber_LL);

            Calculated_MeasureID = (MeasureID_high << 8) + MeasureID_low;
            Calculated_SerialNumber = (SerialNumber_HH << 24) + (SerialNumber_HL << 16) + (SerialNumber_LH << 8) + (SerialNumber_LL);
            #endregion

            #region Masks
            int mask = 0xc000;
            int error_mask = 0x2000;
            int command_mask = 0x3800;
            #endregion

            if ((Data & command_mask) == 0x2800)
            {
                crcNeeded = false;
            }
            else
                crcNeeded = true;

            if (crcNeeded)
            {
                if (CRC != 0)
                {
                    #region check crc

                    calculatedCheckSum = GetChecksum(receivedData);         //calculate received checksum
                    if (CRC != calculatedCheckSum)
                    {
                        //      for (int i = 0; i < receivedData.Length; i++)
                        //     {
                        //       WriteNote(string.Format("HEX:{1}\tDEC:{0}", receivedData[i], receivedData[i].ToString("X")), actForm.Name);
                        //     }
                        //    WriteNote(string.Format("{2}Crc hiba{2}Érkezett crc:\tHex:{5}\tDec:{0}{2}nSzámolt crc:\tHex:{4}\tDec:{1}{2}Kezdje újra a mérést a {3} számú LOT-ból!{2}", CRC,
                        //         calculatedCheckSum, Environment.NewLine, Users_Form.dev.LOT_ID, calculatedCheckSum.ToString("X"), CRC.ToString("X")), ThePanel.Name);
                        //     AppendTextToUser(string.Format("{2}Crc hiba{2}Érkezett crc:\tHex:{5}\tDec:{0}{2}nSzámolt crc:\tHex:{4}\tDec:{1}{2}Kezdje újra a mérést a {3} számú LOT-ból!{2}", CRC,
                        //        calculatedCheckSum, Environment.NewLine, Users_Form.dev.LOT_ID, calculatedCheckSum.ToString("X"), CRC.ToString("X")), Users_Form, Color.Red);

                        ClearAllData();
                        DiscardBuffers(Users_Form);
                    }
                    else
                    {

                        WriteNote("\nCRC OK\n", ThePanel.Name);
                    }
                    #endregion
                }
                else
                {
                    #region CRC error

                    //   WriteNote("\nCRC is 0\nReceived data:\n", actForm.Name);
                    for (int i = 0; i < receivedData.Length; i++)
                    {
                        //       WriteNote(string.Format("\tHex:{0}\tDec:{1}", receivedData[i].ToString("X"), receivedData[i]), actForm.Name);
                    }
                    DiscardBuffers(Users_Form);


                    #endregion
                }
            }


            ThePanel.TheDevice.code_OK = true;

            #region checkStartTime

            if (!ThePanel.TheDevice.startTimeStored)
            {
                ThePanel.TheDevice.startTimeStored = true;
                ThePanel.TheDevice.Start_Date = DateTime.Now;
            }

            #endregion
            #region Switching Data & mask
            switch (Data & mask)
            {
                case 0x0000:
                    /*Glu*/
                    #region Glucose

                    Thread.Sleep(50);
                    CheckMeasureIdAndSerial();   //check wrong command sequence and device mix

                    if (!ThePanel.TheDevice.drop_detect_OK)
                    {

                        WriteNote("\nIncorrect command sequence\nPlease Restart the Measurement\n", actForm.Name);
                        wrong_step = "glucose";
                        Program.storedGlus++;

                        DiscardBuffers(Users_Form);
                        StoreData();
                        ClearAllData();

                        AppendTextToUser(string.Format("Nem megfelelő parancs sorrend a {0}{1}Kezdje újra a mérést a {2} számú LOT-ból.{1}", ThePanel.Name,
                            Environment.NewLine, Users_Form.dev.LOT_ID), Users_Form, Color.Red);
                        Trace.TraceError(string.Format("Incorrect Command sequence on port:{0} at Glucose", ThePanel.Name));
                    }
                    else
                    {
                        glu = (Data & 0x03ff);

                        #region Count Measured Strips
                        Program.BlankMeasuredStripCount++;
                        Program.InOneRoundAccuracyStripCount++;
                        if (Program.measureType == "meo")
                        {
                            if ((glu / 18.02 >= Properties.Settings.Default.MEO_CHECK_MIN)
                                && (glu / 18.02 <= Properties.Settings.Default.MEO_CHECK_MAX))
                            {
                                AppendTextToUser(string.Format("{1}Készülék mérőképessége megfelelő:{1}Mért érték:{0:0.00}{1}SerialNumber:{2}{1}A készülék alkalmas a vizsgálatokra", glu / 18.0, Environment.NewLine, ThePanel.TheDevice.Original_SerialNumber), Users_Form, Color.Green);
                                StoreMeo(true);
                                SetMeasuredToOK(Users_Form);
                            }
                            else
                            {
                                AppendTextToUser(string.Format("{1}Készülék mérőképessége NEM megfelelő:{1}Mért érték:{0:0.00}{1}SerialNumber:{2}{1}Ellenőrizze újra, majd többszöri sikertelenség esetén cseréljen készüléket{1}", glu / 18.0, Environment.NewLine, ThePanel.TheDevice.Original_SerialNumber), Users_Form, Color.DarkRed);
                                StoreMeo(false);
                            }


                        }
                        else
                            SetMeasuredToOK(Users_Form);

                        Trace.TraceInformation("BlankMeasured strip count: {0}", Program.BlankMeasuredStripCount);
                        Program.HomogenityMeasuredStripCount++;
                        Trace.TraceInformation("HomogenityMeasured strip count: {0}", Program.HomogenityMeasuredStripCount);
                        #endregion



                        Trace.TraceInformation("BlankMeasuredStripCount is {0}", Program.BlankMeasuredStripCount);

                        wrong_step = string.Empty;

                        CounterPanel.Instance.IncreaseMeasuredStripCount();

                        WriteNote(string.Format("\n Stored:{0:0.00}\n", glu / 18.0), actForm.Name);
                        if (Program.measureType != "meo")
                        {
                            AppendTextToUser(string.Format("{1}****Eltárolva:{0:0.00}****{1}", glu / 18.0, Environment.NewLine), Users_Form, Color.DarkRed);
                            StoreData();
                        }


                        ThePanel.TheDevice.End_Date = DateTime.Now;

                        ClearAllData();
                    }
                    #endregion
                    break;

                case 0x4000:/*Error*/
                    #region error

                    if ((Data & error_mask) == 0x0000)
                    {        /*Error_h*/
                        Thread.Sleep(50);
                        CheckMeasureIdAndSerial();

                        Program.BarCode = Users_Form.dev.BarCode;

                        error_h = (Data & 0x00ff);

                        ThePanel.TheDevice.wrong_step = string.Format("error_h:{0}", error_h);
                        Trace.TraceInformation("Received data:error_h:{0}", error_h);
                        ThePanel.TheDevice.BarCode = Users_Form.dev.BarCode;
                        AppendTextToUser(string.Format("{0}Hiba történt a készüléken: Errors:{0}", Environment.NewLine), Users_Form, Color.Red);

                        AppendTextToUser(string.Format("{2}Error_h:{1}{2}", error_e, error_h, Environment.NewLine), Users_Form, Color.Red);

                        ///Investigate the error that happened
                        if ((Program.measureType == "homogenity") && (wrong_step == string.Empty))
                        {
                            new CheckError(error_h, Users_Form);
                        }

                        ThePanel.TheDevice.EarlyDribble = Users_Form.dev.EarlyDribble;
                        ThePanel.TheDevice.PostDeviceReplace = Users_Form.dev.PostDeviceReplace;
                    }
                    if ((Data & error_mask) == 0x2000)
                    {
                        /*Error_e*/
                        Thread.Sleep(50);
                        CheckMeasureIdAndSerial();
                        error_e = (Data & 0x001f);

                        ThePanel.TheDevice.LOT_ID = Users_Form.dev.LOT_ID;
                        ThePanel.TheDevice.BarcodeFirst = Users_Form.dev.BarcodeFirst;

                        wrong_step = string.Format("error_e:{0},{1}", error_e, ThePanel.TheDevice.wrong_step);
                        Trace.TraceInformation("Received data:error_e:{0}", error_e);

                        AppendTextToUser(string.Format("{2}Error_e:{0}{2}", error_e, error_h, Environment.NewLine), Users_Form, Color.Red);

                        if ((Program.measureType == "blank")
                            || (Program.measureType == "homogenity"))
                        {
                            AppendTextToUser(string.Format("{0}Kezdje újra a mérést a {1} számú LOT-ból{0}", Environment.NewLine, Users_Form.dev.LOT_ID), Users_Form, Color.Red);

                        }
                        Users_Form.dev.LOT_ID = ThePanel.TheDevice.LOT_ID;

                        DiscardBuffers(Users_Form);
                        StoreData();
                        ClearAllData();
                        if ((Program.measureType == "blank") || (ThePanel.TheDevice.EarlyDribble) || (ThePanel.TheDevice.PostDeviceReplace))
                        {
                        }

                    }
                    break;
                    #endregion
                case 0x8000:/*Command*/
                    #region command

                    if ((Data & command_mask) == 0x00)       /*strip*/
                    {
                        /*Strip wait*/

                        #region Strip Wait

                        if (ThePanel.TheDevice.Original_MeasureID == 0)
                        {
                            ThePanel.TheDevice.Original_MeasureID = (MeasureID_high << 8) + MeasureID_low;
                        }
                        if (ThePanel.TheDevice.Original_SerialNumber == 0)
                        {
                            ThePanel.TheDevice.Original_SerialNumber = (SerialNumber_HH << 24) + (SerialNumber_HL << 16) + (SerialNumber_LH << 8) + (SerialNumber_LL);

                        }
                        Trace.TraceInformation("Received data: strip_wait: {0}", ThePanel.Name);
                        #region checkMEO

                        //HACK remove before validation     CheckDeviceMeasureability();
                        #endregion

                        Thread.Sleep(50);
                        if (ThePanel.TheDevice.drop_detect_OK || ThePanel.TheDevice.strip_wait_OK || ThePanel.TheDevice.drop_wait_OK)
                        {

                            AppendTextToUser(string.Format("Nem megfelelő parancs sorrend a {0} porton{1}Kezdje újra a mérést a {2} számú LOT-ból{1}", ThePanel.Name, Environment.NewLine, Users_Form.dev.LOT_ID), Users_Form, Color.Red);

                            ThePanel.TheDevice.BarCode = Users_Form.dev.BarCode;

                            wrong_step = "Strip wait";
                            Trace.TraceError(string.Format("Incorrect Command sequence on port:{0} at Strip Wait", ThePanel.Name));
                            DiscardBuffers(Users_Form);
                            StoreData();

                            if (Program.measureType != "accuracy")
                            {
                                ClearAllData();
                            }
                        }
                        else
                        {
                            ThePanel.TheDevice.strip_wait_OK = true;

                            command = (int)ECommands.stripWait;
                            if (Program.measureType == "meo")
                            {
                                AppendTextToUser(string.Format("{0}Tegye be a checkstrip-et{0}", Environment.NewLine), Users_Form, Color.Black);
                            }
                            else
                                AppendTextToUser(string.Format("{0}Várakozás a csíkra{0}", Environment.NewLine), Users_Form, Color.Black);

                            WriteNote(string.Format("\nCommand(strip_wait):\n{0}\n", command), ThePanel.Name);
                            Trace.TraceInformation(string.Format("\nCommand(strip_wait):\n{0}\n", command));
                        }
                        #endregion
                        break;
                    }

                    if ((Data & command_mask) == 0x0800)
                    {
                        /*Strip_in*/
                        #region Strip In

                        Trace.TraceInformation("Received data: strip_in: {0}", ThePanel.Name);
                        ThePanel.TheDevice.Original_MeasureID = (MeasureID_high << 8) + MeasureID_low;
                        ThePanel.TheDevice.Original_SerialNumber = (SerialNumber_HH << 24) + (SerialNumber_HL << 16) + (SerialNumber_LH << 8) + (SerialNumber_LL);

                        #region checkMEO

                        //HACK remove before validation    CheckDeviceMeasureability();
                        #endregion

                        Thread.Sleep(50);
                        if (ThePanel.TheDevice.drop_wait_OK || ThePanel.TheDevice.drop_detect_OK || ThePanel.TheDevice.strip_in_OK)
                        {
                            AppendTextToUser(string.Format("Nem megfelelő parancs sorrend a {0} porton{1}Kezdje újra a mérést a {2} számú LOT-ból{1}",
                            ThePanel.Name, Environment.NewLine, Users_Form.dev.LOT_ID), Users_Form, Color.Red);

                            Trace.TraceError(string.Format("Incorrect Command sequence on port:{0} at Strip In", ThePanel.Name));
                            wrong_step = "Strip in";
                            DiscardBuffers(Users_Form);
                            StoreData();
                            ClearAllData();


                        }
                        else
                        {

                            command = (int)ECommands.stripIn;
                            AppendTextToUser(string.Format("{0}Csík érzékelve{0}", Environment.NewLine), Users_Form, Color.Blue);
                            WriteNote(string.Format("\nCommand(strip_in):\n{0}\n", command), actForm.Name);

                            ThePanel.TheDevice.strip_in_OK = true;
                            ThePanel.TheDevice.strip_wait_OK = true;
                        }
                        #endregion

                        break;
                    }
                    if ((Data & command_mask) == 0x1000)
                    {       /*Drop wait*/
                        #region Drop Wait
                        Thread.Sleep(50);
                        CheckMeasureIdAndSerial();

                        Trace.TraceInformation("Received data: drop_wait: {0}", ThePanel.Name);
                        if (!ThePanel.TheDevice.strip_in_OK || !ThePanel.TheDevice.strip_wait_OK || !ThePanel.TheDevice.code_OK)
                        {
                            AppendTextInBox(string.Format("\nThere wasn't 'Strip In' or 'Strip Wait' command before 'Drop wait'\nMeasurement will be restarted"), ThePanel, Color.Red);
                            AppendTextToUser(string.Format("Nem megfelelő parancs sorrend a {0} porton{1}Kezdje újra a mérést a {2} számú LOT-ból{1}", ThePanel.Name,
                                Environment.NewLine, Users_Form.dev.LOT_ID), Users_Form, Color.Red);
                            WriteNote(string.Format("\nThere wasn't 'Strip In' or 'Strip Wait' command before 'Drop wait'\nMeasurement will be restarted"), ThePanel.Name);
                            wrong_step = "Drop Wait";
                            Trace.TraceError(string.Format("Incorrect Command sequence on port:{0} at Drop Wait", ThePanel.Name));
                            DiscardBuffers(Users_Form);
                            StoreData();
                            ClearAllData();
                            //    SetLotId(string.Format("{0}", Users_Form.dev.LOT_ID), string.Format("{0}", Users_Form.dev.BarcodeFirst));

                        }
                        else
                        {

                            ThePanel.TheDevice.drop_wait_OK = true;

                            command = (int)ECommands.dropWait;
                            if (Program.measureType == "meo")
                            {
                                AppendTextToUser(string.Format("{0}Kérem várjon...{0}", Environment.NewLine), Users_Form, Color.Black);
                            }
                            else
                                AppendTextToUser(string.Format("{0}Cseppentésre várás{0}", Environment.NewLine), Users_Form, Color.DarkCyan);
                            WriteNote(string.Format("\nCommand(drop_wait):\n{0}\n", command), ThePanel.Name);
                        }
                        #endregion
                        break;
                    }
                    if ((Data & command_mask) == 0x1800)
                    {
                        /*Drop detect*/
                        #region Drop detect
                        Thread.Sleep(50);
                        CheckMeasureIdAndSerial();
                        Trace.TraceInformation("Received data: drop_detect: {0}", ThePanel.Name);
                        if (Program.measureType == "meo")
                        {
                            ThePanel.TheDevice.code_OK = true;
                            ThePanel.TheDevice.drop_wait_OK = true;
                        }

                        if (!ThePanel.TheDevice.drop_wait_OK || !ThePanel.TheDevice.code_OK)
                        {
                            AppendTextToUser(string.Format("{0}Hibás parancs sorrend a {1} porton!{0}Kezdje újra a mérést a {2} számú LOT-ból!{0}",
          Environment.NewLine, ThePanel.Name, Users_Form.dev.LOT_ID), Users_Form, Color.Red);
                            Trace.TraceError(string.Format("Incorrect Command sequence on port:{0} at Drop Detect", ThePanel.Name));
                            WriteNote(string.Format("\nThere wasn't 'Drop Wait' command before 'Drop detect' on {0} port\nPlease Restart the Measurement\n", ThePanel.Name), ThePanel.Name);
                            wrong_step = "Drop detect";

                            StoreData();
                            ClearAllData();
                            DiscardBuffers(Users_Form);
                            //SetLotId(string.Format("{0}", Users_Form.dev.LOT_ID), string.Format("{0}", Users_Form.dev.BarcodeFirst));

                        }
                        else
                        {
                            ThePanel.TheDevice.drop_detect_OK = true;
                            command = (int)ECommands.dropDetect;
                            if (Program.measureType == "meo")
                            {
                                AppendTextToUser(string.Format("{0}Készülék mérőképességének ellenőrzése{0}", Environment.NewLine), Users_Form, Color.Black);
                            }
                            else
                                AppendTextToUser(string.Format("{0}Cseppentés érzékelve{0}Feldolgozás...{0}", Environment.NewLine), Users_Form, Color.DarkBlue);
                            WriteNote(string.Format("\nCommand(drop_detect):\n{0}\n", command), ThePanel.Name);
                        }
                        #endregion
                        break;
                    }
                    if ((Data & command_mask) == 0x2000)
                    {
                        /*Used Strip*/
                        #region Used Strip
                        Thread.Sleep(50);
                        Trace.TraceInformation("Received data: used_strip: {0}", ThePanel.Name);
                        CheckMeasureIdAndSerial();
                        command = (int)ECommands.usedStrip;
                        WriteNote(string.Format(
                           "\nUsed strip on {1} port\nCommand:{0}\n", command, ThePanel.Name), ThePanel.Name);

                        wrong_step = "Used strip";
                        StoreData();

                        Clear();

                        AppendTextToUser(string.Format(
                            "{1}Használt csík a {0} porton{1}Kezdje újra a mérést a {2} számú LOT-ból!{1}",
                            actPort, Environment.NewLine, Users_Form.dev.LOT_ID), Users_Form, Color.Red);
                        //  SetLotId(string.Format("{0}", Users_Form.dev.LOT_ID), string.Format("{0}", Users_Form.dev.BarcodeFirst));
                        DiscardBuffers(Users_Form);
                        #endregion
                        break;
                    }
                    if ((Data & command_mask) == 0x2800)
                    {
                        /*MeterOff*/
                        #region MeterOff
                        Thread.Sleep(50);
                        ThePanel.TheDevice.LOT_ID = Users_Form.dev.LOT_ID;
                        ThePanel.TheDevice.BarcodeFirst = Users_Form.dev.BarcodeFirst;
                        Trace.TraceInformation("Received data: meter_off: {0}", ThePanel.Name);
                        // WriteNote(string.Format("\nDevice turned off, Meter Off command arrived\nCommand:{0}\n", command), ThePanel.Name);
                        Trace.TraceInformation(string.Format("\nMeter off\nCommand:{0}\n", command));
                        command = (int)ECommands.meterOff;


                        AppendTextToUser(string.Format("{1}Készülék kikapcsolt a {0} porton{1}", ThePanel.Name, Environment.NewLine), Users_Form, Color.DarkBlue);
                        WriteNote(string.Format("\nMeter Off\nCommand:{0}\n", command), ThePanel.Name);
                        DiscardBuffers(Users_Form);
                        #endregion
                        break;
                    }
                    if ((Data & command_mask) == 0x3000)
                    {
                        /*BatFlag*/
                        #region BatFlag
                        Thread.Sleep(50);
                        CheckMeasureIdAndSerial();
                        Trace.TraceInformation("Received data: batflag: {0}", ThePanel.Name);
                        if (!ThePanel.TheDevice.BatteryFlagOn)
                        {
                            ThePanel.TheDevice.BatteryFlagOn = true;
                            //  AddLowBatteryImage(ThePanel);
                            //  AddLowBatteryImage(Users_Form);
                        }
                        WriteNote(string.Format("\nLow battery  at {1} port!\nCommand:{0}\n", command, ThePanel.Name), ThePanel.Name);
                        Trace.TraceInformation(string.Format("\nLow battery at {1} port!\nCommand:{0}\n", command, ThePanel.Name));

                        ThePanel.TheDevice.batterIsLowCounter++;
                        if (ThePanel.TheDevice.batterIsLowCounter >= 20)
                        {
                            // new Thread(() => AskBatteryChange(string.Format("Alacsony elemfeszültség a {0} porton!", ThePanel.Name), actPort,ThePanel,Users_Form)).Start();                                                     

                        }
                        else
                        {
                            command = (int)ECommands.batFlag;
                            WriteNote(string.Format("\nLow battery at {1} port\nCommand:{0}\n", command, ThePanel.Name), ThePanel.Name);
                            Trace.TraceInformation(string.Format("\nLow battery\nCommand:{0}\n", command));
                        }
                        DiscardBuffers(Users_Form);
                        #endregion
                        break;
                    }

                    throw new ArgumentException(string.Format("Kommunikációs hiba\nKezdje újra a mérést\nInvalid value:{0}", Data & command_mask));

                    #endregion

                case 0xc000:
                    /*Code*/
                    #region Code
                    Thread.Sleep(50);
                    CheckMeasureIdAndSerial();

                    ThePanel.TheDevice.code_OK = true;
                    ThePanel.TheDevice.ThaCode = (Data & 0x0fff);
                    Users_Form.dev.ThaCode = (Data & 0x0fff);

                    #region checkCode
                    if ((Program.measureType == "homogenity") && (ConvertFromBcd(ThePanel.TheDevice.ThaCode) != 777) && (Program.measureType != "meo"))
                    {
                        MessageBox.Show("Nem megfelelő kóddugó, a Homogenity vizsgálathoz 777-es kóddugót használjon!");
                        ClearAllData();

                    }
                    else if ((Program.measureType == "blank") && (ConvertFromBcd(ThePanel.TheDevice.ThaCode) != 170))
                    {
                        MessageBox.Show("Nem megfelelő kóddugó, a Blank Current vizsgálathoz 170-es kóddugót használjon!");
                        ClearAllData();

                    }
                    else if ((Program.measureType == "accuracy") && (ConvertFromBcd(ThePanel.TheDevice.ThaCode) != 777))
                    {
                        MessageBox.Show("Nem megfelelő kóddugó, az accuracy vizsgálathoz 777-es kóddugót használjon!");
                        ClearAllData();

                    }
                    else if ((Program.measureType == "meo") && (ConvertFromBcd(ThePanel.TheDevice.ThaCode) != 772))
                    {
                        MessageBox.Show("Nem megfelelő kóddugó, a meo készülék vizsgálathoz 772-es kóddugót használjon!");
                        ClearAllData();

                    }
                    #endregion

                    Trace.TraceInformation(string.Format("Received data is code:{0}", ConvertFromBcd(ThePanel.TheDevice.ThaCode)));
                    WriteNote(string.Format("Received data is code:{0}", ConvertFromBcd(ThePanel.TheDevice.ThaCode)), ThePanel.Name);


                    AppendTextToUser(string.Format("{1}Code:{1}{0}{1}", ConvertFromBcd(ThePanel.TheDevice.ThaCode), Environment.NewLine), Users_Form, Color.DarkBlue);

                    break;
                    #endregion
                default:
                    throw new ArgumentException(string.Format("Kommunikációs hiba\nKezdje újra a mérést\nInvalid value:{0}", Data_high & mask));

            }
            #endregion
        }

        private void StoreSerialInput(byte[] receivedData_in, string actPort_in, int measurementPkId_in)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();

                    using (NpgsqlCommand insertSerial = new NpgsqlCommand(string.Format("insert into serial_input(com_port,serial_data_arrived,communication_date,fk_measurement_id) VALUES({0},{1},{2},{3})", Convert.ToInt32(actPort_in.Substring(3)), "@serial", "@date", measurementPkId_in), conn))
                    {
                        insertSerial.Parameters.AddWithValue("@date", DateTime.Now);
                        insertSerial.Parameters.AddWithValue("@serial", receivedData_in);

                        object result = null;

                        result = insertSerial.ExecuteNonQuery();

                        if (result == null || result == DBNull.Value)
                        {
                            Trace.TraceInformation("Unsuccessfull insert to serial input");
                            throw new Exception("unsuccessfull insert to serial_input");
                        }
                        else
                            Trace.TraceInformation("Successfull insert to serial input");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
                finally
                {

                    conn.Close();
                }
            }
        }

        private void CheckDeviceMeasureability()
        {

            if (Properties.Settings.Default.CHECK_MEO)
            {

                if (Program.measureType == "meo")
                { isMeo = true; }

                else if (Program.measureType != "meo")
                {
                    isMeo = false;

                    if (CheckMeoIsDone())
                        Trace.TraceInformation(string.Format("{0}MEO mérés rendben{0}", Environment.NewLine));

                    else
                    {
                        Trace.TraceError("There are no MEO measurement in the last 24 hours");
                        ClearAllData();
                        throw new ArgumentNullException("Nincs MEO mérés az elmúlt 24 órában");
                    }
                }
            }
            else
                isMeo = true;
        }

        delegate void SetMeasuredToOKDelegate(UserPanel userPanel);
        private void SetMeasuredToOK(UserPanel userPanel)
        {

            Trace.TraceWarning("SetMeasuredToOK method in MessageCompletedEventArgs class.");
            if ((userPanel as UserPanel).Controls["button1"].InvokeRequired)
            {
                (userPanel as UserPanel).Controls["button1"].Invoke(new SetMeasuredToOKDelegate(SetMeasuredToOK), userPanel);
            }
            else
                foreach (Control lb in userPanel.Controls)
                {
                    if (lb is Button
                        && lb.Name.Equals("button1"))
                    {
                        Image image1 = Properties.Resources._94;
                        ((Button)lb).Image = image1;
                        ((Button)lb).ImageAlign = ContentAlignment.MiddleLeft;
                    }
                }
        }

        public int tube_sn;
        private void DiscardBuffers(UserPanel Users_Form)
        {

            Trace.TraceInformation("Portname: {0}", Users_Form.ownedPort.PortName);
            if (Users_Form.ownedPort.IsOpen)
            {
                Users_Form.ownedPort.DiscardInBuffer();
                Users_Form.ownedPort.DiscardOutBuffer();
                Trace.TraceInformation("Port is discarded: {0}", Users_Form.ownedPort.PortName);
            }
            else
                Trace.TraceInformation("Port is closed: {0}", Users_Form.ownedPort.PortName);
        }

        #region Methods

        public bool CheckMeoIsDone()
        {

            object pk = null;
            object lastStartDate;

            using (NpgsqlConnection connection = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    connection.Open();

                    using (NpgsqlCommand scalar = new NpgsqlCommand(string.Format(
                        "select Max(date) from device_meo where ok=True and serial_number={1} limit 1", Convert.ToInt32(pk), ThePanel.TheDevice.Original_SerialNumber), connection))
                    {
                        lastStartDate = scalar.ExecuteScalar();
                        if ((lastStartDate == DBNull.Value)
                            || (lastStartDate == null))
                        {
                            Trace.TraceError("LastStartDate is null");
                            connection.Close();
                            return false;
                        }

                        DateTime lastStart = Convert.ToDateTime(lastStartDate);

                        if ((lastStart < (DateTime.Now - new TimeSpan(Properties.Settings.Default.MEO_CHECK_VALID_TIMERANGE_IN_HOURS, 0, 0))))
                        {
                            Trace.TraceError("LastStartDate is not in last 24 hours");
                            connection.Close();
                            return false;
                        }
                    }//check lastStartDate
                }//try
                catch (Exception ex)
                {
                    Trace.TraceError(string.Format("Exception at MessageCompletedEventArgs.CheckMeoIsDone():{0}", ex.Message));
                    connection.Close();
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }//NpgsqlConnection

            Trace.TraceInformation("Meo is ok");
            return true;//if OK
        }

        public void WriteNote(string txt, string port)
        {
            GetMonth();
            Trace.TraceInformation("MessageCompletedEventArgs.WriteNote()");
            string path = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));  //Get the path of the 'Debug' directory

            if (!Directory.Exists(string.Format("{0}\\..\\..\\Logs", path)))//if Logs directory not exist
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(string.Format("{0}\\..\\..\\", path), "Logs"));
            }
            if (!ThePanel.TheDevice.IsLogging)
            {
                ThePanel.TheDevice.IsLogging = true;

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(string.Format("{0}\\..\\..\\Logs\\{2}_{1}.txt", path, month, port), true))
                {
                    file.WriteLine(string.Format("{0} - {1} - By User:{2} on {3} computer", DateTime.Now.ToString(), txt, controllerUser, machineName));
                }
                ThePanel.TheDevice.IsLogging = false;
            }
        }
        public static void GetMonth()
        {
            month = System.DateTime.Now.Month;

        }

        public static void AppendingWhenInvoked(string txt, Color color, Control c)
        {
            Trace.TraceWarning("AppendingWhenInvoked method in MessageCompletedEventArgs class");
            int startSelect = ((RichTextBox)c).Text.Length;
            ((RichTextBox)c).AppendText(txt);
            int endSelect = ((RichTextBox)c).Text.Length;
            ((RichTextBox)c).Select(startSelect, endSelect);
            ((RichTextBox)c).SelectionColor = color;
            ((RichTextBox)c).ScrollToCaret();

        }
        public void AskBatteryChange(string question, String _port, Panel actPanel, Panel usersPanel)
        {
            WriteNote("MessageCompletedEventArgs.AskBatteryChange()", ThePanel.Name);
            Trace.WriteLine("MessageCompletedEvenArgs.AskBatteryChange()");
            if (!ThePanel.TheDevice.IsBatteryShown)
            {
                ThePanel.TheDevice.IsBatteryShown = true;
                result = MessageBox.Show(string.Format("Cseréljen elemet a {0} porton, majd kattintson az OK gombra!", _port), question, MessageBoxButtons.OK);
                if (result == System.Windows.Forms.DialogResult.OK)
                {

                    //  AppendTextInBox("\n\nLow Battery changed to a new one\n\n", ThePanel, Color.Red);                    
                    AppendTextToUser(string.Format("{0}Elem kicserélve, kezdje újra a mérést{0}", Environment.NewLine), Users_Form, Color.Red);

                    Trace.WriteLine("Low Battery changed to a new one");
                    WriteNote("Low Battery changed to a new one", ThePanel.Name);


                    HideIcon(actPanel, battery_label);
                    HideIcon(usersPanel, battery_label);

                    ThePanel.TheDevice.BatteryFlagOn = false;
                    ThePanel.TheDevice.batterIsLowCounter = 0;

                    ThePanel.TheDevice.IsBatteryShown = false;
                    ClearAllData();
                }

            }
        }
        public void ShowError(string errorText)
        {
            Trace.WriteLine(string.Format("MessageCompletedEvenArgs.ShowError():{0}", errorText));
            if (!ThePanel.TheDevice.IsErrorShown)
            {
                ThePanel.TheDevice.IsErrorShown = true;
                result = MessageBox.Show(errorText, "\nMérés újraindítva\n", MessageBoxButtons.OK);
                WriteNote(string.Format("Measurement restarted is accepted by {0} on {1}", ThePanel.Name, controllerUser), ThePanel.Name);
                if (result == System.Windows.Forms.DialogResult.OK)
                {

                    //   AppendTextInBox("\nMeasurement restarted, waiting for data:\n\n", ThePanel, Color.Red);
                    AppendTextToUser(string.Format("{0}Mérés újrakezdve, várakozás az adatokra:{0}{0}", Environment.NewLine), Users_Form, Color.Red);

                    Trace.WriteLine(string.Format("Measurement restarted because of:{0}, waiting for data", errorText));

                    ClearAllData();
                    ThePanel.TheDevice.ClearAfterFirstReceive = true;
                    ThePanel.TheDevice.IsErrorShown = false;

                }
            }
        }

        public bool CheckMeasureIdAndSerial()
        {
            #region if measureid || serialnumber ==0
            if (ThePanel.TheDevice.Original_MeasureID == 0)
            {
                ThePanel.TheDevice.Original_MeasureID = (MeasureID_high << 8) + MeasureID_low;
            }
            else if (ThePanel.TheDevice.Original_SerialNumber == 0)
            {

                ThePanel.TheDevice.Original_SerialNumber = (SerialNumber_HH << 24) + (SerialNumber_HL << 16) + (SerialNumber_LH << 8) + (SerialNumber_LL);
            }

            #endregion
            #region Calculate actual measureID and SerialNumber
            if (Calculated_MeasureID == 0 || Calculated_SerialNumber == 0)
            {
                Calculated_MeasureID = (MeasureID_high << 8) + MeasureID_low;
                Calculated_SerialNumber = (SerialNumber_HH << 24) + (SerialNumber_HL << 16) + (SerialNumber_LH << 8) + (SerialNumber_LL);


                AppendTextToUser(string.Format("Érkezett MeasureID vagy SerialNumber  0!Kezdje Újra a mérést a {0} számú LOT-ból", Users_Form.dev.LOT_ID), Users_Form, Color.Red);
                WriteNote("Calculated_MeasureID==0 || Calculated_SerialNumber==0", ThePanel.Name);
                Trace.TraceInformation("Calculated_MeasureID==0 or Calculated_SerialNumber==0");
                wrong_step = "Calculated MeasureID or SerialNumber is 0";

                StoreData();
                ClearAllData();
                DiscardBuffers(Users_Form);

                return false;
            }
            #endregion
            #region check MeasureID and SerialNumber
            if (Calculated_MeasureID != ThePanel.TheDevice.Original_MeasureID
                 || Calculated_SerialNumber != ThePanel.TheDevice.Original_SerialNumber)
            {

                AppendTextToUser(string.Format("Különböző MeasureID vagy SerialNumber! Kezdje Újra a mérést a {0} számú LOT-ból", Users_Form.dev.LOT_ID), Users_Form, Color.Red);
                SwitchSerialOrMeasureId();
                DiscardBuffers(Users_Form);
                ClearAllData();

                return false;
            }
            else
                return true;//if OK
            #endregion
        }

        private void SwitchSerialOrMeasureId()
        {
            if (Calculated_MeasureID != ThePanel.TheDevice.Original_MeasureID)
            {
                #region Different MeasureID


                WriteNote(string.Format("Different MeasureID({0}) than the original one:{1}", Calculated_SerialNumber, ThePanel.TheDevice.Original_SerialNumber), ThePanel.Name);
                Trace.TraceInformation(string.Format("Different MeasureID({0}) than the original one:{1}", Calculated_SerialNumber, ThePanel.TheDevice.Original_SerialNumber));
                wrong_step = "Different MeasureID";


                DiscardBuffers(Users_Form);
                StoreData();
                ClearAllData();


                AppendTextToUser(string.Format("Different MeasureID({0}) than the original one:{1}\nKezdje Újra a mérést a {2} számú LOT-ból!\n",
                    Calculated_MeasureID, ThePanel.TheDevice.Original_MeasureID, Users_Form.dev.LOT_ID), Users_Form, Color.Red);

                return;

                #endregion
            }
            else if (Calculated_SerialNumber != ThePanel.TheDevice.Original_SerialNumber)
            {
                #region Different SerialNumber

                WriteNote(string.Format("Different SerialNumber({0}) than the original one:{1}", Calculated_SerialNumber, ThePanel.TheDevice.Original_SerialNumber), ThePanel.Name);
                Trace.TraceInformation(string.Format("Different SerialNumber({0}) than the original one:{1}", Calculated_SerialNumber, ThePanel.TheDevice.Original_SerialNumber));
                wrong_step = "Different SerialNumber";


                DiscardBuffers(Users_Form);
                StoreData();
                Clear();


                AppendTextToUser(string.Format("Different SerialNumber({0}) than the original one:{1}\nKezdje Újra a mérést a {2} számú LOT-ból!\n",
                    Calculated_SerialNumber, ThePanel.TheDevice.Original_SerialNumber, Users_Form.dev.LOT_ID), Users_Form, Color.Red);

                return;

                #endregion
            }
            WriteNote("MessageCompletedEventArgs.SwitchSerialOrMeasureId()", ThePanel.Name);
            Trace.TraceInformation("MessageCompletedEventArgs.SwitchSerialOrMeasureId()");

        }

        public void ClearAllData()
        {
            WriteNote("MessageCompletedEventArgs.ClearAllData()", ThePanel.Name);
            Trace.TraceInformation("MessageCompletedEventArgs.ClearAllData()");
            DiscardBuffers(Users_Form);

            ThePanel.TheDevice.strip_wait_OK = false;
            ThePanel.TheDevice.strip_in_OK = false;
            ThePanel.TheDevice.drop_detect_OK = false;
            ThePanel.TheDevice.drop_wait_OK = false;
            ThePanel.TheDevice.code_OK = false;

            code = 0;
            error_e = 0;
            error_h = 0;
            glu = 0;

            wrong_step = string.Empty;
        }

        public void Clear()
        {
            ChangeText("\nMérés újrakezdve\n", Users_Form);
            DiscardBuffers(Users_Form);


            WriteNote("MessageCompletedEventArgs.Clear()", ThePanel.Name);
            Trace.TraceInformation("MessageCompletedEventArgs.Clear()");

            ThePanel.TheDevice.strip_wait_OK = false;
            ThePanel.TheDevice.strip_in_OK = false;
            ThePanel.TheDevice.drop_detect_OK = false;
            ThePanel.TheDevice.drop_wait_OK = false;
            ThePanel.TheDevice.code_OK = false;

            code = 0;
            error_e = 0;
            error_h = 0;
            glu = 0;

            wrong_step = string.Empty;
        }

        public Label lowBattery_label = new Label();
        #region Unused - TODO: Remove this carefully
        //public void AddLowBatteryImage(Panel actPanelThis)
        //{
        //    DetailedPanel LikeThis=new DetailedPanel();
        //    UserPanel LikeThat = new UserPanel(); 

        //    LikeThis = ThePanel;             
        //    LikeThat = Users_Form;

        //    if (!LikeThis.TheDevice.IsBatteryShown && !LikeThat.dev.IsBatteryShown)
        //    {
        //        LikeThis.TheDevice.IsBatteryShown = true;
        //        LikeThat.dev.IsBatteryShown = true;

        //        WriteNote("MessageCompletedEventArgs.AddLowBatteryImage()", ThePanel.Name);

        //        string path = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));   

        //        battery_label = new Label();
        //        battery_label.Image = Properties.Resources.low_battery1;

        //        ShowIcon(actPanelThis, battery_label);
        //        ShowIcon(actPanelThis, battery_label);

        //        LikeThis.TheDevice.IsBatteryShown = false;
        //        LikeThat.dev.IsBatteryShown = false;
        //    }            
        //} 
        #endregion
        public int GetChecksum(byte[] data)
        {

            Trace.TraceInformation("MessageCompletedEventArgs.GetCheckSum()");
            byte crc = 0;
            for (int i = 0; i < data.Length - 1; i++)
            {
                crc += data[i];
            }

            return 255 - crc;
        }
        public int ConvertFromBcd(int bcd)
        {
            WriteNote("MessageCompletedEventArgs.ConvertFromBcd()", ThePanel.Name);
            Trace.TraceInformation("MessageCompletedEventArgs.ConvertFromBcd()");
            int result = 0;

            int digit1 = bcd & 0x0f;
            int digit2 = (bcd >> 4) & 0x0f;
            int digit3 = (bcd >> 8) & 0x0f;
            result = (digit3 * 100) + digit2 * 10 + digit1;

            return result;
        }

        public void StoreMeo(bool measurableDevice)
        {
            Trace.TraceInformation("Store device meo data");

            using (NpgsqlConnection sqlConn = new NpgsqlConnection(Program.dbConnection))
            {

                sqlConn.Open();
                try
                {
                    #region insert to device_meo
                    using (NpgsqlCommand insertComm = new NpgsqlCommand(string.Format("insert into device_meo(serial_number,glu,date,ok) values({0},{1},{2},{3})",/*0*/ThePanel.TheDevice.Original_SerialNumber,/*1*/ glu / 18.02,/*2*/ "@start", measurableDevice), sqlConn))
                    /*insert to device_meo*/
                    {

                        insertComm.Parameters.AddWithValue("@start", DateTime.Now);
                        insertComm.ExecuteNonQuery();
                        Trace.TraceInformation(string.Format("insert to device_meo, query: {0}", insertComm.CommandText));
                    }//insert to device_meo

                    #endregion
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception at storing meo check data, ex: {0}", ex.StackTrace);
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        /// <summary>
        /// Store data in sql tables...
        /// </summary>
        public void StoreData()
        {

            foreach (Control item in UserWindow.userTables.Controls)
            {
                if (item is UserPanel)
                {
                    ((UserPanel)item).dev.LOT_ID = Program.LOT_ID;
                }

            }

            #region Variables
            object pk = null;
            object errors_id = null;
            object valid_strip_count = null;
            object valid_strip_count2 = null;

            int tha_strip_count_in_measure = 0;
            string lotid = Users_Form.dev.LOT_ID;
            string[] snAndRoll = new string[2];
            #endregion

            try
            {
                Trace.TraceWarning("TextBoxBarcode2Text property value set to {0}", Users_Form.tbBarcode2.Text);
                TextBoxBarcode2Text = Users_Form.tbBarcode2.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);


            }

            Trace.TraceInformation("Sn And Roll Barcode in the textbox is: {0}", TextBoxBarcode2Text);

            if (TextBoxBarcode2Text.Contains(' '))
            {
                snAndRoll = TextBoxBarcode2Text.Split(' ');

                Users_Form.dev.SerialNumber = snAndRoll[0];
                Users_Form.dev.Roll = snAndRoll[1];
            }

            Users_Form.dev.BarCode = string.Format("{0} {1} {2}", Users_Form.dev.LOT_ID, Users_Form.dev.SerialNumber, Users_Form.dev.Roll);
            WriteNote("MessageCompletedEventArgs.StoreData()", ThePanel.Name);
            Trace.TraceInformation(string.Format("MessageCompletedEventArgs.StoreData()_barcode:{0}", Users_Form.dev.BarCode));
            bool temperature_ok;

            #region GetTemperature

            temperature = Program.Temperature;
            if ((temperature <= 26)
                && (temperature >= 20))
            {
                temperature_ok = true;
            }
            else
                temperature_ok = false;

            Trace.TraceInformation(string.Format("Temperature is ok,temperature:{0}", temperature));

            #endregion

            Trace.TraceInformation("lot,roll,sn: {0},{1},{2}", Users_Form.dev.LOT_ID, Users_Form.dev.Roll, Users_Form.dev.SN);
            Trace.TraceInformation("Wrong_step: {0}", wrong_step);
            Trace.TraceInformation("Early dribbling: {0}", Users_Form.dev.EarlyDribble);
            Trace.TraceInformation("Error text: {0}", Users_Form.dev.Error_H_Text);
            Trace.TraceInformation("Errors: Not_H62_error:{0} H62_error:{1} Not_H62_count:{2} H62_count:{3}", CheckError.NOT_H62, CheckError.H62, Users_Form.dev.NotH62_Error, Users_Form.dev.H62_Error);

            using (NpgsqlConnection sqlConn = new NpgsqlConnection(Program.dbConnection))
            {

                sqlConn.Open();
                try
                {

                    #region Inserting Values To The Common Tables

                    if ((ThePanel.TheDevice.wrong_step != string.Empty) && (wrong_step == string.Empty) && (glu == 0))
                    {
                        wrong_step = ThePanel.TheDevice.wrong_step;
                    }

                    #region insert errors
                    using (NpgsqlCommand error_comm = new NpgsqlCommand(
                            string.Format("insert into blank_test_errors(error,error_text,not_h62_error,h62_error,early_dribble,device_replace,remeasured,invalidate,lot_id,roll_id,sn) values('{0}','{1}',{2},{3},{4},{5},{6},{7},'{8}','{9}','{10}') ",
                                wrong_step, Users_Form.dev.Error_H_Text, CheckError.NOT_H62, CheckError.H62, CheckError.EarlyDripp,
                                CheckError.DeviceReplace, false, false, Program.LOT_ID, Users_Form.dev.Roll,
                                Users_Form.dev.SerialNumber), sqlConn))
                    {
                        object res = null;
                        res = error_comm.ExecuteNonQuery();

                        Trace.TraceInformation("insert to blank_test_errors");

                        if ((res == null)
                            || (res == DBNull.Value))
                        {
                            Trace.TraceError("res is null,res:{0} ,query: {1}", res, error_comm.CommandText);
                            throw new ArgumentNullException("res is null");
                        }
                    }//inserting to blank_test_errors


                    #endregion

                    #region get max pkid from errors
                    using (NpgsqlCommand error_scalar = new NpgsqlCommand(string.Format("select max(pk_id) from blank_test_errors"), sqlConn))
                    {
                        errors_id = error_scalar.ExecuteScalar();
                        if ((errors_id == null)
                            || (errors_id == DBNull.Value))
                        {
                            Trace.TraceError("errors_id is null,errors_id: {0} ,query {1}", errors_id, error_scalar.CommandText);
                            throw new ArgumentNullException("errors_id is null");
                        }
                    }
                    #endregion

                    #region insert to measure_type

                    using (NpgsqlCommand insert_meas_type = new NpgsqlCommand(
                            string.Format("insert into measure_type(fk_blank_test_errors_id,measure_type,remeasured,lot_id,roll_id,sn) values({0},'{1}',{2},'{3}','{4}','{5}')",
                            errors_id, Program.measureType, false, Users_Form.dev.LOT_ID, Users_Form.dev.Roll, Users_Form.dev.SerialNumber), sqlConn))
                    {
                        insert_meas_type.ExecuteNonQuery();
                        Trace.TraceInformation("Insert to measure_type, query: {0}", insert_meas_type.CommandText);
                    }//inserting to measure_type
                    #endregion

                    #region insert to results

                    using (NpgsqlCommand insertComm = new NpgsqlCommand(string.Format(
                            "insert into blank_test_result(code,glu,measure_id,serial_number,is_check_strip,fk_blank_test_errors_id,nano_amper,master_lot,remeasured,invalidate,lot_id,roll_id,sn) values({0},{1:0.00},{2},{3},{4},{5},{6:0.00},{7},{8},{9},'{10}','{11}','{12}')",
                            ConvertFromBcd(ThePanel.TheDevice.ThaCode)/*{0}*/, Math.Round(Convert.ToDouble(glu / 18.0), 2)/*{1}*/, ThePanel.TheDevice.Original_MeasureID/*{2}*/,
                            ThePanel.TheDevice.Original_SerialNumber/*{3}*/, isMeo/*{4}*/, errors_id/*{5}*/, Convert.ToDouble(glu / 18.0) * 10/*6*/,
                            Users_Form.dev.MasterLot/*7*/, false, false, Users_Form.dev.LOT_ID, Users_Form.dev.Roll, Users_Form.dev.SerialNumber),
                            sqlConn))                                                                       /*insert to blank_test_result*/
                    {

                        CheckStripValue(glu);

                        object res = null;
                        res = insertComm.ExecuteNonQuery();

                        if (res == DBNull.Value)
                        {
                            Trace.TraceError("Unsuccessful insert to blank_test_result table: insert statement:{0}", insertComm.CommandText);
                            throw new SqlNoValueException("unsuccess insert");
                        }

                        Trace.TraceInformation(string.Format("insert to result, query: {0}", insertComm.CommandText));
                    }//inserting to blank_test_result

                    #endregion

                    #region get max pkid from results
                    using (NpgsqlCommand insertComm = new NpgsqlCommand(string.Format(
                            "select max(pk_id) from blank_test_result limit 1"),
                            sqlConn))                                                                       /*get pk_id of blank_test_result*/
                    {

                        pk = insertComm.ExecuteScalar();
                        Trace.TraceInformation("get blank_test_result pk_id");
                        if ((pk == DBNull.Value)
                            || (pk == null))
                        {
                            Trace.TraceError(string.Format("pk_id(blank_test_result) is null in StoreData,query: {0}", insertComm.CommandText));
                            throw new ArgumentNullException("pk_id is null");
                        }
                    }
                    #endregion

                    #region insert to environment
                    using (NpgsqlCommand insertComm = new NpgsqlCommand(string.Format(
                            "insert into blank_test_environment(user_name,computer_name,start_date,end_date,temperature,fk_blank_test_result_id,remeasured,invalidate,lot_id,roll_id,sn,humidity) values('{0}','{1}',{2},{3},{4},{5},{6},{7},'{8}','{9}','{10}',{11})",
                        /*0*/controllerUser,/*1*/ machineName,/*2*/ "@start",/*3*/ "@end",/*4*/ temperature,/*5*/ Convert.ToInt32(pk),/*6*/ false,/*7*/ false,/*8*/ Users_Form.dev.LOT_ID,/*9*/ Users_Form.dev.Roll,/*10*/ Users_Form.dev.SerialNumber,/*11*/Program.Humidity), sqlConn))
                    /*insert to blank_test_environment*/
                    {
                        insertComm.Parameters.AddWithValue("@start", ThePanel.TheDevice.Start_Date);
                        insertComm.Parameters.AddWithValue("@end", DateTime.Now);
                        insertComm.ExecuteNonQuery();
                        Trace.TraceInformation(string.Format("insert to blank_test_environment, query: {0}", insertComm.CommandText));
                    }//insert to environment

                    #endregion

                    #region insert to identify
                    using (NpgsqlCommand insertComm = new NpgsqlCommand(string.Format(
                            "insert into blank_test_identify(fk_blank_test_result_id,lot_id,roll_id,temperature_ok,sub_roll_id,serial_number,barcode,measure_type,remeasured,invalidate) values({0},'{1}','{2}',{3},'{4}','{5}','{6}','{7}',{8},{9})",
                            Convert.ToInt32(pk), Users_Form.dev.LOT_ID, Users_Form.dev.Roll, temperature_ok, Users_Form.dev.SubRoll_ID, Users_Form.dev.SerialNumber, Users_Form.dev.BarCode,
                            Program.measureType, false, false), sqlConn))           /*insert to blank_test_identify */
                    {

                        insertComm.ExecuteNonQuery();
                        Trace.TraceInformation(string.Format("insert to blank_test_identify, query: {0}", insertComm.CommandText));
                    }//inserting to identify
                    #endregion

                    if (Program.measureType == "homogenity")
                    {

                        #region insert to homogenity_test
                        using (NpgsqlCommand homogenity_comm = new NpgsqlCommand(
                        string.Format("insert into  homogenity_test(strip_ok,fk_blank_test_result_id,roll_id,lot_id,invalidate,remeasured,sn) values({0},{1},'{2}','{3}',{4},{5},'{6}')",
                        Users_Form.dev.IsResultValid, Convert.ToInt32(pk), Users_Form.dev.Roll, Users_Form.dev.LOT_ID, false, false, Users_Form.dev.SerialNumber), sqlConn))
                        {
                            homogenity_comm.ExecuteNonQuery();
                        }//insert to homogenity_test
                        #endregion
                    }
                    else if (Program.measureType == "accuracy")
                    {
                        #region Insert to Accuracy test
                        if (Users_Form.MasterChb.Checked)
                        {
                            using (NpgsqlCommand accuracyTest = new NpgsqlCommand(
                        string.Format("insert into accuracy_test(glu,calibrated_glu,lot_id,roll_id,blood_vial_id,invalidate,is_master,fk_accuracy_values_id,measured) values({0},{1},'{2}','{3}','{4}',{5},{6},{7},{8})", Math.Round(Convert.ToDouble(glu / 18.0), 2), Math.Round(Program.master_calibration * (glu / 18.0), 2), Program.master_lot_id, Users_Form.dev.Roll, Program.Accuracy_sample_blood_vial_ID, false, Users_Form.MasterChb.Checked, Program.ValuesID, false), sqlConn))
                            {
                                object res = null;
                                res = accuracyTest.ExecuteNonQuery();

                                if (res == null)
                                {
                                    Trace.TraceError("Unsuccessfull insert into accuracy_test: query:{0}", accuracyTest.CommandText);
                                }

                            }//insert 
                        #endregion

                        }
                        else
                        {

                            using (NpgsqlCommand accuracyTest = new NpgsqlCommand(
                                string.Format("insert into accuracy_test(glu,calibrated_glu,lot_id,roll_id,blood_vial_id,invalidate,is_master,fk_accuracy_values_id,measured) values({0},{1},'{2}','{3}','{4}',{5},{6},{7},{8})", Math.Round(Convert.ToDouble(glu / 18.0), 2), Math.Round(Program.master_calibration * (glu / 18.0), 2), Users_Form.dev.LOT_ID, Users_Form.dev.Roll, Program.Accuracy_sample_blood_vial_ID, false, Users_Form.MasterChb.Checked, Program.ValuesID, false), sqlConn))
                            {
                                object res = null;
                                res = accuracyTest.ExecuteNonQuery();

                                if (res == null)
                                {
                                    Trace.TraceError("Unsuccessfull insert into accuracy_test: query:{0}", accuracyTest.CommandText);
                                }

                            }//insert into accuracy_test
                        }
                        Program.AlltogetherAccuracyStripCount++;
                    }

                    #region getStripCount

                    #region Valid Strip Count For Blank
                    if (Program.measureType == "blank")
                    {
                        stipCount_part1 =
                      string.Format("SELECT COUNT(blank_test_identify.pk_id) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE (blank_test_errors.error_text='' and blank_test_result.glu<>0) and blank_test_identify.lot_id='{0}' and blank_test_identify.roll_id='{1}' and blank_test_identify.measure_type='{2}' and blank_test_identify.invalidate=false", Program.LOT_ID, Users_Form.dev.Roll, Program.measureType);

                        using (NpgsqlCommand stripCount1 = new NpgsqlCommand(stipCount_part1, sqlConn))      //get valid strip count )
                        {
                            valid_strip_count = stripCount1.ExecuteScalar();
                        }
                        tha_strip_count_in_measure = Convert.ToInt32(valid_strip_count);
                    }
                    #endregion

                    #region Valid Strip Count For Homogenity

                    if (Program.measureType == "homogenity")
                    {

                        ///Perhaps Obsolated and WRONG
                        stipCount_part1 =
                        string.Format("SELECT COUNT(homogenity_test.pk_id) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE (blank_test_errors.error_text='' and blank_test_result.glu<>0) and blank_test_result.code=777 and homogenity_test.lot_id='{0}' and homogenity_test.roll_id='{1}' and  homogenity_test.invalidate=false", Program.LOT_ID, Users_Form.dev.Roll, Program.measureType);
                        stipCount_part2 =
                            string.Format("SELECT COUNT(homogenity_test.pk_id) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE blank_test_errors.error_text<>'' and homogenity_test.lot_id='{0}' and homogenity_test.roll_id='{1}' and blank_test_result.code=777 and homogenity_test.invalidate=false", Program.LOT_ID, Users_Form.dev.Roll, Program.measureType);

                        using (NpgsqlCommand stripCount1 = new NpgsqlCommand(stipCount_part1, sqlConn))      //get valid strip count in two step)
                        {
                            valid_strip_count = stripCount1.ExecuteScalar();
                        }
                        using (NpgsqlCommand stripCount2 = new NpgsqlCommand(stipCount_part2, sqlConn))
                        {
                            valid_strip_count2 = stripCount2.ExecuteScalar();
                        }

                        tha_strip_count_in_measure = Convert.ToInt32(valid_strip_count) + Convert.ToInt32(valid_strip_count2);
                    }
                    #endregion

                    #endregion
                    #endregion
                    #region BlankTest

                    if (Program.measureType == "blank")
                    {
                        if (Convert.ToInt32(valid_strip_count) > 2)
                        {
                            StoreAlternation(Users_Form.dev.Roll);
                        }
                        if (Program.TubeCount % 2 != 0)
                        {
                            if (Convert.ToInt32(Convert.ToInt32(valid_strip_count)) == (Program.TubeCount) + 1)
                            {
                                Trace.TraceInformation("There are {0} valid rows with this roll in the database", tha_strip_count_in_measure);

                                GetAVGOfARoll(Users_Form.dev.Roll);          //insert averages to blank_test_averages
                                Trace.TraceInformation(string.Format("{0} tubus lemérve({0} csík)", Users_Form.dev.lot_count_in_one_roll));
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(Convert.ToInt32(valid_strip_count)) == (Program.TubeCount))
                            {
                                Trace.TraceInformation("There are {0} valid rows with this roll in the database", tha_strip_count_in_measure);

                                GetAVGOfARoll(Users_Form.dev.Roll);          //insert averages to blank_test_averages

                                Trace.TraceInformation(string.Format("{0} tubus lemérve({0} csík)", Users_Form.dev.lot_count_in_one_roll));
                                AppendTextToUser(string.Format("{0} számú roll lemérve a {1} számú LOT-ból", Users_Form.dev.Roll, Users_Form.dev.LOT_ID), Users_Form, Color.Magenta);
                            }
                        }
                    }
                    #endregion

                    #region Homogenity Test
                    else if (Program.measureType == "homogenity")
                    {


                        if (Convert.ToInt32(tha_strip_count_in_measure) > 2)
                        {
                            GetAVGOfARoll(Users_Form.dev.Roll);
                        }

                    }
                    #endregion

                    else if (Program.measureType == "accuracy")
                    {
                        try
                        {
                            #region Calculating and inserting accuracy result
                            int overall_accuracy_count = 0;
                            using (NpgsqlCommand accuracyCount = new NpgsqlCommand(
                                string.Format("SELECT COUNT(accuracy_test.calibrated_glu) FROM accuracy_test where accuracy_test.glu<>0 and accuracy_test.measured=false and accuracy_test.measured=false and accuracy_test.invalidate=false and (accuracy_test.lot_id='{1}' or accuracy_test.lot_id='{2}')", blood_vial_id, Program.LOT_ID, Program.master_lot_id), sqlConn))
                            {
                                object res = null;
                                res = accuracyCount.ExecuteScalar();
                                if ((res != null)
                                    && (res != DBNull.Value))
                                {
                                    Trace.TraceInformation("Overall accuracy stripcount:{0}", res);
                                    overall_accuracy_count = Convert.ToInt32(res);
                                }
                                else
                                    Trace.TraceError("Unsuccessfull query to get overall accuracy strip count: query:{0}", accuracyCount.CommandText);
                            }
                            if (overall_accuracy_count >= ACCURACY_STRIPCOUNT/*48*/)//48 values calulated for 1 blood sample vial
                            {
                                using (NpgsqlCommand masterCount = new NpgsqlCommand(
                                    string.Format("SELECT COUNT(accuracy_test.calibrated_glu) FROM accuracy_test where accuracy_test.is_master=true and accuracy_test.blood_vial_id='{1}' and accuracy_test.measured=false and accuracy_test.lot_id='{0}' and accuracy_test.roll_id='' and accuracy_test.glu<>0 and accuracy_test.invalidate=false", Program.master_lot_id, blood_vial_id), sqlConn))
                                {
                                    object result = null;
                                    result = masterCount.ExecuteScalar();

                                    if ((result == DBNull.Value)
                                        || (result == null))
                                    {
                                        Trace.TraceWarning("Not valid count of master lot query:{0}", masterCount.CommandText);
                                    }
                                    if (Convert.ToInt32(result) >= MASTER_STRIPCOUNT/*24*/)//Minimum 24 master measured with that vial
                                    {
                                        #region Inserting master's values
                                        int fk_test_id = 0;
                                        using (NpgsqlCommand get_accTest_id = new NpgsqlCommand("select max(pk_id) from accuracy_test", sqlConn))
                                        {
                                            object res = null;
                                            res = get_accTest_id.ExecuteScalar();

                                            if (res == null)
                                            {
                                                Trace.TraceError("No value for getting the latest accuracy_test id: {0}", get_accTest_id.CommandText);
                                                Trace.TraceWarning("Get the last executed query: {0}", get_accTest_id.CommandText);
                                            }
                                            else
                                            {
                                                Trace.TraceInformation("Successful get accuracy_test pk_id");
                                                fk_test_id = Convert.ToInt32(res);
                                            }
                                        }
                                        using (NpgsqlCommand subAVGOfMasters = new NpgsqlCommand(string.Format("select * FROM accuracy_test where accuracy_test.is_master=true and accuracy_test.blood_vial_id='{1}' and accuracy_test.lot_id='{0}' and accuracy_test.roll_id='' and accuracy_test.measured=false and accuracy_test.invalidate=false and accuracy_test.glu<>0", Program.master_lot_id, blood_vial_id), sqlConn))
                                        {
                                            using (NpgsqlDataReader dr = subAVGOfMasters.ExecuteReader())
                                            {
                                                if (dr.HasRows)
                                                {
                                                    while (dr.Read())
                                                    {
                                                        if (Convert.ToDouble(dr["calibrated_glu"]) != 0)
                                                        {
                                                            masters_calibrated_glu.Add(Convert.ToDouble(dr["calibrated_glu"]));
                                                            Trace.TraceInformation("Calibrated_glu:{0}", Convert.ToDouble(dr["calibrated_glu"]));
                                                        }

                                                    }

                                                    dr.Close();
                                                }
                                                else
                                                {
                                                    Trace.TraceError("No result for getting calibrated_glus for masters:{0}", subAVGOfMasters.CommandText);
                                                    dr.Close();

                                                }

                                            }
                                            Trace.TraceInformation("Masters' sub-avg's");
                                            List<double> master_sub_glus = new List<double>();
                                            master_sub_glus = GetSubAvg(masters_calibrated_glu);
                                            master_sub_avg = master_sub_glus.Average();
                                            double stddev = Math.Sqrt(master_sub_glus.Average(v => Math.Pow(v - master_sub_avg, 2)));
                                            Trace.TraceInformation("SD of the masters :{0}", stddev);
                                            master_stddev = stddev;
                                            Trace.TraceInformation("Calculated Sub Avg of Masters values, sub-avg:{0}", master_sub_avg);
                                        }
                                        master_cv = (master_stddev / master_sub_avg) * 100;
                                        Trace.TraceInformation("Master's CV:{0}", master_cv);
                                        using (NpgsqlCommand command = new NpgsqlCommand(
                                            string.Format("insert into accuracy_result_master(avg,stddev,cv,lot_id,blood_vial_id,fk_accuracy_test_id) values({0},{1},{2},'{3}','{4}',{5})", master_sub_avg, master_stddev, master_cv, Program.master_lot_id, blood_vial_id, fk_test_id), sqlConn))
                                        {
                                            object res = null;
                                            res = command.ExecuteNonQuery();

                                            if (res == null)
                                            {
                                                Trace.TraceError("res is null at inserting to accuracy_result, query:{0}", command.CommandText);
                                                throw new SqlNoValueException("Exception when store accuracy_result_master table values");
                                            }
                                            else
                                                Trace.TraceInformation("Successfull insert to accuracy_result_master");
                                        }//using (NpgsqlCommand)
                                        using (NpgsqlCommand centralCount = new NpgsqlCommand(
                                       string.Format("SELECT COUNT(accuracy_test.glu) FROM accuracy_test where accuracy_test.is_master=false and accuracy_test.blood_vial_id='{1}' and accuracy_test.lot_id='{0}' and accuracy_test.invalidate=false and accuracy_test.measured=false and accuracy_test.glu<>0", Program.LOT_ID, blood_vial_id), sqlConn))
                                        {
                                            object resultCount = null;
                                            resultCount = centralCount.ExecuteScalar();
                                            List<double> central_normal_glus = new List<double>();
                                            if ((resultCount == DBNull.Value)
                                                || (resultCount == null))
                                            {
                                                Trace.TraceWarning("Not valid count of central lot query:{0}", centralCount.CommandText);
                                            }
                                            if (Convert.ToInt32(resultCount) >= CENTRAL_STRIPCOUNT/*24*/) //Minimum 24 values
                                            {
                                                Trace.TraceInformation("Master Trimmean: {0}", master_sub_avg);

                                                #region Inserting accuracy central values
                                                using (NpgsqlCommand centralAVG = new NpgsqlCommand(
                                                    string.Format("select * FROM accuracy_test where accuracy_test.is_master=false and accuracy_test.blood_vial_id='{1}' and accuracy_test.lot_id='{0}' and accuracy_test.measured=false and accuracy_test.invalidate=false and accuracy_test.glu<>0", Program.LOT_ID, blood_vial_id), sqlConn))
                                                {

                                                    using (NpgsqlDataReader dr = centralAVG.ExecuteReader())
                                                    {
                                                        if (dr.HasRows)
                                                        {
                                                            while (dr.Read())
                                                            {
                                                                if (Convert.ToInt32(blood_vial_id) > 2)
                                                                {
                                                                    Trace.TraceInformation("Original glu: {0}", Convert.ToDouble(dr["glu"]));
                                                                    bias_glu.Add(((Convert.ToDouble(dr["glu"]) - master_sub_avg) * 100) / master_sub_avg);
                                                                    bias_glu_id.Add(Convert.ToInt32(dr["pk_id"]));
                                                                    Trace.TraceInformation("Normalized-bias-glu((glu-masterAVG)*100)/masterAVG: {0}", (((Convert.ToDouble(dr["glu"]) - master_sub_avg) * 100) / master_sub_avg));
                                                                    central_normal_glus.Add(Convert.ToDouble(dr["glu"]));
                                                                }
                                                                else
                                                                {
                                                                    if (Convert.ToDouble(dr["glu"]) != 0)
                                                                    {
                                                                        Trace.TraceInformation("Original glu: {0}", Convert.ToDouble(dr["glu"]));
                                                                        bias_glu.Add(18.02 * ((Convert.ToDouble(dr["glu"]) - master_sub_avg)));
                                                                        bias_glu_id.Add(Convert.ToInt32(dr["pk_id"]));
                                                                        Trace.TraceInformation("Normalized-bias-glu(18.02*(glu-masterAVG):{0}", (18.02 * (Convert.ToDouble(dr["glu"]) - master_sub_avg)));
                                                                        central_normal_glus.Add(Convert.ToDouble(dr["glu"]));
                                                                    }

                                                                }
                                                            }
                                                            dr.Close();
                                                        }
                                                        else
                                                        {
                                                            dr.Close();
                                                            Trace.TraceError("No result for central glus query: {0}", centralAVG.CommandText);

                                                        }

                                                    }//end of using NpgsqlDataReader      
                                                    List<double> central_sub_glus = new List<double>();

                                                    StoreBiasValue(bias_glu, bias_glu_id);

                                                    central_sub_glus = GetSubAvg(central_normal_glus);

                                                    central_avg = central_sub_glus.Average();
                                                    double stddev = Math.Sqrt(central_sub_glus.Average(v => Math.Pow(v - central_avg, 2)));
                                                    central_stddev = stddev;
                                                    central_cv = (central_stddev / central_avg) * 100;

                                                }//end of using


                                                Trace.TraceInformation("Central normal averages:{0}", central_avg);
                                                int fk_master_id = 0;
                                                using (NpgsqlCommand masterID = new NpgsqlCommand("select max(pk_id) from accuracy_result_master", sqlConn))
                                                {
                                                    object res = null;
                                                    res = masterID.ExecuteScalar();

                                                    if (res == null)
                                                    {
                                                        Trace.TraceError("Unsuccessful query to get accuracy_result_master max.pk_id query: {0}", masterID.CommandText);
                                                        throw new SqlNoValueException("Unsuccessful query to get accuracy_result_master max.pk_id");
                                                    }
                                                    else
                                                        fk_master_id = Convert.ToInt32(res);
                                                }
                                                if (Convert.ToInt32(blood_vial_id) <= 2)
                                                {
                                                    using (NpgsqlCommand command = new NpgsqlCommand(
                                                   string.Format("insert into accuracy_result_central(lot_id,avg,stddev,cv,fk_accuracy_result_master_id,blood_sample_id,normalized_bias_avg) values('{0}',{1:0.00},{2:0.00},{3:0.00},{4},'{5}',{6:0.00})", Program.LOT_ID, Math.Round(central_avg, 2), Math.Round(central_stddev, 2), Math.Round(central_cv, 2), fk_master_id, blood_vial_id, Math.Round(18.02 * (central_avg - master_sub_avg), 2)), sqlConn))
                                                    {
                                                        object res = null;
                                                        res = command.ExecuteNonQuery();

                                                        if (res == null)
                                                        {
                                                            Trace.TraceError("res is null at inserting to accuracy_result_central, query:{0}", command.CommandText);
                                                            throw new SqlNoValueException("res is null at storing accuracy_result_central");
                                                        }
                                                        else
                                                            Trace.TraceInformation("Successfull insert to accuracy_result_central");
                                                    }//insert into accuracy_result_central

                                                }
                                                else if (Convert.ToInt32(blood_vial_id) > 2)
                                                {
                                                    using (NpgsqlCommand command = new NpgsqlCommand(
                                                   string.Format("insert into accuracy_result_central(lot_id,avg,stddev,cv,fk_accuracy_result_master_id,blood_sample_id,normalized_bias_avg) values('{0}',{1:0.00},{2:0.00},{3:0.00},{4},'{5}',{6:0.00})", Program.LOT_ID, Math.Round(central_avg, 2), Math.Round(central_stddev, 2), Math.Round(central_cv, 2), fk_master_id, blood_vial_id, Math.Round(100 * (central_avg - master_sub_avg) / master_sub_avg, 2)), sqlConn))
                                                    {
                                                        object res = null;
                                                        res = command.ExecuteNonQuery();

                                                        if (res == null)
                                                        {
                                                            Trace.TraceError("res is null at inserting to accuracy_result_central, query:{0}", command.CommandText);
                                                            throw new SqlNoValueException("res is null at storing accuracy_result_central");
                                                        }
                                                        else
                                                            Trace.TraceInformation("Successfull insert to accuracy_result_central");
                                                    }//insert into accuracy_result_central

                                                }

                                            }//accuracy_result_central <= 24
                                            else
                                                Trace.TraceWarning("Central stripcount is less than {1}, query:{0}", centralCount.CommandText, CENTRAL_STRIPCOUNT);

                                                #endregion
                                        }//using centralCount
                                        #region Calculate and insert accuracy_result and Lot_result
                                        if (overall_accuracy_count >= ACCURACY_STRIPCOUNT * 6)      //when each vial is used 48*6 results will be
                                        {
                                            bool out_ok;
                                            CalculateOutLiers(Program.LOT_ID);
                                            if (accuracyOutPercent < 5)
                                            {
                                                out_ok = true;

                                            }
                                            else
                                                out_ok = false;
                                            int fk_id;
                                            using (NpgsqlCommand getFkCentralId = new NpgsqlCommand("select max(pk_id) from accuracy_result_central", sqlConn))
                                            {
                                                object res = null;
                                                res = getFkCentralId.ExecuteScalar();
                                                if (res == null)
                                                {
                                                    Trace.TraceError("Unsuccessful query to get accuracy_result_central latest pk_id: query: {0}", getFkCentralId.CommandText);
                                                    throw new SqlNoValueException(string.Format("Unsuccessfull query accuracy_result_central table, query: {0}", getFkCentralId.CommandText));
                                                }
                                                else
                                                    fk_id = Convert.ToInt32(res);
                                            }
                                            using (NpgsqlCommand insertToAccuracyResult = new NpgsqlCommand(
                                                string.Format("insert into accuracy_results(lot_id,master_lot_id,central_avg,central_stddev,central_cv,fk_accuracy_result_central_id,blood_vial_id) values('{0}','{1}',{2:0.00},{3:0.00},{4:0.00},{5},'{6}')", Program.LOT_ID, Program.master_lot_id, Math.Round(central_avg, 2), Math.Round(central_stddev, 2), Math.Round(central_cv, 2), fk_id, blood_vial_id), sqlConn))
                                            {
                                                object res = null;
                                                res = insertToAccuracyResult.ExecuteNonQuery();
                                                if ((res != DBNull.Value)
                                                    && (res != null))
                                                {
                                                    Trace.TraceInformation("Successful insert to accuracy result");
                                                }
                                                else
                                                    Trace.TraceError("Unsuccessfull insert to accuracy_results table, insert statement: {0}", insertToAccuracyResult.CommandText);
                                            }//insert into accuracy_results
                                            int fk_result_id = 0;
                                            Trace.TraceInformation("Next get accuracy_results maximum pk_id from accuracy_results");
                                            using (NpgsqlCommand getResultId = new NpgsqlCommand("select max(pk_id) from accuracy_results", sqlConn))
                                            {
                                                object res = null;
                                                res = getResultId.ExecuteScalar();

                                                if (res == null)
                                                {
                                                    Trace.TraceError("Unsuccessfull to get accuracy_results last inserted values, query: {0}", getResultId.CommandText);
                                                    throw new ArgumentException(string.Format("Unsuccessfull attempt to get accuracy_results pk_id"));
                                                }
                                                else
                                                    fk_result_id = Convert.ToInt32(res);
                                            }//get max(pkid) from accuracy_results                                                   
                                            CalculateAccuracyLotValues(Program.master_lot_id, Program.LOT_ID, central_avg, central_stddev, central_cv);
                                            bool lot_ok;
                                            if (out_ok && accuracy_lot_accuracy < 5)
                                            {
                                                lot_ok = true;
                                            }
                                            else
                                                lot_ok = false;

                                            using (NpgsqlCommand insertLotValues = new NpgsqlCommand(
                                                string.Format("insert into accuracy_lot_result(lot_id,fk_accuracy_results_id,lot_accuracy,master_lot_id,outliers_count,outliers_percent,outliers_ok,lot_is_ok,invalidate) values('{0}',{1},{2:0.00},'{3}',{4},{5:0.00},{6},{7},{8})", Program.LOT_ID, fk_result_id, Math.Round(accuracy_lot_accuracy, 2), Program.master_lot_id, accuracyOutCount, Math.Round(accuracyOutPercent, 2), out_ok, lot_ok, false), sqlConn))
                                            {
                                                object res = null;
                                                res = insertLotValues.ExecuteNonQuery();

                                                if (res == null)
                                                {
                                                    Trace.TraceError("Unsuccessfull insert to accuracy_lot_result, query: {0}", insertLotValues.CommandText);
                                                    throw new SqlNoValueException("Unsuccessful insert to accuracy_lot_result table");
                                                }
                                                else
                                                    Trace.TraceInformation("Successfull insert to accuracy_lot_result table");
                                                Trace.TraceInformation("Accuracy Finished Result View will be displayed");
                                                SetMeasuredCalculatedValues(Program.LOT_ID);
                                                CreateAccuracyChild();
                                                SetButtonEnabled();
                                            }//insert into accuracy_lot_result 

                                        }//if overal_accuracy_count>=48*6
                                        #endregion
                                    }//if master count >= 24
                                    else
                                        Trace.TraceWarning("Masters count is less than {1}, query: {0}", masterCount.CommandText, MASTER_STRIPCOUNT);
                                        #endregion

                                }//End of using masterCount
                            }// if overal_accuracy_count>=48 
                            else
                                Trace.TraceWarning("Overall accuracy measured count is less than 48: accuracyCount:{0}", overall_accuracy_count);
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError("Exception in inserting accuracy results: stackTrace: {0}\n\n Message: {1}", ex.StackTrace, ex.Message);
                            throw new ArgumentNullException(string.Format("Exception in accuracy storing values,exception:{0}", ex.InnerException));
                        }
        #endregion
                    }//if Program.measureType=="accuracy"

                }
                catch (Exception ex)
                {
                    Trace.TraceError(string.Format("Exception in StoreData:{0}\n{1} ", ex.Message, ex.InnerException));
                    sqlConn.Close();
                }
                finally
                {
                    Trace.TraceWarning("Finnaly block at StoreData() method in MessgaeCompletedEventArgs class");
                    if (Properties.Settings.Default.CONTINOUSLY_FILL_BARCODES)
                    {
                        #region Continous barcode filling for accuracy
                        if (Properties.Settings.Default.AUTO_CLOSE_WINDOW)
                        {


                            if (Program.measureType == "accuracy")
                            {
                                Trace.TraceInformation("Program measuretype is accuracy");


                                if (Program.InOneRoundAccuracyStripCount >= 16)
                                {
                                    Program.InOneRoundAccuracyStripCount = 0;
                                    Trace.TraceInformation("Program.InOneRoundAccuracyStripCount is greater than 16, window is closed");
                                    Program.CloseWindow();
                                }
                            }
                        }


                        #endregion

                        #region Continously Barcode Filling for Blank



                        if ((Program.BlankMeasuredStripCount >= 16)
                                                                 && (Program.measureType == "blank"))
                        {

                            Trace.TraceWarning("Continously Barcode Filling for Blank");
                            Trace.TraceInformation("BlankMeasuredStripCount is greater than PortCount({1}), BlankMeasuredStripCount: {0}", Program.BlankMeasuredStripCount, Program.PortCount);
                            Trace.TraceInformation("Get the barcodes from the last panel");

                            foreach (Control item in UserWindow.userTables.Controls)
                            {
                                if (item is UserPanel)
                                {
                                    Trace.TraceInformation("item name is {0}", item.Name);
                                    if (item.Name.Equals("15")
                                        || item.Name.Equals("COM16"))
                                    {
                                        Trace.TraceInformation("item.name equals COM16 or 15");
                                        foreach (Control tb in item.Controls)
                                        {
                                            if (tb is TextBox)
                                            {
                                                Trace.TraceInformation("item is Textbox: {0}", tb);
                                                if (tb.Name.Equals("tbBarcode2"))
                                                {
                                                    Trace.TraceInformation("Textbox name is tbBarcode2");
                                                    lastsn = tb.Text.Split(' ')[0];
                                                    lastroll = tb.Text.Split(' ')[1];

                                                    Trace.TraceInformation("SN in Last textbox is {0}", lastsn);
                                                    Trace.TraceInformation("Roll in Last textbox is {0}", lastroll);
                                                }
                                            }
                                        }
                                    }
                                    ((UserPanel)item).dev.LOT_ID = Program.LOT_ID;
                                }
                            }
                            Trace.TraceInformation("Fill the first panel barcodes");

                            tube_sn = Convert.ToInt32(lastsn);
                            foreach (Control item in UserWindow.userTables.Controls)
                            {
                                if (item is UserPanel)
                                {
                                    Trace.TraceInformation("Item is UserPanel {0}", item);
                                    Trace.TraceInformation("Item name is {0}", item.Name);

                                    if (item.Name.Equals("0")
                                        || item.Name.Equals("COM1"))
                                    {
                                        Trace.TraceInformation("item.name equals COM1 or 0 {0}", item.Name);
                                        foreach (Control tb in item.Controls)
                                        {
                                            if (tb is TextBox)
                                            {

                                                if (tb.Name.Equals("tbBarcode2"))
                                                {
                                                    Trace.TraceInformation("Textbox name is {0}", tb.Name);
                                                    Trace.TraceInformation("Textbox text before change: {0}", tb.Text);
                                                    Trace.TraceInformation("last SN: {0}", lastsn);
                                                    Trace.TraceInformation("last Roll: {0}", lastroll);


                                                    Trace.TraceWarning("SetBarCode2TextTo() at StoreData() in MesssageCompletedEventArgs class");
                                                    (item as UserPanel).SetBarCode2TextTo(string.Format("{0} {1}", Convert.ToInt32(lastsn) + 2, Convert.ToInt32(lastroll)));
                                                    //    tb.Text = string.Format("{0} {1}", Convert.ToInt32(lastsn) + 2, Convert.ToInt32(lastroll));
                                                    //tb.Enabled = false;
                                                    Trace.TraceInformation("Textbox text after change: {0}", tb.Text);

                                                    ((UserPanel)item).dev.Roll = lastroll;
                                                    ((UserPanel)item).dev.SerialNumber = Convert.ToString(Convert.ToInt32(lastsn) + 1);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            Trace.TraceInformation("First panel SN and roll is replaced SN:{0}, Roll:{1}", lastsn + 2, lastroll);

                            #region Fill the sn and roll code to all panel


                            Trace.TraceInformation("Fill more panel code except panel one");

                            int count = 0;
                            tube_sn = Convert.ToInt32(lastsn);
                            tube_sn = tube_sn + 2;

                            foreach (Control panel2 in UserWindow.userTables.Controls)
                            {
                                if (panel2 is UserPanel)
                                {
                                    #region Fill 2nd Panel SN_Roll Barcode
                                    Trace.TraceInformation("C is UserPanel, Name: {0}", panel2.Name);
                                    #region Fill second panel SN and Roll barcode
                                    if (((UserPanel)panel2).Name.Equals("1")
                                                                       || ((UserPanel)panel2).Name.Equals("COM2"))
                                    {
                                        Trace.TraceInformation("Name of the panel is COM2 or 1, Name: {0}", panel2.Name);

                                        foreach (Control txtb in panel2.Controls)
                                        {
                                            if ((txtb is TextBox)
                                                && (txtb.Name.Equals("tbBarcode2")))
                                            {
                                                Trace.TraceWarning("SetBarCode2TextTo() at StoreData() in MesssageCompletedEventArgs class");
                                                (panel2 as UserPanel).SetBarCode2TextTo(string.Format("{0} {1}", tube_sn, lastroll));
                                                // ((TextBox)txtb).Enabled = false;
                                                ((UserPanel)panel2).dev.Roll = Program.Roll_ID;
                                                ((UserPanel)panel2).dev.SerialNumber = Convert.ToString(tube_sn);
                                                count++;
                                            }
                                        }
                                        Trace.TraceInformation("2nd Panel SN_ROLL code filled");
                                    }

                                    #endregion

                                    #endregion
                                }
                            }
                            tube_sn = tube_sn + 2;
                            count = 0;


                            #region Fill the other panels sn and roll code


                            foreach (Control c in UserWindow.userTables.Controls)
                            {
                                if (c is UserPanel)
                                {
                                    if (!((UserPanel)c).Name.Equals("0")
                                        && !((UserPanel)c).Name.Equals("COM1")
                                        && !((UserPanel)c).Name.Equals("1")
                                        && !((UserPanel)c).Name.Equals("COM2"))
                                    {
                                        Trace.TraceInformation("Name of the panel is not COM1,COM2 or 0 or 1, Name: {0}", c.Name);

                                        foreach (Control txtb in c.Controls)
                                        {
                                            Trace.TraceInformation("Count is {0}", count);
                                            if ((txtb is TextBox)
                                                && (txtb.Name.Equals("tbBarcode2")))
                                            {

                                                Trace.TraceInformation("Name of the textbox, Name: {0}", txtb.Name);
                                                if (count > 1)
                                                {
                                                    tube_sn = tube_sn + 2;
                                                    count = 0;
                                                }
                                                Trace.TraceWarning("SetBarCode2TextTo() at StoreData() in MesssageCompletedEventArgs class");
                                                (c as UserPanel).SetBarCode2TextTo(string.Format("{0} {1}", tube_sn, lastroll));
                                                //((TextBox)txtb).Enabled = false;
                                                ((UserPanel)c).dev.Roll = Program.Roll_ID;
                                                ((UserPanel)c).dev.SerialNumber = Convert.ToString(tube_sn);
                                                count++;
                                            }
                                        }
                                    }
                                }
                                else
                                    Trace.TraceInformation("Name of the panel is COM1, Name: {0}", c.Name);

                            }
                            #endregion
                            #endregion
                            #region Prepare for next measurement
                            Program.BlankMeasuredStripCount = 0;
                            Trace.TraceInformation("\n\nPrepare for the next measurement\n\n");
                            Trace.TraceWarning("Changing images on button1");
                            foreach (Control item in UserWindow.userTables.Controls)
                            {
                                if (item is UserPanel)
                                {
                                    foreach (Control bt in item.Controls)
                                    {
                                        if (bt is Button
                                                     && bt.Name == "button1")
                                        {
                                            Image image1 = Properties.Resources._801;
                                            ((Button)bt).Image = image1;
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        #endregion

                        #region Continously Barcode Filling for Accuracy
                        if ((Program.storedGlus >= 16)
                            && (Program.measureType == "accuracy"))
                        {

                            Trace.TraceWarning("Continously Barcode Filling for Accuracy");
                            Trace.TraceInformation("BlankMeasuredStripCount is greater than PortCount({1}), BlankMeasuredStripCount: {0}", Program.BlankMeasuredStripCount, Program.PortCount);
                            Trace.TraceInformation("Get the barcodes from the last panel");

                            SetButtonEnabled();
                        }
                        #endregion


                        #region Continously Barcode Filling for Homogenity
                        if ((Program.HomogenityMeasuredStripCount >= 16)
                            && Program.measureType == "homogenity")
                        {
                            Trace.TraceInformation("Beginnig at autofillbarcodes HomogenityMeasuredStripCount is {0}", Program.HomogenityMeasuredStripCount);
                            foreach (Control item in UserWindow.userTables.Controls)
                            {
                                if (item is UserPanel)
                                {
                                    if (item.Name.Equals("COM16")
                                        || item.Name.Equals("15"))
                                    {
                                        Trace.TraceInformation("Panel is COM 16 or 15, Name:{0}", item.Name);

                                        foreach (Control tb in item.Controls)
                                        {
                                            if (tb is TextBox)
                                            {
                                                if (tb.Name.Equals("tbBarcode2"))
                                                {
                                                    lastsn = tb.Text.Split(' ')[0];
                                                    lastroll = tb.Text.Split(' ')[1];
                                                }
                                            }

                                        }
                                    }
                                    ((UserPanel)item).dev.LOT_ID = Program.LOT_ID;
                                    Trace.TraceInformation("Last SN is {0}, Last Roll is {1}", lastsn, lastroll);
                                }

                            }
                            #region Fill 1st rows panel barcode
                            tube_sn = Convert.ToInt32(lastsn);
                            tube_sn = tube_sn + 1;
                            Trace.TraceInformation("TubeSN at 1st row: {0}", tube_sn);
                            foreach (Control item in UserWindow.userTables.Controls)
                            {
                                if (item is UserPanel)
                                {
                                    if (item.Name.Equals("COM1")
                                        || item.Name.Equals("0")
                                        || item.Name.Equals("COM2")
                                        || item.Name.Equals("1")
                                        || item.Name.Equals("COM3")
                                        || item.Name.Equals("2")
                                        || item.Name.Equals("COM4")
                                        || item.Name.Equals("3"))
                                    {
                                        foreach (Control tb in item.Controls)
                                        {
                                            if (tb is TextBox)
                                            {
                                                if (tb.Name.Equals("tbBarcode2"))
                                                {
                                                    (item as UserPanel).SetBarCode2TextTo(string.Format("{0} {1}", Convert.ToInt32(tube_sn), Convert.ToInt32(lastroll)));
                                                    //tb.Enabled = false;
                                                    ((UserPanel)item).dev.Roll = lastroll;
                                                    ((UserPanel)item).dev.SerialNumber = Convert.ToString(Convert.ToInt32(tube_sn) + 1);
                                                }
                                            }
                                        }
                                    }

                                }

                            }

                            #region Fill the sn and roll code to all panel
                            Trace.TraceInformation("Fill more panel code except panel one");
                            int count = 0;

                            tube_sn = tube_sn + 1;
                            Trace.TraceInformation("TubeSN at 2nd row: {0}", tube_sn);
                            foreach (Control panel2 in UserWindow.userTables.Controls)
                            {
                                if (panel2 is UserPanel)
                                {
                                    #region Fill 2nd Panel SN_Roll Barcode
                                    Trace.TraceInformation("C is UserPanel, Name: {0}", panel2.Name);
                                    #region Fill second panel SN and Roll barcode
                                    if (((UserPanel)panel2).Name.Equals("4")
                                        || ((UserPanel)panel2).Name.Equals("COM5")
                                        || ((UserPanel)panel2).Name.Equals("5")
                                        || ((UserPanel)panel2).Name.Equals("COM6")
                                        || ((UserPanel)panel2).Name.Equals("6")
                                        || ((UserPanel)panel2).Name.Equals("COM7")
                                        || ((UserPanel)panel2).Name.Equals("7")
                                        || ((UserPanel)panel2).Name.Equals("COM8"))
                                    {
                                        Trace.TraceInformation("Name of the panel is COM5 or 4, Name: {0}", panel2.Name);

                                        foreach (Control txtb in panel2.Controls)
                                        {
                                            if ((txtb is TextBox)
                                                && (txtb.Name.Equals("tbBarcode2")))
                                            {
                                                (panel2 as UserPanel).SetBarCode2TextTo(string.Format("{0} {1}", tube_sn, lastroll));

                                                ((UserPanel)panel2).dev.Roll = Program.Roll_ID;
                                                ((UserPanel)panel2).dev.SerialNumber = Convert.ToString(tube_sn);
                                                count++;
                                            }
                                        }
                                        Trace.TraceInformation("2nd row SN_ROLL code filled");
                                    }

                                    #endregion

                                    #endregion
                                }
                            }
                            count = 0;
                            tube_sn = tube_sn + 1;
                            Trace.TraceInformation("TubeSN at 3rd row: {0}", tube_sn);
                            #region Fill the other panels sn and roll code
                            foreach (Control c in UserWindow.userTables.Controls)
                            {
                                if (c is UserPanel)
                                {
                                    if (((UserPanel)c).Name.Equals("8")
                                        || ((UserPanel)c).Name.Equals("COM9")
                                        || ((UserPanel)c).Name.Equals("9")
                                        || ((UserPanel)c).Name.Equals("COM10")
                                        || ((UserPanel)c).Name.Equals("10")
                                        || ((UserPanel)c).Name.Equals("COM11")
                                        || ((UserPanel)c).Name.Equals("11")
                                        || ((UserPanel)c).Name.Equals("COM12"))
                                    {
                                        Trace.TraceInformation("Name of the panel is COM5 or 4, Name: {0}", c.Name);

                                        foreach (Control txtb in c.Controls)
                                        {
                                            if ((txtb is TextBox)
                                                && (txtb.Name.Equals("tbBarcode2")))
                                            {
                                                (c as UserPanel).SetBarCode2TextTo(string.Format("{0} {1}", tube_sn, lastroll));
                                                // ((TextBox)txtb).Enabled = false;
                                                ((UserPanel)c).dev.Roll = Program.Roll_ID;
                                                ((UserPanel)c).dev.SerialNumber = Convert.ToString(tube_sn);
                                                count++;
                                            }
                                        }
                                        Trace.TraceInformation("3rd row SN_ROLL code filled");
                                    }
                                }
                                else
                                    Trace.TraceInformation("Name of the panel is COM1, Name: {0}", c.Name);
                            }
                            tube_sn = tube_sn + 1;
                            Trace.TraceInformation("TubeSN at 4th row: {0}", tube_sn);
                            count = 0;
                            foreach (Control panel2 in UserWindow.userTables.Controls)
                            {
                                if (panel2 is UserPanel)
                                {
                                    #region Fill 2nd Panel SN_Roll Barcode
                                    Trace.TraceInformation("C is UserPanel, Name: {0}", panel2.Name);
                                    #region Fill second panel SN and Roll barcode
                                    if (((UserPanel)panel2).Name.Equals("12")
                                        || ((UserPanel)panel2).Name.Equals("COM13")
                                        || ((UserPanel)panel2).Name.Equals("13")
                                        || ((UserPanel)panel2).Name.Equals("COM14")
                                        || ((UserPanel)panel2).Name.Equals("14")
                                        || ((UserPanel)panel2).Name.Equals("COM15")
                                        || ((UserPanel)panel2).Name.Equals("15")
                                        || ((UserPanel)panel2).Name.Equals("COM16"))
                                    {
                                        Trace.TraceInformation("Name of the panel is COM5 or 4, Name: {0}", panel2.Name);

                                        foreach (Control txtb in panel2.Controls)
                                        {
                                            if ((txtb is TextBox)
                                                && (txtb.Name.Equals("tbBarcode2")))
                                            {
                                                (panel2 as UserPanel).SetBarCode2TextTo(string.Format("{0} {1}", tube_sn, lastroll));
                                                //  ((TextBox)txtb).Enabled = false;
                                                ((UserPanel)panel2).dev.Roll = Program.Roll_ID;
                                                ((UserPanel)panel2).dev.SerialNumber = Convert.ToString(tube_sn);
                                                count++;
                                            }
                                        }

                                    }

                                    #endregion

                                    #endregion
                                }
                            }
                            Trace.TraceInformation("4th row Panel SN_ROLL code filled");
                            #endregion
                            #endregion
                            #endregion
                            #region Prepare for next measurement
                            Program.HomogenityMeasuredStripCount = 0;
                            Trace.TraceInformation("HomogenityMEasured StripCount: {0}", Program.HomogenityMeasuredStripCount);

                            foreach (Control item in UserWindow.userTables.Controls)
                            {
                                Trace.TraceWarning("Changing image on button1");
                                if (item is UserPanel)
                                {
                                    foreach (Control bt in item.Controls)
                                    {
                                        if (bt is Button
                                            && bt.Name == "button1")
                                        {
                                            Image image1 = Properties.Resources._801;
                                            ((Button)bt).Image = image1;
                                        }
                                    }
                                }
                            }



                            #endregion
                        }
                        #endregion

                    }
                    Trace.TraceWarning("Finnaly block at StoreData() method - AFTER  aUTO BARCODE FILLING - in MessgaeCompletedEventArgs class");

                    CheckError.DeviceReplace = false;
                    CheckError.EarlyDripp = false;
                    CheckError.H62 = false;
                    CheckError.NOT_H62 = false;
                    Users_Form.dev.Error_H_Text = string.Empty;
                    ThePanel.TheDevice.EarlyDribble = false;
                    Users_Form.dev.Not_H62_error_happened = false;
                    Users_Form.dev.H62_error_happened = false;
                    Users_Form.dev.EarlyDribble = false;
                    ThePanel.TheDevice.startTimeStored = false;
                    Users_Form.dev.BarCode = string.Empty;

                    ClearAllData();
                    sqlConn.Close();
                }
            }//using (NpgsqlConnection sqlConn)
        }

        /// <summary>
        /// Set measured values to measured=True
        /// </summary>
        /// <param name="lot_id"></param>
        private void SetMeasuredCalculatedValues(string lot_id)
        {
            using (NpgsqlConnection setMeasuredConn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    setMeasuredConn.Open();

                    using (NpgsqlCommand measComm = new NpgsqlCommand(string.Format("update accuracy_test set measured=true where (lot_id='{0}' or lot_id='{1}')", lot_id, Program.master_lot_id), setMeasuredConn))
                    {
                        object res = null;

                        res = measComm.ExecuteNonQuery();

                        if (res == null)
                        {
                            Trace.TraceError("Unsuccessful update to set measured calculated values, query: {0}", measComm.CommandText);
                        }

                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    setMeasuredConn.Close();
                }

            }
        }

        public int accuracyOutCount = 0;
        public double accuracyOutPercent;

        private void CalculateOutLiers(string lotid)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();

                    using (NpgsqlCommand outLiers = new NpgsqlCommand(string.Format("SELECT COUNT(accuracy_central_bias.strip_ok) FROM accuracy_central_bias LEFT JOIN accuracy_test ON accuracy_central_bias.fk_accuracy_test_id = accuracy_test.pk_id where accuracy_test.lot_id='{0}' and accuracy_test.invalidate=false and accuracy_central_bias.strip_ok=false and accuracy_test.glu<>0 and accuracy_test.measured=False", lotid), conn))
                    {
                        object res = null;

                        res = outLiers.ExecuteScalar();

                        if (res == null)
                        {
                            Trace.TraceError("Unsuccessful query: {0}", outLiers.CommandText);
                        }
                        else
                            accuracyOutCount = Convert.ToInt32(res);

                    }
                }
                catch (Exception)
                {

                    throw new Exception("Exception while calculating outliers");
                }
                finally
                {
                    conn.Close();
                }

            }
            Trace.TraceInformation("Accuracy outliers count: {0}", accuracyOutCount);
            double diff = 144 - accuracyOutCount;
            accuracyOutPercent = 100 - ((diff / 144) * 100);
            Trace.TraceInformation("Accuracy outliers percent(100 - (((144 - accuracyOutCount) / 144) * 100)): {0}", accuracyOutPercent);

        }

        private List<double> GetNormalGluValuesOfCentral()
        {
            throw new NotImplementedException();
        }
        private void StoreBiasValue(List<double> bias_glu, List<int> bias_glu_id)
        {
            using (NpgsqlConnection biasConn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    biasConn.Open();
                    bool strip_ok = false;
                    for (int i = 0; i < bias_glu.Count; i++)
                    {
                        if (Math.Abs(bias_glu[i]) > 15)
                        {
                            strip_ok = false;
                        }
                        else
                            if (bias_glu[i] != 0)
                            {

                                strip_ok = true;
                            }


                        using (NpgsqlCommand getGlus = new NpgsqlCommand(string.Format("insert into accuracy_central_bias(bias_glu,fk_accuracy_test_id,strip_ok) values({0},{1},{2})", bias_glu[i], bias_glu_id[i], strip_ok), biasConn))
                        {
                            object res = null;

                            res = getGlus.ExecuteNonQuery();

                            if (res == null)
                            {
                                Trace.TraceError("Unsuccessfull insert of bias_glu values, query: {0}", getGlus.CommandText);
                            }

                        }
                    }

                }
                catch (Exception)
                {

                    throw;

                }
                finally
                {
                    biasConn.Close();
                }
            }
        }
        public AccuracyFinishedResultView _accuracyChild;

        /// <summary>
        /// Create a results view for accuracy
        /// </summary>
        private void CreateAccuracyChild()
        {
            if (Properties.Settings.Default.CommitChangesNeeded)
            {
                if (TestReleaseHostConnection())
                    CommitChangesToReleaseHost(Program.measureType);

                //Drop schema then create a new
                DropAndCreateSchema();
                //Restore schema from sql file
                SchemaRestore();
            }


            _accuracyChild = new AccuracyFinishedResultView(Program.LOT_ID, Program.master_lot_id);  //!!!
            _accuracyChild.ShowDialog();
        }

        public List<int> skippedIndexes = new List<int>();
        public List<double> sub_glus = new List<double>();

        /// <summary>
        /// Calculate Sub-AVG of master lot's calibrated glu
        /// </summary>
        /// <param name="act_glus"></param>
        /// <returns></returns>
        private List<double> GetSubAvg(List<double> act_glus)
        {
            #region Skip 3 of the lowest values
            Trace.TraceInformation("Skipping the 3 lowest values");
            for (int j = 0; j < 3; j++)         //skip 3 less value
            {
                double min = act_glus[0];
                int minIndex = 0;

                for (int i = 1; i < act_glus.Count; ++i)
                {
                    if (act_glus[i] < min)
                    {
                        min = act_glus[i];
                        minIndex = i;
                    }
                }
                Trace.TraceInformation("The minimum is position:{0} value:{1}", minIndex, act_glus[minIndex]);
                skippedIndexes.Add(minIndex);
                act_glus.RemoveAt(minIndex);

            }//End of for statement 
            #endregion
            Trace.TraceInformation("Skipping the 3 greatest value");
            #region Skip the 3 greatest value
            for (int j = 0; j < 3; j++)     //skip 3 greatest
            {
                double max = act_glus[0];
                int maxIndex = 0;

                for (int i = 1; i < act_glus.Count; ++i)
                {
                    if (act_glus[i] > max)
                    {
                        max = act_glus[i];
                        maxIndex = i;
                    }
                }
                Trace.TraceInformation("The maximum is position:{0} value:{1}", maxIndex, act_glus[maxIndex]);
                skippedIndexes.Add(maxIndex);
                act_glus.RemoveAt(maxIndex);

            }

            #endregion
            sub_glus = act_glus;

            return act_glus;


        }
        void RemoveLines(TextBoxBase textBox, int maxLine)
        {
            int plusLines = textBox.Lines.Length - maxLine;
            if (plusLines > 0)
            {
                int start_index = textBox.GetFirstCharIndexFromLine(0);
                int count = textBox.GetFirstCharIndexFromLine(plusLines) - start_index;
                textBox.Text = textBox.Text.Remove(start_index, count);
            }
        }
        /// <summary>
        /// Calculate results for a lot in accuracy test
        /// accuracy_lot_avg,accuracy_lot_sd,accuracy_lot_cv,accuracy_lot_accuracy
        /// </summary>
        /// <param name="master_lot_id"></param>
        /// <param name="lot_id"></param>
        private void CalculateAccuracyLotValues(string master_lot_id, string lot_id, double centralAverages, double centralStddev, double centralCv)
        {
            List<double> central_bias_avg = new List<double>();
            List<int> central_ids = new List<int>();
            double lot_central_avg;
            double lot_central_avg_sd;
            Trace.TraceInformation("Calculate accuracy Lot values method started:MessageCompletedEvenArgs.CalculateAccuracyLotValues()");
            using (NpgsqlConnection accLotconn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {

                    accLotconn.Open();
                    for (int i = 1; i <= 6; i++)
                    {
                        using (NpgsqlCommand getID = new NpgsqlCommand(string.Format("select max(pk_id) as max from accuracy_result_central where lot_id='{0}' and blood_sample_id='{1}'", lot_id, Convert.ToString(i)), accLotconn))
                        {
                            object res = null;

                            res = getID.ExecuteScalar();

                            if ((res == null)
                                || (res == DBNull.Value))
                            {
                                Trace.TraceWarning("Calculating lot_accuracy: query: {0}", getID.CommandText);
                            }
                            else
                                central_ids.Add(Convert.ToInt32(res));
                        }

                    }
                    foreach (int ID in central_ids)
                    {


                        using (NpgsqlCommand calcLotAccuracy = new NpgsqlCommand(
                            string.Format("select normalized_bias_avg from accuracy_result_central where pk_id={0}", ID), accLotconn))
                        {

                            using (NpgsqlDataReader dr = calcLotAccuracy.ExecuteReader())
                            {

                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        central_bias_avg.Add(Convert.ToDouble(dr["normalized_bias_avg"]));
                                    }
                                    dr.Close();
                                }
                                else
                                {
                                    Trace.TraceWarning("No value for this query at Lot_accuracy calulating:{0}", calcLotAccuracy.CommandText);
                                    dr.Close();
                                }

                            }//end of using (NpgsqlDataReader)
                        }
                        if (central_bias_avg.Count < 6)
                        {
                            Trace.TraceError("central_bias_avg.Count is less than 6, count: {0}", central_bias_avg.Count);
                        }
                        else
                            Trace.TraceInformation("central_bias_avg.Count is ok, count: {0}", central_bias_avg.Count);

                        lot_central_avg = central_bias_avg.Average();

                        Trace.TraceInformation("Calculated lot central avg:{0}", lot_central_avg);

                        double avg = central_bias_avg.Average();
                        double stddev = Math.Sqrt(central_bias_avg.Average(v => Math.Pow(v - lot_central_avg, 2)));
                        lot_central_avg_sd = stddev;

                        Trace.TraceInformation("LOT central averages standardDeviation:{0}", lot_central_avg_sd);

                        ///Calculating lot_accuracy
                        Trace.TraceInformation("Calculating LOT accuracy");
                        accuracy_lot_accuracy = Math.Abs(lot_central_avg) + (2.015 * lot_central_avg_sd) / Math.Sqrt(6);
                        Trace.TraceInformation("Calculated lot_accuracy:{0}", accuracy_lot_accuracy);
                    }//End of using
                }
                catch (Exception ex)
                {

                    Trace.TraceError("Exception calculating LOT(accuracy) values:{0}", ex.StackTrace);
                }
                finally
                {
                    accLotconn.Close();
                }
            }//End of NpgsqlConnection
        }

        /// <summary>
        /// Store Homogenity test alternation
        /// </summary>
        /// <param name="avg"></param>
        /// <param name="stddev"></param>
        /// <param name="cv"></param>
        /// <param name="not_h62_error_count"></param>
        /// <param name="h62_error_count"></param>
        /// <param name="connection_in"></param>
        public void SaveHomogenityAlternation(object avg, object stddev, object cv, object not_h62_error_count,
           object h62_error_count, NpgsqlConnection connection_in)
        {
            object result = null;

            Trace.TraceInformation("SaveHomogenityResult is started");

            if (Users_Form.dev.Homogenity_Is_Valid)
            {
                out_of_range_valid = true;
                not_h62_is_valid = true;
            }
            try
            {
                using (NpgsqlCommand storeHomogenity = new NpgsqlCommand(
                    string.Format("insert into homogenity_result_alternation(lot_id,roll_id,fk_blank_test_identify_id,avg,stddev,cv,not_h62_errors_count,h62_errors_count,date,out_of_range_strip_count,strip_count,remeasured,invalidate) values('{0}','{1}',{2},{3:0.00},{4:0.000},{5:0.00},{6},{7},{8},{9},{10},{11},{12})",
                    Users_Form.dev.LOT_ID, Users_Form.dev.Roll, Users_Form.dev.Averages_ID, Math.Round(Convert.ToDouble(avg), 2), Math.Round(Convert.ToDouble(stddev), 3), Math.Round(Convert.ToDouble(cv), 2),
                     not_h62_error_count, h62_error_count, "@date", Convert.ToInt32(Program.OutOfRangeCount_Homo),
                    Users_Form.dev.stripCount, false, false), connection_in))
                {
                    storeHomogenity.Parameters.AddWithValue("@date", DateTime.Now);
                    result = storeHomogenity.ExecuteNonQuery();

                    if (Convert.ToInt32(result) == 0)
                    {
                        Trace.TraceError("Unsuccessfull insert in SaveHomogenityAlternation(), query: {0}", storeHomogenity.CommandText);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Exception in SaveHomogenityAlternation: ex{0}", ex.StackTrace);
            }
        }

        /// <summary>
        /// Store blank test alternation
        /// </summary>
        /// <param name="roll_id"></param>
        private void StoreAlternation(string roll_id)
        {
            #region Variables
            bool valid = false;
            object avg = null;
            object identify_pk = null;
            object not_h62_error_count;
            object stddev = null;
            double cv;
            string query = string.Empty;
            string stddev_query = string.Empty;
            #endregion

            Color clr;
            Trace.TraceInformation(string.Format("Insert averages:GetAVGofARoll() roll_id:{0}", roll_id));
            query = string.Format("Select AVG(blank_test_result.nano_amper) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id where blank_test_identify.roll_id='{0}' and blank_test_identify.lot_id='{1}' and blank_test_result.glu<>0 and temperature_ok=true and blank_test_identify.invalidate=false ", roll_id, Program.LOT_ID);

            using (NpgsqlConnection connection = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        #region GetAVGforBlank
                        avg = command.ExecuteScalar();

                        if ((avg == DBNull.Value)
                            || (avg == null))
                        {

                            Trace.TraceError(string.Format("Avg is null, query:{0}", command.CommandText));
                            throw new ArgumentNullException("Avg is null");
                        }
                        if ((Convert.ToDouble(avg) < 13)
                            || (Convert.ToDouble(avg) > 51))
                        {
                            valid = false;
                            Trace.TraceWarning("Averages is less than 13 or greater than 51, averages:{0}", Convert.ToDouble(avg));

                        }
                        else
                        {
                            valid = true;
                            Trace.TraceInformation(string.Format("Average is ok, average:{0}", Convert.ToDouble(avg)));
                        }
                        #endregion
                        Trace.TraceInformation("Calculate stddev and cv for blank test");
                        #region get stddev and cv for blank
                        using (NpgsqlCommand stddev_comm = new NpgsqlCommand(string.Format("Select STDDEV(blank_test_result.nano_amper) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id where blank_test_identify.roll_id='{0}' and blank_test_identify.lot_id='{1}' and blank_test_result.glu<>0 and temperature_ok=true and blank_test_identify.invalidate=false", roll_id, Users_Form.dev.LOT_ID), connection))
                        {
                            stddev = stddev_comm.ExecuteScalar();

                            if ((stddev == DBNull.Value)
                                || (stddev == null))
                            {
                                Trace.TraceError("stddev is null, query: {0}", stddev_comm.CommandText);

                            }
                        }

                        cv = (Convert.ToDouble(stddev) / Convert.ToDouble(avg)) * 100;
                        #endregion

                        using (NpgsqlCommand error_count = new NpgsqlCommand(
                            string.Format("SELECT count(blank_test_errors.not_h62_error) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_identify.measure_type='homogenity' and blank_test_identify.lot_id='{0}' and blank_test_identify.roll_id='{1}' and blank_test_errors.not_h62_error=True and blank_test_identify.remeasured=false and blank_test_identify.invalidate=false",
                            Program.LOT_ID, Users_Form.dev.Roll), connection))
                        {
                            not_h62_error_count = error_count.ExecuteScalar();

                            if ((not_h62_error_count == null)
                                || (not_h62_error_count == DBNull.Value))
                            {
                                Trace.TraceError("error_count is null, query: {0}", error_count.CommandText);
                                throw new ArgumentNullException("error_count is null");
                            }
                            Trace.TraceInformation("error_count is null, query: {0}", error_count.CommandText);
                        }
                        using (NpgsqlCommand comm = new NpgsqlCommand(
                            string.Format("select MAX(pk_id) from  blank_test_identify where roll_id='{0}' and lot_id='{1}' and remeasured=false and blank_test_identify.invalidate=false",
                            roll_id, Program.LOT_ID), connection))
                        {
                            identify_pk = comm.ExecuteScalar();

                            if ((identify_pk == null)
                                || (identify_pk == DBNull.Value))
                            {
                                Trace.TraceError("No pk_id in blank_test_identify for roll_id:{0} query: {1}", roll_id, comm.CommandText);
                                throw new ArgumentNullException("identify_pk is null");
                            }
                            Trace.TraceInformation("No pk_id in blank_test_identify for roll_id:{0} query: {1}", roll_id, comm.CommandText);

                            #region insert to blank_avg_alternation
                            using (NpgsqlCommand command_insert = new NpgsqlCommand(
                                         string.Format("insert into blank_test_averages_alternation(avg,roll_id,fk_blank_test_identify_id,tube_count_in_one_roll,stddev,cv,date,lot_id,remeasured,invalidate) values({0:0.00},'{1}',{2},{3},{4:0.000},{5:0.000},{6},'{7}',{8},{9})",
                                         Convert.ToDouble(avg), roll_id, Convert.ToInt32(identify_pk), Users_Form.dev.lot_count_in_one_roll, Convert.ToDouble(stddev), cv, "@date", Users_Form.dev.LOT_ID, false, false), connection))
                            {
                                object res = null;
                                command_insert.Parameters.AddWithValue("@date", DateTime.Now);
                                res = command_insert.ExecuteNonQuery();

                                if ((res == null)
                                    || (Convert.ToInt32(res) == 0)
                                    || (res == DBNull.Value))
                                {
                                    Trace.TraceError("Unsuccessfull insert in GetAVGOfARoll(), query: {0}", command_insert.CommandText);
                                    throw new ArgumentNullException("insert command is unsuccesfull in getAVG()");
                                }
                                Trace.TraceInformation("Successful insert in GetAVGOfARoll(), query: {0}", command_insert.CommandText);
                            }

                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception in StoreAlternation() ex:{0} ", ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }


        }

        /// <summary>
        /// Calulate avg for
        /// blank_test and homogenity_test
        /// </summary>
        /// <param name="roll_id"></param>
        public void GetAVGOfARoll(string roll_id)
        {
            #region Variables
            bool valid = false;
            object avg = null;
            object identify_pk = null;
            object not_h62_error_count;
            object stddev = null;
            double cv;
            string query = string.Empty;
            string stddev_query = string.Empty;
            Color clr;
            #endregion

            Trace.TraceInformation(string.Format("Insert averages:GetAVGofARoll() roll_id:{0}", roll_id));
            query = string.Format("Select AVG(blank_test_result.nano_amper) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id where blank_test_identify.roll_id='{0}' and blank_test_identify.lot_id='{1}' and blank_test_result.code=170 and blank_test_result.glu<>0 and temperature_ok=true and blank_test_identify.invalidate=false ", roll_id, Users_Form.dev.LOT_ID);

            using (NpgsqlConnection connection = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        #region GetAVGforBlank
                        avg = command.ExecuteScalar();

                        if ((avg == DBNull.Value)
                            || (avg == null))
                        {

                            Trace.TraceError(string.Format("Avg is null, query:{0}", command.CommandText));
                            throw new ArgumentNullException("Avg is null");
                        }
                        if ((Convert.ToDouble(avg) < 13)
                            || (Convert.ToDouble(avg) > 51))
                        {
                            valid = false;
                            Trace.TraceWarning("Averages of nano_ampers is less than 13 or greater than 51, averages:{0}", Convert.ToDouble(avg));
                        }
                        else
                        {
                            valid = true;
                            Trace.TraceInformation(string.Format("Average is ok, average:{0}", Convert.ToDouble(avg)));
                        }
                        #endregion
                        Trace.TraceInformation("Calculate avg and stddev for blank test");
                        #region get stddev and cv for blank
                        using (NpgsqlCommand stddev_comm = new NpgsqlCommand(string.Format("Select STDDEV(blank_test_result.nano_amper) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id where blank_test_identify.roll_id='{0}' and blank_test_identify.lot_id='{1}' and blank_test_result.code=170 and blank_test_result.glu<>0 and temperature_ok=true and blank_test_identify.invalidate=false", roll_id, Users_Form.dev.LOT_ID), connection))
                        {
                            stddev = stddev_comm.ExecuteScalar();
                            if ((stddev == DBNull.Value)
                                || (stddev == null))
                            {
                                Trace.TraceError("stddev is null, query: {0}", stddev_comm.CommandText);
                            }
                        }

                        cv = (Convert.ToDouble(stddev) / Convert.ToDouble(avg)) * 100;
                        #endregion
                        Trace.TraceInformation("calculate not h62 errors count");
                        using (NpgsqlCommand error_count = new NpgsqlCommand(
                            string.Format("SELECT count(blank_test_errors.not_h62_error) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_identify.measure_type='homogenity' and blank_test_identify.lot_id='{0}' and blank_test_identify.roll_id='{1}' and blank_test_errors.not_h62_error=True and blank_test_identify.remeasured=false and blank_test_identify.invalidate=false",
                            Users_Form.dev.LOT_ID, Users_Form.dev.Roll), connection))
                        {
                            not_h62_error_count = error_count.ExecuteScalar();

                            if ((not_h62_error_count == null)
                                || (not_h62_error_count == DBNull.Value))
                            {
                                Trace.TraceError("error_count is null, query: {0}", error_count.CommandText);
                                throw new ArgumentNullException("error_count is null");
                            }
                            Trace.TraceInformation("error_count is null");
                        }
                        using (NpgsqlCommand comm = new NpgsqlCommand(
                            string.Format("select MAX(pk_id) from  blank_test_identify where roll_id='{0}' and lot_id='{1}' and remeasured=false and blank_test_identify.invalidate=false",
                            roll_id, Users_Form.dev.LOT_ID), connection))
                        {
                            identify_pk = comm.ExecuteScalar();

                            if ((identify_pk == null)
                                || (identify_pk == DBNull.Value))
                            {
                                Trace.TraceError("No pk_id in blank_test_identify for roll_id:{0} query: {1}", roll_id, comm.CommandText);
                                throw new ArgumentNullException("identify_pk is null");
                            }
                            Trace.TraceInformation("No pk_id in blank_test_identify for roll_id:{0} query: {1}", roll_id, comm.CommandText);

                            #region blank_test
                            if (Program.measureType == "blank")
                            {
                                using (NpgsqlCommand comm_blank =
                                    new NpgsqlCommand(
                                        string.Format("SELECT COUNT(roll_id) FROM blank_test_averages where roll_id='{0}' and lot_id='{1}' and invalidate=false",
                                    Users_Form.dev.Roll, Users_Form.dev.LOT_ID), connection))
                                {
                                    object result = null;
                                    result = comm_blank.ExecuteScalar();

                                    if (Convert.ToInt32(result) == 0)
                                    {
                                        #region insert to blank_avg
                                        using (NpgsqlCommand command_insert = new NpgsqlCommand(
                                         string.Format("insert into blank_test_averages(avg,roll_id,fk_blank_test_identify_id,blank_is_valid,tube_count_in_one_roll,stddev,cv,date,lot_id,remeasured,invalidate) values({0:0.00},'{1}',{2},{3},{4},{5:0.000},{6:0.000},{7},'{8}',{9},{10})",
                                         Math.Round(Convert.ToDouble(avg), 2), roll_id, Convert.ToInt32(identify_pk), valid, Users_Form.dev.lot_count_in_one_roll, Math.Round(Convert.ToDouble(stddev), 3), Math.Round(cv, 3), "@date", Users_Form.dev.LOT_ID, false, false), connection))
                                        {
                                            object res = null;
                                            command_insert.Parameters.AddWithValue("@date", DateTime.Now);
                                            res = command_insert.ExecuteNonQuery();

                                            if ((res == null)
                                                || (Convert.ToInt32(res) == 0)
                                                || (res == DBNull.Value))
                                            {
                                                Trace.TraceError("Unsuccessfull insert in GetAVGOfARoll(), query: {0}", command_insert.CommandText);
                                                throw new ArgumentNullException("insert command is unsuccesfull in getAVG()");
                                            }
                                            Trace.TraceInformation("Successful insert in GetAVGOfARoll(), query: {0}", command_insert.CommandText);
                                        }
                                        #endregion

                                        if (valid)
                                        {
                                            clr = Color.Green;
                                        }
                                        else
                                            clr = Color.Red;

                                        avg = Math.Round(Convert.ToDouble(avg), 2);
                                        cv = Math.Round(Convert.ToDouble(cv), 2);
                                        Trace.TraceInformation("Next showing the result of the measurement:MessageCompleteEventArgs.GetAVGOfARoll()");
                                        CreateChild(Users_Form.dev.LOT_ID, roll_id);
                                        ///Show actual blank test result                                        
                                    }
                                    else
                                    {
                                        MessageBox.Show(string.Format("A {0}. számú roll már le van mérve a {1}. számú LOT-ból!!", Users_Form.dev.Roll, Users_Form.dev.LOT_ID));
                                        Trace.TraceWarning(string.Format("A {0}. számú roll már le van mérve a {1}. számú LOT-ból!!", Users_Form.dev.Roll, Users_Form.dev.LOT_ID));
                                        Program.CloseWindow();
                                    }
                                }
                            }
                            #endregion
                            using (NpgsqlCommand getAveragesId = new NpgsqlCommand("select max(pk_id) from blank_test_identify", connection))
                            {
                                Users_Form.dev.Averages_ID = Convert.ToInt32(getAveragesId.ExecuteScalar());

                            }
                        }
                        Trace.TraceInformation("check if the measurement is finished in case of homogenity");
                        #region homogenity_test
                        if (Program.measureType == "homogenity")
                        {
                            CheckHomogenityIsValid();

                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception in MessageCompletedEventArgs.getAVG(): Message:{0};StackTrace:{1};InnerExc.:{2}", ex.Message, ex.StackTrace, ex.InnerException);
                }
                finally
                {
                    connection.Close();

                }

            }

        }

        private BlankFinishedResultsView _childForm;



        /// <summary>
        /// Create a result view for blank_test_values
        /// The command to copy differences between local DB and the release database(77i):
        /// pg_dump -U Username -h DatabaseEndPoint -a -t TableToCopy SourceDatabase | psql -h DatabaseEndPoint -p portNumber -U Username -W TargetDatabase
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="roll"></param>
        private void CreateChild(string lot, string roll)
        {
            if (Properties.Settings.Default.CommitChangesNeeded)
            {
                if (TestReleaseHostConnection())
                    CommitChangesToReleaseHost(Program.measureType);

                //Drop schema then create a new
                DropAndCreateSchema();
                //Restore schema from sql file
                SchemaRestore();
            }

            _childForm = new BlankFinishedResultsView(lot, roll);
            _childForm.ShowDialog();
        }
        public static bool networkIsOK = false;
        private void SchemaRestore()
        {
            System.Diagnostics.ProcessStartInfo pi = new System.Diagnostics.ProcessStartInfo(Properties.Settings.Default.DBSchemaRestoreScript);
            pi.UseShellExecute = true;
            System.Diagnostics.Process.Start(pi);
        }

        private void DropAndCreateSchema()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();

                    using (NpgsqlCommand dropSchema = new NpgsqlCommand("DROP SCHEMA public CASCADE", conn))
                    {
                        dropSchema.ExecuteNonQuery();
                    }
                    using (NpgsqlCommand createSchema = new NpgsqlCommand("CREATE SCHEMA public", conn))
                    {
                        createSchema.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public List<bool> PingResults = new List<bool>();
        /// <summary>
        /// This function is ping the 77i RELEASE host and returns true
        /// if the ping status is succeed
        /// </summary>
        /// <returns></returns>
        private bool TestReleaseHostConnection()
        {

            new PingService();

            Ping pingSender = new Ping();
            PingReply reply;

            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() && networkIsNotAvailable >=3 && networkIsOK)
            {
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        reply = pingSender.Send("77i.77el.hu");

                        if (reply.Status == IPStatus.Success)
                        {
                            Trace.TraceInformation("Ping Reply status is success");
                            PingResults.Add(true);
                        }
                        else
                        {
                            Trace.TraceInformation("Ping Reply status is unsuccess");
                            PingResults.Add(false);
                        }
                    }//end of for  

                    foreach (bool res in PingResults)
                    {
                        if (!res)
                        {
                            return false;
                        }
                    }
                    return true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Exception details: {0}; {1}", ex.Message, ex.StackTrace));
                    return false;
                }

            }
            else
            {
                networkIsNotAvailable++;

            }

           
         
            //default return is false
            return false;
        }
        
        

        /// <summary>
        /// Copy all data from localhost database to the release host (77i)
        /// then delete all data from the localhost database tables
        /// It is done with an AutoIt3 script which is using pgadmin's pg_dump and pg_restore
        /// pg_dump -U Username -h DatabaseEndPoint -a -t TableToCopy SourceDatabase | psql -h DatabaseEndPoint -p portNumber -U Username -W ///TargetDatabase
        /// </summary>
        private void CommitChangesToReleaseHost(string measurementType_in)
        {
            Program.QueriesMaxPrimaryKeysThenSetTheSequencesValue(Properties.Settings.Default.DBLocal, Properties.Settings.Default.DBReleaseConnection);

            System.Diagnostics.ProcessStartInfo pi = new System.Diagnostics.ProcessStartInfo(Properties.Settings.Default.DBUploaderScriptPath);
            pi.UseShellExecute = true;
            System.Diagnostics.Process.Start(pi);



        }

        /// <summary>
        /// Check homogenity test values and results
        /// </summary>
        private void CheckHomogenityIsValid()
        {
            #region Variables
            object lot_avg = null;
            object lot_stddev = null;
            Trace.TraceInformation("CheckHomogenity started");
            object identify_id = null;
            object stripCount = null;
            object not_h62_error_count = null;
            object h62_error_count = null;
            object wrong_strip_count = null;
            object wrong_strip_count2 = null;
            object stddev = null;
            object avg = null;
            object cv = null;
            object valid_strip_count = null;
            object valid_strip_count2 = null;
            int all_out_of_range_strip_count = 0;
            string query = string.Format("select count(pk_id) from blank_test_identify where roll_id='{0}' and temperature_ok=true and measure_type='homogenity' and invalidate=false", Users_Form.dev.Roll);

            string avgForLot = string.Format("Select AVG(blank_test_result.glu) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id where blank_test_identify.lot_id='{0}' and blank_test_result.glu<>0 and temperature_ok=true and blank_test_identify.measure_type='{1}'  and blank_test_identify.invalidate=false", Program.LOT_ID, Program.measureType);
            string stddevForLot = string.Format("Select STDDEV(blank_test_result.glu) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id where blank_test_identify.lot_id='{0}' and blank_test_result.glu<>0 and temperature_ok=true and blank_test_identify.measure_type='{1}'  and blank_test_identify.invalidate=false", Program.LOT_ID, Program.measureType);

            #endregion

            Trace.TraceInformation("CheckHomogenityIsValid() started");

            using (NpgsqlConnection homogenity_connection = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    homogenity_connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(string.Format("Select AVG(blank_test_result.glu) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id where homogenity_test.roll_id='{0}' and homogenity_test.lot_id='{1}' and blank_test_result.glu<>0 and blank_test_result.code=777 and homogenity_test.invalidate=false", Users_Form.dev.Roll, Program.LOT_ID), homogenity_connection))
                    {
                        #region Avg,SD,CV calculation
                        avg = command.ExecuteScalar();

                        if ((avg == DBNull.Value)
                            || (avg == null))
                        {

                            Trace.TraceError(string.Format("Avg is null, query:{0}", command.CommandText));
                            throw new ArgumentNullException("Avg is null");
                        }

                        using (NpgsqlCommand stddev_comm = new NpgsqlCommand(string.Format("Select STDDEV(blank_test_result.glu) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id where homogenity_test.roll_id='{0}' and homogenity_test.lot_id='{1}' and blank_test_result.glu<>0 and blank_test_result.code=777 and homogenity_test.invalidate=false", Users_Form.dev.Roll, Program.LOT_ID), homogenity_connection))
                        {
                            stddev = stddev_comm.ExecuteScalar();

                            if ((stddev == DBNull.Value)
                                || (stddev == null))
                            {
                                Trace.TraceError("stddev is null, query: {0}", stddev_comm.CommandText);

                            }
                        }

                        cv = (Convert.ToDouble(stddev) / Convert.ToDouble(avg)) * 100;

                        #endregion
                        #region Error counting
                        using (NpgsqlCommand error_count = new NpgsqlCommand(
                                           string.Format("SELECT count(blank_test_errors.not_h62_error) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where homogenity_test.roll_id='{0}' and homogenity_test.lot_id='{1}' and blank_test_errors.not_h62_error=true and homogenity_test.invalidate=false", Users_Form.dev.Roll, Program.LOT_ID), homogenity_connection))
                        {
                            not_h62_error_count = error_count.ExecuteScalar();

                            if ((not_h62_error_count == null)
                                || (not_h62_error_count == DBNull.Value))
                            {
                                Trace.TraceError("error_count is null, query: {0}", error_count.CommandText);
                                throw new ArgumentNullException("error_count is null");
                            }
                        }
                        using (NpgsqlCommand error_count = new NpgsqlCommand(string.Format("SELECT count(blank_test_errors.h62_error) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_identify.roll_id='{0}' and blank_test_identify.lot_id='{1}' and blank_test_errors.h62_error=true and blank_test_identify.invalidate=false", Users_Form.dev.Roll, Users_Form.dev.LOT_ID), homogenity_connection))
                        {
                            h62_error_count = error_count.ExecuteScalar();

                            if ((h62_error_count == null)
                                || (h62_error_count == DBNull.Value))
                            {
                                Trace.TraceError("error_count is null, query: {0}", error_count.CommandText);
                                throw new ArgumentNullException("error_count is null");
                            }
                        }

                        #endregion

                        #region Get Strip count in the measure
                        using (NpgsqlCommand stripCount1 = new NpgsqlCommand(stipCount_part1, homogenity_connection))      //get valid strip count in two step)
                        {
                            valid_strip_count = stripCount1.ExecuteScalar();
                        }
                        using (NpgsqlCommand stripCount2 = new NpgsqlCommand(stipCount_part2, homogenity_connection))
                        {
                            valid_strip_count2 = stripCount2.ExecuteScalar();
                        }
                        Users_Form.dev.stripCount = Convert.ToInt32(valid_strip_count) + Convert.ToInt32(valid_strip_count2);
                        #endregion

                        using (NpgsqlCommand identify_command = new NpgsqlCommand(
                            string.Format("select max(pk_id) from blank_test_identify where roll_id='{0}'", Users_Form.dev.Roll), homogenity_connection))
                        {                                                           //get the pk_id of blank_test_identify
                            identify_id = identify_command.ExecuteScalar();

                            if ((identify_id == DBNull.Value)
                                || (identify_id == null))
                            {
                                Trace.TraceError("identify_id is null, query: {0}", identify_command.CommandText);
                                throw new ArgumentNullException("identify_id is null");
                            }
                        }


                        #region Get strip count that out of range
                        Trace.TraceInformation("get wrong stripcount in two step:CheckHomogenityIsValid()");

                        using (NpgsqlCommand wrong_strip = new NpgsqlCommand(
                            string.Format(
                            "SELECT COUNT(homogenity_test.pk_id) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where homogenity_test.invalidate=False and homogenity_test.remeasured=False and homogenity_test.lot_id='{1}' and homogenity_test.roll_id='{0}' and blank_test_result.invalidate=False and blank_test_result.remeasured=False and blank_test_result.glu<>0", Users_Form.dev.Roll, Users_Form.dev.LOT_ID), homogenity_connection))//wrong_strip_count
                        {
                            wrong_strip_count = wrong_strip.ExecuteScalar();

                            if ((wrong_strip_count == DBNull.Value))
                            {
                                Trace.TraceError("wrong_strip_count is null, query: {0}", wrong_strip.CommandText);
                                throw new ArgumentNullException("wrong_strip_count is null");
                            }
                            Trace.TraceInformation("get wrong stripcount the first result:{0};CheckHomogenityIsValid()", wrong_strip_count);
                        }
                        using (NpgsqlCommand wrong_strip2 = new NpgsqlCommand(
                           string.Format(
                           "SELECT COUNT(homogenity_test.pk_id) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where homogenity_test.invalidate=False and homogenity_test.remeasured=False and homogenity_test.lot_id='{1}' and homogenity_test.roll_id='{0}' and blank_test_result.invalidate=False and blank_test_result.remeasured=False and blank_test_errors.error_text<>''", Users_Form.dev.Roll, Users_Form.dev.LOT_ID), homogenity_connection))//wrong_strip_count2
                        {
                            wrong_strip_count2 = wrong_strip2.ExecuteScalar();

                            if ((wrong_strip_count2 == DBNull.Value))
                            {
                                Trace.TraceError("wrong_strip_count is null, query: {0}", wrong_strip2.CommandText);
                                throw new ArgumentNullException("wrong_strip_count is null");
                            }
                            Trace.TraceInformation("get wrong stripcount the second result:{0};CheckHomogenityIsValid()", wrong_strip_count2);
                        }

                        all_out_of_range_strip_count = Convert.ToInt32(wrong_strip_count) + Convert.ToInt32(wrong_strip_count2);
                        stripCount = Users_Form.dev.stripCount;
                        #endregion

                        Trace.TraceInformation("Get if homogenity is valid:CheckHomogenityIsValid()");
                        #region Switch stripCount && not_h62_error_count && all_out_of_range_strip_count
                        if ((Convert.ToInt32(stripCount) < 49)
                            || (Convert.ToInt32(stripCount) > 333))
                        {
                            Trace.TraceError("Impossible stripCount");
                            Users_Form.dev.Homogenity_Is_Valid = false;

                        }
                        if ((Convert.ToInt32(stripCount) >= 49)
                            && (Convert.ToInt32(stripCount) <= 71))
                        {
                            Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 1");
                            if ((Convert.ToInt32(not_h62_error_count) <= 1)
                                && (Convert.ToInt32(Program.OutOfRangeCount_Homo) <= 1))
                            {
                                Users_Form.dev.Homogenity_Is_Valid = true;
                            }
                            else
                            {
                                if (Convert.ToInt32(not_h62_error_count) > 1)
                                {
                                    not_h62_is_valid = false;
                                }
                                else
                                    not_h62_is_valid = true;
                                if (Convert.ToInt32(Program.OutOfRangeCount_Homo) > 1)
                                {
                                    out_of_range_valid = false;
                                }
                                else
                                    out_of_range_valid = true;
                            }
                        }
                        if ((Convert.ToInt32(stripCount) >= 72)
                            && (Convert.ToInt32(stripCount) <= 94))
                        {
                            Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 2");
                            if ((Convert.ToInt32(not_h62_error_count) <= 2)
                                && (Convert.ToInt32(Program.OutOfRangeCount_Homo) <= 2))
                            {
                                Users_Form.dev.Homogenity_Is_Valid = true;
                            }
                            else
                            {
                                if (Convert.ToInt32(not_h62_error_count) > 2)
                                {
                                    not_h62_is_valid = false;
                                }
                                else
                                    not_h62_is_valid = true;
                                if (Convert.ToInt32(Program.OutOfRangeCount_Homo) > 2)
                                {
                                    out_of_range_valid = false;
                                }
                                else
                                    out_of_range_valid = true;
                            }
                        }
                        if ((Convert.ToInt32(stripCount) >= 95)
                           && (Convert.ToInt32(stripCount) <= 116))
                        {
                            Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 3");
                            if ((Convert.ToInt32(not_h62_error_count) <= 3)
                                && (Convert.ToInt32(Program.OutOfRangeCount_Homo) <= 3))
                            {
                                Users_Form.dev.Homogenity_Is_Valid = true;
                            }
                            else
                            {
                                if (Convert.ToInt32(not_h62_error_count) > 3)
                                {
                                    not_h62_is_valid = false;
                                }
                                else
                                    not_h62_is_valid = true;
                                if (Convert.ToInt32(Program.OutOfRangeCount_Homo) > 3)
                                {
                                    out_of_range_valid = false;
                                }
                                else
                                    out_of_range_valid = true;
                            }
                        }
                        if ((Convert.ToInt32(stripCount) >= 117)
                           && (Convert.ToInt32(stripCount) <= 139))
                        {
                            Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 4");
                            if ((Convert.ToInt32(not_h62_error_count) <= 4)
                                && (Convert.ToInt32(Program.OutOfRangeCount_Homo) <= 4))
                            {
                                Users_Form.dev.Homogenity_Is_Valid = true;
                            }
                            else
                            {
                                if (Convert.ToInt32(not_h62_error_count) > 4)
                                {
                                    not_h62_is_valid = false;
                                }
                                else
                                    not_h62_is_valid = true;
                                if (Convert.ToInt32(Program.OutOfRangeCount_Homo) > 4)
                                {
                                    out_of_range_valid = false;
                                }
                                else
                                    out_of_range_valid = true;
                            }
                        }
                        if ((Convert.ToInt32(stripCount) >= 140)
                           && (Convert.ToInt32(stripCount) <= 161))
                        {
                            Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 5");
                            if ((Convert.ToInt32(not_h62_error_count) <= 5)
                                && (Convert.ToInt32(Program.OutOfRangeCount_Homo) <= 5))
                            {
                                Users_Form.dev.Homogenity_Is_Valid = true;
                            }
                            else
                            {
                                if (Convert.ToInt32(not_h62_error_count) > 5)
                                {
                                    not_h62_is_valid = false;
                                }
                                else
                                    not_h62_is_valid = true;
                                if (Convert.ToInt32(Program.OutOfRangeCount_Homo) > 5)
                                {
                                    out_of_range_valid = false;
                                }
                                else
                                    out_of_range_valid = true;
                            }
                        }
                        if ((Convert.ToInt32(stripCount) >= 162)
                           && (Convert.ToInt32(stripCount) <= 183))
                        {
                            Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 6");
                            if ((Convert.ToInt32(not_h62_error_count) <= 6)
                                && (Convert.ToInt32(Program.OutOfRangeCount_Homo) <= 6))
                            {
                                Users_Form.dev.Homogenity_Is_Valid = true;
                            }
                            else
                            {
                                if (Convert.ToInt32(not_h62_error_count) > 6)
                                {
                                    not_h62_is_valid = false;
                                }
                                else
                                    not_h62_is_valid = true;
                                if (Convert.ToInt32(Program.OutOfRangeCount_Homo) > 6)
                                {
                                    out_of_range_valid = false;
                                }
                                else
                                    out_of_range_valid = true;
                            }
                        }
                        if ((Convert.ToInt32(stripCount) >= 184)
                           && (Convert.ToInt32(stripCount) <= 204))
                        {
                            Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 7");
                            if ((Convert.ToInt32(not_h62_error_count) <= 7)
                                && (Convert.ToInt32(Program.OutOfRangeCount_Homo) <= 7))
                            {
                                Users_Form.dev.Homogenity_Is_Valid = true;
                            }
                            else
                            {
                                if (Convert.ToInt32(not_h62_error_count) > 7)
                                {
                                    not_h62_is_valid = false;
                                }
                                else
                                    not_h62_is_valid = true;
                                if (Convert.ToInt32(Program.OutOfRangeCount_Homo) > 7)
                                {
                                    out_of_range_valid = false;
                                }
                                else
                                    out_of_range_valid = true;
                            }
                        }
                        if ((Convert.ToInt32(stripCount) >= 205)
                           && (Convert.ToInt32(stripCount) <= 226))
                        {
                            Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 8");
                            if ((Convert.ToInt32(not_h62_error_count) <= 8)
                                && (Convert.ToInt32(Program.OutOfRangeCount_Homo) <= 8))
                            {
                                Users_Form.dev.Homogenity_Is_Valid = true;
                            }
                            else
                            {
                                if (Convert.ToInt32(not_h62_error_count) > 8)
                                {
                                    not_h62_is_valid = false;
                                }
                                else
                                    not_h62_is_valid = true;
                                if (Convert.ToInt32(Program.OutOfRangeCount_Homo) > 8)
                                {
                                    out_of_range_valid = false;
                                }
                                else
                                    out_of_range_valid = true;
                            }
                        }
                        if ((Convert.ToInt32(stripCount) >= 227)
                           && (Convert.ToInt32(stripCount) <= 248))
                        {
                            Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 9");
                            if ((Convert.ToInt32(not_h62_error_count) <= 9)
                                && (Convert.ToInt32(Program.OutOfRangeCount_Homo) <= 9))
                            {
                                Users_Form.dev.Homogenity_Is_Valid = true;
                            }
                            else
                            {
                                if (Convert.ToInt32(not_h62_error_count) > 9)
                                {
                                    not_h62_is_valid = false;
                                }
                                else
                                    not_h62_is_valid = true;
                                if (Convert.ToInt32(Program.OutOfRangeCount_Homo) > 9)
                                {
                                    out_of_range_valid = false;
                                }
                                else
                                    out_of_range_valid = true;
                            }
                        }
                        if ((Convert.ToInt32(stripCount) >= 249)
                           && (Convert.ToInt32(stripCount) <= 269))
                        {
                            Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 10");
                            if ((Convert.ToInt32(not_h62_error_count) <= 10)
                                && (Convert.ToInt32(Program.OutOfRangeCount_Homo) <= 10))
                            {
                                Users_Form.dev.Homogenity_Is_Valid = true;
                            }
                            else
                            {
                                if (Convert.ToInt32(not_h62_error_count) > 10)
                                {
                                    not_h62_is_valid = false;
                                }
                                else
                                    not_h62_is_valid = true;
                                if (Convert.ToInt32(Program.OutOfRangeCount_Homo) > 10)
                                {
                                    out_of_range_valid = false;
                                }
                                else
                                    out_of_range_valid = true;
                            }
                        }
                        if ((Convert.ToInt32(stripCount) >= 270)
                           && (Convert.ToInt32(stripCount) <= 291))
                        {
                            Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 11");
                            if ((Convert.ToInt32(not_h62_error_count) <= 11)
                                && (Convert.ToInt32(Program.OutOfRangeCount_Homo) <= 11))
                            {
                                Users_Form.dev.Homogenity_Is_Valid = true;
                            }
                            else
                            {
                                if (Convert.ToInt32(not_h62_error_count) > 11)
                                {
                                    not_h62_is_valid = false;
                                }
                                else
                                    not_h62_is_valid = true;
                                if (Convert.ToInt32(Program.OutOfRangeCount_Homo) > 11)
                                {
                                    out_of_range_valid = false;
                                }
                                else
                                    out_of_range_valid = true;
                            }
                        }
                        if ((Convert.ToInt32(stripCount) >= 292)
                           && (Convert.ToInt32(stripCount) <= 312))
                        {
                            Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 12");
                            if ((Convert.ToInt32(not_h62_error_count) <= 12)
                                && (Convert.ToInt32(Program.OutOfRangeCount_Homo) <= 12))
                            {
                                Users_Form.dev.Homogenity_Is_Valid = true;
                            }
                            else
                            {
                                if (Convert.ToInt32(not_h62_error_count) > 12)
                                {
                                    not_h62_is_valid = false;
                                }
                                else
                                    not_h62_is_valid = true;
                                if (Convert.ToInt32(Program.OutOfRangeCount_Homo) > 12)
                                {
                                    out_of_range_valid = false;
                                }
                                else
                                    out_of_range_valid = true;
                            }
                        }

                        if ((Convert.ToInt32(stripCount) >= 313)
                           && (Convert.ToInt32(stripCount) <= 333))
                        {
                            Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 13");
                            if ((Convert.ToInt32(not_h62_error_count) <= 13)
                                && (Convert.ToInt32(Program.OutOfRangeCount_Homo) <= 13))
                            {
                                Users_Form.dev.Homogenity_Is_Valid = true;
                            }
                            else
                            {
                                if (Convert.ToInt32(not_h62_error_count) > 13)
                                {
                                    not_h62_is_valid = false;
                                }
                                else
                                    not_h62_is_valid = true;
                                if (Convert.ToInt32(Program.OutOfRangeCount_Homo) > 13)
                                {
                                    out_of_range_valid = false;
                                }
                                else
                                    out_of_range_valid = true;
                            }
                        }
                        outAndStripCountAndError_ok = Users_Form.dev.Homogenity_Is_Valid;
                        #endregion

                        #region Calculate lot avg,sd,cv
                        using (NpgsqlCommand lotAVG = new NpgsqlCommand(string.Format("SELECT AVG(blank_test_result.glu) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where homogenity_test.lot_id='{0}' and blank_test_result.glu<>0  and blank_test_result.code=777 and homogenity_test.invalidate=false", Program.LOT_ID, Program.measureType), homogenity_connection))
                        {

                            lot_avg = lotAVG.ExecuteScalar();

                            if (lot_avg == DBNull.Value)
                            {
                                throw new SqlNoValueException(string.Format("No value for this query: {0}", lotAVG.CommandText));
                            }

                        }
                        using (NpgsqlCommand lotSTDDEV = new NpgsqlCommand(string.Format("SELECT STDDEV(blank_test_result.glu) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where homogenity_test.lot_id='{0}' and blank_test_result.glu<>0  and blank_test_result.code=777 and homogenity_test.invalidate=false", Program.LOT_ID, Program.measureType), homogenity_connection))
                        {

                            lot_stddev = lotSTDDEV.ExecuteScalar();

                            if (lot_stddev == DBNull.Value)
                            {
                                throw new SqlNoValueException(string.Format("No value for this query: {0}", lotSTDDEV.CommandText));
                            }
                        }
                        #endregion

                        Trace.TraceInformation("SaveHomogenity Result started");

                        SaveHomogenityAlternation(avg, stddev, cv, not_h62_error_count, h62_error_count, homogenity_connection);//save homogenity alternation

                        #region StoreHomogenityResult
                        if (Convert.ToInt32(Users_Form.dev.stripCount) >= (Program.TubeCount * 4))
                        {
                            using (NpgsqlCommand comm_homo =
                                                           new NpgsqlCommand(
                                                               string.Format("select COUNT(roll_id) from homogenity_result where roll_id='{0}' and lot_id='{1}' and invalidate=false", Users_Form.dev.Roll, Users_Form.dev.LOT_ID), homogenity_connection))
                            {
                                object res = null;
                                res = comm_homo.ExecuteScalar();

                                if (Convert.ToInt32(res) == 0)
                                {
                                    Trace.TraceInformation("Store to Homogenity result");

                                    SaveHomogenityResult(avg, stddev, cv, not_h62_error_count, h62_error_count,
                                           Users_Form.dev.Homogenity_Is_Valid, homogenity_connection, lot_avg, lot_stddev, Convert.ToInt32(Program.OutOfRangeCount_Homo),
                                          out_of_range_valid, not_h62_is_valid);

                                    Trace.TraceInformation(string.Format("{0} tubus lemérve({0}*4 csík)", Users_Form.dev.lot_count_in_one_roll));
                                    AppendTextToUser(string.Format("{0} számú roll lemérve", Users_Form.dev.Roll), Users_Form, Color.Magenta);
                                    MessageBox.Show(string.Format("A {0} számú roll lemérve a {1} számú LOT-ból", Users_Form.dev.Roll, Users_Form.dev.LOT_ID));

                                    Program.CloseWindow();
                                }
                                else
                                {
                                    MessageBox.Show(string.Format("A {0} számú roll már le van mérve a {1} számú LOT-ból", Users_Form.dev.Roll, Users_Form.dev.LOT_ID));
                                    Program.CloseWindow();
                                }
                            }
                        }
                        else
                        {
                            Trace.TraceWarning("Can't store homogenity final result, Measured stripcount:{0},Needed strip count:{1}", Users_Form.dev.stripCount, Program.TubeCount * 4);

                        }
                        #endregion

                    }
                }
                catch (Exception ex)
                {

                    Trace.TraceError("Exception in CheckHomogenityIsvalid(), exception: {0}", ex.Message);
                }
                finally
                {
                    homogenity_connection.Close();
                }

            }
        }

        /// <summary>
        /// Insert results to lot_result and roll_result table
        /// </summary>
        /// <param name="lot_avg"></param>
        /// <param name="lot_stddev"></param>
        /// <param name="homogenityPKey"></param>
        /// <param name="roll_avg"></param>
        /// <param name="roll_cv"></param>
        /// <param name="roll_stddev"></param>
        private void SaveLotResult(object lot_avg, object lot_stddev, int homogenityPKey, object roll_avg, object roll_cv, object roll_stddev)
        {
            #region Variables
            double lot_cv = (Convert.ToDouble(lot_stddev) / Convert.ToDouble(lot_avg) * 100);
            bool avg_ok;
            bool cv_ok;
            object fk_lot_res_id;
            object not_h62_error_count;
            object h62_error_count;
            object lot_valid_strip_count;
            object lot_valid_strip_count2;
            int tha_strip_count_in_measure;
            bool roll_is_valid;
            #endregion

            Trace.TraceInformation("SaveLotResult started");

            if ((Convert.ToDouble(lot_avg) > 6.17)
                && (Convert.ToDouble(lot_avg) < 6.81))
            {
                avg_ok = true;
            }
            else
                avg_ok = false;

            if (lot_cv < 4.3)
            {
                cv_ok = true;
            }
            else
                cv_ok = false;


            Trace.TraceInformation("SaveLotResult() avg_ok:{0}/cv_ok:{1}", avg_ok, cv_ok);
            using (NpgsqlConnection lot_connection = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    lot_connection.Open();

                    #region get LOT STDDEV
                    using (NpgsqlCommand stddev_comm = new NpgsqlCommand(
                        string.Format("SELECT STDDEV(blank_test_result.glu) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_identify.lot_id='{0}' and blank_test_errors.error='' and blank_test_identify.remeasured=false and blank_test_identify.invalidate=false", Users_Form.dev.LOT_ID), lot_connection))
                    {

                        object lots_stddev = null;
                        lots_stddev = stddev_comm.ExecuteScalar();

                        if ((lots_stddev == DBNull.Value)
                            || (lots_stddev == null))
                        {
                            Trace.TraceError("stddev is null, query: {0}", stddev_comm.CommandText);
                            throw new SqlNoValueException(string.Format("STDDEV query for lot is null or has no value, query:{0}", stddev_comm.CommandText));
                        }
                    }
                    Trace.TraceInformation("Lot STDDEV is {0}", lot_stddev);
                    #endregion

                    lot_cv = (Convert.ToDouble(lot_stddev) / Convert.ToDouble(lot_avg)) * 100;
                    Trace.TraceInformation("Lot CV is {0}", lot_cv);

                    #region Get strip count

                    string strip_count_01 = string.Format("SELECT COUNT(blank_test_identify.pk_id) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE (blank_test_errors.error_text='' and blank_test_result.glu<>0) and blank_test_identify.lot_id='{0}'  and blank_test_identify.measure_type='homogenity'  and blank_test_identify.invalidate=false and blank_test_identify.remeasured=false", Users_Form.dev.LOT_ID);
                    string strip_count_02 = string.Format("SELECT COUNT(blank_test_identify.pk_id) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE blank_test_errors.error_text<>'' and blank_test_identify.lot_id='{0}'  and blank_test_identify.measure_type='homogenity'  and blank_test_identify.invalidate=false and blank_test_identify.remeasured=false", Users_Form.dev.LOT_ID);

                    using (NpgsqlCommand stripCount1 = new NpgsqlCommand(strip_count_01, lot_connection))      //get valid strip count in two step)
                    {
                        lot_valid_strip_count = stripCount1.ExecuteScalar();
                    }
                    using (NpgsqlCommand stripCount2 = new NpgsqlCommand(strip_count_02, lot_connection))
                    {
                        lot_valid_strip_count2 = stripCount2.ExecuteScalar();
                    }
                    int lot_strip_count = Convert.ToInt32(lot_valid_strip_count) + Convert.ToInt32(lot_valid_strip_count2);

                    #endregion

                    #region get Lot error count

                    using (NpgsqlCommand error_count = new NpgsqlCommand(
                        string.Format("SELECT count(blank_test_errors.not_h62_error) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_identify.measure_type='homogenity' and blank_test_identify.lot_id='{0}' and blank_test_errors.not_h62_error=True and blank_test_identify.invalidate=false and  blank_test_identify.remeasured=false", Users_Form.dev.LOT_ID), lot_connection))
                    {
                        not_h62_error_count = error_count.ExecuteScalar();

                        if ((not_h62_error_count == null)
                            || (not_h62_error_count == DBNull.Value))
                        {
                            Trace.TraceError("error_count is null, query: {0}", error_count.CommandText);
                            throw new ArgumentNullException("error_count is null");
                        }
                    }

                    using (NpgsqlCommand error_count = new NpgsqlCommand(
                        string.Format("SELECT count(blank_test_errors.h62_error) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_identify.measure_type='homogenity' and blank_test_identify.lot_id='{0}' and blank_test_errors.h62_error=True and blank_test_identify.invalidate=false and  blank_test_identify.remeasured=false", Users_Form.dev.LOT_ID), lot_connection))
                    {
                        h62_error_count = error_count.ExecuteScalar();

                        if ((h62_error_count == null)
                            || (h62_error_count == DBNull.Value))
                        {
                            Trace.TraceError("error_count is null, query: {0}", error_count.CommandText);
                            throw new ArgumentNullException("error_count is null");
                        }
                    }

                    #endregion

                    #region get lot strip count
                    string lot_strip_count_01 = string.Format("SELECT COUNT(blank_test_identify.pk_id) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE (blank_test_errors.error_text='' and blank_test_result.glu<>0) and blank_test_identify.lot_id='{0}'  and blank_test_identify.measure_type='homogenity' and blank_test_identify.invalidate=false and blank_test_identify.remeasured=false", Users_Form.dev.LOT_ID);
                    string lot_strip_count_02 = string.Format("SELECT COUNT(blank_test_identify.pk_id) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE blank_test_errors.error_text<>'' and blank_test_identify.lot_id='{0}'  and blank_test_identify.measure_type='homogenity' and blank_test_identify.invalidate=false and blank_test_identify.remeasured=false", Users_Form.dev.LOT_ID);

                    using (NpgsqlCommand stripCount1 = new NpgsqlCommand(strip_count_01, lot_connection))      //get valid strip count in two step)
                    {
                        lot_valid_strip_count = stripCount1.ExecuteScalar();
                    }
                    using (NpgsqlCommand stripCount2 = new NpgsqlCommand(strip_count_02, lot_connection))
                    {
                        lot_valid_strip_count2 = stripCount2.ExecuteScalar();
                    }
                    tha_strip_count_in_measure = Convert.ToInt32(lot_valid_strip_count) + Convert.ToInt32(lot_valid_strip_count2);
                    #endregion

                    #region get out of range strip count
                    using (NpgsqlCommand getOUT = new NpgsqlCommand(string.Format("SELECT COUNT(homogenity_test.strip_ok) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_result.lot_id='{0}' and homogenity_test.strip_ok=False and blank_test_result.glu<>0 and homogenity_test.invalidate=false", Program.LOT_ID), lot_connection))
                    {//SELECT blank_test_result.glu,blank_test_result.roll_id,blank_test_result.code,blank_test_result.lot_id,blank_test_result.invalidate,homogenity_test.strip_ok,homogenity_test.invalidate,homogenity_test.pk_id,homogenity_test.sn,blank_test_errors.error_text FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_result.lot_id='finalLOT' and homogenity_test.strip_ok=False and blank_test_result.glu<>0 and blank_test_result.roll_id='1' 

                        object out_count = null;
                        out_count = getOUT.ExecuteScalar();

                        if (out_count == DBNull.Value)
                        {
                            Trace.TraceError("out_count is null, query: {0}", getOUT.CommandText);
                            throw new ArgumentNullException("out_count is null");
                        }
                        altogether_lot_out_count += Convert.ToInt32(out_count);
                    }
                    #endregion


                    #region get Lot level error counting
                    if ((tha_strip_count_in_measure < 244)
                        || (tha_strip_count_in_measure > 3259))
                    {
                        Users_Form.dev.H62_error_count_valid = false;
                        Users_Form.dev.Not_H62_error_count_valid = false;
                        Users_Form.dev.lot_strip_count_ok = false;
                        Users_Form.dev.lot_is_valid = false;
                        Trace.TraceError("Impossible stripCount");

                    }
                    else if ((tha_strip_count_in_measure >= 244)
                               && (tha_strip_count_in_measure <= 360))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 1
                            && Convert.ToInt32(h62_error_count) <= 1)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;
                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                        {
                            Users_Form.dev.lot_strip_count_ok = false;
                            Users_Form.dev.lot_is_valid = false;
                        }
                    }
                    if ((tha_strip_count_in_measure >= 361)
                        && (tha_strip_count_in_measure <= 475))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 2
                            && Convert.ToInt32(h62_error_count) <= 6)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                        {
                            Users_Form.dev.lot_strip_count_ok = false;
                            Users_Form.dev.lot_is_valid = false;
                        }
                    }
                    if ((tha_strip_count_in_measure >= 476)
                       && (tha_strip_count_in_measure <= 588))
                    {

                        Users_Form.dev.lot_strip_count_ok = true;
                        if (Convert.ToInt32(not_h62_error_count) <= 3
                            && Convert.ToInt32(h62_error_count) <= 9)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 589)
                       && (tha_strip_count_in_measure <= 699))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 4
                            && Convert.ToInt32(h62_error_count) <= 11)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 700)
                       && (tha_strip_count_in_measure <= 810))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 5
                            && Convert.ToInt32(h62_error_count) <= 14)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 811)
                      && (tha_strip_count_in_measure <= 919))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 6
                            && Convert.ToInt32(h62_error_count) <= 17)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 920)
                      && (tha_strip_count_in_measure <= 1028))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 7
                            && Convert.ToInt32(h62_error_count) <= 20)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 1029)
                      && (tha_strip_count_in_measure <= 1137))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 8
                            && Convert.ToInt32(h62_error_count) <= 23)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 1138)
                      && (tha_strip_count_in_measure <= 1245))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 9
                            && Convert.ToInt32(h62_error_count) <= 25)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 1246)
                      && (tha_strip_count_in_measure <= 1353))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 10
                            && Convert.ToInt32(h62_error_count) <= 28)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 1354)
                      && (tha_strip_count_in_measure <= 1461))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 11
                            && Convert.ToInt32(h62_error_count) <= 31)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 1462)
                      && (tha_strip_count_in_measure <= 1568))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 12
                            && Convert.ToInt32(h62_error_count) <= 34)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 1569)
                      && (tha_strip_count_in_measure <= 1675))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 13
                            && Convert.ToInt32(h62_error_count) <= 36)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 1676)
                      && (tha_strip_count_in_measure <= 1781))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 14
                            && Convert.ToInt32(h62_error_count) <= 42)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 1782)
                      && (tha_strip_count_in_measure <= 1888))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 15
                            && Convert.ToInt32(h62_error_count) <= 42)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 1889)
                     && (tha_strip_count_in_measure <= 1994))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 16
                            && Convert.ToInt32(h62_error_count) <= 45)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 1995)
                     && (tha_strip_count_in_measure <= 2100))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 17
                            && Convert.ToInt32(h62_error_count) <= 47)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 2101)
                     && (tha_strip_count_in_measure <= 2206))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 18
                            && Convert.ToInt32(h62_error_count) <= 50)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 2207)
                     && (tha_strip_count_in_measure <= 2312))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 19
                            && Convert.ToInt32(h62_error_count) <= 53)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 2313)
                     && (tha_strip_count_in_measure <= 2418))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 20
                            && Convert.ToInt32(h62_error_count) <= 56)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 2419)
                     && (tha_strip_count_in_measure <= 2523))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 21
                            && Convert.ToInt32(h62_error_count) <= 59)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 2524)
                     && (tha_strip_count_in_measure <= 2629))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 22
                            && Convert.ToInt32(h62_error_count) <= 61)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 2630)
                     && (tha_strip_count_in_measure <= 2734))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 23
                            && Convert.ToInt32(h62_error_count) <= 64)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 2735)
                     && (tha_strip_count_in_measure <= 2839))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 24
                            && Convert.ToInt32(h62_error_count) <= 67)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 2840)
                     && (tha_strip_count_in_measure <= 2944))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 25
                            && Convert.ToInt32(h62_error_count) <= 70)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 2945)
                     && (tha_strip_count_in_measure <= 3049))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 26
                            && Convert.ToInt32(h62_error_count) <= 72)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 3050)
                     && (tha_strip_count_in_measure <= 3154))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 27
                            && Convert.ToInt32(h62_error_count) <= 75)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    if ((tha_strip_count_in_measure >= 3155)
                     && (tha_strip_count_in_measure <= 3259))
                    {
                        Users_Form.dev.lot_strip_count_ok = true;

                        if (Convert.ToInt32(not_h62_error_count) <= 28
                            && Convert.ToInt32(h62_error_count) <= 78)
                        {
                            Users_Form.dev.H62_error_count_valid = true;
                            Users_Form.dev.Not_H62_error_count_valid = true;

                            Users_Form.dev.lot_is_valid = true;
                        }
                        else
                            Users_Form.dev.lot_is_valid = false;
                    }
                    #endregion

                    if (cv_ok && Users_Form.dev.lot_is_valid)
                    {
                        Users_Form.dev.lot_is_valid = true;
                    }
                    using (NpgsqlCommand insert_lot_res = new NpgsqlCommand(
                        string.Format("insert into lot_result(avg,stddev,cv,fk_homogenity_result_id,avg_ok,cv_ok,not_h62_strip_errors,h62_strip_errors,lot_id,lot_strip_count,lot_is_valid,date,out_of_range_strip_count,invalidate,modified) values({0:0.00},{1:0.00},{2:0.00},{3},{4},{5},{6},{7},'{8}',{9},{10},{11},{12},{13},{14})",
                        lot_avg, lot_stddev, lot_cv, homogenityPKey, avg_ok, cv_ok, Convert.ToInt32(not_h62_error_count), Convert.ToInt32(h62_error_count), Users_Form.dev.LOT_ID, tha_strip_count_in_measure, Users_Form.dev.lot_is_valid, "@date", altogether_lot_out_count, false, false), lot_connection))
                    {
                        object res = null;

                        insert_lot_res.Parameters.AddWithValue("@date", DateTime.Now);
                        res = insert_lot_res.ExecuteNonQuery();

                        if ((res == DBNull.Value)
                            || (Convert.ToInt32(res) == 0))
                        {
                            Trace.TraceError("No value for this insert statement:{0}", insert_lot_res.CommandText);
                            throw new SqlNoValueException("Unsuccessfull insert in SaveLotResult");
                        }
                    }


                    #region Store roll data

                    string queryAVG = string.Format("Select AVG(blank_test_result.glu) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id where blank_test_identify.roll_id='{0}' and blank_test_identify.lot_id='{1}' and blank_test_result.glu<>0 and temperature_ok=true and blank_test_identify.measure_type='{2}' and blank_test_identify.invalidate=false", Users_Form.dev.Roll, Users_Form.dev.LOT_ID, Program.measureType);
                    string stddev_query = string.Format("Select STDDEV(blank_test_result.glu) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id where blank_test_identify.roll_id='{0}' and blank_test_identify.lot_id='{1}' and blank_test_result.glu<>0 and temperature_ok=true and blank_test_identify.measure_type='{2}' and blank_test_identify.invalidate=false", Users_Form.dev.Roll, Users_Form.dev.LOT_ID, Program.measureType);

                    using (NpgsqlCommand lotResultID = new NpgsqlCommand(string.Format("select max(pk_id) from lot_result", Users_Form.dev.Roll), lot_connection))
                    {

                        fk_lot_res_id = lotResultID.ExecuteScalar();

                        if ((fk_lot_res_id == DBNull.Value)
                            || (fk_lot_res_id == null))
                        {
                            throw new SqlNoValueException(string.Format("No value for this query: {0}", lotResultID.CommandText));
                        }
                    }

                    using (NpgsqlCommand getavgForRoll = new NpgsqlCommand(string.Format("Select AVG(blank_test_result.glu) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id where blank_test_identify.roll_id='{0}' and blank_test_identify.lot_id='{1}' and blank_test_result.glu<>0 and temperature_ok=true and blank_test_identify.measure_type='{2}' and blank_test_identify.invalidate=false", Users_Form.dev.Roll, Program.LOT_ID, Program.measureType), lot_connection))
                    {
                        roll_avg = getavgForRoll.ExecuteScalar();

                        if ((roll_avg == DBNull.Value)
                            || (roll_avg == null))
                        {

                            Trace.TraceError(string.Format("Avg is null, query:{0}", getavgForRoll.CommandText));
                            throw new ArgumentNullException("Avg is null");
                        }
                        Trace.TraceInformation("avg of roll {0} is {1}", Users_Form.dev.Roll, roll_avg);
                        using (NpgsqlCommand stddev_comm = new NpgsqlCommand(string.Format("SELECT STDDEV(blank_test_result.glu)  FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where blank_test_result.code=777 and blank_test_result.roll_id='{1}' and blank_test_result.lot_id='{0}' and blank_test_result.glu<>0 and homogenity_test.invalidate=False ", Program.LOT_ID, Users_Form.dev.Roll), lot_connection))
                        {
                            roll_stddev = stddev_comm.ExecuteScalar();

                            if ((roll_stddev == DBNull.Value)
                                || (roll_stddev == null))
                            {
                                Trace.TraceError("stddev is null, query: {0}", stddev_comm.CommandText);
                            }
                            Trace.TraceInformation("sd of roll {0} is {1}", Users_Form.dev.Roll, roll_stddev);
                        }

                        roll_cv = (Convert.ToDouble(roll_stddev) / Convert.ToDouble(roll_avg)) * 100;
                        double diff = (Convert.ToDouble(lot_avg) / 100) * 5;
                        Trace.TraceInformation("cv of roll {0} is {1}", Users_Form.dev.Roll, roll_cv);
                        bool avg_percent_ok;
                        bool roll_cv_ok;

                        if ((Convert.ToDouble(roll_avg) + diff > Convert.ToDouble(lot_avg))
                            && (Convert.ToDouble(roll_avg) - diff < Convert.ToDouble(lot_avg)))
                        {
                            avg_percent_ok = true;
                        }
                        else
                            avg_percent_ok = false;

                        if (Convert.ToDouble(roll_cv) < 3.3)
                        {
                            roll_cv_ok = true;
                        }
                        else
                            roll_cv_ok = false;

                        #region Get roll strip count

                        object roll_valid_strip_count;
                        object roll_valid_strip_count2;

                        strip_count_01 = string.Format("SELECT COUNT(homogenity_test.pk_id) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE (blank_test_errors.error='' and blank_test_result.glu<>0) and homogenity_test.lot_id='{0}'  and blank_test_result.code=777 and homogenity_test.roll_id='{2}' and homogenity_test.invalidate=false ", Program.LOT_ID, Program.measureType, Users_Form.dev.Roll);
                        strip_count_02 = string.Format("SELECT COUNT(homogenity_test.pk_id) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE blank_test_errors.error_text<>'' and homogenity_test.lot_id='{0}' and blank_test_result.code=777 and homogenity_test.roll_id='{2}' and homogenity_test.invalidate=false ", Program.LOT_ID, Program.measureType, Users_Form.dev.Roll);

                        using (NpgsqlCommand stripCount1 = new NpgsqlCommand(strip_count_01, lot_connection))      //get valid strip count in two step)
                        {
                            roll_valid_strip_count = stripCount1.ExecuteScalar();
                        }
                        using (NpgsqlCommand stripCount2 = new NpgsqlCommand(strip_count_02, lot_connection))
                        {
                            roll_valid_strip_count2 = stripCount2.ExecuteScalar();
                        }
                        int tha_strip_count_in_roll = Convert.ToInt32(roll_valid_strip_count) + Convert.ToInt32(roll_valid_strip_count2);

                        #endregion

                        Trace.TraceInformation("Get out of range strip count in a roll :SaveLotResult()");
                        #region Get out of range strip count in Roll
                        using (NpgsqlCommand get_out_of_range_strip_count =
                            new NpgsqlCommand(
                                string.Format("SELECT COUNT(homogenity_test.pk_id) FROM homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where homogenity_test.invalidate=false and homogenity_test.strip_ok=false and blank_test_result.lot_id='{1}' and blank_test_result.roll_id='{0}' and blank_test_result.glu<>0 ", Users_Form.dev.Roll, Program.LOT_ID), lot_connection))
                        {
                            roll_out_of_range_strip_count = get_out_of_range_strip_count.ExecuteScalar();

                            if (roll_out_of_range_strip_count == DBNull.Value)
                            {
                                Trace.TraceError("No value: {0}", get_out_of_range_strip_count.CommandText);
                            }
                        }
                        if (roll_cv_ok
                           && avg_percent_ok
                           && not_h62_is_valid
                           && out_of_range_valid
                            && outAndStripCountAndError_ok
                            && tha_strip_count_in_roll > 49)
                        {
                            roll_is_valid = true;
                        }
                        else
                            roll_is_valid = false;
                        #endregion

                        using (NpgsqlCommand insert_roll_res = new NpgsqlCommand(
                            string.Format("insert into roll_result(roll_id,lot_id,roll_avg,roll_stddev,avg_ok,cv_ok,fk_lot_result_id,roll_cv,roll_is_valid,roll_strip_count,roll_date,out_of_range_strip_count,invalidate,remeasured) values('{0}','{1}',{2:0.00},{3:0.000},{4},{5},{6},{7:0.00},{8},{9},{10},{11},{12},{13})",
                            Users_Form.dev.Roll, Program.LOT_ID, Math.Round(Convert.ToDouble(roll_avg), 2), Math.Round(Convert.ToDouble(roll_stddev), 3), avg_percent_ok, roll_cv_ok, fk_lot_res_id, Math.Round(Convert.ToDouble(roll_cv), 2), roll_is_valid, tha_strip_count_in_roll, "@roll_date", roll_out_of_range_strip_count, false, false), lot_connection))
                        {
                            object res = null;
                            insert_roll_res.Parameters.AddWithValue("@roll_date", DateTime.Now);
                            res = insert_roll_res.ExecuteNonQuery();

                            if ((res == DBNull.Value)
                                || (Convert.ToInt32(res) == 0))
                            {
                                throw new SqlNoValueException("Unsuccessfull insert in SaveLotResult");
                            }
                        }
                    #endregion
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception in SaveLotResult, ex: {0}", ex.InnerException);
                }
                finally
                {
                    lot_connection.Close();
                }
            }
        }

        ///<summary>
        /// Insert values to homogenity_result table
        /// </summary>
        /// <param name="avg"></param>
        /// <param name="stddev"></param>
        /// <param name="cv"></param>
        /// <param name="not_h62_error_count"></param>
        /// <param name="h62_error_count"></param>
        /// <param name="homogenity_is_valid"></param>
        /// <param name="connection_in"></param>
        /// <param name="lot_avg"></param>
        /// <param name="lot_stddev"></param>
        /// <param name="wrong_strip_count"></param>
        /// <param name="outofrange_valid"></param>
        /// <param name="not_h62_is_valid"></param>
        public void SaveHomogenityResult(object avg, object stddev, object cv, object not_h62_error_count,
            object h62_error_count, bool homogenity_is_valid, NpgsqlConnection connection_in, object lot_avg,
            object lot_stddev, int wrong_strip_count, bool outofrange_valid, bool not_h62_is_valid)
        {
            object fk_homo_res_id;
            object result = null;
            Color clr;
            Trace.TraceInformation("SaveHomogenityResult is started");

            if (Users_Form.dev.Homogenity_Is_Valid)
            {
                out_of_range_valid = true;
                not_h62_is_valid = true;
            }

            using (NpgsqlCommand storeHomogenity = new NpgsqlCommand(
                string.Format("insert into homogenity_result(homogenity_is_valid,roll_id,fk_blank_test_identify_id,strip_count_in_one_roll,avg,stddev,cv,not_h62_error_count,h62_errors_count,date,lot_id,out_of_range_strip_count,out_of_range_is_valid,tube_count,not_h62_is_valid,invalidate,remeasured,included) values({0},'{1}',{2},{3},{4:0.00},{5:0.000},{6:0.00},{7},{8},{9},'{10}',{11},{12},{13},{14},{15},{16},{17})",
                Users_Form.dev.Homogenity_Is_Valid/*0*/, Users_Form.dev.Roll/*1*/, Users_Form.dev.Averages_ID/*2*/,
                Users_Form.dev.stripCount/*3*/, Math.Round(Convert.ToDouble(avg), 2)/*4*/, Math.Round(Convert.ToDouble(stddev), 3)/*5*/, Math.Round(Convert.ToDouble(cv), 2)/*6*/,
                not_h62_error_count/*7*/, h62_error_count/*8*/, "@date"/*9*/, Program.LOT_ID/*10*/, Convert.ToInt32(Program.OutOfRangeCount_Homo)/*11*/,
                out_of_range_valid/*12*/, Program.TubeCount/*13*/, not_h62_is_valid/*14*/, false, false, true), connection_in))
            {
                storeHomogenity.Parameters.AddWithValue("@date", DateTime.Now);
                result = storeHomogenity.ExecuteNonQuery();

                if (Convert.ToInt32(result) == 0)
                {
                    Trace.TraceError("Unsuccessfull insert in SaveHomogenityResult(), query: {0}", storeHomogenity.CommandText);
                }
                if (homogenity_is_valid)
                {
                    clr = Color.Green;
                }
                else
                    clr = Color.Red;


                using (NpgsqlCommand homoResultID =
                    new NpgsqlCommand(
                        string.Format("select max(pk_id) from homogenity_result", Users_Form.dev.Roll), connection_in))
                {

                    fk_homo_res_id = homoResultID.ExecuteScalar();

                    if ((fk_homo_res_id == DBNull.Value)
                        || (fk_homo_res_id == null))
                    {
                        throw new SqlNoValueException(string.Format("No value for this query: {0}", homoResultID.CommandText));
                    }
                }
                string hresult = "Nem Megfelelő";

                Trace.TraceInformation("SaveLotResult() started");
                SaveLotResult(lot_avg, lot_stddev, Convert.ToInt32(fk_homo_res_id), avg, cv, stddev);

                Trace.TraceInformation("calculated avg for homogenity test");
                avg = Math.Round(Convert.ToDouble(avg), 2);

                cv = Math.Round(Convert.ToDouble(cv), 2);
                Trace.TraceInformation("Calculate difference");
                double diff = (Convert.ToDouble(lot_avg) / 100) * 5;

                bool h_percent_ok;
                bool cv_ok;

                if ((Convert.ToDouble(avg) <= Convert.ToDouble(lot_avg) + diff)
                    && (Convert.ToDouble(avg) >= Convert.ToDouble(lot_avg) - diff))
                {
                    h_percent_ok = true;
                }
                else
                    h_percent_ok = false;

                if (Convert.ToDouble(cv) < 3.3)
                {
                    cv_ok = true;
                }
                else
                    cv_ok = false;

                if (Users_Form.dev.Homogenity_Is_Valid)
                {
                    out_of_range_valid = true;
                    not_h62_is_valid = true;
                }

                if (cv_ok
                    && h_percent_ok
                    && Users_Form.dev.Homogenity_Is_Valid)
                {
                    hresult = "Megfelelő";
                }
                CreateHomogenityChild(Users_Form.dev.LOT_ID, Users_Form.dev.Roll);




            }
        }

        #region Results View Variables
        public HomogenityResultsView resultsView;
        public static string blood_vial_id;
        private bool outAndStripCountAndError_ok;
        private string lastsn;
        private string lastroll;
        private bool crcNeeded;
        private int AccuracyMeasuredStripCountInARound;

        #endregion

        /// <summary>
        /// Create a results view for homogenity_test
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="roll"></param>
        private void CreateHomogenityChild(string lot, string roll)
        {
            if (Properties.Settings.Default.CommitChangesNeeded)
            {
                if (TestReleaseHostConnection())
                    CommitChangesToReleaseHost(Program.measureType);

                //Drop schema then create a new
                DropAndCreateSchema();
                //Restore schema from sql file
                SchemaRestore();
            }

            resultsView = new HomogenityResultsView(lot, roll); //!!!!
            resultsView.ShowDialog();
        }

        #region check strip value
        /// <summary>
        /// CheckStripValueForHomogenity
        /// </summary>
        /// <param name="glu"></param>
        public void CheckStripValue(double glu)
        {

            if ((5.4 <= (glu / 18.02))
                && ((glu / 18.02) <= 7.3))
            {
                Users_Form.dev.IsResultValid = true;
            }
            else
            {
                Users_Form.dev.IsResultValid = false;
                if ((glu) != 0)
                {
                    Program.OutOfRangeCount_Homo++;
                }

            }
        }

        #endregion


        private string textBoxBarcode2Text = string.Empty;
        private ServiceBase[] ServicesToRun;
        private int networkIsNotAvailable=0;
        public string TextBoxBarcode2Text { get { return textBoxBarcode2Text; } set { textBoxBarcode2Text = value; } }
    }
}





