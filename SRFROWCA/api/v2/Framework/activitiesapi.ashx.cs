using BusinessLogic;
using SRFROWCA.Common;
using System.Data;
using System.IO;
using System.Web;

namespace SRFROWCA.api.v2.Framework
{
    /// <summary>
    /// Summary description for activitiesapi
    /// </summary>
    public class activitiesapi : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            string procedureName = "GetFrameworkActivities_API"; 
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

            return new object[] { countryIds, clusterIds, yearId, lng };
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