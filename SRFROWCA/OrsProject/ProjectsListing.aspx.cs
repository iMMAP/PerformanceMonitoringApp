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

            string projectStatus = ddlStatus.SelectedValue == "0" ? null : ddlStatus.SelectedValue;
            DataTable dtProjects = DBContext.GetData("GetProjectsWithFullDetails", new object[] {countryID, clusterId,projCode, orgId, RC.SelectedSiteLanguageId, 
                                                                                        yearId, projectId, projectStatus, secClusterId, isOPS, isFunded});
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

                //req = req.Substring(0, req.IndexOf('.'));
                ////funded = funded.Substring(0, funded.IndexOf('.'));
                //lblRequiremens.Text = UI.GetThousandSeparator(req);
                //lblFunded.Text = UI.GetThousandSeparator(funded);
                //lblRequiremens.Text = req;
                //lblFunded.Text = funded;
            }
            else
            {
                lblProjects.Text = "";
                lblOrgs.Text = "";
                lblCountry.Text = "";
                lblClusters.Text = "";
                string req = "";
                string funded = "";
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
            int? orgId = RC.GetSelectedIntVal(ddlOrg);

            if (orgId == 0)
            {
                if (RC.IsDataEntryUser(User))
                    orgId = UserInfo.Organization;
                else
                    orgId = (int?)null;
            }

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


            return DBContext.GetData("GetProjectsListing", new object[] {emgLocationId, emgClusterId, projCode, orgId, 
                                                                            RC.SelectedSiteLanguageId,  yearId, projectStatus, 
                                                                            secClusterId, isOPS, isFunded});
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RC.FormatThousandSeperator(e.Row, "lblOriginalRequest");
                RC.FormatThousandSeperator(e.Row, "lblFundedAmount");

                int isOPS = 0;
                int.TryParse(gvProjects.DataKeys[e.Row.RowIndex]["IsOPS"].ToString(), out isOPS);

                if (!this.User.Identity.IsAuthenticated || isOPS == 1) 
                {
                    ImageButton btnEdit = e.Row.FindControl("btnEdit") as ImageButton;
                    if (btnEdit != null)
                        btnEdit.Visible = false;

                    ImageButton btnDelete = e.Row.FindControl("btnDelete") as ImageButton;
                    if (btnDelete != null)
                        btnDelete.Visible = false;
                    
                }
            }
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}