using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using Saplin.Controls;
using System.Web.UI;
using System.Globalization;
using System;
using System.Security.Principal;

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

        internal static void FillEmergnecyClusters(ListControl cbl, int emergencyId, int removeEntry)
        {
            cbl.DataValueField = "EmergencyClusterId";
            cbl.DataTextField = "ClusterName";
            DataTable dt = RC.GetEmergencyClusters(emergencyId);
            for(int i = 0; i< dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["EmergencyClusterId"].ToString() == removeEntry.ToString())
                {
                    dt.Rows.RemoveAt(i);
                    break;
                }
            }
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

        internal static void FillAdmin1(ListControl ddl, int countryId, int locCatId)
        {
            ddl.DataValueField = "LocationId";
            ddl.DataTextField = "LocationName";
            ddl.DataSource = RC.GetAdmin1(countryId, locCatId);
            ddl.DataBind();
        }

        internal static void FillAdmin2(ListControl ddl, int countryId, int locCatId)
        {
            ddl.DataValueField = "LocationId";
            ddl.DataTextField = "LocationName";
            ddl.DataSource = RC.GetAdmin2(countryId, locCatId);
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

        internal static void FillObjectives(ListControl control, bool useShort, int emergencyId)
        {
            DataTable dt = RC.GetObjectives(emergencyId);
            FillObjectives(control, dt, useShort);
        }

        internal static void PopulateEmergencyObjectives(ListControl control, int yearId, int? emergencyLocationId)
        {
            control.DataValueField = "EmergencyObjectiveId";
            control.DataTextField = "Objective";
            control.DataSource = RC.GetEmergencyObjectives(yearId, emergencyLocationId);
            control.DataBind();

            if (RC.SelectedSiteLanguageId == 1)
                control.Items.Insert(0, new ListItem("Select Objective", "0"));
            else
                control.Items.Insert(0, new ListItem("Sélectionner Objectif", "0"));
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

        internal static void FillObjectives(ListControl control, int emergencyId)
        {
            DataTable dt = RC.GetObjectives(emergencyId);
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
            DataTable dt = RC.GetAllUnits();
            ddl.DataSource = dt;
            ddl.DataBind();
        }

        internal static void FillLocationsOfEmergency(ListControl ctl, int emergencyId)
        {
            DataTable dt = RC.GetEmergencyLocations(emergencyId);
            ctl.DataTextField = "LocationName";
            ctl.DataValueField = "LocationId";
            ctl.DataSource = dt;
            ctl.DataBind();
        }

        internal static void FillEmergencyLocations(ListControl ctl, int emergencyId)
        {
            DataTable dt = RC.GetEmergencyLocations(emergencyId);
            ctl.DataTextField = "LocationName";
            ctl.DataValueField = "EmergencyLocationId";
            ctl.DataSource = dt;
            ctl.DataBind();
        }

        internal static void SetThousandSeparator(GridViewRow row, string ctlId)
        {
            Label lbl = row.FindControl(ctlId) as Label;
            if (lbl != null && !string.IsNullOrEmpty(lbl.Text))
                lbl.Text = GetThousandSeparator(lbl.Text);
        }

        internal static string GetControlText(GridViewRow row, string ctlId)
        {
            ITextControl ctl = row.FindControl(ctlId) as ITextControl;
            if (ctl != null && !string.IsNullOrEmpty(ctl.Text))
                return ctl.Text;
            return "";
        }

        internal static void SetThousandSeparator(RepeaterItem row, string ctlId)
        {
            Label lbl = row.FindControl(ctlId) as Label;
            if (lbl != null && !string.IsNullOrEmpty(lbl.Text))
                lbl.Text = GetThousandSeparator(lbl.Text);
        }

        internal static string GetThousandSeparator(string number)
        {
            string siteCulture = RC.SelectedSiteLanguageId.Equals(1) ? "en-US" : "fr-FR";
            if (number.Length > 1)
                    number  = String.Format(new CultureInfo(siteCulture), "{0:0,0}", Convert.ToInt32(number));
            return number;
        }

        internal static void SetUserCountry(ListControl lstControl)
        {
            if (UserInfo.EmergencyCountry > 0)
            {
                lstControl.SelectedValue = UserInfo.EmergencyCountry.ToString();
            }
        }

        internal static void SetUserCluster(ListControl lstControl)
        {
            if (UserInfo.EmergencyCluster > 0)
            {
                lstControl.SelectedValue = UserInfo.EmergencyCluster.ToString();
            }
        }
    }
}