using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using Saplin.Controls;
using System.Web.UI;

namespace SRFROWCA.Common
{
    public static class UI
    {
        // Populate Clusters drop down.        
        // control can be dropdownlist or checkboxlist or any other listcontrol
        internal static void FillClusters(ListControl control, int languageId)
        {
            control.DataValueField = "ClusterId";
            control.DataTextField = "ClusterName";
            control.DataSource = DBContext.GetData("GetAllClusters", new object[] {languageId});
            control.DataBind();
        }

        internal static void FillClusters(DropDownCheckBoxes control, int languageId)
        {
            control.DataValueField = "ClusterId";
            control.DataTextField = "ClusterName";
            control.DataSource = DBContext.GetData("GetAllClusters", new object[] { languageId });
            control.DataBind();
        }

        // Populate Emergency Clusters drop down.
        internal static void FillEmergnecyClusters(CheckBoxList cbl, DataTable dt)
        {
            cbl.DataValueField = "EmergencyClusterId";
            cbl.DataTextField = "ClusterName";
            cbl.DataSource = dt;
            cbl.DataBind();
        }

        // Populate Clusters Check Box List.
        internal static void FillClustersCheckBoxList(CheckBoxList cbl, DataTable dt)
        {
            cbl.DataValueField = "ClusterId";
            cbl.DataTextField = "ClusterName";
            cbl.DataSource = dt;
            cbl.DataBind();
        }

        // Populate LocationDropDown
        internal static void FillLocations(ListControl ddl, DataTable dt)
        {
            ddl.DataValueField = "LocationId";
            ddl.DataTextField = "LocationName";

            ddl.DataSource = dt;
            ddl.DataBind();
        }

        // Populate LocationDropDown
        internal static void FillLocations(DropDownCheckBoxes ddl, DataTable dt)
        {
            ddl.DataValueField = "LocationId";
            ddl.DataTextField = "LocationName";

            ddl.DataSource = dt;
            ddl.DataBind();
        }

        internal static void FillOrganizations(DropDownCheckBoxes ddl)
        {
            ddl.DataValueField = "OrganizationId";
            ddl.DataTextField = "OrganizationAcronym";
            int? orgId = null;
            ddl.DataSource = DBContext.GetData("GetOrganizations", new object[] { orgId });
            ddl.DataBind();
        }

        internal static void FillAllObjectives(DropDownList ddl, DataTable dt)
        {
            ddl.DataValueField = "ObjectiveId";
            ddl.DataTextField = "Objective";
            ddl.DataSource = dt;
            ddl.DataBind();
        }

        internal static void FillIndicators(DropDownCheckBoxes ddl, DataTable dt)
        {
            ddl.DataValueField = "ObjectiveIndicatorId";
            ddl.DataTextField = "IndicatorName";

            ddl.DataSource = dt;
            ddl.DataBind();
        }

        internal static void FillActivities(DropDownCheckBoxes ddl, DataTable dt)
        {
            ddl.DataValueField = "IndicatorActivityId";
            ddl.DataTextField = "ActivityName";

            ddl.DataSource = dt;
            ddl.DataBind();
        }

        internal static void FillDataItems(DropDownCheckBoxes ddl, DataTable dt)
        {
            ddl.DataValueField = "ActivityDataId";
            ddl.DataTextField = "DataName";

            ddl.DataSource = dt;
            ddl.DataBind();
        }

        internal static void FillLocationEmergency(DropDownList ddl, DataTable dt)
        {
            ddl.DataValueField = "LocationEmergencyId";
            ddl.DataTextField = "EmergencyName";
            ddl.DataSource = dt;
            ddl.DataBind();

            if (ddl.Items.Count > 1)
            {
                ListItem item = new ListItem("Select Emergency", "0");
                ddl.Items.Insert(0, item);
                ddl.SelectedIndex = 0;
            }
        }

        internal static void FillEmergency(DropDownList ddl, DataTable dt)
        {
            ddl.DataValueField = "EmergencyId";
            ddl.DataTextField = "EmergencyName";
            ddl.DataSource = dt;
            ddl.DataBind();

            if (ddl.Items.Count > 1)
            {
                ListItem item = new ListItem("Select Emergency", "0");
                ddl.Items.Insert(0, item);
                ddl.SelectedIndex = 0;
            }
        }

        internal static void FillCountry(DropDownList ddl)
        {
            ddl.DataValueField = "LocationId";
            ddl.DataTextField = "LocationName";

            ddl.DataSource = DBContext.GetData("GetCountries");
            ddl.DataBind();

            ListItem item = new ListItem("Select Country", "0");
            ddl.Items.Insert(0, item);
            ddl.SelectedIndex = 0;
        }

        internal static void FillPriorities(DropDownList ddl, DataTable dt)
        {
            ddl.DataValueField = "HumanitarianPriorityId";
            ddl.DataTextField = "HumanitarianPriority";            
            ddl.DataSource = dt;
            ddl.DataBind();
        }
    }
}