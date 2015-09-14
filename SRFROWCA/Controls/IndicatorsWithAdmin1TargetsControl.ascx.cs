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
            if (!IsPostBack)
            {
                
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

        public void SaveIndicators(int priorityActivityId)
        {
            SaveIndicator(priorityActivityId);
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
                    if (unitId == 269)
                        SaveAdmin1GenderTargets(indicatorId);
                    else
                        SaveAdmin1Targets(indicatorId);
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
                    if (unitId == 269)
                        SaveAdmin1GenderTargets(indicatorId);
                    else
                        SaveAdmin1Targets(indicatorId);
                }
                else
                {
                    int newIndicatorId = DBContext.Add("InsertIndicator", new object[] { activityId, indEn, indFr, 
                                                                                            unitId, userId, gender, 
                                                                                            calMethod, DBNull.Value });
                    if (newIndicatorId > 0)
                    {
                        if (unitId == 269)
                            SaveAdmin1GenderTargets(indicatorId);
                        else
                            SaveAdmin1Targets(indicatorId);
                    }
                }
            }
        }
        private void SaveAdmin1Targets(int indicatorId)
        {
            int insertCount = 1;

            foreach (RepeaterItem item in rptCountry.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var txttarget = (TextBox)item.FindControl("txtTarget");
                    var hfLocationId = (HiddenField)item.FindControl("hfLocationId");
                    if (!string.IsNullOrEmpty(txttarget.Text))
                    {
                        DBContext.Update("InsertIndicatorTarget", new object[] { indicatorId, Convert.ToInt32(hfLocationId.Value), 
                                                                                  Convert.ToInt32(txttarget.Text), insertCount, 
                                                                                  RC.GetCurrentUserId, DBNull.Value });
                        insertCount++;
                    }

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

        private void SaveAdmin1GenderTargets(int indicatorId)
        {
            int insertCount = 1;

            foreach (RepeaterItem item in rptCountryGender.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    int? targetMen = GetAdminTarget(item, "txtTargetMen");
                    int? targetWomen = GetAdminTarget(item, "txtTargetWomen");
                    //int? targetTotal = GetAdminTarget(item, "txtTargetTotal");
                    var hfLocationId = (HiddenField)item.FindControl("hfLocationId");

                    if (targetMen != null || targetWomen != null)
                    {
                        int tm = targetMen == null ? 0 : (int)targetMen;
                        int tw = targetWomen == null ? 0 : (int)targetWomen;
                        int targetTotal = tm + tw;
                        
                        DBContext.Update("InsertIndicatorGenderTarget", new object[] { indicatorId, Convert.ToInt32(hfLocationId.Value), 
                                                                                  targetTotal, targetMen, targetWomen, insertCount, 
                                                                                  RC.GetCurrentUserId, DBNull.Value });
                        insertCount++;
                    }

                }
            }
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
                SaveAdmin1Targets(indicatorId);

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
                HiddenField hfCountryId = e.Item.FindControl("hfCountryIdGender") as HiddenField;
                if (hfCountryId != null)
                {
                    int.TryParse(hfCountryId.Value, out countryId);
                }
                if (countryId > 0)
                {
                    Repeater rptAdmin1Gen = e.Item.FindControl("rptAdmin1Gender") as Repeater;
                    LoadAdmin1Targets(rptAdmin1Gen, countryId);
                }
            }
        }

        protected void rptAdmin1Gender_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int admin1Id = 0;
                HiddenField hfAdmin1Id = e.Item.FindControl("hfAdmin1IdGender") as HiddenField;
                if (hfAdmin1Id != null)
                {
                    int.TryParse(hfAdmin1Id.Value, out admin1Id);
                }
                if (admin1Id > 0)
                {
                    Repeater rptAdmin2 = e.Item.FindControl("rptAdmin2Gender") as Repeater;
                    LoadAdmin2Targets(rptAdmin2, admin1Id);
                }
            }
        }

        public int EmgLocationId { get; set; }
    }

    
}