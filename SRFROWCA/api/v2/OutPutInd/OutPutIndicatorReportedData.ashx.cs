using BusinessLogic;
using SRFROWCA.Common;
using System.Data;
using System.IO;
using System.Web;

namespace SRFROWCA.api.v2.OutPutInd
{
    /// <summary>
    /// Summary description for OutPutIndicatorReportedData
    /// </summary>
    public class OutPutIndicatorReportedData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("OutPutIndicatorReportedDataFeed", param);

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
                ds.DataSetName = "OutputIndicators";
                ds.Tables[0].TableName = "OutputIndicator";
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
            string countryIds = null;
            if (!string.IsNullOrEmpty(context.Request["country"]))
            {
                countryIds = context.Request["country"].ToString();
            }

            val = 0;
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

            val = 0;
            int? isReg = null;
            if (context.Request["reg"] != null)
            {
                string queryVal = context.Request["reg"].ToString();
                if (queryVal == "No" || queryVal == "no" || queryVal == "n" || queryVal == "NO" || queryVal == "0")
                    isReg = 0;

                if (queryVal == "Yes" || queryVal == "yes" || queryVal == "y" || queryVal == "YES" || queryVal == "1")
                    isReg = 1;
            }

            int? yearId = null;
            if (context.Request["year"] != null)
            {
                int.TryParse(context.Request["year"].ToString(), out val);
                yearId = val == 2015 ? (int)RC.Year._2015 : (int)RC.Year._2016;
            }

            int? enFR = 0;
            if (context.Request["enfr"] != null)
            {
                int.TryParse(context.Request["enfr"].ToString(), out val);
                enFR = val == 1 ? 1 : 0;
            }
            string lng = "fr";
            if (context.Request["lng"] != null)
            {
                lng = context.Request["lng"].ToString();
            }

            return new object[] { countryIds, clusterIds, monthIds, isReg, yearId, enFR, lng  };

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