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
            control.DataSource = DBContext.GetData("GetAllClusters", new object[] {languageId,(int?)null});
            control.DataBind();
        }

        internal static void FillClusters(DropDownCheckBoxes control, int languageId)
        {
            control.DataValueField = "ClusterId";
            control.DataTextField = "ClusterName";
            control.DataSource = DBContext.GetData("GetAllClusters", new object[] { languageId, (int?)null });
            control.DataBind();
        }

        // Populate Emergency Clusters drop down.
        internal static void FillEmergnecyClusters(ListControl cbl, int emergencyId)
        {
            cbl.DataValueField = "EmergencyClusterId";
            cbl.DataTextField = "ClusterName";
            cbl.DataSource = RC.GetEmergencyClusters(emergencyId);
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

        internal static void FillAdmin1(ListControl ddl, int countryId)
        {
            ddl.DataValueField = "LocationId";
            ddl.DataTextField = "LocationName";
            ddl.DataSource = RC.GetAdmin1(countryId);
            ddl.DataBind();
        }

        internal static void FillAdmin2(ListControl ddl, int countryId)
        {
            ddl.DataValueField = "LocationId";
            ddl.DataTextField = "LocationName";
            ddl.DataSource = RC.GetAdmin2(countryId);
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

        internal static void FillOrganizations(ListControl ctl, int? orgId)
        {
            ctl.DataValueField = "OrganizationId";
            ctl.DataTextField = "OrganizationName";            
            ctl.DataSource = DBContext.GetData("GetOrganizations", new object[] { orgId , null});
            ctl.DataBind();
        }

        internal static void FillOrganizations(ListControl ctl, DataTable dt)
        {
            ctl.DataValueField = "OrganizationId";
            ctl.DataTextField = "OrganizationName";
            ctl.DataSource = dt;
            ctl.DataBind();
        }

        internal static void FillObjectives(ListControl control, bool useShort)
        {
            DataTable dt = RC.GetObjectives();
            FillObjectives(control, dt, true);
        }

        internal static void FillEmergencyObjectives(ListControl control, bool useShort, int emergencyId)
        {
            DataTable dt = RC.GetEmergencyObjectives(emergencyId);
            FillObjectives(control, dt, true);
        }

        internal static void FillObjectives(ListControl control, DataTable dt, bool useShort)
        {
            control.DataValueField = "ObjectiveId";
            if (useShort)
            {
                control.DataTextField = "ShortObjectiveTitle";
            }
            else
            {
                control.DataTextField = "Objective";
            }

            control.DataSource = dt;
            control.DataBind();
        }

        internal static void FillObjectives(ListControl control)
        {
            DataTable dt = RC.GetObjectives();
            FillObjectives(control, dt, false);
        }

        internal static void FillPriorities(ListControl control, DataTable dt)
        {
            control.DataValueField = "HumanitarianPriorityId";
            control.DataTextField = "ShortPriorityText";            
            control.DataSource = dt;
            control.DataBind();
        }

        internal static void FillPriorities(ListControl control, DataTable dt, bool userShortText)
        {
            control.DataValueField = "HumanitarianPriorityId";
            if (userShortText)
            {
                control.DataTextField = "ShortPriorityText";
            }
            else
            {
                control.DataTextField = "HumanitarianPriority";
            }
            control.DataSource = dt;
            control.DataBind();
        }

        internal static void FillPriorities(ListControl control)
        {
            DataTable dt = RC.GetPriorities();
            FillPriorities(control, dt);
        }

        internal static void FillActivities(ListControl ddl, DataTable dt)
        {
            ddl.DataValueField = "PriorityActivityId";
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

        internal static void FillCountry(ListControl ddl)
        {
            ddl.DataValueField = "LocationId";
            ddl.DataTextField = "LocationName";

            ddl.DataSource = DBContext.GetData("GetCountries");
            ddl.DataBind();
        }

        internal static void FillUnits(DropDownList ddl, DataTable dt)
        {
            ddl.DataValueField = "UnitId";
            ddl.DataTextField = "Unit";
            ddl.DataSource = dt;
            ddl.DataBind();
        }

        internal static void FillUnits(DropDownList ddl)
        {
            ddl.DataValueField = "UnitId";
            ddl.DataTextField = "Unit";
            DataTable dt = RC.GetAllUnits(RC.SelectedSiteLanguageId);
            ddl.DataSource = dt;
            ddl.DataBind();
        }

        internal static void FillEmergencyLocations(ListControl ctl, int emergencyId)
        {
            DataTable dt = RC.GetLocationEmergencies(emergencyId);
            ctl.DataTextField = "LocationName";
            ctl.DataValueField = "LocationId";
            ctl.DataSource = dt;
            ctl.DataBind();
        }

        internal static void FillEmergencyLocations(ListControl ctl, int emergencyId, int siteLangID)
        {
            DataTable dt = RC.GetEmergencyLocations(emergencyId, siteLangID);
            ctl.DataTextField = "LocationName";
            ctl.DataValueField = "EmergencyLocationId";
            ctl.DataSource = dt;
            ctl.DataBind();
        }
    }
}