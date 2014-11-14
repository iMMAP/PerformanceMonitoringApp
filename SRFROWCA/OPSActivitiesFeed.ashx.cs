using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace SRFROWCA
{
    /// <summary>
    /// Summary description for OPSActivitiesFeed
    /// </summary>
    public class OPSActivitiesFeed : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";

            int projectID = 0;
            if (context.Request["pid"] != null)
            {
                int.TryParse(context.Request["pid"], out projectID);
            }

            string type = null;

            if (context.Request["projectrevision"] != null)
            {
                type= context.Request["projectrevision"];
            }

            if (context.Request["datacheck"] != null)
            {
                //int.TryParse(context.Request["datacheck"], out projectID);
            }

            if (type == "R" || type == "r")
            {
                type = "Revision";
            }
            else if (type == "O" || type == "o")
            {
                type = "Original";
            }
            else
            {
                type = null;
            }


            DataSet dsResults = new DataSet();
            DataTable dtResults = DBContext.GetData("uspOPSActivitiesFeed", new object[] { projectID, type });
            dsResults = dtResults.DataSet;
            dsResults.DataSetName = "ors";
            dsResults.Tables[0].TableName = "ors_webservice";

            context.Response.Write(GetReportData(dsResults));
        }

        private string GetReportData(DataSet ds)
        {
            using (var sw = new StringWriter())
            {
                ds.WriteXml(sw, XmlWriteMode.IgnoreSchema);
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