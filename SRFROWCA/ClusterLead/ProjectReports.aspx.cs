using System;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class ProjectReports : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            LoadReports();
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

            string startDate = !string.IsNullOrEmpty(txtFromDate.Text) ? txtFromDate.Text : null;
            string endDate = !string.IsNullOrEmpty(txtToDate.Text) ? txtToDate.Text : null;

            return DBContext.GetData("uspGetReports", new object[] { projectID, startDate, endDate });
        }

        protected void grdReports_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "PrintReport")
            {
                //int projectID = 65761;
                int projectID = 0;

                if (Request.QueryString["pid"] != null)
                    int.TryParse(Request.QueryString["pid"].ToString(), out projectID);

                DataTable dtResults = DBContext.GetData("uspGetReports", new object[] { Convert.ToString(projectID), null, null });

                if (dtResults.Rows.Count > 0)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}-{1}.pdf", UserInfo.CountryName, DateTime.Now.ToString("yyyyMMddHHmmss")));
                    Response.BinaryWrite(WriteDataEntryPDF.GeneratePDF(dtResults, projectID, Convert.ToInt32(e.CommandArgument)).ToArray());
                }
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

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            //int projectID = 65761;
            int projectID = 0;

            if (Request.QueryString["pid"] != null)
                int.TryParse(Request.QueryString["pid"].ToString(), out projectID);

            DataTable dtResults = DBContext.GetData("uspGetReports", new object[] { Convert.ToString(projectID), null, null });

            if (dtResults.Rows.Count > 0)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}-{1}.pdf", UserInfo.CountryName, DateTime.Now.ToString("yyyyMMddHHmmss")));
                Response.BinaryWrite(WriteDataEntryPDF.GeneratePDF(dtResults, projectID, null).ToArray());
            }
        }
    }
}