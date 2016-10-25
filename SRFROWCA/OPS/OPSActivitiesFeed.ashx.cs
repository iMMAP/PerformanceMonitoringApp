using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using BusinessLogic;
using SRFROWCA.Configurations;
using SRFROWCA.Common;

namespace SRFROWCA.OPS
{
    /// <summary>
    /// Summary description for OPSActivitiesFeed
    /// </summary>
    public class OPSActivitiesFeed : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int dataCheck = GetDataCheckParam(context);
            int projectId = GetProjectIdParam(context);
            int year = GetYearParam(context);

            if (dataCheck == 1)
            {
                DataSet dsResults = new DataSet();
                DataTable dtResults = DBContext.GetData("uspOPSActivitiesFeed", new object[] { projectId });
                dsResults = dtResults.DataSet;
                dsResults.DataSetName = "ors";
                dsResults.Tables[0].TableName = "ors_webservice";

                context.Response.Write(GetReportData(dsResults));
            }
            else
            {
                context.Response.Write(GetXMLOfProjectData(projectId, year));
            }
        }

        private int GetProjectIdParam(HttpContext context)
        {
            int projectId = 0;
            if (context.Request["pid"] != null)
            {
                int.TryParse(context.Request["pid"].ToString(), out projectId);
            }

            return projectId;
        }

        private int GetYearParam(HttpContext context)
        {
            int yearId = 0;
            if (context.Request["year"] != null)
            {
                int.TryParse(context.Request["year"].ToString(), out yearId);
            }

            return yearId;
        }

        private int GetDataCheckParam(HttpContext context)
        {
            int dataCheck = 0;
            if (context.Request["datacheck"] != null)
            {
                int.TryParse(context.Request["datacheck"].ToString(), out dataCheck);
            }

            return dataCheck;
        }

        private string GetXMLOfProjectData(int projectId, int year)
        {
            XDocument doc = new XDocument();
            DataTable dt = new DataTable();
            //if (year == 2015)
            //{
            //    dt = DBContext.GetData("GetOPSProjectData2", new object[] { projectId });
            //    OpsXMLFeed2015.GetOPSFeed(dt, doc, projectId, year);
            //}
            if (year == 2014)
            {
                dt = DBContext.GetData("GetOPSProjectData", new object[] { projectId });
                OpsXMLFeed2015.GetOPSFeed(dt, doc, projectId, year);
            }
            else
            {
                DataTable dtProjInfo = DBContext.GetData("GetProjCountryAndCluster", new object[] { projectId });
                int emgLocId = 0;
                int emgClusterId = 0;
                if (dtProjInfo.Rows.Count > 0)
                {
                    int.TryParse(dtProjInfo.Rows[0]["EmergencyLocationId"].ToString(), out emgLocId);
                    int.TryParse(dtProjInfo.Rows[0]["EmergencyClusterId"].ToString(), out emgClusterId);
                }

                AdminTargetSettingItems items = RC.AdminTargetSettings(emgLocId, emgClusterId, year);
                int locationTypeId = (items.Category == RC.LocationCategory.Health) ? (int)RC.LocationCategory.Health
                                                                                        : (int)RC.LocationCategory.Government;
                dt = DBContext.GetData("GetProjectDataForOPSFeed", new object[] { projectId, locationTypeId });
                OpsXMLFeed2.GetOPSFeed(dt, doc, projectId);
            }

            return doc.ToString();
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