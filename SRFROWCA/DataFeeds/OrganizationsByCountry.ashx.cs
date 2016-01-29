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
    /// Summary description for OrganizationsByCountry
    /// </summary>
    public class OrganizationsByCountry : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";

            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("GetOrganizationsByCountry_Feed", param);
            ds = dt.DataSet;
            ds.DataSetName = "CountryOrganizations";
            ds.Tables[0].TableName = "CountryOrganization";
            context.Response.Write(GetReportData(ds));
        }

        // Make xml of datatable and return.
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
            if (context.Request["org"] != null)
            {
                int.TryParse(context.Request["org"].ToString(), out val);
            }
            int? orgId = val > 0 ? val : (int?)null;

            val = 0;
            int? yearId = (int)RC.Year._2016;
            if (context.Request["year"] != null)
            {
                int.TryParse(context.Request["year"].ToString(), out val);
                yearId = val == 2015 ? (int)RC.Year._2015 : (int)RC.Year._2016;
            }

            return new object[] { countryId, orgId, yearId };
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