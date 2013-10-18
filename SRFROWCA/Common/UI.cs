using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using Saplin.Controls;

namespace SRFROWCA.Common
{
    public static class UI
    {
        // Populate Clusters drop down.
        internal static void FillClusters(DropDownCheckBoxes ddl)
        {
            ddl.DataValueField = "ClusterId";
            ddl.DataTextField = "ClusterName";
            ddl.DataSource = DBContext.GetData("GetAllClusters");
            ddl.DataBind();
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
        internal static void FillLocations(DropDownList ddl, DataTable dt)
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

        internal static void FillObjectives(DropDownCheckBoxes ddl, DataTable dt)
        {
            ddl.DataValueField = "ClusterObjectiveId";
            ddl.DataTextField = "ObjectiveName";

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
        }
    }
}