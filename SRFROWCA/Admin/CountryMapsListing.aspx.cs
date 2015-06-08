using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Admin
{
    public partial class CountryMapsListing : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateLocations();
            LoadMaps();
        }



        // Add delete confirmation message with all delete buttons.
        protected void gvEmergency_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton deleteButton = e.Row.FindControl("btnDelete") as ImageButton;
                if (deleteButton != null)
                {
                    deleteButton.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this Map?')");
                }
            }
        }

        // Execute row commands like Edit, Delete etc. on Grid.
        protected void gvEmergency_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // If user click on Delete button.
            if (e.CommandName == "DeleteMap")
            {
                int Id = Convert.ToInt32(e.CommandArgument);

                // Check if any IP has reported on this project. If so then do not delete it.              

                DeleteMap(Id);
                LoadMaps();
                ShowMessage("Map has been deleted successfully!");
            }

            // Edit Project.
            if (e.CommandName == "EditMap")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("AddEditCountryMaps.aspx?id=" + id);

            }
        }
        protected void btnSearch2_Click(object sender, EventArgs e)
        {
            LoadMaps();
        }

        internal override void BindGridData()
        {
            LoadMaps();
        }



        private void DeleteMap(int Id)
        {
            int uid = 0;
            DBContext.Delete("DeleteCountryMap", new object[] { Id, uid });
        }

        protected void gvEmergency_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Retrieve the table from the session object.
            int? locationId = null;// (int)RC.SelectedSiteLanguageId; 
            DataTable dt = GetCountryMaps();
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

        private void LoadMaps()
        {
            gvEmergency.DataSource = GetCountryMaps();
            gvEmergency.DataBind();
        }
        private DataTable GetCountryMaps()
        {
            string mapTitle = String.IsNullOrEmpty(txtMapTitle.Text.Trim()) ? null : txtMapTitle.Text.Trim();

            int? countryId = null;
            int tempVal = 0;
            tempVal = RC.GetSelectedIntVal(ddlCountry);
            if (tempVal > 0)
                countryId = tempVal;

            int? isPublic = null;
            if (cbIsPublic.Checked)
            {
                isPublic = 1;
            }

            return DBContext.GetData("GetCountryMaps", new object[] { countryId, mapTitle, isPublic, RC.SelectedSiteLanguageId });

        }

        //private void PopulateMonths()
        //{
        //    ddlMonth.DataValueField = "MonthId";
        //    ddlMonth.DataTextField = "MonthName";

        //    ddlMonth.DataSource = GetMonth();
        //    ddlMonth.DataBind();

        //    ddlMonth.Items.Insert(0, new ListItem("All", "0"));
        //}

        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private DataTable GetMapTypes()
        {
            return DBContext.GetData("GetMapTypes");
        }

        private void PopulateLocations()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
        }

        protected void btnAddEmergency_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddEditCountryMaps.aspx");
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 0)
        {
            updMessage.Update();
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }
    }
}