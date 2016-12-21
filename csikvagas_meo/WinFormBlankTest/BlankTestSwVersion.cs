using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.Sql;

namespace WinFormBlankTest
{
    class BlankTestSwVersion : SwVersion
    {
        public override string SwVersions
        {
            get
            {
                return string.Format("{0}:{1}/{2}",
                        System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.Assembly.GetExecutingAssembly().GetName().Version,
                        base.SwVersions);
            }
        }
    }
}
