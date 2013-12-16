using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;
using SRFROWCA.Common;
using System.Data;

namespace SRFROWCA
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void navBar_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            SiteMapNode currentRepeaterNode = (SiteMapNode)e.Item.DataItem;
            ((Repeater)e.Item.FindControl("subNavDropdown")).DataSource = currentRepeaterNode.ChildNodes;
            ((Repeater)e.Item.FindControl("subNavDropdown")).DataBind();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["SiteLanguage"] == null)
            {
                ROWCACommon.SelectedSiteLanguageId = (int) Common.ROWCACommon.SiteLanguage.English;
                ROWCACommon.SiteCulture = "en-US";

                if (Request.Cookies["SiteLanguageCookie"] != null)
                {
                    string siteLangId = Request.Cookies["SiteLanguageCookie"].Value;
                    if (siteLangId == "2")
                    {
                        ROWCACommon.SelectedSiteLanguageId = (int)Common.ROWCACommon.SiteLanguage.French;
                        ROWCACommon.SiteCulture = "fr-FR";
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

            //if (!IsPostBack)
            //{
            //    DataTable dt = ROWCACommon.GetUserDetails();
            //    if (dt.Rows.Count > 0)
            //    {
            //        lblOrganization.Text = "(" + dt.Rows[0]["OrganizationAcronym"].ToString() + ")";
            //    }
            //}

        }

        protected void lnkLanguageEnglish_Click(object sender, EventArgs e)
        {
            ROWCACommon.SelectedSiteLanguageId = (int)Common.ROWCACommon.SiteLanguage.English;
            ROWCACommon.AddSiteLangInCookie(this.Response, Common.ROWCACommon.SiteLanguage.English);
        }

        protected void lnkLanguageFrench_Click(object sender, EventArgs e)
        {
            ROWCACommon.SelectedSiteLanguageId = (int)Common.ROWCACommon.SiteLanguage.French;
            ROWCACommon.AddSiteLangInCookie(this.Response, Common.ROWCACommon.SiteLanguage.French);
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
