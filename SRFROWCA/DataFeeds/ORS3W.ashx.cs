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
    /// Summary description for ORS3W
    /// </summary>
    public class ORS3W : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("GetORS3WDataFeed", param);

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

            if (!string.IsNullOrEmpty(context.Request["prj"]))
            {
                int.TryParse(context.Request["prj"].ToString(), out val);
            }
            int? prjId = val > 0 ? val : (int?)null;

            if (!string.IsNullOrEmpty(context.Request["org"]))
            {
                int.TryParse(context.Request["org"].ToString(), out val);
            }
            int? orgId = val > 0 ? val : (int?)null;

            if (!string.IsNullOrEmpty(context.Request["country"]))
            {
                int.TryParse(context.Request["country"].ToString(), out val);
            }
            int? countryId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["admin1"] != null)
            {
                int.TryParse(context.Request["admin1"].ToString(), out val);
            }
            int? admin1Id = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["cluster"] != null)
            {
                int.TryParse(context.Request["cluster"].ToString(), out val);
            }
            int? clusterId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["month"] != null)
            {
                int.TryParse(context.Request["month"].ToString(), out val);
            }
            int? monthId = val > 0 ? val : (int?)null;

            val = 0;

            string status = null;
            if (context.Request["status"] != null)
            {
                status = context.Request["status"].ToString();
            }

            string lng = "fr";
            if (context.Request["lng"] != null)
            {
                lng = context.Request["lng"].ToString();
            }

            return new object[] { prjId, orgId, countryId, admin1Id, clusterId, monthId, status, lng };
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