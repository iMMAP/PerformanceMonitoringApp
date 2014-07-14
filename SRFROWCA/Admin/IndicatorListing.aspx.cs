using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Admin
{
    public partial class IndicatorListing : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            this.Form.DefaultButton = this.btnAdd.UniqueID;

            LoadIndicators();
            PopulateFilters();
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
            if (e.CommandName == "Delete")
            {
                int activityDataId = Convert.ToInt32(e.CommandArgument);

                // Check if any IP has reported on this project. If so then do not delete it.
                if (!IndicatorIsBeingUsed(activityDataId))
                {
                    ShowMessage("Indicator cannot be deleted! It is being used.", RC.NotificationType.Error, true, 500);
                    return;
                }

                //DeleteIndicator(activityDataId);
                //LoadIndicators();
            }

            // Edit Project.
            if (e.CommandName == "EditIndicator")
            {
                ClearPopupControls();

                hdnIndicatorId.Value = e.CommandArgument.ToString();

                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;
                ddlClusterNew.SelectedValue = gvIndicator.DataKeys[row.RowIndex].Values["ClusterId"].ToString();
                LoadObjectivesByCluster();
                ddlObjectiveNew.SelectedValue = gvIndicator.DataKeys[row.RowIndex].Values["ClusterObjectiveId"].ToString();
                LoadPrioritiesByObjective();
                ddlPriorityNew.SelectedValue = gvIndicator.DataKeys[row.RowIndex].Values["ObjectivePriorityId"].ToString();
                LoadActivitiesByPriority();
                ddlActivityNew.SelectedValue = gvIndicator.DataKeys[row.RowIndex].Values["PriorityActivityId"].ToString();
                ddlUnit.SelectedValue = gvIndicator.DataKeys[row.RowIndex].Values["UnitId"].ToString();
                //chkisSRP.Checked = Convert.ToBoolean(gvIndicator.DataKeys[row.RowIndex].Values["IsSRPIndicator"].ToString());
                //chkIsPriority.Checked = Convert.ToBoolean(gvIndicator.DataKeys[row.RowIndex].Values["IsPriorityIndicatory"].ToString());
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
            PopulateFilters();
        }

        private bool IndicatorIsBeingUsed(int activityDataId)
        {
            DataTable dt = DBContext.GetData("GetIsIndicatorBeingUsed", new object[] { activityDataId });
            return !(dt.Rows.Count > 0);
        }

        private void DeleteIndicator(int activityDataId)
        {
            DBContext.Delete("DeleteIndicator", new object[] { activityDataId, DBNull.Value });
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

        private void PopulateFilters()
        {
            ddlCluster.Items.Clear();
            ddlCluster.Items.Add(new ListItem("All", "-1"));
            ddlCluster.DataValueField = "ClusterId";
            ddlCluster.DataTextField = "ClusterName";
            ddlCluster.DataSource = GetClusters();
            ddlCluster.DataBind();

            ddlObjective.Items.Clear();
            ddlObjective.Items.Add(new ListItem("All", "-1"));
            ddlObjective.DataValueField = "ObjectiveId";
            ddlObjective.DataTextField = "Objective";
            ddlObjective.DataSource = GetObjectives();
            ddlObjective.DataBind();

            ddlPriority.Items.Clear();
            ddlPriority.Items.Add(new ListItem("All", "-1"));
            ddlPriority.DataValueField = "HumanitarianPriorityId";
            ddlPriority.DataTextField = "HumanitarianPriority";
            ddlPriority.DataSource = GetPriorities();
            ddlPriority.DataBind();

            ddlActivity.Items.Clear();
            ddlActivity.Items.Add(new ListItem("All", "-1"));
            ddlActivity.DataValueField = "priorityActivityId";
            ddlActivity.DataTextField = "ActivityName";
            ddlActivity.DataSource = GetActivities();
            ddlActivity.DataBind();

            ddlClusterNew.Items.Clear();
            ddlClusterNew.Items.Add(new ListItem("Select", "-1"));
            ddlClusterNew.DataValueField = "ClusterId";
            ddlClusterNew.DataTextField = "ClusterName";
            ddlClusterNew.DataSource = GetClusters();
            ddlClusterNew.DataBind();

            ddlUnit.Items.Clear();
            ddlUnit.Items.Add(new ListItem("Select", "-1"));
            ddlUnit.DataValueField = "UnitId";
            ddlUnit.DataTextField = "Unit";
            ddlUnit.DataSource = GetUnits();
            ddlUnit.DataBind();
        }

        private DataTable GetClusters()
        {

            return DBContext.GetData("GetAllClusters", new object[] { (int)RC.SelectedSiteLanguageId });
        }

        private DataTable GetIndicators()
        {
            int? clusterId = ddlCluster.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlCluster.SelectedValue);
            int? objectiveId = ddlObjective.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlObjective.SelectedValue);
            int? priorityId = ddlPriority.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlPriority.SelectedValue);
            int? activityId = ddlActivity.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlActivity.SelectedValue);
            string search = string.IsNullOrEmpty(txtActivityName.Text) ? null : txtActivityName.Text;
            return DBContext.GetData("GetAllIndicators", new object[] { clusterId, objectiveId, priorityId, activityId, search, (int)RC.SelectedSiteLanguageId });
        }

        private DataTable GetObjectives()
        {
            return DBContext.GetData("GetObjectives", new object[] { (int)RC.SelectedSiteLanguageId });
        }

        private DataTable GetPriorities()
        {
            return DBContext.GetData("GetPriorities", new object[] { (int)RC.SelectedSiteLanguageId });
        }
        private DataTable GetUnits()
        {
            return DBContext.GetData("GetAllUnits", new object[] { (int)RC.SelectedSiteLanguageId });
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
        private void LoadActivitiesByPriority()
        {
            ddlActivityNew.Items.Clear();
            ddlActivityNew.Items.Add(new ListItem("Select", "-1"));
            ddlActivityNew.DataValueField = "PriorityActivityId";
            ddlActivityNew.DataTextField = "ActivityName";
            ddlActivityNew.DataSource = GetActivities(Convert.ToInt32(ddlPriorityNew.SelectedValue));
            ddlActivityNew.DataBind();
        }
        private DataTable GetObjectives(int? clusterId = null)
        {
            if (clusterId == null)
            {
                return DBContext.GetData("GetObjectives", new object[] { (int)RC.SelectedSiteLanguageId });
            }
            else
            {
                return DBContext.GetData("GetObjectivesByClusterId", new object[] { (int)RC.SelectedSiteLanguageId, clusterId });
            }
        }

        private DataTable GetPriorities(int? objectiveId = null)
        {
            if (objectiveId == null)
            {
                return DBContext.GetData("GetPriorities", new object[] { (int)RC.SelectedSiteLanguageId });
            }
            else
            {
                return DBContext.GetData("GetPrioritiesByObjective", new object[] { (int)RC.SelectedSiteLanguageId, objectiveId });
            }
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
            ddlClusterNew.SelectedIndex = 0;
            ddlObjectiveNew.SelectedIndex = 0;
            ddlPriorityNew.SelectedIndex = 0;
            ddlActivityNew.SelectedIndex = 0;
            ddlUnit.SelectedIndex = 0;
            //chkIsPriority.Checked = false;
            //chkisSRP.Checked = false;
            txtActivityEng.Text = txtActivityFr.Text = string.Empty;
            txtActivityFr.Visible = txtActivityEng.Visible = true;
            rfvEmgName.Enabled = rfvEmgNameFr.Enabled = true;
            trEnglish.Visible = trFrench.Visible = true;
            //hdnIndicatorId.Value = txtEmgNameEng.Text = txtEmgNameFr.Text = "";
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

        protected void gvIndicator_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void ddlClusterNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadObjectivesByCluster();
        }

        protected void ddlObjectiveNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPrioritiesByObjective();
        }

        protected void ddlPriorityNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadActivitiesByPriority();
        }

        protected void gvIndicator_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

    }
}