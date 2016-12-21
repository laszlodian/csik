using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using e77.MeasureBase;
using Npgsql;
using WinFormBlankTest.UI.Forms;


namespace WinFormBlankTest
{

    /// <summary>
    /// in case of /show from lotnumberform
    /// </summary>
   public class ShowResult
   {
       #region Variables
       public List<string> homo_rollid = new List<string>();
       public List<string> roll_roll_id = new List<string>();
       public List<string> roll_id=new List<string>();
       public List<string> lot_id = new List<string>();
       public List<string> measure_type = new List<string>();
       public List<string> homogenity_roll_id = new List<string>();
       
       public List<DateTime> roll_date= new List<DateTime>();
       public List<int> roll_strip_count = new List<int>();

       public List<double> blank_avg = new List<double>();
       public List<double> blank_cv = new List<double>();
       public List<double> blank_stddev = new List<double>();
       public List<DateTime> blank_date = new List<DateTime>();
       public List<int> blank_tube_count = new List<int>();
       public List<bool> blank_blank_is_valid = new List<bool>();
       public List<string> blank_roll_id = new List<string>();

       public List<string> blank_is_valid = new List<string>();
       public List<string> avg= new List<string>();
       public List<string> date= new List<string>();
       public List<string> cv= new List<string>();

             public List<bool> invalidate= new List<bool>();
             public List<bool> remeasured= new List<bool>();
       public List<double> glu= new List<double>();
       public List<double> nano_amper= new List<double>();
       public List<int> sn= new List<int>();
       public List<string> barcode= new List<string>();
        public List<int> pk_id= new List<int>();

           public List<string> user= new List<string>();
       public List<string> computer= new List<string>();
       public List<DateTime> start_date= new List<DateTime>();
       public List<DateTime> end_date= new List<DateTime>();
        public List<double> temperature= new List<double>();

       public List<string> homogenity_is_valid = new List<string>();
       public List<string> homogenity_avg= new List<string>();
       public List<string> homogenity_cv = new List<string>();
       public List<string> homogenity_date= new List<string>();

       public List<int> homogenity_strip_count = new List<int>();
       public List<bool> homogenity_strip_count_is_valid = new List<bool>();
       
       public List<string> homogenity_h62 = new List<string>();
       public List<string> homogenity_not_h62 = new List<string>();

       public List<int> wrong_strip_count = new List<int>();
        #endregion


       /// <summary>
       /// In case when program started with /show and one row is selected to display additional info
       /// </summary>
       /// <param name="lotid"></param>
       /// <param name="rollid"></param>
       /// <param name="meastype"></param>
       public ShowResult(string lotid, string rollid, string meastype)
       {
           Trace.TraceInformation("ShowResult started(only one argument:lot_id), this will show the blank and homogenity test results");
           using (NpgsqlConnection connection_in = new NpgsqlConnection(Program.dbConnection))
           {
               try
               {
                   Trace.TraceInformation("opening connection in ShowResults:conn:{0};argument:lotid:{1}", connection_in.State, lotid);
                   connection_in.Open();

                   if (meastype.Equals("blank"))
                   {        
                      #region blank_avg
                       using (NpgsqlCommand get_ids = 
                           new NpgsqlCommand(
                               string.Format("SELECT blank_test_identify.lot_id,blank_test_identify.roll_id,blank_test_identify.measure_type,blank_test_result.nano_amper,blank_test_result.measure_id,blank_test_result.is_check_strip,blank_test_result.invalidate,blank_test_result.fk_blank_test_errors_id FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id where blank_test_result.invalidate=False and blank_test_identify.lot_id='{0}' and blank_test_identify.roll_id='{1}' and blank_test_identify.measure_type='{2}'", lotid, rollid, meastype), connection_in))
                       {
                           using (NpgsqlDataReader dr = get_ids.ExecuteReader())
                           {
                               if (dr.HasRows)
                               {
                                   while (dr.Read())
                                   {
                                       pk_id.Add(Convert.ToInt32(dr["pk_id"]));
                                       blank_roll_id.Add(Convert.ToString(dr["roll_id"]));
                                       lot_id.Add(Convert.ToString(dr["lot_id"]));
                                       measure_type.Add(Convert.ToString(dr["measure_type"]));
                                       glu.Add(Convert.ToDouble(dr["glu"]));
                                       nano_amper.Add(Convert.ToDouble(dr["nano_amper"]));
                                       sn.Add(Convert.ToInt32(dr["serial_number"]));
                                       barcode.Add(Convert.ToString(dr["barcode"]));
                                       remeasured.Add(Convert.ToBoolean(dr["remeasured"]));
                                       // invalidate.Add(Convert.ToBoolean(dr["invalidate"]));
                                   }
                                   dr.Close();
                               }
                               else
                               {

                                   dr.Close();
                            //       //MessageBox.Show("Nincs megfelelő eredmény");

                                   throw new SqlNoValueException("Nincs megfelelő eredmény");
                               }

                           }
                       }
                       #endregion
                       Trace.TraceInformation("started to get ids which are in the blank avg table, method to skip:{0}", StackTrace.METHODS_TO_SKIP);
                       #region get IDs

                       using (NpgsqlCommand get_ids = 
                           new NpgsqlCommand(
                               string.Format("SELECT blank_test_identify.lot_id,blank_test_identify.roll_id,blank_test_identify.measure_type,blank_test_result.nano_amper,blank_test_result.measure_id,blank_test_result.is_check_strip,blank_test_result.invalidate,blank_test_result.fk_blank_test_errors_id FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id where blank_test_result.invalidate=False and blank_test_identify.lot_id='{0}' and blank_test_identify.roll_id='{1}' and blank_test_identify.measure_type='{2}'", lotid, rollid, meastype), connection_in))
                       {
                           using (NpgsqlDataReader dr = get_ids.ExecuteReader())
                           {
                               if (dr.HasRows)
                               {
                                   while (dr.Read())
                                   {
                                       pk_id.Add(Convert.ToInt32(dr["pk_id"]));
                                       blank_roll_id.Add(Convert.ToString(dr["roll_id"]));
                                       lot_id.Add(Convert.ToString(dr["lot_id"]));
                                       measure_type.Add(Convert.ToString(dr["measure_type"]));
                                       glu.Add(Convert.ToDouble(dr["glu"]));
                                       nano_amper.Add(Convert.ToDouble(dr["nano_amper"]));
                                       sn.Add(Convert.ToInt32(dr["serial_number"]));
                                       barcode.Add(Convert.ToString(dr["barcode"]));
                                       remeasured.Add(Convert.ToBoolean(dr["remeasured"]));
                                       // invalidate.Add(Convert.ToBoolean(dr["invalidate"]));
                                   }
                                   dr.Close();
                               }
                               else
                               {

                                   dr.Close();
                               //    //MessageBox.Show("Nincs megfelelő eredmény");

                                   throw new SqlNoValueException("Nincs megfelelő eredmény");
                               }

                           }
                       }
                       #endregion
                       Trace.TraceInformation("data of blankAVG is collected rolls count:{0}", blank_roll_id.Count);
                       new BlankResult(lotid, rollid, meastype, glu.ToArray(), nano_amper.ToArray(), sn.ToArray(), barcode.ToArray(), remeasured.ToArray());//,invalidate);
                       foreach (int pk in pk_id)
                       {

                           #region get blank env
                           using (NpgsqlCommand get_blank = new NpgsqlCommand(
                               string.Format("select * from blank_test_environment where fk_blank_test_result_id={0} and remeasured=false and invalidate=false", pk), connection_in))
                           {
                               using (NpgsqlDataReader dr = get_blank.ExecuteReader())
                               {
                                   if (dr.HasRows)
                                   {
                                       while (dr.Read())
                                       {
                                           user.Add(Convert.ToString(dr["user_name"]));
                                           computer.Add(Convert.ToString(dr["computer_name"]));
                                           start_date.Add(Convert.ToDateTime(dr["start_date"]));
                                           end_date.Add(Convert.ToDateTime(dr["end_date"]));
                                           temperature.Add(Convert.ToDouble(dr["temperature"]));
                                           cv.Add(Convert.ToString(dr["cv"]));
                                       }
                                       dr.Close();
                                   }
                                   else
                                   {
                                       dr.Close();
                                       //MessageBox.Show("Nincs megfelelő eredmény");

                                       throw new SqlNoValueException("Nincs megfelelő eredmény");
                                   }

                               }
                           }
                           #endregion
                       }

                       Trace.TraceInformation("Blank values collection finished,blankResultCount:{0}", avg.Count);

                   }

               }
               catch (Exception)
               {
               }
               finally
               {
                   connection_in.Close();
               }

           }
       }
       public bool no_blank_result;

