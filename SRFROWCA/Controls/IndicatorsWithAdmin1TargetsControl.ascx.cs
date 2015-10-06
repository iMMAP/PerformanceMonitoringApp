using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using System.Data;

namespace SRFROWCA.Controls
{
    public partial class IndicatorsWithAdmin1TargetsControl : System.Web.UI.UserControl
    {
        public int Return { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl1stNumber.Text = " " + (ControlNumber).ToString();
            if (string.IsNullOrEmpty(hfIndicatorId.Value) || hfIndicatorId.Value == "0")
            {
                PopulateUnits(true);
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
        public void SaveRegionalIndicators(int priorityActivityId, bool regional)
        {
            SaveRegionalIndicator(priorityActivityId, regional);
        }

        private void SaveIndicator(int activityId)
        {
            int unitId = 0;
            int.TryParse(ddlUnit.SelectedValue, out unitId);
            string indEn = !string.IsNullOrEmpty(txtInd1Eng.Text.Trim()) ? txtInd1Eng.Text.Trim() : txtInd1Fr.Text.Trim();
            string indFr = !string.IsNullOrEmpty(txtInd1Fr.Text.Trim().Trim()) ? txtInd1Fr.Text.Trim() : txtInd1Eng.Text.Trim();
            Guid userId = RC.GetCurrentUserId;
            int gender = 0;
            int val = RC.GetSelectedIntVal(ddlCalculationMethod);
            int? calMethod = val > 0 ? val : (int?)val;

            if (string.IsNullOrEmpty(hfIndicatorId.Value))
            {
                int indicatorId = DBContext.Add("InsertIndicator", new object[] { activityId, indEn, indFr, 
                                                                                    unitId, userId, gender, 
                                                                                    calMethod, DBNull.Value });
                if (indicatorId > 0)
                {
                    if (unitId == 269 || unitId == 28 || unitId == 38 || unitId == 193
                         || unitId == 219 || unitId == 198 || unitId == 311 || unitId == 287
                         || unitId == 67 || unitId == 132 || unitId == 252 || unitId == 238)
                        SaveAdmin2GenderTargets(indicatorId);
                    else
                        SaveAdmin2Targets(indicatorId);
                }
            }
            else
            {
                int indicatorId = 0;
                int.TryParse(hfIndicatorId.Value, out indicatorId);
                if (indicatorId > 0)
                {
                    DBContext.Update("UpdateIndicatorNew2", new object[] { indicatorId, activityId, 
                                                                            unitId, indEn, indFr, userId, 
                                                                            gender, calMethod, DBNull.Value });
                    if (unitId == 269 || unitId == 28 || unitId == 38 || unitId == 193
                         || unitId == 219 || unitId == 198 || unitId == 311 || unitId == 287
                         || unitId == 67 || unitId == 132 || unitId == 252 || unitId == 238)
                        SaveAdmin2GenderTargets(indicatorId);
                    else
                        SaveAdmin2Targets(indicatorId);
                }
                else
                {
                    int newIndicatorId = DBContext.Add("InsertIndicator", new object[] { activityId, indEn, indFr, 
                                                                                            unitId, userId, gender, 
                                                                                            calMethod, DBNull.Value });
                    if (newIndicatorId > 0)
                    {
                        if (unitId == 269 || unitId == 28 || unitId == 38 || unitId == 193
                         || unitId == 219 || unitId == 198 || unitId == 311 || unitId == 287
                         || unitId == 67 || unitId == 132 || unitId == 252 || unitId == 238)
                            SaveAdmin2GenderTargets(indicatorId);
                        else
                            SaveAdmin2Targets(indicatorId);
                    }
                }
            }
        }
        private void SaveAdmin2Targets(int indicatorId)
        {
            int insertCount = 1;
            foreach (RepeaterItem item in rptCountry.Items)
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
                                                                                                            insertCount, RC.GetCurrentUserId, DBNull.Value });
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

        private void SaveAdmin2GenderTargets(int indicatorId)
        {
            int insertCount = 1;
            foreach (RepeaterItem item in rptCountryGender.Items)
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
                                                                                                                insertCount, RC.GetCurrentUserId, DBNull.Value });
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

