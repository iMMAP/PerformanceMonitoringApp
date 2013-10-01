using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;

namespace SRFROWCA.Admin.Clusters
{
    public partial class IndicatorActivities : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack) return;

            LoadActivities();
            PopulateEmergencies();
            PopulateActivityTypes();
        }

        private void PopulateActivityTypes()
        {

            ddlActivityType.DataValueField = "ActivityTypeId";
            ddlActivityType.DataTextField = "ActivityType";

            ddlActivityType.DataSource = GetActivityTypes();
            ddlActivityType.DataBind();

            if (ddlActivityType.Items.Count > 0)
            {
                ddlActivityType.SelectedIndex = 0;
            }
        }

        private object GetActivityTypes()
        {
            return DBContext.GetData("GetActivityTypes");
        }

        private void LoadActivities()
        {
            gvActivity.DataSource = GetAllActivities();
            gvActivity.DataBind();
        }

        private DataTable GetAllActivities()
        {
            return DBContext.GetData("GetAllActivities");
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
                    "confirm('Are you sure you want to delete this record?')");
                }
            }
        }

        // Execute row commands like Edit, Delete etc. on Grid.
        protected void gvActivity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //// If user click on Delete button.
            //if (e.CommandName == "DeleteIndicator")
            //{
            //    int locEmgId = Convert.ToInt32(e.CommandArgument);

            //    // Check if any IP has reported on this project. If so then do not delete it.
            //    if (AnyIPReportedOnEmg(locEmgId))
            //    {
            //        lblMessage.Text = "Organization cannot be deleted! It is being used Offices and/or Reports.";
            //        lblMessage.CssClass = "error-message";
            //        lblMessage.Visible = true;

            //        return;
            //    }

            //    DeleteEmergencies(locEmgId);
            //    LoadEmergencies();
            //}

            // Edit Project.
            if (e.CommandName == "EditActivity")
            {
                hfPKId.Value = e.CommandArgument.ToString();

                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;

                Label lblLocationEmergencyId = row.FindControl("lblLocationEmergencyId") as Label;
                int emgId = 0;
                if (lblLocationEmergencyId != null)
                {
                    int.TryParse(lblLocationEmergencyId.Text, out emgId);
                    ddlLocEmergencies.SelectedValue = lblLocationEmergencyId.Text;
                }

                PopulateEmergencyClusters(emgId);

                Label lblEmergencyClusterId = row.FindControl("lblEmergencyClusterId") as Label;
                int emgClusterId = 0;
                if (lblEmergencyClusterId != null)
                {
                    int.TryParse(lblEmergencyClusterId.Text, out emgClusterId);
                    ddlEmgClusters.SelectedValue = lblEmergencyClusterId.Text;
                }

                PopulateObjectives(emgClusterId);

                Label lblClusterObjectiveId = row.FindControl("lblClusterObjectiveId") as Label;
                int objId = 0;
                if (lblClusterObjectiveId != null)
                {
                    int.TryParse(lblClusterObjectiveId.Text, out objId);
                    ddlObjectives.SelectedValue = lblClusterObjectiveId.Text;
                }

                PopulateIndicators(objId);
                Label lblIndicatorId = row.FindControl("lblObjectiveIndicatorId") as Label;
                if (lblIndicatorId != null)
                {
                    ddlIndicators.SelectedValue = lblIndicatorId.Text;
                }

                if (row.Cells[6].Text.Equals("One Time"))
                {
                    ddlActivityType.SelectedIndex = 0;
                }
                else
                {
                    ddlActivityType.SelectedIndex = 1;
                }

                Label lblActivity = row.FindControl("lblActivityName") as Label;
                if (lblActivity != null)
                {
                    txtActivity.Text = lblActivity.Text;
                }
                //txtActivity.Text = row.Cells[5].Text;
            }
        }

        private void PopulateIndicators(int objId)
        {
            ddlIndicators.DataValueField = "ObjectiveIndicatorId";
            ddlIndicators.DataTextField = "IndicatorName";

            ddlIndicators.DataSource = GetObjectiveIndicators(objId);
            ddlIndicators.DataBind();

            ListItem item = new ListItem("Select Indicator", "0");
            ddlIndicators.Items.Insert(0, item);
        }

        private object GetObjectiveIndicators(int objId)
        {
            return DBContext.GetData("GetObjectiveIndicators", new object[] { objId });
        }

        protected void gvActivity_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Retrieve the table from the session object.
            DataTable dt = GetAllActivities();
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

        private void PopulateEmergencies()
        {
            ddlLocEmergencies.DataValueField = "LocationEmergencyId";
            ddlLocEmergencies.DataTextField = "EmergencyName";

            ddlLocEmergencies.DataSource = GetEmergencies();
            ddlLocEmergencies.DataBind();

            ListItem item = new ListItem("Select Emergency", "0");
            ddlLocEmergencies.Items.Insert(0, item);
            ddlLocEmergencies.SelectedIndex = 0;
        }

        private DataTable GetEmergencies()
        {
            return DBContext.GetData("GetALLLocationEmergencies");
        }

        protected void ddlLocEmergencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            int emergencyId = 0;
            int.TryParse(ddlLocEmergencies.SelectedValue, out emergencyId);

            if (emergencyId > 0)
            {
                PopulateEmergencyClusters(emergencyId);
            }
        }

        private void PopulateEmergencyClusters(int emregencyId)
        {
            ddlEmgClusters.DataValueField = "EmergencyClusterId";
            ddlEmgClusters.DataTextField = "ClusterName";

            ddlEmgClusters.DataSource = GetEmergencyClusters(emregencyId);
            ddlEmgClusters.DataBind();

            ListItem item = new ListItem("Select Cluster", "0");
            ddlEmgClusters.Items.Insert(0, item);
        }

        protected void ddlEmgClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            int clusterId = 0;
            int.TryParse(ddlEmgClusters.SelectedValue, out clusterId);
            if (clusterId > 0)
            {
                PopulateObjectives(clusterId);
            }
        }

        private void PopulateObjectives(int clusterId)
        {
            ddlObjectives.DataValueField = "ClusterObjectiveId";
            ddlObjectives.DataTextField = "ObjectiveName";

            ddlObjectives.DataSource = GetClusterObjectives(clusterId);
            ddlObjectives.DataBind();

            ListItem item = new ListItem("Select Objective", "0");
            ddlObjectives.Items.Insert(0, item);
        }

        private DataTable GetClusterObjectives(int clusterId)
        {
            return DBContext.GetData("GetEmergencyClusterObjectives", new object[] { clusterId });
        }

        private DataTable GetEmergencyClusters(int emergencyId)
        {
            return DBContext.GetData("GetEmergencyClusters", new object[] { emergencyId });
        }

        protected void ddlObjectives_SelectedIndexChanged(object sender, EventArgs e)
        {
            int objId = 0;
            int.TryParse(ddlObjectives.SelectedValue, out objId);
            if (objId > 0)
            {
                PopulateIndicators(objId);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SaveActivity();
            LoadActivities();
            ClearPopupControls();
        }

        private void ClearPopupControls()
        {
            hfPKId.Value = "";
            txtActivity.Text = "";
        }

        private void SaveActivity()
        {
            try
            {
                int indicatorId = 0;
                int.TryParse(ddlIndicators.SelectedValue, out indicatorId);

                int activityTypeId = 0;
                int.TryParse(ddlActivityType.SelectedValue, out activityTypeId);

                Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
                string activityName = txtActivity.Text.Trim();
                if (!string.IsNullOrEmpty(hfPKId.Value))
                {
                    int pkId = Convert.ToInt32(hfPKId.Value);
                    DBContext.Update("UpdateActivity", new object[] { pkId, indicatorId, activityName, activityTypeId, userId, DBNull.Value });
                }
                else
                {
                    DBContext.Add("InsertActivity", new object[] { indicatorId, activityName, activityTypeId, userId, DBNull.Value });
                }
            }
            catch
            {
                throw;
            }
        }

        protected void gvActivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvActivity.PageIndex = e.NewPageIndex;
            gvActivity.SelectedIndex = -1;
            LoadActivities();
        }

        //public int PKId
        //{
        //    get
        //    {
        //        int pkId = 0;
        //        if (ViewState["PKId"] != null)
        //        {
        //            int.TryParse(ViewState["PKId"].ToString(), out pkId);
        //        }

        //        return pkId;
        //    }
        //    set
        //    {
        //        ViewState["PKId"] = value.ToString();
        //    }
        //}
    }
}