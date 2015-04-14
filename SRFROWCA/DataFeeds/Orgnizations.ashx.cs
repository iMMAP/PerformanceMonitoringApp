using BusinessLogic;
using SRFROWCA.Admin.DataFeeds;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace SRFROWCA.DataFeeds
{
    /// <summary>
    /// Summary description for Orgnizations
    /// </summary>
    public class Orgnizations : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            DataSet ds = new DataSet();
            DataTable dt = DBContext.GetData("GetOrganizationsListing", new object[] {DBNull.Value, DBNull.Value, DBNull.Value} );
            ds = dt.DataSet;
            ds.DataSetName = "Organizations";
            ds.Tables[0].TableName = "Organization";
            context.Response.Write(GetReportData(ds));
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