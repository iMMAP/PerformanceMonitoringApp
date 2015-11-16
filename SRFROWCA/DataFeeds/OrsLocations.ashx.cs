using BusinessLogic;
using System.Data;
using System.IO;
using System.Web;

namespace SRFROWCA.DataFeeds
{
    /// <summary>
    /// Summary description for OrsLocations
    /// </summary>
    public class OrsLocations : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("GetAllLocationsFeed", param);

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
                ds.DataSetName = "Locations";
                ds.Tables[0].TableName = "Location";
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
            if (!string.IsNullOrEmpty(context.Request["id"]))
            {
                int.TryParse(context.Request["id"].ToString(), out val);
            }
            int? locationId = val > 0 ? val : (int?)null;

            string name = null;
            if (!string.IsNullOrEmpty(context.Request["name"]))
            {
                name = context.Request["name"].ToString();
            }

            int? locType = null;
            if (!string.IsNullOrEmpty(context.Request["type"]))
            {
                string type = context.Request["type"].ToString();
                if (type == "country" || type == "Country")
                    locType = 2;
                else if (type == "admin1" || type == "admin1")
                    locType = 3;
                else if (type == "admin2" || type == "admin2")
                    locType = 4;
            }

            int? locCat = null;
            if (!string.IsNullOrEmpty(context.Request["category"]))
            {
                string type = context.Request["category"].ToString();
                if (type == "health" || type == "Health")
                    locCat = 2;
                else if (type == "gov" || type == "gov")
                    locCat = 1;
            }


            return new object[] { locationId, name, locType, locCat};
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