       /// <summary>
       /// In case of /show argument to display values about blank-test and homogenity-test
       /// </summary>
       /// <param name="lot"></param>
       public ShowResult(string lot)
       {

          
           Trace.TraceInformation("ShowResult started(only one argument:lot_id), this will show the blank and homogenity test results");
           using (NpgsqlConnection connection_in = new NpgsqlConnection(Program.dbConnection))
           {
               try
               {
                   Trace.TraceInformation("opening connection in ShowResults:conn:{0};argument:lotid:{1}",connection_in.State,lot);
                   connection_in.Open();

                   Trace.TraceInformation("started to get ids which are in the blank avg table, method to skip:{0}");
                   
                   using (NpgsqlCommand get_ids = new NpgsqlCommand(
                       string.Format("select distinct lot_id,roll_id from blank_test_averages where lot_id='{0}' and remeasured=false and invalidate=false", lot), connection_in))
                   {
                       using (NpgsqlDataReader dr = get_ids.ExecuteReader())
                       {
                           if (dr.HasRows)
                           {
                               while (dr.Read())
                               {
                                   blank_roll_id.Add(Convert.ToString(dr["roll_id"]));
                                   lot_id.Add(Convert.ToString(dr["lot_id"]));

                               }
                               dr.Close();
                           }
                           else
                           {

                               dr.Close();
                               

                               Trace.TraceError(string.Format("Nincs megfelelő eredmény  a {0} LOT azonosítóra blank tesztből", lot));
                               no_blank_result = true;
                           }

                       }
                   }
                   
                   Trace.TraceInformation("data of blankAVG is collected rolls count:{0}",blank_roll_id.Count);

                   #region there is blank result
                   if (!no_blank_result)
                   {


                       foreach (string roll in blank_roll_id)
                       {

                           #region get blank result
                           using (NpgsqlCommand get_blank = new NpgsqlCommand(
                               string.Format("select avg,blank_is_valid,date,cv from blank_test_averages where roll_id='{0}' and lot_id='{1}' and remeasured=false and invalidate=false",
                               roll, lot), connection_in))
                           {
                               using (NpgsqlDataReader dr = get_blank.ExecuteReader())
                               {
                                   if (dr.HasRows)
                                   {
                                       while (dr.Read())
                                       {
                                           avg.Add(Convert.ToString(dr["avg"]));
                                           blank_is_valid.Add(Convert.ToString(dr["blank_is_valid"]));
                                           date.Add(Convert.ToString(dr["date"]));
                                           cv.Add(Convert.ToString(dr["cv"]));
                                       }
                                       dr.Close();
                                   }
                                   else
                                   {
                                       dr.Close();
                                      
                                       Trace.TraceError(string.Format("Nincs megfelelő eredmény  a {0} LOT azonosítóra blank tesztből", lot));
                                   }

                               }
                           }
                           #endregion
                       }
                   }

                   #endregion

                   Trace.TraceInformation("Blank values collection finished,blankResultCount:{0}", avg.Count);
                   
                   roll_id=new List<string>();                  
                   lot_id=new List<string>();

                   Trace.TraceInformation("Empty the roll_id and get all the rollid from homo_result");

                   using (NpgsqlCommand get_ids = new NpgsqlCommand(
                       string.Format("select distinct lot_id,roll_id from homogenity_result where lot_id='{0}' and remeasured=false and invalidate=false", lot), connection_in))
                   {
                       using (NpgsqlDataReader dr = get_ids.ExecuteReader())
                       {
                           if (dr.HasRows)
                           {
                               while (dr.Read())
                               {
                                   homogenity_roll_id.Add(Convert.ToString(dr["roll_id"]));
                                   lot_id.Add(Convert.ToString(dr["lot_id"]));

                               }
                               dr.Close();
                           }
                           else
                           {

                               dr.Close();
                               Trace.TraceWarning("Nincs Homogenitás teszt eredmény a {0} Lot-nál");
                               no_homogenity = true;
                              
                           }

                       }
                   }


                   #region there is homogenity result
                   if (!no_homogenity)
                   {
                       
                       #region get homogenity result
                   
                   Trace.TraceInformation("first get values from homo_res table");

                   foreach (string actroll in homogenity_roll_id)
	                {
                       using (NpgsqlCommand get_homogenity = new NpgsqlCommand(
                           string.Format("select * from homogenity_result where invalidate=false and  remeasured=false and lot_id='{0}' and roll_id='{1}'", lot, actroll), connection_in))
                       {
                           using (NpgsqlDataReader dr = get_homogenity.ExecuteReader())
                           {
                               if (dr.HasRows)
                               {
                                   while (dr.Read())
                                   {

                                       homo_rollid.Add(Convert.ToString(dr["roll_id"]));
                                       homogenity_avg.Add(Convert.ToString(dr["avg"]));
                                       homogenity_cv.Add(Convert.ToString(dr["cv"]));
                                       homogenity_is_valid.Add(Convert.ToString(dr["homogenity_is_valid"]));
                                       homogenity_date.Add(Convert.ToString(dr["date"]));

                                       homogenity_h62.Add(Convert.ToString(dr["h62_errors_count"]));
                                       homogenity_not_h62.Add(Convert.ToString(dr["not_h62_error_count"]));
                                   }
                                   dr.Close();
                               }
                               else
                               {
                                   dr.Close();

                               }

                           }
                        }
                    }
                       #endregion

                List<bool> not_h62=new List<bool>();
                 homogenity_roll_id = new List<string>();
                   roll_id = new List<string>();
                   lot_id = new List<string>();

                   Trace.TraceInformation("Empty the roll_id and get all the rollid from homo_test");
                   
                   using (NpgsqlCommand get_ids = new NpgsqlCommand(string.Format("select distinct lot_id,roll_id from homogenity_test where lot_id='{0}' and remeasured=false and invalidate=false", lot), connection_in))
                   {
                       using (NpgsqlDataReader dr = get_ids.ExecuteReader())
                       {
                           if (dr.HasRows)
                           {
                               while (dr.Read())
                               {
                                   homogenity_roll_id.Add(Convert.ToString(dr["roll_id"]));
                                   lot_id.Add(Convert.ToString(dr["lot_id"]));

                               }
                               dr.Close();
                           }
                           else
                           {

                               dr.Close();
                               ////MessageBox.Show("Nincs megfelelő eredmény");
                               Trace.TraceError("No correct values at homogenity_test in ShowResult class");
                               throw new SqlNoValueException("Nincs megfelelő eredmény");
                           }

                       }
                   }

                   #region get homogenity result

                   Trace.TraceInformation("Started to get all values from homo_test with a foreach statement");
                       int i=0;
                   foreach (string actroll in homogenity_roll_id)
                   {
                       
                       #region get strip count at each roll
                       #region getStripCount


                       stipCount_part1 =
                           string.Format("SELECT COUNT(blank_test_identify.pk_id) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE (blank_test_errors.error_text='' and blank_test_result.glu<>0) and blank_test_identify.lot_id='{0}' and blank_test_identify.roll_id='{1}' and blank_test_identify.measure_type='homogenity' and blank_test_identify.invalidate=false", lot, actroll);
                       stipCount_part2 =
                           string.Format("SELECT COUNT(blank_test_identify.pk_id) FROM blank_test_identify LEFT JOIN blank_test_result ON blank_test_identify.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id WHERE blank_test_errors.error_text<>'' and blank_test_identify.lot_id='{0}' and blank_test_identify.roll_id='{1}' and blank_test_identify.measure_type='homogenity' and blank_test_identify.invalidate=false", lot, actroll);

                       using (NpgsqlCommand stripCount1 = new NpgsqlCommand(stipCount_part1, connection_in))      //get valid strip count in two step)
                       {
                           valid_strip_count = stripCount1.ExecuteScalar();
                       }
                       using (NpgsqlCommand stripCount2 = new NpgsqlCommand(stipCount_part2, connection_in))
                       {
                           valid_strip_count2 = stripCount2.ExecuteScalar();
                       }
                       tha_strip_count_in_a_roll = Convert.ToInt32(valid_strip_count) + Convert.ToInt32(valid_strip_count2);

                       #endregion
                       #endregion

                       object wrongstripCount1 = null;

                       #region get wrong strips(out of range)

                       using (NpgsqlCommand get_test_result = new NpgsqlCommand(
                           string.Format("select COUNT(homogenity_test.strip_ok) from homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where homogenity_test.strip_ok=false and blank_test_result.roll_id='{1}' and  blank_test_errors.error_text<>'' and  blank_test_result.invalidate=False and blank_test_errors.roll_id='{1}' and blank_test_errors.lot_id='{0}' and blank_test_errors.invalidate=False and blank_test_result.lot_id='{0}' and homogenity_test.strip_ok=False and homogenity_test.roll_id='{1}' and homogenity_test.lot_id='{0}' and homogenity_test.invalidate=False and blank_test_result.remeasured=False", lot, actroll), connection_in))
                       { 
                           wrongstripCount1 = get_test_result.ExecuteScalar();
                          
                           if (wrongstripCount1 == DBNull.Value)
                           {
                               throw new SqlNoValueException(string.Format("No Value For this query: {0}", get_test_result.CommandText));
                           }
                       }

                        object wrongstripCount = null;

                       using (NpgsqlCommand get_test_result = new NpgsqlCommand(
                          string.Format("select COUNT(homogenity_test.strip_ok) from homogenity_test LEFT JOIN blank_test_result ON homogenity_test.fk_blank_test_result_id = blank_test_result.pk_id LEFT JOIN blank_test_errors ON blank_test_result.fk_blank_test_errors_id = blank_test_errors.pk_id where homogenity_test.strip_ok=false and blank_test_result.roll_id='{1}'  and (blank_test_errors.error_text='' and blank_test_result.glu<>0) and  blank_test_result.invalidate=False and blank_test_errors.roll_id='{1}' and blank_test_errors.lot_id='{0}' and blank_test_errors.invalidate=False and blank_test_result.lot_id='{0}' and homogenity_test.strip_ok=False and homogenity_test.roll_id='{1}' and homogenity_test.lot_id='{0}' and homogenity_test.invalidate=False and blank_test_result.remeasured=False", lot, actroll), connection_in))
                       {
                           wrongstripCount = get_test_result.ExecuteScalar();

                           if (wrongstripCount == DBNull.Value)
                           {
                               throw new SqlNoValueException(string.Format("No Value For this query: {0}", get_test_result.CommandText));
                           }
                       }

                       ///calculate the wrong strip count in 2 step
                       wrong_strip_count.Add(Convert.ToInt32(wrongstripCount1) + Convert.ToInt32(wrongstripCount));
                       wrongs=Convert.ToInt32(wrongstripCount1) + Convert.ToInt32(wrongstripCount);
                   


                       #endregion

                   #region Switch stripCount && not_h62_error_count && wrong_strip_count
                   if ((Convert.ToInt32(tha_strip_count_in_a_roll) < 49)
                       || (Convert.ToInt32(tha_strip_count_in_a_roll) > 333))
                   {
                       Trace.TraceError("Impossible stripCount");
                       not_h62_ok=false;
                       homogenity_ok=false;
                       out_of_range_valid = false;

                   }
                   if ((Convert.ToInt32(tha_strip_count_in_a_roll) >= 49)
                       && (Convert.ToInt32(tha_strip_count_in_a_roll) <= 71))
                   {
                       Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 1");
                       if ((Convert.ToInt32(homogenity_not_h62[i]) <= 1)
                           && (Convert.ToInt32(wrongs) <= 1))
                       {
                            homogenity_ok=true;
                       }
                       else
                       {
                           homogenity_ok=false;

                           if (Convert.ToInt32(homogenity_not_h62[i]) > 1)
                           {
                                not_h62_ok=false;
                           }
                           else
                              not_h62_ok = true;
                           if (Convert.ToInt32(wrongs) > 1)
                           {
                               out_of_range_valid = false;
                           }
                           else
                               out_of_range_valid=true;
                       }
                   }
                   if ((Convert.ToInt32(tha_strip_count_in_a_roll) >= 72)
                       && (Convert.ToInt32(tha_strip_count_in_a_roll) <= 94))
                   {
                       Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 2");
                       if ((Convert.ToInt32(homogenity_not_h62[i]) <= 1)
                           && (Convert.ToInt32(wrongs) <= 1))
                       {
                            homogenity_ok=true;
                       }
                       else
                       {
                           homogenity_ok=false;

                           if (Convert.ToInt32(homogenity_not_h62[i]) > 2)
                           {
                              not_h62_ok = false;
                           }
                           else
                              not_h62_ok = true;
                           if (Convert.ToInt32(wrongs) > 2)
                           {
                               out_of_range_valid = false;
                           }
                           else
                               out_of_range_valid=true;
                       }
                   }
                   if ((Convert.ToInt32(tha_strip_count_in_a_roll) >= 95)
                      && (Convert.ToInt32(tha_strip_count_in_a_roll) <= 116))
                   {
                       Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 3");
                       if ((Convert.ToInt32(homogenity_not_h62[i]) <= 3)
                           && (Convert.ToInt32(wrongs) <= 3))
                       {
                          homogenity_ok=true;
                       }
                       else
                       {
                           homogenity_ok=false;
                           if (Convert.ToInt32(homogenity_not_h62[i]) > 3)
                           {
                               not_h62_ok = false;
                           }
                           else
                               not_h62_ok=true;
                           if (Convert.ToInt32(wrongs) > 3)
                           {
                               out_of_range_valid = false;
                           }
                           else
                               out_of_range_valid = true;
                       }
                   }
                   if ((Convert.ToInt32(tha_strip_count_in_a_roll) >= 117)
                      && (Convert.ToInt32(tha_strip_count_in_a_roll) <= 139))
                   {
                       Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 4");
                       if ((Convert.ToInt32(homogenity_not_h62[i]) <= 4)
                           && (Convert.ToInt32(wrongs) <= 4))
                       {
                        homogenity_ok=true;
                       }
                       else
                       {
                            
                           homogenity_ok=false;
                           if (Convert.ToInt32(homogenity_not_h62[i]) > 4)
                           {
                               not_h62_ok = false;
                           }
                           else
                               not_h62_ok=true;
                           if (Convert.ToInt32(wrongs) > 4)
                           {
                               out_of_range_valid = false;
                           }
                           else
                               out_of_range_valid = true;
                       }
                   }
                   if ((Convert.ToInt32(tha_strip_count_in_a_roll) >= 140)
                      && (Convert.ToInt32(tha_strip_count_in_a_roll) <= 161))
                   {
                       Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 5");
                       if ((Convert.ToInt32(homogenity_not_h62[i]) <= 5)
                           && (Convert.ToInt32(wrongs) <= 5))
                       {
                            homogenity_ok=true;
                       }
                       else
                       {
                           homogenity_ok=false;
                           if (Convert.ToInt32(homogenity_not_h62[i]) > 5)
                           {
                               not_h62_ok = false;
                           }
                           else
                               not_h62_ok=true;
                           if (Convert.ToInt32(wrongs) > 5)
                           {
                               out_of_range_valid = false;
                           }
                           else
                               out_of_range_valid = true;
                       }
                   }
                   if ((Convert.ToInt32(tha_strip_count_in_a_roll) >= 162)
                      && (Convert.ToInt32(tha_strip_count_in_a_roll) <= 183))
                   {
                       Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 6");
                       if ((Convert.ToInt32(homogenity_not_h62[i]) <= 6)
                           && (Convert.ToInt32(wrongs) <= 6))
                       {
                             homogenity_ok=true;
                       }
                       else
                       {
                           homogenity_ok=false;
                           if (Convert.ToInt32(homogenity_not_h62[i]) > 6)
                           {
                             not_h62_ok = false;
                           }
                           else
                               not_h62_ok=true;
                           if (Convert.ToInt32(wrongs) > 6)
                           {
                               out_of_range_valid = false;
                           }
                           else
                               out_of_range_valid = true;
                       }
                   }
                   if ((Convert.ToInt32(tha_strip_count_in_a_roll) >= 184)
                      && (Convert.ToInt32(tha_strip_count_in_a_roll) <= 204))
                   {
                       Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 7");
                       if ((Convert.ToInt32(homogenity_not_h62[i]) <= 7)
                           && (Convert.ToInt32(wrongs) <= 7))
                       {
                            homogenity_ok=true;
                       }
                       else
                       {
                           homogenity_ok=false;
                           if (Convert.ToInt32(homogenity_not_h62[i]) > 7)
                           {
                               not_h62_ok = false;
                           }
                           else
                               not_h62_ok=true;
                           if (Convert.ToInt32(wrongs) > 7)
                           {
                               out_of_range_valid = false;
                           }
                           else
                                out_of_range_valid=true;
                       }
                   }
                   if ((Convert.ToInt32(tha_strip_count_in_a_roll) >= 205)
                      && (Convert.ToInt32(tha_strip_count_in_a_roll) <= 226))
                   {
                       Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 8");
                       if ((Convert.ToInt32(homogenity_not_h62[i]) <= 8)
                           && (Convert.ToInt32(wrongs) <= 8))
                       {
                           homogenity_ok=true;
                       }
                       else
                       {
                           homogenity_ok=false;
                           if (Convert.ToInt32(homogenity_not_h62[i]) > 8)
                           {
                               not_h62_ok = false;
                           }
                           else
                               not_h62_ok=true;
                           if (Convert.ToInt32(wrongs) > 8)
                           {
                               out_of_range_valid = false;
                           }
                           else
                                out_of_range_valid=true;
                       }
                   }
                   if ((Convert.ToInt32(tha_strip_count_in_a_roll) >= 227)
                      && (Convert.ToInt32(tha_strip_count_in_a_roll) <= 248))
                   {
                       Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 9");
                       if ((Convert.ToInt32(homogenity_not_h62[i]) <= 9)
                           && (Convert.ToInt32(wrongs) <= 9))
                       {
                            homogenity_ok=true;
                       }
                       else
                       {
                           homogenity_ok=false;
                           if (Convert.ToInt32(homogenity_not_h62[i]) > 9)
                           {
                               not_h62_ok = false;
                           }
                           else
                               not_h62_ok=true;
                           if (Convert.ToInt32(wrongs) > 9)
                           {
                               out_of_range_valid = false;
                           }
                           else
                               out_of_range_valid = true;
                       }
                   }
                   if ((Convert.ToInt32(tha_strip_count_in_a_roll) >= 249)
                      && (Convert.ToInt32(tha_strip_count_in_a_roll) <= 269))
                   {
                       Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 10");
                       if ((Convert.ToInt32(homogenity_not_h62[i]) <= 10)
                           && (Convert.ToInt32(wrongs) <= 10))
                       {
                            homogenity_ok=true;
                       }
                       else
                       {
                           homogenity_ok=false;
                           if (Convert.ToInt32(homogenity_not_h62[i]) > 10)
                           {
                               not_h62_ok = false;
                           }
                           else
                               not_h62_ok=true;
                           if (Convert.ToInt32(wrongs) > 10)
                           {
                               out_of_range_valid = false;
                           }
                           else
                               out_of_range_valid = true;
                       }
                   }
                   if ((Convert.ToInt32(tha_strip_count_in_a_roll) >= 270)
                      && (Convert.ToInt32(tha_strip_count_in_a_roll) <= 291))
                   {
                       Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 11");
                       if ((Convert.ToInt32(homogenity_not_h62[i]) <= 11)
                           && (Convert.ToInt32(wrongs) <= 11))
                       {
                            homogenity_ok=true;
                       }
                       else
                       {
                           homogenity_ok=false;
                           if (Convert.ToInt32(homogenity_not_h62[i]) > 11)
                           {
                               not_h62_ok = false;
                           }
                           else
                               not_h62_ok=true;
                           if (Convert.ToInt32(wrongs) > 11)
                           {
                               out_of_range_valid = false;
                           }
                           else
                               out_of_range_valid = true;
                       }
                   }
                   if ((Convert.ToInt32(tha_strip_count_in_a_roll) >= 292)
                      && (Convert.ToInt32(tha_strip_count_in_a_roll) <= 312))
                   {
                       Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 12");
                       if ((Convert.ToInt32(homogenity_not_h62[i]) <= 12)
                           && (Convert.ToInt32(wrongs) <= 12))
                       {
                          homogenity_ok=true;
                       }
                       else
                       {
                          homogenity_ok=false;
                           if (Convert.ToInt32(homogenity_not_h62[i]) > 12)
                           {
                               not_h62_ok = false;
                           }
                           else
                               not_h62_ok=true;
                           if (Convert.ToInt32(wrongs) > 12)
                           {
                               out_of_range_valid = false;
                           }
                           else
                               out_of_range_valid = true;
                       }
                   }
                   tha_strip_count_in_a_roll = Convert.ToInt32(tha_strip_count_in_a_roll);
                   if ((Convert.ToInt32(tha_strip_count_in_a_roll) >= 313)
                      && (Convert.ToInt32(tha_strip_count_in_a_roll) <= 333))
                   {
                       Trace.TraceInformation("Maximum allowed not_H62 error count and wrong strip count is less or equal than 13");
                       if ((Convert.ToInt32(homogenity_not_h62[i]) <= 13)
                           && (Convert.ToInt32(wrongs) <= 13))
                       {
                          homogenity_ok=true;
                       }
                       else
                       {
                           homogenity_ok=false;
                           if (Convert.ToInt32(homogenity_not_h62[i]) > 13)
                           {
                               not_h62_ok=false;
                           }
                           else
                               not_h62_ok=true;
                           if (Convert.ToInt32(wrongs) > 13)
                           {
                               out_of_range_valid = false;
                           }
                           else
                               out_of_range_valid = true;
                       }
                   
                   }
                   i++;
                   out_is_ok.Add(out_of_range_valid);
                   not_h62.Add(not_h62_ok);
                //   homogenity_is_valid.Add(homogenity_ok.ToString());
                   }
                   
                   }
                   #endregion

                   #endregion

                   Trace.TraceInformation("data collection finished result form will be displayed ");

                   
                   #endregion


                   ///display results in case of /show argument
                   new ResultForm(lot,  blank_roll_id.ToArray(),homo_rollid.ToArray(), blank_is_valid.ToArray(), homogenity_is_valid.ToArray(), avg.ToArray(), 
                       homogenity_avg.ToArray(),
                       cv.ToArray(), homogenity_cv.ToArray(), date.ToArray(), homogenity_date.ToArray(), measure_type.ToArray(), wrong_strip_count.ToArray(), 
                       homogenity_h62.ToArray(), homogenity_not_h62.ToArray(),not_h62_ok).ShowDialog();
                  

               }
               catch (Exception ex)
               {
                   Trace.TraceError("Exception in showResult, exception:{0}", ex.InnerException);

               }
               finally
               {
                   connection_in.Close();
               }
           }

           

                
       }
       public List<bool> out_is_ok=new List<bool>();
       private string GetRolls()
       {
           
           string res = string.Empty;

           foreach (string roll in roll_id)
           {
                res += string.Format("{0} ",roll);
           }


           return res;
       }

