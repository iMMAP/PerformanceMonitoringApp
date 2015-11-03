using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using System.Data;
using SRFROWCA.Configurations;
using System.Web.Services;
using System.Web.Script.Services;

namespace SRFROWCA.Controls
{
    public partial class FrameworkIndicators : System.Web.UI.UserControl
    {
        public int Return { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl1stNumber.Text = " " + (ControlNumber).ToString();
            if (string.IsNullOrEmpty(hfIndicatorId.Value) || hfIndicatorId.Value == "0")
            {
                PopulateUnits(true);
            }

            string key = this.indCtlEmgLocId.ToString() + this.indCtlEmgClusterId.ToString();
            AdminTargetSettingItems items = RC.AdminTargetSettings(key);
            if (items.IsTarget)
            {
                UserControl ctl = null;
                if (items.AdminLevel == RC.LocationTypes.National)
                {
                    ctl = (ctlCountryTargets)LoadControl("~/controls/ctlCountryTargets.ascx");
                    ((ctlCountryTargets)ctl).EmgLocId = this.indCtlEmgLocId;
                    ((ctlCountryTargets)ctl).IndicatorId = this.indCtlIndicatorId;
                    ((ctlCountryTargets)ctl).IsGender = this.indCtlIsGender;
                    ((ctlCountryTargets)ctl).ID = "AdminTargetControl";
                }
                else if (items.AdminLevel == RC.LocationTypes.Governorate)
                {
                    ctl = (ctlAdmin1Targets)LoadControl("~/controls/ctlAdmin1Targets.ascx");
                    ((ctlAdmin1Targets)ctl).EmgLocId = this.indCtlEmgLocId;
                    ((ctlAdmin1Targets)ctl).IndicatorId = this.indCtlIndicatorId;
                    ((ctlAdmin1Targets)ctl).Category = items.Category;
                    ((ctlAdmin1Targets)ctl).IsGender = this.indCtlIsGender;
                    ((ctlAdmin1Targets)ctl).ID = "AdminTargetControl";
                }
                else if (items.AdminLevel == RC.LocationTypes.District)
                {
                    ctl = (ctlAdmin2Targets)LoadControl("~/controls/ctlAdmin2Targets.ascx");
                    ((ctlAdmin2Targets)ctl).EmgLocId = this.indCtlEmgLocId;
                    ((ctlAdmin2Targets)ctl).IndicatorId = this.indCtlIndicatorId;
                    ((ctlAdmin2Targets)ctl).Category = items.Category;
                    ((ctlAdmin2Targets)ctl).IsGender = this.indCtlIsGender;
                    ((ctlAdmin2Targets)ctl).ID = "AdminTargetControl";
                }

                if (ctl != null)
                    pnlTargets.Controls.Add(ctl);
            }
        }

        public void PopulateUnits(bool selectIndex)
        {
            ddlUnit.DataValueField = "UnitId";
            ddlUnit.DataTextField = "Unit";

            ddlUnit.DataSource = GetUnits();
            ddlUnit.DataBind();

            if (selectIndex)
            {
                ListItem item = new ListItem("Select Unit", "0");
                ddlUnit.Items.Insert(0, item);
                ddlUnit.SelectedIndex = 0;
            }
        }
        private object GetUnits()
        {
            return DBContext.GetData("GetAllUnits", new object[] { RC.SelectedSiteLanguageId });
        }

        public void SaveIndicators(int activityId)
        {
            SaveIndicator(activityId);
        }

