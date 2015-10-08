using BusinessLogic;
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Xml;
using System.Collections.Generic;
using System.Data;

namespace SRFROWCA.Common
{
    public static class FrameWorkUtil
    {
        public static FrameWorkSettingsCount GetActivityFrameworkSettings(int emgLocationId, int emgClusterId)
        {
            int maxIndicators = 0;
            int maxActivities = 0;
            DateTime endEditDate = DateTime.Now.Date;

            string configKey = "Key-" + emgLocationId.ToString() + emgClusterId.ToString();
            maxIndicators = 0;
            maxActivities = 0;
            endEditDate = DateTime.Now;

            string PATH = HttpRuntime.AppDomainAppPath;
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\ChangeEndSettings.xml";

            if (File.Exists(PATH))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(PATH);

                XmlElement elem_settings = doc.GetElementById("ChangeEndSettings");
                XmlNode settingsNode = doc.DocumentElement;

                foreach (XmlNode node in settingsNode.ChildNodes)
                {
                    if (node.Name.Equals(configKey))
                    {
                        if (node.Attributes["FrameworkCount"] != null)
                            maxIndicators = Convert.ToInt32(node.Attributes["FrameworkCount"].Value);

                        if (node.Attributes["ActivityCount"] != null)
                            maxActivities = Convert.ToInt32(node.Attributes["ActivityCount"].Value);

                        if (node.Attributes["DateLimit"] != null)
                        {
                            endEditDate = DateTime.ParseExact(Convert.ToString(node.Attributes["DateLimit"].Value), "MM-dd-yyyy", CultureInfo.InvariantCulture);
                        }
                    }
                }
            }

            int yearId = 12;
            if (maxIndicators > 0)
            {
                int isActive = 1;
                int indicatorCount = DBContext.Update("GetIndicatorsCount", new object[] { emgLocationId, emgClusterId, 
                                                                                            isActive, RC.SelectedSiteLanguageId, yearId, DBNull.Value });
                if (indicatorCount > 0)
                    maxIndicators = maxIndicators - indicatorCount;
            }

            if (maxActivities > 0)
            {
                int isActive = 1;
                int activityCount = DBContext.Update("GetActivitiesCount", new object[] { emgLocationId, emgClusterId,
                                                                                          RC.SelectedSiteLanguageId, isActive,
                                                                                          yearId, DBNull.Value });
                if (activityCount > 0)
                    maxActivities = maxActivities - activityCount;
            }

            FrameWorkSettingsCount fr = new FrameWorkSettingsCount();
            fr.IndCount = maxIndicators;
            fr.ActCount = maxActivities;
            fr.DateExcedded = endEditDate <= DateTime.Now.Date;
            return fr;
        }

        public static FrameWorkSettingsCount GetOutputFrameworkSettings(int emgLocationId, int emgClusterId)
        {
            int maxIndicators = 0;
            DateTime endEditDate = DateTime.Now.Date;
            string configKey = "Key-" + emgLocationId.ToString() + emgClusterId.ToString();

            maxIndicators = 0;
            endEditDate = DateTime.Now.AddDays(-1);

            string PATH = HttpRuntime.AppDomainAppPath;
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\ChangeEndSettings.xml";

            if (File.Exists(PATH))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(PATH);

                XmlElement elem_settings = doc.GetElementById("ChangeEndSettings");
                XmlNode settingsNode = doc.DocumentElement;

                foreach (XmlNode node in settingsNode.ChildNodes)
                {
                    if (node.Name.Equals(configKey))
                    {
                        if (node.Attributes["ClusterCount"] != null)
                        {
                            maxIndicators = Convert.ToInt32(node.Attributes["ClusterCount"].Value);
                        }

                        if (node.Attributes["DateLimit"] != null)
                        {
                            endEditDate = DateTime.ParseExact(Convert.ToString(node.Attributes["DateLimit"].Value), "MM-dd-yyyy", CultureInfo.InvariantCulture);
                        }
                    }
                }
            }

            int yearId = 12;
            int val = DBContext.Update("GetClusterIndicatorCount", new object[] { emgLocationId, emgClusterId, yearId, DBNull.Value });
            maxIndicators -= val;
            
            FrameWorkSettingsCount fr = new FrameWorkSettingsCount();
            fr.ClsIndCount = maxIndicators;
            fr.DateExcedded = endEditDate <= DateTime.Now.Date;
            return fr;
        }
    }

    public class FrameWorkSettingsCount
    {
        public int IndCount { get; set; }
        public int ActCount { get; set; }
        public int ClsIndCount { get; set; }
        public bool DateExcedded { get; set; }
    }
}