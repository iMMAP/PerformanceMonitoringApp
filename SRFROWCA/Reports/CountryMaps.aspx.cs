using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Reports
{
    public partial class CountryMaps : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadCountries();
        }

        internal override void BindGridData()
        {
            LoadCountries();
        }

        private void LoadCountries()
        {
            DataTable dt = DBContext.GetData("GetMapEmergnecyLocations", new object[] { RC.EmergencySahel2015 });
            if (dt.Rows.Count > 0)
            {
                lblMessage.Visible = false;
                rptCountry.DataSource = dt;
                rptCountry.DataBind();
            }
            else
            {
                lblMessage.Visible = true;
            }
        }

        private DataTable GetMaps(int countryId)
        {
            int isPublic = 1;
            string mapTitle = null;
            return DBContext.GetData("GetCountryMaps", new object[] { countryId, mapTitle, isPublic, RC.SelectedSiteLanguageId });
        }

        protected void rptrptCountry_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hfLocationId = e.Item.FindControl("hfLocationId") as HiddenField;
            int locationId = 0;
            int.TryParse(hfLocationId.Value, out locationId);
            GridView gvMaps = e.Item.FindControl("gvMaps") as GridView;
            if (gvMaps != null && locationId > 0)
            {
                gvMaps.DataSource = GetMaps(locationId);
                gvMaps.DataBind();
            }
        }
    }
}