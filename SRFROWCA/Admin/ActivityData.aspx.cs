using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Admin
{
    public partial class ActivityData : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            this.Form.DefaultButton = this.btnAdd.UniqueID;

            LoadData();
            PopulateEmergencies();
            PopulateUnits();
        }

        private void PopulateUnits()
        {
            ddlUnit.DataValueField = "UnitId";
            ddlUnit.DataTextField = "Unit";

            ddlUnit.DataSource = GetUnits();
            ddlUnit.DataBind();

            ListItem item = new ListItem("Select Unit", "0");
            ddlUnit.Items.Insert(0, item);
            ddlUnit.SelectedIndex = 0;
        }

        private object GetUnits()
        {
            return DBContext.GetData("GetAllUnits");
        }

        private void LoadData()
        {
            gvData.DataSource = RC.GetAllFrameWorkData(this.User);
            gvData.DataBind();
        }        

        // Add delete confirmation message with all delete buttons.
        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
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
            if (e.CommandName == "EditData")
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
                int indicatorId = 0;
                if (lblIndicatorId != null)
                {
                    int.TryParse(lblIndicatorId.Text, out indicatorId);
                    ddlIndicators.SelectedValue = lblIndicatorId.Text;
                }
                PopulateActivities(indicatorId);
                Label lblActivityId = row.FindControl("lblIndicatorActivityId") as Label;
                if (lblActivityId != null)
                {
                    ddlActivity.SelectedValue = lblActivityId.Text;
                }

                Label lblDataName = row.FindControl("lblDataName") as Label;
                if (lblDataName != null)
                {
                    txtData.Text = lblDataName.Text;
                }
                
                ddlUnit.SelectedItem.Text = row.Cells[7].Text;
            }
        }

        private void PopulateActivities(int indicatorId)
        {
            ddlActivity.DataValueField = "IndicatorActivityId";
            ddlActivity.DataTextField = "ActivityName";

            ddlActivity.DataSource = GetIndicatorActivities(indicatorId);
            ddlActivity.DataBind();

            ListItem item = new ListItem("Select Activity", "0");
            ddlActivity.Items.Insert(0, item);
        }

        private object GetIndicatorActivities(int indicatorId)
        {
            return DBContext.GetData("GetIndicatorActivities", new object[] { indicatorId });
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

        protected void gvData_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Retrieve the table from the session object.
            DataTable dt = RC.GetAllFrameWorkData(this.User);
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvData.DataSource = dt;
                gvData.DataBind();
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
            int? locationId = (int?)RC.SiteLanguage.English;
            UI.FillLocationEmergency(ddlLocEmergencies, RC.GetAllEmergencies(locationId));
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

        protected void ddlIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indicatorId = 0;
            int.TryParse(ddlIndicators.SelectedValue, out indicatorId);
            if (indicatorId > 0)
            {
                PopulateActivities(indicatorId);
            }
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
            SaveData();
            LoadData();
            ClearPopupControls();
        }

        private void ClearPopupControls()
        {
            hfPKId.Value = "";
            txtData.Text = "";
        }

        private void SaveData()
        {
            try
            {
                int activityId = 0;
                int.TryParse(ddlActivity.SelectedValue, out activityId);

                int val = 0;
                int.TryParse(ddlUnit.SelectedValue, out val);
                int? unitId = val > 0 ? val : (int?)null;

                Guid userId = RC.GetCurrentUserId;
                string dataName = txtData.Text.Trim();
                if (!string.IsNullOrEmpty(hfPKId.Value))
                {
                    int pkId = Convert.ToInt32(hfPKId.Value);
                    DBContext.Update("UpdateData", new object[] { pkId, activityId, dataName, unitId, userId, DBNull.Value });
                }
                else
                {
                    DBContext.Add("InsertData", new object[] { activityId, dataName, unitId, userId, DBNull.Value });
                }
            }
            catch
            {
                throw;
            }
        }

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            gvData.SelectedIndex = -1;
            LoadData();
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

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "ActivityData", this.User);
        }
    }
}