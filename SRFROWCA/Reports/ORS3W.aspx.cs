using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Reports
{
    public partial class ORS3W : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo.UserProfileInfo(RC.EmergencySahel2015);
                PopulateDropDowns();                
                LoadData();
            }
        }
        private void PopulateDropDowns()
        {
            LoadClustersFilter();
            LoadCountry();
            PopulateOrganizations();       
            SetDropDownOnRole();            
            PopulateMonths();
            PopulateProjects();
            PopulateAdmin1();
        }
        
        private void PopulateMonths()
        {
            int i = ddlMonth.SelectedIndex;

            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            var result = DateTime.Now.ToString("MMMM", new CultureInfo(RC.SiteCulture));
            result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);
            int monthNumber = MonthNumber.GetMonthNumber(result);
            ddlMonth.SelectedIndex = i > -1 ? i : ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(monthNumber.ToString()));
        }

        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void PopulateProjects()
        {
            ddlProjects.DataTextField = "ProjectCode";
            ddlProjects.DataValueField = "ProjectId";

            int? emgLocationId = ddlCountry.SelectedValue != "0" ? Convert.ToInt32(ddlCountry.SelectedValue) : (int?)null;
            int? emgClsuterId = ddlCluster.SelectedValue != "0" ? Convert.ToInt32(ddlCluster.SelectedValue) : (int?)null;
            int? orgId = ddlOrganizations.SelectedValue != "0" ? Convert.ToInt32(ddlOrganizations.SelectedValue) : (int?)null;

            string orgIDs = null;
            if (UserInfo.Organization > 0)
            {
                orgId = UserInfo.Organization;
            }
            
            ddlProjects.DataSource = DBContext.GetData("GetProjectsOnClusterCountryAndOrg", new object[] { emgLocationId, emgClsuterId, orgId, orgIDs });
            ddlProjects.DataBind();
            ddlProjects.Items.Insert(0, new ListItem("--- Select Project ---", "0"));
        }
        private void PopulateOrganizations()
        {
            ddlOrganizations.DataValueField = "OrganizationId";
            ddlOrganizations.DataTextField = "OrganizationAcronym";
            int? orgId = null;
            string projIDs = null;

            if (UserInfo.Organization > 0)
                orgId = UserInfo.Organization;

            projIDs = RC.GetSelectedValues(ddlProjects);

            ddlOrganizations.DataSource = GetOrganizations(orgId, projIDs);
            ddlOrganizations.DataBind();

            if (UserInfo.Organization > 0)
            {
                ddlOrganizations.SelectedValue = UserInfo.Organization.ToString();
                ddlOrganizations.Enabled = false;
            }
            ddlOrganizations.Items.Insert(0, new ListItem("--- Select Organization ---", "0"));
        }

        private DataTable GetOrganizations(int? orgId, string projIDs)
        {
            return DBContext.GetData("GetOrganizations", new object[] { orgId, projIDs });
        }
        private void PopulateAdmin1()
        {
            string countryIds = RC.GetSelectedValues(ddlCountry);
            if (countryIds != null)
            {
                ddlAdmin1.DataValueField = "LocationId";
                ddlAdmin1.DataTextField = "LocationName";

                ddlAdmin1.DataSource = DBContext.GetData("GetAdmin1OfEmergnecyLocation", new object[] { countryIds });
                ddlAdmin1.DataBind();
                ddlAdmin1.Items.Insert(0, new ListItem("--- Select Admin1 ---", "0"));
            }
        }

         protected void ddlOrganizations_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            PopulateProjects();
        }

        protected void ddlProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            PopulateOrganizations();
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {

            PopulateProjects();
        }  

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateProjects();
               PopulateAdmin1();
        }
       
        protected void btnSearch2_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void SetDropDownOnRole()
        {
            if (RC.IsClusterLead(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
                ddlCountry.Enabled = false;
                ddlCountry.BackColor = Color.LightGray;
                ddlCluster.Enabled = false;
                ddlCluster.BackColor = Color.LightGray;
            }

            if (RC.IsRegionalClusterLead(this.User))
            {
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
                ddlCluster.Enabled = false;
                ddlCluster.BackColor = Color.LightGray;
            }

            if (RC.IsCountryAdmin(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlCountry.Enabled = false;
                ddlCountry.BackColor = Color.LightGray;
            }
            if (RC.IsDataEntryUser(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlCountry.Enabled = false;
                ddlCountry.BackColor = Color.LightGray;

                ddlOrganizations.SelectedValue = UserInfo.Organization.ToString();
                ddlOrganizations.Enabled = false;
                ddlOrganizations.BackColor = Color.LightGray;
            }
        }     
      

        protected void btnReset_Click(object sender, EventArgs e)
        {
            var result = DateTime.Now.ToString("MMMM", new CultureInfo(RC.SiteCulture));
            result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);
            int monthNumber = MonthNumber.GetMonthNumber(result);
            ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(monthNumber.ToString()));

            ddlStatus.SelectedIndex = 0;

            if (ddlProjects.Items.Count > 0)
            {
                ddlProjects.SelectedIndex = 0;
            }

            if (RC.IsClusterLead(this.User) || RC.IsRegionalClusterLead(this.User))
            {
                ddlOrganizations.SelectedValue = "0";
                ddlAdmin1.SelectedValue = "0";
            }

            if (RC.IsCountryAdmin(this.User))
            {
                ddlCluster.SelectedValue = "0";
                ddlOrganizations.SelectedValue = "0";
                ddlAdmin1.SelectedValue = "0";
            }
            if (RC.IsDataEntryUser(this.User))
            {
                ddlCluster.SelectedValue = "0";
            }

            if (RC.IsAdmin(this.User))
            {
                ddlOrganizations.SelectedValue = "0";
                ddlAdmin1.SelectedValue = "0";
                ddlCluster.SelectedValue = "0";
                ddlCountry.SelectedValue = "0";
            }
            else
            {
                SetDropDownOnRole();
            }


            LoadData();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            GridView gvExport = new GridView();

            DataTable dt = GetData();
            RemoveColumnsFromDataTable(dt);
            gvExport.DataSource = dt;
            gvExport.DataBind();

            string fileName = "ORS3W";
            string fileExtention = ".xls";
            ExportUtility.ExportGridView(gvExport, fileName, fileExtention, Response);
        }

     
        internal override void BindGridData()
        {
            PopulateDropDowns();
            SetDropDownOnRole();
            LoadData();
        }

        private void LoadData()
        {
            DataTable dt = GetData();
            if (dt.Rows.Count > 0)
            {
                gvActivity.VirtualItemCount = Convert.ToInt32(dt.Rows[0]["VirtualCount"].ToString());
            }
            gvActivity.DataSource = dt;
            gvActivity.DataBind();
        }

        private void LoadCountry()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));
        }

        private DataTable GetData()
        {
            int? emgLocationId = ddlCountry.SelectedValue != "0" ? Convert.ToInt32(ddlCountry.SelectedValue) : (int?)null;
            int? emgClsuterId = ddlCluster.SelectedValue != "0" ? Convert.ToInt32(ddlCluster.SelectedValue) : (int?)null;
            int? orgId = ddlOrganizations.SelectedValue != "0" ? Convert.ToInt32(ddlOrganizations.SelectedValue) : (int?)null;
            int? prjId = ddlProjects.SelectedValue != "0" ? Convert.ToInt32(ddlProjects.SelectedValue) : (int?)null;
            int? admin1 = ddlAdmin1.SelectedValue != "0" ? Convert.ToInt32(ddlAdmin1.SelectedValue) : (int?)null;
            int? month = ddlMonth.SelectedValue != "0" ? Convert.ToInt32(ddlMonth.SelectedValue) : (int?)null;
            string status = ddlStatus.SelectedValue != "0" ? ddlStatus.SelectedValue : null;
            int pageSize = gvActivity.PageSize;
            int pageIndex = gvActivity.PageIndex;

            return DBContext.GetData("GetORS3WData", new object[]{prjId,orgId,emgLocationId,
                                                                    admin1,emgClsuterId,month,status,
                                                                    RC.SelectedSiteLanguageId, pageSize, pageIndex});
        }

        private void LoadClustersFilter()
        {
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            ddlCluster.Items.Insert(0, new ListItem("--- Select Cluster ---", "0"));
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "ActivityListing", this.User);
        }

        protected void gvActivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvActivity.PageIndex = e.NewPageIndex;
            gvActivity.SelectedIndex = -1;
            LoadData();
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            try
            {
                dt.Columns.Remove("ActivityId");
                dt.Columns.Remove("ClusterId");
                dt.Columns.Remove("IndicatorId");
                dt.Columns.Remove("IndicatorDetailId");
                dt.Columns.Remove("ClusterName");
                dt.Columns.Remove("ShortObjective");
                dt.Columns.Remove("EmergencyClusterId");
                dt.Columns.Remove("EmergencyLocationId");
                dt.Columns.Remove("ShortObjective");

            }
            catch { }
        }
    }
}