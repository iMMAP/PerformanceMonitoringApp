using System.Data;
using System.Web;

namespace SRFROWCA.Common
{
    public static class UserInfo
    {
        internal static void UserProfileInfo(int emergencyId)
        {
            DataTable dt = RC.GetUserDetails(emergencyId);

            if (dt.Rows.Count > 0)
            {
                HttpContext.Current.Session["UserCluster"] = dt.Rows[0]["ClusterId"];
                HttpContext.Current.Session["UserOrg"] = dt.Rows[0]["OrganizationId"];
                HttpContext.Current.Session["UserOrgName"] = dt.Rows[0]["OrganizationName"];
                
                if (dt.Rows[0]["EmergencyLocationId"] != null)
                {
                    if ((!(HttpContext.Current.User.IsInRole("User"))) && dt.Rows[0]["EmergencyLocationId"].ToString() == "1")
                    {
                        HttpContext.Current.Session["UserLocationEmergencyId"] = null;
                        HttpContext.Current.Session["UserCountry"] = null;
                        HttpContext.Current.Session["UserCountryName"] = null;
                    }
                    else
                    {
                        HttpContext.Current.Session["UserLocationEmergencyId"] = dt.Rows[0]["EmergencyLocationId"];
                        HttpContext.Current.Session["UserCountry"] = dt.Rows[0]["LocationId"];
                        HttpContext.Current.Session["UserCountryName"] = dt.Rows[0]["LocationName"];
                    }
                }
                
                HttpContext.Current.Session["UserEmergencyClusterId"] = dt.Rows[0]["EmergencyClusterId"];
                //HttpContext.Current.Session["EmergencyId"] = dt.Rows[0]["EmergencyId"];
                
            }

           
        }

        internal static int Country
        {
            get
            {
                int i = 0;
                if (HttpContext.Current.Session["UserCountry"] != null)
                {
                    int.TryParse(HttpContext.Current.Session["UserCountry"].ToString(), out i);
                }
                return i;
            }
        }

        internal static int Cluster
        {
            get
            {
                int i = 0;
                if (HttpContext.Current.Session["UserCluster"] != null)
                {
                    int.TryParse(HttpContext.Current.Session["UserCluster"].ToString(), out i);
                }
                return i;
            }
        }

        internal static int Organization
        {
            get
            {
                int i = 0;
                if (HttpContext.Current.Session["UserOrg"] != null)
                {
                    int.TryParse(HttpContext.Current.Session["UserOrg"].ToString(), out i);
                }
                return i;
            }
        }

        internal static int EmergencyCountry
        {
            get
            {
                int i = 0;
                if (HttpContext.Current.Session["UserLocationEmergencyId"] != null)
                {
                    int.TryParse(HttpContext.Current.Session["UserLocationEmergencyId"].ToString(), out i);
                }
                return i;
            }
        }

        internal static int EmergencyCluster
        {
            get
            {
                int i = 0;
                if (HttpContext.Current.Session["UserEmergencyClusterId"] != null)
                {
                    int.TryParse(HttpContext.Current.Session["UserEmergencyClusterId"].ToString(), out i);
                }
                return i;
            }
        }

        //internal static int Emergency
        //{
        //    get
        //    {
        //        int i = 0;
        //        if (HttpContext.Current.Session["EmergencyId"] != null)
        //        {
        //            int.TryParse(HttpContext.Current.Session["EmergencyId"].ToString(), out i);
        //        }
        //        return i;
        //    }
        //}

        internal static string CountryName
        {
            get
            {
                return HttpContext.Current.Session["UserCountryName"] != null ?
                HttpContext.Current.Session["UserCountryName"].ToString() : "";
            }
        }

        internal static string OrgName
        {
            get
            {
                return HttpContext.Current.Session["UserOrgName"] != null ?
                HttpContext.Current.Session["UserOrgName"].ToString() : "";
            }
        }
    }
}