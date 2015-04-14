﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class ValidateIndicators : BasePage
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
                lblProjectTitle.Text = "(" + dt.Rows[0]["ProjectCode"].ToString() + ") " + dt.Rows[0]["ProjectTitle"].ToString();
                lblOrganization.Text = dt.Rows[0]["OrganizationName"].ToString();
                lblUpdatedBy.Text = dt.Rows[0]["Email"].ToString();
                lblUpdatedOn.Text = dt.Rows[0]["CreatedDate"].ToString();
                lblReportingPeriod.Text = dt.Rows[0]["MonthName"].ToString() + "-2015";
            }
        }

        private void LoadIndicators(int reporId)
        {
            gvIndicators.DataSource = DBContext.GetData("GetReportIndicatorsToValidate2015", new object[] { reporId, RC.SelectedSiteLanguageId });
            gvIndicators.DataBind();
        }

        protected void gvIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ObjPrToolTip.ObjectiveIconToolTip(e, 0);
        }

        protected void gvIndicators_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddComments")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());
                int activityDataId = 0;
                int.TryParse(gvIndicators.DataKeys[rowIndex]["ActivityDataId"].ToString(), out activityDataId);
                int reportId = 0;
                int.TryParse(gvIndicators.DataKeys[rowIndex]["ReportId"].ToString(), out reportId);

                if (activityDataId > 0 && reportId > 0)
                {
                    ActivityDataId = activityDataId;
                    ReportId = reportId;

                    if (ucIndComments.LoadComments(reportId, activityDataId))
                        btnSaveComments.Visible = false;

                    mpeComments.Show();
                }
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e) 
        {
            bool isApproved = false;
            int reportId = 0;
            foreach (GridViewRow row in gvIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    reportId = Convert.ToInt32(gvIndicators.DataKeys[row.RowIndex].Values["ReportId"].ToString());
                    int reportDetailId = Convert.ToInt32(gvIndicators.DataKeys[row.RowIndex].Values["ReportDetailId"].ToString());
                    CheckBox cb = gvIndicators.Rows[row.RowIndex].FindControl("chkApproved") as CheckBox;
                    if (cb != null)
                    {
                        ApproveIndicatorData(reportDetailId, cb.Checked);
                        isApproved = true;
                    }
                }
            }
           
            if (isApproved)
            {
                RC.SendEmail(UserInfo.EmergencyCountry, (int?)null, "Sahel ORS: Reports Approved!", "ORS reports approved!");
            }
            ShowMessage("Data Saved Successfully!");
        }

        protected void btnSaveComments_Click(object sender, EventArgs e)
        {
            string comments = txtComments.Value;// ucIndComments.GetComments();
            int indictorCommentDetID = ucIndComments.GetIndicatorCommentDetailID();

            if (!string.IsNullOrEmpty(comments))
            {
                DBContext.Add("InsertIndicatorComments", new object[] { ReportId, ActivityDataId, comments, RC.GetCurrentUserId, DBNull.Value, indictorCommentDetID });
            }
            
        }

        protected void btnCancelComments_Click(object sender, EventArgs e)
        {}

        private void ApproveIndicatorData(int reportDetailId, bool isApproved)
        {
            DBContext.Update("InsertUpdateApproveIndicator", new object[] { reportDetailId, RC.GetCurrentUserId, isApproved, DBNull.Value });
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success)
        {
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, true, 500);
        }

        private int ReportId
        {
            get
            {
                int reportId = 0;
                if (ViewState["ReportId"] != null)
                {
                    int.TryParse(ViewState["ReportId"].ToString(), out reportId);
                }

                return reportId;
            }
            set
            {
                ViewState["ReportId"] = value;
            }
        }

        private int ActivityDataId
        {
            get
            {
                int commentsIndId = 0;
                if (ViewState["ActivityDataId"] != null)
                {
                    int.TryParse(ViewState["ActivityDataId"].ToString(), out commentsIndId);
                }

                return commentsIndId;
            }
            set
            {
                ViewState["ActivityDataId"] = value;
            }
        }

        protected void rbCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            int reportId = 0;
            int.TryParse(Request.QueryString["rid"].ToString(), out reportId);

            LoadIndicators(reportId);
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


    }
}