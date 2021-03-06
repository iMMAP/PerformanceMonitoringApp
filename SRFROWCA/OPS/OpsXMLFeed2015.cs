﻿using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using BusinessLogic;

namespace SRFROWCA.OPS
{
    public static class OpsXMLFeed2015
    {
        public static string GetOPSFeed(DataTable dt, XDocument doc, int projectId, int year)
        {
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

                WriteXML(logFrameValues, filteredTable, year);
            }
            return doc.ToString();
        }

        private static void WriteXML(XElement logFrameValues, DataTable dt, int year)
        {
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                //string logFrameType = row["LogFrameType"].ToString();
                string projectId = row["OPSProjectId"].ToString();

                string strObjId = row["StrategicObjectiveId"].ToString();
                string strObjName = row["StrategicObjectiveName"].ToString();
                string priorityId = "";
                string priority = "";
                string clusterPartnerId = "";
                string clusterPartner = "";
                if (year == 2014)
                {
                    priorityId = row["HumanitarianPriorityId"].ToString();
                    priority = row["HumanitarianPriority"].ToString();

                    clusterPartnerId = row["ClusterPartnerId"].ToString();
                    clusterPartner = row["ClusterPartner"].ToString();
                }
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
                if (year == 2014)
                {
                    logFrame.Add(GetElement("Priority", priorityId, priority));
                    logFrame.Add(GetElement("ClusterPartner", clusterPartnerId, clusterPartner));
                }
                logFrame.Add(GetElement("Activity", priorityActivityId, activityName));
                logFrame.Add(GetElement("OutputIndicator", outputIndicatorId, outputIndicator));

                foreach (DataRow locRow in dt.Rows)
                {
                    XElement locationElement = new XElement("Locations");

                    string locName = locRow["LocationName"].ToString();
                    string locPCode = locRow["PCode"].ToString();
                    string countryName = locRow["CountryName"].ToString();
                    string targetMid2014 = "";
                    if (year == 2014)
                    {
                        targetMid2014 = locRow["TargetMid2014"].ToString();
                    }
                    string target2014 = locRow["Target2014"].ToString();

                    locationElement.Add(GetLocationElement("Location", locPCode, locName));
                    if (year == 2014)
                    {
                        locationElement.Add(GetTargetElement("TargetMid", targetMid2014, targetMid2014, year.ToString()));
                    }
                    locationElement.Add(GetTargetElement("Target", target2014, target2014, year.ToString()));
                    logFrame.Add(locationElement);
                }
            }
        }

        private static XElement GetElement(string name, string idValue, string nameValue)
        {
            XElement element = new XElement(name);
            element.Value = nameValue;
            return element;
        }

        private static XElement GetElement(string text)
        {
            XElement element = new XElement("Name");
            element.Value = text;
            return element;
        }

        private static XElement GetTargetElement(string name, string idValue, string nameValue, string year)
        {
            XElement element = new XElement(name);
            element.SetAttributeValue("Year", year);
            element.Value = nameValue;
            return element;
        }

        private static XElement GetLocationElement(string name, string idValue, string nameValue)
        {
            XElement element = new XElement(name);
            element.SetAttributeValue("pcode", idValue);
            element.SetAttributeValue("adminLevel", "1");
            element.Value = nameValue;
            return element;
        }
    }
}