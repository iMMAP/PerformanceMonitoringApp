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

            foreach (RepeaterItem item in rptAdmin1.Items)
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

            foreach (RepeaterItem item in rptAdmin1Gender.Items)
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

        //protected void rptAdmin1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        if (IsPopulation == 0)
        //        {
        //            HtmlTableCell td = e.Item.FindControl("tdFromLocTop") as HtmlTableCell;
        //            if (td != null)
        //                td.Visible = false;

        //            td = e.Item.FindControl("tdFromLoc") as HtmlTableCell;
        //            if (td != null)
        //                td.Visible = false;

        //        }
        //    }
        //}
    }
}