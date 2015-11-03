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
    public partial class ctlAdmin1Targets : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack) return;
            LoadCountry();
        }

        private void LoadCountry()
        {
            DataTable dt = GetCountryIndTarget(this.EmgLocId, this.IndicatorId);
            rptCountry.DataSource = dt;
            rptCountry.DataBind();
            rptCountryGender.DataSource = dt;
            rptCountryGender.DataBind();

            if (!this.IsGender)
                UpdateRepeaterTargetColumn(rptCountryGender);
            else
                UpdateRepeaterTargetColumn(rptCountry);
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

        private DataTable GetCountryIndTarget(int emgLocationId, int indicatorId)
        {
            return DBContext.GetData("GetCountryTargetOfIndicator", new object[] { emgLocationId, indicatorId });
        }

        private void LoadAdmin1Targets(Repeater rpt, int countryId)
        {
            if (rpt != null)
            {
                int categoryId = (int)Category == 0 ? 1 : (int)Category;
                rpt.DataSource = DBContext.GetData("[GetAdmin1TargetOfIndicator_Admin1]",
                    new object[] { countryId, this.IndicatorId, categoryId });
                rpt.DataBind();
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
                }
            }
        }

        public int EmgLocId { get; set; }
        public int IndicatorId { get; set; }
        public RC.LocationCategory Category { get; set; }
        public bool IsGender { get; set; }
    }
}