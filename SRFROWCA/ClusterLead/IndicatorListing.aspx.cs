using BusinessLogic;
using Microsoft.Reporting.WebForms;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClusterLead
{
    public partial class IndicatorListing : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo.UserProfileInfo(RC.EmergencySahel2015);
                LoadClustersFilter();
                LoadCountry();
                LoadObjectivesFilter();
                PopulateActivities();
                SetDropDownOnRole(true);
                LoadIndicators();
            }

            ToggleControlsToAddIndicator();
        }

        private void ToggleControlsToAddIndicator()
        {
            if (RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User))
            {
                int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
                int emgClusterId = RC.GetSelectedIntVal(ddlCluster);

                FrameWorkSettingsCount frCount = FrameWorkUtil.GetActivityFrameworkSettings(emgLocationId, emgClusterId);
                if (frCount.IndCount <= 0 || (frCount.ActCount <= 0 || frCount.DateExcedded))
                {
                    btnAddActivityAndIndicators.Enabled = false;
                }
                else
                {
                    btnAddActivityAndIndicators.Enabled = true;
                }
            }

            if (RC.IsRegionalClusterLead(this.User))
            {
                btnAddActivityAndIndicators.Enabled = false;
            }
        }

        private void SetDropDownOnRole(bool bindAll)
        {
            if (RC.IsClusterLead(this.User))
            {
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
                ddlCluster.Enabled = false;
                ddlCluster.BackColor = Color.LightGray;
                if (bindAll)
                {
                    ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                    ddlCountry.Enabled = false;
                    ddlCountry.BackColor = Color.LightGray;
                }
            }

            if (RC.IsRegionalClusterLead(this.User))
            {
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
                ddlCluster.Enabled = false;
                ddlCluster.BackColor = Color.LightGray;
                if (bindAll)
                {
                    ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                }
            }

            if (RC.IsCountryAdmin(this.User))
            {
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
                if (bindAll)
                {
                    ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                    ddlCountry.Enabled = false;
                    ddlCountry.BackColor = Color.LightGray;
                }
            }
        }

        // Add delete confirmation message with all delete buttons.
        protected void gvActivity_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblTarget = e.Row.FindControl("lblIndTarget") as Label;
                if (lblTarget != null)
                {
                    if (string.IsNullOrEmpty(lblTarget.Text))
                    {
                        e.Row.BackColor = ColorTranslator.FromHtml("#CD5C5C");
                        divMissingTarget.Visible = true;
                    }
                    else
                    {
                        UI.SetThousandSeparator(e.Row, "lblIndTarget");
                    }
                }

                ImageButton btnDelete = e.Row.FindControl("btnDelete") as ImageButton;
                ImageButton btnEdit = e.Row.FindControl("btnEdit") as ImageButton;

                if (yearId == 11)
                {
                    btnDelete.Visible = false;
                    btnEdit.Visible = false;
                }
                else
                {
                    int emgLocationId = 0;
                    int emgClusterId = 0;

                    Label lblCountryId = e.Row.FindControl("lblCountryID") as Label;
                    if (lblCountryId != null)
                    {
                        int.TryParse(lblCountryId.Text, out emgLocationId);
                    }

                    Label lblClusterId = e.Row.FindControl("lblClusterID") as Label;
                    if (lblClusterId != null)
                    {
                        int.TryParse(lblClusterId.Text, out emgClusterId);
                    }

                    FrameWorkSettingsCount frCount = FrameWorkUtil.GetActivityFrameworkSettings(emgLocationId, emgClusterId);
                    if (btnDelete != null)
                    {
                        btnDelete.Attributes.Add("onclick", "javascript:return " +
                        "confirm('Are you sure you want to delete this Indicator?')");

                        if (frCount.DateExcedded)
                        {
                            if (RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User) || RC.IsRegionalClusterLead(this.User))
                            {
                                btnDelete.Visible = false;
                            }
                        }
                    }

                    CheckBox cbActivity = e.Row.FindControl("cbIsActivityActive") as CheckBox;
                    if (btnEdit != null)
                    {
                        if (frCount.DateExcedded)
                        {
                            if (RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User) || RC.IsRegionalClusterLead(this.User))
                            {
                                btnEdit.Visible = false;
                            }
                        }
                        else if (!cbActivity.Checked || frCount.IndCount == 0)
                        {
                            if (!cbActivity.Checked)
                            {
                                btnEdit.Visible = false;
                            }
                            CheckBox cbIndicator = e.Row.FindControl("cbIsActive") as CheckBox;
                            if (!cbIndicator.Checked)
                                cbIndicator.Enabled = false;
                        }
                    }

                    if (RC.IsRegionalClusterLead(this.User))
                    {
                        btnEdit.Visible = false;
                        btnDelete.Visible = false;
                    }
                }
            }
        }

        private void DeleteActivity(int activityId)
        {
            DBContext.Delete("DeleteActivityNew", new object[] { activityId, DBNull.Value });
        }

        // Execute row commands like Edit, Delete etc. on Grid.
        protected void gvActivity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // If user click on Delete button.
            if (e.CommandName == "DeleteInd")
            {
                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;

                int indicatorId = 0;
                int.TryParse(gvActivity.DataKeys[row.RowIndex].Values["IndicatorId"].ToString(), out indicatorId);

                int activityId = 0;
                int.TryParse(gvActivity.DataKeys[row.RowIndex].Values["ActivityId"].ToString(), out activityId);

                if (indicatorId > 0)
                {
                    if (!IndicatorIsInUse(indicatorId))
                    {
                        DeleteIndicator(indicatorId, activityId);
                        ShowMessage("Indicator Deleted Successfully!");
                    }
                    else
                    {
                        ShowMessage("Indicator can not be deleted. It is being used!", RC.NotificationType.Error, true, 2000);
                    }
                }
                // This is to delete activity
                //else if (activityId > 0)
                //{
                //    DeleteActivity(activityId);
                //    ShowMessage("Activity Deleted Successfully!");
                //}

                LoadIndicators();
                ToggleControlsToAddIndicator();
            }


            // Edit Project.
            if (e.CommandName == "EditActivity")
            {
                int activityId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("AddActivityAndIndicators.aspx?a=" + activityId);

            }
        }
        protected void btnSearch2_Click(object sender, EventArgs e)
        {
            LoadIndicators();
            PopulateActivities();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (RC.IsAdmin(this.User))
            {
                ddlCluster.SelectedValue = "0";
                ddlActivity.SelectedValue = "0";
                ddlCountry.SelectedValue = "0";
            }
            else
            {
                SetDropDownOnRole(true);
            }

            ddlObjective.SelectedValue = "0";
            txtActivityName.Text = "";
            LoadIndicators();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            GridView gvExport = new GridView();
            if (RC.GetSelectedIntVal(ddlCountry) == 0)
            {
                ShowMessage("Please select a country to export data!", RC.NotificationType.Warning, true, 1000);
                return;
            }
            DataTable dt = GetActivitiesForExcel();
            RemoveColumnsFromDataTable(dt);
            gvExport.DataSource = dt;
            gvExport.DataBind();

            string fileName = "Indicators";
            string fileExtention = ".xls";
            ExportUtility.ExportGridView(gvExport, fileName, fileExtention, Response);
        }

        protected void ExportToPDF(object sender, EventArgs e)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            byte[] bytes;
            ReportViewer rvCountry = new ReportViewer();
            rvCountry.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://win-78sij2cjpjj/Reportserver");
            //rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://54.83.26.190/Reportserver");
            ReportParameter[] RptParameters = null;
            // rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://localhost/Reportserver");

            string activityId = null;

            string emergencyClusterId = null;
            string emergencyObjectiveId = null;
            string search = null;
            string emgLocationId = null;
            string IsGender = null;

            RptParameters = new ReportParameter[7];
            RptParameters[0] = new ReportParameter("emgLocationId", emgLocationId, false);
            RptParameters[1] = new ReportParameter("emgClusterId", emergencyClusterId, false);
            RptParameters[2] = new ReportParameter("emgObjectiveId", emergencyObjectiveId, false);
            RptParameters[3] = new ReportParameter("search", search, false);
            RptParameters[4] = new ReportParameter("activityId", activityId, false);
            RptParameters[5] = new ReportParameter("isGender", IsGender, false);
            RptParameters[6] = new ReportParameter("lngId", ((int)RC.SiteLanguage.English).ToString(), false);

            rvCountry.ServerReport.ReportPath = "/reports/activityindicators";
            string fileName = "ActivityIndicators" + DateTime.Now.ToString("yyyy-MM-dd_hh_mm_ss") + ".pdf";
            rvCountry.ServerReport.ReportServerCredentials = new ReportServerCredentials("Administrator", "&qisW.c@Jq", "");
            rvCountry.ServerReport.SetParameters(RptParameters);
            bytes = rvCountry.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush();
        }

        internal override void BindGridData()
        {
            LoadClustersFilter();
            LoadObjectivesFilter();
            PopulateActivities();
            SetDropDownOnRole(false);
            LoadIndicators();
        }

        private bool IndicatorIsInUse(int indicatorId)
        {
            DataTable dt = DBContext.GetData("IndicatorInUse", new object[] { indicatorId });
            return dt.Rows.Count > 0;
        }

        private void DeleteIndicator(int indicatorDetailId, int activityId)
        {
            DBContext.Delete("DeleteIndicatorNew", new object[] { indicatorDetailId, activityId, DBNull.Value });
        }

        protected void gvActivity_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetActivities();
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvActivity.DataSource = dt;
                gvActivity.DataBind();
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

        private void LoadIndicators()
        {
            gvActivity.DataSource = GetActivities();
            gvActivity.DataBind();
        }

        private void LoadCountry()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            ddlCountry.Items.Insert(0, new ListItem("--- Select Country ---", "0"));
        }

        private void PopulateActivities()
        {
            int? emergencyClusterId = ddlCluster.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCluster.SelectedValue);
            int? emergencyObjectiveId = ddlObjective.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlObjective.SelectedValue);
            int? emergencyLocationId = ddlCountry.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCountry.SelectedValue);

            ddlActivity.DataSource = DBContext.GetData("GetActivitiesNew", new object[] { emergencyLocationId, emergencyClusterId, emergencyObjectiveId, RC.SelectedSiteLanguageId });
            ddlActivity.DataTextField = "Activity";
            ddlActivity.DataValueField = "ActivityId";
            ddlActivity.DataBind();

            ListItem item = new ListItem("Select Activity", "0");
            ddlActivity.Items.Insert(0, item);
        }

        private void LoadClustersFilter()
        {
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            ddlCluster.Items.Insert(0, new ListItem("--- Select Cluster ---", "0"));
        }

        private void LoadObjectivesFilter()
        {
            ddlObjective.Items.Clear();
            ddlObjective.Items.Add(new ListItem("All", "0"));
            ddlObjective.DataValueField = "EmergencyObjectiveId";
            ddlObjective.DataTextField = "Objective";
            ddlObjective.DataSource = GetObjectives();
            ddlObjective.DataBind();
        }

        private DataTable GetClusters()
        {
            int emergencyId = RC.SelectedEmergencyId;
            if (emergencyId == 0)
            {
                emergencyId = 1;
            }

            return DBContext.GetData("GetClusters", new object[] { (int)RC.SelectedSiteLanguageId, emergencyId });
        }

        private DataTable GetActivities()
        {
            int? emergencyClusterId = ddlCluster.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCluster.SelectedValue);
            int? emergencyObjectiveId = ddlObjective.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlObjective.SelectedValue);
            int? activityId = ddlActivity.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlActivity.SelectedValue);
            string search = string.IsNullOrEmpty(txtActivityName.Text) ? null : txtActivityName.Text;
            int? emergencyLocationId = ddlCountry.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCountry.SelectedValue);
            int frameworkYear = RC.GetSelectedIntVal(ddlFrameworkYear);

            return DBContext.GetData("GetAllIndicatorsNew2", new object[] { emergencyLocationId, emergencyClusterId, 
                                                                            emergencyObjectiveId, search, activityId, 
                                                                            frameworkYear, (int)RC.SelectedSiteLanguageId });
        }

        private DataTable GetActivitiesForExcel()
        {
            int? emergencyClusterId = ddlCluster.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCluster.SelectedValue);
            int? emergencyObjectiveId = ddlObjective.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlObjective.SelectedValue);
            int? activityId = ddlActivity.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlActivity.SelectedValue);
            string search = string.IsNullOrEmpty(txtActivityName.Text) ? null : txtActivityName.Text;
            int? emergencyLocationId = ddlCountry.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCountry.SelectedValue);
            int frameworkYear = RC.GetSelectedIntVal(ddlFrameworkYear);

            DataTable dt = new DataTable();
            if (emergencyLocationId > 0)
            {
                dt = DBContext.GetData("GetAllIndicatorsNew2WithTargets", new object[] { emergencyLocationId, emergencyClusterId, 
                                                                                            emergencyObjectiveId, search, activityId, 
                                                                                            frameworkYear, (int)RC.SelectedSiteLanguageId });
            }

            return dt;
        }
        private DataTable GetObjectives()
        {
            return DBContext.GetData("GetEmergencyObjectives", new object[] { (int)RC.SelectedSiteLanguageId, RC.EmergencySahel2015 });
        }


        private DataTable GetActivityTypes()
        {

            return DBContext.GetData("GetActivityTypes");

        }

        protected void btnAddIndicator_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddIndicators.aspx");

        }

        protected void btnMigrate2016_Click(object sender, EventArgs e)
        {
            Response.Redirect("IndicatorListingMigrate.aspx");
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
            LoadIndicators();
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

        protected void btnAddActivityAndIndicators_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddActivityAndIndicators.aspx");
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }


        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (IsIndicatorCheckBox == 1)
                ToggleIndicatorStatus();
            else
                ToggleActivityStatus();

            ToggleControlsToAddIndicator();
            
            LoadIndicators();
            //SendEmail(IndicatorActiveStatus > 0);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadIndicators();
        }

        protected void cbActivityActive_Changed(object sender, EventArgs e)
        {
            IsIndicatorCheckBox = 0;
            ActivityActiveStatus = -1;
            ToggleActivityId = 0;
            GridViewRow row = (GridViewRow)(((CheckBox)sender).NamingContainer);
            int activityId = 0;
            int.TryParse(gvActivity.DataKeys[row.RowIndex].Values["ActivityId"].ToString(), out activityId);
            ToggleActivityId = activityId;
            
            if (ToggleActivityId > 0)
            {
                CheckBox cbIsActive = row.FindControl("cbIsActivityActive") as CheckBox;
                if (cbIsActive != null)
                {
                    localDisableConfirmBox.Text = !cbIsActive.Checked ? "Are you sure you want to deactivate this Activity?" :
                                                                            "Are you sure you want to activate this Activity?";
                    ActivityActiveStatus = cbIsActive.Checked ? 1 : 0;
                    List<string> projects = GetProjectsOnActivity(ToggleActivityId);
                    if (!string.IsNullOrEmpty(projects[0]))
                    {
                        //SetEmailItem(row, projects);
                        lblProjectsCaption.Visible = true;
                        lblProjectsCaption.Text = !cbIsActive.Checked ? "Indicators of this activity will be removed from the following projects." :
                                                                        "Indicators of this activity will be added in the following projects.";
                        lblProjectUsingIndicator.Text = projects[0];
                    }
                    else
                    {
                        lblProjectsCaption.Text = "";
                        lblProjectUsingIndicator.Text = "";
                    }
                    ModalPopupExtender1.Show();
                }
            }            
        }

        protected void cbActive_Changed(object sender, EventArgs e)
        {
            IsIndicatorCheckBox = 1;
            IndicatorActiveStatus = -1;
            ToggleIndicatorId = 0;
            GridViewRow row = (GridViewRow)(((CheckBox)sender).NamingContainer);
            int indicatorId = 0;
            int.TryParse(gvActivity.DataKeys[row.RowIndex].Values["IndicatorId"].ToString(), out indicatorId);
            ToggleIndicatorId = indicatorId;
            if (ToggleIndicatorId > 0)
            {
                CheckBox cbIsActive = row.FindControl("cbIsActive") as CheckBox;
                if (cbIsActive != null)
                {
                    localDisableConfirmBox.Text = !cbIsActive.Checked ? "Are you sure you want to deactivate this indicator?" :
                                                                            "Are you sure you want to activate this indicator?";
                    IndicatorActiveStatus = cbIsActive.Checked ? 1 : 0;
                    List<string> projects = GetProjectsUsingIndicator();
                    if (!string.IsNullOrEmpty(projects[0]))
                    {
                        SetEmailItem(row, projects);
                        lblProjectsCaption.Visible = true;
                        lblProjectsCaption.Text = !cbIsActive.Checked ? "This indicator will be removed from these projects:" :
                                                                        "This indicator will be added in these projects:";
                        lblProjectUsingIndicator.Text = projects[0];
                    }
                    else
                    {
                        lblProjectsCaption.Text = "";
                        lblProjectUsingIndicator.Text = "";
                    }
                    
                    ModalPopupExtender1.Show();
                }
            }
        }

        private void SetEmailItem(GridViewRow row, List<string> projects)
        {
            List<string> emailItems = new List<string>();
            Label lblCountry = row.FindControl("lblCountryID") as Label;
            if (lblCountry != null)
            {
                emailItems.Add(lblCountry.Text.Trim());
            }

            Label lblCluster = row.FindControl("lblClusterID") as Label;
            if (lblCluster != null)
            {
                emailItems.Add(lblCluster.Text.Trim());
            }
            emailItems.Add(row.Cells[1].Text.ToString());
            emailItems.Add(row.Cells[2].Text.ToString());
            emailItems.Add(row.Cells[5].Text.ToString());
            emailItems.Add(projects[0]);
            emailItems.Add(projects[1]);
            EmailItems = emailItems;
        }

        private void ToggleIndicatorStatus()
        {
            int yearId = (int)RC.YearsInDB.Year2015;
            DBContext.Update("UpdateIndicatorAndProjectsActiveStatus", new object[] { ToggleIndicatorId, IndicatorActiveStatus, yearId, DBNull.Value });
        }

        private void ToggleActivityStatus()
        {
            int yearId = (int)RC.YearsInDB.Year2015;
            DBContext.Update("UpdateActivityAndProjectsActiveStatus", new object[] { ToggleActivityId, ActivityActiveStatus, yearId, DBNull.Value });
        }

        private List<string> GetProjectsUsingIndicator()
        {
            DataTable dt = DBContext.GetData("GetAllProjectsUsingIndicator", new object[] { ToggleIndicatorId, (int)RC.YearsInDB.Year2015});
            StringBuilder sbProjCodes = new StringBuilder();
            StringBuilder sbProjIds = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                sbProjCodes.AppendFormat(" {0},", dr["ProjectCode"].ToString());
                sbProjIds.AppendFormat("{0},", dr["ProjectId"].ToString());
            }

            List<string> projects = new List<string>();
            projects.Add(sbProjCodes.ToString().Trim().TrimEnd(','));
            projects.Add(sbProjIds.ToString().Trim().TrimEnd(','));

            return projects;
        }

        private List<string> GetProjectsOnActivity(int toggleActivityId)
        {
            DataTable dt = DBContext.GetData("GetAllProjectsUsingIndicatorActivity", new object[] { toggleActivityId, (int)RC.YearsInDB.Year2015});
            StringBuilder sbProjCodes = new StringBuilder();
            StringBuilder sbProjIds = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                sbProjCodes.AppendFormat(" {0},", dr["ProjectCode"].ToString());
                sbProjIds.AppendFormat("{0},", dr["ProjectId"].ToString());
            }

            List<string> projects = new List<string>();
            projects.Add(sbProjCodes.ToString().Trim().TrimEnd(','));
            projects.Add(sbProjIds.ToString().Trim().TrimEnd(','));

            return projects;
        }

        protected void ddlYear_SelectedIndexChnaged(object sender, EventArgs e)
        {
            LoadIndicators();
        }

        private void SendEmail(bool isAdded)
        {
            List<string> emailItems = EmailItems;
            int emgCountryId = 0;
            int.TryParse(emailItems[0], out emgCountryId);
            int val = 0;
            int.TryParse(emailItems[1], out val);
            int? emgClusterId = val > 0 ? val : (int?)null;

            string subject = "Activity Indicator has been disabled!";
            if (isAdded)
            {
                subject = "Activity Indicator has been activated!";
            }

            string country = emailItems[2];
            string cluster = emailItems[3];
            string user = "";
            try { user = User.Identity.Name; }
            catch { }

            string body = string.Format(@"<b>{0}</b><br/>
                                         <b>Country:</b> {1}<br/>
                                         <b>Cluster:</b> {2}<br/>
                                         <b>Indicator:</b> {3}<br/>
                                         <b>By:</b> {4}<br/>
                                         <b>Projects Using This Indicator:</b> {5}"

                                         , subject, country, cluster, emailItems[4], user, emailItems[5]);
            DataTable dtUsers = DBContext.GetData("GetDataEntryUsersEmails", new object[] { emailItems[6], DBNull.Value, emgCountryId });
            RC.SendEmail(emgCountryId, emgClusterId, subject, body, dtUsers);
        }

        private int ToggleIndicatorId
        {
            get
            {
                int indId = 0;
                if (ViewState["ToggleIndicatorId"] != null)
                {
                    int.TryParse(ViewState["ToggleIndicatorId"].ToString(), out indId);
                }

                return indId;
            }
            set
            {
                ViewState["ToggleIndicatorId"] = value;
            }
        }

        private int ToggleActivityId
        {
            get
            {
                int indId = 0;
                if (ViewState["ToggleActivityId"] != null)
                {
                    int.TryParse(ViewState["ToggleActivityId"].ToString(), out indId);
                }

                return indId;
            }
            set
            {
                ViewState["ToggleActivityId"] = value;
            }
        }

        private int IndicatorActiveStatus
        {
            get
            {
                int isActive = 0;
                if (ViewState["IndicatorActiveStatusCB"] != null)
                {
                    int.TryParse(ViewState["IndicatorActiveStatusCB"].ToString(), out isActive);
                }

                return isActive;
            }
            set
            {
                ViewState["IndicatorActiveStatusCB"] = value;
            }
        }

        private int ActivityActiveStatus
        {
            get
            {
                int isActive = 0;
                if (ViewState["ActivityActiveStatusClsInds"] != null)
                {
                    int.TryParse(ViewState["ActivityActiveStatusClsInds"].ToString(), out isActive);
                }

                return isActive;
            }
            set
            {
                ViewState["ActivityActiveStatusClsInds"] = value;
            }
        }

        private int IsIndicatorCheckBox
        {
            get
            {
                int isActive = 0;
                if (ViewState["IsIndicatorCheckBox"] != null)
                {
                    int.TryParse(ViewState["IsIndicatorCheckBox"].ToString(), out isActive);
                }

                return isActive;
            }
            set
            {
                ViewState["IsIndicatorCheckBox"] = value;
            }
        }

        private List<string> EmailItems
        {
            get
            {
                return ViewState["IndicatorActiveStatus"] as List<string>;
            }
            set
            {
                ViewState["IndicatorActiveStatus"] = value;
            }
        }
    }
}