using System;
using System.Data;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Admin.Users
{
    public partial class UsersListing : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            this.Form.DefaultButton = this.btnAddUser.UniqueID;
            LoadUsers();
        }

        private DataTable GetUsers()
        {
            if (ROWCACommon.IsCountryAdmin(this.User))
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
            Guid userId = ROWCACommon.GetCurrentUserId();
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
            int index = ((GridViewRow)((CheckBox)sender).NamingContainer).RowIndex;
            CheckBox cb = gvUsers.Rows[index].FindControl("chkIsCountryAdmin") as CheckBox;
            Label lblUserId = gvUsers.Rows[index].FindControl("lblUserId") as Label;

            if (cb == null || lblUserId == null) return;

            AddRemoveUserInRole(cb.Checked, lblUserId.Text, ROWCACommon.GetCountryAdminRoleName);
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditUser")
            {
                Session["EditUserId"] = e.CommandArgument.ToString();
                Response.Redirect("~/Account/UpdateUser.aspx");
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
            string fileName = "3WPMusers";
            string fileExtention = ".xls";
            ExportUtility.ExportGridView(gvUsers, fileName, fileExtention, Response);            
        }

        public override void VerifyRenderingInServerForm(Control control) { }

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

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "UsersListing", this.User);
        }
    }
}