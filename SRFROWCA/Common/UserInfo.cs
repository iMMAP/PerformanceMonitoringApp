using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SRFROWCA.Common
{
    public static class UserInfo
    {
        internal static void UserProfileInfo()
        {
            DataTable dt = RC.GetUserDetails();

            if (dt.Rows.Count > 0)
            {
                HttpContext.Current.Session["UserCountry"] = dt.Rows[0]["LocationId"];
                HttpContext.Current.Session["UserCluster"] = dt.Rows[0]["ClusterId"];
                HttpContext.Current.Session["UserOrg"] = dt.Rows[0]["OrganizationId"];

                HttpContext.Current.Session["UserCountryName"] = dt.Rows[0]["LocationName"];
                HttpContext.Current.Session["UserOrgName"] = dt.Rows[0]["OrganizationName"];
            }
        }

        internal static int GetCountry
        {
            get
            {
                return HttpContext.Current.Session["UserCountry"] != null ?
                Convert.ToInt32(HttpContext.Current.Session["UserCountry"]) : 0;
            }
        }

        internal static int GetCluster
        {
            get
            {
                return HttpContext.Current.Session["UserCluster"] != null ?
                Convert.ToInt32(HttpContext.Current.Session["UserCluster"]) : 0;
            }
        }

        internal static int GetOrganization
        {
            get
            {
                return HttpContext.Current.Session["UserOrg"] != null ?
                Convert.ToInt32(HttpContext.Current.Session["UserOrg"]) : 0;
            }
        }

        internal static string GetCountryName
        {
            get
            {
                return HttpContext.Current.Session["UserCountryName"] != null ?
                HttpContext.Current.Session["UserCountryName"].ToString() : "";
            }
        }

        internal static string GetOrgName
        {
            get
            {
                return HttpContext.Current.Session["UserOrgName"] != null ?
                HttpContext.Current.Session["UserOrgName"].ToString() : "";
            }
        }
    }
}