       /// <summary>
       /// In case of invalidate a strip value at blank_test
       /// </summary>
       /// <param name="lotid"></param>
      /// <param name="rollid"></param>
       public ShowResult(string lotid, string rollid)
       {
           #region Get blank result
           using (NpgsqlConnection invalidateConn = new NpgsqlConnection(Program.dbConnection))
           {
               try
               {
                   invalidateConn.Open();
               }
               catch (Exception ex)
               {
                   Trace.TraceError("Exception when opening connection ex.:{0}",ex.StackTrace);
                   throw;
               }

               using (NpgsqlCommand get_blank = new NpgsqlCommand(string.Format("select * from blank_test_averages where lot_id='{0}' and roll_id='{1}'", lotid, rollid), invalidateConn))
               {
                   using (NpgsqlDataReader dr = get_blank.ExecuteReader())
                   {
                       if (dr.HasRows)
                       {
                           while (dr.Read())
                           {
                               blank_roll_id.Add(Convert.ToString(dr["roll_id"]));
                               blank_avg.Add(Convert.ToDouble(dr["avg"]));
                               blank_blank_is_valid.Add(Convert.ToBoolean(dr["blank_is_valid"]));
                               blank_date.Add(Convert.ToDateTime(dr["date"]));
                               blank_cv.Add(Convert.ToDouble(dr["cv"]));
                               blank_tube_count.Add(Convert.ToInt32(dr["tube_count_in_one_roll"]));
                               blank_stddev.Add(Convert.ToDouble(dr["stddev"]));
                           }
                           dr.Close();
                       }
                       else
                       {
                           dr.Close();
                            throw new SqlNoValueException("Nincs megfelelő eredmény");
                       }
                   }
               }
           #endregion
           }

           new BlankResult(lotid, blank_roll_id.ToArray(),
             blank_avg.ToArray(), blank_blank_is_valid.ToArray(), blank_cv.ToArray(),
             blank_date.ToArray(), blank_tube_count.ToArray(), blank_stddev.ToArray()).ShowDialog();
       }

