using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Data;
using BusinessLogic;

namespace SRFROWCA.OPS
{
    /// <summary>
    /// Summary description for OPSProjectFeed
    /// </summary>
    public class OPSProjectFeed : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            DataTable dt = GetProjectDetails(context);
            
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-16", "yes"));
            XElement logFrameValues = new XElement("Projects");

            foreach (DataRow dr in dt.Rows)
            {
                XElement logFrame = new XElement("Record");
                foreach (DataColumn dc in dt.Columns)
                {
                    string tagName = dc.ColumnName;
                    string tagValue = dr[dc].ToString().Trim();
                    logFrame.Add(GetElement(tagName, tagValue));
                }

                logFrameValues.Add(logFrame);
            }

            doc.Add(logFrameValues);
            //doc.Save("e://testing1.xml");
            context.Response.Write(doc.ToString());
        }

        private DataTable GetProjectDetails(HttpContext context)
        {
            int tempVal = 0;
            int? clusterId = null;
            if (context.Request["cls"] != null)
            {
                int.TryParse(context.Request["cls"].ToString(), out tempVal);
                clusterId = tempVal > 0 ? tempVal : (int?)null;
            }

            int? locationId = null;
            if (context.Request["loc"] != null)
            {
                tempVal = 0;
                int.TryParse(context.Request["loc"].ToString(), out tempVal);
                locationId = tempVal > 0 ? tempVal : (int?)null;
            }

            return DBContext.GetData("GetOPSProjectDetailsFeed", new object[] {clusterId, locationId});
        }

        private XElement GetElement(string name, string nameValue)
        {
            XElement element = new XElement(name);
            element.Value = nameValue;
            return element;
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