using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.OPS
{
    public partial class OPSDataEntry : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetOPSIds();
                if (OPSProjectId > 0 && !string.IsNullOrEmpty(OPSClusterName) && !string.IsNullOrEmpty(OPSCountryName))
                {
                    OPSLocationEmergencyId = GetEmergencyId();
                    OPSEmergencyClusterId = GetClusterId();
                    lblCluster.Text = OPSClusterName;
                }
                PopulateDropDowns();
            }

            this.Form.DefaultButton = this.btnSave.UniqueID;

            DataTable dtActivities = GetActivities();
            AddDynamicColumnsInGrid(dtActivities);
            GetReport(dtActivities);

        }

        #region Events.

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
            LocationRemoved = 1;
            int k = 0;
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (k > 0) break;
                k++;
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DataTable dtActivities = (DataTable)Session["dtOPSActivities"];

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

                    foreach (ListItem item in cbAdmin1Locaitons.Items)
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
            SaveData();
        }

        private void SaveData()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (OPSReportId > 0)
                {
                    DeleteReportAndItsChild();
                }

                if (IsDataExistsToSave())
                {
                    SaveReport();
                }

                scope.Complete();

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "popup",
                //    "$('#divMsg').addClass('info message').text('Hello There').animate({ top: '0' }, 500).fadeOut(4000, function() {});", true);
                ShowMessage("Your Data Saved Successfuly!");
            }
        }

        #endregion

        #endregion

        #region Methods.

        private void SetOPSIds()
        {
            int tempVal = 0;
            if (Request.QueryString["uid"] != null)
            {
                int.TryParse(Request.QueryString["uid"].ToString(), out tempVal);
                OPSUserId = tempVal;
            }

            if (Request.QueryString["pid"] != null)
            {
                tempVal = 0;
                int.TryParse(Request.QueryString["pid"].ToString(), out tempVal);
                OPSProjectId = tempVal;
            }

            if (Request.QueryString["clname"] != null)
            {
                OPSClusterName = Request.QueryString["clname"].ToString();
            }

            if (Request.QueryString["cname"] != null)
            {
                OPSCountryName = Request.QueryString["cname"].ToString();
            }
        }

        private int GetEmergencyId()
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(OPSCountryName))
            {
                //Mali Complex Emergency
                //Mauritania Complex
                //Burkina Faso Complex
                //string emergencyName = GetEmergencyName();
                int isOPSEmergency = 1;
                dt = DBContext.GetData("GetOPSEmergencyId", new object[] { OPSCountryName, isOPSEmergency });
            }

            return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["LocationEmergencyId"].ToString()) : 0;
        }

        private int GetClusterId()
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(OPSClusterName) && !string.IsNullOrEmpty(OPSCountryName))
            {
                int opsEmergency = 1;
                string clusterName = "";
                if (OPSClusterName == "abrisnfi")
                {
                    clusterName = "Abris et NFI";
                }
                else
                {
                    clusterName = OPSClusterName;
                }

                dt = DBContext.GetData("GetEmergencyClustersId", new object[] { clusterName, OPSCountryName, opsEmergency });
            }

            return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["EmergencyClusterId"].ToString()) : 0;
        }

        private void PopulateDropDowns()
        {
            LocationId = 2;
            PopulateLocations(LocationId);
            PopulateStrategicObjectives();
            PopulateObjectives();
        }

        protected void ddlStrObjectives_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlSpcObjectives_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void PopulateStrategicObjectives()
        {
            ddlStrObjectives.DataValueField = "StrategicObjectiveId";
            ddlStrObjectives.DataTextField = "StrategicObjectiveName";

            DataTable dt = GetStrategicObjectives();
            ddlStrObjectives.DataSource = dt;
            ddlStrObjectives.DataBind();

            //cbStrObj.DataValueField = "StrategicObjectiveId";
            //cbStrObj.DataTextField = "StrategicObjectiveName";

            //cbStrObj.DataSource = dt;
            //cbStrObj.DataBind();

            if (ddlStrObjectives.Items.Count > 1)
            {
                ListItem item = new ListItem("Select Str Objective", "0");
                ddlStrObjectives.Items.Insert(0, item);
            }
            else
            {
                PopulateSpcObjectives();
            }
        }

        private DataTable GetStrategicObjectives()
        {
            return DBContext.GetData("GetStrategicObjectives", new object[] { OPSEmergencyClusterId });
        }

        private void PopulateSpcObjectives()
        {
            int strObjId = 0;
            int.TryParse(ddlStrObjectives.SelectedValue, out strObjId);
            PopulateObjectives();
        }

        private void PopulateObjectives()
        {
            ddlSpcObjectives.DataValueField = "StrSpcObjId";
            ddlSpcObjectives.DataTextField = "ObjectiveName";

            DataTable dt = GetClusterObjectives();
            ddlSpcObjectives.DataSource = dt;
            ddlSpcObjectives.DataBind();

            ListItem item = new ListItem("Select Specific Objective", "0");
            ddlSpcObjectives.Items.Insert(0, item);

            //cbSpcObj.DataValueField = "ClusterObjectiveId";
            //cbSpcObj.DataTextField = "ObjectiveName";

            //cbSpcObj.DataSource = dt;
            //cbSpcObj.DataBind();
        }

        private DataTable GetClusterObjectives()
        {
            //return DBContext.GetData("GetAllSpecifObjectivesOfAStrObjective", new object[] { strObjId });
            return DBContext.GetData("GetEmergencyClusterObjectivesWithSpcObjId", new object[] { OPSEmergencyClusterId });
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

        protected void BindGridData()
        {
            DataTable dt = GetActivities();
            GetReport(dt);
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
            string locationIds2 = GetSelectedLocationIds();
            string locIdsNotIncluded2 = GetNotSelectedLocationIds();

            string locationIds = GetSelectedItems(cbAdmin1Locaitons);
            string locIdsNotIncluded = GetNotSelectedItems(cbAdmin1Locaitons);

            DataTable dt = DBContext.GetData("GetOPSActivities", new object[] { OPSLocationEmergencyId, locationIds, locIdsNotIncluded, OPSProjectId, OPSEmergencyClusterId });
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

            DataTable dt = GetChildLocations(parentLocationId);
            lstLocations.DataSource = dt;
            lstLocations.DataBind();

            if (lstLocations.Items.Count > 0)
            {
                lstLocations.SelectedIndex = 0;
            }


            cbAdmin1Locaitons.DataValueField = "LocationId";
            cbAdmin1Locaitons.DataTextField = "LocationName";

            cbAdmin1Locaitons.DataSource = dt;
            cbAdmin1Locaitons.DataBind();
        }

        private DataTable GetReportLocations()
        {
            DataTable dt = DBContext.GetData("GetReportLocations", new object[] { OPSReportId });
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
                if (!(columnName == "OPSReportId" || columnName == "ClusterName" || columnName == "IndicatorName" ||
                        columnName == "StrObjName" || columnName == "SpcObjName" || columnName == "ActivityDataId" ||
                        columnName == "ActivityName" || columnName == "DataName" || columnName == "IsActive"))
                {

                    customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, column.ColumnName, "1");
                    customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, column.ColumnName, "1");
                    gvActivities.Columns.Add(customField);
                }
            }
        }

        private DataTable GetChildLocations(int parentLocationId)
        {
            DataTable dt = DBContext.GetData("GetSecondLevelChildLocations", new object[] { parentLocationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private DataTable GetOrganizations()
        {
            DataTable dt = DBContext.GetData("GetOrganizations");
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private DataTable GetCountries()
        {
            int locationType = (int)ROWCACommon.LocationTypes.Governorate;
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
            DeleteReport();
        }

        private void DeleteReport()
        {
            DBContext.Delete("DeleteOPSReport", new object[] { OPSReportId, OPSEmergencyClusterId, DBNull.Value });
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
                    DataTable dtActivities = (DataTable)Session["dtOPSActivities"];

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
                                    DBContext.Add("InsertOPSReportLocations", new object[] { OPSReportId, locationId, OPSEmergencyClusterId, DBNull.Value });
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
            OPSReportId = DBContext.Add("InsertOPSReport", new object[] { OPSLocationEmergencyId, OPSProjectId, OPSEmergencyClusterId, OPSUserId, DBNull.Value });
        }

        private bool IsDataExistsToSave()
        {
            bool returnValue = false;
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DataTable dtActivities = (DataTable)Session["dtOPSActivities"];

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

                DataTable dtActivities = (DataTable)Session["dtOPSActivities"];
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

                DataTable dtActivities = (DataTable)Session["dtOPSActivities"];
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
                    DataTable dtActivities = (DataTable)Session["dtOPSActivities"];
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
                                decimal? valToSaveMid2014 = null;
                                decimal? valToSave2014 = null;
                                int j = 0;
                                foreach (var item in dataSave)
                                {
                                    if (j == 0)
                                    {
                                        locationIdToSaveT = item.Key;
                                        valToSaveMid2014 = item.Value;
                                        j++;
                                    }
                                    else
                                    {
                                        valToSave2014 = item.Value;
                                        j = 0;
                                    }
                                }

                                dataSave.Clear();

                                if (!(valToSaveMid2014 == null && valToSave2014 == null))
                                {
                                    int newReportDetailId = DBContext.Add("InsertOPSReportDetails",
                                                                            new object[] { OPSReportId, activityDataId, locationIdToSaveT, 
                                                                                            valToSaveMid2014, valToSave2014, 1, DBNull.Value });
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
            Session["dtOPSActivities"] = dt;

            DataTable dtReport = DBContext.GetData("GetOPSReportId", new object[] { OPSLocationEmergencyId, OPSProjectId, OPSEmergencyClusterId });
            if (dtReport.Rows.Count > 0)
            {
                OPSReportId = string.IsNullOrEmpty(dtReport.Rows[0]["OPSReportId"].ToString()) ? 0 : Convert.ToInt32(dtReport.Rows[0]["OPSReportId"].ToString());
            }
            else
            {
                OPSReportId = 0;
            }

            gvActivities.DataSource = dt;
            gvActivities.DataBind();
        }

        protected void gvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["dtOPSActivities"] == null) return;


                DataTable dtActivities = (DataTable)Session["dtOPSActivities"];
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

        private void ShowMessage(string message, ROWCACommon.NotificationType notificationType = ROWCACommon.NotificationType.Success)
        {
            //updMessage.Update();
            ROWCACommon.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, true, 500);
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            ShowMessage("<b>Some Error Occoured. Admin Has Notified About It</b>.<br/> Please Try Again.", ROWCACommon.NotificationType.Error);
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "AddActivites", this.User);
        }

        #endregion

        #region Properties & Enums

        public int OPSReportId
        {
            get
            {
                int opsReportId = 0;
                if (ViewState["OPSReportId"] != null)
                {
                    int.TryParse(ViewState["OPSReportId"].ToString(), out opsReportId);
                }

                return opsReportId;
            }
            set
            {
                ViewState["OPSReportId"] = value.ToString();
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

        public int OPSUserId
        {
            get
            {
                int opsUserId = 0;
                if (ViewState["OPSUserId"] != null)
                {
                    int.TryParse(ViewState["OPSUserId"].ToString(), out opsUserId);
                }

                return opsUserId;
            }
            set
            {
                ViewState["OPSUserId"] = value.ToString();
            }
        }

        public int OPSProjectId
        {
            get
            {
                int opsProjectId = 0;
                if (ViewState["OPSProjectId"] != null)
                {
                    int.TryParse(ViewState["OPSProjectId"].ToString(), out opsProjectId);
                }

                return opsProjectId;
            }
            set
            {
                ViewState["OPSProjectId"] = value.ToString();
            }
        }

        public string OPSClusterName
        {
            get
            {
                if (ViewState["OPSClusterName"] != null)
                {
                    return ViewState["OPSClusterName"].ToString();
                }

                return "";
            }
            set
            {
                ViewState["OPSClusterName"] = value.ToString();
            }
        }

        public string OPSCountryName
        {
            get
            {
                if (ViewState["OPSCountryName"] != null)
                {
                    return ViewState["OPSCountryName"].ToString();
                }

                return null;
            }
            set
            {
                ViewState["OPSCountryName"] = value.ToString();
            }
        }

        public int OPSLocationEmergencyId
        {
            get
            {
                int opsLocationEmergencyId = 0;
                if (ViewState["OPSLocationEmergencyId"] != null)
                {
                    int.TryParse(ViewState["OPSLocationEmergencyId"].ToString(), out opsLocationEmergencyId);
                }

                return opsLocationEmergencyId;
            }
            set
            {
                ViewState["OPSLocationEmergencyId"] = value.ToString();
            }
        }

        public int OPSEmergencyClusterId
        {
            get
            {
                int opsEmgClusterId = 0;
                if (ViewState["OPSEmergencyClusterId"] != null)
                {
                    int.TryParse(ViewState["OPSEmergencyClusterId"].ToString(), out opsEmgClusterId);
                }

                return opsEmgClusterId;
            }
            set
            {
                ViewState["OPSEmergencyClusterId"] = value.ToString();
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
                    lc.Width = 40;
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
                    firstName.Width = 40;
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
            l.MaxLength = 12;
            GridViewRow row = (GridViewRow)l.NamingContainer;
            l.Text = DataBinder.Eval(row.DataItem, columnName).ToString();
        }
    }
}