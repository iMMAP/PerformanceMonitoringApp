using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Pages
{
    public partial class AddActivities : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string languageChange = "";
            if (Session["SiteChanged"] != null)
            {
                languageChange = Session["SiteChanged"].ToString();
            }

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
                {
                    rblProjects.SelectedIndex = 0;
                }

                PopulateObjectives();
                PopulatePriorities();
            }

            PopulateToolTips();

            //this.Form.DefaultButton = this.btnSave.UniqueID;
            string controlName = GetPostBackControlId(this);

            if (controlName == "ddlMonth" || controlName == "ddlYear" || controlName == "rblProjects")
            {
                LocationRemoved = 0;
                RemoveSelectedLocations(cblAdmin1);
                RemoveSelectedLocations(cblLocations);
            }

            if (controlName != "imgbtnComments")
            {
                DataTable dtActivities = GetActivities();
                AddDynamicColumnsInGrid(dtActivities);
                Session["dtActivities"] = dtActivities;
                GetReportId();
                gvActivities.DataSource = dtActivities;
                gvActivities.DataBind();
            }
        }

        #region Events.

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
        protected void rblProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridData();
            AddLocationsInSelectedList();
        }

        protected void gvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.ObjectiveIconToolTip(e, 0);
                ObjPrToolTip.PrioritiesIconToolTip(e, 1);
                ObjPrToolTip.RegionalIndicatorIcon(e, 12);
                ObjPrToolTip.CountryIndicatorIcon(e, 13);
            }
        }
        protected void gvActivities_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "AddComments")
            {
                if (ReportId == 0)
                {
                    SaveReportMainInfo();
                }

                int rowIndex = int.Parse(e.CommandArgument.ToString());

                int activityDataId = 0;
                int.TryParse(gvActivities.DataKeys[rowIndex]["ActivityDataId"].ToString(), out activityDataId);

                if (activityDataId > 0)
                {
                    CommentsIndId = activityDataId;
                    //int yearId = 0;
                    //int.TryParse(ddlYear.SelectedValue, out yearId);
                    //int monthId = 0;
                    //int.TryParse(ddlMonth.SelectedValue, out monthId);
                    //int projectId = RC.GetSelectedIntVal(rblProjects);

                    //ucIndComments.ActivityDataId = activityDataId;
                    //ucIndComments.YearId = yearId;
                    //ucIndComments.MonthId = monthId;
                    //ucIndComments.ProjectId = projectId;
                    //ucIndComments.EmgLocationId = UserInfo.EmergencyCountry;
                    ucIndComments.LoadComments(ReportId, activityDataId);
                    mpeComments.Show();
                }
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
            string comments = ucIndComments.GetComments();
            if (!string.IsNullOrEmpty(comments))
            {
                using (ORSEntities db = new ORSEntities())
                {
                    //int yearId = 0;
                    //int.TryParse(ddlYear.SelectedValue, out yearId);
                    //int monthId = 0;
                    //int.TryParse(ddlMonth.SelectedValue, out monthId);
                    //int projectId = RC.GetSelectedIntVal(rblProjects);
                    DBContext.Add("InsertIndicatorComments", new object[] { ReportId, CommentsIndId, comments, RC.GetCurrentUserId, DBNull.Value });
                }
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
                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                string fileName = UserInfo.CountryName + "_" + UserInfo.OrgName + "_" + ddlMonth.SelectedItem.Text + "_Report";
                ExportUtility.ExportGridView(gv, fileName, ".xls", Response, true);
            }
        }
        protected void btnPDF_Export(object sender, EventArgs e)
        {
            SaveProjectData();
            DataTable dt = GetProjectsData(false);
            if (dt.Rows.Count > 0)
            {
                GeneratePDF(dt);
            }
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            ShowMessage("<b>Some Error Occoured. Admin Has Notified About It</b>.<br/> Please Try Again.", RC.NotificationType.Error);

            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, "AddActivites", this.User);
        }

        #endregion

        #region Methods.

        // Populate Months Drop Down
        private void PopulateMonths()
        {
            int i = ddlMonth.SelectedIndex;

            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            var result = DateTime.Now.ToString("MMMM", new CultureInfo(RC.SiteCulture));
            result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);
            ddlMonth.SelectedIndex = i > -1 ? i : ddlMonth.Items.IndexOf(ddlMonth.Items.FindByText(result));
        }
        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        // Populate Years Drop Down
        private void PopulateYears()
        {
            ddlYear.DataValueField = "YearId";
            ddlYear.DataTextField = "Year";

            ddlYear.DataSource = GetYears();
            ddlYear.DataBind();

            var result = DateTime.Parse(DateTime.Now.ToShortDateString()).Year;
            ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByText(result.ToString()));
        }
        private DataTable GetYears()
        {
            DataTable dt = DBContext.GetData("GetYears");
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void PopulateLocations()
        {
            int countryId = UserInfo.Country;
            PopulateAdmin1(countryId);
            //PopulateAdmin2(countryId);
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
            DataTable dt = DBContext.GetData("GetSecondLevelChildLocations", new object[] { parentLocationId });
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
            rblProjects.DataValueField = "ProjectId";
            rblProjects.DataTextField = "ProjectCode";
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
            ObjPrToolTip.ObjectivesToolTip(cblObjectives);
            ObjPrToolTip.PrioritiesToolTip(cblPriorities);

            DataTable dt = GetUserProjects();
            ProjectsToolTip(rblProjects, dt);
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
            int yearId = 0;
            int.TryParse(ddlYear.SelectedValue, out yearId);

            int monthId = 0;
            int.TryParse(ddlMonth.SelectedValue, out monthId);

            int projectId = RC.GetSelectedIntVal(rblProjects);

            string locationIds = GetSelectedLocations();
            string locIdsNotIncluded = GetNotSelectedLocations();
            Guid userId = RC.GetCurrentUserId;

            DataTable dt = DBContext.GetData("GetIPData", new object[] { UserInfo.EmergencyCountry, locationIds, yearId, monthId,
                                                                        locIdsNotIncluded, RC.SelectedSiteLanguageId, userId,
                                                                        UserInfo.Organization, projectId});
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
                    columnName == "HumanitarianPriority" || columnName == "ActivityName" || columnName == "DataName" ||
                    columnName == "ActivityDataId" || columnName == "ProjectTitle" || columnName == "ProjectId" ||
                    columnName == "ObjAndPrId" || columnName == "ObjectiveId" || columnName == "HumanitarianPriorityId" ||
                    columnName == "objAndPrAndPId" || columnName == "objAndPId" || columnName == "PrAndPId" ||
                    columnName == "ProjectIndicatorId" || columnName == "RInd" || columnName == "CInd"
                    //|| columnName == "Accum" 
                    || columnName == "Unit"
                    ))
                {
                    if (columnName.Contains("Accum"))
                    {
                        customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, "CheckBox", column.ColumnName, "1");
                        //customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, "CheckBox", column.ColumnName, "1");
                        customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, "CheckBox", "ACM", "1");
                        gvActivities.Columns.Add(customField);
                    }
                    else
                    {
                        if (columnName.Contains("_1-"))
                        {
                            customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, "TextBox", column.ColumnName, "Annual");
                            customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, "TextBox", column.ColumnName, "Annual");
                            gvActivities.Columns.Add(customField);
                        }
                        else
                        {
                            customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, "TextBox", column.ColumnName);
                            customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, "TextBox", column.ColumnName);
                            gvActivities.Columns.Add(customField);
                        }
                    }
                }
            }
        }
        private void GetReportId()
        {
            int yearId = RC.GetSelectedIntVal(ddlYear);
            int monthId = RC.GetSelectedIntVal(ddlMonth);
            int projectId = RC.GetSelectedIntVal(rblProjects);

            using (ORSEntities db = new ORSEntities())
            {
                Report r = db.Reports.Where(x => x.ProjectId == projectId
                                            && x.YearId == yearId
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
                if (locationIds.Count > 0)
                {
                    if (ReportId == 0)
                    {
                        SaveReportMainInfo();
                    }

                    SaveReport();
                }
                else
                {
                    DeleteReport();
                }

                scope.Complete();
                ShowMessage((string)GetLocalResourceObject("AddActivities_SaveMessageSuccess"));
            }
        }

        private void DeleteReport()
        {
            if (ReportId > 0)
                DBContext.Delete("DeleteReport", new object[] { ReportId, DBNull.Value });
        }
        private void SaveReport()
        {
            //SaveReportMainInfo();
            DeleteReportAccumulatives();
            SaveReportLocations();
            SaveReportDetails();
        }

        private void SaveReportMainInfo()
        {
            int yearId = RC.GetSelectedIntVal(ddlYear);
            int monthId = RC.GetSelectedIntVal(ddlMonth);
            int projId = RC.GetSelectedIntVal(rblProjects);
            Guid loginUserId = RC.GetCurrentUserId;
            string reportName = rblProjects.SelectedItem.Text + " (" + ddlMonth.SelectedItem.Text + "-14)";
            ReportId = DBContext.Add("InsertReport", new object[] { yearId, monthId, projId, UserInfo.EmergencyCountry,
                                                                    UserInfo.Organization, loginUserId, reportName,  DBNull.Value });
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

        private void SaveReportDetails()
        {
            int activityDataId = 0;
            int projIndicatorId = 0;
            int yearId = RC.GetSelectedIntVal(ddlYear);

            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int projectId = RC.GetSelectedIntVal(rblProjects);
                    activityDataId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());
                    projIndicatorId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["ProjectIndicatorId"].ToString());

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
                            {
                                SaveAccumulative(projectId, yearId, activityDataId, cbAccum.Checked);
                            }
                        }

                        HiddenField hf = row.FindControl("hf" + colName) as HiddenField;
                        if (hf != null)
                        {
                            locationId = Convert.ToInt32(hf.Value);
                        }

                        decimal? value = null;
                        TextBox t = row.FindControl(colName) as TextBox;
                        if (t != null)
                        {
                            if (!string.IsNullOrEmpty(t.Text))
                            {
                                value = Convert.ToDecimal(t.Text, CultureInfo.InvariantCulture);
                            }
                        }

                        if (locationId > 0)
                        {
                            dataSave.Add(new KeyValuePair<int, decimal?>(locationId, value));
                            if (i == 1)
                            {
                                i = 0;
                                int locationIdToSave = 0;
                                decimal? annualTarget = null;
                                decimal? achieved = null;
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
                                    DBContext.Add("InsertReportDetails", new object[] { ReportId, activityDataId, locationIdToSave, achieved, 
                                                                                        RC.GetCurrentUserId, projIndicatorId, annualTarget, DBNull.Value });
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
        }

        private void DeleteReportAccumulatives()
        {
            int projectId = RC.GetSelectedIntVal(rblProjects);
            int yearId = RC.GetSelectedIntVal(ddlYear);
            DBContext.Delete("DeleteReportAccumulatives", new object[] { projectId, yearId, DBNull.Value });
        }

        private void SaveAccumulative(int projectId, int yearId, int activityDataId, bool isAccum)
        {
            DBContext.Add("InsertReportAccumulative", new object[] { projectId, yearId, activityDataId, isAccum, RC.GetCurrentUserId, DBNull.Value });
        }

        // In this method we will get the postback control.
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
            int yearId = 0;
            int.TryParse(ddlYear.SelectedValue, out yearId);

            int monthId = 0;
            int.TryParse(ddlMonth.SelectedValue, out monthId);

            int projectId = RC.GetSelectedIntVal(rblProjects);
            Guid userId = RC.GetCurrentUserId;

            string procedureName = "GetProjectsReportDataOfMultipleProjectsAndMonths";
            if (!isPivot)
            {
                procedureName = "GetProjectsDataByLocations";
            }

            DataTable dt = DBContext.GetData(procedureName, new object[] { UserInfo.EmergencyCountry, UserInfo.Organization, yearId, monthId,
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
            lblLocAdmin1.Text = UserInfo.CountryName + " " + (string)GetLocalResourceObject("AddActivities_PopulateAdmin1__Admin_1_Locations");
            //lblLocAdmin2.Text = UserInfo.CountryName + " " + (string)GetLocalResourceObject("AddActivities_PopulateAdmin2__Admin_2_Locations");

        }
        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, true, 500);
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
                    TextBox txtAchieved = new TextBox { CssClass = "numeric1", Width = 50 };
                    txtAchieved.DataBinding += txtAchieved_DataBinding;
                    container.Controls.Add(txtAchieved);
                    HiddenField hf = new HiddenField();
                    string[] words1 = _columnName.Split('^');
                    hf.Value = words1[0];
                    hf.ID = "hf" + _columnName;
                    container.Controls.Add(hf);
                    if (_txtBoxType == "Annual")
                    {
                        string color = RC.ConfigSettings("AnnualTargetTextBoxColor");
                        txtAchieved.BackColor = System.Drawing.ColorTranslator.FromHtml(color);
                    }
                }
                else if (_controlType == "CheckBox")
                {
                    CheckBox cbLocAccum = new CheckBox();
                    cbLocAccum.DataBinding += cbAccum_DataBinding;
                    container.Controls.Add(cbLocAccum);
                    //HiddenField hf = new HiddenField();
                    //string[] words1 = _columnName.Split('^');
                    //hf.Value = words1[0];
                    //hf.ID = "hf" + _columnName;
                    //container.Controls.Add(hf);
                }
            }
        }

        private void txtAchieved_DataBinding(Object sender, EventArgs e)
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