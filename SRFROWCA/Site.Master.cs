using System;
using System.Web;
using System.Web.UI.WebControls;
using SRFROWCA.Common;

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
                        RC.SelectedSiteLanguageId = (int)Common.RC.SiteLanguage.French;
                        RC.SiteCulture = RC.FrenchCulture;
                    }
                    else
                    {
                        RC.SelectedSiteLanguageId = (int)Common.RC.SiteLanguage.English;
                        RC.SiteCulture = RC.EnglishCulture;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {                
                LoginStatus.Visible = false;
                HeadLoginStatus.Visible = true;
                ResiterStatus.Visible = false;
                if (!(HttpContext.Current.User.IsInRole("Admin"))
                    && !(HttpContext.Current.User.IsInRole("CountryAdmin")))
                {
                    AdminMenue.Visible = false;

                    if (HttpContext.Current.User.IsInRole("ClusterLead"))
                    {
                        menuMyActivities.HRef = "~/ClusterLead/AddSRPActivitiesFromMasterList.aspx";
                        spnManageActivities.InnerText = "SRP Activities";

                        menuManageProjects.HRef = "~/ClusterLead/ProjectsListing.aspx";
                        spnManageProject.InnerText = UserInfo.GetCountryName + " Projects";

                        menuDataEntry.Visible = false;
                    }
                    else
                    {
                        spnManageActivities.InnerText = "Manage Activities";
                        spnManageProject.InnerText = "Manage Projects";
                    }
                }
                else
                {
                    
                    menuMyActivities.Visible = false;
                    menuDataEntry.Visible = false;
                }
            }
            else
            {
                LoginStatus.Visible = true;
                HeadLoginStatus.Visible = false;
                ResiterStatus.Visible = true;
                AdminMenue.Visible = false;
                MySettingsMenue.Visible = false;
                menuMyActivities.Visible = false;
                menuDataEntry.Visible = false;
            }
        }

        protected void lnkLanguageEnglish_Click(object sender, EventArgs e)
        {
            RC.SelectedSiteLanguageId = (int)RC.SiteLanguage.English;
            RC.AddSiteLangInCookie(this.Response, RC.SiteLanguage.English);
            (MainContent.Page as BasePage).BindGridData();
        }

        protected void lnkLanguageFrench_Click(object sender, EventArgs e)
        {
            RC.SelectedSiteLanguageId = (int)Common.RC.SiteLanguage.French;
            RC.AddSiteLangInCookie(this.Response, Common.RC.SiteLanguage.French);            
            (MainContent.Page as BasePage).BindGridData();
        }

        protected void HeadLoginStatus_LoggedOut(object sender, EventArgs e)
        {
            HttpContext.Current.Session["UserCountry"] = null;
            HttpContext.Current.Session["UserCluster"] = null;
            HttpContext.Current.Session["UserOrg"] = null;
        }

        // Gets the ASP.NET application's virtual application root path on the server.
        private static string VirtualFolder
        {
            get { return HttpContext.Current.Request.ApplicationPath; }
        }

        // This property is to use it in markup where css and js files
        // use this to create virtualpath.
        protected string BaseURL
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
