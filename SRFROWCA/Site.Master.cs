using SRFROWCA.Common;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SRFROWCA
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        public string MetaDescription = "ROWCA ORS";
        public string PageTitle = "ORS - Home";
        public string CurrentEmergency = "Select";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["SiteLanguage"] == null)
            {
                if (Request.Cookies["SiteLanguageCookie"] != null)
                {
                    string siteLangId = Request.Cookies["SiteLanguageCookie"].Value;
                    if (siteLangId == "2")
                    {
                        RC.SelectedSiteLanguageId = (int)RC.SiteLanguage.French;
                        RC.SiteCulture = RC.FrenchCulture;
                    }
                    else
                    {
                        RC.SelectedSiteLanguageId = (int)RC.SiteLanguage.English;
                        RC.SiteCulture = RC.EnglishCulture;
                    }
                }
            }
            RC.SelectedEmergencyId = 3;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RC.SelectedEmergencyId = 3;
            SetUserName();
            HideAllAuthenticatedMenues();

            if (!IsPostBack)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    UserInfo.UserProfileInfo(RC.SelectedEmergencyId);
                }
            }

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                LoginStatus.Visible = false;
                spanWelcome.Visible = true;
                liRegister.Visible = false;
                ShowHideMenuesOnRole();
            }

            ActiveMenueItem();
        }

        private void ShowHideMenuesOnRole()
        {
            if (HttpContext.Current.User.IsInRole("User"))
            {
                ShowUserMenue();
            }

            if (HttpContext.Current.User.IsInRole("ClusterLead"))
            {
                ShowClusterLeadMenue();
            }

            if (HttpContext.Current.User.IsInRole("RegionalClusterLead "))
            {
                ShowRegionalLeadMenue();
            }

            if (HttpContext.Current.User.IsInRole("OCHA"))
            {
                ShowOCHAMenue();
            }

            if (HttpContext.Current.User.IsInRole("CountryAdmin"))
            {
                ShowCountryAdminMenue();
            }

            if (HttpContext.Current.User.IsInRole("Admin"))
            {
                ShowAdminMenue();
            }
        }

        private void SetUserName()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                using (ORSEntities db = new ORSEntities())
                {
                    var user = db.aspnet_Users_Custom.Where(x => x.UserId == RC.GetCurrentUserId).Select(y => new { y.FullName }).SingleOrDefault();
                    if (user != null && !string.IsNullOrEmpty(user.FullName))
                    {
                        HeadLoginName.FormatString = user.FullName;
                    }
                }
            }
        }

        protected void btnLinkNotifications_Click(object sender, EventArgs e)
        {

        }

        protected void rptNotification_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkButton = e.Item.FindControl("btnLinkNotifications") as LinkButton;
                if (lnkButton != null)
                {
                    lnkButton.Click += btnLinkNotifications_Click;
                }
            }
        }

        protected void rptNotifications_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "GoToNotification")
            {
                string i = e.CommandArgument.ToString();
            }
        }

        private void HideAllAuthenticatedMenues()
        {
            bool isShow = false;
            menuDataEntry.Visible = isShow;
            liDataEntry.Visible = isShow;
            //menuManageActivities.Visible = isShow;
            //liManageActivity.Visible = isShow;
            liValidateAchievements.Visible = isShow;
            //liFundingStatus.Visible = isShow;
            liUserListing.Visible = isShow;
            liKeyFigures.Visible = isShow;
            liBulkImport.Visible = isShow;
            liBulkImportUser.Visible = isShow;
            liOrganizationList.Visible = isShow;
            liEmergency.Visible = isShow;
            liProgressSummary.Visible = isShow;
            liLocations.Visible = isShow;
            liClusterFrameworks.Visible = isShow;
            liProjects.Visible = isShow;
            liClusterFrameworkImport.Visible = isShow;
            liSettings.Visible = isShow;
            liManageUnits.Visible = isShow;
            liMangeTarSettings.Visible = isShow;
            //liManagePartners.Visible = isShow;
            liRequestedOrganizations.Visible = isShow;
            liKeyFiguresFramework.Visible = isShow;
            liContactList.Visible = isShow;
            liMaps.Visible = isShow;
            liSyncProjects.Visible = isShow;
        }

        private void ShowUserMenue()
        {
            bool isShow = true;
            menuDataEntry.Visible = isShow;
            liDataEntry.Visible = isShow;
            //menuManageActivities.Visible = isShow;
            //liManageActivity.Visible = isShow;
            liBulkImportUser.Visible = isShow;
            liClusterFrameworks.Visible = !isShow;
            liProjects.Visible = isShow;
            //liManagePartners.Visible = isShow;
            liKeyFiguresPublic.Visible = isShow;
            liOutputIndReportPublic.Visible = isShow;
            liProjectsPublic.Visible = !isShow;
            liActivitesFrameworkPublic.Visible = isShow;
        }

        private void ShowRegionalLeadMenue()
        {
            bool isShow = true;
            liClusterFrameworks.Visible = isShow;
            liProjects.Visible = isShow;
            liKeyFiguresPublic.Visible = !isShow;
            liProjectsPublic.Visible = !isShow;
            liActivitesFrameworkPublic.Visible = !isShow;
            liKeyFigures.Visible = isShow;
            liClusterFrameworkImport.Visible = isShow;
        }

        private void ShowClusterLeadMenue()
        {
            bool isShow = true;
            liValidateAchievements.Visible = isShow;
            liBulkImport.Visible = isShow;
            liClusterFrameworks.Visible = isShow;
            liProjects.Visible = isShow;
            liKeyFiguresPublic.Visible = !isShow;
            liKeyFigures.Visible = isShow;
            liOutputIndReportPublic.Visible = !isShow;
            liProgressSummary.Visible = isShow;
            liProjectsPublic.Visible = !isShow;
            liActivitesFrameworkPublic.Visible = !isShow;
            liClusterFrameworkImport.Visible = isShow;
        }

        private void ShowOCHAMenue()
        {
            bool isShow = true;
            liProjects.Visible = isShow;
            liProjectsPublic.Visible = !isShow;
            liKeyFiguresPublic.Visible = isShow;
            liOutputIndReportPublic.Visible = isShow;
            liProgressSummary.Visible = isShow;
            liActivitesFrameworkPublic.Visible = !isShow;
        }

        private void ShowCountryAdminMenue()
        {
            bool isShow = true;
            liValidateAchievements.Visible = isShow;
            liProgressSummary.Visible = isShow;
            liSettings.Visible = isShow;
           
            liUserListing.Visible = isShow;
            liKeyFigures.Visible = isShow;
            liClusterFrameworks.Visible = isShow;
            liProjects.Visible = isShow;
            liClusterFrameworkImport.Visible = isShow;
            liKeyFiguresPublic.Visible = !isShow;
            liOutputIndReportPublic.Visible = !isShow;
            liContactList.Visible = isShow;
            liProjectsPublic.Visible = !isShow;
            liActivitesFrameworkPublic.Visible = isShow;
        }

        private void ShowAdminMenue()
        {
            bool isShow = true;
            //liFundingStatus.Visible = isShow;
            liUserListing.Visible = isShow;
            liKeyFigures.Visible = isShow;
            liOrganizationList.Visible = isShow;
            liEmergency.Visible = isShow;
            liProgressSummary.Visible = isShow;
            liSettings.Visible = isShow;
            liManageUnits.Visible = isShow;
            liMangeTarSettings.Visible = isShow;
            liLocations.Visible = isShow;
            liClusterFrameworks.Visible = isShow;
            liProjects.Visible = isShow;
            liClusterFrameworkImport.Visible = isShow;
            liRequestedOrganizations.Visible = isShow;
            liKeyFiguresFramework.Visible = isShow;
            liKeyFiguresPublic.Visible = !isShow;
            liOutputIndReportPublic.Visible = !isShow;
            liContactList.Visible = isShow;
            liProjectsPublic.Visible = !isShow;
            liActivitesFrameworkPublic.Visible = !isShow;
            liMaps.Visible = isShow;
            liSyncProjects.Visible = isShow;
        }

        private void ActiveMenueItem()
        {
            string uri = HttpContext.Current.Request.Url.AbsolutePath;
            if (uri.Contains("/Default.aspx") || uri.Contains("Landing/ClusterCord"))
            {
                //liDashboards.Attributes.Add("class", "active");
                liHome.Attributes.Add("class", "active");
                PageTitle = "ORS - Home";
            }
            else if (uri.Contains("/Dashboard.aspx"))
            {
                liDashboards.Attributes.Add("class", "active");
                PageTitle = "ORS - Dashboard";
            }
            else if (uri == "/ReportingStatus.aspx")
            {
                liDashboards.Attributes.Add("class", "active open");
                PageTitle = "ORS - Achievements";
            }
            else if (uri == "/Pages/AddActivities.aspx")
            {
                PageTitle = "ORS - Data Entry";
                liDataEntry.Attributes.Add("class", "active");
            }
            else if (uri.Contains("/Pages/ManageProject.aspx"))
            {
                PageTitle = "ORS - Manage Project";
                liProjects.Attributes.Add("class", "active");
            }
            //else if (uri == "/Pages/ManageActivities.aspx")
            //{
            //    PageTitle = "ORS - Manage Activity";
            //    liManageActivity.Attributes.Add("class", "active");
            //}
            else if (uri == "/Anonymous/AllData.aspx")
            {
                liCustomReport.Attributes.Add("class", "active");
            }
            else if (uri.Contains("Pages/UploadAchived.aspx"))
            {
                PageTitle = "ORS - Bulk Upload";
                liBulkImportUser.Attributes.Add("class", "active");
            }
            else if (uri == "/ClusterLead/ValidateReportList.aspx")
            {
                liValidateAchievements.Attributes.Add("class", "active");
            }
            else if (uri == "/Anonymous/OutputIndicatorReport.aspx")
            {
                liOutputIndReportPublic.Attributes.Add("class", "active");
            }
            else if (uri == "/ClusterLead/ProjectsListing.aspx" || uri.Contains("/ClusterLead/ProjectDetails.aspx"))
            {
                PageTitle = "ORS - Projects";
                liProjects.Attributes.Add("class", "active");
            }
            else if (uri == "/Anonymous/ProjectsListingPublic.aspx")
            {
                PageTitle = "ORS - Projects";
                liProjectsPublic.Attributes.Add("class", "active");
            }
            else if (uri.Contains("/Admin/UsersListing.aspx") || uri.Contains("/Account/Registerca.aspx") || uri.Contains("/Account/UpdateUser.aspx"))
            {
                PageTitle = "ORS - User Details";
                liUserListing.Attributes.Add("class", "active");
            }

            else if (uri.Contains("/Admin/organization/OrganizationList.aspx") ||
                    uri.Contains("/Admin/organization/AddEditOrganization.aspx"))
            {
                PageTitle = "ORS - Manage Organizations";
                liOrganizationList.Attributes.Add("class", "active");
            }

            else if (uri.Contains("/Admin/RequestedOrgListing.aspx"))
            {
                PageTitle = "ORS - Manage Organizations";
                liRequestedOrganizations.Attributes.Add("class", "active");
            }
            else if (uri.Contains("/admin/LinkEmergencyObjective.aspx"))
            {
                PageTitle = "ORS - Manage Objectives";
                liLinkEmgObjectives.Attributes.Add("class", "active");
            }

            else if (uri.Contains("/admin/EmergencyObjectives.aspx"))
            {
                PageTitle = "ORS - Manage Objectives";
                liEmgObjectives.Attributes.Add("class", "active");
            }

            else if (uri == "/KeyFigures/KeyFiguresListing.aspx" || uri == "/KeyFigures/AddKeyFigure.aspx")
            {
                PageTitle = "ORS - Key Figures";
                liKeyFigures.Attributes.Add("class", "active");
            }
            else if (uri == "/KeyFigures/KeyFigureIndicatorListing.aspx" || uri == "/KeyFigures/AddKeyFigureCategory.aspx" ||
                        uri == "/KeyFigures/AddKeyFigureSubCategory.aspx" || uri == "/KeyFigures/AddKeyFigureIndicator.aspx")
            {
                PageTitle = "ORS - Key Figures";
                liKeyFiguresFramework.Attributes.Add("class", "active");
            }
            else if (uri == "/KeyFigures/KeyFiguresListingPublic.aspx")
            {
                PageTitle = "ORS - Key Figures";
                liKeyFiguresPublic.Attributes.Add("class", "active");
            }
            //else if (uri == "/LeadPages/FundingListing.aspx")
            //{
            //    PageTitle = "ORS - Fundings";
            //    liFundingStatus.Attributes.Add("class", "active");
            //}
            else if (uri == "/organization/OrganizationList.aspx")
            {
                PageTitle = "ORS - Organizations";
                liOrganizationList.Attributes.Add("class", "active");
            }
            else if (uri.Contains("ClusterLead/UploadAchieved.aspx"))
            {
                liBulkImport.Attributes.Add("class", "active");
            }
            else if (uri.Contains("/EmergencyListing.aspx"))
            {
                PageTitle = "ORS - Emergencies";
                liEmergency.Attributes.Add("class", "active open");
                liEmgList.Attributes.Add("class", "active");
            }
            else if (uri.Contains("/EmergencyLocations.aspx"))
            {
                PageTitle = "ORS - Emergency Locations";
                liEmergency.Attributes.Add("class", "active open");
                liEmgLocation.Attributes.Add("class", "active");
            }
            else if (uri.Contains("/EmergencyClusters.aspx"))
            {
                PageTitle = "ORS - Emergency Clusters";
                liEmergency.Attributes.Add("class", "active open");
                liEmgCluster.Attributes.Add("class", "active");
            }
            else if (uri.Contains("ClusterLead/CountryIndicators.aspx") ||
                uri.Contains("ClusterLead/EditOutputIndicator.aspx"))
            {
                PageTitle = "ORS - Country Indicators";
                liClusterFrameworks.Attributes.Add("class", "active open");
                liOutputIndicators.Attributes.Add("class", "active open");
                liClusterIndicators.Attributes.Add("class", "active");
            }
            else if (uri.Contains("ClusterLead/ClusterDataEntry.aspx") || uri.Contains("RegionalLead/ClusterDataEntry.aspx"))
            {
                PageTitle = "ORS - Country Indicators";
                liClusterFrameworks.Attributes.Add("class", "active open");
                liOutputIndicators.Attributes.Add("class", "active open");
                liIndicatorReporting.Attributes.Add("class", "active");
            }
            else if (uri.Contains("ClusterLead/ClusterDataEntry16.aspx"))
            {
                PageTitle = "ORS - Country Indicators";
                liClusterFrameworks.Attributes.Add("class", "active open");
                liOutputIndicators.Attributes.Add("class", "active open");
                liIndicatorReporting16.Attributes.Add("class", "active");
            }
            else if (uri.Contains("ClusterLead/ClusterReports.aspx"))
            {
                PageTitle = "ORS - Country Indicators";
                liClusterFrameworks.Attributes.Add("class", "active open");
                liOutputIndicators.Attributes.Add("class", "active open");
                liIndicatorReport.Attributes.Add("class", "active");
            }
            else if (uri.Contains("ClusterLead/CountryIndicators.aspx"))
            {
                PageTitle = "ORS - Country Indicators";
                liClusterFrameworks.Attributes.Add("class", "active open");
                liOutputIndicators.Attributes.Add("class", "active open");
                liClusterIndicators.Attributes.Add("class", "active");
            }

            else if (uri.Contains("ClusterLead/IndicatorListing.aspx") 
                || uri.Contains("ClusterLead/AddActivityAndIndicators.aspx")
                ||uri.Contains("ClusterLead/IndicatorListingMigrate.aspx"))
            {
                PageTitle = "ORS - Activity Indicators";
                liClusterFrameworks.Attributes.Add("class", "active open");
                liNewIndicatorListing.Attributes.Add("class", "active open");
            }

            else if (uri.Contains("Anonymous/ExpClusterFramework.aspx"))
            {
                PageTitle = "ORS - Export Framework";
                liClusterFrameworks.Attributes.Add("class", "active open");
                liClusterFrameworkImport.Attributes.Add("class", "active");
            }
            else if (uri.Contains("ClusterLead/ProgressSummary.aspx"))
            {
                PageTitle = "ORS - Progress Summary";
                liProgressSummary.Attributes.Add("class", "active");
            }
            else if (uri.Contains("Admin/ConfigSettings.aspx"))
            {
                PageTitle = "ORS - Settings";
                liSettings.Attributes.Add("class", "active");
            }
            //else if (uri.Contains("Pages/ProjectIP.aspx"))
            //{
            //    liManagePartners.Attributes.Add("class", "active");
            //}
            else if (uri.Contains("Admin/Location/AddNewLocation.aspx") || uri.Contains("Admin/Location/LocationsList.aspx"))
            {
                liLocations.Attributes.Add("class", "active");
            }
            else if (uri.Contains("Reports/ORS3W.aspx"))
            {
                PageTitle = "ORS - ORS 3W";
                liORS3W.Attributes.Add("class", "active");
            }

            else if (uri.Contains("Anonymous/ActivitiesFrameworkPublic.aspx"))
            {
                PageTitle = "ORS - Cluster Framework";
                liActivitesFrameworkPublic.Attributes.Add("class", "active");
            }

            else if (uri.Contains("ContactUs.aspx"))
            {
                PageTitle = "ORS - Contact Us";
            }

            else if (uri.Contains("faq.aspx"))
            {
                PageTitle = "ORS - FAQ";
            }
        }

        protected void lnkLanguageEnglish_Click(object sender, EventArgs e)
        {
            try
            {
                RC.SelectedSiteLanguageId = (int)RC.SiteLanguage.English;
                RC.AddSiteLangInCookie(this.Response, RC.SiteLanguage.English);
                (MainContent.Page as BasePage).BindGridData();
            }
            catch { }
        }

        protected void lnkLanguageFrench_Click(object sender, EventArgs e)
        {
            try
            {
                RC.SelectedSiteLanguageId = (int)Common.RC.SiteLanguage.French;
                RC.AddSiteLangInCookie(this.Response, Common.RC.SiteLanguage.French);
                (MainContent.Page as BasePage).BindGridData();
            }
            catch { }
        }

        protected void HeadLoginStatus_LoggedOut(object sender, EventArgs e)
        {
            HttpContext.Current.Session["UserCountry"] = null;
            HttpContext.Current.Session["UserCluster"] = null;
            HttpContext.Current.Session["UserOrg"] = null;
            HttpContext.Current.Session["UserOrgName"] = null;
            HttpContext.Current.Session["UserLocationEmergencyId"] = null;
            HttpContext.Current.Session["UserCountryName"] = null;
            HttpContext.Current.Session["UserEmergencyClusterId"] = null;
            HttpContext.Current.Session["EmergencyId"] = null;
        }

        // Gets the ASP.NET application's virtual application root path on the server.
        private static string VirtualFolder
        {
            get { return HttpContext.Current.Request.ApplicationPath; }
        }

        // This property is to use it in markup where css and js files
        // use this to create virtualpath.
        public string BaseURL
        {
            get
            {
                return string.Format("http://{0}{1}",
                                     HttpContext.Current.Request.ServerVariables["HTTP_HOST"],
                                     (VirtualFolder.Equals("/")) ? string.Empty : VirtualFolder);
            }
        }
    }
}
