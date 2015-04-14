using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace SRFROWCA.DataFeeds
{
    /// <summary>
    /// Summary description for Indicators
    /// </summary>
    public class Indicators : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("GetFrameworkIndicators", param);

            string format = "xml";
            if (!string.IsNullOrEmpty(context.Request["format"]))
            {
                format = context.Request["format"].ToString();
            }

            if (format == "json")
            {
                context.Response.ContentType = "text/plain";
                string strJson = SRFROWCA.Common.DataTableToJson.DataTableToJsonBySerializer(dt);
                context.Response.Write(strJson);
            }
            else
            {
                context.Response.ContentType = "text/xml";
                ds = dt.DataSet;
                ds.DataSetName = "Indicators";
                ds.Tables[0].TableName = "Indicator";
                context.Response.Write(GetReportData(ds));
            }
        }

        private string GetReportData(DataSet ds)
        {
            using (var sw = new StringWriter())
            {
                ds.WriteXml(sw);
                return sw.ToString();
            }
        }

        private object[] GetReportParam(HttpContext context)
        {
            int val = 0;
            if (!string.IsNullOrEmpty(context.Request["country"]))
            {
                int.TryParse(context.Request["country"].ToString(), out val);
            }
            int? countryId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["cluster"] != null)
            {
                int.TryParse(context.Request["cluster"].ToString(), out val);
            }
            int? clusterId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["obj"] != null)
            {
                int.TryParse(context.Request["obj"].ToString(), out val);
            }
            int? objId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["act"] != null)
            {
                int.TryParse(context.Request["act"].ToString(), out val);
            }
            int? actId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["ind"] != null)
            {
                int.TryParse(context.Request["ind"].ToString(), out val);
            }
            int? indId = val > 0 ? val : (int?)null;

            return new object[] { countryId, clusterId, objId, actId, indId };
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