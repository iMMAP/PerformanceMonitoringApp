using System.Data;
using System.IO;
using System.Web;
using BusinessLogic;

namespace SRFROWCA.Admin.DataFeeds
{
    /// <summary>
    /// Summary description for Organizations
    /// </summary>
    public class Organizations : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet(); 
            DataTable dt = DBContext.GetData("GetAllOrganizations");

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
                ds.DataSetName = "Organizations";
                ds.Tables[0].TableName = "Organization";
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}