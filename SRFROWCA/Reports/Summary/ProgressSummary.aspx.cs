using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SRFROWCA.Reports.Summary
{
    public partial class ProgressSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCombos();
                LoadData();
            }
        }

        private void PopulateMonths()
        {
            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            if (ddlMonth.Items.Count > 0)
                ddlMonth.Items.Insert(0, new ListItem("All", "0"));
        }

        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private DataTable GetSummaryData()
        {
            int val = RC.GetSelectedIntVal(ddlMonth);
            int? monthId = val > 0 ? val : (int?)null;

            val = RC.GetSelectedIntVal(ddlCountry);
            int? emgLocationId = val > 0 ? val : (int?)null;

            val = RC.GetSelectedIntVal(ddlClusters);
            int? emgClusterId = val > 0 ? val : (int?)null;

            int? isOPS = null;

            isOPS = RC.GetSelectedIntVal(rbIsOPSProject);
            isOPS = isOPS == -1 ? (int?)null : isOPS;

            int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);

            return DBContext.GetData("ProgressReportSummary", new object[] { monthId, emgLocationId, emgClusterId, isOPS, yearId });
        }

        private DataTable GetCountryOrganizations()
        {
            //DateTime dt = DateTime.MinValue;
            //if (!string.IsNullOrEmpty(txtFromDate.Text.Trim()))
            //{
            //    DateTime.TryParse(txtFromDate.Text.Trim(), out dt);
            //}
            //DateTime? fromDate = dt == DateTime.MinValue ? (DateTime?)null : DateTime.ParseExact(dt.ToShortDateString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);

            //dt = DateTime.MinValue;
            //if (!string.IsNullOrEmpty(txtToDate.Text.Trim()))
            //{
            //    DateTime.TryParse(txtToDate.Text.Trim(), out dt);
            //}
            //DateTime? toDate = dt == DateTime.MinValue ? (DateTime?)null : DateTime.ParseExact(dt.ToShortDateString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);

            int val = RC.GetSelectedIntVal(ddlMonth);
            int? monthId = val > 0 ? val : (int?)null;

            val = RC.GetSelectedIntVal(ddlCountry);
            int? emgLocationId = val > 0 ? val : (int?)null;

            val = RC.GetSelectedIntVal(ddlClusters);
            int? emgClusterId = val > 0 ? val : (int?)null;

            int? isOPS = null;

            isOPS = RC.GetSelectedIntVal(rbIsOPSProject);
            isOPS = isOPS == -1 ? (int?)null : isOPS;

            int year = 2015;
            return DBContext.GetData("GetReportingOrganizationsByCountry", new object[] { monthId, emgLocationId, emgClusterId, isOPS, year });
        }

        private void LoadData()
        {

            DataTable dtResult = GetSummaryData();
            if (dtResult.Rows.Count > 0)
            {
                lblTotalProjects.Text = dtResult.Rows[0]["Projects"].ToString();
                lblTotalOrgs.Text = dtResult.Rows[0]["Organizations"].ToString();
                lblReportingProjects.Text = dtResult.Rows[0]["ReportingProjects"].ToString();
                lblReportingOrgs.Text = dtResult.Rows[0]["ReportingOrganizations"].ToString();
                lblReportingCountries.Text = dtResult.Rows[0]["ReportingCountriesCount"].ToString();
                lblReportsCount.Text = dtResult.Rows[0]["ReportsCount"].ToString();

                if (dtResult.Rows[0]["Country"].ToString() != "")
                {
                    var distinctCountries = (from DataRow dRow in dtResult.Rows
                                             select new
                                             {
                                                 Country = dRow["Country"],
                                                 TotalProjectsCountByCountry = dRow["TotalProjectsCountByCountry"],
                                                 ReportingProjectsCountByCountry = dRow["ReportingProjectsCountByCountry"],
                                                 TotalOrgsCountByCountry = dRow["TotalOrgsCountByCountry"],
                                                 ReportingOrgsCountByCountry = dRow["ReportingOrgsCountByCountry"],
                                                 TotalActivitiesByCountry = dRow["TotalActivitiesByCountry"],
                                                 ReportingActivitiesCountByCountry = dRow["ReportingActivitiesCountByCountry"],
                                                 TotalIndicatorsByCountry = dRow["TotalIndicatorsByCountry"],
                                                 ReportingIndicatorsCountByCountry = dRow["ReportingIndicatorsCountByCountry"],
                                             })
                                         .Distinct();
                    int count = 0;
                    foreach (var country in distinctCountries)
                    {
                        int projectCount = 0;
                        int.TryParse(country.TotalProjectsCountByCountry.ToString(), out projectCount);
                        if (projectCount > 0)
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
                            cell.InnerHtml = Convert.ToString(country.ReportingProjectsCountByCountry) + " out of " + Convert.ToString(country.TotalProjectsCountByCountry);
                            tr.Cells.Add(cell);

                            cell = new HtmlTableCell();
                            cell.InnerHtml = Convert.ToString(country.ReportingOrgsCountByCountry) + " out of " + Convert.ToString(country.TotalOrgsCountByCountry);
                            tr.Cells.Add(cell);

                            cell = new HtmlTableCell();
                            cell.InnerHtml = Convert.ToString(country.ReportingActivitiesCountByCountry) + " out of " + Convert.ToString(country.TotalActivitiesByCountry);
                            tr.Cells.Add(cell);

                            cell = new HtmlTableCell();
                            cell.InnerHtml = Convert.ToString(country.ReportingIndicatorsCountByCountry) + " out of " + Convert.ToString(country.TotalIndicatorsByCountry);
                            tr.Cells.Add(cell);

                            tblCountry.Rows.Add(tr);
                        }
                    }
                }

                DataTable dtOrgs = GetCountryOrganizations();
                var Countries = (from DataRow row in dtOrgs.Rows
                                 select new
                                 {
                                     CountryId = row["CountryId"],
                                     Country = row["Country"]
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
                    var distinctOrganizations = (from DataRow dRow in dtOrgs.Rows
                                                 where dRow.Field<int>("CountryId") == (int)country.CountryId
                                                 select new
                                                 {
                                                     OrganizationName = dRow["OrganizationName"]
                                                 }).ToList();


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
            PopulateMonths();
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            ddlCountry.Items.Insert(0, new ListItem("All", "0"));

            UI.FillEmergnecyClusters(ddlClusters, RC.EmergencySahel2015);
            ddlClusters.Items.Insert(0, new ListItem("All", "0"));

            SetComboValues();
        }

        private void SetComboValues()
        {
            if (!RC.IsAdmin(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlClusters.SelectedValue = UserInfo.EmergencyCluster.ToString();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnPDFPrint_Click(object sender, EventArgs e)
        {
            DataTable dtResults = GetSummaryData();
            DataTable dtCountryOrgs = GetCountryOrganizations();
            string month = ddlMonth.SelectedItem.Text;
            string country = ddlCountry.SelectedItem.Text;
            string cluster = ddlClusters.SelectedItem.Text;
            string year = ddlFrameworkYear.SelectedItem.Text;
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}-{1}.pdf", UserInfo.CountryName, DateTime.Now.ToString("yyyyMMddHHmmss")));
            Response.BinaryWrite(WriteDataEntryPDF.GenerateSummaryPDF(dtResults, dtCountryOrgs, month, country, cluster, year).ToArray());
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void rbIsOPSProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddlCountry.SelectedIndex = 0;
            ddlClusters.SelectedIndex = 0;
            ddlMonth.SelectedIndex = 0;
            rbIsOPSProject.SelectedIndex = 0;
            SetComboValues();
            LoadData();
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    
    }
}