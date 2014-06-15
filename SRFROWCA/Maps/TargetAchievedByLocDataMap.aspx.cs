using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using SRFROWCA.Reports;
using SRFROWCA.Common;

namespace SRFROWCA.Maps
{
    public partial class TargetAchievedByLocDataMap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateDataDropDown();
            PopulateCountry();
            PopulateLocationDropDowns();
            DrawMap(ReportsCommon.LocationType.Country);
        }

        private void PopulateDataDropDown()
        {
            ddlData.DataValueField = "ActivityDataId";
            ddlData.DataTextField = "DataName";

            ddlData.DataSource = DBContext.GetData("GetDataWithActivityIndicator");
            ddlData.DataBind();
        }

        protected void ddlData_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawMap(LastLocationType);
        }

        private void PopulateLocationDropDowns()
        {
            int countryId = 0;
            int.TryParse(ddlCountry.SelectedValue, out countryId);
            if (countryId > 0)
            {
                PopulateAdmin1(countryId);
                PopulateAdmin2(countryId);
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateLocationDropDowns();
            DrawMap(ReportsCommon.LocationType.Country);
        }

        protected void ddlAdmin1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LastLocationType = ReportsCommon.LocationType.Admin1;
            DrawMap(ReportsCommon.LocationType.Admin1);
        }

        protected void ddlLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            LastLocationType = ReportsCommon.LocationType.Admin2;
            DrawMap(ReportsCommon.LocationType.Admin2);
        }

        private void PopulateCountry()
        {
            ddlCountry.DataValueField = "LocationId";
            ddlCountry.DataTextField = "LocationName";

            ddlCountry.DataSource = DBContext.GetData("GetCountries");
            ddlCountry.DataBind();
        }

        private void PopulateAdmin1(int countryId)
        {
            ddlAdmin1.DataValueField = "LocationId";
            ddlAdmin1.DataTextField = "LocationName";

            ddlAdmin1.DataSource = DBContext.GetData("GetAdmin1LocationsOfCountry", new object[] { countryId });
            ddlAdmin1.DataBind();
        }

        // Populate Locations drop down
        private void PopulateAdmin2(int countryId)
        {
            ddlLocations.DataValueField = "LocationId";
            ddlLocations.DataTextField = "LocationName";

            ddlLocations.DataSource = DBContext.GetData("GetAdmin2LocationsOfCountry", new object[] { countryId });
            ddlLocations.DataBind();
        }

        private void DrawMap(ReportsCommon.LocationType locationType)
        {
            GetLocationData(locationType);
            
        }

        private DataTable GetLocationData(ReportsCommon.LocationType locationType)
        {
            object[] param = GetParamValues((int)locationType);
            return DBContext.GetData("GetTargetAchievedByLocationMap", param);
        }

        private object[] GetParamValues(int locationTypeId)
        {
            string locationIds = null;
            if (locationTypeId == 1)
            {
                locationTypeId = 2;
                locationIds = ddlCountry.SelectedValue;
            }
            else if (locationTypeId == 2)
            {
                locationIds = RC.GetSelectedValues(ddlAdmin1);
            }
            else if (locationTypeId == 3)
            {
                locationIds = RC.GetSelectedValues(ddlLocations);
            }

            int? dataId = RC.GetSelectedValue(ddlData);
            return new object[] { locationIds, locationTypeId, dataId, DBNull.Value };
        }

        private ReportsCommon.LocationType LastLocationType
        {
            get
            {
                if (ViewState["LastLocationType"] != null)
                {
                    return LastLocationType;
                }
                else
                {
                    return ReportsCommon.LocationType.Country;
                }
            }
            set
            {
                ViewState["LastLocationType"] = value;
            }
        }
    }
}