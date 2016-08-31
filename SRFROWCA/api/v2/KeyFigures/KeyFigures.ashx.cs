using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;

namespace SRFROWCA.api.v2.KeyFigures
{
    /// <summary>
    /// Summary description for KeyFigures
    /// </summary>
    public class KeyFigures : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("GetKeyFiguresApiFeed", param);

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
                ds.DataSetName = "KeyFigures";
                ds.Tables[0].TableName = "KeyFigure";
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

            string subCatIds = null;
            val = 0;
            if (context.Request["subcat"] != null)
            {
                subCatIds = context.Request["subcat"].ToString();
            }

            val = 0;
            if (context.Request["keyfigure"] != null)
            {
                int.TryParse(context.Request["keyfigure"].ToString(), out val);
            }
            int? kfIndId = val > 0 ? val : (int?)null;

            DateTime date = DateTime.MinValue;
            if (context.Request["datefrom"] != null)
                DateTime.TryParseExact(context.Request["datefrom"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            
            DateTime? dateFrom = null;
            if (date != DateTime.MinValue)
            {
                dateFrom = date;
            }

            date = DateTime.MinValue;
            if (context.Request["dateto"] != null)
            {
                DateTime.TryParseExact(context.Request["dateto"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            }
            DateTime? dateTo = null;
            if (date != DateTime.MinValue)
            {
                dateTo = date;
            }

            int final = 0;
            if (context.Request["final"] != null)
            {
                if (!string.IsNullOrEmpty(context.Request["final"].ToString()))
                {
                    int.TryParse(context.Request["final"].ToString(), out final);
                    final = final == 1 ? 1 : 0;
                }
            }

            int inclIds = 0;
            if (context.Request["inclids"] != null)
            {
                string queryVal = context.Request["inclids"].ToString();

                if (queryVal == "Yes" || queryVal == "yes" || queryVal == "y" || queryVal == "YES" || queryVal == "1")
                    inclIds = 1;
            }

            string lng = "fr";
            if (context.Request["lng"] != null)
            {
                lng = context.Request["lng"].ToString();
            }

            return new object[] { countryIds, subCatIds, kfIndId, dateFrom, dateTo, final, inclIds, lng };
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