using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;

namespace SRFROWCA.Pages
{
    public partial class AddActivities : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!IsPostBack)
            {
                PopulateDropDowns();
            }

            string controlName = GetPostBackControlId(this);
            if (controlName == "ddlMonth" || controlName == "ddlOffice")
            {
                LocationRemoved = 0;
                lstSelectedLocations.Items.Clear();
            }

            DataTable dtActivities = GetActivities();
            AddDynamicColumnsInGrid(dtActivities);
            GetReport(dtActivities);
        }

        #region Events.

        #region DropDownLists Events
        protected void ddlEmergency_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationRemoved = 0;
            BindGridData();
            AddLocationsInSelectedList();
        }
        protected void ddlOffice_SelectedIndexChanged(object sender, EventArgs e)
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
            AddLocationsInSelectedList();
            BindGridData();
        }
        #endregion

        #region Location List Box Events.
        protected void btnAddAll_Click(object sender, EventArgs e)
        {
            List<ListItem> sortedList = GetSortedList(lstLocations, lstSelectedLocations, null);

            if (sortedList.Count > 0)
            {
                LocationRemoved = 0;
                lstSelectedLocations.Items.Clear();
                lstSelectedLocations.Items.AddRange(sortedList.ToArray());
            }

            // Remove all items from list.
            lstLocations.Items.Clear();

            if (lstSelectedLocations.Items.Count > 0)
            {
                lstSelectedLocations.SelectedIndex = 0;
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (lstLocations.SelectedIndex > -1)
            {
                LocationRemoved = 0;
                List<ListItem> items = new List<ListItem>();

                for (int i = 0; i < lstLocations.Items.Count; i++)
                {
                    if (lstLocations.Items[i].Selected)
                    {
                        items.Add(lstLocations.Items[i]);
                    }
                }

                foreach (ListItem selectedItem in items)
                {
                    // Get sorted list items.
                    List<ListItem> sortedList = GetSortedList(lstSelectedLocations, null, selectedItem);

                    if (sortedList.Count > 0)
                    {
                        // Clear all items from list box.
                        lstSelectedLocations.Items.Clear();

                        // Add items in listbox.
                        lstSelectedLocations.Items.AddRange(sortedList.ToArray());
                    }

                    // Remove item from selected items phase list(on right);
                    lstLocations.Items.Remove(selectedItem);

                    // Select first item in selected phases list box.
                    if (lstSelectedLocations.Items.Count > 0)
                    {
                        lstSelectedLocations.SelectedIndex = 0;
                    }

                    // Select first item in phases list box.
                    if (lstLocations.Items.Count > 0)
                    {
                        lstLocations.SelectedIndex = 0;
                    }
                }
            }

            btnGetReports_Click(null, null);
        }
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstSelectedLocations.SelectedIndex > -1)
            {
                LocationRemoved = 1;
                List<ListItem> items = new List<ListItem>();

                for (int i = 0; i < lstSelectedLocations.Items.Count; i++)
                {
                    if (lstSelectedLocations.Items[i].Selected)
                    {
                        items.Add(lstSelectedLocations.Items[i]);
                    }
                }

                foreach (ListItem selectedItem in items)
                {
                    // Get sorted list items.
                    List<ListItem> sortedList = GetSortedList(lstLocations, null, selectedItem);

                    if (sortedList.Count > 0)
                    {
                        // Clear all items from list box.
                        lstLocations.Items.Clear();

                        // Add items in listbox.
                        lstLocations.Items.AddRange(sortedList.ToArray());
                    }

                    // Remove item from selected items phase list(on right);
                    lstSelectedLocations.Items.Remove(selectedItem);

                    // Select first item in selected phases list box.
                    if (lstSelectedLocations.Items.Count > 0)
                    {
                        lstSelectedLocations.SelectedIndex = 0;
                    }

                    // Select first item in phases list box.
                    if (lstLocations.Items.Count > 0)
                    {
                        lstLocations.SelectedIndex = 0;
                    }
                }
            }
        }
        protected void btnRemoveAll_Click(object sender, EventArgs e)
        {
            List<ListItem> sortedList = GetSortedList(lstSelectedLocations, lstLocations, null);

            if (sortedList.Count > 0)
            {
                LocationRemoved = 1;
                lstLocations.Items.Clear();
                lstLocations.Items.AddRange(sortedList.ToArray());
            }

            // Remove all items from listboxes.
            lstSelectedLocations.Items.Clear();

            // Select first item if exists.
            if (lstLocations.Items.Count > 0)
            {
                lstLocations.SelectedIndex = 0;
            }
        }
        #endregion

        #region Button Click Events.

        protected void btnGetReports_Click(object sender, EventArgs e)
        {
            Session["dtClone"] = null;
            CaptureDataFromGrid();
            BindGridData();
            UpdateGridWithData();
        }
        protected void btnLocation_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);

            int k = 0;
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (k > 0) break;
                k++;
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DataTable dtActivities = (DataTable)Session["dtActivities"];

                    //Dictionary<int, decimal?> dataSave = new Dictionary<int, decimal?>();
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

                    foreach (ListItem item in lstLocations.Items)
                    {
                        if (dataSave.Contains(Convert.ToInt32(item.Value)))
                        {
                            ListItem selectedItem = item;
                            itemsToDelete.Add(item);

                            // Get sorted list items.
                            List<ListItem> sortedList = GetSortedList(lstSelectedLocations, null, selectedItem);

                            if (sortedList.Count > 0)
                            {
                                // Clear all items from list box.
                                lstSelectedLocations.Items.Clear();

                                // Add items in listbox.
                                lstSelectedLocations.Items.AddRange(sortedList.ToArray());

                                lstSelectedLocations.Items.Clear();
                                lstSelectedLocations.Items.AddRange(sortedList.ToArray());
                            }

                            // Select first item in selected phases list box.
                            if (lstSelectedLocations.Items.Count > 0)
                            {
                                lstSelectedLocations.SelectedIndex = 0;
                            }
                        }
                    }

                    foreach (ListItem item in itemsToDelete)
                    {
                        lstLocations.Items.Remove(item);
                    }

                    if (lstLocations.Items.Count > 0)
                    {
                        lstLocations.SelectedIndex = 0;
                    }
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
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

                    lblMessage.Visible = true;
                    lblMessage.CssClass = "info-message";
                    lblMessage.Text = "Data Saved Successfully.";
                }
            }
            catch
            {
                //lblMessage.Visible = true;
                //lblMessage.Text = "Some Error Occoured, try again.";

                throw;
            }
        }

        #endregion

        #endregion

        #region Methods.

        private void PopulateDropDowns()
        {
            // Get details of user from aspnet_Users_Custom tbale
            DataTable dt = GetUserDetails();
            if (dt.Rows.Count > 0)
            {
                lblCountry.Text = dt.Rows[0]["LocationName"].ToString();
                lblOrganization.Text = dt.Rows[0]["OrganizationName"].ToString();

                // Set Header of Location List Box.
                lblLocationLevelOfCountry.Text = "Admin2 Locations of " + lblCountry.Text;

                LocationId = Convert.ToInt32(dt.Rows[0]["LocationId"].ToString());
                int organizationId = Convert.ToInt32(dt.Rows[0]["OrganizationId"].ToString());

                PopulateLocationEmergencies(LocationId);
                PopulateOffices(LocationId, organizationId);
                PopulateLocations(LocationId);
            }

            // Populate Year Drop Down.
            var result = DateTime.Parse(DateTime.Now.ToShortDateString(), new CultureInfo("en-US")).Year;
            ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByText(result.ToString()));

            // Populate Months Drop Down.
            var result1 = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
            ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByText(result1.ToString()));
        }

        #region Drop Downs Methods.

        // Populate Emergency Drop Down.
        private void PopulateLocationEmergencies(int locationId)
        {
            ddlEmergency.DataValueField = "LocationEmergencyId";
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
            DataTable dt = DBContext.GetData("GetLocationEmergencies", new object[] { locationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        // Populate Office Drop Down.
        private void PopulateOffices(int locationId, int organizationId)
        {
            ddlOffice.DataValueField = "OfficeId";
            ddlOffice.DataTextField = "OfficeName";

            ddlOffice.DataSource = GetOffices(locationId, organizationId);
            ddlOffice.DataBind();

            if (ddlOffice.Items.Count > 1)
            {
                ListItem item = new ListItem("Select Your Office", "0");
                ddlOffice.Items.Insert(0, item);
            }
        }
        private DataTable GetOffices(int locationId, int organizationId)
        {
            DataTable dt = DBContext.GetData("GetOrganizationOffices", new object[] { locationId, organizationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        // Populate Months Drop Down
        private void PopulateMonths()
        {
            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            ListItem item = new ListItem("Select Month", "0");
            ddlMonth.Items.Insert(0, item);

            var result = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
            ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByText(result.ToString()));
        }
        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths");
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        // Populate Years Drop Down
        private void PopulateYears()
        {
            ddlYear.DataValueField = "YearId";
            ddlYear.DataTextField = "Year";

            ddlYear.DataSource = GetYears();
            ddlYear.DataBind();

            ListItem item = new ListItem("Select Year", "0");
            ddlYear.Items.Insert(0, item);

            var result = DateTime.Parse(DateTime.Now.ToShortDateString(), new CultureInfo("en-US")).Year;
            ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByText(result.ToString()));
        }
        private DataTable GetYears()
        {
            DataTable dt = DBContext.GetData("GetYears");
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        #endregion

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

        protected void BindGridData()
        {
            DataTable dt = GetActivities();
            GetReport(dt);
        }

        private string GetSelectedLocationIds()
        {
            string locationIds = "";

            for (int i = 0; i < lstSelectedLocations.Items.Count; i++)
            {
                if (locationIds != "")
                {
                    locationIds += "," + lstSelectedLocations.Items[i].Value;
                }
                else
                {
                    locationIds += lstSelectedLocations.Items[i].Value;
                }
            }

            return locationIds;
        }

        private string GetNotSelectedLocationIds()
        {
            string locIdsNotIncluded = "";
            if (LocationRemoved == 1)
            {
                for (int i = 0; i < lstLocations.Items.Count; i++)
                {
                    if (locIdsNotIncluded != "")
                    {
                        locIdsNotIncluded += "," + lstLocations.Items[i].Value;
                    }
                    else
                    {
                        locIdsNotIncluded += lstLocations.Items[i].Value;
                    }
                }
            }

            return locIdsNotIncluded;
        }

        private DataTable GetActivities()
        {
            int locEmergencyId = 0;
            int.TryParse(ddlEmergency.SelectedValue, out locEmergencyId);

            int officeId = 0;
            int.TryParse(ddlOffice.SelectedValue, out officeId);

            int yearId = 0;
            int.TryParse(ddlYear.SelectedValue, out yearId);

            int monthId = 0;
            int.TryParse(ddlMonth.SelectedValue, out monthId);

            string locationIds = GetSelectedLocationIds();
            string locIdsNotIncluded = GetNotSelectedLocationIds();

            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            DataTable dt = DBContext.GetData("GetIPData", new object[] { locEmergencyId, locationIds, officeId, yearId, monthId, locIdsNotIncluded, userId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void AddLocationsInSelectedList()
        {
            lstSelectedLocations.Items.Clear();
            PopulateLocations(LocationId);
        }

        private void PopulateLocations(int parentLocationId)
        {
            lstLocations.DataValueField = "LocationId";
            lstLocations.DataTextField = "LocationName";

            lstLocations.DataSource = GetChildLocations(parentLocationId);
            lstLocations.DataBind();

            if (lstLocations.Items.Count > 0)
            {
                lstLocations.SelectedIndex = 0;
            }
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
                if (!(columnName == "ReportId" || columnName == "ClusterName" || columnName == "IndicatorName" || columnName == "ActivityDataId" || columnName == "ActivityName" || columnName == "DataName" || columnName == "IsActive"))
                {
                    customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, column.ColumnName, "1");
                    customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, column.ColumnName, "1");
                    gvActivities.Columns.Add(customField);
                }
            }
        }

        private object GetChildLocations(int parentLocationId)
        {
            DataTable dt = DBContext.GetData("GetThirdLevelChildLocations", new object[] { parentLocationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private DataTable GetUserDetails()
        {
            Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;
            return DBContext.GetData("GetUserDetails", new object[] { userGuid });
        }

        private DataTable GetOrganizations()
        {
            DataTable dt = DBContext.GetData("GetOrganizations");
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private DataTable GetCountries()
        {
            int locationType = (int)ROWCATypes.LocationTypes.Governorate;
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
            int officeId = Convert.ToInt32(ddlOffice.SelectedValue);
            int locEmergencyId = Convert.ToInt32(ddlEmergency.SelectedValue);
            int yearId = Convert.ToInt32(ddlYear.SelectedValue);
            int monthId = Convert.ToInt32(ddlMonth.SelectedValue);
            int reportFrequencyId = 1;
            //int emergencyClusterId = Convert.ToInt32(ddlEmergency.SelectedValue);
            Guid loginUserId = (Guid)Membership.GetUser().ProviderUserKey;

            ReportId = DBContext.Add("InsertReport", new object[] { reportName, officeId, yearId, monthId, locEmergencyId, reportFrequencyId, loginUserId, DBNull.Value });
        }

        private bool IsDataExistsToSave()
        {
            bool returnValue = false;
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DataTable dtActivities = (DataTable)Session["dtActivities"];

                    //Dictionary<int, decimal?> dataSave = new Dictionary<int, decimal?>();
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
                                value = Convert.ToDecimal(t.Text);
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
                                    Guid loginUserId = (Guid)Membership.GetUser().ProviderUserKey;
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

        private void GetReport(DataTable dt)
        {
            Session["dtActivities"] = dt;

            int officeId = 0;
            int.TryParse(ddlOffice.SelectedValue, out officeId);

            int locEmergencyId = 0;
            int.TryParse(ddlEmergency.SelectedValue, out locEmergencyId);

            int yearId = 0;
            int.TryParse(ddlYear.SelectedValue, out yearId);

            int monthId = 0;
            int.TryParse(ddlMonth.SelectedValue, out monthId);
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;

            DataTable dtReport = DBContext.GetData("GetReportId", new object[] { officeId, locEmergencyId, yearId, monthId, userId });
            if (dtReport.Rows.Count > 0)
            {
                ReportId = string.IsNullOrEmpty(dtReport.Rows[0]["ReportId"].ToString()) ? 0 : Convert.ToInt32(dtReport.Rows[0]["ReportId"].ToString());
            }
            else
            {
                ReportId = 0;
            }

            gvActivities.DataSource = dt;
            gvActivities.DataBind();
        }

        protected void gvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["dtActivities"] == null) return;


                DataTable dtActivities = (DataTable)Session["dtActivities"];
                if (dtActivities == null) return;

                foreach (DataColumn dc in dtActivities.Columns)
                {
                    string colName = dc.ColumnName;
                    TextBox txt = e.Row.FindControl(colName) as TextBox;
                    if (txt != null)
                    {
                        if (txt.Text == "-1.00")
                        {
                            txt.Text = "";
                        }
                    }
                }
            }
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
        public int Count1
        {
            get
            {
                int count1 = 0;
                if (ViewState["Count1"] != null)
                {
                    int.TryParse(ViewState["Count1"].ToString(), out count1);
                }

                return count1;
            }
            set
            {
                ViewState["Count1"] = value.ToString();
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
        private DataControlRowType templateType;
        private string columnName;
        private string locationId;

        public GridViewTemplate(DataControlRowType type, string colname, string locId)
        {
            templateType = type;
            columnName = colname;
            locationId = locId;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            // Create the content for the different row types.
            switch (templateType)
            {
                case DataControlRowType.Header:
                    string[] words = columnName.Split('^');
                    Label lc = new Label();
                    lc.Width = 50;
                    lc.Text = "<b>" + words[1] + "</b>";
                    container.Controls.Add(lc);
                    //CheckBox cb = new CheckBox();
                    //if (columnName.Contains('T'))
                    //{
                    //    cb.ID = columnName;
                    //    container.Controls.Add(cb);
                    //}
                    break;
                case DataControlRowType.DataRow:
                    TextBox firstName = new TextBox();
                    firstName.CssClass = "numeric1";
                    firstName.Width = 50;
                    firstName.DataBinding += new EventHandler(this.FirstName_DataBinding);
                    container.Controls.Add(firstName);
                    HiddenField hf = new HiddenField();
                    string[] words1 = columnName.Split('^');
                    hf.Value = words1[0];
                    hf.ID = "hf" + columnName;
                    container.Controls.Add(hf);
                    break;

                default:
                    // Insert code to handle unexpected values.
                    break;
            }
        }

        private void FirstName_DataBinding(Object sender, EventArgs e)
        {
            TextBox l = (TextBox)sender;
            l.ID = columnName;
            GridViewRow row = (GridViewRow)l.NamingContainer;
            l.Text = DataBinder.Eval(row.DataItem, columnName).ToString();
        }
    }
}