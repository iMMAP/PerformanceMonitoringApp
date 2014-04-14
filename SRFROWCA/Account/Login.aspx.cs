using System;
using System.Web;
using System.Web.Security;
using SRFROWCA.Common;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace SRFROWCA.Account
{
    public partial class Login : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);

            Button btn = this.LoginUser.FindControl("LoginButton") as Button;
            if (btn != null)
            {
                this.Form.DefaultButton = btn.UniqueID;
            }

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
            string message = "";
            if (usrInfo != null)
            {
                // Is this user locked out?
                if (usrInfo.IsLockedOut)
                {
                    if (RC.SelectedSiteLanguageId == 2)//French
                    {
                        message = "Votre compte a été fermé. Veuillez contacter l'administrateur du site pour plus de détails.";
                    }
                    else
                    {
                        message = "Your account has been locked out. Please contact the admin of the site for futher details.";
                    }
                    LoginUser.FailureText = message;
                }
                else if (!usrInfo.IsApproved)
                {
                    if (RC.SelectedSiteLanguageId == 2)
                    {
                        message = "Votre compte est maintenant approuvé. Vous ne pouvez pas vous connecter tant qu'un administarteur du site n'a pas approuvé votre compte.";
                    }
                    else
                    {
                        message = "Your account has not yet been approved. You cannot login until an admin of the site approve your account.";
                    }

                    LoginUser.FailureText = message;
                }
            }
            else
            {
                if (RC.SelectedSiteLanguageId == 2)
                {
                    message = "Combinaison nom d'utilisateur et mot de passe incorrect";
                }
                else
                {
                    message = "Wrong Username and password combination!";
                }
                LoginUser.FailureText = message;
            }
        }

        protected void btnForgotPassword_Click(object sender, EventArgs e)
        { }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "Login", this.User);
        }
    }
}