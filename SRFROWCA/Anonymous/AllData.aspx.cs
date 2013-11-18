using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Web;
using System.IO.Compression;
using SRFROWCA.Common;
using Saplin.Controls;
using SRFROWCA.Reports;
using System.Linq;
namespace SRFROWCA.Anonymous
{
    public partial class AllData : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Register btnExportToExcel to trigger postback, in updatepanel.
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnExportToExcel);

            if (IsPostBack) return;

            // Load data in gridview.
            LoadData();

            // Populate all drop downs.
            PopulateDropDowns();
        }

        #region Events.

        protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetReportData();

            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvReport.DataSource = dt;
                gvReport.DataBind();
            }
        }

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            LoadData();
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            //SQLPaging = PagingStatus.OFF;
            //gvReport.AllowPaging = false;
            //LoadData();
            //DataTable dt = GetReportData();
            //string[] columnNames = dt.Columns.Cast<DataColumn>()
            //                     .Select(x => x.ColumnName)
            //                     .ToArray();
            //cbColumns.DataSource = columnNames;
            //cbColumns.DataBind();
            ModalPopupExtender1.Show();
            //GridView gv = new GridView();
            //gv.DataSource = dt;
            //gv.DataBind();
            //ExportUtility.ExportGridView(gv, "3WPMAllData", ".xls", Response);
            //gv.Dispose();
        }

        #region DropDown SelectedIndexChanged.

        protected void ddlEmergency_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataOnMultipleCheckBoxControl();
        }

        protected void ddlClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataOnMultipleCheckBoxControl();
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataOnMultipleCheckBoxControl();
        }

        protected void ddlOrgTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataOnMultipleCheckBoxControl();
        }

        protected void ddlOrganizations_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataOnMultipleCheckBoxControl();
        }

        protected void ddlOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LastLocationType = ReportsCommon.LocationType.Country;
            PopulateLocationDropDowns();
            PopulateOffice();
            LoadData();
        }

        protected void ddlAdmin1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LastLocationType = ReportsCommon.LocationType.Admin1;
            LoadDataOnMultipleCheckBoxControl();
        }

        protected void ddlAdmin2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LastLocationType = ReportsCommon.LocationType.Admin2;
            LoadDataOnMultipleCheckBoxControl();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            DataTable dt = GetReportData();
            RemoveColumnsFromDataTable(dt);
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            
            ExportUtility.ExportGridView(gv, "3WPMAllData", ".xls", Response, true);
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            foreach (ListItem item in cbColumns.Items)
            {
                if (!item.Selected)
                {
                    dt.Columns.Remove(item.Value);
                }
            }

            dt.Columns.Remove("rnumber");
            dt.Columns.Remove("cnt");

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            LoadData();
            ModalPopupExtender1.Hide();
        }

        #endregion

        #endregion

        #region Class Methods.

        #region DropDown Methods.

        // Populate All drop downs.
        private void PopulateDropDowns()
        {
            PopulateEmergency();
            PopulateClusters();
            PopulateLocations();
            PopulateMonths();
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
            PopulateCountry();
            PopulateLocationDropDowns();
        }

        private void PopulateCountry()
        {
            UI.FillCountry(ddlCountry);
        }

        private void PopulateLocationDropDowns()
        {
            int countryId = 0;
            int.TryParse(ddlCountry.SelectedValue, out countryId);
            if (countryId > 0)
            {
                PopulateAdmin1(countryId);
                PopulateAdmin2(countryId);
            }
        }

        private void PopulateAdmin1(int countryId)
        {
            ddlAdmin1.DataValueField = "LocationId";
            ddlAdmin1.DataTextField = "LocationName";

            ddlAdmin1.DataSource = DBContext.GetData("GetAdmin1LocationsOfCountry", new object[] { countryId });
            ddlAdmin1.DataBind();
        }

        // Populate Locations drop down
        private void PopulateAdmin2(int countryId)
        {
            ddlAdmin2.DataValueField = "LocationId";
            ddlAdmin2.DataTextField = "LocationName";

            ddlAdmin2.DataSource = DBContext.GetData("GetAdmin2LocationsOfCountry", new object[] { countryId });
            ddlAdmin2.DataBind();
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
            return DBContext.GetData("GetOrganizations", new object[] { orgId });
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
            int countryId = 0;
            int.TryParse(ddlCountry.SelectedValue, out countryId);

            if (countryId > 0)
            {
                return DBContext.GetData("GetOrganizationOffices", new object[] { countryId, (int?)null });
            }
            else
            {
                return DBContext.GetData("GetAllOffices");
            }
        }

        #endregion

        #region GridView Methods.

        private void LoadDataOnMultipleCheckBoxControl()
        {
            if (Session["DDEventAlreadyFired"] == null)
            {
                Session["DDEventAlreadyFired"] = "true";
                LoadData();
            }
            else
            {
                Session["DDEventAlreadyFired"] = null;
            }
        }

        // Load data on the basis of filter criteria.
        private void LoadData()
        {
            DataTable dt = GetReportData();

            // Cnt column has total number of records in resultset.
            // Gridview is using custom paging so to tell gridview that how many pages
            // it has to generate we have to set VirtualItemCount property with number of records            
            if (dt.Rows.Count > 0)
            {
                gvReport.VirtualItemCount = Convert.ToInt32(dt.Rows[0]["Cnt"]);
            }

            // rnumber and Cnt colums are not needed in gridview
            // so remove these two columns from datatable.
            dt.Columns.Remove("rnumber");
            dt.Columns.Remove("Cnt");

            gvReport.DataSource = dt;
            gvReport.DataBind();
        }

        // Get data from db.
        private DataTable GetReportData()
        {
            object[] paramValue = GetParamValues();
            return DBContext.GetData("GetAllTasksDataReport", paramValue);
        }

        // Get filter criteria and create an object with parameter values.
        private object[] GetParamValues()
        {
            Guid userId = new Guid();// ddlUsers.SelectedIndex > 0 ? new Guid(ddlUsers.SelectedValue) : new Guid();
            string emergencyIds = GetSelectedValues(ddlEmergency);
            int? officeId = GetSelectedValue(ddlOffice);
            int? yearId = GetSelectedValue(ddlYear);
            string monthIds = GetSelectedValues(ddlMonth);
            string locationIds = GetLocationIds();
            string clusterIds = GetSelectedValues(ddlClusters);
            string orgIds = GetSelectedValues(ddlOrganizations);
            string orgTypeIds = GetSelectedValues(ddlOrgTypes);
            int pageSize = gvReport.PageSize;
            int pageIndex = gvReport.PageIndex;

            SetHFQueryString(emergencyIds, officeId, userId, yearId, monthIds, locationIds, clusterIds, orgIds, orgTypeIds);

            return new object[] { emergencyIds, officeId, userId, yearId, monthIds, locationIds, clusterIds, orgIds, orgTypeIds, pageIndex, pageSize, Convert.ToInt32(SQLPaging) };
        }

        private string GetLocationIds()
        {
            string locationIds = null;
            if ((int)LastLocationType == 1)
            {
                locationIds = ddlCountry.SelectedValue;
            }
            else if ((int)LastLocationType == 2)
            {
                locationIds = GetSelectedValues(ddlAdmin1);
            }
            else if ((int)LastLocationType == 3)
            {
                locationIds = GetSelectedValues(ddlAdmin2);
            }

            if (locationIds == "0" || locationIds == null)
            {
                return null;
            }

            return locationIds;
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
            int.TryParse(ddl.SelectedValue, out val);
            return val > 0 ? val : (int?)null;
        }

        // set hidden field. This string is being used in XML DataFeed.
        private void SetHFQueryString(string emergencyIds, int? officeId, Guid userId, int? yearId, string monthIds,
                                                string locationIds, string clusterIds, string orgIds, string orgTypeIds)
        {
            if (!string.IsNullOrEmpty(emergencyIds) || officeId != null || yearId != null ||
                !string.IsNullOrEmpty(monthIds) || !string.IsNullOrEmpty(locationIds) ||
                !string.IsNullOrEmpty(clusterIds) || !string.IsNullOrEmpty(orgIds) || string.IsNullOrEmpty(orgTypeIds))
            {
                hfReportLink.Value = string.Format("?emg={0}&cls={1}&loc={2}&y={3}&m={4}&u={5}&ot={6}&org={7}&ofc={8}",
                                                     emergencyIds, clusterIds, locationIds, yearId, monthIds,
                                                     userId, orgTypeIds, orgIds, officeId);
            }
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

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        public override void VerifyRenderingInServerForm(Control control) { }

        #endregion



        #endregion

        #region Properties & Enum

        private ReportsCommon.LocationType LastLocationType
        {
            get
            {
                if (ViewState["LastLocationType"] != null)
                {
                    return (ReportsCommon.LocationType)ViewState["LastLocationType"];
                }
                else
                {
                    return ReportsCommon.LocationType.None;
                }
            }
            set
            {
                ViewState["LastLocationType"] = value;
            }
        }

        private PagingStatus SQLPaging
        {
            get
            {
                if (ViewState["SQLPaging"] != null)
                {
                    if (ViewState["SQLPaging"].Equals("OFF"))
                        return PagingStatus.OFF;
                    else
                        return PagingStatus.ON;
                }
                else
                {
                    return PagingStatus.ON;
                }
            }
            set
            {
                ViewState["SQLPaging"] = value.ToString();
            }
        }
        enum PagingStatus
        {
            ON = 1,
            OFF = 0,
        }

        #endregion
    }
}