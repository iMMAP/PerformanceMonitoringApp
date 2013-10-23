using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using System.IO;
using System.Web.Security;

namespace SRFROWCA.Admin.Users
{
    public partial class UsersListing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.User.IsInRole("Admin") && !this.User.IsInRole("CountryAdmin"))
            {
                Response.Redirect("~/Default.aspx");
            }

            if (IsPostBack) return;
            LoadUsers();
        }

        private DataTable GetUsers()
        {
            if (this.User.IsInRole("CountryAdmin"))
            {
                object[] parameters = GetParameters();
                return DBContext.GetData("GetUsers", parameters);
            }
            else
            {
                return DBContext.GetData("GetAllUsersInfo");
            }
        }

        private object[] GetParameters()
        {
            string userId = Membership.GetUser(HttpContext.Current.User.Identity.Name).ProviderUserKey.ToString();
            string userName = null;
            string email = null;
            int? isApproved = null;
            int? isLockedOut = null;
            string orgName = null;
            string locationName = null;
            return new object[] { userId, userName, email, isApproved, isLockedOut, orgName, locationName };
        }

        private void LoadUsers()
        {
            gvUsers.DataSource = GetUsers();
            gvUsers.DataBind();
        }

        protected void chkIsApproved_CheckedChanged(object sender, EventArgs e)
        {
            //foreach (GridViewRow row in gvUsers.Rows)
            {
                int index = ((GridViewRow)((CheckBox)sender).NamingContainer).RowIndex;
                CheckBox cb = gvUsers.Rows[index].FindControl("chkIsApproved") as CheckBox;
                if (cb != null)
                {
                    Label lblUserId = gvUsers.Rows[index].FindControl("lblUserId") as Label;
                    if (lblUserId != null)
                    {
                        UpdateUserApproval(cb.Checked, lblUserId.Text);
                    }
                }
            }
        }

        protected void chkIsLocked_CheckedChanged(object sender, EventArgs e)
        {
            //foreach (GridViewRow row in gvUsers.Rows)
            {
                int index = ((GridViewRow)((CheckBox)sender).NamingContainer).RowIndex;
                CheckBox cb = gvUsers.Rows[index].FindControl("chkIsLocked") as CheckBox;
                if (cb != null)
                {
                    Label lblUserId = gvUsers.Rows[index].FindControl("lblUserId") as Label;
                    if (lblUserId != null)
                    {
                        UpdateUserLocked(cb.Checked, lblUserId.Text);
                    }
                }
            }
        }

        protected void chkIsCountryAdmin_CheckedChanged(object sender, EventArgs e)
        {
            //foreach (GridViewRow row in gvUsers.Rows)
            {
                int index = ((GridViewRow)((CheckBox)sender).NamingContainer).RowIndex;
                CheckBox cb = gvUsers.Rows[index].FindControl("chkIsCountryAdmin") as CheckBox;
                if (cb != null)
                {
                    Label lblUserId = gvUsers.Rows[index].FindControl("lblUserId") as Label;
                    if (lblUserId != null)
                    {
                        string roleName = "CountryAdmin";
                        AddRemoveUserInRole(cb.Checked, lblUserId.Text, roleName);
                    }
                }
            }
        }

        private void UpdateUserApproval(bool isApproved, string userId)
        {
            Guid uId = new Guid(userId);
            DBContext.Update("UpdateUserApproval", new object[] { uId, isApproved, DBNull.Value });
        }

        private void UpdateUserLocked(bool isLocked, string userId)
        {
            Guid uId = new Guid(userId);
            DBContext.Update("UpdateUserLocked", new object[] { uId, isLocked, DBNull.Value });
        }

        private void AddRemoveUserInRole(bool isAdd, string userId, string roleName)
        {
            Guid uId = new Guid(userId);
            DBContext.Update("AddRemoveUserInRole", new object[] { uId, isAdd, roleName, DBNull.Value });
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportUtility.PrepareGridViewForExport(gvUsers);
            ExportGridView();
        }

        public override void VerifyRenderingInServerForm(Control control) { }

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

        protected void btnAddUser_Click(object sender, GridViewPageEventArgs e)
        {
            Response.Redirect("~/Account/Register.aspx");
        }
    }
}