        private void SaveRegionalIndicator(int ActivityId, bool regional)
        {
            int unitId = Convert.ToInt32(ddlUnit.SelectedValue);// RC.GetSelectedIntVal(ddlUnitsInd1);
            string indEn = txtInd1Eng.Text.Trim();
            string indFr = txtInd1Fr.Text.Trim();
            Guid userId = RC.GetCurrentUserId;
            bool isSRP = regional;


            int indicatorId = DBContext.Add("InsertIndicator", new object[] { ActivityId, indEn, indFr, 
                                                                unitId, userId, DBNull.Value});
            if (indicatorId > 0)
            {
                SaveAdmin2Targets(indicatorId);

            }
        }


        public int ControlNumber { get; set; }

        protected void rptCountry_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int countryId = 0;
                HiddenField hfCountryId = e.Item.FindControl("hfCountryId") as HiddenField;
                if (hfCountryId != null)
                {
                    int.TryParse(hfCountryId.Value, out countryId);
                }
                if (countryId > 0)
                {
                    Repeater rptAdmin1 = e.Item.FindControl("rptAdmin1") as Repeater;
                    LoadAdmin1Targets(rptAdmin1, countryId);
                }
            }
        }

        protected void rptAdmin1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int admin1Id = 0;
                HiddenField hfAdmin1Id = e.Item.FindControl("hfAdmin1Id") as HiddenField;
                if (hfAdmin1Id != null)
                {
                    int.TryParse(hfAdmin1Id.Value, out admin1Id);
                }
                if (admin1Id > 0)
                {
                    Repeater rptAdmin2 = e.Item.FindControl("rptAdmin2") as Repeater;
                    LoadAdmin2Targets(rptAdmin2, admin1Id);
                }
            }
        }

        private void LoadAdmin1Targets(Repeater rpt, int countryId)
        {

            if (rpt != null)
            {
                int indicatorId = 0;
                int.TryParse(hfIndicatorId.Value, out indicatorId);
                rpt.DataSource = DBContext.GetData("[GetAdmin1TargetOfIndicator]", new object[] { countryId, indicatorId });
                rpt.DataBind();
            }
        }
        private void LoadAdmin2Targets(Repeater rptAdmin2, int admin1Id)
        {
            if (rptAdmin2 != null)
            {
                int indicatorId = 0;
                int.TryParse(hfIndicatorId.Value, out indicatorId);
                rptAdmin2.DataSource = DBContext.GetData("[GetAdmin2TargetOfIndicator]", new object[] { admin1Id, indicatorId });
                rptAdmin2.DataBind();
            }
        }

        protected void rptCountryGender_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int countryId = 0;
                HiddenField hfCountryId = e.Item.FindControl("hfCountryId") as HiddenField;
                if (hfCountryId != null)
                {
                    int.TryParse(hfCountryId.Value, out countryId);
                }
                if (countryId > 0)
                {
                    Repeater rptAdmin1Gen = e.Item.FindControl("rptAdmin1") as Repeater;
                    LoadAdmin1Targets(rptAdmin1Gen, countryId);
                }
            }
        }

        protected void rptAdmin1Gender_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int admin1Id = 0;
                HiddenField hfAdmin1Id = e.Item.FindControl("hfAdmin1Id") as HiddenField;
                if (hfAdmin1Id != null)
                {
                    int.TryParse(hfAdmin1Id.Value, out admin1Id);
                }
                if (admin1Id > 0)
                {
                    Repeater rptAdmin2 = e.Item.FindControl("rptAdmin2") as Repeater;
                    LoadAdmin2Targets(rptAdmin2, admin1Id);
                }
            }
        }

        public int EmgLocationId { get; set; }
    }


}