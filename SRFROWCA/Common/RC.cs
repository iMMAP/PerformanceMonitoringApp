using BusinessLogic;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SRFROWCA.Common
{
    public class RC
    {
        #region Roles & Rights

        internal static bool IsAuthenticated(IPrincipal iPrincipal)
        {
            return iPrincipal.Identity.IsAuthenticated;
        }

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

        internal static bool IsRegionalClusterLead(IPrincipal iPrincipal)
        {
            if (iPrincipal.IsInRole("RegionalClusterLead"))
                return true;
            return false;
        }

        internal static bool IsOCHAStaff(IPrincipal iPrincipal)
        {
            if (iPrincipal.IsInRole("OCHACountryStaff"))
                return true;
            return false;
        }

        internal static bool IsClusterLead(IPrincipal iPrincipal)
        {
            if (iPrincipal.IsInRole("ClusterLead"))
                return true;
            return false;
        }

        internal static bool IsDataEntryUser(IPrincipal iPrincipal)
        {
            if (iPrincipal.IsInRole("User"))
                return true;
            return false;
        }

        internal static string GetCountryAdminRoleName
        {
            get
            {
                return "CountryAdmin";
            }
        }
        #endregion

        #region Utilities

        internal static void RedirectUserToPage(HttpResponse Response)
        {
            Response.Redirect("~/Default.aspx");
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

        #endregion

        #region Cluster Methods

        internal static DataTable GetClusters()
        {
            return DBContext.GetData("GetAllClusters", new object[] { RC.SelectedSiteLanguageId });
        }

        internal static DataTable GetEmergencyClusters(int emergencyId)
        {
            return DBContext.GetData("GetEmergencyClusters", new object[] { emergencyId, RC.SelectedSiteLanguageId });
            ////return DBContext.GetData("GetEmergencyClusters", new object[] { emergencyId, RC.SelectedSiteLanguageId });
            //using (ORSEntities db = new ORSEntities())
            //{
            //    return(
            //                from ec in db.EmergencyClusters
            //                join cl in db.Clusters on ec.ClusterId equals cl.ClusterId
            //                join em in db.Emergencies on ec.EmergencyId equals em.EmergencyId
            //                where ec.EmergencyId == emergencyId
            //                && cl.SiteLanguageId == SelectedSiteLanguageId
            //                && em.SiteLanguageId == SelectedSiteLanguageId
            //                select new
            //                {
            //                    ec.EmergencyClusterId,
            //                    cl.ClusterName
            //                }
            //        ).ToList();
            //}
        }

        #endregion

        #region Logframe

        internal static DataTable GetObjectives(int emergencyId)
        {
            return DBContext.GetData("GetObjectives", new object[] { SelectedSiteLanguageId, emergencyId });
        }

        internal static DataTable GetEmergencyObjectives(int emergencyId)
        {
            return DBContext.GetData("GetObjectives", new object[] { SelectedSiteLanguageId, emergencyId });
        }

        internal static DataTable GetPriorities()
        {
            return DBContext.GetData("GetPriorities", new object[] { SelectedSiteLanguageId, RC.EmergencyEbola });
        }

        #endregion

        internal static void SendEmail(int emgLocationId, int? emgClusterId, string subject, string body, DataTable dtUsers = null, string extraEmails = null)
        {
            try
            {
                DataTable dtEmails = DBContext.GetData("GetEmailsOnCountyrAndCluster", new object[] { emgLocationId, emgClusterId });
                string emails = string.Empty;
                emails = "orsocharowca@gmail.com";
                emails += ",kashif.nadeem@hotmail.com";
                emails += ",kashif.nadeem@hotmail.com";
                emails += ",orsocharowca@gmail.com";
                if (!string.IsNullOrEmpty(extraEmails))
                {
                    emails += "," + extraEmails;
                }
                using (MailMessage mailMsg = new MailMessage())
                {
                    for (int i = 0; i < dtEmails.Rows.Count; i++)
                    {
                        string email = dtEmails.Rows[i]["Email"].ToString();
                        email = email.TrimStart(',').TrimEnd(',').Trim();
                        try
                        {
                            MailAddress ma = new MailAddress(email);
                            emails += "," + email;
                        }
                        catch { }
                    }

                    if (dtUsers != null)
                    {
                        for (int i = 0; i < dtUsers.Rows.Count; i++)
                        {
                            string email = dtUsers.Rows[i]["Email"].ToString();
                            email = email.TrimStart(',').TrimEnd(',').Trim();
                            try
                            {
                                MailAddress ma = new MailAddress(email);
                                emails += "," + email;
                            }
                            catch { }
                        }
                    }

                    emails = emails.TrimEnd(',');
                    string[] emailsArray = emails.Split(',');
                    emails = string.Join(",", (emailsArray.Distinct().ToArray()));
                    mailMsg.From = new MailAddress("orsocharowca@gmail.com", "Sahel ORS");
                    mailMsg.To.Add(emails.TrimEnd(','));
                    mailMsg.Subject = subject;
                    mailMsg.IsBodyHtml = true;
                    mailMsg.Body = string.Format("{0} <br/><br/>Regards,<br/>Sahel ORS", body);
                    Mail.SendMail(mailMsg);
                }
            }
            catch
            {

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

        internal static int SelectedEmergencyId
        {
            get
            {
                if (HttpContext.Current.Session["SelectedEmergencyId"] != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["SelectedEmergencyId"]);
                }

                return 0;
            }

            set
            {
                HttpContext.Current.Session["SelectedEmergencyId"] = value;
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

        internal static void AddSiteLangInCookie(HttpResponse httpResponse, RC.SiteLanguage lng)
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
                Guid userId = GetCurrentUserId;
                dt = DBContext.GetData("GetLocationOnTypeAndPrincipal", new object[] { locationType, userId });
            }
            else if (user.Identity.IsAuthenticated)
            {
                dt = DBContext.GetData("GetLocationOnType", new object[] { locationType });
            }

            return dt;
        }

        internal static DataTable GetAdmin1(int countryId)
        {
            return DBContext.GetData("GetAdmin1LocationsOfCountry", new object[] { countryId });
        }

        internal static DataTable GetAdmin2(int countryId)
        {
            return DBContext.GetData("GetAdmin2LocationsOfCountry", new object[] { countryId });
        }

        internal static DataTable GetAdmin1OfEmgLocation(int emergencyLocationId)
        {
            return DBContext.GetData("GetAdmin1OfEmergnecyLocation", new object[] { emergencyLocationId });
        }

        internal static DataTable GetLocationsAndChilds(int locationId, int childTypeId)
        {
            return DBContext.GetData("GetLocationAndItsChildOnType", new object[] { locationId, childTypeId });
        }

        internal static DataTable GetEmergencyLocations(int emergencyId)
        {
            return DBContext.GetData("GetEmergnecyLocations", new object[] { emergencyId });
        }

        internal static DataTable GetLocationEmergencies(IPrincipal user)
        {
            DataTable dt = new DataTable();
            if (IsCountryAdmin(user))
            {
                Guid userId = GetCurrentUserId;
                dt = DBContext.GetData("GetLocationEmergenciesOfUser", new object[] { userId });
            }
            else
            {
                dt = DBContext.GetData("GetAllLocationEmergencies");
            }

            return dt;
        }

        internal static DataTable GetAllEmergencies(int? languageId, string search = "", int disasterType = 0)
        {

            return DBContext.GetData("GetAllEmergencies", new object[] { languageId, string.IsNullOrEmpty(search) ? null : search, disasterType > 0 ? disasterType : 0 });
        }

        internal static DataTable GetAllActivities(IPrincipal user)
        {
            if (IsAdmin(user))
            {
                return DBContext.GetData("GetAllActivities");
            }
            else
            {
                Guid userId = GetCurrentUserId;
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
                Guid userId = GetCurrentUserId;
                return DBContext.GetData("GetAllFrameWorkDataOfUser", new object[] { userId });
            }
        }

        internal static DataTable GetAllUnits()
        {
            return DBContext.GetData("GetAllUnits", new object[] { RC.SelectedSiteLanguageId });
        }

        internal static DataTable GetOrganizations(int? orgId)
        {
            return DBContext.GetData("GetOrganizations", new object[] { orgId, null });
        }

        internal static DataTable GetProjectsOrganizations(int? locId, int? clusterId)
        {
            return DBContext.GetData("GetProjectsOrganizations", new object[] { locId, clusterId });
        }

        internal static DataTable GetOrgProjectsOnLocation(bool? isOPSProject)
        {
            return DBContext.GetData("GetOrgProjectsOnLocation", new object[] { UserInfo.EmergencyCountry, UserInfo.Organization, isOPSProject, 2015 });
        }

        internal static DataTable GetUserDetails(int emergencyId)
        {
            Guid userId = GetCurrentUserId;
            return DBContext.GetData("GetUserDetails", new object[] { emergencyId, userId });
        }

        internal static DataTable GetMonths()
        {
            return DBContext.GetData("GetMonths", new object[] { SelectedSiteLanguageId });
        }

        public static Guid GetCurrentUserId
        {
            get
            {
                if (Membership.GetUser() != null)
                {
                    return (Guid)Membership.GetUser().ProviderUserKey;
                }

                return new Guid();
            }
        }

        internal static void ShowMessage(Page page, Type pageType, string UniqueID, string message, NotificationType notificationType = NotificationType.Success, bool fadeOut = true, int animationTime = 0)
        {
            string cssClass = GetClass(notificationType);

            if (fadeOut)
            {
                ScriptManager.RegisterStartupScript(page, pageType, UniqueID,
                    "$('#divMsg').addClass('" + cssClass + "').html('" + message + "').animate({ top: '40' }," + animationTime.ToString() + ").fadeOut(4000, function() {});", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(page, pageType, UniqueID,
                "$('#divMsg').addClass('" + cssClass + "').html('" + message + "').animate({ top: '40' }," + animationTime.ToString() + ").click(function(){$(this).animate({top: -$(this).outerHeight()}, 400);});", true);
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

        internal static void AddSelectItemInList(ListControl ctl, string text)
        {
            ListItem item = new ListItem(text, "0");
            ctl.Items.Insert(0, item);
        }


        // Get multiple selected values from drop down checkbox.
        internal static string GetSelectedValues(object sender)
        {
            string ids = GetSelectedItems(sender);
            ids = !string.IsNullOrEmpty(ids) ? ids : null;
            return ids;
        }

        internal static int? GetSelectedValue(DropDownList ddl)
        {
            int val = 0;
            int.TryParse(ddl.SelectedValue, out val);
            return val > 0 ? val : (int?)null;
        }

        private static string GetSelectedItems(object sender)
        {
            string itemIds = "";
            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    if (itemIds != "")
                    {
                        itemIds += "," + item.Value;
                    }
                    else
                    {
                        itemIds += item.Value;
                    }
                }
            }

            return itemIds;
        }

        internal static void ClearSelectedItems(object sender)
        {
            foreach (ListItem item in (sender as ListControl).Items)
            {
                item.Selected = false;
            }
        }

        // Get multiple selected values from drop down checkbox.
        internal static string GetItemsText(object sender)
        {
            string items = GetSelectedItemsText(sender);
            items = !string.IsNullOrEmpty(items) ? items : null;
            return items;
        }

        private static string GetSelectedItemsText(object sender)
        {
            string items = "";
            int i = 0;
            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    i++;
                    if (items != "")
                    {
                        items += ", " + item.Text;
                    }
                    else
                    {
                        items += item.Text;
                    }
                }

                if (i > 3)
                {
                    items += "... and more";
                    break;
                }
            }

            return items;
        }

        internal static int GetSelectedIntVal(ListControl ctl)
        {
            int i = 0;
            int.TryParse(ctl.SelectedValue, out i);
            return i;
        }

        internal static decimal GetSelectedDecimalVal(ListControl ctl)
        {
            decimal i = 0m;
            decimal.TryParse(ctl.SelectedValue, out i);
            return i;
        }

        internal static string GetClusterName
        {
            get
            {
                using (ORSEntities db = new ORSEntities())
                {
                    return db.Clusters.Where(x => x.ClusterId == UserInfo.Cluster && x.SiteLanguageId == RC.SelectedSiteLanguageId)
                                            .Select(x => x.ClusterName).SingleOrDefault();
                }
            }
        }

        internal static void EnableDisableControls(WebControl ctl, bool isEnable)
        {
            if (!isEnable)
            {
                ctl.Enabled = isEnable;
                ctl.BackColor = Color.LightGray;
            }
            else
            {
                ctl.Enabled = isEnable;
            }
        }

        internal static string ConfigSettings(string key)
        {
            string setting = "";
            if (ConfigurationManager.AppSettings[key] != null)
            {
                setting += ConfigurationManager.AppSettings[key].ToString();
            }

            return setting;
        }

        internal static string FrenchCulture
        {
            get
            {
                return "fr-FR";
            }
        }

        internal static string EnglishCulture
        {
            get
            {
                return "en-US";
            }
        }

        internal static int EmergencySahel2015
        {
            get
            {
                return 3;
            }
        }

        internal static int EmergencyEbola
        {
            get
            {
                return 2;
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

        public enum LocationTypeForUI
        {
            Country = 1,
            Admin1 = 2,
            Admin2 = 3,
            None = -1
        }

        public enum YearsInDB
        {
            Year2014 = 10,
            Year2015 = 11
        }
    }
}