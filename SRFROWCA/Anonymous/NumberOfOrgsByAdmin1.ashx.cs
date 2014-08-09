using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using BusinessLogic;

namespace SRFROWCA.Anonymous
{
    /// <summary>
    /// Summary description for NumberOfOrgsByAdmin1
    /// </summary>
    public class NumberOfOrgsByAdmin1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            object[] reportParams = GetReportParam(context);
            DataTable dt = DBContext.GetData("GetNumberOfOrgsByAdmin1", reportParams);

            // reutrn xml of datatable.
            context.Response.Write(GetReportData(dt));
        }

        private object[] GetReportParam(HttpContext context)
        {
            int val = 0;
            
            if (context.Request["lng"] != null)
            {
                int.TryParse((string)context.Request["lng"], out val);
            }
            int langId = (val == 1 || val == 2) ? val : 1;

            val = 0;
            if (context.Request["cluster"] != null)
            {
                int.TryParse((string)context.Request["cluster"], out val);
            }
            int? cluster = val > 0 ? val : (int?)null;

            val = 0;            
            if (context.Request["month"] != null)
            {
                int.TryParse((string)context.Request["month"], out val);
            }
            int? month = val > 0 ? val : (int?)null;

            
            return new object[] { langId, cluster, month };
        }

        // Make xml of datatable and return.
        private string GetReportData(DataTable dt)
        {
            using (var sw = new StringWriter())
            {
                dt.WriteXml(sw);
                return sw.ToString();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}