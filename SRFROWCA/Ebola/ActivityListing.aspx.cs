using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Ebola
{
    public partial class ActivityListing : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            this.Form.DefaultButton = this.btnAdd.UniqueID;
            LoadActivities();
            PopulateFilters();
            LoadObjectivesByCluster();
        }

        #region Events.

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
            if (e.CommandName == "DeleteActivity")
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
                    ShowMessage("Activity Deleted Successfully!");
                }
            }

            // Edit Project.
            if (e.CommandName == "EditActivity")
            {
                ClearPopupControls();

                hdnPriorityActivityId.Value = e.CommandArgument.ToString();

                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;
                LoadObjectivesByCluster();
                ddlObjectiveNew.SelectedValue = gvActivity.DataKeys[row.RowIndex].Values["ClusterObjectiveId"].ToString();

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

        protected void btnSearch2_Click(object sender, EventArgs e)
        {
            LoadActivities();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            GridView gvExport = new GridView();
            DataTable dt = DBContext.GetData("GetAllActivities", new object[] { null, null, null, null, (int)RC.SelectedSiteLanguageId });//GetActivities();
            RemoveColumnsFromDataTable(dt);
            gvExport.DataSource = dt;
            gvExport.DataBind();

            string fileName = "Activities";
            string fileExtention = ".xls";
            ExportUtility.ExportGridView(gvExport, fileName, fileExtention, Response);
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

        protected void ddlObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadActivities();
        }


        #endregion

        private void LoadActivities()
        {
            gvActivity.DataSource = GetActivities();
            gvActivity.DataBind();
        }

        internal override void BindGridData()
        {
            LoadActivities();
            PopulateFilters();
        }

        private void PopulateFilters()
        {
            LoadObjectivesFilter();
        }

        private DataTable GetActivities()
        {
            int? emergencyId = 2;
            int? clusterId = 3;
            int? objectiveId = ddlObjective.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlObjective.SelectedValue);
            int? priorityId = GetPrioirtyIdOnObjective(objectiveId);
            string search = string.IsNullOrEmpty(txtActivityName.Text) ? null : txtActivityName.Text;
            return DBContext.GetData("GetAllActivities", new object[] { emergencyId, clusterId, objectiveId, priorityId, search, (int)RC.SelectedSiteLanguageId });
        }

        private bool ActivityIsBeingUsed(int priorityActivityId)
        {
            DataTable dt = DBContext.GetData("GetIsActvityBeingUsed_Ebola", new object[] { priorityActivityId });
            return !(dt.Rows.Count > 0);
        }

        private void DeleteActivity(int priorityActivityId)
        {
            DBContext.Delete("DeleteActivity_Ebola", new object[] { priorityActivityId, DBNull.Value });
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

        private void LoadClustersByEmergency()
        {
            GetClustersByEmergency();
        }

        private void LoadEmergencyFilter()
        {
            RC.GetAllEmergencies((int)RC.SelectedSiteLanguageId);
        }

        private void LoadObjectivesFilter()
        {
            ddlObjective.Items.Clear();
            ddlObjective.Items.Add(new ListItem("All", "-1"));
            ddlObjective.DataValueField = "ObjectiveId";
            ddlObjective.DataTextField = "Objective";
            ddlObjective.DataSource = GetObjectivesByEmergencyAndCluster();
            ddlObjective.DataBind();
        }

        private DataTable GetClusters()
        {
            int? emgId = -1;
            return DBContext.GetData("GetAllClusters", new object[] { (int)RC.SelectedSiteLanguageId, emgId });
        }

        private DataTable GetClustersByEmergency()
        {
            int? emgId = -1;
            return DBContext.GetData("GetAllClusters", new object[] { (int)RC.SelectedSiteLanguageId, emgId });
        }

        private DataTable GetObjectivesByEmergencyAndCluster()
        {
            int emergencyId = 2;
            int clusterId = 3;
            return DBContext.GetData("GetObjectivesByEmergencyAndCluster", new object[] { (int)RC.SelectedSiteLanguageId, emergencyId, clusterId });
        }

        private DataTable GetObjectives()
        {
            int emergencyId = 2;
            int? clusterId = 3;
            return DBContext.GetData("GetObjectivesByClusterId", new object[] { (int)RC.SelectedSiteLanguageId, emergencyId, clusterId });
        }

        private DataTable GetPriorities(int? objectiveId)
        {
            int emergencyId = 2;
            int clusterId = 3;            
            return DBContext.GetData("GetPrioritiesByObjective", new object[] { (int)RC.SelectedSiteLanguageId, emergencyId, clusterId, objectiveId });
        }

        private int? GetPrioirtyIdOnObjective(int? objectiveId)
        {
            DataTable dt = GetPriorities(objectiveId);
            int priorityId = 0;
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["ObjectivePriorityId"].ToString(), out priorityId);
            }

            return priorityId > 0 ? priorityId : (int?)null;
        }

        private DataTable GetActivityTypes()
        {

            return DBContext.GetData("GetActivityTypes");

        }

        private void ClearPopupControls()
        {
            hdnPriorityActivityId.Value = "";
            ddlObjectiveNew.SelectedIndex = 0;
            txtActivityEng.Text = txtActivityFr.Text = string.Empty;
            txtActivityFr.Visible = txtActivityEng.Visible = true;
            rfvEmgName.Enabled = rfvEmgNameFr.Enabled = true;
            trEnglish.Visible = trFrench.Visible = true;
        }

        private void SaveActivity()
        {
            int val = RC.GetSelectedIntVal(ddlObjectiveNew);
            int? objectiveId = val > 0 ? val : (int?)null;
            if (objectiveId > 0)
            {
                int? objPriorityId = GetPrioirtyIdOnObjective(objectiveId);
                int activityTypeId = 1;

                Guid userId = RC.GetCurrentUserId;

                if (!string.IsNullOrEmpty(hdnPriorityActivityId.Value))
                {
                    int priorityActivityId = Convert.ToInt32(hdnPriorityActivityId.Value);
                    DBContext.Update("UpdatePriorityActivity_Ebola", new object[] { priorityActivityId, objPriorityId, activityTypeId, txtActivityEng.Text, txtActivityFr.Text, userId, txtActivityEng.Visible ? 1 : 2, DBNull.Value });
                }
                else
                {
                    DBContext.Add("InsertActivity_Ebola", new object[] { objPriorityId, activityTypeId, txtActivityEng.Text, txtActivityFr.Text, userId, DBNull.Value });
                }
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
            ddlObjectiveNew.DataSource = GetObjectives();
            ddlObjectiveNew.DataBind();
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