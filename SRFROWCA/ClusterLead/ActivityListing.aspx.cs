using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using Microsoft.Reporting.WebForms;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class ActivityListing : BasePage
    {
        public bool applyFilter = false;
        public int maxIndCount = 0;
        public int maxActCount = 0;
        public DateTime dateLimit = DateTime.Now.AddDays(1);

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            if (RC.IsClusterLead(this.User))
            {
                GetMaxCount("Key-" + UserInfo.EmergencyCountry + UserInfo.EmergencyCluster, out maxIndCount, out maxActCount, out dateLimit);
                if (maxIndCount <= 0 || (maxActCount <= 0 || (applyFilter && DateTime.Now > dateLimit)))
                {
                    btnAddActivityAndIndicators.Enabled = false;
                }
                if (maxActCount <= 0 || (applyFilter && DateTime.Now > dateLimit))
                {
                    btnAddActivity.Enabled = false;
                }
            }
        }


        private void GetMaxCount(string configKey, out int maxIndValue, out int maxActValue, out DateTime maxDate)
        {
            maxIndValue = 0;
            maxActValue = 0;
            maxDate = DateTime.Now.AddDays(1);

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
                            maxIndValue = Convert.ToInt32(node.Attributes["FrameworkCount"].Value);

                        if (node.Attributes["ActivityCount"] != null)
                            maxActValue = Convert.ToInt32(node.Attributes["ActivityCount"].Value);

                        if (node.Attributes["DateLimit"] != null)
                            maxDate = DateTime.ParseExact(Convert.ToString(node.Attributes["DateLimit"].Value), "MM-dd-yyyy", CultureInfo.InvariantCulture);
                    }
                }
            }

            if (maxIndValue > 0)
            {
                DataTable dt = DBContext.GetData("GetAllIndicatorsNew", new object[] { null, null, null, null, null, null, (int)RC.SelectedSiteLanguageId });
                if (dt.Rows.Count > 0)
                    maxIndValue = maxIndValue - dt.Rows.Count;
            }

            if (maxActValue > 0)
            {
                DataTable dt = DBContext.GetData("GetAllActivitiesNew", new object[] { null, null, null, null, (int)RC.SelectedSiteLanguageId });
                if (dt != null && dt.Rows.Count > 0)
                    maxActValue = maxActValue - dt.Rows.Count;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;


            LoadActivities();
            PopulateFilters();
        }



        // Add delete confirmation message with all delete buttons.
        protected void gvActivity_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button deleteButton = e.Row.FindControl("btnDelete") as Button;
                if (deleteButton != null)
                {
                    deleteButton.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this Activity?')");
                }
            }
        }

        // Execute row commands like Edit, Delete etc. on Grid.
        protected void gvActivity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // If user click on Delete button.
            if (e.CommandName == "Delete")
            {
                int activityDetailId = Convert.ToInt32(e.CommandArgument);

                // Check if any IP has reported on this project. If so then do not delete it.
                if (!ActivityIsBeingUsed(activityDetailId))
                {
                    RC.ShowMessage(Page, Page.GetType(), "asasa", "Activity cannot be deleted! It is being used.", RC.NotificationType.Error, true, 500);
                }
                else
                {
                    DeleteActivity(activityDetailId);
                    LoadActivities();
                }
            }

            // Edit Project.
            if (e.CommandName == "EditActivity")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("EditActivity.aspx?id=" + id);

            }
        }
        protected void btnSearch2_Click(object sender, EventArgs e)
        {
            LoadActivities();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddlCluster.SelectedValue = "-1";
            ddlObjective.SelectedValue = "-1";
            txtActivityName.Text = "";
            LoadActivities();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            GridView gvExport = new GridView();
            DataTable dt = DBContext.GetData("GetAllActivitiesNew", new object[] { null, null, null, null, (int)RC.SelectedSiteLanguageId });//GetActivities();
            RemoveColumnsFromDataTable(dt);
            gvExport.DataSource = dt;
            gvExport.DataBind();

            string fileName = "Activities";
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
            string emergencyClusterId = null;
            string emergencyObjectiveId = null;
            string search = null;
            string emgLocationId = null;

            RptParameters = new ReportParameter[5];
            RptParameters[0] = new ReportParameter("emgLocationId", emgLocationId, false);
            RptParameters[1] = new ReportParameter("emgClusterId", emergencyClusterId, false);
            RptParameters[2] = new ReportParameter("emgObjectiveId", emergencyObjectiveId, false);
            RptParameters[3] = new ReportParameter("search", search, false);
            RptParameters[4] = new ReportParameter("lngId", ((int)RC.SiteLanguage.English).ToString(), false);

            rvCountry.ServerReport.ReportPath = "/reports/clusteractivities";
            string fileName = "ClusterActivities" + DateTime.Now.ToString("yyyy-MM-dd_hh_mm_ss") + ".pdf";
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
            LoadActivities();
            PopulateFilters();
        }

        private bool ActivityIsBeingUsed(int ActivityDetailId)
        {
            DataTable dt = DBContext.GetData("GetIsActvityBeingUsed", new object[] { ActivityDetailId });
            return !(dt.Rows.Count > 0);
        }

        private void DeleteActivity(int activityDetailId)
        {
            DBContext.Delete("DeleteActivityNew", new object[] { activityDetailId, DBNull.Value });
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

        private void LoadActivities()
        {
            gvActivity.DataSource = GetActivities();
            gvActivity.DataBind();
            if (RC.IsClusterLead(this.User))
            {
                GetMaxCount("Key-" + UserInfo.EmergencyCountry + UserInfo.EmergencyCluster, out maxIndCount, out maxActCount, out dateLimit);
                if (maxActCount <= 0 || (applyFilter && DateTime.Now > dateLimit))
                {
                    gvActivity.Columns[3].Visible = false;
                    gvActivity.Columns[4].Visible = false;
                }

            }
        }

        private void PopulateFilters()
        {
            //LoadEmergencyFilter();
            LoadClustersFilter();
            LoadObjectivesFilter();
            //LoadPriorityFilter();
            //LoadEmergencyFilterNew();          

        }

        private void LoadClustersFilter()
        {
            ddlCluster.Items.Clear();
            ddlCluster.Items.Add(new ListItem("All", "-1"));
            ddlCluster.DataValueField = "EmergencyClusterId";
            ddlCluster.DataTextField = "ClusterName";
            ddlCluster.DataSource = GetClusters();
            ddlCluster.DataBind();
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
            int? emgId = UserInfo.Emergency;
            return DBContext.GetData("GetClusters", new object[] { (int)RC.SelectedSiteLanguageId, emgId });
        }

        private DataTable GetActivities()
        {
            int? emergencyClusterId = ddlCluster.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlCluster.SelectedValue);
            int? emergencyObjectiveId = ddlObjective.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlObjective.SelectedValue);

            string search = string.IsNullOrEmpty(txtActivityName.Text) ? null : txtActivityName.Text;

            return DBContext.GetData("GetAllActivitiesNew", new object[] { DBNull.Value, emergencyClusterId, emergencyObjectiveId, search, (int)RC.SelectedSiteLanguageId });
        }
        private DataTable GetObjectives()
        {
            return DBContext.GetData("GetEmergencyObjectives", new object[] { (int)RC.SelectedSiteLanguageId, UserInfo.Emergency });
        }


        private DataTable GetActivityTypes()
        {

            return DBContext.GetData("GetActivityTypes");

        }

        protected void btnAddActivity_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddActivity.aspx");

        }

        protected void btnAddActivityAndIndicators_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddActivityAndIndicators.aspx?b=a");

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
            LoadActivities();
        }


        protected void gvActivity_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }


        protected void gvActivity_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            try
            {
                dt.Columns.Remove("ClusterId");
                dt.Columns.Remove("ActivityId");
                dt.Columns.Remove("ActivityDetailId");
                dt.Columns.Remove("ClusterName");
                dt.Columns.Remove("ShortObjective");
            }
            catch { }
        }
    }
}