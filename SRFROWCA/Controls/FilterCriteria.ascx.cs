using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using SRFROWCA.Reports;

namespace SRFROWCA.Controls
{
    public partial class FilterCriteria : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateDropDowns();
        }

        #region DropDown Methods.

        // Populate All drop downs.
        private void PopulateDropDowns()
        {
            PopulateEmergency();
            PopulateClusters();
            PopulateLocations();
            PopulateMonths();
            PopulateUses();
            PopulateOrganizationTypes();
            PopulateOrganizations();
            PopulateOffice();
        }

        // Populate emergency drop down.
        private void PopulateEmergency()
        {
            ddlEmergency.DataValueField = "EmergencyId";
            ddlEmergency.DataTextField = "EmergencyName";
            ddlEmergency.DataSource = GetEmergencies();
            ddlEmergency.DataBind();
        }
        private DataTable GetEmergencies()
        {
            return DBContext.GetData("GetAllEmergencies");
        }

        // Populate Clusters drop down.
        private void PopulateClusters()
        {
            ddlClusters.DataValueField = "ClusterId";
            ddlClusters.DataTextField = "ClusterName";
            ddlClusters.DataSource = GetClusteres();
            ddlClusters.DataBind();
        }
        private object GetClusteres()
        {
            return DBContext.GetData("GetAllClusters");
        }

        // Populate Locations drop down
        private void PopulateLocations()
        {
            ddlLocations.DataValueField = "LocationId";
            ddlLocations.DataTextField = "LocationName";

            ddlLocations.DataSource = GetReportLocations();
            ddlLocations.DataBind();
        }
        private object GetReportLocations()
        {
            return DBContext.GetData("GetAllLocationsInReports");
        }

        // Populate Months drop down
        private void PopulateMonths()
        {
            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();
        }
        private DataTable GetMonth()
        {
            return DBContext.GetData("GetMonths");
        }

        // Populate Users drop down
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

        // Populate Organization types drop down
        private void PopulateOrganizationTypes()
        {
            ddlOrgTypes.DataValueField = "OrganizationTypeId";
            ddlOrgTypes.DataTextField = "OrganizationType";
            ddlOrgTypes.DataSource = GetOrganizationTypes();
            ddlOrgTypes.DataBind();
        }
        private object GetOrganizationTypes()
        {
            return DBContext.GetData("GetOrganizationTypes");
        }

        // Populate Organizations drop down.
        private void PopulateOrganizations()
        {
            ddlOrganizations.DataValueField = "OrganizationId";
            ddlOrganizations.DataTextField = "OrganizationAcronym";
            int? orgId = null;
            ddlOrganizations.DataSource = GetOrganizations(orgId);
            ddlOrganizations.DataBind();
        }
        private object GetOrganizations(int? orgId)
        {
            return DBContext.GetData("GetOrganizations", new object[] { orgId , null});
        }

        // Populate office drop down
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

        #region DropDown SelectedIndexChanged.

        protected void ddlEmergency_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlLocations_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void ddlUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlOrgTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlOrganizations_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        #endregion

        #endregion

        public void LoadData()
        {
            //AllTargetsAndAchieved ctrlLocationTargetAchieved = this.Parent.FindControl("AllTargetsAndAchieved1") as AllTargetsAndAchieved;
            //if (ctrlLocationTargetAchieved != null)
            //{
            //    object[] paramValue = GetParamValues();
            //    ctrlLocationTargetAchieved.DrawChart(paramValue);
            //}
        }

        private object[] GetParamValues()
        {
            Guid userId = ddlUsers.SelectedIndex > 0 ? new Guid(ddlUsers.SelectedValue) : new Guid();
            string emergencyIds = GetSelectedValues(ddlEmergency);
            int? officeId = GetSelectedValue(ddlOffice);
            int? yearId = GetSelectedValue(ddlYear);
            string monthIds = GetSelectedValues(ddlMonth);
            string locationIds = GetSelectedValues(ddlLocations);
            string clusterIds = GetSelectedValues(ddlClusters);
            string orgIds = GetSelectedValues(ddlOrganizations);
            string orgTypeIds = GetSelectedValues(ddlOrgTypes);

            return new object[] { emergencyIds, officeId, userId, yearId, monthIds, locationIds, clusterIds, orgIds, orgTypeIds };
        }

        // Get multiple selected values from drop down checkbox.
        private string GetSelectedValues(object sender)
        {
            string ids = GetSelectedItems(sender);
            ids = !string.IsNullOrEmpty(ids) ? ids : null;
            return ids;
        }

        // Get Selected Value from Drop Down.
        private int? GetSelectedValue(DropDownList ddl)
        {
            int val = 0;
            int.TryParse(ddlOffice.SelectedValue, out val);
            return val > 0 ? val : (int?)null;
        }

        private string GetSelectedItems(object sender)
        {
            string itemIds = "";
            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    if (itemIds != "")
                    {
                        itemIds += "," + item.Value;
                    }
                    else
                    {
                        itemIds += item.Value;
                    }
                }
            }

            return itemIds;
        }

    }
}