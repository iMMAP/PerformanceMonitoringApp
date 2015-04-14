using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace SRFROWCA.Admin
{
    public partial class ProgressSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCombos();
                DisableDropDowns();
                LoadData(null, null, ddlCountry.SelectedValue, "-1", "-1");
            }
        }

        private void LoadData(string startDate, string endDate, string countryID, string clusterID, string isOPSProject)
        {
            
            DataTable dtResult = DBContext.GetData("uspGetReportDataNew", new object[] {startDate, endDate, countryID, clusterID, isOPSProject, RC.SelectedSiteLanguageId});

            if (dtResult.Rows.Count > 0)
            {
                lblSRPProjects.Text = Convert.ToString(dtResult.Rows[0]["SRPProjectsCount"]);
                lblSRPFunded.Text = Convert.ToString(dtResult.Rows[0]["SRPFundedProjects"]);
                lblSRPOrg.Text = Convert.ToString(dtResult.Rows[0]["SRPOrgCount"]);
                lblSRPReported.Text = Convert.ToString(dtResult.Rows[0]["SRPReportedProjects"]);
                lblReportingorg.Text = Convert.ToString(dtResult.Rows[0]["ReportedOrganizationCount"]);
                lblNonSRPPrj.Text = Convert.ToString(dtResult.Rows[0]["ProjectCount"]);
                lblNonSRPReported.Text = Convert.ToString(dtResult.Rows[0]["NonSRPReportedProjects"]);
                lblCountriesReporting.Text = Convert.ToString(dtResult.Rows[0]["CountriesCount"]);

                var distinctCountries = (from DataRow dRow in dtResult.Rows
                                         select new
                                         {
                                             Country = dRow["Country"],
                                             SRPReportedProjects = dRow["SRPProjectsReportedByCountry"],
                                             NONSRPReportedProjects = dRow["NONSRPProjectsReportedByCountry"],
                                             Users = dRow["Users"],
                                             Organizations = dRow["Organizations"],
                                             SRPProjectsPerCountry = dRow["SRPProjectsPerCountry"],
                                             NONSRPProjectsPerCountry = dRow["NonSRPProjectsPerCountry"],
                                             OrgPerCountry = dRow["OrgPerCountry"]

                                         })
                                     .Distinct();
                int count = 0;
                foreach (var country in distinctCountries)
                {
                    count++;
                    HtmlTableRow tr = new HtmlTableRow();

                    HtmlTableCell cell = new HtmlTableCell();
                    cell.InnerHtml = count.ToString();
                    tr.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.InnerHtml = Convert.ToString(country.Country);
                    tr.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.InnerHtml = Convert.ToString(country.SRPReportedProjects) + " / " + Convert.ToString(country.SRPProjectsPerCountry);
                    tr.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.InnerHtml = Convert.ToString(country.NONSRPReportedProjects) + " / " + Convert.ToString(country.NONSRPProjectsPerCountry);
                    tr.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.InnerHtml = Convert.ToString(country.Organizations) + " / " + Convert.ToString(country.OrgPerCountry);
                    tr.Cells.Add(cell);

                    tblCountry.Rows.Add(tr);
                }

                var Countries = (from DataRow dRow in dtResult.Rows
                                 select new
                                 {
                                     CountryID = dRow["CountryID"],
                                     Country = dRow["Country"]
                                 })
                                      .Distinct();

                int newCount = 0;
                foreach (var country in Countries)
                {
                    newCount = 0;
                    HtmlTableRow tr = new HtmlTableRow();
                    tr.Attributes.Add("class", "countryHeading");
                    HtmlTableCell cell = new HtmlTableCell();
                    cell.InnerHtml = country.Country.ToString();
                    cell.ColSpan = 2;
                    tr.Cells.Add(cell);
                    tblOrg.Rows.Add(tr);
                    var distinctOrganizations = (from DataRow dRow in dtResult.Rows
                                             where dRow.Field<int>("CountryID") == Convert.ToInt32(country.CountryID)
                                             select new
                                             {
                                                 CountryID = dRow["CountryID"],
                                                 OrganizationName = dRow["OrganizationName"]
                                             })
                                        .Distinct();

                     foreach (var org in distinctOrganizations)
                     {
                         tr = new HtmlTableRow();
                         tr.Attributes.Add("class", "countryData");
                         newCount++;
                         cell = new HtmlTableCell();
                         cell.InnerHtml = newCount.ToString();                        
                         tr.Cells.Add(cell);

                         cell = new HtmlTableCell();
                         cell.InnerHtml = Convert.ToString(org.OrganizationName);
                         tr.Cells.Add(cell);
                         tblOrg.Rows.Add(tr);
                     }

                     
                }
            }
           
        }

        private void LoadCombos()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            ddlCountry.Items.Insert(0, new ListItem("--- Select Country ---", "-1"));

            SetComboValues();
        }

        private void SetComboValues()
        {
            if (RC.IsCountryAdmin(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
            }
        }

        private void DisableDropDowns()
        {
            if (RC.IsCountryAdmin(this.User))
            {
                RC.EnableDisableControls(ddlCountry, false);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string startDate = !string.IsNullOrEmpty(txtFromDate.Text) ? txtFromDate.Text.Split('-')[2] + '-' + txtFromDate.Text.Split('-')[1] + '-' + txtFromDate.Text.Split('-')[0] : null;
            string endDate = !string.IsNullOrEmpty(txtToDate.Text) ? txtToDate.Text.Split('-')[2] + '-' + txtToDate.Text.Split('-')[1] + '-' + txtToDate.Text.Split('-')[0] : null;
            string countryID = ddlCountry.SelectedValue;
            //string clusterID = ddlClusters.SelectedValue;
            string isOPSProject = rbIsOPSProject.SelectedValue;

            LoadData(startDate, endDate, countryID, "-1", isOPSProject);
        }

        protected void btnPDFPrint_Click(object sender, EventArgs e)
        {
            string startDate = !string.IsNullOrEmpty(txtFromDate.Text) ? txtFromDate.Text.Split('-')[2] + '-' + txtFromDate.Text.Split('-')[1] + '-' + txtFromDate.Text.Split('-')[0] : null;
            string endDate = !string.IsNullOrEmpty(txtToDate.Text) ? txtToDate.Text.Split('-')[2] + '-' + txtToDate.Text.Split('-')[1] + '-' + txtToDate.Text.Split('-')[0] : null;
            string isOPSProject = rbIsOPSProject.SelectedValue;

            DataTable dtResults = DBContext.GetData("uspGetReportDataNew", new object[] { startDate, endDate, ddlCountry.SelectedValue, "-1", isOPSProject, RC.SelectedSiteLanguageId });

            if (dtResults.Rows.Count > 0)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}-{1}.pdf", UserInfo.CountryName, DateTime.Now.ToString("yyyyMMddHHmmss")));
                Response.BinaryWrite(WriteDataEntryPDF.GenerateSummaryPDF(dtResults, startDate, endDate).ToArray());
            }
            else
            {
                lblMessage.Text = "Error: No data to report!";
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            string startDate = !string.IsNullOrEmpty(txtFromDate.Text) ? txtFromDate.Text.Split('-')[2] + '-' + txtFromDate.Text.Split('-')[1] + '-' + txtFromDate.Text.Split('-')[0] : null;
            string endDate = !string.IsNullOrEmpty(txtToDate.Text) ? txtToDate.Text.Split('-')[2] + '-' + txtToDate.Text.Split('-')[1] + '-' + txtToDate.Text.Split('-')[0] : null;
            string countryID = ddlCountry.SelectedValue;
            //string clusterID = ddlClusters.SelectedValue;
            string isOPSProject = rbIsOPSProject.SelectedValue;

            LoadData(startDate, endDate, countryID, "-1", isOPSProject);
        }
    }
}