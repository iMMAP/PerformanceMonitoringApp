using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClusterLead
{
    public partial class ReportDetails : BasePage
    {
        public string ProjectID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int reportId = 0;
                int.TryParse(Request.QueryString["rid"].ToString(), out reportId);

                LoadReportMainInfo(reportId);
                LoadIndicators(reportId);
            }
        }

        internal override void BindGridData()
        {
            int reportId = 0;
            int.TryParse(Request.QueryString["rid"].ToString(), out reportId);

            LoadReportMainInfo(reportId);
            LoadIndicators(reportId);
        }

        private void LoadReportMainInfo(int reportId)
        {
            DataTable dt = DBContext.GetData("GetReportMainInfo", new object[] { reportId });

            if (dt.Rows.Count > 0)
            {
                ProjectID = Convert.ToString(dt.Rows[0]["ProjectID"]);
                lblProjectTitle.Text = "(" + dt.Rows[0]["ProjectCode"].ToString() + ") " + dt.Rows[0]["ProjectTitle"].ToString();
                lblOrganization.Text = dt.Rows[0]["OrganizationName"].ToString();
                lblUpdatedBy.Text = dt.Rows[0]["Email"].ToString();
                lblUpdatedOn.Text = dt.Rows[0]["CreatedDate"].ToString();
                lblReportingPeriod.Text = dt.Rows[0]["MonthName"].ToString() + "-2014";
            }
        }

        private void LoadIndicators(int reporId)
        {
            gvIndicators.DataSource = DBContext.GetData("GetReportIndicatorsToValidate", new object[] { reporId, RC.SelectedSiteLanguageId, null });
            gvIndicators.DataBind();
        }

        protected void gvIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ObjPrToolTip.ObjectiveIconToolTip(e, 0);
            ObjPrToolTip.PrioritiesIconToolTip(e, 1);
            ObjPrToolTip.CountryIndicatorIcon(e, 2);
        }
    }
}
