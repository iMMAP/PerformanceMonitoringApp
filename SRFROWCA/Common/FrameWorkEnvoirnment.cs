using BusinessLogic;
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Xml;

namespace SRFROWCA.Common
{
    public static class SectorFramework
    {
        private static XmlElement GetClusterDocElements(int emgLocationId, int emgClusterId)
        {
            XmlDocument doc = new XmlDocument();
            string PATH = HttpRuntime.AppDomainAppPath;
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\ChangeEndSettings.xml";
            if (File.Exists(PATH))
                doc.Load(PATH);
            return doc.DocumentElement;
        }

        public static bool DateExceeded(int emgLocationId, int emgClusterId)
        {
            DateTime frDate = DateTime.MaxValue;

            // Make Key which is saved in configuration file
            string configKey = "Key-" + emgLocationId.ToString() + emgClusterId.ToString();
            XmlNode settingsNode = GetClusterDocElements(emgLocationId, emgClusterId);
            foreach (XmlNode node in settingsNode.ChildNodes)
            {
                if (node.Name.Equals(configKey))
                {
                    if (node.Attributes["DateLimit"] != null)
                        frDate = DateTime.ParseExact(Convert.ToString(node.Attributes["DateLimit"].Value),
                                                                        "MM-dd-yyyy", CultureInfo.InvariantCulture);
                }
            }

            // If Date is less than or equal than current 
            // date then it is not exceeded, return false.
            return frDate <= DateTime.Now.Date;
        }

        public static int IndUnused(int emgLocationId, int emgClusterId)
        {
            int allowedIndicators = 0;
            string configKey = "Key-" + emgLocationId.ToString() + emgClusterId.ToString();
            XmlNode settingsNode = GetClusterDocElements(emgLocationId, emgClusterId);
            foreach (XmlNode node in settingsNode.ChildNodes)
            {
                if (node.Name.Equals(configKey))
                {
                    if (node.Attributes["FrameworkCount"] != null)
                        allowedIndicators = Convert.ToInt32(node.Attributes["FrameworkCount"].Value);
                }
            }

            int yearId = (int)RC.Year._2017;
            if (allowedIndicators > 0)
            {
                int isActive = 1;
                int indicatorCount = DBContext.Update("GetIndicatorsCount", new object[] { emgLocationId, emgClusterId, 
                                                                                            isActive, RC.SelectedSiteLanguageId, yearId, DBNull.Value });
                if (indicatorCount > 0)
                    allowedIndicators -= indicatorCount;
            }

            return allowedIndicators;
        }

        public static int ActivityUnused(int emgLocationId, int emgClusterId)
        {
            int allowedActivities = 0;
            string configKey = "Key-" + emgLocationId.ToString() + emgClusterId.ToString();
            XmlNode settingsNode = GetClusterDocElements(emgLocationId, emgClusterId);
            foreach (XmlNode node in settingsNode.ChildNodes)
            {
                if (node.Name.Equals(configKey))
                {
                    if (node.Attributes["ActivityCount"] != null)
                        allowedActivities = Convert.ToInt32(node.Attributes["ActivityCount"].Value);
                }
            }

            int yearId = (int)RC.Year._2017;
            if (allowedActivities > 0)
            {
                int isActive = 1;
                int activityCount = DBContext.Update("GetActivitiesCount", new object[] { emgLocationId, emgClusterId,
                                                                                          RC.SelectedSiteLanguageId, isActive,
                                                                                          yearId, DBNull.Value });
                if (activityCount > 0)
                    allowedActivities -= activityCount;
            }

            return allowedActivities;
        }

        public static int OutputIndUnused(int emgLocationId, int emgClusterId)
        {
            int outputIndAllowed = 0;

            string configKey = "Key-" + emgLocationId.ToString() + emgClusterId.ToString();

            XmlNode settingsNode = GetClusterDocElements(emgLocationId, emgClusterId);
            foreach (XmlNode node in settingsNode.ChildNodes)
            {
                if (node.Name.Equals(configKey))
                {
                    if (node.Attributes["ClusterCount"] != null)
                        outputIndAllowed = Convert.ToInt32(node.Attributes["ClusterCount"].Value);
                }
            }

            int yearId = (int)RC.Year._2017;
            int val = DBContext.Update("GetClusterIndicatorCount", new object[] { emgLocationId, emgClusterId, yearId, DBNull.Value });
            return (outputIndAllowed -= val);
        }
    }
}