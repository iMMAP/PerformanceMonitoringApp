using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using BusinessLogic;
using System.Data;
using System.Globalization;

namespace SRFROWCA.Ebola
{
    public partial class ValidateReportList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateDropDowns();
                LoadReports();
            }
        }

        internal override void BindGridData()
        {
            PopulateMonths();
        }

        private void PopulateDropDowns()
        {
            PopulateMonths();
            PopulateOrganizations();
            PopulateProjectCodes();
        }

        private void PopulateMonths()
        {
            
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
            int tempVal = 0;

            int? clusterId = tempVal > 0 ? tempVal : UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;
            int? emgLocationId = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;

            int id = RC.GetSelectedIntVal(ddlProjects);

            if (id == 0)
            {
                id = RC.GetSelectedIntVal(ddlProjectTitle);
            }
            int? projectID = id == 0 ? (int?)null : id;

            //id = RC.GetSelectedIntVal(ddlMonths);
            int? monthId = id == 0 ? (int?)null : id;

            id = RC.GetSelectedIntVal(ddlOrganizations);
            int? orgId = id == 0 ? (int?)null : id;

            //bool srpInd = cbCountryIndicators.Checked;

            //bool? isOPS = rbIsOPSProject.SelectedValue.Equals("-1") ? (bool?)null : Convert.ToBoolean(rbIsOPSProject.SelectedValue);

            DateTime? startDate = txtFromDate.Text.Trim().Length > 0 ?
                                    DateTime.ParseExact(txtFromDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture) :
                                (DateTime?)null;

            DateTime? endDate = txtToDate.Text.Trim().Length > 0 ?
                                DateTime.ParseExact(txtToDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture) :
                                (DateTime?)null;

            gvReports.DataSource = DBContext.GetData("GetCountryReports_Ebola", new object[] { emgLocationId, clusterId, projectID, orgId, startDate, endDate });
            gvReports.DataBind();
        }

        protected void btnSearch_Click(object sednder, EventArgs e)
        {
            LoadReports();
        }

        protected void gvReports_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewReport")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());

                int reportId = 0;
                int.TryParse(this.gvReports.DataKeys[rowIndex]["ReportDataEntryId"].ToString(), out reportId);

                Response.Redirect("ValidateIndicators.aspx?rid=" + reportId);
            }
        }

        protected void ddlClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReports();
        }

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReports();
        }

        protected void rbIsOPSProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReports();
        }
    }
}