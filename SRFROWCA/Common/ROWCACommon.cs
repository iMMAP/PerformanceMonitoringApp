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
using System.Threading;
using System.Globalization;

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

        internal static int SelectedSiteLanguageId
        {
            get
            {
                if (HttpContext.Current.Session["SiteLanguage"] != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["SiteLanguage"]);
                }

                return Convert.ToInt32(SiteLanguage.English);
            }

            set
            {
                HttpContext.Current.Session["SiteLanguage"] = value;
            }
        }

        internal static string SiteCulture
        {
            get
            {
                if (HttpContext.Current.Session["SiteCulture"] != null)
                {
                    return HttpContext.Current.Session["SiteCulture"].ToString();
                }

                return "en-US";
            }

            set
            {
                HttpContext.Current.Session["SiteCulture"] = value;
            }
        }

        internal static void AddSiteLangInCookie(HttpResponse httpResponse, ROWCACommon.SiteLanguage lng)
        {
            httpResponse.Cookies["SiteLanguageCookie"].Value = ((int)lng).ToString();
            httpResponse.Cookies["SiteLanguageCookie"].Expires = DateTime.Now.AddDays(365);
        }


        internal static DataTable GetLocations(IPrincipal user, int locationType)
        {
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
            else if (user.Identity.IsAuthenticated)
            {
                dt = DBContext.GetData("GetLocationOnType", new object[] { locationType });
            }

            return dt;
        }

        internal static DataTable GetLocationsAndChilds(int locationId, int childTypeId)
        {
            return DBContext.GetData("GetLocationAndItsChildOnType", new object[] { locationId, childTypeId });
        }

        internal static DataTable GetLocationEmergencies(IPrincipal user)
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

        internal static DataTable GetAllEmergencies(int? languageId)
        {
            return DBContext.GetData("GetAllEmergencies", new object[] { languageId });
        }

        internal static DataTable GetStrategicObjectives(IPrincipal user, int? languageId)
        {
            if (ROWCACommon.IsAdmin(user))
            {
                return DBContext.GetData("GetAllStrategicObjectives", new object[] { languageId });
            }
            else
            {
                Guid userId = GetCurrentUserId();
                return DBContext.GetData("GetAllStrategicObjectivesOfUser", new object[] { userId });
            }
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

        internal static DataTable GetUserDetails()
        {
            Guid userId = GetCurrentUserId();
            return DBContext.GetData("GetUserDetails", new object[] { userId });
        }

        public static Guid GetCurrentUserId()
        {
            if (Membership.GetUser() != null)
            {
                return (Guid)Membership.GetUser().ProviderUserKey;
            }

            return new Guid();
        }

        internal static void ShowMessage(Page page, Type pageType, string UniqueID, string message, NotificationType notificationType = NotificationType.Success, bool fadeOut = true, int animationTime = 0)
        {
            string cssClass = GetClass(notificationType);

            if (fadeOut)
            {
                ScriptManager.RegisterStartupScript(page, pageType, UniqueID,
                    "$('#divMsg').addClass('" + cssClass + "').html('" + message + "').animate({ top: '0' }," + animationTime.ToString() + ").fadeOut(4000, function() {});", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(page, pageType, UniqueID,
                "$('#divMsg').addClass('" + cssClass + "').html('" + message + "').animate({ top: '0' }," + animationTime.ToString() + ").click(function(){$(this).animate({top: -$(this).outerHeight()}, 400);});", true);
            }
        }

        private static string GetClass(NotificationType type)
        {
            if (type == NotificationType.Success)
            {
                return "success message";
            }

            if (type == NotificationType.Error)
            {
                return "error message";
            }

            if (type == NotificationType.Info)
            {
                return "info message";
            }

            if (type == NotificationType.Warning)
            {
                return "warning message";
            }

            return "info message";
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

        internal static void SetCulture(string siteCulture, int languageId, short siteChanged)
        {
            HttpContext.Current.Session["SiteChanged"] = siteChanged;
            SelectedSiteLanguageId = languageId;
            SiteCulture = siteCulture;
        }

        internal static void CultureSettings(string postBackControl)
        {
            if (postBackControl.EndsWith("French"))
            {
                SetCulture("fr-FR", (int)SiteLanguage.French, 1);
            }
            else if (postBackControl.EndsWith("English"))
            {
                SetCulture("en-US", (int)SiteLanguage.English, 1);
            }
        }

        internal static void SetCulture()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SiteCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(SiteCulture);
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

        internal enum NotificationType
        {
            Success = 1,
            Error = 2,
            Info = 3,
            Warning = 4,
        }

        internal enum SiteLanguage
        {
            English = 1,
            French = 2
        }
    }
}