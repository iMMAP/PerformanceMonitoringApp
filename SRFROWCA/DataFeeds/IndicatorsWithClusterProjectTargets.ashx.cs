using BusinessLogic;
using System.Data;
using System.IO;
using System.Web;

namespace SRFROWCA.DataFeeds
{
    /// <summary>
    /// Summary description for IndicatorsWithClusterProjectTargets
    /// </summary>
    public class IndicatorsWithClusterProjectTargets : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("GetAllIndicatorsClusterProjectAndReportsTargets", param);

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
                ds.DataSetName = "Indicators";
                ds.Tables[0].TableName = "Indicator";
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
            if (context.Request["objective"] != null)
            {
                int.TryParse(context.Request["objective"].ToString(), out val);
            }
            int? objId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["admin1id"] != null)
            {
                int.TryParse(context.Request["admin1id"].ToString(), out val);
            }
            int? admin1Id = val > 0 ? val : (int?)null;

            val = 0;
            int? isOPS = null;
            if (context.Request["ops"] != null)
            {
                bool isParsed = int.TryParse(context.Request["ops"].ToString(), out val);
                if (isParsed)
                {
                    isOPS = (val == 0 || val == 1) ? val : (int?)null;
                }
            }

            string lng = "fr";
            if (context.Request["lng"] != null)
            {
                lng = context.Request["lng"].ToString();
            }

            return new object[] { countryId, clusterId, objId, admin1Id, isOPS, lng };
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