﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.OPS
{
    public partial class OPSDataEntry : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string languageChange = "";
            if (Session["SiteChanged"] != null)
            {
                languageChange = Session["SiteChanged"].ToString();
            }

            if (!IsPostBack)
            {
                SetOPSIds();

                if (OPSProjectId > 0 && !string.IsNullOrEmpty(OPSClusterName) && !string.IsNullOrEmpty(OPSCountryName))
                {
                    GetEmergencyId();
                    OPSEmergencyClusterId = GetClusterId();
                    UpdateClusterName();
                }

                PopulateDropDowns();
            }

            PopulateToolTips();
            UpdateClusterName();
            lblCluster.Text = OPSClusterNameLabel;

            DataTable dtActivities = GetActivities();
            AddDynamicColumnsInGrid(dtActivities);
            GetReport(dtActivities);
        }

        private void UpdateClusterName()
        {
            string clusterName = MapClusterNames();
            DataTable dt = DBContext.GetData("GetClusterNameOnLanguage", new object[] { clusterName, RC.SelectedSiteLanguageId });
            if (dt.Rows.Count > 0)
            {
                OPSClusterNameLabel = dt.Rows[0]["ClusterName"].ToString();
            }
            else
            {
                OPSClusterNameLabel = "";
            }
        }

        #region Events.

        // Get English version of Objectives and Priorities and full drop downs.
        protected void lnkLanguageEnglish_Click(object sender, EventArgs e)
        {
            //SiteLanguageId = 1;
            //PopulateStrategicObjectives();
            //PopulatePriorities();
        }

        // Get French version of Objectives and Priorities and full drop downs.
        protected void lnkLanguageFrench_Click(object sender, EventArgs e)
        {
            //SiteLanguageId = 2;
            //PopulateStrategicObjectives();
            //PopulatePriorities();
        }

        #region Button Click Events.

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
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateData()) return;

            using (TransactionScope scope = new TransactionScope())
            {
                List<int> locationIds = GetLocationIdsFromGrid();
                if (locationIds.Count > 0)
                {
                    if (OPSReportId == 0)
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
                ShowMessage("Your Data Saved Successfuly!");
            }
        }

        //private bool DataIsValid()
        //{
        //    int activityDataId = 0;
        //    foreach (GridViewRow row in gvActivities.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            //activityDataId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());
        //            DataTable dtActivities = (DataTable)Session["dtOPSActivities"];

        //            bool isAdded = false;
        //            foreach (DataColumn dc in dtActivities.Columns)
        //            {
        //                string colName = dc.ColumnName;
        //                int locationId = 0;

        //                CheckBox cbAccum = row.FindControl(colName) as CheckBox;
        //                if (cbAccum != null && cbAccum.Checked)
        //                {
        //                    isAdded = true;
        //                }

        //                if (isAdded)
        //                {
        //                    HiddenField hf = row.FindControl("hf" + colName) as HiddenField;
        //                    if (hf != null)
        //                    {
        //                        locationId = Convert.ToInt32(hf.Value);
        //                    }

        //                    decimal? fullYearTarget = null;
        //                    TextBox t = row.FindControl(colName) as TextBox;
        //                    if (t != null)
        //                    {
        //                        if (!string.IsNullOrEmpty(t.Text))
        //                        {
        //                            fullYearTarget = Convert.ToDecimal(t.Text, CultureInfo.InvariantCulture);
        //                        }
        //                    }

        //                    if (locationId > 0)
        //                    {
        //                        decimal? midYearTarget = null;
        //                        fullYearTarget = fullYearTarget == null ? 0 : fullYearTarget;
        //                        DBContext.Add("InsertOPSReportDetails", new object[] { OPSReportId, activityDataId, locationId,
        //                                                                                    midYearTarget, fullYearTarget, 1, DBNull.Value });
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return true;
        //}

        private List<int> GetLocationIdsFromGrid()
        {
            List<int> locationIds = new List<int>();
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {

                    DataTable dtActivities = (DataTable)Session["dtOPSActivities"];
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

        #endregion

        #endregion

        #region Methods.

        // Get values from querystring and set variables.
        private void SetOPSIds()
        {
            int tempVal = 0;
            // UserId
            if (Request.QueryString["uid"] != null)
            {
                int.TryParse(Request.QueryString["uid"].ToString(), out tempVal);
                OPSUserId = tempVal;
            }

            // ProjectId
            if (Request.QueryString["pid"] != null)
            {
                tempVal = 0;
                int.TryParse(Request.QueryString["pid"].ToString(), out tempVal);
                OPSProjectId = tempVal;
            }

            // Cluster Name
            if (Request.QueryString["clname"] != null)
            {
                OPSClusterName = Request.QueryString["clname"].ToString();
            }

            // Country Name
            if (Request.QueryString["cname"] != null)
            {
                string cName = Request.QueryString["cname"].ToString();
                if (cName == "burkinafaso" || cName == "Burkinafaso" || cName == "BURKINAFASO")
                {
                    OPSCountryName = "burkina faso";
                }
                else if (cName == "region" || cName == "Region" || cName == "REGION")
                {
                    OPSCountryName = "Sahel Region";
                }
                else
                {
                    OPSCountryName = cName;
                }
            }
        }

        private void GetEmergencyId()
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(OPSCountryName))
            {
                dt = DBContext.GetData("GetOPSEmergencyId", new object[] { OPSCountryName, RC.EmergencySahel2015, RC.SelectedSiteLanguageId });
            }

            if (dt.Rows.Count > 0)
            {
                OPSLocationEmergencyId = Convert.ToInt32(dt.Rows[0]["EmergencyLocationId"].ToString());
                OPSEmergencyId = Convert.ToInt32(dt.Rows[0]["EmergencyId"].ToString());
            }
        }

        private int GetClusterId()
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(OPSClusterName) && !string.IsNullOrEmpty(OPSCountryName))
            {
                string clusterName = MapClusterNames();
                OPSClusterNameLabel = clusterName;

                dt = DBContext.GetData("GetEmergencyClustersId", new object[] { clusterName, OPSEmergencyId, RC.SelectedSiteLanguageId });
            }

            return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["EmergencyClusterId"].ToString()) : 0;
        }

        private string MapClusterNames()
        {
            string clusterName = "";
            if (OPSClusterName == "coordinationandsupportservices")
            {
                clusterName = RC.SelectedSiteLanguageId == 2 ? "Coodination et Services de Soutien" : "Coordination And Support Services";
            }
            else if (OPSClusterName == "earlyrecovery")
            {
                clusterName = RC.SelectedSiteLanguageId == 2 ? "Relèvement Précoce" : "Early Recovery";
            }
            else if (OPSClusterName == "education")
            {
                clusterName = "Education";
            }
            else if (OPSClusterName == "emergencyshelterandnfi")
            {
                clusterName = RC.SelectedSiteLanguageId == 2 ? "Abris D'urgence et Biens Non-Alimentaires" : "Emergency Shelter And NFI";
            }
            else if (OPSClusterName == "foodsecurity")
            {
                clusterName = RC.SelectedSiteLanguageId == 2 ? "Sécurité Alimentaire" : "Food Security";
            }
            else if (OPSClusterName == "health")
            {
                clusterName = RC.SelectedSiteLanguageId == 2 ? "Santé" : "Health";
            }
            else if (OPSClusterName == "logistics")
            {
                clusterName = "Logistics";
            }
            else if (OPSClusterName == "nutrition")
            {
                clusterName = "Nutrition";
            }
            else if (OPSClusterName == "protection")
            {
                clusterName = "Protection";
            }
            else if (OPSClusterName == "logistics")
            {
                clusterName = RC.SelectedSiteLanguageId == 2 ? "Logistique" : "Logistics";
            }
            else if (OPSClusterName == "waterandsanitation")
            {
                clusterName = RC.SelectedSiteLanguageId == 2 ? "Eau-Hygiene-Assainissement" : "Water Sanitation & Hygiene";
            }
            else if (OPSClusterName == "emergencytelecommunication")
            {
                clusterName = RC.SelectedSiteLanguageId == 2 ? "Télécommunication d'urgence" : "Emergency Telecommunication";
            }
            else if (OPSClusterName == "multisectorforrefugees")
            {
                clusterName = RC.SelectedSiteLanguageId == 2 ? "Assistance Multi Sectorielles aux Refugies" : "Multi Sector for Refugees";
            }
            else
            {
                clusterName = OPSClusterName;
            }

            return clusterName;
        }

        private void PopulateDropDowns()
        {
            LocationId = GetLocationId();
            PopulateLocations(LocationId);
            UI.FillObjectives(cblObjectives, true, RC.EmergencySahel2015);
        }

        private int GetLocationId()
        {
            DataTable dt = DBContext.GetData("GetLocationIdOnName", new object[] { OPSCountryName });
            return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["LocationId"]) : 0;
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
            GetReport(dt);

            PopulateToolTips();
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
            string locationIds = GetSelectedItems(cbAdmin1Locaitons);
            string locIdsNotIncluded = GetNotSelectedItems(cbAdmin1Locaitons);
            int yearId = 11; //2015
            DataTable dt = DBContext.GetData("GetOPSActivities2", new object[] { OPSLocationEmergencyId, locationIds, locIdsNotIncluded, 
                                                                            OPSProjectId, OPSEmergencyClusterId, RC.SelectedSiteLanguageId, OPSCountryName, yearId });
            
            if (dt.Rows.Count <= 0 && !string.IsNullOrEmpty(locationIds))
            {
                if (RC.SelectedSiteLanguageId == 1)
                {
                    ShowMessage("The framework for your cluster (i.e. activties and indicators) has not been uploaded yet.<br/> Please contact your Cluster (Sector) coordinator for more information.<br/>Please click on this message to go back to the main window.", RC.NotificationType.Error, false, 500);
                }
                else
                {
                    ShowMessage("le cadre de travail sectoriel (les activités et les indicateurs) ne sont pas encore enregistrés.<br/> Merci de contacter votre coordinateur de cluster (secteur) pour plus de renseignements.<br/>Veuillez cliquer sur ce message pour retourner à l apage principale.", RC.NotificationType.Error, false, 500);
                }
            }

            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void AddLocationsInSelectedList()
        {
            PopulateLocations(LocationId);
        }

        private void PopulateLocations(int parentLocationId)
        {
            DataTable dt = GetChildLocations(parentLocationId);

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
                if (!(columnName == "OPSReportId" || columnName == "ClusterName" || columnName == "SecondaryCluster" ||
                        columnName == "Objective" || columnName == "HumanitarianPriority" || columnName == "ActivityDataId" ||
                        columnName == "ActivityName" || columnName == "DataName" || columnName == "IsActive" ||
                        columnName == "ObjectiveId" || columnName == "HumanitarianPriorityId" || columnName == "ObjAndPrId" ||
                        columnName == "PriorityActivityId" || columnName == "RInd" || columnName == "CInd" || columnName == "Unit"))
                {
                    if (columnName == "IsAdded")
                    {

                        customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, column.ColumnName, "1", "CheckBox");
                        customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, "Selected", "1", "CheckBox");
                        gvActivities.Columns.Add(customField);
                    }
                    else
                    {
                        customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, column.ColumnName, "1", "TextBox");
                        customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, column.ColumnName, "1", "TextBox");
                        gvActivities.Columns.Add(customField);
                    }
                }
            }
        }

        private DataTable GetChildLocations(int parentLocationId)
        {
            DataTable dt = new DataTable();
            //if (OPSCountryName == "Sahel Region")
            //{
            //    dt = DBContext.GetData("GetSahelRegionalLocationsForOPS");
            //}
            //else
            {
                dt = DBContext.GetData("GetSecondLevelChildLocationsAndCountry", new object[] { parentLocationId });
            }
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
            DeleteReport();
        }

        private void DeleteReport()
        {
            DBContext.Delete("DeleteOPSReport", new object[] { OPSReportId, OPSEmergencyClusterId, DBNull.Value });
        }

        private void DeleteOPSReportDetails()
        {
            DBContext.Delete("DeleteOPSReportDetails", new object[] { OPSReportId, OPSEmergencyClusterId, DBNull.Value });
        }

        private void SaveReport()
        {
            SaveReportLocations();
            DeleteOPSReportDetails();
            SaveReportDetails();
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

            DBContext.Add("InsertOPSReportLocations", new object[] { OPSReportId, locIds, OPSEmergencyClusterId, DBNull.Value });
        }

        private void SaveReportMainInfo()
        {
            int yearId = 11; //2015
            OPSReportId = DBContext.Add("InsertOPSReport", new object[] { OPSLocationEmergencyId, OPSProjectId, OPSEmergencyClusterId, OPSUserId, RC.SelectedSiteLanguageId, yearId, DBNull.Value });
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

        private bool ValidateData()
        {
            int activityDataId = 0;
            int? userId = OPSUserId > 0 ? OPSUserId : (int?)null;
            bool isValid = true;
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    activityDataId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());
                    DataTable dtActivities = (DataTable)Session["dtOPSActivities"];

                    bool isAdded = false;
                    foreach (DataColumn dc in dtActivities.Columns)
                    {
                        string colName = dc.ColumnName;
                        int locationId = 0;

                        CheckBox cbAccum = row.FindControl(colName) as CheckBox;
                        if (cbAccum != null && cbAccum.Checked)
                        {
                            isAdded = true;
                        }

                        if (isAdded)
                        {
                            isValid = false;
                            HiddenField hf = row.FindControl("hf" + colName) as HiddenField;
                            if (hf != null)
                            {
                                locationId = Convert.ToInt32(hf.Value);
                            }

                            decimal? fullYearTarget = null;
                            TextBox t = row.FindControl(colName) as TextBox;
                            if (t != null)
                            {
                                if (!string.IsNullOrEmpty(t.Text))
                                {
                                    fullYearTarget = Convert.ToDecimal(t.Text, CultureInfo.InvariantCulture);
                                }
                            }

                            if (locationId > 0 && fullYearTarget != null)
                            {
                                isValid = true;
                                break;
                            }
                        }
                    }

                    if (!isValid)
                    {
                        if (RC.SelectedSiteLanguageId == 1)
                        {
                            ShowMessage("Sorry, we are unable to save your selection.<br/> Please provide a target for each activity indicator.<br/> If you do not know the target at this time please put a 0 (Zero).<br/>Please click on this message to go back to the main window.", RC.NotificationType.Error, false, 500);
                        }
                        else
                        {
                            ShowMessage("Désolé, nous ne pouvons enregistrer votre sélection.<br/> Veuillez fournir une cible pour chaque indicateur par activité.<br/> Si vous ne connaissez pas encore la cible, veuillez metre un 0 (Zero).<br/>Veuillez cliquer sur ce message pour retourner à l apage principale.", RC.NotificationType.Error, false, 500);
                        }
                    }
                }
            }

            return isValid;
        }

        private void SaveReportDetails()
        {
            int activityDataId = 0;
            int? userId = OPSUserId > 0 ? OPSUserId : (int?)null;

            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    activityDataId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());
                    DataTable dtActivities = (DataTable)Session["dtOPSActivities"];

                    bool isAdded = false;
                    foreach (DataColumn dc in dtActivities.Columns)
                    {
                        string colName = dc.ColumnName;
                        int locationId = 0;

                        CheckBox cbAccum = row.FindControl(colName) as CheckBox;
                        if (cbAccum != null && cbAccum.Checked)
                        {
                            isAdded = true;
                        }

                        if (isAdded)
                        {
                            HiddenField hf = row.FindControl("hf" + colName) as HiddenField;
                            if (hf != null)
                            {
                                locationId = Convert.ToInt32(hf.Value);
                            }

                            decimal? fullYearTarget = null;
                            TextBox t = row.FindControl(colName) as TextBox;
                            if (t != null)
                            {
                                if (!string.IsNullOrEmpty(t.Text))
                                {
                                    fullYearTarget = Convert.ToDecimal(t.Text, CultureInfo.InvariantCulture);
                                }
                            }

                            if (locationId > 0 && fullYearTarget != null)
                            {
                                decimal? midYearTarget = null;
                                //fullYearTarget = fullYearTarget == null ? 0 : fullYearTarget;
                                DBContext.Add("InsertOPSReportDetails", new object[] { OPSReportId, activityDataId, locationId,
                                                                                            midYearTarget, fullYearTarget, userId, DBNull.Value });
                            }
                        }
                    }
                }
            }
        }

        private void GetReport(DataTable dt)
        {
            Session["dtOPSActivities"] = dt;

            int yearId = 11; //2015
            DataTable dtReport = DBContext.GetData("GetOPSReportId", new object[] { OPSLocationEmergencyId, OPSProjectId, OPSEmergencyClusterId, yearId });
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
                        if (txt.Text == "-1")
                        {
                            txt.Text = "";
                        }
                    }
                }

                ObjPrToolTip.ObjectiveLableToolTip(e, 0);
                //ObjPrToolTip.PrioritiesIconToolTip(e, 1);
                //ObjPrToolTip.RegionalIndicatorIcon(e, 5);
                //ObjPrToolTip.CountryIndicatorIcon(e, 6);
            }
        }

        private void PopulateToolTips()
        {
            ObjPrToolTip.ObjectivesToolTip(cblObjectives);
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            //updMessage.Update();
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
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

        public string OPSClusterNameLabel
        {
            get
            {
                if (ViewState["OPSClusterNameLabel"] != null)
                {
                    return ViewState["OPSClusterNameLabel"].ToString();
                }

                return "";
            }
            set
            {
                ViewState["OPSClusterNameLabel"] = value.ToString();
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

        public int OPSEmergencyId
        {
            get
            {
                int opsEmergencyId = 0;
                if (ViewState["OPSEmergencyId"] != null)
                {
                    int.TryParse(ViewState["OPSEmergencyId"].ToString(), out opsEmergencyId);
                }

                return opsEmergencyId;
            }
            set
            {
                ViewState["OPSEmergencyId"] = value.ToString();
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

        //public int SiteLanguageId
        //{
        //    get
        //    {
        //        int langId = 0;
        //        if (Session["SiteLanguageId"] != null)
        //        {
        //            int.TryParse(Session["SiteLanguageId"].ToString(), out langId);
        //        }

        //        return langId;
        //    }
        //    set
        //    {
        //        Session["SiteLanguageId"] = value.ToString();
        //    }
        //}

        public int Reload
        {
            get
            {
                int reloadId = 0;
                if (ViewState["Reload"] != null)
                {
                    int.TryParse(ViewState["Reload"].ToString(), out reloadId);
                }

                return reloadId;
            }
            set
            {
                ViewState["Reload"] = value.ToString();
            }
        }

        #endregion
    }

    public class GridViewTemplate : ITemplate
    {
        private DataControlRowType templateType;
        private string columnName;
        private string locationId;
        private string controlType;

        public GridViewTemplate(DataControlRowType type, string colname, string locId, string cntrlType)
        {
            templateType = type;
            columnName = colname;
            locationId = locId;
            controlType = cntrlType;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            if (templateType == DataControlRowType.Header)
            {
                if (columnName == "Selected")
                {
                    Label lc = new Label { Width = 50, Text = "<b>Selected</b>" };
                    container.Controls.Add(lc);
                }
                else
                {
                    string[] words = columnName.Split('^');
                    Label lc = new Label { Width = 50, Text = "<b>" + words[1] + "</b>" };
                    container.Controls.Add(lc);
                }
            }
            else if (templateType == DataControlRowType.DataRow)
            {
                if (controlType == "TextBox")
                {
                    TextBox txtTA = new TextBox();
                    txtTA.CssClass = "numeric1";
                    txtTA.Width = 43;
                    txtTA.Style["font-size"] = 10 + "px";
                    txtTA.DataBinding += new EventHandler(this.FirstName_DataBinding);
                    container.Controls.Add(txtTA);
                    HiddenField hf = new HiddenField();
                    string[] words1 = columnName.Split('^');
                    hf.Value = words1[0];
                    hf.ID = "hf" + columnName;
                    container.Controls.Add(hf);
                }
                else if (controlType == "CheckBox")
                {
                    CheckBox cbLocAccum = new CheckBox();
                    cbLocAccum.DataBinding += cbAccum_DataBinding;
                    container.Controls.Add(cbLocAccum);
                }
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

        private void cbAccum_DataBinding(Object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            cb.ID = columnName;
            GridViewRow row = (GridViewRow)cb.NamingContainer;
            cb.Checked = (DataBinder.Eval(row.DataItem, columnName)).ToString() == "True";
        }
    }
}