using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace SRFROWCA.DataFeeds
{
    /// <summary>
    /// Summary description for KeyFiguresFeed
    /// </summary>
    public class KeyFiguresFeed : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("GetKeyFiguresFeed", param);

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

            val = 0;
            if (!string.IsNullOrEmpty(context.Request["country"]))
            {
                int.TryParse(context.Request["country"].ToString(), out val);
            }
            int? countryId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["category"] != null)
            {
                int.TryParse(context.Request["category"].ToString(), out val);
            }
            int? catId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["subcategory"] != null)
            {
                int.TryParse(context.Request["subcategory"].ToString(), out val);
            }
            int? subCatId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["keyfigure"] != null)
            {
                int.TryParse(context.Request["keyfigure"].ToString(), out val);
            }
            int? kfIndId = val > 0 ? val : (int?)null;

            DateTime date = DateTime.MinValue;
            if (context.Request["datefrom"] != null)
            {
                DateTime.TryParseExact(context.Request["datefrom"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            }

            DateTime? dateFrom = null;
            if (date != DateTime.MinValue)
            {
                dateFrom = date;
            }

            date = DateTime.MinValue;
            if (context.Request["dateto"] != null)
            {
                DateTime.TryParseExact(context.Request["dateto"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            }
            DateTime? dateTo = null;
            if (date != DateTime.MinValue)
            {
                dateTo = date;
            }

            string final = null;
            if (context.Request["final"] != null)
            {
                if (!string.IsNullOrEmpty(context.Request["final"].ToString()))
                {
                    final = context.Request["final"].ToString();
                }
            }

            string lng = "fr";
            if (context.Request["lng"] != null)
            {
                lng = context.Request["lng"].ToString();
            }

            return new object[] { countryId, catId, subCatId, kfIndId, dateFrom, dateTo, final, lng };
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