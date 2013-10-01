using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Locations;
using BusinessLogic;
using System.Data;
using Artem.Google;
using Artem.Google.UI;
using Artem;
using System.Web.Security;

namespace SRFROWCA.Admin.Location
{
    public partial class AddLocation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.User.Identity.IsAuthenticated || !this.User.IsInRole("Admin"))
            {
                Response.Redirect("~/Default.aspx");
            }

            // If page is posted back then no need to run code in load event.
            if (IsPostBack) return;

            // If Session["ArtemLocationValues"] is null then no need to go further because this session
            // has all the information of location.
            if (Session["ArtemLocationValues"] == null) return;

            MapLocation();
            CommonLocations.PopulateProvincesDropDown(ddlProvince);
        }

        private void MapLocation()
        {
            // Get location values from session.
            Dictionary<string, string> locValues = Session["ArtemLocationValues"] as Dictionary<string, string>;
            if (locValues != null)
            {
                // Set location on map either using lat/lng or name of location.
                LoadLocationOnMap(locValues);

                string locationType = locValues["LocationType"].ToString();
                SetLocationZoom(locationType);
            }
        }

        //private void PopulateProvincesDropDown()
        //{
        //    structResult provinces = _dataContext.GetAllRecords(ConnectionString, "Usp_Common_GetAllProvicesAndTerretories", "Locations");

        //    if (provinces.intCode == (int)FetchStatus.Success)
        //    {
        //        if (provinces.dstResult.Tables["Locations"].Rows.Count > 0)
        //        {
        //            ddlProvince.DataValueField = "LocationId";
        //            ddlProvince.DataTextField = "LocationName";

        //            ddlProvince.DataSource = provinces.dstResult.Tables["Locations"];
        //            ddlProvince.DataBind();

        //            ddlProvince.Items.Insert(0, new ListItem("Select Province", "0"));
        //            ddlProvince.SelectedIndex = 0;

        //            //ddlProvince_SelectedIndexChanged(null, null);
        //        }
        //    }
        //}

        //private void PopulateLocationDropDowns(int parentId, DropDownList ddl)
        //{
        //    structResult tehsils = _dataContext.GetAllRecordsByID(ConnectionString, "Usp_Common_GetAllLocationsOnParentLocation", "Locations", new object[] { parentId });

        //    if (tehsils.intCode == (int)FetchStatus.Success)
        //    {
        //        if (tehsils.dstResult.Tables["Locations"].Rows.Count > 0)
        //        {
        //            ddl.DataValueField = "LocationId";
        //            ddl.DataTextField = "LocationName";

        //            ddl.DataSource = tehsils.dstResult.Tables["Locations"];
        //            ddl.DataBind();
        //        }
        //    }
        //}

        // Change controls status (Enable/Disable, Show/Hide) on the basis of selected item
        protected void ddlLocationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var a = gmapView.Markers.Count;

            int locationType = 0;
            int.TryParse(ddlLocationType.SelectedItem.Value, out locationType);

            EnableRelavantLocaitonControls(locationType);
            CommonLocations.PopulateProvincesDropDown(ddlProvince);

            //MapLocation();
        }

        protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            int parentId = 0;
            int.TryParse(ddlProvince.SelectedItem.Value, out parentId);

            CommonLocations.PopulateLocationDropDowns(parentId, ddlDistrict);

            MapLocation(ddlProvince, "Province");
        }

        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            int parentId = 0;

            int.TryParse(ddlDistrict.SelectedItem.Value, out parentId);
            CommonLocations.PopulateLocationDropDowns(parentId, ddlTehsil);
            MapLocation(ddlProvince, "District");
        }

        protected void ddlTehsil_SelectedIndexChanged(object sender, EventArgs e)
        {
            int parentId = 0;
            int.TryParse(ddlTehsil.SelectedItem.Value, out parentId);

            CommonLocations.PopulateLocationDropDowns(parentId, ddlUC);
            MapLocation(ddlTehsil, "Sub-District");
        }

        protected void ddlUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            int parentId = 0;
            int.TryParse(ddlUC.SelectedItem.Value, out parentId);

            CommonLocations.PopulateLocationDropDowns(parentId, ddlVillage);
            MapLocation(ddlUC, "UC");
        }

        protected void btnAddLocation_Click(object sender, EventArgs e)
        {
            int i = gmapView.Markers.Count;

            int locationType = GetSelectedLocationType();
            if (locationType > 0)
            {
                SaveLocation(locationType);
            }
        }

        private void SaveLocation(int locationType)
        {
            string locationName = "";
            int? parentId = null;

            if (locationType == 2)
            {
                locationName = txtProvince.Text.Trim();
                parentId = 1;
            }
            else if (locationType == 3)
            {
                locationName = txtDistrict.Text.Trim();
                parentId = Convert.ToInt32(ddlProvince.SelectedItem.Value);
            }
            else if (locationType == 4)
            {
                locationName = txtTehsil.Text.Trim();
                parentId = Convert.ToInt32(ddlDistrict.SelectedItem.Value);
            }
            else if (locationType == 5)
            {
                locationName = txtUC.Text.Trim();
                parentId = Convert.ToInt32(ddlTehsil.SelectedItem.Value);
            }
            else if (locationType == 6)
            {
                locationName = txtVillage.Text.Trim();
                parentId = Convert.ToInt32(ddlUC.SelectedItem.Value);
            }

            double? lat = null;
            double? lng = null;

            int i = gmapView.Markers.Count;
            if (i > 0)
            {
                Marker m = gmapView.Markers[0];
                lat = m.Position.Latitude;
                lng = m.Position.Longitude;
            }

            int? isAccurate = 0;
            Guid loginUserId = (Guid)Membership.GetUser().ProviderUserKey;

            DBContext.Add("InsertLocation", new object[] { locationType, parentId, locationName, lat, lng, isAccurate, loginUserId, DBNull.Value });
        }


        private int GetSelectedLocationType()
        {
            int id = 0;
            int.TryParse(ddlLocationType.SelectedItem.Value, out id);
            return id;
        }

        private void EnableRelavantLocaitonControls(int locationType)
        {
            switch (locationType)
            {
                case 0:
                    ControlsDefaultValue();
                    DefaultControlsPosition();
                    break;
                case 2:
                    ControlsDefaultValue();
                    EnableControlsForProvince();
                    break;
                case 3:
                    ControlsDefaultValue();
                    EnableControlsForDistrict();
                    break;
                case 4:
                    ControlsDefaultValue();
                    EnableControlsForTehsil();
                    break;
                case 5:
                    ControlsDefaultValue();
                    EnableControlsForUC();
                    break;
                case 6:
                    ControlsDefaultValue();
                    EnableControlsForVillage();
                    break;
                default:
                    ControlsDefaultValue();
                    DefaultControlsPosition();
                    break;
            }
        }

        private void ControlsDefaultValue()
        {
            ddlDistrict.DataSource = ddlTehsil.DataSource = ddlUC.DataSource = ddlVillage.DataSource = new DataTable();
            ddlDistrict.DataBind();
            ddlTehsil.DataBind();
            ddlUC.DataBind();
            ddlVillage.DataBind();

            txtProvince.Text = txtDistrict.Text = txtTehsil.Text = txtUC.Text = txtVillage.Text = "";
        }

        // Show all drop downs.
        // Hide all textboxes and disable all controls.
        private void DefaultControlsPosition()
        {
            ddlProvince.Enabled = false;
            ddlProvince.Visible = true;
            txtProvince.Enabled = false;
            txtProvince.Visible = false;

            ddlDistrict.Enabled = false;
            ddlDistrict.Visible = true;
            ddlDistrict.Enabled = false;
            txtDistrict.Visible = false;

            ddlTehsil.Enabled = false;
            ddlTehsil.Visible = true;
            txtTehsil.Enabled = false;
            txtTehsil.Visible = false;

            ddlUC.Enabled = false;
            ddlUC.Visible = true;
            txtUC.Enabled = false;
            txtUC.Visible = false;

            ddlVillage.Enabled = false;
            ddlVillage.Visible = true;
            txtVillage.Enabled = false;
            txtVillage.Visible = false;
        }

        // Show all drop downs except province drop down and disable all.
        // Show and enable TextBox for province instead of drop down so user can 
        // enter name of province.
        private void EnableControlsForProvince()
        {
            ddlProvince.Enabled = false;
            ddlProvince.Visible = false;
            txtProvince.Enabled = true;
            txtProvince.Visible = true;

            ddlDistrict.Enabled = false;
            ddlDistrict.Visible = true;
            ddlDistrict.Enabled = false;
            txtDistrict.Visible = false;

            ddlTehsil.Enabled = false;
            ddlTehsil.Visible = true;
            txtTehsil.Enabled = false;
            txtTehsil.Visible = false;

            ddlUC.Enabled = false;
            ddlUC.Visible = true;
            txtUC.Enabled = false;
            txtUC.Visible = false;

            ddlVillage.Enabled = false;
            ddlVillage.Visible = true;
            txtVillage.Enabled = false;
            txtVillage.Visible = false;
        }

        // Show and Enable 'Province' drop down.
        // Hide and Disable 'District' drop down.
        // Show and Enable 'District' text box.
        // Show and Disable all other drop downs beneath District.
        private void EnableControlsForDistrict()
        {
            ddlProvince.Enabled = true;
            ddlProvince.Visible = true;
            txtProvince.Enabled = false;
            txtProvince.Visible = false;

            ddlDistrict.Enabled = false;
            ddlDistrict.Visible = false;
            txtDistrict.Enabled = true;
            txtDistrict.Visible = true;

            ddlTehsil.Enabled = false;
            ddlTehsil.Visible = true;
            txtTehsil.Enabled = false;
            txtTehsil.Visible = false;

            ddlUC.Enabled = false;
            ddlUC.Visible = true;
            txtUC.Enabled = false;
            txtUC.Visible = false;

            ddlVillage.Enabled = false;
            ddlVillage.Visible = true;
            txtVillage.Enabled = false;
            txtVillage.Visible = false;
        }

        private void EnableControlsForTehsil()
        {
            ddlProvince.Enabled = true;
            ddlProvince.Visible = true;
            txtProvince.Enabled = false;
            txtProvince.Visible = false;

            ddlDistrict.Enabled = true;
            ddlDistrict.Visible = true;
            txtDistrict.Enabled = false;
            txtDistrict.Visible = false;

            ddlTehsil.Enabled = false;
            ddlTehsil.Visible = false;
            txtTehsil.Enabled = true;
            txtTehsil.Visible = true;

            ddlUC.Enabled = false;
            ddlUC.Visible = true;
            txtUC.Enabled = false;
            txtUC.Visible = false;

            ddlVillage.Enabled = false;
            ddlVillage.Visible = true;
            txtVillage.Enabled = false;
            txtVillage.Visible = false;
        }

        private void EnableControlsForUC()
        {
            ddlProvince.Enabled = true;
            ddlProvince.Visible = true;
            txtProvince.Enabled = false;
            txtProvince.Visible = false;

            ddlDistrict.Enabled = true;
            ddlDistrict.Visible = true;
            txtDistrict.Enabled = false;
            txtDistrict.Visible = false;

            ddlTehsil.Enabled = true;
            ddlTehsil.Visible = true;
            txtTehsil.Enabled = false;
            txtTehsil.Visible = false;

            ddlUC.Enabled = false;
            ddlUC.Visible = false;
            txtUC.Enabled = true;
            txtUC.Visible = true;

            ddlVillage.Enabled = false;
            ddlVillage.Visible = true;
            txtVillage.Enabled = false;
            txtVillage.Visible = false;
        }

        private void EnableControlsForVillage()
        {
            ddlProvince.Enabled = true;
            ddlProvince.Visible = true;
            txtProvince.Enabled = false;
            txtProvince.Visible = false;

            ddlDistrict.Enabled = true;
            ddlDistrict.Visible = true;
            txtDistrict.Enabled = false;
            txtDistrict.Visible = false;

            ddlTehsil.Enabled = true;
            ddlTehsil.Visible = true;
            txtTehsil.Enabled = false;
            txtTehsil.Visible = false;

            ddlUC.Enabled = true;
            ddlUC.Visible = true;
            txtUC.Enabled = false;
            txtUC.Visible = false;

            ddlVillage.Enabled = false;
            ddlVillage.Visible = false;
            txtVillage.Enabled = true;
            txtVillage.Visible = true;
        }

        private void MapLocation(DropDownList ddl, string type)
        {
            string fullLocationName = "";

            if (Convert.ToInt32(ddl.SelectedItem.Value) > 0)
            {

                if (type.Equals("Province"))
                {
                    fullLocationName = string.Format("{0}, West Africa", ddl.SelectedItem.Text);
                    SetLocationZoom("Country");
                }
                else if (type.Equals("District"))
                {
                    fullLocationName = string.Format("{0} {1}", ddl.SelectedItem.Text, ddlDistrict.SelectedItem.Text);
                    SetLocationZoom("Governorate");
                }
                else if (type.Equals("Sub-District"))
                {
                    fullLocationName = string.Format("{0}, {1}, {2}", ddlProvince.SelectedItem.Text, ddlDistrict.SelectedItem.Text, ddl.SelectedItem.Text);
                    SetLocationZoom("District");
                }
                else
                {
                    fullLocationName = string.Format("{0}, {1}, {2}, {3}", ddlProvince.SelectedItem.Text, ddlDistrict.SelectedItem.Text, ddlTehsil.SelectedItem.Text, ddl.SelectedItem.Text);
                    SetLocationZoom("Sub-District");
                }


                Marker marker = new Marker();
                marker.Clickable = true;
                marker.Draggable = true;
                marker.Title = fullLocationName;
                gmapView.Address = fullLocationName;
                marker.Address = fullLocationName;
                gmapView.Markers.Add(marker);
            }
        }

        // Set location on map either using lat/lng or name of location.
        private void LoadLocationOnMap(Dictionary<string, string> locValues)
        {
            string fullLocationName = locValues["FullLocationName"].ToString();
            string lat = locValues["Lat"].ToString();
            string lng = locValues["Lng"].ToString();

            Marker marker = new Marker();
            marker.Clickable = true;
            marker.Draggable = true;
            marker.Title = fullLocationName;

            // If both lat and lng values exists then load location on map using lat/lng.
            if (lat != "" && lng != "")
            {
                gmapView.Latitude = Convert.ToDouble(lat);
                gmapView.Longitude = Convert.ToDouble(lng);

                marker.Position.Latitude = Convert.ToDouble(lat);
                marker.Position.Longitude = Convert.ToDouble(lng);
                gmapView.Markers.Add(marker);
            }
            else
            {
                gmapView.Address = fullLocationName;
                marker.Address = fullLocationName;
                gmapView.Markers.Add(marker);
            }
        }

        // Set zoom of location with respect to location type i.e. Province, District, Tehsil etc.
        private void SetLocationZoom(string locationType)
        {
            switch (locationType)
            {
                case "Country":
                    gmapView.Zoom = 6;
                    break;
                case "Governorate":
                    gmapView.Zoom = 7;
                    break;
                case "District":
                    gmapView.Zoom = 12;
                    break;
                case "Sub-District":
                    gmapView.Zoom = 14;
                    break;
                case "Village":
                    gmapView.Zoom = 14;
                    break;
                default:
                    gmapView.Zoom = 7;
                    break;
            }
        }

        private string ConnectionString
        {
            get { return "LIVE"; }
        }

        private enum FetchStatus
        {
            Success = 1
        }
    }
}