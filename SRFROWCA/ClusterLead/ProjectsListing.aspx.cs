﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using System.Data;
using BusinessLogic;

namespace SRFROWCA.ClusterLead
{
    public partial class ProjectsListing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            UserInfo.UserProfileInfo();
            PopulateControls();
            LoadProjects();

        }

        private void PopulateControls()
        {
            PopulateOrganizations();
            PopulateLocations();
            PopulateProjectStatus();
        }

        private void PopulateOrganizations()
        {
            DataTable dt = RC.GetProjectsOrganizations(UserInfo.GetCountry, UserInfo.GetCluster);
            UI.FillOrganizations(ddlOrg, dt);

            if (ddlOrg.Items.Count > 0)
            {
                ListItem item = new ListItem("All", "0");
                ddlOrg.Items.Insert(0, item);
            }
        }

        private void PopulateLocations()
        {
            UI.FillAdmin1(ddlAdmin1, UserInfo.GetCountry);

            ListItem item = new ListItem("All", "0");
            if (ddlAdmin1.Items.Count > 0)
            {
                ddlAdmin1.Items.Insert(0, item);
            }

            UI.FillAdmin2(ddlAdmin2, UserInfo.GetCountry);
            if (ddlAdmin2.Items.Count > 0)
            {
                ddlAdmin2.Items.Insert(0, item);
            }
        }

        private void PopulateProjectStatus()
        {

        }

        protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        protected void ddlAdmin1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        protected void ddlAdmin2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        protected void ddlProjStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void cblReportingStatus_SelectedIndexChanged(object sender, EventArgs e)
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
            string projCode = txtProjectCode.Text.Trim().Length > 0 ? txtProjectCode.Text.Trim() : null;
            int? orgId = RC.GetSelectedIntVal(ddlOrg);
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

            return DBContext.GetData("GetProjects", new object[] {UserInfo.GetCountry, UserInfo.GetCluster,
                                                                            projCode, orgId, admin1, admin2, 
                                                                            DBNull.Value, DBNull.Value, cbReported, 1 });
        }
    }
}