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
    /// Summary description for ProjectPartners
    /// </summary>
    public class ProjectPartners : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";

            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("GetProjectPartnersFeed", param);
            ds = dt.DataSet;
            ds.DataSetName = "Partners";
            ds.Tables[0].TableName = "Partner";
            context.Response.Write(GetReportData(ds));
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
            if (context.Request["org"] != null)
            {
                int.TryParse(context.Request["org"].ToString(), out val);
            }
            int? orgId = val > 0 ? val : (int?)null;
            int? yearId = (int)RC.Year._Current;
            if (context.Request["year"] != null)
            {
                if (context.Request["year"].ToString() != "no")
                    yearId = null;
                else
                {
                    val = 0;
                    int.TryParse(context.Request["year"].ToString(), out val);

                    RC.Year yearEnum;
                    if (Enum.TryParse("_" + val.ToString(), out yearEnum))
                        yearId = (int)yearEnum;

                    if (yearId <= 0)
                        yearId = (int)RC.Year._Current;

                }
            }

            return new object[] { projectId, orgId, yearId };
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