using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using BusinessLogic;

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
            int projectId = GetReportParam(context);
            context.Response.Write(GetXMLOfProjectData(projectId));
        }

        private int GetReportParam(HttpContext context)
        {
            int projectId = 0;
            if (context.Request["pid"] != null)
            {
                int.TryParse(context.Request["pid"].ToString(), out projectId);
            }

            return projectId;
        }

        private string GetXMLOfProjectData(int projectId)
        {
            DataTable dt = DBContext.GetData("GetOPSProjectData", new object[] { projectId });

            XDocument doc = new XDocument();
            XElement logFrameValues = new XElement("ProjectActivities");
            logFrameValues.SetAttributeValue("OPSID", projectId);
            doc.Add(logFrameValues);

            var distinctRows = (from DataRow dRow in dt.Rows
                                select new
                                {
                                    ActivityDataId = dRow["ActivityDataId"]
                                }).Distinct();

            foreach (var item in distinctRows)
            {
                int dataId = (int)item.ActivityDataId;

                IEnumerable<DataRow> query =
                        from chartData in dt.AsEnumerable()
                        where chartData.Field<int>("ActivityDataId") == dataId
                        select chartData;                

                // Create a table from the query.
                DataTable filteredTable = query.CopyToDataTable<DataRow>();

                WriteXML(logFrameValues, filteredTable);
            }
            return doc.ToString();
        }

        private void WriteXML(XElement logFrameValues, DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                //string logFrameType = row["LogFrameType"].ToString();
                string projectId = row["OPSProjectId"].ToString();
                
                string strObjId = row["StrategicObjectiveId"].ToString();
                string strObjName = row["StrategicObjectiveName"].ToString();                
                string priorityId = row["HumanitarianPriorityId"].ToString();
                string priority = row["HumanitarianPriority"].ToString();                
                string clusterPartnerId = row["ClusterPartnerId"].ToString();
                string clusterPartner = row["ClusterPartner"].ToString();                
                string priorityActivityId = row["PriorityActivityId"].ToString();
                string activityName = row["ActivityName"].ToString();                
                string outputIndicatorId = row["OutputIndicatorId"].ToString();
                string outputIndicator = row["OutputIndicator"].ToString();

                string activityDataId = row["ActivityDataId"].ToString();
                
                string clusterName = row["ClusterName"].ToString();

                XElement logFrame = new XElement("ProjectActivity");                
                logFrame.SetAttributeValue("ID", activityDataId);
                logFrameValues.Add(logFrame);

                logFrame.Add(GetElement("StrategicObjective", strObjId, strObjName));
                logFrame.Add(GetElement("Priority", priorityId, priority));
                logFrame.Add(GetElement("ClusterPartner", clusterPartnerId, clusterPartner));
                logFrame.Add(GetElement("Activity", priorityActivityId, activityName));
                logFrame.Add(GetElement("OutputIndicator", outputIndicatorId, outputIndicator));

                foreach (DataRow locRow in dt.Rows)
                {
                    XElement locationElement = new XElement("Locations");
                    
                    string locName = locRow["LocationName"].ToString();
                    string locPCode = locRow["PCode"].ToString();
                    string countryName = locRow["CountryName"].ToString();
                    string targetMid2014 = locRow["TargetMid2014"].ToString();
                    string target2014 = locRow["Target2014"].ToString();

                    locationElement.Add(GetLocationElement("Location", locPCode, locName));
                    locationElement.Add(GetTargetElement("TargetMid", targetMid2014, targetMid2014));
                    locationElement.Add(GetTargetElement("Target", target2014, target2014));
                    logFrame.Add(locationElement);
                }
            }
        }

        private XElement GetElement(string name, string idValue, string nameValue)
        {
            XElement element = new XElement(name);
            element.Value = nameValue;
            return element;
        }

        private XElement GetElement(string text)
        {
            XElement element = new XElement("Name");
            element.Value = text;
            return element;
        }

        private XElement GetTargetElement(string name, string idValue, string nameValue)
        {
            XElement element = new XElement(name);
            element.SetAttributeValue("Year", "2014");
            element.Value = nameValue;
            return element;
        }

        private XElement GetLocationElement(string name, string idValue, string nameValue)
        {
            XElement element = new XElement(name);
            element.SetAttributeValue("pcode", idValue);
            element.SetAttributeValue("adminLevel", "1");
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