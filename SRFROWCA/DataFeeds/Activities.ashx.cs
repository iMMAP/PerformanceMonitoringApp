using BusinessLogic;
using SRFROWCA.Admin.DataFeeds;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace SRFROWCA.DataFeeds
{
    /// <summary>
    /// Summary description for Activities
    /// </summary>
    public class Activities : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";

            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("GetFrameworkActivities", param);
            ds = dt.DataSet;
            ds.DataSetName = "Activities";
            ds.Tables[0].TableName = "Activity";
            context.Response.Write(GetReportData(ds));
        }

        // Make xml of datatable and return.
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
            string country = null;
            if (!string.IsNullOrEmpty(context.Request["country"]))
            {
                country = context.Request["country"].ToString();
            }

            string cluster = null;
            if (context.Request["cluster"] != null)
            {
                cluster = context.Request["cluster"].ToString();
            }

            string obj = null;
            if (context.Request["obj"] != null)
            {
                obj = context.Request["obj"].ToString();
            }

            string act = null;
            if (context.Request["act"] != null)
            {
                act = context.Request["act"].ToString();
            }

            return new object[] {country, cluster, obj, act};
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