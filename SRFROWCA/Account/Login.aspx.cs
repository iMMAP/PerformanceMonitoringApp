using System;
using System.Web;
using System.Web.Security;

namespace SRFROWCA.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
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
    }
}
