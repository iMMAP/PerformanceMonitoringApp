using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Admin
{
    public partial class PriorityActivities : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateEmergencies();
                PopulateObjectives();
                PopulatePriorities();
            }
        }

        protected void ddlEmergencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateActivities();
        }

        protected void ddlEmgClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateActivities();
        }

        protected void ddlObjectives_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateActivities();
        }

        protected void ddlPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateActivities();
        }

        // Populate Emergencies Drop down.
        private void PopulateEmergencies()
        {
            int locationId = (int)RC.SiteLanguage.English;
            UI.FillEmergency(ddlEmergencies, RC.GetAllEmergencies(locationId));

            if (ddlEmergencies.Items.Count == 1)
            {
                int emergencyId = 0;
                int.TryParse(ddlEmergencies.SelectedValue, out emergencyId);
                PopulateEmergencyClusters(emergencyId);
            }
        }

        private void PopulateEmergencyClusters(int emregencyId)
        {
            ddlEmgClusters.DataValueField = "EmergencyClusterId";
            ddlEmgClusters.DataTextField = "ClusterName";

            ddlEmgClusters.DataSource = GetEmergencyClusters(emregencyId);
            ddlEmgClusters.DataBind();

            ListItem item = new ListItem("Select Cluster", "0");
            ddlEmgClusters.Items.Insert(0, item);
        }

        private DataTable GetEmergencyClusters(int emergencyId)
        {
            return DBContext.GetData("GetEmergencyClusters", new object[] { emergencyId, 1 });
        }

        private void PopulateObjectives()
        {
            DataTable dt = GetObjectives();
            UI.FillObjectives(ddlObjectives, dt, false);

            ListItem item = new ListItem("Select Objective", "0");
            ddlObjectives.Items.Insert(0, item);
        }

        private DataTable GetObjectives()
        {
            int isLogFrame = 1;
            return DBContext.GetData("GetObjectivesLogFrame", new object[] { (int)RC.SiteLanguage.English, isLogFrame });
        }

        private void PopulatePriorities()
        {
            DataTable dt = GetPriorites();
            UI.FillPriorities(ddlPriority, dt);
            ListItem item = new ListItem("Select Priority", "0");
            ddlPriority.Items.Insert(0, item);
        }

        private DataTable GetPriorites()
        {
            int isLogFrame = 1;
            return DBContext.GetData("GetPrioritiesLogFrame", new object[] { (int)RC.SiteLanguage.English, isLogFrame });
        }

        private void PopulateActivities()
        {
            int emergencyId = 0;
            int.TryParse(ddlEmergencies.SelectedValue, out emergencyId);

            int clusterId = 0;
            int.TryParse(ddlEmgClusters.SelectedValue, out clusterId);

            int objId = 0;
            int.TryParse(ddlObjectives.SelectedValue, out objId);

            int prId = 0;
            int.TryParse(ddlPriority.SelectedValue, out prId);

            int languageId = (int) RC.SiteLanguage.English;
            DataTable dt = DBContext.GetData("GetActivities", new object[] { emergencyId, clusterId, objId, prId, languageId });
            gvActivity.DataSource = dt;
            gvActivity.DataBind();
        }
    }
}