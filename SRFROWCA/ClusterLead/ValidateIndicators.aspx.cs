using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;

namespace SRFROWCA.ClusterLead
{
    public partial class ValidateIndicators : System.Web.UI.Page
    {
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

        private void LoadReportMainInfo(int reportId)
        {
            DataTable dt = DBContext.GetData("GetReportMainInfo", new object[] { reportId });
            if (dt.Rows.Count > 0)
            {
                lblProjectTitle.Text = dt.Rows[0]["ProjectTitle"].ToString();
                lblProjectCode.Text = dt.Rows[0]["ProjectCode"].ToString();
                lblOrganization.Text = dt.Rows[0]["OrganizationName"].ToString();
                lblUpdatedBy.Text = dt.Rows[0]["Email"].ToString();
                lblUpdatedOn.Text = dt.Rows[0]["CreatedDate"].ToString();
                lblReportingPeriod.Text = "";
                
            }
        }

        private void LoadIndicators(int reporId)
        {
            gvIndicators.DataSource = DBContext.GetData("GetReportIndicatorsToValidate", new object[] { reporId, 1 });
            gvIndicators.DataBind();
        }

        protected void gvIndicators_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}