       public static bool Instance;
       private bool no_homogenity;
       private string stipCount_part1;
       private string stipCount_part2;
       private object valid_strip_count;
       private object valid_strip_count2;
       private int tha_strip_count_in_a_roll;
        private  bool out_of_range_valid;
        private  bool not_h62_is_valid;
        private  int wrongs;
        private  bool is_out_ok;
        private  bool not_h62_ok;
        private  bool homogenity_ok;

       /// <summary>
       /// In case of /show-all results and rolls can be invalidated,remeasured 
       /// </summary>
       /// <param name="lotid"></param>
       /// <param name="is_valid"></param>
       public ShowResult(string lotid,bool is_valid)
       {

           #region Variables

           List<bool> lot_strip_count_is_valid = new List<bool>();
           List<int> lot_strip_count = new List<int>();
           List<bool> lot_is_valid = new List<bool>();
           List<double> lot_cv = new List<double>();
           List<double> lot_avg = new List<double>();
           List<int> lot_h62 = new List<int>();
           List<int> lot_not_h62 = new List<int>();
          
           List<double> roll_cv = new List<double>();
           List<double> roll_avg = new List<double>();
           List<double> roll_stddev = new List<double>();

           List<double> blank_stddev= new List<double>();
           List<double> blank_cv= new List<double>();
           List<double> blank_avg= new List<double>();
           List<DateTime> blank_date = new List<DateTime>();
           List<bool> blank_blank_is_valid = new List<bool>();
           List<int> blank_tube_count = new List<int>();
           List<string> blank_rollid = new List<string>();

           List<string> homogenity_rollid = new List<string>();
            List<bool> homogenity_is_valid = new List<bool>();
            List<double> homogenity_avg= new List<double>();
            List<double> homogenity_cv = new List<double>();
            List<double> homogenity_stddev= new List<double>();
            List<DateTime> homogenity_date= new List<DateTime>();
            List<int> homogenity_h62 = new List<int>();
            List<int> homogenity_not_h62 = new List<int>();
        List<int> homogenity_tube_count= new List<int>();
           

            List<bool> lot_avg_is_valid = new List<bool>();
            List<bool> lot_cv_is_valid = new List<bool>();

            List<bool> roll_avg_is_valid = new List<bool>();
            List<bool> roll_cv_is_valid = new List<bool>();
            List<bool> roll_ok= new List<bool>();
            List<int> roll_out_of_range_strip_count = new List<int>();

           List<bool> roll_h62_is_ok= new List<bool>();
           List<bool> roll_not_h62_is_ok= new List<bool>();

           #endregion

           object pk_id = null;

           using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
           {
               try
               {
                   conn.Open();
                   using (NpgsqlCommand get_ids = 
                       new NpgsqlCommand(
                       string.Format("select * from blank_test_identify where lot_id='{0}' and remeasured=false", lotid), conn))
                   {
                       using (NpgsqlDataReader dr = get_ids.ExecuteReader())
                       {
                           if (dr.HasRows)
                           {
                               while (dr.Read())
                               {
                                   lot_id.Add(Convert.ToString(dr["lot_id"]));
                                   roll_id.Add(Convert.ToString(dr["roll_id"]));
                                  
                                   
                               }
                               dr.Close();
                           }
                           else
                           {

                               dr.Close();
                               throw new SqlNoValueException(string.Format("Nincs megfelelő eredmény:{0}",get_ids.CommandText));
                           }

                       }
                   }//Get lot_id,roll_id
                   using (NpgsqlCommand get_measureT = new NpgsqlCommand(string.Format("select distinct measure_type from blank_test_identify where lot_id='{0}' and remeasured=false",lotid),conn))
                   {
                       using (NpgsqlDataReader dr = get_measureT.ExecuteReader())
                       {
                           if (dr.HasRows)
                           {
                               while (dr.Read())
                               {
                                   measure_type.Add(Convert.ToString(dr["measure_type"]));
                               }
                               dr.Close();
                           }
                           else
                           {
                               dr.Close();
                           
                               throw new SqlNoValueException("Nincs megfelelő eredmény");
                           }
                       }

                   }
                   int i = 0;
                   foreach (string measT in measure_type)
                   {
                       if (measT == "blank")
                       {
                           
                           #region Get blank result
                           using (NpgsqlCommand get_blank = new NpgsqlCommand(string.Format("select * from blank_test_averages where lot_id='{0}' and invalidate=false",  lotid), conn))
                           {
                               using (NpgsqlDataReader dr = get_blank.ExecuteReader())
                               {
                                   if (dr.HasRows)
                                   {
                                       while (dr.Read())
                                       {
                                           blank_rollid.Add(Convert.ToString(dr["roll_id"]));
                                           blank_avg.Add(Convert.ToDouble(dr["avg"]));
                                           blank_blank_is_valid.Add(Convert.ToBoolean(dr["blank_is_valid"]));
                                           blank_date.Add(Convert.ToDateTime(dr["date"]));
                                           blank_cv.Add(Convert.ToDouble(dr["cv"]));
                                           blank_tube_count.Add(Convert.ToInt32(dr["tube_count_in_one_roll"]));
                                           blank_stddev.Add(Convert.ToDouble(dr["stddev"]));
                                       }
                                       dr.Close();
                                   }
                                   else
                                   {
                                       dr.Close();
                                   //    //MessageBox.Show("Nincs megfelelő eredmény");

                                       Trace.TraceError("Nincs megfelelő eredmény");
                                   }

                               }
                           }
                           #endregion

                          ///Show blank results 
                             new BlankResult(lotid,blank_rollid.ToArray(),
                               blank_avg.ToArray(),blank_blank_is_valid.ToArray(),blank_cv.ToArray(),
                               blank_date.ToArray(),blank_tube_count.ToArray(),blank_stddev.ToArray()).ShowDialog();
                          

                       }
                       else if (measT == "homogenity")
                       {
                           roll_id = new List<string>();
                           lot_id = new List<string>();

                           using (NpgsqlCommand get_ids = new NpgsqlCommand(string.Format("select distinct lot_id,roll_id from homogenity_result where lot_id='{0}' and remeasured=false", lotid), conn))
                           {
                               using (NpgsqlDataReader dr = get_ids.ExecuteReader())
                               {
                                   if (dr.HasRows)
                                   {
                                       while (dr.Read())
                                       {
                                           roll_id.Add(Convert.ToString(dr["roll_id"]));
                                           lot_id.Add(Convert.ToString(dr["lot_id"]));

                                       }
                                       dr.Close();
                                   }
                                   else
                                   {

                                       dr.Close();
                                      Trace.TraceError("Nincs megfelelő eredmény");
                                   }

                               }
                           }

                           i = 0;

                           foreach (string act_roll_id in roll_id)
                           {

                               #region Get Homogenity result

                               using (NpgsqlCommand get_homogenity = new NpgsqlCommand(string.Format("select * from homogenity_result where roll_id='{0}' and lot_id='{1}' and invalidate=false and remeasured=false", act_roll_id, lotid), conn))
                               {
                                   using (NpgsqlDataReader dr = get_homogenity.ExecuteReader())
                                   {
                                       bool strip_count_is_ok;
                                       if (dr.HasRows)
                                       {
                                           while (dr.Read())
                                           {
                                               homogenity_rollid.Add(Convert.ToString(dr["roll_id"]));
                                               homogenity_avg.Add(Convert.ToDouble(dr["avg"]));
                                               homogenity_cv.Add(Convert.ToDouble(dr["cv"]));
                                               homogenity_is_valid.Add(Convert.ToBoolean(dr["homogenity_is_valid"]));
                                               homogenity_date.Add(Convert.ToDateTime(dr["date"]));
                                               homogenity_stddev.Add(Convert.ToDouble(dr["stddev"]));
                                               homogenity_tube_count.Add(Convert.ToInt32(dr["tube_count"]));
                                               homogenity_strip_count.Add(Convert.ToInt32(dr["strip_count_in_one_roll"]));
                                               homogenity_h62.Add(Convert.ToInt32(dr["h62_errors_count"]));
                                               homogenity_not_h62.Add(Convert.ToInt32(dr["not_h62_error_count"]));

                                               roll_out_of_range_strip_count.Add(Convert.ToInt32(dr["out_of_range_strip_count"]));

                                               if ((Convert.ToInt32(dr["strip_count_in_one_roll"])) >= 49
                                                   && (Convert.ToInt32(dr["strip_count_in_one_roll"])) <= 333)
                                               {
                                                   strip_count_is_ok = true;
                                               }
                                               else
                                                   strip_count_is_ok = false;

                                               homogenity_strip_count_is_valid.Add(strip_count_is_ok);
                                           }
                                           dr.Close();
                                       }
                                       else
                                       {
                                           dr.Close();

                                       }

                                   }
                               #endregion
                               }
                               i++;
                           }//End of foreach in roll_id
                           
                           new HomogenityResult(lotid,homogenity_rollid.ToArray(),homogenity_avg.ToArray(),
                               homogenity_is_valid.ToArray(),
                               homogenity_cv.ToArray(),homogenity_date.ToArray(),
                               homogenity_h62.ToArray(),homogenity_not_h62.ToArray(),
                               roll_out_of_range_strip_count.ToArray(), homogenity_strip_count.ToArray(),
                              homogenity_tube_count.ToArray(),homogenity_stddev.ToArray()).ShowDialog();
                         
                       }
                       i++;
                   }//End of foreach in measreType
                   roll_id = new List<string>();
                   lot_id = new List<string>();

                   using (NpgsqlCommand get_ids = new NpgsqlCommand(string.Format("select distinct lot_id,roll_id from roll_result where lot_id='{0}' and invalidate=false and remeasured=false", lotid), conn))
                   {
                       using (NpgsqlDataReader dr = get_ids.ExecuteReader())
                       {
                           if (dr.HasRows)
                           {
                               while (dr.Read())
                               {
                                   roll_id.Add(Convert.ToString(dr["roll_id"]));
                                   lot_id.Add(Convert.ToString(dr["lot_id"]));

                               }
                               dr.Close();
                           }
                           else
                           {

                               dr.Close();
                              throw new SqlNoValueException("Nincs megfelelő eredmény");
                           }

                       }
                   }
                   roll_out_of_range_strip_count = new List<int>();
                   roll_roll_id = new List<string>();
                   roll_avg = new List<double>();
                   roll_cv = new List<double>();
                   roll_stddev = new List<double>();
                   roll_strip_count = new List<int>();
                   roll_date = new List<DateTime>();
                   roll_out_of_range_strip_count = new List<int>();
                   roll_ok = new List<bool>();
                   roll_cv_is_valid = new List<bool>();
                   roll_avg_is_valid = new List<bool>();
                   List<int> roll_h62 = new List<int>();
                   List<int> roll_not_h62 = new List<int>();
                   //foreach (string act_roll_id in roll_id)
                   //{                      
                       
                       #region Get Roll Result


                       
                   
                   using (NpgsqlCommand get_roll_res = new NpgsqlCommand(string.Format("select * FROM roll_result LEFT JOIN lot_result ON roll_result.fk_lot_result_id = lot_result.pk_id LEFT JOIN homogenity_result ON lot_result.fk_homogenity_result_id = homogenity_result.pk_id where roll_result.lot_id='{0}' and homogenity_result.invalidate=false and homogenity_result.remeasured=false", lotid), conn))
                       {
                           using (NpgsqlDataReader dr = get_roll_res.ExecuteReader())
                           {
                               if (dr.HasRows)
                               {
                                   while (dr.Read())
                                   {
                                       roll_roll_id.Add(Convert.ToString(dr["roll_id"]));
                                       roll_avg.Add(Convert.ToDouble(dr["roll_avg"]));
                                       roll_cv.Add(Convert.ToDouble(dr["roll_cv"]));
                                       roll_stddev.Add(Convert.ToDouble(dr["roll_stddev"]));
                                       roll_avg_is_valid.Add(Convert.ToBoolean(dr["avg_ok"]));
                                       roll_cv_is_valid.Add(Convert.ToBoolean(dr["cv_ok"]));
                                       roll_ok.Add(Convert.ToBoolean(dr["roll_is_valid"]));
                                       roll_strip_count.Add(Convert.ToInt32(dr["roll_strip_count"]));
                                       roll_date.Add(Convert.ToDateTime(dr["roll_date"]));
                                       roll_out_of_range_strip_count.Add(Convert.ToInt32(dr["out_of_range_strip_count"]));
                                   
                                   }
                                   dr.Close();
                               }
                               else
                               {
                                   dr.Close();

                               }

                           }
                       }
                       #endregion
                       /// }
                    
                   new RollResults(lotid,roll_roll_id.ToArray(),roll_avg.ToArray(),roll_avg_is_valid.ToArray(),roll_cv.ToArray(),roll_cv_is_valid.ToArray(),
                       roll_date.ToArray(), roll_strip_count.ToArray(), roll_out_of_range_strip_count.ToArray(), roll_ok.ToArray()/*,roll_h62.ToArray(),roll_not_h62.ToArray*/).ShowDialog();

                   ClearVariablesWithDBData(lotid, roll_roll_id, roll_avg, roll_avg_is_valid, roll_cv, roll_cv_is_valid,
                       roll_date, roll_strip_count, roll_out_of_range_strip_count, roll_ok);
                   
               }
               catch (Exception ex)
               {
                 
                   Trace.TraceError("Exception in showresult: exception: {0};StackTrace:{1}", ex.Message,ex.StackTrace);
               }
               finally
               {
                   conn.Close();
               }

           }//End Of NpgsqlConnection

       }

