using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using System.Linq;
using System.Web.UI.HtmlControls;
using BusinessLogic;

namespace SRFROWCA.Ebola
{
    public partial class Ebola : System.Web.UI.MasterPage
    {
        public string MetaDescription = "ROWCA ORS";
        public string PageTitle = "ORS - Home";

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

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                LoginStatus.Visible = false;
                spanWelcome.Visible = true;
                liRegister.Visible = false;

                if (HttpContext.Current.User.IsInRole("User"))
                {
                    ShowUserMenue();
                }

                if (HttpContext.Current.User.IsInRole("ClusterLead"))
                {
                    ShowClusterLeadMenue();
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

            ActiveMenueItem();
        }

        //private void SetCurrentEmergency()
        //{
        //    try
        //    {
        //        DataTable dtEmergencies = (DataTable)rptEmergencies.DataSource;
        //        if (dtEmergencies.Rows.Count > 0)
        //        {
        //            CurrentEmergency = Convert.ToString(dtEmergencies.Select("ID='" + UserInfo.Emergency + "'")[0]["Emergency"]);
        //        }
        //    }
        //    catch { }
        //}

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

        //private void LoadEmergencies()
        //{
        //    DataTable dtEmergencies = DBContext.GetData("uspEmergencies", new object[] { RC.SelectedSiteLanguageId });

        //    if (dtEmergencies.Rows.Count > 0)
        //    {
        //        rptEmergencies.DataSource = dtEmergencies.Select("IsSRP=1").CopyToDataTable();
        //        rptEmergencies.DataBind();
        //    }
        //}

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

        protected void btnEmergency_Click(object sender, EventArgs e)
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
            menuManageProjects.Visible = isShow;
            liManageProject.Visible = isShow;
            menuManageActivities.Visible = isShow;
            liManageActivity.Visible = isShow;
            liValidateAchievements.Visible = isShow;
            menueValidateAchievements.Visible = isShow;
            liActivities.Visible = isShow;
            liIndicators.Visible = isShow;
        }

        private void ShowUserMenue()
        {
            bool isShow = true;

            menuDataEntry.Visible = isShow;
            liDataEntry.Visible = isShow;
            menuManageProjects.Visible = isShow;
            liManageProject.Visible = isShow;
            menuManageActivities.Visible = isShow;
            liManageActivity.Visible = isShow;
        }


        private void ShowClusterLeadMenue()
        {
            bool isShow = true;
            liValidateAchievements.Visible = isShow;
            menueValidateAchievements.Visible = isShow;
        }

        private void ShowCountryAdminMenue()
        {
            bool isShow = true;
            liValidateAchievements.Visible = isShow;
            liActivities.Visible = isShow;
            liIndicators.Visible = isShow;
        }

        private void ShowAdminMenue()
        {
            bool isShow = true;
            liIndicators.Visible = isShow;
            liActivities.Visible = isShow;
        }

        private void ActiveMenueItem()
        {
            string uri = HttpContext.Current.Request.Url.AbsolutePath;
            if (uri == "/Ebola/Default.aspx")
            {
                //liDashboards.Attributes.Add("class", "active open");
                liDefault.Attributes.Add("class", "active open");
                PageTitle = "ORS - Home";
            }
            
            else if (uri == "/Ebola/ReportDataEntry.aspx")
            {
                PageTitle = "ORS - Add Activity";

                liDataEntry.Attributes.Add("class", "active");
            }
            else if (uri == "/Ebola/CreateProject.aspx")
            {
                PageTitle = "ORS - Manage Project";

                liManageProject.Attributes.Add("class", "active");
            }
            else if (uri == "/Ebola/ManageActivities.aspx")
            {
                PageTitle = "ORS - Manage Activity";

                liManageActivity.Attributes.Add("class", "active");
            }
            else if (uri == "/Ebola/AllData.aspx")
            {
                PageTitle = "ORS - Reports";

                liCustomReport.Attributes.Add("class", "active");
            }
            
            else if (uri == "/ClusterLead/ValidateReportList.aspx")
            {
                liValidateAchievements.Attributes.Add("class", "active open");
            }
            else if (uri.Contains("ClusterLead/ValidateIndicators.aspx"))
            {
                liValidateAchievements.Attributes.Add("class", "active open");
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