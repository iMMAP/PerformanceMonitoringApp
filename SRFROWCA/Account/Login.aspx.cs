using System;
using System.Web;
using System.Web.Security;
using SRFROWCA.Common;
using System.Web.UI.WebControls;

namespace SRFROWCA.Account
{
    public partial class Login : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);

            if (!IsPostBack)
            {
                Button btn = this.LoginUser.FindControl("LoginButton") as Button;
                if (btn != null)
                {
                    this.Form.DefaultButton = btn.UniqueID;
                }

                TextBox tb = this.LoginUser.FindControl("UserName") as TextBox;
                if (tb != null)
                {
                    this.SetFocus(tb);
                }
            }
        }

        protected void LoginUser_LoggedIn(Object sender, EventArgs e)
        {
            //if (Roles.IsUserInRole(LoginUser.UserName, "ClusterLead"))
            //{
            //    Response.Redirect("~/ClusterLead/ProjectsListing.aspx");
            //}

            if (Roles.IsUserInRole(LoginUser.UserName, "User"))
            {
                Response.Redirect("~/Pages/AddActivities.aspx");
            }

            //else if (Roles.IsUserInRole(LoginUser.UserName, "RegionalClusterLead"))
            //{
            //    Response.Redirect("~/Pages/AddActivities.aspx");
            //}
        }

        protected void LoginUser_LoginError(object sender, EventArgs e)
        {
            // Does there exist a User account for this user?
            MembershipUser usrInfo = Membership.GetUser(LoginUser.UserName);
            if (usrInfo != null)
            {
                // Is this user locked out?
                if (usrInfo.IsLockedOut)
                {
                    LoginUser.FailureText = "Your account has been locked out. Please contact the admin of the site for futher details.";
                }
                else if (!usrInfo.IsApproved)
                {
                    LoginUser.FailureText = "Your account has not yet been approved. You cannot login until an admin of the site approve your account.";
                }
            }
            else
            {
                LoginUser.FailureText = "Wrong Username and password combination!";
            }
        }

        protected void btnForgotPassword_Click(object sender, EventArgs e)
        {}

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "Login", this.User);
        }
    }
}