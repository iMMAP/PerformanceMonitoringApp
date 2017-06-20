using BusinessLogic;
using SRFROWCA.Common;
using SRFROWCA.Configurations;
using SRFROWCA.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.Transactions;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClusterLead
{
    public partial class AddActivityAndIndicators : BasePage
    {
        protected void Page_PreLoad(object sender, EventArgs e)
        {
            string control = Utils.GetPostBackControlId(this);
            if (control == "btnAddIndicatorControl")
            {
                IndControlId += 1;
                NewIndicatorControlsAdded += 1;
            }

            if (control == "btnRemoveIndicatorControl")
            {
                IndControlId -= 1;
                NewIndicatorControlsAdded -= 1;
            }
            btnRemoveIndicatorControl.Visible = !(NewIndicatorControlsAdded < 1);

            for (int i = 0; i < IndControlId; i++)
                AddIndicatorControl(i);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ToggleMSRefugeeControls();
                LoadCombos();
                RC.SetFiltersFromSessionCluster(ddlCountry, ddlCluster, Session);
                DisableDropDowns();
                PopulateObjective();

                // IN Edit mode.
                if (Request.QueryString["a"] != null)
                {
                    int activityId = 0;
                    int.TryParse(Request.QueryString["a"].ToString(), out activityId);
                    if (activityId > 0)
                        LoadIndicatorToEdit(activityId);
                }
                else
                {
                    AddIndicatorControl(0);
                    IndControlId = 1;
                }
            }

            if (RC.IsClusterLead(this.User))
            {
                int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
                int emgClusterId = RC.GetSelectedIntVal(ddlCluster);

                if (emgClusterId <= 0)
                {
                    if (Session["ClusterFrameworkSelectedCluster"] != null)
                    {
                        int clusterId = 0;
                        int.TryParse(Session["ClusterFrameworkSelectedCluster"].ToString(), out clusterId);

                        int year = 0;
                        if (Request.QueryString["year"] != null)
                            int.TryParse(Request.QueryString["year"].ToString(), out year);
                        int indUnused = SectorFramework.IndUnused(emgLocationId, emgClusterId, year);
                        if (indUnused <= 0)
                            btnAddIndicatorControl.Visible = false;
                        else
                            btnAddIndicatorControl.Visible = true;
                    }
                }
            }
        }

        private void ToggleMSRefugeeControls()
        {
            if (RC.IsAdmin(this.User) || RC.IsCountryAdmin(this.User))
            {
                if (Session["ClusterFrameworkSelectedCluster"] != null)
                {
                    int clusterId = 0;
                    int.TryParse(Session["ClusterFrameworkSelectedCluster"].ToString(), out clusterId);
                    if (clusterId > 0)
                    {
                        if (clusterId == (int)RC.ClusterSAH2015.MS)
                        {
                            lblMSRefugee.Visible = true;
                            cbMSRefugees.Checked = true;
                            cbMSRefugees.Visible = true;
                            cbMSRefugees.Enabled = false;
                        }

                    }
                }
            }
            else if (UserInfo.EmergencyCluster == (int)RC.ClusterSAH2015.MS)
            {
                lblMSRefugee.Visible = true;
            }
        }

        private void LoadIndicatorToEdit(int activityId)
        {
            DataTable dt = GetActivityIndicators(activityId);
            if (dt.Rows.Count > 0)
            {
                int count = dt.Rows.Count;
                PopulateActivitiesControls(dt);
                for (int i = 0; i < count; i++)
                {
                    FrameworkIndicators indCtl = (FrameworkIndicators)LoadControl("~/controls/FrameworkIndicators.ascx");
                    PopulateIndicatorsControls(indCtl, dt, i);
                    pnlAdditionalIndicaotrs.Controls.Add(indCtl);
                    IndControlId += 1;
                }
            }
        }

        private void PopulateIndicatorsControls(FrameworkIndicators ctl, DataTable dt, int i)
        {
            int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
            ctl.ControlNumber = i + 1;
            ctl.ID = "indicatorControlId" + i.ToString();

            ctl.txtInd1Eng.Text = dt.Rows[i]["IndicatorEng"].ToString();
            ctl.txtInd1Fr.Text = dt.Rows[i]["IndicatorFr"].ToString();
            int unitId = 0;
            int.TryParse(dt.Rows[i]["UnitId"].ToString(), out unitId);
            if (unitId > 0)
            {
                ctl.PopulateUnits(false);
                ctl.ddlUnit.SelectedValue = unitId.ToString();
            }

            int calMethod = 0;
            int.TryParse(dt.Rows[i]["IndicatorCalculationTypeId"].ToString(), out calMethod);
            if (calMethod > 0)
            {
                ctl.ddlCalculationMethod.SelectedValue = calMethod.ToString();
            }

            bool isCP = false;
            bool.TryParse(dt.Rows[i]["IsChildProtection"].ToString(), out isCP);
            ctl.cbCP.Checked = isCP;

            bool isSGBV = false;
            bool.TryParse(dt.Rows[i]["IsSGBV"].ToString(), out isSGBV);
            ctl.cbSGBV.Checked = isSGBV;

            int indicatorId = 0;
            int.TryParse(dt.Rows[i]["IndicatorId"].ToString(), out indicatorId);
            ctl.hfIndicatorId.Value = indicatorId.ToString();

            bool isGender = RC.IsGenderUnit(unitId);
            LoadIndLocations(ctl, indicatorId, isGender);
        }

        private void PopulateActivitiesControls(DataTable dt)
        {
            txtActivityEng.Text = dt.Rows[0]["ActivityEng"].ToString();
            txtActivityFr.Text = dt.Rows[0]["ActivityFr"].ToString();

            int emgLocationId = 0;
            int.TryParse(dt.Rows[0]["EmergencyLocationId"].ToString(), out emgLocationId);
            if (emgLocationId > 0)
                ddlCountry.SelectedValue = emgLocationId.ToString();

            int emgClusterId = 0;
            int.TryParse(dt.Rows[0]["EmergencyClusterId"].ToString(), out emgClusterId);
            if (emgClusterId > 0)
            {
                if (emgClusterId == (int)RC.ClusterSAH2015.MS)
                {
                    cbMSRefugees.Checked = true;
                    int secEmgClusterId = 0;
                    int.TryParse(dt.Rows[0]["EmgergencySecClusterId"].ToString(), out secEmgClusterId);
                    ddlCluster.SelectedValue = secEmgClusterId.ToString();
                }
                else
                    ddlCluster.SelectedValue = emgClusterId.ToString();
            }

            int emgObjectiveId = 0;
            int.TryParse(dt.Rows[0]["EmergencyObjectiveId"].ToString(), out emgObjectiveId);
            if (emgObjectiveId > 0)
                ddlObjective.SelectedValue = emgObjectiveId.ToString();
        }

        private void LoadIndLocations(FrameworkIndicators ctl, int indicatorId, bool isGender)
        {
            int year = 0;
            if (Request.QueryString["year"] != null)
                int.TryParse(Request.QueryString["year"].ToString(), out year);
            ctl.indYear = year;
            ctl.indCtlEmgLocId = RC.GetSelectedIntVal(ddlCountry);
            ctl.indCtlEmgClusterId = RC.GetSelectedIntVal(ddlCluster);
            ctl.indCtlIndicatorId = indicatorId;
            ctl.indCtlIsGender = isGender;
            ctl.hfAdmin2CtlIsGender.Value = isGender.ToString();

            if (cbMSRefugees.Checked || UserInfo.EmergencyCluster == (int)RC.ClusterSAH2015.MS)
            {
                ctl.indCtlEmgClusterId = (int)RC.ClusterSAH2015.MS;
            }
            
        }



        private void UpdateRepeaterTargetColumn(Repeater repeater)
        {
            foreach (RepeaterItem item in repeater.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtTotal = item.FindControl("txtTarget") as TextBox;
                    if (txtTotal != null)
                    {
                        txtTotal.Text = "";
                    }

                    Repeater rptAdmin1 = item.FindControl("rptAdmin1") as Repeater;
                    if (rptAdmin1 != null)
                        UpdateRepeaterTargetColumn(rptAdmin1);

                    Repeater rptAdmin2 = item.FindControl("rptAdmin2") as Repeater;
                    if (rptAdmin2 != null)
                        UpdateRepeaterTargetColumn(rptAdmin2);
                }
            }
        }

        private DataTable GetCountryIndTarget(int emgLocationId, int indicatorId)
        {
            return DBContext.GetData("GetCountryTargetOfIndicator", new object[] { emgLocationId, indicatorId });
        }


        private DataTable GetActivityIndicators(int activityId)
        {
            int active = 1;
            return DBContext.GetData("GetActivityIndicators", new object[] { activityId, active, RC.SelectedSiteLanguageId });
        }

        private void LoadCombos()
        {
            PopulateClusters();
            PopulateCountries();
            SetComboValues();
            
        }

        internal override void BindGridData()
        {
            PopulateClusters();
            PopulateObjective();
        }

        private void SetComboValues()
        {
            if (RC.IsClusterLead(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();

                if (UserInfo.EmergencyCluster != (int)RC.ClusterSAH2015.MS)
                    ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
            }

            if (RC.IsCountryAdmin(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
            }
        }

        private void DisableDropDowns()
        {
            int clusterId = 0;
            if (Session["ClusterFrameworkSelectedCluster"] != null)
                int.TryParse(Session["ClusterFrameworkSelectedCluster"].ToString(), out clusterId);

            if (!(UserInfo.EmergencyCluster == (int)RC.ClusterSAH2015.MS || clusterId == (int)RC.ClusterSAH2015.MS))
                RC.EnableDisableControls(ddlCluster, false);

            RC.EnableDisableControls(ddlCountry, false);
        }

        private void PopulateClusters()
        {
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            if (RC.SelectedSiteLanguageId == 1)
                ddlCluster.Items.Insert(0, new ListItem("Select Cluster", "0"));
            else
                ddlCluster.Items.Insert(0, new ListItem("Sélectionner Cluster", "0"));
        }

        private void PopulateCountries()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            if (RC.SelectedSiteLanguageId == 1)
                ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));
            else
                ddlCountry.Items.Insert(0, new ListItem("Sélectionner Pays", "0"));
        }

        private void PopulateObjective()
        {
            int? emergencyLocationId = ddlCountry.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCountry.SelectedValue);
            int year = 0;
            if (Request.QueryString["year"] != null)
                int.TryParse(Request.QueryString["year"].ToString(), out year);

            RC.Year yearEnum;
            int yearId = 0;
            if (Enum.TryParse("_" + year.ToString(), out yearEnum))
                yearId = (int)yearEnum;

            if (yearId == 0)
                yearId = (int)RC.Year._2017;

            UI.PopulateEmergencyObjectives(ddlObjective, yearId, emergencyLocationId);
        }

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control ctl in pnlAdditionalIndicaotrs.Controls)
            {
                if (ctl != null && ctl.ID != null && ctl.ID.Contains("indicatorControlId"))
                {
                    FrameworkIndicators indCtl = ctl as FrameworkIndicators;
                    LoadIndLocations(indCtl, 0, false);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsTargetProvided())
            {
                if (RC.SelectedSiteLanguageId == 1)
                {
                    ShowMessage("Please provide target for at least one location in all indicators.", RC.NotificationType.Error, true, 3000);
                }
                else
                {

                    ShowMessage("Se il vous plaît fournir cible pour au moins un emplacement de tous les indicateurs.", RC.NotificationType.Error, true, 3000);
                }
                return;
            }

            using (TransactionScope scope = new TransactionScope())
            {
                SaveData();
                scope.Complete();
                Response.Redirect("~/ClusterLead/IndicatorListing.aspx");
            }
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        private bool IsTargetProvided()
        {
            int year = 0;
            if (Request.QueryString["year"] != null)
                int.TryParse(Request.QueryString["year"].ToString(), out year);

            int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
            int emgClusterId = RC.GetSelectedIntVal(ddlCluster);

            AdminTargetSettingItems items = RC.AdminTargetSettings(emgLocationId, emgClusterId, year);

            bool isTargetValid = true;

            if (items.IsMandatory)
            {
                foreach (Control ctl in pnlAdditionalIndicaotrs.Controls)
                {
                    if (ctl != null && ctl.ID != null && ctl.ID.Contains("indicatorControlId"))
                    {
                        FrameworkIndicators indCtl = ctl as FrameworkIndicators;
                        if (indCtl != null)
                        {
                            int unitId = RC.GetSelectedIntVal(indCtl.ddlUnit);
                            Panel pnlTarget = indCtl.FindControl("pnlTargets") as Panel;
                            if (pnlTarget != null)
                            {

                                foreach (Control ctlTareget in pnlTarget.Controls)
                                {
                                    if (ctlTareget != null && ctlTareget.ID != null && ctlTareget.ID.Contains("AdminTargetControl"))
                                    {
                                        if (items.AdminLevel == RC.AdminLevels.Country)
                                        {
                                            ctlCountryTargets countryCtl = ctlTareget as ctlCountryTargets;
                                            if (countryCtl != null)
                                            {
                                                if (RC.IsGenderUnit(unitId))
                                                    isTargetValid = TargetProvidedCountry(countryCtl.rptCountryGender, true);
                                                else
                                                    isTargetValid = TargetProvidedCountry(countryCtl.rptCountry, false);
                                            }
                                        }
                                        else if (items.AdminLevel == RC.AdminLevels.Admin1)
                                        {
                                            ctlAdmin1Targets adm1Ctl = ctlTareget as ctlAdmin1Targets;
                                            if (adm1Ctl != null)
                                            {
                                                if (RC.IsGenderUnit(unitId))
                                                    isTargetValid = TargetProvidedAdmin1(adm1Ctl.rptCountryGender, true);
                                                else
                                                    isTargetValid = TargetProvidedAdmin1(adm1Ctl.rptCountry, false);
                                            }
                                        }
                                        else if (items.AdminLevel == RC.AdminLevels.Admin2)
                                        {
                                            ctlAdmin2Targets adm2Ctl = ctlTareget as ctlAdmin2Targets;
                                            if (adm2Ctl != null)
                                            {
                                                if (RC.IsGenderUnit(unitId))
                                                    isTargetValid = TargetProvidedAdmin2(adm2Ctl.rptCountryGender, true);
                                                else
                                                    isTargetValid = TargetProvidedAdmin2(adm2Ctl.rptCountry, false);
                                            }
                                        }

                                        if (!isTargetValid)
                                            break;
                                    }
                                }
                            }
                            if (!isTargetValid)
                                break;
                        }
                    }
                }
            }

            return isTargetValid;
        }


        private bool TargetProvidedAdmin2(Repeater rpt, bool isGender)
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
                                    TextBox txtTargetMale = admin2Item.FindControl("txtTargetMale") as TextBox;
                                    TextBox txtTargetFemale = admin2Item.FindControl("txtTargetFemale") as TextBox;
                                    if (!string.IsNullOrEmpty(txtTargetMale.Text.Trim()) || !string.IsNullOrEmpty(txtTargetFemale.Text.Trim()))
                                    {
                                        isTargetValid = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    TextBox txtTarget = admin2Item.FindControl("txtTarget") as TextBox;
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

        private bool TargetProvidedAdmin1(Repeater rpt, bool isGender)
        {
            bool isTargetValid = false;
            foreach (RepeaterItem item in rpt.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater rptAdmin1 = (Repeater)item.FindControl("rptAdmin1");
                    foreach (RepeaterItem admin1Item in rptAdmin1.Items)
                    {
                        if (isGender)
                        {
                            TextBox txtTargetMale = admin1Item.FindControl("txtTargetMale") as TextBox;
                            TextBox txtTargetFemale = admin1Item.FindControl("txtTargetFemale") as TextBox;
                            if (!string.IsNullOrEmpty(txtTargetMale.Text.Trim()) || !string.IsNullOrEmpty(txtTargetFemale.Text.Trim()))
                            {
                                isTargetValid = true;
                                break;
                            }
                        }
                        else
                        {
                            TextBox txtTarget = admin1Item.FindControl("txtTarget") as TextBox;
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

            return isTargetValid;
        }

        private bool TargetProvidedCountry(Repeater rpt, bool isGender)
        {
            bool isTargetValid = false;
            foreach (RepeaterItem item in rpt.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    if (isGender)
                    {
                        TextBox txtTargetMale = item.FindControl("txtTargetMale") as TextBox;
                        TextBox txtTargetFemale = item.FindControl("txtTargetFemale") as TextBox;
                        if (!string.IsNullOrEmpty(txtTargetMale.Text.Trim()) || !string.IsNullOrEmpty(txtTargetFemale.Text.Trim()))
                            isTargetValid = true;
                    }
                    else
                    {
                        TextBox txtTarget = item.FindControl("txtTarget") as TextBox;
                        if (!string.IsNullOrEmpty(txtTarget.Text.Trim()))
                            isTargetValid = true;
                    }
                }
            }

            return isTargetValid;
        }

        private void SaveData()
        {
            if (!Page.IsValid) return;
            if (Request.QueryString["a"] == null)
            {
                int ActivityId = SaveActivity();
                if (ActivityId > 0)
                {
                    StringBuilder strIndcators = new StringBuilder();
                    foreach (Control ctl in pnlAdditionalIndicaotrs.Controls)
                    {
                        if (ctl != null && ctl.ID != null && ctl.ID.Contains("indicatorControlId"))
                        {
                            FrameworkIndicators indControl = ctl as FrameworkIndicators;

                            if (indControl != null)
                            {
                                indControl.SaveIndicators(ActivityId);
                                //strIndcators.AppendFormat("Indicator (English): {0}<br/>Indicator (French): {1}<br/><br/>", txtEng.Text, txtFr.Text);
                            }
                        }
                    }
                }
            }
            else
            {
                int activityId = 0;
                if (Request.QueryString["a"] != null)
                {
                    int.TryParse(Request.QueryString["a"].ToString(), out activityId);
                }

                if (activityId > 0)
                {
                    UpdateActivity(activityId);
                    AddUpdateIndicators(activityId);
                }
            }
        }

        private void AddUpdateIndicators(int activityId)
        {
            foreach (Control ctl in pnlAdditionalIndicaotrs.Controls)
            {
                if (ctl != null && ctl.ID != null && ctl.ID.Contains("indicatorControlId"))
                {
                    FrameworkIndicators indControl = ctl as FrameworkIndicators;

                    if (indControl != null)
                    {
                        indControl.SaveIndicators(activityId);
                    }
                }
            }
        }

        private void UpdateActivity(int activityId)
        {
            int emgLocationId = Convert.ToInt32(ddlCountry.SelectedValue);
            int emgClusterId = 0;
            int? emgSecClusterId = null;
            if (UserInfo.EmergencyCluster == (int)RC.ClusterSAH2015.MS || cbMSRefugees.Checked)
            {
                emgClusterId = (int)RC.ClusterSAH2015.MS;
                emgSecClusterId = RC.GetSelectedIntVal(ddlCluster);
            }
            else
                emgClusterId = RC.GetSelectedIntVal(ddlCluster);

            int emgObjectiveId = Convert.ToInt32(ddlObjective.SelectedValue);
            string actEn = !string.IsNullOrEmpty(txtActivityEng.Text.Trim()) ? txtActivityEng.Text.Trim() : null;
            string actFr = !string.IsNullOrEmpty(txtActivityFr.Text.Trim()) ? txtActivityFr.Text.Trim() : null;

            if (!string.IsNullOrEmpty(actFr) || !string.IsNullOrEmpty(actEn))
            {
                DBContext.Update("UpdateActivtiyNew2", new object[] { activityId, emgLocationId, emgClusterId, 
                                                                        emgObjectiveId, actEn, actFr, 
                                                                        RC.GetCurrentUserId, emgSecClusterId, DBNull.Value });
            }
        }


        private int SaveActivity()
        {
            int emergencyLocationId = RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User) ? UserInfo.EmergencyCountry : Convert.ToInt32(ddlCountry.SelectedValue);

            int emergencyClusterId = 0;
            int? emgSecClusterId = null;
            if (UserInfo.EmergencyCluster == (int)RC.ClusterSAH2015.MS || cbMSRefugees.Checked)
            {
                emergencyClusterId = (int)RC.ClusterSAH2015.MS;
                emgSecClusterId = RC.GetSelectedIntVal(ddlCluster);
            }
            else
                emergencyClusterId = RC.GetSelectedIntVal(ddlCluster);

            Guid userId = RC.GetCurrentUserId;

            int objId = RC.GetSelectedIntVal(ddlObjective);
            string actEn = !string.IsNullOrEmpty(txtActivityEng.Text.Trim()) ? txtActivityEng.Text.Trim() : null;
            string actFr = !string.IsNullOrEmpty(txtActivityFr.Text.Trim()) ? txtActivityFr.Text.Trim() : null;
            int yearId = (int)RC.Year._2017;
            return DBContext.Add("InsertActivityNew", new object[] { emergencyClusterId, objId,emergencyLocationId, 
                                                                        actEn, actFr, userId, yearId,
                                                                        RC.SelectedSiteLanguageId, emgSecClusterId, DBNull.Value });
        }

        protected void btnAddIndiatorControl_Click(object sender, EventArgs e)
        {

        }

        private void AddIndicatorControl(int i)
        {
            FrameworkIndicators indCtl = (FrameworkIndicators)LoadControl("~/controls/FrameworkIndicators.ascx");

            LoadIndLocations(indCtl, 0, false);
            indCtl.ControlNumber = i + 1;
            indCtl.ID = "indicatorControlId" + i.ToString();
            pnlAdditionalIndicaotrs.Controls.Add(indCtl);
        }

        

        public int IndControlId
        {
            get
            {
                int indControlId = 0;
                if (ViewState["IndicatorControlId"] != null)
                {
                    int.TryParse(ViewState["IndicatorControlId"].ToString(), out indControlId);
                }

                return indControlId;
            }
            set
            {
                ViewState["IndicatorControlId"] = value.ToString();
            }
        }

        protected void btnBackToSRPList_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["b"] == "a")
            {
                Response.Redirect("~/ClusterLead/ActivityListing.aspx");
            }
            else
            {
                Response.Redirect("~/ClusterLead/IndicatorListing.aspx");
            }

        }

        private int NewIndicatorControlsAdded
        {
            get
            {
                int numberOfControls = 0;
                if (ViewState["NewIndicatorControlsAdded"] != null)
                {
                    int.TryParse(ViewState["NewIndicatorControlsAdded"].ToString(), out numberOfControls);
                }

                return numberOfControls;
            }
            set
            {
                ViewState["NewIndicatorControlsAdded"] = value;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<ActivityForAutoComplete> GetActivities(string cnId, string clId, string obId, string lngId, string searchTxt)
        {
            List<ActivityForAutoComplete> activities = new List<ActivityForAutoComplete>();
            if (searchTxt.Length < 150)
            {
                int emgLocId = 0;
                int.TryParse(cnId, out emgLocId);
                int emgClsId = 0;
                int.TryParse(clId, out emgClsId);
                int emgObjId = 0;
                int.TryParse(obId, out emgObjId);
                int langId = 0;
                int.TryParse(lngId, out langId);

                string[] split = null;
                split = searchTxt.Split(' ');
                string searchQuery = "";
                if (split.Length > 0)
                {
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (split[i].Length > 3)
                        {
                            string word = split[i];
                            word = word.Replace("'", "''");
                            if (searchQuery.Length > 0)
                            {
                                searchQuery += string.Format("OR ad.Activity like '{0}{1}{2}'", "%", word, "%");
                            }
                            else
                            {
                                searchQuery += string.Format("(ad.Activity like '{0}{1}{2}'", "%", word, "%");
                            }
                        }
                    }

                    if (searchQuery.Length > 0)
                    {
                        searchQuery += " )";
                    }
                    else
                    {
                        searchQuery = "ad.Activity like '%1-##2at#3f1%'";
                    }
                    DataTable dt = DBContext.GetData("GetActivitiesAutoComplete", new object[] { emgLocId, emgClsId, emgObjId, searchQuery, langId });
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ActivityForAutoComplete objAct = new ActivityForAutoComplete();
                        objAct.Activity = dt.Rows[i]["Activity"].ToString();
                        objAct.ActivityAlt = dt.Rows[i]["ActivityAlt"].ToString();

                        activities.Add(objAct);
                    }
                }
            }
            return activities;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool IsGenderDisaggregated(string unitId)
        {
            int unitIdToSearch = Convert.ToInt32(unitId);
            return RC.IsGenderUnit(unitIdToSearch);
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }



        //protected void MsRefugeeChange(object sender, EventArgs e)
        //{
        //    PopulateClusters();
        //}
    }

    public class ActivityForAutoComplete
    {
        public int ActivityId { get; set; }
        public string Activity { get; set; }
        public string ActivityAlt { get; set; }
    }

    public class TargetCountry
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
    }
}