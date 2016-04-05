﻿using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClustOutputIndicators
{
    public partial class TestDataEntry : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CliearFilterSession();
                LoadCombos();
                DisableDropDowns();
                SetDates();
                                AddIndicatorControl();
                //LoadClusterIndicators();
               
            }

            string controlName = GetPostBackControlId(this);

            if (controlName == "btnSaveAll")
            {
                AddIndicatorControl();
            }
        }

        private void AddIndicatorControl()
        {
            //pnlAdditionalIndicaotrs.Controls.RemoveAt(0);
            if (ddlFrameworkYear.SelectedValue == "12")
            {
                ctlOutputIndicatorsDataEntry indCtl = (ctlOutputIndicatorsDataEntry)LoadControl("ctlOutputIndicatorsDataEntry.ascx");

                int val = 0;
                val = RC.GetSelectedIntVal(ddlCountry);
                indCtl.EmgLocationId = val > 0 ? val : (int?)null;
                val = RC.GetSelectedIntVal(ddlCluster);
                indCtl.EmgClusterId = val > 0 ? val : (int?)null;
                indCtl.YearId = RC.GetSelectedIntVal(ddlFrameworkYear);
                indCtl.MonthId = Convert.ToInt32(ddlMonth.SelectedValue);
                indCtl.IsRegional = RC.IsRegionalClusterLead(this.User) ? 1 : (int?)null;

                //LoadIndLocations(indCtl, 0, false);
                //indCtl.ControlNumber = 1;
                indCtl.ID = "indicatorControlId" + "1";
                pnlAdditionalIndicaotrs.Controls.Add(indCtl);
            }
            else
            {
                ctlOutputIndicatorDataEntryCountry indCtl = (ctlOutputIndicatorDataEntryCountry)LoadControl("ctlOutputIndicatorDataEntryCountry.ascx");

                int val = 0;
                val = RC.GetSelectedIntVal(ddlCountry);
                indCtl.EmgLocationId = val > 0 ? val : (int?)null;
                val = RC.GetSelectedIntVal(ddlCluster);
                indCtl.EmgClusterId = val > 0 ? val : (int?)null;
                indCtl.YearId = RC.GetSelectedIntVal(ddlFrameworkYear);
                indCtl.MonthId = Convert.ToInt32(ddlMonth.SelectedValue);
                indCtl.IsRegional = RC.IsRegionalClusterLead(this.User) ? 1 : (int?)null;

                indCtl.ID = "indicatorControlId" + "1";
                pnlAdditionalIndicaotrs.Controls.Add(indCtl);
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

        //internal override void BindGridData()
        //{
        //    LoadCombos();
        //    DisableDropDowns();
        //    SetDates();
        //    SetFiltersFromSession();
        //    LoadClusterIndicators();
        //}

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

        //private void LoadClusterIndicators()
        //{
        //    gvIndicators.DataSource = GetClusterIndicatros();
        //    gvIndicators.DataBind();
        //}

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
                int? isRegional = RC.IsRegionalClusterLead(this.User) ? 1 : (int?)null;
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
            int yearId = (int)RC.Year._2015; ;
            int monthId = Convert.ToInt32(ddlMonth.SelectedValue);

            //foreach (GridViewRow row in gvIndicators.Rows)
            //{
            //    if (row.RowType == DataControlRowType.DataRow)
            //    {
            //        TextBox txtAchieved = (TextBox)row.FindControl("txtAchieved");
            //        Label lblClusterIndicatorID = (Label)row.FindControl("lblClusterIndicatorID");

            //        if (lblClusterIndicatorID != null)
            //            clusterIndicatorID = Convert.ToInt32(lblClusterIndicatorID.Text);

            //        if (txtAchieved != null)
            //        {
            //            achieved = !string.IsNullOrEmpty(txtAchieved.Text.Trim()) ? Convert.ToInt32(txtAchieved.Text.Trim()) : (int?)null;
            //            if (achieved > 0)
            //                isDataProvided = true;
            //        }

            //        DBContext.Add("uspInsertClusterReport", new object[] { clusterIndicatorID, clusterId, countryId, yearId, monthId, achieved, RC.GetCurrentUserId, null });
            //    }
            //}
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

            //bool isAdded = SaveClusterIndicatorDetails();
            //LoadClusterIndicators();

            int countryId = RC.GetSelectedIntVal(ddlCountry);
            int clusterId = RC.GetSelectedIntVal(ddlCluster);
            int monthId = Convert.ToInt32(ddlMonth.SelectedValue);
            bool isAdded = false;

            if (ddlFrameworkYear.SelectedValue == "11")
            {
                foreach (Control ctl in pnlAdditionalIndicaotrs.Controls)
                {
                    if (ctl != null && ctl.ID != null && ctl.ID.Contains("indicatorControlId"))
                    {
                        ctlOutputIndicatorDataEntryCountry indControl = ctl as ctlOutputIndicatorDataEntryCountry;

                        if (indControl != null)
                        {
                            isAdded = indControl.SaveClusterIndicatorDetails(countryId, clusterId, monthId);
                        }
                    }
                }
            }

            if (ddlFrameworkYear.SelectedValue == "12")
            {
                foreach (Control ctl in pnlAdditionalIndicaotrs.Controls)
                {
                    if (ctl != null && ctl.ID != null && ctl.ID.Contains("indicatorControlId"))
                    {
                        ctlOutputIndicatorsDataEntry indControl = ctl as ctlOutputIndicatorsDataEntry;

                        if (indControl != null)
                        {
                            indControl.SaveReportDetails(countryId, clusterId, monthId);
                        }
                    }
                }
            }

            ShowMessage("Data Saved Successfully!");
            if (isAdded)
            {
               // SendEmail();
            }
        }

        private void SendEmail()
        {
            int emgCountryId = RC.GetSelectedIntVal(ddlCountry);
            int? emgClsuterId = RC.GetSelectedIntVal(ddlCluster);
            string subject = "Output Indicator Report Saved/Updated For the month of " + ddlMonth.SelectedItem.Text;
            string country = ddlCountry.SelectedItem.Text;
            string cluster = ddlCluster.SelectedItem.Text;
            string user = "";
            try { user = User.Identity.Name; }
            catch { }

            string body = string.Format(@"<b>{0}</b><br/>
                                         <b>Country:</b> {1}<br/>
                                         <b>Cluster:</b> {2}<br/>
                                         <b>Month:</b> {3}<br/>
                                         <b>Added By:</b> {4}"
                                         , subject, country, cluster, ddlMonth.SelectedItem.Text, user);
            RC.SendEmail(emgCountryId, emgClsuterId, subject, body);
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadClusterIndicators();
            AddIndicatorControl();
            SaveFiltersInSession();
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadClusterIndicators();
            AddIndicatorControl();
            SaveFiltersInSession();
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadClusterIndicators();
            AddIndicatorControl();
            SaveFiltersInSession();
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadClusterIndicators();
            AddIndicatorControl();
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

        private void SaveFiltersInSession()
        {
            int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
            int emgClusterId = RC.GetSelectedIntVal(ddlCluster);

            if (emgLocationId > 0)
                Session["ClusterDataEntryCountry"] = emgLocationId;
            else
                Session["ClusterDataEntryCountry"] = null;

            if (emgClusterId > 0)
                Session["ClusterDataEntryCluster"] = emgClusterId;
            else
                Session["ClusterDataEntryCluster"] = null;
        }

        private void SetFiltersFromSession()
        {
            if (Session["ClusterDataEntryCountry"] != null)
            {
                int emgLocationId = 0;
                int.TryParse(Session["ClusterDataEntryCountry"].ToString(), out emgLocationId);
                if (emgLocationId > 0)
                {
                    try
                    {
                        ddlCountry.SelectedValue = emgLocationId.ToString();
                    }
                    catch { }
                }
            }

            if (Session["ClusterDataEntryCluster"] != null)
            {
                int clusterId = 0;
                int.TryParse(Session["ClusterDataEntryCluster"].ToString(), out clusterId);
                if (clusterId > 0)
                {
                    try
                    {
                        ddlCluster.SelectedValue = clusterId.ToString();
                    }
                    catch { }
                }
            }
        }

        private void CliearFilterSession()
        {
            Session["ClusterDataEntryCountry"] = null;
            Session["ClusterDataEntryCluster"] = null;
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

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}