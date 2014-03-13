using System;
using System.Web;
using System.Web.Security;
using SRFROWCA.Common;
using System.Web.UI.WebControls;

namespace SRFROWCA.Account
{
    public partial class Login : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);

            if (!IsPostBack)
            {
                TextBox tb = this.LoginUser.FindControl("UserName") as TextBox;
                if (tb != null)
                {
                    this.SetFocus(tb);
                }
            }
        }

        protected void LoginUser_LoggedIn(Object sender, EventArgs e)
        {
            if (Roles.IsUserInRole(LoginUser.UserName, "ClusterLead"))
            {
                Response.Redirect("~/ClusterLead/ProjectsListing.aspx");
            }
            else if (Roles.IsUserInRole(LoginUser.UserName, "User"))
            {
                Response.Redirect("~/Pages/AddActivities.aspx");
            }
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
                    LoginUser.FailureText = "Your account has been locked out. Please contact the administrator to have your account unlocked.";
                }
                else if (!usrInfo.IsApproved)
                {
                    LoginUser.FailureText = "Your account has not yet been approved. You cannot login until an administrator has approved your account.";
                }
            }
            else
            {
                LoginUser.FailureText = "Wrong Username and password combination!";
            }
        }
        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "Login", this.User);
        }
    }
}