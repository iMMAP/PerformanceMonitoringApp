using BusinessLogic;
using Microsoft.Reporting.WebForms;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace SRFROWCA.ClusterLead
{
    public partial class IndicatorListing : BasePage
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadClusters();
                LoadCountry();
                SetDropDownOnRole(true);
                RC.SetFiltersFromSessionCluster(ddlCountry, ddlCluster, Session);
                LoadObjectives();
                //PopulateActivities();


                if (Request.QueryString["year"] != null)
                {
                    if (Request.QueryString["year"] == "2015")
                        ddlFrameworkYear.SelectedValue = "11";
                    if (Request.QueryString["year"] == "2016")
                        ddlFrameworkYear.SelectedValue = "12";
                }
                LoadIndicators();
                ToggleControlsToAddIndicator();
            }
        }

        #region Events.
        protected void gvActivity_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.ObjectiveIconToolTip(e, 1);
                //ObjPrToolTip.ObjectiveLableToolTip(e, 0);

                if (yearId == (int)RC.Year._2015 || yearId == (int)RC.Year._2016)
                {
                    divMissingTarget.Visible = false;
                }
                else
                {
                    Label lblTarget = e.Row.FindControl("lblIndTarget") as Label;
                    if (lblTarget != null)
                    {
                        if (string.IsNullOrEmpty(lblTarget.Text))
                        {
                            e.Row.BackColor = ColorTranslator.FromHtml("#ff9999");
                            divMissingTarget.Visible = true;
                        }
                        else
                        {
                            UI.SetThousandSeparator(e.Row, "lblIndTarget");
                        }
                    }
                }

                ImageButton btnDelete = e.Row.FindControl("btnDelete") as ImageButton;
                ImageButton btnEdit = e.Row.FindControl("btnEdit") as ImageButton;

                if (!RC.IsAdmin(this.User) && (RC.GetSelectedIntVal(ddlFrameworkYear) == (int)RC.Year._2015))
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

                    //int year = 0;
                    //int.TryParse(ddlFrameworkYear.SelectedItem.Text, out year);
                    //int indUnused = SectorFramework.IndUnused(emgLocationId, emgClusterId, year);
                    //int actUnused = SectorFramework.ActivityUnused(emgLocationId, emgClusterId, year);
                    Label lblIsDateExceeded = e.Row.FindControl("lblIsDateExceeded") as Label;
                    bool IsDateExceeded = false;
                    if (lblIsDateExceeded != null)
                        IsDateExceeded = lblIsDateExceeded.Text == "1";

                    if (btnDelete != null)
                    {
                        btnDelete.Attributes.Add("onclick", "javascript:return " +
                        "confirm('Are you sure you want to delete this Indicator?')");

                        if (IsDateExceeded)
                        {
                            if (RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User)
                                || RC.IsRegionalClusterLead(this.User) || RC.IsDataEntryUser(this.User))
                            {
                                btnDelete.Visible = false;
                            }
                        }
                    }

                    //CheckBox cbActivity = e.Row.FindControl("cbIsActivityActive") as CheckBox;
                    if (btnEdit != null)
                    {
                        if (IsDateExceeded)
                        {
                            if (RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User)
                                || RC.IsRegionalClusterLead(this.User) || RC.IsDataEntryUser(this.User))
                            {
                                btnEdit.Visible = false;
                            }
                        }
                        //else
                        //    if (!cbActivity.Checked || indUnused <= 0)
                        //    {
                        //        if (!cbActivity.Checked)
                        //        {
                        //            btnEdit.Visible = false;
                        //        }
                        //        CheckBox cbIndicator = e.Row.FindControl("cbIsActive") as CheckBox;
                        //        if (!cbIndicator.Checked)
                        //            cbIndicator.Enabled = false;
                        //    }
                    }

                    if (RC.IsRegionalClusterLead(this.User))
                    {
                        btnEdit.Visible = false;
                        btnDelete.Visible = false;
                    }
                }
            }
        }

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
                LoadIndicators();
                ToggleControlsToAddIndicator();
            }

            // Edit Project.
            if (e.CommandName == "EditActivity")
            {
                int activityId = Convert.ToInt32(e.CommandArgument);
                string year = ddlFrameworkYear.SelectedItem.Text;
                Response.Redirect("AddActivityAndIndicators.aspx?a=" + activityId.ToString() + "&year=" + year);

            }
        }

        protected void btnSearch2_Click(object sender, EventArgs e)
        {
            LoadIndicators();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (RC.IsAdmin(this.User))
            {
                ddlCluster.SelectedValue = "0";
                //ddlActivity.SelectedValue = "0";
                ddlCountry.SelectedValue = "0";
            }
            else
            {
                SetDropDownOnRole(true);
            }

            ddlObjective.SelectedValue = "0";
            ddlFrameworkYear.SelectedIndex = 0;
            txtActivityName.Text = "";
            LoadIndicators();
            ToggleControlsToAddIndicator();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Show();
        }

        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Anonymous/ExpClusterFramework.aspx");
        }

        protected void btnExportExcelOK_Click(object sender, EventArgs e)
        {
            bool admin2 = rbExlAdmin2Yes.Checked;
            DataTable dt = GetActivitiesForExcel(admin2);
            if (rbExlIdnNO.Checked)
                RemoveColumnsFromDataTable(dt);

            string fileName = "Indicators";
            ExportUtility.ExportGridView(dt, fileName, Response);
            ModalPopupExtender2.Hide();
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

        protected void ddlSelectedIndexChnaged(object sender, EventArgs e)
        {
            LoadIndicators();
            RC.SaveFiltersInSessionCluster(ddlCountry, ddlCluster, Session);
            ToggleControlsToAddIndicator();
            LoadObjectives();
        }

        protected void ddlObj_SelectedIndexChnaged(object sender, EventArgs e)
        {
            LoadIndicators();
            RC.SaveFiltersInSessionCluster(ddlCountry, ddlCluster, Session);
            ToggleControlsToAddIndicator();
        }

        protected void gvActivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvActivity.PageIndex = e.NewPageIndex;
            LoadIndicators();
        }

        protected void btnAddActivityAndIndicators_Click(object sender, EventArgs e)
        {
            string year = ddlFrameworkYear.SelectedItem.Text;
            Response.Redirect("AddActivityAndIndicators.aspx?year=" + year);
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

        protected void ddlYear_SelectedIndexChnaged(object sender, EventArgs e)
        {
            LoadObjectives();
            LoadIndicators();
            ToggleControlsToAddIndicator();
        }

        protected void ddlActivitySelectedIndexChnaged(object sender, EventArgs e)
        {
            LoadIndicators();
        }

        #endregion

        #region Methods.
        private void ToggleControlsToAddIndicator()
        {
            if (RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User) || RC.IsAdmin(this.User))
            {
                int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
                int emgClusterId = RC.GetSelectedIntVal(ddlCluster);
                if (emgLocationId > 0 && emgClusterId > 0)
                {
                    int year = 0;
                    int.TryParse(ddlFrameworkYear.SelectedItem.Text, out year);
                    int indUnused = SectorFramework.IndUnused(emgLocationId, emgClusterId, year);
                    int actUnused = SectorFramework.ActivityUnused(emgLocationId, emgClusterId, year);
                    bool IsDateExceeded = SectorFramework.DateExceeded(emgLocationId, emgClusterId, year);
                    if (indUnused <= 0 || (actUnused <= 0 || IsDateExceeded))
                    {
                        btnAddActivityAndIndicators.Enabled = false;
                        btnMigrate2016.Enabled = false;
                    }
                    else
                    {
                        btnAddActivityAndIndicators.Enabled = true;
                        btnMigrate2016.Enabled = true;
                    }
                }
                else
                {
                    btnAddActivityAndIndicators.Enabled = false;
                    btnMigrate2016.Enabled = false;
                }
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


        private void DeleteActivity(int activityId)
        {
            DBContext.Delete("DeleteActivityNew", new object[] { activityId, DBNull.Value });
        }

        // Execute row commands like Edit, Delete etc. on Grid.


        internal override void BindGridData()
        {
            LoadClusters();
            LoadObjectives();
            //PopulateActivities();
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
            DataTable dt = GetActivities();
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
            if (RC.SelectedSiteLanguageId == 1)
                ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));
            else
                ddlCountry.Items.Insert(0, new ListItem("Sélectionner Pays", "0"));
        }

        private void LoadClusters()
        {
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            if (RC.SelectedSiteLanguageId == 1)
                ddlCluster.Items.Insert(0, new ListItem("Select Cluster", "0"));
            else
                ddlCluster.Items.Insert(0, new ListItem("Sélectionner Cluster", "0"));
        }

        private void LoadObjectives()
        {
            int? emergencyLocationId = ddlCountry.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCountry.SelectedValue);
            int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);
            UI.PopulateEmergencyObjectives(ddlObjective, yearId, emergencyLocationId);
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
            string search = string.IsNullOrEmpty(txtActivityName.Text) ? null : txtActivityName.Text;
            int? emergencyLocationId = ddlCountry.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCountry.SelectedValue);
            int frameworkYear = RC.GetSelectedIntVal(ddlFrameworkYear);
            bool? isCP = cbCPActivity.Checked ? true : (bool?)null;
            int? pageSize = gvActivity.PageSize;
            int? pageIndex = gvActivity.PageIndex;

            return DBContext.GetData("GetAllIndicatorsNew2", new object[] { emergencyLocationId, emergencyClusterId, 
                                                                            emergencyObjectiveId, search, frameworkYear, 
                                                                            isCP, (int)RC.SelectedSiteLanguageId,
                                                                               pageIndex, pageSize  });
        }

        private DataTable GetActivitiesForExcel(bool admin2)
        {
            int? emergencyClusterId = ddlCluster.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCluster.SelectedValue);
            int? emergencyObjectiveId = ddlObjective.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlObjective.SelectedValue);
            string search = string.IsNullOrEmpty(txtActivityName.Text) ? null : txtActivityName.Text;
            int? emergencyLocationId = ddlCountry.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCountry.SelectedValue);
            int frameworkYear = RC.GetSelectedIntVal(ddlFrameworkYear);
            bool? isCP = cbCPActivity.Checked ? true : (bool?)null;

            return DBContext.GetData("GetAllIndicatorsNew2WithT", new object[] { emergencyLocationId, emergencyClusterId, emergencyObjectiveId, 
                                                                                    search, frameworkYear, admin2, 
                                                                                    isCP, (int)RC.SelectedSiteLanguageId });
        }


        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            try
            {

                dt.Columns.Remove("GenderDisaggregated");
                dt.Columns.Remove("ActivityId");
                dt.Columns.Remove("ClusterId");
                dt.Columns.Remove("IndicatorId");
                dt.Columns.Remove("IndicatorDetailId");
                dt.Columns.Remove("EmergencyClusterId");
                dt.Columns.Remove("EmergencyLocationId");
                dt.Columns.Remove("LocationId");
                dt.Columns.Remove("ObjectiveId");

                if (!((RC.GetSelectedIntVal(ddlCluster)) ==  (int)RC.ClusterSAH2015.PRO))
                    dt.Columns.Remove("IsChildProtection");

                try
                {
                    if ((RC.GetSelectedIntVal(ddlFrameworkYear)) == 11)
                    {

                        dt.Columns.Remove("TargetMale");
                        dt.Columns.Remove("TargetFemale");
                    }
                    dt.Columns.Remove("TargetLocationId");
                }
                catch { }

                try
                {
                    dt.Columns.Remove("IndicatorCalculationTypeId");
                }
                catch { }
            }
            catch { }
        }





        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }


        #endregion


        #region Unused Code

        //protected void cbActivityActive_Changed(object sender, EventArgs e)
        //{
        //    IsIndicatorCheckBox = 0;
        //    ActivityActiveStatus = -1;
        //    ToggleActivityId = 0;
        //    GridViewRow row = (GridViewRow)(((CheckBox)sender).NamingContainer);
        //    int activityId = 0;
        //    int.TryParse(gvActivity.DataKeys[row.RowIndex].Values["ActivityId"].ToString(), out activityId);
        //    ToggleActivityId = activityId;

        //    if (ToggleActivityId > 0)
        //    {
        //        CheckBox cbIsActive = row.FindControl("cbIsActivityActive") as CheckBox;
        //        if (cbIsActive != null)
        //        {
        //            localDisableConfirmBox.Text = !cbIsActive.Checked ? "Are you sure you want to deactivate this Activity?" :
        //                                                                    "Are you sure you want to activate this Activity?";
        //            ActivityActiveStatus = cbIsActive.Checked ? 1 : 0;
        //            List<string> projects = GetProjectsOnActivity(ToggleActivityId);
        //            if (!string.IsNullOrEmpty(projects[0]))
        //            {
        //                //SetEmailItem(row, projects);
        //                lblProjectsCaption.Visible = true;
        //                lblProjectsCaption.Text = !cbIsActive.Checked ? "Indicators of this activity will be removed from the following projects." :
        //                                                                "Indicators of this activity will be added in the following projects.";
        //                lblProjectUsingIndicator.Text = projects[0];
        //            }
        //            else
        //            {
        //                lblProjectsCaption.Text = "";
        //                lblProjectUsingIndicator.Text = "";
        //            }
        //            ModalPopupExtender1.Show();
        //        }
        //    }
        //}

        //protected void cbActive_Changed(object sender, EventArgs e)
        //{
        //    IsIndicatorCheckBox = 1;
        //    IndicatorActiveStatus = -1;
        //    ToggleIndicatorId = 0;
        //    GridViewRow row = (GridViewRow)(((CheckBox)sender).NamingContainer);
        //    int indicatorId = 0;
        //    int.TryParse(gvActivity.DataKeys[row.RowIndex].Values["IndicatorId"].ToString(), out indicatorId);
        //    ToggleIndicatorId = indicatorId;
        //    if (ToggleIndicatorId > 0)
        //    {
        //        CheckBox cbIsActive = row.FindControl("cbIsActive") as CheckBox;
        //        if (cbIsActive != null)
        //        {
        //            localDisableConfirmBox.Text = !cbIsActive.Checked ? "Are you sure you want to deactivate this indicator?" :
        //                                                                    "Are you sure you want to activate this indicator?";
        //            IndicatorActiveStatus = cbIsActive.Checked ? 1 : 0;
        //            List<string> projects = GetProjectsUsingIndicator();
        //            if (!string.IsNullOrEmpty(projects[0]))
        //            {
        //                SetEmailItem(row, projects);
        //                lblProjectsCaption.Visible = true;
        //                lblProjectsCaption.Text = !cbIsActive.Checked ? "This indicator will be removed from these projects:" :
        //                                                                "This indicator will be added in these projects:";
        //                lblProjectUsingIndicator.Text = projects[0];
        //            }
        //            else
        //            {
        //                lblProjectsCaption.Text = "";
        //                lblProjectUsingIndicator.Text = "";
        //            }

        //            ModalPopupExtender1.Show();
        //        }
        //    }
        //}

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
            int yearId = (int)RC.Year._2017;
            DBContext.Update("UpdateIndicatorAndProjectsActiveStatus", new object[] { ToggleIndicatorId, IndicatorActiveStatus, yearId, DBNull.Value });
        }

        private void ToggleActivityStatus()
        {
            int yearId = (int)RC.Year._2017;
            DBContext.Update("UpdateActivityAndProjectsActiveStatus", new object[] { ToggleActivityId, ActivityActiveStatus, yearId, DBNull.Value });
        }

        private List<string> GetProjectsUsingIndicator()
        {
            DataTable dt = DBContext.GetData("GetAllProjectsUsingIndicator", new object[] { ToggleIndicatorId, (int)RC.Year._2015 });
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
            DataTable dt = DBContext.GetData("GetAllProjectsUsingIndicatorActivity", new object[] { toggleActivityId, (int)RC.Year._2015 });
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

        protected void btnMigrate2016_Click(object sender, EventArgs e)
        {
            Response.Redirect("IndicatorListingMigrate.aspx");
        }

        #endregion

    }
}