using BusinessLogic;
using SRFROWCA.Common;
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