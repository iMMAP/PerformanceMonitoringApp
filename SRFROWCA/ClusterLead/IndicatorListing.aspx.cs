using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using Microsoft.Reporting.WebForms;
using SRFROWCA.Common;

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

            if (RC.IsClusterLead(this.User))
            {
                int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
                int emgClusterId = RC.GetSelectedIntVal(ddlCluster);

                int maxIndicators = 0;
                int maxActivities = 0;
                DateTime endEditDate = DateTime.Now.AddDays(1);

                GetMaxCount(emgLocationId, emgClusterId, out maxIndicators, out maxActivities, out endEditDate);
                if (maxIndicators <= 0 || (maxActivities <= 0 || DateTime.Now > endEditDate))
                {
                    btnAddActivityAndIndicators.Enabled = false;
                }
                if (maxIndicators <= 0 || DateTime.Now > endEditDate)
                {
                    btnAddIndicator.Enabled = false;
                }
            }

            if (RC.IsRegionalClusterLead(this.User))
            {
                btnAddIndicator.Enabled = false;
                btnAddActivityAndIndicators.Enabled = false;
            }
        }

        private void GetMaxCount(int emgLocationId, int emgClusterId, out int maxIndicators, out int maxActivities, out DateTime endEditDate)
        {
            string configKey = "Key-" + emgLocationId.ToString() + emgClusterId.ToString();
            maxIndicators = 0;
            maxActivities = 0;
            endEditDate = DateTime.Now.AddDays(1);

            string PATH = HttpRuntime.AppDomainAppPath;
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\ChangeEndSettings.xml";

            if (File.Exists(PATH))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(PATH);

                XmlElement elem_settings = doc.GetElementById("ChangeEndSettings");
                XmlNode settingsNode = doc.DocumentElement;

                foreach (XmlNode node in settingsNode.ChildNodes)
                {
                    if (node.Name.Equals(configKey))
                    {
                        if (node.Attributes["FrameworkCount"] != null)
                            maxIndicators = Convert.ToInt32(node.Attributes["FrameworkCount"].Value);

                        if (node.Attributes["ActivityCount"] != null)
                            maxActivities = Convert.ToInt32(node.Attributes["ActivityCount"].Value);

                        if (node.Attributes["DateLimit"] != null)
                            endEditDate = DateTime.ParseExact(Convert.ToString(node.Attributes["DateLimit"].Value), "MM-dd-yyyy", CultureInfo.InvariantCulture);
                    }
                }
            }

            if (maxIndicators > 0)
            {
                DataTable dt = DBContext.GetData("GetAllIndicatorsNew", new object[] { emgLocationId, emgClusterId, null, null, null, null, (int)RC.SelectedSiteLanguageId });
                if (dt.Rows.Count > 0)
                    maxIndicators = maxIndicators - dt.Rows.Count;
            }

            if (maxActivities > 0)
            {
                DataTable dt = DBContext.GetData("GetAllActivitiesNew", new object[] { emgLocationId, emgClusterId, null, null, (int)RC.SelectedSiteLanguageId });
                if (dt != null && dt.Rows.Count > 0)
                    maxActivities = maxActivities - dt.Rows.Count;
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
                LinkButton btnDelete = e.Row.FindControl("btnDelete") as LinkButton;
                LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;

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

                int maxIndicators = 0;
                int maxActivities = 0;
                DateTime endEditDate = DateTime.Now.AddDays(1);
                GetMaxCount(emgLocationId, emgClusterId, out maxIndicators, out maxActivities, out endEditDate);

                if (btnDelete != null)
                {
                    btnDelete.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this record?')");

                    if (endEditDate < DateTime.Now)
                    {
                        if (RC.IsClusterLead(this.User) || RC.IsRegionalClusterLead(this.User))
                        {
                            btnDelete.Visible = false;
                        }
                    }
                }

                if (btnEdit != null && endEditDate < DateTime.Now)
                {
                    if (RC.IsClusterLead(this.User) || RC.IsRegionalClusterLead(this.User))
                    {
                        btnEdit.Visible = false;
                    }
                }

                if (RC.IsRegionalClusterLead(this.User))
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
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
                    DeleteIndicator(indicatorId);
                    ShowMessage("Indicator Deleted Successfully!");
                }
                else if (activityId > 0)
                {
                    DeleteActivity(activityId);
                    ShowMessage("Activity Deleted Successfully!");
                }

                LoadIndicators();
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
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (RC.IsAdmin(this.User))
            {
                ddlCluster.SelectedValue = "-1";
                ddlActivity.SelectedValue = "-1";
                ddlCountry.SelectedValue = "-1";
            }
            else
            {
                SetDropDownOnRole(true);
            }

            ddlObjective.SelectedValue = "-1";
            txtActivityName.Text = "";
            chkIsGender.Checked = false;
            LoadIndicators();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            GridView gvExport = new GridView();

            DataTable dt = GetActivities();
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

        private bool IndicatorIsBeingUsed(int indicatorDetailId)
        {
            DataTable dt = DBContext.GetData("GetIsNewIndicatorBeingUsed", new object[] { indicatorDetailId });
            return !(dt.Rows.Count > 0);
        }

        private void DeleteIndicator(int indicatorDetailId)
        {
            DBContext.Delete("DeleteIndicatorNew", new object[] { indicatorDetailId, DBNull.Value });
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
            ddlCountry.Items.Insert(0, new ListItem("--- Select Country ---", "-1"));
        }

        private void PopulateActivities()
        {
            int? emergencyClusterId = ddlCluster.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlCluster.SelectedValue);
            int? emergencyObjectiveId = ddlObjective.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlObjective.SelectedValue);

            ddlActivity.DataSource = DBContext.GetData("GetActivitiesNew", new object[] { DBNull.Value, emergencyClusterId, emergencyObjectiveId, RC.EmergencySahel2015 });
            ddlActivity.DataTextField = "Activity";
            ddlActivity.DataValueField = "ActivityId";
            ddlActivity.DataBind();

            ListItem item = new ListItem("Select Activity", "0");
            ddlActivity.Items.Insert(0, item);
        }

        private void LoadClustersFilter()
        {
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            ddlCluster.Items.Insert(0, new ListItem("--- Select Cluster ---", "-1"));
        }

        private void LoadObjectivesFilter()
        {
            ddlObjective.Items.Clear();
            ddlObjective.Items.Add(new ListItem("All", "-1"));
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
            int? emergencyClusterId = ddlCluster.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlCluster.SelectedValue);
            int? emergencyObjectiveId = ddlObjective.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlObjective.SelectedValue);
            int? activityId = ddlActivity.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlActivity.SelectedValue);
            string search = string.IsNullOrEmpty(txtActivityName.Text) ? null : txtActivityName.Text;

            int? emergencyLocationId = ddlCountry.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlCountry.SelectedValue);
            int? isGender = chkIsGender.Checked ? 1 : (int?)null;

            return DBContext.GetData("GetAllIndicatorsNew2", new object[] { emergencyLocationId, emergencyClusterId, emergencyObjectiveId, search, activityId, isGender, (int)RC.SelectedSiteLanguageId });
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
                dt.Columns.Remove("ClusterId");
                dt.Columns.Remove("IndicatorId");
                dt.Columns.Remove("IndicatorDetailId");
                dt.Columns.Remove("ClusterName");
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
    }
}