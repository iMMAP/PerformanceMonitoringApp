using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;
using System.Web.Security;

namespace SRFROWCA.Admin.Office
{
    public partial class OfficeListing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            LoadOffices();
            PopulateCountries();
            //PopulateLocations();
            PopulateOrganizations();
        }

        // Add delete confirmation message with all delete buttons.
        protected void gvOffice_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvOffice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // If user click on Delete button.
            if (e.CommandName == "DeleteOffice")
            {
                int officeId = Convert.ToInt32(e.CommandArgument);

                // Check if any IP has reported on this project. If so then do not delete it.
                if (!OfficeIsBeingUsedInReports(officeId))
                {
                    lblMessage.Text = "Office cannot be deleted! It is being used in reports.";
                    lblMessage.CssClass = "error-message";
                    lblMessage.Visible = true;

                    return;
                }

                DeleteOffice(officeId);
                LoadOffices();
            }

            // Edit Project.
            if (e.CommandName == "EditOffice")
            {
                hfOfficeId.Value = e.CommandArgument.ToString();

                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;
                Label lblOrganizaitonId = row.FindControl("lblOrganizaitonId") as Label;
                if (lblOrganizaitonId != null)
                {
                    ddlOrganizations.SelectedValue = lblOrganizaitonId.Text;
                }

                Label lblLocationId = row.FindControl("lblLocationId") as Label;
                if (lblLocationId != null)
                {
                    ddlLocations.SelectedValue = lblLocationId.Text;
                }

                txtOfficeName.Text = row.Cells[1].Text;
                mpeAddOrg.Show();
            }
        }

        private bool OfficeIsBeingUsedInReports(int officeId)
        {
            DataTable dt = DBContext.GetData("GetIsOfficeBeingUsed", new object[] { officeId });
            return !(dt.Rows.Count > 0);
        }

        private void DeleteOffice(int officeId)
        {
            DBContext.Delete("DeleteOffice", new object[] { officeId, DBNull.Value });
        }

        protected void gvOffice_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Retrieve the table from the session object.
            DataTable dt = GetOffices();
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvOffice.DataSource = dt;
                gvOffice.DataBind();
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

        private void LoadOffices()
        {
            gvOffice.DataSource = GetOffices();
            gvOffice.DataBind();
        }

        private DataTable GetOffices()
        {
            return DBContext.GetData("GetAllOffices");
        }

        private void PopulateCountries()
        {
            ddlCountry.DataValueField = "LocationId";
            ddlCountry.DataTextField = "LocationName";

            int locationType = (int)LocationTypes.Country;
            ddlCountry.DataSource = GetLocations(locationType);
            ddlCountry.DataBind();

            ListItem item = new ListItem("Select Country", "0");
            ddlCountry.Items.Insert(0, item);

            ddlCountry.SelectedIndexChanged += new EventHandler(ddlCountry_SelectedIndexChanged);
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            int countryId = 0;
            int.TryParse(ddlCountry.SelectedValue, out countryId);
            PopulateLocations(countryId);
        }

        private void PopulateLocations(int countryId)
        {
            ddlLocations.DataValueField = "LocationId";
            ddlLocations.DataTextField = "LocationName";

            int locationType = (int)LocationTypes.Admin1;
            ddlLocations.DataSource = GetLocationsOnParent(countryId, locationType);
            ddlLocations.DataBind();

            ListItem item = new ListItem("Select Location", "0");
            ddlLocations.Items.Insert(0, item);
        }

        private object GetLocationsOnParent(int countryId, int locationType)
        {
            return DBContext.GetData("GetLocationOnParentAndType", new object[] { countryId, locationType });
        }

        private object GetLocations(int locationType)
        {
            return DBContext.GetData("GetLocationOnType", new object[] { locationType });
        }

        private void PopulateOrganizations()
        {
            ddlOrganizations.DataValueField = "OrganizationId";
            ddlOrganizations.DataTextField = "OrganizationAcronym";
            int? orgId = null;
            ddlOrganizations.DataSource = GetOrganizations(orgId);
            ddlOrganizations.DataBind();

            ListItem item = new ListItem("Select Organization", "0");
            ddlOrganizations.Items.Insert(0, item);
        }
        private object GetOrganizations(int? orgId)
        {
            return DBContext.GetData("GetOrganizations", new object[] { orgId });
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!IsValidOffice())
            {
                mpeAddOrg.Show();
            }
            else
            {
                SaveOffice();
                LoadOffices();
                mpeAddOrg.Hide();
                ClearPopupControls();
            }
        }

        private bool IsValidOffice()
        {
            int locId = 0;
            int.TryParse(ddlLocations.SelectedValue, out locId);

            if (locId == 0)
            {
                lblMessage2.Text = "Please Select Location To Add Office!";
                lblMessage2.CssClass = "error-message";
                lblMessage2.Visible = true;
                return false;
            }
            return true;
        }

        private void ClearPopupControls()
        {
            if (ddlOrganizations.Items.Count > 0)
            {
                ddlOrganizations.SelectedIndex = 0;
            }

            if (ddlLocations.Items.Count > 0)
            {
                ddlLocations.SelectedIndex = 0;
            }

            hfOfficeId.Value = txtOfficeName.Text = "";
        }

        private void SaveOffice()
        {
            int orgId = 0;
            int.TryParse(ddlOrganizations.SelectedValue, out orgId);

            int locId = 0;
            int.TryParse(ddlLocations.SelectedValue, out locId);

            string officeName = txtOfficeName.Text.Trim();
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;

            if (!string.IsNullOrEmpty(hfOfficeId.Value))
            {
                int officeId = Convert.ToInt32(hfOfficeId.Value);
                DBContext.Update("UpdateOffice", new object[] { officeId, orgId, locId, officeName, userId, DBNull.Value });
            }
            else
            {
                DBContext.Add("InsertOffice", new object[] { orgId, locId, officeName, userId, DBNull.Value });
            }
        }

        enum LocationTypes
        {
            Country = 2,
            Admin1 = 3,
        }
    }
}