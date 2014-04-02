﻿using System;
using System.Web;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;

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
            HideAllAuthenticatedMenues();

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                LoginStatus.Visible = false;
                spanWelcome.Visible = true;

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

                ShowAuthenticatedMenues();
            }

            ActiveMenueItem();
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
            liValidateIndicators.Visible = isShow;
            liReportsTopIndicators1.Visible = isShow;
            liReportsTopIndicatorsGeneral1.Visible = isShow;
            liReportsTopIndicatorRegional1.Visible = isShow;
            liBulkImport.Visible = isShow;
            menueBulkImport.Visible = isShow;
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
        }

        private void ShowRegionalLeadMenue()
        {
            bool isShow = true;
            liReports.Visible = isShow;
            menueReports.Visible = isShow;
            menuRegionalIndicators.Visible = isShow;
            liRegionalIndicators.Visible = isShow;
            liReportsTopIndicators1.Visible = isShow;
            liReportsTopIndicatorsGeneral1.Visible = isShow;
            liReportsTopIndicatorRegional1.Visible = isShow;
        }

        private void ShowClusterLeadMenue()
        {
            bool isShow = true;

            liReports.Visible = isShow;
            menueReports.Visible = isShow;
            menueSRPIndicators.Visible = isShow;
            liSRPIndicators.Visible = isShow;
            menueValidateIndicaotrs.Visible = isShow;
            liValidateIndicators.Visible = isShow;
            liReportsTopIndicators1.Visible = isShow;
            liReportsTopIndicatorsGeneral1.Visible = isShow;
            liReportsTopIndicatorRegional1.Visible = isShow;
        }

        private void ShowAuthenticatedMenues()
        {
            bool isShow = true;
            liBulkImport.Visible = isShow;
            menueBulkImport.Visible = isShow;
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
            }
            else if (uri == "/ClusterLead/AddSRPActivitiesFromMasterList.aspx")
            {
                liSRPIndicators.Attributes.Add("class", "active");
            }
            else if (uri == "/ClusterLead/ValidateReportList.aspx")
            {
                liValidateIndicators.Attributes.Add("class", "active");
            }
            else if (uri == "/ClusterLead/ValidateIndicators.aspx")
            {
                liValidateIndicators.Attributes.Add("class", "active");
            }
            else if (uri == "/RegionalLead/ManageRegionalIndicators.aspx")
            {
                liRegionalIndicators.Attributes.Add("class", "active");
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
            //else if (uri == "/Reports/TopIndicatorsGeneral.aspx")
            //{
            //    liReports.Attributes.Add("class", "active open");
            //    liReportsTopIndicators.Attributes.Add("class", "active open");
            //    liReportsTopIndicatorsGeneral.Attributes.Add("class", "active");
            //}
            //else if (uri == "/Reports/TopIndicaotrsRegional.aspx")
            //{
            //    liReports.Attributes.Add("class", "active open");
            //    liReportsTopIndicators.Attributes.Add("class", "active open");
            //    liReportsTopIndicatorRegional.Attributes.Add("class", "active");
            //}

            //foreach (Control ctrl in nav.Controls)
            //{

            //    if (ctrl is HtmlAnchor)
            //    {
            //        string absoluteURL = ((HtmlAnchor)ctrl).HRef;
            //        string url = absoluteURL.Substring(absoluteURL.IndexOf('/'));
            //        string uri = HttpContext.Current.Request.Url.AbsolutePath;
            //        if (url == uri)
            //        {
            //            HtmlGenericControl c = (HtmlGenericControl)ctrl;
            //            c.Attributes.Add("class", "active");
            //        }


            //        //if (url == GetCurrentPage())  // <-- you'd need to write that
            //          //  ctrl.Parent.Attributes.Add("class", "active");
            //    }
            //}
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
