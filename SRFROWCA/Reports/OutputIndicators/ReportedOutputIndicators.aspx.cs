using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Web.UI.WebControls;


namespace SRFROWCA.Reports.OutputIndicators
{
    public partial class ReportedOutputIndicators : BasePage
    {
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
            DataTable dt = SetDataSource(true);
            if (dt.Rows.Count > 0)
            {
                gvClusterReports.VirtualItemCount = Convert.ToInt32(dt.Rows[0]["VirtualCount"].ToString());
            }

            gvClusterReports.DataSource = dt;
            gvClusterReports.DataBind();
        }

        private DataTable SetDataSource(bool paging)
        {
            int? countryId = null;
            int? clusterId = null;
            string monthIDs = null;
            string indicator = null;

            if (!string.IsNullOrEmpty(txtIndicatorName.Text.Trim()))
                indicator = txtIndicatorName.Text.Trim();

            if (Convert.ToInt32(ddlCluster.SelectedValue) > 0)
                clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            if (Convert.ToInt32(ddlCountry.SelectedValue) > 0)
                countryId = Convert.ToInt32(ddlCountry.SelectedValue);

            monthIDs = RC.GetSelectedValues(ddlMonth);

            int? isRegional = null;
            if (RC.IsRegionalClusterLead(this.User))
            {
                isRegional = 1;
            }
            else
            {
                isRegional = cbIncludeRegional.Checked ? (int?)null : 1;
            }

            int? pageSize = null;
            int? pageIndex = null;
            if (paging)
            {
                pageSize = gvClusterReports.PageSize;
                pageIndex = gvClusterReports.PageIndex;
            }

            int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);

            return DBContext.GetData("GetOutputIndicatorReports_2016", new object[] { indicator, countryId, clusterId, 
                                                                             RC.SelectedSiteLanguageId, yearId, monthIDs, 
                                                                             isRegional, pageSize, pageIndex });
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

            ddlCluster.Items.Insert(0, new ListItem("Select Cluster", "0"));
            ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));

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
            gvClusterReports.PageIndex = 0;
            LoadClusterReports();
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvClusterReports.PageIndex = 0;
            LoadClusterReports();
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvClusterReports.PageIndex = 0;
            LoadClusterReports();
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvClusterReports.PageIndex = 0;
            LoadClusterReports();
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvClusterReports.PageIndex = 0;
            LoadClusterReports();
        }

        protected void gvClusterReports_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = SetDataSource(true);

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
            gvClusterReports.PageIndex = 0;
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
                ObjPrToolTip.RegionalIndicatorIcon(e, 15);
                ObjPrToolTip.CountryIndicatorIcon(e, 16);

                UI.SetThousandSeparator(e.Row, "lblTarget");
                UI.SetThousandSeparator(e.Row, "lblCountryAchieved");
                UI.SetThousandSeparator(e.Row, "lblCountrySum");
            }
        }

        protected void btnExportToExcel_ServerClick(object sender, EventArgs e)
        {
            GridView gvExport = new GridView();
            DataTable dt = SetDataSource(false);

            RemoveColumnsFromDataTable(dt);

            dt.DefaultView.Sort = "Country, Cluster, Indicator, Unit";
            gvExport.DataSource = dt.DefaultView;
            gvExport.DataBind();

            string fileName = "ClusterIndicatorReport";
            string fileExtention = ".xls";
            ExportUtility.ExportGridView(gvExport, fileName, fileExtention, Response);
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            try
            {
                dt.Columns.Remove("IsSRP");
                dt.Columns.Remove("EmergencyLocationId");
                dt.Columns.Remove("EmergencyLocationIdSahel");
                dt.Columns.Remove("ClusterIndicatorId");
                dt.Columns.Remove("SiteLanguageId");
                dt.Columns.Remove("CreatedById");
                dt.Columns.Remove("ReportedDate");
                dt.Columns.Remove("UpdatedById");
                dt.Columns.Remove("UpdatedDate");
                dt.Columns.Remove("OriginalTarget");
                dt.Columns.Remove("IndicatorCalculationTypeId");
                dt.Columns.Remove("NumberOfRecords");
            }
            catch { }
        }

        private void SaveFiltersInSession()
        {
            int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
            int emgClusterId = RC.GetSelectedIntVal(ddlCluster);

            if (emgLocationId > 0)
                Session["ClusterDataEntryCountry"] = emgLocationId;
            else
                Session["ClusterDataEntryCountry"] = null;

            if (emgClusterId > 0)
                Session["ClusterDataEntryCluster"] = emgClusterId;
            else
                Session["ClusterDataEntryCluster"] = null;
        }

        private void SetFiltersFromSession()
        {
            if (Session["ClusterDataEntryCountry"] != null)
            {
                int emgLocationId = 0;
                int.TryParse(Session["ClusterDataEntryCountry"].ToString(), out emgLocationId);
                if (emgLocationId > 0)
                {
                    try
                    {
                        ddlCountry.SelectedValue = emgLocationId.ToString();
                    }
                    catch { }
                }
            }

            if (Session["ClusterDataEntryCluster"] != null)
            {
                int clusterId = 0;
                int.TryParse(Session["ClusterDataEntryCluster"].ToString(), out clusterId);
                if (clusterId > 0)
                {
                    try
                    {
                        ddlCluster.SelectedValue = clusterId.ToString();
                    }
                    catch { }
                }
            }
        }

        private void CliearFilterSession()
        {
            Session["ClusterDataEntryCountry"] = null;
            Session["ClusterDataEntryCluster"] = null;
        }

        protected void gvClusterReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClusterReports.PageIndex = e.NewPageIndex;
            gvClusterReports.SelectedIndex = -1;
            LoadClusterReports();
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}