using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using WinFormBlankTest.UI.Forms;
using System.Drawing;

namespace WinFormBlankTest
{
  public class HomogenityTest
  {
      #region Variables

        private DateTime date;

        private DateTime roll_date;

        private string result;
        private string rollid;
        private string lotid;

        public bool roll_ok;
        private bool homogenity_ok;
        private bool blank_ok;        

        public int stripcount;
        public bool stripcount_is_valid;

        public int tubecount;
        public int lot_out_of_range_stripcount;

        public int lot_strip_count;
        public bool lot_strip_count_is_valid;

        public int out_of_range_stripcount;
        public bool out_of_range_stripcount_is_valid;

        public bool lot_valid;
        public bool roll_is_valid;

        public double lot_avg;
        public bool lot_avg_is_valid;
        public string lot_avg_ok;
        public string lot_cv_ok;
        public double lot_cv;
        public bool lot_cv_is_valid;

        public double h_avg;
        public double h_cv;

        public double roll_avg;
        public bool roll_avg_is_valid;

        public double roll_cv;
        public bool roll_cv_is_valid;

        public bool lot_ok;
        public int h62_error;
        public bool h62_is_valid;

        public int not_h62_error;
        public bool not_h62_is_valid;
      #endregion

      /// <summary>
      /// LOT Results
      /// </summary>
      /// <param name="lot"></param>
      /// <param name="lot_averages"></param>
      /// <param name="lot_averages_is_valid"></param>
      /// <param name="lot_ceve"></param>
      /// <param name="lot_ceve_is_valid"></param>
      /// <param name="test_date"></param>
      /// <param name="stripCount"></param>
      /// <param name="stripCount_is_valid"></param>
      /// <param name="lot_is_valid"></param>
      /// <param name="h62_error_count"></param>
      /// <param name="not_h62_error_count"></param>
        public HomogenityTest(string lot,double lot_averages,bool lot_averages_is_valid,
             double lot_ceve,bool lot_ceve_is_valid,
             DateTime test_date,int out_of_range_count,
             int stripCount,bool stripCount_is_valid,
             bool lot_is_valid,
             int h62_error_count,int not_h62_error_count)
          {
              lotid = lot;
              lot_avg = lot_averages;
              lot_avg_is_valid = lot_averages_is_valid;
              lot_cv = lot_ceve;
              lot_cv_is_valid = lot_ceve_is_valid;
              lot_out_of_range_stripcount = out_of_range_count;
              lot_valid = lot_is_valid;

              if (lot_valid)
              {
                  result = "Megfelelő";
              }
              else
                  result = "Nem megfelelő";

              stripcount = stripCount;
              lot_strip_count_is_valid = stripCount_is_valid;
              
              date = test_date;

              h62_error = h62_error_count;
              not_h62_error = not_h62_error_count;          

          }

            /// <summary>
            /// Homogenity Results
            /// </summary>
            /// <param name="lot"></param>
            /// <param name="act_roll"></param>
            /// <param name="averages"></param>
            /// <param name="ceve"></param>
            /// <param name="test_date"></param>
            /// <param name="tube_count"></param>
            /// <param name="h62"></param>
            /// <param name="not_h62"></param>
            /// <param name="out_of_range_strip_count"></param>
            /// <param name="stddev"></param>
            /// <param name="is_valid"></param>
        public HomogenityTest(string lot,
                        string r,string roll,
                      double averages, double ceve,
                     DateTime test_date, int tube_count,
            int stripC,int h62,int not_h62,
                      int out_of_range_strip_count,           
                         double stddev,
                       bool is_valid)
            
