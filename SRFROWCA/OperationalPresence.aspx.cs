using System;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using System.Data;
using BusinessLogic;

namespace SRFROWCA
{
    public partial class OperationalPresence : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateControls();
            LoadContacts();
        }

        private void PopulateControls()
        {
            PopulateOrganizations();
            PopulateLocations();
        }

        private void PopulateOrganizations()
        {
            int? nullVal = null;
            DataTable dt = RC.GetOrganizations(nullVal);
            UI.FillOrganizations(ddlOrganizations, dt);
            

            if (ddlOrganizations.Items.Count > 0)
            {
                ListItem item = new ListItem("All", "0");
                ddlOrganizations.Items.Insert(0, item);
            }
        }

        // Populate Locations drop down
        private void PopulateLocations()
        {
            PopulateCountry();
        }

        private void PopulateCountry()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);

            ListItem item = new ListItem("All", "0");
            ddlCountry.Items.Insert(0, item);
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadContacts();
        }

        protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadContacts();
        }

        protected void gvPresence_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetContacts();
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvPresence.DataSource = dt;
                gvPresence.DataBind();
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

        protected void gvPresence_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPresence.PageIndex = e.NewPageIndex;
            gvPresence.SelectedIndex = -1;
            LoadContacts();
        }

        private void LoadContacts()
        {
            gvPresence.DataSource = GetContacts();
            gvPresence.DataBind();
        }

        private DataTable GetContacts()
        {
            int? orgId = RC.GetSelectedIntVal(ddlOrganizations);
            if (orgId == 0)
            {
                orgId = (int?)null;
            }

            int? emgLocId = null;//  UserInfo.EmergencyCountry == 0 ? (int?)null : UserInfo.EmergencyCountry;
            int countryId = RC.GetSelectedIntVal(ddlCountry);
            emgLocId = countryId == 0 ? (int?)null : countryId;

            return DBContext.GetData("OperationalPresence2015", new object[] { emgLocId, orgId });
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {
            DataTable dt = GetContacts();
            string fileName = "Operational_Presence";
            ExportUtility.ExportGridView(dt, fileName, Response);
        }
    }
}