using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BusinessLogic;
using System.IO;

namespace SRFROWCA.Admin.DataFeeds
{
    /// <summary>
    /// Summary description for YearFeed
    /// </summary>
    public class YearFeed : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            DataTable dt = DBContext.GetData("");

            // reutrn xml of datatable.
            context.Response.Write(GetData(dt));
        }

        private string GetData(DataTable dt)
        {
            using (var sw = new StringWriter())
            {
                dt.WriteXml(sw);
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