        {
            lotid = lot;
           rollid = roll;

            h_avg = averages;            
            h_cv = ceve;

            tubecount = tube_count;
            stripcount = stripC;

            out_of_range_stripcount = out_of_range_strip_count;
            h62_error =h62;
            not_h62_error= not_h62;

            out_of_range_stripcount = out_of_range_strip_count;

            result = r;

            date = test_date;
            homogenity_ok = is_valid;
            

        }
      /// <summary>
      /// Roll Result
      /// </summary>
      /// <param name="lot"></param>
      /// <param name="roll"></param>
      /// <param name="roll_averages"></param>
      /// <param name="roll_averages_is_valid"></param>
      /// <param name="roll_ceve"></param>
      /// <param name="roll_ceve_is_valid"></param>
      /// <param name="test_date"></param>
      /// <param name="stripCount"></param>
      /// <param name="stripCount_is_valid"></param>
      /// <param name="outofrange_stripcount"></param>
      /// <param name="roll_is_valid"></param>
        public HomogenityTest(string lot, string roll, 
            double roll_averages, bool roll_averages_is_valid, 
            double roll_ceve, bool roll_ceve_is_valid ,
            DateTime test_date, 
            int stripCount, 
            int outofrange_stripcount,
            bool roll_is_valid/* int roll_h62count,int roll_noth62count*/)
        {

            rollid = roll;
            lotid=lot;

            roll_avg = roll_averages;
            roll_avg_is_valid = roll_averages_is_valid;
            roll_cv = roll_ceve;
            roll_cv_is_valid = roll_ceve_is_valid;

            roll_ok=roll_is_valid;

            if (roll_ok)
            {
                result = "Megfelelő";
            }
            else
                result = "Nem megfelelő";

            stripcount = stripCount;
         
            out_of_range_stripcount = outofrange_stripcount;
        
            roll_date = test_date;
           /* roll_h62 = roll_h62count;
            roll_noth62 = roll_noth62count;*/

        }
      /// <summary>
      /// Roll presentation
      /// </summary>
      /// <param name="lot"></param>
      /// <param name="roll"></param>
      /// <param name="lot_averages"></param>
        public HomogenityTest(string lot, string roll, double lot_averages,
            bool lot_averages_is_valid,string lot_avg_res,
            double roll_averages,bool roll_averages_is_valid,string roll_avg_res,double lot_ceve, bool lot_ceve_is_valid,
            string lot_cv_res,double roll_ceve,bool roll_ceve_is_valid,string roll_cv_res, DateTime test_date,
            int stripCount,bool stripCount_valid,int outofrange_stripcount,bool outofrange_is_valid,string out_valid_res,
            bool lot_is_valid,string lot_ok_res,bool roll_is_valid,string roll_ok_res,bool homogenity_is_valid,string homo_ok_res,bool blank_is_valid,string blank_ok_res,
            int h62_error_count,int not_h62_error_count)
        {
            lotid = lot;
            rollid = roll;            
            
            lot_avg = lot_averages;
            lot_avg_is_valid=lot_averages_is_valid;
            lot_cv = lot_ceve;
            lot_cv_is_valid=lot_ceve_is_valid;

            roll_avg=roll_averages;
            roll_avg_is_valid=roll_averages_is_valid;
            roll_cv = roll_ceve;
            roll_cv_is_valid=roll_ceve_is_valid;
            
            stripcount=stripCount;
            stripcount_is_valid=stripCount_valid;

            out_of_range_stripcount=outofrange_stripcount;
            out_of_range_stripcount_is_valid=outofrange_is_valid;

            lot_ok = lot_is_valid;
            roll_ok = roll_is_valid;

            homogenity_ok = homogenity_is_valid;
            blank_ok = blank_is_valid;

            date = test_date;

            h62_error = h62_error_count;
           not_h62_error = not_h62_error_count;

           ResultForm.lot_avg_res = lot_avg_res;
           ResultForm.roll_avg_res = roll_avg_res;
           ResultForm.lot_cv_res = lot_cv_res;
           ResultForm.roll_cv_res = roll_cv_res;
           ResultForm.lot_out_res = out_valid_res;
           ResultForm.homo_valid_res = homo_ok_res;
           ResultForm.blank_valid_res = blank_ok_res;
           ResultForm.lot_valid_res = lot_ok_res;
           ResultForm.roll_valid_res = roll_ok_res;
        }


#region properties
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }
        public DateTime RollDate
        {
            get
            {
                return roll_date;
            }
            set
            {
                roll_date = value;
            }
        }

