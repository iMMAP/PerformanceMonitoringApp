using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.IO;
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

            string lng = "fr";
            if (context.Request["lng"] != null)
            {
                lng = context.Request["lng"].ToString();
            }

            return new object[] {countryId, clusterId, yearId, lng};
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