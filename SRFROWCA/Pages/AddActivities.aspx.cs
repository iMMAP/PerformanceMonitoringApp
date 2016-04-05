using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Pages
{
    public partial class AddActivities : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserInfo.UserProfileInfo(RC.EmergencySahel2015);
            }

            string languageChange = "";
            if (Session["SiteChanged"] != null)
                languageChange = Session["SiteChanged"].ToString();

            if (!string.IsNullOrEmpty(languageChange))
            {
                PopulateMonths();
                Session["SiteChanged"] = null;
            }

            //var wdth = Page.Request.Params["width"];
            //var a = 
            //var width1 = Page.Request.Params["width"];

            if (!IsPostBack)
            {
                PopulateLocations();
                PopulateMonths();
                PopulateProjects();

                if (ddlProjects.Items.Count > 0)
                    ddlProjects.SelectedIndex = 0;
            }

            PopulateToolTips();

            //this.Form.DefaultButton = this.btnSave.UniqueID;
            string controlName = GetPostBackControlId(this);

            if (controlName == "ddlMonth" || controlName == "ddlProjects")
            {
                LocationRemoved = 0;
                RemoveSelectedLocations(cblAdmin1);
                RemoveSelectedLocations(cblLocations);
            }

            DataTable dtActivities = GetActivities();
            AddDynamicColumnsInGrid(dtActivities);
            Session["dtActivities"] = dtActivities;
            GetReportId();
            gvActivities.DataSource = dtActivities;
            gvActivities.DataBind();

        }

        #region Events

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationRemoved = 0;
            BindGridData();
            AddLocationsInSelectedList();
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationRemoved = 0;
            BindGridData();
            AddLocationsInSelectedList();
        }

        protected void ddlWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationRemoved = 0;
            BindGridData();
            AddLocationsInSelectedList();
        }

        protected void ddlProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridData();
            AddLocationsInSelectedList();
        }

        private DataTable AddColumnsInBeneficGrid(GridView gvBenefic)
        {
            DataTable dt = new DataTable();

            // Add columns in table.
            dt.Columns.Add("BeneficUnit");
            dt.Columns.Add("BeneficVal");
            dt.Columns.Add("BeneficValReported");

            // Add rows in table.
            dt.Rows.Add(new object[] { "Male", "", "" });
            dt.Rows.Add(new object[] { "Female", "", "" });
            dt.Rows.Add(new object[] { "Boys", "", "" });
            dt.Rows.Add(new object[] { "Girls", "", "" });
            dt.Rows.Add(new object[] { "Disabled", "", "" });
            dt.Rows.Add(new object[] { "Elderly", "", "" });
            dt.Rows.Add(new object[] { "Children < 2", "", "" });
            dt.Rows.Add(new object[] { "Kid (2-5)", "", "" });
            dt.Rows.Add(new object[] { "PLW", "", "" });

            return dt;
            // Fill grid.
            //gvBenefic.DataSource = dt;
            //gvBenefic.DataBind();
        }

        protected void gvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.ObjectiveIconToolTip(e, 0);
            }

            //var TheBrowserWidth = width.Value;

            e.Row.Cells[0].Visible = false;
            //e.Row.Cells[e.Row.Cells.Count - 1].Width = 60;            

            //for (int i = 6; i < cellCount; i++)
            //{   
            //    e.Row.Cells[i].Width = 60;
            //}
        }

        protected void btnImgClick(object sender, EventArgs e)
        {
            int j = 0;
            ImageButton btn = (ImageButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            //Get rowindex
            int rowIndex = gvr.RowIndex;

            if (ReportId == 0)
            {
                SaveReportMainInfo();
            }

            //SaveProjectData();

            //int rowIndex = int.Parse(e.CommandArgument.ToString());

            int activityDataId = 0;
            int.TryParse(gvActivities.DataKeys[rowIndex]["ActivityDataId"].ToString(), out activityDataId);

            if (activityDataId > 0)
            {
                CommentsIndId = activityDataId;

                if (ucIndComments.LoadComments(ReportId, activityDataId))
                    btnSaveComments.Visible = false;

                mpeComments.Show();
            }
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
            string comments = txtComments.Value;//ucIndComments.GetComments(); 
            int indictorCommentDetID = ucIndComments.GetIndicatorCommentDetailID();

            if (!string.IsNullOrEmpty(comments))
            {
                using (ORSEntities db = new ORSEntities())
                {
                    DBContext.Add("InsertIndicatorComments", new object[] { ReportId, CommentsIndId, comments, RC.GetCurrentUserId, DBNull.Value, indictorCommentDetID });
                }
            }
        }

        protected void btnCancelComments_Click(object sender, EventArgs e)
        {
            mpeComments.Hide();
        }

        protected void btnExcel_Export(object sender, EventArgs e)
        {
            //SaveProjectData();
            //DataTable dt = GetProjectsData(true);
            //if (dt.Rows.Count > 0)
            //{
            //    GridView gv = new GridView();
            //    gv.DataSource = dt;
            //    gv.DataBind();
            //    string fileName = UserInfo.CountryName + "_" + UserInfo.OrgName + "_" + ddlMonth.SelectedItem.Text + "_Report";
            //    ExportUtility.ExportGridView(gv, fileName, ".xls", Response, true);
            //}
        }

        protected void btnPDF_Export(object sender, EventArgs e)
        {
            //SaveProjectData();
            //DataTable dt = GetProjectsData(false);
            //if (dt.Rows.Count > 0)
            //{
            //    //_Commented_GeneratePDF_Commented(dt);
            //}
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            ShowMessage("<b>Some Error Occoured. Admin Has Notified About It</b>.<br/> Please Try Again.", RC.NotificationType.Error);

            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }

        #endregion

        #region Methods

        private void PopulateMonths()
        {
            int i = ddlMonth.SelectedIndex;

            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            var result = DateTime.Now.ToString("MMMM", new CultureInfo(RC.SiteCulture));
            result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);
            int monthNumber = MonthNumber.GetMonthNumber(result);
            monthNumber = monthNumber == 1 ? monthNumber : monthNumber - 1;
            ddlMonth.SelectedIndex = i > -1 ? i : ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(monthNumber.ToString()));
        }

        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private DataTable GetYears()
        {
            DataTable dt = DBContext.GetData("GetYears");
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void PopulateLocations()
        {
            int countryId = UserInfo.Country;
            if (RC.SelectedEmergencyId == 2)
            {
                PopulateAdmin2(countryId);
            }
            else
            {
                PopulateAdmin1(countryId);
            }
        }

        private void PopulateAdmin1(int parentLocationId)
        {
            DataTable dt = GetAdmin1Locations(parentLocationId);
            cblAdmin1.DataValueField = "LocationId";
            cblAdmin1.DataTextField = "LocationName";
            cblAdmin1.DataSource = dt;
            cblAdmin1.DataBind();

            lblLocAdmin1.Text = UserInfo.CountryName + " " + (string)GetLocalResourceObject("AddActivities_PopulateAdmin1__Admin_1_Locations");
        }

        private DataTable GetAdmin1Locations(int parentLocationId)
        {
            string procedure = "GetSecondLevelChildLocations";

            if (parentLocationId == 567)
            {
                procedure = "GetSecondLevelChildLocationsAndCountry";
            }

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
            //lblLocAdmin2.Text = UserInfo.CountryName + (string)GetLocalResourceObject("AddActivities_PopulateAdmin2__Admin_2_Locations");
        }

        private DataTable GetAdmin2Locations(int parentLocationId)
        {
            DataTable dt = DBContext.GetData("GetThirdLevelChildLocations", new object[] { parentLocationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void PopulateProjects()
        {
            DataTable dt = GetUserProjects();
            ddlProjects.DataValueField = "ProjectId";
            ddlProjects.DataTextField = "ProjectCode";
            ddlProjects.DataSource = dt;
            ddlProjects.DataBind();
        }

        private DataTable GetUserProjects()
        {
            int yearId = 11;//RC.GetSelectedIntVal(ddlFrameworkYear);
            return RC.GetOrgProjectsOnLocation(null, yearId);
        }

        private void PopulateToolTips()
        {
            DataTable dt = GetUserProjects();
            ProjectsToolTip(ddlProjects, dt);
        }

        private void ProjectsToolTip(ListControl ctl, DataTable dt)
        {
            foreach (ListItem item in ctl.Items)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (item.Text == row["ProjectCode"].ToString())
                        item.Attributes["title"] = row["ProjectTitle"].ToString();
                }
            }
        }

        private void RemoveSelectedLocations(ListControl control)
        {
            foreach (ListItem item in control.Items)
            {
                item.Selected = false;
            }
        }

        private DataTable GetActivities()
        {
            int monthId = 0;
            int.TryParse(ddlMonth.SelectedValue, out monthId);

            int projectId = RC.GetSelectedIntVal(ddlProjects);

            string locationIds = GetSelectedLocations();
            string locIdsNotIncluded = GetNotSelectedLocations();
            Guid userId = RC.GetCurrentUserId;
            int yearId = 11; // RC.GetSelectedIntVal(ddlFrameworkYear);
            DataTable dt = DBContext.GetData("GetIPData2015", new object[] { UserInfo.EmergencyCountry, locationIds, yearId, monthId,
                                                                        locIdsNotIncluded, RC.SelectedSiteLanguageId, userId,
                                                                        UserInfo.Organization, projectId});
            if (dt.Rows.Count <= 0 && !string.IsNullOrEmpty(locationIds))
            {
                ShowMessage("Are you sure you have activites for this project. Please click on Manage Activites to select the activities you want to report on!", RC.NotificationType.Error, false);
            }

            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void AddDynamicColumnsInGrid(DataTable dt)
        {
            foreach (DataColumn column in dt.Columns)
            {
                TemplateField customField = new TemplateField();
                // Create the dynamic templates and assign them to 
                // the appropriate template property.

                string columnName = column.ColumnName;
                if (!(columnName == "ReportId" || columnName == "ProjectCode" || columnName == "Objective" ||
                     columnName == "Activity" || columnName == "Indicator" || columnName == "ActivityDataId" ||
                     columnName == "ProjectTitle" || columnName == "ProjectId" || columnName == "ObjectiveId" ||
                     columnName == "objAndPId" || columnName == "ProjectIndicatorId" || columnName == "Unit" || columnName == "ActivityId"
                    ))
                {
                    if (columnName.Contains("_AT"))
                    {
                        customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, column.ColumnName, "Annual");
                        customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, column.ColumnName, "Annual");
                        gvActivities.Columns.Add(customField);
                    }
                    else
                    {
                        customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, column.ColumnName);
                        customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, column.ColumnName);
                        gvActivities.Columns.Add(customField);
                    }
                }
            }
        }

        private void GetReportId()
        {
            int monthId = RC.GetSelectedIntVal(ddlMonth);
            int projectId = RC.GetSelectedIntVal(ddlProjects);

            using (ORSEntities db = new ORSEntities())
            {
                Report r = db.Reports.Where(x => x.ProjectId == projectId
                                            && x.YearId == (int)RC.Year._2015
                                            && x.MonthId == monthId
                                            && x.EmergencyLocationId == UserInfo.EmergencyCountry
                                            && x.OrganizationId == UserInfo.Organization).SingleOrDefault();
                ReportId = r != null ? r.ReportId : 0;
            }
        }

        internal override void BindGridData()
        {
            DataTable dt = GetActivities();
            Session["dtActivities"] = dt;
            GetReportId();
            gvActivities.DataSource = dt;
            gvActivities.DataBind();
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
            foreach (GridViewRow row in gvActivities.Rows)
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
            {
                item.Selected = locationIds.Contains(Convert.ToInt32(item.Value));
            }

            foreach (ListItem item in cblAdmin1.Items)
            {
                item.Selected = locationIds.Contains(Convert.ToInt32(item.Value));
            }
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
                        DeleteReporProjectPartners();
                        DeleteReport();
                        mailType = "Deleted";
                        SendMail("Report Delete Summary! " + DateTime.Now.ToString("dd-MMM-yyyy"), ReportId, mailType);
                    }
                }

                scope.Complete();
                ShowMessage((string)GetLocalResourceObject("AddActivities_SaveMessageSuccess"));
            }
        }

        private void SendMail(string subject, int reportID, string mailType)
        {
            try
            {
                int countryID = UserInfo.Country > 0 ? UserInfo.Country : 0;
                int projId = RC.GetSelectedIntVal(ddlProjects);

                string monthName = ddlMonth.SelectedItem.Text + " - 2015";
                string projectCode = ddlProjects.SelectedItem.Text;
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
                        mailMsg.To.Add("orsocharowca@gmail.com");
                        mailMsg.Bcc.Add(emails.TrimEnd(','));
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
            DeleteReporProjectPartners();
            SaveReportLocations();
            return SaveReportDetails();
        }

        private void DeleteReporProjectPartners()
        {
            DBContext.Delete("DeleteReportProjectPartners", new object[] { ReportId, DBNull.Value });
        }

        private void UpdateReportUpdatedDate()
        {
            DBContext.Update("UpdateReportUpdatedDate", new object[] { ReportId, RC.GetCurrentUserId, DBNull.Value });
        }

        private void SaveReportMainInfo()
        {
            int yearId = 11;// RC.GetSelectedIntVal(ddlYear);
            int monthId = RC.GetSelectedIntVal(ddlMonth);
            int projId = RC.GetSelectedIntVal(ddlProjects);
            Guid loginUserId = RC.GetCurrentUserId;
            int reportingYear = 2015;
            string reportName = ddlProjects.SelectedItem.Text + " (" + ddlMonth.SelectedItem.Text + "15)";
            ReportId = DBContext.Add("InsertReport2015", new object[] { yearId, monthId, projId, UserInfo.Organization, 
                                                                        loginUserId, reportName, reportingYear, DBNull.Value });
        }

        private void SaveReportLocations()
        {
            List<int> locationIds = GetLocationIdsFromGrid();
            string locIds = "";
            foreach (int locationId in locationIds)
            {
                if (string.IsNullOrEmpty(locIds))
                {
                    locIds = locationId.ToString();
                }
                else
                {
                    locIds += "," + locationId.ToString();
                }
            }

            DBContext.Add("InsertReportLocations", new object[] { ReportId, locIds, DBNull.Value });
        }

        private int SaveReportDetails()
        {
            int activityDataId = 0;
            int projIndicatorId = 0;
            int monthId = RC.GetSelectedIntVal(ddlMonth);
            int activityId = 0;
            int returnCodeForUpdate = 0;
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int projectId = RC.GetSelectedIntVal(ddlProjects);
                    activityDataId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());
                    projIndicatorId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["ProjectIndicatorId"].ToString());
                    activityId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["ActivityId"].ToString());

                    DataTable dtActivities = (DataTable)Session["dtActivities"];
                    List<KeyValuePair<int, int?>> dataSave = new List<KeyValuePair<int, int?>>();
                    int i = 0;

                    foreach (DataColumn dc in dtActivities.Columns)
                    {
                        string colName = dc.ColumnName;
                        int locationId = 0;

                        HiddenField hf = row.FindControl("hf" + colName) as HiddenField;
                        if (hf != null)
                        {
                            locationId = Convert.ToInt32(hf.Value);
                        }

                        int? value = null;
                        TextBox t = row.FindControl(colName) as TextBox;
                        if (t != null)
                        {
                            if (!string.IsNullOrEmpty(t.Text))
                            {
                                value = Convert.ToInt32(t.Text, CultureInfo.InvariantCulture);
                            }
                        }

                        if (locationId > 0)
                        {
                            dataSave.Add(new KeyValuePair<int, int?>(locationId, value));
                            if (i == 1)
                            {
                                i = 0;
                                int locationIdToSave = 0;
                                int? annualTarget = null;
                                int? achieved = null;
                                int j = 0;
                                foreach (var item in dataSave)
                                {
                                    if (j == 0)
                                    {
                                        locationIdToSave = item.Key;
                                        annualTarget = item.Value;
                                        j++;
                                    }
                                    else
                                    {
                                        achieved = item.Value;
                                        j = 0;
                                    }
                                }

                                dataSave.Clear();
                                if (locationIdToSave > 0)
                                {
                                    if (achieved != null)
                                    {
                                        DBContext.Add("InsertReportProjectPartners", new object[] { ReportId, projectId, monthId, RC.Year._2015, activityId, 
                                                                                                locationIdToSave, UserInfo.Organization, RC.GetCurrentUserId, DBNull.Value });
                                    }

                                    int returnCode = DBContext.Add("InsertReportDetails", new object[] { ReportId, activityDataId, locationIdToSave, achieved, 
                                                                    RC.GetCurrentUserId, projIndicatorId, annualTarget, DBNull.Value });

                                    returnCodeForUpdate = returnCodeForUpdate == 0 ? returnCode : returnCodeForUpdate;
                                }
                            }
                            else
                            {
                                i += 1;
                            }
                        }
                    }
                }
            }

            return returnCodeForUpdate;
        }

        public string GetPostBackControlId(Page page)
        {
            // If page is requested first time then return.
            if (!page.IsPostBack)
                return "";

            Control control = null;
            // first we will check the "__EVENTTARGET" because if post back made by the controls
            // which used "_doPostBack" function also available in Request.Form collection.
            string controlName = page.Request.Params["__EVENTTARGET"];
            if (!String.IsNullOrEmpty(controlName))
            {
                control = page.FindControl(controlName);
            }
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
            {
                return admin1;
            }
            else if (string.IsNullOrEmpty(admin1) && !string.IsNullOrEmpty(admin2))
            {
                return admin2;
            }
            else if (!string.IsNullOrEmpty(admin1) && !string.IsNullOrEmpty(admin2))
            {
                return admin1 + ", " + admin2;
            }

            return "";
        }

        private string GetNotSelectedLocations()
        {
            string admin1 = GetNotSelectedItems(cblAdmin1);
            string admin2 = GetNotSelectedItems(cblLocations);

            if (!string.IsNullOrEmpty(admin1) && string.IsNullOrEmpty(admin2))
            {
                return admin1;
            }
            else if (string.IsNullOrEmpty(admin1) && !string.IsNullOrEmpty(admin2))
            {
                return admin2;
            }
            else if (!string.IsNullOrEmpty(admin1) && !string.IsNullOrEmpty(admin2))
            {
                return admin1 + ", " + admin2;
            }

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
                    {
                        itemIds += "," + item.Value;
                    }
                    else
                    {
                        itemIds += item.Value;
                    }
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
                        {
                            itemIds += "," + item.Value;
                        }
                        else
                        {
                            itemIds += item.Value;
                        }
                    }
                }
            }

            return itemIds;
        }

        private DataTable GetProjectsData(bool isPivot)
        {
            int monthId = 0;
            int.TryParse(ddlMonth.SelectedValue, out monthId);

            int projectId = RC.GetSelectedIntVal(ddlProjects);
            Guid userId = RC.GetCurrentUserId;

            string procedureName = "GetProjectsReportDataOfMultipleProjectsAndMonths";
            if (!isPivot)
            {
                procedureName = "GetProjectsDataByLocations";
            }

            DataTable dt = DBContext.GetData(procedureName, new object[] { UserInfo.EmergencyCountry, UserInfo.Organization, RC.Year._2015,
                                                                            monthId, projectId, RC.SelectedSiteLanguageId, userId});
            return dt;
        }
        

        //private void GeneratePDF(DataTable dt)
        //{
        //    using (MemoryStream outputStream = new MemoryStream())
        //    {
        //        iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 8, 8, 14, 6);
        //        iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, outputStream);
        //        document.Open();
        //        WriteDataEntryPDF.GenerateDocument(document, dt);
        //        document.Close();
        //        Response.ContentType = "application/pdf";
        //        Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}.pdf", UserInfo.CountryName));
        //        Response.BinaryWrite(outputStream.ToArray());
        //    }
        //}

        private void BindCultureResourcesOfPage()
        {
            lblLocAdmin1.Text = UserInfo.CountryName + " " + (string)GetLocalResourceObject("AddActivities_PopulateAdmin1__Admin_1_Locations");
            //lblLocAdmin2.Text = UserInfo.CountryName + " " + (string)GetLocalResourceObject("AddActivities_PopulateAdmin2__Admin_2_Locations");

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

        protected void gvActivities_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvHeaderRow = e.Row;
                GridViewRow gvHeaderRowCopy = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                this.gvActivities.Controls[0].Controls.AddAt(0, gvHeaderRowCopy);

                int headerCellCount = gvHeaderRow.Cells.Count;
                TableCell tcMergeProduct = new TableCell();
                tcMergeProduct.Text = "-";
                tcMergeProduct.ColumnSpan = 5;
                gvHeaderRowCopy.Cells.AddAt(0, tcMergeProduct);

                if (headerCellCount > 6)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        int j = 1;
                        for (int i = 6; i < headerCellCount; i++)
                        {
                            string headerText = ((e.Row.Cells[i].Controls[0]) as Label).Text;
                            string cityName = headerText.Substring(0, headerText.IndexOf('_'));

                            TableCell tcHeader = gvHeaderRow.Cells[i];
                            TableCell tcMergePackage0 = new TableCell();
                            tcMergePackage0.Text = "<center>" + cityName + "</b></center>";
                            tcMergePackage0.HorizontalAlign = HorizontalAlign.Center;
                            tcMergePackage0.ColumnSpan = 2;
                            gvHeaderRowCopy.Cells.AddAt(j, tcMergePackage0);
                            ((e.Row.Cells[i].Controls[0]) as Label).Text = "Annual</br>Target";
                            ((e.Row.Cells[++i].Controls[0]) as Label).Text = "Monthly</br>Achieved";
                            j++;
                        }
                    }
                }
            }
        }

    }

    public class GridViewTemplate : ITemplate
    {
        private readonly DataControlRowType _templateType;
        private readonly string _columnName;
        private readonly string _txtBoxType;

        public GridViewTemplate(DataControlRowType type, string colname, string txtBoxType = "Achieved")
        {
            _templateType = type;
            _columnName = colname;
            _txtBoxType = txtBoxType;
        }

        public void InstantiateIn(Control container)
        {
            // Create the content for the different row types.
            if (_templateType == DataControlRowType.Header)
            {
                string[] words = _columnName.Split(new string[] { "--" }, StringSplitOptions.None);
                Label lc = new Label { Width = 50, Text = "<b>" + words[1] + "</b>" };
                container.Controls.Add(lc);

            }
            else if (_templateType == DataControlRowType.DataRow)
            {
                TextBox txt = new TextBox { CssClass = "numeric1", Width = 50 };
                txt.DataBinding += txt_DataBinding;
                container.Controls.Add(txt);
                HiddenField hf = new HiddenField();
                string[] words1 = _columnName.Split(new string[] { "--" }, StringSplitOptions.None);
                hf.Value = words1[0];
                hf.ID = "hf" + _columnName;
                container.Controls.Add(hf);

                if (_txtBoxType == "Annual")
                    txt.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFF0");
            }
        }

        private void txt_DataBinding(Object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            txt.ID = _columnName;
            txt.MaxLength = 8;
            txt.CssClass = "numeric1";
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            txt.Text = DataBinder.Eval(row.DataItem, _columnName).ToString();
        }
    }
}