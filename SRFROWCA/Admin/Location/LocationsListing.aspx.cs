using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;
using SRFROWCA.Locations;

namespace SRFROWCA.Admin
{
    public partial class LocationsListing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.User.Identity.IsAuthenticated || !this.User.IsInRole("Admin"))
            {
                Response.Redirect("~/Default.aspx");
            }

            LoadLocations();
        }

        private DataTable GetLocations()
        {
            return DBContext.GetData("GetAllLocations");
        }

        private void LoadLocations()
        {
            radgridLocations.DataSource = GetLocations();
            radgridLocations.DataBind();
        }

        protected void radgridLocations_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            radgridLocations.DataSource = GetLocations();
        }

        protected void radgridLocations_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                if (e.CommandName == "MapLocation")
                {
                    // Put location values in Session. GetLocationValues funcation will
                    // return Dictionary<string,string> object.
                    Session["ArtemLocationValues"] = GetLocationValues(e.Item);

                    // Got to View/Edit page.
                    Response.Redirect("ViewLocation.aspx");
                }

                if (e.CommandName == "DeleteLocation")
                {
                    int locationId = 0;
                    int.TryParse(e.CommandArgument.ToString(), out locationId);

                    if (locationId > 0)
                    {
                        DeleteLocation(locationId);
                    }
                }
            }
        }

        private void DeleteLocation(int locationId)
        {
            if (!LocationBeingUsedInReports(locationId))
            {
                Delete(locationId);
                LoadLocations();
            }
            else
            {
                Session["LocationIdOld"] = locationId;
                mpeAddActivity.Show();
            }
        }

        private bool LocationBeingUsedInReports(int locationId)
        {
            DataTable dt = DBContext.GetData("", new object[] { locationId });
            if (dt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        private void Delete(int locationId)
        {
            DBContext.Delete("Usp_Location_DeleteLocation", new object[] { locationId, DBNull.Value });
        }

        private string GetItemControlValue(GridItem item, string control)
        {
            Label lblControl = item.FindControl(control) as Label;
            if (lblControl != null)
            {
                return lblControl.Text;
            }

            return "";
        }

        // Get control value of grid of current item.
        // Put these values in dictionary and return dictionary object.
        private Dictionary<string, string> GetLocationValues(GridItem item)
        {
            Dictionary<string, string> locationValues = new Dictionary<string, string>();

            locationValues.Add("LocationId", GetItemControlValue(item, "lblLocationId"));
            locationValues.Add("FullLocationName", GetItemControlValue(item, "lblFullLocationName"));
            locationValues.Add("Lat", GetItemControlValue(item, "lblLat"));
            locationValues.Add("Lng", GetItemControlValue(item, "lblLng"));
            locationValues.Add("LocationType", GetItemControlValue(item, "lblLocationType"));

            return locationValues;
        }

        // Change controls status (Enable/Disable, Show/Hide) on the basis of selected item
        protected void ddlLocationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int locationType = 0;
            int.TryParse(ddlLocationType.SelectedItem.Value, out locationType);

            CommonLocations.PopulateProvincesDropDown(ddlProvince);
        }

        protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            int parentId = 0;
            int.TryParse(ddlProvince.SelectedItem.Value, out parentId);

            CommonLocations.PopulateLocationDropDowns(parentId, ddlDistrict);
        }

        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            int parentId = 0;

            int.TryParse(ddlDistrict.SelectedItem.Value, out parentId);
            CommonLocations.PopulateLocationDropDowns(parentId, ddlTehsil);
        }

        protected void ddlTehsil_SelectedIndexChanged(object sender, EventArgs e)
        {
            int parentId = 0;
            int.TryParse(ddlTehsil.SelectedItem.Value, out parentId);

            CommonLocations.PopulateLocationDropDowns(parentId, ddlUC);
        }

        protected void ddlUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            int parentId = 0;
            int.TryParse(ddlUC.SelectedItem.Value, out parentId);

            CommonLocations.PopulateLocationDropDowns(parentId, ddlVillage);
        }

        protected void btnAddSP_Click(object sender, EventArgs e)
        {

        }

        public void ConfigureExport()
        {
            radgridLocations.ExportSettings.ExportOnlyData = true;
            radgridLocations.ExportSettings.IgnorePaging = true;
            radgridLocations.ExportSettings.OpenInNewWindow = true;
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            ConfigureExport();
            radgridLocations.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, EventArgs e)
        {
            ConfigureExport();
            radgridLocations.MasterTableView.ExportToWord();
        }

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            ConfigureExport();
            radgridLocations.MasterTableView.ExportToCSV();
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            ConfigureExport();
            radgridLocations.MasterTableView.ExportToPdf();
        }

        protected void btnAddLocation_Click(object sender, EventArgs e)
        {
            //mpeAddActivity.Show();
            Response.Redirect("AddLocation.aspx");
        }

        protected void btnUpdateLocation_Click(object sender, EventArgs e)
        {
            // Get Selected Location Id on the basis of type.
            int locationIdNew = GetSelectedLocationId();

            if (locationIdNew > 0)
            {
                int locationIdOld = 0;

                try
                {
                    if (Session["LocationIdOld"] != null)
                    {
                        locationIdOld = Convert.ToInt32(Session["LocationIdOld"].ToString());
                    }

                    UpdateReportsWithLocation(locationIdOld, locationIdNew);
                    Delete(locationIdOld);
                    //mpeAddActivity.Hide();
                    LoadLocations();
                }
                finally
                {
                    Session["LocationIdOld"] = null;
                }
            }
        }

        private void UpdateReportsWithLocation(int locationIdOld, int locationIdNew)
        {
            DBContext.Add("Usp_Location_UpdateReportsWithLocation", new object[] { locationIdOld, locationIdNew, DBNull.Value });
        }

        private int GetSelectedLocationId()
        {
            int locationId = 0;

            locationId = GetSelectedItemId(ddlVillage);
            if (locationId > 0) return locationId;

            locationId = GetSelectedItemId(ddlUC);
            if (locationId > 0) return locationId;

            locationId = GetSelectedItemId(ddlTehsil);
            if (locationId > 0) return locationId;

            locationId = GetSelectedItemId(ddlDistrict);
            if (locationId > 0) return locationId;

            return locationId;
        }

        private int GetSelectedItemId(DropDownList ddl)
        {
            int id = 0;
            if (ddl.SelectedItem != null)
            {
                int.TryParse(ddl.SelectedItem.Value, out id);
            }

            return id;
        }

        private enum FetchStatus
        {
            Success = 1
        }
    }
}