        public double HAverages
        {
            get
            {
                return h_avg;
            }
            set
            {
                h_avg = value;
            }
        }
        public double HCV
        {
            get
            {
                return h_cv;
            }
            set
            {
                h_cv = value;
            }
        }
        public int h62errorscount
        {
            get
            {
                return roll_h62;
            }
            set
            {
                roll_h62 = value;
            }
        }
        public int noth62errorscount
        {
            get
            {
                return roll_noth62;
            }
            set
            {
                roll_noth62 = value;
            }
        }
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

        public double Averages
        {
            get
            {
                return Math.Round(roll_avg,2);
            }
            set
            {
                roll_avg= value;
            }
        }
        public double CV
        {
            get
            {
                return Math.Round(roll_cv,2);  
            }
            set
            {
                roll_cv = value;
            }
        }

      public string lot_complete_valid;
        public string Lot_Result_is_Valid
        {
         get{    if (lot_valid)
                {
                    return "Megfelelő";
                }
                else
                    return "Nem Megfelelő";
                
            }
            set
            {
                lot_complete_valid = value;
            }
        }


        public double Lot_Averages
        {
            get
            {
                if (lot_valid)
                {
                    LOTResults.cellColor = Color.Green;
                }
                else
                    LOTResults.cellColor = Color.Red;
                return lot_avg;
            }
            set
            {
                lot_avg = value;
            }
        }
        public int Lot_not_h62_Count
        {
            get
            {
                return not_h62_error;
            }
            set
            {
                not_h62_error = value;
            }
        }
      public int Lot_h62_Count
      {
          get
          {
              return h62_error;
          }
          set
          {
              h62_error= value;
          }
      }
        public double Lot_CV
        {
            get
            {
                return lot_cv;
            }
            set
            {
                lot_cv= value;
            }
        }
        public string Lot_Averages_IsValid
        {
            get
            {
                if (lot_avg_is_valid)
                    return "Megfelelő";
                else
                    return "Nem Megfelelő";
            }
            set
            {
                lot_avg_ok = value;
            }
        }
        public string Lot_AVG_IsValid_Res
        {
            get
            {
                return ResultForm.lot_avg_res;
            }
            set
            {
                ResultForm.lot_avg_res = value;
            }
        }
        public bool LotAverages_IsValid
        {
            get
            {
                return ResultForm.lot_avg_ok;
            }
            set
            {
                ResultForm.lot_avg_ok = value;
            }
        }
        public string Lot_CV_IsValid_Res
        {
            get
            {
                return ResultForm.lot_cv_res;
            }
            set
            {
                ResultForm.lot_cv_res = value;
            }
        }
        public string Lot_CV_IsValid
        {
            get
            {
                if (lot_cv_is_valid)
                    return "Megfelelő";
                else
                    return "Nem Megfelelő";
            }
            set
            {
                lot_cv_ok = value;
            }
        }




        public string roll_avg_ok;
        public string Roll_Averages_IsValid
        {
            get
            {
                if (roll_avg_is_valid)
                    return "Megfelelő";
                else
                    return "Nem Megfelelő";
            }
            set
            {
                roll_avg_ok = value;
            }
        }
        public string Roll_AVG_IsValid_Res
        {
            get
            {
                return ResultForm.roll_avg_res;
            }
            set
            {
                ResultForm.roll_avg_res = value;
            }
        }
        public string Roll_CV_IsValid_Res
        {
            get
            {
                return ResultForm.roll_cv_res;
            }
            set
            {
                ResultForm.roll_cv_res = value;
            }
        }
        public string roll_cv_ok;
        public string Roll_CV_IsValid
        {
            get
            {
                if (roll_cv_is_valid)
                    return "Megfelelő";
                else
                    return "Nem Megfelelő";
            }
            set
            {
                roll_cv_ok = value;
            }
        }


