using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Admin.Location
{
    public partial class LocationsList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLocations();
                LoadLocationtypes();
            }
        }
        private void LoadLocationtypes()
        {
            ddlType.DataSource = DBContext.GetData("GetLocationTypes");
            ddlType.DataTextField = "LocationType";
            ddlType.DataValueField = "LocationTypeId";
            ddlType.DataBind();
        }

        private void LoadLocations()
        {           
            gvLocation.DataSource = GetLocationData();
            gvLocation.DataBind();
        }
        private DataTable GetLocationData()
        {
            string locationName = string.IsNullOrEmpty(txtLocationName.Text) ? null : txtLocationName.Text;
            int? type = ddlType.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlType.SelectedValue);
            return DBContext.GetData("GetLocations", new object[] { locationName, type });
        }

        protected void gvLocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLocation.PageIndex = e.NewPageIndex;
            gvLocation.SelectedIndex = -1;
            LoadLocations();
        }

        protected void gvLocation_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetLocationData();
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvLocation.DataSource = dt;
                gvLocation.DataBind();
            }
        }
        protected void gvLocation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                Response.Redirect(string.Format("{0}/Admin/Location/AddNewLocation.aspx?id={1}", Master.BaseURL, Utils.EncryptQueryString(e.CommandArgument.ToString())), true);
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadLocations();
        }


    }
}