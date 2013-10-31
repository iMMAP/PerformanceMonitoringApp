using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Admin.Clusters
{
    public partial class ClusterObjectives : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            this.Form.DefaultButton = this.btnAdd.UniqueID;

            PopulateEmergencies();
            LoadObjectives();
        }

        // Add delete confirmation message with all delete buttons.
        protected void gvObjective_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvObjective_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //// If user click on Delete button.
            //if (e.CommandName == "DeleteOrg")
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
            if (e.CommandName == "EditObjective")
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
                if (lblEmergencyClusterId != null)
                {
                    ddlEmgClusters.SelectedValue = lblEmergencyClusterId.Text;
                }

                PopulateStrategicObjectives();
                Label lblStrategicObjectiveId = row.FindControl("lblStrategicObjectiveId") as Label;
                if (lblStrategicObjectiveId != null)
                {
                    ddlStrObjectives.SelectedValue = lblStrategicObjectiveId.Text;
                }

                Label lblObjective = row.FindControl("lblObjective") as Label;
                if (lblObjective != null)
                {
                    txtObj.Text = lblObjective.Text;
                }
            }
        }

        protected void gvObjective_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Retrieve the table from the session object.
            DataTable dt = ROWCACommon.GetObjectives(this.User);
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvObjective.DataSource = dt;
                gvObjective.DataBind();
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

        private void LoadObjectives()
        {
            gvObjective.DataSource = ROWCACommon.GetObjectives(this.User);
            gvObjective.DataBind();
        }

        private void PopulateEmergencies()
        {
            UI.FillLocationEmergency(ddlLocEmergencies, ROWCACommon.GetEmergencies(this.User));
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

        private DataTable GetEmergencyClusters(int emergencyId)
        {
            return DBContext.GetData("GetEmergencyClusters", new object[] { emergencyId });
        }

        protected void ddlEmgClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateStrategicObjectives();
        }

        private void PopulateStrategicObjectives()
        {
            ddlStrObjectives.DataValueField = "StrategicObjectiveId";
            ddlStrObjectives.DataTextField = "StrategicObjectiveName";

            ddlStrObjectives.DataSource = GetClusterObjectives();
            ddlStrObjectives.DataBind();

            ListItem item = new ListItem("Select Str Objective", "0");
            ddlStrObjectives.Items.Insert(0, item);
        }

        private DataTable GetClusterObjectives()
        {
            int emgClusterId = 0;
            int.TryParse(ddlEmgClusters.SelectedValue, out emgClusterId);
            if (emgClusterId > 0)
            {
                return DBContext.GetData("GetStrategicObjectives", new object[] { emgClusterId });
            }

            return new DataTable();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SaveObjective();
            LoadObjectives();
            ClearPopupControls();
        }

        private void ClearPopupControls()
        {
            //if (ddlLocEmergencies.Items.Count > 0)
            //{
            //    ddlLocEmergencies.SelectedIndex = 0;
            //}

            //if (ddlEmgClusters.Items.Count > 0)
            //{
            //    ddlEmgClusters.SelectedIndex = 0;
            //}

            hfPKId.Value = "";
            txtObj.Text = "";
        }

        private void SaveObjective()
        {
            int emgClusterId = 0;
            int.TryParse(ddlEmgClusters.SelectedValue, out emgClusterId);

            Guid userId = ROWCACommon.GetCurrentUserId();
            string objectiveName = txtObj.Text.Trim();
            Server.HtmlEncode(objectiveName);
            int strObjId = 0;
            int.TryParse(ddlStrObjectives.SelectedValue, out strObjId);

            if (!string.IsNullOrEmpty(hfPKId.Value))
            {
                int pkId = Convert.ToInt32(hfPKId.Value);
                DBContext.Update("UpdateObjective", new object[] { pkId, emgClusterId, objectiveName, userId, strObjId, DBNull.Value });
            }
            else
            {
                DBContext.Add("InsertObjective", new object[] { emgClusterId, objectiveName, userId, strObjId, DBNull.Value });
            }
        }

        protected void gvObjective_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvObjective.PageIndex = e.NewPageIndex;
            gvObjective.SelectedIndex = -1;
            LoadObjectives();
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
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "ClusterObjectives", this.User);
        }
    }
}