        public string lot_cv_is_valid_ok;
        public string CV_IsValid
        {
            get
            {

                if (lot_cv_is_valid)
                    return "Megfelelő";
                else
                    return "Nem Megfelelő";
            }
            set
            {
                lot_cv_is_valid_ok = value;
            }
        }
        public bool IsValid
        {
            get
            {
                return blank_ok;
            }
            set
            {
                blank_ok = value;
            }
        }
        public string BlankIsValidRes
        {
            get
            {
                return ResultForm.blank_valid_res;
            }
            set
            {
                ResultForm.blank_valid_res = value;
            }
        }
        public string HomogenityIsValidRes
        {
            get
            {
                return ResultForm.homo_valid_res;
            }
            set
            {
                ResultForm.homo_valid_res = value;
            }
        }
        public bool HomogenityIsValid
        {
            get
            {
                return homogenity_ok;
            }
            set
            {
                homogenity_ok = value;
            }
        }
        
      public int LotStripCount
        {
            get
            {
                return stripcount;
            }
            set
            {
                stripcount = value;
            }
        }
        
       public int StripCount
        {
            get
            {
                return stripcount;
            }
            set 
            {
                stripcount = value;
            }
        }
        public bool StripCountIsValid
        {
            get
            {
                return stripcount_is_valid;
            }
            set
            {
                stripcount_is_valid= value;
            }
        }

        public string StripCountIsValidRes
        {
            get
            {
                return ResultForm.stripcount_res;
            }
            set
            {
                ResultForm.stripcount_res = value;
            }
        }
        public int OutOfRangeStripCount
        {
            get
            {
                return out_of_range_stripcount;
            }
            set
            {
                out_of_range_stripcount= value;
            }
        }
        public bool OutOfRangeStripCountIsValid
        {
            get
            {
                return out_of_range_stripcount_is_valid;
            }
            set
            {
                out_of_range_stripcount_is_valid = value;
            }
        }

        public string OutOfRangeStripCountIsValidRes
        {
            get
            {
                return ResultForm.lot_out_res;
            }
            set
            {
                ResultForm.lot_out_res = value;
            }
        }

        public string LotStripCountIsValidRes
        {
            get
            {
                return ResultForm.stripcount_res;
            }
            set
            {
                ResultForm.stripcount_res = value;
            }
        }

        public int LotOutOfRangeStripCount
        {
            get
            {
                return lot_out_of_range_stripcount;
            }
            set
            {
                lot_out_of_range_stripcount = value;
            }
        }

        public int TubeCount
        {
            get
            {
                return tubecount;
            }
            set
            {
                tubecount = value;
            }
        }
        public string lot_stripcount_ok;
        public string LotStripCountIsValid
        {
            get
            {
                if (lot_strip_count_is_valid)
                    return "Megfelelő";
                else return "Nem Megfelelő";
            }
            set
            {
                lot_stripcount_ok = value;
            }
        }

        public int NotH62ErrorCount
        {
            get
            {
                return not_h62_error;
            }
            set
            {
                not_h62_error= value;
            }
        }
        public bool NotH62ErrorCountIsValid
        {
            get
            {
                return not_h62_is_valid;
            }
            set
            {
                not_h62_is_valid= value;
            }
        }
        public int H62ErrorCount
        {
            get
            {
                return h62_error;
            }
            set
            {
                h62_error= value;
            }
        }
        public bool H62ErrorCountIsValid
        {
            get
            {
                return h62_is_valid;
            }
            set
            {
                h62_is_valid= value;
            }
        }

        public bool LotIsValid
        {
            get
            {
                return lot_ok;
            }

            set
            {
                lot_ok = value;
            }


        }
        public bool RollIsValid
        {
            get
            {
                return roll_ok;
            }

            set
            {
                roll_ok= value;
            }


        }

        public string Result
        {
            get
            {
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
                return lotid;
            }
            set
            {
                lotid = value;
            }
        }

        bool select;
        private Color cellColor;
        private int roll_h62;
        private int roll_noth62;
        public bool Select
        {
            get { return select; }
            set { select = value; }
        }
#endregion

  }
}
