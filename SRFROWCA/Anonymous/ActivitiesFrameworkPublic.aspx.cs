using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;

namespace SRFROWCA.Anonymous
{
    public partial class ActivitiesFrameworkPublic : BasePage
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

        protected void ddlSelectedIndexChnaged(object sender, EventArgs e)
        {
            PopulateActivities();
            LoadIndicators();
        }

        protected void ddlActivitySelectedIndexChnaged(object sender, EventArgs e)
        {
            LoadIndicators();
            //PopulateActivities();
        }

        protected void ddlYear_SelectedIndexChnaged(object sender, EventArgs e)
        {
            PopulateActivities();
            LoadIndicators();
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
                ObjPrToolTip.ObjectiveIconToolTip(e, 1);
                Label lblTarget = e.Row.FindControl("lblIndTarget") as Label;
                if (lblTarget != null)
                {
                    if (!string.IsNullOrEmpty(lblTarget.Text))
                        UI.SetThousandSeparator(e.Row, "lblIndTarget");
                }
            }
        }

        private void DeleteActivity(int activityId)
        {
            DBContext.Delete("DeleteActivityNew", new object[] { activityId, DBNull.Value });
        }

        // Execute row commands like Edit, Delete etc. on Grid.
        protected void btnSearch2_Click(object sender, EventArgs e)
        {
            PopulateActivities();
            LoadIndicators();
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
            ModalPopupExtender2.Show();
        }

        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            Response.Redirect("ExpClusterFramework.aspx");
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

        private void PopulateActivities()
        {
            int? emgLocId = ddlCountry.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCountry.SelectedValue);
            int? emgClusterId = ddlCluster.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCluster.SelectedValue);
            int? emgObjId = ddlObjective.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlObjective.SelectedValue);

            DataTable dtAct = new DataTable();
            if (emgLocId > 0 && emgClusterId > 0)
            {
                int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);
                dtAct = DBContext.GetData("GetActivitiesNew", new object[] { emgLocId, emgClusterId, emgObjId, 
                                                                              yearId, RC.SelectedSiteLanguageId });
                ddlActivity.DataTextField = "Activity";
                ddlActivity.DataValueField = "ActivityId";
            }

            ddlActivity.DataSource = dtAct;
            ddlActivity.DataBind();
            if (RC.SelectedSiteLanguageId == 1)
                ddlActivity.Items.Insert(0, new ListItem("Select Activity", "0"));
            else
                ddlActivity.Items.Insert(0, new ListItem("Sélectionner Activité", "0"));
        }

        private void LoadClustersFilter()
        {
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            if (RC.SelectedSiteLanguageId == 1)
                ddlCluster.Items.Insert(0, new ListItem("Select Cluster", "0"));
            else
                ddlCluster.Items.Insert(0, new ListItem("Sélectionner Cluster", "0"));
        }

        private void LoadObjectivesFilter()
        {
            ddlObjective.Items.Clear();
            if (RC.SelectedSiteLanguageId == 1)
                ddlObjective.Items.Add(new ListItem("Select Objective", "0"));
            else
                ddlObjective.Items.Add(new ListItem("Sélectionner Objectif", "0"));
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
            bool? isCP = cbCPActivity.Checked ? true : (bool?)null;
            int? pageSize = gvActivity.PageSize;
            int? pageIndex = gvActivity.PageIndex;

            return DBContext.GetData("GetAllIndicatorsNew2", new object[] { emergencyLocationId, emergencyClusterId, 
                                                                            emergencyObjectiveId, search, activityId, 
                                                                            frameworkYear, isCP, (int)RC.SelectedSiteLanguageId ,
                                                                             pageIndex, pageSize  });
            
        }

        private DataTable GetActivitiesForExcel(bool admin2)
        {
            int? emergencyClusterId = ddlCluster.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCluster.SelectedValue);
            int? emergencyObjectiveId = ddlObjective.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlObjective.SelectedValue);
            int? activityId = ddlActivity.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlActivity.SelectedValue);
            string search = string.IsNullOrEmpty(txtActivityName.Text) ? null : txtActivityName.Text;
            int? emergencyLocationId = ddlCountry.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCountry.SelectedValue);
            int frameworkYear = RC.GetSelectedIntVal(ddlFrameworkYear);
            bool? isCP = cbCPActivity.Checked ? true : (bool?)null;
            return DBContext.GetData("GetAllIndicatorsNew2WithT", new object[] { emergencyLocationId, emergencyClusterId, emergencyObjectiveId, 
                                                                                    search, activityId, frameworkYear, 
                                                                                    admin2, isCP, (int)RC.SelectedSiteLanguageId });
        }
        private DataTable GetObjectives()
        {
            return DBContext.GetData("GetEmergencyObjectives", new object[] { (int)RC.SelectedSiteLanguageId, RC.EmergencySahel2015 });
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
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
                dt.Columns.Remove("EmergencyClusterId");
                dt.Columns.Remove("EmergencyLocationId");
                dt.Columns.Remove("IndicatorCalculationTypeId");
                dt.Columns.Remove("LocationId");
                dt.Columns.Remove("ObjectiveId");
                dt.Columns.Remove("IsActiveIndicator");
                dt.Columns.Remove("IsActivityActive");
                dt.Columns.Remove("IndicatorCalculationTypeId");
                try
                {
                    dt.Columns.Remove("TargetLocationId");
                }
                catch { }
            }
            catch { }
        }
    }
}