﻿using BusinessLogic;
using SRFROWCA.Common;
using System.Data;
using System.IO;
using System.Web;

namespace SRFROWCA.DataFeeds
{
    /// <summary>
    /// Summary description for IndicatorsClusterProjectsTargets
    /// </summary>
    public class IndicatorsClusterProjectsTargets : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("IndicatorsWithClusterAndProjectTargets", param);
            string format = "xml";
            if (!string.IsNullOrEmpty(context.Request["format"]))
                format = context.Request["format"].ToString();

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
                int.TryParse(context.Request["project"].ToString(), out val);
            int? projectId = val > 0 ? val : (int?)null;

            val = 0;
            if (!string.IsNullOrEmpty(context.Request["country"]))
                int.TryParse(context.Request["country"].ToString(), out val);
            int? countryId = val > 0 ? val : (int?)null;
            
            val = 0;
            if (context.Request["cluster"] != null)
                int.TryParse(context.Request["cluster"].ToString(), out val);
            int? clusterId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["org"] != null)
                int.TryParse(context.Request["org"].ToString(), out val);
            int? orgId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["act"] != null)
                int.TryParse(context.Request["act"].ToString(), out val);
            int? actId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["ind"] != null)
                int.TryParse(context.Request["ind"].ToString(), out val);
            int? indId = val > 0 ? val : (int?)null;

            string status = null;
            if (!string.IsNullOrEmpty(context.Request["status"]))
                status = context.Request["status"].ToString();

            val = 0;
            if (context.Request["year"] != null)
                int.TryParse(context.Request["year"].ToString(), out val);
            int yearId = val > 0 ? val : (int)RC.Year._2016;

            string lng = "fr";
            if (context.Request["lng"] != null)
                lng = context.Request["lng"].ToString();


            return new object[] { projectId, countryId, clusterId, orgId, actId, indId, status, yearId, lng };
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