        private void SaveTargets(int indicatorId, bool IsGender)
        {
            string key = this.indCtlEmgLocId.ToString() + this.indCtlEmgClusterId.ToString();
            AdminTargetSettingItems items = RC.AdminTargetSettings(key);
            foreach (Control ctlTareget in pnlTargets.Controls)
            {
                if (ctlTareget != null && ctlTareget.ID != null && ctlTareget.ID.Contains("AdminTargetControl"))
                {
                    string ctlType = ctlTareget.GetType().ToString();
                    //if (ctlTareget.GetType() == ctlCountryTargets)
                    if (items.AdminLevel == RC.LocationTypes.National)
                    {
                        ctlCountryTargets countryCtl = ctlTareget as ctlCountryTargets;
                        if (countryCtl != null)
                        {
                            if (IsGender)
                                SaveCountryTargetsGender(countryCtl.rptCountryGender, indicatorId);
                            else
                                SaveCountryTargets(countryCtl.rptCountry, indicatorId);
                        }
                    }
                    else if (items.AdminLevel == RC.LocationTypes.Governorate)
                    {
                        ctlAdmin1Targets adm1Ctl = ctlTareget as ctlAdmin1Targets;
                        if (adm1Ctl != null)
                        {
                            if (IsGender)
                                SaveAdmin1TargetGender(adm1Ctl.rptCountryGender, indicatorId);
                            else
                                SaveAdmin1Target(adm1Ctl.rptCountry, indicatorId);
                        }
                    }
                    else if (items.AdminLevel == RC.LocationTypes.District)
                    {
                        ctlAdmin2Targets adm2Ctl = ctlTareget as ctlAdmin2Targets;
                        if (adm2Ctl != null)
                        {
                            if (IsGender)
                                SaveAdmin2TargetGender(adm2Ctl.rptCountryGender, indicatorId);
                            else
                                SaveAdmin2Target(adm2Ctl.rptCountry, indicatorId);
                        }
                    }
                }
            }
        }

