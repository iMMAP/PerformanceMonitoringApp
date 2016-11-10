using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.IO;
using System.Web;

namespace SRFROWCA.DataFeeds
{
    /// <summary>
    /// Summary description for Projects
    /// </summary>
    public class Projects : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("GetProjectsFeed", param);

            string format = "xml";
            if (!string.IsNullOrEmpty(context.Request["format"]))
            {
                format = context.Request["format"].ToString();
            }

            if (format == "json")
            {
                context.Response.ContentType = "text/plain";
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                string strJson = SRFROWCA.Common.DataTableToJson.DataTableToJsonBySerializer(dt);
                context.Response.Write(strJson);
            }
            else
            {
                context.Response.ContentType = "text/xml";
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                ds = dt.DataSet;
                ds.DataSetName = "Projects";
                ds.Tables[0].TableName = "Project";
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

            string status = null;
            if (!string.IsNullOrEmpty(context.Request["projectstatus"]))
            {
                status = context.Request["projectstatus"].ToString();
            }

            int? yearId = null;
            if (context.Request["year"] != null)
            {
                int.TryParse(context.Request["year"].ToString(), out val);
                if (val == 2015)
                    yearId = (int)RC.Year._2015;
                else if (val == 2016)
                    yearId = (int)RC.Year._2016;
                else
                    yearId = (int)RC.Year._2017;
            }

            val = 0;
            int? isOPS = null;
            if (context.Request["onlyops"] != null)
            {
                string queryVal = context.Request["onlyops"].ToString();
                if (queryVal == "Yes" || queryVal == "yes" || queryVal == "y" || queryVal == "1")
                    isOPS = 1;
                else if (queryVal == "No" || queryVal == "no" || queryVal == "n" || queryVal == "0")
                    isOPS = 0;
            }

            val = 0;
            int allData = 0;
            if (context.Request["allcolumns"] != null)
            {
                string queryVal = context.Request["allcolumns"].ToString();
                if (queryVal == "Yes" || queryVal == "yes" || queryVal == "y" || queryVal == "1")
                    allData = 1;
            }           

            string lng = "fr";
            if (context.Request["lng"] != null)
            {
                lng = context.Request["lng"].ToString();
            }

            return new object[] {projectId, countryId, clusterId, orgId, status, yearId, isOPS, allData, lng };
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