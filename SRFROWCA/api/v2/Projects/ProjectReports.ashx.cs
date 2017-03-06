using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace SRFROWCA.api.v2.Projects
{
    /// <summary>
    /// Summary description for ProjectReports
    /// </summary>
    public class ProjectReports : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);

            string procedureName = "GetProjectReports";
            
            DataTable dt = DBContext.GetData(procedureName, param);

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
            string countryIds = null;
            if (!string.IsNullOrEmpty(context.Request["country"]))
            {
                countryIds = context.Request["country"].ToString();
            }

            string clusterIds = null;
            if (!string.IsNullOrEmpty(context.Request["cluster"]))
            {
                clusterIds = context.Request["cluster"].ToString();
            }

            string monthIds = null;
            if (!string.IsNullOrEmpty(context.Request["month"]))
            {
                monthIds = context.Request["month"].ToString();
            }

            string orgId = null;
            if (!string.IsNullOrEmpty(context.Request["org"]))
            {
                orgId = context.Request["org"].ToString();
            }

            string prjIds = null;
            if (!string.IsNullOrEmpty(context.Request["pid"]))
            {
                prjIds = context.Request["pid"].ToString();
            }

            int? isOPS = null;
            if (context.Request["isops"] != null)
            {
                string queryVal = context.Request["isops"].ToString();

                if (queryVal == "yes")
                    isOPS = 1;
                else if (queryVal == "no")
                    isOPS = 0;
            }

            int? isApproved = null;
            if (context.Request["appr"] != null)
            {
                string queryVal = context.Request["appr"].ToString();

                if (queryVal == "yes")
                    isApproved = 1;
                else if (queryVal == "no")
                    isApproved = 0;
            }

            int? isCP = null;
            if (context.Request["cp"] != null)
            {
                string queryVal = context.Request["cp"].ToString();

                if (queryVal == "yes")
                    isApproved = 1;
                else if (queryVal == "no")
                    isApproved = 0;
            }


            int val = 0;
            int? yearId = null;
            if (context.Request["year"] != null)
            {
                int.TryParse(context.Request["year"].ToString(), out val);
                if (val == 2015)
                    yearId = (int)RC.Year._2015;
                else if (val == 2016)
                    yearId = (int)RC.Year._2016;
                else if (val == 2017)
                    yearId = (int)RC.Year._2017;
            }
          
            int langId = (int)RC.SiteLanguage.French;
            if (context.Request["lng"] != null)
            {
                string lng = context.Request["lng"].ToString();
                if (lng == "en")
                    langId = (int)RC.SiteLanguage.English;
            }

            return new object[] {monthIds, countryIds, clusterIds, orgId, prjIds, isOPS, isApproved, 
                                    isCP, langId, yearId };

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