        private void SaveAdmin2Target(Repeater repeater, int indicatorId)
        {
            int insertCount = 1;
            foreach (RepeaterItem item in repeater.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    int? countryTarget = null;
                    TextBox txtCountryTarget = item.FindControl("txtTarget") as TextBox;
                    var hfCountryId = (HiddenField)item.FindControl("hfCountryId");
                    int countryId = Convert.ToInt32(hfCountryId.Value);
                    if (txtCountryTarget != null)
                    {
                        countryTarget = string.IsNullOrEmpty(txtCountryTarget.Text.Trim()) ? (int?)null : Convert.ToInt32(txtCountryTarget.Text.Trim());

                    }

                    Repeater rptAdmin1 = item.FindControl("rptAdmin1") as Repeater;
                    if (rptAdmin1 != null)
                    {
                        foreach (RepeaterItem rptAdmin1Item in rptAdmin1.Items)
                        {
                            if (rptAdmin1Item.ItemType == ListItemType.Item || rptAdmin1Item.ItemType == ListItemType.AlternatingItem)
                            {
                                int? admin1Target = null;
                                TextBox txtAdmin1Target = rptAdmin1Item.FindControl("txtTarget") as TextBox;
                                var hfAdmin1Id = (HiddenField)rptAdmin1Item.FindControl("hfAdmin1Id");
                                int admin1LocationId = Convert.ToInt32(hfAdmin1Id.Value);
                                if (txtAdmin1Target != null)
                                {
                                    admin1Target = string.IsNullOrEmpty(txtAdmin1Target.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin1Target.Text.Trim());
                                }

                                Repeater rptAdmin2 = rptAdmin1Item.FindControl("rptAdmin2") as Repeater;
                                if (rptAdmin2 != null)
                                {
                                    foreach (RepeaterItem rptAdmin2Item in rptAdmin2.Items)
                                    {
                                        if (rptAdmin2Item.ItemType == ListItemType.Item || rptAdmin2Item.ItemType == ListItemType.AlternatingItem)
                                        {
                                            int? admin2Target = null;
                                            TextBox txtAdmin2Target = rptAdmin2Item.FindControl("txtTarget") as TextBox;
                                            var hfAdmin2Id = (HiddenField)rptAdmin2Item.FindControl("hfAdmin2Id");
                                            int admin2LocationId = Convert.ToInt32(hfAdmin2Id.Value);
                                            if (txtAdmin2Target != null)
                                            {
                                                admin2Target = string.IsNullOrEmpty(txtAdmin2Target.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin2Target.Text.Trim());
                                                if (admin2Target != null)
                                                {
                                                    DBContext.Update("InsertIndicatorTarget", new object[] { indicatorId, Convert.ToInt32(hfCountryId.Value),
                                                                                                            admin1LocationId, admin2LocationId, admin2Target,
                                                                                                            insertCount, 4, RC.GetCurrentUserId, DBNull.Value });
                                                    insertCount++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }

            DBContext.Update("UpdateCountryAndAdmin1TargetsOfIndicator", new object[] { indicatorId, DBNull.Value });
        }

        private void SaveAdmin2TargetGender(Repeater repeater, int indicatorId)
        {
            int insertCount = 1;
            foreach (RepeaterItem item in repeater.Items)
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
                        foreach (RepeaterItem rptAdmin1Item in rptAdmin1.Items)
                        {
                            if (rptAdmin1Item.ItemType == ListItemType.Item || rptAdmin1Item.ItemType == ListItemType.AlternatingItem)
                            {
                                HiddenField hfAdmin1Id = rptAdmin1Item.FindControl("hfAdmin1Id") as HiddenField;
                                int admin1Id = 0;
                                if (hfAdmin1Id != null)
                                    int.TryParse(hfAdmin1Id.Value, out admin1Id);

                                Repeater rptAdmin2 = rptAdmin1Item.FindControl("rptAdmin2") as Repeater;
                                if (rptAdmin2 != null)
                                {
                                    foreach (RepeaterItem rptAdmin2Item in rptAdmin2.Items)
                                    {
                                        if (rptAdmin2Item.ItemType == ListItemType.Item || rptAdmin2Item.ItemType == ListItemType.AlternatingItem)
                                        {
                                            TextBox txtAdmin2Male = rptAdmin2Item.FindControl("txtTargetMale") as TextBox;
                                            TextBox txtAdmin2Female = rptAdmin2Item.FindControl("txtTargetFemale") as TextBox;
                                            int? admin2MaleTarget = null;
                                            if (txtAdmin2Male != null)
                                                admin2MaleTarget = string.IsNullOrEmpty(txtAdmin2Male.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin2Male.Text.Trim());

                                            int? admin2FemaleTarget = null;
                                            if (txtAdmin2Female != null)
                                                admin2FemaleTarget = string.IsNullOrEmpty(txtAdmin2Female.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin2Female.Text.Trim());

                                            HiddenField hfAdmin2Id = rptAdmin2Item.FindControl("hfAdmin2Id") as HiddenField;
                                            int admin2Id = 0;
                                            if (hfAdmin2Id != null)
                                                int.TryParse(hfAdmin2Id.Value, out admin2Id);
                                            if (admin2MaleTarget != null || admin2FemaleTarget != null)
                                            {
                                                DBContext.Update("InsertIndicatorTargetGender", new object[] { indicatorId, countryId, admin1Id , 
                                                                                                                admin2Id, admin2MaleTarget, admin2FemaleTarget,
                                                                                                                insertCount, 4, RC.GetCurrentUserId, DBNull.Value });
                                                insertCount++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }

            DBContext.Update("UpdateCountryAndAdmin1TargetsOfIndicatorGender", new object[] { indicatorId, DBNull.Value });
        }

        private void SaveAdmin1Target(Repeater repeater, int indicatorId)
        {
            int insertCount = 1;
            foreach (RepeaterItem item in repeater.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtCountryTarget = item.FindControl("txtTarget") as TextBox;
                    var hfCountryId = (HiddenField)item.FindControl("hfCountryId");
                    int countryId = Convert.ToInt32(hfCountryId.Value);

                    Repeater rptAdmin1 = item.FindControl("rptAdmin1") as Repeater;
                    if (rptAdmin1 != null)
                    {
                        foreach (RepeaterItem rptAdmin1Item in rptAdmin1.Items)
                        {
                            if (rptAdmin1Item.ItemType == ListItemType.Item || rptAdmin1Item.ItemType == ListItemType.AlternatingItem)
                            {
                                int? admin1Target = null;
                                TextBox txtAdmin1Target = rptAdmin1Item.FindControl("txtTarget") as TextBox;
                                var hfAdmin1Id = (HiddenField)rptAdmin1Item.FindControl("hfAdmin1Id");
                                int admin1LocationId = Convert.ToInt32(hfAdmin1Id.Value);
                                if (txtAdmin1Target != null)
                                {
                                    admin1Target = string.IsNullOrEmpty(txtAdmin1Target.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin1Target.Text.Trim());
                                    if (admin1Target != null)
                                    {
                                        DBContext.Update("InsertIndicatorTarget_Admin1", new object[] { indicatorId, countryId, admin1LocationId, admin1Target,
                                                                                                            insertCount, 3, RC.GetCurrentUserId, DBNull.Value });
                                        insertCount++;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            
            DBContext.Update("UpdateCountryAndAdmin1TargetsOfIndicator_Admin1", new object[] { indicatorId, DBNull.Value });
        }

        private void SaveAdmin1TargetGender(Repeater repeater, int indicatorId)
        {
            int insertCount = 1;
            foreach (RepeaterItem item in repeater.Items)
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
                        foreach (RepeaterItem rptAdmin1Item in rptAdmin1.Items)
                        {
                            if (rptAdmin1Item.ItemType == ListItemType.Item || rptAdmin1Item.ItemType == ListItemType.AlternatingItem)
                            {
                                HiddenField hfAdmin1Id = rptAdmin1Item.FindControl("hfAdmin1Id") as HiddenField;
                                int admin1Id = 0;
                                if (hfAdmin1Id != null)
                                    int.TryParse(hfAdmin1Id.Value, out admin1Id);

                                int? admin1MaleTarget = null;
                                int? admin1FemaleTarget = null;
                                TextBox txtTargetMale = rptAdmin1Item.FindControl("txtTargetMale") as TextBox;
                                TextBox txtTargetFemale = rptAdmin1Item.FindControl("txtTargetFemale") as TextBox;
                                if (txtTargetMale != null || txtTargetFemale != null)
                                {
                                    admin1MaleTarget = string.IsNullOrEmpty(txtTargetMale.Text.Trim()) ? (int?)null : Convert.ToInt32(txtTargetMale.Text.Trim());
                                    admin1FemaleTarget = string.IsNullOrEmpty(txtTargetFemale.Text.Trim()) ? (int?)null : Convert.ToInt32(txtTargetFemale.Text.Trim());
                                    if (admin1MaleTarget != null || admin1FemaleTarget != null)
                                    {
                                        DBContext.Update("InsertIndicatorTargetGender_Admin1", new object[] { indicatorId, countryId, admin1Id , 
                                                                                                                admin1MaleTarget, admin1FemaleTarget,
                                                                                                                insertCount, 3, RC.GetCurrentUserId, DBNull.Value });
                                    }
                                    insertCount++;

                                }
                            }
                        }
                    }

                }
            }
            DBContext.Update("UpdateCountryAndAdmin1TargetsOfIndicatorGender_Admin1", new object[] { indicatorId, DBNull.Value });
        }

        private void SaveCountryTargets(Repeater repeater, int indicatorId)
        {
            int insertCount = 1;
            foreach (RepeaterItem item in repeater.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    int? countryTarget = null;
                    TextBox txtCountryTarget = item.FindControl("txtTarget") as TextBox;
                    var hfCountryId = (HiddenField)item.FindControl("hfCountryId");
                    int countryId = Convert.ToInt32(hfCountryId.Value);
                    if (txtCountryTarget != null)
                    {
                        countryTarget = string.IsNullOrEmpty(txtCountryTarget.Text.Trim()) ? (int?)null : Convert.ToInt32(txtCountryTarget.Text.Trim());
                        if (countryTarget != null)
                        {
                            DBContext.Update("InsertIndicatorTarget_Country", new object[] { indicatorId, countryId, countryTarget,
                                                                                                         2,   insertCount, RC.GetCurrentUserId, DBNull.Value });
                            insertCount++;

                        }
                    }
                }
            }
        }

        private void SaveCountryTargetsGender(Repeater repeater, int indicatorId)
        {
            int insertCount = 1;
            foreach (RepeaterItem item in repeater.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfCountryId = item.FindControl("hfCountryId") as HiddenField;
                    int countryId = 0;
                    if (hfCountryId != null)
                        int.TryParse(hfCountryId.Value, out countryId);
                    int? countryMaleTarget = null;
                    int? countryFemaleTarget = null;
                    TextBox txtTargetMale = item.FindControl("txtTargetMale") as TextBox;
                    TextBox txtTargetFemale = item.FindControl("txtTargetFemale") as TextBox;
                    if (txtTargetMale != null || txtTargetFemale != null)
                    {
                        countryMaleTarget = string.IsNullOrEmpty(txtTargetMale.Text.Trim()) ? (int?)null : Convert.ToInt32(txtTargetMale.Text.Trim());
                        countryFemaleTarget = string.IsNullOrEmpty(txtTargetFemale.Text.Trim()) ? (int?)null : Convert.ToInt32(txtTargetFemale.Text.Trim());
                        if (countryMaleTarget != null || countryFemaleTarget != null)
                        {
                            DBContext.Update("InsertIndicatorTargetGender_Country", new object[] { indicatorId, countryId, countryMaleTarget, 
                                                                                        countryFemaleTarget, insertCount, 2, RC.GetCurrentUserId, DBNull.Value });
                        }
                        insertCount++;

                    }
                }
            }
        }

        private void SaveIndicator(int activityId)
        {
            int unitId = 0;
            int.TryParse(ddlUnit.SelectedValue, out unitId);
            string indEn = !string.IsNullOrEmpty(txtInd1Eng.Text.Trim()) ? txtInd1Eng.Text.Trim() : txtInd1Fr.Text.Trim();
            string indFr = !string.IsNullOrEmpty(txtInd1Fr.Text.Trim().Trim()) ? txtInd1Fr.Text.Trim() : txtInd1Eng.Text.Trim();
            Guid userId = RC.GetCurrentUserId;
            bool isGender = RC.IsGenderUnit(unitId);
            int val = RC.GetSelectedIntVal(ddlCalculationMethod);
            int? calMethod = val > 0 ? val : (int?)val;

            if (string.IsNullOrEmpty(hfIndicatorId.Value))
            {
                int indicatorId = DBContext.Add("InsertIndicator", new object[] { activityId, indEn, indFr, 
                                                                                    unitId, userId, isGender, 
                                                                                    calMethod, DBNull.Value });
                SaveTargets(indicatorId, isGender);
            }
            else
            {
                int indicatorId = 0;
                int.TryParse(hfIndicatorId.Value, out indicatorId);
                if (indicatorId > 0)
                {
                    DBContext.Update("UpdateIndicatorNew2", new object[] { indicatorId, activityId, 
                                                                            unitId, indEn, indFr, userId, 
                                                                            isGender, calMethod, DBNull.Value });
                    SaveTargets(indicatorId, isGender);
                }
                else
                {
                    int newIndicatorId = DBContext.Add("InsertIndicator", new object[] { activityId, indEn, indFr, 
                                                                                            unitId, userId, isGender, 
                                                                                            calMethod, DBNull.Value });
                    SaveTargets(newIndicatorId, isGender);
                }
            }
        }


        private int? GetAdminTarget(RepeaterItem item, string controlName)
        {
            int? reportedValue = null;
            TextBox txt = item.FindControl(controlName) as TextBox;
            if (!string.IsNullOrEmpty(txt.Text.Trim()))
            {
                reportedValue = Convert.ToInt32(txt.Text.Trim());
            }

            return reportedValue;
        }

        public int ControlNumber { get; set; }

        public int indCtlEmgLocId { get; set; }
        public int indCtlEmgClusterId { get; set; }
        public int indCtlIndicatorId { get; set; }
        public int ClusterFrameworkLocCatId { get; set; }
        public bool indCtlIsGender { get; set; }
    }


}