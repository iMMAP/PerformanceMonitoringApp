using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using BusinessLogic;
using Microsoft.Reporting.WebForms;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class ClusterReports : BasePage
    {
        public string CountryDisplayNone = string.Empty;
        public string ClusterDisplayNone = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCombos();
                DisableDropDowns();
                LoadClusterReports();
            }
        }

        private void SetComboValues()
        {
            if (RC.IsClusterLead(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
            }

            if (RC.IsCountryAdmin(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
            }

            if (RC.IsRegionalClusterLead(this.User))
            {
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
            }
        }

        private void LoadClusterReports()
        {
            gvClusterReports.DataSource = SetDataSource();
            gvClusterReports.DataBind();
        }

        private DataTable SetDataSource()
        {
            int? countryId = null;
            int? clusterId = null;
            string monthIDs = null;
            string indicator = null;

            if (!string.IsNullOrEmpty(txtIndicatorName.Text.Trim()))
                indicator = txtIndicatorName.Text.Trim();

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                countryId = Convert.ToInt32(ddlCountry.SelectedValue);

            monthIDs = RC.GetSelectedValues(ddlMonth);

            bool isRegional = RC.IsRegionalClusterLead(this.User);

            return DBContext.GetData("uspGetClusterReports", new object[] { indicator, countryId, clusterId, 
                                                                             RC.SelectedSiteLanguageId, monthIDs, isRegional });
        }

        internal override void BindGridData()
        {
            LoadCombos();
            LoadClusterReports();
        }

        private void LoadCombos()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            PopulateMonths();

            ddlCluster.Items.Insert(0, new ListItem("--- Select Cluster ---", "-1"));
            ddlCountry.Items.Insert(0, new ListItem("--- Select Country ---", "-1"));

            SetComboValues();
        }        

        private void DisableDropDowns()
        {
            if (RC.IsClusterLead(this.User))
            {
                RC.EnableDisableControls(ddlCluster, false);
                RC.EnableDisableControls(ddlCountry, false);
            }

            if (RC.IsCountryAdmin(this.User))
            {
                RC.EnableDisableControls(ddlCountry, false);
            }

            if (RC.IsRegionalClusterLead(this.User))
            {
                RC.EnableDisableControls(ddlCluster, false);
            }
        }

        private void PopulateMonths()
        {
            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();
        }

        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadClusterReports();
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterReports();
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterReports();
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterReports();
        }

        protected void gvClusterReports_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = SetDataSource();

            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvClusterReports.DataSource = dt;
                gvClusterReports.DataBind();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (RC.IsCountryAdmin(this.User))
            {
                RC.ClearSelectedItems(ddlCluster);
            }

            if (RC.IsAdmin(this.User))
            {
                RC.ClearSelectedItems(ddlCluster);
                RC.ClearSelectedItems(ddlCountry);
            }

            RC.ClearSelectedItems(ddlMonth);
            LoadClusterReports();
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        protected void gvClusterReports_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.RegionalIndicatorIcon(e, 11);
                ObjPrToolTip.CountryIndicatorIcon(e, 12);

                string siteCulture = RC.SelectedSiteLanguageId.Equals(1) ? "en-US" : "de-DE";

                Label lblRegionalAchieved = (Label)e.Row.FindControl("lblRegionalAchieved");
                if (lblRegionalAchieved != null && !string.IsNullOrEmpty(lblRegionalAchieved.Text))
                    lblRegionalAchieved.Text = String.Format(new CultureInfo(siteCulture), "{0:0,0}", Convert.ToInt32(lblRegionalAchieved.Text));

                Label lblTotalSum = (Label)e.Row.FindControl("lblTotalSum");
                if (lblTotalSum != null && !string.IsNullOrEmpty(lblTotalSum.Text))
                    lblTotalSum.Text = String.Format(new CultureInfo(siteCulture), "{0:0,0}", Convert.ToInt32(lblTotalSum.Text));

                Label lblCountryAchieved = (Label)e.Row.FindControl("lblCountryAchieved");
                if (lblCountryAchieved != null && !string.IsNullOrEmpty(lblCountryAchieved.Text))
                    lblCountryAchieved.Text = String.Format(new CultureInfo(siteCulture), "{0:0,0}", Convert.ToInt32(lblCountryAchieved.Text));

                Label lblCountrySum = (Label)e.Row.FindControl("lblCountrySum");
                if (lblCountrySum != null && !string.IsNullOrEmpty(lblCountrySum.Text))
                    lblCountrySum.Text = String.Format(new CultureInfo(siteCulture), "{0:0,0}", Convert.ToInt32(lblCountrySum.Text));

                Label lblOrigionalTarget = (Label)e.Row.FindControl("lblOrigionalTarget");
                if (lblOrigionalTarget != null && !string.IsNullOrEmpty(lblOrigionalTarget.Text))
                    lblOrigionalTarget.Text = String.Format(new CultureInfo(siteCulture), "{0:0,0}", Convert.ToInt32(lblOrigionalTarget.Text));

                Label lblTarget = (Label)e.Row.FindControl("lblTarget");
                if (lblTarget != null && !string.IsNullOrEmpty(lblTarget.Text))
                    lblTarget.Text = String.Format(new CultureInfo(siteCulture), "{0:0,0}", Convert.ToInt32(lblTarget.Text));

                Label lblAchieved = (Label)e.Row.FindControl("lblAchieved");
                if (lblAchieved != null && !string.IsNullOrEmpty(lblAchieved.Text))
                    lblAchieved.Text = String.Format(new CultureInfo(siteCulture), "{0:0,0}", Convert.ToInt32(lblAchieved.Text));
            }
        }

        protected void btnExportToExcel_ServerClick(object sender, EventArgs e)
        {
            GridView gvExport = new GridView();
            DataTable dt = SetDataSource();

            RemoveColumnsFromDataTable(dt);

            dt.DefaultView.Sort = "Country, Cluster, Indicator, Unit";
            gvExport.DataSource = dt.DefaultView;
            gvExport.DataBind();

            string fileName = "ClusterIndicatorReport";
            string fileExtention = ".xls";
            ExportUtility.ExportGridView(gvExport, fileName, fileExtention, Response);
        }

        protected void ExportToPDF(object sender, EventArgs e)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            byte[] bytes;
            ReportViewer rvCountry = new ReportViewer();
            rvCountry.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://win-78sij2cjpjj/Reportserver");
            //rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://54.83.26.190/Reportserver");
            ReportParameter[] RptParameters = null;
            //rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://localhost/Reportserver");
            string countryId = null;
            string countryIds = null;
            string clusterId = null;
            string clusterIds = null;
            string monthID = null;
            string monthIDs = null;
            string indicator = null;

            if (!string.IsNullOrEmpty(txtIndicatorName.Text.Trim()))
                indicator = txtIndicatorName.Text.Trim();

            //if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
            //countryId = Convert.ToInt32(ddlCountry.SelectedValue);

            countryIds = RC.GetSelectedValues(ddlCountry);

            //if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
            //    clusterId = ddlCluster.SelectedValue;

            //if (Convert.ToInt32(ddlMonth.SelectedValue) > -1)
            //    monthID = ddlMonth.SelectedValue;

            clusterIds = RC.GetSelectedValues(ddlCluster);
            monthIDs = RC.GetSelectedValues(ddlMonth);

            RptParameters = new ReportParameter[9];
            RptParameters[0] = new ReportParameter("pClusterID", clusterId, false);
            RptParameters[1] = new ReportParameter("pCountryID", countryId, false);
            RptParameters[2] = new ReportParameter("pIndicator", indicator, false);
            RptParameters[3] = new ReportParameter("pLocationIDs", countryIds, false);
            RptParameters[4] = new ReportParameter("pLangId", ((int)RC.SiteLanguage.English).ToString(), false);
            RptParameters[5] = new ReportParameter("pIsRegional", cbIncludeRegional.Checked ? "true" : "false", false);
            RptParameters[6] = new ReportParameter("pMonthID", monthID, false);
            RptParameters[7] = new ReportParameter("pMonthIDs", monthIDs, false);
            RptParameters[8] = new ReportParameter("pClusterIDs", clusterIds, false);

            rvCountry.ServerReport.ReportPath = "/reports/outputreport";
            string fileName = "ClusterIndicatorReport" + DateTime.Now.ToString("yyyy-MM-dd_hh_mm_ss") + ".pdf";
            rvCountry.ServerReport.ReportServerCredentials = new ReportServerCredentials("Administrator", "&qisW.c@Jq", "");
            rvCountry.ServerReport.SetParameters(RptParameters);
            bytes = rvCountry.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush();
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            try
            {
                dt.Columns.Remove("IsSRP");
                //dt.Columns.Remove("IsRegional");
                dt.Columns.Remove("EmergencyLocationId");
                dt.Columns.Remove("EmergencyLocationIdSahel");                
                dt.Columns.Remove("ClusterIndicatorId");
                dt.Columns.Remove("SiteLanguageId");
                dt.Columns.Remove("CreatedById");
                dt.Columns.Remove("CreatedDate");
                dt.Columns.Remove("UpdatedById");
                dt.Columns.Remove("UpdatedDate");
            }
            catch { }
        }
    }
}