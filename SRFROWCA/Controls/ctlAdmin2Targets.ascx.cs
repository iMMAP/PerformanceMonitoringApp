using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Controls
{
    public partial class ctlAdmin2Targets : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack) return;
            LoadCountry();
        }

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

        private void LoadCountry()
        {
            DataTable dt = GetCountryIndTarget(this.EmgLocId);
            rptCountry.DataSource = dt;
            rptCountry.DataBind();
            rptCountryGender.DataSource = dt;
            rptCountryGender.DataBind();

            if (!this.IsGender)
                UpdateRepeaterTargetColumn(rptCountryGender);
            else
                UpdateRepeaterTargetColumn(rptCountry);
        }

        private DataTable GetCountryIndTarget(int emgLocationId)
        {
            return DBContext.GetData("GetCountryTargetOfIndicator", new object[] { emgLocationId, this.IndicatorId });
        }

        private void LoadAdmin1Targets(Repeater rpt, int countryId)
        {
            if (rpt != null)
            {
                int categoryId = (int)Category == 0 ? 1 : (int)Category;
                rpt.DataSource = DBContext.GetData("[GetAdmin1TargetOfIndicator]",
                    new object[] { countryId, this.IndicatorId, categoryId });
                rpt.DataBind();
            }
        }

        private void LoadAdmin2Targets(Repeater rptAdmin2, int admin1Id)
        {
            if (rptAdmin2 != null)
            {
                int categoryId = (int)Category == 0 ? 1 : (int)Category;
                rptAdmin2.DataSource = DBContext.GetData("[GetAdmin2TargetOfIndicator]",
                                                            new object[] { admin1Id, this.IndicatorId, categoryId });
                rptAdmin2.DataBind();
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

        public int EmgLocId { get; set; }
        public int IndicatorId { get; set; }
        public RC.LocationCategory Category { get; set; }
        public bool IsGender { get; set; }
    }
}