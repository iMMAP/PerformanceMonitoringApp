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
    public partial class ctlOutputIndicatorDataEntryCountry : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
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
                
                dt = DBContext.GetData("GetOutputIndicatorsForReporting", new object[] {EmgLocationId, EmgClusterId
                                                                                        ,YearId, MonthId, IsRegional, 
                                                                                        RC.SelectedSiteLanguageId});
            return dt;
        }

        protected void gvIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.RegionalIndicatorIcon(e, 11);
                ObjPrToolTip.CountryIndicatorIcon(e, 12);

                UI.SetThousandSeparator(e.Row, "lblTarget");
                UI.SetThousandSeparator(e.Row, "lblSum");
            }
        }

        public bool SaveClusterIndicatorDetails(int countryId, int clusterId, int monthId)
        {
            bool isDataProvided = false;
            int clusterIndicatorID = 0;
            int? achieved = null;

            foreach (GridViewRow row in gvIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtAchieved = (TextBox)row.FindControl("txtAchieved");
                    Label lblClusterIndicatorID = (Label)row.FindControl("lblClusterIndicatorID");

                    if (lblClusterIndicatorID != null)
                        clusterIndicatorID = Convert.ToInt32(lblClusterIndicatorID.Text);

                    if (txtAchieved != null)
                    {
                        achieved = !string.IsNullOrEmpty(txtAchieved.Text.Trim()) ? Convert.ToInt32(txtAchieved.Text.Trim()) : (int?)null;
                        if (achieved > 0)
                            isDataProvided = true;
                    }

                    DBContext.Add("uspInsertClusterReport", new object[] { clusterIndicatorID, clusterId, countryId, 11, monthId,
                                                                                                    achieved, RC.GetCurrentUserId, null });
                }
            }
            return isDataProvided;
        }


        public int? EmgLocationId { get; set; }
        public int? EmgClusterId { get; set; }
        public int YearId { get; set; }
        public int MonthId { get; set; }
        public int? IsRegional { get; set; }
    }
}