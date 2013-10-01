using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;

namespace SRFROWCA.Admin.Reports
{
    public partial class ActivitiesReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadData();

            PopulateDropDowns();
        }

        private void PopulateDropDowns()
        {
            PopulateEmergency();
            PopulateOffice();
            PopulateUses();
            PopulateYears();
            PopulateMonths();
            PopulateLocations();
            PopulateClusters();
            PopulateOrganizationTypes();
            PopulateOrganizations();
        }

        private void PopulateEmergency()
        {
            ddlEmergency.DataValueField = "EmergencyId";
            ddlEmergency.DataTextField = "EmergencyName";

            ddlEmergency.DataSource = GetEmergencies();
            ddlEmergency.DataBind();

            ListItem item = new ListItem("Select Emergency", "0");
            ddlEmergency.Items.Insert(0, item);
        }

        private DataTable GetEmergencies()
        {
            return DBContext.GetData("GetAllEmergencies");
        }

        private void PopulateOffice()
        {
            ddlOffice.DataValueField = "OfficeId";
            ddlOffice.DataTextField = "OfficeName";

            ddlOffice.DataSource = GetOffices();
            ddlOffice.DataBind();

            ListItem item = new ListItem("Select Office", "0");
            ddlOffice.Items.Insert(0, item);
        }

        private DataTable GetOffices()
        {
            return DBContext.GetData("GetAllOffices");
        }

        private void PopulateUses()
        {
            ddlUsers.DataValueField = "UserId";
            ddlUsers.DataTextField = "UserName";
            ddlUsers.DataSource = GetUsers();
            ddlUsers.DataBind();
            ListItem item = new ListItem("Select User", "0");
            ddlUsers.Items.Insert(0, item);
        }

        private object GetUsers()
        {
            return DBContext.GetData("GetAllUsers");
        }

        private void PopulateYears()
        {
            ddlYear.DataValueField = "YearId";
            ddlYear.DataTextField = "Year";

            ddlYear.DataSource = GetYears();
            ddlYear.DataBind();

            ListItem item = new ListItem("Select Year", "0");
            ddlYear.Items.Insert(0, item);
        }

        private DataTable GetYears()
        {
            return DBContext.GetData("GetYears");
        }

        private void PopulateMonths()
        {
            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            ListItem item = new ListItem("Select Month", "0");
            ddlMonth.Items.Insert(0, item);
        }

        private DataTable GetMonth()
        {
            return DBContext.GetData("GetMonths");
        }

        private void PopulateLocations()
        {
            ddlLocations.DataValueField = "LocationId";
            ddlLocations.DataTextField = "LocationName";

            ddlLocations.DataSource = GetReportLocations();
            ddlLocations.DataBind();

            ListItem item = new ListItem("Select Location", "0");
            ddlLocations.Items.Insert(0, item);
        }

        private object GetReportLocations()
        {
            return DBContext.GetData("GetAllLocationsInReports");
        }

        private void PopulateClusters()
        {
            //ddlClusters.DataValueField = "ClusterId";
            //ddlClusters.DataTextField = "ClusterName";
            //ddlClusters.DataSource = GetClusteres();
            //ddlClusters.DataBind();

            //ListItem item = new ListItem("Select Cluster", "0");
            //ddlClusters.Items.Insert(0, item);
        }

        private object GetClusteres()
        {
            return DBContext.GetData("GetAllClusters");
        }

        private void PopulateObjectives(string clusterIds)
        {
            ddlObjective.DataValueField = "ClusterObjectiveId";
            ddlObjective.DataTextField = "ObjectiveName";
            ddlObjective.DataSource = GetClusterObjectives(clusterIds);
            ddlObjective.DataBind();

            ListItem item = new ListItem("Select Objective", "0");
            ddlObjective.Items.Insert(0, item);
        }

        private object GetClusterObjectives(string clusterIds)
        {
            return DBContext.GetData("GetClusterObjectives", new object[] { clusterIds });
        }

        private void PopulateIndicators(int objectiveId)
        {
            ddlIndicators.DataValueField = "ObjectiveIndicatorId";
            ddlIndicators.DataTextField = "IndicatorName";
            ddlIndicators.DataSource = GetObjectiveIndicators(objectiveId);
            ddlIndicators.DataBind();

            ListItem item = new ListItem("Select Indicator", "0");
            ddlIndicators.Items.Insert(0, item);
        }

        private object GetObjectiveIndicators(int objectiveId)
        {
            return DBContext.GetData("GetObjectiveIndicators", new object[] { objectiveId });
        }

        private void PopulateActivities(int indicatorId)
        {
            ddlActivities.DataValueField = "IndicatorActivityId";
            ddlActivities.DataTextField = "ActivityName";
            ddlActivities.DataSource = GetIndicatorActivities(indicatorId);
            ddlActivities.DataBind();

            ListItem item = new ListItem("Select Activity", "0");
            ddlActivities.Items.Insert(0, item);
        }

        private object GetIndicatorActivities(int indicatorId)
        {
            return DBContext.GetData("GetIndicatorActivities", new object[] { indicatorId });
        }

        private void PopulateData(int activityId)
        {
            ddlData.DataValueField = "ActivityDataId";
            ddlData.DataTextField = "DataName";
            ddlData.DataSource = GetActivityData(activityId);
            ddlData.DataBind();

            ListItem item = new ListItem("Select Data", "0");
            ddlData.Items.Insert(0, item);
        }

        private object GetActivityData(int activityId)
        {
            return DBContext.GetData("GetActivityData", new object[] { activityId });
        }

        private void PopulateOrganizations()
        {
            ddlOrganizations.DataValueField = "OrganizationId";
            ddlOrganizations.DataTextField = "OrganizationName";
            int? orgId = null;
            ddlOrganizations.DataSource = GetOrganizations(orgId);
            ddlOrganizations.DataBind();

            ListItem item = new ListItem("Select Organization", "0");
            ddlOrganizations.Items.Insert(0, item);
        }

        private object GetOrganizations(int? orgId)
        {
            return DBContext.GetData("GetOrganizations", new object[] { orgId });
        }

        private void PopulateOrganizationTypes()
        {

            ddlOrgTypes.DataValueField = "OrganizationTypeId";
            ddlOrgTypes.DataTextField = "OrganizationType";
            ddlOrgTypes.DataSource = GetOrganizationTypes();
            ddlOrgTypes.DataBind();

            ListItem item = new ListItem("Select Organization Type", "0");
            ddlOrgTypes.Items.Insert(0, item);
        }

        private object GetOrganizationTypes()
        {
            return DBContext.GetData("GetOrganizationTypes");
        }

        private void GetData(string procName, object[] parameters)
        {

        }

        private DataTable GetReportData()
        {
            object[] paramValue = GetParamValues();
            return DBContext.GetData("GetAllTasksDataReport", paramValue);
        }

        private void LoadData()
        {
            gvReport.DataSource = GetReportData();
            gvReport.DataBind();
        }

        private object[] GetParamValues()
        {
            int val = 0;
            int.TryParse(ddlEmergency.SelectedValue, out val);
            int? emergencyId = val > 0 ? val : (int?)null;

            val = 0;
            int.TryParse(ddlOffice.SelectedValue, out val);
            int? officeId = val > 0 ? val : (int?)null;

            Guid userId = ddlUsers.SelectedIndex > 0 ? new Guid(ddlUsers.SelectedValue) : new Guid();

            val = 0;
            int.TryParse(ddlYear.SelectedValue, out val);
            int? yearId = val > 0 ? val : (int?)null;

            val = 0;
            int.TryParse(ddlMonth.SelectedValue, out val);
            int? monthId = val > 0 ? val : (int?)null;

            val = 0;
            int.TryParse(ddlLocations.SelectedValue, out val);
            int? locationId = val > 0 ? val : (int?)null;

            //val = 0;
            //int.TryParse(ddlClusters.SelectedValue, out val);
            //int? clusterId = val > 0 ? val : (int?)null;
            string clusterIds = ""; // GetSelectedClusters(ddlClusters);
            clusterIds = !string.IsNullOrEmpty(clusterIds) ? clusterIds : null;

            val = 0;
            int.TryParse(ddlObjective.SelectedValue, out val);
            int? objectiveId = val > 0 ? val : (int?)null;

            val = 0;
            int.TryParse(ddlIndicators.SelectedValue, out val);
            int? indicatorId = val > 0 ? val : (int?)null;

            val = 0;
            int.TryParse(ddlActivities.SelectedValue, out val);
            int? activityId = val > 0 ? val : (int?)null;

            val = 0;
            int.TryParse(ddlData.SelectedValue, out val);
            int? dataId = val > 0 ? val : (int?)null;

            val = 0;
            int.TryParse(ddlOrganizations.SelectedValue, out val);
            int? orgId = val > 0 ? val : (int?)null;

            val = 0;
            int.TryParse(ddlOrgTypes.SelectedValue, out val);
            int? orgTyepId = val > 0 ? val : (int?)null;

            SetHFQueryString(emergencyId, clusterIds, orgId);

            return new object[] { emergencyId, officeId, userId, yearId, monthId, locationId, clusterIds, objectiveId, indicatorId, activityId, dataId, orgId, orgTyepId };
        }

        private void SetHFQueryString(int? emergencyId, string clusterIds, int? orgId)
        {
            string emergencies = "";
            string clusters = "";
            string orgs = "";

            if (emergencyId != null)
            {
                emergencies = ddlEmergency.SelectedItem.Text;
            }

            if (clusterIds != null)
            {
                //foreach (ListItem item in (ddlClusters as ListControl).Items)
                //{
                //    if (item.Selected)
                //    {
                //        if (clusters != "")
                //        {
                //            clusters += "," + item.Text;
                //        }
                //        else
                //        {
                //            clusters += item.Text;
                //        }
                //    }
                //}
            }

            if (orgId != null)
            {
                orgs = ddlOrganizations.SelectedItem.Text;
            }

            if (!string.IsNullOrEmpty(emergencies) || !string.IsNullOrEmpty(clusters) || !string.IsNullOrEmpty(orgs))
            {
                hfReportLink.Value = string.Format("?emg={0}&org={1}&cls={2}", emergencies, orgs, clusters);
            }
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int j = 0; j < e.Row.Cells.Count; j++)
                {
                    TableCell cell = e.Row.Cells[j];
                    cell.Wrap = false;

                    if (j == 11 || j == 12)
                    {
                        cell.HorizontalAlign = HorizontalAlign.Right;
                    }
                }
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ExportUtility.PrepareGridViewForExport(gvReport);
            ExportGridView();
        }

        public override void VerifyRenderingInServerForm(Control control) { }

        private void ExportGridView()
        {
            string fileName = string.Format("attachment; filename=3wopreport{0}.{1}", DateTime.Now.ToString("MMyyddhhmmss"), "xls");
            //string attachment = "attachment; filename=3wopreport.xls";
            string attachment = fileName;
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvReport.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

        protected void ddlEmergency_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private string GetSelectedClusters(object sender)
        {
            string clusterIds = "";
            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    if (clusterIds != "")
                    {
                        clusterIds += "," + item.Value;
                    }
                    else
                    {
                        clusterIds += item.Value;
                    }
                }
            }

            return clusterIds;
        }

        protected void ddlClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            int objectiveId = 0;
            int.TryParse(ddlObjective.SelectedValue, out objectiveId);
            PopulateIndicators(objectiveId);
            PopulateActivities(0);
            PopulateData(0);
            LoadData();
        }

        protected void ddlIndicators_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indicatorId = 0;
            int.TryParse(ddlIndicators.SelectedValue, out indicatorId);
            PopulateActivities(indicatorId);
            PopulateData(0);
            LoadData();
        }

        protected void ddlActivities_SelectedIndexChanged(object sender, EventArgs e)
        {
            int activityId = 0;
            int.TryParse(ddlActivities.SelectedValue, out activityId);
            PopulateData(activityId);
            LoadData();
        }

        protected void ddlData_SelectedIndexChanged(object sender, EventArgs e)
        {
            int dataId = 0;
            int.TryParse(ddlData.SelectedValue, out dataId);
            LoadData();
        }

        protected void ddlOrganizations_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int orgId = 0;
            //int.TryParse(ddlOrganizations.SelectedValue, out orgId);
            LoadData();
        }

        protected void ddlOrgTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int orgTypeId = 0;
            //int.TryParse(ddlOrgTypes.SelectedValue, out orgTypeId);
            LoadData();
        }

        private void UserFilter()
        {
            
        }
    }
}