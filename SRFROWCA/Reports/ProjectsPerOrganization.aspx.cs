using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using System.Data;
using BusinessLogic;

namespace SRFROWCA.Reports
{
    public partial class ProjectsPerOrganization : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            PopulateControls();
            LoadProjects();
        }

        private void PopulateControls()
        {
            PopulateOrganizations();
            PopulateLocations();
            PopulateProjectStatus();
            PopulateProjectCodes();

        }

        private void PopulateProjectCodes()
        {
            using (ORSEntities db = new ORSEntities())
            {
                ddlProjects.DataValueField = "ProjectId";
                ddlProjects.DataTextField = "ProjectCode";

                int? emgLocId = UserInfo.EmergencyCountry == 0 ? (int?)null : UserInfo.EmergencyCountry;
                int countryId = RC.GetSelectedIntVal(ddlCountry);
                emgLocId = countryId == 0 ? (int?)null : countryId;
                int? emgClusterId = UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;

                DataTable dt = DBContext.GetData("GetProjectsOnCountryAndCluster", new object[] { emgLocId, emgClusterId });
                ddlProjects.DataSource = dt;
                ddlProjects.DataBind();

                ListItem item = new ListItem("All", "0");
                ddlProjects.Items.Insert(0, item);
            }
        }

        private void PopulateOrganizations()
        {
            int? emgLocId = UserInfo.EmergencyCountry == 0 ? (int?)null : UserInfo.EmergencyCountry;
            int? emgClusterId = UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;
            int countryId = RC.GetSelectedIntVal(ddlCountry);
            emgLocId = countryId == 0 ? (int?)null : countryId;

            DataTable dt = RC.GetProjectsOrganizations(emgLocId, emgClusterId);
            UI.FillOrganizations(ddlOrganizations, dt);

            if (ddlOrganizations.Items.Count > 0)
            {
                ListItem item = new ListItem("All", "0");
                ddlOrganizations.Items.Insert(0, item);
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
            UI.FillCountry(ddlCountry);

            ListItem item = new ListItem("All", "0");
            ddlCountry.Items.Insert(0, item);

            if (UserInfo.EmergencyCountry != 0)
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlCountry.Enabled = false;
            }

        }

        private void PopulateLocationDropDowns()
        {
            PopulateAdmin1();
            PopulateAdmin2();
        }

        private void PopulateAdmin1()
        {
            int countryId = RC.GetSelectedIntVal(ddlCountry);

            ddlAdmin1.DataValueField = "LocationId";
            ddlAdmin1.DataTextField = "LocationName";

            ddlAdmin1.DataSource = DBContext.GetData("GetAdmin1LocationsOfCountry", new object[] { countryId });
            ddlAdmin1.DataBind();

            ListItem item = new ListItem("All", "0");
            ddlAdmin1.Items.Insert(0, item);
        }

        // Populate Locations drop down
        private void PopulateAdmin2()
        {
            int countryId = RC.GetSelectedIntVal(ddlCountry);

            ddlAdmin2.DataValueField = "LocationId";
            ddlAdmin2.DataTextField = "LocationName";
            ddlAdmin2.DataSource = DBContext.GetData("GetAdmin1LocationsOfCountry", new object[] { countryId });
            ddlAdmin2.DataBind();

            ListItem item = new ListItem("All", "0");
            ddlAdmin2.Items.Insert(0, item);
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateLocationDropDowns();
            LoadProjects();
        }

        protected void ddlAdmin1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LastLocationType = ReportsCommon.LocationType.Admin1;
            LoadProjects();
        }

        protected void ddlAdmin2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        //private void PopulateLocations()
        //{
        //    //UI.FillAdmin1(ddlAdmin1, UserInfo.Country);

        //    //ListItem item = new ListItem("All", "0");
        //    //if (ddlAdmin1.Items.Count > 0)
        //    //{
        //    //    ddlAdmin1.Items.Insert(0, item);
        //    //}

        //    //UI.FillAdmin2(ddlAdmin2, UserInfo.Country);
        //    //if (ddlAdmin2.Items.Count > 0)
        //    //{
        //    //    ddlAdmin2.Items.Insert(0, item);
        //    //}
        //}

        private void PopulateProjectStatus()
        {

        }

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }
         

        protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        protected void cblReportingStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        protected void cblFundingStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        protected void txtProjectCode_TextChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        protected void gvProjects_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewProject")
            {
                Session["ViewProjectId"] = e.CommandArgument.ToString();
                Response.Redirect("~/ClusterLead/ProjectDetails.aspx");
            }
        }

        protected void gvProjects_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetProjects();
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvProjects.DataSource = dt;
                gvProjects.DataBind();
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

        protected void gvProjects_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProjects.PageIndex = e.NewPageIndex;
            gvProjects.SelectedIndex = -1;
            LoadProjects();
        }

        private void LoadProjects()
        {
            gvProjects.DataSource = GetProjects();
            gvProjects.DataBind();
        }

        private DataTable GetProjects()
        {
            string projCode = null;
            if (ddlProjects.SelectedIndex > 0)
            {
                projCode = ddlProjects.SelectedItem.Text;
            }

            //txtProjectCode.Text.Trim().Length > 0 ? txtProjectCode.Text.Trim() : null;
            int? orgId = RC.GetSelectedIntVal(ddlOrganizations);
            if (orgId == 0)
            {
                orgId = (int?)null;
            }

            int? admin1 = RC.GetSelectedIntVal(ddlAdmin1);
            if (admin1 == 0)
            {
                admin1 = (int?)null;
            }

            int? admin2 = RC.GetSelectedIntVal(ddlAdmin2);
            if (admin2 == 0)
            {
                admin2 = (int?)null;
            }

            int? cbReported = cblReportingStatus.SelectedIndex > -1 ? RC.GetSelectedIntVal(cblReportingStatus) : (int?)null;
            int? cbFunded = cblFundingStatus.SelectedIndex > -1 ? RC.GetSelectedIntVal(cblFundingStatus) : (int?)null;

            int? emgLocId = UserInfo.EmergencyCountry == 0 ? (int?)null : UserInfo.EmergencyCountry;
            int? clusterId = UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;
            int countryId = RC.GetSelectedIntVal(ddlCountry);
            emgLocId = countryId == 0 ? (int?)null : countryId;

            return DBContext.GetData("GetProjects", new object[] {emgLocId, clusterId, projCode, orgId, admin1, admin2, 
                                                                            DBNull.Value, cbFunded, cbReported, 1 });
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {

            //DataTable dt = GetReportData();

            //RemoveColumnsFromDataTable(dt);
            //GridView gv = new GridView();
            //gv.DataSource = dt;
            //gv.DataBind();

            ExportUtility.ExportGridView(gvProjects, "ORS_OganizationProjects", ".xls", Response, false);
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

            //dt.Columns.Remove("rnumber");
            //dt.Columns.Remove("ObjectiveId");
            //dt.Columns.Remove("PriorityId");
            //dt.Columns.Remove("ActivityId");
            //dt.Columns.Remove("IndicatorId");
            //dt.Columns.Remove("MonthId");
            //dt.Columns.Remove("cnt");

        }
    }
}