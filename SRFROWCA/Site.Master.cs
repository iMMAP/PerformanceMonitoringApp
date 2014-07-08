using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using System.Linq;
using System.Web.UI.HtmlControls;

namespace SRFROWCA
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetUserName();

            HideAllAuthenticatedMenues();

            LoadNotifications();

            DataTable dt = new DataTable();
            dt.Columns.Add("FullName");
            dt.Columns.Add("Message");
            dt.Columns.Add("DateTime");
            dt.Rows.Add(new object[] { "Kashif", "Hello, There how are you?", DateTime.Now.AddDays(1) });
            dt.Rows.Add(new object[] { "Max M", "Added new report.", DateTime.Now });
            dt.Rows.Add(new object[] { "Some One", "New ORS project Added", DateTime.Now.AddDays(3) });
            //rptMessages.DataSource = dt;
            //rptMessages.DataBind();

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                LoginStatus.Visible = false;
                spanWelcome.Visible = true;
                liRegister.Visible = false;

                liPublicAllData.Visible = false;
                menuePublicAllData.Visible = false;

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

                ShowAuthenticatedMenues();
            }

            ActiveMenueItem();
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
                    //lblNumberOfNotifications2.Text = count.ToString();
                    
                                                                  

                    rptNotifications.DataSource = db.Notifications.Where(x => x.EmergencyLocationId == UserInfo.EmergencyCountry
                                                                            && x.EmergencyClusterId == UserInfo.EmergencyCluster
                                                                            && x.IsRead == false)
                                                                  .Select(y => new { y.Notification1, y.PageURL });
                    rptNotifications.DataBind();
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
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
            liActivities.Visible = isShow;
            liIndicators.Visible = isShow;
        }

        private void ShowUserMenue()
        {
            bool isShow = true;

            liReports.Visible = isShow;
            menueReports.Visible = isShow;
            menuDataEntry.Visible = isShow;
            liDataEntry.Visible = isShow;
            menuManageProjects.Visible = isShow;
            liManageProject.Visible = isShow;
            menuManageActivities.Visible = isShow;
            liManageActivity.Visible = isShow;
            liReportsSummaryReports.Visible = !isShow;
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
        }

        private void ShowRegionalLeadMenue()
        {
            bool isShow = true;
            liReports.Visible = isShow;
            menueReports.Visible = isShow;
            menuRegionalIndicators.Visible = isShow;
            liRegionalIndicators.Visible = isShow;
            liSumOfRegionalIndicators.Visible = isShow;
            //liReportsTopIndicators1.Visible = isShow;
            //liReportsTopIndicatorsGeneral1.Visible = isShow;
            //liReportsTopIndicatorRegional1.Visible = isShow;
        }

        private void ShowClusterLeadMenue()
        {
            bool isShow = true;

            liReports.Visible = isShow;
            menueReports.Visible = isShow;
            menueSRPIndicators.Visible = isShow;
            liSRPIndicators.Visible = isShow;
            menueValidateIndicaotrs.Visible = isShow;
            liValidate.Visible = isShow;
            liCLprojectsListing.Visible = isShow;
            liSumOfCountryIndicators.Visible = isShow;
            liClusterTarget.Visible = isShow;
            liPivotSumOfCountryIndicators.Visible = isShow;
            liBulkImport.Visible = isShow;
            liORSDocuments.Visible = isShow;
            menueORSDocuments.Visible = isShow;
            //liReportsTopIndicators1.Visible = isShow;
            //liReportsTopIndicatorsGeneral1.Visible = isShow;
            //liReportsTopIndicatorRegional1.Visible = isShow;
        }

        private void ShowAuthenticatedMenues()
        {
            bool isShow = true;
            //liBulkImport.Visible = isShow;
            liPivotTables.Visible = isShow;
            liPivotPerfMonitoring.Visible = isShow;
            liPivotNumberOfOrgs.Visible = isShow;
            liPivotOrgOperational.Visible = isShow;
        }

        private void ShowOCHAMenue()
        {
            bool isShow = true;
            liFundingStatus.Visible = isShow;
        }

        private void ShowCountryAdminMenue()
        {
            bool isShow = true;
            liReports.Visible = isShow;
            menueReports.Visible = isShow;
            menueSRPIndicators.Visible = isShow;
            liSRPIndicators.Visible = isShow;
            menueValidateIndicaotrs.Visible = isShow;
            liValidate.Visible = isShow;
            liCLprojectsListing.Visible = isShow;
            liSumOfCountryIndicators.Visible = isShow;
            liClusterTarget.Visible = isShow;
            liPivotSumOfCountryIndicators.Visible = isShow;
            liBulkImport.Visible = isShow;
            liUserListing.Visible = isShow;
        }

        private void ShowOCHACountryStaff()
        {
            bool isShow = true;
            liReports.Visible = isShow;
            menueReports.Visible = isShow;
            menueSRPIndicators.Visible = isShow;
            liCLprojectsListing.Visible = isShow;
            liSumOfCountryIndicators.Visible = isShow;
            liPivotSumOfCountryIndicators.Visible = isShow;
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
            liProjectXMLFeeds.Visible = isShow;
            liIndicators.Visible = isShow;
            liActivities.Visible = isShow;
        }

        private void ActiveMenueItem()
        {
            string uri = HttpContext.Current.Request.Url.AbsolutePath;
            if (uri == "/Default.aspx")
            {
                liDashboards.Attributes.Add("class", "active open");
                liDefault.Attributes.Add("class", "active");
            }
            else if (uri == "/ReportingStatus.aspx")
            {
                liDashboards.Attributes.Add("class", "active open");
                liReportingStatus.Attributes.Add("class", "active");
            }
            //else if (uri == "/OperationalPresenceDB.aspx")
            //{
            //    liDashboards.Attributes.Add("class", "active open");
            //    liOperationalPresenceDB.Attributes.Add("class", "active");
            //}
            else if (uri == "/Pages/AddActivities.aspx")
            {
                liDataEntry.Attributes.Add("class", "active");
            }
            else if (uri == "/Pages/CreateProject.aspx")
            {
                liManageProject.Attributes.Add("class", "active");
            }
            else if (uri == "/Pages/ManageActivities.aspx")
            {
                liManageActivity.Attributes.Add("class", "active");
            }
            else if (uri == "/Anonymous/AllData.aspx")
            {
                liReports.Attributes.Add("class", "active open");
                liCustomReport.Attributes.Add("class", "active");
                liPublicAllData.Attributes.Add("class", "active open");                
            }
            else if (uri == "/Anonymous/AllDataPublic.aspx")
            {
                liPublicAllData.Attributes.Add("class", "active");
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
                liRegionalIndicators.Attributes.Add("class", "active");
            }
            else if (uri == "/Reports/OperationalPresence.aspx")
            {
                liReports.Attributes.Add("class", "active open");
                liReportsSummaryReports.Attributes.Add("class", "active open");
                liContactList.Attributes.Add("class", "active");
            }
            else if (uri == "/ClusterLead/SumOfCountryIndicators.aspx")
            {
                liReports.Attributes.Add("class", "active open");
                liReportsSummaryReports.Attributes.Add("class", "active open");
                liSumOfCountryIndicators.Attributes.Add("class", "active");
            }
            else if (uri == "/PivotTables/RegionalIndicatorsAchievedReport.aspx")
            {
                liReports.Attributes.Add("class", "active open");
                liReportsSummaryReports.Attributes.Add("class", "active open");
                liSumOfRegionalIndicators.Attributes.Add("class", "active");
            }
            else if (uri == "/Reports/ProjectsPerOrganization.aspx")
            {
                liReports.Attributes.Add("class", "active open");
                liReportsSummaryReports.Attributes.Add("class", "active open");
                liReportsSummaryReportsProjectReports.Attributes.Add("class", "active open");
                liReportsSummaryReportsProjectReportsByOrg.Attributes.Add("class", "active");
            }
            else if (uri == "/Reports/ProjectByClusters.aspx")
            {
                liReports.Attributes.Add("class", "active open");
                liReportsSummaryReports.Attributes.Add("class", "active open");
                liReportsSummaryReportsProjectReports.Attributes.Add("class", "active open");
                liReportsSummaryReportsProjectReportsByClusters.Attributes.Add("class", "active");
            }
            else if (uri == "/Reports/ProjectsByCountry.aspx")
            {
                liReports.Attributes.Add("class", "active open");
                liReportsSummaryReports.Attributes.Add("class", "active open");
                liReportsSummaryReportsProjectReports.Attributes.Add("class", "active open");
                liReportsSummaryReportsProjectReportsByCountry.Attributes.Add("class", "active");
            }
            else if (uri == "/PivotTables/PerfMontPivot.aspx")
            {
                liPivotTables.Attributes.Add("class", "active open");
                liPivotPerfMonitoring.Attributes.Add("class", "active");
            }
            else if (uri == "/PivotTables/NumOfOrgsClsCnt.aspx")
            {
                liPivotTables.Attributes.Add("class", "active open");
                liPivotNumberOfOrgs.Attributes.Add("class", "active");
            }
            else if (uri == "/PivotTables/OprOrgsClsCnt.aspx")
            {
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
                liCLprojectsListing.Attributes.Add("class", "active");
            }
            else if (uri == "/Admin/UsersListing.aspx")
            {
                liUserListing.Attributes.Add("class", "active");
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
                liEmergency.Attributes.Add("class", "active open");
                liEmgList.Attributes.Add("class", "active");
            }
            else if (uri.Contains("/EmergencyLocations.aspx"))
            {
                liEmergency.Attributes.Add("class", "active open");
                liEmgLocation.Attributes.Add("class", "active");
            }
            else if (uri.Contains("/EmergencyClusters.aspx"))
            {
                liEmergency.Attributes.Add("class", "active open");
                liEmgCluster.Attributes.Add("class", "active");
            }
            else if (uri.Contains("Admin/ActivityListing.aspx"))
            {
                liActivities.Attributes.Add("class", "active");
            }
            else if (uri.Contains("Admin/IndicatorListing.aspx"))
            {
                liIndicators.Attributes.Add("class", "active");
            }
            else if (uri.Contains("Admin/ProgressSummary.aspx"))
            {
                liProgressSummary.Attributes.Add("class", "active");
            }
            else if (uri.Contains("Admin/ProjectXMLFeeds.aspx"))
            {
                liProjectXMLFeeds.Attributes.Add("class", "active");
            }
            else if (uri.Contains("Reports/CountriesForReports.aspx") || uri.Contains("CountryReports"))
            {
                liCountryReports.Attributes.Add("class", "active");
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

