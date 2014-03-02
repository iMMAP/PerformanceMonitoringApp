using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using System.Text;
using System.Text.RegularExpressions;

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

            if (!IsPostBack && !string.IsNullOrEmpty(languageChange)) return;
            {
                PopulateDropDowns();
                PopulateObjectives();
                PopulatePriorities();

                Session["SiteChanged"] = null;
                cblObjectives.Items[0].Attributes["title"] = "STRATEGIC OBJECTIVE 1: Track and analyse risk and vulnerability, integrating findings into humanitarian and development programming.";
                cblObjectives.Items[1].Attributes["title"] = "STRATEGIC OBJECTIVE 2: Support vulnerable populations to better cope with shocks by responding earlier to warning signals, by reducing post-crisis recovery times and by building capacity of national actors.";
                cblObjectives.Items[2].Attributes["title"] = "STRATEGIC OBJECTIVE 3: Deliver coordinated and integrated life-saving assistance to people affected by emergencies.";
            }

            if (!IsPostBack)
            {
                PopulateLocations(LocationId);
                PopulateYears();
                PopulateMonths();
                UserInfo.UserProfileInfo();
            }

            this.Form.DefaultButton = this.btnSave.UniqueID;

            string controlName = GetPostBackControlId(this);
            if (controlName == "ddlMonth" || controlName == "ddlYear")
            {
                LocationRemoved = 0;
                RemoveSelectedLocations();
            }

            DataTable dtActivities = GetActivities();
            AddDynamicColumnsInGrid(dtActivities);
            Session["dtActivities"] = dtActivities;
            GetReportId(dtActivities);
            gvActivities.DataSource = dtActivities;
            gvActivities.DataBind();

            PopulateProjects(dtActivities);
        }
        private DataTable GetUserProjects()
        {
            DataTable dt = DBContext.GetData("GetOPSAndORSUserProjects", new object[] { UserInfo.GetCountry, UserInfo.GetOrganization });
            Session["testprojectdata"] = dt;
            return dt;
        }

        private void PopulateProjects(DataTable dtActivities)
        {
            cblProjects.DataValueField = "ProjectId";
            cblProjects.DataTextField = "ProjectCode";
            cblProjects.DataSource = GetUserProjects(); ;
            cblProjects.DataBind();
        }

        #region Events.

        protected void gvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image imgObj = e.Row.FindControl("imgObjective") as Image;
                if (imgObj != null)
                {
                    string txt = e.Row.Cells[0].Text;
                    if (txt.Contains("1"))
                    {
                        imgObj.ImageUrl = "~/images/icon/so1.png";
                        imgObj.ToolTip = "STRATEGIC OBJECTIVE 1: Track and analyse risk and vulnerability, integrating findings into humanitarian and evelopment programming.";
                    }
                    else if (txt.Contains("2"))
                    {
                        imgObj.ImageUrl = "~/images/icon/so2.png";
                        imgObj.ToolTip = "STRATEGIC OBJECTIVE 2: Support vulnerable populations to better cope with shocks by responding earlier to warning signals, by reducing post-crisis recovery times and by building capacity of national actors.";
                    }
                    else if (txt.Contains("3"))
                    {
                        imgObj.ImageUrl = "~/images/icon/so3.png";
                        imgObj.ToolTip = " STRATEGIC OBJECTIVE 3: Deliver coordinated and integrated life-saving assistance to people affected by emergencies.";
                    }
                }
                Image imghp = e.Row.FindControl("imgPriority") as Image;
                if (imghp != null)
                {
                    string txtHP = e.Row.Cells[1].Text;
                    if (txtHP == "1")
                    {
                        imghp.ImageUrl = "~/images/icon/hp1.png";
                        imghp.ToolTip = "Addressing the humanitarian impact Natural disasters (floods, etc.)";
                    }
                    else if (txtHP == "2")
                    {
                        imghp.ImageUrl = "~/images/icon/hp2.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Conflict (IDPs, refugees, protection, etc.)";
                    }
                    else if (txtHP == "3")
                    {
                        imghp.ImageUrl = "~/images/icon/hp3.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Epidemics (cholera, malaria, etc.)";
                    }
                    else if (txtHP == "4")
                    {
                        imghp.ImageUrl = "~/images/icon/hp4.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Food insecurity";
                    }
                    else if (txtHP == "5")
                    {
                        imghp.ImageUrl = "~/images/icon/hp5.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Malnutrition";
                    }

                }

                Image imgRind = e.Row.FindControl("imgRind") as Image;
                if (imgRind != null)
                {
                    if (e.Row.RowIndex == 2 || e.Row.RowIndex == 1)
                    {
                        imgRind.ImageUrl = "~/images/rind.png";
                        imgRind.ToolTip = "Regional Indicator";
                    }
                    else
                    {
                        imgRind.Visible = false;
                    }
                }

                Image imgCind = e.Row.FindControl("imgCind") as Image;
                if (imgCind != null)
                {
                    if (e.Row.RowIndex == 2 || e.Row.RowIndex == 3)
                    {
                        imgCind.ImageUrl = "~/images/cind.png";
                        imgCind.ToolTip = "Country Specific Indicator";
                    }
                    else
                    {
                        imgCind.Visible = false;
                    }
                }
            }
        }

        private string BreakString(string fullString)
        {
            const Int32 MAX_WIDTH = 60;
            int offset = 0;
            string text = Regex.Replace(fullString, @"\s{2,}", " ");
            List<string> lines = new List<string>();
            StringBuilder sb = new StringBuilder();

            while (offset < text.Length)
            {
                int index = text.LastIndexOf(" ",
                                 Math.Min(text.Length, offset + MAX_WIDTH));
                string line = text.Substring(offset,
                    (index - offset <= 0 ? text.Length : index) - offset);
                offset += line.Length + 1;
                lines.Add(line);
                sb.Append(line);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        protected void ddlEmergency_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationRemoved = 0;
            BindGridData();
            AddLocationsInSelectedList();
        }
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

        protected void btnLocation_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            LocationRemoved = 1;
            int k = 0;
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (k > 0) break;
                k++;
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DataTable dtActivities = (DataTable)Session["dtActivities"];
                    List<int> dataSave = new List<int>();
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

                        if (locationId > 0)
                        {
                            dataSave.Add(locationId);
                            if (i == 1)
                            {
                                i = 0;
                                int locationIdToSaveT = 0;
                                int j = 0;
                                foreach (var item in dataSave)
                                {
                                    if (j == 0)
                                    {
                                        locationIdToSaveT = Convert.ToInt32(item.ToString());
                                        j++;
                                    }
                                    else
                                    {
                                        j = 0;
                                    }
                                }
                            }
                            else
                            {
                                i = 1;
                            }
                        }
                    }

                    List<ListItem> itemsToDelete = new List<ListItem>();

                    foreach (ListItem item in cblLocations.Items)
                    {
                        if (dataSave.Contains(Convert.ToInt32(item.Value)))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (ReportId > 0)
                {
                    DeleteReportAndItsChild();
                }

                if (IsDataExistsToSave())
                {
                    SaveReport();
                }

                scope.Complete();
                ShowMessage("Your Data Saved Successfuly!");
            }
        }

        #endregion

        #region Methods.

        private void PopulateObjectives()
        {
            UI.FillObjectives(cblObjectives, true);
        }

        private void PopulatePriorities()
        {
            UI.FillPriorities(cblPriorities);
        }

        private void PopulateDropDowns()
        {
            // Get details of user from aspnet_Users_Custom tbale
            DataTable dt = RC.GetUserDetails();
            if (dt.Rows.Count > 0)
            {
                LocationId = Convert.ToInt32(dt.Rows[0]["LocationId"].ToString());
                int organizationId = Convert.ToInt32(dt.Rows[0]["OrganizationId"].ToString());

                PopulateLocationEmergencies(LocationId);
            }
        }

        // Populate Emergency Drop Down.
        private void PopulateLocationEmergencies(int locationId)
        {
            ddlEmergency.DataValueField = "EmergencyId";
            ddlEmergency.DataTextField = "EmergencyName";

            ddlEmergency.DataSource = GetLocationEmergencies(locationId);
            ddlEmergency.DataBind();

            if (ddlEmergency.Items.Count > 1)
            {
                ListItem item = new ListItem("Select Emergency", "0");
                ddlEmergency.Items.Insert(0, item);
            }
        }
        private DataTable GetLocationEmergencies(int locationId)
        {
            DataTable dt = DBContext.GetData("GetEmergencyOnLocation", new object[] { locationId, RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        // Populate Months Drop Down
        private void PopulateMonths()
        {
            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            var result = DateTime.Now.ToString("MMMM", new CultureInfo(RC.SiteCulture));
            result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);
            ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByText(result.ToString()));
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

        private DataTable GetEmergencyClusters(int emergencyId)
        {
            DataTable dt = DBContext.GetData("GetEmergencyClusters", new object[] { emergencyId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        internal override void BindGridData()
        {
            DataTable dt = GetActivities();
            Session["dtActivities"] = dt;
            GetReportId(dt);
            gvActivities.DataSource = dt;
            gvActivities.DataBind();
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

        private DataTable GetActivities()
        {
            int locEmergencyId = 0;
            int.TryParse(ddlEmergency.SelectedValue, out locEmergencyId);

            int yearId = 0;
            int.TryParse(ddlYear.SelectedValue, out yearId);

            int monthId = 0;
            int.TryParse(ddlMonth.SelectedValue, out monthId);

            string locationIds = GetSelectedLocations();
            string locIdsNotIncluded = GetNotSelectedLocations();

            Guid userId = RC.GetCurrentUserId;
            DataTable dt = DBContext.GetData("GetIPData", new object[] { locEmergencyId, locationIds, yearId, monthId,
                                                                        locIdsNotIncluded, RC.SelectedSiteLanguageId, userId,
                                                                        UserInfo.GetCountry, UserInfo.GetOrganization   });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void AddLocationsInSelectedList()
        {
            PopulateLocations(LocationId);
        }

        private void PopulateLocations(int parentLocationId)
        {
            PopulateAdmin1(parentLocationId);
            PopulateAdmin2(parentLocationId);

        }

        private void PopulateAdmin1(int parentLocationId)
        {
            DataTable dt = GetAdmin1Locations(parentLocationId);
            cblAdmin1.DataValueField = "LocationId";
            cblAdmin1.DataTextField = "LocationName";
            cblAdmin1.DataSource = dt;
            cblAdmin1.DataBind();
        }

        private void PopulateAdmin2(int parentLocationId)
        {
            DataTable dt = GetAdmin2Locations(parentLocationId);
            cblLocations.DataValueField = "LocationId";
            cblLocations.DataTextField = "LocationName";
            cblLocations.DataSource = dt;
            cblLocations.DataBind();
        }

        private DataTable GetReportLocations()
        {
            DataTable dt = DBContext.GetData("GetReportLocations", new object[] { ReportId });
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
                if (!(columnName == "ReportId" || columnName == "ClusterName" || columnName == "Objective" ||
                    columnName == "HumanitarianPriority" || columnName == "SecondaryCluster" || columnName == "ActivityName" ||
                    columnName == "DataName" || columnName == "IsActive" || columnName == "ActivityDataId" ||
                    columnName == "ProjectTitle" || columnName == "ProjectId" ||
                    columnName == "ObjAndPrId" || columnName == "ObjectiveId" || columnName == "HumanitarianPriorityId" ||
                    columnName == "objAndPrAndPId" || columnName == "objAndPId" || columnName == "PrAndPId"))
                {
                    if (columnName.Contains("_2-ACCUM"))
                    {
                        customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, "CheckBox", column.ColumnName, "1");
                        customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, "CheckBox", column.ColumnName, "1");
                        gvActivities.Columns.Add(customField);
                    }
                    else
                    {
                        customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, "TextBox", column.ColumnName, "1");
                        customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, "TextBox", column.ColumnName, "1");
                        gvActivities.Columns.Add(customField);
                    }
                }
            }
        }

        private DataTable GetAdmin1Locations(int parentLocationId)
        {
            DataTable dt = DBContext.GetData("GetSecondLevelChildLocations", new object[] { parentLocationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private DataTable GetAdmin2Locations(int parentLocationId)
        {
            DataTable dt = DBContext.GetData("GetThirdLevelChildLocations", new object[] { parentLocationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private DataTable GetOrganizations()
        {
            DataTable dt = DBContext.GetData("GetOrganizations");
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private DataTable GetCountries()
        {
            int locationType = (int)RC.LocationTypes.Governorate;
            DataTable dt = DBContext.GetData("GetLocationOnType", new object[] { locationType });

            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        public static List<ListItem> GetSortedList(ListBox sourceListBox, ListBox destinationListBox, ListItem selectedItem)
        {
            List<ListItem> sortedList = new List<ListItem>();

            // Add all items from source listbox to sortedList List.
            if (sourceListBox != null)
            {
                foreach (ListItem item in sourceListBox.Items)
                {
                    sortedList.Add(item);
                }
            }

            // Add all items from destination listbox to sortedList List.
            // We need this to sort items which are already in listbox.
            if (destinationListBox != null)
            {
                foreach (ListItem item in destinationListBox.Items)
                {
                    sortedList.Add(item);
                }
            }

            // If items is passed from calling method then add it in sortedList.
            // selectedItem will have data when only one item is being add/remove
            if (selectedItem != null)
            {
                sortedList.Add(selectedItem);
            }

            // Sort items in listbox.
            sortedList = sortedList.OrderBy(li => li.Text).ToList();

            return sortedList;
        }

        private void DeleteReportAndItsChild()
        {
            //DeleteReportDetail();
            DeleteReport();
        }

        private void DeleteReport()
        {
            DBContext.Delete("DeleteReport", new object[] { ReportId, DBNull.Value });
        }

        private void DeleteReportDetail()
        {
            DBContext.Delete("DeleteReportDetail", new object[] { ReportId, DBNull.Value });
        }

        private void SaveReport()
        {
            SaveReportMainInfo();
            SaveReportLocations();
            SaveReportDetails();
        }

        private void SaveReportLocations()
        {
            int k = 0;
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (k > 0) break;
                k++;
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DataTable dtActivities = (DataTable)Session["dtActivities"];

                    List<int> dataSave = new List<int>();
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

                        if (locationId > 0)
                        {
                            dataSave.Add(locationId);
                            if (i == 1)
                            {
                                i = 0;
                                int locationIdToSaveT = 0;
                                int j = 0;
                                foreach (var item in dataSave)
                                {
                                    if (j == 0)
                                    {
                                        locationIdToSaveT = Convert.ToInt32(item.ToString());
                                        j++;
                                    }
                                    else
                                    {
                                        j = 0;
                                    }
                                }

                                if (locationId > 0)
                                {
                                    DBContext.Add("InsertReportLocations", new object[] { ReportId, locationId, DBNull.Value });
                                }
                            }
                            else
                            {
                                i = 1;
                            }
                        }
                    }
                }
            }
        }

        private void SaveReportMainInfo()
        {
            string reportName = "test"; // TODO:
            int locEmergencyId = Convert.ToInt32(ddlEmergency.SelectedValue);
            int yearId = Convert.ToInt32(ddlYear.SelectedValue);
            int monthId = Convert.ToInt32(ddlMonth.SelectedValue);
            int reportFrequencyId = 1;
            Guid loginUserId = RC.GetCurrentUserId;

            ReportId = DBContext.Add("InsertReport", new object[] { reportName, yearId, monthId, locEmergencyId, reportFrequencyId, loginUserId, DBNull.Value });
        }

        private bool IsDataExistsToSave()
        {
            bool returnValue = false;
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DataTable dtActivities = (DataTable)Session["dtActivities"];

                    List<int> dataSave = new List<int>();
                    foreach (DataColumn dc in dtActivities.Columns)
                    {
                        string colName = dc.ColumnName;
                        int locationId = 0;
                        HiddenField hf = row.FindControl("hf" + colName) as HiddenField;
                        if (hf != null)
                        {
                            locationId = Convert.ToInt32(hf.Value);
                            if (locationId > 0)
                            {
                                returnValue = true;
                                break;
                            }
                        }
                    }

                    if (returnValue) break;
                }
            }

            return returnValue;
        }

        private void UpdateGridWithData()
        {
            string activityDataId = "";
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType != DataControlRowType.DataRow) return;

                activityDataId = gvActivities.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString();

                int colummCounts = gvActivities.Columns.Count;

                DataTable dtActivities = (DataTable)Session["dtActivities"];
                if (dtActivities == null) return;

                DataTable dtClone = (DataTable)Session["dtClone"];
                if (dtClone == null) return;

                foreach (DataColumn dc in dtActivities.Columns)
                {
                    string colName = dc.ColumnName;
                    if (dtClone.Columns.Contains(colName))
                    {
                        TextBox t = row.FindControl(colName) as TextBox;
                        if (t != null)
                        {
                            string val = dtClone.Rows[row.RowIndex][colName].ToString();
                            t.Text = val;
                        }
                    }
                }
            }
        }

        protected void CaptureDataFromGrid()
        {
            string activityDataId = "";
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType != DataControlRowType.DataRow) return;

                activityDataId = gvActivities.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString();
                int colummCounts = gvActivities.Columns.Count;

                DataTable dtActivities = (DataTable)Session["dtActivities"];
                if (dtActivities == null) return;

                DataTable dtClone;
                if (Session["dtClone"] != null)
                {
                    dtClone = (DataTable)Session["dtClone"];
                }
                else
                {
                    dtClone = dtActivities.Copy();
                    foreach (DataRow dr in dtClone.Rows)
                    {
                        foreach (DataColumn dc in dtClone.Columns)
                        {
                            if (dc.DataType == typeof(string))
                            {
                                dr[dc] = "";
                            }
                        }
                    }
                }

                foreach (DataColumn dc in dtClone.Columns)
                {
                    string colName = dc.ColumnName;
                    TextBox t = row.FindControl(colName) as TextBox;
                    if (t != null)
                    {
                        dtClone.Rows[row.RowIndex][colName] = t.Text;
                        dtClone.Rows[row.RowIndex]["ActivityDataId"] = activityDataId;
                    }
                }

                Session["dtClone"] = dtClone;
            }
        }

        private void SaveReportDetails()
        {
            int activityDataId = 0;

            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    activityDataId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());

                    int colummCounts = gvActivities.Columns.Count;
                    DataTable dtActivities = (DataTable)Session["dtActivities"];
                    List<KeyValuePair<int, decimal?>> dataSave = new List<KeyValuePair<int, decimal?>>();
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

                        decimal? value = null;
                        TextBox t = row.FindControl(colName) as TextBox;
                        if (t != null)
                        {
                            if (!string.IsNullOrEmpty(t.Text))
                            {
                                value = Convert.ToDecimal(t.Text, System.Globalization.CultureInfo.InvariantCulture);
                            }
                        }

                        if (locationId > 0)
                        {
                            dataSave.Add(new KeyValuePair<int, decimal?>(locationId, value));
                            if (i == 1)
                            {
                                i = 0;
                                int locationIdToSaveT = 0;
                                decimal? valToSaveT = null;
                                decimal? valToSaveA = null;
                                int j = 0;
                                foreach (var item in dataSave)
                                {
                                    if (j == 0)
                                    {
                                        locationIdToSaveT = item.Key;
                                        valToSaveT = item.Value;
                                        j++;
                                    }
                                    else
                                    {
                                        valToSaveA = item.Value;
                                        j = 0;
                                    }
                                }

                                dataSave.Clear();

                                if (!(valToSaveA == null && valToSaveT == null))
                                {
                                    Guid loginUserId = RC.GetCurrentUserId;
                                    int newReportDetailId = DBContext.Add("InsertReportDetails",
                                                                            new object[] { ReportId, activityDataId, locationIdToSaveT, 
                                                                                            valToSaveT, valToSaveA, 1, loginUserId, DBNull.Value });
                                }
                            }
                            else
                            {
                                i = 1;
                            }
                        }
                    }
                }
            }
        }

        private void GetReportId(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                ReportId = dt.Rows[0]["ReportId"].ToString() == "" ? 0 : Convert.ToInt32(dt.Rows[0]["ReportId"].ToString());
            }
            else
            {
                ReportId = 0;
            }
        }

        private void RemoveSelectedLocations()
        {
            foreach (ListItem item in cblLocations.Items)
            {
                item.Selected = false;
            }
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success)
        {
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, true, 500);
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            ShowMessage("<b>Some Error Occoured. Admin Has Notified About It</b>.<br/> Please Try Again.", RC.NotificationType.Error);

            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "AddActivites", this.User);
        }

        #endregion

        #region Properties & Enums

        public int ReportId
        {
            get
            {
                int ReportId = 0;
                if (ViewState["ReportId"] != null)
                {
                    int.TryParse(ViewState["ReportId"].ToString(), out ReportId);
                }

                return ReportId;
            }
            set
            {
                ViewState["ReportId"] = value.ToString();
            }
        }

        public int LocationId
        {
            get
            {
                int locationId = 0;
                if (ViewState["LocationId"] != null)
                {
                    int.TryParse(ViewState["LocationId"].ToString(), out locationId);
                }

                return locationId;
            }
            set
            {
                ViewState["LocationId"] = value.ToString();
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
                ViewState["LocationRemoved"] = value.ToString();
            }
        }

        #endregion

    }

    public class GridViewTemplate : ITemplate
    {
        private DataControlRowType _templateType;
        private string _columnName;
        private string _locationId;
        private string _controlType;

        public GridViewTemplate(DataControlRowType type, string controlType, string colname, string locId)
        {
            _templateType = type;
            _controlType = controlType;
            _columnName = colname;
            _locationId = locId;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            // Create the content for the different row types.
            if (_templateType == DataControlRowType.Header)
            {
                string[] words = _columnName.Split('^');
                Label lc = new Label();
                lc.Width = 50;
                lc.Text = "<b>" + words[1] + "</b>";
                container.Controls.Add(lc);
            }
            else if (_templateType == DataControlRowType.DataRow)
            {
                if (_controlType == "TextBox")
                {
                    TextBox txtAchieved = new TextBox();
                    txtAchieved.CssClass = "numeric1";
                    txtAchieved.Width = 50;
                    txtAchieved.DataBinding += new EventHandler(this.txtAchieved_DataBinding);
                    container.Controls.Add(txtAchieved);
                    HiddenField hf = new HiddenField();
                    string[] words1 = _columnName.Split('^');
                    hf.Value = words1[0];
                    hf.ID = "hf" + _columnName;
                    container.Controls.Add(hf);
                }
                else if (_controlType == "CheckBox")
                {
                    CheckBox cbLocAccum = new CheckBox();
                    //firstName.CssClass = "numeric1";
                    //cbLocAccum.Width = 50;
                    cbLocAccum.DataBinding += new EventHandler(this.cbAccum_DataBinding);
                    container.Controls.Add(cbLocAccum);
                    HiddenField hf = new HiddenField();
                    string[] words1 = _columnName.Split('^');
                    hf.Value = words1[0];
                    hf.ID = "hf" + _columnName;
                    container.Controls.Add(hf);
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
            //cb.Text = DataBinder.Eval(row.DataItem, _columnName).ToString();
            bool isChecked = false;
            bool.TryParse((DataBinder.Eval(row.DataItem, _columnName)).ToString(), out isChecked);
            cb.Checked = isChecked;
        }
    }
}