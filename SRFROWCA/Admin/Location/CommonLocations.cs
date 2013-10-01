using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;

namespace SRFROWCA.Locations
{
    public static class CommonLocations
    {
        internal static void PopulateProvincesDropDown(DropDownList ddl)
        {
            DataTable dt = DBContext.GetData("GetLocationOnType", new object[] { 2 });

            if (dt.Rows.Count > 0)
            {
                ddl.DataValueField = "LocationId";
                ddl.DataTextField = "LocationName";

                ddl.DataSource = dt;
                ddl.DataBind();

                ddl.Items.Insert(0, new ListItem("Select Province", "0"));
                ddl.SelectedIndex = 0;
            }
        }

        internal static void PopulateLocationDropDowns(int parentId, DropDownList ddl)
        {
            DataTable dt = DBContext.GetData("GetLocationsOnParent", new object[] { parentId });

            if (dt.Rows.Count > 0)
            {
                ddl.DataValueField = "LocationId";
                ddl.DataTextField = "LocationName";

                ddl.DataSource = dt;
                ddl.DataBind();

                ddl.Items.Insert(0, new ListItem("Select Location", "0"));
                ddl.SelectedIndex = 0;
            }
        }

        private static string ConnectionString
        {
            get { return "LIVE"; }
        }

        private enum FetchStatus
        {
            Success = 1
        }
    }
}