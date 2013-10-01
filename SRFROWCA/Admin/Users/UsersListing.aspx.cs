using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using System.IO;

namespace SRFROWCA.Admin.Users
{
    public partial class UsersListing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.User.Identity.IsAuthenticated || !this.User.IsInRole("Admin"))
            {
                Response.Redirect("~/Default.aspx");
            }

            if (IsPostBack) return;
            LoadUsers();
        }

        private DataTable GetUsers()
        {
            return DBContext.GetData("GetUsers");
        }

        private void LoadUsers()
        {
            gvUsers.DataSource = GetUsers();
            gvUsers.DataBind();
        }

        protected void chkIsApproved_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvUsers.Rows)
            {
                CheckBox cb = row.FindControl("chkIsApproved") as CheckBox;
                if (cb != null)
                {
                    Label lblUserId = row.FindControl("lblUserId") as Label;
                    if (lblUserId != null)
                    {
                        UpdateUserApproval(cb.Checked, lblUserId.Text);
                    }
                }
            }
        }

        private void UpdateUserApproval(bool isApproved, string userId)
        {
            Guid uId = new Guid(userId);
            DBContext.Update("UpdateUserApproval", new object[] { uId, isApproved, DBNull.Value });
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportUtility.PrepareGridViewForExport(gvUsers);
            ExportGridView();
        }

        public override void VerifyRenderingInServerForm(Control control){}

        private void ExportGridView()
        {
            string attachment = "attachment; filename=3wopusers.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvUsers.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

        protected void gvUsers_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetUsers();
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
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

        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            gvUsers.SelectedIndex = -1;
            LoadUsers();
        }
    }
}