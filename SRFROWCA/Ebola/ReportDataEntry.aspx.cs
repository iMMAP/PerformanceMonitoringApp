using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Ebola
{
    public partial class ReportDataEntry : BasePage
    {
        public int YearID = 0;
        public int MonthID = 0;
        public int DayID = 0;

        public static DataTable dtYears = new DataTable();
        public static DataTable dtMonths = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            string languageChange = "";

            if (Session["SiteChanged"] != null)
                languageChange = Session["SiteChanged"].ToString();

            if (!string.IsNullOrEmpty(languageChange))
            {
                PopulateMonths();
                Session["SiteChanged"] = null;
            }

            if (!IsPostBack)
            {
                PopulateLocations();
                PopulateYears();
                PopulateMonths();
                PopulateProjects();

                if (rblProjects.Items.Count > 0)
                    rblProjects.SelectedIndex = 0;

                PopulateObjectives();
                PopulatePriorities();
            }

            PopulateToolTips();

            //this.Form.DefaultButton = this.btnSave.UniqueID;
            string controlName = GetPostBackControlId(this);

            if (/*controlName == "ddlMonth" || controlName == "ddlYear" ||*/ controlName == "rblProjects" || controlName == "txtDate" || controlName == "rblFrequency" /*|| controlName == "ddlWeeks"*/)
            {
                LocationRemoved = 0;
                RemoveSelectedLocations(cblAdmin1);
                RemoveSelectedLocations(cblLocations);
            }

            PopulateDate();

            //if (controlName != "imgbtnComments")
            {
                DataTable dtActivities = GetActivities();
                AddDynamicColumnsInGrid(dtActivities);

                Session["dtActivities"] = dtActivities;
                GetReportId();

                gvIndicatorData.DataSource = dtActivities;
                gvIndicatorData.DataBind();
            }
        }

        #region Events

        private void ReloadGrid()
        {
            int yearId = 0;
            int monthId = 0;
            int dayId = 0;

            string[] dateSplit = txtDate.Text.Trim().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (dateSplit.Length > 2)
            {
                int.TryParse(dateSplit[2], out yearId);
                int.TryParse(dateSplit[1], out dayId);
                int.TryParse(dateSplit[0], out monthId);
            }

            SetDateIDs(new DateTime(1, monthId, 1).ToString("MMMM"), new DateTime(yearId, 1, 1).ToString("yyyy"), dayId);

            LocationRemoved = 0;
            BindGridData();
            AddLocationsInSelectedList();

            //LoadWeeks();
        }

        private void PopulateDate()
        {
            if (!IsPostBack)
                txtDate.Text = DateTime.Now.ToString("MM-dd-yyyy");

            int yearId = 0;
            int monthId = 0;
            int dayId = 0;

            string[] dateSplit = txtDate.Text.Trim().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (dateSplit.Length > 2)
            {
                int.TryParse(dateSplit[2], out yearId);
                int.TryParse(dateSplit[1], out dayId);
                int.TryParse(dateSplit[0], out monthId);
            }

            SetDateIDs(new DateTime(1, monthId, 1).ToString("MMMM"), new DateTime(yearId, 1, 1).ToString("yyyy"), dayId);
        }

        private void SetDateIDs(string month, string year, int day)
        {
            DataRow[] drMonth = dtMonths.Select("MonthName='" + month + "' AND SiteLanguageId='" + RC.SelectedSiteLanguageId + "'");
            DataRow[] drYear = dtYears.Select("Year='" + year + "'");

            if (drMonth.Length > 0)
                MonthID = Convert.ToInt32(drMonth[0]["MonthID"]);

            if (drYear.Length > 0)
                YearID = Convert.ToInt32(drYear[0]["YearID"]);

            DayID = new DateTime(Convert.ToInt32(year), Convert.ToInt32(DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month), Convert.ToInt32(day)).DayOfYear;
        }

        protected void rblProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridData();
            AddLocationsInSelectedList();
        }

        protected void gvIndicatorData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.ObjectiveIconToolTip(e, 0);
                ObjPrToolTip.PrioritiesIconToolTip(e, 1);
                ObjPrToolTip.RegionalIndicatorIcon(e, 12);
                ObjPrToolTip.CountryIndicatorIcon(e, 13);
            }
        }

        protected void btnImgClick(object sender, EventArgs e)
        {
            int j = 0;
            ImageButton btn = (ImageButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            int rowIndex = gvr.RowIndex;

            if (ReportId == 0)
                SaveReportMainInfo();

            int activityDataId = 0;
            int.TryParse(gvIndicatorData.DataKeys[rowIndex]["ActivityDataId"].ToString(), out activityDataId);

            if (activityDataId > 0)
            {
                CommentsIndId = activityDataId;

                if (ucIndComments.LoadComments(ReportId, activityDataId))
                    btnSaveComments.Visible = false;

                mpeComments.Show();
            }
        }

        protected void gvIndicatorData_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnLocation_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "key", "launchModal();", true);
            LocationRemoved = 1;

            List<int> locationIds = GetLocationIdsFromGrid();
            SelectLocationsOfGrid(locationIds);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveProjectData();
        }

        protected void btnSaveComments_Click(object sender, EventArgs e)
        {
            string comments = txtComments.Value;
            int indictorCommentDetID = ucIndComments.GetIndicatorCommentDetailID();

            if (!string.IsNullOrEmpty(comments))
            {
                using (ORSEntities db = new ORSEntities())
                    DBContext.Add("InsertIndicatorComments", new object[] { ReportId, CommentsIndId, comments, RC.GetCurrentUserId, DBNull.Value, indictorCommentDetID });
            }
        }

        protected void btnCancelComments_Click(object sender, EventArgs e)
        {
            mpeComments.Hide();
        }

        protected void btnExcel_Export(object sender, EventArgs e)
        {
            SaveProjectData();
            DataTable dt = GetProjectsData(true);

            if (dt.Rows.Count > 0)
            {
                int monthId = 0;
                int dayId = 0;

                string[] dateSplit = txtDate.Text.Trim().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                if (dateSplit.Length > 2)
                {
                    int.TryParse(dateSplit[1], out dayId);
                    int.TryParse(dateSplit[0], out monthId);
                }

                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();

                string fileName = UserInfo.CountryName + "_" + UserInfo.OrgName + "_" + new DateTime(1, monthId, 1).ToString("MMMM") + "_Report";
                ExportUtility.ExportGridView(gv, fileName, ".xls", Response, true);
            }
        }

        protected void btnPDF_Export(object sender, EventArgs e)
        {
            SaveProjectData();

            DataTable dt = GetProjectsData(false);
            if (dt.Rows.Count > 0)
                GeneratePDF(dt);
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            ShowMessage("<b>Some Error Occoured. Admin Has Notified About It</b>.<br/> Please Try Again.", RC.NotificationType.Error);

            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, "ReportDataEntry", this.User);
        }

        #endregion

        #region Methods

        private void PopulateMonths()
        {
            dtMonths = DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
        }

        private void PopulateYears()
        {
            dtYears = DBContext.GetData("GetYears");
        }

        private void PopulateLocations()
        {
            int countryId = UserInfo.Country;

            if (UserInfo.Emergency == 2)
                PopulateAdmin2(countryId);
            else
                PopulateAdmin1(countryId);
        }

        private void PopulateAdmin1(int parentLocationId)
        {
            DataTable dt = GetAdmin1Locations(parentLocationId);

            cblAdmin1.DataValueField = "LocationId";
            cblAdmin1.DataTextField = "LocationName";

            cblAdmin1.DataSource = dt;
            cblAdmin1.DataBind();

            lblLocAdmin1.Text = UserInfo.CountryName + " " + "Region";//(string)GetLocalResourceObject("ReportDataEntry_PopulateAdmin1__Admin_1_Locations");
        }

        private DataTable GetAdmin1Locations(int parentLocationId)
        {
            string procedure = "GetSecondLevelChildLocations";

            if (parentLocationId == 567)
                procedure = "GetSecondLevelChildLocationsAndCountry";

            DataTable dt = DBContext.GetData(procedure, new object[] { parentLocationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void PopulateAdmin2(int parentLocationId)
        {
            DataTable dt = GetAdmin2Locations(parentLocationId);

            cblLocations.DataValueField = "LocationId";
            cblLocations.DataTextField = "LocationName";

            cblLocations.DataSource = dt;
            cblLocations.DataBind();
            //lblLocAdmin2.Text = UserInfo.CountryName + (string)GetLocalResourceObject("ReportDataEntry_PopulateAdmin2__Admin_2_Locations");
        }

        private DataTable GetAdmin2Locations(int parentLocationId)
        {
            DataTable dt = DBContext.GetData("GetThirdLevelChildLocations", new object[] { parentLocationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void PopulateProjects()
        {
            DataTable dt = GetUserProjects();

            rblProjects.DataValueField = "ProjectId";
            rblProjects.DataTextField = "ProjectShortTitle";

            rblProjects.DataSource = dt;
            rblProjects.DataBind();
        }

        private DataTable GetUserProjects()
        {
            bool? isOPSProject = null;
            return DBContext.GetData("GetOrgProjectsOnLocation", new object[] { UserInfo.EmergencyCountry, UserInfo.Organization, isOPSProject });
        }

        private void PopulateObjectives()
        {
            UI.FillObjectives(cblObjectives, true);
        }

        private void PopulatePriorities()
        {
            UI.FillPriorities(cblPriorities);
        }

        private void PopulateToolTips()
        {

            DataTable dt = GetUserProjects();
            ProjectsToolTip(rblProjects, dt);
        }

        private void ProjectsToolTip(ListControl ctl, DataTable dt)
        {
            foreach (ListItem item in ctl.Items)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (item.Text == row["ProjectShortTitle"].ToString())
                        item.Attributes["title"] = row["ProjectTitle"].ToString();
                }
            }
        }

        private void RemoveSelectedLocations(ListControl control)
        {
            foreach (ListItem item in control.Items)
                item.Selected = false;
        }

        private DataTable GetActivities()
        {
            int projectId = RC.GetSelectedIntVal(rblProjects);
            string locationIds = GetSelectedLocations();
            string locIdsNotIncluded = GetNotSelectedLocations();

            DateTime rDate = DateTime.ParseExact(txtDate.Text.Trim(), "MM-dd-yyyy", CultureInfo.InvariantCulture);


            Guid userId = RC.GetCurrentUserId;
            DataTable dt = DBContext.GetData("GetIPData_E", new object[] { UserInfo.EmergencyCountry, locationIds, YearID, MonthID,DayID,
                                                                        locIdsNotIncluded, RC.SelectedSiteLanguageId, userId,
                                                                        UserInfo.Organization, projectId, Convert.ToInt32(rblFrequency.SelectedValue), rDate});

            if (dt.Rows.Count <= 0 && !string.IsNullOrEmpty(locationIds))
                ShowMessage("Are you sure you have activites for this project. Please click on Manage Activites to select the activities you want to report on!", RC.NotificationType.Error, false);

            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void AddDynamicColumnsInGrid(DataTable dt)
        {
            foreach (DataColumn column in dt.Columns)
            {
                TemplateField customField = new TemplateField();

                string columnName = column.ColumnName;
                if (!(columnName == "ReportId" || columnName == "ProjectCode" || columnName == "Objective" ||
                    columnName == "HumanitarianPriority" || columnName == "ActivityName" || columnName == "DataName" ||
                    columnName == "ActivityDataId" || columnName == "ProjectTitle" || columnName == "ProjectId" ||
                    columnName == "ObjAndPrId" || columnName == "ObjectiveId" || columnName == "HumanitarianPriorityId" ||
                    columnName == "objAndPrAndPId" || columnName == "objAndPId" || columnName == "PrAndPId" ||
                    columnName == "ProjectIndicatorId" || columnName == "RInd" || columnName == "CInd" || columnName == "Unit"))
                {
                    if (columnName.Contains("Accum"))
                    {
                        customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, "CheckBox", column.ColumnName, "1");
                        customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, "CheckBox", "ACM", "1");
                        gvIndicatorData.Columns.Add(customField);
                    }
                    else if (columnName.Contains("Variable"))
                    {
                        customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, "TextBox", column.ColumnName, "Variable");
                        customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, "TextBox", column.ColumnName, "Variable");
                        gvIndicatorData.Columns.Add(customField);
                    }
                }
            }
        }

        private void GetReportId()
        {
            string[] dateSplit = txtDate.Text.Trim().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            //int yearId = dateSplit.Length > 2 ? Convert.ToInt32(dateSplit[2]) : 0;
            //int monthId = dateSplit.Length > 1 ? Convert.ToInt32(dateSplit[1]) : 0;
            //int dayId = dateSplit.Length > 0 ? Convert.ToInt32(dateSplit[0]) : 0;

            int projectId = RC.GetSelectedIntVal(rblProjects);
            DateTime rDate = DateTime.ParseExact(txtDate.Text.Trim(), "MM-dd-yyyy", CultureInfo.InvariantCulture);
            DataTable dtReports = DBContext.GetData("uspGetReportID", new object[] { projectId, YearID, MonthID, DayID, UserInfo.EmergencyCountry, UserInfo.Organization, Convert.ToInt32(rblFrequency.SelectedValue),  rDate});

            if (dtReports.Rows.Count > 0)
                ReportId = Convert.ToInt32(dtReports.Rows[0]["ReportID"]);
            else
                ReportId = 0;
        }

        internal override void BindGridData()
        {
            DataTable dt = GetActivities();
            Session["dtActivities"] = dt;

            GetReportId();
            gvIndicatorData.DataSource = dt;
            gvIndicatorData.DataBind();

            PopulateObjectives();
            PopulatePriorities();
            PopulateToolTips();
            BindCultureResourcesOfPage();
        }

        private void AddLocationsInSelectedList()
        {
            PopulateLocations();
        }

        private List<int> GetLocationIdsFromGrid()
        {
            List<int> locationIds = new List<int>();

            foreach (GridViewRow row in gvIndicatorData.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DataTable dtActivities = (DataTable)Session["dtActivities"];

                    foreach (DataColumn dc in dtActivities.Columns)
                    {
                        string colName = dc.ColumnName;
                        HiddenField hf = row.FindControl("hf" + colName) as HiddenField;

                        if (hf != null)
                        {
                            int locationId = 0;
                            int.TryParse(hf.Value, out locationId);

                            if (locationId > 0)
                                locationIds.Add((locationId));
                        }
                    }

                    // If data row then iterate only once bece we need column names
                    // from grid to get ids from hidden fields.
                    break;
                }
            }

            return locationIds.Distinct().ToList();
        }

        private void SelectLocationsOfGrid(List<int> locationIds)
        {
            foreach (ListItem item in cblLocations.Items)
                item.Selected = locationIds.Contains(Convert.ToInt32(item.Value));

            foreach (ListItem item in cblAdmin1.Items)
                item.Selected = locationIds.Contains(Convert.ToInt32(item.Value));
        }

        private void SaveProjectData()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                List<int> locationIds = GetLocationIdsFromGrid();

                string mailType = "";
                if (locationIds.Count > 0)
                {
                    bool isNewReport = false;
                    if (ReportId == 0)
                    {
                        SaveReportMainInfo();
                        isNewReport = true;
                        mailType = "Added";
                    }

                    int returnCode = SaveReport();
                    if (returnCode == 1 && !isNewReport)
                    {
                        UpdateReportUpdatedDate();
                        mailType = "Updated";
                    }

                    if (returnCode == 1)
                        SendMail("Report Saved Summary! " + DateTime.Now.ToString("dd-MMM-yyyy"), ReportId, mailType);
                }
                else
                {
                    if (ReportId > 0)
                    {
                        DeleteReport();
                        mailType = "Deleted";
                        SendMail("Report Delete Summary! " + DateTime.Now.ToString("dd-MMM-yyyy"), ReportId, mailType);
                    }
                }

                scope.Complete();
                ShowMessage("Your data saves successfully!"/*(string)GetLocalResourceObject("ReportDataEntry_SaveMessageSuccess")*/);
            }
        }

        private void SendMail(string subject, int reportID, string mailType)
        {
            try
            {
                int countryID = UserInfo.Country > 0 ? UserInfo.Country : 0;
                int projId = RC.GetSelectedIntVal(rblProjects);

                int yearId = 0;
                int monthId = 0;
                int dayId = 0;

                string[] dateSplit = txtDate.Text.Trim().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                if (dateSplit.Length > 2)
                {
                    int.TryParse(dateSplit[2], out yearId);
                    int.TryParse(dateSplit[1], out dayId);
                    int.TryParse(dateSplit[0], out monthId);
                }

                string monthName = new DateTime(1, monthId, 1).ToString("MMMM") + " - " + new DateTime(1, yearId, 1).ToString("YYYY");
                string projectCode = rblProjects.SelectedItem.Text;
                string country = UserInfo.CountryName;

                DataTable dtEmails = DBContext.GetData("uspGetUserEmails", new object[] { countryID, projId });

                string emails = string.Empty;
                emails = "orsocharowca@gmail.com";


                if (dtEmails.Rows.Count > 0)
                {
                    using (MailMessage mailMsg = new MailMessage())
                    {
                        for (int i = 0; i < dtEmails.Rows.Count; i++)
                            emails += "," + Convert.ToString(dtEmails.Rows[i]["Email"]);

                        mailMsg.From = new MailAddress("orsocharowca@gmail.com");
                        mailMsg.To.Add(emails.TrimEnd(','));
                        mailMsg.Subject = subject;
                        mailMsg.IsBodyHtml = true;
                        mailMsg.Body = string.Format(@"Notification:" + Environment.NewLine +
                                                      "The user: : " + User.Identity.Name + " has " + mailType + " the report with following details:" + Environment.NewLine +
                                                      " Country: " + country + Environment.NewLine +
                                                      " Project: " + projectCode + Environment.NewLine +
                                                      " Report: " + monthName + Environment.NewLine);

                        Mail.SendMail(mailMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                //message = "You have been registered successfully but some error occoured on sending email to site admin. Contact admin and ask for the verification! We apologies for the inconvenience!";
            }
        }

        private void DeleteReport()
        {
            if (ReportId > 0)
                DBContext.Delete("DeleteReport", new object[] { ReportId, DBNull.Value });
        }

        private int SaveReport()
        {
            DeleteReportAccumulatives();
            SaveReportLocations();
            return SaveReportDetails();
        }

        private void UpdateReportUpdatedDate()
        {
            //DBContext.Update("UpdateReportUpdatedDate", new object[] { ReportId, RC.GetCurrentUserId, DBNull.Value });
        }

        private void SaveReportMainInfo()
        {
            int yearId = 0;
            int monthId = 0;
            int dayId = 0;

            string[] dateSplit = txtDate.Text.Trim().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (dateSplit.Length > 2)
            {
                yearId = Convert.ToInt32(dateSplit[2]);
                dayId = Convert.ToInt32(dateSplit[1]);
                monthId = Convert.ToInt32(dateSplit[0]);
            }

            int projId = RC.GetSelectedIntVal(rblProjects);
            Guid loginUserId = RC.GetCurrentUserId;
            string reportName = rblProjects.SelectedItem.Text + " (" + new DateTime(1, monthId, 1).ToString("MMMM") + "-14)";

            DateTime rDate = DateTime.ParseExact(txtDate.Text.Trim(), "MM-dd-yyyy", CultureInfo.InvariantCulture);
            ReportId = DBContext.Add("InsertReport_Ebola", new object[] { YearID, MonthID, 0, DayID, projId, UserInfo.EmergencyCountry, UserInfo.Organization, loginUserId, reportName, Convert.ToInt32(rblFrequency.SelectedValue), rDate, DBNull.Value });
        }

        private void SaveReportLocations()
        {
            List<int> locationIds = GetLocationIdsFromGrid();
            string locIds = string.Empty;

            foreach (int locationId in locationIds)
            {
                if (string.IsNullOrEmpty(locIds))
                    locIds = locationId.ToString();
                else
                    locIds += "," + locationId.ToString();
            }

            DBContext.Add("InsertReportLocations_Ebola", new object[] { ReportId, locIds, DBNull.Value });
        }

        private int SaveReportDetails()
        {
            int yearId = 0;
            string[] dateSplit = txtDate.Text.Trim().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (dateSplit.Length > 2)
                int.TryParse(dateSplit[2], out yearId);

            int activityDataId = 0;
            int projIndicatorId = 0;
            //int yearId = RC.GetSelectedIntVal(ddlYear);
            int returnCodeForUpdate = 0;

            foreach (GridViewRow row in gvIndicatorData.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int projectId = RC.GetSelectedIntVal(rblProjects);
                    activityDataId = Convert.ToInt32(gvIndicatorData.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());
                    projIndicatorId = Convert.ToInt32(gvIndicatorData.DataKeys[row.RowIndex].Values["ProjectIndicatorId"].ToString());

                    DataTable dtActivities = (DataTable)Session["dtActivities"];
                    List<KeyValuePair<int, decimal?>> dataSave = new List<KeyValuePair<int, decimal?>>();
                    int i = 0;

                    foreach (DataColumn dc in dtActivities.Columns)
                    {
                        string colName = dc.ColumnName;
                        int locationId = 0;

                        CheckBox cbAccum = row.FindControl(colName) as CheckBox;
                        if (cbAccum != null)
                        {
                            if (cbAccum.Checked)
                                SaveAccumulative(projectId, yearId, activityDataId, cbAccum.Checked);
                        }

                        HiddenField hf = row.FindControl("hf" + colName) as HiddenField;

                        if (hf != null)
                            locationId = Convert.ToInt32(hf.Value);

                        decimal? value = null;
                        TextBox t = row.FindControl(colName) as TextBox;
                        if (t != null)
                        {
                            if (!string.IsNullOrEmpty(t.Text))
                                value = Convert.ToDecimal(t.Text, CultureInfo.InvariantCulture);
                        }

                        if (locationId > 0)
                        {
                            dataSave.Add(new KeyValuePair<int, decimal?>(locationId, value));

                            int locationIdToSave = 0;
                            decimal? achieved = null;

                            foreach (var item in dataSave)
                            {
                                locationIdToSave = item.Key;
                                achieved = item.Value;
                            }

                            dataSave.Clear();

                            if (locationIdToSave > 0)
                            {
                                DateTime rDate = DateTime.ParseExact(txtDate.Text.Trim(), "MM-dd-yyyy", CultureInfo.InvariantCulture);
                                int returnCode = DBContext.Add("InsertReportDetails_E", new object[] { ReportId, activityDataId, locationIdToSave, achieved, 
                                                                    RC.GetCurrentUserId, projIndicatorId, DBNull.Value,Convert.ToInt32(rblFrequency.SelectedValue), MonthID, DayID,  rDate});

                                returnCodeForUpdate = returnCodeForUpdate == 0 ? returnCode : returnCodeForUpdate;
                            }

                        }
                    }
                }
            }

            return returnCodeForUpdate;
        }

        private void DeleteReportAccumulatives()
        {
            int yearId = 0;
            string[] dateSplit = txtDate.Text.Trim().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (dateSplit.Length > 2)
                int.TryParse(dateSplit[2], out yearId);

            int projectId = RC.GetSelectedIntVal(rblProjects);

            DBContext.Delete("DeleteReportAccumulatives_Ebola", new object[] { projectId, yearId, DBNull.Value });
        }

        private void SaveAccumulative(int projectId, int yearId, int activityDataId, bool isAccum)
        {
            DBContext.Add("InsertReportAccumulative_Ebola", new object[] { projectId, yearId, activityDataId, isAccum, RC.GetCurrentUserId, DBNull.Value });
        }

        public string GetPostBackControlId(Page page)
        {
            if (!page.IsPostBack)
                return "";

            Control control = null;

            // first we will check the "__EVENTTARGET" because if post back made by the controls
            // which used "_doPostBack" function also available in Request.Form collection.

            string controlName = page.Request.Params["__EVENTTARGET"];
            if (!string.IsNullOrEmpty(controlName))
                control = page.FindControl(controlName);
            else
            {
                // if __EVENTTARGET is null, the control is a button type and we need to
                // iterate over the form collection to find it

                string controlId;
                Control foundControl;

                foreach (string ctl in page.Request.Form)
                {
                    // handle ImageButton they having an additional "quasi-property" 
                    // in their Id which identifies mouse x and y coordinates
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        controlId = ctl.Substring(0, ctl.Length - 2);
                        foundControl = page.FindControl(controlId);
                    }
                    else
                    {
                        foundControl = page.FindControl(ctl);
                    }

                    if (!(foundControl is Button || foundControl is ImageButton)) continue;

                    control = foundControl;
                    break;
                }
            }

            return control == null ? String.Empty : control.ID;
        }

        private string GetSelectedLocations()
        {
            string admin1 = GetSelectedItems(cblAdmin1);
            string admin2 = GetSelectedItems(cblLocations);

            if (!string.IsNullOrEmpty(admin1) && string.IsNullOrEmpty(admin2))
                return admin1;
            else if (string.IsNullOrEmpty(admin1) && !string.IsNullOrEmpty(admin2))
                return admin2;
            else if (!string.IsNullOrEmpty(admin1) && !string.IsNullOrEmpty(admin2))
                return admin1 + ", " + admin2;

            return "";
        }

        private string GetNotSelectedLocations()
        {
            string admin1 = GetNotSelectedItems(cblAdmin1);
            string admin2 = GetNotSelectedItems(cblLocations);

            if (!string.IsNullOrEmpty(admin1) && string.IsNullOrEmpty(admin2))
                return admin1;
            else if (string.IsNullOrEmpty(admin1) && !string.IsNullOrEmpty(admin2))
                return admin2;
            else if (!string.IsNullOrEmpty(admin1) && !string.IsNullOrEmpty(admin2))
                return admin1 + ", " + admin2;

            return "";
        }

        private string GetSelectedItems(object sender)
        {
            string itemIds = "";
            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    if (itemIds != "")
                        itemIds += "," + item.Value;
                    else
                        itemIds += item.Value;
                }
            }

            return itemIds;
        }

        private string GetNotSelectedItems(object sender)
        {
            string itemIds = "";
            if (LocationRemoved == 1)
            {
                foreach (ListItem item in (sender as ListControl).Items)
                {
                    if (!item.Selected)
                    {
                        if (itemIds != "")
                            itemIds += "," + item.Value;
                        else
                            itemIds += item.Value;
                    }
                }
            }

            return itemIds;
        }

        private DataTable GetProjectsData(bool isPivot)
        {
            //int yearId = 0;
            //int monthId = 0;
            //int dayId = 0;

            //string[] dateSplit = txtDate.Text.Trim().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            //if (dateSplit.Length > 2)
            //{
            //    int.TryParse(dateSplit[2], out yearId);
            //    int.TryParse(dateSplit[1], out monthId);
            //    int.TryParse(dateSplit[0], out dayId);
            //}

            int projectId = RC.GetSelectedIntVal(rblProjects);
            Guid userId = RC.GetCurrentUserId;

            string procedureName = "GetProjectsReportDataOfMultipleProjectsAndMonths";

            if (!isPivot)
                procedureName = "GetProjectsDataByLocations";

            DataTable dt = DBContext.GetData(procedureName, new object[] { UserInfo.EmergencyCountry, UserInfo.Organization, YearID, MonthID,
                                                                        projectId, RC.SelectedSiteLanguageId, userId});
            return dt;
        }

        private void GeneratePDF(DataTable dt)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 8, 8, 14, 6);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, outputStream);
                document.Open();

                WriteDataEntryPDF.GenerateDocument(document, dt);
                document.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}.pdf", UserInfo.CountryName));
                Response.BinaryWrite(outputStream.ToArray());
            }
        }

        private void BindCultureResourcesOfPage()
        {
            //lblLocAdmin1.Text = UserInfo.CountryName + " " + (string)GetLocalResourceObject("ReportDataEntry_PopulateAdmin1__Admin_1_Locations");
            lblLocAdmin1.Text = UserInfo.CountryName + " Region";
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        #endregion

        #region Properties & Enums

        public int ReportId
        {
            get
            {
                int reportId = 0;
                if (ViewState["ReportId"] != null)
                {
                    int.TryParse(ViewState["ReportId"].ToString(), out reportId);
                }

                return reportId;
            }
            set
            {
                ViewState["ReportId"] = value;
            }
        }

        public int LocationRemoved
        {
            get
            {
                int locationRemoved = 0;
                if (ViewState["LocationRemoved"] != null)
                {
                    int.TryParse(ViewState["LocationRemoved"].ToString(), out locationRemoved);
                }

                return locationRemoved;
            }
            set
            {
                ViewState["LocationRemoved"] = value;
            }
        }

        public int CommentsIndId
        {
            get
            {
                int commentsIndId = 0;
                if (ViewState["CommentsIndId"] != null)
                {
                    int.TryParse(ViewState["CommentsIndId"].ToString(), out commentsIndId);
                }

                return commentsIndId;
            }
            set
            {
                ViewState["CommentsIndId"] = value;
            }
        }

        #endregion

        protected void rblFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadGrid();
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            ReloadGrid();
        }
    }

    public class GridViewTemplate : ITemplate
    {
        private readonly DataControlRowType _templateType;
        private readonly string _columnName;
        private readonly string _controlType;
        private readonly string _txtBoxType;

        public GridViewTemplate(DataControlRowType type, string controlType, string colname, string txtBoxType = "Achieved")
        {
            _templateType = type;
            _controlType = controlType;
            _columnName = colname;
            _txtBoxType = txtBoxType;
        }

        public void InstantiateIn(Control container)
        {
            // Create the content for the different row types.
            if (_templateType == DataControlRowType.Header)
            {
                if (_columnName == "ACM")
                {
                    Label lc = new Label { Width = 25, Text = "<b>ACM</b>" };
                    container.Controls.Add(lc);
                }
                else
                {
                    string[] words = _columnName.Split('^');
                    Label lc = new Label { Width = 50, Text = "<b>" + words[1] + "</b>" };
                    container.Controls.Add(lc);
                }
            }
            else if (_templateType == DataControlRowType.DataRow)
            {
                if (_controlType == "TextBox")
                {
                    TextBox txtVariable = new TextBox { CssClass = "numeric1", Width = 50 };
                    txtVariable.DataBinding += txtVariable_DataBinding;
                    container.Controls.Add(txtVariable);

                    HiddenField hf = new HiddenField();
                    string[] words1 = _columnName.Split('^');
                    hf.Value = words1[0];
                    hf.ID = "hf" + _columnName;
                    container.Controls.Add(hf);

                    //if (_txtBoxType == "Variable")
                    //{
                    string color = RC.ConfigSettings("AnnualTargetTextBoxColor");
                    txtVariable.BackColor = System.Drawing.ColorTranslator.FromHtml(color);
                    //}
                }
                else if (_controlType == "CheckBox")
                {
                    CheckBox cbLocAccum = new CheckBox();
                    cbLocAccum.DataBinding += cbAccum_DataBinding;
                    container.Controls.Add(cbLocAccum);
                }
            }
        }

        private void txtVariable_DataBinding(Object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            txt.ID = _columnName;
            txt.MaxLength = 12;

            GridViewRow row = (GridViewRow)txt.NamingContainer;
            txt.Text = DataBinder.Eval(row.DataItem, _columnName).ToString();
        }

        private void cbAccum_DataBinding(Object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            cb.ID = _columnName;

            GridViewRow row = (GridViewRow)cb.NamingContainer;
            cb.Checked = (DataBinder.Eval(row.DataItem, _columnName)).ToString() == "True";
        }
    }
}
