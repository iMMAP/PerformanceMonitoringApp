using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class ProjectDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            
            //if (Session["ViewProjectId"] != null)
            if (Request.QueryString["pid"] != null)
            {
                GetProjectDetails();
                LoadReports();
                UpdateNotificationStatus();
            }
        }

        private void UpdateNotificationStatus()
        {
            int notificationId = 0;
            if (Request.QueryString["nid"] != null)
            {
                int.TryParse(Request.QueryString["nid"], out notificationId);

                if (notificationId > 0)
                {
                    DBContext.Update("UpdateNotificationStatus", new object[] { notificationId,  DBNull.Value});
                }
            }
        }

        private void GetProjectDetails()
        {
            //int projectId = Convert.ToInt32(Session["ViewProjectId"].ToString());
            int projectId = 0;
            int.TryParse(Request.QueryString["pid"].ToString(), out projectId);
            DataTable dt = DBContext.GetData("GetProjectDetails", new object[] { projectId, 1 });
            fvProjects.DataSource = dt;
            fvProjects.DataBind();
        }

        private void LoadReports()
        {
            grdReports.DataSource = GetReports();
            grdReports.DataBind();
        }

        private DataTable GetReports()
        {
            int projectID = 0;

            if (Request.QueryString["pid"] != null)
                int.TryParse(Request.QueryString["pid"].ToString(), out projectID);

            string startDate = /*!string.IsNullOrEmpty(txtFromDate.Text) ? txtFromDate.Text :*/ null;
            string endDate = /*!string.IsNullOrEmpty(txtToDate.Text) ? txtToDate.Text :*/ null;

            DataTable dtGrid = DBContext.GetData("uspGetReports", new object[] { projectID, startDate, endDate, null, null,RC.SelectedSiteLanguageId,
                                                                                null, null, null, null, null, null});

            string[] selectedColumns = new[] { "ReportID", "ReportName", "ProjectCode", "IsApproved", "Country", "ProjectID", "CreatedDate" };
            DataTable dtFiltered = new DataTable();
            try
            {
                dtFiltered = new DataView(dtGrid).ToTable(true, selectedColumns).Select("ReportID IS NOT NULL").CopyToDataTable<DataRow>();
            }
            catch { }

            return dtFiltered;
        }

        protected void fvProjects_PageIndexChanging(object sender, FormViewPageEventArgs e)
        {

        }

        //protected void btnViewReport_Click(object sender, EventArgs e)
        //{
        //    string projectID = "0";

        //    if (!string.IsNullOrEmpty(Request.QueryString["pid"]))
        //        projectID = Convert.ToString(Request.QueryString["pid"]);

        //    Response.Redirect("~/ClusterLead/ProjectReports.aspx?pid=" + projectID);
        //}

        protected void btExportPDF_Click(object sender, EventArgs e)
        {
            int projectID = 0;

            if (Request.QueryString["pid"] != null)
                int.TryParse(Request.QueryString["pid"].ToString(), out projectID);

            DataTable dtResults = DBContext.GetData("uspGetReports", new object[] { Convert.ToString(projectID), null, null , null, null, RC.SelectedSiteLanguageId,
                                                                                        null, null, null, null, null, null});

            if (dtResults.Rows.Count > 0)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}-{1}.pdf", SRFROWCA.Common.UserInfo.CountryName, DateTime.Now.ToString("yyyyMMddHHmmss")));
                Response.BinaryWrite(SRFROWCA.Common.WriteDataEntryPDF.GeneratePDF(dtResults, projectID, null).ToArray());
            }
        }

        protected void grdReports_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "PrintReport")
            {
                //int projectID = 65761;
                int projectID = 0;

                if (Request.QueryString["pid"] != null)
                    int.TryParse(Request.QueryString["pid"].ToString(), out projectID);

                DataTable dtResults = DBContext.GetData("uspGetReports", new object[] { Convert.ToString(projectID), null, null, Convert.ToInt32(e.CommandArgument), null, RC.SelectedSiteLanguageId, null, null, null, null, null, null });

                if (dtResults.Rows.Count > 0)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}-{1}.pdf", UserInfo.CountryName, DateTime.Now.ToString("yyyyMMddHHmmss")));
                    Response.BinaryWrite(WriteDataEntryPDF.GeneratePDF(dtResults, projectID, Convert.ToInt32(e.CommandArgument)).ToArray());
                }
            }
            else if (e.CommandName == "ViewReport")
            {
                Response.Redirect("~/ClusterLead/ReportDetails.aspx?rid=" + Convert.ToString(e.CommandArgument));
            }
        }

        protected void grdReports_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetReports();
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                grdReports.DataSource = dt;
                grdReports.DataBind();
            }
        }

        private string GetSortDirection(string column)
        {
            string sortDirection = "ASC";

            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        protected void grdReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdReports.PageIndex = e.NewPageIndex;
            grdReports.SelectedIndex = -1;

            LoadReports();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadReports();
        }
    }
}