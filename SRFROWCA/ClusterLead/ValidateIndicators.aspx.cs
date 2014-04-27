using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;
using SRFROWCA.Common;
using SRFROWCA.Controls;

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

        private void LoadReportMainInfo(int reportId)
        {
            DataTable dt = DBContext.GetData("GetReportMainInfo", new object[] { reportId });
            if (dt.Rows.Count > 0)
            {
                lblProjectTitle.Text = "(" + dt.Rows[0]["ProjectCode"].ToString() + ") " + dt.Rows[0]["ProjectTitle"].ToString();
                lblOrganization.Text = dt.Rows[0]["OrganizationName"].ToString();
                lblUpdatedBy.Text = dt.Rows[0]["Email"].ToString();
                lblUpdatedOn.Text = dt.Rows[0]["CreatedDate"].ToString();
                lblReportingPeriod.Text = dt.Rows[0]["MonthName"].ToString() + "-2014";
            }
        }

        private void LoadIndicators(int reporId)
        {
            gvIndicators.DataSource = DBContext.GetData("GetReportIndicatorsToValidate", new object[] { reporId, RC.SelectedSiteLanguageId });
            gvIndicators.DataBind();
        }

        protected void gvIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ObjPrToolTip.ObjectiveIconToolTip(e, 6);
            ObjPrToolTip.PrioritiesIconToolTip(e, 7);

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
            }
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

                //if (activityDataId > 0 && reportId > 0 && ucIndComments != null)
                if (activityDataId > 0 && reportId > 0)
                {
                    ActivityDataId = activityDataId;
                    ReportId = reportId;

                    using (ORSEntities db = new ORSEntities())
                    {
                        var reportInfo = db.Reports.Where(x => x.ReportId == ReportId).Select(y => new { y.YearId, y.MonthId, y.ProjectId, y.EmergencyLocationId }).ToList();
                        if (reportInfo.Count > 0)
                        {
                            int yearId = reportInfo[0].YearId;
                            int monthId = reportInfo[0].MonthId;
                            int projectId = reportInfo[0].ProjectId;
                            int emgLocationId = reportInfo[0].EmergencyLocationId;

                            ucIndComments.ActivityDataId = activityDataId;
                            ucIndComments.YearId = yearId;
                            ucIndComments.MonthId = monthId;
                            ucIndComments.ProjectId = projectId;
                            ucIndComments.EmgLocationId = UserInfo.EmergencyCountry;
                            ucIndComments.LoadComments();
                            mpeComments.Show();
                        }
                    }                    
                }
            }
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

        protected void btnSaveComments_Click(object sender, EventArgs e)
        {
            string comments = ucIndComments.GetComments();
            if (!string.IsNullOrEmpty(comments))
            {
                using (ORSEntities db = new ORSEntities())
                {
                    var reportInfo = db.Reports.Where(x => x.ReportId == ReportId).Select(y => new { y.YearId, y.MonthId, y.ProjectId, y.EmergencyLocationId }).ToList();
                    if (reportInfo.Count > 0)
                    {
                        int yearId = reportInfo[0].YearId;
                        int monthId = reportInfo[0].MonthId;
                        int projectId = reportInfo[0].ProjectId;
                        int emgLocationId = reportInfo[0].EmergencyLocationId;
                        DBContext.Add("InsertIndicatorComments", new object[] { yearId, monthId, projectId, ActivityDataId, emgLocationId, comments, RC.GetCurrentUserId, DBNull.Value });
                    }
                }
            }
            //mpeComments.Show();
        }

        protected void btnCancelComments_Click(object sender, EventArgs e)
        {

            //mpeComments.Hide();
        }

        private void ApproveIndicatorData(int reportDetailId)
        {
            DBContext.Update("UpdateReportDetailApproved", new object[] { reportDetailId, RC.GetCurrentUserId, DBNull.Value });
        }

        //private void RejectIndicatorData(int reportDetailId)
        //{
        //    DBContext.Update("UpdateReportDetailRejected", new object[] { reportDetailId, RC.GetCurrentUserId, DBNull.Value });
        //}

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