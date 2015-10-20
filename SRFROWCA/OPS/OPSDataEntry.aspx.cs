﻿using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                UpdateClusterName();
                lblCluster.Text = OPSClusterNameLabel;

                GetReport();
                PopulateIndicators();
            }
        }

        private void PopulateIndicators()
        {
            DataTable dtActivities = GetActivities();
            gvActivities.DataSource = dtActivities;
            gvActivities.DataBind();
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

        #region Button Click Events.

        private bool TargetProvided(Repeater rpt, bool isGender)
        {
            bool isTargetValid = false;
            foreach (RepeaterItem item in rpt.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater rptAdmin1 = (Repeater)item.FindControl("rptAdmin1");
                    foreach (RepeaterItem admin1Item in rptAdmin1.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            Repeater rptAdmin2 = (Repeater)admin1Item.FindControl("rptAdmin2");
                            foreach (RepeaterItem admin2Item in rptAdmin2.Items)
                            {
                                if (isGender)
                                {
                                    TextBox txtTargetMale = admin2Item.FindControl("txtAdmin2TargetMaleProject") as TextBox;
                                    TextBox txtTargetFemale = admin2Item.FindControl("txtAdmin2TargetFemaleProject") as TextBox;
                                    if (!string.IsNullOrEmpty(txtTargetMale.Text.Trim()) || !string.IsNullOrEmpty(txtTargetFemale.Text.Trim()))
                                    {
                                        isTargetValid = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    TextBox txtTarget = admin2Item.FindControl("txtAdmin2TargetProject") as TextBox;
                                    if (!string.IsNullOrEmpty(txtTarget.Text.Trim()))
                                    {
                                        isTargetValid = true;
                                        break;
                                    }
                                }
                            }

                            if (isTargetValid) break;
                        }
                    }

                    if (isTargetValid) break;
                }
            }

            return isTargetValid;
        }

        private bool IsTargetProvided()
        {
            bool isTargetValid = false;
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int unitId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["UnitId"].ToString());

                    Repeater rptCountry = row.FindControl("rptCountryGender") as Repeater;
                    if (rptCountry != null)
                    {
                        if (unitId == 269 || unitId == 28 || unitId == 38 || unitId == 193 || unitId == 219 
                            || unitId == 198 || unitId == 311 || unitId == 132 || unitId == 252 || unitId == 238)
                           isTargetValid = TargetProvided(rptCountry, true);
                        else
                            isTargetValid =  TargetProvided(rptCountry, false);
                    }
                    if (isTargetValid) break;
                }
            }

            if (!isTargetValid)
            {
                if (RC.SelectedSiteLanguageId == 1)
                {
                    ShowMessage("Please provide target for at least one location.", RC.NotificationType.Error, true, 3000);
                }
                else
                {

                    ShowMessage("Se il vous plaît fournir cible pour au moins un emplacement.", RC.NotificationType.Error, true, 3000);
                }
            }

            return isTargetValid;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsTargetProvided())
            {
                bool isSaved = false;
                using (TransactionScope scope = new TransactionScope())
                {
                    if (OPSReportId == 0)
                    {
                        SaveReportMainInfo();
                    }
                    else
                    {
                        UpdateReportMainInfo();
                    }

                    SaveReport();
                    //    DeleteReport();
                    scope.Complete();
                    isSaved = true;
                    if (RC.SelectedSiteLanguageId == 1)
                        ShowMessage("Your Data Saved Successfully!");
                    else
                        ShowMessage("Vos données sauvegardées avec succès");
                }

                if (isSaved)
                    PopulateIndicators();
            }
        }

        private void SaveReportDetails()
        {
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int indicatorId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["IndicatorId"].ToString());
                    int unitId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["UnitId"].ToString());
                    Repeater rptCountry = row.FindControl("rptCountryGender") as Repeater;
                    if (rptCountry != null)
                    {
                        CountryRepeater(rptCountry, indicatorId, unitId);
                    }
                }
            }
        }

        private void CountryRepeater(Repeater rptCountry, int indicatorId, int unitId)
        {
            foreach (RepeaterItem item in rptCountry.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfCountryId = item.FindControl("hfCountryId") as HiddenField;
                    int countryId = 0;
                    if (hfCountryId != null)
                        int.TryParse(hfCountryId.Value, out countryId);

                    Repeater rptAdmin1 = item.FindControl("rptAdmin1") as Repeater;
                    if (rptAdmin1 != null)
                    {
                        Admin1Repeater(rptAdmin1, indicatorId, unitId, countryId);
                    }
                }
            }
        }

        private void Admin1Repeater(Repeater rptAdmin1, int indicatorId, int unitId, int countryId)
        {
            foreach (RepeaterItem item in rptAdmin1.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfAdmin1Id = item.FindControl("hfAdmin1Id") as HiddenField;
                    int admin1Id = 0;
                    if (hfAdmin1Id != null)
                        int.TryParse(hfAdmin1Id.Value, out admin1Id);

                    Repeater rptAdmin2 = item.FindControl("rptAdmin2") as Repeater;
                    if (rptAdmin2 != null)
                    {
                        if (unitId == 269 || unitId == 28 || unitId == 38 || unitId == 193
                                || unitId == 219 || unitId == 198 || unitId == 311 || unitId == 287
                                || unitId == 67 || unitId == 132 || unitId == 252)
                            SaveAdmin2GenderTargets(rptAdmin2, indicatorId, countryId, admin1Id);
                        else
                            SaveAdmin2Targets(rptAdmin2, indicatorId, countryId, admin1Id);


                    }
                }
            }
        }

        private void SaveAdmin2Targets(Repeater rptAdmin2, int indicatorId, int countryId, int admin1Id)
        {
            int? userId = OPSUserId > 0 ? OPSUserId : (int?)null;
            foreach (RepeaterItem item in rptAdmin2.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfAdmin2Id = item.FindControl("hfAdmin2Id") as HiddenField;
                    int admin2Id = 0;
                    if (hfAdmin2Id != null)
                        int.TryParse(hfAdmin2Id.Value, out admin2Id);

                    int? target = null;
                    TextBox txtTarget = item.FindControl("txtAdmin2TargetProject") as TextBox;
                    target = string.IsNullOrEmpty(txtTarget.Text.Trim()) ? (int?)null : Convert.ToInt32(txtTarget.Text.Trim());
                    if (admin2Id > 0)
                    {
                        DBContext.Update("InsertOPSReportDetails", new object[] { OPSReportId, indicatorId, countryId, admin1Id , admin2Id, 
                                                                                        target, userId, DBNull.Value });
                    }
                }
            }
        }

        private void SaveAdmin2GenderTargets(Repeater rptAdmin2, int indicatorId, int countryId, int admin1Id)
        {
            int? userId = OPSUserId > 0 ? OPSUserId : (int?)null;
            foreach (RepeaterItem item in rptAdmin2.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtAdmin2Male = item.FindControl("txtAdmin2TargetMaleProject") as TextBox;
                    TextBox txtAdmin2Female = item.FindControl("txtAdmin2TargetFemaleProject") as TextBox;

                    int? maleTarget = null;
                    if (txtAdmin2Male != null)
                        maleTarget = string.IsNullOrEmpty(txtAdmin2Male.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin2Male.Text.Trim());

                    int? femaleTarget = null;
                    if (txtAdmin2Female != null)
                        femaleTarget = string.IsNullOrEmpty(txtAdmin2Female.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin2Female.Text.Trim());

                    HiddenField hfAdmin2Id = item.FindControl("hfAdmin2Id") as HiddenField;
                    int admin2Id = 0;
                    if (hfAdmin2Id != null)
                        int.TryParse(hfAdmin2Id.Value, out admin2Id);
                    if (admin2Id > 0)
                    {
                        DBContext.Update("InsertOPSReportDetailsGender", new object[] {OPSReportId, indicatorId, countryId, admin1Id , 
                                                                                        admin2Id, maleTarget, femaleTarget,
                                                                                         userId, DBNull.Value });
                    }
                }
            }
        }

        private void UpdateReportMainInfo()
        {
            DBContext.Update("UpdateOPSReport", new object[] { OPSProjectId, OPSLocationEmergencyId, OPSEmergencyClusterId, OPSUserId, DBNull.Value });
        }

        private List<int> GetLocationIdsFromGrid()
        {
            List<int> locationIds = new List<int>();
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {

                    //DataTable dtActivities = (DataTable)Session["dtOPSActivities"];
                    //foreach (DataColumn dc in dtActivities.Columns)
                    //{
                    //    string colName = dc.ColumnName;

                    //    HiddenField hf = row.FindControl("hf" + colName) as HiddenField;
                    //    if (hf != null)
                    //    {
                    //        int locationId = 0;
                    //        int.TryParse(hf.Value, out locationId);
                    //        if (locationId > 0)
                    //            locationIds.Add((locationId));
                    //    }
                    //}

                    // If data row then iterate only once bece we need column names
                    // from grid to get ids from hidden fields.
                    break;
                }
            }

            return locationIds.Distinct().ToList();
        }

        //private void SelectLocationsOfGrid(List<int> locationIds)
        //{
        //    foreach (ListItem item in cbAdmin1Locaitons.Items)
        //    {
        //        item.Selected = locationIds.Contains(Convert.ToInt32(item.Value));
        //    }
        //}

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

            if (Request.QueryString["clname2"] != null)
            {
                if (Request.QueryString["clname2"].ToString() == " c" ||
                    Request.QueryString["clname2"].ToString() == "")
                {
                    if (Request.QueryString["clname"] != null)
                    {
                        OPSClusterName = Request.QueryString["clname"].ToString();
                    }
                }
                else
                {
                    OPSClusterName = Request.QueryString["clname2"].ToString();
                }

            }
            else
            {
                if (Request.QueryString["clname"] != null)
                {
                    OPSClusterName = Request.QueryString["clname"].ToString();
                }
            }

            // Country Name
            if (Request.QueryString["cname"] != null)
            {
                string cName = Request.QueryString["cname"].ToString();
                if (cName == "burkinafaso" || cName == "Burkinafaso" || cName == "BURKINAFASO")
                {
                    OPSCountryName = "burkina faso";
                }
                else if (cName == "region" || cName == "Region" || cName == "REGION" || cName == "sahelregion")
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
                clusterName = RC.SelectedSiteLanguageId == 2 ? "Coordination et Services de Soutien" : "Coordination And Support Services";
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
            else if (OPSClusterName == "sheltercccm")
            {
                clusterName = RC.SelectedSiteLanguageId == 2 ? "Coordination et gestion des sites" : "Camp Coordination And Camp Management";
            }
            else if (OPSClusterName == "cccm")
            {
                clusterName = RC.SelectedSiteLanguageId == 2 ? "Coordination et gestion des sites" : "Camp Coordination And Camp Management";
            }
            else if (OPSClusterName == "campcoordinationandcampmanagement")
            {
                clusterName = RC.SelectedSiteLanguageId == 2 ? "Coordination et gestion des sites" : "Camp Coordination And Camp Management";
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
            //UI.FillObjectives(cblObjectives, true, RC.EmergencySahel2015);
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
            PopulateIndicators();
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
            int yearId = 12; //2015
            DataTable dt = DBContext.GetData("GetOPSActivities3", new object[] { OPSLocationEmergencyId, OPSEmergencyClusterId, OPSProjectId, RC.SelectedSiteLanguageId, yearId });

            if (dt.Rows.Count <= 0)
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
            //SaveReportLocations();
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
            int yearId = 12; //2016
            OPSReportId = DBContext.Add("InsertOPSReport", new object[] { OPSLocationEmergencyId, OPSProjectId, 
                                                                            OPSEmergencyClusterId, OPSUserId, 
                                                                            RC.SelectedSiteLanguageId, yearId, DBNull.Value });
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

        //protected void CaptureDataFromGrid()
        //{
        //    string activityDataId = "";
        //    foreach (GridViewRow row in gvActivities.Rows)
        //    {
        //        if (row.RowType != DataControlRowType.DataRow) return;

        //        activityDataId = gvActivities.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString();
        //        int colummCounts = gvActivities.Columns.Count;

        //        DataTable dtActivities = (DataTable)Session["dtOPSActivities"];
        //        if (dtActivities == null) return;

        //        DataTable dtClone;
        //        if (Session["dtClone"] != null)
        //        {
        //            dtClone = (DataTable)Session["dtClone"];
        //        }
        //        else
        //        {
        //            dtClone = dtActivities.Copy();
        //            foreach (DataRow dr in dtClone.Rows)
        //            {
        //                foreach (DataColumn dc in dtClone.Columns)
        //                {
        //                    if (dc.DataType == typeof(string))
        //                    {
        //                        dr[dc] = "";
        //                    }
        //                }
        //            }
        //        }

        //        foreach (DataColumn dc in dtClone.Columns)
        //        {
        //            string colName = dc.ColumnName;
        //            TextBox t = row.FindControl(colName) as TextBox;
        //            if (t != null)
        //            {
        //                dtClone.Rows[row.RowIndex][colName] = t.Text;
        //                dtClone.Rows[row.RowIndex]["ActivityDataId"] = activityDataId;
        //            }
        //        }

        //        Session["dtClone"] = dtClone;
        //    }
        //}

        //private bool ValidateData()
        //{
        //    int activityDataId = 0;
        //    int? userId = OPSUserId > 0 ? OPSUserId : (int?)null;
        //    bool isValid = true;
        //    foreach (GridViewRow row in gvActivities.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            activityDataId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());
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
        //                    isValid = false;
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

        //                    if (locationId > 0 && fullYearTarget != null)
        //                    {
        //                        isValid = true;
        //                        break;
        //                    }
        //                }
        //            }

        //            if (!isValid)
        //            {
        //                if (RC.SelectedSiteLanguageId == 1)
        //                {
        //                    ShowMessage("Sorry, we are unable to save your selection.<br/> Please provide a target for each activity indicator.<br/> If you do not know the target at this time please put a 0 (Zero).<br/>Please click on this message to go back to the main window.", RC.NotificationType.Error, false, 500);
        //                }
        //                else
        //                {
        //                    ShowMessage("Désolé, nous ne pouvons enregistrer votre sélection.<br/> Veuillez fournir une cible pour chaque indicateur par activité.<br/> Si vous ne connaissez pas encore la cible, veuillez metre un 0 (Zero).<br/>Veuillez cliquer sur ce message pour retourner à l apage principale.", RC.NotificationType.Error, false, 500);
        //                }
        //            }
        //        }
        //    }

        //    return isValid;
        //}

        //private void SaveReportDetails()
        //{
        //    int activityDataId = 0;
        //    int? userId = OPSUserId > 0 ? OPSUserId : (int?)null;

        //    foreach (GridViewRow row in gvActivities.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            activityDataId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());
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

        //                //if (isAdded)
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

        //                    if (locationId > 0 && fullYearTarget != null)
        //                    {
        //                        decimal? midYearTarget = null;
        //                        //fullYearTarget = fullYearTarget == null ? 0 : fullYearTarget;
        //                        DBContext.Add("InsertOPSReportDetails", new object[] { OPSReportId, activityDataId, locationId,
        //                                                                                    midYearTarget, fullYearTarget, userId, DBNull.Value });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        private void GetReport()
        {
            int yearId = 12; //2015
            DataTable dtReport = DBContext.GetData("GetOPSReportId", new object[] { OPSProjectId, yearId });
            if (dtReport.Rows.Count > 0)
            {
                OPSReportId = string.IsNullOrEmpty(dtReport.Rows[0]["OPSReportId"].ToString()) ? 0 : Convert.ToInt32(dtReport.Rows[0]["OPSReportId"].ToString());
            }
            else
            {
                OPSReportId = 0;
            }
        }

        protected void gvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hfIndicatorId = e.Row.FindControl("hfIndicatorId") as HiddenField;
                int indicatorId = 0;
                if (hfIndicatorId != null)
                {
                    int.TryParse(hfIndicatorId.Value, out indicatorId);
                }
                if (indicatorId > 0)
                {
                    Repeater rptCountry = e.Row.FindControl("rptCountryGender") as Repeater;
                    if (rptCountry != null)
                    {
                        int yearId = 12;
                        DataTable dt = DBContext.GetData("GetOPSCountryTargetOfIndicator",
                                                                    new object[] { OPSLocationEmergencyId, OPSEmergencyClusterId, 
                                                                               OPSProjectId, yearId, indicatorId});
                        rptCountry.DataSource = dt;
                        rptCountry.DataBind();

                        if (dt.Rows.Count == 0)
                        {
                            Label lblNoTarget = e.Row.FindControl("lblNoTarget") as Label;
                            if (lblNoTarget != null)
                                lblNoTarget.Visible = true;
                        }
                    }
                }
                ObjPrToolTip.ObjectiveIconToolTip(e, 0);
                ObjPrToolTip.ObjectiveLableToolTip(e, 0);
            }
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            //ShowMessage("<b>Some Error Occoured. Admin Has Notified About It</b>.<br/> Please Try Again.", RC.NotificationType.Error);
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
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

        private int LocationId
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

        private int OPSLocationEmergencyId
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



        protected void rptCountryGender_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UI.SetThousandSeparator(e.Item, "lblCountryTargetMaleCluster");
                UI.SetThousandSeparator(e.Item, "lblCountryTargetFemaleCluster");
                UI.SetThousandSeparator(e.Item, "lblCountryTargetCluster");
                UI.SetThousandSeparator(e.Item, "lblCountryTargetMaleProject");
                UI.SetThousandSeparator(e.Item, "lblCountryTargetFemaleProject");
                UI.SetThousandSeparator(e.Item, "lblCountryTargetProject");

                HiddenField hfIndicatorId = e.Item.FindControl("hfCountryIndicatorId") as HiddenField;
                int indicatorId = 0;
                if (hfIndicatorId != null)
                {
                    int.TryParse(hfIndicatorId.Value, out indicatorId);
                }
                if (indicatorId > 0)
                {
                    HiddenField hfCountryId = e.Item.FindControl("hfCountryId") as HiddenField;
                    int countryId = 0;
                    if (hfCountryId != null)
                    {
                        int.TryParse(hfCountryId.Value, out countryId);
                    }

                    Repeater rptAdmin1 = e.Item.FindControl("rptAdmin1") as Repeater;
                    if (rptAdmin1 != null)
                    {
                        int yearId = 12;
                        rptAdmin1.DataSource = DBContext.GetData("[GetOPSAdmin1TargetOfIndicator]", new object[] { countryId, OPSLocationEmergencyId, 
                                                                                                                OPSEmergencyClusterId, OPSProjectId, 
                                                                                                                yearId, indicatorId });
                        rptAdmin1.DataBind();
                    }
                }
            }
        }

        protected void rptAdmin1Gender_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UI.SetThousandSeparator(e.Item, "lblAdmin1TargetMaleCluster");
                UI.SetThousandSeparator(e.Item, "lblAdmin1TargetFemaleCluster");
                UI.SetThousandSeparator(e.Item, "lblAdmin1TargetCluster");
                UI.SetThousandSeparator(e.Item, "lblAdmin1TargetMaleProject");
                UI.SetThousandSeparator(e.Item, "lblAdmin1TargetFemaleProject");
                UI.SetThousandSeparator(e.Item, "lblAdmin1TargetProject");

                HiddenField hfIndicatorId = e.Item.FindControl("hfAdmin1IndicatorId") as HiddenField;
                int indicatorId = 0;
                if (hfIndicatorId != null)
                {
                    int.TryParse(hfIndicatorId.Value, out indicatorId);
                }
                if (indicatorId > 0)
                {
                    HiddenField hfAdmin1Id = e.Item.FindControl("hfAdmin1Id") as HiddenField;
                    int admin1Id = 0;
                    if (hfAdmin1Id != null)
                    {
                        int.TryParse(hfAdmin1Id.Value, out admin1Id);
                    }

                    Repeater rptAdmin2 = e.Item.FindControl("rptAdmin2") as Repeater;
                    if (rptAdmin2 != null)
                    {
                        int yearId = 12;
                        DataTable dt = DBContext.GetData("[GetOPSAdmin2TargetOfIndicator]", new object[] {admin1Id, OPSLocationEmergencyId, 
                                                                                                                OPSEmergencyClusterId, OPSProjectId, 
                                                                                                                yearId, indicatorId });
                        rptAdmin2.DataSource = dt;
                        rptAdmin2.DataBind();
                    }
                }
            }
        }

        protected void rptAdmin2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //var a = e.Item.Parent.FindControl("");
                GridViewRow row = (e.Item.Parent.Parent.Parent.Parent.Parent.Parent.Parent) as GridViewRow;
                if (row != null)
                {
                    int unitId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["UnitId"].ToString());

                    TextBox txtAdmin2Male = e.Item.FindControl("txtAdmin2TargetMaleProject") as TextBox;
                    TextBox txtAdmin2Female = e.Item.FindControl("txtAdmin2TargetFemaleProject") as TextBox;
                    TextBox txtAdmin2Target = e.Item.FindControl("txtAdmin2TargetProject") as TextBox;

                    txtAdmin2Male.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFE0");
                    txtAdmin2Female.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFE0");
                    txtAdmin2Target.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFE0");

                    if (unitId == 269 || unitId == 28 || unitId == 38 || unitId == 193
                            || unitId == 219 || unitId == 198 || unitId == 311 || unitId == 287
                            || unitId == 67 || unitId == 132 || unitId == 252)
                    {
                        txtAdmin2Target.Enabled = false;
                    }
                    else
                    {
                        txtAdmin2Male.Enabled = false;
                        txtAdmin2Female.Enabled = false;
                    }
                }
            }
        }

        protected void lnkLanguageEnglish_Click(object sender, EventArgs e)
        {
            try
            {
                RC.SelectedSiteLanguageId = (int)RC.SiteLanguage.English;
                RC.AddSiteLangInCookie(this.Response, RC.SiteLanguage.English);
                BindGridData();
            }
            catch { }
        }

        protected void lnkLanguageFrench_Click(object sender, EventArgs e)
        {
            try
            {
                RC.SelectedSiteLanguageId = (int)Common.RC.SiteLanguage.French;
                RC.AddSiteLangInCookie(this.Response, Common.RC.SiteLanguage.French);
                BindGridData();
            }
            catch { }
        }


    }
}