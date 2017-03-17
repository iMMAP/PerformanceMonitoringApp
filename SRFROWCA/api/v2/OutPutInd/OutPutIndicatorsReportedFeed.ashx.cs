using BusinessLogic;
using SRFROWCA.Common;
using System.Data;
using System.IO;
using System.Web;
namespace SRFROWCA.api.v2.OutPutInd
{
    /// <summary>
    /// Summary description for OutPutIndicatorsReportedFeed
    /// </summary>
    public class OutPutIndicatorsReportedFeed : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);

            //int val = 0;
            string procedureName = "OutPutIndicatorRedportedDataFeed_OnlyReportedAtAdmin1";

            int reportLevel = 0;
            if (!string.IsNullOrEmpty(context.Request["rlevel"]))
                int.TryParse(context.Request["rlevel"].ToString(), out reportLevel);
            
            int reportType = (int)ItemizeAPI.ReportLevel.NotSelected;
            if (!string.IsNullOrEmpty(context.Request["rtype"]))
            {
                int.TryParse(context.Request["rtype"].ToString(), out reportType);
                if (reportLevel == (int)ItemizeAPI.ReportLevel.NotSelected && reportType == (int)ItemizeAPI.ReportType.OnlyReported)
                    procedureName = "OutPutIndicatorReportedDataFeed_OnlyReportedAtAdmin1";
                else if (reportLevel == (int)ItemizeAPI.ReportLevel.Country && reportType == (int)ItemizeAPI.ReportType.OnlyReported)
                    procedureName = "OutPutIndicatorReportedDataFeed_OnlyReportedAtCountry";
                else if (reportLevel == (int)ItemizeAPI.ReportLevel.NotSelected && reportType == (int)ItemizeAPI.ReportType.MonthlyStatus)
                    procedureName = "OutPutIndicatorReportedDataFeed_MonthlyStatusAtAdmin1";
                else if (reportLevel == (int)ItemizeAPI.ReportLevel.Country && reportType == (int)ItemizeAPI.ReportType.MonthlyStatus)
                    procedureName = "OutPutIndicatorReportedDataFeed_MonthlyStatusAtCountry";
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

            int? isReg = null;
            if (context.Request["reg"] != null)
            {
                string queryVal = context.Request["reg"].ToString();
                if (queryVal == "No" || queryVal == "no" || queryVal == "n" || queryVal == "NO" || queryVal == "0")
                    isReg = 0;

                if (queryVal == "Yes" || queryVal == "yes" || queryVal == "y" || queryVal == "YES" || queryVal == "1")
                    isReg = 1;
            }

            int inclIds = 0;
            if (context.Request["inclids"] != null)
            {
                string queryVal = context.Request["inclids"].ToString();

                if (queryVal == "Yes" || queryVal == "yes" || queryVal == "y" || queryVal == "YES" || queryVal == "1")
                    inclIds = 1;
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

            return new object[] { countryIds, clusterIds, monthIds, isReg, inclIds, yearId, lng };

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