using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;
using SRFROWCA.Common;

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
                lblProjectTitle.Text = "("+ dt.Rows[0]["ProjectCode"].ToString() + ") " + dt.Rows[0]["ProjectTitle"].ToString();
                lblOrganization.Text = dt.Rows[0]["OrganizationName"].ToString();
                lblUpdatedBy.Text = dt.Rows[0]["Email"].ToString();
                lblUpdatedOn.Text = dt.Rows[0]["CreatedDate"].ToString();
                lblReportingPeriod.Text = dt.Rows[0]["MonthName"].ToString() + "-2014";
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

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int reportDetailId = Convert.ToInt32(gvIndicators.DataKeys[row.RowIndex].Values["ReportDetailId"].ToString());
                    CheckBox cb = gvIndicators.Rows[row.RowIndex].FindControl("chkApproved") as CheckBox;
                    if (cb != null)
                    {
                        if (cb.Checked)
                        {
                            ApproveIndicatorData(reportDetailId);
                        }
                    }
                }
            }
        }

        //protected void btnReject_Click(object sender, EventArgs e)
        //{
        //    foreach (GridViewRow row in gvIndicators.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            int reportDetailId = Convert.ToInt32(gvIndicators.DataKeys[row.RowIndex].Values["ReportDetailId"].ToString());
        //            CheckBox cb = gvIndicators.Rows[row.RowIndex].FindControl("chkApproved") as CheckBox;
        //            if (cb != null)
        //            {
        //                if (cb.Checked)
        //                {
        //                    RejectIndicatorData(reportDetailId);
        //                }
        //            }
        //        }
        //    }
        //}

        private void ApproveIndicatorData(int reportDetailId)
        {
            DBContext.Update("UpdateReportDetailApproved", new object[] { reportDetailId, RC.GetCurrentUserId, DBNull.Value });
        }

        //private void RejectIndicatorData(int reportDetailId)
        //{
        //    DBContext.Update("UpdateReportDetailRejected", new object[] { reportDetailId, RC.GetCurrentUserId, DBNull.Value });
        //}

        
    }
}