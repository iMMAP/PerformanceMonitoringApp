using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Account
{
    public partial class Login : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Button btn = LoginUser.FindControl("LoginButton") as Button;
            if (btn != null)
            {
                Form.DefaultButton = btn.UniqueID;
            }

            if (!IsPostBack)
            {
                TextBox tb = LoginUser.FindControl("UserName") as TextBox;
                if (tb != null)
                {
                    SetFocus(tb);
                }
            }
        }

        protected void LoginUser_LoggedIn(Object sender, EventArgs e)
        {

            //CleanSessions();
            UserInfo.UserProfileInfo(RC.EmergencySahel2015);
            if (Roles.IsUserInRole(LoginUser.UserName, "User"))
            {
                Response.Redirect("~/Pages/AddActivities.aspx");
            }
            else if (Roles.IsUserInRole(LoginUser.UserName, "ClusterLead"))
            {
                Response.Redirect("~/Landing/ClusterCord.aspx");
            }
            else
            {
                Response.Redirect("~/ClusterLead/ProjectsListing.aspx");
            }
        }

        private void CleanSessions()
        {
            Session["ClusterFrameworkSelectedCountry"] = null;
            Session["ClusterFrameworkSelectedCluster"] = null;
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
                    LoginUser.FailureText = RC.SelectedSiteLanguageId == 2 ?
                        "Votre compte a été fermé. Veuillez contacter l'administrateur du site pour plus de détails." :
                        "Your account has been locked out. Please contact the admin of the site for futher details.";
                }
                else if (!usrInfo.IsApproved)
                {
                    LoginUser.FailureText = RC.SelectedSiteLanguageId == 2 ?
                        "Votre compte est maintenant approuvé. Vous ne pouvez pas vous connecter tant qu'un administarteur du site n'a pas approuvé votre compte." :
                        "Your account has not yet been approved. You cannot login until an admin of the site approve your account.";
                }
            }
            else
            {
                LoginUser.FailureText = RC.SelectedSiteLanguageId == 2 ?
                    "Combinaison nom d'utilisateur et mot de passe incorrect" :
                    "Wrong Username and password combination!";
            }
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}