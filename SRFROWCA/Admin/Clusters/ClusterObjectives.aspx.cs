using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using System.Web.Security;

namespace SRFROWCA.Admin.Clusters
{
    public partial class ClusterObjectives : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            LoadObjectives();
            PopulateEmergencies();
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
            DataTable dt = GetObjectives();
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
            gvObjective.DataSource = GetObjectives();
            gvObjective.DataBind();
        }

        private DataTable GetObjectives()
        {
            return DBContext.GetData("GetAllObjectives");
        }

        private void PopulateEmergencies()
        {
            ddlLocEmergencies.DataValueField = "LocationEmergencyId";
            ddlLocEmergencies.DataTextField = "EmergencyName";

            ddlLocEmergencies.DataSource = GetEmergencies();
            ddlLocEmergencies.DataBind();

            ListItem item = new ListItem("Select Emergency", "0");
            ddlLocEmergencies.Items.Insert(0, item);
            ddlEmgClusters.SelectedIndex = 0;
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

        private DataTable GetEmergencyClusters(int emergencyId)
        {
            return DBContext.GetData("GetEmergencyClusters", new object[] { emergencyId });
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
            try
            {
                int emgClusterId = 0;
                int.TryParse(ddlEmgClusters.SelectedValue, out emgClusterId);

                Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
                string objectiveName = txtObj.Text.Trim();
                Server.HtmlEncode(objectiveName);
                if(!string.IsNullOrEmpty(hfPKId.Value))
                {
                    int pkId = Convert.ToInt32(hfPKId.Value);
                    DBContext.Update("UpdateObjective", new object[] { pkId, emgClusterId, objectiveName, userId, DBNull.Value });
                }
                else
                {
                     DBContext.Add("InsertObjective", new object[] { emgClusterId, objectiveName, userId, DBNull.Value });
                }
            }
            catch
            {
                throw;
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
    }
}