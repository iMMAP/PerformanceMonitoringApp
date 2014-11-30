using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Admin
{
    public partial class EmergencyObjectives : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmergencyObjectives();
                LoadCombos();
            }
        }

        internal override void BindGridData()
        {
            LoadCombos();
            LoadEmergencyObjectives();
        }

        private void LoadCombos()
        {
            ddlEmergency.DataValueField = 
                ddlEmergencySearch.DataValueField= "EmergencyId";
            ddlEmergency.DataTextField = 
                ddlEmergencySearch.DataTextField = "EmergencyName";

            ddlEmergency.DataSource = ddlEmergencySearch.DataSource = RC.GetAllEmergencies(RC.SelectedSiteLanguageId);
            ddlEmergency.DataBind();
            ddlEmergencySearch.DataBind();
        }

        private void LoadEmergencyObjectives()
        {
            int? emergencyId = null;
            string objective = null;

            if (Convert.ToInt32(ddlEmergencySearch.SelectedValue) > -1)
                emergencyId = Convert.ToInt32(ddlEmergencySearch.SelectedValue);

            if (!string.IsNullOrEmpty(txtObjectiveName.Text.Trim()))
                objective = txtObjectiveName.Text;

            gvEmergencyObjectives.DataSource = GetEmergencyObjectives(emergencyId, objective);
            gvEmergencyObjectives.DataBind();
        }

        private DataTable GetEmergencyObjectives(int? emergencyId, string objective)
        {
            return DBContext.GetData("GetEmgObjectives", new object[] { emergencyId, objective, RC.SelectedSiteLanguageId });
        }

        protected void gvEmergencyObjectives_Sorting(object sender, GridViewSortEventArgs e)
        {
            int? emergencyId = null;
            string objective = null;

            if (Convert.ToInt32(ddlEmergencySearch.SelectedValue) > -1)
                emergencyId = Convert.ToInt32(ddlEmergencySearch.SelectedValue);

            if (!string.IsNullOrEmpty(txtObjectiveName.Text.Trim()))
                objective = txtObjectiveName.Text;

            DataTable dt = GetEmergencyObjectives(emergencyId, objective);

            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvEmergencyObjectives.DataSource = dt;
                gvEmergencyObjectives.DataBind();
            }
        }

        protected void gvEmergencyObjectives_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton deleteButton = e.Row.FindControl("btnDelete") as LinkButton;
                if (deleteButton != null)
                {
                    deleteButton.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this Objective?')");
                }
            }
        }

        protected void gvEmergencyObjectives_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteObjective")
            {
                int emgObjectiveId = Convert.ToInt32(e.CommandArgument);

                if (IsObjectiveUsed(emgObjectiveId))
                {
                    ShowMessage("Objective cannot be deleted! It is being used.", RC.NotificationType.Error, true, 500);
                    return;
                }

                if (!DeleteEmergencyObjective(emgObjectiveId))
                    ShowMessage("Objective cannot be deleted!", RC.NotificationType.Error, true, 500);

                LoadEmergencyObjectives();
            }

            if (e.CommandName == "EditObjective")
            {
                ClearPopupControls();
                hfEmgObjID.Value = e.CommandArgument.ToString();

                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;
                Label lblEmergencyID = row.FindControl("lblEmergencyId") as Label;
                Label lblObjAlternate = row.FindControl("lblObjAlternate") as Label;

                if (lblEmergencyID != null)
                    ddlEmergency.SelectedValue = lblEmergencyID.Text;

                if (gvEmergencyObjectives.DataKeys[row.RowIndex].Value.ToString() == "1")
                {
                    txtObjectiveEng.Text = row.Cells[3].Text;

                    if (lblObjAlternate != null)
                        txtObjectiveFr.Text = lblObjAlternate.Text;
                }
                else
                {
                    txtObjectiveFr.Text = row.Cells[3].Text;

                    if (lblObjAlternate != null)
                        txtObjectiveEng.Text = lblObjAlternate.Text;
                }

                mpeAddObjective.Show();
            }
        }

        private bool IsObjectiveUsed(int emgObjectiveId)
        {
            if (DBContext.GetData("uspIsObjectiveUsed", new object[] { emgObjectiveId }).Rows.Count > 0)
                return true;
            else
                return false;
        }

        private bool DeleteEmergencyObjective(int emgObjectiveId)
        {
            if (DBContext.Delete("uspDeleteEmergencyObjective", new object[] { emgObjectiveId, null }) > 0)
                return true;
            else
                return false;
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadEmergencyObjectives();
        }

        protected void btnAddObjectives_Click(object sender, EventArgs e)
        {
            ClearPopupControls();
            mpeAddObjective.Show();
        }

        private void ClearPopupControls()
        {
            if (ddlEmergency.Items.Count > 0)
                ddlEmergency.SelectedIndex = 0;

            hfEmgObjID.Value = txtObjectiveFr.Text = txtObjectiveEng.Text = string.Empty;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SaveEmergencyObjective();
            LoadEmergencyObjectives();
            ClearPopupControls();
            mpeAddObjective.Hide();
        }

        private void SaveEmergencyObjective()
        {
            if (string.IsNullOrEmpty(txtObjectiveEng.Text))
                txtObjectiveEng.Text = txtObjectiveFr.Text;
            else if (string.IsNullOrEmpty(txtObjectiveFr.Text))
                txtObjectiveFr.Text = txtObjectiveEng.Text;

            Guid userId = RC.GetCurrentUserId;
            string objectiveEng = txtObjectiveEng.Text.Trim();
            string objectiveFr = txtObjectiveFr.Text.Trim();
            int emergencyID = Convert.ToInt32(ddlEmergency.SelectedValue);

            if (!string.IsNullOrEmpty(hfEmgObjID.Value))
                DBContext.Add("uspInsertEmergencyObjective", new object[] { emergencyID, objectiveEng, objectiveFr, userId, Convert.ToInt32(hfEmgObjID.Value), null, null });
            else
                DBContext.Add("uspInsertEmergencyObjective", new object[] { emergencyID, objectiveEng, objectiveFr, userId, null, null, null });
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 0)
        {
            updMessage.Update();
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void ddlEmergencySearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEmergencyObjectives();
        }
    }
}