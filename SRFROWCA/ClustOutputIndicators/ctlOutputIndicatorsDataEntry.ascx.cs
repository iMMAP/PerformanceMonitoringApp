using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClustOutputIndicators
{
    public partial class ctlOutputIndicatorsDataEntry : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        private void LoadClusterIndicators()
        {
            gvIndicators.DataSource = GetClusterIndicatros();
            gvIndicators.DataBind();
        }

        private DataTable GetClusterIndicatros()
        {
            DataTable dt = new DataTable();
            if (EmgLocationId > 0 && EmgClusterId > 0)

                dt = DBContext.GetData("GetOutputIndicatorsForReporting_2016", new object[] {EmgLocationId, EmgClusterId
                                                                                        ,YearId, MonthId, IsRegional, 
                                                                                        RC.SelectedSiteLanguageId});
            return dt;
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
                        int yearId = 12;
                        DataTable dt = DBContext.GetData("GetOutputCountryTargetOfIndicator",
                                                                    new object[] { EmgLocationId, EmgClusterId, 
                                                                               yearId, indicatorId});
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
                        int yearId = 12;
                        DataTable dt1 = DBContext.GetData("[GetOutputAdmin1TargetOfIndicator]", new object[] {countryId, EmgLocationId, 
                                                                                                                EmgClusterId, yearId, indicatorId });
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

        public int? EmgLocationId { get; set; }
        public int? EmgClusterId { get; set; }
        public int YearId { get; set; }
        public int MonthId { get; set; }
        public int? IsRegional { get; set; }
    }
}