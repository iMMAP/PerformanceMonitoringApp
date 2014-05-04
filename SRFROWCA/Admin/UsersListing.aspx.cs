using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Admin
{
    public partial class UsersListing : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            this.Form.DefaultButton = this.btnAddUser.UniqueID;
            LoadUsers();
        }

        private DataTable GetUsers()
        {
            object[] parameters = GetParameters();
            if (RC.IsCountryAdmin(this.User))
            {

                return DBContext.GetData("GetUsers", parameters);
            }
            else
            {
                return DBContext.GetData("GetAllUsersInfo", parameters);
            }
        }

        private object[] GetParameters()
        {
            Guid userId = RC.GetCurrentUserId;

            string userName = string.IsNullOrEmpty(txtUserName.Text.Trim()) ? null : txtUserName.Text.Trim();
            string email = string.IsNullOrEmpty(txtEmail.Text.Trim()) ? null : txtEmail.Text.Trim();
            string orgName = string.IsNullOrEmpty(txtOrg.Text.Trim()) ? null : txtOrg.Text.Trim();
            string locationName = string.IsNullOrEmpty(txtCountry.Text.Trim()) ? null : txtCountry.Text.Trim();
            string userType = ddlRoles.SelectedValue == "All"? null: ddlRoles.SelectedValue;
            int? isApproved = rbIsApproved.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(rbIsApproved.SelectedValue);
            int? isLockedOut = null;
            DateTime? startDate = txtFromDate.Text.Trim().Length > 0 ?
                                    DateTime.ParseExact(txtFromDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture) :
                                (DateTime?)null;

            DateTime? endDate = txtToDate.Text.Trim().Length > 0 ?
                                DateTime.ParseExact(txtToDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture) :
                                (DateTime?)null;
            int? countryId = null;
            if (RC.IsCountryAdmin(User))
            {
                countryId = UserInfo.EmergencyCountry;
            }

            return new object[] { userId, userName, email, isApproved, isLockedOut, orgName, locationName, userType, startDate, endDate, countryId };
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

                        Guid userId = new Guid(lblUserId.Text);
                        MembershipUser mu = Membership.GetUser(userId);
                        EmailUser(mu.Email, mu.UserName, true, cb.Checked);
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

                        Guid userId = new Guid(lblUserId.Text);
                        MembershipUser mu = Membership.GetUser(userId);
                        EmailUser(mu.Email, mu.UserName, false, cb.Checked);
                    }
                }
            }
        }

        protected void ddlRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUsers();
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

        protected void EmailUser(string to, string userName, bool isApproved, bool isChecked)
        {
            try
            {
                using (MailMessage mailMsg = new MailMessage())
                {
                    mailMsg.From = new MailAddress("orsocharowca@gmail.com");
                    mailMsg.To.Add(new MailAddress(to));                    
                    mailMsg.IsBodyHtml = true;
                    string mailBody = "";
                    string siteAddress = "http://ors.ocharowca.info/";

                    if (isApproved)
                    {
                        if (isChecked)
                        {
                            mailMsg.Subject = "ORS user approved by admin of the site!";
                            mailBody = @"Your ORS user '" + userName + "' is approved by admin!<br/> You can login to your account at " + siteAddress;
                        }
                        else
                        {
                            mailMsg.Subject = "ORS user disapproved by admin of the site!";
                            mailBody = @"Your " + siteAddress + " user '" + userName + "' is disapproved by admin. Please contact admin of the site for further details.";
                        }
                    }
                    else
                    {
                        if (isChecked)
                        {
                            mailMsg.Subject = "ORS user locked-out by admin of the site!";
                            mailBody = @"Your " + siteAddress + " user '" + userName + "' is locked by admin. Please contact admin of the site for further details.";
                        }
                        else
                        {
                            mailMsg.Subject = "ORS user unlocked by admin of the site!";
                            mailBody = @"Your " + siteAddress + " user '" + userName + "' is unlocked by admin. Please contact admin of the site for further details.";
                        }
                    }

                    mailMsg.Body = mailBody;
                    Mail.SendMail(mailMsg);
                }
            }
            catch
            {
                
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
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