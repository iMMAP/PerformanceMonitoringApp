using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using BusinessLogic;
using System.Data;

namespace SRFROWCA.ClusterLead
{
    public partial class ValidateReportList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadReports();
                PopulateDropDowns();
            }
        }

        private void PopulateDropDowns()
        {
            PopulateMonths();
            PopulateOrganizations();
            PopulateProjectCodes();
        }

        private void PopulateMonths()
        {
            ddlMonths.DataValueField = "MonthId";
            ddlMonths.DataTextField = "MonthName";

            ddlMonths.DataSource = GetMonth();
            ddlMonths.DataBind();

            ListItem item = new ListItem("Select", "0");
            ddlMonths.Items.Insert(0, item);
        }

        private DataTable GetMonth()
        {
            return DBContext.GetData("GetMonths", new object[] { 1 });
        }

        // Populate Organizations drop down.
        private void PopulateOrganizations()
        {
            ddlOrganizations.DataValueField = "OrganizationId";
            ddlOrganizations.DataTextField = "OrganizationName";
            int? orgId = null;
            ddlOrganizations.DataSource = GetOrganizations(orgId);
            ddlOrganizations.DataBind();

            ListItem item = new ListItem("Select", "0");
            ddlOrganizations.Items.Insert(0, item);
        }

        private object GetOrganizations(int? orgId)
        {
            return DBContext.GetData("GetReportOrganizationsForClusterLead", new object[] { UserInfo.EmergencyCountry, UserInfo.EmergencyCluster });
        }

        private void PopulateProjectCodes()
        {
            using (ORSEntities db = new ORSEntities())
            {
                ddlProjects.DataValueField = "ProjectId";
                ddlProjects.DataTextField = "ProjectCode";

                DataTable dt = DBContext.GetData("GetReportProjectsForClusterLead", new object[] { UserInfo.EmergencyCountry, UserInfo.EmergencyCluster });
                ddlProjects.DataSource = dt;
                ddlProjects.DataBind();


                ListItem item = new ListItem("Select", "0");
                ddlProjects.Items.Insert(0, item);

                ddlProjectTitle.DataValueField = "ProjectId";
                ddlProjectTitle.DataTextField = "ProjectTitle";

                ddlProjectTitle.DataSource = dt;
                ddlProjectTitle.DataBind();

                ddlProjectTitle.Items.Insert(0, item);
            }
        }

        private void LoadReports()
        {
            int id = RC.GetSelectedIntVal(ddlProjects);

            if (id == 0)
            {
                id = RC.GetSelectedIntVal(ddlProjectTitle);
            }
            int? projectID = id == 0 ? (int?)null : id;

            id = RC.GetSelectedIntVal(ddlMonths);
            int? monthId = id == 0 ? (int?)null : id;

            id = RC.GetSelectedIntVal(ddlOrganizations);
            int? orgId = id == 0 ? (int?)null : id;

            gvReports.DataSource = DBContext.GetData("GetCountryReports", new object[] { UserInfo.EmergencyCountry, UserInfo.EmergencyCluster, projectID, monthId, orgId });
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

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReports();
        }
    }
}