       /// <summary>
       /// Empty the values of these variables that at the next call the new data couldn't be mixed with the previously
       /// </summary>
       /// <param name="lotid"></param>
       /// <param name="roll_roll_id"></param>
       /// <param name="roll_avg"></param>
       /// <param name="roll_avg_is_valid"></param>
       /// <param name="roll_cv"></param>
       /// <param name="roll_cv_is_valid"></param>
       /// <param name="roll_date"></param>
       /// <param name="roll_strip_count"></param>
       /// <param name="roll_out_of_range_strip_count"></param>
       /// <param name="roll_ok"></param>
       private void ClearVariablesWithDBData(string lotid, List<string> roll_roll_id, List<double> roll_avg, List<bool> roll_avg_is_valid, 
           List<double> roll_cv, List<bool> roll_cv_is_valid, List<DateTime> roll_date, List<int> roll_strip_count, List<int> roll_out_of_range_strip_count,
           List<bool> roll_ok)
       {
           lotid = string.Empty;
           roll_roll_id = new List<string>();
           roll_avg = new List<double>();
           roll_avg_is_valid = new List<bool>();
           roll_cv = new List<double>();
           roll_cv_is_valid = new List<bool>();
           roll_date = new List<DateTime>();
           roll_strip_count = new List<int>();
           roll_out_of_range_strip_count = new List<int>();
           roll_ok = new List<bool>();

       }

    }
}
