using BusinessLogic;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Xml;

namespace SRFROWCA.Common
{
    public static class SectorFramework
    {
        public static bool DateExceeded(int emgLocationId, int emgClusterId, int year)
        {
            DateTime frDate = DateTime.MaxValue;

            // Make Key which is saved in configuration file
            DataTable dt = DBContext.GetData("GetTargetSettings", new object[] { emgLocationId, emgClusterId, year });
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                if (row["DateLimit"] != DBNull.Value)
                {
                    frDate = DateTime.ParseExact(Convert.ToString(row["DateLimit"].ToString()),
                                                                   "dd-MM-yyyy", CultureInfo.InvariantCulture);
                }
            }

            // If Date is less than or equal than current 
            // date then it is not exceeded, return false.
            return frDate < DateTime.Now.Date;
        }

        public static int IndUnused(int emgLocationId, int emgClusterId, int year)
        {
            int allowedIndicators = 0;
            DataTable dt = DBContext.GetData("GetTargetSettings", new object[] { emgLocationId, emgClusterId, year });
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                int.TryParse(row["IndicatorMax"].ToString(), out allowedIndicators);
            }

            RC.Year yearEnum;
            int yearId = 0;
            if (Enum.TryParse("_" + year.ToString(), out yearEnum))
                yearId = (int)yearEnum;

            if (allowedIndicators > 0)
            {
                int isActive = 1;
                int indicatorCount = DBContext.Update("GetIndicatorsCount", new object[] { emgLocationId, emgClusterId, 
                                                                                            isActive, RC.SelectedSiteLanguageId, (int)yearId, DBNull.Value });
                if (indicatorCount > 0)
                    allowedIndicators -= indicatorCount;
            }

            return allowedIndicators;
        }

        public static int ActivityUnused(int emgLocationId, int emgClusterId, int year)
        {
            int allowedActivities = 0;
            DataTable dt = DBContext.GetData("GetTargetSettings", new object[] { emgLocationId, emgClusterId, year });
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                int.TryParse(row["ActivityMax"].ToString(), out allowedActivities);
            }

            RC.Year yearEnum;
            int yearId = 0;
            if (Enum.TryParse("_" + year.ToString(), out yearEnum))
                yearId = (int)yearEnum;
            if (allowedActivities > 0)
            {
                int isActive = 1;
                int activityCount = DBContext.Update("GetActivitiesCount", new object[] { emgLocationId, emgClusterId,
                                                                                          RC.SelectedSiteLanguageId, isActive,
                                                                                          (int)yearId, DBNull.Value });
                if (activityCount > 0)
                    allowedActivities -= activityCount;
            }

            return allowedActivities;
        }

        public static int OutputIndUnused(int emgLocationId, int emgClusterId, int year)
        {
            int outputIndAllowed = 0;

            DataTable dt = DBContext.GetData("GetTargetSettings", new object[] { emgLocationId, emgClusterId, year });
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                int.TryParse(row["ClusterIndicatorMax"].ToString(), out outputIndAllowed);
            }


           RC.Year yearEnum;
            int yearId = 0;
            if (Enum.TryParse("_" + year.ToString(), out yearEnum))
                yearId = (int)yearEnum;
            int val = DBContext.Update("GetClusterIndicatorCount", new object[] { emgLocationId, emgClusterId, (int)yearId, DBNull.Value });
            return (outputIndAllowed -= val);
        }
    }
}