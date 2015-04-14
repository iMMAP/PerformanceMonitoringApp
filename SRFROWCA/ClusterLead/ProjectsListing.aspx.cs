using System;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using System.Web;

namespace SRFROWCA.ClusterLead
{
    public partial class ProjectsListing : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserInfo.UserProfileInfo(RC.SelectedEmergencyId);
            }

            PopulateControls();
            LoadProjects();
            DisableDropDowns();
        }

        internal override void BindGridData()
        {
            LoadProjects();
        }

        private void PopulateControls()
        {
            PopulateOrganizations();
            PopulateCountries();
            PopulateClusters();

            SetComboValues();
        }

        private void PopulateClusters()
        {
            UI.FillEmergnecyClusters(ddlClusters, RC.SelectedEmergencyId);
            RC.AddSelectItemInList(ddlClusters, "Select");

            UI.FillEmergnecyClusters(ddlSecClusters, RC.SelectedEmergencyId);
            RC.AddSelectItemInList(ddlSecClusters, "Select");
        }

        private void SetComboValues()
        {
            if (RC.IsClusterLead(this.User) || RC.IsRegionalClusterLead(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlClusters.SelectedValue = UserInfo.EmergencyCluster.ToString();
            }

            if (RC.IsCountryAdmin(this.User) || RC.IsDataEntryUser(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
            }
        }

        private void DisableDropDowns()
        {
            if (RC.IsClusterLead(this.User))
            {
                RC.EnableDisableControls(ddlClusters, false);
                RC.EnableDisableControls(ddlCountry, false);
            }

            if (RC.IsRegionalClusterLead(this.User))
            {
                RC.EnableDisableControls(ddlClusters, false);
            }

            if (RC.IsCountryAdmin(this.User) || RC.IsDataEntryUser(this.User))
            {
                RC.EnableDisableControls(ddlCountry, false);
            }
        }

        private void PopulateOrganizations()
        {
            int tempVal = 0;
            int.TryParse(ddlClusters.SelectedValue, out tempVal);
            int? clusterId = tempVal > 0 ? tempVal : (int?)null;

            tempVal = 0;
            int.TryParse(ddlCountry.SelectedValue, out tempVal);
            int? countryID = tempVal > 0 ? tempVal : (int?)null;

            DataTable dt = RC.GetProjectsOrganizations(countryID, clusterId);
            UI.FillOrganizations(ddlOrg, dt);

            if (ddlOrg.Items.Count > 0)
            {
                ListItem item = new ListItem("Select", "0");
                ddlOrg.Items.Insert(0, item);
            }
        }

        private void PopulateCountries()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.SelectedEmergencyId);
            RC.AddSelectItemInList(ddlCountry, "Select");
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {
            DataTable dt = GetProjects();//DBContext.GetData("GetProjects", new object[] { countryID, clusterId, null, orgId, null, DBNull.Value, DBNull.Value, null, null, 1 });
            //RemoveColumnsFromDataTable(dt);

            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();

            ExportUtility.ExportGridView(gv, "ProjectListing", ".xls", Response, true);
        }

        protected void ExportToPDF(object sender, EventArgs e)
        {
            ExportToPDF(null);
        }

        private void ExportToPDF(int? projectId)
        {
            int year = RC.SelectedEmergencyId == 1 ? 2014 : 2015;

            DataTable dtResults = new DataTable();

            int tempVal = 0;
            if (ddlClusters.Visible)
            {
                int.TryParse(ddlClusters.SelectedValue, out tempVal);
            }
            int? clusterId = tempVal > 0 ? tempVal : UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;

            tempVal = 0;
            if (ddlSecClusters.Visible)
            {
                int.TryParse(ddlSecClusters.SelectedValue, out tempVal);
            }
            int? secClusterId = tempVal > 0 ? tempVal : (int?)null;

            tempVal = 0;
            if (ddlCountry.Visible)
            {
                tempVal = 0;
                int.TryParse(ddlCountry.SelectedValue, out tempVal);
            }
            int? countryID = tempVal > 0 ? tempVal : UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;

            string projCode = txtProjectCode.Text.Trim().Length > 0 ? txtProjectCode.Text.Trim() : null;

            int? orgId = RC.GetSelectedIntVal(ddlOrg);

            if (orgId == 0)
            {
                if (RC.IsDataEntryUser(User))
                    orgId = UserInfo.Organization;
                else
                    orgId = (int?)null;
            }


            string projectStatus = ddlStatus.SelectedValue == "0" ? null : ddlStatus.SelectedValue;
            dtResults = DBContext.GetData("uspGetReports2", new object[] {countryID, clusterId,projCode, orgId, RC.SelectedSiteLanguageId, 
                                                                                        year, projectId, projectStatus, secClusterId});

            if (dtResults.Rows.Count > 0)
            {
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");

                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}.pdf", fileName));
                Response.BinaryWrite(WriteDataEntryPDF.GeneratePDF(dtResults, projectId, null).ToArray());
            }
        }

        protected void cblReportingStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        protected void gvProjects_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewProject")
            {
                //Session["ViewProjectId"] = e.CommandArgument.ToString();
                Response.Redirect("~/ClusterLead/ProjectDetails.aspx?pid=" + e.CommandArgument.ToString());
            }
            else if (e.CommandName == "PrintReport")
            {
                ExportToPDF(Convert.ToInt32(e.CommandArgument));
            }
        }

        protected void gvProjects_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetProjects();
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvProjects.DataSource = dt;
                gvProjects.DataBind();
            }
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

        protected void gvProjects_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProjects.PageIndex = e.NewPageIndex;
            gvProjects.SelectedIndex = -1;
            LoadProjects();
        }

        private void LoadProjects()
        {
            gvProjects.DataSource = GetProjects();
            gvProjects.DataBind();
        }

        private DataTable GetProjects()
        {
            int tempVal = 0;
            if (ddlClusters.Visible)
            {
                int.TryParse(ddlClusters.SelectedValue, out tempVal);
            }
            int? clusterId = tempVal > 0 ? tempVal : UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;

            tempVal = 0;
            if (ddlSecClusters.Visible)
            {
                int.TryParse(ddlSecClusters.SelectedValue, out tempVal);
            }
            int? secClusterId = tempVal > 0 ? tempVal : (int?)null;

            tempVal = 0;
            if (ddlCountry.Visible)
            {
                tempVal = 0;
                int.TryParse(ddlCountry.SelectedValue, out tempVal);
            }
            int? countryID = tempVal > 0 ? tempVal : UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;

            string projCode = txtProjectCode.Text.Trim().Length > 0 ? txtProjectCode.Text.Trim() : null;
            int? orgId = RC.GetSelectedIntVal(ddlOrg);

            if (orgId == 0)
            {
                if (RC.IsDataEntryUser(User))
                    orgId = UserInfo.Organization;
                else
                    orgId = (int?)null;
            }

            int year = RC.SelectedEmergencyId == 1 ? 2014 : 2015;
            int? admin1 = null;
            int? cbReported = null;
            int? cbFunded = null;

            string projectStatus = ddlStatus.SelectedValue == "0" ? null : ddlStatus.SelectedValue;

            return DBContext.GetData("GetProjects", new object[] {countryID, clusterId,projCode, orgId, admin1, DBNull.Value, 
                                                                            DBNull.Value, cbFunded, cbReported, 1,  year, 
                                                                            projectStatus, secClusterId});
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadProjects();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddlClusters.SelectedIndex = 0;
            ddlCountry.SelectedIndex = 0;
            txtProjectCode.Text = "";
            ddlOrg.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            ddlSecClusters.SelectedIndex = 0;
            LoadProjects();
        }
    }
}