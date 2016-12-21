using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WinFormBlankTest.UI.Forms
{
   public class ShowAllData
    {
       public static object act_lot=string.Empty;
       public static object act_roll = string.Empty;
       public static string act_sn = string.Empty;
       public static string act_user = string.Empty;
       public static string act_comp = string.Empty;
       public static string act_error = string.Empty;
       public static string act_error_text = string.Empty;
       public static double act_glu = 0;
       public static double act_nano_amper = 0;
       public static bool act_not_h62 = false;
       public static bool act_h62 = false;
       public static DateTime act_start;
       public static DateTime act_end;



       /// <summary>
       /// SHOW every measurement from a roll which has homogenity test
       /// </summary>
       /// <param name="lot"></param>
       /// <param name="roll"></param>
       /// <param name="sn"></param>
       /// <param name="user"></param>
       /// <param name="comp"></param>
       /// <param name="error"></param>
       /// <param name="error_text"></param>
       /// <param name="glu"></param>
       /// <param name="nano_amper"></param>
       /// <param name="not_h62"></param>
       /// <param name="h62"></param>
       /// <param name="start"></param>
       /// <param name="end"></param>
       /// Alltogether is 12 arguments/parameter
       public ShowAllData(string lot, string roll, string sn, string user, string comp, string error, string error_text,
           double glu, bool not_h62, bool h62, DateTime meas_date) 
       {

           Trace.TraceInformation("ShowAllData() started");

           act_lot = lot;
           act_roll = roll;

           act_sn = sn;
           act_user = user;
           act_comp = comp;

           act_error = error;
           act_error_text = error_text;

           act_glu = glu;
         //  act_nano_amper = nano_amper;

           act_not_h62 = not_h62;
           act_h62 = h62;
           act_start = meas_date;
           

       }



       /// <summary>
       /// Show every measure from a roll which is selected from BLANK TESTS
       /// </summary>
       /// <param name="lot"></param>
       /// <param name="roll"></param>
       /// <param name="sn"></param>
       /// <param name="user"></param>
       /// <param name="comp"></param>
       /// <param name="error"></param>
       /// <param name="error_text"></param>
       /// <param name="glu"></param>
       /// <param name="nano_amper"></param>
       /// <param name="not_h62"></param>
       /// <param name="h62"></param>
       /// <param name="start"></param>
       /// <param name="end"></param>
       public ShowAllData(object lot, object roll, string sn, string user, string comp,
             double glu, double nano_amper, DateTime start, DateTime end) 
       {

           act_lot = lot;
           act_roll = roll;
           act_sn = sn;
           act_user = user;
           act_comp = comp;
           act_glu = glu;
           act_nano_amper = nano_amper;
           act_start = start;
           act_end = end;


       }


           #region Properties
        
    public string LOT
    {
        get{return act_lot.ToString();}
        
        set
        {
            act_lot = value;
        }
    
    }

    public string ROLL
    {
        get { return act_roll.ToString(); }

        set
        {
            act_roll = value;
        }

    }

    public string SN
    {
        get { return act_sn; }

        set
        {
            act_sn = value;
        }

    }

    public string User
    {
        get { return act_user; }

        set
        {
            act_user = value;
        }

    }

    public string Computer
    {
        get { return act_comp; }

        set
        {
            act_comp = value;
        }

    }



    public string Error
    {
        get {
                if ((act_error==string.Empty) || (act_error == "''"))
                {
                    act_error = "Nem történt";
                }
                return act_error; 
            }

        set
        {
            act_error = value;
        }

    }

    
    public string ErrorText
    {
        get {
                if (act_error_text.Equals(string.Empty))
                {
                    act_error = "Nem történt";
                } 
                return act_error_text;
            }

        set
        {
            act_error_text = value;
        }

    }

    public string not_h62_result;
    public string Not_H62
    {
        get {
                if (!act_not_h62)
                {
                    not_h62_result = "Nem történt";
                }else
                    not_h62_result = "Történt";
            
                 return not_h62_result; }

        set
        {
            act_not_h62 = Convert.ToBoolean(value);
        }

    }
    public string h62_result;
    public string H62
    {
        get 
        {
            if (!act_not_h62)
            {
                h62_result = "Nem történt";
            }
            else
                h62_result = "Történt";
            
            return h62_result; 
        }

        set
        {
            act_h62 = Convert.ToBoolean(value);
        }

    }

    public double Glu
    {
        get { return act_glu; }

        set
        {
            act_glu = value;
        }

    }

    public double NanoAmper
    {
        get { return Convert.ToInt32(act_nano_amper); }

        set
        {
            act_nano_amper = value;
        }

    }

    public DateTime StartTime
    {
        get { return act_start; }

        set
        {
            act_start = value;
        }

    }

    public DateTime EndTime
    {
        get { return act_end; }

        set
        {
            act_end = value;
        }

    }
        #endregion



            public static void ClearAllProperty()
            {
                act_end = new DateTime();
                act_start = new DateTime();
                act_nano_amper = new double();
                act_glu = new double();
                act_not_h62 = new bool();
                act_h62 = new bool();
                act_error = string.Empty;
                act_error_text = string.Empty;
                act_comp = string.Empty;
                act_user = string.Empty;
                act_sn = string.Empty;
                act_roll = string.Empty;
                act_lot = string.Empty;

            }
    }


    
}
