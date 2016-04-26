using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using BusinessLogic;
using System.Data;
using System.Drawing;

namespace SRFROWCA.ClusterLead
{
    public partial class ValidateReportList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateDropDowns();
                PopulateClusters();
                SetDropDownOnRole();
                LoadReports();
            }
        }

        internal override void BindGridData()
        {
            PopulateMonths();
        }

        private void PopulateClusters()
        {
            UI.FillEmergnecyClusters(ddlClusters, RC.EmergencySahel2015);
            RC.AddSelectItemInList(ddlClusters, "Select Cluster");
        }

        private void SetDropDownOnRole()
        {
            if (RC.IsClusterLead(this.User))
            {
                ddlClusters.SelectedValue = UserInfo.EmergencyCluster.ToString();
                ddlClusters.Enabled = false;
                ddlClusters.BackColor = Color.LightGray;
            }
        }

        private void PopulateDropDowns()
        {
            PopulateProjectCodes();
            PopulateMonths();
            PopulateOrganizations();
        }

        private void PopulateMonths()
        {
            ddlMonths.DataValueField = "MonthId";
            ddlMonths.DataTextField = "MonthName";

            ddlMonths.DataSource = RC.GetMonths();
            ddlMonths.DataBind();

            ListItem item = new ListItem("Select", "0");
            ddlMonths.Items.Insert(0, item);
        }

        // Populate Organizations drop down.
        private void PopulateOrganizations()
        {
            ddlOrganizations.DataValueField = "OrganizationId";
            ddlOrganizations.DataTextField = "OrganizationName";
            ddlOrganizations.DataSource = GetOrganizations();
            ddlOrganizations.DataBind();

            ListItem item = new ListItem("Select", "0");
            ddlOrganizations.Items.Insert(0, item);
        }

        private object GetOrganizations()
        {
            int val = RC.GetSelectedIntVal(ddlProjects);
            int? projectId = val > 0 ? val : (int?)null;
            val = RC.GetSelectedIntVal(ddlOrganizations);
            int? orgId = val > 0 ? val : (int?)null;
            val = RC.GetSelectedIntVal(ddlClusters);
            int? emgClusterId = val > 0 ? val : UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;
            val = RC.GetSelectedIntVal(ddlMonths);
            int? monthId = val > 0 ? val : (int?)null;
            int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);
            int? emgLocationId = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;
            return DBContext.GetData("GetReportOrganizationsForClusterLead", new object[] { projectId, orgId, emgLocationId, 
                                                                                            emgClusterId, monthId, yearId });
        }

        private void PopulateProjectCodes()
        {
            ddlProjects.DataValueField = "ProjectId";
            ddlProjects.DataTextField = "ProjectCode";

            int val = RC.GetSelectedIntVal(ddlOrganizations);
            int? orgId = val > 0 ? val : (int?)null;
            val = RC.GetSelectedIntVal(ddlClusters);
            int? emgClusterId = val > 0 ? val : UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;
            val = RC.GetSelectedIntVal(ddlMonths);
            int? monthId = val > 0 ? val : (int?)null;
            int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);
            int? emgLocationId = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;
            ddlProjects.DataSource = DBContext.GetData("GetReportProjectsForClusterLead", new object[] { orgId, emgLocationId, emgClusterId, monthId, yearId });
            ddlProjects.DataBind();
            if (ddlProjects.Items.Count > 0)
                ddlProjects.Items.Insert(0, new ListItem("Select", "0"));
        }

        private void LoadReports()
        {
            int tempVal = 0;
            if (ddlClusters.Visible)
            {
                int.TryParse(ddlClusters.SelectedValue, out tempVal);
            }

            int? clusterId = tempVal > 0 ? tempVal : UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;
            int? emgLocationId = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;

            int id = RC.GetSelectedIntVal(ddlProjects);
            int? projectId = id == 0 ? (int?)null : id;

            id = RC.GetSelectedIntVal(ddlMonths);
            int? monthId = id == 0 ? (int?)null : id;

            id = RC.GetSelectedIntVal(ddlOrganizations);
            int? orgId = id == 0 ? (int?)null : id;

            bool? isOPS = rbIsOPSProject.SelectedValue.Equals("-1") ? (bool?)null : Convert.ToBoolean(rbIsOPSProject.SelectedValue);
            int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);
            gvReports.DataSource = DBContext.GetData("GetCountryReports", new object[] { emgLocationId, clusterId, projectId, 
                                                                                        monthId, orgId, isOPS, yearId, RC.SelectedSiteLanguageId });
            gvReports.DataBind();
        }

        protected void gvReports_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewReport")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());

                int reportId = 0;
                int.TryParse(this.gvReports.DataKeys[rowIndex]["ReportId"].ToString(), out reportId);

                Response.Redirect("ValidateIndicators.aspx?rid=" + reportId);
            }
        }

        protected void ddlClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReports();
        }

        protected void ddlProjectCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateOrganizations();
            LoadReports();
        }

        protected void ddlOrganizations_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateProjectCodes();
            LoadReports();
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateProjectCodes();
            PopulateOrganizations();
            LoadReports();
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateProjectCodes();
            PopulateOrganizations();
            LoadReports();
        }
        protected void rbIsOPSProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReports();
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}