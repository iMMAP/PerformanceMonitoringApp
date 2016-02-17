using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
namespace SRFROWCA.DataFeeds
{
    /// <summary>
    /// Summary description for ReportBasic
    /// </summary>
    public class ReportBasic : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);

            DataTable dt = DBContext.GetData("GetReportedData2016_Basic", param);

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
                ds.DataSetName = "ReportsBasic";
                ds.Tables[0].TableName = "ReportBasic";
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
            if (!string.IsNullOrEmpty(context.Request["project"]))
            {
                int.TryParse(context.Request["project"].ToString(), out val);
            }
            int? projectId = val > 0 ? val : (int?)null;

            val = 0;
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
            if (context.Request["org"] != null)
            {
                int.TryParse(context.Request["org"].ToString(), out val);
            }
            int? orgId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["partner"] != null)
            {
                int.TryParse(context.Request["partner"].ToString(), out val);
            }
            int? ipId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["month"] != null)
            {
                int.TryParse(context.Request["month"].ToString(), out val);
            }
            int? monthId = val > 0 ? val : (int?)null;


            return new object[] { projectId, countryId, clusterId, orgId, ipId, monthId };
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