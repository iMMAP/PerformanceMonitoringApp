﻿using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClusterLead
{
    public partial class IndicatorListingMigrate : BasePage
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox cbIsSelected = e.Row.FindControl("cbIsSelected") as CheckBox;
                if (cbIsSelected != null)
                    cbIsSelected.Enabled = !cbIsSelected.Checked;
            }
        }

        // Execute row commands like Edit, Delete etc. on Grid.
        protected void btnSearch2_Click(object sender, EventArgs e)
        {
            LoadIndicators();
            PopulateActivities();
        }

        protected void btnMigrate_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvActivity.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int activityId = Convert.ToInt32(gvActivity.DataKeys[row.RowIndex].Values["ActivityId"].ToString());
                    int indicatorId = Convert.ToInt32(gvActivity.DataKeys[row.RowIndex].Values["IndicatorId"].ToString());
                    CheckBox cb = gvActivity.Rows[row.RowIndex].FindControl("cbIsSelected") as CheckBox;
                    if (cb != null)
                    {
                        if (cb.Checked && cb.Enabled)
                        {
                            DBContext.Add("Insert2016Framework", new object[] { activityId, indicatorId, 12, RC.GetCurrentUserId, DBNull.Value });
                        }
                    }
                }
            }

            Response.Redirect("~/ClusterLead/IndicatorListing.aspx");
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
            LoadIndicators();
        }

        internal override void BindGridData()
        {
            LoadClustersFilter();
            LoadObjectivesFilter();
            PopulateActivities();
            SetDropDownOnRole(false);
            LoadIndicators();
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
            int yearId = 11;
            ddlActivity.DataSource = DBContext.GetData("GetActivitiesNew", new object[] { emergencyLocationId, emergencyClusterId, 
                                                                                            emergencyObjectiveId, yearId,
                                                                                            RC.SelectedSiteLanguageId });
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
            int? emergencyLocationId = ddlCountry.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCountry.SelectedValue);
            return DBContext.GetData("GetAllIndicatorsToMigrate", new object[] { emergencyLocationId, emergencyClusterId, 
                                                                                    emergencyObjectiveId, activityId, 
                                                                                    (int)RC.SelectedSiteLanguageId });
        }
        private DataTable GetObjectives()
        {
            return DBContext.GetData("GetEmergencyObjectives", new object[] { (int)RC.SelectedSiteLanguageId, RC.EmergencySahel2015 });
        }


        private DataTable GetActivityTypes()
        {

            return DBContext.GetData("GetActivityTypes");

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

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
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