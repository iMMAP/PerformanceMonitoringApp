using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Web;
using System.IO.Compression;
using SRFROWCA.Common;
using System.Web.Security;

namespace SRFROWCA
{
    public partial class _Default : System.Web.UI.Page
    {

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
                Response.Redirect("~/ClusterLead/AddSRPActivitiesFromMasterList.aspx");
            }
            else
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