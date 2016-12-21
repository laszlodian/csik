using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace WinFormBlankTest
{
   public class BlankTest
    { 
        private string date;
        private string cv;
        private string avg;
        private string result;
        private string rollid;
        private string lotid;
        private string outofrange;

        private Button setButtonToGrid=new Button();

        private string h62_count;
        private string not_h62_count;

       private string measure_type;

       public double all_blank_avg;
       public double all_blank_cv;
       public DateTime all_blank_date;
       public int all_blank_tubecount;
       public bool all_blank_valid;

       public double glucose;
       public double n_amper;
       public int serial;
       public string bar;
       public bool remeasure;

       public double homo_avg;
       public double homo_cv;
       public bool homo_avg_ok;
       public bool homo_cv_ok;
       public int homo_not_h62;
       public int homo_h62;
       public int homo_outofrange;
       public bool homo_out_of_valid;


       public string after_homo_avg;
       public string after_homo_cv;
       public string after_homo_avg_ok;
       public string after_homo_cv_ok;
       public string after_homo_outofrange;

       public bool homo_h62_ok;
       public bool homo_not_h62_ok;
       public string after_homo_h62;
       public string after_homo_not_h62;
       public string after_homo_out_of_valid;
       public object homo_bt;



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
       public BlankTest(string lot, string roll, string sn, string user, string comp, string error, string error_text,
           double glu, bool not_h62, bool h62, DateTime start,DateTime end)
       {

           Trace.TraceInformation("BlankTest() started");

           act_lot = lot;
           act_roll = roll;

           act_sn = sn;
           act_user = user;
           act_comp = comp;

           act_error = error;
           act_error_text = error_text;

           act_glu = glu;
           //act_nano_amper = nano_amper;

           act_not_h62 = not_h62;
           act_h62 = h62;
           act_start = start;
           act_end = end;

       }

       /// <summary>
       ///  After store homogenity result(string)
       /// </summary>
       /// <param name="lotid"></param>
       /// <param name="rollid"></param>
       /// <param name="result"></param>
       /// <param name="homogenity_avg"></param>
       /// <param name="avg_ok"></param>
       /// <param name="homogenity_cv"></param>
       /// <param name="cv_ok"></param>
       /// <param name="meastype"></param>
       /// <param name="wrong_strip"></param>
       /// <param name="homogenity_result"></param>
       /// <param name="h62"></param>
       /// <param name="h62_ok"></param>
       /// <param name="not_h62"></param>
       /// <param name="not_h62_ok"></param>
       /// <param name="btRemeasure"></param>
       /// LotID, rollid, result, avg,avg_ok, cv,cv_ok, date,out_of_range_strip_count,strip_count,h62,not_h62
       public BlankTest(string lot_id, string roll_id, string res, string homogenity_avg, string avg_ok, string homogenity_cv, string cv_ok,
                          string actdate, string out_strip,string out_strip_ok, string srip_count, string h62,  string not_h62)
       {
           lotid = lot_id;
           rollid = roll_id;
           measure_type = "Homogenity Check";

           date = actdate;
           result = res;
           after_homo_avg = homogenity_avg;
          after_homo_cv = homogenity_cv;
          after_homo_cv_ok = cv_ok;
          after_homo_avg_ok = avg_ok;
          after_homo_outofrange = out_strip;
          after_homo_out_of_valid = out_strip_ok;

          after_homo_h62 = h62;
          after_homo_not_h62 = not_h62;

        

       }
       #region Properties


       public string LOT
       {
           get
           {
               return act_lot;
           }

           set
           {
               act_lot = value;
           }
       }

       public string ROLL
       {
           get
           {
               return act_roll;
           }

           set
           {
               act_roll= value;
           }
       }

       public string SerialN
       {
           get
           {
               return act_sn;
           }

           set
           {
               act_sn= value;
           }
       }

       public string Computer
       {
           get
           {
               return act_comp;
           }

           set
           {
               act_comp = value;
           }
       }

       public string User
       {
           get
           {
               return act_user;
           }

           set
           {
               act_user= value;
           }
       }
       public string Error
       {
           get
           {
               return act_error;
           }

           set
           {
               act_error= value;
           }
       }

       public string ErrorText
       {

           get
           {
               return act_error_text;
           }

           set
           {
               act_error_text = value;
           }
       }
       public bool H62
       {
           get
           {
               return act_h62;
           }

           set
           {
               act_h62 = value;
           }
       }

       public bool Not_H62
       {
           get
           {
               return act_not_h62;
           }

           set
           {
               act_not_h62 = value;
           }
       }
       public double Glucose
       {
           get
           {
               return act_glu;
           }

           set
           {
               act_glu = value;
           }
       }
       public DateTime StartTime
       {
           get
           {
               return act_start;
           }

           set
           {
               act_start = value;
           }
       }

       public DateTime EndTime
       {
           get
           {
               return act_end;
           }

           set
           {
               act_end = value;
           }
       }

       /// <summary>
       /// After Homogenity finished Not_H62 error count String
       /// </summary>
       public string After_H_NotH62
       {
           get
           {
               return after_homo_not_h62;
           }

           set
           {
               after_homo_not_h62 = value;
           }

       }

       /// <summary>
       /// After Homogenity finished H62 error count String
       /// </summary>
       public string After_H_H62
       {
           get
           {
               return after_homo_h62;
           }

           set
           {
               after_homo_h62 = value;
           }

       }

       /// <summary>
       /// After Homogenity finished OutOfRange strip count String
       /// </summary>
       public string After_H_OutOfRange
       {
           get
           {
               return Program.OutOfRangeCount_Homo.ToString();
           }

           set
           {
               after_homo_outofrange = Program.OutOfRangeCount_Homo.ToString();
           }

       }

       /// <summary>
       /// After Homogenity finished AVG string
       /// </summary>
       public string After_H_AVG
       {
           get
           {
            return after_homo_avg; 
           }
           set
           {
               after_homo_avg=value;
           }

       }

       /// <summary>
       /// After Homogenity finished MeasureType string
       /// </summary>
       public string After_H_MeasureType
       {
           get
           {
               return "Homogenity Check";
           }
           set
           {
               after_homo_avg = "Homogenity Check";
           }

       }
       /// <summary>
       /// After Homogenity finished  AVG ok
       /// </summary>
       public string After_H_AVG_ok
       {
           get
           {
               if (Convert.ToBoolean(after_homo_avg_ok))
               {
                   return "Megfelelő";
               }
               else
                   return "Nem Megfelelő";

              
           }
           set
           {
               after_homo_avg_ok = value;
           }

       }
       
       public string After_H_OutOfRange_OK
       {
           get
           {

               return after_homo_out_of_valid;
           }
           set
           {

               after_homo_out_of_valid = value;
           }
       }
       /// <summary>
       /// After CV ok
       /// </summary>
       public string After_H_CV_ok
       {
           get
           {
               if (Convert.ToBoolean(after_homo_cv_ok))
               {
                   return "Megfelelő";
               }
               else
                   return "Nem Megfelelő";
               
           }
           set
           {
               after_homo_cv_ok = value;
           }

       }

       /// <summary>
       /// After CV 
       /// </summary>
       public string After_H_CV
       {
           get
           {
               return after_homo_cv;
           }
           set
           {
               after_homo_cv = value;
           }

       }
       

       public double Homo_avg
       {
           get { return homo_avg; }

           set { homo_avg = value; }
       }
       public double Homo_cv
       {
           get { return homo_cv; }

           set { homo_cv = value; }
       }
       public bool Homo_avg_ok
       {
           get { return homo_avg_ok; }

           set { homo_avg_ok = value; }
       }
       public bool Homo_cv_ok
       {
           get { return homo_cv_ok; }

           set { homo_cv_ok = value; }
       }
       public int Homo_h62
       {
           get { return homo_h62; }

           set { homo_h62 = value; }
       }
       public int Homo_not_h62
       {
           get { return homo_not_h62; }

           set { homo_not_h62 = value; }

       }
       public bool Homo_h62_ok
       {
           get { return homo_h62_ok; }

           set { homo_h62_ok = value; }
       }
       public bool Homo_not_h62_ok
       {
           get { return homo_h62_ok; }

           set { homo_h62_ok = value; }

       }
       public int Out_of_range
       {
           get { return homo_outofrange; }

           set { homo_outofrange= value; }
       }
       public bool Out_of_range_valid
       {
           get { return homo_out_of_valid; }

           set { homo_out_of_valid= value; }
       }



       #endregion














       /// <summary>
       /// When /show started then one row clicked to display properties
       /// </summary>
       /// <param name="lot"></param>
       /// <param name="roll"></param>
       /// <param name="meas"></param>
       /// <param name="glu"></param>
       /// <param name="nano"></param>
       /// <param name="sn"></param>
       /// <param name="barcode"></param>
       /// <param name="remeas"></param>
       public BlankTest(string lot,string roll,string meas,double glu,double nano,int sn, string barcode,bool remeas)
       {

           lotid = lot;
           rollid = roll;

           measure_type = meas;
           glucose=glu;
           n_amper = nano;
           serial= sn;
           bar = barcode;
           remeasure = remeas;
       }

       /// <summary>
       /// In case of Blank test results show
       /// </summary>
       /// <param name="lot"></param>
       /// <param name="roll"></param>
       /// <param name="averages"></param>
       /// <param name="ceve"></param>
       /// <param name="test_date"></param>
       /// <param name="tube_count"></param>
       /// <param name="blank_is_valid"></param>
       /// <param name="res"></param>
       public BlankTest(string lot, string roll, double averages, double ceve, DateTime test_date,int tube_count,bool blank_is_valid,string res)
       {
           lotid = lot;
           rollid = roll;
          
           all_blank_avg = averages;
           all_blank_cv = ceve;
           all_blank_date = test_date;
           all_blank_tubecount = tube_count;
           all_blank_valid = blank_is_valid;

           result = res;
       }

       /// <summary>
       /// after homogenity stored
       /// </summary>
       /// <param name="lot"></param>
       /// <param name="roll"></param>
       /// <param name="res"></param>
       /// <param name="averages"></param>
       /// <param name="ceve"></param>
       /// <param name="test_date"></param>
       public BlankTest(string lot, string roll, string res, string averages, string ceve, string test_date)
        {
            try
            {
                
            Trace.TraceError("blanktest started 90 row");
            lotid = lot;
            rollid = roll;
            result = res;
            avg = averages;
            cv = ceve;
            date = test_date;
            }
            catch (Exception ex)
            {
                Trace.TraceError("exception in blanktest row 114: {0}",ex.StackTrace);
                throw;
            }
            
        }


       public bool after_blank_avg_ok;
       /// <summary>
       /// after blank stored
       /// </summary>
       /// <param name="lot"></param>
       /// <param name="roll"></param>
       /// <param name="res"></param>
       /// <param name="averages"></param>
       /// <param name="ceve"></param>
       /// <param name="test_date"></param>
       public BlankTest(string lot, string roll, string res, string averages, bool avg_ok,double blank_cv,string measure,DateTime dt)
       {
           try
           {

               Trace.TraceError("blanktest started 90 row");

               lotid = lot;
               rollid = roll;
               result = res;
               avg = averages;
               cv = blank_cv.ToString();
               after_blank_avg_ok = avg_ok;
               measure_type = measure;
               date = dt.ToString();
           }
           catch (Exception ex)
           {
               Trace.TraceError("exception in blanktest row 114: {0}", ex.StackTrace);
               throw;
           }

       }

       /// <summary>
       /// In case of /show to display test results and option to remeasure
       /// </summary>
       /// <param name="lot"></param>
       /// <param name="roll"></param>
       /// <param name="res"></param>
       /// <param name="averages"></param>
       /// <param name="ceve"></param>
       /// <param name="test_date"></param>
       /// <param name="meas"></param>
       /// <param name="wrong_strip_count"></param>
       /// <param name="h62"></param>
       /// <param name="not_h62"></param>
       /// <param name="setButton"></param>
       public BlankTest(string lot, string roll, string res, string averages, string ceve,
           string test_date,string meas,string wrong_strip_count,string h62,string not_h62,bool not_h62_ok)
       {

           try
           {
               Trace.TraceInformation("blanktest constructor started{0}", this);
               lotid = lot;
               rollid = roll;
               result = res;
               avg = averages;
               cv = ceve;
               date = test_date;

               measure_type = meas;

               outofrange = wrong_strip_count;
               h62_count = h62;
               not_h62_count = not_h62;

               noth62isok = not_h62_ok;
              

           }catch(Exception e)
           {
               Trace.TraceError("exc in blanktest: {0},{1},{2}",e.Data,e.Source,e.StackTrace);
           }
           Trace.TraceInformation("set values to an other variable finished{0}", this);
       }





       public bool NotH62Valid
       {

           get
           {
               return noth62isok;
           }

           set
           {
               noth62isok = value;
           }
       }

       public bool OutOk
       {
           get
           {
               return wrongsok;
           }

           set
           {
               wrongsok = value;
           }
       }
       public bool After_blank_ok
       {
           get
           {
               return after_blank_avg_ok;
           }

           set
           {
               after_blank_avg_ok = value;
           }
       }
       double lot_avg;
       public double Lot_Averages
       {
           get
           {
               return lot_avg;
           }
           set
           {
               lot_avg = value;
           }
       }
       double lot_cv;
       public double Lot_CV
       {
           get
           {
               return lot_cv;
           }
           set
           {
               lot_cv = value;
           }
       }

       double roll_avg;
       public double Roll_Averages
       {
           get
           {
               return roll_avg;
           }
           set
           {
               roll_avg = value;
           }
       }
       double roll_cv;
       public double Roll_CV
       {
           get
           {
               return roll_cv;
           }
           set
           {
               roll_cv = value;
           }
       }


        public string MeasureType
        {
            get
            {
                Trace.TraceInformation("Get measure_type property");
                return measure_type;
            }
            set
            {
                measure_type = value;
            }
        }

        public double Glu
        {
            get
            {
                Trace.TraceInformation("Get blankcv property");
                return glucose;
            }
            set { glucose = value; }
        }

        public double NanoA
        {
            get
            {
                Trace.TraceInformation("Get blankcv property");
                return n_amper;
            }
            set { n_amper= value; }
        }

        public int SN
        {
            get
            {
                Trace.TraceInformation("Get cv property");
                return serial;
            }
            set
            {
                serial= value;
            }
        }
        public string Barcode
        {
            get
            {
                Trace.TraceInformation("Get cv property");
                return bar;
            }
            set
            {
                bar = value;
            }
        }

        public bool Remeasured
        {
            get
            {
                Trace.TraceInformation("Get cv property");
                return remeasure;
            }
            set
            {
                remeasure = value;
            }
        }


        public string CV
        {
            get
            {
                Trace.TraceInformation("Get cv property");
                return Math.Round(Convert.ToDouble(cv),2).ToString();
            }
            set
            {
                cv= value;
            }
        }

        public string Date
        {
            get
            {
                Trace.TraceInformation("Get date property");
                return date;
            }
            set
            {
                date = value;
            }
        }
        public double BlankAVG
        {
            get {
                Trace.TraceInformation("Get blankavg property");
                return all_blank_avg; }
            set { all_blank_avg = value; }
        }
       
        public double BlankCV
        {
            get {
                Trace.TraceInformation("Get blankcv property"); 
                return all_blank_cv; }
            set { all_blank_cv = value; }
        }
        public bool BlankValid
        {
            get
            {
                return all_blank_valid;
            }
            set
            {
                all_blank_valid = value;
            }
        }
        public int BlankTubeCount
        {
            get
            {
                return all_blank_tubecount;
            }
            set { all_blank_tubecount = value; }
        }
        public DateTime BlankDate
        {
            get { return all_blank_date; }
            set { all_blank_date = value; }
        }
        public string Averages
        {
            get
            {
                Trace.TraceInformation("Get averages property");
                return Math.Round(Convert.ToDouble(avg), 2).ToString(); 
            }
            set
            {
                avg= value;
            }
        }

        public string Result
        {
            get
            {
                Trace.TraceInformation("Get result property");
                return result;
            }
            set
            {
                result = value;
            }
        }

        public string RollID
        {
            get
            {
                Trace.TraceInformation("Get rollid property");
                return rollid;
            }
            set
            {
                rollid= value;
            }
        }

        public string LotID
        {
            get
            {
                Trace.TraceInformation("Get lotid property");
                return lotid;
            }
            set
            {
                lotid = value;
            }
        }

        public string OutOfRange
        {
            get
            {
                return outofrange;
            }
            set
            {
                outofrange = value;
            }
        }

        public string H62_count
        {
            get
            {
                return h62_count;
            }
            set
            {
                h62_count = value;
            }

        }
        public Button btRemeasure;
        private bool wrongsok;
        private bool noth62isok;
        private string act_lot;
        private string act_roll;
        private string act_sn;
        private string act_user;
        private string act_comp;
        private string act_error;
        private string act_error_text;
        private double act_glu;
        private bool act_not_h62;
        private bool act_h62;
        private DateTime act_start;
        private DateTime act_end;
      
       
       
       
       public Button Remeasure
        {
            get { return setButtonToGrid; }

            set { setButtonToGrid = value; }
        }
        public string Not_H62_count
        {
            get
            {
                return not_h62_count;
            }
            set
            {
                not_h62_count = value;
            }

        }
    }
    
}

