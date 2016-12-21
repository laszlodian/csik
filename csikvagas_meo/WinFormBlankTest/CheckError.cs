using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WinFormBlankTest
{
  public  class CheckError
    {
      public string LOT {get; set;}
      public string BarcodeFirst {get; set;}

      public static bool H62 { get; set; }
      public static bool NOT_H62 { get; set; }
      public static bool EarlyDripp { get; set; }
      public static bool DeviceReplace { get; set; }

      delegate void SetLotIdDelegate(string barcode1, string barcode2, UserPanel Users_form);
        public void SetLotId(string barcode1, string barcode2, UserPanel Users_form)
        {
            if (Users_form.InvokeRequired)
            {
                Users_form.Invoke(new SetLotIdDelegate(SetLotId), barcode1, barcode2, Users_form);
            }
            else
                foreach (Control item in Users_form.Controls)
                {
                    if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "tbBarcode")
                        {
                            ((TextBox)item).Text = barcode1;
                            SendKeys.Send("{ENTER}");
                           
                        }
                        if (((TextBox)item).Name == "tbBarcode2")
                        {
                            ((TextBox)item).Text = barcode2;
                            SendKeys.Send("{ENTER}");
                        }
                    }
                }
        }
        delegate void SetLabelVisibleDelegate(UserPanel Users_form);
        public void SetLabelVisible(UserPanel Users_form)
        {
            if (Users_form.InvokeRequired)
            {
                Users_form.Invoke(new SetLabelVisibleDelegate(SetLabelVisible),Users_form);
            }
            else
                foreach (Control lb in Users_form.Controls)
                {
                    if (lb is Label)
                    {
                        ((Label)lb).Visible = true;

                    }
                }
        }
        public static void AppendingWhenInvoked(string txt, Color color, Control c)
        {
            int startSelect = ((RichTextBox)c).Text.Length;
           // ((RichTextBox)c).AppendText(txt);
            int endSelect = ((RichTextBox)c).Text.Length;
            ((RichTextBox)c).Select(startSelect, endSelect);
            ((RichTextBox)c).SelectionColor = color;
            ((RichTextBox)c).ScrollToCaret();

        }
        delegate void AppendUserTextDelegate(string value, Panel actForm, Color color);
        public void AppendTextToUser(string txt, Panel myForm, Color color)
        {
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
       
     
      DialogResult answare;
      private string selected_lot_id;
      public CheckError(int error_h,UserPanel Users_form)
      {
         H62 = false;
          NOT_H62= false;
          EarlyDripp= false;

          switch (error_h)
          {
              #region CheckPostError
              case 1:
                  Users_form.dev.Error_H_Text = "Csík kontakthiba";
                  CheckPostError(Users_form);
                  break;
              case 87:
                  Users_form.dev.Error_H_Text = "WE WSE RESISTANCE HIGH";
                  CheckPostError(Users_form);
                  break;
              case 88: 
                  Users_form.dev.Error_H_Text = "CE CSE RESISTANCE HIGH";
                  CheckPostError(Users_form);
                  break;
              case 100:
                  Users_form.dev.Error_H_Text = "DC0 ADC SET DELTA hiba";
                  CheckPostError(Users_form);
                 break;
              case 101:
                 Users_form.dev.Error_H_Text = "DC1 ADC SET DELTA hiba";
                 CheckPostError(Users_form);
                 break;
              case 102:
                 Users_form.dev.Error_H_Text = "DC3 ADC SET DELTA hiba";
                 CheckPostError(Users_form);
                 break;
              case 103:
                 Users_form.dev.Error_H_Text = "AC1 ADC SET DELTA hiba";
                 CheckPostError(Users_form);
                 break;
              case 104:
                 Users_form.dev.Error_H_Text = "AC2 ADC SET DELTA hiba";
                 CheckPostError(Users_form);
                 break;
              case 105:
                 Users_form.dev.Error_H_Text = "A ADC SET DELTA hiba";
                 CheckPostError(Users_form);
                 break;
              #endregion

              case 62: 
                  Users_form.dev.Error_H_Text = "Elegendő fölcseppentett mennyiség time out";
                  CheckCorrectDribble(Users_form);
                  break;
              case 61: 
                  Users_form.dev.Error_H_Text = "Csíkszivárgás cseppentés előtt CE WE";
                  CheckEarlyDribble(Users_form);

                  break;

              #region CheckEarlyDribbleWithDeviceReplace
              case 57: 
                  Users_form.dev.Error_H_Text = "Helytelen tárolás WE CE szivárgás";
                  CheckEarlyDribbleWithDeviceReplace(Users_form);
                  break;
              case 58: 
                  Users_form.dev.Error_H_Text = "Csíkszivárgás SSCE SSWE";
                  CheckEarlyDribbleWithDeviceReplace(Users_form);
                  break;
              case 72: 
                  Users_form.dev.Error_H_Text = "Készülék_szivárgás_WE_SSCE";
                  CheckEarlyDribbleWithDeviceReplace(Users_form);
                  break;
              case 73: 
                  Users_form.dev.Error_H_Text = "Készülék_szivárgás_WE_CE";
                  CheckEarlyDribbleWithDeviceReplace(Users_form);
                  break;
              case 74: 
                  Users_form.dev.Error_H_Text = "Készülék_szivárgás_SSCE_SSWE";
                  CheckEarlyDribbleWithDeviceReplace(Users_form);
                  break;
              case 75: 
                  Users_form.dev.Error_H_Text = "Készülék_szivárgás_CE_SSWE";
                  CheckEarlyDribbleWithDeviceReplace(Users_form);
                  break;

              #endregion

              #region Nem H62-es hibák
              #region Errors before Dribble
              case 82: 
                  Users_form.dev.Error_H_Text = "WE WSE RESISTANCE LOW";
                  ReplaceDevice(Users_form);
                  break;
              case 83: 
                  Users_form.dev.Error_H_Text = "WESENSE SSWE RESISTANCE LOW";
                  ReplaceDevice(Users_form);                  
                  break;
              case 84: 
                  Users_form.dev.Error_H_Text = "CE CSE RESISTANCE LOW";
                  ReplaceDevice(Users_form);
                  break;
              #endregion

              #region Errors after Dribble
              case 33: 
                  Users_form.dev.Error_H_Text = "NORMALIZED COTTRELL HIGH DC BLOCK 1";
                  Program.HomogenityMeasuredStripCount++;
                  NOT_H62 = true;
                  Users_form.dev.NotH62_Error++;
                  CleanStrip(Users_form);
                  break;

              case 34: 
                  Users_form.dev.Error_H_Text = "NORMALIZED COTTRELL LOW DC BLOCK 1";
                  Program.HomogenityMeasuredStripCount++;
                  NOT_H62 = true;
                  Users_form.dev.NotH62_Error++;
                  CleanStrip(Users_form);
                  break;
              case 37: 
                  Users_form.dev.Error_H_Text = "CONSISTENCY FAILSAFE 1 csíkhiba";
                  Program.HomogenityMeasuredStripCount++;
                  NOT_H62 = true;
                  Users_form.dev.NotH62_Error++;
                  CleanStrip(Users_form);
                  break;
              case 38: 
                  Users_form.dev.Error_H_Text = "CONSISTENCY FAILSAFE 2 csíkhiba";
                  Program.HomogenityMeasuredStripCount++;
                  NOT_H62 = true;
                  Users_form.dev.NotH62_Error++;
                  CleanStrip(Users_form);
                  break;

              case 39: 
                  Users_form.dev.Error_H_Text = "CONSISTENCY FAILSAFE 3 csíkhiba";
                  NOT_H62 = true;
                  Program.HomogenityMeasuredStripCount++;
                  Users_form.dev.NotH62_Error++;
                  CleanStrip(Users_form);
                  break;
              case 40: 
                  Users_form.dev.Error_H_Text = "CONSISTENCY FAILSAFE 4 csíkhiba";
                  NOT_H62 = true;
                  Program.HomogenityMeasuredStripCount++;
                  Users_form.dev.NotH62_Error++;
                  CleanStrip(Users_form);
                  break;
              case 41: 
                  Users_form.dev.Error_H_Text = "CONSISTENCY FAILSAFE 5 csíkhiba";
                  NOT_H62 = true;
                  Program.HomogenityMeasuredStripCount++;
                  Users_form.dev.NotH62_Error++;
                  CleanStrip(Users_form);
                  break;
              case 42: 
                  Users_form.dev.Error_H_Text = "DC CURENT RANGE HIGH DC BLOCK 1";
                  NOT_H62 = true;
                  Program.HomogenityMeasuredStripCount++;
                  Users_form.dev.NotH62_Error++;
                  CleanStrip(Users_form);
                  break;
              case 43: 
                  Users_form.dev.Error_H_Text = "DC CURENT RANGE LOW DC BLOCK 1";
                  NOT_H62 = true;
                  Program.HomogenityMeasuredStripCount++;
                  Users_form.dev.NotH62_Error++;
                  CleanStrip(Users_form);
                  break;
              case 46: 
                  Users_form.dev.Error_H_Text = "ADMITTANCE RANGE HIGH";
                  NOT_H62 = true;
                  Program.HomogenityMeasuredStripCount++;
                  Users_form.dev.NotH62_Error++;
                  CleanStrip(Users_form);
                  break;
              case 47: 
                  Users_form.dev.Error_H_Text = "ADMITTANCE RANGE LOW";
                  NOT_H62 = true;
                  Program.HomogenityMeasuredStripCount++;
                  Users_form.dev.NotH62_Error++;
                  CleanStrip(Users_form);
                  break;
              case 48: 
                  Users_form.dev.Error_H_Text = "PHASE RANGE HIGH";
                  NOT_H62 = true;
                  Program.HomogenityMeasuredStripCount++;
                  Users_form.dev.NotH62_Error++;
                  CleanStrip(Users_form);
                  break;
              case 49: 
                  Users_form.dev.Error_H_Text = "PHASE RANGE LOW";
                  Program.HomogenityMeasuredStripCount++;
                  NOT_H62 = true;
                  Users_form.dev.NotH62_Error++;
                  CleanStrip(Users_form);
                  break;
              #endregion

              #endregion


              default: Users_form.dev.Error_H_Text = "Unknow error";
                  break;
          }
      }

      #region Methods
      private void CleanStrip(UserPanel Users_form)
      {
          //AppendTextToUser("Törölje le a cseppet a tesztcsíkról nedvszívó anyaggal, majd tegye félre a csíkot további vizsgálatra",Users_form,Color.Red);
        //  AppendTextToUser("\n\n***Az eredmény a H62 hibák közé lesz sorolva***\n\n", Users_form, Color.DarkRed);

      }

      private void CheckCorrectDribble(UserPanel Users_form)
      {
          answare = MessageBox.Show("Megfelelően cseppentett?", Users_form.dev.Error_H_Text, MessageBoxButtons.YesNo);

          if (answare == DialogResult.Yes)
          {
              Program.HomogenityMeasuredStripCount++;
              H62 = true;
              //AppendTextToUser("\n\n***Az eredmény a H62-es hibák közé lesz sorolva***\n\n", Users_form, Color.DarkRed);
              Users_form.dev.H62_Error++;
              EarlyDripp = false;
          }
          else
          {

              H62 = false;
              EarlyDripp = true;
           //   AppendTextToUser(string.Format("Kezdje újra a mérést a {1} számú LOT-ból {0}",
              //                  Environment.NewLine, selected_lot_id), Users_form, Color.Red);

          }
      }

      private void CheckEarlyDribble(UserPanel Users_form)
      {
          answare = MessageBox.Show("Korábban cseppentett?", Users_form.dev.Error_H_Text, MessageBoxButtons.YesNo);

          if (answare == DialogResult.No)
          {
              Program.HomogenityMeasuredStripCount++;
              NOT_H62 = true;
            //  AppendTextToUser("\n\n***Az eredmény a Nem H62 hibák közé lesz sorolva***\n\n", Users_form, Color.DarkRed);
              Users_form.dev.NotH62_Error++;
             EarlyDripp = false;
          }
          else
          {
             
              NOT_H62 = false;
            EarlyDripp = true;
              //AppendTextToUser(string.Format("Kezdje újra a mérést a {1} számú LOT-ból {0}", 
                            //    Environment.NewLine, selected_lot_id), Users_form, Color.Red);
            
          }
      }

      private void CheckEarlyDribbleWithDeviceReplace(UserPanel Users_form)
      {
          answare = MessageBox.Show("Korábban cseppentett?", Users_form.dev.Error_H_Text, MessageBoxButtons.YesNo);

          if (answare == DialogResult.No)
          {
             EarlyDripp= false;
              ReplaceDevice(Users_form);
          }
          else
          {
              
              EarlyDripp = true;
              //AppendTextToUser(string.Format("Kezdje újra a mérést a {1} számú LOT-ból{0}",
                         //                     Environment.NewLine, selected_lot_id), Users_form, Color.Red);
            
          }

      }

      private void ReplaceDevice(UserPanel Users_form)
      {
         
          
          if (Users_form.dev.PostDeviceReplace)
          {
              CheckPostErrorAtDeviceReplace(Users_form);
              Users_form.dev.PostDeviceReplace = false;
             
          }
          else
          {
          //    AppendTextToUser(string.Format("\n Cserélje ki a készüléket, majd kezdje újra a mérést a {0} számú LOT-ból", selected_lot_id), Users_form, Color.Red);
              Users_form.dev.PostDeviceReplace = true;
              DeviceReplace = true;
          }
      }
      private void CheckPostErrorAtDeviceReplace(UserPanel Users_form)
      {
             Program.HomogenityMeasuredStripCount++;
              NOT_H62 = true;
           //   AppendTextToUser("\n\n***Az eredmény a Nem H62 hibák közé lesz sorolva***\n\n", Users_form, Color.DarkRed);
              Users_form.dev.NotH62_Error++;
              Users_form.dev.PostErrorAtDeviceReplace = false;

         
      }
      private void CheckPostError(UserPanel Users_form)
      {
          if (Users_form.dev.PostError)
          {
              Program.HomogenityMeasuredStripCount++;

              NOT_H62 = true;
              Users_form.dev.NotH62_Error++;
              Users_form.dev.PostError = false;

          //    AppendTextToUser("\n\n***Az eredmény a Nem H62 hibák közé lesz sorolva***\n\n", Users_form, Color.DarkRed);
          }
          else
          {
              NOT_H62 = false;
           //   AppendTextToUser("****Vegye ki a csíkot, majd mérjen egyet ugyanabból a tubusból*****", Users_form, Color.DarkRed);
              Users_form.dev.PostError = true;            
          }
      }
      #endregion
    }
}
