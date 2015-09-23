using BusinessLogic;
using SRFROWCA.Common;
using SRFROWCA.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
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

            if (NewIndicatorControlsAdded < 1)
            {
                btnRemoveIndicatorControl.Visible = false;
            }
            else
            {
                btnRemoveIndicatorControl.Visible = true;
            }

            for (int i = 0; i < IndControlId; i++)
            {
                AddIndicatorControl(i);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCombos();
                DisableDropDowns();

                if (Request.QueryString["a"] != null)
                {
                    LoadActivityData();
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

                FrameWorkSettingsCount frCount = FrameWorkUtil.GetActivityFrameworkSettings(emgLocationId, emgClusterId);
                if (frCount.IndCount <= 0)
                    btnAddIndicatorControl.Visible = false;
                else
                    btnAddIndicatorControl.Visible = true;
            }
        }

        private void LoadActivityData()
        {
            int activityId = 0;
            if (Request.QueryString["a"] != null)
            {
                int.TryParse(Request.QueryString["a"].ToString(), out activityId);
            }

            if (activityId > 0)
            {
                DataTable dt = GetActivityIndicators(activityId);
                if (dt.Rows.Count > 0)
                {
                    int count = dt.Rows.Count;
                    txtActivityEng.Text = dt.Rows[0]["ActivityEng"].ToString();
                    txtActivityFr.Text = dt.Rows[0]["ActivityFr"].ToString();

                    int emgLocationId1 = 0;
                    int.TryParse(dt.Rows[0]["EmergencyLocationId"].ToString(), out emgLocationId1);
                    if (emgLocationId1 > 0)
                    {
                        ddlCountry.SelectedValue = emgLocationId1.ToString();
                    }
                    int emgClusterId = 0;
                    int.TryParse(dt.Rows[0]["EmergencyClusterId"].ToString(), out emgClusterId);
                    if (emgClusterId > 0)
                    {
                        ddlCluster.SelectedValue = emgClusterId.ToString();
                    }

                    int emgObjectiveId = 0;
                    int.TryParse(dt.Rows[0]["EmergencyObjectiveId"].ToString(), out emgObjectiveId);
                    if (emgObjectiveId > 0)
                    {
                        ddlObjective.SelectedValue = emgObjectiveId.ToString();
                    }

                    for (int i = 0; i < count; i++)
                    {
                        IndicatorsWithAdmin1TargetsControl indicatorCtl = (IndicatorsWithAdmin1TargetsControl)LoadControl("~/controls/IndicatorsWithAdmin1TargetsControl.ascx");
                        int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
                        indicatorCtl.EmgLocationId = emgLocationId;
                        indicatorCtl.ControlNumber = i + 1;
                        indicatorCtl.ID = "indicatorControlId" + i.ToString();

                        indicatorCtl.txtInd1Eng.Text = dt.Rows[i]["IndicatorEng"].ToString();
                        indicatorCtl.txtInd1Fr.Text = dt.Rows[i]["IndicatorFr"].ToString();
                        int unitId = 0;
                        int.TryParse(dt.Rows[i]["UnitId"].ToString(), out unitId);
                        if (unitId > 0)
                        {
                            indicatorCtl.PopulateUnits(false);
                            indicatorCtl.ddlUnit.SelectedValue = unitId.ToString();
                        }

                        bool isGender = false;
                        int calMethod = 0;
                        int.TryParse(dt.Rows[i]["IndicatorCalculationTypeId"].ToString(), out calMethod);
                        if (calMethod > 0)
                        {
                            indicatorCtl.ddlCalculationMethod.SelectedValue = calMethod.ToString();
                        }

                        int indicatorId = 0;
                        int.TryParse(dt.Rows[i]["IndicatorId"].ToString(), out indicatorId);
                        indicatorCtl.hfIndicatorId.Value = indicatorId.ToString();

                        if (unitId == 269 || unitId == 28 || unitId == 38 || unitId == 193
                         || unitId == 219 || unitId == 198 || unitId == 311 || unitId == 287
                         || unitId == 67 || unitId == 132 || unitId == 252)
                        {
                            isGender = true;
                        }
                        DataTable dtTargets = GetCountryIndTarget(emgLocationId, indicatorId);
                        indicatorCtl.rptCountry.DataSource = dtTargets;
                        indicatorCtl.rptCountry.DataBind();
                        indicatorCtl.rptCountryGender.DataSource = dtTargets;
                        indicatorCtl.rptCountryGender.DataBind();
                        if (!isGender)
                            UpdateRepeaterTargetColumn(indicatorCtl.rptCountryGender);
                        else
                            UpdateRepeaterTargetColumn(indicatorCtl.rptCountry);

                        pnlAdditionalIndicaotrs.Controls.Add(indicatorCtl);
                        IndControlId += 1;
                    }
                }
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


        //private DataTable GetAdmin1Location(int emgLocationId, int indicatorId)
        //{
        //    return DBContext.GetData("GetAdmin1LocationsOfCountry", new object[] { emgLocationId });
        //}

        private DataTable GetCountryIndTarget(int emgLocationId, int indicatorId)
        {
            return DBContext.GetData("GetCountryTargetOfIndicator", new object[] { emgLocationId, indicatorId });
        }
        private DataTable GetAdminIndTargets(int emgLocationId, int indicatorId)
        {
            return DBContext.GetData("GetAllAdmin1AndIndicatorTargets", new object[] { indicatorId, emgLocationId });
        }

        private DataTable GetActivityIndicators(int activityId)
        {
            int active = 1;
            return DBContext.GetData("GetActivityIndicators", new object[] { activityId, active, RC.SelectedSiteLanguageId });
        }

        private void LoadCombos()
        {
            PopulateObjective();
            PopulateClusters();
            PopulateCountries();

            SetComboValues();
        }

        internal override void BindGridData()
        {
            //PopulateClusters();
            //PopulateObjective();

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
        }

        private void PopulateClusters()
        {
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            ListItem item = new ListItem("Select Cluster", "0");
            ddlCluster.Items.Insert(0, item);
        }

        private void PopulateCountries()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            ListItem item = new ListItem("Select Country", "0");
            ddlCountry.Items.Insert(0, item);
        }

        private void PopulateObjective()
        {
            ddlObjective.DataSource = DBContext.GetData("GetEmergencyObjectives", new object[] { RC.SelectedSiteLanguageId, RC.EmergencySahel2015 });
            ddlObjective.DataTextField = "Objective";
            ddlObjective.DataValueField = "EmergencyObjectiveId";
            ddlObjective.DataBind();
            ListItem item = new ListItem("Select Objective", "0");
            ddlObjective.Items.Insert(0, item);
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            int countryId = RC.GetSelectedIntVal(ddlCountry);
            foreach (Control ctl in pnlAdditionalIndicaotrs.Controls)
            {
                if (ctl != null && ctl.ID != null && ctl.ID.Contains("indicatorControlId"))
                {
                    IndicatorsWithAdmin1TargetsControl indicatorCtl = ctl as IndicatorsWithAdmin1TargetsControl;
                    indicatorCtl.EmgLocationId = countryId;
                    DataTable dtTargets = GetCountryIndTarget(countryId, 0);
                    indicatorCtl.rptCountry.DataSource = dtTargets;
                    indicatorCtl.rptCountry.DataBind();
                    indicatorCtl.rptCountryGender.DataSource = dtTargets;
                    indicatorCtl.rptCountryGender.DataBind();
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

            //using (TransactionScope scope = new TransactionScope())
            {
                SaveData();
                //scope.Complete();
                Response.Redirect("~/ClusterLead/IndicatorListing.aspx");
            }
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        private bool IsTargetProvided()
        {
            bool isTargetValid = true;

            foreach (Control ctl in pnlAdditionalIndicaotrs.Controls)
            {
                if (ctl != null && ctl.ID != null && ctl.ID.Contains("indicatorControlId"))
                {
                    IndicatorsWithAdmin1TargetsControl indControl = ctl as IndicatorsWithAdmin1TargetsControl;

                    if (indControl != null)
                    {
                        if (indControl.ddlUnit.SelectedValue == "269" || indControl.ddlUnit.SelectedValue == "28"
                            || indControl.ddlUnit.SelectedValue == "38" || indControl.ddlUnit.SelectedValue == "193"
                            || indControl.ddlUnit.SelectedValue == "219" || indControl.ddlUnit.SelectedValue == "198"
                            || indControl.ddlUnit.SelectedValue == "311" || indControl.ddlUnit.SelectedValue == "132"
                            || indControl.ddlUnit.SelectedValue == "252")
                            isTargetValid = TargetProvided(indControl.rptCountryGender, true);
                        else
                            isTargetValid = TargetProvided(indControl.rptCountry, false);
                    }
                }
            }

            return isTargetValid;
        }

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
                            IndicatorsWithAdmin1TargetsControl indControl = ctl as IndicatorsWithAdmin1TargetsControl;

                            if (indControl != null)
                            {
                                //TextBox txtEng = (TextBox)indControl.FindControl("txtInd1Eng");
                                //TextBox txtFr = (TextBox)indControl.FindControl("txtInd1Fr");

                                //if (!string.IsNullOrEmpty(txtEng.Text.Trim()) || !string.IsNullOrEmpty(txtFr.Text.Trim()))
                                {
                                    indControl.SaveIndicators(ActivityId);
                                    //strIndcators.AppendFormat("Indicator (English): {0}<br/>Indicator (French): {1}<br/><br/>", txtEng.Text, txtFr.Text);
                                }
                            }
                        }
                    }
                    SendNewIndicatorEmail(strIndcators.ToString());
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
                    IndicatorsWithAdmin1TargetsControl indControl = ctl as IndicatorsWithAdmin1TargetsControl;

                    if (indControl != null)
                    {
                        //TextBox txtEng = (TextBox)indControl.FindControl("txtInd1Eng");
                        //TextBox txtFr = (TextBox)indControl.FindControl("txtInd1Fr");

                        //if (!string.IsNullOrEmpty(txtEng.Text.Trim()) || !string.IsNullOrEmpty(txtFr.Text.Trim()))
                        {
                            indControl.SaveIndicators(activityId);
                        }
                    }
                }
            }
        }

        private void UpdateActivity(int activityId)
        {
            int emgLocationId = Convert.ToInt32(ddlCountry.SelectedValue);
            int emgClusterId = Convert.ToInt32(ddlCluster.SelectedValue);
            int emgObjectiveId = Convert.ToInt32(ddlObjective.SelectedValue);
            string actEn = !string.IsNullOrEmpty(txtActivityEng.Text.Trim()) ? txtActivityEng.Text.Trim() : null;
            string actFr = !string.IsNullOrEmpty(txtActivityFr.Text.Trim()) ? txtActivityFr.Text.Trim() : null;

            if (!string.IsNullOrEmpty(actFr) || !string.IsNullOrEmpty(actEn))
            {
                DBContext.Update("UpdateActivtiyNew2", new object[] { activityId, emgLocationId, emgClusterId, emgObjectiveId, actEn, actFr, RC.GetCurrentUserId, DBNull.Value });
            }
        }


        private void SendNewIndicatorEmail(string strIndicators)
        {
            try
            {
                using (MailMessage mailMsg = new MailMessage())
                {
                    mailMsg.From = new MailAddress("orsocharowca@gmail.com");
                    mailMsg.To.Add("orsocharowca@gmail.com");
                    mailMsg.Subject = "Framework 2015 - New Indicators Has Been Added in ORS Master List";
                    mailMsg.IsBodyHtml = true;
                    mailMsg.Body = string.Format(@"New Indicators Has Been Added in ORS For 2015 Framework<hr/>
                                                {0}<br/>                                              
                                                <b>By Following User:</b><br/>                                                        
                                                        <b>User Name:</b> {1}<br/>
                                                        <b>Email:</b> {2}<br/>                                                        
                                                        <b>Phone:</b> {3}<b/>"
                                                            , strIndicators, Membership.GetUser().UserName, Membership.GetUser().Email, "");
                    Mail.SendMail(mailMsg);
                }
            }
            catch
            {

            }
        }

        private int SaveActivity()
        {
            int emergencyId = RC.SelectedEmergencyId;
            int clusterId = RC.IsClusterLead(this.User) ? UserInfo.Cluster : Convert.ToInt32(ddlCluster.SelectedValue);
            int emergencyLocationId = RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User) ? UserInfo.EmergencyCountry : Convert.ToInt32(ddlCountry.SelectedValue);
            int emergencyClusterId = RC.IsClusterLead(this.User) ? UserInfo.EmergencyCluster : Convert.ToInt32(ddlCluster.SelectedValue);

            if (emergencyClusterId == 0)
            {
                if (Request.QueryString["cid"] != null)
                {
                    int.TryParse(Request.QueryString["cid"], out emergencyClusterId);
                }
            }

            Guid userId = RC.GetCurrentUserId;

            int objId = RC.GetSelectedIntVal(ddlObjective);
            string actEn = !string.IsNullOrEmpty(txtActivityEng.Text.Trim()) ? txtActivityEng.Text.Trim() : null;
            string actFr = !string.IsNullOrEmpty(txtActivityFr.Text.Trim()) ? txtActivityFr.Text.Trim() : null;
            int yearId = 12;
            return DBContext.Add("InsertActivityNew", new object[] { emergencyClusterId, objId,emergencyLocationId, 
                                                                        actEn, actFr, userId, yearId, RC.SelectedSiteLanguageId, DBNull.Value });
        }

        protected void btnAddIndiatorControl_Click(object sender, EventArgs e)
        {

        }

        private void AddIndicatorControl(int i)
        {
            IndicatorsWithAdmin1TargetsControl newIndSet = (IndicatorsWithAdmin1TargetsControl)LoadControl("~/controls/IndicatorsWithAdmin1TargetsControl.ascx");
            int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
            if (emgLocationId > 0)
            {
                int indicatorId = 0;
                DataTable dt = GetCountryIndTarget(emgLocationId, indicatorId);
                newIndSet.rptCountry.DataSource = dt;
                newIndSet.rptCountry.DataBind();
                newIndSet.rptCountryGender.DataSource = dt;
                newIndSet.rptCountryGender.DataBind();
            }
            newIndSet.ControlNumber = i + 1;
            newIndSet.ID = "indicatorControlId" + i.ToString();
            pnlAdditionalIndicaotrs.Controls.Add(newIndSet);
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