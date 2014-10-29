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
    /// Summary description for _3W
    /// </summary>
    public class _3W : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            object[] reportParams = GetReportParam(context);
            DataTable dt = DBContext.GetData("Get3W", reportParams);

            // reutrn xml of datatable.
            context.Response.Write(GetReportData(dt));
        }

        private object[] GetReportParam(HttpContext context)
        {
            string country = null;

            if (!string.IsNullOrEmpty(context.Request["country"]))
            {
                country = context.Request["country"];
            }
            return new object[] { country };
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