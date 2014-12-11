using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using System.Linq;
using System.Web.UI.HtmlControls;
using BusinessLogic;
using System.Collections.Generic;

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

            if (RC.SelectedEmergencyId <= 0)
            {
                if (Request.Cookies["SelectedEmergencyCookie"] != null)
                {
                    ddlEmgergency.SelectedValue = Request.Cookies["SelectedEmergencyCookie"].Value;
                    RC.SelectedEmergencyId = Convert.ToInt32(ddlEmgergency.SelectedValue);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetUserName();
            HideAllAuthenticatedMenues();
            LoadNotifications();

            if (!IsPostBack)
            {
                if (RC.SelectedEmergencyId > 0)
                {
                    ddlEmgergency.SelectedValue = RC.SelectedEmergencyId.ToString();
                }
                else
                {
                    RC.SelectedEmergencyId = Convert.ToInt32(ddlEmgergency.SelectedValue);
                }

                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    UserInfo.UserProfileInfo(RC.SelectedEmergencyId);
                }
            }

            if (RC.SelectedEmergencyId == 1)
            {
                liCountryReports.Visible = true;
                liMaps.Visible = true;
            }
            else
            {
                liCountryReports.Visible = false;
                liMaps.Visible = false;
            }

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //SetCurrentEmergency();

                LoginStatus.Visible = false;
                spanWelcome.Visible = true;
                liRegister.Visible = false;

                liPublicAllData.Visible = false;
                menuePublicAllData.Visible = false;

                ShowHideMenuesOnRole();

                //ShowAuthenticatedMenues();
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

            if (HttpContext.Current.User.IsInRole("OCHACountryStaff"))
            {
                ShowOCHACountryStaff();
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

        private void LoadNotifications()
        {
            try
            {
                using (ORSEntities db = new ORSEntities())
                {
                    int count = db.Notifications.Where(x => x.EmergencyLocationId == UserInfo.EmergencyCountry
                                                                            && x.EmergencyClusterId == UserInfo.EmergencyCluster
                                                                            && x.IsRead == false).Count();
                    lblNumberOfNotifications.Text = count.ToString();

                    rptNotifications.DataSource = db.Notifications.Where(x => x.EmergencyLocationId == UserInfo.EmergencyCountry
                                                                            && x.EmergencyClusterId == UserInfo.EmergencyCluster
                                                                            && x.IsRead == false)
                                                                  .Select(y => new { y.Notification1, y.PageURL, y.NotificationId });
                    rptNotifications.DataBind();
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
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

            liReports.Visible = isShow;
            menueReports.Visible = isShow;
            menuDataEntry.Visible = isShow;
            liDataEntry.Visible = isShow;
            menuManageProjects.Visible = isShow;
            liManageProject.Visible = isShow;
            menuManageActivities.Visible = isShow;
            liManageActivity.Visible = isShow;
            menuRegionalIndicators.Visible = isShow;
            liRegionalIndicators.Visible = isShow;
            menueSRPIndicators.Visible = isShow;
            liSRPIndicators.Visible = isShow;
            menueValidateIndicaotrs.Visible = isShow;
            liValidate.Visible = isShow;
            //liReportsTopIndicators1.Visible = isShow;
            //liReportsTopIndicatorsGeneral1.Visible = isShow;
            //liReportsTopIndicatorRegional1.Visible = isShow;
            liPivotTables.Visible = isShow;
            liPivotPerfMonitoring.Visible = isShow;
            liPivotNumberOfOrgs.Visible = isShow;
            liPivotOrgOperational.Visible = isShow;
            liCLprojectsListing.Visible = isShow;
            liSumOfCountryIndicators.Visible = isShow;
            liSumOfRegionalIndicators.Visible = isShow;
            liClusterTarget.Visible = isShow;
            liPivotSumOfCountryIndicators.Visible = isShow;
            liFundingStatus.Visible = isShow;
            liUserListing.Visible = isShow;
            liBulkImport.Visible = isShow;
            liBulkImportUser.Visible = isShow;
            liORSDocuments.Visible = isShow;
            liOrganizationList.Visible = isShow;
            liEmergency.Visible = isShow;
            liProgressSummary.Visible = isShow;
            liProjectXMLFeeds.Visible = isShow;
            liSettings.Visible = isShow;
            liActivities.Visible = isShow;
            liIndicators.Visible = isShow;
            liContactList.Visible = isShow;

            liSumOfRegionalIndicators.Visible = isShow;
            liCountryReports.Visible = isShow;
            liLocations.Visible = isShow;
            liMapsListing.Visible = isShow;
            liMaps.Visible = isShow;
            //liReport.Visible = isShow;
            liClusterFrameworks.Visible = isShow;
            //liCustomReport.Visible = isShow;
            //menueCustomReport.Visible = isShow;
            liPublicAllData.Visible = isShow;
            

        }

        private void ShowUserMenue()
        {
            bool isShow = true;

            //liReports.Visible = isShow;
            menueReports.Visible = isShow;
            menuDataEntry.Visible = isShow;
            liDataEntry.Visible = isShow;
            menuManageProjects.Visible = isShow;
            liManageProject.Visible = isShow;
            menuManageActivities.Visible = isShow;
            liManageActivity.Visible = isShow;
            //liReportsSummaryReports.Visible = !isShow;
            liPivotOrgOperational.Visible = !isShow;
            menuePivotOPrPresence.Visible = !isShow;
            liPivotNumberOfOrgs.Visible = !isShow;
            menuePivotOrgPresence.Visible = !isShow;
            liPivotSumOfCountryIndicators.Visible = !isShow;
            menuePivotSumOfCountryIndicators.Visible = !isShow;
            liBulkImportUser.Visible = isShow;
            liORSDocuments.Visible = isShow;
            menueORSDocuments.Visible = isShow;
            liCLprojectsListing.Visible = isShow;
            liClusterFrameworks.Visible = !isShow;
            if (RC.SelectedEmergencyId == 1)
            {
                liCountryReports.Visible = isShow;
                liMaps.Visible = isShow;
            }
        }

        private void ShowRegionalLeadMenue()
        {
            bool isShow = true;
            if (RC.SelectedEmergencyId == 1)
            {
                //liReports.Visible = isShow;
                menueReports.Visible = isShow;
                menuRegionalIndicators.Visible = isShow;
                liRegionalIndicators.Visible = isShow;
                liSumOfRegionalIndicators.Visible = isShow;
                liCLprojectsListing.Visible = isShow;
                liCountryReports.Visible = isShow;
                liMaps.Visible = isShow;
            }

            liClusterFrameworks.Visible = isShow;
        }

        private void ShowClusterLeadMenue()
        {
            bool isShow = true;

            if (RC.SelectedEmergencyId == 1)
            {
                //liReports.Visible = isShow;
                menueReports.Visible = isShow;
                menueSRPIndicators.Visible = isShow;
                menueValidateIndicaotrs.Visible = isShow;
                liValidate.Visible = isShow;
                liCLprojectsListing.Visible = isShow;
                liSumOfCountryIndicators.Visible = isShow;
                liSRPIndicators.Visible = isShow;
                liClusterTarget.Visible = isShow;
                liCountryReports.Visible = isShow;
                liMaps.Visible = isShow;
                liPivotSumOfCountryIndicators.Visible = isShow;
                liBulkImport.Visible = isShow;
                liORSDocuments.Visible = isShow;
                menueORSDocuments.Visible = isShow;
            }
            liClusterFrameworks.Visible = isShow;
        }

        private void ShowOCHAMenue()
        {
            bool isShow = true;
            if (RC.SelectedEmergencyId == 1)
            {
                liFundingStatus.Visible = isShow;
                //liReports.Visible = isShow;
                menueReports.Visible = isShow;
                liContactList.Visible = isShow;
                liCountryReports.Visible = isShow;
                liMaps.Visible = isShow;
            }
        }

        private void ShowCountryAdminMenue()
        {
            bool isShow = true;
            if (RC.SelectedEmergencyId == 1)
            {
                //liReports.Visible = isShow;
                menueReports.Visible = isShow;
                menueSRPIndicators.Visible = isShow;

                menueValidateIndicaotrs.Visible = isShow;
                liValidate.Visible = isShow;
                liCLprojectsListing.Visible = isShow;
                liSumOfCountryIndicators.Visible = isShow;
                liSRPIndicators.Visible = isShow;
                liClusterTarget.Visible = isShow;
                liContactList.Visible = isShow;
                liCountryReports.Visible = isShow;
                liMaps.Visible = isShow;
                liPivotSumOfCountryIndicators.Visible = isShow;
                liBulkImport.Visible = isShow;
                liUserListing.Visible = isShow;
                liProgressSummary.Visible = isShow;
                liSettings.Visible = isShow;
            }
            liClusterFrameworks.Visible = isShow;
        }

        private void ShowOCHACountryStaff()
        {
            bool isShow = true;
            if (RC.SelectedEmergencyId == 1)
            {
                //liReports.Visible = isShow;
                menueReports.Visible = isShow;
                menueSRPIndicators.Visible = isShow;
                liCLprojectsListing.Visible = isShow;
                liPivotSumOfCountryIndicators.Visible = isShow;
                liSumOfCountryIndicators.Visible = isShow;
                liCountryReports.Visible = isShow;
                liMaps.Visible = isShow;
            }
        }

        private void ShowAdminMenue()
        {
            bool isShow = true;
            liFundingStatus.Visible = isShow;
            liUserListing.Visible = isShow;
            liOrganizationList.Visible = isShow;
            liProjectXMLFeeds.Visible = isShow;
            liEmergency.Visible = isShow;
            liProgressSummary.Visible = isShow;
            liSettings.Visible = isShow;
            liIndicators.Visible = isShow;
            liActivities.Visible = isShow;
            liLocations.Visible = isShow;

            liClusterFrameworks.Visible = isShow;

            if (RC.SelectedEmergencyId == 1)
            {
                liCountryReports.Visible = isShow;
                liMapsListing.Visible = isShow;
                liMaps.Visible = isShow;
            }
        }

        private void ActiveMenueItem()
        {
            string uri = HttpContext.Current.Request.Url.AbsolutePath;
            if (uri == "/Default.aspx")
            {
                liDashboards.Attributes.Add("class", "active open");
                liDefault.Attributes.Add("class", "active");
                PageTitle = "ORS - Home";
            }
            else if (uri == "/ReportingStatus.aspx")
            {
                liDashboards.Attributes.Add("class", "active open");
                //liReportingStatus.Attributes.Add("class", "active");
                PageTitle = "ORS - Achievements";
            }
            //else if (uri == "/OperationalPresenceDB.aspx")
            //{
            //    liDashboards.Attributes.Add("class", "active open");
            //    liOperationalPresenceDB.Attributes.Add("class", "active");
            //}
            else if (uri == "/Pages/AddActivities.aspx")
            {
                PageTitle = "ORS - Add Activity";

                liDataEntry.Attributes.Add("class", "active");
            }
            else if (uri == "/Pages/CreateProject.aspx")
            {
                PageTitle = "ORS - Manage Project";

                liManageProject.Attributes.Add("class", "active");
            }
            else if (uri == "/Pages/ManageActivities.aspx")
            {
                PageTitle = "ORS - Manage Activity";

                liManageActivity.Attributes.Add("class", "active");
            }
            else if (uri == "/Anonymous/AllData.aspx")
            {
                PageTitle = "ORS - Reports";

                liReports.Attributes.Add("class", "active open");
                //liCustomReport.Attributes.Add("class", "active");
                liPublicAllData.Attributes.Add("class", "active open");
            }
            else if (uri == "/Anonymous/AllDataPublic.aspx")
            {
                liPublicAllData.Attributes.Add("class", "active");
            }
            else if (uri.Contains("UploadAchived.aspx"))
            {
                PageTitle = "ORS - Bulk Upload";
                liBulkImport.Attributes.Add("class", "active");
            }
            else if (uri.Contains("/ORSDocuments/orsdocs.aspx"))
            {
                PageTitle = "ORS - Documents";
                liORSDocuments.Attributes.Add("class", "active");
            }
            else if (uri == "/ClusterLead/AddSRPActivitiesFromMasterList.aspx")
            {
                liSRPIndicators.Attributes.Add("class", "active");
            }
            else if (uri == "/ClusterLead/ClusterIndicatorTargets.aspx")
            {
                liClusterTarget.Attributes.Add("class", "active");
            }
            else if (uri == "/ClusterLead/ValidateReportList.aspx")
            {
                liValidate.Attributes.Add("class", "active open");
                liValidateAchievements.Attributes.Add("class", "active");
            }
            else if (uri == "/ClusterLead/ApproveIndicatorAddRemove.aspx")
            {
                liValidate.Attributes.Add("class", "active open");
                liValidateIndicators.Attributes.Add("class", "active");
            }
            else if (uri == "/RegionalLead/ManageRegionalIndicators.aspx")
            {
                PageTitle = "ORS - Reg. Indicators";
                liRegionalIndicators.Attributes.Add("class", "active");
            }
            else if (uri == "/Reports/OperationalPresence.aspx")
            {
                liReports.Attributes.Add("class", "active open");
                //liReportsSummaryReports.Attributes.Add("class", "active open");
                liContactList.Attributes.Add("class", "active");
            }
            else if (uri == "/ClusterLead/SumOfCountryIndicators.aspx")
            {
                liReports.Attributes.Add("class", "active open");
                //liReportsSummaryReports.Attributes.Add("class", "active open");
                liSumOfCountryIndicators.Attributes.Add("class", "active");
            }
            else if (uri == "/PivotTables/RegionalIndicatorsAchievedReport.aspx")
            {
                liReports.Attributes.Add("class", "active open");
                //liReportsSummaryReports.Attributes.Add("class", "active open");
                liSumOfRegionalIndicators.Attributes.Add("class", "active");
            }
            else if (uri == "/Reports/ProjectsPerOrganization.aspx")
            {
                liReports.Attributes.Add("class", "active open");
                //liReportsSummaryReports.Attributes.Add("class", "active open");
                //liReportsSummaryReportsProjectReports.Attributes.Add("class", "active open");
                //liReportsSummaryReportsProjectReportsByOrg.Attributes.Add("class", "active");
            }
            else if (uri == "/Reports/ProjectByClusters.aspx")
            {
                liReports.Attributes.Add("class", "active open");
                //liReportsSummaryReports.Attributes.Add("class", "active open");
                //liReportsSummaryReportsProjectReports.Attributes.Add("class", "active open");
                //liReportsSummaryReportsProjectReportsByClusters.Attributes.Add("class", "active");
            }
            else if (uri == "/Reports/ProjectsByCountry.aspx")
            {
                liReports.Attributes.Add("class", "active open");
                //liReportsSummaryReports.Attributes.Add("class", "active open");
                //liReportsSummaryReportsProjectReports.Attributes.Add("class", "active open");
                //liReportsSummaryReportsProjectReportsByCountry.Attributes.Add("class", "active");
            }
            else if (uri == "/PivotTables/PerfMontPivot.aspx")
            {
                PageTitle = "ORS - Perf. Monitoring";

                liPivotTables.Attributes.Add("class", "active open");
                liPivotPerfMonitoring.Attributes.Add("class", "active");
            }
            else if (uri == "/PivotTables/NumOfOrgsClsCnt.aspx")
            {
                PageTitle = "ORS - Op. Presence";

                liPivotTables.Attributes.Add("class", "active open");
                liPivotNumberOfOrgs.Attributes.Add("class", "active");
            }
            else if (uri == "/PivotTables/OprOrgsClsCnt.aspx")
            {
                PageTitle = "ORS - Org. Presence";

                liPivotTables.Attributes.Add("class", "active open");
                liPivotOrgOperational.Attributes.Add("class", "active");
            }
            else if (uri == "/PivotTables/CountryIndicatorsAchievedReport.aspx")
            {
                liPivotTables.Attributes.Add("class", "active open");
                liPivotSumOfCountryIndicators.Attributes.Add("class", "active");
            }
            else if (uri == "/ClusterLead/ProjectsListing.aspx" || uri.Contains("/ClusterLead/ProjectDetails.aspx"))
            {
                PageTitle = "ORS - Projects";
                liCLprojectsListing.Attributes.Add("class", "active");
            }
            else if (uri == "/Admin/UsersListing.aspx")
            {
                PageTitle = "ORS - User Details";

                liUserListing.Attributes.Add("class", "active");
            }
            else if (uri == "/LeadPages/FundingListing.aspx")
            {
                PageTitle = "ORS - Fundings";

                liFundingStatus.Attributes.Add("class", "active");
            }
            else if (uri == "/organization/OrganizationList.aspx")
            {
                PageTitle = "ORS - Organizations";

                liOrganizationList.Attributes.Add("class", "active");
            }
            else if (uri.Contains("ClusterLead/ValidateIndicators.aspx"))
            {
                liValidate.Attributes.Add("class", "active open");
                liValidateAchievements.Attributes.Add("class", "active");
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
            else if (uri.Contains("ClusterLead/CountryIndicators.aspx") || uri.Contains("ClusterLead/AddCountryIndicator.aspx"))
            {
                PageTitle = "ORS - Country Indicators";

                liClusterFrameworks.Attributes.Add("class", "active open");
                liClusterIndicators.Attributes.Add("class", "active");
            }
            else if (uri.Contains("ClusterLead/ActivityListing.aspx") || uri.Contains("ClusterLead/AddActivity.aspx")
                || uri.Contains("ClusterLead/EditActivity.aspx"))
            {
                PageTitle = "ORS - Activity Listing";

                liClusterFrameworks.Attributes.Add("class", "active open");
                liNewActivityListing.Attributes.Add("class", "active");
            }
            else if (uri.Contains("ClusterLead/IndicatorListing.aspx")
                || uri.Contains("AddActivityAndIndicators.aspx")
                || uri.Contains("ClusterLead/AddIndicators.aspx"))
            {
                PageTitle = "ORS - Indicator Listing";

                liClusterFrameworks.Attributes.Add("class", "active open");
                liNewIndicatorListing.Attributes.Add("class", "active");
            }
            else if (uri.Contains("ClusterLead/UploadSRP.aspx"))
            {
                PageTitle = "ORS - Import Framework";

                liClusterFrameworks.Attributes.Add("class", "active open");
                liClusterFrameworkImport.Attributes.Add("class", "active");
            }

            else if (uri.Contains("Admin/ActivityListing.aspx"))
            {
                PageTitle = "ORS - Activites";

                liActivities.Attributes.Add("class", "active");
            }
            else if (uri.Contains("Admin/IndicatorListing.aspx"))
            {
                PageTitle = "ORS - Indicators";

                liIndicators.Attributes.Add("class", "active");
            }
            else if (uri.Contains("Admin/ProgressSummary.aspx"))
            {
                PageTitle = "ORS - Progress Summary";

                liProgressSummary.Attributes.Add("class", "active");
            }
            else if (uri.Contains("Admin/ProjectXMLFeeds.aspx"))
            {
                PageTitle = "ORS - XML Feeds";

                liProjectXMLFeeds.Attributes.Add("class", "active");
            }
            else if (uri.Contains("CountryMapsListing") || uri.Contains("AddEditCountryMaps"))
            {
                PageTitle = "ORS - Country Maps";
                liMapsListing.Attributes.Add("class", "active open");
            }
            else if (uri.Contains("Admin/ConfigSettings.aspx"))
            {
                PageTitle = "ORS - Settings";

                liSettings.Attributes.Add("class", "active");
            }
            else if (uri.Contains("NewCountryReports") || uri.Contains("LoadCountryReport"))
            {
                PageTitle = "ORS - Country Reports";

                liCountryReports.Attributes.Add("class", "active open");
                //liCountryReports.Attributes.Add("class", "active");
            }
            else if (uri.Contains("CountryReports"))
            {
                PageTitle = "ORS - Country Reports";
                //liCountryConsolidatedReports.Attributes.Add("class", "active open");
                //liCountryReports.Attributes.Add("class", "active");
            }
            else if (uri.Contains("CountryMaps.aspx"))
            {
                PageTitle = "ORS - Country Maps";

                liMaps.Attributes.Add("class", "active open");
                //liCountryReports.Attributes.Add("class", "active");
            }
            else if (uri.Contains("Admin/Location/AddNewLocation.aspx") || uri.Contains("Admin/Location/LocationsList.aspx"))
            {
                liLocations.Attributes.Add("class", "active");
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

        protected void ddlEmg_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ActivityEmergencyId = ddlEmgergency.SelectedValue;
            RC.SelectedEmergencyId = Convert.ToInt32(ddlEmgergency.SelectedValue);
            Response.Cookies["SelectedEmergencyCookie"].Value = ddlEmgergency.SelectedValue;
            Response.Cookies["SelectedEmergencyCookie"].Expires = DateTime.Now.AddDays(365);

            if (RC.SelectedEmergencyId == 1)
            {
                liCountryReports.Visible = true;
                liMaps.Visible = true;
            }
            else
            {
                liCountryReports.Visible = false;
                liMaps.Visible = false;
            }

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserInfo.UserProfileInfo(RC.SelectedEmergencyId);
                HideAllAuthenticatedMenues();
                ShowHideMenuesOnRole();
            }
        }
    }
}
