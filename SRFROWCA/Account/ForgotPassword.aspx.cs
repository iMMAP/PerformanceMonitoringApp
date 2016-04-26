using System;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Web.Security;
using BusinessLogic;
using SRFROWCA.Common;
using System.Web.UI;

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
                    message = RC.SelectedSiteLanguageId == 2 ? "Nous sommes désolés, ce code de réinitialisation a expiré. Merci d en demander un nouveau." : "We're sorry, but this reset code has expired. Please request a new one.";
                    ShowMessage(message, RC.NotificationType.Error, false);
                }
                else if (msg == "Msg2")
                {
                    message = RC.SelectedSiteLanguageId == 2 ? "Désolé! Nous ne pourrions pas vérifier que cet utilisateur a demandé une réinitialisation de mot de passe. Merci de réssayer l inititialisation." : "Sorry! We couldn't verify that this user requested a password reset. Please try resetting again.";
                    ShowMessage(message, RC.NotificationType.Error, false);
                }

                Session["FromResetPageError"] = null;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
                string userName = txtUserName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string message = "";
                if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(email))
                {
                    message = RC.SelectedSiteLanguageId == 2 ? "Merci de fournir au moins soit le nom de utilisateur ou le mot de passe." : "Please provide at least either of the one, User Name or Email.";
                    ShowMessage(message, RC.NotificationType.Error, false);
                }
                else
                {
                    MembershipUser mu = GetUserFromProvidedInfo(userName, email);
                    if (mu == null)
                    {
                        message = RC.SelectedSiteLanguageId == 2 ? "Nous ne pourrions pas vous trouver en utilisant les informations que vous avez saisi. Merci de réessayer." : "We could not find you using the information you entered. Please try again.";
                        ShowMessage(message, RC.NotificationType.Error, false);
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
                ? @"Mot de passe oublié!{0}{1}ORS a reçu une requête de réinitialiser le mot de passe de votre compte '{2}'.{3}{4}Pour réinitialiser votre mot de passe, cliquer sur le lien ci-dessous (ou faites un copie-coller de l'URL sur votre navigateur): {5}{6}{7}{8}Si vous n'avez pas demandé de changement, ignorer ce message.{9}Ce lien va expirer dans 24 heures{10}{11}Meric,{12}ORS Support"
                : @"Forgot your password!{0}{1}ORS received a request to reset the password for your account '{2}'.{3}{4}To reset your password, click on the link below (or copy and paste the URL into your browser):{5}{6}{7}{8}If you did not request this change, you do not need to do anything.{9}This link will expire in 24 hours.{10}{11}Thanks,{12}ORS Support";

            string mailBody = string.Format(mailMessage, "<br/>", "<br/>", userName, "<br/>", "<br/>", "<br/>", link, "<br/>", "<br/>", "<br/>", "<br/>", "<br/>", "<br/>");
            //Mail.SendMail("orsocharowca@gmail.com ", toEmail, "Password Change Request", mailBody);
            try
            {
                using (MailMessage mailMsg = new MailMessage())
                {
                    mailMsg.From = new MailAddress("ors@ocharowca.info", "Sahel - ORS");
                    mailMsg.To.Add(new MailAddress(toEmail));
                    mailMsg.Subject = "ORS Password Change Request";
                    mailMsg.IsBodyHtml = true;
                    mailMsg.Body = mailBody;
                    Mail.SendMail(mailMsg);
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
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

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User); 
        }
    }
}