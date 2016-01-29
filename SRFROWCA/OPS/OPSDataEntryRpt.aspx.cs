using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.OPS
{
    public partial class OPSDataEntryRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = DBContext.GetData("GetOPSActivities3", new object[] { 12, 13, 0, 1, 12 });
                rptIndicators.DataSource = dt;
                rptIndicators.DataBind();
            }
        }

        protected void rptIndicators_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int admin1Id = 0;
                HiddenField hfAdmin1Id = e.Item.FindControl("hfAdmin1Id") as HiddenField;
                if (hfAdmin1Id != null)
                {
                    int.TryParse(hfAdmin1Id.Value, out admin1Id);
                }
                //if (admin1Id > 0)
                {
                    Repeater rptAdmin1 = e.Item.FindControl("rptAdmin1") as Repeater;
                    if (rptAdmin1 != null)
                    {
                        DataTable dtTargets = DBContext.GetData("[GetAdmin1TargetOfIndicator]", new object[] { 2, 0 });
                        rptAdmin1.DataSource = dtTargets;
                        rptAdmin1.DataBind();
                    }
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
                rpt.DataSource = DBContext.GetData("[GetAdmin1TargetOfIndicator]", new object[] { countryId, 0 });
                rpt.DataBind();
            }
        }
        private void LoadAdmin2Targets(Repeater rptAdmin2, int admin1Id)
        {
            if (rptAdmin2 != null)
            {
                rptAdmin2.DataSource = DBContext.GetData("[GetAdmin2TargetOfIndicator]", new object[] { admin1Id, 0 });
                rptAdmin2.DataBind();
            }
        }

        
    }
}