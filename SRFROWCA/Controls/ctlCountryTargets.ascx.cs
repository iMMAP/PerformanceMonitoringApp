using BusinessLogic;
using SRFROWCA.Common;
using SRFROWCA.Configurations;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace SRFROWCA.Controls
{
    public partial class ctlCountryTargets : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack) return;            
            LoadCountry();
        }

        private void LoadCountry()
        {
            DataTable dt= GetCountryIndTarget(this.EmgLocId, this.IndicatorId);
            rptCountry.DataSource = dt;
            rptCountry.DataBind();
            rptCountryGender.DataSource = dt;
            rptCountryGender.DataBind();

            if (!this.IsGender)
                UpdateRepeaterTargetColumn(rptCountryGender);
            else
                UpdateRepeaterTargetColumn(rptCountry);
        }

        private void UpdateRepeaterTargetColumn(Repeater rpt)
        {
            foreach (RepeaterItem item in rpt.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtTotal = item.FindControl("txtTarget") as TextBox;
                    if (txtTotal != null)
                    {
                        txtTotal.Text = "";
                    }
                }
            }
        }

        private DataTable GetCountryIndTarget(int emgLocationId, int indicatorId)
        {
            return DBContext.GetData("GetCountryTargetOfIndicator_Country", new object[] { emgLocationId, indicatorId });
        }

       

        public bool IsGender { get; set; }

        public int EmgLocId { get; set; }
        public int IndicatorId { get; set; }
        
    }
}