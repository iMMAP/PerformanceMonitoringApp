using BusinessLogic;
using SRFROWCA.Admin.DataFeeds;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace SRFROWCA.DataFeeds
{
    /// <summary>
    /// Summary description for Country
    /// </summary>
    public class Country : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {            
            DataTable dt = DBContext.GetData("GetLocationOnType", new object[] { 2 });
            string format = "xml";
            if (!string.IsNullOrEmpty(context.Request["format"]))
            {
                format = context.Request["format"].ToString();
            }

            if (format == "json")
            {
                context.Response.ContentType = "text/plain";
                string strJson = DataTableToJson.DataTableToJsonBySerializer(dt);
                context.Response.Write(strJson);
            }
            else
            {
                context.Response.ContentType = "text/xml";
                DataSet ds = new DataSet();
                ds = dt.DataSet;
                ds.DataSetName = "Countries";
                ds.Tables[0].TableName = "Contry";
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