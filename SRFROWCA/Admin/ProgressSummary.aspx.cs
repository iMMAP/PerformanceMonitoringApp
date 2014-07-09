using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Admin
{
    public partial class ProgressSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData(null, null, "-1", "-1", "-1");
                //LoadCombo();
            }
        }

        private void LoadData(string startDate, string endDate, string countryID, string clusterID, string isOPSProject)
        {
            DataTable dtResult = DBContext.GetData("uspGetReportData", new object[] {startDate, endDate, countryID, clusterID, isOPSProject, RC.SelectedSiteLanguageId});

            if (dtResult.Rows.Count > 0)
            {
                lblUsers.Text = Convert.ToString(dtResult.Rows[0]["UserCount"]);
                lblOrganizations.Text = Convert.ToString(dtResult.Rows[0]["OrganizationCount"]);
                lblReportedOrgs.Text = Convert.ToString(dtResult.Rows[0]["ReportedOrganizationCount"]);
                lblReportedCountries.Text = Convert.ToString(dtResult.Rows[0]["CountriesCount"]);
                lblReports.Text = Convert.ToString(dtResult.Rows[0]["ReportCount"]);
                lblReportedProjects.Text = Convert.ToString(dtResult.Rows[0]["ReportedProjectCount"]);
            }
        }

        //private void LoadCombo()
        //{
        //    UI.FillClusters(ddlClusters, RC.SelectedSiteLanguageId);
        //    if (UserInfo.Cluster > 0)
        //    {
        //        ddlClusters.SelectedValue = UserInfo.Cluster.ToString();
        //        ddlClusters.Visible = false;
        //    }

        //    UI.FillLocations(ddlCountry, RC.GetLocations(this.User, (int)RC.LocationTypes.National));
        //}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string startDate = !string.IsNullOrEmpty(txtFromDate.Text)?txtFromDate.Text:null;
            string endDate = !string.IsNullOrEmpty(txtToDate.Text)?txtToDate.Text:null;
            //string countryID = ddlCountry.SelectedValue;
            //string clusterID = ddlClusters.SelectedValue;
            string isOPSProject = rbIsOPSProject.SelectedValue;

            LoadData(startDate, endDate, "-1", "-1", isOPSProject);
        }

        protected void btnPDFPrint_Click(object sender, EventArgs e)
        {
            string startDate = !string.IsNullOrEmpty(txtFromDate.Text) ? txtFromDate.Text : null;
            string endDate = !string.IsNullOrEmpty(txtToDate.Text) ? txtToDate.Text : null;
            string isOPSProject = rbIsOPSProject.SelectedValue;

            DataTable dtResults = DBContext.GetData("uspGetReportData", new object[] { startDate, endDate, "-1", "-1", isOPSProject, RC.SelectedSiteLanguageId });

            if (dtResults.Rows.Count > 0)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}-{1}.pdf", UserInfo.CountryName, DateTime.Now.ToString("yyyyMMddHHmmss")));
                Response.BinaryWrite(WriteDataEntryPDF.GenerateSummaryPDF(dtResults).ToArray());
            }
        }
    }
}