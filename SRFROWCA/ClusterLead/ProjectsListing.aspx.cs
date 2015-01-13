using System;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class ProjectsListing : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            PopulateControls();
            LoadProjects();

            if (RC.IsClusterLead(this.User))
            {
                ddlClusters.Visible = false;
            }
            else
            {
                PopulateClusters();
            }

            if (RC.IsRegionalClusterLead(User) || RC.IsAdmin(this.User))
            {
                ddlCountry.Enabled = true;
            }
        }

        private void PopulateControls()
        {
            //if (!RC.IsDataEntryUser(User))
            PopulateOrganizations();
            //else
            //{
            //    ddlOrg.Visible = false;
            //    divOrganization.Visible = false;
            //}

            //if (RC.IsRegionalClusterLead(User))
            PopulateCountries();

            PopulateLocations();
            //PopulateProjectStatus();
        }

        private void PopulateClusters()
        {
            int emgId = RC.SelectedEmergencyId;
            UI.FillEmergnecyClusters(ddlClusters, emgId);
            RC.AddSelectItemInList(ddlClusters, "All");
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
                ListItem item = new ListItem("All", "0");
                ddlOrg.Items.Insert(0, item);
            }
        }

        private void PopulateCountries()
        {
            UI.FillCountry(ddlCountry);
            if (ddlCountry.Items.Count > 0)
            {
                ListItem item = new ListItem("All", "0");
                ddlCountry.Items.Insert(0, item);
            }
        }

        private void PopulateLocations()
        {
            int tempVal = 0;
            if (ddlCountry.Visible)
            {
                int.TryParse(ddlCountry.SelectedValue, out tempVal);
            }

            int countryID = tempVal > 0 ? tempVal : UserInfo.Country > 0 ? UserInfo.Country : 0;

            //UI.FillAdmin1(ddlAdmin1, countryID);

            //ListItem item = new ListItem("All", "0");
            //if (ddlAdmin1.Items.Count > 0)
            //{
            //    ddlAdmin1.Items.Insert(0, item);
            //}
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {
            //int? clusterId = UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;
            //int? countryID = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;
            //int? orgId = (RC.IsDataEntryUser(User))?orgId = UserInfo.Organization:orgId = (int?)null;

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
            int tempVal = 0;
            if (ddlClusters.Visible)
            {
                int.TryParse(ddlClusters.SelectedValue, out tempVal);
            }
            int? clusterId = tempVal > 0 ? tempVal : UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;

            tempVal = 0;
            if (ddlCountry.Visible)
            {
                tempVal = 0;
                int.TryParse(ddlCountry.SelectedValue, out tempVal);
            }

            int? countryID = tempVal > 0 ? tempVal : UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;
            projectId = projectId > 0 ? projectId : txtProjectCode.Text.Trim().Length > 0 ? (int?)Convert.ToInt32(txtProjectCode.Text.Trim()) : null;

            int? orgId = RC.GetSelectedIntVal(ddlOrg);

            if (orgId == 0)
            {
                if (RC.IsDataEntryUser(User))
                    orgId = UserInfo.Organization;
                else
                    orgId = (int?)null;
            }

            //int? admin1 = RC.GetSelectedIntVal(ddlAdmin1);
            //if (admin1 == 0)
            //{
            //    admin1 = (int?)null;
            //}

            //int? cbReported = cblReportingStatus.SelectedIndex > -1 ? RC.GetSelectedIntVal(cblReportingStatus) : (int?)null;
            //int? cbFunded = cblFundingStatus.SelectedIndex > -1 ? RC.GetSelectedIntVal(cblFundingStatus) : (int?)null;

            int? admin1 = null;
            int? cbReported = null;
            int? cbFunded = null;

            //DataTable dtResults = DBContext.GetData("uspGetReports", new object[] { projectId, null, null, null, RC.GetCurrentUserId, RC.SelectedSiteLanguageId, countryID, clusterId, orgId, admin1, cbFunded, cbReported, });
            DataTable dtResults = DBContext.GetData("uspGetReports", new object[] { projectId, null, null, null, RC.GetCurrentUserId, RC.SelectedSiteLanguageId, countryID, clusterId, orgId, admin1, cbFunded, cbReported, });

            if (dtResults.Rows.Count > 0)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}-{1}.pdf", UserInfo.CountryName, DateTime.Now.ToString("yyyyMMddHHmmss")));
                Response.BinaryWrite(WriteDataEntryPDF.GeneratePDF(dtResults, projectId, null).ToArray());
            }
        }

        //private void PopulateProjectStatus()
        //{

        //}

        //protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadProjects();
        //}

        //protected void ddlAdmin1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadProjects();
        //}

        //protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlCountry.SelectedIndex > -1)
        //    {
        //        //UserInfo.Country = Convert.ToInt32(ddlCountry.SelectedValue);
        //        PopulateOrganizations();
        //        PopulateLocations();
        //        LoadProjects();
        //    }
        //}

        //protected void ddlAdmin2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadProjects();
        //}

        //protected void ddlProjStatus_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

        protected void cblReportingStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        //protected void txtProjectCode_TextChanged(object sender, EventArgs e)
        //{
        //    LoadProjects();
        //}

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
            //if (RC.SelectedEmergencyId == 1)
            {
                gvProjects.DataSource = GetProjects();
                gvProjects.DataBind();
            }
            //else
            //{
            //    gvProjects.DataSource = new DataTable();
            //    gvProjects.DataBind();
            //}
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

            //int? admin1 = RC.GetSelectedIntVal(ddlAdmin1);
            //if (admin1 == 0)
            //{
            //    admin1 = (int?)null;
            //}

            int year = RC.SelectedEmergencyId == 1 ? 2014 : 2015;

            //int? cbReported = cblReportingStatus.SelectedIndex > -1 ? RC.GetSelectedIntVal(cblReportingStatus) : (int?)null;
            //int? cbFunded = cblFundingStatus.SelectedIndex > -1 ? RC.GetSelectedIntVal(cblFundingStatus) : (int?)null;

            int? admin1 = null;
            int? cbReported = null;
            int? cbFunded = null;

            return DBContext.GetData("GetProjects", new object[] {countryID, clusterId,projCode, orgId, admin1, DBNull.Value, 
                                                                            DBNull.Value, cbFunded, cbReported, 1,  year});
        }

        //protected void ddlClusters_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadProjects();
        //}

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
            LoadProjects();
        }
    }
}