using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Admin
{
    public partial class ActivityListing : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            this.Form.DefaultButton = this.btnAdd.UniqueID;
            LoadActivities();
            PopulateFilters();
        }

       

        // Add delete confirmation message with all delete buttons.
        protected void gvActivity_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button deleteButton = e.Row.FindControl("btnDelete") as Button;
                if (deleteButton != null)
                {
                    deleteButton.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this Activity?')");
                }
            }
        }

        // Execute row commands like Edit, Delete etc. on Grid.
        protected void gvActivity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // If user click on Delete button.
            if (e.CommandName == "Delete")
            {
                int priorityActivityId = Convert.ToInt32(e.CommandArgument);

                // Check if any IP has reported on this project. If so then do not delete it.
                if (!ActivityIsBeingUsed(priorityActivityId))
                {
                    ShowMessage("Activity cannot be deleted! It is being used.", RC.NotificationType.Error, true, 500);
                }
                else
                {
                    DeleteActivity(priorityActivityId);
                    LoadActivities();
                }
            }

            // Edit Project.
            if (e.CommandName == "EditActivity")
            {
                ClearPopupControls();

                hdnPriorityActivityId.Value = e.CommandArgument.ToString();
                
                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;
                ddlEmergencyNew.SelectedValue = gvActivity.DataKeys[row.RowIndex].Values["EmergencyId"].ToString();
                LoadClustersByEmergency();
                ddlClusterNew.SelectedValue = gvActivity.DataKeys[row.RowIndex].Values["ClusterId"].ToString();
                LoadObjectivesByCluster();
                ddlObjectiveNew.SelectedValue = gvActivity.DataKeys[row.RowIndex].Values["ClusterObjectiveId"].ToString();
                LoadPrioritiesByObjective();
                ddlPriorityNew.SelectedValue = gvActivity.DataKeys[row.RowIndex].Values["ObjectivePriorityId"].ToString();
                //ddlActivityType.SelectedValue = gvActivity.DataKeys[row.RowIndex].Values["ActivityTypeId"].ToString();

                if (gvActivity.DataKeys[row.RowIndex].Values["SiteLanguageId"].ToString() == "1")
                {
                    txtActivityEng.Text = gvActivity.DataKeys[row.RowIndex].Values["ActivityName"].ToString();
                    txtActivityFr.Visible = false;
                    rfvEmgNameFr.Enabled = false;
                    trFrench.Visible = false;
                }
                else
                {
                    txtActivityFr.Text = gvActivity.DataKeys[row.RowIndex].Values["ActivityName"].ToString();
                    txtActivityEng.Visible = false;
                    rfvEmgName.Enabled = false;
                    trEnglish.Visible = false;
                }
                mpeAddOrg.Show();
            }
        }
        protected void btnSearch2_Click(object sender, EventArgs e)
        {
            LoadActivities();
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            GridView gvExport = new GridView();
            DataTable dt = DBContext.GetData("GetAllActivities", new object[] { null,null, null, null, null, (int)RC.SelectedSiteLanguageId });//GetActivities();
            RemoveColumnsFromDataTable(dt);
            gvExport.DataSource = dt;
            gvExport.DataBind();

            string fileName = "Activities";
            string fileExtention = ".xls";
            ExportUtility.ExportGridView(gvExport, fileName, fileExtention, Response);
        }

        internal override void BindGridData()
        {
            LoadActivities();
            PopulateFilters();
        }

        private bool ActivityIsBeingUsed(int priorityActivityId)
        {
            DataTable dt = DBContext.GetData("GetIsActvityBeingUsed", new object[] { priorityActivityId });
            return !(dt.Rows.Count > 0);
        }

        private void DeleteActivity(int priorityActivityId)
        {
            DBContext.Delete("DeleteActivity", new object[] { priorityActivityId, DBNull.Value });
        }

        protected void gvActivity_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetActivities();
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvActivity.DataSource = dt;
                gvActivity.DataBind();
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

        private void LoadActivities()
        {
            gvActivity.DataSource = GetActivities();
            gvActivity.DataBind();
        }

        private void PopulateFilters()
        {
            LoadEmergencyFilter();
            LoadClustersFilter();
            LoadObjectivesFilter();
            LoadPriorityFilter();
            LoadEmergencyFilterNew();          
          
        }

        private void LoadClustersByEmergency()
        {
            ddlClusterNew.Items.Clear();
            ddlClusterNew.Items.Add(new ListItem("Select", "-1"));
            ddlClusterNew.DataValueField = "ClusterId";
            ddlClusterNew.DataTextField = "ClusterName";
            ddlClusterNew.DataSource = GetClustersByEmergency();
            ddlClusterNew.DataBind();
        }

        private void LoadEmergencyFilter(){
            ddlEmergency.Items.Clear();
            ddlEmergency.Items.Add(new ListItem("All", "-1"));
            ddlEmergency.DataValueField = "EmergencyId";
            ddlEmergency.DataTextField = "EmergencyName";
            ddlEmergency.DataSource = RC.GetAllEmergencies((int)RC.SelectedSiteLanguageId);
            ddlEmergency.DataBind();
        }

        private void LoadEmergencyFilterNew()
        {
            ddlEmergencyNew.Items.Clear();
            ddlEmergencyNew.Items.Add(new ListItem("Select", "-1"));
            ddlEmergencyNew.DataValueField = "EmergencyId";
            ddlEmergencyNew.DataTextField = "EmergencyName";
            ddlEmergencyNew.DataSource = RC.GetAllEmergencies((int)RC.SelectedSiteLanguageId);
            ddlEmergencyNew.DataBind();
        }

        private void LoadClustersFilter()
        {
            ddlCluster.Items.Clear();
            ddlCluster.Items.Add(new ListItem("All", "-1"));
            ddlCluster.DataValueField = "ClusterId";
            ddlCluster.DataTextField = "ClusterName";
            ddlCluster.DataSource = GetClusters();
            ddlCluster.DataBind();
        }

        private void LoadObjectivesFilter()
        {
            ddlObjective.Items.Clear();
            ddlObjective.Items.Add(new ListItem("All", "-1"));
            ddlObjective.DataValueField = "ObjectiveId";
            ddlObjective.DataTextField = "Objective";
            ddlObjective.DataSource = GetObjectivesByEmergencyAndCluster(ddlCluster.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlCluster.SelectedValue));
            ddlObjective.DataBind();
        }

        private void LoadPriorityFilter()
        {
            ddlPriority.Items.Clear();
            ddlPriority.Items.Add(new ListItem("All", "-1"));
            ddlPriority.DataValueField = "HumanitarianPriorityId";
            ddlPriority.DataTextField = "HumanitarianPriority";
            ddlPriority.DataSource = GetPrioritiesByEmergencyClusertAndObjective();
            ddlPriority.DataBind();
        }

     
        private DataTable GetClusters()
        {
            int? emgId = ddlEmergency.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlEmergency.SelectedValue);
            return DBContext.GetData("GetAllClusters", new object[]{(int)RC.SelectedSiteLanguageId, emgId});
        }

        private DataTable GetClustersByEmergency()
        {
            int? emgId = ddlEmergencyNew.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlEmergencyNew.SelectedValue);
            return DBContext.GetData("GetAllClusters", new object[] { (int)RC.SelectedSiteLanguageId, emgId });
        }

        private DataTable GetActivities()
        {
            int? clusterId = ddlCluster.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlCluster.SelectedValue);
            int? objectiveId = ddlObjective.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlObjective.SelectedValue);
            int? priorityId = ddlPriority.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlPriority.SelectedValue);
            int? emergencyId = ddlEmergency.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlEmergency.SelectedValue);
            string search = string.IsNullOrEmpty(txtActivityName.Text) ? null : txtActivityName.Text;

            return DBContext.GetData("GetAllActivities", new object[] {emergencyId,clusterId, objectiveId, priorityId, search, (int)RC.SelectedSiteLanguageId });
        }
        private DataTable GetObjectivesByEmergencyAndCluster(int? clusterId = null)
        {
            return DBContext.GetData("GetObjectivesByEmergencyAndCluster", new object[] { (int)RC.SelectedSiteLanguageId, (ddlEmergency.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlEmergency.SelectedValue)), clusterId });
        }

        private DataTable GetPrioritiesByEmergencyClusertAndObjective()
        {
            return DBContext.GetData("GetPrioritiesByEmergencyClusertAndObjective", new object[] { (int)RC.SelectedSiteLanguageId, (ddlEmergency.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlEmergency.SelectedValue)), (ddlCluster.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlCluster.SelectedValue)),
            (ddlObjective.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlObjective.SelectedValue))});
        }

        private DataTable GetObjectives(int? clusterId = null)
        {           
                return DBContext.GetData("GetObjectivesByClusterId", new object[] { (int)RC.SelectedSiteLanguageId, 
                    (ddlEmergencyNew.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlEmergencyNew.SelectedValue)), clusterId });           
        }

        private DataTable GetPriorities(int? objectiveId = null)
        {           
                return DBContext.GetData("GetPrioritiesByObjective", new object[] { (int)RC.SelectedSiteLanguageId, 
                     (ddlEmergencyNew.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlEmergencyNew.SelectedValue)),
                     (ddlClusterNew.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlClusterNew.SelectedValue)),
                     objectiveId });           
        }

        private DataTable GetActivityTypes()
        {

            return DBContext.GetData("GetActivityTypes");

        }
        
        protected void btnAddActivity_Click(object sender, EventArgs e)
        {
            ClearPopupControls();
            mpeAddOrg.Show();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SaveActivity();
            LoadActivities();
            mpeAddOrg.Hide();
            ClearPopupControls();
        }

        private void ClearPopupControls()
        {
            ddlEmergencyNew.SelectedIndex = 0;
            ddlClusterNew.SelectedIndex = 0;
            ddlObjectiveNew.SelectedIndex = 0;
            ddlPriorityNew.SelectedIndex = 0;
            //ddlActivityType.SelectedIndex = 0;
            txtActivityEng.Text = txtActivityFr.Text = string.Empty;
            txtActivityFr.Visible = txtActivityEng.Visible = true;
            rfvEmgName.Enabled = rfvEmgNameFr.Enabled = true;
            trEnglish.Visible = trFrench.Visible = true;
            //hfLocEmgId.Value = txtEmgNameEng.Text = txtEmgNameFr.Text = "";
        }

        private void SaveActivity()
        {
            int clusterId = Convert.ToInt32(ddlClusterNew.SelectedValue);
            int objectiveId = Convert.ToInt32(ddlObjectiveNew.SelectedValue);
            int priorityId = Convert.ToInt32(ddlPriorityNew.SelectedValue);
            int activityTypeId = 1;// Convert.ToInt32(ddlActivityType.SelectedValue);            

            Guid userId = RC.GetCurrentUserId;

            if (!string.IsNullOrEmpty(hdnPriorityActivityId.Value))
            {
                int priorityActivityId = Convert.ToInt32(hdnPriorityActivityId.Value);
                DBContext.Update("UpdatePriorityActivity", new object[] { priorityActivityId, clusterId, objectiveId, priorityId, activityTypeId, txtActivityEng.Text, txtActivityFr.Text, userId, txtActivityEng.Visible ? 1:2, DBNull.Value });
            }
            else
            {
                DBContext.Add("InsertActivity", new object[] { clusterId, objectiveId, priorityId, activityTypeId, txtActivityEng.Text, txtActivityFr.Text, userId, DBNull.Value });
            }
        }

        
        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 0)
        {
            updMessage.Update();
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }
        private void LoadObjectivesByCluster()
        {
            ddlObjectiveNew.Items.Clear();
            ddlObjectiveNew.Items.Add(new ListItem("Select", "-1"));
            ddlObjectiveNew.DataValueField = "ClusterObjectiveId";
            ddlObjectiveNew.DataTextField = "Objective";
            ddlObjectiveNew.DataSource = GetObjectives(Convert.ToInt32(ddlClusterNew.SelectedValue));
            ddlObjectiveNew.DataBind();
        }
        private void LoadPrioritiesByObjective()
        {
            ddlPriorityNew.Items.Clear();
            ddlPriorityNew.Items.Add(new ListItem("Select", "-1"));
            ddlPriorityNew.DataValueField = "ObjectivePriorityId";
            ddlPriorityNew.DataTextField = "HumanitarianPriority";
            ddlPriorityNew.DataSource = GetPriorities(Convert.ToInt32(ddlObjectiveNew.SelectedValue));
            ddlPriorityNew.DataBind();
        }

        enum LocationTypes
        {
            Country = 2,
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "ActivityListing", this.User);
        }

        protected void gvActivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvActivity.PageIndex = e.NewPageIndex;
            LoadActivities();
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {           
            LoadObjectivesFilter();
            LoadPriorityFilter();
        }

        protected void ddlObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPriorityFilter();
        }

        protected void gvActivity_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void ddlEmergencyNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClustersByEmergency();
        }

        protected void ddlClusterNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadObjectivesByCluster();
        }

        protected void ddlEmergency_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClustersFilter();
            LoadObjectivesFilter();
            LoadPriorityFilter();
        }

        protected void ddlObjectiveNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPrioritiesByObjective();
        }

        protected void gvActivity_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            try
            {
                dt.Columns.Remove("ClusterId");
                dt.Columns.Remove("ClusterObjectiveId");
                dt.Columns.Remove("ObjectivePriorityId");
                dt.Columns.Remove("PriorityActivityId");
                dt.Columns.Remove("ActivityType");
                dt.Columns.Remove("SiteLanguageId");
                dt.Columns.Remove("ActivityTypeId");
            }
            catch { }
        }
    }
}