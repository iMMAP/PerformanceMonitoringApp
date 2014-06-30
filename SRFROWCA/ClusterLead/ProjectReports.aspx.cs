using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClusterLead
{
    public partial class ProjectReports : System.Web.UI.Page
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

            return DBContext.GetData("uspGetReports", new object[] { projectID , startDate, endDate});
        }

        protected void grdReports_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "PrintReport")
            {
                Response.Redirect("~/ClusterLead/ProjectReports.aspx?rid=" + e.CommandArgument.ToString());
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