﻿using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClusterLead
{
    public partial class ClusterDataEntry : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CliearFilterSession();
                LoadCombos();
                RC.SetFiltersFromSession(ddlCountry, ddlCluster, Session);
                DisableDropDowns();
                SetDates();
                LoadClusterIndicators();
            }
        }

        private void DisableDropDowns()
        {
            if (RC.IsClusterLead(this.User))
            {
                RC.EnableDisableControls(ddlCluster, false);
                RC.EnableDisableControls(ddlCountry, false);
            }

            if (RC.IsCountryAdmin(this.User))
            {
                RC.EnableDisableControls(ddlCountry, false);
            }

            if (RC.IsRegionalClusterLead(this.User))
            {
                RC.EnableDisableControls(ddlCluster, false);
            }
        }

        internal override void BindGridData()
        {
            LoadCombos();
            DisableDropDowns();
            SetDates();
            RC.SetFiltersFromSession(ddlCountry, ddlCluster, Session);
            LoadClusterIndicators();
        }

        private void LoadCombos()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);

            PopulateMonths();

            ddlCluster.Items.Insert(0, new ListItem("Select Cluster", "0"));
            ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));

            SetComboValues();
        }

        private void SetDates()
        {
            int month = DateTime.Now.Month - 1;
            if (month.Equals(12))
                month = 1;
            else if (month.Equals(11))
                month = 12;

            ddlMonth.SelectedValue = month.ToString();
        }

        private void LoadClusterIndicators()
        {            
            gvIndicators.DataSource = GetClusterIndicatros();
            gvIndicators.DataBind();
        }

        private void SetComboValues()
        {
            if (RC.IsClusterLead(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
            }

            if (RC.IsCountryAdmin(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
            }

            if (RC.IsRegionalClusterLead(this.User))
            {
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
            }
        }

        private DataTable GetClusterIndicatros()
        {
            DataTable dt = new DataTable();
            int val = 0;
            val = RC.GetSelectedIntVal(ddlCountry);
            int? emgLocationId = val > 0 ? val : (int?)null;

            val = RC.GetSelectedIntVal(ddlCluster);
            int? emgClusterId = val > 0 ? val : (int?)null;

            int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);

            if (emgLocationId > 0 && emgClusterId > 0)
            {
                int monthId = Convert.ToInt32(ddlMonth.SelectedValue);
                int? isRegional = RC.IsRegionalClusterLead(this.User) ? 1 : (int?) null;
                dt = DBContext.GetData("GetOutputIndicatorsForReporting", new object[] {emgLocationId, emgClusterId
                                                                                        ,yearId,monthId, isRegional, 
                                                                                        RC.SelectedSiteLanguageId});
            }

            return dt;
        }

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

        private bool SaveClusterIndicatorDetails()
        {
            bool isDataProvided = false;
            int clusterIndicatorID = 0;
            int? achieved = null;
            int countryId = 0;
            int clusterId = 0;

            countryId = RC.GetSelectedIntVal(ddlCountry);
            clusterId = RC.GetSelectedIntVal(ddlCluster);
            int yearId = (int)RC.Year._2015; 
            int monthId = Convert.ToInt32(ddlMonth.SelectedValue);            

            foreach (GridViewRow row in gvIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtAchieved = (TextBox)row.FindControl("txtAchieved");
                    Label lblClusterIndicatorID = (Label)row.FindControl("lblClusterIndicatorID");

                    if (lblClusterIndicatorID != null)
                        clusterIndicatorID = Convert.ToInt32(lblClusterIndicatorID.Text);

                    if (txtAchieved != null)
                    {
                        achieved = !string.IsNullOrEmpty(txtAchieved.Text.Trim()) ? Convert.ToInt32(txtAchieved.Text.Trim()) : (int?)null;
                        if (achieved > 0)
                            isDataProvided = true;
                    }

                    DBContext.Add("uspInsertClusterReport", new object[] { clusterIndicatorID, clusterId, countryId, yearId, monthId, achieved, RC.GetCurrentUserId, null });
                }
            }
            return isDataProvided;
        }

        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            int emgClusterId = RC.GetSelectedIntVal(ddlCluster);
            int emgCountryId = RC.GetSelectedIntVal(ddlCountry);

            if (RC.IsAdmin(this.User))
            {
                if (emgClusterId <= 0 && emgCountryId <= 0)
                {
                    ShowMessage("Please select Cluster && Country to save data", RC.NotificationType.Error, true, 4000);
                    return;
                }
            }

            if (RC.IsCountryAdmin(this.User))
            {
                if (emgClusterId <= 0)
                {
                    ShowMessage("Please select Cluster to save data", RC.NotificationType.Error, true, 4000);
                    return;
                }
            }

            bool isAdded = SaveClusterIndicatorDetails();
            LoadClusterIndicators();

            ShowMessage("Data Saved Successfully!");            
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
            RC.SaveFiltersInSession(ddlCountry, ddlCluster, Session);
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
            RC.SaveFiltersInSession(ddlCountry, ddlCluster, Session);
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
            RC.SaveFiltersInSession(ddlCountry, ddlCluster, Session);
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }       

        protected void gvIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.RegionalIndicatorIcon(e, 11);
                ObjPrToolTip.CountryIndicatorIcon(e, 12);

                UI.SetThousandSeparator(e.Row, "lblOrigionalTarget");
                UI.SetThousandSeparator(e.Row, "lblTarget");
                UI.SetThousandSeparator(e.Row, "lblSum");
            }
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

       

        private void CliearFilterSession()
        {
            Session["ClusterDataEntryCountry"] = null;
            Session["ClusterDataEntryCluster"] = null;
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}