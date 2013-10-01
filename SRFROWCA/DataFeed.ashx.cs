using System.Data;
using System.IO;
using System.Web;
using BusinessLogic;
using System;

namespace SRFROWCA
{
    /// <summary>
    /// Return xml of reprot data.
    /// </summary>
    public class DataFeed : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            object[] reportParams = GetReportParam(context);
            DataTable dt = DBContext.GetData("GetAllTasksDataReport1", reportParams);

            // reutrn xml of datatable.
            context.Response.Write(GetReportData(dt));
        }

        // Get all parameters from querystring.
        private object[] GetReportParam(HttpContext context)
        {
            string emergency = null;
            if (context.Request["emg"] != null)
            {
                emergency = (string)context.Request["emg"];
            }
            emergency = !string.IsNullOrEmpty(emergency) ? emergency : null;

            string clusters = null;
            if (context.Request["cls"] != null)
            {
                clusters = (string)context.Request["cls"];
            }
            clusters = !string.IsNullOrEmpty(clusters) ? clusters : null;

            string locations = null;
            if (context.Request["loc"] != null)
            {
                locations = (string)context.Request["loc"];
            }
            locations = !string.IsNullOrEmpty(locations) ? locations : null;

            int val = 0;
            if (context.Request["y"] != null)
            {
                int.TryParse((string)context.Request["y"], out val);
            }
            int? yearId = val > 0 ? val : (int?)null;

            string months = null;
            if (context.Request["m"] != null)
            {
                months = (string)context.Request["m"];
            }
            months = !string.IsNullOrEmpty(months) ? months : null;

            string uId = null;
            if (context.Request["u"] != null)
            {
                uId = (string)context.Request["u"];
            }
            Guid userId = uId != "00000000 - 0000 - 0000 - 0000 - 000000000000" ? new Guid(uId) : new Guid();

            string orgTypes = null;
            if (context.Request["ot"] != null)
            {
                orgTypes = (string)context.Request["ot"];
            }
            orgTypes = !string.IsNullOrEmpty(orgTypes) ? orgTypes : null;

            string organizations = null;
            if (context.Request["org"] != null)
            {
                organizations = (string)context.Request["org"];
            }
            organizations = !string.IsNullOrEmpty(organizations) ? organizations : null;

            val = 0;
            if (context.Request["ofc"] != null)
            {
                int.TryParse((string)context.Request["ofc"], out val);
            }
            int? officeId = val > 0 ? val : (int?)null;

            return new object[] { emergency, officeId, userId, yearId, months, locations, clusters, organizations, orgTypes };
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