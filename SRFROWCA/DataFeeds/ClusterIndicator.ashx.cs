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
    /// Summary description for ClusterIndicator
    /// </summary>
    public class ClusterIndicator : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("ClusterIndicatorFeed", param);

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
                ds.DataSetName = "OutputIndicators";
                ds.Tables[0].TableName = "OutputIndicator";
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
            if (!string.IsNullOrEmpty(context.Request["cluster"]))
            {
                int.TryParse(context.Request["cluster"].ToString(), out val);
            }
            int? clusterId = val > 0 ? val : (int?)null;

            val = 0;
            int isReg = 1;
            if (context.Request["addregional"] != null)
            {
                string queryVal = context.Request["addregional"].ToString();
                if (queryVal == "No" || queryVal == "no" || queryVal == "n" || queryVal == "0")
                    isReg = 0;
            }

            val = 0;
            int isAdmin = 0;
            if (context.Request["admin1target"] != null)
            {
                string queryVal = context.Request["admintarget"].ToString();
                if (queryVal == "Yes" || queryVal == "yes" || queryVal == "y" || queryVal == "1")
                    isAdmin = 1;
            }

            int? yearId = null;
            if (context.Request["year"] != null)
            {
                int.TryParse(context.Request["year"].ToString(), out val);
                yearId = val == 2015 ? (int)RC.Year._2015 : (int)RC.Year._2016;
            }

            string lng = "fr";
            if (context.Request["lng"] != null)
            {
                lng = context.Request["lng"].ToString();
            }

            return new object[] { countryId, clusterId, yearId, isReg, isAdmin, lng};
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