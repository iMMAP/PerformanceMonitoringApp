using BusinessLogic;
using SRFROWCA.Admin.DataFeeds;
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
            if (!string.IsNullOrEmpty(context.Request["cluster"]))
            {
                cluster = context.Request["cluster"].ToString();
            }

            string obj = null;
            if (!string.IsNullOrEmpty(context.Request["obj"] ))
            {
                obj = context.Request["obj"].ToString().NullIfEmpty(); 
            }

            string act = null;
            if (!string.IsNullOrEmpty(context.Request["act"] ))
            {
                act = context.Request["act"].ToString().NullIfEmpty();
            }

            int? yearId = (int)RC.Year._2016;
            if (context.Request["year"] != null)
            {
                if (context.Request["year"].ToString() != "no")
                    yearId = null;
                else
                {
                    int val = 0;
                    int.TryParse(context.Request["year"].ToString(), out val);
                    yearId = val == 2015 ? (int)RC.Year._2015 : (int)RC.Year._2016;
                }
            }

            return new object[] {country, cluster, obj, act, yearId};
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