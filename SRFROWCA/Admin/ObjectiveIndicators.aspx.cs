using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Admin
{
    public partial class ObjectiveIndicators : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            this.Form.DefaultButton = this.btnAdd.UniqueID;

            LoadIndicators();
            PopulateEmergencies();
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
                    "confirm('Are you sure you want to delete this organization?')");
                }
            }
        }

        // Execute row commands like Edit, Delete etc. on Grid.
        protected void gvIndicator_RowCommand(object sender, GridViewCommandEventArgs e)
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
            if (e.CommandName == "EditIndicator")
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
                if (lblClusterObjectiveId != null)
                {
                    ddlObjectives.SelectedValue = lblClusterObjectiveId.Text;
                }

                Label lblIndicator = row.FindControl("lblIndicator") as Label;
                if (lblIndicator != null)
                {
                    txtIndicator.Text = lblIndicator.Text;
                }
            }
        }

        protected void gvIndicator_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Retrieve the table from the session object.
            DataTable dt = ROWCACommon.GetIndicators(this.User);
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
            gvIndicator.DataSource = ROWCACommon.GetIndicators(this.User);
            gvIndicator.DataBind();
        }

        private void PopulateEmergencies()
        {
            UI.FillLocationEmergency(ddlLocEmergencies, ROWCACommon.GetAllEmergencies());
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SaveIndicator();
            LoadIndicators();
            ClearPopupControls();
        }

        private void ClearPopupControls()
        {
            hfPKId.Value = "";
            txtIndicator.Text = "";
        }

        private void SaveIndicator()
        {
            try
            {
                int clusterObjId = 0;
                int.TryParse(ddlObjectives.SelectedValue, out clusterObjId);

                Guid userId = ROWCACommon.GetCurrentUserId();
                string indicatorName = txtIndicator.Text.Trim();
                if (!string.IsNullOrEmpty(hfPKId.Value))
                {
                    int pkId = Convert.ToInt32(hfPKId.Value);
                    DBContext.Update("UpdateIndicator", new object[] { pkId, clusterObjId, indicatorName, userId, DBNull.Value });
                }
                else
                {
                    DBContext.Add("InsertIndicator", new object[] { clusterObjId, indicatorName, userId, DBNull.Value });
                }
            }
            catch
            {
                throw;
            }
        }

        protected void gvIndicator_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvIndicator.PageIndex = e.NewPageIndex;
            gvIndicator.SelectedIndex = -1;
            LoadIndicators();
        }

        //private int PKId1
        //{
        //    get
        //    {
        //        int pkId1 = 0;
        //        if (ViewState["PKId1"] != null)
        //        {
        //            int.TryParse(ViewState["PKId1"].ToString(), out pkId1);
        //        }

        //        return pkId1;
        //    }
        //    set
        //    {
        //        ViewState["PKId1"] = value.ToString();
        //    }
        //}

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "ObjectiveIndicators", this.User);
        }
    }
}