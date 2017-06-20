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
    /// Summary description for ProjectSRPTargets
    /// </summary>
    public class ProjectSRPTargets : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);

            int? yearId = (int)RC.Year._Current;
            if (context.Request["year"] != null)
            {
                int val = 0;
                int.TryParse(context.Request["year"].ToString(), out val);

                RC.Year yearEnum;
                if (Enum.TryParse("_" + val.ToString(), out yearEnum))
                    yearId = (int)yearEnum;

                if (yearId <= 0)
                    yearId = (int)RC.Year._Current;
            }

            DataTable dt = new DataTable();
            if (yearId == 11)
            {
                dt = DBContext.GetData("ProjectSRPTargets", param);
            }
            else
            {
                dt = DBContext.GetData("ProjectSRPTargets_2016", param);
            }

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
            if (context.Request["subcluster"] != null)
            {
                int.TryParse(context.Request["subcluster"].ToString(), out val);
            }
            int? subClusterId = val > 0 ? val : (int?)null;

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

            string status = null;
            if (!string.IsNullOrEmpty(context.Request["status"]))
            {
                status = context.Request["status"].ToString();
            }

            string targetLoc = null;
            if (!string.IsNullOrEmpty(context.Request["tloc"]))
            {
                targetLoc = context.Request["tloc"].ToString();
            }

            int? yearId = (int)RC.Year._Current;
            if (context.Request["year"] != null)
            {
                val = 0;
                int.TryParse(context.Request["year"].ToString(), out val);

                RC.Year yearEnum;
                if (Enum.TryParse("_" + val.ToString(), out yearEnum))
                    yearId = (int)yearEnum;

                if (yearId <= 0)
                    yearId = (int)RC.Year._Current;

            }

            string lng = "fr";
            if (context.Request["lng"] != null)
            {
                lng = context.Request["lng"].ToString();
            }

            return new object[] { projectId, countryId, subClusterId, clusterId, orgId, actId, indId, status, targetLoc, yearId, lng };
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