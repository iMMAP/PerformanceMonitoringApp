using System;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Web.Script.Services;

namespace SRFROWCA.OrsProject
{
    public partial class ProjectsListing : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSearch.UniqueID;
            if (!RC.IsAuthenticated(this.User))
                btnCreateProject.Visible = false;

            if (IsPostBack) return;

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserInfo.UserProfileInfo(RC.SelectedEmergencyId);
            }

            if (RC.IsRegionalClusterLead(this.User))
            {
                btnCreateProject.Visible = false;
            }

            if (Request.QueryString["year"] != null)
            {
                if (Request.QueryString["year"] == "2015")
                    ddlFrameworkYear.SelectedValue = "11";
            }

            PopulateControls();
            LoadProjects();
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
            if (!RC.IsRegionalClusterLead(this.User))
                UI.SetUserCountry(ddlCountry);
            UI.SetUserCluster(ddlSecClusters);
            SetOrganization();
        }

        private void SetOrganization()
        {
            if (RC.IsDataEntryUser(this.User) && UserInfo.Organization > 0)
            {
                ddlOrg.SelectedValue = UserInfo.Organization.ToString();
            }
        }

        private void PopulateClusters()
        {
            UI.FillEmergnecyClusters(ddlClusters, RC.SelectedEmergencyId);
            RC.AddSelectItemInList(ddlClusters, "Select");

            UI.FillEmergnecyClusters(ddlSecClusters, RC.SelectedEmergencyId);
            RC.AddSelectItemInList(ddlSecClusters, "Select");
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

        protected void ExportToPDF(object sender, EventArgs e)
        {
            ExportToPDF(null);
        }

        private void ExportToPDF(int? projectId)
        {
            int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);
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

            int? isFunded = null;
            if (cbFuned.Checked && cbNotFunded.Checked)
            {
                isFunded = null;
            }
            else if (cbFuned.Checked)
            {
                isFunded = 1;
            }
            else if (cbNotFunded.Checked)
            {
                isFunded = 0;
            }

            int? isLCB = null;
            if (cbLCB.Checked)
                isLCB = 1;

            string projectStatus = ddlStatus.SelectedValue == "0" ? null : ddlStatus.SelectedValue;
            DataTable dtProjects = DBContext.GetData("GetProjectsWithFullDetails", new object[] {countryID, clusterId,projCode, orgId, RC.SelectedSiteLanguageId, 
                                                                                        yearId, projectId, projectStatus, secClusterId, isOPS, isFunded, isLCB});
            if (dtProjects.Rows.Count > 0)
            {
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");

                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}.pdf", fileName));
                Response.BinaryWrite(WriteDataEntryPDF.GenerateProjectsListingPDF(dtProjects, true, yearId).ToArray());
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
                string queryString = QueryStringModule.Encrypt("pid=" + e.CommandArgument.ToString());
                Response.Redirect("~/ClusterLead/ProjectDetails.aspx?" + queryString);
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

                if (!string.IsNullOrEmpty(projectId.Trim())
                    && !string.IsNullOrEmpty(projOrgId.Trim())
                    && !string.IsNullOrEmpty(orgId.Trim()))
                {
                    projectId = Utils.EncryptQueryString(projectId);
                    projOrgId = Utils.EncryptQueryString(projOrgId);
                    orgId = Utils.EncryptQueryString(orgId);
                    Response.Redirect("~/OrsProject/CreateProject.aspx?" + "pid=" + projectId + "&poid=" + projOrgId + "&oid=" + orgId);
                }
            }

            if (e.CommandName == "DeleteProject")
            {
                int projId = Convert.ToInt32(e.CommandArgument);
                DeleteProject(projId);
            }
        }

        private void DeleteProject(int projectId)
        {
            if (!IsProjectBeingUsed(projectId))
            {
                DBContext.Delete("DeleteProject", new object[] { projectId, DBNull.Value });
                ShowMessage("Project Deleted Successfully!", RC.NotificationType.Success);
                LoadProjects();
            }
            else
            {
                ShowMessage("This project can not be deleted becasue its being used in reports!", RC.NotificationType.Error);
            }
        }

        private bool IsProjectBeingUsed(int projectId)
        {
            return ((DBContext.GetData("IsReportExistsForProject", new object[] { projectId }).Rows.Count > 0));
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
            DataTable dt = GetProjects();
            if (dt.Rows.Count > 0)
            {
                gvProjects.VirtualItemCount = Convert.ToInt32(dt.Rows[0]["VirtualCount"].ToString());
            }
            gvProjects.DataSource = dt;
            gvProjects.DataBind();

            MakeSummaryInfo(dt);
        }

        private void MakeSummaryInfo(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                lblProjects.Text = dt.Rows[0]["TotalProjects"].ToString();
                lblOrgs.Text = dt.Rows[0]["TotalOrgs"].ToString();
                lblCountry.Text = dt.Rows[0]["TotalCountries"].ToString();
                lblClusters.Text = dt.Rows[0]["TotalClusters"].ToString();
                string req = dt.Rows[0]["TotalRequested"].ToString();
                string funded = dt.Rows[0]["TotalFunded"].ToString();
                lblPercentFunded.Text = dt.Rows[0]["TotalPercentageFunded"].ToString() + "%";
                decimal? requested = null;
                if (!string.IsNullOrEmpty(req))
                {
                    requested = Convert.ToDecimal(req);
                    if ((requested / 1000000000) > 1)
                    {
                        requested /= 1000000000;
                        lblRequiremens.Text = string.Format("{0:0.00}", requested) + "B";
                    }
                    else if ((requested / 1000000) > 1)
                    {
                        requested /= 1000000;
                        lblRequiremens.Text = string.Format("{0:0.00}", requested) + "M";
                    }
                    else
                    {
                        long reqAmount = Convert.ToInt64(requested);
                        lblRequiremens.Text = UI.GetThousandSeparator(reqAmount.ToString());
                    }

                }
                else
                    lblRequiremens.Text = "";

                decimal? fundedAmount = null;
                if (!string.IsNullOrEmpty(funded))
                {
                    fundedAmount = Convert.ToDecimal(funded);

                    if ((fundedAmount / 1000000000) > 1)
                    {
                        fundedAmount /= 1000000000;
                        lblFunded.Text = string.Format("{0:0.00}", fundedAmount) + "B";
                    }
                    else if ((fundedAmount / 1000000) > 1)
                    {
                        fundedAmount /= 1000000;
                        lblFunded.Text = string.Format("{0:0.00}", fundedAmount) + "M";
                    }
                    else
                    {
                        long tempFundedAmount = Convert.ToInt64(fundedAmount);
                        lblFunded.Text = UI.GetThousandSeparator(tempFundedAmount.ToString());
                    }

                }
                else
                    lblFunded.Text = "";
            }
            else
            {
                lblProjects.Text = "";
                lblOrgs.Text = "";
                lblCountry.Text = "";
                lblClusters.Text = "";
                lblPercentFunded.Text = "";
                lblRequiremens.Text = "";
                lblFunded.Text = "";
            }
        }

        private DataTable GetProjects()
        {
            int tempVal = 0;

            if (ddlCountry.Visible)
            {
                tempVal = 0;
                int.TryParse(ddlCountry.SelectedValue, out tempVal);
            }
            int? emgLocationId = tempVal > 0 ? tempVal : (int?)null;

            tempVal = 0;
            if (ddlSecClusters.Visible)
            {
                int.TryParse(ddlSecClusters.SelectedValue, out tempVal);
            }
            int? secClusterId = tempVal > 0 ? tempVal : (int?)null;

            tempVal = 0;
            if (ddlClusters.Visible)
            {
                int.TryParse(ddlClusters.SelectedValue, out tempVal);
            }
            int? emgClusterId = tempVal > 0 ? tempVal : (int?)null;

            string projCode = txtProjectCode.Text.Trim().Length > 0 ? txtProjectCode.Text.Trim() : null;

            tempVal = RC.GetSelectedIntVal(ddlOrg);
            int? orgId = tempVal > 0 ? tempVal : (int?)null;

            int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);
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

            int? isFunded = null;
            if (cbFuned.Checked && cbNotFunded.Checked)
            {
                isFunded = null;
            }
            else if (cbFuned.Checked)
            {
                isFunded = 1;
            }
            else if (cbNotFunded.Checked)
            {
                isFunded = 0;
            }

            int? isLCB = null;
            if (cbLCB.Checked)
                isLCB = 1;

            int? pageSize = gvProjects.PageSize;
            int? pageIndex = gvProjects.PageIndex;

            int? userOrg = 
            (RC.IsDataEntryUser(this.User) && orgId > 0 && orgId == UserInfo.Organization) ?
                UserInfo.Organization : (int?)null;
            

            return DBContext.GetData("GetProjectsListing", new object[] {emgLocationId, emgClusterId, projCode, orgId, 
                                                                            RC.SelectedSiteLanguageId,  yearId, projectStatus, 
                                                                            secClusterId, isOPS, isFunded, isLCB,
                                                                            pageIndex, pageSize, userOrg});
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadProjects();
        }

        protected void SelectedIndexChanged(object sender, EventArgs e)
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

        protected void gvProjects_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (!this.User.Identity.IsAuthenticated || RC.IsRegionalClusterLead(this.User) || yearId == (int)RC.Year._2015)
                {
                    e.Row.Cells[11].Visible = false;
                    e.Row.Cells[12].Visible = false;
                    e.Row.Cells[13].Visible = false;
                    e.Row.Cells[14].Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RC.FormatThousandSeperator(e.Row, "lblOriginalRequest");
                RC.FormatThousandSeperator(e.Row, "lblFundedAmount");

                if (!this.User.Identity.IsAuthenticated || RC.IsRegionalClusterLead(this.User) || yearId == (int)RC.Year._2015)
                {
                    e.Row.Cells[11].Visible = false;
                    e.Row.Cells[12].Visible = false;
                    e.Row.Cells[13].Visible = false;
                    e.Row.Cells[14].Visible = false;
                }
                else
                {
                    ImageButton deleteButton = e.Row.FindControl("btnDelete") as ImageButton;
                    if (deleteButton != null)
                    {
                        deleteButton.Attributes.Add("onclick", "javascript:return " +
                        "confirm('Are you sure you want to delete this project?')");
                    }

                    int isOPS = 0;
                    int.TryParse(gvProjects.DataKeys[e.Row.RowIndex]["IsOPS"].ToString(), out isOPS);
                    if (isOPS == 1)
                    {
                        HideFunctionButtons(e.Row, "btnEdit");
                        HideFunctionButtons(e.Row, "btnDelete");
                    }

                    if (RC.IsAdmin(this.User)) return;

                    int orgId = 0;
                    int.TryParse(gvProjects.DataKeys[e.Row.RowIndex].Values["UserOrgId"].ToString(), out orgId);
                    int userOrgId = 0;
                    int.TryParse(gvProjects.DataKeys[e.Row.RowIndex].Values["UserOrgId"].ToString(), out userOrgId);
                    int isPartner = 0;
                    int.TryParse(gvProjects.DataKeys[e.Row.RowIndex].Values["IsPartner"].ToString(), out isPartner);

                    int emgLocationId = 0;
                    int.TryParse(gvProjects.DataKeys[e.Row.RowIndex].Values["EmergencyLocationId"].ToString(), out emgLocationId);

                    int emgClusterId = 0;
                    int.TryParse(gvProjects.DataKeys[e.Row.RowIndex].Values["EmergencyClusterId"].ToString(), out emgClusterId);

                    if (RC.IsCountryAdmin(this.User))
                    {
                        if (!(emgLocationId == UserInfo.EmergencyCountry))
                        {
                            HideFunctionButtons(e.Row, "imgbtnReport");
                            HideFunctionButtons(e.Row, "imgbtnTargets");
                            HideFunctionButtons(e.Row, "imgbtnPartners");
                            HideFunctionButtons(e.Row, "btnEdit");
                            HideFunctionButtons(e.Row, "btnDelete");
                        }
                    }
                    else if (RC.IsClusterLead(this.User))
                    {
                        if (!(emgClusterId == UserInfo.EmergencyCluster && emgLocationId == UserInfo.EmergencyCountry))
                        {
                            HideFunctionButtons(e.Row, "imgbtnReport");
                            HideFunctionButtons(e.Row, "imgbtnTargets");
                            HideFunctionButtons(e.Row, "imgbtnPartners");
                            HideFunctionButtons(e.Row, "btnEdit");
                            HideFunctionButtons(e.Row, "btnDelete");
                        }
                    }
                    else if (!(orgId == UserInfo.Organization && emgLocationId == UserInfo.EmergencyCountry))
                    {
                       
                            HideFunctionButtons(e.Row, "imgbtnReport");
                            HideFunctionButtons(e.Row, "imgbtnTargets");
                            HideFunctionButtons(e.Row, "imgbtnPartners");
                            HideFunctionButtons(e.Row, "btnEdit");
                            HideFunctionButtons(e.Row, "btnDelete");
                       
                    }
                    else if (!(isPartner == 0 && orgId == UserInfo.Organization && emgLocationId == UserInfo.EmergencyCountry))
                    {

                        HideFunctionButtons(e.Row, "imgbtnTargets");
                        HideFunctionButtons(e.Row, "imgbtnPartners");
                        HideFunctionButtons(e.Row, "btnEdit");
                        HideFunctionButtons(e.Row, "btnDelete");

                    }
                }
            }
        }

        private void HideFunctionButtons(GridViewRow row, string controlName)
        {
            ImageButton btnTargets = row.FindControl(controlName) as ImageButton;
            if (btnTargets != null)
                btnTargets.Visible = false;
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}