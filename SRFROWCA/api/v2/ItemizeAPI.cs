using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRFROWCA.api.v2
{
    public class ItemizeAPI
    {
        internal enum ReportLevel
        {
            Country  = 1,
            NotSelected = 0
        }

        internal enum ReportType
        {
            OnlyReported = 1,
            MonthlyStatus = 2,
            FinalStatus = 3
        }

    }
}