using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.Properties;

namespace e77.MeasureBase
{
    public static class ExtensionMethodsNullableBool
    {
        static public string LocalizedStr(this bool? obj_in)
        {
            if (!obj_in.HasValue)
                return Resources.MEASURING;
            return obj_in.Value.LocalizedStr();
        }

        static public string LocalizedStr(this bool obj_in)
        {
            return obj_in ? Resources.MEASURE_SUCCESSFULL : Resources.MEASURE_FAILED;
        }

        static public void MergeSubResult(ref bool? res_inout, bool subRes_in)
        {
            MergeSubResult(ref res_inout, (bool?)subRes_in);
        }
        /// <summary>
        /// res_in_out  sub_res       result
        /// =====================     =======
        /// null        null      |    null
        /// true        null      |    null
        /// false       null      |    null
        /// null        true      |    null
        /// true        true      |    true     <=   only this "returns" true
        /// false       true      |   false
        /// null        false     |   false
        /// true        false     |   false
        /// false       false     |   false
        /// </summary>
        /// <param name="res_inout"></param>
        /// <param name="subRes_in"></param>
        static public void MergeSubResult(ref bool? res_inout, bool? subRes_in)
        {
            if (subRes_in.HasValue)
            {
                if (subRes_in.Value == false)
                    res_inout = false;
            }
            else
            {   //no subres
                if (res_inout.HasValue && res_inout.Value) //res_inout == true
                    res_inout = null;   //remove true flag
            }
        }
    }
}
