using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using BusinessLogic;

namespace SRFROWCA.OPS
{
    public class OpsXMLFeed2
    {
        public static string GetOPSFeed(DataTable dt, XDocument doc, int projectId)
        {
            string year = null;
            if (dt.Rows.Count > 0)
                year = dt.Rows[0]["Year"].ToString();
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
                WriteIndicatorMainInfo(rootElement, filteredTable, year);
            }
            return doc.ToString();
        }

        private static void WriteIndicatorMainInfo(XElement rootElement, DataTable dt, string year)
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
                WriteIndLocation(dt, indLocationsRoot, year);
                rootElement.Add(indicatorElement);
            }
        }

        private static void WriteIndLocation(DataTable dt, XElement indicatorElement, string year)
        {
            foreach (DataRow locRow in dt.Rows)
            {
                XElement locationInnerElement = new XElement("Location");

                string admin1 = locRow["Admin1"].ToString();
                string admin1PCode = locRow["Admin1PCode"].ToString();
                string admin2 = locRow["Admin2"].ToString();
                string admin2PCode = locRow["Admin2PCode"].ToString();
                string clusterTotal = locRow["ClusterTotal"].ToString();
                string clusterMale = locRow["ClusterMale"].ToString();
                string clusterFemale = locRow["ClusterFemale"].ToString();
                string projectTotal = locRow["ProjectTotal"].ToString();
                string projectMale = locRow["ProjectMale"].ToString();
                string projectFemale = locRow["ProjectFemale"].ToString();

                string adminLeve1 = year == "2015" ? "Country" : "Admin1";
                string adminLeve2 = year == "2015" ? "Admin1" : "Admin2";

                locationInnerElement.Add(GetLocationElement("Admin1", admin1PCode, admin1, adminLeve1));
                locationInnerElement.Add(GetLocationElement("Admin2", admin2PCode, admin2, adminLeve2));
                locationInnerElement.Add(GetTargetElement("ClusterMale", clusterMale, year.ToString()));
                locationInnerElement.Add(GetTargetElement("ClusterFemale", clusterFemale, year.ToString()));
                locationInnerElement.Add(GetTargetElement("ClusterTotal", clusterTotal, year.ToString()));
                locationInnerElement.Add(GetTargetElement("ProjectMale", projectMale, year.ToString()));
                locationInnerElement.Add(GetTargetElement("ProjectFemale", projectFemale, year.ToString()));
                locationInnerElement.Add(GetTargetElement("ProjectTotal", projectTotal, year.ToString()));
                indicatorElement.Add(locationInnerElement);
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

        private static XElement GetTargetElement(string name, string nameValue, string year)
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