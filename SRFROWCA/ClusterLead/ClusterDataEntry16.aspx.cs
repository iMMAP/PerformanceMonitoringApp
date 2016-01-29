using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClusterLead
{
    public partial class ClusterDataEntry16 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CliearFilterSession();
                LoadCombos();
                DisableDropDowns();
                SetDates();
                //AddIndicatorControl();
                LoadClusterIndicators();

            }

            //string controlName = GetPostBackControlId(this);

            //if (controlName == "btnSaveAll")
            //{
            //    AddIndicatorControl();
            //}
        }

        //private void AddIndicatorControl()
        //{
        //    //pnlAdditionalIndicaotrs.Controls.RemoveAt(0);
        //    if (ddlFrameworkYear.SelectedValue == "12")
        //    {
        //        ctlOutputIndicatorsDataEntry indCtl = (ctlOutputIndicatorsDataEntry)LoadControl("ctlOutputIndicatorsDataEntry.ascx");

        //        int val = 0;
        //        val = RC.GetSelectedIntVal(ddlCountry);
        //        indCtl.EmgLocationId = val > 0 ? val : (int?)null;
        //        val = RC.GetSelectedIntVal(ddlCluster);
        //        indCtl.EmgClusterId = val > 0 ? val : (int?)null;
        //        indCtl.YearId = RC.GetSelectedIntVal(ddlFrameworkYear);
        //        indCtl.MonthId = Convert.ToInt32(ddlMonth.SelectedValue);
        //        indCtl.IsRegional = RC.IsRegionalClusterLead(this.User) ? 1 : (int?)null;

        //        //LoadIndLocations(indCtl, 0, false);
        //        //indCtl.ControlNumber = 1;
        //        indCtl.ID = "indicatorControlId" + "1";
        //        pnlAdditionalIndicaotrs.Controls.Add(indCtl);
        //    }
        //    else
        //    {
        //        ctlOutputIndicatorDataEntryCountry indCtl = (ctlOutputIndicatorDataEntryCountry)LoadControl("ctlOutputIndicatorDataEntryCountry.ascx");

        //        int val = 0;
        //        val = RC.GetSelectedIntVal(ddlCountry);
        //        indCtl.EmgLocationId = val > 0 ? val : (int?)null;
        //        val = RC.GetSelectedIntVal(ddlCluster);
        //        indCtl.EmgClusterId = val > 0 ? val : (int?)null;
        //        indCtl.YearId = RC.GetSelectedIntVal(ddlFrameworkYear);
        //        indCtl.MonthId = Convert.ToInt32(ddlMonth.SelectedValue);
        //        indCtl.IsRegional = RC.IsRegionalClusterLead(this.User) ? 1 : (int?)null;

        //        indCtl.ID = "indicatorControlId" + "1";
        //        pnlAdditionalIndicaotrs.Controls.Add(indCtl);
        //    }
        //}

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

            int yearId = 12;

            if (emgLocationId > 0 && emgClusterId > 0)
            {
                int monthId = Convert.ToInt32(ddlMonth.SelectedValue);
                int? isRegional = RC.IsRegionalClusterLead(this.User) ? 1 : (int?)null;
                dt = DBContext.GetData("GetOutputIndicatorsForReporting_2016", new object[] {emgLocationId, emgClusterId
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

        protected void gvIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.RegionalIndicatorIcon(e, 1);
                ObjPrToolTip.CountryIndicatorIcon(e, 2);
                

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
                        int emgLocId = RC.GetSelectedIntVal(ddlCountry);
                        int emgClusterId = RC.GetSelectedIntVal(ddlCluster);
                        int yearId = 12;
                        int monthId = RC.GetSelectedIntVal(ddlMonth);
                        DataTable dt = DBContext.GetData("GetOutputCountryTargetOfIndicator",
                                                                    new object[] { emgLocId, emgClusterId, 
                                                                               yearId, indicatorId, monthId});
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

        protected void rptCountryGender_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //UI.SetThousandSeparator(e.Item, "lblCountryTargetMaleCluster");
                //UI.SetThousandSeparator(e.Item, "lblCountryTargetFemaleCluster");
                //UI.SetThousandSeparator(e.Item, "lblCountryTargetCluster");
                //UI.SetThousandSeparator(e.Item, "lblCountryTargetMaleProject");
                //UI.SetThousandSeparator(e.Item, "lblCountryTargetFemaleProject");
                //UI.SetThousandSeparator(e.Item, "lblCountryTargetProject");

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
                        int emgLocId = RC.GetSelectedIntVal(ddlCountry);
                        int emgClusterId = RC.GetSelectedIntVal(ddlCluster);
                        int yearId = 12;
                        int monthId = RC.GetSelectedIntVal(ddlMonth);
                        DataTable dt1 = DBContext.GetData("[GetOutputAdmin1TargetOfIndicator]", new object[] {countryId, emgLocId, 
                                                                                                                emgClusterId, yearId, indicatorId, monthId });
                        rptAdmin1.DataSource = dt1;
                        rptAdmin1.DataBind();
                    }
                }
            }
        }

        protected void rptAdmin1Gender_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //UI.SetThousandSeparator(e.Item, "lblAdmin1TargetMaleCluster");
                //UI.SetThousandSeparator(e.Item, "lblAdmin1TargetFemaleCluster");
                //UI.SetThousandSeparator(e.Item, "lblAdmin1TargetCluster");

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
                }

                GridViewRow row = (e.Item.Parent.Parent.Parent.Parent.Parent) as GridViewRow;
                if (row != null)
                {
                    int unitId = 0; // Convert.ToInt32(gvIndicators.DataKeys[row.RowIndex].Values["UnitId"].ToString());
                    Label lblUnit = row.FindControl("lblUnitId") as Label;
                    if (lblUnit != null)
                    {
                        int.TryParse(lblUnit.Text, out unitId);
                    }

                    TextBox txtAdmin2Male = e.Item.FindControl("txtAdmin1TargetMaleProject") as TextBox;
                    TextBox txtAdmin2Female = e.Item.FindControl("txtAdmin1TargetFemaleProject") as TextBox;
                    TextBox txtAdmin2Target = e.Item.FindControl("txtAdmin1TargetProject") as TextBox;

                    txtAdmin2Male.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFE0");
                    txtAdmin2Female.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFE0");
                    txtAdmin2Target.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFE0");

                    if (RC.IsGenderUnit(unitId))
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

        //public bool SaveClusterIndicatorDetails(int countryId, int clusterId, int monthId)
        //{
        //    bool isDataProvided = false;
        //    int clusterIndicatorID = 0;
        //    int? achieved = null;

        //    foreach (GridViewRow row in gvIndicators.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            TextBox txtAchieved = (TextBox)row.FindControl("txtAchieved");
        //            Label lblClusterIndicatorID = (Label)row.FindControl("lblClusterIndicatorID");

        //            if (lblClusterIndicatorID != null)
        //                clusterIndicatorID = Convert.ToInt32(lblClusterIndicatorID.Text);

        //            if (txtAchieved != null)
        //            {
        //                achieved = !string.IsNullOrEmpty(txtAchieved.Text.Trim()) ? Convert.ToInt32(txtAchieved.Text.Trim()) : (int?)null;
        //                if (achieved > 0)
        //                    isDataProvided = true;
        //            }

        //            DBContext.Add("uspInsertClusterReport", new object[] { clusterIndicatorID, clusterId, countryId, 11, monthId,
        //                                                                                            achieved, RC.GetCurrentUserId, null });
        //        }
        //    }
        //    return isDataProvided;
        //}

        public void SaveReportDetails(int emgLocId, int clusterId, int monthId)
        {
            foreach (GridViewRow row in gvIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int indicatorId = 0;
                    int unitId = 0;
                    Label lblIndicator = row.FindControl("lblClusterIndicatorID") as Label;
                    if (lblIndicator != null)
                        int.TryParse(lblIndicator.Text, out indicatorId);

                    Label lblUnitId = row.FindControl("lblUnitId") as Label;
                    if (lblUnitId != null)
                        int.TryParse(lblUnitId.Text, out unitId);


                    Repeater rptCountry = row.FindControl("rptCountryGender") as Repeater;
                    if (rptCountry != null)
                    {
                        CountryRepeater(rptCountry, indicatorId, unitId, emgLocId, clusterId, monthId);
                    }
                }
            }
        }

        private void CountryRepeater(Repeater rptCountry, int indicatorId, int unitId, int emgLocId, int clusterId, int monthId)
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
                        if (RC.IsGenderUnit(unitId))
                            SaveAdmin1GenderTargets(rptAdmin1, indicatorId, countryId, emgLocId, clusterId, monthId);
                        else
                            SaveAdmin1Targets(rptAdmin1, indicatorId, countryId, emgLocId, clusterId, monthId);
                    }
                }
            }
        }

        private void SaveAdmin1Targets(Repeater rptAdmin1, int indicatorId, int countryId, int emgLocId, int clusterId, int monthId)
        {
            foreach (RepeaterItem item in rptAdmin1.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfAdmin1Id = item.FindControl("hfAdmin1Id") as HiddenField;
                    int admin1Id = 0;
                    if (hfAdmin1Id != null)
                        int.TryParse(hfAdmin1Id.Value, out admin1Id);

                    int? achieved = null;
                    TextBox txtTarget = item.FindControl("txtAdmin1TargetProject") as TextBox;
                    achieved = string.IsNullOrEmpty(txtTarget.Text.Trim()) ? (int?)null : Convert.ToInt32(txtTarget.Text.Trim());
                    if (admin1Id > 0)
                    {
                        DBContext.Update("uspInsertClusterReport2", new object[] { indicatorId, emgLocId, clusterId, countryId, admin1Id, 12, monthId,
                                                                                      achieved, RC.GetCurrentUserId, null });
                    }
                }
            }
        }

        private void SaveAdmin1GenderTargets(Repeater rptAdmin1, int indicatorId, int countryId, int emgLocId, int clusterId, int monthId)
        {
            foreach (RepeaterItem item in rptAdmin1.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtAdmin1Male = item.FindControl("txtAdmin1TargetMaleProject") as TextBox;
                    TextBox txtAdmin1Female = item.FindControl("txtAdmin1TargetFemaleProject") as TextBox;

                    int? maleTarget = null;
                    if (txtAdmin1Male != null)
                        maleTarget = string.IsNullOrEmpty(txtAdmin1Male.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin1Male.Text.Trim());

                    int? femaleTarget = null;
                    if (txtAdmin1Female != null)
                        femaleTarget = string.IsNullOrEmpty(txtAdmin1Female.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin1Female.Text.Trim());

                    HiddenField hfAdmin1Id = item.FindControl("hfAdmin1Id") as HiddenField;
                    int admin1Id = 0;
                    if (hfAdmin1Id != null)
                        int.TryParse(hfAdmin1Id.Value, out admin1Id);
                    if (admin1Id > 0)
                    {
                        DBContext.Update("uspInsertClusterReport2_Admin1", new object[] {indicatorId, emgLocId, clusterId, countryId, admin1Id, 12, monthId,
                                                                                        maleTarget, femaleTarget ,RC.GetCurrentUserId, DBNull.Value });
                    }
                }
            }
        }

        //private bool SaveClusterIndicatorDetails()
        //{
        //    bool isDataProvided = false;
        //    int clusterIndicatorID = 0;
        //    int? achieved = null;
        //    int countryId = 0;
        //    int clusterId = 0;

        //    countryId = RC.GetSelectedIntVal(ddlCountry);
        //    clusterId = RC.GetSelectedIntVal(ddlCluster);
        //    int yearId = (int)RC.Year._2015; ;
        //    int monthId = Convert.ToInt32(ddlMonth.SelectedValue);

        //    //foreach (GridViewRow row in gvIndicators.Rows)
        //    //{
        //    //    if (row.RowType == DataControlRowType.DataRow)
        //    //    {
        //    //        TextBox txtAchieved = (TextBox)row.FindControl("txtAchieved");
        //    //        Label lblClusterIndicatorID = (Label)row.FindControl("lblClusterIndicatorID");

        //    //        if (lblClusterIndicatorID != null)
        //    //            clusterIndicatorID = Convert.ToInt32(lblClusterIndicatorID.Text);

        //    //        if (txtAchieved != null)
        //    //        {
        //    //            achieved = !string.IsNullOrEmpty(txtAchieved.Text.Trim()) ? Convert.ToInt32(txtAchieved.Text.Trim()) : (int?)null;
        //    //            if (achieved > 0)
        //    //                isDataProvided = true;
        //    //        }

        //    //        DBContext.Add("uspInsertClusterReport", new object[] { clusterIndicatorID, clusterId, countryId, yearId, monthId, achieved, RC.GetCurrentUserId, null });
        //    //    }
        //    //}
        //    return isDataProvided;
        //}

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

            //int countryId = RC.GetSelectedIntVal(ddlCountry);
            //int clusterId = RC.GetSelectedIntVal(ddlCluster);
            int monthId = Convert.ToInt32(ddlMonth.SelectedValue);
            //bool isAdded = false;

            //if (ddlFrameworkYear.SelectedValue == "11")
            //{
            //    foreach (Control ctl in pnlAdditionalIndicaotrs.Controls)
            //    {
            //        if (ctl != null && ctl.ID != null && ctl.ID.Contains("indicatorControlId"))
            //        {
            //            ctlOutputIndicatorDataEntryCountry indControl = ctl as ctlOutputIndicatorDataEntryCountry;

            //            if (indControl != null)
            //            {
            //                isAdded = indControl.SaveClusterIndicatorDetails(countryId, clusterId, monthId);
            //            }
            //        }
            //    }
            //}

            ////if (ddlFrameworkYear.SelectedValue == "12")
            //{
            //    //foreach (Control ctl in pnlAdditionalIndicaotrs.Controls)
            //    {
            //        //if (ctl != null && ctl.ID != null && ctl.ID.Contains("indicatorControlId"))
            //        {
            //           // ctlOutputIndicatorsDataEntry indControl = ctl as ctlOutputIndicatorsDataEntry;

            //           // if (indControl != null)
            //            {
            //                indControl.SaveReportDetails(countryId, clusterId, monthId);
            //            }
            //        }
            //    }
            //}

            SaveReportDetails(emgCountryId, emgClusterId, monthId);

            ShowMessage("Data Saved Successfully!");
            //if (isAdded)
            //{
            //    // SendEmail();
            //}
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
            LoadClusterIndicators();
            //AddIndicatorControl();
            SaveFiltersInSession();
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
            //AddIndicatorControl();
            SaveFiltersInSession();
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
            //AddIndicatorControl();
            SaveFiltersInSession();
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadClusterIndicators();
            //AddIndicatorControl();
        }

        //protected void gvIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        ObjPrToolTip.RegionalIndicatorIcon(e, 11);
        //        ObjPrToolTip.CountryIndicatorIcon(e, 12);

        //        UI.SetThousandSeparator(e.Row, "lblOrigionalTarget");
        //        UI.SetThousandSeparator(e.Row, "lblTarget");
        //        UI.SetThousandSeparator(e.Row, "lblSum");
        //    }
        //}

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