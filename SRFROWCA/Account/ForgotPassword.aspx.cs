using System;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Web.Security;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Account
{
    public partial class ForgotPassword : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            Form.DefaultButton = this.btnSubmit.UniqueID;
            SetFocus(txtEmail);

            if (Session["FromResetPageError"] != null)
            {
                string msg = Session["FromResetPageError"].ToString();
                string message = "";
                if (msg == "Msg1")
                {
                    message = RC.SelectedSiteLanguageId == 2 ? "Nous sommes désolés, ce code de réinitialisation a expiré. Merci d'en demander un nouveau." : "We're sorry, but this reset code has expired. Please request a new one.";
                    ShowMessage(RC.ErrorMessage, message);
                }
                else if (msg == "Msg2")
                {
                    message = RC.SelectedSiteLanguageId == 2 ? "Désolé! Nous ne pourrions pas vérifier que cet utilisateur a demandé une réinitialisation de mot de passe. Merci de réssayer l'inititialisation." : "Sorry! We couldn't verify that this user requested a password reset. Please try resetting again.";
                    ShowMessage(RC.ErrorMessage, message);
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

                string message = "";
                if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(email))
                {
                    message = RC.SelectedSiteLanguageId == 2 ? "Merci de fournir au moins soit le nom d'utilisateur ou le mot de passe." : "Please provide at least either of the one, User Name or Email.";
                    ShowMessage(RC.ErrorMessage, message);
                }
                else
                {

                    MembershipUser mu = GetUserFromProvidedInfo(userName, email);
                    if (mu == null)
                    {
                        message = RC.SelectedSiteLanguageId == 2 ? "Nous ne pourrions pas vous trouver en utilisant les informations que vous avez saisi. Merci de réessayer." : "We couldn't find you using the information you entered. Please try again.";
                        ShowMessage(RC.ErrorMessage, message);
                    }
                    else
                    {
                        string link = "";
                        DataTable dt = CheckPendingRequest(mu.UserName);
                        if (dt.Rows.Count > 0)
                        {
                            link = GenerateTempLinkForUser(dt.Rows[0]["LinkGUID"].ToString());
                            //EmailLink(mu.Email, link, mu.UserName);
                            RedirecToConfimationPage();
                        }
                        else
                        {
                            Guid guid = Guid.NewGuid();
                            string tempString = "I8$pUs9\\";
                            if (ConfigurationManager.AppSettings["tempresetstring"] != null)
                            {
                                tempString += ConfigurationManager.AppSettings["tempresetstring"];
                            }

                            string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            string hash = RC.GetHashString(guid + tempString + datetime);
                            SaveValuesInDB(mu.UserName, hash, guid, datetime);
                            link = GenerateTempLinkForUser(guid.ToString());
                            EmailLink(mu.Email, link, mu.UserName);
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
            string mailMessage = RC.SelectedSiteLanguageId == 2
                ? @"Mot de passe oublié!
                                            ORS a reçu une requête de réinitialiser le mot de passe de votre compte. '{0}' 
                                            Pour réinitialiser votre mot de passe, cliquer sur le lien ci-dessous (ou faites un copie-coller de l'URL sur votre navigateur): {1}
                                            Si vous n'avez pas demandé de changement, ignorer ce message.
                                            Meric,
                                            ORS Support"
                : @"Forgot your password!

                                              ORS received a request to reset the password for your account '{0}'
                                              To reset your password, click on the link below (or copy and paste the URL into your browser):
                                              {1}

                                              If you did not request this change, you do not need to do anything.
                                              This link will expire in 24 hours.

                                              Thanks, 
                                              ORS Support";


            string mailBody = string.Format(mailMessage,userName, link);
            //Mail.SendMail("orsocharowca@gmail.com ", toEmail, "Password Change Request", mailBody);
            try
            {
                using (MailMessage mailMsg = new MailMessage())
                {
                    mailMsg.From = new MailAddress("orsocharowca@gmail.com");
                    mailMsg.To.Add(new MailAddress("orsocharowca@gmail.com"));
                    mailMsg.Subject = "ORS Password Change Request";
                    mailMsg.IsBodyHtml = true;
                    mailMsg.Body = mailBody;
                    Mail.SendMail(mailMsg);
                }
            }
            catch
            {
            }
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
                domain = ConfigurationManager.AppSettings["domain"];
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
            //lblMessage.Visible = true;
            //lblMessage.CssClass = css;
            //lblMessage.Text = message;
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "ForgotPassword", this.User);
        }
    }
}