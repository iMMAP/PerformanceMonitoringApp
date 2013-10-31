using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;
using BusinessLogic;
using System.Security.Principal;
using System.Web.Security;

namespace SRFROWCA.Common
{
    public class ROWCACommon
    {
        internal static bool IsInAdminRoles(IPrincipal iPrincipal)
        {
            if (IsAdmin(iPrincipal) || IsCountryAdmin(iPrincipal))
                return true;
            return false;
        }

        internal static bool IsAdmin(IPrincipal iPrincipal)
        {
            if (iPrincipal.IsInRole("Admin"))
                return true;
            return false;
        }

        internal static bool IsCountryAdmin(IPrincipal iPrincipal)
        {
            if (iPrincipal.IsInRole("CountryAdmin"))
                return true;
            return false;
        }

        internal static bool IsAuthenticated(IPrincipal iPrincipal)
        {
            return iPrincipal.Identity.IsAuthenticated;
        }

        internal static void RedirectUserToPage(HttpResponse Response)
        {
            Response.Redirect("~/Default.aspx");
        }

        internal static string GetCountryAdminRoleName
        {
            get
            {
                return "CountryAdmin";
            }
        }

        public static string CreateFolderForFiles(string dir, string sessionId)
        {
            // Concat sessionid with path to generate seperate
            // folder for each user.
            dir += "\\" + sessionId;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static bool DataInserted
        {
            get
            {
                if (HttpContext.Current.Session["DataInserted"] == null)
                {
                    HttpContext.Current.Session["DataInserted"] = false;
                }
                bool? dataInserted = HttpContext.Current.Session["DataInserted"] as bool?;

                return dataInserted.Value;
            }
            set
            {
                HttpContext.Current.Session["DataInserted"] = value;
            }
        }

        internal static DataTable GetLocations(IPrincipal user)
        {
            int locationType = (int)LocationTypes.National;
            DataTable dt = new DataTable();

            if (!user.Identity.IsAuthenticated || IsAdmin(user))
            {
                dt = DBContext.GetData("GetLocationOnType", new object[] { locationType });
            }
            else if (IsCountryAdmin(user))
            {
                Guid userId = GetCurrentUserId();
                dt = DBContext.GetData("GetLocationOnTypeAndPrincipal", new object[] { locationType, userId });
            }

            return dt;
        }

        internal static DataTable GetEmergencies(IPrincipal user)
        {
            DataTable dt = new DataTable();
            if (IsCountryAdmin(user))
            {
                Guid userId = GetCurrentUserId();
                dt = DBContext.GetData("GetLocationEmergenciesOfUser", new object[] { userId });
            }
            else
            {
                dt = DBContext.GetData("GetAllLocationEmergencies");
            }

            return dt;
        }

        internal static DataTable GetObjectives(IPrincipal user)
        {
            if (ROWCACommon.IsAdmin(user))
            {
                return DBContext.GetData("GetAllObjectives");
            }
            else
            {
                Guid userId = GetCurrentUserId();
                return DBContext.GetData("GetAllObjectivesOfUser", new object[] { userId });
            }
        }

        internal static DataTable GetIndicators(IPrincipal user)
        {

            if (IsAdmin(user))
            {
                return DBContext.GetData("GetAllIndicators");
            }
            else
            {
                Guid userId = GetCurrentUserId();
                return DBContext.GetData("GetAllIndicatorsOfUser", new object[] { userId });
            }
        }

        internal static DataTable GetAllActivities(IPrincipal user)
        {
            if (IsAdmin(user))
            {
                return DBContext.GetData("GetAllActivities");
            }
            else
            {
                Guid userId = GetCurrentUserId();
                return DBContext.GetData("GetAllActivitiesOfUser", new object[] { userId });
            }
        }

        internal static DataTable GetAllFrameWorkData(IPrincipal user)
        {
            if (IsAdmin(user))
            {
                return DBContext.GetData("GetAllData");
            }
            else
            {
                Guid userId = GetCurrentUserId();
                return DBContext.GetData("GetAllFrameWorkDataOfUser", new object[] { userId });
            }
        }

        public static Guid GetCurrentUserId()
        {
            return (Guid)Membership.GetUser().ProviderUserKey;
        }

        public static string ErrorMessage
        {
            get
            {
                return "error-message";
            }
        }

        public static string InfoMessage
        {
            get
            {
                return "info-message";
            }
        }

        public enum LocationTypes
        {
            Region = 1,
            National = 2,
            Governorate = 3,
            District = 4,
            Subdistrict = 5,
            Village = 6,
            Nonrepresentative = 7,
            Other = 8,
        }
    }
}