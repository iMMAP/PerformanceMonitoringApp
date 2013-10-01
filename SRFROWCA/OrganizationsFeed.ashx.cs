using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BusinessLogic;
using System.IO;

namespace SRFROWCA
{
    /// <summary>
    /// Summary description for OrganizationsFeed
    /// </summary>
    public class OrganizationsFeed : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            DataTable dt = DBContext.GetData("GetAllOrganizations");

            // reutrn xml of datatable.
            context.Response.Write(GetReportData(dt));
        }

        // Make xml of datatable and return.
        private string GetReportData(DataTable dt)
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