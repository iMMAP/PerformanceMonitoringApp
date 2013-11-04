using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Admin
{
    public partial class EmergencyListing : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            this.Form.DefaultButton = this.btnAdd.UniqueID;

            LoadEmergencies();
            PopulateDisasterTypes();
            PopulateLocations();
        }

        // Add delete confirmation message with all delete buttons.
        protected void gvEmergency_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvEmergency_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // If user click on Delete button.
            if (e.CommandName == "DeleteEmergency")
            {
                int locEmgId = Convert.ToInt32(e.CommandArgument);

                // Check if any IP has reported on this project. If so then do not delete it.
                if (!EmgIsBeingUsed(locEmgId))
                {
                    lblMessage.Text = "Emergency cannot be deleted! It is being used in reported data.";
                    lblMessage.CssClass = "error-message";
                    lblMessage.Visible = true;

                    return;
                }

                DeleteEmergency(locEmgId);
                LoadEmergencies();
            }

            // Edit Project.
            if (e.CommandName == "EditEmergency")
            {
                ClearPopupControls();

                hfLocEmgId.Value = e.CommandArgument.ToString();

                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;
                Label lblEmgTypeId = row.FindControl("lblEmergencyTypeId") as Label;
                if (lblEmgTypeId != null)
                {
                    ddlEmgType.SelectedValue = lblEmgTypeId.Text;
                }

                Label lblLocationId = row.FindControl("lblLocationId") as Label;
                if (lblLocationId != null)
                {
                    ddlLocations.SelectedValue = lblLocationId.Text;
                }

                txtEmgName.Text = row.Cells[2].Text;                
                mpeAddOrg.Show();
            }
        }

        private bool EmgIsBeingUsed(int locEmgId)
        {
            DataTable dt = DBContext.GetData("GetIsEmergencyBeingUsed", new object[] { locEmgId });
            return !(dt.Rows.Count > 0);
        }

        private void DeleteEmergency(int locEmgId)
        {
            DBContext.Delete("DeleteEmergency", new object[] { locEmgId, DBNull.Value });
        }

        protected void gvEmergency_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Retrieve the table from the session object.
            DataTable dt = ROWCACommon.GetEmergencies(this.User);
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvEmergency.DataSource = dt;
                gvEmergency.DataBind();
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

        private void LoadEmergencies()
        {
            gvEmergency.DataSource = ROWCACommon.GetEmergencies(this.User);
            gvEmergency.DataBind();
        }

        private void PopulateDisasterTypes()
        {
            ddlEmgType.DataValueField = "EmergencyTypeId";
            ddlEmgType.DataTextField = "EmergencyType";
            ddlEmgType.DataSource = GetDisasterTypes();
            ddlEmgType.DataBind();

            ListItem item = new ListItem("Select Disaster Type", "0");
            ddlEmgType.Items.Insert(0, item);
        }

        private DataTable GetDisasterTypes()
        {
            return DBContext.GetData("GetAllDisasterTypes");
        }

        private void PopulateLocations()
        {
            ddlLocations.DataValueField = "LocationId";
            ddlLocations.DataTextField = "LocationName";

            ddlLocations.DataSource = ROWCACommon.GetLocations(this.User);
            ddlLocations.DataBind();

            ListItem item = new ListItem("Select Country", "0");
            ddlLocations.Items.Insert(0, item);
        }

        protected void btnAddEmergency_Click(object sender, EventArgs e)
        {
            ClearPopupControls();
            mpeAddOrg.Show();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SaveEmergency();
            LoadEmergencies();
            mpeAddOrg.Hide();
            ClearPopupControls();
        }

        private void ClearPopupControls()
        {
            if (ddlEmgType.Items.Count > 0)
            {
                ddlEmgType.SelectedIndex = 0;
            }

            if (ddlLocations.Items.Count > 0)
            {
                ddlLocations.SelectedIndex = 0;
            }

            hfLocEmgId.Value = txtEmgName.Text = "";
        }

        private void SaveEmergency()
        {
            int emgTyepId = 0;
            int.TryParse(ddlEmgType.SelectedValue, out emgTyepId);

            int locId = 0;
            int.TryParse(ddlLocations.SelectedValue, out locId);

            string emgName = txtEmgName.Text.Trim();
            Guid userId = ROWCACommon.GetCurrentUserId();

            if (!string.IsNullOrEmpty(hfLocEmgId.Value))
            {
                int emgLocId = Convert.ToInt32(hfLocEmgId.Value);
                DBContext.Update("UpdateEmergency", new object[] { emgLocId, emgTyepId, emgName, locId, userId, DBNull.Value });
            }
            else
            {
                DBContext.Add("InsertEmergency", new object[] { emgTyepId, emgName, locId, userId, DBNull.Value });
            }
        }

        enum LocationTypes
        {
            Country = 2,
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "EmergencyListing", this.User);
        }
    }
}