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
    /// Summary description for ClusterIndicatorReports16
    /// </summary>
    public class ClusterIndicatorReports16 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("GetClusterReportsFeed_16", param);
            RemoveColumnsFromDataTable(dt);

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
                ds.DataSetName = "OutputIndicatorReports";
                ds.Tables[0].TableName = "OutputIndicatorReport";
                context.Response.Write(GetReportData(ds));
            }
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            try
            {
                dt.Columns.Remove("EmergencyLocationIdSahel");
                dt.Columns.Remove("SiteLanguageId");
                dt.Columns.Remove("CreatedById");
                dt.Columns.Remove("UpdatedById");
            }
            catch { }
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
            if (context.Request["regional"] != null)
            {
                int.TryParse(context.Request["regional"].ToString(), out val);
            }
            int isRegional = val == 1 ? val : 0;

            string monthIds = null;
            if (!string.IsNullOrEmpty(context.Request["month"]))
            {
                monthIds = context.Request["month"].ToString();
            }

            string lng = "fr";
            if (context.Request["lng"] != null)
            {
                lng = context.Request["lng"].ToString();
            }

            return new object[] { countryId, clusterId, lng, monthIds, isRegional };
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