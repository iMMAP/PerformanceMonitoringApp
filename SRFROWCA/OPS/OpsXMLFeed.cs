using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using BusinessLogic;

namespace SRFROWCA.OPS
{
    public class OpsXMLFeed
    {
        public static string GetOPSFeed(DataTable dt, XDocument doc, int projectId, int year)
        {
            XElement rootElement = new XElement("ProjectActivities");
            rootElement.SetAttributeValue("OPSID", projectId);
            doc.Add(rootElement);

            var distinctRows = (from DataRow dRow in dt.Rows
                                select new
                                {
                                    IndicatorId = dRow["IndicatorId"]
                                }).Distinct();

            foreach (var item in distinctRows)
            {
                int dataId = (int)item.IndicatorId;
                IEnumerable<DataRow> query =
                        from chartData in dt.AsEnumerable()
                        where chartData.Field<int>("IndicatorId") == dataId
                        select chartData;

                // Create a table from the query.
                DataTable filteredTable = query.CopyToDataTable<DataRow>();
                WriteIndicatorMainInfo(rootElement, filteredTable);
            }
            return doc.ToString();
        }

        private static void WriteIndicatorMainInfo(XElement rootElement, DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                string objName = row["ShortObjectiveTitle"].ToString();
                string activity = row["Activity"].ToString();
                string indicatorId = row["IndicatorId"].ToString();
                string indicator = row["Indicator"].ToString();
                string unit = row["Unit"].ToString();
                string calcMethod = row["CalculationType"].ToString();
                
                // Write ProjectActivity Element with IndicatorId
                XElement indicatorElement = new XElement("ProjectActivity");                
                indicatorElement.SetAttributeValue("ID", indicatorId);
                indicatorElement.Add(GetElement("StrategicObjective", objName));
                indicatorElement.Add(GetElement("Activity", activity));
                indicatorElement.Add(GetElement("OutputIndicator", indicator));
                indicatorElement.Add(GetElement("Unit", unit));
                indicatorElement.Add(GetElement("CalcMethod", calcMethod));
                
                XElement indLocationsRoot = new XElement("Locations");
                indicatorElement.Add(indLocationsRoot);
                WriteIndLocation(dt, indLocationsRoot);
                rootElement.Add(indicatorElement);
            }
        }

        private static void WriteIndLocation(DataTable dt, XElement elemLocations)
        {
            foreach (DataRow locRow in dt.Rows)
            {
                XElement locationInnerElement = new XElement("Location");

                string admin1 = locRow["Admin1"].ToString();
                string admin1PCode = locRow["Admin1PCode"].ToString();
                string admin2 = locRow["Admin2"].ToString();
                string admin2PCode = locRow["Admin2PCode"].ToString();
                string countryName = locRow["Country"].ToString();
                string clusterTotal = locRow["ClusterTotal"].ToString();
                string clusterMale = locRow["ClusterMale"].ToString();
                string clusterFemale = locRow["ClusterFemale"].ToString();
                string projectTotal = locRow["ProjectTotal"].ToString();
                string projectMale = locRow["ProjectMale"].ToString();
                string projectFemale = locRow["ProjectFemale"].ToString();

                locationInnerElement.Add(GetLocationElement("Admin1", admin1PCode, admin1, "1"));
                locationInnerElement.Add(GetLocationElement("Admin2", admin2PCode, admin2, "2"));
                locationInnerElement.Add(GetTargetElement("ClusterMale", clusterMale));
                locationInnerElement.Add(GetTargetElement("ClusterFemale", clusterFemale));
                locationInnerElement.Add(GetTargetElement("ClusterTotal", clusterTotal));
                locationInnerElement.Add(GetTargetElement("ProjectMale", projectMale));
                locationInnerElement.Add(GetTargetElement("ProjectFemale", projectFemale));
                locationInnerElement.Add(GetTargetElement("ProjectTotal", projectTotal));
                elemLocations.Add(locationInnerElement);
            }
        }

        private static XElement GetElement(string name, string nameValue)
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

        private static XElement GetTargetElement(string name, string nameValue)
        {
            XElement element = new XElement(name);
            //element.SetAttributeValue("Year", year);
            element.Value = nameValue;
            return element;
        }

        private static XElement GetLocationElement(string name, string idValue, string nameValue, string levelNumber)
        {
            XElement element = new XElement(name);
            element.SetAttributeValue("pcode", idValue);
            element.SetAttributeValue("adminLevel", levelNumber);
            element.Value = nameValue;
            return element;
        }

        
    }
}