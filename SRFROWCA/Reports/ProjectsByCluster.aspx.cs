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
    public partial class ProjectsByCluster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            PopulateDropDowns();
            LoadClusters();
        }

        private void PopulateDropDowns()
        {
            PopulateClusters();
            PopulateCountry();
            PopulateOrganizations();

        }

        // Populate Clusters drop down.
        private void PopulateClusters()
        {
            UI.FillClusters(ddlClusters, 1);
        }

        private void PopulateCountry()
        {
            UI.FillCountry(ddlCountry);
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

        private void LoadClusters()
        {
            string clusterIds = RC.GetSelectedValues(ddlClusters);
            gvClusters.DataSource = DBContext.GetData("[GetMultipleClusters]", new object[] { clusterIds, RC.SelectedSiteLanguageId });
            gvClusters.DataBind();
        }

        private Dictionary<string, string> GetSelectedClusters()
        {
            Dictionary<string, string> clusters = new Dictionary<string, string>();
            foreach (ListItem item in ddlClusters.Items)
            {
                if (item.Selected)
                {
                    clusters.Add(item.Value, item.Text);
                }
            }

            return clusters;
        }

        protected void gvClusters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Return if this is not a datarow
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            GridView gvActivities = e.Row.FindControl("gvActivities") as GridView;
            if (gvActivities != null)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                int clusterId = 0;
                int.TryParse(dr["ClusterId"].ToString(), out clusterId);

                // Get all activities and bind grid.
                DataTable dt = GetProjectData(clusterId);
                gvActivities.DataSource = dt;
                gvActivities.DataBind();

                // Attch row command event with grid.
                gvActivities.RowCommand += gvActivities_RowCommand;
            }
        }

        // Get data from db.
        private DataTable GetProjectData(int clusterId)
        {
            object[] paramValue = GetParamValues(clusterId);
            return DBContext.GetData("GetProjectsForReport", paramValue);
        }

        // Get filter criteria and create an object with parameter values.
        private object[] GetParamValues(int clusterId)
        {
            string locationIds = RC.GetSelectedValues(ddlCountry);
            string orgIds = RC.GetSelectedValues(ddlOrganizations);
            string fts = ddlFundingStatus.SelectedValue != "0" ? ddlFundingStatus.SelectedItem.Text : null;
            int? fromMonth = !string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? Convert.ToInt32(txtFromDate.Text.Trim().Substring(0, 2)) : (int?)null;
            int? toMonth = !string.IsNullOrEmpty(txtToDate.Text.Trim()) ? Convert.ToInt32(txtToDate.Text.Trim().Substring(0, 2)) : (int?)null;
            string projectTitle = !string.IsNullOrEmpty(txtProjectTitle.Text.Trim()) ? txtProjectTitle.Text.Trim() : null;
            string fundingStatus = null;
            string startDate = null;
            string endDate = null;
            int? regionInd = null;
            int? countryInd = null;
            int? isReported = null;
            int langId = RC.SelectedSiteLanguageId;

            string projectCode = !string.IsNullOrEmpty(txtProjectCode.Text.Trim()) ? txtProjectCode.Text.Trim() : null;

            //SetHFQueryString(monthIds, locationIds, clusterIds, orgIds);

            return new object[] { clusterId, locationIds, orgIds, projectCode, projectTitle, fundingStatus, startDate, endDate,
                                   regionInd, countryInd, isReported,  langId };
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        { }

        protected void btnSearch_Click(object sender, EventArgs e)
        { }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusters();
        }

        protected void ddlFundingStatus_SelectedIndexChanged(object sender, EventArgs e)
        { }

        protected void btnReset_Click(object sender, EventArgs e)
        { }



        protected void ExportToExcel(object sender, EventArgs e)
        {
            //SQLPaging = PagingStatus.OFF;
            //DataTable dt = GetReportData();
            //SQLPaging = PagingStatus.ON;
            //RemoveColumnsFromDataTable(dt);
            //GridView gv = new GridView();
            //gv.DataSource = dt;
            //gv.DataBind();

            //ExportUtility.ExportGridView(gv, "ORS_CustomReport", ".xls", Response, true);
        }

        protected void gvActivities_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //// Get row of the image clicked
            //GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

            //if (e.CommandName == "AddActivity")
            //{
            //    AddActivity(row);
            //    ShowMessage("Saved Successfully!", "info-message");
            //}

            //if (e.CommandName == "RemoveActivity")
            //{
            //    RemoveActivity(row);
            //    ShowMessage("Saved Successfully!", "info-message");
            //}
        }
    }
}