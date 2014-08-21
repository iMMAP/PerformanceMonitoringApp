using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Admin.Location
{
    public partial class AddNewLocation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdowns();
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnId.Value = Utils.DecryptQueryString(Request.QueryString["id"]);
                    PopulateForm();
                }
            }
        }

        private void PopulateForm()
        {
            DataTable dtLocation = DBContext.GetData("GetLocationByID", new object[] { Convert.ToInt32(hdnId.Value) });
            if (dtLocation != null && dtLocation.Rows.Count > 0)
            {
                txtLocationName.Text = dtLocation.Rows[0]["LocationName"].ToString();
                txtPCode.Text = dtLocation.Rows[0]["LocationPCode"].ToString();
                txtPopulation.Text = dtLocation.Rows[0]["EstimatedPopulation"].ToString();
                txtLat.Text = dtLocation.Rows[0]["Latitude"].ToString();
                txtLong.Text = dtLocation.Rows[0]["Longitude"].ToString();
                chkIsAccurate.Checked = Convert.ToBoolean(dtLocation.Rows[0]["IsAccurateLatLng"].ToString());
                ddlType.SelectedValue = dtLocation.Rows[0]["LocationTypeId"].ToString();
                ShowHideReleventDropdowns();
                if (Convert.ToInt32(ddlType.SelectedValue) == (int)RC.LocationTypes.National)
                {
                    ddlRegion.SelectedValue = dtLocation.Rows[0]["RegionID"].ToString();
                    LoadNationals();
                }
                else if (Convert.ToInt32(ddlType.SelectedValue) == (int)RC.LocationTypes.Governorate)
                {
                    ddlRegion.SelectedValue = dtLocation.Rows[0]["RegionID"].ToString();
                    LoadNationals();
                    ddlNational.SelectedValue = dtLocation.Rows[0]["NationalID"].ToString();
                }
                else if (Convert.ToInt32(ddlType.SelectedValue) == (int)RC.LocationTypes.District)
                {
                    ddlRegion.SelectedValue = dtLocation.Rows[0]["RegionID"].ToString();
                    LoadNationals();
                    ddlNational.SelectedValue = dtLocation.Rows[0]["NationalID"].ToString();
                    LoadGovernorate();
                    ddlGovernorate.SelectedValue = dtLocation.Rows[0]["GovernorateID"].ToString();

                }
                else if (Convert.ToInt32(ddlType.SelectedValue) == (int)RC.LocationTypes.Subdistrict)
                {
                    ddlRegion.SelectedValue = dtLocation.Rows[0]["RegionID"].ToString();
                    LoadNationals();
                    ddlNational.SelectedValue = dtLocation.Rows[0]["NationalID"].ToString();
                    LoadGovernorate();
                    ddlGovernorate.SelectedValue = dtLocation.Rows[0]["GovernorateID"].ToString();                    
                    LoadDistrict();
                    ddlDistrict.SelectedValue = dtLocation.Rows[0]["DistrictID"].ToString();
                }
                else if (Convert.ToInt32(ddlType.SelectedValue) == (int)RC.LocationTypes.Village)
                {
                    ddlRegion.SelectedValue = dtLocation.Rows[0]["RegionID"].ToString();
                    LoadNationals();
                    ddlNational.SelectedValue = dtLocation.Rows[0]["NationalID"].ToString();
                    LoadGovernorate();
                    ddlGovernorate.SelectedValue = dtLocation.Rows[0]["GovernorateID"].ToString();
                    LoadDistrict();
                    ddlDistrict.SelectedValue = dtLocation.Rows[0]["DistrictID"].ToString();
                    LoadSubDistrict();
                    ddlSubDistrict.SelectedValue = dtLocation.Rows[0]["SubDistrictID"].ToString();
                }
            }
        }

        private void LoadDropdowns()
        {
            LoadLocationtypes();
            LoadRegions();
        }

        private void LoadLocationtypes()
        {
            ddlType.DataSource = DBContext.GetData("GetLocationTypes");
            ddlType.DataTextField = "LocationType";
            ddlType.DataValueField = "LocationTypeId";
            ddlType.DataBind();
        }
        private void LoadRegions()
        {
            ddlRegion.DataSource = DBContext.GetData("GetLocationsByType", new object[] { (int)RC.LocationTypes.Region, (int?)null});
            ddlRegion.DataValueField = "LocationId";
            ddlRegion.DataTextField = "LocationName";
            ddlRegion.DataBind();
        }
        private void LoadNationals()
        {
            ListItem item = ddlNational.Items[0];
            ddlNational.Items.Clear();
            ddlNational.Items.Add(item);
            ddlNational.DataSource = DBContext.GetData("GetLocationsByType", new object[] { (int)RC.LocationTypes.National, Convert.ToInt32(ddlRegion.SelectedValue)});
            ddlNational.DataValueField = "LocationId";
            ddlNational.DataTextField = "LocationName";
            ddlNational.DataBind();
        }
        private void LoadGovernorate()
        {
            ListItem item = ddlGovernorate.Items[0];
            ddlGovernorate.Items.Clear();
            ddlGovernorate.Items.Add(item);
            ddlGovernorate.DataSource = DBContext.GetData("GetLocationsByType", new object[] { (int)RC.LocationTypes.Governorate, Convert.ToInt32(ddlNational.SelectedValue) });
            ddlGovernorate.DataValueField = "LocationId";
            ddlGovernorate.DataTextField = "LocationName";
            ddlGovernorate.DataBind();
        }
        private void LoadDistrict()
        {
            ListItem item = ddlDistrict.Items[0];
            ddlDistrict.Items.Clear();
            ddlDistrict.Items.Add(item);
            ddlDistrict.DataSource = DBContext.GetData("GetLocationsByType", new object[] { (int)RC.LocationTypes.District, Convert.ToInt32(ddlGovernorate.SelectedValue) });
            ddlDistrict.DataValueField = "LocationId";
            ddlDistrict.DataTextField = "LocationName";
            ddlDistrict.DataBind();
        }
        private void LoadSubDistrict()
        {
            ListItem item = ddlSubDistrict.Items[0];
            ddlSubDistrict.Items.Clear();
            ddlSubDistrict.Items.Add(item);
            ddlSubDistrict.DataSource = DBContext.GetData("GetLocationsByType", new object[] { (int)RC.LocationTypes.Subdistrict, Convert.ToInt32(ddlDistrict.SelectedValue) });
            ddlSubDistrict.DataValueField = "LocationId";
            ddlSubDistrict.DataTextField = "LocationName";
            ddlSubDistrict.DataBind();
        }
        private void LoadVillage()
        {
            ddlVillage.DataSource = DBContext.GetData("GetLocationsByType", new object[] { (int)RC.LocationTypes.Village, Convert.ToInt32(ddlSubDistrict.SelectedValue) });
            ddlVillage.DataValueField = "LocationId";
            ddlVillage.DataTextField = "LocationName";
            ddlVillage.DataBind();
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowHideReleventDropdowns();
        }

        private void ShowHideReleventDropdowns()
        {
            switch (Convert.ToInt32(ddlType.SelectedValue))
            {
                case (int)RC.LocationTypes.Region:
                    trRegion.Visible = false;
                    trNational.Visible = false;
                    trGovernorate.Visible = false;
                    trDistrict.Visible = false;
                    trSubDistrict.Visible = false;
                    trVillage.Visible = false;
                    break;
                case (int)RC.LocationTypes.National:
                    trRegion.Visible = true;
                    trNational.Visible = false;
                    trGovernorate.Visible = false;
                    trDistrict.Visible = false;
                    trSubDistrict.Visible = false;
                    trVillage.Visible = false;
                    break;
                case (int)RC.LocationTypes.Governorate:
                    trRegion.Visible = true;
                    trNational.Visible = true;
                    trGovernorate.Visible = false;
                    trDistrict.Visible = false;
                    trSubDistrict.Visible = false;
                    trVillage.Visible = false;
                    break;
                case (int)RC.LocationTypes.District:
                     trRegion.Visible = true;
                    trNational.Visible = true;
                    trGovernorate.Visible = true;
                     trDistrict.Visible = false;
                    trSubDistrict.Visible = false;
                    trVillage.Visible = false;
                    break;
                case (int)RC.LocationTypes.Subdistrict:
                      trRegion.Visible = true;
                    trNational.Visible = true;
                    trGovernorate.Visible = true;
                     trDistrict.Visible = true;
                     trSubDistrict.Visible = false;
                    trVillage.Visible = false;
                    break;
                case (int)RC.LocationTypes.Village:
                     trRegion.Visible = true;
                    trNational.Visible = true;
                    trGovernorate.Visible = true;
                     trDistrict.Visible = true;
                     trSubDistrict.Visible = true;
                    trVillage.Visible = false;
                    break;
                case (int)RC.LocationTypes.Nonrepresentative:
                      trRegion.Visible = false;
                    trNational.Visible = false;
                    trGovernorate.Visible = false;
                    trDistrict.Visible = false;
                    trSubDistrict.Visible = false;
                    trVillage.Visible = false;
                    break;
                case (int)RC.LocationTypes.Other:
                      trRegion.Visible = false;
                    trNational.Visible = false;
                    trGovernorate.Visible = false;
                    trDistrict.Visible = false;
                    trSubDistrict.Visible = false;
                    trVillage.Visible = false;
                    break;

            }
        }

        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadNationals();
            LoadGovernorate();
            LoadDistrict();
            LoadSubDistrict();
        }

        protected void ddlNational_SelectedIndexChanged(object sender, EventArgs e)
        {           
            LoadGovernorate();
            LoadDistrict();
            LoadSubDistrict();
        }

        protected void ddlGovernorate_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDistrict();
            LoadSubDistrict();
        }

        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSubDistrict();
        }

       

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveLocation();
        }

        private void SaveLocation()
        {
            int? parentId = GetParentId();
            Guid loginUserId = (Guid)Membership.GetUser().ProviderUserKey;
            if (hdnId.Value == "0")
            {
                List<string> arrLocationName = new List<string>();
                arrLocationName.AddRange(hdnLocationName.Value.Split('|'));

                List<string> arrPCode = new List<string>();
                arrPCode.AddRange(hdnPCode.Value.Split('|'));
                if (arrPCode.Count == 0) { arrPCode.Add(""); }

                List<string> arrPopulation = new List<string>();
                arrPopulation.AddRange(hdnPopulation.Value.Split('|'));
                if (arrPopulation.Count == 0) { arrPopulation.Add(""); }

                List<string> arrLat = new List<string>();
                arrLat.AddRange(hdnLat.Value.Split('|'));
                if (arrLat.Count == 0) { arrLat.Add("0"); }

                List<string> arrLong = new List<string>();
                arrLong.AddRange(hdnLong.Value.Split('|'));
                if (arrLong.Count == 0) { arrLong.Add("0"); }

                List<string> arrIsAccurate = new List<string>();
                arrIsAccurate.AddRange(hdnIsAccurate.Value.Split('|'));
                if (arrIsAccurate.Count == 0) { arrIsAccurate.Add("off"); }

                
                
                string LocationsAlreadyExists = string.Empty;
                for (int i = 0; i < arrLocationName.Count; i++)
                {
                    if (DBContext.GetData("CheckDuplicateLocations", new object[] { arrLocationName[i] }).Rows[0][0].ToString() == "1")
                    {
                        LocationsAlreadyExists = string.IsNullOrEmpty(LocationsAlreadyExists) ? arrLocationName[i] : LocationsAlreadyExists + ", " + arrLocationName[i];
                    }


                }
                if (string.IsNullOrEmpty(LocationsAlreadyExists))
                {
                    for (int i = 0; i < arrLocationName.Count; i++)
                    {
                        DBContext.Add("InsertLocation", new object[] {Convert.ToInt32(ddlType.SelectedValue), parentId, arrLocationName[i], 
                String.IsNullOrEmpty(arrLat[i]) ? 0: Convert.ToDouble(arrLat[i]), String.IsNullOrEmpty(arrLong[i]) ? 0: Convert.ToDouble(arrLong[i]),
                arrIsAccurate[i] == "on" ? 1: 0,loginUserId, DBNull.Value,String.IsNullOrEmpty(arrPopulation[i]) ? null :arrPopulation[i],
                String.IsNullOrEmpty(arrPCode[i]) ? null :arrPCode[i] });
                    }
                    RC.ShowMessage(this.Page, typeof(Page), UniqueID, "Location(s) have been added successfully!", RC.NotificationType.Success, false);
                }
                else
                {
                    RC.ShowMessage(this.Page, typeof(Page), UniqueID, "These Location(s) already exists, " + LocationsAlreadyExists, RC.NotificationType.Error, false);
                }

            }
            else
            {
                if (
                    DBContext.Update("UpdateLocation", new object[] {Convert.ToInt32(hdnId.Value), Convert.ToInt32(ddlType.SelectedValue), parentId, txtLocationName.Text, 
                String.IsNullOrEmpty(txtLat.Text) ? 0: Convert.ToDouble(txtLat.Text), String.IsNullOrEmpty(txtLong.Text) ? 0: Convert.ToDouble(txtLong.Text),
                chkIsAccurate.Checked,loginUserId, DBNull.Value,String.IsNullOrEmpty(txtPopulation.Text) ? null :txtPopulation.Text,
                String.IsNullOrEmpty(txtPCode.Text) ? null :txtPCode.Text }) == -1
                )
                {
                    RC.ShowMessage(this.Page, typeof(Page), UniqueID, "Location Name already exists!", RC.NotificationType.Error, false);
                   
                }
                else
                {
                    DBContext.Update("UpdateLocation", new object[] {Convert.ToInt32(hdnId.Value), Convert.ToInt32(ddlType.SelectedValue), parentId, txtLocationName.Text, 
                String.IsNullOrEmpty(txtLat.Text) ? 0: Convert.ToDouble(txtLat.Text), String.IsNullOrEmpty(txtLong.Text) ? 0: Convert.ToDouble(txtLong.Text),
                chkIsAccurate.Checked,loginUserId, DBNull.Value,String.IsNullOrEmpty(txtPopulation.Text) ? null :txtPopulation.Text,
                String.IsNullOrEmpty(txtPCode.Text) ? null :txtPCode.Text });
                    RC.ShowMessage(this.Page, typeof(Page), UniqueID, "Location has been updated successfully!", RC.NotificationType.Success, true);
                }
            }
        }

        private int? GetParentId()
        {
            int? returnValue = null;
            switch (Convert.ToInt32(ddlType.SelectedValue))
            {
                case (int)RC.LocationTypes.Region:                    
                    break;
                case (int)RC.LocationTypes.National:
                    returnValue = Convert.ToInt32(ddlRegion.SelectedValue);
                    break;
                case (int)RC.LocationTypes.Governorate:
                    returnValue = Convert.ToInt32(ddlNational.SelectedValue);
                    break;
                case (int)RC.LocationTypes.District:
                    returnValue = Convert.ToInt32(ddlGovernorate.SelectedValue);
                    break;
                case (int)RC.LocationTypes.Subdistrict:
                    returnValue = Convert.ToInt32(ddlDistrict.SelectedValue);
                    break;
                case (int)RC.LocationTypes.Village:
                    returnValue = Convert.ToInt32(ddlSubDistrict.SelectedValue);
                    break;
                case (int)RC.LocationTypes.Nonrepresentative:                    
                    break;
                case (int)RC.LocationTypes.Other:                    
                    break;

            }
            return returnValue;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Master.BaseURL + "/admin/location/LocationsList.aspx");
        }
    }
}