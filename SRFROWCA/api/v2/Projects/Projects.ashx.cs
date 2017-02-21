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
    /// Summary description for Projects
    /// </summary>
    public class Projects : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);

            string procedureName = "GetProjects_API";
            if (!string.IsNullOrEmpty(context.Request["rtype"]))
            {
                int reportType = 1;
                int.TryParse(context.Request["rtype"].ToString(), out reportType);
                if (reportType == 2)
                {
                    int val = 0;
                    if (context.Request["year"] != null)
                    {
                        int.TryParse(context.Request["year"].ToString(), out val);
                        if (val == 2015)
                            procedureName = "ProjectIndicatorsWithTargets_API";
                        else 
                            procedureName = "ProjectIndicatorsWithTargets_API";
                    }
                }
            }

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

            int? isFunded = null;
            if (context.Request["isfunded"] != null)
            {
                string queryVal = context.Request["isfunded"].ToString();

                if (queryVal == "yes")
                    isFunded = 1;
                else if (queryVal == "no")
                    isFunded = 0;
            }

            int? isLCB = null;
            if (context.Request["lcb"] != null)
            {
                string queryVal = context.Request["lcb"].ToString();

                if (queryVal == "yes")
                    isLCB = 1;
                else if (queryVal == "no")
                    isLCB = 0;
            }

            int inclIds = 0;
            if (context.Request["inclids"] != null)
            {
                string queryVal = context.Request["inclids"].ToString();

                if (queryVal == "Yes" || queryVal == "yes" || queryVal == "y" || queryVal == "YES" || queryVal == "1")
                    inclIds = 1;
                else if (queryVal == "ALL" || queryVal == "all" || queryVal == "All")
                    inclIds = 2;
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

            string lng = "fr";
            if (context.Request["lng"] != null)
            {
                lng = context.Request["lng"].ToString();
            }

            return new object[] { countryIds, clusterIds, orgId, prjIds, isOPS, isLCB, inclIds, yearId, isFunded, lng };
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