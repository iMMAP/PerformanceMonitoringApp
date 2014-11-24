﻿using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

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
                ShowHideControls();
                
                LoadClusterReports();
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
            string countryIds = null;
            int? clusterId = null;
            string clusterIds = null;
            int? monthID = null;
            string monthIDs = null;
            string indicator = null;

            if (!string.IsNullOrEmpty(txtIndicatorName.Text.Trim()))
                indicator = txtIndicatorName.Text.Trim();

            countryIds = RC.GetSelectedValues(ddlCountry);

            //if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
            //    clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            clusterIds = RC.GetSelectedValues(ddlCluster);

            //if (Convert.ToInt32(ddlMonth.SelectedValue) > -1)
            //    monthID = Convert.ToInt32(ddlMonth.SelectedValue);

            monthIDs = RC.GetSelectedValues(ddlMonth);

            return DBContext.GetData("uspGetClusterReports", new object[] { indicator, monthID, countryId, clusterId, RC.SelectedSiteLanguageId, cbIncludeRegional.Checked, countryIds, clusterIds, monthIDs });
        }

        internal override void BindGridData()
        {
            LoadCombos();
            LoadClusterReports();
        }

        private void LoadCombos()
        {
            UI.FillEmergencyLocations(ddlCountry, UserInfo.Emergency, RC.SelectedSiteLanguageId);
            UI.FillEmergnecyClusters(ddlCluster, RC.SelectedSiteLanguageId);
            PopulateMonths();


            //ddlCluster.Items.Insert(0, new ListItem("--- Select Cluster ---", "-1"));
            //ddlCountry.Items.Insert(0, new ListItem("--- Select Country ---", "-1"));
            //ddlMonth.Items.Insert(0, new ListItem("-- Select --", "-1"));
        }

        private void ShowHideControls()
        {
            if (RC.IsCountryAdmin(this.User))
            {
                lblCountry.Visible =
                    ddlCountry.Visible = false;

                ddlCountry.SelectedValue = Convert.ToString(UserInfo.EmergencyCountry);
                CountryDisplayNone = "display:none";
            }
            else if (RC.IsClusterLead(this.User))
            {
                lblCountry.Visible =
                    ddlCountry.Visible =
                        ddlCluster.Visible =
                            lblCluster.Visible = false;

                ddlCountry.SelectedValue = Convert.ToString(UserInfo.EmergencyCountry);
                ddlCluster.SelectedValue = Convert.ToString(UserInfo.EmergencyCluster);

                ClusterDisplayNone = "display:none";
            }

        }

        private void PopulateMonths()
        {
            int i = ddlMonth.SelectedIndex;

            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            //var result = DateTime.Now.ToString("MMMM", new CultureInfo(RC.SiteCulture));
            //result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);

            //int monthNumber = MonthNumber.GetMonthNumber(result);
            //monthNumber = monthNumber == 1 ? monthNumber : monthNumber - 1;

            //ddlMonth.SelectedIndex = i > -1 ? i : ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(monthNumber.ToString()));
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
                ObjPrToolTip.RegionalIndicatorIcon(e, 8);
                ObjPrToolTip.CountryIndicatorIcon(e, 9);

                Label lblTarget = (Label)e.Row.FindControl("lblTarget");
                Label lblAchieved = (Label)e.Row.FindControl("lblAchieved");
                string siteCulture = RC.SelectedSiteLanguageId.Equals(1) ? "en-US" : "de-DE";

                if (lblTarget != null)
                    lblTarget.Text = String.Format(new CultureInfo(siteCulture), "{0:0,0}", Convert.ToInt32(lblTarget.Text));

                if (lblAchieved != null)
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
           // rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://localhost/Reportserver");
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

            RptParameters = new ReportParameter[7];
            RptParameters[0] = new ReportParameter("pClusterID", clusterId, false);
            RptParameters[1] = new ReportParameter("pCountryID", countryId, false);
            RptParameters[2] = new ReportParameter("pIndicator", indicator, false);
            RptParameters[3] = new ReportParameter("pLocationIDs", countryIds, false);
            RptParameters[4] = new ReportParameter("pLangId", ((int)RC.SiteLanguage.English).ToString(), false);
            RptParameters[5] = new ReportParameter("pIsRegional", cbIncludeRegional.Checked ? "true" : "false", false);
            RptParameters[6] = new ReportParameter("pMonthID", monthID, false);

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
                dt.Columns.Remove("IsRegional");
                dt.Columns.Remove("EmergencyLocationId");
                dt.Columns.Remove("SiteLanguageId");

            }
            catch { }
        }
    }

}