using System;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using System.Web;
using System.Web.UI;

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

            if (RC.IsRegionalClusterLead(this.User))
            {
                btnCreateProject.Visible = false;
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

            if (RC.IsCountryAdmin(this.User) || RC.IsDataEntryUser(this.User) || RC.IsOCHAStaff(this.User))
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

            if (RC.IsCountryAdmin(this.User) || RC.IsDataEntryUser(this.User) || RC.IsOCHAStaff(this.User))
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
            int year = 2015;

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

            int? isOPS = null;
            if (cbIsOPS.Checked && cbIsORS.Checked)
            {
                isOPS = null;
            }
            else if (cbIsOPS.Checked)
            {
                isOPS = 1;
            }
            else if (cbIsORS.Checked)
            {
                isOPS = 0;
            }

            string projectStatus = ddlStatus.SelectedValue == "0" ? null : ddlStatus.SelectedValue;
            DataTable dtProjects = DBContext.GetData("GetProjectsWithFullDetails", new object[] {countryID, clusterId,projCode, orgId, RC.SelectedSiteLanguageId, 
                                                                                        year, projectId, projectStatus, secClusterId, isOPS});
            if (dtProjects.Rows.Count > 0)
            {
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");

                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}.pdf", fileName));
                Response.BinaryWrite(WriteDataEntryPDF.GenerateProjectsListingPDF(dtProjects, true).ToArray());
            }
            else
            {
                ShowMessage("NO Project To Export!", RC.NotificationType.Info, true, 2000);
            }
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
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

            if (e.CommandName == "EditProject")
            {
                int rowIndex = 0;
                int.TryParse(e.CommandArgument.ToString(), out rowIndex);
                string projectId = gvProjects.DataKeys[rowIndex].Values["ProjectId"].ToString();
                string projOrgId = gvProjects.DataKeys[rowIndex].Values["ProjectOrganizationId"].ToString();
                string orgId = gvProjects.DataKeys[rowIndex].Values["OrganizationId"].ToString();
                Response.Redirect("~/Pages/CreateProject.aspx?pid=" + projectId + "&poid=" + projOrgId + "&oid=" + orgId);
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
            int? emgClusterId = tempVal > 0 ? tempVal : UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;

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
            int? emgLocationId = tempVal > 0 ? tempVal : UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;

            string projCode = txtProjectCode.Text.Trim().Length > 0 ? txtProjectCode.Text.Trim() : null;
            int? orgId = RC.GetSelectedIntVal(ddlOrg);

            if (orgId == 0)
            {
                if (RC.IsDataEntryUser(User))
                    orgId = UserInfo.Organization;
                else
                    orgId = (int?)null;
            }

            int year = 2015;
            string projectStatus = ddlStatus.SelectedValue == "0" ? null : ddlStatus.SelectedValue;
            int? isOPS = null;

            if (cbIsOPS.Checked && cbIsORS.Checked)
            {
                isOPS = null;
            }
            else if (cbIsOPS.Checked)
            {
                isOPS = 1;
            }
            else if (cbIsORS.Checked)
            {
                isOPS = 0;
            }


            return DBContext.GetData("GetProjectsListing", new object[] {emgLocationId, emgClusterId, projCode, orgId, 
                                                                            RC.SelectedSiteLanguageId,  year, projectStatus, 
                                                                            secClusterId, isOPS});
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
            cbIsOPS.Checked = false;
            cbIsORS.Checked = false;
            LoadProjects();
        }

        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/CreateProject.aspx");
        }

        protected void gvProjects_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (RC.IsRegionalClusterLead(this.User) || (!this.User.Identity.IsAuthenticated))
                {
                    ImageButton btnEdit = e.Row.FindControl("btnEdit") as ImageButton;
                    if (btnEdit != null)
                    {
                        btnEdit.Visible = false;
                    }
                }
            }
        }
    }
}