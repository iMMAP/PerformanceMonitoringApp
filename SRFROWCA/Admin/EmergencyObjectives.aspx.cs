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
    public partial class EmergencyObjectives: BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmergencyObjectives();
            }
        }

        private void LoadEmergencyObjectives()
        {
            string emergency = null;
            string objective = null;

            if (!string.IsNullOrEmpty(txtEmergencyName.Text.Trim()))
                emergency = txtEmergencyName.Text;

            if (!string.IsNullOrEmpty(txtObjectiveName.Text.Trim()))
                objective = txtObjectiveName.Text;

            gvEmergencyObjectives.DataSource = GetEmergencyObjectives(emergency, objective);
            gvEmergencyObjectives.DataBind();
        }

        private DataTable GetEmergencyObjectives(string emergency, string objective)
        {
            return DBContext.GetData("uspGetEmergencyObjectives", new object[] { emergency, objective, RC.SelectedSiteLanguageId });
        }

        protected void gvEmergencyObjectives_Sorting(object sender, GridViewSortEventArgs e)
        {
            string emergency = null;
            string objective = null;

            if (!string.IsNullOrEmpty(txtEmergencyName.Text.Trim()))
                emergency = txtEmergencyName.Text;

            if (!string.IsNullOrEmpty(txtObjectiveName.Text.Trim()))
                objective = txtObjectiveName.Text;

            DataTable dt = GetEmergencyObjectives(emergency, objective);
            
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
                Button deleteButton = e.Row.FindControl("btnDelete") as Button;
                if (deleteButton != null)
                {
                    deleteButton.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this Objective?')");
                }
            }
        }

        protected void gvEmergencyObjectives_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // If user click on Delete button.
            if (e.CommandName == "DeleteObjective")
            {
                int emgObjectiveId = Convert.ToInt32(e.CommandArgument);
            }

            // Edit Project.
            if (e.CommandName == "EditObjective")
            {
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadEmergencyObjectives();
        }
    }
}