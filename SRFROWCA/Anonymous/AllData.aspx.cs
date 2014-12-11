using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Saplin.Controls;
using SRFROWCA.Common;
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

            if (!this.User.Identity.IsAuthenticated)
            {
                ltrlValidated.Visible = false;
                cbValidated.Visible = false;
                cbNotValidated.Visible = false;
            }

            LoadData();
        }

        internal override void BindGridData()
        {
            PopulateDropDowns();
            LoadData();
        }

        #region Events

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            PopulateDropDowns();
            txtFromDate.Text = "";
            txtToDate.Text = "";
            
            cbORSProjects.Checked = cbORSProjects.Checked = cbRegional.Checked = cbCountry.Checked = false;
            cbFunded.Checked = cbNotFunded.Checked = cbValidated.Checked = cbNotValidated.Checked = false;

            LoadData();
        }

        protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetReportData(true);

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

        protected void ddlOrganizations_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataOnMultipleCheckBoxControl();
            PopulateProjects();
        }

        protected void ddlProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataOnMultipleCheckBoxControl(); 
            PopulateOrganizations();
        }  

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LastLocationType = RC.LocationTypeForUI.Country;
            LoadDataOnMultipleCheckBoxControl();
            PopulateLocationDropDowns();
        }

        protected void ddlAdmin1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LastLocationType = RC.LocationTypeForUI.Admin1;
            LoadDataOnMultipleCheckBoxControl();
            PopulateAdmin2();
        }

        //protected void ddlAdmin2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LastLocationType = RC.LocationTypeForUI.Admin2;
        //    LoadDataOnMultipleCheckBoxControl();
        //}

        #endregion

        #endregion

        #region Class Methods

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
            ddlProjects.DataTextField = "ProjectCode";
            ddlProjects.DataValueField = "ProjectId";

            int? emgLocationId = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;
            int? emgClsuterId = UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;
            int? orgId = UserInfo.Organization > 0 ? UserInfo.Organization : (int?)null;

            string orgIDs = RC.GetSelectedValues(ddlOrganizations);

            if (!string.IsNullOrEmpty(orgIDs))
                orgId = null;

            ddlProjects.DataSource = DBContext.GetData("GetProjectsOnClusterCountryAndOrg", new object[] { emgLocationId, emgClsuterId, orgId, orgIDs });
            ddlProjects.DataBind();

            //if (!string.IsNullOrEmpty(orgIDs))
            //    SelectAll(ddlProjects);
        }

        private void PopulateActivities()
        {
            string clusterIds = RC.GetSelectedValues(ddlClusters);
            string objIds = RC.GetSelectedValues(ddlObjectives);
            string priorityIds = RC.GetSelectedValues(ddlPriority);

            if (clusterIds != null)
            {
                ddlActivities.DataTextField = "ActivityName";
                ddlActivities.DataValueField = "PriorityActivityId";
                ddlActivities.DataSource = DBContext.GetData("[GetActivitiesOfMultipleClusterObjAndPriorities]", new object[] { 1, clusterIds, objIds, priorityIds, RC.SelectedSiteLanguageId });
                ddlActivities.DataBind();
            }
        }

        private void PopulateIndicators()
        {
            string clusterIds = RC.GetSelectedValues(ddlClusters);
            string objIds = RC.GetSelectedValues(ddlObjectives);
            string priorityIds = RC.GetSelectedValues(ddlPriority);
            string activityIds = RC.GetSelectedValues(ddlActivities);

            if (clusterIds != null)
            {
                ddlIndicators.DataTextField = "DataName";
                ddlIndicators.DataValueField = "ActivityDataId";
                ddlIndicators.DataSource = DBContext.GetData("[GetIndicatorsOfMultipleClusterObjPriAndActivities]", new object[] { 1, clusterIds, objIds, priorityIds, activityIds, RC.SelectedSiteLanguageId });
                ddlIndicators.DataBind();
            }
        }

        private void PopulatePriorities()
        {
            UI.FillPriorities(ddlPriority);
        }

        private void PopulateObjectives()
        {
            UI.FillObjectives(ddlObjectives, true, RC.SelectedEmergencyId);
        }

        // Populate Clusters drop down.
        private void PopulateClusters()
        {
            UI.FillClusters(ddlClusters, RC.SelectedSiteLanguageId);
            if (UserInfo.Cluster > 0)
            {
                ddlClusters.SelectedValue = UserInfo.Cluster.ToString();
                ddlClusters.Visible = false;
                lblCluster.Text = ddlClusters.SelectedItem.Text;
                lblCluster.Visible = true;
            }
        }

        // Populate Locations drop down
        private void PopulateLocations()
        {
            PopulateCountry();
            PopulateLocationDropDowns();
        }

        private void PopulateCountry()
        {
            LastLocationType = RC.LocationTypeForUI.Country;
            UI.FillCountry(ddlCountry);
            if (UserInfo.Country > 0)
            {
                ddlCountry.SelectedValue = UserInfo.Country.ToString();
                ddlCountry.Visible = false;
                lblCountry.Text = ddlCountry.SelectedItem.Text;
                lblCountry.Visible = true;
            }
        }

        private void PopulateLocationDropDowns()
        {
            PopulateAdmin1();
            PopulateAdmin2();
        }

        private void PopulateAdmin1()
        {
            string countryIds = RC.GetSelectedValues(ddlCountry);
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
            string admin1Ids = RC.GetSelectedValues(ddlAdmin1);
            string countryIds = RC.GetSelectedValues(ddlCountry);
            if (countryIds != null)
            {

                //ddlAdmin2.DataValueField = "LocationId";
                //ddlAdmin2.DataTextField = "LocationName";
                //ddlAdmin2.DataSource = DBContext.GetData("GetAdmin2LocationsOfMultipleCountries", new object[] { countryIds, admin1Ids });
                //ddlAdmin2.DataBind();
            }
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
            return DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
        }

        // Populate Organizations drop down.
        private void PopulateOrganizations()
        {
            ddlOrganizations.DataValueField = "OrganizationId";
            ddlOrganizations.DataTextField = "OrganizationAcronym";
            int? orgId = null;
            string projIDs = null;

            if (UserInfo.Organization > 0)
                orgId = UserInfo.Organization;

            projIDs = RC.GetSelectedValues(ddlProjects);

            ddlOrganizations.DataSource = GetOrganizations(orgId, projIDs);
            ddlOrganizations.DataBind();

            if (UserInfo.Organization > 0)
            {
                ddlOrganizations.SelectedValue = UserInfo.Organization.ToString();
                ddlOrganizations.Visible = false;
                lblOrganization.Text = ddlOrganizations.SelectedItem.Text;
                lblOrganization.Visible = true;
            }
            else if (!string.IsNullOrEmpty(projIDs))
                SelectAll(ddlOrganizations);
        }

        private DataTable GetOrganizations(int? orgId, string projIDs)
        {
            return DBContext.GetData("GetOrganizations", new object[] { orgId, projIDs });
        }

        private void ReplaceHTMLTags(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Comments"] = StripTagsRegex(Convert.ToString(dt.Rows[i]["Comments"]).Replace("<br>", "\r\n"));

            }
        }

        private string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            try
            {
                dt.Columns.Remove("rnumber");
                dt.Columns.Remove("ObjectiveId");
                dt.Columns.Remove("PriorityId");
                dt.Columns.Remove("MonthId");
                dt.Columns.Remove("cnt");
            }
            catch { }
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
            divSearchCriteria.InnerHtml = GetSearchCriteria();

            DataTable dt = GetReportData(true);

            // Cnt column has total number of records in resultset.
            // Gridview is using custom paging so to tell gridview that how many pages
            // it has to generate we have to set VirtualItemCount property with number of records            
            if (dt.Rows.Count > 0)
            {
                gvReport.VirtualItemCount = Convert.ToInt32(dt.Rows[0]["Cnt"]);

                // rnumber and Cnt colums are not needed in gridview
                // so remove these two columns from datatable.
                dt.Columns.Remove("rnumber");
                dt.Columns.Remove("Cnt");
            }

            gvReport.DataSource = dt;
            gvReport.DataBind();
        }

        // Get data from db.
        private DataTable GetReportData(bool filter)
        {
            object[] paramValue = GetParamValues(filter);
            return DBContext.GetData("GetAllTasksDataReport", paramValue);
        }

        // Get filter criteria and create an object with parameter values.
        private object[] GetParamValues(bool filter)
        {
            string monthIds = RC.GetSelectedValues(ddlMonth);
            string locationIds = GetLocationIds();
            if (string.IsNullOrEmpty(locationIds))
            {
                if (!string.IsNullOrEmpty(lblCountry.Text.Trim()))
                {
                    locationIds = UserInfo.Country.ToString();
                }
            }
            string clusterIds = RC.GetSelectedValues(ddlClusters);
            string orgIds = RC.GetSelectedValues(ddlOrganizations);
            string objIds = RC.GetSelectedValues(ddlObjectives);
            string prIds = RC.GetSelectedValues(ddlPriority);
            string actIds = RC.GetSelectedValues(ddlActivities);
            string indIds = RC.GetSelectedValues(ddlIndicators);
            string projectIds = RC.GetSelectedValues(ddlProjects);
            int? fromMonth = !string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? Convert.ToInt32(txtFromDate.Text.Trim().Substring(0, 2)) : (int?)null;
            int? toMonth = !string.IsNullOrEmpty(txtToDate.Text.Trim()) ? Convert.ToInt32(txtToDate.Text.Trim().Substring(0, 2)) : (int?)null;
            int? regionalInd = cbRegional.Checked ? 1 : (int?)null;
            int? countryInd = cbCountry.Checked ? 1 : (int?)null;
            int? funded = cbFunded.Checked ? 1 : (int?)null;
            int? notFunded = cbNotFunded.Checked ? 1 : (int?)null;
            int? isOPS = cbOPSProjects.Checked && cbORSProjects.Checked ? null : cbOPSProjects.Checked ? 1 : cbORSProjects.Checked ? 0 : (int?)null;
            int? isApproved = 0;
            if (this.User.Identity.IsAuthenticated)
            {
                isApproved = cbValidated.Checked && cbNotValidated.Checked ? null : cbValidated.Checked ? 0 : cbNotValidated.Checked ? 1 : (int?)null;
            }

            int? pageSize = null;
            int? pageIndex = null;

            if (filter)
            {
                pageSize = gvReport.PageSize;
                pageIndex = GridPageIndex; 
            }

            int langId = RC.SelectedSiteLanguageId;
            int emergencyId = RC.SelectedEmergencyId;

            return new object[] {monthIds, locationIds, clusterIds, orgIds, 
                                    objIds, prIds, actIds, indIds, projectIds,
                                    fromMonth, toMonth, regionalInd, countryInd, funded, notFunded,
                                    isOPS, isApproved, pageIndex, pageSize, Convert.ToInt32(SQLPaging), langId, emergencyId };
        }

        private string GetSearchCriteria()
        {
            string months = RC.GetItemsText(ddlMonth);
            string country = RC.GetItemsText(ddlCountry);
            string admin1 = RC.GetItemsText(ddlAdmin1);
            string clusters = RC.GetItemsText(ddlClusters);
            string orgs = RC.GetItemsText(ddlOrganizations);
            string objs = RC.GetItemsText(ddlObjectives);
            string prs = RC.GetItemsText(ddlPriority);
            string acts = RC.GetItemsText(ddlActivities);
            string inds = RC.GetItemsText(ddlIndicators);
            string projects = RC.GetItemsText(ddlProjects);

            string criteria = "";
            criteria += MakeSearchCriteriaString("Clusters: ", clusters);
            criteria += MakeSearchCriteriaString("Organizations: ", orgs);
            criteria += MakeSearchCriteriaString("Country: ", country);
            criteria += MakeSearchCriteriaString("Admin1: ", admin1);
            criteria += MakeSearchCriteriaString("Projects: ", projects);
            criteria += MakeSearchCriteriaString("Months: ", months);
            criteria += MakeSearchCriteriaString("Objectives: ", objs);
            criteria += MakeSearchCriteriaString("Priorities: ", prs);
            criteria += MakeSearchCriteriaString("Activities: ", acts);
            criteria += MakeSearchCriteriaString("Indicators: ", inds);

            return criteria;
        }

        private string MakeSearchCriteriaString(string caption, string items)
        {
            string criteria = "";
            if (!string.IsNullOrEmpty(items))
            {
                criteria += "<b>" + caption + "</b>" + items + "&nbsp;&nbsp;";
            }

            return criteria;
        }

        private string GetLocationIds()
        {
            string locationIds = null;
            if ((int)LastLocationType == 1)
            {
                locationIds = RC.GetSelectedValues(ddlCountry);
            }
            else if ((int)LastLocationType == 2)
            {
                locationIds = RC.GetSelectedValues(ddlAdmin1);
            }
            //else if ((int)LastLocationType == 3)
            //{
            //    locationIds = RC.GetSelectedValues(ddlAdmin2);
            //}

            if (locationIds == "0" || locationIds == null)
            {
                return null;
            }

            return locationIds;
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
            DataTable dt = GetReportData(false);

            RemoveColumnsFromDataTable(dt);
            ReplaceHTMLTags(dt);

            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();

            ExportUtility.ExportGridView(gv, "ORS_CustomReport", ".xls", Response, true);
        }

        //private void testexport()
        //{

        //    Response.Clear();

        //    Response.AddHeader("content-disposition", "attachment; filename=FileName1.xls");

        //    Response.Charset = "";

        //    Response.ContentType = "application/vnd.xls";

        //    System.IO.StringWriter stringWrite = new System.IO.StringWriter();

        //    System.Web.UI.HtmlTextWriter htmlWrite =
        //    new HtmlTextWriter(stringWrite);
        //    GridView g = new GridView();
        //    this.Form.Controls.Add(g);

        //    g.HeaderStyle.BackColor = System.Drawing.Color.Red;
        //    SQLPaging = PagingStatus.OFF;
        //    DataTable dt = GetReportData();
        //    SQLPaging = PagingStatus.ON;
        //    g.DataSource = dt;
        //    g.DataBind();
        //    foreach (GridViewRow i in g.Rows)
        //    {

        //        foreach (TableCell tc in i.Cells)

        //            tc.Attributes.Add("class", "text");


        //    }

        //    g.RenderControl(htmlWrite);
        //    string style = @"<style> .text { mso-number-format:\@; } </style> ";

        //    Response.Write(style);

        //    Response.Write(stringWrite.ToString());

        //    Response.End();


        //}

        public override void VerifyRenderingInServerForm(Control control) { }

        #endregion

        #endregion

        #region Properties & Enum

        private RC.LocationTypeForUI LastLocationType
        {
            get
            {
                if (ViewState["LastLocationType"] != null)
                {
                    return (RC.LocationTypeForUI)ViewState["LastLocationType"];
                }
                return RC.LocationTypeForUI.None;
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

                    return PagingStatus.ON;
                }

                return PagingStatus.ON;
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

        private void SelectAll(DropDownCheckBoxes ddlControl)
        {
            foreach (ListItem item in (ddlControl as ListControl).Items)
            {
                if (!item.Selected)
                    item.Selected = true;
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