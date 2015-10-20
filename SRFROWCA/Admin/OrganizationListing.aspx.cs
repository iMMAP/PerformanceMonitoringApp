using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Admin
{
    public partial class OrganizationListing : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            //this.Form.DefaultButton = this.btnAdd.UniqueID;

            // Populate grid with organizations
            PopulateOrganizations();

            // Populate org types in modal popup
            PopulateOrganizationTypes();
        }

        // Add delete confirmation message with all delete buttons.
        protected void gvOrgs_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvOrgs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // If user click on Delete button.
            if (e.CommandName == "DeleteOrg")
            {
                int orgId = Convert.ToInt32(e.CommandArgument);

                // Check if any IP has reported on this project. If so then do not delete it.
                if (AnyUserExistsInOrganization(orgId))
                {
                    lblMessage.Text = "Organization cannot be deleted! Users are registered under this organization.";
                    lblMessage.CssClass = "error-message";
                    lblMessage.Visible = true;

                    return;
                }

                DeleteOrganization(orgId);
                PopulateOrganizations();
            }

            // Edit Project.
            if (e.CommandName == "EditOrg")
            {
                hfOrgId.Value = e.CommandArgument.ToString();

                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;

                Label lblOrgName = row.FindControl("lblOrgName") as Label;
                if (lblOrgName != null)
                {
                    txtOrgName.Text = lblOrgName.Text;
                }

                Label lblAcronym = row.FindControl("lblOrgAcronym") as Label;
                if (lblAcronym != null)
                {
                    txtOrgAcronym.Text = lblAcronym.Text;
                }

                Label lblOrgTypeId = row.FindControl("lblOrgTypeId") as Label;
                if (lblOrgTypeId != null)
                {
                    ddlOrgTypes.SelectedValue = lblOrgTypeId.Text;
                }

                mpeAddOrg.Show();
            }
        }

        protected void gvOrgs_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Retrieve the table from the session object.
            DataTable dt = GetOrganizations();

            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvOrgs.DataSource = dt;
                gvOrgs.DataBind();
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

        // Populate grid with organizations
        private void PopulateOrganizations()
        {
            gvOrgs.DataSource = GetOrganizations();
            gvOrgs.DataBind();
        }

        private DataTable GetOrganizations()
        {
            return DBContext.GetData("GetOrganizations");
        }

        private void DeleteOrganization(int orgId)
        {
            DBContext.Delete("DeleteOrganization", new object[] { orgId, DBNull.Value });
        }

        private bool AnyUserExistsInOrganization(int orgId)
        {
            DataTable dt = DBContext.GetData("IsOrganizationHasUsers", new object[] { orgId });
            return dt.Rows.Count > 0;
        }

        #region Popup Controls

        private void PopulateOrganizationTypes()
        {
            ddlOrgTypes.DataValueField = "OrganizationTypeId";
            ddlOrgTypes.DataTextField = "OrganizationType";
            ddlOrgTypes.DataSource = GetOrganizationTypes();
            ddlOrgTypes.DataBind();

            ListItem item = new ListItem("Select Organization Type", "0");
            ddlOrgTypes.Items.Insert(0, item);
        }

        private object GetOrganizationTypes()
        {
            return DBContext.GetData("GetOrganizationTypes");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SaveOrganization();
            PopulateOrganizations();
            mpeAddOrg.Hide();
            ClearPopupControls();
        }

        private void ClearPopupControls()
        {
            if (ddlOrgTypes.Items.Count > 0)
            {
                ddlOrgTypes.SelectedIndex = 0;
            }

            hfOrgId.Value = txtOrgAcronym.Text = txtOrgName.Text = "";
        }


        private void SaveOrganization()
        {
            int orgTypeId = 0;
            int.TryParse(ddlOrgTypes.SelectedValue, out orgTypeId);
            string orgName = txtOrgName.Text.Trim();
            string orgAcronym = txtOrgAcronym.Text.Trim();
            Guid userId = RC.GetCurrentUserId;

            if (!string.IsNullOrEmpty(hfOrgId.Value))
            {
                int orgId = Convert.ToInt32(hfOrgId.Value);
                DBContext.Update("UpdateOrganization", new object[] { orgId, orgTypeId, orgName, orgAcronym, userId, DBNull.Value });
            }
            else
            {
                DBContext.Add("InsertOrganization", new object[] { orgTypeId, orgName, orgAcronym, userId, DBNull.Value });
            }
        }

        #endregion

        protected void gvOrgs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrgs.PageIndex = e.NewPageIndex;
            gvOrgs.SelectedIndex = -1;
            PopulateOrganizations();
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            string fileName = "3WPMorgs";
            string fileExtention = ".xls";
            gvOrgs.AllowSorting = false;
            ExportUtility.ExportGridView(gvOrgs, fileName, fileExtention, Response);
            gvOrgs.AllowSorting = false;
        }

        public override void VerifyRenderingInServerForm(Control control) { }

        //public override void VerifyRenderingInServerForm(Control control)
        //{
        //    /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
        //       server control at run time. */
        //}

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}