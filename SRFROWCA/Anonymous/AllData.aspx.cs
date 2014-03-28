﻿using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Web;
using System.IO.Compression;
using SRFROWCA.Common;
using Saplin.Controls;
using SRFROWCA.Reports;
using System.Linq;
namespace SRFROWCA.Anonymous
{
    public partial class AllData : BasePage
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            // Load data in gridview.
            //LoadData();
            gvReport.DataSource = new DataTable();
            gvReport.DataBind();

            // Populate all drop downs.
            PopulateDropDowns();
        }

        #region Events.

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            PopulateDropDowns();
            txtFromDate.Text = "";
            txtToDate.Text = "";
            ddlFundingStatus.SelectedValue = "0";
            cbRegional.Checked = cbCountry.Checked = false;
            gvReport.DataSource = new DataTable();
            gvReport.DataBind();
        }

        protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetReportData();

            if (dt != null)
            {
                //Sort the data.
                if (dt.Rows.Count > 0)
                {
                    gvReport.VirtualItemCount = Convert.ToInt32(dt.Rows[0]["Cnt"]);
                }

                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvReport.DataSource = dt;
                gvReport.DataBind();
            }
        }

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            GridPageIndex = e.NewPageIndex;
            LoadData();
        }

        #region DropDown SelectedIndexChanged.

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataOnMultipleCheckBoxControl();
            PopulateActivities();
            PopulateIndicators();
        }

        protected void ddlActivities_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataOnMultipleCheckBoxControl();
            PopulateIndicators();
        }

        protected void ddlIndicators_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataOnMultipleCheckBoxControl();
        }

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataOnMultipleCheckBoxControl();
        }

        protected void ddlFundingStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LastLocationType = ReportsCommon.LocationType.Country;
            LoadDataOnMultipleCheckBoxControl();
            PopulateLocationDropDowns();
        }

        protected void ddlAdmin1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LastLocationType = ReportsCommon.LocationType.Admin1;
            LoadDataOnMultipleCheckBoxControl();
            PopulateAdmin2();
        }

        protected void ddlAdmin2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LastLocationType = ReportsCommon.LocationType.Admin2;
            LoadDataOnMultipleCheckBoxControl();
        }

        #endregion

        #endregion

        #region Class Methods.

        #region DropDown Methods.

        // Populate All drop downs.
        private void PopulateDropDowns()
        {
            PopulateClusters();
            PopulateLocations();
            PopulateMonths();
            PopulateOrganizations();
            PopulateObjectives();
            PopulatePriorities();
            PopulateActivities();
            PopulateIndicators();
            PopulateProjects();
        }

        private void PopulateProjects()
        {
            using (ORSEntities db = new ORSEntities())
            {
                ddlProjects.DataValueField = "ProjectId";
                ddlProjects.DataTextField = "ProjectCode";
                ddlProjects.DataSource = db.Projects.Select(x => new { x.ProjectId, x.ProjectCode }).OrderBy(o => o.ProjectCode);
                ddlProjects.DataBind();
            }
        }

        private void PopulateActivities()
        {
            string clusterIds = GetSelectedValues(ddlClusters);
            string objIds = GetSelectedValues(ddlObjectives);
            string priorityIds = GetSelectedValues(ddlPriority);

            if (clusterIds != null)
            {
                ddlActivities.DataTextField = "ActivityName";
                ddlActivities.DataValueField = "PriorityActivityId";
                ddlActivities.DataSource = DBContext.GetData("[GetActivitiesOfMultipleClusterObjAndPriorities]", new object[] { 1, clusterIds, objIds, priorityIds, 1 });
                ddlActivities.DataBind();
            }
        }

        private void PopulateIndicators()
        {
            string clusterIds = GetSelectedValues(ddlClusters);
            string objIds = GetSelectedValues(ddlObjectives);
            string priorityIds = GetSelectedValues(ddlPriority);
            string activityIds = GetSelectedValues(ddlActivities);

            if (clusterIds != null)
            {
                ddlIndicators.DataTextField = "DataName";
                ddlIndicators.DataValueField = "ActivityDataId";
                ddlIndicators.DataSource = DBContext.GetData("[GetIndicatorsOfMultipleClusterObjPriAndActivities]", new object[] { 1, clusterIds, objIds, priorityIds, activityIds, 1 });
                ddlIndicators.DataBind();
            }
        }

        private void PopulatePriorities()
        {
            UI.FillPriorities(ddlPriority);
        }

        private void PopulateObjectives()
        {
            UI.FillObjectives(ddlObjectives, true);
        }

        // Populate Clusters drop down.
        private void PopulateClusters()
        {
            UI.FillClusters(ddlClusters, 1);
        }

        // Populate Locations drop down
        private void PopulateLocations()
        {
            PopulateCountry();
            PopulateLocationDropDowns();
        }

        private void PopulateCountry()
        {
            UI.FillCountry(ddlCountry);
        }

        private void PopulateLocationDropDowns()
        {
            PopulateAdmin1();
            PopulateAdmin2();
        }

        private void PopulateAdmin1()
        {
            string countryIds = GetSelectedValues(ddlCountry);
            if (countryIds != null)
            {
                ddlAdmin1.DataValueField = "LocationId";
                ddlAdmin1.DataTextField = "LocationName";

                ddlAdmin1.DataSource = DBContext.GetData("GetAdmin1LocationsOfMultipleCountries", new object[] { countryIds });
                ddlAdmin1.DataBind();
            }
        }

        // Populate Locations drop down
        private void PopulateAdmin2()
        {
            string admin1Ids = GetSelectedValues(ddlAdmin1);
            string countryIds = GetSelectedValues(ddlCountry);
            if (countryIds != null)
            {

                ddlAdmin2.DataValueField = "LocationId";
                ddlAdmin2.DataTextField = "LocationName";
                ddlAdmin2.DataSource = DBContext.GetData("GetAdmin2LocationsOfMultipleCountries", new object[] { countryIds, admin1Ids });
                ddlAdmin2.DataBind();
            }
        }

        private object GetReportLocations()
        {
            return DBContext.GetData("GetAllLocationsInReports");
        }

        // Populate Months drop down
        private void PopulateMonths()
        {
            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();
        }

        private DataTable GetMonth()
        {
            return DBContext.GetData("GetMonths", new object[] { 1 });
        }

        private object GetUsers()
        {
            return DBContext.GetData("GetAllUsers");
        }

        private object GetOrganizationTypes()
        {
            return DBContext.GetData("GetOrganizationTypes");
        }

        // Populate Organizations drop down.
        private void PopulateOrganizations()
        {
            ddlOrganizations.DataValueField = "OrganizationId";
            ddlOrganizations.DataTextField = "OrganizationAcronym";
            int? orgId = null;
            ddlOrganizations.DataSource = GetOrganizations(orgId);
            ddlOrganizations.DataBind();
        }
        private object GetOrganizations(int? orgId)
        {
            return DBContext.GetData("GetOrganizations", new object[] { orgId });
        }

        private DataTable GetOffices()
        {
            int countryId = 0;
            int.TryParse(ddlCountry.SelectedValue, out countryId);

            if (countryId > 0)
            {
                return DBContext.GetData("GetOrganizationOffices", new object[] { countryId, (int?)null });
            }
            else
            {
                return DBContext.GetData("GetAllOffices");
            }
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            //foreach (ListItem item in cbColumns.Items)
            //{
            //    if (!item.Selected)
            //    {
            //        dt.Columns.Remove(item.Value);
            //    }
            //}

            dt.Columns.Remove("rnumber");
            dt.Columns.Remove("ObjectiveId");
            dt.Columns.Remove("PriorityId");
            dt.Columns.Remove("ActivityId");
            dt.Columns.Remove("IndicatorId");
            dt.Columns.Remove("MonthId");
            dt.Columns.Remove("cnt");

        }

        #endregion

        #region GridView Methods.

        private void LoadDataOnMultipleCheckBoxControl()
        {
            if (Session["DDEventAlreadyFired"] == null)
            {
                Session["DDEventAlreadyFired"] = "true";
                LoadData();
            }
            else
            {
                Session["DDEventAlreadyFired"] = null;
            }
        }

        // Load data on the basis of filter criteria.
        private void LoadData()
        {
            DataTable dt = GetReportData();

            // Cnt column has total number of records in resultset.
            // Gridview is using custom paging so to tell gridview that how many pages
            // it has to generate we have to set VirtualItemCount property with number of records            
            if (dt.Rows.Count > 0)
            {
                gvReport.VirtualItemCount = Convert.ToInt32(dt.Rows[0]["Cnt"]);
            }

            // rnumber and Cnt colums are not needed in gridview
            // so remove these two columns from datatable.
            dt.Columns.Remove("rnumber");
            dt.Columns.Remove("Cnt");

            gvReport.DataSource = dt;
            gvReport.DataBind();
        }

        // Get data from db.
        private DataTable GetReportData()
        {
            object[] paramValue = GetParamValues();
            return DBContext.GetData("GetAllTasksDataReport", paramValue);
        }

        // Get filter criteria and create an object with parameter values.
        private object[] GetParamValues()
        {
            string monthIds = GetSelectedValues(ddlMonth);
            string locationIds = GetLocationIds();
            string clusterIds = GetSelectedValues(ddlClusters);
            string orgIds = GetSelectedValues(ddlOrganizations);
            string objIds = GetSelectedValues(ddlObjectives);
            string prIds = GetSelectedValues(ddlPriority);
            string actIds = GetSelectedValues(ddlActivities);
            string indIds = GetSelectedValues(ddlIndicators);
            string projectIds = GetSelectedValues(ddlProjects);
            string fts = ddlFundingStatus.SelectedValue != "0" ? ddlFundingStatus.SelectedItem.Text : null;
            int? fromMonth = !string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? Convert.ToInt32(txtFromDate.Text.Trim().Substring(0, 2)) : (int?)null;
            int? toMonth = !string.IsNullOrEmpty(txtToDate.Text.Trim()) ? Convert.ToInt32(txtToDate.Text.Trim().Substring(0, 2)) : (int?)null;
            int? regionalInd = cbRegional.Checked ? 1 : (int?)null;
            int? countryInd = cbCountry.Checked ? 1 : (int?)null;
            int pageSize = gvReport.PageSize;
            int pageIndex = GridPageIndex; //gvReport.PageIndex;
            int langId = 1;

            //SetHFQueryString(monthIds, locationIds, clusterIds, orgIds);

            return new object[] { monthIds, locationIds, clusterIds, orgIds, 
                                    objIds, prIds, actIds, indIds, projectIds, fts,
                                    fromMonth, toMonth, regionalInd, countryInd,
                                    pageIndex, pageSize, Convert.ToInt32(SQLPaging), langId };
        }

        private string GetLocationIds()
        {
            string locationIds = null;
            if ((int)LastLocationType == 1)
            {
                locationIds = GetSelectedValues(ddlCountry);
            }
            else if ((int)LastLocationType == 2)
            {
                locationIds = GetSelectedValues(ddlAdmin1);
            }
            else if ((int)LastLocationType == 3)
            {
                locationIds = GetSelectedValues(ddlAdmin2);
            }

            if (locationIds == "0" || locationIds == null)
            {
                return null;
            }

            return locationIds;
        }

        // Get multiple selected values from drop down checkbox.
        private string GetSelectedValues(object sender)
        {
            string ids = GetSelectedItems(sender);
            ids = !string.IsNullOrEmpty(ids) ? ids : null;
            return ids;
        }

        // Get Selected Value from Drop Down.
        private int? GetSelectedValue(DropDownList ddl)
        {
            int val = 0;
            int.TryParse(ddl.SelectedValue, out val);
            return val > 0 ? val : (int?)null;
        }

        // set hidden field. This string is being used in XML DataFeed.
        private void SetHFQueryString(string emergencyIds, int? officeId, Guid userId, int? yearId, string monthIds,
                                                string locationIds, string clusterIds, string orgIds, string orgTypeIds)
        {
            if (!string.IsNullOrEmpty(emergencyIds) || officeId != null || yearId != null ||
                !string.IsNullOrEmpty(monthIds) || !string.IsNullOrEmpty(locationIds) ||
                !string.IsNullOrEmpty(clusterIds) || !string.IsNullOrEmpty(orgIds) || string.IsNullOrEmpty(orgTypeIds))
            {
                //hfReportLink.Value = string.Format("?emg={0}&cls={1}&loc={2}&y={3}&m={4}&u={5}&ot={6}&org={7}&ofc={8}",
                //                                     emergencyIds, clusterIds, locationIds, yearId, monthIds,
                //                                     userId, orgTypeIds, orgIds, officeId);
            }
        }

        private string GetSelectedItems(object sender)
        {
            string itemIds = "";
            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    if (itemIds != "")
                    {
                        itemIds += "," + item.Value;
                    }
                    else
                    {
                        itemIds += item.Value;
                    }
                }
            }

            return itemIds;
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

        protected void ExportToExcel(object sender, EventArgs e)
        {
            SQLPaging = PagingStatus.OFF;
            DataTable dt = GetReportData();
            SQLPaging = PagingStatus.ON;
            RemoveColumnsFromDataTable(dt);
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();

            ExportUtility.ExportGridView(gv, "ORS_CustomReport", ".xls", Response, true);
        }

        public override void VerifyRenderingInServerForm(Control control) { }

        #endregion

        #endregion

        #region Properties & Enum

        private ReportsCommon.LocationType LastLocationType
        {
            get
            {
                if (ViewState["LastLocationType"] != null)
                {
                    return (ReportsCommon.LocationType)ViewState["LastLocationType"];
                }
                else
                {
                    return ReportsCommon.LocationType.None;
                }
            }
            set
            {
                ViewState["LastLocationType"] = value;
            }
        }

        private PagingStatus SQLPaging
        {
            get
            {
                if (ViewState["SQLPaging"] != null)
                {
                    if (ViewState["SQLPaging"].Equals("OFF"))
                        return PagingStatus.OFF;
                    else
                        return PagingStatus.ON;
                }
                else
                {
                    return PagingStatus.ON;
                }
            }
            set
            {
                ViewState["SQLPaging"] = value.ToString();
            }
        }

        private int GridPageIndex
        {
            get
            {
                int index = 0;
                if (ViewState["GridPageIndex"] != null)
                {
                    int.TryParse(ViewState["GridPageIndex"].ToString(), out index);
                }

                return index;
            }
            set
            {
                ViewState["GridPageIndex"] = value.ToString();
            }
        }

        enum PagingStatus
        {
            ON = 1,
            OFF = 0,
        }

        #endregion
    }
}