using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Ebola
{
    public partial class IndicatorListing : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            this.Form.DefaultButton = this.btnAdd.UniqueID;
            LoadIndicators();
            PopulateControls();
        }

        // Add delete confirmation message with all delete buttons.
        protected void gvIndicator_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button deleteButton = e.Row.FindControl("btnDelete") as Button;
                if (deleteButton != null)
                {
                    deleteButton.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this Indicator?')");
                }
            }
        }

        // Execute row commands like Edit, Delete etc. on Grid.
        protected void gvIndicator_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // If user click on Delete button.
            if (e.CommandName == "DeleteIndicator")
            {
                int activityDataId = Convert.ToInt32(e.CommandArgument);

                // Check if any IP has reported on this project. If so then do not delete it.
                if (!IndicatorIsBeingUsed(activityDataId))
                {
                    ShowMessage("Indicator cannot be deleted! It is being used.", RC.NotificationType.Error, true, 2000);
                }
                else
                {
                    DeleteIndicator(activityDataId);
                    LoadIndicators();
                    ShowMessage("Indicator Deleted Successfully!");
                }
            }

            // Edit Project.
            if (e.CommandName == "EditIndicator")
            {
                ClearPopupControls();

                hdnIndicatorId.Value = e.CommandArgument.ToString();

                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;
                int clusterObjectiveId = Convert.ToInt32( gvIndicator.DataKeys[row.RowIndex].Values["ClusterObjectiveId"].ToString());
                ddlObjectiveNew.SelectedValue = gvIndicator.DataKeys[row.RowIndex].Values["ClusterObjectiveId"].ToString();
                LoadActivities(clusterObjectiveId);
                ddlActivityNew.SelectedValue = gvIndicator.DataKeys[row.RowIndex].Values["PriorityActivityId"].ToString();
                ddlUnit.SelectedValue = gvIndicator.DataKeys[row.RowIndex].Values["UnitId"].ToString();

                if (gvIndicator.DataKeys[row.RowIndex].Values["SiteLanguageId"].ToString() == "1")
                {
                    txtActivityEng.Text = gvIndicator.DataKeys[row.RowIndex].Values["DataName"].ToString();
                    txtActivityFr.Visible = false;
                    rfvEmgNameFr.Enabled = false;
                    trFrench.Visible = false;
                }
                else
                {
                    txtActivityFr.Text = gvIndicator.DataKeys[row.RowIndex].Values["DataName"].ToString();
                    txtActivityEng.Visible = false;
                    rfvEmgName.Enabled = false;
                    trEnglish.Visible = false;
                }

                mpeAddOrg.Show();
            }
        }
        protected void btnSearch2_Click(object sender, EventArgs e)
        {
            LoadIndicators();
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string fileName = "Indicators";
            string fileExtention = ".xls";
            ExportUtility.ExportGridView(gvIndicator, fileName, fileExtention, Response);
        }

        internal override void BindGridData()
        {
            LoadIndicators();
            PopulateControls();
        }

        private bool IndicatorIsBeingUsed(int activityDataId)
        {
            DataTable dt = DBContext.GetData("GetIsIndicatorBeingUsed_Ebola", new object[] { activityDataId });
            return !(dt.Rows.Count > 0);
        }

        private void DeleteIndicator(int activityDataId)
        {
            DBContext.Delete("DeleteIndicatorNew_Ebola", new object[] { activityDataId, DBNull.Value });
        }

        protected void gvIndicator_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetIndicators();
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvIndicator.DataSource = dt;
                gvIndicator.DataBind();
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

        private void LoadIndicators()
        {
            gvIndicator.DataSource = GetIndicators();
            gvIndicator.DataBind();
        }

        private void PopulateControls()
        {
            LoadObjectives(ddlObjective);
            LoadObjectivesByCluster();
            LoadUnits();
            LoadActivityFilter(ddlActivity, (int?)null);
        }

        private void LoadUnits()
        {
            ddlUnit.Items.Clear();
            ddlUnit.Items.Add(new ListItem("Select", "-1"));
            ddlUnit.DataValueField = "UnitId";
            ddlUnit.DataTextField = "Unit";
            ddlUnit.DataSource = GetUnits();
            ddlUnit.DataBind();
            
        }

        private void LoadObjectives(DropDownList ddl)
        {
            ddl.DataValueField = "ObjectiveId";
            ddl.DataTextField = "Objective";
            ddl.DataSource = GetObjectivesByEmergencyAndCluster();
            ddl.DataBind();
        }

        private void LoadActivityFilter(DropDownList ddl, int? objectiveId)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("All", "-1"));
            ddl.DataValueField = "priorityActivityId";
            ddl.DataTextField = "ActivityName";
            ddl.DataSource = GetActivitiesByEmergencyAndPriority(objectiveId);
            ddl.DataBind();
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

        private DataTable GetObjectivesByEmergencyAndCluster()
        {
            int emergencyId = 2;
            int? clusterId = 3;
            return DBContext.GetData("GetObjectivesByEmergencyAndCluster", new object[] { (int)RC.SelectedSiteLanguageId, emergencyId, clusterId });
        }


        private DataTable GetClusters()
        {
            int? emgId = 2;
            return DBContext.GetData("GetAllClusters", new object[] { (int)RC.SelectedSiteLanguageId, emgId });
        }

        private DataTable GetIndicators()
        {
            int? emergencyId = 2;
            int? clusterId = 3;
            int? objectiveId = ddlObjective.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlObjective.SelectedValue);
            int? priorityId = GetPrioirtyIdOnObjective(objectiveId);
            int? activityId = ddlActivity.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlActivity.SelectedValue);
            string search = string.IsNullOrEmpty(txtActivityName.Text) ? null : txtActivityName.Text;
            return DBContext.GetData("GetAllIndicators", new object[] { emergencyId, clusterId, objectiveId, priorityId, activityId, search, (int)RC.SelectedSiteLanguageId });
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

        private DataTable GetUnits()
        {
            return DBContext.GetData("GetAllUnits", new object[] { (int)RC.SelectedSiteLanguageId });
        }


        private DataTable GetActivitiesByEmergencyAndPriority(int? objectiveId)
        {
            int emergencyId = 2;
            int? clusterId = 3;            
            int? priorityId = GetPrioirtyIdOnObjective(objectiveId);
            return DBContext.GetData("GetActivitiesForddl", new object[] { (int)RC.SelectedSiteLanguageId, emergencyId, clusterId, objectiveId, priorityId });
        }

        private DataTable GetActivities(int? priorityId = null)
        {
            if (priorityId == null)
            {
                return DBContext.GetData("GetActivitiesForddl", new object[] { (int)RC.SelectedSiteLanguageId });
            }
            else
            {
                return DBContext.GetData("GetActivitiesByPriority", new object[] { (int)RC.SelectedSiteLanguageId, priorityId });
            }
        }

        protected void btnAddIndicator_Click(object sender, EventArgs e)
        {
            ClearPopupControls();
            LoadActivities((int?)null);
            mpeAddOrg.Show();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SaveIndicator();
            LoadIndicators();
            mpeAddOrg.Hide();
            ClearPopupControls();
        }

        private void ClearPopupControls()
        {
            hdnIndicatorId.Value = "";
            ddlObjectiveNew.SelectedIndex = 0;
            ddlActivityNew.SelectedIndex = 0;
            ddlUnit.SelectedIndex = 0;
            txtActivityEng.Text = txtActivityFr.Text = string.Empty;
            txtActivityFr.Visible = txtActivityEng.Visible = true;
            rfvEmgName.Enabled = rfvEmgNameFr.Enabled = true;
            trEnglish.Visible = trFrench.Visible = true;
        }

        private void SaveIndicator()
        {
            int priorityActivityId = Convert.ToInt32(ddlActivityNew.SelectedValue);
            Guid userId = RC.GetCurrentUserId;

            if (!string.IsNullOrEmpty(hdnIndicatorId.Value))
            {
                int indicatorId = Convert.ToInt32(hdnIndicatorId.Value);
                DBContext.Update("UpdateActivityData", new object[] { indicatorId, priorityActivityId, Convert.ToInt32(ddlUnit.SelectedValue), false, false, txtActivityEng.Text, txtActivityFr.Text, userId, txtActivityEng.Visible ? 1 : 2, DBNull.Value });
            }
            else
            {
                DBContext.Add("InsertOutPutIndicator", new object[] { priorityActivityId, txtActivityEng.Text, txtActivityFr.Text, Convert.ToInt32(ddlUnit.SelectedValue), false,
                                                                    false, userId, DBNull.Value });
            }
        }


        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 0)
        {
            updMessage.Update();
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        enum LocationTypes
        {
            Country = 2,
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "IndicatorListing", this.User);
        }

        protected void gvIndicator_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvIndicator.PageIndex = e.NewPageIndex;
            LoadIndicators();
        }

        protected void ddlObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            int? objectiveId = (ddlObjective.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlObjective.SelectedValue));
            LoadActivityFilter(ddlActivity, objectiveId);
            LoadIndicators();
        }

        protected void ddlObjectiveNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            int? clusterObjectiveId = (ddlObjectiveNew.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlObjectiveNew.SelectedValue));
            LoadActivities(clusterObjectiveId);
        }

        protected void ddlActivity_SelectedindexChanged(object sender, EventArgs e)
        {
            LoadIndicators();
        }

        private void LoadActivities(int? clusterObjectiveId)
        {

            ddlActivityNew.Items.Clear();
            ddlActivityNew.Items.Add(new ListItem("All", "-1"));
            ddlActivityNew.DataValueField = "priorityActivityId";
            ddlActivityNew.DataTextField = "ActivityName";
            ddlActivityNew.DataSource = GetActivitiesOnClusterObjective(clusterObjectiveId);
            ddlActivityNew.DataBind();
        }

        private object GetActivitiesOnClusterObjective(int? clusterObjectiveId)
        {
            int emergencyId = 2;
            int? clusterId = 3;            
            return DBContext.GetData("GetActivitiesForddl_Ebola", new object[] { (int)RC.SelectedSiteLanguageId, emergencyId, clusterId, clusterObjectiveId });
        
        }
    }
}