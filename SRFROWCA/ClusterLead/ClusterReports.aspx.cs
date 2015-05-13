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

            if (Convert.ToInt32(ddlCluster.SelectedValue) > 0)
                clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            if (Convert.ToInt32(ddlCountry.SelectedValue) > 0)
                countryId = Convert.ToInt32(ddlCountry.SelectedValue);

            monthIDs = RC.GetSelectedValues(ddlMonth);

            bool isRegional = RC.IsRegionalClusterLead(this.User);

            return DBContext.GetData("GetOutputIndicatorReports", new object[] { indicator, countryId, clusterId, 
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

                UI.SetThousandSeparator(e.Row, "lblTarget");
                UI.SetThousandSeparator(e.Row, "lblCountryAchieved");
                UI.SetThousandSeparator(e.Row, "lblCountrySum");
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
    }
}