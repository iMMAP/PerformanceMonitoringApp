using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Account
{
    public partial class ForgotPassword : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            this.Form.DefaultButton = this.btnSubmit.UniqueID;
            this.SetFocus(this.txtUserName);

            if (Session["FromResetPageError"] != null)
            {
                string msg = Session["FromResetPageError"].ToString();
                if (msg == "Msg1")
                {
                    ShowMessage(RC.ErrorMessage, "We're sorry, but this reset code has expired. Please request a new one.");
                }
                else if (msg == "Msg2")
                {
                    ShowMessage(RC.ErrorMessage, "Sorry! We couldn't verify that this user requested a password reset. Please try resetting again.");
                }

                Session["FromResetPageError"] = null;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = txtUserName.Text.Trim();
                string email = txtEmail.Text.Trim();

                if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(email))
                {
                    ShowMessage(RC.ErrorMessage, "Please provide at least either of the one, User Name or Email.");
                    return;
                }
                else
                {
                    MembershipUser mu = GetUserFromProvidedInfo(userName, email);
                    if (mu == null)
                    {
                        ShowMessage(RC.ErrorMessage, "We couldn't find you using the information you entered. Please try again.");
                        return;
                    }
                    else
                    {
                        string link = "";
                        DataTable dt = CheckPendingRequest(mu.UserName);
                        if (dt.Rows.Count > 0)
                        {
                            link = GenerateTempLinkForUser(dt.Rows[0]["LinkGUID"].ToString());
                            EmailLink(mu.Email, link, mu.UserName);
                            RedirecToConfimationPage();
                        }
                        else
                        {
                            Guid guid = Guid.NewGuid();
                            string tempString = "I8$pUs9\\";
                            if (ConfigurationManager.AppSettings["tempresetstring"] != null)
                            {
                                tempString += ConfigurationManager.AppSettings["tempresetstring"].ToString();
                            }
                            string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            string hash = RC.GetHashString(guid + tempString + datetime);
                            SaveValuesInDB(mu.UserName, hash, guid, datetime);
                            link = GenerateTempLinkForUser(guid.ToString());
                            //EmailLink(mu.Email, link, mu.UserName);
                            RedirecToConfimationPage();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(RC.ErrorMessage, ex.Message);
            }
        }

        private void RedirecToConfimationPage()
        {
            Response.Redirect("~/Account/PasswordSendConfirmation.aspx");
        }

        private DataTable CheckPendingRequest(string userName)
        {
            return DBContext.GetData("GetResetPasswordValuesOnUserName", new object[] { userName });
        }

        private void EmailLink(string toEmail, string link, string userName)
        {
            string mailBody = string.Format(@"Forgot your password!

                                              3WPM received a request to reset the password for your account '{0}'
                                              To reset your password, click on the link below (or copy and paste the URL into your browser):
                                              {1}

                                              If you did not request this change, you do not need to do anything.
                                              This link will expire in 24 hours.

                                              Thanks, 
                                              3WPM Support",
                                              userName, link);
            Mail.SendMail("3wopactivities@gmail.com", toEmail, "Password Change Request", mailBody);
            
        }

        private void SaveValuesInDB(string userName, string hash, Guid guid, string dt)
        {
            DBContext.Add("InsertLinkValueInPasswordReset", new object[] { userName, hash, guid, dt, DBNull.Value });
        }

        private string GenerateTempLinkForUser(string guid)
        {
            string domain = "3wpm";
            if (ConfigurationManager.AppSettings["domain"] != null)
            {
                domain = ConfigurationManager.AppSettings["domain"].ToString();
            }

            return @domain + "/account/reset.aspx?key=" + guid;
        }

        private MembershipUser GetUserFromProvidedInfo(string userName, string email)
        {
            MembershipUser mu = null;
            if (!string.IsNullOrEmpty(userName))
            {
                mu = Membership.GetUser(userName);
                if (mu != null)
                {
                    return mu;
                }
            }

            else if (!string.IsNullOrEmpty(email))
            {
                userName = Membership.GetUserNameByEmail(email);
                if (!string.IsNullOrEmpty(userName))
                {
                    return Membership.GetUser(userName);
                }
            }

            return mu;
        }

        private void ShowMessage(string css, string message)
        {
            lblMessage.Visible = true;
            lblMessage.CssClass = css;
            lblMessage.Text = message;
        }

        private void SendNewPasswordToUser(string userName, string email)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                MembershipUser mu = Membership.GetUser(userName);
                if (mu != null)
                {
                    string password = mu.ResetPassword();
                    EmailPassword(password, mu.Email);
                }
            }
            else if (!string.IsNullOrEmpty(email))
            {
                userName = Membership.GetUserNameByEmail(email);
                if (!string.IsNullOrEmpty(userName))
                {
                    MembershipUser mu = Membership.GetUser(userName);
                    string password = mu.ResetPassword();
                    EmailPassword(password, mu.Email);
                }
            }
        }

        private void EmailPassword(string password, string toEmail)
        {
            string mailBody = string.Format("Your new password is {0}", password);
            Mail.SendMail("admin@abc.com", toEmail, "New Password", mailBody);
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "ForgotPassword", this.User);
        }
    }
}